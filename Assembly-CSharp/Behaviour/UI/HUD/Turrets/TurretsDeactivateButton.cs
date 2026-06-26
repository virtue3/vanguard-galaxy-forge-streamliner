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
	// Token: 0x0200028E RID: 654
	public class TurretsDeactivateButton : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x0009613F File Offset: 0x0009433F
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

		// Token: 0x060017F3 RID: 6131 RVA: 0x0009616C File Offset: 0x0009436C
		public void SetColor(TurretStatus status)
		{
			switch (status)
			{
			case TurretStatus.On:
				this.borderImage.color = ColorHelper.boringGrey;
				this.label.color = ColorHelper.boringGrey;
				return;
			case TurretStatus.Off:
				this.borderImage.color = ColorHelper.reddish;
				this.label.color = ColorHelper.reddish;
				return;
			case TurretStatus.Mixed:
				this.borderImage.color = ColorHelper.boringGrey;
				this.label.color = ColorHelper.boringGrey;
				return;
			default:
				return;
			}
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000961F0 File Offset: 0x000943F0
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (Singleton<TravelManager>.Instance.emergencyJumpActive)
			{
				tooltip.AddTextLine(Translation.Translate("@TCTravelControlsDisabled", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.red90;
				return;
			}
			string text = Translation.TranslateOnly("@TCDeactivate", Array.Empty<object>()).HighlightWithColor(ColorHelper.reddish);
			tooltip.AddTextLine(Translation.TranslateOnly("@TCTurretsDeactivate", new object[]
			{
				text
			}), 12, 8f);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00096271 File Offset: 0x00094471
		public void OnPointerClick(PointerEventData eventData)
		{
			if (Singleton<TravelManager>.Instance.emergencyJumpActive)
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.turretControl.DeactivateTurrets();
			}
		}

		// Token: 0x04000EE7 RID: 3815
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000EE8 RID: 3816
		[SerializeField]
		private Image borderImage;

		// Token: 0x04000EE9 RID: 3817
		private TurretControl turretControl;
	}
}
