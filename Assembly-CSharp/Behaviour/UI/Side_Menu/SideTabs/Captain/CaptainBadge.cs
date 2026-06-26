using System;
using Behaviour.Crew;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs.Captain
{
	// Token: 0x020002C2 RID: 706
	public class CaptainBadge : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ITooltipCustomSource
	{
		// Token: 0x060019EA RID: 6634 RVA: 0x000A1693 File Offset: 0x0009F893
		private void Start()
		{
			this.commander = GamePlayer.current.commander;
			this.Refresh();
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x000A16AC File Offset: 0x0009F8AC
		public void Refresh()
		{
			this.captainSprite.sprite = this.commander.sprite;
			this.captainName.text = this.commander.GetFullName();
			this.captainLevel.text = string.Format("{0} {1}", Translation.Translate("@SPLevel", Array.Empty<object>()), this.commander.level);
			bool flag = !string.IsNullOrEmpty(this.commander.selectedTitle);
			this.captainTitle.gameObject.SetActive(flag);
			if (flag)
			{
				this.captainTitle.text = this.commander.selectedTitle;
				this.captainTitle.color = this.commander.selectedTitleColor;
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x000A176D File Offset: 0x0009F96D
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Right)
			{
				return;
			}
			UnityEngine.Object.Instantiate<CaptainCustomizePopup>(this.customizePopupPrefab, UITooltip.tooltipParent).Open(this.commander, delegate(string firstName, string callsign, string lastName, CrewIcon icon, string title, Color color)
			{
				this.commander.SetName(firstName, callsign, lastName);
				this.commander.SetIcon(icon);
				this.commander.SetTitle(title, color);
				this.Refresh();
			});
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x000A17A0 File Offset: 0x0009F9A0
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@UILeftClickToChange", Array.Empty<object>()), 12, 8f);
		}

		// Token: 0x0400104D RID: 4173
		[SerializeField]
		private Image captainSprite;

		// Token: 0x0400104E RID: 4174
		[SerializeField]
		private TextMeshProUGUI captainTitle;

		// Token: 0x0400104F RID: 4175
		[SerializeField]
		private TextMeshProUGUI captainName;

		// Token: 0x04001050 RID: 4176
		[SerializeField]
		private TextMeshProUGUI captainLevel;

		// Token: 0x04001051 RID: 4177
		[SerializeField]
		private CaptainCustomizePopup customizePopupPrefab;

		// Token: 0x04001052 RID: 4178
		private CommanderData commander;
	}
}
