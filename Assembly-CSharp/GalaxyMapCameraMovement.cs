using System;
using System.Collections.Generic;
using Behaviour.Bootstrap;
using Behaviour.GalaxyMap;
using Behaviour.Transparency;
using Behaviour.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Token: 0x02000005 RID: 5
public class GalaxyMapCameraMovement : MonoBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000023 RID: 35 RVA: 0x00002AF9 File Offset: 0x00000CF9
	// (set) Token: 0x06000024 RID: 36 RVA: 0x00002B01 File Offset: 0x00000D01
	public bool followTarget { get; private set; }

	// Token: 0x06000025 RID: 37 RVA: 0x00002B0A File Offset: 0x00000D0A
	private void Awake()
	{
		this.mapCamera = base.GetComponent<Camera>();
		this.mapLayerMask = LayerMask.NameToLayer("GalaxyMap");
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002B28 File Offset: 0x00000D28
	private void LateUpdate()
	{
		this.HandleDragging();
		this.HandleZoom();
		if (this.target != null && this.followTarget)
		{
			this.FollowTarget();
		}
		this.ApplyInertia();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002B58 File Offset: 0x00000D58
	private void HandleZoom()
	{
		if (this.IsMouseOverMap())
		{
			AbstractWindow activeWindow = PersistentSingleton<Bootstrapper>.Instance.activeWindow;
			if (activeWindow == null || !activeWindow.isClickThrough)
			{
				float y = Mouse.current.scroll.ReadValue().y;
				if (y != 0f)
				{
					if (Input.GetKey(KeyCode.LeftShift))
					{
						float num = this.mapCamera.orthographicSize - y * this.zoomSpeed * Time.deltaTime;
						num = Mathf.Clamp(num, this.minZoom, this.maxZoom);
						Vector3 a = this.mapCamera.ScreenToWorldPoint(Input.mousePosition);
						this.mapCamera.orthographicSize = num;
						Vector3 b = this.mapCamera.ScreenToWorldPoint(Input.mousePosition);
						Vector3 position = a - b;
						this.SetPosition(position);
						AbstractGalaxyMapManager.current.StoreCurrentZoom();
						return;
					}
					AbstractGalaxyMapManager.ChangeZoomLevel(AbstractGalaxyMapManager.current.zoomLevel + ((y > 0f) ? -1 : 1));
				}
				return;
			}
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002C44 File Offset: 0x00000E44
	private void HandleDragging()
	{
		if (this.IsMouseOverMap())
		{
			AbstractWindow activeWindow = PersistentSingleton<Bootstrapper>.Instance.activeWindow;
			if (activeWindow == null || !activeWindow.isClickThrough)
			{
				goto IL_30;
			}
		}
		if (!this.isDragging)
		{
			this.isDragging = false;
			return;
		}
		IL_30:
		if (Input.GetMouseButtonDown(1))
		{
			this.dragOrigin = Input.mousePosition;
			this.isDragging = true;
			this.followTarget = false;
			this.dragVelocity = Vector3.zero;
		}
		if (Input.GetMouseButtonUp(1))
		{
			this.isDragging = false;
		}
		if (this.isDragging)
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 a = this.dragOrigin - mousePosition;
			this.dragVelocity = a * this.mapCamera.orthographicSize / (float)Screen.height * 2f;
			this.SetPosition(this.dragVelocity);
			this.dragOrigin = mousePosition;
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002D18 File Offset: 0x00000F18
	private void ApplyInertia()
	{
		if (!this.isDragging && this.dragVelocity != Vector3.zero)
		{
			this.SetPosition(this.dragVelocity);
			this.dragVelocity *= this.inertiaDamping;
			if (this.dragVelocity.magnitude < 0.01f)
			{
				this.dragVelocity = Vector3.zero;
			}
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002D7F File Offset: 0x00000F7F
	public void JumpTo(Vector2 position)
	{
		base.transform.position = this.ClampTargetPosition(position);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002D98 File Offset: 0x00000F98
	private void SetPosition(Vector3 move = default(Vector3))
	{
		Vector3 targetPosition = base.transform.position + move;
		base.transform.position = this.ClampTargetPosition(targetPosition);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002DC9 File Offset: 0x00000FC9
	public void SetTarget(GameObject target, bool follow)
	{
		this.target = target;
		this.followTarget = follow;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002DDC File Offset: 0x00000FDC
	protected void FollowTarget()
	{
		Vector3 position = base.transform.position;
		Vector2 vector = this.target.transform.position;
		Vector3 vector2 = new Vector3(vector.x, vector.y, position.z);
		vector2 = this.ClampTargetPosition(vector2);
		Vector3 position2 = vector2;
		base.transform.position = position2;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002E3C File Offset: 0x0000103C
	private Vector3 ClampTargetPosition(Vector3 targetPosition)
	{
		Vector2 zero = Vector2.zero;
		this.cameraMovementBoundary = AbstractGalaxyMapManager.current.xyBounds;
		targetPosition.z = base.transform.position.z;
		float min = Mathf.Clamp(zero.y + this.cameraMovementBoundary.width, zero.y + this.cameraMovementBoundary.width, 0f);
		float max = Mathf.Clamp(zero.y + this.cameraMovementBoundary.height, 0f, zero.y + this.cameraMovementBoundary.height);
		targetPosition.y = Mathf.Clamp(targetPosition.y, min, max);
		targetPosition.x = Mathf.Clamp(targetPosition.x, zero.x + this.cameraMovementBoundary.x, zero.x + this.cameraMovementBoundary.y);
		return targetPosition;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002F20 File Offset: 0x00001120
	private bool IsMouseOverMap()
	{
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		EventSystem.current.RaycastAll(pointerEventData, list);
		foreach (RaycastResult raycastResult in list)
		{
			if (raycastResult.gameObject.layer == this.mapLayerMask)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000023 RID: 35
	[SerializeField]
	private float inertiaDamping = 0.9f;

	// Token: 0x04000024 RID: 36
	[SerializeField]
	private float zoomSpeed = 2f;

	// Token: 0x04000025 RID: 37
	[SerializeField]
	private float minZoom = 3f;

	// Token: 0x04000026 RID: 38
	public float maxZoom = 27f;

	// Token: 0x04000027 RID: 39
	private Vector3 dragVelocity = Vector3.zero;

	// Token: 0x04000028 RID: 40
	private Camera mapCamera;

	// Token: 0x04000029 RID: 41
	private Vector3 dragOrigin;

	// Token: 0x0400002A RID: 42
	private bool isDragging;

	// Token: 0x0400002C RID: 44
	private GameObject target;

	// Token: 0x0400002D RID: 45
	private int mapLayerMask;

	// Token: 0x0400002E RID: 46
	private Rect cameraMovementBoundary;
}
