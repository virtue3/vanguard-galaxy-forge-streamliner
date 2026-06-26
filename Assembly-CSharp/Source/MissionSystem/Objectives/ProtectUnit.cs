using System;
using Behaviour.Item;
using LightJson;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000CA RID: 202
	public class ProtectUnit : TriggerObjective
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0003F11B File Offset: 0x0003D31B
		public override MissionTrigger? triggeredBy
		{
			get
			{
				return new MissionTrigger?(MissionTrigger.UnitProtected);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0003F124 File Offset: 0x0003D324
		public override string statusText
		{
			get
			{
				string text = Translation.Translate(this.protectText ?? "@ProtectUnitDesc", Array.Empty<object>());
				if (this.requiredAmount != 1)
				{
					return string.Concat(new string[]
					{
						text,
						": ",
						this.currentAmount.ToString(),
						"/",
						this.requiredAmount.ToString()
					});
				}
				return text;
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0003F194 File Offset: 0x0003D394
		protected override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			string key = "faction";
			Faction faction = this.enemyFaction;
			data[key] = ((faction != null) ? faction.identifier : null);
			data["protectText"] = this.protectText;
			data["hostileNoRepLoss"] = new bool?(this.hostileNoRepLoss);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0003F1FC File Offset: 0x0003D3FC
		protected override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data);
			if (!data["faction"].IsNull)
			{
				this.enemyFaction = Faction.Get(data["faction"]);
			}
			this.protectText = data["protectText"];
			if (!data["hostileNoRepLoss"].IsNull)
			{
				this.hostileNoRepLoss = data["hostileNoRepLoss"].AsBoolean;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0003F284 File Offset: 0x0003D484
		public override Sprite GetIcon()
		{
			return InventoryItemType.Get("EscortMissionItem0").icon;
		}

		// Token: 0x04000469 RID: 1129
		public Faction enemyFaction;

		// Token: 0x0400046A RID: 1130
		public string protectText;

		// Token: 0x0400046B RID: 1131
		public bool hostileNoRepLoss;
	}
}
