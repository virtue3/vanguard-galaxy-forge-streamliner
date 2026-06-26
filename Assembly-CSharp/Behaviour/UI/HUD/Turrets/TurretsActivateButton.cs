using System;
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
	// Token: 0x0200028D RID: 653
	public class TurretsActivateButton : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060017ED RID: 6125 RVA: 0x00095FE6 File Offset: 0x000941E6
		public void Init(TurretControl turretControl)
		{
			this.turretControl = turretControl;
			if (turretControl.NoTurrets())
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00096010 File Offset: 0x00094210
		public void SetColor(TurretStatus status)
		{
			switch (status)
			{
			case TurretStatus.On:
				this.borderImage.color = ColorHelper.greenish;
				this.label.color = ColorHelper.greenish;
				return;
			case TurretStatus.Off:
				this.borderImage.color = ColorHelper.boringGrey;
				this.label.color = ColorHelper.boringGrey;
				return;
			case TurretStatus.Mixed:
				this.borderImage.color = ColorHelper.boringGrey;
				this.label.color = ColorHelper.boringGrey;
				return;
			default:
				return;
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00096094 File Offset: 0x00094294
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (Singleton<TravelManager>.Instance.emergencyJumpActive)
			{
				tooltip.AddTextLine(Translation.Translate("@TCTravelControlsDisabled", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.red90;
				return;
			}
			string text = Translation.TranslateOnly("@TCActivate", Array.Empty<object>()).HighlightWithColor(ColorHelper.greenish);
			tooltip.AddTextLine(Translation.TranslateOnly("@TCTurretsActivate", new object[]
			{
				text
			}), 12, 8f);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00096115 File Offset: 0x00094315
		public void OnPointerClick(PointerEventData eventData)
		{
			if (Singleton<TravelManager>.Instance.emergencyJumpActive)
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.turretControl.ActivateTurrets();
			}
		}

		// Token: 0x04000EE4 RID: 3812
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000EE5 RID: 3813
		[SerializeField]
		private Image borderImage;

		// Token: 0x04000EE6 RID: 3814
		private TurretControl turretControl;
	}
}
