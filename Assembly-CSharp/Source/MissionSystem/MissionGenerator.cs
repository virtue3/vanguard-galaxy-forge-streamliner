using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A5 RID: 165
	public abstract class MissionGenerator
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x000388B6 File Offset: 0x00036AB6
		public string identifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060006A4 RID: 1700
		public abstract bool canBeIdled { get; }

		// Token: 0x060006A5 RID: 1701
		public abstract Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null);

		// Token: 0x060006A6 RID: 1702
		public abstract GameplayType GetMissionType();

		// Token: 0x060006A7 RID: 1703 RVA: 0x000388C3 File Offset: 0x00036AC3
		public virtual float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			return 1f;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000388CC File Offset: 0x00036ACC
		protected TargetLayer GetDefaultTargetLayer(MissionDifficulty difficulty)
		{
			TargetLayer result = TargetLayer.Surface;
			if (difficulty != MissionDifficulty.Easy)
			{
				result = (SeededRandom.Global.RandomBool(0.3f) ? TargetLayer.Core : TargetLayer.Surface);
			}
			return result;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000388F8 File Offset: 0x00036AF8
		protected virtual void AddRewards(Mission mission, float itemValue, MissionDifficulty missionDifficulty, SeededRandom random, Faction faction)
		{
			bool flag = false;
			if (GamePlayer.current.GetStoryteller<Conquest>() != null)
			{
				flag = Conquest.conquestFactions.Contains(faction);
			}
			float rewardMultiplier = missionDifficulty.GetRewardMultiplier();
			float num = SkilltreeNode.PromptEngineeringBasicMissionRunner.isActive ? 1f : 0.5f;
			float num2 = SkilltreeNode.promptRepBonus.isActive ? 2f : 1f;
			Rarity rarity;
			switch (missionDifficulty)
			{
			case MissionDifficulty.Easy:
				rarity = (random.RandomBool(0.5f) ? Rarity.Enhanced : Rarity.Standard);
				break;
			case MissionDifficulty.Normal:
				rarity = Rarity.Enhanced;
				break;
			case MissionDifficulty.Hard:
				rarity = (random.RandomBool(0.5f) ? Rarity.HighGrade : Rarity.Enhanced);
				break;
			case MissionDifficulty.Skull:
				rarity = (random.RandomBool(0.1f) ? Rarity.Exotic : Rarity.HighGrade);
				break;
			case MissionDifficulty.Insane:
				rarity = (random.RandomBool(0.3f) ? Rarity.Exotic : Rarity.HighGrade);
				break;
			default:
				rarity = Rarity.Enhanced;
				break;
			}
			Rarity rarity2 = rarity;
			int num3 = Mathf.RoundToInt(itemValue * random.RandomRange(0.8f, 1.25f) * (this.idle ? (0.33f * num) : 1f));
			float num4 = this.idle ? (0.05f * num) : 0.3f;
			if (this.idle && SkilltreeNode.promptItemRewardBonus.isActive)
			{
				num4 *= (float)(1 + SkilltreeNode.promptItemRewardBonus.currentPoints);
			}
			if (Conquest.conquestFactions.Contains(faction) && (mission.sourcePoi is EmbassyStation || mission.sourcePoi is ConquestStation))
			{
				float num5 = this.idle ? random.RandomRange(1f, 3f) : random.RandomRange(3f, 6f);
				if (this.idle && SkilltreeNode.promptFleetStrengthBoost.isActive)
				{
					num5 = (float)Mathf.CeilToInt(num5 * SkilltreeNode.promptFleetStrengthBoost.currentIncrease);
				}
				if (Conquest.conquestFactions.Contains(mission.enemyFaction) && mission.enemyFaction != Faction.marauders && mission.enemyFaction != Faction.fanatics)
				{
					num5 *= 2f;
				}
				ConquestRank conquestRank = faction.GetConquestRank();
				float num6 = 1f;
				if (flag)
				{
					num6 += conquestRank.GetFleetStrengthRewardBonus(faction.identifier);
				}
				mission.rewards.Add(new ConquestStrength
				{
					amount = Mathf.RoundToInt(num5 * num6)
				});
				InventoryItemType item = "ConquestCurrency";
				float num7 = 1f;
				if (flag)
				{
					num7 += conquestRank.GetMissionCommendationsRewardBonus(faction.identifier);
				}
				int amount = Mathf.RoundToInt(random.RandomRange(num5, num5 * 2f) * GameMath.CostMultiplier(mission.sourcePoi.level - 30) * num7);
				mission.rewards.Add(new Item
				{
					item = item,
					amount = amount
				});
			}
			if (random.RandomBool(num4))
			{
				mission.rewards.Add(new Item
				{
					item = random.Choose<EquipmentBuilder>(EquipmentBuilder.GetItemsForMissionReward(mission)).CreateItemType(rarity2, mission.level + 1, false, null, false, false)
				});
				if (!this.idle && rarity2 <= Rarity.Enhanced)
				{
					mission.rewards.Add(this.ExtraCreditReward(rarity2, itemValue, num, this.idle, random, faction));
				}
			}
			else
			{
				SpaceStation current = SpaceStation.current;
				if (((current != null) ? current.forge : null) != null && random.RandomBool(this.idle ? (0.05f * num) : 0.2f))
				{
					InventoryItemType inventoryItemType = ItemBuilder.Get("Blueprint").CreateRandomBlueprint(mission.level, new Rarity?(rarity2), null, true);
					if (inventoryItemType)
					{
						mission.rewards.Add(new Item
						{
							item = inventoryItemType
						});
						if (!this.idle && rarity2 <= Rarity.Enhanced)
						{
							mission.rewards.Add(this.ExtraCreditReward(rarity2, itemValue, num, this.idle, random, faction));
						}
					}
					else
					{
						mission.rewards.Add(this.CreditReward(num3, faction));
					}
				}
				else if (num3 > 0)
				{
					mission.rewards.Add(this.CreditReward(num3, faction));
					if (random.RandomBool(this.idle ? (0.1f * num) : 0.25f))
					{
						ItemBuilder itemBuilder = ItemBuilder.Get("WarpFuel");
						mission.rewards.Insert(0, new Item
						{
							item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f)
						});
					}
				}
			}
			Faction faction2;
			if ((faction2 = mission.sourceFaction) == null)
			{
				MapPointOfInterest sourcePoi = mission.sourcePoi;
				faction2 = ((sourcePoi != null) ? sourcePoi.faction : null);
			}
			Faction faction3 = faction2;
			float num8 = 1f;
			if (faction3 != null && faction3.IsEnemy(Faction.player))
			{
				mission.rewards.Clear();
				num8 = 4f;
			}
			int[] list = new int[]
			{
				200,
				250,
				300,
				350
			};
			float num9 = 1f;
			if (flag)
			{
				num9 += faction.GetConquestRank().GetReputationBonus();
			}
			mission.rewards.Add(new Reputation
			{
				amount = (int)((float)random.Choose<int>(list) * (this.idle ? (0.1f * num * num2) : 1f) * num8 * num9),
				faction = faction3
			});
			float num10 = (float)GameMath.GetExperienceRewardValue(Mathf.Sqrt(rewardMultiplier) * 50f, mission.level) * (this.idle ? (0.5f * num) : 1f);
			if (num10 > 1f && GamePlayer.current.level < GameMath.maxLevel)
			{
				mission.rewards.Add(new Experience
				{
					amount = Mathf.RoundToInt(num10 * random.RandomRange(0.8f, 1.25f))
				});
			}
			if (faction == Faction.salvageGuild && mission.level > 15)
			{
				mission.rewards.Add(new WorkshopCredit
				{
					amount = Mathf.RoundToInt(itemValue * random.RandomRange(0.8f, 1.25f) * (this.idle ? (0.33f * num) : 1f))
				});
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00038EFC File Offset: 0x000370FC
		protected Credits CreditReward(int amount, Faction faction)
		{
			ReputationLevel reputationLevel = faction.GetReputationLevel(Faction.player);
			float num = 1f;
			num += reputationLevel.GetMissionRewardMultiplier();
			bool flag = false;
			if (GamePlayer.current.GetStoryteller<Conquest>() != null)
			{
				flag = Conquest.conquestFactions.Contains(faction);
			}
			if (flag)
			{
				num += faction.GetConquestRank().GetCreditRewardMultiplier();
			}
			return new Credits
			{
				amount = Mathf.RoundToInt((float)amount * num)
			};
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00038F64 File Offset: 0x00037164
		private Credits ExtraCreditReward(Rarity rarity, float itemValue, float idleFactor, bool idle, SeededRandom random, Faction faction)
		{
			float num = itemValue * random.RandomRange(0.8f, 1.25f) * (idle ? (0.33f * idleFactor) : 1f);
			float num2 = (rarity == Rarity.Standard) ? 0.5f : 0.25f;
			return this.CreditReward(Mathf.RoundToInt(num * num2), faction);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00038FB8 File Offset: 0x000371B8
		protected virtual float GetRewardValue(int level)
		{
			return (float)GameMath.GetCreditsValue(40f, level);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00038FC8 File Offset: 0x000371C8
		public static MissionGenerator Get(string id)
		{
			MissionGenerator missionGenerator;
			if (!MissionGenerator.generators.TryGetValue(id, out missionGenerator))
			{
				missionGenerator = MissionGenerator.Create(id);
				MissionGenerator.generators[id] = missionGenerator;
			}
			return missionGenerator;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00038FF8 File Offset: 0x000371F8
		private static MissionGenerator Create(string id)
		{
			return (MissionGenerator)Type.GetType("Source.MissionSystem.Generator." + id).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00039028 File Offset: 0x00037228
		public static Mission GenerateRandomMission(SpaceStation poi, SeededRandom random, GameplayType? typeFilter = null, bool idle = false, TargetLayer? targetLayer = null)
		{
			MissionDifficulty missionDifficulty = SkilltreeNode.promptEngineeringEnhancedMissionRunner.isActive ? MissionDifficulty.Normal : MissionDifficulty.Easy;
			MissionDifficulty difficulty = idle ? missionDifficulty : MissionDifficultyExtension.GetMissionDifficulty(poi.level, poi.level >= 3, poi.level >= 5, poi.level >= 20, poi.level >= 50, random);
			Dictionary<MissionGenerator, float> dictionary = new Dictionary<MissionGenerator, float>();
			foreach (string id in poi.faction.missionTypes)
			{
				MissionGenerator missionGenerator = MissionGenerator.Get(id);
				if (typeFilter != null)
				{
					GameplayType missionType = missionGenerator.GetMissionType();
					GameplayType? gameplayType = typeFilter;
					if (!(missionType == gameplayType.GetValueOrDefault() & gameplayType != null))
					{
						continue;
					}
				}
				if (!idle || missionGenerator.canBeIdled)
				{
					dictionary[missionGenerator] = missionGenerator.GetMissionChance(poi, difficulty, random);
				}
			}
			MissionGenerator missionGenerator2 = random.Choose<MissionGenerator>(dictionary);
			if (missionGenerator2 != null)
			{
				missionGenerator2.idle = idle;
				return MissionGenerator.GenerateMission(missionGenerator2, difficulty, poi, random, targetLayer);
			}
			return null;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00039140 File Offset: 0x00037340
		public static Mission GenerateMission(MissionGenerator gen, MissionDifficulty difficulty, SpaceStation poi, SeededRandom random, TargetLayer? targetLayer = null)
		{
			Mission mission = gen.GenerateMission(poi, difficulty, random, targetLayer);
			if (mission != null)
			{
				mission.difficulty = difficulty;
				if (mission.sourcePoi == null)
				{
					mission.sourcePoi = poi;
				}
				if (mission.sourceFaction == null)
				{
					mission.sourceFaction = poi.faction;
				}
				if (mission.sourceName == null)
				{
					mission.sourceName = poi.faction.name;
				}
				if (mission.turnIn == null)
				{
					mission.turnIn = poi;
				}
			}
			return mission;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000391B0 File Offset: 0x000373B0
		public virtual Faction GetEnemyFaction(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random)
		{
			Faction faction = Faction.marauders;
			if (!GamePlayer.current.IsPrologueActive() && random.RandomBool(0.5f) && poi.IsEnemyAvailable(Faction.fanatics))
			{
				faction = Faction.fanatics;
			}
			if (poi.system.storyteller is ConquestSystem && poi.faction == poi.system.faction)
			{
				List<Faction> list = new List<Faction>
				{
					Faction.marauders,
					Faction.fanatics
				};
				foreach (SystemMapData systemMapData in poi.system.GetAdjacentSystems())
				{
					if (systemMapData != null && systemMapData.faction != poi.faction)
					{
						list.Add(systemMapData.faction);
					}
				}
				if (random.RandomBool(0.5f))
				{
					list.AddRange(Faction.corporations);
				}
				list.Remove(poi.faction);
				if (list.Count > 0)
				{
					faction = random.Choose<Faction>(list);
				}
			}
			if (poi.faction == faction)
			{
				if (faction == Faction.marauders)
				{
					faction = random.Choose<Faction>(Faction.corporations);
				}
				else
				{
					faction = Faction.marauders;
				}
			}
			if (faction == Faction.marauders && !faction.IsEnemy(Faction.player))
			{
				faction = Faction.fanatics;
			}
			return faction;
		}

		// Token: 0x040003BD RID: 957
		private static Dictionary<string, MissionGenerator> generators = new Dictionary<string, MissionGenerator>();

		// Token: 0x040003BE RID: 958
		public bool idle;
	}
}
