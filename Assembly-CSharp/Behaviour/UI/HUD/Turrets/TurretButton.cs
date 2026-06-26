using System;
using Behaviour.Crew;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.HUD.Turrets
{
	// Token: 0x0200028A RID: 650
	public class TurretButton : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x060017D2 RID: 6098 RVA: 0x000956E5 File Offset: 0x000938E5
		private void Start()
		{
			this.tooltip = base.GetComponent<TooltipSource>();
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000956F4 File Offset: 0x000938F4
		private void Update()
		{
			if (!this.parent.up)
			{
				return;
			}
			string text = this.turret.currentMagSize.ToString();
			Color color;
			float num;
			if (this.turret.isReloading)
			{
				color = ColorHelper.orange75.WithAlpha(0.1f);
				num = 1f - this.turret.currentReloadDelay / this.turret.reloadDelay;
			}
			else
			{
				color = ColorHelper.boringGrey.WithAlpha(0.1f);
				num = (float)this.turret.currentMagSize / (float)this.turret.maxMagSize;
			}
			if (!Mathf.Approximately(this.previousFillAmount, num))
			{
				this.fillImage.fillAmount = num;
				this.previousFillAmount = num;
			}
			if (this.previousColor != color)
			{
				this.fillImage.color = color;
				this.previousColor = color;
			}
			if (this.previousAmmoText != text)
			{
				this.ammoInMag.text = text;
				this.previousAmmoText = text;
				UITooltip.Refresh(this.tooltip);
			}
			if (this.turret.active != this.lastActiveState)
			{
				this.lastActiveState = this.turret.active;
				this.borderImage.color = (this.turret.active ? ColorHelper.greenish : ColorHelper.reddish);
				this.parent.CheckTurretStatus();
			}
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0009584C File Offset: 0x00093A4C
		public void SetTurret(AbstractTurret turret, TurretControl parent)
		{
			this.turret = turret;
			this.image.sprite = turret.item.icon;
			this.parent = parent;
			this.ammoInMag.text = turret.currentMagSize.ToString();
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00095898 File Offset: 0x00093A98
		public void PressButton()
		{
			float num = 1f;
			if (SkilltreeNode.combatFastReloadManual.isActive)
			{
				num += SkilltreeNode.combatFastReloadManual.currentIncrease;
			}
			this.turret.Reload(false, false, num);
			UITooltip.Refresh(this.tooltip);
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000958E0 File Offset: 0x00093AE0
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.turret.item.displayName, 14, 8f);
			string str = this.turret.HasAmmoType() ? this.turret.AmmoAmountAvailable().ToString() : "-";
			tooltip.AddTextLine(this.turret.currentMagSize.ToString() + " / " + str, 12, 8f).Text.color = ColorHelper.offWhite;
			if (this.turret.isReloading)
			{
				tooltip.AddTextLine(Translation.Translate("@TCReloading", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
			}
			else if (!this.turret.isReloading && this.turret.HasAmmo() && this.turret.currentMagSize != this.turret.maxMagSize)
			{
				tooltip.AddTextLine(Translation.Translate("@TCClickToReload", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.orange75;
			}
			tooltip.AddTextLine(Translation.Translate("@TCReloadTime", new object[]
			{
				GameMath.FormatNumber(this.turret.reloadDelay, 2)
			}), 12, 8f);
			string str2 = Translation.Translate("@None", Array.Empty<object>()).HighlightWithColor(ColorHelper.boringGrey);
			if (this.turret.targetingModule.priorityTarget != null)
			{
				str2 = Translation.Translate(this.turret.targetingModule.priorityTarget.targetName, Array.Empty<object>()).HighlightWithColor(ColorHelper.red90);
			}
			tooltip.AddTextLine(Translation.Translate("Target", Array.Empty<object>()) + ": " + str2, 12, 8f);
			bool active = this.turret.active;
			string text = this.turret.active ? "Active".HighlightWithColor(ColorHelper.greenish) : "Inactive".HighlightWithColor(ColorHelper.reddish);
			string text2 = (!this.turret.active) ? "activate" : "deactivate";
			tooltip.AddTextLine(text, 12, 8f);
			if (Singleton<TravelManager>.Instance.emergencyJumpActive)
			{
				tooltip.AddTextLine(Translation.Translate("@TCTravelControlsDisabled", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.red90;
				return;
			}
			string text3 = Translation.TranslateOnly("@RCTurretToggleActive", new object[]
			{
				text2
			});
			tooltip.AddTextLine(text3 ?? "", 12, 8f);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00095B86 File Offset: 0x00093D86
		public void OnPointerClick(PointerEventData eventData)
		{
			if (Singleton<TravelManager>.Instance.emergencyJumpActive)
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				this.turret.ToggleActive();
				this.parent.CheckTurretStatus();
				this.turret.ChangeRangeIndicatorColor();
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00095BBF File Offset: 0x00093DBF
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.turret.ShowRange();
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00095BCC File Offset: 0x00093DCC
		public void OnPointerExit(PointerEventData eventData)
		{
			this.turret.HideRange();
		}

		// Token: 0x04000EC6 RID: 3782
		[SerializeField]
		private Image borderImage;

		// Token: 0x04000EC7 RID: 3783
		[SerializeField]
		private Image image;

		// Token: 0x04000EC8 RID: 3784
		[SerializeField]
		private TextMeshProUGUI timer;

		// Token: 0x04000EC9 RID: 3785
		[SerializeField]
		private Image fillImage;

		// Token: 0x04000ECA RID: 3786
		[SerializeField]
		private TextMeshProUGUI ammoInMag;

		// Token: 0x04000ECB RID: 3787
		[SerializeField]
		private TextMeshProUGUI maxAmmoAvailable;

		// Token: 0x04000ECC RID: 3788
		private AbstractTurret turret;

		// Token: 0x04000ECD RID: 3789
		private TurretControl parent;

		// Token: 0x04000ECE RID: 3790
		private TooltipSource tooltip;

		// Token: 0x04000ECF RID: 3791
		private Color previousColor;

		// Token: 0x04000ED0 RID: 3792
		private float previousFillAmount = -1f;

		// Token: 0x04000ED1 RID: 3793
		private string previousAmmoText = "";

		// Token: 0x04000ED2 RID: 3794
		private bool lastActiveState;
	}
}
