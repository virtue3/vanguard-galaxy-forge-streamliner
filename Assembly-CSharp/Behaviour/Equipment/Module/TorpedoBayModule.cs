using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.Equipment.Module.DroneBay;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;
using Source.Drone;
using Source.Galaxy;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200036D RID: 877
	public class TorpedoBayModule : AbstractModule
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x000C423F File Offset: 0x000C243F
		// (set) Token: 0x060021C2 RID: 8642 RVA: 0x000C4247 File Offset: 0x000C2447
		public TargetableUnit target { get; private set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x000C4250 File Offset: 0x000C2450
		// (set) Token: 0x060021C4 RID: 8644 RVA: 0x000C4258 File Offset: 0x000C2458
		public InventoryItemType ammoType { get; private set; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060021C5 RID: 8645 RVA: 0x000C4261 File Offset: 0x000C2461
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.TopedoBay;
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000C4265 File Offset: 0x000C2465
		protected override void Awake()
		{
			base.Awake();
			this.SetDoorMechanismAndLatchPoints();
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000C4274 File Offset: 0x000C2474
		private void SetDoorMechanismAndLatchPoints()
		{
			DoorMechanism[] componentsInChildren = base.parent.GetComponentsInChildren<DoorMechanism>();
			this.doorLatchDict = new Dictionary<DoorMechanism, TorpedoLatchPoint>();
			foreach (DoorMechanism doorMechanism in componentsInChildren)
			{
				TorpedoLatchPoint component = doorMechanism.GetComponent<TorpedoLatchPoint>();
				if (component != null)
				{
					this.doorLatchDict.Add(doorMechanism, component);
					component.isReadyToFire = true;
					component.isDeployed = false;
				}
			}
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000C42D8 File Offset: 0x000C24D8
		protected override void Update()
		{
			if (this.outOfTorpedoWarning > 0f)
			{
				this.outOfTorpedoWarning -= Time.deltaTime;
			}
			if (this.firing || !this.target || !base.active)
			{
				return;
			}
			if (this.CanFire() && this.coroutineFire == null)
			{
				if (this.HasAmmo())
				{
					this.coroutineFire = base.StartCoroutine(this.InstantiateTorpedo());
					return;
				}
				if (base.IsPlayer(true) && this.outOfTorpedoWarning <= 0f)
				{
					this.outOfTorpedoWarning = 10f;
					UIInfoTextParent instance = UIInfoTextParent.instance;
					if (instance == null)
					{
						return;
					}
					instance.ShowWarningText("@OutOfTorp", null, null);
				}
			}
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000C438C File Offset: 0x000C258C
		private bool CanFire()
		{
			return Time.time >= this.nextFireTime && !this.firing;
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000C43A6 File Offset: 0x000C25A6
		public bool HasAmmo()
		{
			return !this.ammoType || base.parent.unitData.ItemAmountInCargoHold(this.ammoType) != 0;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000C43D0 File Offset: 0x000C25D0
		private void IsLinedUpWithTarget()
		{
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000C43D2 File Offset: 0x000C25D2
		private void IsReadyToFire()
		{
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000C43D4 File Offset: 0x000C25D4
		public void AddManualTarget(TargetableUnit target, bool overrideTarget = true)
		{
			TargetableUnit component = target.GetComponent<TargetableUnit>();
			if (!component.isSalvage && !(component is Asteroid))
			{
				this.target = component;
			}
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000C43FF File Offset: 0x000C25FF
		private IEnumerator InstantiateTorpedo()
		{
			ValueTuple<TorpedoLatchPoint, DoorMechanism>? result = this.GetAvailableTorpedoLatch();
			if (result == null)
			{
				Debug.Log("No torpedoes available?");
				yield break;
			}
			TorpedoLatchPoint availableLatch = result.Value.Item1;
			DoorMechanism item = result.Value.Item2;
			Vector2 torpedoPosition = availableLatch.TorpedoPosition();
			yield return base.StartCoroutine(item.ToggleDoors(true));
			yield return new WaitForSeconds(0.5f);
			availableLatch.isDeployed = true;
			availableLatch.SetReadyToFire(false);
			this.firing = true;
			base.parent.unitData.RemoveCargo(this.ammoType, 1, false);
			Transform transform = base.parent.transform;
			Torpedo torpedo = UnityEngine.Object.Instantiate<Torpedo>(this.torpedoPrefab, torpedoPosition, transform.rotation, base.transform);
			TorpedoData torpedoData = new TorpedoData(torpedo, this.torpedoPrefab);
			torpedoData.faction = base.parent.faction;
			torpedoData.LoadDefaultEquipment(MapPointOfInterest.current.level, -1f, null, null, null, null, false, null);
			torpedo.SetData(torpedoData, true, false);
			torpedo.InitModules();
			torpedo.SetCommander(base.parent);
			torpedo.SetTarget(this.target);
			torpedo.SetDamageData(this.CreateDamageData(null, null, TargetLayer.Surface));
			torpedo.SetEngineState(false, true);
			yield return base.StartCoroutine(this.FadeInAndGrow(torpedo, availableLatch.transform, 0.5f, 0.8f));
			if (torpedo != null)
			{
				torpedo.transform.SetParent(GameplayManager.Instance.transform);
				this.activeTorpedoes.Add(torpedo);
				torpedo.SetEngineState(true, true);
				torpedo.Initialize(base.parent.rigidbody.linearVelocity);
			}
			yield return new WaitForSeconds(3f);
			this.firing = false;
			result.Value.Item1.isReloading = true;
			this.nextFireTime = Time.time + this.fireRate;
			base.StartCoroutine(this.ReloadTorpedo(result.Value.Item1, result.Value.Item2));
			this.coroutineFire = null;
			yield break;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000C440E File Offset: 0x000C260E
		private IEnumerator FadeInAndGrow(Torpedo torpedo, Transform latch, float duration = 0.5f, float maxScale = 0.8f)
		{
			SpriteRenderer sr;
			if (!torpedo.TryGetComponent<SpriteRenderer>(out sr))
			{
				yield break;
			}
			float timer = 0f;
			Color color = sr.color;
			color.a = 0f;
			sr.color = color;
			torpedo.transform.localScale = Vector3.zero;
			while (timer < duration)
			{
				if (torpedo == null)
				{
					yield break;
				}
				timer += Time.deltaTime;
				float t = timer / duration;
				color.a = Mathf.Lerp(0f, 1f, t);
				sr.color = color;
				torpedo.transform.position = latch.position;
				torpedo.transform.localScale = Vector3.one * Mathf.Lerp(0.3f, maxScale, t);
				yield return null;
			}
			if (torpedo == null)
			{
				yield break;
			}
			color.a = 1f;
			sr.color = color;
			torpedo.transform.localScale = Vector3.one * maxScale;
			yield break;
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000C4434 File Offset: 0x000C2634
		private ValueTuple<TorpedoLatchPoint, DoorMechanism>? GetAvailableTorpedoLatch()
		{
			foreach (KeyValuePair<DoorMechanism, TorpedoLatchPoint> keyValuePair in this.doorLatchDict)
			{
				TorpedoLatchPoint value = keyValuePair.Value;
				if (value.isReadyToFire && !value.isReloading && !value.isDeployed)
				{
					return new ValueTuple<TorpedoLatchPoint, DoorMechanism>?(new ValueTuple<TorpedoLatchPoint, DoorMechanism>(value, keyValuePair.Key));
				}
			}
			return null;
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000C44C4 File Offset: 0x000C26C4
		public override void Deactivate()
		{
			base.Deactivate();
			if (this.coroutineFire != null)
			{
				base.StopCoroutine(this.coroutineFire);
				this.coroutineFire = null;
				this.firing = false;
			}
			foreach (KeyValuePair<DoorMechanism, TorpedoLatchPoint> keyValuePair in this.doorLatchDict)
			{
				keyValuePair.Value.isDeployed = false;
				keyValuePair.Value.isReloading = false;
				keyValuePair.Value.SetReadyToFire(true);
				base.StartCoroutine(keyValuePair.Key.ToggleDoors(false));
			}
			foreach (Torpedo torpedo in this.activeTorpedoes)
			{
				if (torpedo)
				{
					UnityEngine.Object.Destroy(torpedo.gameObject);
				}
			}
			this.activeTorpedoes.Clear();
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000C45CC File Offset: 0x000C27CC
		private IEnumerator ReloadTorpedo(TorpedoLatchPoint latchPoint, DoorMechanism door)
		{
			yield return base.StartCoroutine(door.ToggleDoors(false));
			yield return new WaitForSeconds(this.reloadSpeed);
			latchPoint.isReloading = false;
			latchPoint.SetReadyToFire(true);
			yield break;
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000C45E9 File Offset: 0x000C27E9
		public bool HasLoadout(GameplayType type, TargetLayer targetLayer = TargetLayer.Both)
		{
			return type == GameplayType.Combat;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000C45F2 File Offset: 0x000C27F2
		public bool HasMiningLoadout(bool core)
		{
			return false;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000C45F8 File Offset: 0x000C27F8
		public float GetTorpedoPower()
		{
			foreach (EquipStatLine equipStatLine in base.stats)
			{
				if (equipStatLine.stat == EquipStat.TorpedoPower)
				{
					return equipStatLine.amount;
				}
			}
			return 0f;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000C4660 File Offset: 0x000C2860
		public override MainStat GetMainStat()
		{
			return new MainStat("Torpedo Power", this.GetTorpedoPower());
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000C4672 File Offset: 0x000C2872
		protected override void SetMainSubStats()
		{
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000C4674 File Offset: 0x000C2874
		protected DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			return new DamageData(base.parent)
			{
				power = this.GetAttackPower(),
				criticalChance = this.GetStat(EquipStat.CriticalChance),
				type = DamageType.Heat,
				targetLayer = targetLayer
			};
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x000C46A9 File Offset: 0x000C28A9
		private float GetAttackPower()
		{
			return base.parent.GetStat(EquipStat.TorpedoPower) + base.parent.GetNormalizedPower(EquipStat.CombatPower);
		}

		// Token: 0x040013F1 RID: 5105
		public Torpedo torpedoPrefab;

		// Token: 0x040013F2 RID: 5106
		[SerializeField]
		private float reloadSpeed = 12f;

		// Token: 0x040013F3 RID: 5107
		[SerializeField]
		private float fireRate = 8f;

		// Token: 0x040013F4 RID: 5108
		private float nextFireTime;

		// Token: 0x040013F5 RID: 5109
		private Dictionary<DoorMechanism, TorpedoLatchPoint> doorLatchDict;

		// Token: 0x040013F7 RID: 5111
		private bool firing;

		// Token: 0x040013F8 RID: 5112
		private readonly List<Torpedo> activeTorpedoes = new List<Torpedo>();

		// Token: 0x040013FA RID: 5114
		private Coroutine coroutineFire;

		// Token: 0x040013FB RID: 5115
		private float outOfTorpedoWarning;
	}
}
