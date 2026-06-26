using System;
using System.Collections.Generic;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using UnityEngine;

namespace Source.Simulation.TravelEvents
{
	// Token: 0x02000086 RID: 134
	public class Salvage : TravelDynamicEvent
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x0002783C File Offset: 0x00025A3C
		public override MapPointOfInterest CreateDynamicPOI(SystemMapData system)
		{
			List<Faction> list = new List<Faction>
			{
				Faction.red,
				Faction.blue,
				Faction.gold,
				Faction.miningGuild,
				Faction.salvageGuild,
				Faction.tradingGuild
			};
			SeededRandom global = SeededRandom.Global;
			Salvage salvage = new Salvage();
			salvage.name = "@TravelEventGeneric";
			salvage.faction = Faction.salvageGuild;
			salvage.system = system;
			salvage.level = system.level;
			salvage.position = system.GetDynamicMissionPosition();
			SalvageData salvageData = new SalvageData
			{
				position = salvage.GetWorldPosition() + new Vector2(8f, 2f),
				shipTemplate = salvage.FindSalvageShipTemplate(global.Choose<Faction>(list))
			};
			salvageData.AddItemContent(salvage.level, -1, 1f);
			salvageData.AddScrapContent(salvage.level, 1f, 2);
			salvageData.AddStructuralContent(salvage.level, 2, 1f);
			salvage.AddPersistable(salvageData);
			base.AddBeacon(salvage);
			return salvage;
		}
	}
}
