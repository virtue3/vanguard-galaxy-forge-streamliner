using System;
using System.Collections.Generic;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.TravelEvents
{
	// Token: 0x02000085 RID: 133
	public class DistressSalvage : TravelDynamicEvent
	{
		// Token: 0x060004AD RID: 1197 RVA: 0x00027700 File Offset: 0x00025900
		public override MapPointOfInterest CreateDynamicPOI(SystemMapData system)
		{
			List<Faction> list = new List<Faction>
			{
				Faction.red,
				Faction.blue,
				Faction.gold,
				Faction.miningGuild,
				Faction.tradingGuild
			};
			SeededRandom global = SeededRandom.Global;
			Salvage salvage = new Salvage();
			salvage.name = "@TravelEventDistressCall";
			salvage.faction = Faction.marauders;
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
			salvage.AddGuards(salvage.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), global);
			base.AddBeacon(salvage);
			return salvage;
		}
	}
}
