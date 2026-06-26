using System;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C8 RID: 200
	public class KillEnemies : MissionObjective
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0003EA8F File Offset: 0x0003CC8F
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x0003EA97 File Offset: 0x0003CC97
		public int currentAmount { get; protected set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0003EAA0 File Offset: 0x0003CCA0
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x0003EAA8 File Offset: 0x0003CCA8
		public override GameplayType gameplayType { get; set; } = GameplayType.Combat;

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0003EAB4 File Offset: 0x0003CCB4
		public override string statusText
		{
			get
			{
				return Translation.TranslateOnly("@MissionObjectiveKillEnemies", new object[]
				{
					this.shipType ?? this.enemyFaction.name,
					Mathf.Clamp(this.currentAmount, 0, this.requiredAmount),
					this.requiredAmount
				});
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0003EB11 File Offset: 0x0003CD11
		public override bool IsComplete()
		{
			return this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0003EB24 File Offset: 0x0003CD24
		public override MissionTrigger? triggeredBy
		{
			get
			{
				return new MissionTrigger?(MissionTrigger.UnitDestroyed);
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0003EB2C File Offset: 0x0003CD2C
		public override Sprite GetIcon()
		{
			return MissionIcons.Get("Combat");
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0003EB38 File Offset: 0x0003CD38
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			AbstractUnit item = ((ValueTuple<AbstractUnit, DamageData>)data).Item1;
			if (item.faction != null && item.faction == this.enemyFaction && (this.shipType == null || item.identifier == this.shipType) && this.currentAmount < this.requiredAmount)
			{
				int currentAmount = this.currentAmount;
				this.currentAmount = currentAmount + 1;
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0003EBA2 File Offset: 0x0003CDA2
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0003EBA8 File Offset: 0x0003CDA8
		protected override void DataToJson(JsonObject data)
		{
			data["shipType"] = this.shipType;
			data["faction"] = this.enemyFaction.identifier;
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			data["currentAmount"] = new double?((double)this.currentAmount);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0003EC20 File Offset: 0x0003CE20
		protected override void LoadFromJson(JsonObject data)
		{
			this.shipType = data["shipType"];
			this.enemyFaction = Faction.Get(data["faction"]);
			this.requiredAmount = data["requiredAmount"];
			this.currentAmount = data["currentAmount"];
		}

		// Token: 0x0400045D RID: 1117
		public string shipType;

		// Token: 0x0400045E RID: 1118
		public Faction enemyFaction;

		// Token: 0x0400045F RID: 1119
		public int requiredAmount;
	}
}
