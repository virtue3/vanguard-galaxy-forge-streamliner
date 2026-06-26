using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Dialogues;
using Behaviour.Item;
using Behaviour.UI.Missions;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Simulation;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A3 RID: 163
	public class Mission : IJsonSource
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x0003778C File Offset: 0x0003598C
		public TargetLayer? targetLayer
		{
			get
			{
				MissionStep currentStep = this.currentStep;
				TargetLayer? targetLayer;
				if (currentStep == null)
				{
					targetLayer = null;
				}
				else
				{
					MissionObjective currentObjective = currentStep.currentObjective;
					targetLayer = ((currentObjective != null) ? currentObjective.targetLayer : null);
				}
				TargetLayer? result = targetLayer;
				if (result == null)
				{
					return null;
				}
				return result;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x000377DC File Offset: 0x000359DC
		public string coreName
		{
			get
			{
				MissionStep currentStep = this.currentStep;
				string text;
				if (currentStep == null)
				{
					text = null;
				}
				else
				{
					MissionObjective currentObjective = currentStep.currentObjective;
					text = ((currentObjective != null) ? currentObjective.coreName : null);
				}
				return text ?? "Core";
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x00037805 File Offset: 0x00035A05
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x0003780D File Offset: 0x00035A0D
		public List<MissionStep> steps { get; private set; } = new List<MissionStep>();

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00037816 File Offset: 0x00035A16
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0003781E File Offset: 0x00035A1E
		public List<MissionReward> rewards { get; private set; } = new List<MissionReward>();

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00037827 File Offset: 0x00035A27
		public bool isComplete
		{
			get
			{
				return this.currentStep == null;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00037834 File Offset: 0x00035A34
		public int level
		{
			get
			{
				if (this.dynamicLevel)
				{
					return GamePlayer.current.level;
				}
				int num = this.sourcePoi.level;
				foreach (MissionStep missionStep in this.steps)
				{
					foreach (MapPointOfInterest mapPointOfInterest in missionStep.pointsOfInterest)
					{
						num = Mathf.Max(num, mapPointOfInterest.level);
					}
				}
				return num;
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000378E0 File Offset: 0x00035AE0
		public bool CanBeIdled()
		{
			return this.canBeIdled && this.storyId == null;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x000378F8 File Offset: 0x00035AF8
		public GameplayType gameplayType
		{
			get
			{
				MissionStep currentStep = this.currentStep;
				GameplayType? gameplayType;
				if (currentStep == null)
				{
					gameplayType = null;
				}
				else
				{
					MissionObjective currentObjective = currentStep.currentObjective;
					gameplayType = ((currentObjective != null) ? new GameplayType?(currentObjective.gameplayType) : null);
				}
				GameplayType? gameplayType2 = gameplayType;
				return gameplayType2.GetValueOrDefault();
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00037940 File Offset: 0x00035B40
		public MissionStep currentStep
		{
			get
			{
				foreach (MissionStep missionStep in this.steps)
				{
					if (!missionStep.isComplete)
					{
						missionStep.hidden = false;
						return missionStep;
					}
				}
				return null;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x000379A4 File Offset: 0x00035BA4
		public MapPointOfInterest dynamicPointOfInterest
		{
			get
			{
				MissionStep currentStep = this.currentStep;
				if (currentStep != null && currentStep.dynamicPointOfInterest != null)
				{
					return currentStep.dynamicPointOfInterest;
				}
				for (int i = this.steps.Count - 1; i >= 0; i--)
				{
					if (this.steps[i].isComplete && this.steps[i].dynamicPointOfInterest != null)
					{
						return this.steps[i].dynamicPointOfInterest;
					}
				}
				return null;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00037A1C File Offset: 0x00035C1C
		public MissionStep mostRecentStep
		{
			get
			{
				MissionStep currentStep = this.currentStep;
				if (currentStep != null)
				{
					return currentStep;
				}
				for (int i = this.steps.Count - 1; i >= 0; i--)
				{
					if (this.steps[i].isComplete)
					{
						return this.steps[i];
					}
				}
				return null;
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00037A70 File Offset: 0x00035C70
		public MapPointOfInterest GetActivePoi(bool forEcho = false)
		{
			if (this.failed)
			{
				return null;
			}
			if (forEcho)
			{
				MissionStep currentStep = this.currentStep;
				MapPointOfInterest mapPointOfInterest;
				if (currentStep == null)
				{
					mapPointOfInterest = null;
				}
				else
				{
					MissionObjective currentObjective = currentStep.currentObjective;
					mapPointOfInterest = ((currentObjective != null) ? currentObjective.GetPoiForEcho() : null);
				}
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				if (mapPointOfInterest2 != null)
				{
					return mapPointOfInterest2;
				}
			}
			MissionStep currentStep2 = this.currentStep;
			bool flag;
			if (currentStep2 == null)
			{
				flag = (null != null);
			}
			else
			{
				MissionObjective currentObjective2 = currentStep2.currentObjective;
				flag = (((currentObjective2 != null) ? currentObjective2.GetPoi() : null) != null);
			}
			if (flag)
			{
				return this.currentStep.currentObjective.GetPoi();
			}
			if (this.currentStep != null)
			{
				using (IEnumerator<MapPointOfInterest> enumerator = this.currentStep.pointsOfInterest.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			return this.turnIn;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00037B30 File Offset: 0x00035D30
		public virtual Sprite GetIcon()
		{
			if (!string.IsNullOrEmpty(this.iconName))
			{
				return MissionIcons.Get(this.iconName);
			}
			if (this.missionItems.Count > 0)
			{
				return this.missionItems.Keys.First<InventoryItemType>().icon;
			}
			if (this.currentStep != null && this.currentStep.GetIcon() != null)
			{
				return this.currentStep.GetIcon();
			}
			if (this.steps.Count > 0 && this.steps.Last<MissionStep>().GetIcon() != null)
			{
				return this.steps.Last<MissionStep>().GetIcon();
			}
			return null;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00037BDC File Offset: 0x00035DDC
		public bool CanBeIdleCompleted()
		{
			if (!this.CanPerformMission())
			{
				return false;
			}
			MapPointOfInterest activePoi = this.GetActivePoi(false);
			if (activePoi != null)
			{
				MissionStep currentStep = this.currentStep;
				bool flag;
				if (currentStep == null)
				{
					flag = false;
				}
				else
				{
					MissionObjective currentObjective = currentStep.currentObjective;
					bool? flag2 = (currentObjective != null) ? new bool?(currentObjective.LoadoutCanRetrieveItem(activePoi)) : null;
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					return true;
				}
			}
			if (activePoi == this.turnIn && (this.isComplete || this.CanClaimRewards()))
			{
				SpaceStation spaceStation = this.turnIn as SpaceStation;
				if (spaceStation == null || spaceStation.DockingAvailableFor(GameplayManager.Instance.spaceShip))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00037C84 File Offset: 0x00035E84
		private bool CanPerformMission()
		{
			GameplayType? gameplayType = new GameplayType?(GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false));
			TargetLayer? targetLayer = new TargetLayer?(GameplayManager.Instance.spaceShip.preferredTargetLayer);
			GameplayType gameplayType2 = this.gameplayType;
			GameplayType? gameplayType3 = gameplayType;
			if (gameplayType2 == gameplayType3.GetValueOrDefault() & gameplayType3 != null)
			{
				if (this.targetLayer == null)
				{
					return true;
				}
				TargetLayer? targetLayer2 = targetLayer;
				TargetLayer? targetLayer3 = this.targetLayer;
				if (targetLayer2.GetValueOrDefault() == targetLayer3.GetValueOrDefault() & targetLayer2 != null == (targetLayer3 != null))
				{
					return true;
				}
			}
			if (this.gameplayType != GameplayType.Generic)
			{
				return this.gameplayType == GameplayType.Cargo;
			}
			return true;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00037D2C File Offset: 0x00035F2C
		public void OnMissionStart()
		{
			foreach (MissionStep missionStep in this.steps)
			{
				foreach (MissionObjective missionObjective in missionStep.objectives)
				{
					missionObjective.OnMissionStart();
				}
			}
			if (this.missionItems.Count > 0)
			{
				foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.missionItems)
				{
					GamePlayer.current.currentSpaceShip.AddCargo(keyValuePair.Key, keyValuePair.Value, true);
				}
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00037E20 File Offset: 0x00036020
		public virtual void OnMissionAbandoned()
		{
			if (this.removeItemsOnAbandon)
			{
				foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.missionItems)
				{
					GamePlayer.current.currentSpaceShip.RemoveCargo(keyValuePair.Key, keyValuePair.Value, true);
				}
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00037E94 File Offset: 0x00036094
		public void Update(float delta)
		{
			if (this.autoComplete && !DialogueManager.isOpen && this.CanClaimRewards())
			{
				GamePlayer.current.CompleteMission(this, false);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00037EBC File Offset: 0x000360BC
		public virtual bool CanClaimRewards()
		{
			return !this.failed && this.currentStep == null && (this.turnIn == null || MapPointOfInterest.current == this.turnIn) && (!(this.turnIn is SpaceStation) || SpaceStationInterior.instance) && this.isComplete;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00037F18 File Offset: 0x00036118
		public virtual void ClaimRewards(bool force = false)
		{
			if (!this.CanClaimRewards() && !force)
			{
				return;
			}
			if (this.storyId != "tutorial_11")
			{
				Singleton<NotificationManager>.Instance.CreateNotification("Mission Completed: \"" + this.name + "\"").WithColor(ColorHelper.green50).WithCustomTime(8f).WithMissionRewards(this, this.rewards).Show();
			}
			foreach (MissionStep missionStep in this.steps)
			{
				missionStep.OnMissionTurnedIn();
			}
			foreach (MissionReward missionReward in this.rewards)
			{
				missionReward.OnComplete(this);
			}
			GamePlayer.current.RemoveMission(this, true);
			if (this.trackedOnHud)
			{
				this.trackedOnHud = false;
				Singleton<FocusedMissionHandler>.Current.ResetFocusedMission();
			}
			if (GamePlayer.current.lastVisitedMiningPOI != null)
			{
				using (List<MissionStep>.Enumerator enumerator = this.steps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.dynamicPointOfInterest == GamePlayer.current.lastVisitedMiningPOI)
						{
							GamePlayer.current.lastVisitedMiningPOI = null;
						}
					}
				}
			}
			if (string.IsNullOrWhiteSpace(this.storyId))
			{
				MissionObjective.Trigger(MissionTrigger.CompleteDynamicMission, null, null, false);
				return;
			}
			foreach (Storyteller storyteller in GamePlayer.current.storytellers)
			{
				storyteller.OnStoryMissionComplete(this.storyId);
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000380F4 File Offset: 0x000362F4
		public void MissionFailed(string reason)
		{
			this.failed = true;
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UIMissionFailedDesc", new object[]
			{
				reason
			})).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00038134 File Offset: 0x00036334
		public virtual JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.missionItems)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"item",
						keyValuePair.Key.ToJson()
					},
					{
						"count",
						new double?((double)keyValuePair.Value)
					}
				});
			}
			JsonObject jsonObject = new JsonObject();
			jsonObject.Add("name", this.name);
			jsonObject.Add("storyId", this.storyId);
			jsonObject.Add("description", this.description);
			jsonObject.Add("category", this.category);
			jsonObject.Add("completionText", this.completionText);
			jsonObject.Add("difficulty", this.difficulty.ToString());
			jsonObject.Add("steps", this.steps.ToJsonArray<MissionStep>());
			jsonObject.Add("rewards", this.rewards.ToJsonArray<MissionReward>());
			string key = "sourcePoi";
			MapPointOfInterest mapPointOfInterest = this.sourcePoi;
			jsonObject.Add(key, ((mapPointOfInterest != null) ? mapPointOfInterest.guid : null) ?? JsonValue.Null);
			jsonObject.Add("sourceFaction", this.sourceFaction.identifier);
			jsonObject.Add("sourceName", this.sourceName);
			string key2 = "turnIn";
			MapPointOfInterest mapPointOfInterest2 = this.turnIn;
			jsonObject.Add(key2, ((mapPointOfInterest2 != null) ? mapPointOfInterest2.guid : null) ?? JsonValue.Null);
			jsonObject.Add("canAb", new bool?(this.canAbandon));
			jsonObject.Add("tracked", new bool?(this.trackedOnHud));
			jsonObject.Add("canBeIdled", new bool?(this.canBeIdled));
			jsonObject.Add("idle", new bool?(this.idle));
			jsonObject.Add("autoComplete", new bool?(this.autoComplete));
			jsonObject.Add("failed", new bool?(this.failed));
			jsonObject.Add("iconName", this.iconName);
			jsonObject.Add("dynamicLevel", new bool?(this.dynamicLevel));
			JsonObject jsonObject2 = jsonObject;
			if (jsonArray.Count > 0)
			{
				jsonObject2["missionItems"] = jsonArray;
				jsonObject2["removeItemsOnAbandon"] = new bool?(this.removeItemsOnAbandon);
			}
			if (this.nextMissionOnFailed != null)
			{
				jsonObject2["nextMissionOnFailed"] = this.nextMissionOnFailed;
			}
			if (this.enemyFaction != null)
			{
				jsonObject2["enemyFaction"] = this.enemyFaction.identifier;
			}
			return jsonObject2;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0003848C File Offset: 0x0003668C
		public virtual void DataFromJson(JsonObject data)
		{
			this.name = data["name"];
			this.storyId = data["storyId"];
			this.description = data["description"];
			this.category = data["category"];
			this.completionText = data["completionText"];
			this.difficulty = (data["difficulty"].IsString ? Enum.Parse<MissionDifficulty>(data["difficulty"]) : MissionDifficulty.Normal);
			this.sourceFaction = Faction.Get(data["sourceFaction"]);
			this.canAbandon = data["canAb"].AsBoolean;
			this.trackedOnHud = data["tracked"].AsBoolean;
			this.idle = data["idle"].AsBoolean;
			this.canBeIdled = data["canBeIdled"].AsBoolean;
			this.autoComplete = data["autoComplete"];
			this.failed = data["failed"];
			this.iconName = data["iconName"];
			this.dynamicLevel = data["dynamicLevel"];
			if (data["sourcePoi"].IsString)
			{
				GalaxyMapData.current.LoadPointOfInterest(data["sourcePoi"], delegate(MapPointOfInterest poi)
				{
					this.sourcePoi = poi;
				});
			}
			if (data["sourceName"].IsString)
			{
				this.sourceName = data["sourceName"];
			}
			if (data["turnIn"].IsString)
			{
				GalaxyMapData.current.LoadPointOfInterest(data["turnIn"], delegate(MapPointOfInterest poi)
				{
					this.turnIn = poi;
				});
			}
			if (data["missionItem"] != JsonValue.Null)
			{
				this.missionItems.Add(InventoryItemType.FromJson(data["missionItem"]), data["missionItemCount"]);
			}
			if (data["missionItems"] != JsonValue.Null)
			{
				foreach (JsonValue jsonValue in data["missionItems"].AsJsonArray)
				{
					this.missionItems[InventoryItemType.FromJson(jsonValue["item"])] = jsonValue["count"];
				}
			}
			this.removeItemsOnAbandon = data["removeItemsOnAbandon"];
			this.steps.FromJsonArray(data["steps"], new ClassExtensions.ParseJsonValue<MissionStep>(MissionStep.FromJson));
			this.rewards.FromJsonArray(data["rewards"], new ClassExtensions.ParseJsonValue<MissionReward>(MissionReward.FromJson));
			if (data["nextMissionOnFailed"].IsString)
			{
				this.nextMissionOnFailed = data["nextMissionOnFailed"];
			}
			if (data["enemyFaction"].IsString)
			{
				this.enemyFaction = Faction.Get(data["enemyFaction"]);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00038840 File Offset: 0x00036A40
		public static Mission FromJson(JsonValue data)
		{
			if (data.IsString)
			{
				return StoryMission.Get(GamePlayer.current, data);
			}
			Mission mission = new Mission();
			mission.DataFromJson(data);
			return mission;
		}

		// Token: 0x0400039C RID: 924
		public string name;

		// Token: 0x0400039D RID: 925
		public string description;

		// Token: 0x0400039E RID: 926
		public string category;

		// Token: 0x0400039F RID: 927
		public string completionText;

		// Token: 0x040003A0 RID: 928
		public MissionDifficulty difficulty;

		// Token: 0x040003A1 RID: 929
		public MapPointOfInterest sourcePoi;

		// Token: 0x040003A2 RID: 930
		public Faction sourceFaction;

		// Token: 0x040003A3 RID: 931
		public string sourceName;

		// Token: 0x040003A4 RID: 932
		public string iconName;

		// Token: 0x040003A5 RID: 933
		public MapPointOfInterest turnIn;

		// Token: 0x040003A6 RID: 934
		public Faction enemyFaction;

		// Token: 0x040003A7 RID: 935
		public bool canAbandon = true;

		// Token: 0x040003A8 RID: 936
		public bool trackedOnHud = true;

		// Token: 0x040003A9 RID: 937
		public bool canBeIdled;

		// Token: 0x040003AA RID: 938
		public bool idle;

		// Token: 0x040003AB RID: 939
		public bool autoComplete;

		// Token: 0x040003AC RID: 940
		public bool failed;

		// Token: 0x040003AF RID: 943
		public Dictionary<InventoryItemType, int> missionItems = new Dictionary<InventoryItemType, int>();

		// Token: 0x040003B0 RID: 944
		public bool removeItemsOnAbandon;

		// Token: 0x040003B1 RID: 945
		public bool dynamicLevel;

		// Token: 0x040003B2 RID: 946
		public string storyId;

		// Token: 0x040003B3 RID: 947
		public string nextMissionOnFailed;
	}
}
