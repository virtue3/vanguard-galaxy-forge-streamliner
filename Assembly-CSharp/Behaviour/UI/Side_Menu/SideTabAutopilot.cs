using System;
using Behaviour.Gameplay;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x0200029E RID: 670
	public class SideTabAutopilot : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ITooltipCustomSource, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x0600190A RID: 6410 RVA: 0x0009B8A0 File Offset: 0x00099AA0
		private void Start()
		{
			if (this.toggle)
			{
				if (!GamePlayer.current.autoPlayUnlocked)
				{
					this.toggle.interactable = false;
				}
				this.highlightImage.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0009B8D8 File Offset: 0x00099AD8
		private void Update()
		{
			if (!this.toggle)
			{
				return;
			}
			if (GamePlayer.current == null || !Singleton<IdleManager>.Instance)
			{
				this.toggle.image.material = Materials.Grayscale;
				return;
			}
			if (GamePlayer.current.AutopilotCanBeEnabled())
			{
				this.toggle.interactable = true;
				this.toggle.image.material = null;
			}
			else
			{
				this.toggle.interactable = false;
				this.toggle.image.material = Materials.Grayscale;
			}
			this.toggleImage.gameObject.SetActive(!GamePlayer.current.autoPlay);
			if (GamePlayer.current.autoPlay)
			{
				if (Singleton<IdleManager>.Instance.delayTimer > 0f)
				{
					this.toggle.SetIsOnWithoutNotify(false);
					this.timerFill.fillAmount = 1f - Singleton<IdleManager>.Instance.delayTimer / Singleton<IdleManager>.Instance.delayTimerBase;
					this.timerFill.color = ColorHelper.orange75;
				}
				else
				{
					this.toggle.SetIsOnWithoutNotify(true);
					this.timerFill.fillAmount = 1f - Singleton<IdleManager>.Instance.updateTimer / Singleton<IdleManager>.Instance.updateTimerBase;
					this.timerFill.color = ColorHelper.greenish;
				}
			}
			else
			{
				this.toggle.SetIsOnWithoutNotify(false);
				this.timerFill.fillAmount = 0f;
			}
			if (this.hoverTime > 0f)
			{
				this.hoverTime -= Time.deltaTime;
			}
			if (GamePlayer.current.autoPlay && this.noTravelImage)
			{
				this.noTravelImage.gameObject.SetActive(GamePlayer.current.autopilotSettings.noTravel);
			}
			SidePanel.instance.ShowIdleStatus(this.toggle.isOn || (Singleton<IdleManager>.HasInstance && Singleton<IdleManager>.Instance.delayTimer > 0f && Singleton<IdleManager>.Instance.delayTimer < 5f));
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0009BAE8 File Offset: 0x00099CE8
		public void ToggleChanged()
		{
			if (!GamePlayer.current.autoPlayUnlocked || Keyboard.current.ctrlKey.isPressed)
			{
				return;
			}
			if (Singleton<IdleManager>.Instance.delayTimer > 0f)
			{
				GamePlayer.current.autoPlay = true;
			}
			else
			{
				GamePlayer.current.autoPlay = !GamePlayer.current.autoPlay;
			}
			if (GamePlayer.current.autoPlay)
			{
				GamePlayer.current.StartAutopilotSession();
				IdleManager.UpdateActivity("@IdleCalculating", Array.Empty<object>());
				GamePlayer.current.holdPosition = false;
			}
			else
			{
				GamePlayer.current.EndAutopilotSession();
			}
			IdleManager.DelayIdleActivities(0f);
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0009BB90 File Offset: 0x00099D90
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				GamePlayer.current.EndAutopilotSession();
				IdleManager.DelayIdleActivities(0f);
			}
			else if (eventData.button == PointerEventData.InputButton.Left && Keyboard.current.ctrlKey.isPressed)
			{
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Ship, 2);
			}
			else if (!this.toggle)
			{
				this.ToggleChanged();
			}
			this.ToggleHighlight(false);
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x0009BBFD File Offset: 0x00099DFD
		public void ToggleEchoTravel()
		{
			GamePlayer.current.autopilotSettings.noTravel = !GamePlayer.current.autopilotSettings.noTravel;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0009BC20 File Offset: 0x00099E20
		public void ToggleHighlight(bool show)
		{
			if (this.highlightImage)
			{
				this.highlightImage.gameObject.SetActive(show);
			}
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0009BC40 File Offset: 0x00099E40
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine("@UIEnableAutoplay", 16, 8f);
			tooltip.AddTextLine("@UIEnableAutoplayDesc", 12, 8f);
			if (GamePlayer.current.autoPlay)
			{
				tooltip.AddTextLine("@UIAutoplayInteraction", 12, 8f);
			}
			if (!GamePlayer.current.autoPlayUnlocked)
			{
				tooltip.AddTextLine("@UIAutoplayLocked", 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if (!GamePlayer.current.AutopilotCanBeEnabled())
			{
				tooltip.AddTextLine("@UIAutoplayBlocked", 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if (!GamePlayer.current.autoPlay)
			{
				tooltip.AddTextLine("@UIAutoplayInteractionEnable", 12, 8f);
			}
			tooltip.AddTextLine("@UIAutoplayInteractionSettings", 12, 8f);
			tooltip.AddTextLine(Translation.Translate("@UIAutoplayActivity", Array.Empty<object>()) + " " + GameplayManager.Instance.spaceShip.GetPreferredActivityName(false), 12, 8f);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0009BD57 File Offset: 0x00099F57
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hoverTime = 0.5f;
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0009BD64 File Offset: 0x00099F64
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.hoverTime <= 0f)
			{
				this.ToggleHighlight(false);
			}
		}

		// Token: 0x04000F81 RID: 3969
		[SerializeField]
		private Toggle toggle;

		// Token: 0x04000F82 RID: 3970
		[SerializeField]
		private Image timerFill;

		// Token: 0x04000F83 RID: 3971
		[SerializeField]
		private Image highlightImage;

		// Token: 0x04000F84 RID: 3972
		[SerializeField]
		private Image toggleImage;

		// Token: 0x04000F85 RID: 3973
		[SerializeField]
		private RectTransform noTravelImage;

		// Token: 0x04000F86 RID: 3974
		private float hoverTime;
	}
}
