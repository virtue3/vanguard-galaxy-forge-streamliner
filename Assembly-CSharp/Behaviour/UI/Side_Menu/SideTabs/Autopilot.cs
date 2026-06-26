using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Managers;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B7 RID: 695
	public class Autopilot : SideTabContent
	{
		// Token: 0x060019A8 RID: 6568 RVA: 0x0009F974 File Offset: 0x0009DB74
		private void Awake()
		{
			this.autopilotSettings = GamePlayer.current.autopilotSettings;
			bool active = GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Mining, TargetLayer.Both) && GamePlayer.current.lastVisitedMiningPOI != null;
			int num = 0;
			if (GamePlayer.current.lastVisitedMiningPOI != null)
			{
				using (List<MapPointOfInterest>.Enumerator enumerator = Singleton<TravelManager>.Instance.GenerateShortestRoute(GamePlayer.current.lastVisitedMiningPOI).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is JumpGate)
						{
							num++;
						}
					}
				}
				TMP_Text tmp_Text = this.lastMiningPoi;
				MapPointOfInterest lastVisitedMiningPOI = GamePlayer.current.lastVisitedMiningPOI;
				tmp_Text.text = Translation.Translate((lastVisitedMiningPOI != null) ? lastVisitedMiningPOI.name : null, Array.Empty<object>()) + ", jumps (" + num.ToString() + ")";
			}
			else
			{
				this.lastMiningPoi.text = "";
			}
			this.lastMiningPoi.gameObject.SetActive(active);
			this.resetMiningPoi.gameObject.SetActive(active);
			this.prioritizeToggle.SetIsOnWithoutNotify(this.autopilotSettings.prioritizeHomestation);
			this.runMissionsToggle.SetIsOnWithoutNotify(this.autopilotSettings.runMissions);
			this.preferMissionsToggle.SetIsOnWithoutNotify(this.autopilotSettings.preferMissions);
			if (!SkilltreeNode.PromptEngineeringBasicMissionRunner.isActive)
			{
				this.runMissionsText.GetComponent<TooltipSource>().BodyText = "@AutopilotBasicMissionRunnerRequired";
			}
			this.ammoMinutesSlider.value = (float)this.autopilotSettings.ammoMinutes;
			this.ammoMinutes.text = this.autopilotSettings.ammoMinutes.ToString();
			this.noTravelToggle.SetIsOnWithoutNotify(this.autopilotSettings.noTravel);
			this.autoSellToggle.SetIsOnWithoutNotify(this.autopilotSettings.autoSell);
			this.SetLoadoutOptions();
			this.loadoutAutodetect.text = GameplayManager.Instance.spaceShip.GetPreferredActivityName(true);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0009FB74 File Offset: 0x0009DD74
		private void Update()
		{
			if (GamePlayer.current.autopilotSettings.noTravel != this.noTravelToggle.isOn)
			{
				this.noTravelToggle.SetIsOnWithoutNotify(GamePlayer.current.autopilotSettings.noTravel);
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0009FBAC File Offset: 0x0009DDAC
		public void ResetMiningPoi()
		{
			GamePlayer.current.lastVisitedMiningPOI = null;
			this.lastMiningPoi.text = "";
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0009FBC9 File Offset: 0x0009DDC9
		public void TogglePrioritizeHomestation()
		{
			this.autopilotSettings.prioritizeHomestation = this.prioritizeToggle.isOn;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0009FBE1 File Offset: 0x0009DDE1
		public void TogglePrioritizeMissions()
		{
			this.autopilotSettings.preferMissions = this.preferMissionsToggle.isOn;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0009FBF9 File Offset: 0x0009DDF9
		public void ToggleRunMissions()
		{
			this.autopilotSettings.runMissions = this.runMissionsToggle.isOn;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0009FC11 File Offset: 0x0009DE11
		public void ChangeAmmoMinutes()
		{
			this.autopilotSettings.ammoMinutes = (int)this.ammoMinutesSlider.value;
			this.ammoMinutes.text = this.autopilotSettings.ammoMinutes.ToString();
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0009FC45 File Offset: 0x0009DE45
		public void ToggleNoTravel()
		{
			this.autopilotSettings.noTravel = this.noTravelToggle.isOn;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0009FC5D File Offset: 0x0009DE5D
		public void ToggleAutoSell()
		{
			this.autopilotSettings.autoSell = this.autoSellToggle.isOn;
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0009FC78 File Offset: 0x0009DE78
		public void ChangeLoadout()
		{
			switch (this.loadoutDropdown.value)
			{
			case 1:
				this.autopilotSettings.preferredGameplayType = GameplayType.Mining;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Surface;
				break;
			case 2:
				this.autopilotSettings.preferredGameplayType = GameplayType.Mining;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Core;
				break;
			case 3:
				this.autopilotSettings.preferredGameplayType = GameplayType.Salvage;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Surface;
				break;
			case 4:
				this.autopilotSettings.preferredGameplayType = GameplayType.Salvage;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Core;
				break;
			case 5:
				this.autopilotSettings.preferredGameplayType = GameplayType.Combat;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Both;
				break;
			case 6:
				this.autopilotSettings.preferredGameplayType = GameplayType.Cargo;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Both;
				break;
			default:
				this.autopilotSettings.preferredGameplayType = GameplayType.Generic;
				this.autopilotSettings.preferredTargetLayer = TargetLayer.Both;
				break;
			}
			if (this.autopilotSettings.preferredGameplayType == GameplayType.Cargo)
			{
				this.autopilotSettings.runMissions = true;
				this.runMissionsToggle.SetIsOnWithoutNotify(true);
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0009FD98 File Offset: 0x0009DF98
		public void SetLoadoutOptions()
		{
			List<string> list = new List<string>
			{
				"Autodetect",
				GameplayType.Mining.ToString() + " " + TargetLayer.Surface.ToString(),
				GameplayType.Mining.ToString() + " " + TargetLayer.Core.ToString(),
				GameplayType.Salvage.ToString() + " " + TargetLayer.Surface.ToString(),
				GameplayType.Salvage.ToString() + " " + SpaceShip.GetTargetLayerName(new GameplayType?(GameplayType.Salvage), new TargetLayer?(TargetLayer.Core)),
				GameplayType.Combat.ToString(),
				GameplayType.Cargo.ToString()
			};
			this.loadoutDropdown.AddOptions(list);
			if (this.autopilotSettings.preferredGameplayType != GameplayType.Generic)
			{
				string targetLayerName = SpaceShip.GetTargetLayerName(new GameplayType?(this.autopilotSettings.preferredGameplayType), new TargetLayer?(this.autopilotSettings.preferredTargetLayer));
				int valueWithoutNotify = list.IndexOf((targetLayerName == "") ? this.autopilotSettings.preferredGameplayType.ToString() : (this.autopilotSettings.preferredGameplayType.ToString() + " " + targetLayerName));
				this.loadoutDropdown.SetValueWithoutNotify(valueWithoutNotify);
				return;
			}
			this.loadoutDropdown.SetValueWithoutNotify(0);
		}

		// Token: 0x04001019 RID: 4121
		[SerializeField]
		private TMP_Text lastMiningPoi;

		// Token: 0x0400101A RID: 4122
		[SerializeField]
		private Button resetMiningPoi;

		// Token: 0x0400101B RID: 4123
		[SerializeField]
		private Toggle prioritizeToggle;

		// Token: 0x0400101C RID: 4124
		[SerializeField]
		private TMP_Text runMissionsText;

		// Token: 0x0400101D RID: 4125
		[SerializeField]
		private Toggle runMissionsToggle;

		// Token: 0x0400101E RID: 4126
		[SerializeField]
		private Toggle preferMissionsToggle;

		// Token: 0x0400101F RID: 4127
		[SerializeField]
		private Slider ammoMinutesSlider;

		// Token: 0x04001020 RID: 4128
		[SerializeField]
		private TMP_Text ammoMinutes;

		// Token: 0x04001021 RID: 4129
		[SerializeField]
		private Toggle noTravelToggle;

		// Token: 0x04001022 RID: 4130
		[SerializeField]
		private TMP_Dropdown loadoutDropdown;

		// Token: 0x04001023 RID: 4131
		[SerializeField]
		private TMP_Text loadoutAutodetect;

		// Token: 0x04001024 RID: 4132
		[SerializeField]
		private Toggle autoSellToggle;

		// Token: 0x04001025 RID: 4133
		private AutopilotSettings autopilotSettings;
	}
}
