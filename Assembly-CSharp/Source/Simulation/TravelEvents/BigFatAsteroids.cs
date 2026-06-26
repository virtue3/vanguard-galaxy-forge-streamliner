using System;
using Behaviour.Mining;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;

namespace Source.Simulation.TravelEvents
{
	// Token: 0x02000083 RID: 131
	public class BigFatAsteroids : TravelDynamicEvent
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x00027514 File Offset: 0x00025714
		public override MapPointOfInterest CreateDynamicPOI(SystemMapData system)
		{
			SeededRandom global = SeededRandom.Global;
			Mining mining = new Mining();
			mining.name = "@TravelEventGeneric";
			mining.faction = Faction.miningGuild;
			mining.system = system;
			mining.level = system.level;
			mining.position = system.GetDynamicMissionPosition();
			AsteroidFieldOreSet surface = new AsteroidFieldOreSet(system.systemOreData.surfaceOres.rareOre, global.Choose<OreItemData>(system.systemOreData.surfaceOres.wildcardOres), null);
			AsteroidFieldOreSet core = new AsteroidFieldOreSet(system.systemOreData.coreOres.rareOre, global.Choose<OreItemData>(system.systemOreData.coreOres.wildcardOres), null);
			int num = global.RandomRange(1, 3);
			mining.SetAsteroidFieldData(new AsteroidFieldData(global.RandomRange(2, 4), 1f, 4f - (float)num, surface, core, -1f), 0);
			base.AddBeacon(mining);
			return mining;
		}
	}
}
