using System;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Galaxy.POI;
using Source.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000281 RID: 641
	public class HoldPositionToggle : MonoBehaviour
	{
		// Token: 0x0600175F RID: 5983 RVA: 0x00093C85 File Offset: 0x00091E85
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00093C98 File Offset: 0x00091E98
		private void Update()
		{
			if (GamePlayer.current.holdPosition != this.toggle.isOn)
			{
				this.toggle.SetIsOnWithoutNotify(GamePlayer.current.holdPosition);
			}
			this.statusText.gameObject.SetActive(this.toggle.isOn);
			this.border.gameObject.SetActive(this.toggle.isOn);
			(base.transform as RectTransform).sizeDelta = new Vector2((float)(this.toggle.isOn ? 146 : 38), 38f);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00093D38 File Offset: 0x00091F38
		public void OnToggle()
		{
			GamePlayer.current.holdPosition = this.toggle.isOn;
			if (GamePlayer.current.holdPosition)
			{
				if (GamePlayer.current.waypoints.Count > 0 && Singleton<TravelManager>.Instance.AreWeLeaving())
				{
					BasePoiManager current = BasePoiManager.current;
					if (current != null && current.SpaceShipIsAtPoi())
					{
						Singleton<TravelManager>.Instance.CancelTravel(null);
						SpaceStation spaceStation = GamePlayer.current.currentPointOfInterest as SpaceStation;
						if (spaceStation != null && spaceStation.PlayerIsFriendly())
						{
							HudManager.Instance.ToggleDockButton(true);
						}
					}
				}
				GameplayManager.Instance.spaceShip.SetBrakeDestination();
				GamePlayer.current.EndAutopilotSession();
			}
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00093DED File Offset: 0x00091FED
		public void DoToggle()
		{
			this.toggle.isOn = !this.toggle.isOn;
		}

		// Token: 0x04000E82 RID: 3714
		[SerializeField]
		private Toggle toggle;

		// Token: 0x04000E83 RID: 3715
		[SerializeField]
		private Image border;

		// Token: 0x04000E84 RID: 3716
		[SerializeField]
		private TMP_Text statusText;

		// Token: 0x04000E85 RID: 3717
		private RectTransform rectTransform;
	}
}
