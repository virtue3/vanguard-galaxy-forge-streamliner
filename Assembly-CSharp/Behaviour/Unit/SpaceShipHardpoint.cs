using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.UI;
using Behaviour.UI.Main;
using Behaviour.UI.ShipCarousel;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Tooltip;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Unit
{
	// Token: 0x020001C7 RID: 455
	public class SpaceShipHardpoint : MonoBehaviour, IHighlightable
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x00072CB6 File Offset: 0x00070EB6
		// (set) Token: 0x0600113B RID: 4411 RVA: 0x00072CBE File Offset: 0x00070EBE
		public int index { get; set; } = -1;

		// Token: 0x0600113C RID: 4412 RVA: 0x00072CC8 File Offset: 0x00070EC8
		private void Awake()
		{
			this.hardpointVisuals = new Dictionary<ModuleSize, ValueTuple<Color, Sprite>>
			{
				{
					ModuleSize.Tiny,
					new ValueTuple<Color, Sprite>(new Color(0.364705f, 1f, 0.4849552f, 0.75f), this.tinySprite)
				},
				{
					ModuleSize.Small,
					new ValueTuple<Color, Sprite>(new Color(0.3632075f, 0.6496469f, 1f, 0.75f), this.smallSprite)
				},
				{
					ModuleSize.Medium,
					new ValueTuple<Color, Sprite>(new Color(1f, 0.9774383f, 0.3647059f, 0.75f), this.mediumSprite)
				},
				{
					ModuleSize.Large,
					new ValueTuple<Color, Sprite>(new Color(1f, 0.3837572f, 0.3647059f, 0.75f), this.largeSprite)
				}
			};
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			this.spriteRenderer.color = this.hardpointVisuals[this.size].Item1;
			this.spriteRenderer.sprite = this.hardpointVisuals[this.size].Item2;
			this.spriteRenderer.sortingOrder = 0;
			this.normalMaterial = this.spriteRenderer.material;
			this.tooltip = base.GetComponent<ItemTooltipSource>();
			this.collider = base.GetComponent<BoxCollider2D>();
			this.emptyTooltip = base.gameObject.AddComponent<TooltipSource>();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00072E20 File Offset: 0x00071020
		private void Start()
		{
			this.spriteRenderer.size = new Vector2(0.45f, 0.45f);
			this.collider.size = this.spriteRenderer.size;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00072E54 File Offset: 0x00071054
		private void Update()
		{
			InventoryInteractionManager instance = InventoryInteractionManager.Instance;
			if (((instance != null) ? instance.hoveringHardpoint : null) == this && InventoryInteractionManager.Instance.isPersonalHangarOpen && !UIHelper.IsMouseOverUi)
			{
				bool wasReleasedThisFrame = Mouse.current.rightButton.wasReleasedThisFrame;
				bool wasReleasedThisFrame2 = Mouse.current.leftButton.wasReleasedThisFrame;
				if (wasReleasedThisFrame || wasReleasedThisFrame2)
				{
					ShipCarousel componentInParent = base.GetComponentInParent<ShipCarousel>();
					if (!componentInParent || !componentInParent.inPersonalHangar)
					{
						return;
					}
				}
				if (wasReleasedThisFrame)
				{
					InventoryInteractionManager.Instance.UnequipHardpoint(this);
					return;
				}
				if (wasReleasedThisFrame2)
				{
					AbstractEquipment componentInChildren = base.GetComponentInChildren<AbstractEquipment>();
					InventoryItemType inventoryItemType = (componentInChildren != null) ? componentInChildren.item : null;
					if (inventoryItemType && SalvageWorkshop.current && inventoryItemType.CanGoInWorkshop())
					{
						SalvageWorkshop.current.SelectItem(inventoryItemType, true, null);
					}
				}
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00072F21 File Offset: 0x00071121
		protected void OnMouseEnter()
		{
			this.spriteRenderer.enabled = true;
			if (InventoryInteractionManager.Instance)
			{
				InventoryInteractionManager.Instance.hoveringHardpoint = this;
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00072F46 File Offset: 0x00071146
		protected void OnMouseExit()
		{
			this.spriteRenderer.enabled = false;
			if (InventoryInteractionManager.Instance)
			{
				InventoryInteractionManager.Instance.hoveringHardpoint = null;
			}
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00072F6B File Offset: 0x0007116B
		public void ShowHighlight()
		{
			this.spriteRenderer.material = this.highlightMaterial;
			this.spriteRenderer.enabled = true;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00072F8C File Offset: 0x0007118C
		public void HideHighlight()
		{
			if (!this.spriteRenderer)
			{
				return;
			}
			if (this.normalMaterial == null)
			{
				Debug.LogError("Highlight normalMaterial is missing.");
			}
			this.spriteRenderer.material = this.normalMaterial;
			this.spriteRenderer.enabled = false;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00072FDC File Offset: 0x000711DC
		public void SetItem(InventoryItemType item)
		{
			this.tooltip.SetItem(item, 1, false, ItemTooltipContext.InCarousel, false, null);
			if (this.emptyTooltip)
			{
				this.emptyTooltip.BodyText = Translation.Translate("@SPHardpointEmpty", new object[]
				{
					this.size.GetDisplayName()
				});
				this.emptyTooltip.enabled = (item == null);
			}
		}

		// Token: 0x04000960 RID: 2400
		public float rotate;

		// Token: 0x04000961 RID: 2401
		public ModuleSize size;

		// Token: 0x04000963 RID: 2403
		[SerializeField]
		private Sprite tinySprite;

		// Token: 0x04000964 RID: 2404
		[SerializeField]
		private Sprite smallSprite;

		// Token: 0x04000965 RID: 2405
		[SerializeField]
		private Sprite mediumSprite;

		// Token: 0x04000966 RID: 2406
		[SerializeField]
		private Sprite largeSprite;

		// Token: 0x04000967 RID: 2407
		[SerializeField]
		private Material highlightMaterial;

		// Token: 0x04000968 RID: 2408
		public EquipmentBuilder defaultEquipment;

		// Token: 0x04000969 RID: 2409
		private Material normalMaterial;

		// Token: 0x0400096A RID: 2410
		private Dictionary<ModuleSize, ValueTuple<Color, Sprite>> hardpointVisuals;

		// Token: 0x0400096B RID: 2411
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400096C RID: 2412
		private BoxCollider2D collider;

		// Token: 0x0400096D RID: 2413
		private ItemTooltipSource tooltip;

		// Token: 0x0400096E RID: 2414
		private TooltipSource emptyTooltip;
	}
}
