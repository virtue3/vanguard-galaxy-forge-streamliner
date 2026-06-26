using System;
using Behaviour.Item;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A6 RID: 166
	public abstract class MissionObjective : IJsonSource
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060006B4 RID: 1716
		public abstract string statusText { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0003931C File Offset: 0x0003751C
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x00039324 File Offset: 0x00037524
		public virtual string customDescription { get; set; }

		// Token: 0x060006B7 RID: 1719 RVA: 0x0003932D File Offset: 0x0003752D
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0003932F File Offset: 0x0003752F
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00039337 File Offset: 0x00037537
		public virtual GameplayType gameplayType { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00039340 File Offset: 0x00037540
		public virtual string coreName
		{
			get
			{
				return "Core";
			}
		}

		// Token: 0x060006BB RID: 1723
		public abstract bool IsComplete();

		// Token: 0x060006BC RID: 1724 RVA: 0x00039347 File Offset: 0x00037547
		public virtual void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00039349 File Offset: 0x00037549
		public virtual void OnMissionTurnedIn()
		{
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0003934B File Offset: 0x0003754B
		public virtual void OnMissionStart()
		{
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0003934D File Offset: 0x0003754D
		public virtual MapPointOfInterest GetPoiForEcho()
		{
			return this.GetPoi();
		}

		// Token: 0x060006C0 RID: 1728
		public abstract MapPointOfInterest GetPoi();

		// Token: 0x060006C1 RID: 1729 RVA: 0x00039355 File Offset: 0x00037555
		public virtual bool LoadoutCanRetrieveItem(MapPointOfInterest poi)
		{
			return true;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00039358 File Offset: 0x00037558
		public virtual int ItemCountRequired(InventoryItemType item)
		{
			return 0;
		}

		// Token: 0x060006C3 RID: 1731
		protected abstract void DataToJson(JsonObject data);

		// Token: 0x060006C4 RID: 1732
		protected abstract void LoadFromJson(JsonObject data);

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0003935C File Offset: 0x0003755C
		public virtual MissionTrigger? triggeredBy
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00039374 File Offset: 0x00037574
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["type"] = base.GetType().Name;
			jsonObject["missionType"] = this.gameplayType.ToString();
			if (this.targetLayer != null)
			{
				jsonObject["targetLayer"] = this.targetLayer.ToString();
			}
			this.DataToJson(jsonObject);
			return jsonObject;
		}

		// Token: 0x060006C7 RID: 1735
		public abstract Sprite GetIcon();

		// Token: 0x060006C8 RID: 1736 RVA: 0x00039404 File Offset: 0x00037604
		public static MissionObjective FromJson(JsonValue data)
		{
			MissionObjective missionObjective = MissionObjective.Create(data["type"]);
			if (!data["missionType"].IsNull)
			{
				missionObjective.gameplayType = Enum.Parse<GameplayType>(data["missionType"]);
			}
			if (data["targetLayer"].IsString)
			{
				missionObjective.targetLayer = new TargetLayer?(Enum.Parse<TargetLayer>(data["targetLayer"]));
			}
			missionObjective.LoadFromJson(data);
			return missionObjective;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0003949E File Offset: 0x0003769E
		private static MissionObjective Create(string name)
		{
			return (MissionObjective)Type.GetType("Source.MissionSystem.Objectives." + name).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000394CC File Offset: 0x000376CC
		public static void Trigger(MissionTrigger trigger, object data = null, MapPointOfInterest overridePOI = null, bool ignorePOI = false)
		{
			if (GamePlayer.current == null)
			{
				return;
			}
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				MissionStep currentStep = mission.currentStep;
				if (currentStep != null && (ignorePOI || !currentStep.hasPointOfInterest || currentStep.IsPointOfInterest(MapPointOfInterest.current)))
				{
					foreach (MissionObjective missionObjective in currentStep.objectives)
					{
						MissionTrigger? triggeredBy = missionObjective.triggeredBy;
						if (triggeredBy.GetValueOrDefault() == trigger & triggeredBy != null)
						{
							missionObjective.ProcessMissionTrigger(trigger, data);
						}
					}
				}
			}
			if (trigger == MissionTrigger.MinedOre)
			{
				SteamStatsManager.Add(SteamStatType.OreMined, 1);
				return;
			}
			if (trigger == MissionTrigger.SalvagedItem)
			{
				SteamStatsManager.Add(SteamStatType.SalvageCollected, 1);
			}
		}

		// Token: 0x040003C1 RID: 961
		public TargetLayer? targetLayer;
	}
}
