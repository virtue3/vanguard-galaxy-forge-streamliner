using System;
using System.Collections.Generic;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Simulation.World.POI;
using Source.Util;

namespace Source.Simulation.TravelEvents
{
	// Token: 0x02000084 RID: 132
	public class DistressCombat : TravelDynamicEvent
	{
		// Token: 0x060004AB RID: 1195 RVA: 0x00027600 File Offset: 0x00025800
		public override MapPointOfInterest CreateDynamicPOI(SystemMapData system)
		{
			SeededRandom global = SeededRandom.Global;
			Combat combat = new Combat();
			combat.name = "@TravelEventDistressCall";
			combat.faction = Faction.marauders;
			combat.system = system;
			combat.level = system.level;
			combat.position = system.GetDynamicMissionPosition();
			combat.storyteller = new DistressCombat(combat);
			List<AbstractUnitData> list = combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), Faction.miningGuild, 0, 0, 1, 1, null);
			foreach (AbstractUnitData abstractUnitData in list)
			{
				abstractUnitData.playerFriendly = true;
			}
			combat.AddGuards(list, null);
			combat.AddGuards(combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), global);
			base.AddBeacon(combat);
			return combat;
		}
	}
}
