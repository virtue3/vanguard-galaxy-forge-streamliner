using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.ShipCarousel
{
	// Token: 0x0200023E RID: 574
	public class Badge : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x00089899 File Offset: 0x00087A99
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x000898A1 File Offset: 0x00087AA1
		public BonusType bonusType { get; private set; }

		// Token: 0x0600155E RID: 5470 RVA: 0x000898AA File Offset: 0x00087AAA
		public void SetBadgeData(SpaceShip ship, BonusType bonusType, List<string> bonus)
		{
			this.ship = ship;
			this.bonusType = bonusType;
			this.bonus = bonus;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x000898C4 File Offset: 0x00087AC4
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate(string.Format("@{0}", this.bonusType), new object[]
			{
				this.ship.shipRoleType.GetTypeName()
			}), 12, 8f).Text.color = this.headerColor;
			foreach (string text in this.bonus)
			{
				tooltip.AddTextLine(text, 12, 8f).Text.color = ColorHelper.greenish;
			}
		}

		// Token: 0x04000CB3 RID: 3251
		private List<string> bonus;

		// Token: 0x04000CB4 RID: 3252
		private SpaceShip ship;

		// Token: 0x04000CB5 RID: 3253
		[SerializeField]
		private Color headerColor = ColorHelper.detailsColor;
	}
}
