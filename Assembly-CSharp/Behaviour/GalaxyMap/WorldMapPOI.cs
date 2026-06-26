using System;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.DebugScreen;
using Behaviour.UI.HUD;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000335 RID: 821
	public class WorldMapPOI : WorldMapElement, ITooltipCustomSource, IWorldMapVisual, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001F20 RID: 7968 RVA: 0x000B947A File Offset: 0x000B767A
		protected override void Update()
		{
			base.highlightPlayer = (MapPointOfInterest.currentOrNext == this.content);
			base.Update();
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000B9498 File Offset: 0x000B7698
		public static WorldMapPOI GetPrefab(string name)
		{
			if (WorldMapPOI.prefabs == null)
			{
				WorldMapPOI.prefabs = new Dictionary<string, WorldMapPOI>();
				foreach (WorldMapPOI worldMapPOI in Resources.LoadAll<WorldMapPOI>("GalaxyMap/POI"))
				{
					WorldMapPOI.prefabs[worldMapPOI.gameObject.name] = worldMapPOI;
				}
			}
			return WorldMapPOI.prefabs[name];
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000B94F4 File Offset: 0x000B76F4
		public void SetContent(MapPointOfInterest poi)
		{
			this.content = poi;
			this.SetMissionExclamationMark();
			if (poi == GamePlayer.current.homeStation)
			{
				base.SetHomebaseMarker();
			}
			if (base.factionIcons != null && poi is SpaceStation)
			{
				base.factionIcons.SetPoiFactionIcon(this);
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000B9544 File Offset: 0x000B7744
		private void SetMissionExclamationMark()
		{
			base.missionLocation.gameObject.SetActive(false);
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				if (mission.isComplete && mission.turnIn == this.content)
				{
					base.SetMissionMarker(mission);
					break;
				}
				MissionStep currentStep = mission.currentStep;
				if (currentStep != null && !currentStep.isComplete && mission.currentStep.IsPointOfInterest(this.content))
				{
					base.SetMissionMarker(mission);
					break;
				}
			}
			SpaceStation spaceStation = this.content as SpaceStation;
			if (spaceStation != null && spaceStation.HasAvailableMission())
			{
				base.SetAvailableMissionMarker(base.missionLocation.gameObject.activeSelf ? base.missionLocation2 : base.missionLocation);
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000B962C File Offset: 0x000B782C
		public void SetMissionPoi()
		{
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000B962E File Offset: 0x000B782E
		private void OnMouseUpAsButton()
		{
			this.MouseUpHandling();
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000B9638 File Offset: 0x000B7838
		private void MouseUpHandling()
		{
			if (ConsoleScreen.DebugModifier())
			{
				ConsoleScreen.Teleport(this.content.guid);
				if (HudManager.Instance)
				{
					HudManager.Instance.ToggleHudElements(true);
				}
				AbstractGalaxyMapManager.ToggleGalaxyMap();
				return;
			}
			if (this.content.CanTravelHere())
			{
				SpaceStation spaceStation = this.content as SpaceStation;
				if ((spaceStation != null && spaceStation.canBeHomeStation) || !Singleton<TravelManager>.Instance.emergencyJumpActive)
				{
					if (SpaceStationInterior.instance && Singleton<TravelManager>.Instance.CanWeTravel(this.content) && !GameplayManager.Instance.spaceShip.AmmoInCargoForTurrets(false))
					{
						AlertPopup.ShowQuery("@SSNoAmmoForTurretQuery", null, null, new Action(this.LeaveSpacestation), null, null, null);
						return;
					}
					this.LeaveSpacestation();
					return;
				}
			}
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000B96FC File Offset: 0x000B78FC
		private void LeaveSpacestation()
		{
			if (Singleton<TravelManager>.Instance.TryInitiateTravel(this.content))
			{
				AbstractGalaxyMapManager.ToggleGalaxyMap();
				HudManager.Instance.echoRemarks.ChangeVisibilityStatus(true);
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000B9728 File Offset: 0x000B7928
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddHeader(this.content.name, this.content.level, 0, 16, 8f);
			tooltip.AddTextLine(this.content.typeName, 12, 8f).Text.color = ColorHelper.boringGrey;
			if (MapPointOfInterest.current == this.content)
			{
				tooltip.AddTextLine(Translation.Translate("@YouAreHere", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			else if (!Singleton<TravelManager>.Instance.TravelActive())
			{
				tooltip.AddTextLine(Translation.Translate("@ClickToTravel", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
			}
			if (this.content is SpaceStation)
			{
				tooltip.AddTextLine(Translation.Highlight("@Owner", this.content.faction.IsEnemy(Faction.player) ? ColorHelper.reddish : ColorHelper.greenish, new object[]
				{
					this.content.faction.name
				}), 12, 8f);
				if (this.content is EmbassyStation && this.content.faction != Faction.puppeteers)
				{
					tooltip.AddTextLine("@EmbassyTooltipDesc", 12, 8f);
				}
			}
			tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
			if (!string.IsNullOrEmpty(this.content.dangerLevel))
			{
				tooltip.AddTextLine(this.content.dangerLevel, 12, 8f).Text.color = ColorHelper.reddish;
			}
			if (!string.IsNullOrEmpty(this.content.hazardsDescription))
			{
				tooltip.AddTextLine(this.content.hazardsDescription, 12, 8f).Text.color = ColorHelper.orange75;
			}
			this.content.AddTooltipInfo(tooltip);
			bool flag = false;
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (!mission.isComplete || mission.turnIn != this.content)
				{
					MissionStep currentStep = mission.currentStep;
					if (currentStep == null || !currentStep.IsPointOfInterest(this.content))
					{
						continue;
					}
				}
				if (!flag)
				{
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
					tooltip.AddTextLine("@MapPOIMissions", 12, 8f);
					flag = true;
				}
				tooltip.AddTextLine(mission.name, 12, 8f).Text.color = mission.difficulty.GetColor();
			}
			if (this.content.timeLeft > 0f)
			{
				tooltip.AddTextLine("", 12, 8f).gameObject.AddComponent<WorldMapPOITimer>().poi = this.content;
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000B9A30 File Offset: 0x000B7C30
		public List<SpriteRenderer> GetSpriteRenderers()
		{
			return new List<SpriteRenderer>
			{
				this.spriteRenderer,
				base.backgroundRenderer,
				base.missionLocation
			};
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000B9A5B File Offset: 0x000B7C5B
		public void OnPointerClick(PointerEventData eventData)
		{
			this.MouseUpHandling();
		}

		// Token: 0x040012AB RID: 4779
		private static Dictionary<string, WorldMapPOI> prefabs;

		// Token: 0x040012AC RID: 4780
		public MapPointOfInterest content;
	}
}
