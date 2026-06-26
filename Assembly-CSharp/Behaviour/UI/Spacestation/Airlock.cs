using System;
using System.Collections.Generic;
using Behaviour.Dialogues;
using Behaviour.UI.Missions;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.Util;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Spacestation
{
	// Token: 0x02000212 RID: 530
	public class Airlock : MonoBehaviour
	{
		// Token: 0x0600138A RID: 5002 RVA: 0x0007EC00 File Offset: 0x0007CE00
		private void Start()
		{
			this.ShowUmbral(false);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0007EC09 File Offset: 0x0007CE09
		private void Update()
		{
			if (this.homeStationLabel)
			{
				this.SetHomeStationInfo();
			}
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0007EC20 File Offset: 0x0007CE20
		private void SetHomeStationInfo()
		{
			this.homeStationLabel.SetActive(GamePlayer.current.homeStation == SpaceStation.current);
			this.homeStationButton.SetActive(SpaceStation.current.canBeHomeStation && GamePlayer.current.homeStation != SpaceStation.current);
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0007EC78 File Offset: 0x0007CE78
		public void InitCharacters(List<Character> characters)
		{
			foreach (Character character in characters)
			{
				UnityEngine.Object.Instantiate<CharacterMono>(this.characterPrefab, this.characterContainer).SetCharacter(character);
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0007ECD8 File Offset: 0x0007CED8
		public void SetSpacestationAsHome()
		{
			GamePlayer.current.homeStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
			Singleton<NotificationManager>.Instance.CreateNotification("Home station set: " + GamePlayer.current.currentPointOfInterest.name).WithColor(ColorHelper.green50).Show();
			SidePanel.instance.RefreshIfOpen();
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0007ED3C File Offset: 0x0007CF3C
		public void ToggleMissionBoard()
		{
			FocusedMissionHandler instance = Singleton<FocusedMissionHandler>.Instance;
			Mission mission = (instance != null) ? instance.focusedMission : null;
			if (mission != null && mission.turnIn == SpaceStation.current && SpaceStation.current.missionBoard != null)
			{
				MissionBoard.preselectMission = mission;
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.MissionBoard, true);
				return;
			}
			if (mission != null)
			{
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Missions, 0);
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0007ED9C File Offset: 0x0007CF9C
		public void ShowUmbral(bool show)
		{
			MapPointOfInterest currentPointOfInterest = GamePlayer.current.currentPointOfInterest;
			Faction faction = show ? Faction.puppeteers : currentPointOfInterest.faction;
			Color color = faction.IsEnemy(Faction.player) ? ColorHelper.reddish : ColorHelper.greenish;
			this.stationName.text = Translation.Highlight("{0} <size=12>#({1})#</size>", (faction == Faction.puppeteers) ? ColorHelper.umbralColor : color, new object[]
			{
				currentPointOfInterest.name,
				faction.name
			});
		}

		// Token: 0x04000B2B RID: 2859
		[SerializeField]
		private TextMeshProUGUI stationName;

		// Token: 0x04000B2C RID: 2860
		[SerializeField]
		private RectTransform characterContainer;

		// Token: 0x04000B2D RID: 2861
		[SerializeField]
		private CharacterMono characterPrefab;

		// Token: 0x04000B2E RID: 2862
		[SerializeField]
		private GameObject homeStationLabel;

		// Token: 0x04000B2F RID: 2863
		[SerializeField]
		private GameObject homeStationButton;
	}
}
