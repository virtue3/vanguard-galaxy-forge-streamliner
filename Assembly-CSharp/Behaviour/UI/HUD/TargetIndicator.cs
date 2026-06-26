using System;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000286 RID: 646
	public class TargetIndicator : MonoBehaviour
	{
		// Token: 0x060017BF RID: 6079 RVA: 0x0009517B File Offset: 0x0009337B
		private void Awake()
		{
			this.canvas = base.GetComponentInParent<Canvas>();
			this.rectTransform = (base.transform as RectTransform);
			this.gameCamera = GameplayManager.Instance.cameraMovement.gameCamera;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000951B0 File Offset: 0x000933B0
		public void Show(TargetableUnit unit)
		{
			Vector2 sizeDelta = this.rectTransform.sizeDelta;
			sizeDelta.x = unit.GetBoundsX() * 30f;
			this.rectTransform.sizeDelta = sizeDelta;
			this.unit = unit;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000951EF File Offset: 0x000933EF
		public void Destroy()
		{
			if (base.gameObject)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0009520C File Offset: 0x0009340C
		private void LateUpdate()
		{
			if (!this.unit || this.unit == null)
			{
				return;
			}
			Vector2 v = this.unit.transform.position;
			v.y += this.unit.GetBoundsY();
			((RectTransform)base.transform).anchoredPosition = this.gameCamera.WorldToScreenPoint(v) / this.canvas.scaleFactor;
		}

		// Token: 0x04000EB8 RID: 3768
		private TargetableUnit unit;

		// Token: 0x04000EB9 RID: 3769
		private Canvas canvas;

		// Token: 0x04000EBA RID: 3770
		private RectTransform rectTransform;

		// Token: 0x04000EBB RID: 3771
		private Camera gameCamera;
	}
}
