using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Gameplay;
using Behaviour.Tractoring;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Item;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200036E RID: 878
	public class TractorModule : AbstractTargetingModule
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x000C46EF File Offset: 0x000C28EF
		protected override TargetingPriority baseTargetPriority
		{
			get
			{
				return TargetingPriority.Medium;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x000C46F2 File Offset: 0x000C28F2
		public override bool runFromTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x000C46F5 File Offset: 0x000C28F5
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.TractorBeam;
			}
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000C46F8 File Offset: 0x000C28F8
		protected override void Awake()
		{
			base.Awake();
			this.SetTractorTransform();
			this.CreateTractorBeams(this.amountOfBeams, false);
			this.CreateTractorBeams(this.amountOfBonusBeams, true);
			base.approachDistance = 5f;
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000C472C File Offset: 0x000C292C
		protected override void Update()
		{
			if (!base.active)
			{
				return;
			}
			if (!this.disableAutoTargeting)
			{
				foreach (TargetableUnit targetableUnit in this.filteredTargets)
				{
					TractorableItem tractorableItem = targetableUnit as TractorableItem;
					if (tractorableItem != null && !tractorableItem.isTractored)
					{
						if (base.parent.unitData.GetCargoAvailable() >= tractorableItem.data.itemType.m3)
						{
							this.StartTrackingTarget(tractorableItem, false);
						}
						else if (base.IsPlayer(true))
						{
							Singleton<IdleManager>.Current.TriggerInventoryFull();
						}
					}
				}
			}
			foreach (TractorBeam tractorBeam in this.tractorBeams)
			{
				if (tractorBeam.HasTarget())
				{
					tractorBeam.MoveTargetTowardsShip(this.tractorLocation.position, base.parent);
				}
			}
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000C4840 File Offset: 0x000C2A40
		private void SetTractorTransform()
		{
			Transform x = base.parent.transform.Find("TractorTransform");
			if (x != null)
			{
				this.tractorLocation = x;
				return;
			}
			this.tractorLocation = base.transform;
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000C4880 File Offset: 0x000C2A80
		public override void Deactivate()
		{
			base.Deactivate();
			this.StopTractoringItems();
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000C4890 File Offset: 0x000C2A90
		private void CreateTractorBeams(int amount, bool bonus)
		{
			for (int i = 0; i < amount; i++)
			{
				TractorBeam tractorBeam = UnityEngine.Object.Instantiate<TractorBeam>(this.tractorBeamPrefab, this.tractorLocation);
				tractorBeam.bonusBeam = bonus;
				tractorBeam.tractorModule = this;
				this.tractorBeams.Add(tractorBeam);
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x000C48D8 File Offset: 0x000C2AD8
		public TractorBeam GetAvailableTractorBeam(bool bonus)
		{
			foreach (TractorBeam tractorBeam in this.tractorBeams)
			{
				if (!tractorBeam.HasTarget() && bonus == tractorBeam.bonusBeam)
				{
					return tractorBeam;
				}
			}
			return null;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000C493C File Offset: 0x000C2B3C
		private bool StartTrackingTarget(TractorableItem item, bool bonus)
		{
			TractorBeam availableTractorBeam = this.GetAvailableTractorBeam(bonus);
			if (!availableTractorBeam)
			{
				return false;
			}
			if (item && !item.isTractored)
			{
				availableTractorBeam.StartTractoring(item, 0f);
			}
			return true;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000C4978 File Offset: 0x000C2B78
		public void StopTractoringItems()
		{
			foreach (TractorBeam tractorBeam in this.tractorBeams)
			{
				tractorBeam.StopTractoring();
			}
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000C49C8 File Offset: 0x000C2BC8
		public override void UpdateAvailableTargets(IEnumerable<TargetableUnit> targets)
		{
			this.filteredTargets.Clear();
			int num = this.tractorBeams.Count((TractorBeam t) => !t.bonusBeam);
			foreach (TargetableUnit targetableUnit in targets)
			{
				if (targetableUnit)
				{
					TractorableItem tractorableItem = targetableUnit as TractorableItem;
					if (tractorableItem != null && tractorableItem.CanBeAutoTractoredBy(base.parent) && !tractorableItem.isTractored)
					{
						this.filteredTargets.Add(tractorableItem);
						if (this.filteredTargets.Count >= num)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000C4A84 File Offset: 0x000C2C84
		public override void SetManualTarget(TargetableUnit manualTarget)
		{
			base.SetManualTarget(manualTarget);
			if (manualTarget)
			{
				this.StartTrackingTarget(manualTarget as TractorableItem, true);
			}
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000C4AA3 File Offset: 0x000C2CA3
		public void DisableAutoTargeting(bool change)
		{
			this.disableAutoTargeting = change;
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000C4AAC File Offset: 0x000C2CAC
		public override bool ShouldAutoTarget()
		{
			return true;
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000C4AAF File Offset: 0x000C2CAF
		public IEnumerator WaitForIdle()
		{
			float timer = 10f;
			do
			{
				bool flag = false;
				using (List<TractorBeam>.Enumerator enumerator = this.tractorBeams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasTarget())
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					timer -= Time.deltaTime;
					yield return null;
				}
				else
				{
					timer = 0f;
				}
			}
			while (timer > 0f);
			yield break;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000C4ABE File Offset: 0x000C2CBE
		public override MainStat GetMainStat()
		{
			return new MainStat(Translation.Plural("Tractor Beam", this.amountOfBeams), (float)this.amountOfBeams);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000C4ADC File Offset: 0x000C2CDC
		protected override void SetMainSubStats()
		{
			this.mainSubStats.AddMainSubStat(this.amountOfBonusBeams.ToString(), Translation.Plural("Manual Tractor Beam", this.amountOfBonusBeams));
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000C4B04 File Offset: 0x000C2D04
		public int NewBeamCount(AbstractEquipment item, SeededRandom random)
		{
			int num = 1;
			int num2;
			switch (item.size)
			{
			case ModuleSize.Small:
				num2 = 5;
				break;
			case ModuleSize.Medium:
				num2 = 15;
				break;
			case ModuleSize.Large:
				num2 = 35;
				break;
			default:
				num2 = 5;
				break;
			}
			int num3 = num2;
			if (item.item.itemLevel >= num3 * 4)
			{
				num++;
			}
			else if (item.item.itemLevel >= num3)
			{
				float chanceOfTrue = (item.item.itemLevel >= num3 * 2) ? 0.65f : 0.35f;
				if (random.RandomBool(chanceOfTrue))
				{
					num++;
				}
			}
			if (item.item.rarity >= Rarity.Exotic)
			{
				num++;
			}
			else
			{
				Rarity rarity = item.item.rarity;
				float num4;
				if (rarity != Rarity.Enhanced)
				{
					if (rarity != Rarity.HighGrade)
					{
						num4 = 0f;
					}
					else
					{
						num4 = 0.65f;
					}
				}
				else
				{
					num4 = 0.35f;
				}
				float chanceOfTrue2 = num4;
				if (random.RandomBool(chanceOfTrue2))
				{
					num++;
				}
			}
			if (item.item.itemLevel > 5 && random.RandomBool(0.05f))
			{
				num++;
			}
			if (item.size == ModuleSize.Medium)
			{
				num *= 2;
				if (item.item.itemLevel >= num3 * 4 || random.RandomBool(0.5f))
				{
					num++;
				}
			}
			else if (item.size == ModuleSize.Large)
			{
				num *= 3;
				if (item.item.itemLevel >= num3 * 4 || random.RandomBool(0.5f))
				{
					num++;
				}
			}
			switch (item.item.rarity)
			{
			case Rarity.Enhanced:
				num2 = 2;
				break;
			case Rarity.HighGrade:
				num2 = 3;
				break;
			case Rarity.Exotic:
				num2 = 4;
				break;
			case Rarity.Legendary:
				num2 = 5;
				break;
			default:
				num2 = 1;
				break;
			}
			return Mathf.Max(num2, num);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000C4CA8 File Offset: 0x000C2EA8
		public override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["amountOfBeams"] = new double?((double)this.amountOfBeams);
			data["amountOfBonusBeams"] = new double?((double)this.amountOfBonusBeams);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000C4CF4 File Offset: 0x000C2EF4
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			if (data.ContainsKey("amountOfBeams"))
			{
				this.amountOfBeams = data["amountOfBeams"];
			}
			if (data.ContainsKey("amountOfBonusBeams"))
			{
				this.amountOfBonusBeams = data["amountOfBonusBeams"];
			}
		}

		// Token: 0x040013FC RID: 5116
		[SerializeField]
		private TractorBeam tractorBeamPrefab;

		// Token: 0x040013FD RID: 5117
		private List<TractorBeam> tractorBeams = new List<TractorBeam>();

		// Token: 0x040013FE RID: 5118
		[Header("Module Specific")]
		public int amountOfBeams;

		// Token: 0x040013FF RID: 5119
		public int amountOfBonusBeams;

		// Token: 0x04001400 RID: 5120
		public int tractorRange;

		// Token: 0x04001401 RID: 5121
		private Transform tractorLocation;

		// Token: 0x04001402 RID: 5122
		public bool disableAutoTargeting;
	}
}
