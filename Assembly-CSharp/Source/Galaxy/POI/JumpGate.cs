using System;
using System.Collections.Generic;
using Behaviour.GalaxyMap;
using Behaviour.Managers;
using Behaviour.Travel;
using Behaviour.UI;
using Behaviour.Util;
using LightJson;
using Source.Data.Persistable;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI
{
	// Token: 0x02000157 RID: 343
	public class JumpGate : MapPointOfInterest
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0005D7A5 File Offset: 0x0005B9A5
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("JumpGate");
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0005D7B1 File Offset: 0x0005B9B1
		public override string sceneName
		{
			get
			{
				return "JumpGate";
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0005D7B8 File Offset: 0x0005B9B8
		public override bool storeLastX
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0005D7BC File Offset: 0x0005B9BC
		public bool sectorLine
		{
			get
			{
				JumpGate jumpGate = this.GetTargetPOI() as JumpGate;
				return jumpGate != null && this.sectorJumpgate && jumpGate.sectorJumpgate;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0005D7EA File Offset: 0x0005B9EA
		public SystemMapData targetSystem
		{
			get
			{
				return GalaxyMapData.current.GetSystem(this.targetSystemGuid);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0005D7FC File Offset: 0x0005B9FC
		public bool canUseJumpGate
		{
			get
			{
				return this.jumpgateOpen && !this.hidden;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0005D811 File Offset: 0x0005BA11
		public override string typeName
		{
			get
			{
				if (!this.sectorJumpgate)
				{
					return base.typeName;
				}
				return "@MapPOISectorJumpGate";
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0005D827 File Offset: 0x0005BA27
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x0005D834 File Offset: 0x0005BA34
		public override Faction faction
		{
			get
			{
				return this.system.faction;
			}
			set
			{
				base.faction = value;
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0005D840 File Offset: 0x0005BA40
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			string text = Translation.Translate("@Locked", Array.Empty<object>());
			Color color = ColorHelper.reddish;
			if (this.jumpgateOpen)
			{
				text = Translation.Translate("@Open", Array.Empty<object>());
				color = ColorHelper.greenish;
			}
			tooltip.AddTextLine(Translation.Translate("@GateStatus", Array.Empty<object>()) + ": " + text.HighlightWithColor(color), 12, 8f);
			SystemMapData targetSystem = this.targetSystem;
			if (targetSystem != null)
			{
				tooltip.AddTextLine(Translation.Translate("@GateToLevel", new object[]
				{
					targetSystem.name,
					targetSystem.level
				}), 12, 8f);
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0005D8EE File Offset: 0x0005BAEE
		public void LockGate()
		{
			this.jumpgateOpen = false;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0005D8F8 File Offset: 0x0005BAF8
		public void UnlockJumpgate()
		{
			this.jumpgateOpen = true;
			if (this.targetSystemGuid == null)
			{
				return;
			}
			string name = GalaxyMapData.current.GetSystem(this.targetSystemGuid).name;
			if (name == "Balam")
			{
				MissionObjective.Trigger(MissionTrigger.UnlockJumpgateBalam, null, null, false);
			}
			else if (name == "Orbitan")
			{
				MissionObjective.Trigger(MissionTrigger.UnlockJumpgateOrbitan, null, null, false);
			}
			JumpGate jumpGate = this.GetTargetPOI() as JumpGate;
			if (jumpGate != null)
			{
				jumpGate.jumpgateOpen = true;
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0005D974 File Offset: 0x0005BB74
		public override Vector2 GetLocalOffset()
		{
			int num = 1;
			if (Singleton<TravelManager>.Instance.fastLaneTravelActive)
			{
				num = -1;
			}
			if (this.GetTravelDirection() != TravelDirection.Right)
			{
				return new Vector2((float)(10 * num), 0f);
			}
			return new Vector2((float)(-10 * num), 0f);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0005D9B9 File Offset: 0x0005BBB9
		public MapPointOfInterest GetTargetPOI()
		{
			return GalaxyMapData.current.GetSystem(this.targetSystemGuid).GetPoiWithId(this.targetPoiGuid);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0005D9D8 File Offset: 0x0005BBD8
		public TravelDirection GetTravelDirection()
		{
			SystemMapData system = GalaxyMapData.current.GetSystem(this.targetSystemGuid);
			if (system == null)
			{
				return TravelDirection.Left;
			}
			float x;
			float x2;
			if (this.system.sector != system.sector)
			{
				x = this.system.sectorPosition.x;
				x2 = system.sectorPosition.x;
			}
			else
			{
				x = this.system.position.x;
				x2 = system.position.x;
			}
			if (x >= x2)
			{
				return TravelDirection.Left;
			}
			return TravelDirection.Right;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0005DA51 File Offset: 0x0005BC51
		public void AddWindowDressing()
		{
			this.initializingPersistables = true;
			JumpGate.SetupJumpGateArea(this);
			this.initializingPersistables = false;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0005DA67 File Offset: 0x0005BC67
		public void CleanupWindowDressing()
		{
			this.lastVisitedTime = 0f;
			this.payloads.Clear();
			if (GamePlayer.current.IsInSandBox())
			{
				this.units.Clear();
			}
			this.persistables.Clear();
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0005DAA4 File Offset: 0x0005BCA4
		public override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["targetSystem"] = this.targetSystemGuid;
			data["targetPoi"] = this.targetPoiGuid;
			data["sectorJumpgate"] = new bool?(this.sectorJumpgate);
			data["jumpgateOpen"] = new bool?(this.jumpgateOpen);
			if (!this.hasGate)
			{
				data["hasGate"] = new bool?(false);
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0005DB38 File Offset: 0x0005BD38
		public override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data);
			this.targetSystemGuid = data["targetSystem"];
			this.targetPoiGuid = data["targetPoi"];
			this.sectorJumpgate = data["sectorJumpgate"];
			this.jumpgateOpen = data["jumpgateOpen"];
			if (data["hasGate"].IsBoolean)
			{
				this.hasGate = data["hasGate"];
			}
			if (base.name.EndsWith(')'))
			{
				base.name = base.name.Substring(0, base.name.LastIndexOf('(') - 1);
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005DC00 File Offset: 0x0005BE00
		private static void SetupJumpGateArea(JumpGate jumpGate)
		{
			SeededRandom global = SeededRandom.Global;
			jumpGate.GetWorldPosition();
			bool flag = jumpGate.GetTravelDirection() == TravelDirection.Left;
			Vector2[] list = new Vector2[]
			{
				new Vector2((float)(flag ? 6 : -6), 6f),
				new Vector2((float)(flag ? 5 : -5), -6f)
			};
			if (GamePlayer.current.IsInSandBox() && !jumpGate.system.pocketSystem && global.RandomBool(0.85f))
			{
				Faction faction = (Faction.policeGuild.IsEnemy(jumpGate.faction) || jumpGate.system.storyteller is ConquestSystem) ? jumpGate.faction : Faction.policeGuild;
				CombatStationFactory.CreateGunPlatform(jumpGate, faction, new Vector2?(global.Choose<Vector2>(list)), (float)global.RandomRange(-45, 45), 0);
			}
			if (global.RandomBool(0.45f))
			{
				AsteroidFieldOreSet surface = AsteroidFieldData.CreateOreSet(jumpGate.level, true);
				AsteroidFieldOreSet core = AsteroidFieldData.CreateOreSet(jumpGate.level, false);
				int amount = global.RandomRange(5, 8);
				jumpGate.SetAsteroidFieldData(new AsteroidFieldData(amount, 0.5f, 1f, surface, core, -1f), 0);
				jumpGate.asteroidsInitialized = false;
			}
			jumpGate.AddCargoContainers(new Vector2(50f, 16f), 1, 0.2f);
			if (global.RandomBool(0.2f))
			{
				list = new Vector2[]
				{
					new Vector2((float)global.RandomRange(-40, -15), (float)global.RandomRange(-10, 10)),
					new Vector2((float)global.RandomRange(15, 40), (float)global.RandomRange(-10, 10))
				};
				List<Faction> list2 = new List<Faction>
				{
					Faction.red,
					Faction.blue,
					Faction.gold,
					Faction.miningGuild,
					Faction.tradingGuild,
					Faction.marauders
				};
				SalvageData salvageData = new SalvageData
				{
					position = jumpGate.GetWorldPosition() + global.Choose<Vector2>(list),
					shipTemplate = jumpGate.FindSalvageShipTemplate(global.Choose<Faction>(list2))
				};
				salvageData.AddItemContent(jumpGate.level, -1, 1f);
				salvageData.AddScrapContent(jumpGate.level, 1f, 2);
				salvageData.AddStructuralContent(jumpGate.level, 2, 1f);
				jumpGate.AddPersistable(salvageData);
			}
		}

		// Token: 0x0400071A RID: 1818
		public string targetSystemGuid;

		// Token: 0x0400071B RID: 1819
		public string targetPoiGuid;

		// Token: 0x0400071C RID: 1820
		public bool sectorJumpgate;

		// Token: 0x0400071D RID: 1821
		public bool hasGate = true;

		// Token: 0x0400071E RID: 1822
		protected bool jumpgateOpen;
	}
}
