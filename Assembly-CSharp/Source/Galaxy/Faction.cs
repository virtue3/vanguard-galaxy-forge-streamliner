using System;
using System.Collections.Generic;
using Behaviour.Unit;
using Source.Data;
using Source.Galaxy.NameGenerator;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000140 RID: 320
	public class Faction
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x00056272 File Offset: 0x00054472
		public static IEnumerable<Faction> all
		{
			get
			{
				return Faction.allFactions.Values;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0005627E File Offset: 0x0005447E
		public virtual bool offersMissionsForShip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00056281 File Offset: 0x00054481
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x00056289 File Offset: 0x00054489
		public string identifier { get; private set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00056292 File Offset: 0x00054492
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x0005629A File Offset: 0x0005449A
		public virtual string name { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x000562A3 File Offset: 0x000544A3
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x000562AB File Offset: 0x000544AB
		public string description { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x000562B4 File Offset: 0x000544B4
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x000562BC File Offset: 0x000544BC
		public List<string> missionTypes { get; private set; } = new List<string>();

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x000562C5 File Offset: 0x000544C5
		public Color relationColor
		{
			get
			{
				if (!this.IsEnemy(Faction.player))
				{
					return ColorHelper.greenish;
				}
				return ColorHelper.reddish;
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000562E0 File Offset: 0x000544E0
		public Faction()
		{
			this.identifier = base.GetType().Name;
			this.name = "@FactionName" + this.identifier;
			this.description = "@FactionDesc" + this.identifier;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00056354 File Offset: 0x00054554
		public virtual Behaviour.Unit.SpaceShip GetRandomNPCShipType(int level, int minPoints, int maxPoints, GameplayType? activity)
		{
			FactionShipCollection npcshipTypes = this.GetNPCShipTypes(level, minPoints, maxPoints, activity);
			if (npcshipTypes.ownShips.Count > 0)
			{
				return SeededRandom.Global.Choose<Behaviour.Unit.SpaceShip>(npcshipTypes.ownShips);
			}
			if (npcshipTypes.alliedShips.Count > 0)
			{
				return SeededRandom.Global.Choose<Behaviour.Unit.SpaceShip>(npcshipTypes.alliedShips);
			}
			return SeededRandom.Global.Choose<Behaviour.Unit.SpaceShip>(npcshipTypes.fallbackShips);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000563BC File Offset: 0x000545BC
		public virtual FactionShipCollection GetNPCShipTypes(int level, int minPoints, int maxPoints, GameplayType? activity)
		{
			FactionShipCollection factionShipCollection = new FactionShipCollection();
			foreach (Behaviour.Unit.SpaceShip spaceShip in SpaceShip.allShips.Values)
			{
				if (spaceShip.pointValue >= minPoints && spaceShip.pointValue <= maxPoints && this.IsNPCShipAvailable(level, spaceShip, activity))
				{
					bool flag = false;
					foreach (FactionPrerequisites factionPrerequisites in spaceShip.shopItemData.factionPrereq)
					{
						if (factionPrerequisites.reqFaction == this)
						{
							factionShipCollection.ownShips.Add(spaceShip);
							flag = true;
							break;
						}
						if (factionPrerequisites.reqFaction == null || (!factionPrerequisites.reqFaction.IsEnemy(this) && factionPrerequisites.reqFaction.allowCrossFactionShipUse))
						{
							factionShipCollection.alliedShips.Add(spaceShip);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						factionShipCollection.fallbackShips.Add(spaceShip);
					}
				}
			}
			int num = this.minShipVariety - factionShipCollection.ownShips.Count;
			for (int i = 0; i < num; i++)
			{
				Behaviour.Unit.SpaceShip spaceShip2 = SeededRandom.Global.Choose<Behaviour.Unit.SpaceShip>(factionShipCollection.alliedShips);
				if (spaceShip2 != null)
				{
					factionShipCollection.ownShips.Add(spaceShip2);
				}
			}
			return factionShipCollection;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00056534 File Offset: 0x00054734
		public virtual bool IsNPCShipAvailable(int level, Behaviour.Unit.SpaceShip ship, GameplayType? activity)
		{
			return activity == null || ship.shipRoleType.GetGameplayType() == activity.Value;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00056555 File Offset: 0x00054755
		public void ChangePlayerReputation(int change)
		{
			FactionData.current.ChangeReputation(Faction.player, this, change);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00056568 File Offset: 0x00054768
		public void ChangeFactionReputation(Faction faction, int change)
		{
			FactionData.current.ChangeReputation(faction, this, change);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00056577 File Offset: 0x00054777
		public bool IsEnemy(Faction faction)
		{
			return FactionData.current != null && FactionData.current.IsEnemy(faction, this);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0005658E File Offset: 0x0005478E
		public bool IsDamageable(Faction faction)
		{
			return faction == null || this.IsEnemy(faction);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0005659C File Offset: 0x0005479C
		public int GetReputation(Faction other)
		{
			return FactionData.current.GetReputation(this, other);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x000565AA File Offset: 0x000547AA
		public ReputationLevel GetReputationLevel(Faction other)
		{
			return ReputationLevelExtensions.GetReputationLevel(FactionData.current.GetReputation(this, other));
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000565BD File Offset: 0x000547BD
		public void SetReputation(Faction other, int setTo)
		{
			FactionData.current.SetReputation(this, other, setTo);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000565CC File Offset: 0x000547CC
		public bool ReputationIsAtLeast(ReputationLevel level, Faction other)
		{
			int reputation = this.GetReputation(other);
			int num = ReputationLevelExtensions.ReputationThresholds[level];
			return reputation >= num;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x000565F4 File Offset: 0x000547F4
		public ConquestRank GetConquestRank()
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller == null)
			{
				return ConquestRank.None;
			}
			if (this.identifier == "Puppeteers")
			{
				return ConquestRankExtension.GetConquestRankLevel(storyteller.umbralContribution);
			}
			return ConquestRankExtension.GetConquestRankLevel(storyteller.GetFactionStanding(this).playerContribution);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00056640 File Offset: 0x00054840
		public virtual string GenerateStationName(MapPointOfInterest ss)
		{
			return Station.GenerateStationName();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00056648 File Offset: 0x00054848
		public static Faction Get(string id)
		{
			Faction faction;
			if (!Faction.allFactions.TryGetValue(id, out faction))
			{
				faction = Faction.Create(id);
				Faction.allFactions[id] = faction;
			}
			return faction;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00056678 File Offset: 0x00054878
		private static Faction Create(string id)
		{
			return (Faction)Type.GetType("Source.Galaxy.Factions." + id).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x000566A5 File Offset: 0x000548A5
		public Sprite GetIcon()
		{
			return Resources.Load<Sprite>("Sprites/FactionIcons/" + this.identifier + "Icon");
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000566C4 File Offset: 0x000548C4
		public Faction RandomEnemyFaction(SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			List<Faction> list = new List<Faction>(Faction.all);
			list.Remove(Faction.amalgam);
			list.Remove(Faction.holyRadicals);
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.IsEnemy(list[i]))
				{
					list.RemoveAt(i);
					i--;
				}
			}
			return random.Choose<Faction>(list);
		}

		// Token: 0x040006A2 RID: 1698
		private static Dictionary<string, Faction> allFactions = new Dictionary<string, Faction>();

		// Token: 0x040006A3 RID: 1699
		public static Faction player = Faction.Get("Player");

		// Token: 0x040006A4 RID: 1700
		public static Faction gold = Faction.Get("Gold");

		// Token: 0x040006A5 RID: 1701
		public static Faction red = Faction.Get("Red");

		// Token: 0x040006A6 RID: 1702
		public static Faction blue = Faction.Get("Blue");

		// Token: 0x040006A7 RID: 1703
		public static Faction miningGuild = Faction.Get("MiningGuild");

		// Token: 0x040006A8 RID: 1704
		public static Faction tradingGuild = Faction.Get("TradingGuild");

		// Token: 0x040006A9 RID: 1705
		public static Faction salvageGuild = Faction.Get("SalvageGuild");

		// Token: 0x040006AA RID: 1706
		public static Faction policeGuild = Faction.Get("PoliceGuild");

		// Token: 0x040006AB RID: 1707
		public static Faction bountyGuild = Faction.Get("BountyGuild");

		// Token: 0x040006AC RID: 1708
		public static Faction industrialGuild = Faction.Get("IndustrialGuild");

		// Token: 0x040006AD RID: 1709
		public static Faction stranded = Faction.Get("Stranded");

		// Token: 0x040006AE RID: 1710
		public static Faction mercenaryGuild = Faction.Get("MercenaryGuild");

		// Token: 0x040006AF RID: 1711
		public static Faction darkspacers = Faction.Get("Darkspacers");

		// Token: 0x040006B0 RID: 1712
		public static Faction smugglers = Faction.Get("Smugglers");

		// Token: 0x040006B1 RID: 1713
		public static Faction puppeteers = Faction.Get("Puppeteers");

		// Token: 0x040006B2 RID: 1714
		public static Faction marauders = Faction.Get("Marauders");

		// Token: 0x040006B3 RID: 1715
		public static Faction fanatics = Faction.Get("Fanatics");

		// Token: 0x040006B4 RID: 1716
		public static Faction amalgam = Faction.Get("Amalgam");

		// Token: 0x040006B5 RID: 1717
		public static Faction holyRadicals = Faction.Get("HolyRadicals");

		// Token: 0x040006B6 RID: 1718
		public static List<Faction> corporations = new List<Faction>
		{
			Faction.red,
			Faction.blue,
			Faction.gold
		};

		// Token: 0x040006BA RID: 1722
		public bool allowCrossFactionShipUse = true;

		// Token: 0x040006BB RID: 1723
		public int minShipVariety = 4;

		// Token: 0x040006BD RID: 1725
		public Color conquestColor = Color.clear;
	}
}
