using System;
using Behaviour.Hazard;
using Behaviour.UI;
using Behaviour.Unit;
using LightJson;
using Source.Combat;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Hazard;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000311 RID: 785
	public class ExplosiveMineItem : DefensiveTurretItem
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x000B05FC File Offset: 0x000AE7FC
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x000B0604 File Offset: 0x000AE804
		public string hazardName { get; private set; }

		// Token: 0x06001D7D RID: 7549 RVA: 0x000B060D File Offset: 0x000AE80D
		public override void DataFromJson(JsonObject data)
		{
			this.hazardName = data["hazardName"];
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000B0625 File Offset: 0x000AE825
		public override void DataToJson(JsonObject data)
		{
			data["hazardName"] = this.hazardName;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x000B0640 File Offset: 0x000AE840
		public override void SpawnDefensiveTurret()
		{
			MineHazardData mineHazardData = (MineHazardData)((Mine)LocalHazard.Get(this.hazardName)).CreateData(base.item.itemLevel, DamageType.Explosive);
			mineHazardData.faction = Faction.player;
			MineData mineData = (MineData)mineHazardData.CreatePersistableData();
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			mineData.position = spaceShip.transform.position + spaceShip.transform.rotation * new Vector3(-2f, 0f, 0f);
			MapPointOfInterest current = MapPointOfInterest.current;
			if (current == null)
			{
				return;
			}
			current.AddPersistable(mineData);
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000B06E4 File Offset: 0x000AE8E4
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			float minePower = Mine.GetMinePower(base.item.itemLevel);
			tooltip.AddTextLine("<color=green>" + GameMath.FormatNumber(minePower, -1) + "</color> " + Translation.Translate(EquipStat.CombatPower.GetDisplayName(), Array.Empty<object>()), 16, 8f).Text.color = CompareTooltip.detailsColor;
		}
	}
}
