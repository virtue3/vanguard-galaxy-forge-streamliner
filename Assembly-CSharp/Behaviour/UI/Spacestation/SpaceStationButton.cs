using System;
using Behaviour.UI.Side_Menu;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation
{
	// Token: 0x02000215 RID: 533
	public class SpaceStationButton : BaseMenuButton
	{
		// Token: 0x060013A7 RID: 5031 RVA: 0x0007FA23 File Offset: 0x0007DC23
		protected override void Awake()
		{
			base.Awake();
			this.defaultTabColor = this.selectedTab.color;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0007FA3C File Offset: 0x0007DC3C
		protected override void Start()
		{
			base.Start();
			base.SetButtonText(Translation.Translate("@SS" + this.spaceStationFacility.ToString(), Array.Empty<object>()));
			this.SetupUmbralHighlight();
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0007FA78 File Offset: 0x0007DC78
		private void Update()
		{
			if (this.spaceStationFacility == SpaceStationFacility.MissionBoard)
			{
				this.updateHighlightTimer -= Time.deltaTime;
				if (this.updateHighlightTimer < 0f)
				{
					this.updateHighlightTimer = 0.25f;
					bool active = false;
					foreach (Mission mission in GamePlayer.current.allMissions)
					{
						if (mission.turnIn == SpaceStation.current && mission.CanClaimRewards())
						{
							active = true;
							break;
						}
					}
					this.ToggleMissionHightlight(active);
				}
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0007FB18 File Offset: 0x0007DD18
		protected override void OnClick()
		{
			base.OnClick();
			SpaceStationInterior.instance.GoToLocation(this.spaceStationFacility, true);
			base.Highlight(true);
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0007FB38 File Offset: 0x0007DD38
		public void SetupUmbralHighlight()
		{
			if (!this.umbralHighlight)
			{
				return;
			}
			float umbralControlLevel = SpaceStation.current.umbralControlLevel;
			if ((this.spaceStationFacility == SpaceStationFacility.MissionBoard && umbralControlLevel >= 0.05f) || (this.spaceStationFacility.IsShop() && umbralControlLevel >= 0.5f))
			{
				this.umbralHighlight.gameObject.SetActive(true);
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0007FB96 File Offset: 0x0007DD96
		public void ToggleMissionHightlight(bool active)
		{
			this.missionHightlight.GameObject().SetActive(active);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0007FBA9 File Offset: 0x0007DDA9
		public void UpdateSelectedFacility(SpaceStationFacility facility)
		{
			this.selectedTab.color = ((facility == this.spaceStationFacility) ? this.highlightTabColor : this.defaultTabColor);
		}

		// Token: 0x04000B45 RID: 2885
		public SpaceStationFacility spaceStationFacility;

		// Token: 0x04000B46 RID: 2886
		[SerializeField]
		private Image umbralHighlight;

		// Token: 0x04000B47 RID: 2887
		[SerializeField]
		private Image missionHightlight;

		// Token: 0x04000B48 RID: 2888
		[SerializeField]
		private Image selectedTab;

		// Token: 0x04000B49 RID: 2889
		[SerializeField]
		private Color highlightTabColor;

		// Token: 0x04000B4A RID: 2890
		private Color defaultTabColor;

		// Token: 0x04000B4B RID: 2891
		private float updateHighlightTimer;
	}
}
