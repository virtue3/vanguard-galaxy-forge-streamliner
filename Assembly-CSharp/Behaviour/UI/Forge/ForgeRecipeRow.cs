using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Source.Mining;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Forge
{
	// Token: 0x02000292 RID: 658
	public class ForgeRecipeRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x00096CA0 File Offset: 0x00094EA0
		// (set) Token: 0x0600181A RID: 6170 RVA: 0x00096CA8 File Offset: 0x00094EA8
		public CraftingRecipe recipe { get; private set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600181B RID: 6171 RVA: 0x00096CB1 File Offset: 0x00094EB1
		// (set) Token: 0x0600181C RID: 6172 RVA: 0x00096CB9 File Offset: 0x00094EB9
		public List<CraftingRecipe> unlockedRecipes { get; private set; }

		// Token: 0x0600181D RID: 6173 RVA: 0x00096CC2 File Offset: 0x00094EC2
		private void Awake()
		{
			this.idleColor = this.background.color;
			this.updateTimer = SeededRandom.Global.RandomRange(0f, 0.5f);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00096CEF File Offset: 0x00094EEF
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.SetRecipe(this.recipe, this.unlockedRecipes);
				this.updateTimer = 0.5f;
			}
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00096D30 File Offset: 0x00094F30
		public void SetRecipe(CraftingRecipe cr, List<CraftingRecipe> unlockedRecipes)
		{
			this.recipe = cr;
			this.unlockedRecipes = unlockedRecipes;
			string text = Translation.Translate(string.IsNullOrEmpty(cr.recipeCategoryName) ? cr.displayName : cr.recipeCategoryName, Array.Empty<object>());
			int num = cr.CountAvailableForCrafting(Source.Mining.Forge.current);
			if (num > 0)
			{
				text = text + " (" + GameMath.FormatNumber((float)num, -1) + ")";
			}
			if (this.currentCount >= 0 && this.currentCount != num && this.selected)
			{
				base.GetComponentInParent<ForgeUI>().UpdateContent();
			}
			this.currentCount = num;
			this.icon.sprite = cr.icon;
			this.label.text = text;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00096DE3 File Offset: 0x00094FE3
		public void SetSelected(bool selected)
		{
			this.selected = selected;
			this.background.color = (selected ? this.highlightColor : this.idleColor);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00096E08 File Offset: 0x00095008
		public void OnPointerClick(PointerEventData eventData)
		{
			base.GetComponentInParent<ForgeUI>().SelectRecipe(this.recipe, this.unlockedRecipes, null);
		}

		// Token: 0x04000F10 RID: 3856
		[SerializeField]
		private Image icon;

		// Token: 0x04000F11 RID: 3857
		[SerializeField]
		private Image background;

		// Token: 0x04000F12 RID: 3858
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000F13 RID: 3859
		[SerializeField]
		private Color highlightColor;

		// Token: 0x04000F14 RID: 3860
		private Color idleColor;

		// Token: 0x04000F17 RID: 3863
		private float updateTimer;

		// Token: 0x04000F18 RID: 3864
		private bool selected;

		// Token: 0x04000F19 RID: 3865
		private int currentCount = -1;
	}
}
