using System;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001E7 RID: 487
	public interface IDraggableWindow
	{
		// Token: 0x06001283 RID: 4739
		float GetDefaultWidth();

		// Token: 0x06001284 RID: 4740
		float GetDefaultHeight();

		// Token: 0x06001285 RID: 4741
		RectTransform GetRectTransform();

		// Token: 0x06001286 RID: 4742
		void UpdateAnchoredPosition(Vector2 pos);

		// Token: 0x06001287 RID: 4743
		void UpdateScale(float scale);

		// Token: 0x06001288 RID: 4744
		void UpdateSize(Vector2 size);

		// Token: 0x06001289 RID: 4745
		void OnStartResize();

		// Token: 0x0600128A RID: 4746
		Vector2 GetAnchoredPosition();

		// Token: 0x0600128B RID: 4747
		float GetScale();

		// Token: 0x0600128C RID: 4748
		Vector2 GetSize();

		// Token: 0x0600128D RID: 4749
		bool IsScalable();
	}
}
