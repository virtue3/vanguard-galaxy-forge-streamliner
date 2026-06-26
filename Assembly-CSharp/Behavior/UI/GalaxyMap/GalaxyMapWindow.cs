using System;
using Behaviour.GalaxyMap;
using Behaviour.Transparency;
using Behaviour.UI;
using Source.Player;
using UnityEngine;

namespace Behavior.UI.GalaxyMap
{
	// Token: 0x02000190 RID: 400
	public class GalaxyMapWindow : MonoBehaviour, IDraggableWindow
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x00066F3B File Offset: 0x0006513B
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.draggableWindowBar.SetDraggableWindow(this);
			ScreenSettings.CheckMapSize();
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00066F5F File Offset: 0x0006515F
		public float GetDefaultWidth()
		{
			return ScreenSettings.maxMapWidth;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00066F66 File Offset: 0x00065166
		public float GetDefaultHeight()
		{
			return ScreenSettings.maxMapHeight;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00066F6D File Offset: 0x0006516D
		public RectTransform GetRectTransform()
		{
			return base.transform as RectTransform;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00066F7A File Offset: 0x0006517A
		public void UpdateAnchoredPosition(Vector2 pos)
		{
			GameplayerPrefs.SetGalaxyMapPosition(pos);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00066F82 File Offset: 0x00065182
		public Vector2 GetAnchoredPosition()
		{
			return GameplayerPrefs.GetGalaxyMapPosition();
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00066F89 File Offset: 0x00065189
		public void OnStartResize()
		{
			AbstractGalaxyMapManager.current.StartResize();
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00066F98 File Offset: 0x00065198
		public void UpdateSize(Vector2 size)
		{
			size.x = Mathf.Clamp(size.x, this.minWidth, size.x);
			size.y = Mathf.Clamp(size.y, this.minHeight, size.y);
			this.rectTransform.sizeDelta = size;
			this.mapInteraction.OnUpdateSize(size);
			float num = size.y / this.GetDefaultHeight();
			float num2 = size.x / this.GetDefaultWidth();
			AbstractGalaxyMapManager.current.SetAspect(size.x / size.y, (num2 > num) ? num2 : num);
			GameplayerPrefs.SetGalaxyMapSize(size);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0006703A File Offset: 0x0006523A
		public void UpdateScale(float scale)
		{
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0006703C File Offset: 0x0006523C
		public float GetScale()
		{
			return 1f;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00067043 File Offset: 0x00065243
		public Vector2 GetSize()
		{
			return GameplayerPrefs.GetGalaxyMapSize();
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0006704A File Offset: 0x0006524A
		public bool IsScalable()
		{
			return false;
		}

		// Token: 0x04000806 RID: 2054
		[SerializeField]
		private DraggableWindowBar draggableWindowBar;

		// Token: 0x04000807 RID: 2055
		[SerializeField]
		private MapInteraction mapInteraction;

		// Token: 0x04000808 RID: 2056
		[SerializeField]
		private float minHeight = 100f;

		// Token: 0x04000809 RID: 2057
		[SerializeField]
		private float minWidth = 100f;

		// Token: 0x0400080A RID: 2058
		private RectTransform rectTransform;
	}
}
