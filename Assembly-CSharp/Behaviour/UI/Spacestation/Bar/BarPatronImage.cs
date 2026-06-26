using System;
using Behaviour.UI.Tooltip;
using Source.Galaxy.POI.Station;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Bar
{
	// Token: 0x02000239 RID: 569
	public class BarPatronImage : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITooltipTitleSource, ITooltipCustomSource, IPointerClickHandler
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x0008911E File Offset: 0x0008731E
		private void Start()
		{
			this.defaultColor = this.image.color;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00089131 File Offset: 0x00087331
		public void SetPatronData(BarPatron patron)
		{
			this.patron = patron;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0008913C File Offset: 0x0008733C
		public void SetPatronSprite(BarPatronSprite spriteData, Image background)
		{
			float width = background.sprite.rect.width;
			float height = background.sprite.rect.height;
			float num = width / height;
			float width2 = background.rectTransform.rect.width;
			float height2 = background.rectTransform.rect.height;
			float num2;
			if (width2 / height2 < num)
			{
				num2 = width2 / width;
			}
			else
			{
				num2 = height2 / height;
			}
			this.image.sprite = spriteData.image;
			this.image.rectTransform.anchoredPosition = new Vector2((float)spriteData.offset.x * num2, (float)spriteData.offset.y * num2);
			this.image.rectTransform.sizeDelta = new Vector2(spriteData.image.rect.width * num2, spriteData.image.rect.height * num2);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0008923F File Offset: 0x0008743F
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.image.color = Color.white;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00089251 File Offset: 0x00087451
		public void OnPointerExit(PointerEventData eventData)
		{
			this.image.color = this.defaultColor;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00089264 File Offset: 0x00087464
		public string GetTooltipTitle()
		{
			return this.patron.name;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00089274 File Offset: 0x00087474
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			this.patron.AddTooltipContent(tooltip);
			if (this.patron.icon)
			{
				Image image = UnityEngine.Object.Instantiate<Image>(this.tooltipIcon, tooltip.transform);
				image.transform.GetChild(0).GetComponent<Image>().sprite = this.patron.icon;
				image.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x000892DC File Offset: 0x000874DC
		public void OnPointerClick(PointerEventData eventData)
		{
			base.GetComponentInParent<BarUI>().PatronClicked(this.patron);
		}

		// Token: 0x04000C92 RID: 3218
		public const float unscaledWidth = 871f;

		// Token: 0x04000C93 RID: 3219
		[SerializeField]
		private Image image;

		// Token: 0x04000C94 RID: 3220
		[SerializeField]
		private Image tooltipIcon;

		// Token: 0x04000C95 RID: 3221
		private Color defaultColor;

		// Token: 0x04000C96 RID: 3222
		private BarPatron patron;
	}
}
