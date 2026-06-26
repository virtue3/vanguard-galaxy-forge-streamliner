using System;
using System.Collections.Generic;
using Source.Item;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000364 RID: 868
	public class ReactorModule : AbstractModule
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x0600213A RID: 8506 RVA: 0x000C247E File Offset: 0x000C067E
		public float energyCapacity
		{
			get
			{
				return this.GetStat(EquipStat.EnergyCapacity);
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x000C2488 File Offset: 0x000C0688
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Reactor;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x000C248C File Offset: 0x000C068C
		public float availableCapacity
		{
			get
			{
				float num = this.energyCapacity;
				foreach (AbstractEquipment abstractEquipment in this.connectedEquipment)
				{
					num -= abstractEquipment.energyDraw;
				}
				return num;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x000C24EC File Offset: 0x000C06EC
		public float usedCapacity
		{
			get
			{
				return this.energyCapacity - this.availableCapacity;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600213E RID: 8510 RVA: 0x000C24FC File Offset: 0x000C06FC
		public float energyBonusOrPenalty
		{
			get
			{
				foreach (KeyValuePair<float, float> keyValuePair in ReactorModule.energyThresholdModifiers)
				{
					if (this.energyUsage <= keyValuePair.Key)
					{
						return keyValuePair.Value;
					}
				}
				return 0f;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x000C2568 File Offset: 0x000C0768
		public float energyUsage
		{
			get
			{
				return this.usedCapacity / this.energyCapacity;
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000C2577 File Offset: 0x000C0777
		public void ConnectEquipment(AbstractEquipment equipment)
		{
			this.connectedEquipment.Add(equipment);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000C2585 File Offset: 0x000C0785
		public void DisconnectEquipment(AbstractEquipment equipment)
		{
			if (this.connectedEquipment.Contains(equipment))
			{
				this.connectedEquipment.Remove(equipment);
			}
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000C25A2 File Offset: 0x000C07A2
		public List<AbstractEquipment> GetConnectedEquipment()
		{
			return this.connectedEquipment;
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000C25AA File Offset: 0x000C07AA
		public void SetConnectedEquipment(List<AbstractEquipment> connectedEquipment)
		{
			this.connectedEquipment = connectedEquipment;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000C25B4 File Offset: 0x000C07B4
		public override MainStat GetMainStat()
		{
			EquipStatLine? equipStatLine = base.GetStatLine(EquipStat.EnergyCapacity);
			return new MainStat("Energy", (equipStatLine != null) ? equipStatLine.GetValueOrDefault().amount : 0f);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000C25F0 File Offset: 0x000C07F0
		protected override void SetMainSubStats()
		{
		}

		// Token: 0x040013B3 RID: 5043
		private List<AbstractEquipment> connectedEquipment = new List<AbstractEquipment>();

		// Token: 0x040013B4 RID: 5044
		public static readonly SortedDictionary<float, float> energyThresholdModifiers = new SortedDictionary<float, float>
		{
			{
				0.5f,
				0.2f
			},
			{
				0.75f,
				0.1f
			},
			{
				1f,
				0f
			},
			{
				1.25f,
				-0.25f
			},
			{
				1.5f,
				-0.5f
			},
			{
				float.MaxValue,
				-0.75f
			}
		};
	}
}
