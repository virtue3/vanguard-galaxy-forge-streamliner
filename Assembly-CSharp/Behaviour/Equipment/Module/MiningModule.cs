using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.UI.HUD.Hover;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Mining;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200036A RID: 874
	public class MiningModule : AbstractTargetingModule
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000C370E File Offset: 0x000C190E
		protected override TargetingPriority baseTargetPriority
		{
			get
			{
				return TargetingPriority.Low;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x000C3711 File Offset: 0x000C1911
		public override bool maintainTargetAngle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000C3714 File Offset: 0x000C1914
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x000C371C File Offset: 0x000C191C
		public AbstractMiningTurret[] miningTurrets { get; protected set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x000C3725 File Offset: 0x000C1925
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x000C372D File Offset: 0x000C192D
		public bool canMineSurface { get; private set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x000C3736 File Offset: 0x000C1936
		// (set) Token: 0x06002199 RID: 8601 RVA: 0x000C373E File Offset: 0x000C193E
		public bool canMineCore { get; private set; }

		// Token: 0x0600219A RID: 8602 RVA: 0x000C3747 File Offset: 0x000C1947
		protected override void Awake()
		{
			base.Awake();
			base.approachDistance = 4f;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000C375A File Offset: 0x000C195A
		protected override void Start()
		{
			base.Start();
			this.SetCanTurretsMineSurface();
			this.SetCanTurretsMineCore();
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000C3770 File Offset: 0x000C1970
		public override void UpdateAvailableTargets(IEnumerable<TargetableUnit> targets)
		{
			if (!this.IsTargetMineable(base.autoTarget as Asteroid))
			{
				base.ResetAutoTarget();
			}
			if (!this.IsTargetMineable(base.manualTarget as Asteroid))
			{
				TargetableUnit manualTarget = base.manualTarget;
				if (manualTarget != null && !manualTarget.damagableByAll)
				{
					base.ResetCurrentTargets();
				}
			}
			this.FilterMiningTargets(new List<TargetableUnit>(targets), true);
			foreach (TargetableUnit targetableUnit in this.filteredTargets)
			{
				TargetScannedEffect targetScannedEffect;
				if (targetableUnit.gameObject.TryGetComponent<TargetScannedEffect>(out targetScannedEffect))
				{
					Color originalFinalScanningColor = targetScannedEffect.GetOriginalFinalScanningColor();
					int num;
					if (this.targetValues.TryGetValue(targetableUnit.gameObject, out num))
					{
						targetScannedEffect.SetFinalScanningColor(Color.Lerp(targetScannedEffect.GetLowResourceColor(), originalFinalScanningColor, (float)num / (float)this.maxValue));
					}
				}
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000C385C File Offset: 0x000C1A5C
		private void FilterMiningTargets(List<TargetableUnit> targets, bool avoidPlayer)
		{
			this.filteredTargets.Clear();
			this.targetValues.Clear();
			foreach (TargetableUnit targetableUnit in targets)
			{
				if (!avoidPlayer || targetableUnit.targetedCount <= 0)
				{
					Asteroid asteroid = targetableUnit as Asteroid;
					if (asteroid != null && this.IsTargetMineable(asteroid))
					{
						this.filteredTargets.Add(asteroid);
					}
				}
			}
			if (avoidPlayer && this.filteredTargets.Count == 0)
			{
				this.FilterMiningTargets(targets, false);
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000C38FC File Offset: 0x000C1AFC
		public bool IsCurrentTargetMineable()
		{
			return this.priorityTarget == null || (this.priorityTarget is Asteroid && this.IsTargetMineable((Asteroid)this.priorityTarget)) || this.priorityTarget.damagableByAll;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000C393C File Offset: 0x000C1B3C
		public bool IsTargetMineable(Asteroid target)
		{
			if (target == null)
			{
				return false;
			}
			bool flag = (target.GetSurfaceAmount() > 0 && this.canMineSurface) || (target.GetInnerAmount() > 0 && this.canMineCore);
			bool flag2 = this.canMineCore && target.hasInnerCore && target.currentCoreHealth > 0 && !target.innerCoreDepleted;
			bool flag3 = this.canMineSurface && !target.IsSurfaceOreDepleted();
			if (flag2 && !this.targetValues.ContainsKey(target.gameObject))
			{
				this.maxValue = ((target.currentCoreHealth > this.maxValue) ? target.currentCoreHealth : this.maxValue);
				this.targetValues.Add(target.gameObject, target.currentCoreHealth);
			}
			if (flag3 && !this.targetValues.ContainsKey(target.gameObject))
			{
				this.maxValue = ((target.currentSurfaceHealth > this.maxValue) ? target.currentSurfaceHealth : this.maxValue);
				this.targetValues.Add(target.gameObject, target.currentSurfaceHealth);
			}
			Drone drone = base.parent as Drone;
			return (drone == null || !drone.isDriller || target.CanWeAttach(base.parent as Drone)) && flag;
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000C3A80 File Offset: 0x000C1C80
		public void SetCanTurretsMineSurface()
		{
			bool canMineSurface;
			if (!this.miningTurrets.Any((AbstractMiningTurret turret) => turret.targetsSurface))
			{
				DroneBayModule droneBay = base.droneBay;
				if (droneBay == null || !droneBay.HasMiningLoadout(false))
				{
					TorpedoBayModule torpedoBayModule = base.torpedoBayModule;
					canMineSurface = (torpedoBayModule != null && torpedoBayModule.HasMiningLoadout(false));
					goto IL_58;
				}
			}
			canMineSurface = true;
			IL_58:
			this.canMineSurface = canMineSurface;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000C3AEC File Offset: 0x000C1CEC
		public void SetCanTurretsMineCore()
		{
			bool canMineCore;
			if (!this.miningTurrets.Any((AbstractMiningTurret turret) => turret.targetsCore))
			{
				DroneBayModule droneBay = base.droneBay;
				canMineCore = (droneBay != null && droneBay.HasMiningLoadout(true));
			}
			else
			{
				canMineCore = true;
			}
			this.canMineCore = canMineCore;
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000C3B44 File Offset: 0x000C1D44
		public void SetMiningTurrets(AbstractMiningTurret[] miningTurrets)
		{
			base.turrets = miningTurrets;
			this.miningTurrets = miningTurrets;
			this.InitMiningTurrets();
			List<TargetLayer> list = new List<TargetLayer>();
			foreach (AbstractTurret abstractTurret in base.turrets)
			{
				abstractTurret.SetTargetingModule(this);
				if (!list.Contains(abstractTurret.targetLayer))
				{
					list.Add(abstractTurret.targetLayer);
				}
			}
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000C3BA8 File Offset: 0x000C1DA8
		private void InitMiningTurrets()
		{
			AbstractMiningTurret[] miningTurrets = this.miningTurrets;
			for (int i = 0; i < miningTurrets.Length; i++)
			{
				miningTurrets[i].SetTargetingModule(this);
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000C3BD4 File Offset: 0x000C1DD4
		public bool CanMineItemFromFieldData(AsteroidFieldData asteroidFieldData, InventoryItemType itemType)
		{
			bool result = false;
			OreItemData oreItemData;
			if (asteroidFieldData != null && itemType.TryGetComponent<OreItemData>(out oreItemData))
			{
				if (asteroidFieldData.surfaceOres.HasOre(oreItemData, false) && this.canMineSurface)
				{
					result = true;
				}
				if (asteroidFieldData.coreOres.HasOre(oreItemData, false) && this.canMineCore)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000C3C23 File Offset: 0x000C1E23
		public override MainStat GetMainStat()
		{
			return new MainStat("Mining Module", 0f);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000C3C34 File Offset: 0x000C1E34
		protected override void SetMainSubStats()
		{
		}

		// Token: 0x040013E8 RID: 5096
		private int maxValue;

		// Token: 0x040013E9 RID: 5097
		private Dictionary<GameObject, int> targetValues = new Dictionary<GameObject, int>();
	}
}
