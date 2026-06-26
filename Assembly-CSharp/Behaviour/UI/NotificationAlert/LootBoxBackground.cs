using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.NotificationAlert
{
	// Token: 0x02000250 RID: 592
	public class LootBoxBackground : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x060015DB RID: 5595 RVA: 0x0008B7B8 File Offset: 0x000899B8
		private void Awake()
		{
			this.colorId = Shader.PropertyToID("_Color");
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0008B7CA File Offset: 0x000899CA
		public void SetColor(Color color)
		{
			this.image.material.SetColor(this.colorId, color);
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0008B7E4 File Offset: 0x000899E4
		private void Update()
		{
			Color color = this.image.color;
			float num = 0.2f;
			if (this.mouseOver && this.alpha > 0.2f)
			{
				this.tweenIn = false;
			}
			else if (this.mouseOver && this.alpha < 0.05f)
			{
				this.tweenIn = true;
			}
			else if (!this.mouseOver)
			{
				this.tweenIn = false;
			}
			if (this.tweenIn)
			{
				this.alpha += num * Time.deltaTime;
			}
			else
			{
				this.alpha -= num * Time.deltaTime;
			}
			color.a = Mathf.Clamp(this.alpha, 0.05f, 0.2f);
			this.image.color = color;
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0008B8A8 File Offset: 0x00089AA8
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.alpha = this.image.color.a;
			this.mouseOver = true;
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0008B8C7 File Offset: 0x00089AC7
		public void OnPointerExit(PointerEventData eventData)
		{
			this.mouseOver = false;
		}

		// Token: 0x04000D18 RID: 3352
		private bool mouseOver;

		// Token: 0x04000D19 RID: 3353
		private float alpha;

		// Token: 0x04000D1A RID: 3354
		[SerializeField]
		private Image image;

		// Token: 0x04000D1B RID: 3355
		private bool tweenIn;

		// Token: 0x04000D1C RID: 3356
		private int colorId;
	}
}
