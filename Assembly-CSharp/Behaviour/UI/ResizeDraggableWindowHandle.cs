using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001E8 RID: 488
	public class ResizeDraggableWindowHandle : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
	{
		// Token: 0x0600128E RID: 4750 RVA: 0x00079690 File Offset: 0x00077890
		private void Awake()
		{
			this.rectTransform = this.draggableWindowBar.draggableWindow.GetRectTransform();
			this.canvas = base.GetComponentInParent<Canvas>();
			this.image = base.GetComponent<Image>();
			this.defaultColor = this.image.color;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x000796DC File Offset: 0x000778DC
		public void OnDrag(PointerEventData eventData)
		{
			Vector2 vector = eventData.delta / this.canvas.scaleFactor;
			Vector2 zero = Vector2.zero;
			bool flag = false;
			bool flag2 = false;
			switch (this.direction)
			{
			case ResizeDraggableWindowHandle.ResizeDirection.TopLeft:
				zero.x -= vector.x;
				zero.y += vector.y;
				flag = true;
				break;
			case ResizeDraggableWindowHandle.ResizeDirection.TopRight:
				zero.x += vector.x;
				zero.y += vector.y;
				break;
			case ResizeDraggableWindowHandle.ResizeDirection.BottomLeft:
				zero.x -= vector.x;
				zero.y -= vector.y;
				flag = true;
				flag2 = true;
				break;
			case ResizeDraggableWindowHandle.ResizeDirection.BottomRight:
				zero.x += vector.x;
				zero.y -= vector.y;
				flag2 = true;
				break;
			}
			if (this.scaleMode)
			{
				float num = this.draggableWindowBar.draggableWindow.GetScale();
				float num2;
				if (Mathf.Abs(zero.x) > Mathf.Abs(zero.y))
				{
					num2 = zero.x / this.draggableWindowBar.draggableWindow.GetDefaultWidth();
				}
				else
				{
					num2 = zero.y / this.draggableWindowBar.draggableWindow.GetDefaultHeight();
				}
				num += num2;
				num = Mathf.Clamp(num, this.minScale, this.maxScale);
				if (this.draggableWindowBar.IsFullyVisible() || num2 < 0f)
				{
					this.draggableWindowBar.draggableWindow.UpdateScale(num);
					this.rectTransform.localScale = new Vector3(num, num, 1f);
				}
			}
			else
			{
				Vector2 vector2 = this.draggableWindowBar.draggableWindow.GetSize();
				vector2 += zero;
				Vector2 anchoredPosition = this.draggableWindowBar.draggableWindow.GetAnchoredPosition();
				if (flag)
				{
					anchoredPosition.x -= zero.x;
				}
				if (flag2)
				{
					anchoredPosition.y -= zero.y;
				}
				if (!this.draggableWindowBar.IsFullyVisible())
				{
					this.draggableWindowBar.ClampToScreen();
				}
				else
				{
					this.rectTransform.anchoredPosition = anchoredPosition;
					this.draggableWindowBar.draggableWindow.UpdateAnchoredPosition(this.rectTransform.anchoredPosition);
					this.draggableWindowBar.draggableWindow.UpdateSize(vector2);
				}
			}
			this.draggableWindowBar.ClampToScreen();
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00079947 File Offset: 0x00077B47
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.image.color = this.hoverColor;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0007995A File Offset: 0x00077B5A
		public void OnPointerExit(PointerEventData eventData)
		{
			this.image.color = this.defaultColor;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0007996D File Offset: 0x00077B6D
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.draggableWindowBar.draggableWindow.OnStartResize();
		}

		// Token: 0x04000A5E RID: 2654
		public ResizeDraggableWindowHandle.ResizeDirection direction;

		// Token: 0x04000A5F RID: 2655
		public float minScale = 0.5f;

		// Token: 0x04000A60 RID: 2656
		public float maxScale = 1.5f;

		// Token: 0x04000A61 RID: 2657
		private Image image;

		// Token: 0x04000A62 RID: 2658
		private Color defaultColor;

		// Token: 0x04000A63 RID: 2659
		[SerializeField]
		private Color hoverColor;

		// Token: 0x04000A64 RID: 2660
		[SerializeField]
		private DraggableWindowBar draggableWindowBar;

		// Token: 0x04000A65 RID: 2661
		[SerializeField]
		private bool scaleMode = true;

		// Token: 0x04000A66 RID: 2662
		private RectTransform rectTransform;

		// Token: 0x04000A67 RID: 2663
		private Canvas canvas;

		// Token: 0x02000509 RID: 1289
		public enum ResizeDirection
		{
			// Token: 0x04001B07 RID: 6919
			TopLeft,
			// Token: 0x04001B08 RID: 6920
			TopRight,
			// Token: 0x04001B09 RID: 6921
			BottomLeft,
			// Token: 0x04001B0A RID: 6922
			BottomRight
		}
	}
}
