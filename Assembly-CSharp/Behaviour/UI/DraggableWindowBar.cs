using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI
{
	// Token: 0x020001E4 RID: 484
	public class DraggableWindowBar : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06001266 RID: 4710 RVA: 0x00078E30 File Offset: 0x00077030
		public void SetDraggableWindow(IDraggableWindow draggableWindow)
		{
			this.draggableWindow = draggableWindow;
			this.rectTransform = draggableWindow.GetRectTransform();
			this.canvas = base.GetComponentInParent<Canvas>();
			this.rectTransform.anchoredPosition = draggableWindow.GetAnchoredPosition();
			this.rectTransform.localScale = new Vector3(draggableWindow.GetScale(), draggableWindow.GetScale(), 1f);
			if (!draggableWindow.IsScalable())
			{
				draggableWindow.UpdateSize(draggableWindow.GetSize());
			}
			if (!this.IsFullyVisible())
			{
				this.ClampToScreen();
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00078EB0 File Offset: 0x000770B0
		public void OnPointerDown(PointerEventData eventData)
		{
			this.rectTransform.SetAsLastSibling();
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00078EC0 File Offset: 0x000770C0
		public void OnDrag(PointerEventData eventData)
		{
			this.rectTransform.anchoredPosition += eventData.delta / this.canvas.scaleFactor;
			if (!this.IsFullyVisible())
			{
				this.ClampToScreen();
				return;
			}
			this.draggableWindow.UpdateAnchoredPosition(this.rectTransform.anchoredPosition);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00078F1E File Offset: 0x0007711E
		public bool IsFullyVisible()
		{
			return this.GetWindowOffset() == Vector2.zero;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00078F30 File Offset: 0x00077130
		public void ClampToScreen()
		{
			Vector2 windowOffset = this.GetWindowOffset();
			if (windowOffset != Vector2.zero)
			{
				Vector2 b = this.WorldToAnchoredOffset(windowOffset);
				this.rectTransform.anchoredPosition += b;
				this.draggableWindow.UpdateAnchoredPosition(this.rectTransform.anchoredPosition);
				if (!this.draggableWindow.IsScalable())
				{
					Vector2 b2 = new Vector2(Mathf.Clamp(windowOffset.x, windowOffset.x, 0f), Mathf.Clamp(windowOffset.y, windowOffset.y, 0f));
					Vector2 size = this.draggableWindow.GetSize() + b2;
					this.draggableWindow.UpdateSize(size);
				}
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00078FEC File Offset: 0x000771EC
		private Vector2 GetWindowOffset()
		{
			RectTransform component = this.canvas.GetComponent<RectTransform>();
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			this.rectTransform.GetWorldCorners(array);
			component.GetWorldCorners(array2);
			float x = array2[0].x;
			float x2 = array2[2].x;
			float y = array2[0].y;
			float y2 = array2[2].y;
			float x3 = array[0].x;
			float x4 = array[2].x;
			float y3 = array[0].y;
			float y4 = array[2].y;
			Vector2 zero = Vector2.zero;
			if (x3 < x)
			{
				zero.x += x - x3;
			}
			else if (x4 > x2)
			{
				zero.x += x2 - x4;
			}
			if (y3 < y)
			{
				zero.y += y - y3;
			}
			else if (y4 > y2)
			{
				zero.y += y2 - y4;
			}
			return zero;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x000790F4 File Offset: 0x000772F4
		private Vector2 WorldToAnchoredOffset(Vector3 worldOffset)
		{
			Vector3 vector = (this.rectTransform.parent as RectTransform).InverseTransformVector(worldOffset);
			return new Vector2(vector.x, vector.y);
		}

		// Token: 0x04000A3B RID: 2619
		public IDraggableWindow draggableWindow;

		// Token: 0x04000A3C RID: 2620
		private RectTransform rectTransform;

		// Token: 0x04000A3D RID: 2621
		private Canvas canvas;
	}
}
