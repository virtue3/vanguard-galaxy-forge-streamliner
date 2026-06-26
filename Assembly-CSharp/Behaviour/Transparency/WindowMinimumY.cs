using System;
using Behaviour.Bootstrap;
using Behaviour.Util;
using Source.Util;
using UnityEngine;

namespace Behaviour.Transparency
{
	// Token: 0x020002D5 RID: 725
	public class WindowMinimumY : MonoBehaviour
	{
		// Token: 0x06001A83 RID: 6787 RVA: 0x000A4248 File Offset: 0x000A2448
		private void Start()
		{
			this.cameraComponent = base.GetComponent<Camera>();
			this.UpdateCameraRect();
			this.childrenToNotify = base.GetComponentsInChildren<INotifyScreenChange>();
			this.NotifyChildren();
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000A4270 File Offset: 0x000A2470
		public void Update()
		{
			bool flag = !ScreenSettings.minimumY.ApproximatelyEqual(this.currentY);
			bool flag2 = !this.GetScreenPercentage().ApproximatelyEqual(this.currentScreenPercentage);
			bool flag3 = this.currentWidth != Screen.width;
			this.currentWidth = Screen.width;
			if (PersistentSingleton<Bootstrapper>.Instance.activeWindow && (flag || flag2 || flag3))
			{
				this.UpdateCameraRect();
				this.NotifyChildren();
			}
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000A42E8 File Offset: 0x000A24E8
		private void NotifyChildren()
		{
			INotifyScreenChange[] array = this.childrenToNotify;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].NewScreenPercentage();
			}
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000A4314 File Offset: 0x000A2514
		private void UpdateCameraRect()
		{
			this.currentY = ScreenSettings.minimumY;
			this.currentScreenPercentage = this.GetScreenPercentage();
			float num = this.currentY / (float)Screen.height;
			if (this.cameraComponent)
			{
				this.cameraComponent.rect = new Rect(this.cameraComponent.rect.x, num, this.cameraComponent.rect.width, this.GetScreenPercentage());
				this.cameraComponent.ResetAspect();
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				if (this.stackUI && ScreenSettings.doWeStackUI)
				{
					rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, num + this.GetScreenPercentage());
					rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, this.GetScreenPercentage() * 2f + num);
					return;
				}
				rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, num);
				rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, this.GetScreenPercentage() + num);
			}
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x000A4435 File Offset: 0x000A2635
		private float GetScreenPercentage()
		{
			if (this.useGamePercentage)
			{
				return ScreenSettings.gameScreenPercentage;
			}
			if (!ScreenSettings.doWeStackUI)
			{
				return ScreenSettings.clickableScreenPercentage;
			}
			return ScreenSettings.clickableScreenPercentage / 2f;
		}

		// Token: 0x040010B2 RID: 4274
		[SerializeField]
		private bool useGamePercentage;

		// Token: 0x040010B3 RID: 4275
		[SerializeField]
		private bool stackUI;

		// Token: 0x040010B4 RID: 4276
		private float currentY;

		// Token: 0x040010B5 RID: 4277
		private float currentScreenPercentage;

		// Token: 0x040010B6 RID: 4278
		private int currentWidth;

		// Token: 0x040010B7 RID: 4279
		private Camera cameraComponent;

		// Token: 0x040010B8 RID: 4280
		private INotifyScreenChange[] childrenToNotify;
	}
}
