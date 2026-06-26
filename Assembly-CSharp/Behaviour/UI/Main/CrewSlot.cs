using System;
using Behaviour.Crew;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000267 RID: 615
	public class CrewSlot : MonoBehaviour, ITooltipCustomSource, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x0008F1CD File Offset: 0x0008D3CD
		public void Init(CrewView crewView, Behaviour.Crew.Crew crew = null)
		{
			this.crewView = crewView;
			if (crew != null)
			{
				this.crew = crew;
				this.image.sprite = crew.icon;
				return;
			}
			this.image.sprite = this.empty;
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0008F20C File Offset: 0x0008D40C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.crew == null)
			{
				tooltip.AddTextLine("No crew assigned to this escape pod.", 12, 8f);
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@" + this.crew.identifier, Array.Empty<object>()), 12, 8f).Text.color = this.crew.rarity.GetColor();
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(this.crew.GetBonus(), 12, 8f);
			tooltip.AddTextLine("Left-Click to jettison LOLOL", 12, 8f);
			tooltip.AddTextLine("Right-Click to stow in cargo (3m3)", 12, 8f);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0008F2D0 File Offset: 0x0008D4D0
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.crew == null)
			{
				return;
			}
			if (SidePanel.instance == null)
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.crewView.JettisonCrew(this.crew, 1);
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				this.crewView.AddToCargo(this.crew, 1);
			}
		}

		// Token: 0x04000DC7 RID: 3527
		[SerializeField]
		private Image image;

		// Token: 0x04000DC8 RID: 3528
		[SerializeField]
		private Sprite empty;

		// Token: 0x04000DC9 RID: 3529
		private Behaviour.Crew.Crew crew;

		// Token: 0x04000DCA RID: 3530
		private CrewView crewView;
	}
}
