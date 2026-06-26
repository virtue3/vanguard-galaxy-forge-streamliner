using System;
using System.Collections.Generic;
using Source.Galaxy.POI.Station;
using Source.MissionSystem;
using Source.MissionSystem.Generator.Umbral;
using Source.Player;
using Source.Simulation.Story;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000187 RID: 391
	public class Puppeteers : Faction
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00066928 File Offset: 0x00064B28
		public override bool offersMissionsForShip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0006692B File Offset: 0x00064B2B
		public override string name
		{
			get
			{
				if (!Register.HasFlag("PuppeteersNameChange", false))
				{
					GamePlayer current = GamePlayer.current;
					if (current == null || !current.HasStoryteller<Conquest>())
					{
						return base.name;
					}
				}
				return "@FactionNamePuppeteers2";
			}
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0006695C File Offset: 0x00064B5C
		public Mission GenerateUmbralMission(MissionBoard mb, bool force = false)
		{
			SeededRandom seededRandom = force ? SeededRandom.Global : new SeedGenerator().Add(mb.owner.guid).Add(DateTime.Now.DayOfYear).CreateRandom();
			return MissionGenerator.GenerateMission(seededRandom.Choose<UmbralMissionGenerator>(Puppeteers.umbralMissions), MissionDifficulty.Hard, mb.owner, seededRandom, null);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000669C6 File Offset: 0x00064BC6
		public Puppeteers()
		{
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 1;
		}

		// Token: 0x040007DB RID: 2011
		private static List<UmbralMissionGenerator> umbralMissions = new List<UmbralMissionGenerator>
		{
			new SalvageDeadDrop(),
			new BountyHunt(),
			new MiningDeadDrop()
		};
	}
}
