using System;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Data;
using Source.Data.Persistable;
using Source.MissionSystem;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Tractoring
{
	// Token: 0x020002D9 RID: 729
	public class TractorableItem : TargetableUnit, ITractorable
	{
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x000A4525 File Offset: 0x000A2725
		public bool jettisoned
		{
			get
			{
				return this.data.jettisoned;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x000A4532 File Offset: 0x000A2732
		public float cargoSpace
		{
			get
			{
				return this.data.itemType.m3 * (float)this.data.itemAmount;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x000A4551 File Offset: 0x000A2751
		// (set) Token: 0x06001A92 RID: 6802 RVA: 0x000A4559 File Offset: 0x000A2759
		public TractorableItemData data { get; protected set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x000A4562 File Offset: 0x000A2762
		// (set) Token: 0x06001A94 RID: 6804 RVA: 0x000A456A File Offset: 0x000A276A
		public bool isTractored { get; internal set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x000A4573 File Offset: 0x000A2773
		public override string targetName
		{
			get
			{
				return "Loot";
			}
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000A457A File Offset: 0x000A277A
		protected override void Awake()
		{
			base.Awake();
			this.materialBlock = new MaterialPropertyBlock();
			this.lightIntensityFactor = this.maxIntensity;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000A4599 File Offset: 0x000A2799
		protected override void Start()
		{
			base.Start();
			base.transform.Z(ZIndex.TractorableItem);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000A45B0 File Offset: 0x000A27B0
		protected override void Update()
		{
			base.Update();
			Vector2 vector = this.data.impulse;
			float maxDistanceDelta = 0.001f;
			vector = Vector2.MoveTowards(vector, Vector2.zero, maxDistanceDelta);
			this.data.impulse = vector;
			base.transform.position += (Vector3)(vector * Time.deltaTime);
			if (this.glowLight)
			{
				this.glowLight.intensity = this.lightIntensity;
				this.lightIntensity += Time.deltaTime * this.lightIntensityFactor;
				if (this.lightIntensity < 0f)
				{
					this.lightIntensityFactor = this.maxIntensity;
				}
				else if (this.lightIntensity > this.maxIntensity)
				{
					this.lightIntensityFactor = -this.maxIntensity;
				}
			}
			if (base.GetDistanceFromBorder() < 1f)
			{
				this.data.impulse = Vector2.zero;
			}
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000A469E File Offset: 0x000A289E
		public override void TakeDamage(DamageData data)
		{
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000A46A0 File Offset: 0x000A28A0
		public void SetItemSprite(Sprite icon)
		{
			base.spriteRenderer.sprite = icon;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000A46AE File Offset: 0x000A28AE
		public void SetData(TractorableItemData data)
		{
			this.data = data;
			ItemTooltipSource component = base.GetComponent<ItemTooltipSource>();
			if (component != null)
			{
				component.SetItem(data.itemType, data.itemAmount, true, ItemTooltipContext.InSpace, false, null);
			}
			this.SetGlowColor();
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000A46DE File Offset: 0x000A28DE
		public void SetJettisoned(bool jettisoned)
		{
			this.data.jettisoned = jettisoned;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000A46EC File Offset: 0x000A28EC
		public bool CanBeAutoTractoredBy(AbstractUnit unit)
		{
			return !this.jettisoned && !this.bounced && (this.data.ownerFaction == null || this.data.ownerFaction == unit.faction);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000A4724 File Offset: 0x000A2924
		public void SetGlowColor()
		{
			if (!this.rarityGlow)
			{
				return;
			}
			this.rarityGlow.color = this.data.itemType.rarity.GetColor().WithAlpha(0.8f);
			this.materialBlock.SetColor("_Color", this.rarityGlow.color);
			this.materialBlock.SetFloat("_Speed", 3f);
			this.rarityGlow.SetPropertyBlock(this.materialBlock);
			this.glowLight.color = this.rarityGlow.color;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000A47C0 File Offset: 0x000A29C0
		public void SetForcePickup(bool force = true)
		{
			this.forceAddItem = force;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000A47C9 File Offset: 0x000A29C9
		public void OnMouseUpAsButton()
		{
			if (!this.canBeTractorAssisted || !UIHelper.clickTargetingAvailable)
			{
				return;
			}
			TractorModule module = GameplayManager.Instance.spaceShip.GetModule<TractorModule>();
			if (module == null)
			{
				return;
			}
			module.SetManualTarget(this);
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000A47F8 File Offset: 0x000A29F8
		public int AddItemToShipInventory(AbstractUnit unit)
		{
			SpaceShip spaceShip = unit as SpaceShip;
			if (spaceShip != null)
			{
				int num = spaceShip.AddItemToCargo(this.data.itemType, this.data.itemAmount, this.forceAddItem);
				if (num > 0 && !this.jettisoned)
				{
					MissionObjective.Trigger(MissionTrigger.ItemCollected, new ValueTuple<TractorableItemData, AbstractUnitData>(this.data, spaceShip.unitData), null, false);
				}
				return num;
			}
			return 0;
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000A485D File Offset: 0x000A2A5D
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return false;
		}

		// Token: 0x040010C1 RID: 4289
		[SerializeField]
		private SpriteRenderer rarityGlow;

		// Token: 0x040010C2 RID: 4290
		[SerializeField]
		private Light2D glowLight;

		// Token: 0x040010C3 RID: 4291
		[SerializeField]
		private float maxIntensity;

		// Token: 0x040010C4 RID: 4292
		private float lightIntensityFactor;

		// Token: 0x040010C5 RID: 4293
		private float lightIntensity;

		// Token: 0x040010C6 RID: 4294
		private MaterialPropertyBlock materialBlock;

		// Token: 0x040010C7 RID: 4295
		private bool forceAddItem;

		// Token: 0x040010C8 RID: 4296
		public float speed = 2.5f;

		// Token: 0x040010C9 RID: 4297
		public bool canBeTractorAssisted = true;

		// Token: 0x040010CA RID: 4298
		public bool bounced;
	}
}
