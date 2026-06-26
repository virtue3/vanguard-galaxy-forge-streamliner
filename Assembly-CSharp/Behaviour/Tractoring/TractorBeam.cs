using System;
using Behaviour.Effects;
using Behaviour.Equipment.Module;
using Behaviour.Gameplay;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Galaxy;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.VFX;

namespace Behaviour.Tractoring
{
	// Token: 0x020002DA RID: 730
	public class TractorBeam : MonoBehaviour
	{
		// Token: 0x06001AA4 RID: 6820 RVA: 0x000A487C File Offset: 0x000A2A7C
		private void Start()
		{
			this.beamEffect = base.GetComponentInChildren<BeamEffect>();
			this.beamEffect.SetPower(this.effectPower);
			this.beamEffect.SetFrequency(this.effectFrequency);
			this.beamEffect.SetSize(this.effectSize);
			this.beamEffect.Stop();
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000A48D4 File Offset: 0x000A2AD4
		private void OnEnable()
		{
			if (!this.beamEffect || !this.beamEffect.visualEffect)
			{
				return;
			}
			BeamEffect beamEffect = this.beamEffect;
			if (beamEffect != null)
			{
				VisualEffect visualEffect = beamEffect.visualEffect;
				if (visualEffect != null)
				{
					visualEffect.Reinit();
				}
			}
			BeamEffect beamEffect2 = this.beamEffect;
			if (beamEffect2 == null)
			{
				return;
			}
			beamEffect2.Stop();
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000A492D File Offset: 0x000A2B2D
		private void OnDestroy()
		{
			this.StopTractoring();
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x000A4935 File Offset: 0x000A2B35
		public void StartTractoring(TractorableItem target, float delay = 0f)
		{
			this.target = target;
			target.isTractored = true;
			this.tractorDelay = delay;
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x000A494C File Offset: 0x000A2B4C
		public void MoveTargetTowardsShip(Vector2 shipPosition, AbstractUnit spaceShip)
		{
			if (this.DelayTractor())
			{
				return;
			}
			Vector2 vector = shipPosition - (Vector2)this.target.transform.position;
			float z = this.AngleBetweenPoints(this.target.transform.position, base.transform.position);
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
			if (vector.magnitude <= 0.25f)
			{
				if (this.tractorModule.manualTarget == this.target)
				{
					this.tractorModule.SetManualTarget(null);
				}
				UsableItem usableItem;
				if (this.tractorModule.IsPlayer(true) && this.target.data.itemType.TryGetComponent<UsableItem>(out usableItem))
				{
					if (usableItem is CreditsItem)
					{
						usableItem.OnUse();
						UnityEngine.Object.Destroy(this.target.gameObject);
						Singleton<LootManager>.Instance.RemoveLootItem(this.target.data);
						this.StopTractoring();
						return;
					}
					CrewPodItem crewPodItem = usableItem as CrewPodItem;
					if (crewPodItem != null)
					{
						if (crewPodItem.OnUse())
						{
							UnityEngine.Object.Destroy(this.target.gameObject);
							Singleton<LootManager>.Instance.RemoveLootItem(this.target.data);
							this.StopTractoring();
							return;
						}
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCrewFull", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
						this.target.bounced = true;
						this.target.data.impulse = Vector2.one / 2f + UnityEngine.Random.insideUnitCircle;
						Vector2 impulse = this.target.data.impulse;
						this.target.data.angle = Mathf.Atan2(impulse.y, impulse.x) * 57.29578f - 90f;
						this.target.transform.rotation = Quaternion.Euler(0f, 0f, z);
						this.StopTractoring();
						return;
					}
				}
				int num = this.target.AddItemToShipInventory(spaceShip);
				if (num == this.target.data.itemAmount)
				{
					if (this.target.data.itemType.itemCategory == ItemCategory.Module)
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Modules, 1);
					}
					if (this.target.data.itemType.itemCategory == ItemCategory.Turret)
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Turrets, 1);
					}
					if (this.tractorModule.IsPlayer(true))
					{
						Faction ownerFaction = this.target.data.ownerFaction;
						if (ownerFaction != null && ownerFaction != Faction.player)
						{
							int num2 = 1;
							ownerFaction.ChangePlayerReputation(-num2);
							Singleton<EventLogManager>.Instance.NewEvent("rep", Translation.Translate("@StoleItem", new object[]
							{
								num2,
								Translation.Translate(ownerFaction.name, Array.Empty<object>())
							}));
							if (this.target.data.itemType.itemCategory == ItemCategory.Ore)
							{
								Register.AddCounter("OreStolen", this.target.data.itemAmount, 0);
							}
						}
					}
					DefensiveTurret defensiveTurret;
					if (this.target.TryGetComponent<DefensiveTurret>(out defensiveTurret))
					{
						defensiveTurret.defensiveTurretData.turretItem.GetComponent<DefensiveTurretItem>().ResetCooldown(0.75f);
						defensiveTurret.UpdateAmmoData();
						BasePoiManager.current.poi.RemoveUnit(defensiveTurret.defensiveTurretData);
						UnityEngine.Object.Destroy(defensiveTurret.GetComponent<TractorableItem>());
					}
					UnityEngine.Object.Destroy(this.target.gameObject);
					Singleton<LootManager>.Instance.RemoveLootItem(this.target.data);
				}
				else
				{
					if (this.tractorModule.IsPlayer(true))
					{
						Debug.Log("inv full trigger Beam");
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCargoFull", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
						Singleton<IdleManager>.Current.TriggerInventoryFull();
					}
					DefensiveTurret defensiveTurret2;
					Collider2D collider2D;
					if (this.target.TryGetComponent<DefensiveTurret>(out defensiveTurret2) && defensiveTurret2.TryGetComponent<Collider2D>(out collider2D))
					{
						collider2D.enabled = true;
						UnityEngine.Object.Destroy(defensiveTurret2.GetComponent<TractorableItem>());
					}
					this.target.data.itemAmount -= num;
					this.target.SetData(this.target.data);
					this.target.bounced = true;
					this.target.data.impulse = Vector2.one / 2f + UnityEngine.Random.insideUnitCircle;
				}
				this.StopTractoring();
				return;
			}
			this.target.transform.Translate(vector.normalized * this.target.speed * Time.deltaTime, global::UnityEngine.Space.World);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x000A4E38 File Offset: 0x000A3038
		public void MoveTargetTowardsObject(Vector2 objectPosition, ITractorParent parent)
		{
			if (this.DelayTractor())
			{
				return;
			}
			Vector2 vector = objectPosition - (Vector2)this.target.transform.position;
			float z = this.AngleBetweenPoints(this.target.transform.position, base.transform.position);
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
			if (vector.magnitude <= 0.25f)
			{
				parent.ItemTractored(this.target);
				UnityEngine.Object.Destroy(this.target.gameObject);
				Singleton<LootManager>.Instance.RemoveLootItem(this.target.data);
				this.StopTractoring();
				return;
			}
			this.target.transform.Translate(vector.normalized * this.target.speed * Time.deltaTime, global::UnityEngine.Space.World);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x000A4F34 File Offset: 0x000A3134
		private bool DelayTractor()
		{
			if (this.tractorDelay >= 0f)
			{
				this.tractorDelay -= Time.deltaTime;
				if (this.tractorDelay < 0f)
				{
					float num = this.bonusBeam ? this.bonusSpeed : this.tractorSpeed;
					this.target.speed = UnityEngine.Random.Range(num * 1.5f, num * 3f);
					this.beamEffect.SetColor(this.bonusBeam ? this.bonusEffectColor : this.effectColor);
					this.beamEffect.SetObjectsToTrack(base.gameObject, this.target.gameObject);
					this.beamEffect.Play();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000A4FF0 File Offset: 0x000A31F0
		private float AngleBetweenPoints(Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(a.y - b.y, a.x - b.x) * 57.29578f;
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000A5018 File Offset: 0x000A3218
		public void StopTractoring()
		{
			if (this.target)
			{
				this.target.isTractored = false;
				this.target = null;
			}
			if (this.beamEffect && this.beamEffect.visualEffect)
			{
				this.beamEffect.Stop();
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000A506F File Offset: 0x000A326F
		public bool HasTarget()
		{
			return this.target != null;
		}

		// Token: 0x040010CD RID: 4301
		[Header("Beam effect")]
		[SerializeField]
		private BeamEffect beamEffect;

		// Token: 0x040010CE RID: 4302
		[SerializeField]
		private Color effectColor;

		// Token: 0x040010CF RID: 4303
		[SerializeField]
		private Color bonusEffectColor;

		// Token: 0x040010D0 RID: 4304
		[SerializeField]
		private float effectPower;

		// Token: 0x040010D1 RID: 4305
		[SerializeField]
		private float effectFrequency;

		// Token: 0x040010D2 RID: 4306
		[SerializeField]
		private float effectSize;

		// Token: 0x040010D3 RID: 4307
		private TractorableItem target;

		// Token: 0x040010D4 RID: 4308
		private float tractorDelay;

		// Token: 0x040010D5 RID: 4309
		public TractorModule tractorModule;

		// Token: 0x040010D6 RID: 4310
		public bool bonusBeam;

		// Token: 0x040010D7 RID: 4311
		public float bonusSpeed;

		// Token: 0x040010D8 RID: 4312
		public float tractorSpeed;
	}
}
