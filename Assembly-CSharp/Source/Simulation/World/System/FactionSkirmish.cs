using System;
using System.Collections.Generic;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.World.System
{
	// Token: 0x0200007A RID: 122
	public class FactionSkirmish : SystemStoryteller
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x00024B4A File Offset: 0x00022D4A
		public FactionSkirmish(SystemMapData system) : base(system)
		{
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00024B68 File Offset: 0x00022D68
		public override void SetupSystem()
		{
			SeededRandom random = SeededRandom.Global;
			JumpGate entranceJumpgate = this.system.GetEntranceJumpgate();
			SystemMapData system = entranceJumpgate.system;
			this.system.pocketSystem = true;
			this.system.name = SandboxWorld.EnsureUniqueName(() => Translation.Translate("@PocketSystemSkirmish", new object[]
			{
				random.Choose<string>(FactionSkirmish.systemNames)
			}));
			List<Faction> list = new List<Faction>(Faction.corporations)
			{
				Faction.policeGuild
			};
			if (this.factionA == null)
			{
				this.factionA = (list.Contains(system.faction) ? system.faction : random.Choose<Faction>(list));
			}
			this.system.faction = this.factionA;
			if (this.factionA == Faction.policeGuild)
			{
				this.factionB = Faction.marauders;
			}
			else
			{
				List<Faction> list2 = new List<Faction>(Faction.corporations)
				{
					Faction.salvageGuild,
					Faction.marauders
				};
				list2.Remove(this.factionA);
				if (this.factionB == null)
				{
					this.factionB = random.Choose<Faction>(list2);
				}
			}
			this.CreateMediumSpaceStation(this.factionA);
			this.CreateMediumSpaceStation(this.factionB);
			for (int i = 0; i < 3; i++)
			{
				this.CreateNewCombat();
			}
			foreach (SystemMapData systemMapData in this.system.sector.allSystems)
			{
				if (systemMapData.jumpgateOpen)
				{
					foreach (MapPointOfInterest mapPointOfInterest in systemMapData.allPointsOfInterest)
					{
						if (mapPointOfInterest.faction == this.factionA || mapPointOfInterest.faction == this.factionB)
						{
							SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
							if (spaceStation != null)
							{
								spaceStation.shopInventory.AddPermanentItem(ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(entranceJumpgate, null), 1);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00024D88 File Offset: 0x00022F88
		public void CreateNewCombat()
		{
			Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
			this.system.SetupPOI(combat, null, null, 0);
			this.system.pointsOfInterest.Add(combat);
			this.AddSkirmishToPoi(combat, true, true);
			List<Source.Galaxy.POI.Combat> list = this.activeCombats;
			if (list == null)
			{
				return;
			}
			list.Add(combat);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00024DE0 File Offset: 0x00022FE0
		private void AddSkirmishToPoi(MapPointOfInterest poi, bool doA = true, bool doB = true)
		{
			Vector2 worldPosition = poi.GetWorldPosition();
			float pointsScale = 1.5f;
			Faction f = this.factionA;
			List<AbstractUnitData> list = poi.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), f, 0, 0, 1, 5, null);
			float pointsScale2 = 1.5f;
			f = this.factionB;
			List<AbstractUnitData> list2 = poi.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), f, 0, 0, 1, 5, null);
			if (doA)
			{
				float num = -5f;
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.positionData.position = worldPosition + new Vector2(-10f, num);
					num += 5f;
					if (this.helpingFaction == this.factionB)
					{
						abstractUnitData.alwaysHostile = true;
					}
					if (!Faction.player.IsEnemy(this.factionA))
					{
						abstractUnitData.noReputationLoss = true;
					}
					if (this.helpingFaction != this.factionA || SeededRandom.Global.RandomBool(0.5f))
					{
						poi.AddUnit(abstractUnitData, null, false);
					}
				}
			}
			if (doB)
			{
				float num2 = -5f;
				foreach (AbstractUnitData abstractUnitData2 in list2)
				{
					abstractUnitData2.positionData.position = worldPosition + new Vector2(10f, num2);
					num2 += 5f;
					if (this.helpingFaction == this.factionA)
					{
						abstractUnitData2.alwaysHostile = true;
					}
					if (!Faction.player.IsEnemy(this.factionB))
					{
						abstractUnitData2.noReputationLoss = true;
					}
					if (this.helpingFaction != this.factionB || SeededRandom.Global.RandomBool(0.5f))
					{
						poi.AddUnit(abstractUnitData2, null, false);
					}
				}
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00024FE4 File Offset: 0x000231E4
		public void CreateMediumSpaceStation(Faction f)
		{
			Source.Galaxy.POI.Combat combat = this.system.AddCombatStation(f, null, true, 0);
			combat.name = f.GenerateStationName(combat);
			CombatStationData combatStationData = CombatStationFactory.CreateMediumStation(combat, null);
			combatStationData.AddLoot(this.CreateLoot((EquipmentBuilder b) => b.slot != EquipmentSlot.Hardpoint && b.slot != EquipmentSlot.Booster), 1);
			combatStationData.AddLoot(this.CreateLoot((EquipmentBuilder b) => b.slot == EquipmentSlot.Hardpoint), 1);
			combatStationData.GetPart(CombatStationPartType.Core).AddLoot("BonusSkillPointTemplate", 1);
			combat.AddGuards(combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), f, 0, 0, 1, 5, null), null);
			combat.AddTriggeredSpawn(combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), (this.factionA == f) ? this.factionB : this.factionA, 0, 0, 1, 5, null), 0.5f, 0, false, true);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00025100 File Offset: 0x00023300
		public override void UpdateActive(float deltaTime)
		{
			this.combatTimer -= Time.deltaTime;
			if (this.combatTimer < 0f)
			{
				this.combatTimer = 0.5f;
				if (this.activeCombats == null)
				{
					this.activeCombats = new List<Source.Galaxy.POI.Combat>();
					foreach (Source.Galaxy.POI.Combat combat in this.system.GetPointsOfInterest<Source.Galaxy.POI.Combat>())
					{
						if (!(combat is CombatStation))
						{
							this.activeCombats.Add(combat);
						}
					}
				}
				for (int i = 0; i < this.activeCombats.Count; i++)
				{
					bool flag = false;
					using (IEnumerator<AbstractUnitData> enumerator2 = this.activeCombats[i].GetUnits(false).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.faction != this.helpingFaction)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						this.activeCombats.RemoveAt(i);
						i--;
						if (this.enemyStrength > 0)
						{
							this.enemyStrength--;
							Faction faction = (this.factionA == this.helpingFaction) ? this.factionB : this.factionA;
							Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@FactionSkirmishReduced", new object[]
							{
								faction.name
							})).WithColor(ColorHelper.greenish).Show();
						}
					}
				}
				this.CheckForReinforcements();
			}
			if (this.helpingFaction == null && this.queryTimer > 0f)
			{
				this.queryTimer -= deltaTime;
				if (this.queryTimer <= 0f)
				{
					if (this.factionA.IsEnemy(Faction.player))
					{
						this.UpdateHelpingFaction(this.factionB);
						return;
					}
					if (this.factionB.IsEnemy(Faction.player))
					{
						this.UpdateHelpingFaction(this.factionA);
						return;
					}
					AlertPopup.ShowQuery("@FactionSkirmishPopup", this.factionA.name, this.factionB.name, delegate
					{
						this.UpdateHelpingFaction(this.factionA);
					}, delegate
					{
						this.UpdateHelpingFaction(this.factionB);
					}, null, null);
				}
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00025344 File Offset: 0x00023544
		private void CheckForReinforcements()
		{
			if (this.enemyStrength <= 0)
			{
				return;
			}
			MapPointOfInterest current = MapPointOfInterest.current;
			if (current is CombatStation && current.faction != this.helpingFaction)
			{
				using (IEnumerator<MapTriggeredPayload> enumerator = current.GetTriggeredPayloads().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						MapTriggeredPayload mapTriggeredPayload = enumerator.Current;
						return;
					}
				}
				int num = 0;
				using (IEnumerator<AbstractUnitData> enumerator2 = current.GetUnits(false).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.faction != this.helpingFaction)
						{
							num++;
						}
					}
				}
				int num2 = Mathf.Max(3, this.enemyStrength + 1);
				if (num < num2)
				{
					List<AbstractUnitData> list = current.CreateUnitPayload(0.5f + (float)this.enemyStrength * 0.3f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), current.faction, 0, 0, 1, 5, null);
					foreach (AbstractUnitData abstractUnitData in list)
					{
						if (!abstractUnitData.IsPlayerEnemy())
						{
							abstractUnitData.noReputationLoss = true;
						}
						abstractUnitData.alwaysHostile = true;
					}
					current.AddTriggeredSpawn(list, (float)(20 - this.enemyStrength * 3), 0, false, true);
					if (SeededRandom.Global.RandomBool(0.3f))
					{
						this.enemyStrength--;
					}
				}
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000254DC File Offset: 0x000236DC
		public override void UpdateAmbient(float deltaTime)
		{
			int num = 0;
			foreach (MapPointOfInterest mapPointOfInterest in this.system.allPointsOfInterest)
			{
				if (mapPointOfInterest is Source.Galaxy.POI.Combat && !(mapPointOfInterest is CombatStation))
				{
					using (IEnumerator<AbstractUnitData> enumerator2 = mapPointOfInterest.GetUnits(false).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.faction != this.helpingFaction)
							{
								num++;
								break;
							}
						}
					}
				}
			}
			if (num < 3)
			{
				this.CreateNewCombat();
			}
			foreach (CombatStation combatStation in this.system.GetPointsOfInterest<CombatStation>())
			{
				if (MapPointOfInterest.current != combatStation)
				{
					List<AbstractUnitData> list = new List<AbstractUnitData>();
					using (IEnumerator<AbstractUnitData> enumerator2 = combatStation.GetUnits(false).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							AbstractUnitData abstractUnitData = enumerator2.Current;
							if (abstractUnitData.faction != this.helpingFaction)
							{
								list.Add(abstractUnitData);
							}
						}
						goto IL_11B;
					}
					goto IL_FA;
					IL_11B:
					if (list.Count <= 5)
					{
						continue;
					}
					IL_FA:
					AbstractUnitData abstractUnitData2 = SeededRandom.Global.Choose<AbstractUnitData>(list);
					list.Remove(abstractUnitData2);
					combatStation.RemoveUnit(abstractUnitData2);
					goto IL_11B;
				}
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0002565C File Offset: 0x0002385C
		public void UpdateHelpingFaction(Faction f)
		{
			this.helpingFaction = f;
			foreach (MapPointOfInterest mapPointOfInterest in this.system.allPointsOfInterest)
			{
				List<AbstractUnitData> list = new List<AbstractUnitData>();
				foreach (AbstractUnitData abstractUnitData in mapPointOfInterest.GetUnits(false))
				{
					if (!abstractUnitData.IsPlayerEnemy())
					{
						abstractUnitData.noReputationLoss = true;
					}
					abstractUnitData.alwaysHostile = (abstractUnitData.faction != this.helpingFaction);
					if (mapPointOfInterest is Source.Galaxy.POI.Combat && abstractUnitData.faction == this.helpingFaction && SeededRandom.Global.RandomBool(0.5f))
					{
						list.Add(abstractUnitData);
					}
				}
				foreach (MapTriggeredPayload mapTriggeredPayload in mapPointOfInterest.GetTriggeredPayloads())
				{
					foreach (AbstractUnitData abstractUnitData2 in mapTriggeredPayload.units)
					{
						if (!abstractUnitData2.IsPlayerEnemy())
						{
							abstractUnitData2.noReputationLoss = true;
						}
						abstractUnitData2.alwaysHostile = (abstractUnitData2.faction != this.helpingFaction);
					}
				}
				foreach (AbstractUnitData unit in list)
				{
					mapPointOfInterest.RemoveUnit(unit);
				}
				foreach (PersistableData persistableData in mapPointOfInterest.GetPersistables())
				{
					CombatStationData combatStationData = persistableData as CombatStationData;
					if (combatStationData != null)
					{
						foreach (CombatStationPartData combatStationPartData in combatStationData.stationParts)
						{
							if (!combatStationPartData.IsPlayerEnemy())
							{
								combatStationPartData.noReputationLoss = true;
							}
							combatStationPartData.alwaysHostile = (combatStationPartData.faction != this.helpingFaction);
						}
					}
				}
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00025928 File Offset: 0x00023B28
		private InventoryItemType CreateLoot(EquipmentBuilder.BuilderFilter filter)
		{
			Rarity rarity = Rarity.HighGrade;
			if (this.system.level > 20 && SeededRandom.Global.RandomBool(0.05f))
			{
				rarity = Rarity.Exotic;
			}
			else if (this.system.level > 30 && SeededRandom.Global.RandomBool(0.1f))
			{
				rarity = Rarity.Exotic;
			}
			return EquipmentBuilder.GetRandom(filter, this.system.level, true, null).CreateItemType(rarity, this.system.level, false, null, false, false);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000259A8 File Offset: 0x00023BA8
		public override void DataFromJson(JsonObject data)
		{
			this.factionA = Faction.Get(data["factionA"]);
			this.factionB = Faction.Get(data["factionB"]);
			if (data["enemyStrength"].IsInteger)
			{
				this.enemyStrength = data["enemyStrength"];
			}
			if (!data["helpingFaction"].IsNull)
			{
				this.helpingFaction = Faction.Get(data["helpingFaction"]);
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00025A48 File Offset: 0x00023C48
		public override void DataToJson(JsonObject data)
		{
			data["factionA"] = this.factionA.identifier;
			data["factionB"] = this.factionB.identifier;
			data["enemyStrength"] = new double?((double)this.enemyStrength);
			if (this.helpingFaction != null)
			{
				data["helpingFaction"] = this.helpingFaction.identifier;
			}
		}

		// Token: 0x0400026D RID: 621
		private static string[] systemNames = new string[]
		{
			"Alpha",
			"Delta",
			"Omicron",
			"Theta",
			"O-681",
			"B-4",
			"Mercury",
			"Meridian",
			"Exodus",
			"Eternia",
			"Crimson",
			"Iota",
			"Lambda",
			"Zeta",
			"Psi",
			"K-7",
			"V-12",
			"R-9",
			"Bastion",
			"Crucible",
			"Pyrrhus"
		};

		// Token: 0x0400026E RID: 622
		public Faction factionA;

		// Token: 0x0400026F RID: 623
		public Faction factionB;

		// Token: 0x04000270 RID: 624
		public Faction helpingFaction;

		// Token: 0x04000271 RID: 625
		public int enemyStrength = 5;

		// Token: 0x04000272 RID: 626
		private float combatTimer;

		// Token: 0x04000273 RID: 627
		private float queryTimer = 5f;

		// Token: 0x04000274 RID: 628
		private List<Source.Galaxy.POI.Combat> activeCombats;
	}
}
