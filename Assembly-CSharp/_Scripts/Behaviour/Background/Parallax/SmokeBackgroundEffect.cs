using System;
using Behaviour.Background;
using Behaviour.Effects;
using Source.Galaxy;
using UnityEngine;

namespace _Scripts.Behaviour.Background.Parallax
{
	// Token: 0x0200019F RID: 415
	public class SmokeBackgroundEffect : AbstractEffect, ICameraTrackable
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000687B4 File Offset: 0x000669B4
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x000687BC File Offset: 0x000669BC
		public bool tileWarpEnabled { get; private set; } = true;

		// Token: 0x06000EA2 RID: 3746 RVA: 0x000687C5 File Offset: 0x000669C5
		protected override void Awake()
		{
			base.Awake();
			this.cameraTracker = base.GetComponent<CameraTracker>();
			this.cameraToTrack = Camera.main.GetComponent<CameraMovement>().gameCamera;
			this.cameraTracker.SetCamera(this.cameraToTrack);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x000687FF File Offset: 0x000669FF
		public void SetData(SmokeBackgroundEffectData data, Vector2 screenSize)
		{
			this.SetProperties(data.size, screenSize, data.colors, data.amount, data.distance, data.seed);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00068828 File Offset: 0x00066A28
		private void SetProperties(float size, Vector2 area, Gradient colors, int amount, float distance, float seed)
		{
			this.size = size;
			this.area = area * CameraMovement.maxZoom * 1.201f;
			this.colors = colors;
			this.amount = amount;
			this.distance = distance;
			this.seed = seed;
			base.visualEffect.SetFloat("Size", size);
			base.visualEffect.SetVector2("Area", area);
			base.visualEffect.SetGradient("Colors", colors);
			base.visualEffect.SetInt("Amount", amount);
			base.visualEffect.SetFloat("Distance", distance);
			base.visualEffect.SetFloat("Seed", seed);
			if (distance == 1f)
			{
				Renderer component = base.visualEffect.GetComponent<Renderer>();
				if (component)
				{
					component.sortingLayerName = "BackgroundLit";
				}
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00068907 File Offset: 0x00066B07
		public void SetTileWarpEnabled(bool enabled)
		{
			this.tileWarpEnabled = enabled;
			base.visualEffect.SetBool("TileWarpEnabled", this.tileWarpEnabled);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00068928 File Offset: 0x00066B28
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			if (!this.cameraToTrack)
			{
				return;
			}
			if (this.distance == 1f)
			{
				return;
			}
			float d = (CameraMovement.maxZoom - this.cameraToTrack.orthographicSize) * 0.001f;
			Vector3 localScale = Vector3.one * this.cameraToTrack.orthographicSize / CameraMovement.maxZoom + Vector3.one * d;
			base.transform.localScale = localScale;
		}

		// Token: 0x0400083A RID: 2106
		[SerializeField]
		private float size;

		// Token: 0x0400083B RID: 2107
		[SerializeField]
		private Vector2 area;

		// Token: 0x0400083C RID: 2108
		[SerializeField]
		private Gradient colors;

		// Token: 0x0400083D RID: 2109
		[SerializeField]
		private float distance;

		// Token: 0x0400083E RID: 2110
		[SerializeField]
		private int amount;

		// Token: 0x0400083F RID: 2111
		[SerializeField]
		private float seed;

		// Token: 0x04000840 RID: 2112
		[SerializeField]
		private Vector2 cameraDirection;

		// Token: 0x04000842 RID: 2114
		private Camera cameraToTrack;

		// Token: 0x04000843 RID: 2115
		private CameraTracker cameraTracker;
	}
}
