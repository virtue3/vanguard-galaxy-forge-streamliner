using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behavior.UI.GalaxyMap
{
	// Token: 0x02000191 RID: 401
	public class MapButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06000E4D RID: 3661 RVA: 0x0006706B File Offset: 0x0006526B
		private void Start()
		{
			this.mapWidget = base.GetComponentInParent<MapWidget>();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00067079 File Offset: 0x00065279
		public void SetSelected(bool selected)
		{
			this.highlightImage.gameObject.SetActive(selected);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0006708C File Offset: 0x0006528C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.mapWidget.OnChangeMapZoomLevel(this.zoomLevel);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x000670A0 File Offset: 0x000652A0
		public float SetLabel(string name)
		{
			this.label.text = name;
			float num = Mathf.Max(180f, this.label.rectTransform.anchoredPosition.x + this.label.preferredWidth + 10f);
			RectTransform rectTransform = (RectTransform)base.transform;
			rectTransform.sizeDelta = new Vector2(num, rectTransform.sizeDelta.y);
			return num;
		}

		// Token: 0x0400080B RID: 2059
		[SerializeField]
		private Image buttonImage;

		// Token: 0x0400080C RID: 2060
		[SerializeField]
		protected Image highlightImage;

		// Token: 0x0400080D RID: 2061
		[SerializeField]
		private int zoomLevel;

		// Token: 0x0400080E RID: 2062
		[SerializeField]
		private TMP_Text label;

		// Token: 0x0400080F RID: 2063
		private MapWidget mapWidget;
	}
}
