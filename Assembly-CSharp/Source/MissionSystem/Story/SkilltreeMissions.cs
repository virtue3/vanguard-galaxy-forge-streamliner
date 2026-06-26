using System;
using System.Collections.Generic;
using Behaviour.Equipment;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Unit;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Story
{
	// Token: 0x020000AE RID: 174
	public class SkilltreeMissions
	{
		// Token: 0x0600070A RID: 1802 RVA: 0x0003AB7D File Offset: 0x00038D7D
		public SkilltreeMissions()
		{
			this.SkilltreeMissionMining();
			this.SkilltreeMissionCombat();
			this.SkilltreeMissionSalvage();
			this.SkilltreeMissionIndustrial();
			this.SkilltreeMissionDrones();
			this.SkilltreeMissionDefense();
			this.SkilltreeMissionEconomy();
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0003ABB0 File Offset: 0x00038DB0
		private void SkilltreeMissionMining()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionMining", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SkilltreeMissionMining", Array.Empty<object>());
				mission.description = Translation.Translate("@SkilltreeMissionMiningDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SkilltreeMissionMiningComplete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.turnIn = MapPointOfInterest.current;
				mission.sourceFaction = Faction.miningGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
				{
					requiredAmount = 10,
					itemCategory = new ItemCategory?(ItemCategory.Ore)
				});
				missionStep.objectives.Add(new CreditOffer
				{
					requiredAmount = 10000,
					deliverTo = (MapPointOfInterest.current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Skilltree
				{
					treeName = "Mining"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.miningGuild,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Mining"), "Unlock Mining skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionMining2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionMining2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionMining2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionMining2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionMining2Complete", Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.miningGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
				{
					requiredAmount = (flag ? 2000 : 1000),
					itemCategory = new ItemCategory?(ItemCategory.Ore)
				});
				missionStep.objectives.Add(new CollectItemTypes
				{
					requiredAmount = (flag ? 30 : 15),
					itemCategory = new ItemCategory?(ItemCategory.Ore)
				});
				mission.steps.Add(missionStep);
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.miningGuild,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Mining skill test"));
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0003AC44 File Offset: 0x00038E44
		private void SkilltreeMissionCombat()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionCombat", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SkilltreeMissionCombat", Array.Empty<object>());
				mission.description = Translation.Translate("@SkilltreeMissionCombatDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SkilltreeMissionCombatComplete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.turnIn = MapPointOfInterest.current;
				mission.sourceFaction = Faction.bountyGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				MapPointOfInterest mapPointOfInterest = MapPointOfInterest.current.system.SetupPOI(new Combat(), null, null, 0) as MapPointOfInterest;
				mapPointOfInterest.faction = Faction.marauders;
				mapPointOfInterest.dangerLevel = "@MapPOIDangerPirates";
				mapPointOfInterest.AddGuards(mapPointOfInterest.CreateFixedPayload("PirateDrone", 2, null, null, UnitRank.Rookie), null);
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new KillEnemies
				{
					requiredAmount = 2,
					enemyFaction = Faction.marauders
				});
				missionStep.objectives.Add(new CreditOffer
				{
					requiredAmount = 10000,
					deliverTo = (MapPointOfInterest.current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Skilltree
				{
					treeName = "Combat - Offensive"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.bountyGuild,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Combat - Offensive"), "Unlock Combat skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionCombat2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionCombat2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionCombat2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionCombat2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionComplete" + (flag ? "2" : "1"), Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.bountyGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new KillEnemies
				{
					requiredAmount = (flag ? 1000 : 500),
					enemyFaction = Faction.marauders
				});
				mission.steps.Add(missionStep);
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.bountyGuild,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Combat skill test"));
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0003ACD8 File Offset: 0x00038ED8
		private void SkilltreeMissionSalvage()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionSalvage", delegate(GamePlayer ply)
			{
				MapPointOfInterest current = MapPointOfInterest.current;
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionSalvage", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionSalvageDesc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionSalvageComplete", Array.Empty<object>()),
					sourcePoi = current,
					turnIn = current,
					sourceFaction = Faction.salvageGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				InventoryItemType inventoryItemType = "SalvageOreMissionItem0";
				Source.Galaxy.POI.Salvage salvage = new Source.Galaxy.POI.Salvage();
				current.system.SetupPOI(salvage, null, null, 0);
				for (int i = 0; i < 3; i++)
				{
					SalvageData salvageData = new SalvageData
					{
						position = salvage.GetWorldPosition() + new Vector2((float)SeededRandom.Global.RandomRange(0, 20), SeededRandom.Global.RandomRange(-12f, 12f)),
						angle = (float)SeededRandom.Global.RandomRange(0, 360),
						shipTemplate = "AncientWreck",
						initialBattleDamage = 80
					};
					salvageData.scrapContent.Add(inventoryItemType, 23);
					salvage.AddPersistable(salvageData);
				}
				MissionStep missionStep = new MissionStep
				{
					system = current.system,
					dynamicPointOfInterest = salvage
				};
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
				{
					requiredAmount = 10,
					itemType = inventoryItemType
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = current.system
				};
				missionStep2.objectives.Add(new TradeOffer
				{
					itemType = inventoryItemType,
					requiredAmount = 10,
					deliverTo = (current as SpaceStation)
				});
				missionStep2.objectives.Add(new CreditOffer
				{
					requiredAmount = 10000,
					deliverTo = (current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Skilltree
				{
					treeName = "Salvaging"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.salvageGuild,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Salvaging"), "Unlock Salvage skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionSalvage2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionSalvage2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionSalvage2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionSalvage2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionComplete" + (flag ? "2" : "1"), Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.salvageGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = (flag ? 50 : 25),
					trigger = MissionTrigger.SalvagedModule,
					description = "Equipment salvaged"
				});
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
				{
					requiredAmount = (flag ? 1000 : 500),
					itemCategory = new ItemCategory?(ItemCategory.Salvage)
				});
				mission.steps.Add(missionStep);
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.salvageGuild,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Salvage skill test"));
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0003AD6C File Offset: 0x00038F6C
		private void SkilltreeMissionIndustrial()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionIndustrial", delegate(GamePlayer ply)
			{
				MapPointOfInterest current = MapPointOfInterest.current;
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionIndustrial", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionIndustrialDesc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionIndustrialComplete", Array.Empty<object>()),
					sourcePoi = current,
					turnIn = current,
					sourceFaction = Faction.industrialGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep
				{
					system = current.system
				};
				missionStep.objectives.Add(new TradeOffer
				{
					requiredAmount = 2,
					itemType = "Titanium Plate",
					deliverTo = (current as SpaceStation)
				});
				missionStep.objectives.Add(new TradeOffer
				{
					requiredAmount = 2,
					itemType = "Graphite Wafers",
					deliverTo = (current as SpaceStation)
				});
				missionStep.objectives.Add(new CreditOffer
				{
					requiredAmount = 10000,
					deliverTo = (current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				InventoryItemType inventoryItemType = ItemBuilder.Get("Blueprint").CreateRandomBlueprint(current.level, null, null, true);
				if (inventoryItemType)
				{
					mission.rewards.Add(new Source.MissionSystem.Rewards.Item
					{
						item = inventoryItemType
					});
				}
				mission.rewards.Add(new Skilltree
				{
					treeName = "Industrial"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.industrialGuild,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Industrial"), "Unlock Industrial skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionIndustrial2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionIndustrial2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionIndustrial2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionIndustrial2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionComplete" + (flag ? "2" : "1"), Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.industrialGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new Crafting
				{
					requiredAmount = (flag ? 500 : 250),
					requiredCategory = new ItemCategory?(ItemCategory.RefinedProduct)
				});
				missionStep.objectives.Add(new Crafting
				{
					requiredAmount = (flag ? 40 : 20),
					requiredRarity = new Rarity?(Rarity.Enhanced),
					requiredEquipment = new bool?(true)
				});
				missionStep.objectives.Add(new Crafting
				{
					requiredAmount = (flag ? 10 : 5),
					requiredRarity = new Rarity?(Rarity.HighGrade),
					requiredEquipment = new bool?(true)
				});
				mission.steps.Add(missionStep);
				InventoryItemType inventoryItemType = ItemBuilder.Get("Blueprint").CreateRandomBlueprint(MapPointOfInterest.current.level, new Rarity?(Rarity.HighGrade), null, true);
				if (inventoryItemType)
				{
					mission.rewards.Add(new Source.MissionSystem.Rewards.Item
					{
						item = inventoryItemType
					});
				}
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.industrialGuild,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Industrial skill test"));
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0003AE00 File Offset: 0x00039000
		private void SkilltreeMissionDrones()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionDrones", delegate(GamePlayer ply)
			{
				MapPointOfInterest current = MapPointOfInterest.current;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SkilltreeMissionDrones", Array.Empty<object>());
				mission.description = Translation.Translate("@SkilltreeMissionDronesDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SkilltreeMissionDronesComplete", Array.Empty<object>());
				mission.sourcePoi = current;
				mission.turnIn = current;
				mission.sourceFaction = Faction.gold;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				MissionStep missionStep = new MissionStep
				{
					system = current.system
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = 1,
					trigger = MissionTrigger.EquipDroneShip,
					description = "Drone ship equipped"
				});
				missionStep.objectives.Add(new CreditOffer
				{
					requiredAmount = 10000,
					deliverTo = (current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Skilltree
				{
					treeName = "Drones"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.gold,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Drones"), "Unlock Drones skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionDrones2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionDrones2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionDrones2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionDrones2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionComplete" + (flag ? "2" : "1"), Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.gold,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new DroneTrigger
				{
					requiredAmount = (flag ? 400 : 200),
					trigger = MissionTrigger.UnitDestroyed,
					description = "Hostiles destroyed by drones"
				});
				missionStep.objectives.Add(new DroneTrigger
				{
					requiredAmount = (flag ? 400 : 200),
					trigger = MissionTrigger.MinedOre,
					description = "Ore mined by drones"
				});
				missionStep.objectives.Add(new DroneTrigger
				{
					requiredAmount = (flag ? 400 : 200),
					trigger = MissionTrigger.SalvagedItem,
					description = "Items salvaged by drones"
				});
				mission.steps.Add(missionStep);
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.gold,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Drones skill test"));
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0003AE94 File Offset: 0x00039094
		private void SkilltreeMissionDefense()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionDefense", delegate(GamePlayer ply)
			{
				MapPointOfInterest current = MapPointOfInterest.current;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SkilltreeMissionDefense", Array.Empty<object>());
				mission.description = Translation.Translate("@SkilltreeMissionDefenseDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SkilltreeMissionDefenseComplete", Array.Empty<object>());
				mission.sourcePoi = current;
				mission.turnIn = current;
				mission.sourceFaction = Faction.policeGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				MapPointOfInterest mapPointOfInterest = current.system.SetupPOI(new Combat(), null, null, 0) as MapPointOfInterest;
				mapPointOfInterest.faction = Faction.tradingGuild;
				List<AbstractUnitData> list = mapPointOfInterest.CreateFixedPayload("Vectura", 1, null, null, UnitRank.Rookie);
				mapPointOfInterest.AddGuards(list, null);
				list[0].LoadDefaultEquipment(current.level, -1f, null, null, null, null, false, null);
				list[0].ApplyRandomBattleDamage(8, 20);
				list[0].autoActions = "DamagedShip";
				list[0].GetEquippedItem(EquipmentSlot.Hull).GetComponent<AbstractEquipment>().stats.Add(new EquipStatLine(EquipStat.ShieldRechargeRate, -999f, 1f, true));
				list[0].currentShieldHP = 102f;
				mapPointOfInterest.AddGuards(mapPointOfInterest.CreateFixedPayload("PirateDrone", 1, Faction.marauders, null, UnitRank.Rookie), null);
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = 1,
					trigger = MissionTrigger.FriendlyRepaired,
					description = "Restore the crippled ship's shields"
				});
				missionStep.objectives.Add(new CreditOffer
				{
					requiredAmount = 5000,
					deliverTo = (current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Skilltree
				{
					treeName = "Defense"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.policeGuild,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Defense"), "Unlock Defense skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionDefense2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionDefense2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionDefense2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionDefense2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionComplete" + (flag ? "2" : "1"), Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.policeGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = (flag ? 200000 : 100000),
					trigger = MissionTrigger.TakeDamage,
					description = "Damage taken"
				});
				mission.steps.Add(missionStep);
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.policeGuild,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Defense skill test"));
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0003AF28 File Offset: 0x00039128
		private void SkilltreeMissionEconomy()
		{
			StoryMission.Add(new StoryMission("SkilltreeMissionEconomy", delegate(GamePlayer ply)
			{
				MapPointOfInterest current = MapPointOfInterest.current;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SkilltreeMissionEconomy", Array.Empty<object>());
				mission.description = Translation.Translate("@SkilltreeMissionEconomyDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SkilltreeMissionEconomyComplete", Array.Empty<object>());
				mission.sourcePoi = current;
				mission.turnIn = current;
				mission.sourceFaction = Faction.tradingGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				MissionStep missionStep = new MissionStep
				{
					system = current.system
				};
				missionStep.objectives.Add(new CollectCredits
				{
					requiredAmount = 1000000
				});
				missionStep.objectives.Add(new CreditOffer
				{
					requiredAmount = 5000,
					deliverTo = (current as SpaceStation)
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Skilltree
				{
					treeName = "Economy"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.tradingGuild,
					amount = 300
				});
				return mission;
			}, (GamePlayer player) => !player.commander.HasSkilltree("Economy"), "Unlock Economy skilltree"));
			StoryMission.Add(new StoryMission("SkilltreeMissionEconomy2", delegate(GamePlayer ply)
			{
				bool flag = GamePlayer.current.missionsArchive.Contains("SkilltreeMissionEconomy2");
				Mission mission = new Mission
				{
					name = Translation.Translate("@SkilltreeMissionEconomy2", Array.Empty<object>()),
					description = Translation.Translate("@SkilltreeMissionEconomy2Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@SkilltreeMissionComplete" + (flag ? "2" : "1"), Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = MapPointOfInterest.current,
					sourceFaction = Faction.tradingGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					canBeIdled = false
				};
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = (flag ? 10000000 : 5000000),
					trigger = MissionTrigger.TradeTerminalProfit,
					description = "Trade Terminal profit"
				});
				mission.steps.Add(missionStep);
				if (flag)
				{
					mission.rewards.Add(new Credits
					{
						amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
					});
				}
				else
				{
					mission.rewards.Add(new Skillpoint
					{
						amount = 1
					});
				}
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.tradingGuild,
					amount = (flag ? 800 : 500)
				});
				return mission;
			}, null, "Economy skill test"));
		}
	}
}
