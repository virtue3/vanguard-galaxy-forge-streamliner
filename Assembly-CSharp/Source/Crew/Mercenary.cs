using System;
using Behaviour.Unit;
using LightJson;
using Source.Galaxy.Factions;
using Source.Item;
using Source.Util;

namespace Source.Crew
{
	// Token: 0x0200012B RID: 299
	public class Mercenary : CommanderData
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00053528 File Offset: 0x00051728
		public float repairTime
		{
			get
			{
				return 120f * MercenaryGuild.GetRepairTimeFactor();
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00053535 File Offset: 0x00051735
		public int creditCost
		{
			get
			{
				return (int)((float)this.GetRarityCostMultiplier() * this.GetSpaceShip().costMultiplier * 150f);
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00053554 File Offset: 0x00051754
		public Mercenary(string seed, Gender? gender = null, string callsign = null, string faction = null)
		{
			this.seed = seed;
			SeededRandom random = new SeedGenerator().Add(seed).CreateRandom();
			base.SetRandom(random, gender, callsign, faction);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00053595 File Offset: 0x00051795
		public void AddMercTime(int hours = 1)
		{
			this.timeLeft += 3600f * (float)hours;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x000535AC File Offset: 0x000517AC
		public GameplayType GetGameplayType()
		{
			Profession profession = this.profession;
			GameplayType result;
			if (profession != Profession.Mining)
			{
				if (profession != Profession.Salvaging)
				{
					result = GameplayType.Combat;
				}
				else
				{
					result = GameplayType.Salvage;
				}
			}
			else
			{
				result = GameplayType.Mining;
			}
			return result;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x000535D8 File Offset: 0x000517D8
		public UnitRank GetUnitRank()
		{
			UnitRank result;
			switch (this.rarity)
			{
			case Rarity.Standard:
				result = UnitRank.Standard;
				break;
			case Rarity.Enhanced:
				result = UnitRank.Veteran;
				break;
			case Rarity.HighGrade:
				result = UnitRank.Elite;
				break;
			case Rarity.Exotic:
				result = UnitRank.Champion;
				break;
			case Rarity.Legendary:
				result = UnitRank.Commander;
				break;
			default:
				throw new NotImplementedException(this.rarity.ToString() + " kon niet gematched worden met een rank!");
			}
			return result;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0005363E File Offset: 0x0005183E
		public Behaviour.Unit.SpaceShip GetSpaceShip()
		{
			if (this.spaceShip == null)
			{
				this.spaceShip = Behaviour.Unit.SpaceShip.Get(this.ship);
			}
			return this.spaceShip;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00053668 File Offset: 0x00051868
		public int GetRarityCostMultiplier()
		{
			int result;
			switch (this.rarity)
			{
			case Rarity.Standard:
				result = 10;
				break;
			case Rarity.Enhanced:
				result = 12;
				break;
			case Rarity.HighGrade:
				result = 14;
				break;
			case Rarity.Exotic:
				result = 16;
				break;
			case Rarity.Legendary:
				result = 18;
				break;
			default:
				result = 10;
				break;
			}
			return result;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000536B8 File Offset: 0x000518B8
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			result["seed"] = this.seed;
			result["profession"] = this.profession.ToString();
			result["rarity"] = this.rarity.ToString();
			result["ship"] = this.ship;
			result["timeLeft"] = new double?((double)this.timeLeft);
			result["behaviour"] = this.behaviour.ToString();
			result["autoExtend"] = new bool?(this.autoExtend);
			result["repairing"] = new bool?(this.repairing);
			result["repUpdateTimer"] = new double?((double)this.repUpdateTimer);
			result["battlecry"] = this.battlecry;
			result["isExtra"] = new bool?(this.isExtra);
			return result;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00053808 File Offset: 0x00051A08
		public new static Mercenary FromJson(JsonValue data)
		{
			Mercenary mercenary = new Mercenary(data["seed"], null, null, null);
			mercenary.DataFromJson(data.AsJsonObject);
			mercenary.profession = Enum.Parse<Profession>(data["profession"]);
			mercenary.rarity = Enum.Parse<Rarity>(data["rarity"]);
			mercenary.ship = data["ship"];
			mercenary.timeLeft = (float)data["timeLeft"].AsNumber;
			if (!data["behaviour"].IsNull)
			{
				mercenary.behaviour = Enum.Parse<WingmanBehaviour>(data["behaviour"]);
			}
			if (!data["autoExtend"].IsNull)
			{
				mercenary.autoExtend = data["autoExtend"].AsBoolean;
			}
			if (!data["repairing"].IsNull)
			{
				mercenary.repairing = data["repairing"].AsBoolean;
			}
			mercenary.repUpdateTimer = (float)data["repUpdateTimer"].AsNumber;
			if (!data["battlecry"].IsNull)
			{
				mercenary.battlecry = data["battlecry"];
			}
			if (!data["isExtra"].IsNull)
			{
				mercenary.isExtra = data["isExtra"].AsBoolean;
			}
			return mercenary;
		}

		// Token: 0x0400061D RID: 1565
		public const float MercTime = 3600f;

		// Token: 0x0400061E RID: 1566
		public const float BaseRepairTime = 120f;

		// Token: 0x0400061F RID: 1567
		public const float RepUpdateTime = 60f;

		// Token: 0x04000620 RID: 1568
		public Profession profession;

		// Token: 0x04000621 RID: 1569
		public Rarity rarity;

		// Token: 0x04000622 RID: 1570
		public string ship;

		// Token: 0x04000623 RID: 1571
		public WingmanBehaviour behaviour;

		// Token: 0x04000624 RID: 1572
		public string seed;

		// Token: 0x04000625 RID: 1573
		public string battlecry;

		// Token: 0x04000626 RID: 1574
		public bool isExtra;

		// Token: 0x04000627 RID: 1575
		public float timeLeft = 3600f;

		// Token: 0x04000628 RID: 1576
		public float repUpdateTimer;

		// Token: 0x04000629 RID: 1577
		public bool autoExtend;

		// Token: 0x0400062A RID: 1578
		public bool repairing;

		// Token: 0x0400062B RID: 1579
		private Behaviour.Unit.SpaceShip spaceShip;
	}
}
