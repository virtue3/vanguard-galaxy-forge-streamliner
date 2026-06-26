using System;
using System.Collections.Generic;
using System.Linq;
using LightJson;
using Source.Crew;
using Source.Item;
using Source.SpaceShip;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000164 RID: 356
	public class RecruitmentCenter : IJsonSource
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x00062944 File Offset: 0x00060B44
		public RecruitmentCenter(SpaceStation spaceStation)
		{
			this.station = spaceStation;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0006299F File Offset: 0x00060B9F
		public void ClearMercs()
		{
			this.combatters.Clear();
			this.miners.Clear();
			this.salvagers.Clear();
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x000629C4 File Offset: 0x00060BC4
		public void CreateRecruits(Profession profession, bool force = false)
		{
			List<Mercenary> mercenaries = this.GetMercenaries(profession);
			if (force)
			{
				mercenaries.Clear();
			}
			if (mercenaries.Count == 0)
			{
				Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> ships = this.GetShips(profession);
				List<ValueTuple<SpaceShipRole, SpaceShipType>> list = ships.Keys.ToList<ValueTuple<SpaceShipRole, SpaceShipType>>();
				foreach (Rarity rarity in this.raritiesAvailable)
				{
					foreach (ValueTuple<SpaceShipRole, SpaceShipType> valueTuple in list)
					{
						if (valueTuple.Item2 < SpaceShipType.Size5)
						{
							this.CreateMercenary(profession, rarity, SeededRandom.Global.Choose<string>(ships[valueTuple]));
						}
					}
				}
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00062AA0 File Offset: 0x00060CA0
		private Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> GetShips(Profession profession)
		{
			Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> result;
			if (profession != Profession.Mining)
			{
				if (profession != Profession.Salvaging)
				{
					result = RecruitmentCenter.combatShips;
				}
				else
				{
					result = RecruitmentCenter.salvageShips;
				}
			}
			else
			{
				result = RecruitmentCenter.miningShips;
			}
			return result;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00062AD0 File Offset: 0x00060CD0
		public List<Mercenary> GetMercenaries(Profession profession)
		{
			List<Mercenary> result;
			if (profession != Profession.Mining)
			{
				if (profession != Profession.Salvaging)
				{
					result = this.combatters;
				}
				else
				{
					result = this.salvagers;
				}
			}
			else
			{
				result = this.miners;
			}
			return result;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00062B04 File Offset: 0x00060D04
		private void CreateMercenary(Profession profession, Rarity rarity, string ship)
		{
			string text = SeededRandom.Global.RandomItemSeed();
			List<Mercenary> mercenaries = this.GetMercenaries(profession);
			string seed = text;
			Faction faction = this.station.faction;
			string faction2 = (faction != null) ? faction.identifier : null;
			mercenaries.Add(new Mercenary(seed, null, null, faction2)
			{
				profession = profession,
				rarity = rarity,
				ship = ship
			});
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00062B66 File Offset: 0x00060D66
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"mercenaries",
					this.combatters.ToJsonArray<Mercenary>()
				}
			};
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00062B8E File Offset: 0x00060D8E
		public static RecruitmentCenter FromJson(JsonObject data, SpaceStation ss)
		{
			RecruitmentCenter recruitmentCenter = new RecruitmentCenter(ss);
			recruitmentCenter.combatters.FromJsonArray(data["mercenaries"], new ClassExtensions.ParseJsonValue<Mercenary>(Mercenary.FromJson));
			return recruitmentCenter;
		}

		// Token: 0x04000771 RID: 1905
		private SpaceStation station;

		// Token: 0x04000772 RID: 1906
		private List<Mercenary> combatters = new List<Mercenary>();

		// Token: 0x04000773 RID: 1907
		private List<Mercenary> miners = new List<Mercenary>();

		// Token: 0x04000774 RID: 1908
		private List<Mercenary> salvagers = new List<Mercenary>();

		// Token: 0x04000775 RID: 1909
		public static Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> combatShips = new Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>>
		{
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Combat, SpaceShipType.Size1),
				new List<string>
				{
					"Zephyr",
					"Raptor",
					"Margil",
					"Cudal"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Combat, SpaceShipType.Size2),
				new List<string>
				{
					"Shrike",
					"Tergal"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Combat, SpaceShipType.Size3),
				new List<string>
				{
					"Sarnil",
					"Cyclone"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Combat, SpaceShipType.Size4),
				new List<string>
				{
					"Tempest"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Combat, SpaceShipType.Size5),
				new List<string>
				{
					"Hurricane",
					"Typhoon"
				}
			}
		};

		// Token: 0x04000776 RID: 1910
		public static Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> miningShips = new Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>>
		{
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Mining, SpaceShipType.Size1),
				new List<string>
				{
					"Bore",
					"Chisel Mk I",
					"Chisel Mk II",
					"Garnil"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Mining, SpaceShipType.Size2),
				new List<string>
				{
					"Foreman DC-2",
					"Pickaxe LM",
					"Gale"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Mining, SpaceShipType.Size3),
				new List<string>
				{
					"Moxe",
					"Auger ERC-2",
					"Delvir"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Mining, SpaceShipType.Size4),
				new List<string>
				{
					"Warden DSF-3"
				}
			}
		};

		// Token: 0x04000777 RID: 1911
		public static Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> salvageShips = new Dictionary<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>>
		{
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Salvaging, SpaceShipType.Size1),
				new List<string>
				{
					"Chisel Mk I SN",
					"Squall"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Salvaging, SpaceShipType.Size2),
				new List<string>
				{
					"Tugbit",
					"Rekkem",
					"Kite"
				}
			},
			{
				new ValueTuple<SpaceShipRole, SpaceShipType>(SpaceShipRole.Salvaging, SpaceShipType.Size3),
				new List<string>
				{
					"Fultor",
					"Exdyne",
					"Bero",
					"Osprey"
				}
			}
		};

		// Token: 0x04000778 RID: 1912
		private List<Rarity> raritiesAvailable = new List<Rarity>
		{
			Rarity.Enhanced,
			Rarity.HighGrade,
			Rarity.Exotic
		};
	}
}
