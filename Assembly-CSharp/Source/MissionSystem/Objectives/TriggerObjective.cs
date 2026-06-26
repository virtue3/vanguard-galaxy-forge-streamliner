using System;
using LightJson;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000D1 RID: 209
	public class TriggerObjective : MissionObjective
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0003FCEE File Offset: 0x0003DEEE
		public float progress
		{
			get
			{
				return (float)this.currentAmount / (float)this.requiredAmount;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0003FD00 File Offset: 0x0003DF00
		public override string statusText
		{
			get
			{
				string text = Translation.TranslateOnly(this.description, Array.Empty<object>());
				if (this.requiredAmount != 1)
				{
					return string.Concat(new string[]
					{
						text,
						": ",
						GameMath.FormatNumber((float)this.currentAmount, -1),
						"/",
						GameMath.FormatNumber((float)this.requiredAmount, -1)
					});
				}
				return text;
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0003FD68 File Offset: 0x0003DF68
		public override bool IsComplete()
		{
			return this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0003FD7B File Offset: 0x0003DF7B
		public override MissionTrigger? triggeredBy
		{
			get
			{
				return new MissionTrigger?(this.trigger);
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0003FD88 File Offset: 0x0003DF88
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0003FD8C File Offset: 0x0003DF8C
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			int num2;
			if (data is int)
			{
				int num = (int)data;
				num2 = num;
			}
			else
			{
				num2 = 1;
			}
			int num3 = num2;
			this.currentAmount = Math.Min(this.currentAmount + num3, this.requiredAmount);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0003FDC8 File Offset: 0x0003DFC8
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0003FDCC File Offset: 0x0003DFCC
		protected override void DataToJson(JsonObject data)
		{
			data["trigger"] = this.trigger.ToString();
			data["description"] = this.description;
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			data["currentAmount"] = new double?((double)this.currentAmount);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0003FE48 File Offset: 0x0003E048
		protected override void LoadFromJson(JsonObject data)
		{
			this.trigger = Enum.Parse<MissionTrigger>(data["trigger"]);
			this.description = data["description"];
			this.requiredAmount = data["requiredAmount"];
			this.currentAmount = data["currentAmount"];
		}

		// Token: 0x0400047E RID: 1150
		public MissionTrigger trigger;

		// Token: 0x0400047F RID: 1151
		public string description;

		// Token: 0x04000480 RID: 1152
		public int requiredAmount = 1;

		// Token: 0x04000481 RID: 1153
		public int currentAmount;
	}
}
