using System;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Module.DroneBay.OpeningMechanism;
using Behaviour.UI;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Equipment
{
	// Token: 0x02000340 RID: 832
	public class DecalSlot : MonoBehaviour
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001F98 RID: 8088 RVA: 0x000BB1B6 File Offset: 0x000B93B6
		// (set) Token: 0x06001F99 RID: 8089 RVA: 0x000BB1BE File Offset: 0x000B93BE
		public int PlacementIndex { get; private set; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x000BB1C7 File Offset: 0x000B93C7
		// (set) Token: 0x06001F9B RID: 8091 RVA: 0x000BB1CF File Offset: 0x000B93CF
		public int BayIndex { get; private set; }

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000BB1D8 File Offset: 0x000B93D8
		// (set) Token: 0x06001F9D RID: 8093 RVA: 0x000BB1E0 File Offset: 0x000B93E0
		public int DoorIndex { get; private set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x000BB1E9 File Offset: 0x000B93E9
		public static bool AnySlotOwned
		{
			get
			{
				return DecalSlot._exclusiveSlot != null;
			}
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000BB1F8 File Offset: 0x000B93F8
		private void Awake()
		{
			if (!this.spriteRenderer)
			{
				this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			if (!this.tooltipSource)
			{
				this.tooltipSource = base.GetComponent<TooltipSource>();
			}
			this._collider = base.GetComponent<CircleCollider2D>();
			if (this.spriteRenderer)
			{
				this.spriteRenderer.enabled = false;
			}
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000BB25C File Offset: 0x000B945C
		public void Init(SpaceShip ship, int index, DecalPlacement placement)
		{
			this._ship = ship;
			this.PlacementIndex = index;
			this._placement = placement;
			this.SetTransformFromPlacement(placement);
			this.PrimeCache(placement.decalId);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x000BB288 File Offset: 0x000B9488
		public void InitDoor(SpaceShip ship, Door door, DroneBayModule module, int bayIndex, int doorIndex, int decalIndex, DecalPlacement placement)
		{
			this._ship = ship;
			this._door = door;
			this._module = module;
			this.BayIndex = bayIndex;
			this.DoorIndex = doorIndex;
			this.PlacementIndex = decalIndex;
			this._placement = placement;
			this.SetTransformFromPlacement(placement);
			this.PrimeCache(placement.decalId);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000BB2E0 File Offset: 0x000B94E0
		private void SetTransformFromPlacement(DecalPlacement placement)
		{
			base.transform.localPosition = new Vector3(placement.position.x, placement.position.y, -0.008f);
			base.transform.localScale = Vector3.one * placement.scale;
			base.transform.localEulerAngles = new Vector3(0f, 0f, placement.rotation);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000BB354 File Offset: 0x000B9554
		private void PrimeCache(string decalId)
		{
			if (!string.IsNullOrEmpty(decalId))
			{
				DecalDefinition decalDefinition = Decals.Get(decalId);
				this._cachedDecalId = decalId;
				this._cachedSprite = ((decalDefinition != null) ? decalDefinition.GetSprite() : null);
			}
			this.RefreshColliderRadius();
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000BB38F File Offset: 0x000B958F
		public void ApplyScale(float scale)
		{
			scale = Mathf.Max(0.5f, scale);
			if (this._placement != null)
			{
				this._placement.scale = scale;
			}
			base.transform.localScale = Vector3.one * scale;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000BB3C8 File Offset: 0x000B95C8
		public void ApplyRotation(float rotation)
		{
			if (this._placement != null)
			{
				this._placement.rotation = rotation;
			}
			base.transform.localEulerAngles = new Vector3(0f, 0f, rotation);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x000BB3FC File Offset: 0x000B95FC
		private void Update()
		{
			if (!this.InDecalMode())
			{
				this._hovering = false;
				this._mouseIsDown = false;
				this._isDragging = false;
				if (this.spriteRenderer)
				{
					this.spriteRenderer.enabled = false;
				}
				if (this.tooltipSource)
				{
					this.tooltipSource.enabled = false;
				}
				return;
			}
			bool hovering = this._hovering;
			this._hovering = this.IsMouseOverSlot();
			if (!hovering && this._hovering && this.tooltipSource)
			{
				this.tooltipSource.enabled = true;
			}
			if (hovering && !this._hovering)
			{
				if (this.tooltipSource)
				{
					this.tooltipSource.enabled = false;
				}
				if (this._mouseIsDown && !this._isDragging)
				{
					this._mouseIsDown = false;
					if (DecalSlot._exclusiveSlot == this)
					{
						DecalSlot._exclusiveSlot = null;
					}
				}
			}
			bool wasPressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
			bool isPressed = Mouse.current.leftButton.isPressed;
			bool wasReleasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;
			if (this._mouseIsDown && !isPressed && !wasReleasedThisFrame)
			{
				this.FinalizeDrag();
			}
			if (!this._mouseIsDown && this._hovering && wasPressedThisFrame && !UIHelper.IsMouseOverUi && (DecalSlot._exclusiveSlot == null || this.PlacementIndex > DecalSlot._exclusiveSlot.PlacementIndex))
			{
				if (DecalSlot._exclusiveSlot != null && DecalSlot._exclusiveSlot != this)
				{
					DecalSlot._exclusiveSlot._mouseIsDown = false;
					DecalSlot._exclusiveSlot._isDragging = false;
				}
				DecalSlot._exclusiveSlot = this;
				this._mouseIsDown = true;
				this._isDragging = false;
				this._mouseDownScreenPos = Mouse.current.position.ReadValue();
				DecalPlacement placement = this._placement;
				this._dragStartPosition = ((placement != null) ? placement.position : Vector2.zero);
				Vector3 position = this.GetRenderCamera().ScreenToWorldPoint(this._mouseDownScreenPos);
				position.z = 0f;
				this._mouseDownParentLocalPos = base.transform.parent.InverseTransformPoint(position);
			}
			if (this._mouseIsDown)
			{
				if (wasReleasedThisFrame)
				{
					bool isDragging = this._isDragging;
					this.FinalizeDrag();
					if (DecalSlot._exclusiveSlot == this)
					{
						DecalSlot._exclusiveSlot = null;
					}
					if (!isDragging && !UIHelper.IsMouseOverUi && PersonalHangar.current)
					{
						if (this._door != null)
						{
							PersonalHangar.current.OnDoorDecalSlotClicked(this.BayIndex, this.DoorIndex, this.PlacementIndex);
						}
						else
						{
							PersonalHangar.current.OnDecalSlotClicked(this.PlacementIndex);
						}
					}
				}
				else if (isPressed)
				{
					if (!this._isDragging && Vector2.Distance(Mouse.current.position.ReadValue(), this._mouseDownScreenPos) > 8f)
					{
						this._isDragging = true;
					}
					if (this._isDragging)
					{
						this.UpdateDragPosition();
					}
				}
			}
			if (this.tooltipSource)
			{
				this.tooltipSource.enabled = (this._hovering && !this._isDragging);
			}
			if (this.spriteRenderer)
			{
				this.spriteRenderer.enabled = true;
				DecalPlacement placement2 = this._placement;
				string text = (placement2 != null) ? placement2.decalId : null;
				if (text != this._cachedDecalId)
				{
					this._cachedDecalId = text;
					this._cachedSprite = null;
					if (!string.IsNullOrEmpty(text))
					{
						DecalDefinition decalDefinition = Decals.Get(text);
						if (decalDefinition != null)
						{
							this._cachedSprite = decalDefinition.GetSprite();
						}
					}
					this.RefreshColliderRadius();
				}
				this.spriteRenderer.sprite = ((this._cachedSprite != null) ? this._cachedSprite : this.placeholderSprite);
			}
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000BB7AC File Offset: 0x000B99AC
		private bool IsMouseOverSlot()
		{
			if (this._collider == null)
			{
				return false;
			}
			Camera renderCamera = this.GetRenderCamera();
			if (renderCamera == null)
			{
				return false;
			}
			Vector3 v = renderCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			v.z = base.transform.position.z;
			float num = this._collider.radius * Mathf.Abs(base.transform.lossyScale.x);
			return Vector2.Distance(v, base.transform.position) <= num;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000BB850 File Offset: 0x000B9A50
		private void UpdateDragPosition()
		{
			Vector3 position = this.GetRenderCamera().ScreenToWorldPoint(Mouse.current.position.ReadValue());
			position.z = 0f;
			Vector2 b = base.transform.parent.InverseTransformPoint(position) - this._mouseDownParentLocalPos;
			Vector2 vector = this._dragStartPosition + b;
			if ((this._door != null) ? this._door.IsPixelOnDoor(vector) : (this._ship != null && this._ship.IsPixelOnShip(vector)))
			{
				base.transform.localPosition = new Vector3(vector.x, vector.y, -0.008f);
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x000BB914 File Offset: 0x000B9B14
		private void FinalizeDrag()
		{
			if (this._isDragging)
			{
				Vector2 newPosition = base.transform.localPosition;
				if (PersonalHangar.current)
				{
					if (this._door != null)
					{
						PersonalHangar.current.OnDoorDecalMoved(this.BayIndex, this.DoorIndex, this.PlacementIndex, newPosition);
					}
					else
					{
						PersonalHangar.current.OnDecalMoved(this.PlacementIndex, newPosition);
					}
				}
			}
			this._mouseIsDown = false;
			this._isDragging = false;
			if (DecalSlot._exclusiveSlot == this)
			{
				DecalSlot._exclusiveSlot = null;
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000BB9A8 File Offset: 0x000B9BA8
		private void RefreshColliderRadius()
		{
			if (this._collider == null)
			{
				return;
			}
			Sprite sprite = (this._cachedSprite != null) ? this._cachedSprite : this.placeholderSprite;
			if (sprite == null)
			{
				return;
			}
			float num = sprite.rect.width / sprite.pixelsPerUnit;
			float num2 = sprite.rect.height / sprite.pixelsPerUnit;
			this._collider.radius = Mathf.Sqrt(num * num + num2 * num2) * 0.5f;
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x000BBA34 File Offset: 0x000B9C34
		private Camera GetRenderCamera()
		{
			if (PersonalHangar.current && PersonalHangar.current.shipSelect)
			{
				return PersonalHangar.current.shipSelect.CarouselCamera;
			}
			return Camera.main;
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000BBA68 File Offset: 0x000B9C68
		private bool InDecalMode()
		{
			return SpaceStationInterior.instance != null && PersonalHangar.current != null && PersonalHangar.current.decalModeActive && this._ship == PersonalHangar.current.shipSelect.selectedShip;
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000BBAB7 File Offset: 0x000B9CB7
		private void OnDestroy()
		{
			if (DecalSlot._exclusiveSlot == this)
			{
				DecalSlot._exclusiveSlot = null;
			}
		}

		// Token: 0x040012D8 RID: 4824
		private const float DragThresholdPx = 8f;

		// Token: 0x040012D9 RID: 4825
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x040012DA RID: 4826
		[SerializeField]
		private TooltipSource tooltipSource;

		// Token: 0x040012DB RID: 4827
		[SerializeField]
		private Sprite placeholderSprite;

		// Token: 0x040012DF RID: 4831
		private DecalPlacement _placement;

		// Token: 0x040012E0 RID: 4832
		private SpaceShip _ship;

		// Token: 0x040012E1 RID: 4833
		private Door _door;

		// Token: 0x040012E2 RID: 4834
		private DroneBayModule _module;

		// Token: 0x040012E3 RID: 4835
		private CircleCollider2D _collider;

		// Token: 0x040012E4 RID: 4836
		private static DecalSlot _exclusiveSlot;

		// Token: 0x040012E5 RID: 4837
		private string _cachedDecalId;

		// Token: 0x040012E6 RID: 4838
		private Sprite _cachedSprite;

		// Token: 0x040012E7 RID: 4839
		private bool _hovering;

		// Token: 0x040012E8 RID: 4840
		private bool _mouseIsDown;

		// Token: 0x040012E9 RID: 4841
		private bool _isDragging;

		// Token: 0x040012EA RID: 4842
		private Vector2 _mouseDownScreenPos;

		// Token: 0x040012EB RID: 4843
		private Vector3 _mouseDownParentLocalPos;

		// Token: 0x040012EC RID: 4844
		private Vector2 _dragStartPosition;
	}
}
