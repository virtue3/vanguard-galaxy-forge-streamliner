using System;
using Behaviour.Equipment.Module;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation.Location.Recruitment;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Crew;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.HUD.Fleet
{
	// Token: 0x02000290 RID: 656
	public class WingmanDisplay : MonoBehaviour
	{
		// Token: 0x06001802 RID: 6146 RVA: 0x0009655C File Offset: 0x0009475C
		private void Update()
		{
			if (this.wingman != null)
			{
				SpaceShipData spaceShipData = null;
				foreach (SpaceShipData spaceShipData2 in GamePlayer.current.activeFleet)
				{
					if (spaceShipData2.commanderData == this.wingman)
					{
						spaceShipData = spaceShipData2;
						break;
					}
				}
				if (spaceShipData == null)
				{
					return;
				}
				if (this.wingman.repairing)
				{
					if (spaceShipData.repairTimer > 0f)
					{
						this.status.text = Translation.Translate("@UIWingmanRepairing", new object[]
						{
							GameMath.FormatTime((int)spaceShipData.repairTimer, true)
						});
						return;
					}
					this.status.text = Translation.Translate("@UIWingmanRepairedWait", Array.Empty<object>());
					return;
				}
				else
				{
					SpaceShip spaceShip = spaceShipData.unit as SpaceShip;
					if (!spaceShip)
					{
						return;
					}
					AbstractTargetingModule targetProvider = spaceShip.targetProvider;
					TargetableUnit y = (targetProvider != null) ? targetProvider.priorityTarget : null;
					if (this.currentTarget != y)
					{
						this.currentTarget = y;
					}
					if (this.wingman.timeLeft < 0f)
					{
						this.status.text = Translation.Translate("@UIWingmanLeaving", Array.Empty<object>());
						this.timeLeft.gameObject.SetActive(false);
					}
					else if (this.currentTarget == null)
					{
						this.status.text = Translation.Translate("@UIWingmanFormation", Array.Empty<object>());
					}
					else
					{
						this.status.text = Translation.Translate("@UIWingmanTarget", new object[]
						{
							this.currentTarget.targetName
						});
					}
					this.timeLeft.text = GameMath.FormatTime((int)this.wingman.timeLeft, true);
				}
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00096720 File Offset: 0x00094920
		public void SetWingman(Mercenary wingman)
		{
			this.wingman = wingman;
			this.portrait.SetMercenary(wingman);
			this.behaviour.text = wingman.behaviour.ToString();
			this.portrait.SetRightClickCallback(new Action(this.ToggleWingmanStatus));
			this.timeLeft.gameObject.SetActive(true);
			this.fireMercButton.GetComponent<TooltipSource>().BodyText = "@HUDMercFire";
			this.hireExtendButton.GetComponent<TooltipSource>().BodyText = Translation.Translate("@HUDMercHireExtend", new object[]
			{
				"$ " + GameMath.FormatNumber((float)wingman.creditCost, -1)
			});
			this.autoExtendToggle.GetComponent<TooltipSource>().BodyText = Translation.Translate("@HUDMercAutoExtend", new object[]
			{
				wingman.callsign
			});
			this.autoExtendToggle.SetIsOnWithoutNotify(wingman.autoExtend);
			if (wingman.isExtra)
			{
				this.hireExtendButton.gameObject.SetActive(false);
				this.autoExtendToggle.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00096838 File Offset: 0x00094A38
		public void SetAnchoredPosition(float y)
		{
			Vector2 anchoredPosition = (base.transform as RectTransform).anchoredPosition;
			anchoredPosition.y = y;
			(base.transform as RectTransform).anchoredPosition = anchoredPosition;
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00096870 File Offset: 0x00094A70
		public void ToggleWingmanStatus()
		{
			if (this.wingman.behaviour == WingmanBehaviour.Formation)
			{
				this.wingman.behaviour = WingmanBehaviour.Defensive;
			}
			else
			{
				this.wingman.behaviour++;
			}
			this.behaviour.text = this.wingman.behaviour.ToString();
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x000968CD File Offset: 0x00094ACD
		public void OnToggleAutoExtend()
		{
			this.wingman.autoExtend = this.autoExtendToggle.isOn;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000968E5 File Offset: 0x00094AE5
		public void OnFireMerc()
		{
			if (GlobalControls.modifierShift)
			{
				this.wingman.autoExtend = false;
				this.wingman.timeLeft = -1f;
			}
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0009690C File Offset: 0x00094B0C
		public void OnExtendHire()
		{
			if (!GamePlayer.current.CanAfford((float)this.wingman.creditCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			GamePlayer.current.RemoveCredits((float)this.wingman.creditCost);
			this.wingman.AddMercTime(1);
		}

		// Token: 0x04000EFF RID: 3839
		[SerializeField]
		private MercenaryPortrait portrait;

		// Token: 0x04000F00 RID: 3840
		[SerializeField]
		private TMP_Text behaviour;

		// Token: 0x04000F01 RID: 3841
		[SerializeField]
		private TMP_Text status;

		// Token: 0x04000F02 RID: 3842
		[SerializeField]
		private TMP_Text timeLeft;

		// Token: 0x04000F03 RID: 3843
		[SerializeField]
		private Button fireMercButton;

		// Token: 0x04000F04 RID: 3844
		[SerializeField]
		private Button hireExtendButton;

		// Token: 0x04000F05 RID: 3845
		[SerializeField]
		private Toggle autoExtendToggle;

		// Token: 0x04000F06 RID: 3846
		private Mercenary wingman;

		// Token: 0x04000F07 RID: 3847
		private TargetableUnit currentTarget;
	}
}
