using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Spacestation.Docking
{
	// Token: 0x020002E3 RID: 739
	public class DockingLight : MonoBehaviour
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x000A56D5 File Offset: 0x000A38D5
		private void Awake()
		{
			if (this.lightSource == null)
			{
				this.lightSource = base.GetComponentInChildren<Light2D>();
			}
			if (this.lightRenderer == null)
			{
				this.lightRenderer = base.GetComponentInChildren<SpriteRenderer>();
			}
			this.SetState(false);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x000A5714 File Offset: 0x000A3914
		public void SetState(bool state)
		{
			this.isOn = state;
			if (this.lightSource != null)
			{
				this.lightSource.enabled = this.isOn;
			}
			if (this.lightRenderer != null)
			{
				this.lightRenderer.material.color = (this.isOn ? this.activeColor : this.inactiveColor);
				this.lightSource.color = (this.isOn ? this.activeColor : this.inactiveColor);
			}
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000A579C File Offset: 0x000A399C
		public void Toggle()
		{
			this.SetState(!this.isOn);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000A57AD File Offset: 0x000A39AD
		public bool IsActive()
		{
			return this.isOn;
		}

		// Token: 0x040010F0 RID: 4336
		private Light2D lightSource;

		// Token: 0x040010F1 RID: 4337
		private SpriteRenderer lightRenderer;

		// Token: 0x040010F2 RID: 4338
		[SerializeField]
		private Color activeColor = Color.green;

		// Token: 0x040010F3 RID: 4339
		[SerializeField]
		private Color inactiveColor = Color.red;

		// Token: 0x040010F4 RID: 4340
		private bool isOn;
	}
}
