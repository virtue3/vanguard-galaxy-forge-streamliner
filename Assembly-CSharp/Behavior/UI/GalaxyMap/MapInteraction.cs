using System;
using Source.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behavior.UI.GalaxyMap
{
	// Token: 0x02000192 RID: 402
	public class MapInteraction : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerMoveHandler
	{
		// Token: 0x06000E52 RID: 3666 RVA: 0x00067118 File Offset: 0x00065318
		private void Awake()
		{
			RenderTexture renderTexture = (RenderTexture)this.rawImage.texture;
			RenderTexture renderTexture2 = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
			renderTexture2.Create();
			Graphics.Blit(renderTexture, renderTexture2);
			this.rawImage.texture = renderTexture2;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00067170 File Offset: 0x00065370
		public void OnUpdateSize(Vector2 size)
		{
			size *= GameplayerPrefs.GetScaleFactor();
			this.renderTexture = (this.rawImage.texture as RenderTexture);
			if (this.renderTexture != null)
			{
				this.renderTexture.Release();
				this.renderTexture.width = (int)size.x;
				this.renderTexture.height = (int)size.y;
				this.renderTexture.Create();
				this.targetCamera.targetTexture = this.renderTexture;
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000671FC File Offset: 0x000653FC
		public void OnPointerClick(PointerEventData eventData)
		{
			GameObject gameObject = this.Raycast2D(eventData);
			if (gameObject == null)
			{
				return;
			}
			ExecuteEvents.Execute<IPointerClickHandler>(gameObject, eventData, ExecuteEvents.pointerClickHandler);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00067228 File Offset: 0x00065428
		public void OnPointerMove(PointerEventData eventData)
		{
			GameObject x = this.Raycast2D(eventData);
			if (x != this.currentHover)
			{
				if (this.currentHover != null)
				{
					ExecuteEvents.Execute<IPointerExitHandler>(this.currentHover, eventData, ExecuteEvents.pointerExitHandler);
				}
				this.currentHover = x;
				if (this.currentHover != null)
				{
					ExecuteEvents.Execute<IPointerEnterHandler>(this.currentHover, eventData, ExecuteEvents.pointerEnterHandler);
				}
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00067292 File Offset: 0x00065492
		private void OnDestroy()
		{
			this.renderTexture.Release();
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x000672A0 File Offset: 0x000654A0
		private GameObject Raycast2D(PointerEventData eventData)
		{
			RectTransform rectTransform = this.rawImage.rectTransform;
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out vector))
			{
				return null;
			}
			Vector2 vector2 = new Vector2((vector.x - rectTransform.rect.x) / rectTransform.rect.width, (vector.y - rectTransform.rect.y) / rectTransform.rect.height);
			Vector3 position = new Vector3(vector2.x * (float)this.targetCamera.pixelWidth, vector2.y * (float)this.targetCamera.pixelHeight, 0f);
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.targetCamera.ScreenToWorldPoint(position), Vector2.zero);
			if (!raycastHit2D.collider)
			{
				return null;
			}
			return raycastHit2D.collider.gameObject;
		}

		// Token: 0x04000810 RID: 2064
		public Camera targetCamera;

		// Token: 0x04000811 RID: 2065
		public RawImage rawImage;

		// Token: 0x04000812 RID: 2066
		private RenderTexture renderTexture;

		// Token: 0x04000813 RID: 2067
		private GameObject currentHover;
	}
}
