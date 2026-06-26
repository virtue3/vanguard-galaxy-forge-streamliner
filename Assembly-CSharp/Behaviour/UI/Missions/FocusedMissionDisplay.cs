using System;
using System.Collections.Generic;
using Behaviour.UI.Side_Menu;
using Behaviour.Util;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Missions
{
	// Token: 0x02000258 RID: 600
	public class FocusedMissionDisplay : MonoBehaviour
	{
		// Token: 0x06001625 RID: 5669 RVA: 0x0008CDD7 File Offset: 0x0008AFD7
		private void Start()
		{
			this.SetupFocusedMission();
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0008CDDF File Offset: 0x0008AFDF
		private void Update()
		{
			this.updateTimer += Time.deltaTime;
			if (this.updateTimer > 0.25f)
			{
				this.updateTimer = 0f;
				this.UpdateObjectives();
			}
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0008CE11 File Offset: 0x0008B011
		private void OnEnable()
		{
			this.UpdateObjectives();
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0008CE19 File Offset: 0x0008B019
		private void SetupFocusedMission()
		{
			if (Singleton<FocusedMissionHandler>.Instance == null)
			{
				return;
			}
			this.UpdateObjectives();
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0008CE30 File Offset: 0x0008B030
		public void UpdateObjectives()
		{
			FocusedMissionHandler instance = Singleton<FocusedMissionHandler>.Instance;
			Mission mission = (instance != null) ? instance.focusedMission : null;
			if (mission == null || !mission.trackedOnHud)
			{
				this.ClearDisplay();
				return;
			}
			this.missionName.text = mission.name;
			this.missionName.color = mission.difficulty.GetColor();
			if (mission.failed)
			{
				this.objectivesText.TL("@UIMissionFailed", Array.Empty<object>());
				this.objectivesText.color = ColorHelper.reddish;
			}
			else
			{
				string text = "";
				int num = 0;
				if (mission.currentStep != null)
				{
					int count = mission.currentStep.objectives.Count;
					foreach (MissionObjective missionObjective in mission.currentStep.objectives)
					{
						string text2 = " - " + missionObjective.statusText;
						text = text + text2.HighlightWithColor(missionObjective.IsComplete() ? ColorHelper.greenish : ColorHelper.white75) + "\n";
						num++;
						if (!mission.currentStep.requireAllObjectives && num < count)
						{
							text += " OR \n";
						}
					}
					text += "\n";
					using (IEnumerator<MapPointOfInterest> enumerator2 = mission.currentStep.pointsOfInterest.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MapPointOfInterest mapPointOfInterest = enumerator2.Current;
							text = text + Translation.Translate("@MissionLocation", new object[]
							{
								mapPointOfInterest.name
							}) + "\n";
						}
						goto IL_1B9;
					}
				}
				text += Translation.Translate("@MissionReadyForTurnIn", Array.Empty<object>()).HighlightWithColor(ColorHelper.greenish);
				IL_1B9:
				this.objectivesText.text = text;
			}
			if (this.locateButton)
			{
				MapPointOfInterest activePoi = mission.GetActivePoi(false);
				this.locateButton.gameObject.SetActive(!mission.failed && activePoi != null && activePoi != GamePlayer.current.currentPointOfInterest);
			}
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0008D064 File Offset: 0x0008B264
		private void ClearDisplay()
		{
			this.missionName.text = "";
			this.objectivesText.text = "";
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0008D088 File Offset: 0x0008B288
		public void OnLocate()
		{
			FocusedMissionHandler instance = Singleton<FocusedMissionHandler>.Instance;
			Mission mission = (instance != null) ? instance.focusedMission : null;
			if (mission != null)
			{
				Debug.Log("ShopPoiOnMap");
				SidePanel.instance.ShowPoiOnMap(mission.GetActivePoi(false));
			}
		}

		// Token: 0x04000D5D RID: 3421
		private float updateTimer;

		// Token: 0x04000D5E RID: 3422
		[SerializeField]
		private TextMeshProUGUI missionName;

		// Token: 0x04000D5F RID: 3423
		[SerializeField]
		private TextMeshProUGUI objectivesText;

		// Token: 0x04000D60 RID: 3424
		[SerializeField]
		private Button locateButton;
	}
}
