using System;
using Behaviour.Background;
using UnityEngine;
using UnityEngine.VFX;

// Token: 0x02000004 RID: 4
public class CameraTracker : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600001D RID: 29 RVA: 0x000029D6 File Offset: 0x00000BD6
	// (set) Token: 0x0600001E RID: 30 RVA: 0x000029DE File Offset: 0x00000BDE
	public Camera trackedCamera { get; private set; }

	// Token: 0x0600001F RID: 31 RVA: 0x000029E7 File Offset: 0x00000BE7
	private void Awake()
	{
		this.visualEffect = base.GetComponent<VisualEffect>();
		this.cameraTrackable = base.GetComponentInParent<ICameraTrackable>();
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002A01 File Offset: 0x00000C01
	public void SetCamera(Camera camera)
	{
		this.trackedCamera = camera;
		this.previousCameraPosition = this.trackedCamera.transform.position;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002A28 File Offset: 0x00000C28
	private void LateUpdate()
	{
		if (!this.trackedCamera || this.cameraTrackable == null)
		{
			return;
		}
		Vector2 vector = this.trackedCamera.transform.position;
		Vector2 vector2 = vector - this.previousCameraPosition;
		this.totalPositionChange += vector2;
		if (this.visualEffect)
		{
			this.visualEffect.SetVector2("CameraDirection", -(vector - this.previousCameraPosition));
		}
		ICameraTrackable cameraTrackable = this.cameraTrackable;
		if (cameraTrackable != null)
		{
			cameraTrackable.SetPositionDelta(vector2, vector);
		}
		if (this.cameraTrackable == null)
		{
			base.gameObject.transform.position = vector;
		}
		this.previousCameraPosition = vector;
	}

	// Token: 0x0400001C RID: 28
	private VisualEffect visualEffect;

	// Token: 0x0400001D RID: 29
	private Vector2 previousCameraPosition;

	// Token: 0x0400001E RID: 30
	private ICameraTrackable cameraTrackable;

	// Token: 0x0400001F RID: 31
	private Vector2 shaderOffset;

	// Token: 0x04000020 RID: 32
	private Vector2 mapOffset;

	// Token: 0x04000021 RID: 33
	private Vector2 localOffset;

	// Token: 0x04000022 RID: 34
	private Vector2 totalPositionChange = Vector2.zero;
}
