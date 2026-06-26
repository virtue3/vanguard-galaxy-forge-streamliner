using System;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station;
using Source.Galaxy.POI.Station.Patrons;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Bar
{
	// Token: 0x0200023D RID: 573
	public class ItemSaleInfo : MonoBehaviour
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x00089668 File Offset: 0x00087868
		public void Show(Salesman salesman)
		{
			this.salesmanData = salesman;
			this.salesmanName.text = salesman.name;
			this.salesmanDesc.text = salesman.description;
			this.salesmanIcon.sprite = salesman.icon;
			this.itemName.TL(salesman.itemForSale.displayName, Array.Empty<object>());
			this.itemName.color = salesman.itemForSale.rarity.GetColor();
			this.itemName.GetComponent<ItemTooltipSource>().SetItem(salesman.itemForSale, 1, true, ItemTooltipContext.InCarousel, false, null);
			this.itemIcon.sprite = salesman.itemForSale.icon;
			this.itemIcon.GetComponent<ItemTooltipSource>().SetItem(salesman.itemForSale, 1, true, ItemTooltipContext.InCarousel, false, null);
			this.itemDescription.TL(salesman.itemForSale.description, Array.Empty<object>());
			this.itemDescription.color = CompareTooltip.detailsColor;
			this.costLabel.TL("@UIBarItemCost", new object[]
			{
				salesman.itemCost
			});
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0008978C File Offset: 0x0008798C
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0008979A File Offset: 0x0008799A
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000897A8 File Offset: 0x000879A8
		public void ButtonPurchase()
		{
			if (!GamePlayer.current.CanAfford((float)this.salesmanData.itemCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (this.trigger != null)
			{
				MissionObjective.Trigger(this.trigger.Value, 1, null, false);
			}
			GamePlayer.current.RemoveCredits((float)this.salesmanData.itemCost);
			GamePlayer.current.currentSpaceShip.cargo.Add(this.salesmanData.itemForSale, 1, false, false);
			SpaceStation current = SpaceStation.current;
			if (current != null)
			{
				Bar bar = current.bar;
				if (bar != null)
				{
					bar.availablePatrons.Remove(this.salesmanData);
				}
			}
			BarUI barUI;
			if (this.TryGetComponentInParent(out barUI))
			{
				barUI.RefreshPatrons();
			}
			this.Destroy();
		}

		// Token: 0x04000CA9 RID: 3241
		[SerializeField]
		private TMP_Text salesmanName;

		// Token: 0x04000CAA RID: 3242
		[SerializeField]
		private TMP_Text salesmanDesc;

		// Token: 0x04000CAB RID: 3243
		[SerializeField]
		private Image salesmanIcon;

		// Token: 0x04000CAC RID: 3244
		[SerializeField]
		private TMP_Text itemName;

		// Token: 0x04000CAD RID: 3245
		[SerializeField]
		private TMP_Text itemDescription;

		// Token: 0x04000CAE RID: 3246
		[SerializeField]
		private Image itemIcon;

		// Token: 0x04000CAF RID: 3247
		[SerializeField]
		private TMP_Text costLabel;

		// Token: 0x04000CB0 RID: 3248
		public MissionTrigger? trigger;

		// Token: 0x04000CB1 RID: 3249
		private Salesman salesmanData;
	}
}
