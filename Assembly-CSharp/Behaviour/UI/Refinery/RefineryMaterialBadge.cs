using System;
using System.Collections;
using Behaviour.Item;
using Behaviour.UI.Forge;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Mining;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Refinery
{
	// Token: 0x0200024B RID: 587
	public class RefineryMaterialBadge : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ITooltipTitleSource, ITooltipTextSource
	{
		// Token: 0x060015BB RID: 5563 RVA: 0x0008B2B6 File Offset: 0x000894B6
		private void OnEnable()
		{
			base.StartCoroutine(this.UpdateProgress());
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0008B2C8 File Offset: 0x000894C8
		public void SetMaterial(RefinedMaterial mat)
		{
			this.material = mat;
			this.label.color = mat.GetColor();
			this.count.color = mat.GetColor();
			this.icon.sprite = mat.GetIcon();
			this.UpdateLabel();
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0008B315 File Offset: 0x00089515
		private IEnumerator UpdateProgress()
		{
			yield return null;
			for (;;)
			{
				GamePlayer.current.CountRefinedMaterial(this.material);
				this.UpdateLabel();
				yield return new WaitForSeconds(0.5f);
			}
			yield break;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0008B324 File Offset: 0x00089524
		private void UpdateLabel()
		{
			this.count.text = GameMath.FormatNumber(GamePlayer.current.CountRefinedMaterial(this.material), -1);
			this.label.text = this.material.GetDisplayName();
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0008B360 File Offset: 0x00089560
		public void OnPointerClick(PointerEventData eventData)
		{
			Refinery refinery = Refinery.current;
			if (refinery == null)
			{
				return;
			}
			InventoryItemType item = InventoryItemType.Get("Canister" + this.material.ToString());
			Transform transform = base.transform;
			if (RefineryUI.current)
			{
				transform = RefineryUI.current.transform;
			}
			else if (ForgeUI.current)
			{
				transform = ForgeUI.current.transform;
			}
			this.actionPopup = UnityEngine.Object.Instantiate<DragableActionPopup>(this.popupPrefab, transform);
			this.actionPopup.extractMaterial = this.material;
			this.actionPopup.SetInventoryItem(item, "Extract", Mathf.FloorToInt(GamePlayer.current.CountRefinedMaterial(this.material)), new int?(0), null);
			this.actionPopup.cancelButton.onClick.AddListener(new UnityAction(this.OnCancel));
			this.actionPopup.actionButton.onClick.AddListener(delegate()
			{
				refinery.ExtractMaterial(this.material, this.actionPopup.GetAmount());
				this.DestroyPopup();
				if (RefineryUI.current)
				{
					RefineryUI.current.UpdateContent();
				}
			});
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0008B478 File Offset: 0x00089678
		public string GetTooltipText()
		{
			Refinery current = Refinery.current;
			if (GamePlayer.current.CountRefinedMaterial(this.material) >= 1f && current != null)
			{
				return Translation.TranslateOnly("@SSRefineryMaterialDesc", new object[]
				{
					this.material.GetDisplayName()
				});
			}
			return Translation.TranslateOnly("@SSRefineryMaterialEmpty", new object[]
			{
				this.material.GetDisplayName()
			});
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0008B4E2 File Offset: 0x000896E2
		public string GetTooltipTitle()
		{
			return this.material.GetDisplayName().HighlightWithColor(this.material.GetColor());
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0008B4FF File Offset: 0x000896FF
		private void DestroyPopup()
		{
			if (this.actionPopup)
			{
				UnityEngine.Object.Destroy(this.actionPopup.gameObject);
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0008B51E File Offset: 0x0008971E
		private void OnCancel()
		{
			this.DestroyPopup();
		}

		// Token: 0x04000D03 RID: 3331
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000D04 RID: 3332
		[SerializeField]
		private TMP_Text count;

		// Token: 0x04000D05 RID: 3333
		[SerializeField]
		private Image icon;

		// Token: 0x04000D06 RID: 3334
		[SerializeField]
		private RectTransform progress;

		// Token: 0x04000D07 RID: 3335
		[SerializeField]
		private DragableActionPopup popupPrefab;

		// Token: 0x04000D08 RID: 3336
		private DragableActionPopup actionPopup;

		// Token: 0x04000D09 RID: 3337
		private RefinedMaterial material;
	}
}
