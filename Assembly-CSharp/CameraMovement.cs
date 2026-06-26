using System;
using System.Collections.Generic;
using Behaviour.Bootstrap;
using Behaviour.GalaxyMap;
using Behaviour.Managers;
using Behaviour.Transparency;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Util;
using Source.MissionSystem;
using Source.Player;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000003 RID: 3
public class CameraMovement : MonoBehaviour
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
	// (set) Token: 0x06000008 RID: 8 RVA: 0x00002171 File Offset: 0x00000371
	public static float maxZoom { get; protected set; } = 27f;

	// Token: 0x06000009 RID: 9 RVA: 0x00002179 File Offset: 0x00000379
	private void Awake()
	{
		this.mainCamera = base.GetComponent<Camera>();
		this.eventSystem = EventSystem.current;
		CameraMovement.uiLayerMask = LayerMask.NameToLayer("UI");
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021A1 File Offset: 0x000003A1
	private void Start()
	{
		this.gameCamera.orthographicSize = GameplayerPrefs.GetZoom();
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021B3 File Offset: 0x000003B3
	private void LateUpdate()
	{
		if (this.shipyardZoom)
		{
			return;
		}
		this.HandleDragging();
		this.HandleZoom();
		if (this.target != null && this.followTarget)
		{
			this.FollowTarget();
		}
		this.ApplyInertia();
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000021EC File Offset: 0x000003EC
	private void HandleZoom()
	{
		if (!this.IsMouseOverUI())
		{
			AbstractWindow activeWindow = PersistentSingleton<Bootstrapper>.Instance.activeWindow;
			if (activeWindow == null || !activeWindow.isClickThrough)
			{
				if (SpaceStationInterior.instance)
				{
					return;
				}
				if (AbstractGalaxyMapManager.current)
				{
					return;
				}
				float y = Input.mouseScrollDelta.y;
				if (y != 0f)
				{
					float num = this.gameCamera.orthographicSize - y * this.zoomSpeed * Time.deltaTime;
					num = Mathf.Clamp(num, this.minZoom, CameraMovement.maxZoom);
					Vector3 a = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
					this.gameCamera.orthographicSize = num;
					Vector3 b = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
					Vector3 position = a - b;
					this.SetPosition(position);
					float num2 = this.mainCamera.orthographicSize - y * this.backgroundZoomSpeed * Time.deltaTime;
					num2 = Mathf.Clamp(num2, this.backgroundMinZoom, this.backgroundMaxZoom);
					this.mainCamera.orthographicSize = num2;
					GameplayerPrefs.SetZoom(num);
				}
				float orthographicSize = this.gameCamera.orthographicSize;
				if (this.previousZoom != orthographicSize && this.switchers.Count > 0)
				{
					float num3 = orthographicSize - this.previousZoom;
					if (num3 > 0f && this.previousZoom < this.zoomBilinearTreshold && orthographicSize >= this.zoomBilinearTreshold)
					{
						this.SwitchFilteringTo(FilterMode.Bilinear);
					}
					else if (num3 < 0f && this.previousZoom >= this.zoomBilinearTreshold && orthographicSize < this.zoomBilinearTreshold)
					{
						this.SwitchFilteringTo(FilterMode.Point);
					}
					this.previousZoom = orthographicSize;
				}
				return;
			}
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000237C File Offset: 0x0000057C
	private bool IsMouseOverUI()
	{
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
		pointerEventData.position = Input.mousePosition;
		this.eventSystem.RaycastAll(pointerEventData, list);
		foreach (RaycastResult raycastResult in list)
		{
			if (raycastResult.gameObject.layer == CameraMovement.uiLayerMask)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002410 File Offset: 0x00000610
	public void ShipyardZoom()
	{
		this.shipyardZoom = true;
		this.gameCamera.orthographicSize = (ScreenSettings.fullscreen ? 5f : 1.4f);
		this.minZoom = 1f;
		this.followOffset = -2f;
		if (GameplayManager.Instance)
		{
			this.SetTarget(GameplayManager.Instance.spaceShip, true);
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002475 File Offset: 0x00000675
	public void NormalZoom(float orthographicSize = 5f)
	{
		if (!this.gameCamera)
		{
			return;
		}
		this.shipyardZoom = false;
		this.gameCamera.orthographicSize = orthographicSize;
		this.minZoom = 2f;
		this.followOffset = 0f;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000024B0 File Offset: 0x000006B0
	private void HandleDragging()
	{
		if (!AbstractGalaxyMapManager.current && !this.IsMouseOverUI())
		{
			AbstractWindow activeWindow = PersistentSingleton<Bootstrapper>.Instance.activeWindow;
			if ((activeWindow == null || !activeWindow.isClickThrough) && !SpaceStationInterior.instance)
			{
				goto IL_40;
			}
		}
		if (!this.isDragging)
		{
			goto IL_53;
		}
		IL_40:
		if (!Singleton<TravelManager>.HasInstance || !Singleton<TravelManager>.Instance.TravelActive())
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.spaceKeyPressed = true;
			}
			if (Input.GetKeyUp(KeyCode.Space))
			{
				this.spaceKeyPressed = false;
			}
			if (Input.GetMouseButtonDown(1) || (this.spaceKeyPressed && Input.GetMouseButtonDown(0)))
			{
				MissionObjective.Trigger(MissionTrigger.MoveCamera, null, null, false);
				if (!this.targetSelecting)
				{
					this.dragOrigin = Input.mousePosition;
					this.isDragging = true;
					this.followTarget = false;
					this.dragVelocity = Vector3.zero;
				}
			}
			if (Input.GetMouseButtonUp(1) || (this.spaceKeyPressed && Input.GetMouseButtonUp(0)))
			{
				this.isDragging = false;
			}
			if (this.isDragging)
			{
				Vector3 mousePosition = Input.mousePosition;
				Vector3 a = this.dragOrigin - mousePosition;
				this.dragVelocity = a * this.gameCamera.orthographicSize / (float)Screen.height * 2f;
				this.SetPosition(this.dragVelocity);
				this.dragOrigin = mousePosition;
			}
			this.targetSelecting = false;
			return;
		}
		IL_53:
		this.isDragging = false;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002608 File Offset: 0x00000808
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

	// Token: 0x06000012 RID: 18 RVA: 0x00002670 File Offset: 0x00000870
	private void SetPosition(Vector3 move = default(Vector3))
	{
		Vector3 targetPosition = base.transform.position + move;
		base.transform.position = this.ClampTargetPosition(targetPosition);
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000026A4 File Offset: 0x000008A4
	public void SetNewPosition(Vector2 position)
	{
		Vector3 position2 = new Vector3(position.x, position.y, base.transform.position.z);
		base.transform.position = position2;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000026E0 File Offset: 0x000008E0
	public void SetTarget(SpaceShip target, bool follow)
	{
		this.targetSelecting = true;
		this.target = target;
		this.FocusTarget(follow);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000026F8 File Offset: 0x000008F8
	public void FocusTarget(bool follow = false)
	{
		this.followTarget = follow;
		Vector2 vector = this.target.rigidbody ? this.target.rigidbody.position : (Vector2)this.target.transform.position;
		Vector3 position = this.ClampTargetPosition(new Vector3(vector.x, vector.y, base.transform.position.z));
		base.transform.position = position;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x0000277C File Offset: 0x0000097C
	protected void FollowTarget()
	{
		Vector3 position = base.transform.position;
		Vector2 vector = this.target.rigidbody ? this.target.rigidbody.position : (Vector2)this.target.transform.position;
		Vector3 vector2 = new Vector3(vector.x + this.followOffset, vector.y, position.z);
		vector2 = this.ClampTargetPosition(vector2);
		Vector3 position2 = vector2;
		base.transform.position = position2;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002808 File Offset: 0x00000A08
	private Vector3 ClampTargetPosition(Vector3 targetPosition)
	{
		Vector2 vector = Vector2.zero;
		if (this.target)
		{
			vector = (this.target.rigidbody ? this.target.rigidbody.position : this.target.transform.position);
		}
		float orthographicSize = this.gameCamera.orthographicSize;
		targetPosition.y = Mathf.Clamp(targetPosition.y, vector.y + this.cameraMovementBoundary.width + orthographicSize, vector.y + this.cameraMovementBoundary.height - orthographicSize);
		targetPosition.x = Mathf.Clamp(targetPosition.x, vector.x + this.cameraMovementBoundary.x, vector.x + this.cameraMovementBoundary.y);
		return targetPosition;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000028DF File Offset: 0x00000ADF
	public void LoseTarget()
	{
		this.target = null;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000028E8 File Offset: 0x00000AE8
	public void AddZoomSwitchFiltering(ZoomSwitchFiltering zsf)
	{
		this.switchers.Add(zsf);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000028F8 File Offset: 0x00000AF8
	public void SwitchFilteringTo(FilterMode mode)
	{
		for (int i = 0; i < this.switchers.Count; i++)
		{
			if (this.switchers[i])
			{
				this.switchers[i].SetFilterMode(mode);
			}
			else
			{
				this.switchers.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x04000003 RID: 3
	private static int uiLayerMask;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	private SpaceShip target;

	// Token: 0x04000005 RID: 5
	[SerializeField]
	private float inertiaDamping = 0.9f;

	// Token: 0x04000006 RID: 6
	[SerializeField]
	private float zoomSpeed = 2f;

	// Token: 0x04000007 RID: 7
	[SerializeField]
	private float minZoom = 3f;

	// Token: 0x04000009 RID: 9
	[SerializeField]
	private float backgroundZoomSpeed = 0.01f;

	// Token: 0x0400000A RID: 10
	[SerializeField]
	private float backgroundMinZoom = 3.9f;

	// Token: 0x0400000B RID: 11
	[SerializeField]
	private float backgroundMaxZoom = 4.1f;

	// Token: 0x0400000C RID: 12
	[SerializeField]
	private float zoomBilinearTreshold = 8.5f;

	// Token: 0x0400000D RID: 13
	private Vector3 dragVelocity = Vector3.zero;

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private bool followTarget;

	// Token: 0x0400000F RID: 15
	private float followOffset;

	// Token: 0x04000010 RID: 16
	[Tooltip("rect with x, y as min x, max x en width, height as min y, max y")]
	[SerializeField]
	private Rect cameraMovementBoundary;

	// Token: 0x04000011 RID: 17
	private bool targetSelecting;

	// Token: 0x04000012 RID: 18
	private bool spaceKeyPressed;

	// Token: 0x04000013 RID: 19
	private Vector3 dragOrigin;

	// Token: 0x04000014 RID: 20
	private bool isDragging;

	// Token: 0x04000015 RID: 21
	private Camera mainCamera;

	// Token: 0x04000016 RID: 22
	public Camera gameCamera;

	// Token: 0x04000017 RID: 23
	private EventSystem eventSystem;

	// Token: 0x04000018 RID: 24
	private bool shipyardZoom;

	// Token: 0x04000019 RID: 25
	private float previousZoom;

	// Token: 0x0400001A RID: 26
	private List<ZoomSwitchFiltering> switchers = new List<ZoomSwitchFiltering>();
}
