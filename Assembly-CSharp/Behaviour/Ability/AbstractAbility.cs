using System;
using Behaviour.Crew;
using Behaviour.Equipment;
using Behaviour.Unit;
using Source.Player;
using UnityEngine;

namespace Behaviour.Ability
{
	// Token: 0x020003C0 RID: 960
	public abstract class AbstractAbility : StackableEffect
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x000CFE75 File Offset: 0x000CE075
		public string identifier
		{
			get
			{
				return base.name.Replace("(Clone)", "");
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x000CFE8C File Offset: 0x000CE08C
		// (set) Token: 0x06002503 RID: 9475 RVA: 0x000CFE94 File Offset: 0x000CE094
		public GameObject payload { get; private set; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x000CFE9D File Offset: 0x000CE09D
		// (set) Token: 0x06002505 RID: 9477 RVA: 0x000CFEA5 File Offset: 0x000CE0A5
		public float cooldown { get; private set; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000CFEAE File Offset: 0x000CE0AE
		// (set) Token: 0x06002507 RID: 9479 RVA: 0x000CFEB6 File Offset: 0x000CE0B6
		public bool stackEffects { get; private set; }

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000CFEBF File Offset: 0x000CE0BF
		// (set) Token: 0x06002509 RID: 9481 RVA: 0x000CFEC7 File Offset: 0x000CE0C7
		public bool stackDuration { get; private set; }

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x0600250A RID: 9482 RVA: 0x000CFED0 File Offset: 0x000CE0D0
		// (set) Token: 0x0600250B RID: 9483 RVA: 0x000CFED8 File Offset: 0x000CE0D8
		public bool droneAbility { get; private set; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x000CFEE1 File Offset: 0x000CE0E1
		// (set) Token: 0x0600250D RID: 9485 RVA: 0x000CFEE9 File Offset: 0x000CE0E9
		public float cooldownRemaining { get; private set; }

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x000CFEF2 File Offset: 0x000CE0F2
		public bool isReady
		{
			get
			{
				return this.cooldownRemaining <= 0f;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x000CFF04 File Offset: 0x000CE104
		// (set) Token: 0x06002510 RID: 9488 RVA: 0x000CFF0C File Offset: 0x000CE10C
		public bool toggledAbility { get; private set; }

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000CFF15 File Offset: 0x000CE115
		// (set) Token: 0x06002512 RID: 9490 RVA: 0x000CFF1D File Offset: 0x000CE11D
		public bool toggleActive { get; private set; }

		// Token: 0x06002513 RID: 9491 RVA: 0x000CFF28 File Offset: 0x000CE128
		private void Awake()
		{
			if (this.payload == null && base.transform.childCount == 1)
			{
				this.payload = base.transform.GetChild(0).gameObject;
			}
			if (this.payload.transform.parent == base.transform && this.payload.gameObject.activeSelf)
			{
				this.payload.gameObject.SetActive(false);
			}
			this.parentEquipment = base.GetComponentInParent<AbstractEquipment>();
			this.parentUnit = base.GetComponentInParent<AbstractUnit>();
			this.crewMember = base.GetComponentInParent<CrewMember>();
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000CFFCC File Offset: 0x000CE1CC
		protected virtual void Start()
		{
			if (!this.persistCooldown || GamePlayer.current == null)
			{
				return;
			}
			float cooldownRemaining;
			if (GamePlayer.current.abilityCooldowns.TryGetValue(this.identifier, out cooldownRemaining))
			{
				this.cooldownRemaining = cooldownRemaining;
			}
			ActiveEffectData activeEffectData;
			if (GamePlayer.current.activeEffects.TryGetValue(this.identifier, out activeEffectData))
			{
				this.RestorePayload(activeEffectData.durationRemaining, activeEffectData.stackSize);
				if (this.toggledAbility)
				{
					this.toggleActive = true;
					return;
				}
			}
			else if (this.toggledAbility && GamePlayer.current.activeToggles.Contains(this.identifier))
			{
				this.RestoreToggle();
			}
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000D006C File Offset: 0x000CE26C
		private void RestorePayload(float savedDuration, int savedStackSize)
		{
			CrewMember crewMember = this.crewMember;
			Transform transform;
			if ((transform = ((crewMember != null) ? crewMember.transform : null)) == null)
			{
				AbstractEquipment abstractEquipment = this.parentEquipment;
				transform = (((abstractEquipment != null) ? abstractEquipment.transform : null) ?? this.parentUnit.transform);
			}
			Transform parent = transform;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.payload, parent);
			gameObject.name = base.name + gameObject.name;
			StackableEffect[] components = gameObject.GetComponents<StackableEffect>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SetStackSize(savedStackSize);
			}
			foreach (TemporaryEffect temporaryEffect in gameObject.GetComponents<TemporaryEffect>())
			{
				temporaryEffect.SetDuration(savedDuration);
				temporaryEffect.SetAbilityIdentifier(this.identifier);
			}
			if (!gameObject.activeSelf)
			{
				gameObject.gameObject.SetActive(true);
			}
			if (this.parentUnit)
			{
				this.parentUnit.CalculateStats();
			}
			this.triggeredPayload = gameObject;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000D0154 File Offset: 0x000CE354
		private void RestoreToggle()
		{
			this.toggleActive = true;
			CrewMember crewMember = this.crewMember;
			Transform transform;
			if ((transform = ((crewMember != null) ? crewMember.transform : null)) == null)
			{
				AbstractEquipment abstractEquipment = this.parentEquipment;
				transform = (((abstractEquipment != null) ? abstractEquipment.transform : null) ?? this.parentUnit.transform);
			}
			Transform parent = transform;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.payload, parent);
			gameObject.name = base.name + gameObject.name;
			if (this.stackEffects)
			{
				StackableEffect[] componentsInChildren = gameObject.GetComponentsInChildren<StackableEffect>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].SetStackSize(base.stackSize);
				}
			}
			if (!gameObject.activeSelf)
			{
				gameObject.gameObject.SetActive(true);
			}
			if (this.parentUnit)
			{
				this.parentUnit.CalculateStats();
			}
			this.triggeredPayload = gameObject;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000D0222 File Offset: 0x000CE422
		public void StartCooldown()
		{
			this.cooldownRemaining = this.cooldown;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000D0230 File Offset: 0x000CE430
		public GameObject TriggerPayload(GameObject payload = null, Transform payloadParent = null, object source = null, bool force = false)
		{
			if (!this.isReady && !force)
			{
				return null;
			}
			if (payload == null)
			{
				payload = this.payload;
			}
			if (payloadParent == null)
			{
				CrewMember crewMember = this.crewMember;
				Transform transform;
				if ((transform = ((crewMember != null) ? crewMember.transform : null)) == null)
				{
					AbstractEquipment abstractEquipment = this.parentEquipment;
					transform = (((abstractEquipment != null) ? abstractEquipment.transform : null) ?? this.parentUnit.transform);
				}
				payloadParent = transform;
			}
			if (this.toggledAbility)
			{
				if (this.toggleActive)
				{
					UnityEngine.Object.Destroy(this.triggeredPayload);
					this.triggeredPayload = null;
					this.toggleActive = false;
					if (this.persistCooldown)
					{
						GamePlayer current = GamePlayer.current;
						if (current != null)
						{
							current.activeToggles.Remove(this.identifier);
						}
					}
					return null;
				}
				this.toggleActive = true;
				if (this.persistCooldown)
				{
					GamePlayer current2 = GamePlayer.current;
					if (current2 != null)
					{
						current2.activeToggles.Add(this.identifier);
					}
				}
			}
			this.StartCooldown();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(payload, payloadParent);
			gameObject.name = base.name + gameObject.name;
			if (this.stackEffects)
			{
				StackableEffect[] componentsInChildren = gameObject.GetComponentsInChildren<StackableEffect>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].SetStackSize(base.stackSize);
				}
			}
			TriggeredPayload[] componentsInChildren2 = gameObject.GetComponentsInChildren<TriggeredPayload>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].PayloadTriggered(source, base.stackSize);
			}
			foreach (TemporaryEffect temporaryEffect in gameObject.GetComponents<TemporaryEffect>())
			{
				if (this.stackDuration)
				{
					temporaryEffect.SetDuration(temporaryEffect.duration * (float)base.stackSize);
				}
				temporaryEffect.SetAbilityIdentifier(this.identifier);
				temporaryEffect.CheckStackability();
			}
			if (!gameObject.activeSelf)
			{
				gameObject.gameObject.SetActive(true);
			}
			if (this.parentUnit)
			{
				this.parentUnit.CalculateStats();
			}
			this.triggeredPayload = gameObject;
			return gameObject;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x000D0403 File Offset: 0x000CE603
		protected virtual bool persistCooldown
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000D0408 File Offset: 0x000CE608
		protected virtual void Update()
		{
			if (this.cooldownRemaining > 0f)
			{
				this.cooldownRemaining -= Time.deltaTime;
				if (this.persistCooldown && GamePlayer.current != null)
				{
					GamePlayer.current.abilityCooldowns[this.identifier] = this.cooldownRemaining;
				}
			}
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000D045E File Offset: 0x000CE65E
		public float GetCooldownProgress()
		{
			return Mathf.Clamp01(0f + this.cooldownRemaining / this.cooldown);
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000D0478 File Offset: 0x000CE678
		public bool IsPayloadActive()
		{
			return this.triggeredPayload;
		}

		// Token: 0x0400168E RID: 5774
		protected AbstractEquipment parentEquipment;

		// Token: 0x0400168F RID: 5775
		protected AbstractUnit parentUnit;

		// Token: 0x04001690 RID: 5776
		protected CrewMember crewMember;

		// Token: 0x04001691 RID: 5777
		private GameObject triggeredPayload;
	}
}
