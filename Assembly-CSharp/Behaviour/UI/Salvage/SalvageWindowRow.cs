using System;
using Behaviour.Item;
using Behaviour.UI.Tooltip;
using Source.Data.Persistable;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Salvage
{
	// Token: 0x02000249 RID: 585
	public class SalvageWindowRow : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0008AA59 File Offset: 0x00088C59
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x0008AA61 File Offset: 0x00088C61
		public SalvageItemData contained { get; private set; }

		// Token: 0x060015AA RID: 5546 RVA: 0x0008AA6C File Offset: 0x00088C6C
		public void SetItem(SalvageItemData data, float chance)
		{
			this.contained = data;
			this.icon.sprite = data.item.icon;
			this.label.TL(data.item.displayName, Array.Empty<object>());
			this.label.color = data.item.rarity.GetColor();
			this.chance.text = GameMath.FormatPercentage(chance, FormatPercentageMode.Default, 1);
			base.GetComponent<ItemTooltipSource>().SetItem(data.item, 1, true, ItemTooltipContext.InSalvage, false, null);
			this.OnPointerExit(null);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0008AB00 File Offset: 0x00088D00
		public void SetScrap(InventoryItemType scrap)
		{
			if (scrap != null)
			{
				this.icon.sprite = scrap.icon;
				this.label.TL("@SalvageScrapOnly", Array.Empty<object>());
			}
			else
			{
				this.icon.gameObject.SetActive(false);
				this.label.TL("@SalvageIgnore", Array.Empty<object>());
			}
			this.chance.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.GetComponent<ItemTooltipSource>());
			this.OnPointerExit(null);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0008AB87 File Offset: 0x00088D87
		public void OnPointerClick(PointerEventData eventData)
		{
			SalvageWindow componentInParent = base.GetComponentInParent<SalvageWindow>();
			SalvageItemData contained = this.contained;
			componentInParent.SetSelectedItem((contained != null) ? contained.item : null);
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0008ABA6 File Offset: 0x00088DA6
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.background.color = this.highlightColor;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0008ABB9 File Offset: 0x00088DB9
		public void OnPointerExit(PointerEventData eventData)
		{
			Graphic graphic = this.background;
			SalvageItemData contained = this.contained;
			graphic.color = ((contained != null && contained.active) ? this.selectedColor : this.defaultColor);
		}

		// Token: 0x04000CEB RID: 3307
		[SerializeField]
		private Image icon;

		// Token: 0x04000CEC RID: 3308
		[SerializeField]
		private Image background;

		// Token: 0x04000CED RID: 3309
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000CEE RID: 3310
		[SerializeField]
		private TMP_Text chance;

		// Token: 0x04000CEF RID: 3311
		private Color highlightColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);

		// Token: 0x04000CF0 RID: 3312
		private Color selectedColor = new Color(0f, 0.3f, 0f, 0.9f);

		// Token: 0x04000CF1 RID: 3313
		private Color defaultColor = new Color(0f, 0f, 0f, 0.9f);
	}
}
