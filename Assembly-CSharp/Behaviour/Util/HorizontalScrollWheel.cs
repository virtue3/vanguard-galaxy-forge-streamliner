using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.Util
{
	// Token: 0x020001AF RID: 431
	[RequireComponent(typeof(ScrollRect))]
	public class HorizontalScrollWheel : MonoBehaviour, IScrollHandler, IEventSystemHandler
	{
		// Token: 0x06000F33 RID: 3891 RVA: 0x00069E41 File Offset: 0x00068041
		private void Awake()
		{
			this._scrollRect = base.GetComponent<ScrollRect>();
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00069E50 File Offset: 0x00068050
		public void OnScroll(PointerEventData eventData)
		{
			if (this._scrollRect.horizontal && !this._scrollRect.vertical)
			{
				this._scrollRect.horizontalNormalizedPosition -= eventData.scrollDelta.y * (0.1f / Mathf.Max(1f, this._scrollRect.content.rect.width - this._scrollRect.viewport.rect.width));
				this._scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(this._scrollRect.horizontalNormalizedPosition);
			}
		}

		// Token: 0x04000896 RID: 2198
		private ScrollRect _scrollRect;
	}
}
