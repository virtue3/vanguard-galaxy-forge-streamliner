using System;
using Behaviour.Managers;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.Travel
{
	// Token: 0x020001FC RID: 508
	public class TravelDestinationInfo : MonoBehaviour, ITooltipCustomSource, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x0600131B RID: 4891 RVA: 0x0007D744 File Offset: 0x0007B944
		public void Init(MapPointOfInterest targetDestination)
		{
			this.targetDestination = targetDestination;
			this.destinationText.text = Translation.Translate("@TravelTarget", Array.Empty<object>()) + ": " + Translation.Translate(targetDestination.name, Array.Empty<object>());
			if (targetDestination != Singleton<TravelManager>.Current.localTarget)
			{
				this.currentDestinationText.gameObject.SetActive(true);
				TMP_Text tmp_Text = this.currentDestinationText;
				string str = Translation.Translate("@TravelDestination", Array.Empty<object>());
				string str2 = ": ";
				MapPointOfInterest localTarget = Singleton<TravelManager>.Current.localTarget;
				tmp_Text.text = str + str2 + ((localTarget != null) ? localTarget.name : null);
				return;
			}
			this.currentDestinationText.gameObject.SetActive(false);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0007D7F8 File Offset: 0x0007B9F8
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@TravelTarget", Array.Empty<object>()) + ": " + this.targetDestination.name.HighlightWithColor(ColorHelper.detailsColor), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@TravelDestinationType", Array.Empty<object>()) + ": " + Translation.Translate(this.targetDestination.typeName, Array.Empty<object>()).HighlightWithColor(ColorHelper.boringGrey), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@MapStaticSystem", Array.Empty<object>()) + ": " + this.targetDestination.system.name, 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@MapStaticSector", Array.Empty<object>()) + ": " + this.targetDestination.system.sector.name, 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddSeparator(null);
			int num = GamePlayer.current.waypoints.Count;
			num--;
			string text = (num > 0) ? num.ToString() : ("(" + Translation.Translate("@TravelAlreadyInSystem", Array.Empty<object>()) + ")");
			tooltip.AddTextLine(Translation.Translate("@TravelJumps", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.boringGrey), 12, 8f).Text.color = ColorHelper.offWhite;
			if (this.targetDestination != Singleton<TravelManager>.Current.localTarget)
			{
				string str = Translation.Translate("@TravelDestination", Array.Empty<object>());
				string str2 = ": ";
				MapPointOfInterest localTarget = Singleton<TravelManager>.Current.localTarget;
				tooltip.AddTextLine(str + str2 + ((localTarget != null) ? localTarget.name.HighlightWithColor(ColorHelper.lightCyan) : null), 12, 8f).Text.color = ColorHelper.offWhite;
			}
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@TravelLeftClickToHighlight", Array.Empty<object>()) ?? "", 12, 8f);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x0007DA5F File Offset: 0x0007BC5F
		public void OnPointerDown(PointerEventData eventData)
		{
			SidePanel.instance.ShowPoiOnMap(this.targetDestination);
		}

		// Token: 0x04000AC5 RID: 2757
		private MapPointOfInterest targetDestination;

		// Token: 0x04000AC6 RID: 2758
		[SerializeField]
		private TextMeshProUGUI destinationText;

		// Token: 0x04000AC7 RID: 2759
		[SerializeField]
		private TextMeshProUGUI currentDestinationText;
	}
}
