using System;
using LightJson;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C0 RID: 192
	public class CollectCredits : MissionObjective
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0003E10E File Offset: 0x0003C30E
		public override string statusText
		{
			get
			{
				return Translation.Translate("@ObjectiveCollectCredits", new object[]
				{
					GamePlayer.current.credits,
					this.requiredAmount
				});
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0003E140 File Offset: 0x0003C340
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0003E143 File Offset: 0x0003C343
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0003E146 File Offset: 0x0003C346
		public override bool IsComplete()
		{
			return GamePlayer.current.credits >= (long)this.requiredAmount;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0003E15E File Offset: 0x0003C35E
		protected override void DataToJson(JsonObject data)
		{
			data["requiredAmount"] = new double?((double)this.requiredAmount);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0003E17C File Offset: 0x0003C37C
		protected override void LoadFromJson(JsonObject data)
		{
			this.requiredAmount = data["requiredAmount"];
		}

		// Token: 0x0400044F RID: 1103
		public int requiredAmount;
	}
}
