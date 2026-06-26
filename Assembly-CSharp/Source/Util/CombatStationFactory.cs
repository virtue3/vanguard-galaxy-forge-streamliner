using System;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200002A RID: 42
	public static class CombatStationFactory
	{
		// Token: 0x0600022B RID: 555 RVA: 0x0000E3E6 File Offset: 0x0000C5E6
		public static string GetFactionPrefix(this Faction faction)
		{
			if (faction == Faction.gold)
			{
				return "Luminate";
			}
			if (faction == Faction.red)
			{
				return "Generic";
			}
			if (faction == Faction.blue)
			{
				return "Stellar";
			}
			if (faction == Faction.marauders)
			{
				return "Corsair";
			}
			return "Generic";
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E428 File Offset: 0x0000C628
		public static CombatStationData CreateStationVariant(string seed, MapPointOfInterest poi)
		{
			char c = seed[seed.Length - 1];
			if (seed.StartsWith("Small"))
			{
				int num;
				if (!int.TryParse(c.ToString(), out num))
				{
					Debug.LogWarning(string.Format("Seed does not end with a number: {0}, last char: {1}", seed, c));
					return CombatStationFactory.CreateSmallConquestStation1(poi);
				}
				if (num > 3)
				{
					return CombatStationFactory.CreateSmallConquestStation2(poi);
				}
				return CombatStationFactory.CreateSmallConquestStation1(poi);
			}
			else
			{
				if (!seed.StartsWith("Medium"))
				{
					seed.StartsWith("Large");
					return CombatStationFactory.CreateLargeConquestStation1(poi);
				}
				int num2;
				if (!int.TryParse(c.ToString(), out num2))
				{
					Debug.LogWarning(string.Format("Seed does not end with a number: {0}, last char: {1}", seed, c));
					return CombatStationFactory.CreateMediumConquestStation2(poi);
				}
				if (num2 > 3)
				{
					return CombatStationFactory.CreateMediumConquestStation2(poi);
				}
				return CombatStationFactory.CreateMediumConquestStation1(poi);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		public static CombatStationData CreateUmbralOutpost(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			bool flag = poi.level >= 45;
			SeededRandom global = SeededRandom.Global;
			string text = "S2";
			string text2 = "M2";
			string text3 = "L1";
			string str = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Refinery1_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 1, 5, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		public static CombatStationData CreateMediumStation(MapPointOfInterest poi, Vector2? position = null)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + (position ?? new Vector2(15f, 0f));
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 2, 0);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 2, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 1);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_1C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(4, 2, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 3, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 7, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(7, 1, 8, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_3C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(7, 0, 9, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(9, 1, 10, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(9, 2, 11, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(11, 1, 12, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(12, 1, 13, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E9EC File Offset: 0x0000CBEC
		public static CombatStationData CreateLargeConquestStation1(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			bool flag = poi.level >= 45;
			SeededRandom global = SeededRandom.Global;
			string text = "S2";
			string text2 = "M2";
			string text3 = "L1";
			string text4 = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 3);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 1, 5, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 2, 7, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 2, 8, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 1, 9, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(6, 1, 10, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(10, 1, 11, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(11, 1, 12, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 0, 13, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(10, 0, 14, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(13, 1, 15, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(14, 1, 16, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(15, 1, 17, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(15, 2, 18, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 1, 19, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 3, 20, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(15, 3, 21, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 2, 22, 1);
			combatStationData.ConnectParts(21, 1, 22, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(21, 0, 23, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(23, 0, 24, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(24, 0, 25, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(25, 0, 26, 1);
			string str = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(26, 0, 27, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(22, 0, 28, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(28, 0, 29, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(29, 1, 30, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(30, 1, 31, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(31, 1, 32, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(7, 2, 33, 3);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(7, 3, 34, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(33, 0, 35, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(33, 1, 36, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(33, 2, 37, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(34, 0, 38, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(34, 3, 39, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(7, 0, 40, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(40, 2, 41, 1);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(41, 0, 42, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(40, 1, 43, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(43, 1, 44, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000F44C File Offset: 0x0000D64C
		public static CombatStationData CreateSmallConquestStation1(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			bool flag = poi.level >= 45;
			SeededRandom global = SeededRandom.Global;
			string text = "S2";
			string text2 = "M2";
			string text3 = "L1";
			string str = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Refinery1_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 5, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(3, 1, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 0, 7, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 3, 8, 2);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 1, 9, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 3, 10, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 0, 11, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 2, 12, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(12, 1, 13, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(13, 0, 14, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 1, 15, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(15, 0, 16, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 1, 17, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(17, 1, 18, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000F904 File Offset: 0x0000DB04
		public static CombatStationData CreateSmallConquestStation2(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			bool flag = poi.level >= 45;
			SeededRandom global = SeededRandom.Global;
			string text = "S2";
			string text2 = "M2";
			string text3 = "L1";
			string text4 = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 1, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 2, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 3, 7, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 1, 8, 3);
			string str = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 1, 9, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 0, 10, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 2, 11, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 12, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(12, 1, 13, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 1, 14, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(14, 1, 15, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 2, 16, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 1, 17, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000FD80 File Offset: 0x0000DF80
		public static CombatStationData CreateMediumConquestStation1(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			bool flag = poi.level >= 45;
			SeededRandom global = SeededRandom.Global;
			string text = "S2";
			string text2 = "M2";
			string text3 = "L1";
			string str = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 2, 3);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 1, 4, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 1, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 2, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 3, 7, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 1, 8, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 2, 9, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 3, 10, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 1, 11, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 1, 12, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 2, 13, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(13, 1, 14, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(14, 1, 15, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 0, 16, 1);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 2, 17, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 3, 18, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 0, 19, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 20, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(20, 0, 21, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(21, 0, 22, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00010354 File Offset: 0x0000E554
		public static CombatStationData CreateMediumConquestStation2(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			bool flag = poi.level >= 45;
			SeededRandom global = SeededRandom.Global;
			string text = "S2";
			string text2 = "M2";
			string text3 = "L1";
			string text4 = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(0f, 0f);
			combatStationData.AddPart(combatStationPartData);
			string str = (!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 3, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 4, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 1, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 3, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 1, 7, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 1, 8, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_CargoDock_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(3, 0, 9, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 2, 10, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 3, 11, 1);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(8, 1, 12, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingTunnel_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(9, 2, 13, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingMedium_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(9, 3, 14, 0);
			str = ((!flag) ? text : (global.RandomBool(0.5f) ? text2 : text3));
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurret" + str + "_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(9, 0, 15, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS2_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(14, 1, 16, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS2_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(11, 0, 17, 2);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 2, 18, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(17, 1, 19, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(16, 0, 20, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(20, 1, 21, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(21, 1, 22, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(17, 0, 23, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Conq_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(23, 0, 24, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(24, 0, 25, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 26, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000109CC File Offset: 0x0000EBCC
		public static CombatStationData CreateLargeStation(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 2, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 2, 2);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 1, 3, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 0, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(3, 1, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 2, 6, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2CSlim")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(5, 1, 7, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(7, 0, 8, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 9, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(9, 1, 10, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2CSlim")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(9, 2, 11, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(11, 0, 12, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 0, 13, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 1, 14, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_1C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(2, 3, 15, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(13, 0, 16, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(14, 1, 17, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00010DA8 File Offset: 0x0000EFA8
		public static CombatStationData CreateEasyStation1(MapPointOfInterest poi, Vector2? pos = null)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + (pos ?? new Vector2(15f, 0f));
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 2, 0);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 2, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 0, 5, 1);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(5, 3, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 0, 7, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(5, 2, 8, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00010FCC File Offset: 0x0000F1CC
		public static CombatStationData CreateEasyStation2(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = poi.faction
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Energy1_4C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 3, 2, 0);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 2, 3, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(0, 0, 4, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(3, 0, 5, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(2, 1, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 3, 7, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 2, 8, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(4, 1, 9, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001120C File Offset: 0x0000F40C
		public static CombatStationData CreateEscortLocation(MapPointOfInterest poi)
		{
			string factionPrefix = poi.faction.GetFactionPrefix();
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_EscortPartRefinery1_2C")
			{
				faction = poi.faction,
				playerFriendly = true
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(105f, 0f);
			combatStationData.AddPart(combatStationPartData);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 1, 1, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_3C")
			{
				faction = poi.faction
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 0, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(1, 1, 3, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 1, 4, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = poi.faction
			});
			combatStationData.ConnectParts(2, 2, 5, 0);
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00011390 File Offset: 0x0000F590
		public static CombatStationData CreateGunPlatform(MapPointOfInterest poi, Faction faction = null, Vector2? position = null, float rotation = 0f, int variant = 0)
		{
			Faction faction2 = faction ?? (poi.faction ?? Faction.policeGuild);
			string factionPrefix = faction2.GetFactionPrefix();
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + (position ?? new Vector2(9f, 10f));
			float num = SeededRandom.Global.RandomFloat();
			if (variant == 1 || num > 0.666f)
			{
				CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2C")
				{
					faction = faction2
				};
				combatStationPartData.positionData.rotation = rotation;
				combatStationData.AddPart(combatStationPartData);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 0, 1, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 1, 2, 0);
			}
			else if (variant == 2 || num > 0.3333f)
			{
				CombatStationPartData combatStationPartData2 = new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
				{
					faction = faction2
				};
				combatStationPartData2.positionData.rotation = rotation;
				combatStationData.AddPart(combatStationPartData2);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 0, 1, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 1, 2, 0);
			}
			else
			{
				CombatStationPartData combatStationPartData3 = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_3C")
				{
					faction = faction2
				};
				combatStationPartData3.positionData.rotation = rotation;
				combatStationData.AddPart(combatStationPartData3);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 0, 1, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 1, 2, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
				{
					faction = faction2
				});
				combatStationData.ConnectParts(0, 2, 3, 0);
			}
			poi.AddPersistable(combatStationData);
			return combatStationData;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000115FF File Offset: 0x0000F7FF
		public static int GetFleetReinforcementReduction(this CombatStationPartType type)
		{
			switch (type)
			{
			case CombatStationPartType.Core:
				return 10;
			case CombatStationPartType.Connector:
				return 0;
			case CombatStationPartType.DockingPad:
				return 1;
			case CombatStationPartType.DefensePlatform:
				return 2;
			case CombatStationPartType.Energy:
				return 1;
			case CombatStationPartType.Refinery:
				return 1;
			case CombatStationPartType.CargoDock:
				return 1;
			case CombatStationPartType.DockingTunnel:
				return 1;
			default:
				return 1;
			}
		}
	}
}
