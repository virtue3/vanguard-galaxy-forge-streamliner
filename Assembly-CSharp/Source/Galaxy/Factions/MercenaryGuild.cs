using System;
using System.Collections.Generic;
using Source.Crew;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000182 RID: 386
	public class MercenaryGuild : Faction
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x000663EF File Offset: 0x000645EF
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat;
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00066404 File Offset: 0x00064604
		public MercenaryGuild()
		{
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("TradeTerminal");
			base.missionTypes.Add("StationBattle");
			base.missionTypes.Add("EscortShip");
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00066477 File Offset: 0x00064677
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationBounty.GenerateOrsanonStationName();
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00066480 File Offset: 0x00064680
		public static float GetRepairTimeFactor()
		{
			float result;
			switch (Faction.mercenaryGuild.GetReputationLevel(Faction.player))
			{
			case ReputationLevel.Neutral:
				result = 1f;
				break;
			case ReputationLevel.Cordial:
				result = 0.9f;
				break;
			case ReputationLevel.Friendly:
				result = 0.8f;
				break;
			case ReputationLevel.Respected:
				result = 0.7f;
				break;
			case ReputationLevel.Distinguished:
				result = 0.6f;
				break;
			case ReputationLevel.Exalted:
				result = 0.5f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x040007D6 RID: 2006
		public static List<ExtraMercenaryPreset> extraMercenaryPresets = new List<ExtraMercenaryPreset>
		{
			new ExtraMercenaryPreset("DByte", new string[]
			{
				"...."
			}, Gender.Male, "dbyte_seed"),
			new ExtraMercenaryPreset("The Old Golem", new string[]
			{
				"I got you covered!"
			}, Gender.Female, "old_golem_seed"),
			new ExtraMercenaryPreset("Irrelevant", new string[]
			{
				"We are leaves on the wind!"
			}, Gender.Female, "leaves_seed"),
			new ExtraMercenaryPreset("Jindan Belt", new string[]
			{
				"Here to walk the walk"
			}, Gender.Male, "jindan_seed"),
			new ExtraMercenaryPreset("Egnerion", new string[]
			{
				"Dragon on station. You're under my wings."
			}, Gender.Male, "egnerion_seed"),
			new ExtraMercenaryPreset("Disassembler", new string[]
			{
				"Salvaging formation!"
			}, Gender.Male, "disassembler_seed"),
			new ExtraMercenaryPreset("Zemurin", new string[]
			{
				"There better be some profit in this!"
			}, Gender.Male, "zemurin_seed"),
			new ExtraMercenaryPreset("Pinecone", new string[]
			{
				"Shields up, rails hot! On your command"
			}, Gender.Female, "pinecone_seed"),
			new ExtraMercenaryPreset("Insane Cleem", new string[]
			{
				"Need some candy?",
				"Currahee!"
			}, Gender.Male, "insance_cleem_seed"),
			new ExtraMercenaryPreset("Erelith, Vessel of the Holy Ones", new string[]
			{
				"The holy swarm deems you worthy of my aid",
				"The drones called your name… I obey.",
				"You’re not alone. You’re never alone.",
				"You were judged. You passed."
			}, Gender.Female, "erelith_seed"),
			new ExtraMercenaryPreset("FrogEater", new string[]
			{
				"Baguette to the rescue!"
			}, Gender.Male, "frogeater_seed"),
			new ExtraMercenaryPreset("Laser Chud", new string[]
			{
				"I'll handle those punks!"
			}, Gender.Male, "laser_chud_seed"),
			new ExtraMercenaryPreset("Giga", new string[]
			{
				"No surrender. No mercy. Only Giga."
			}, Gender.Male, "giga_seed"),
			new ExtraMercenaryPreset("Screaming Angel", new string[]
			{
				"Time to lighten the ammo supply!"
			}, Gender.Female, "screaming_angel_seed"),
			new ExtraMercenaryPreset("ROSE", new string[]
			{
				"Every Rose... Has It's Thorn!"
			}, Gender.Female, "rose_seed"),
			new ExtraMercenaryPreset("Strong Arm", new string[]
			{
				"Time for a BBQ"
			}, Gender.Male, "strong_arm_seed"),
			new ExtraMercenaryPreset("Azdule", new string[]
			{
				"Follow me to Victory!"
			}, Gender.Male, "azdule_seed"),
			new ExtraMercenaryPreset("The Bear", new string[]
			{
				"roar"
			}, Gender.Male, "the_bear_seed")
		};
	}
}
