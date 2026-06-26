using System;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Forge
{
	// Token: 0x02000295 RID: 661
	public class RarityButton : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001842 RID: 6210 RVA: 0x00097CB4 File Offset: 0x00095EB4
		public void SetButton(Rarity rarity, bool locked, bool active, ForgeTabContents ftc)
		{
			this.rarity = rarity;
			this.locked = locked;
			this.selected = active;
			this.forgeTabContents = ftc;
			this.textLabel.text = rarity.GetDisplayName()[0].ToString();
			Image component = base.GetComponent<Image>();
			if (locked)
			{
				this.textLabel.color = ColorHelper.boringGrey.WithAlpha(0.5f);
				component.color = ColorHelper.boringGrey.WithAlpha(0.5f);
				return;
			}
			component.color = (this.selected ? ColorHelper.greenish : Color.white);
			this.textLabel.color = rarity.GetColor();
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00097D62 File Offset: 0x00095F62
		public void SetRarityRecipe()
		{
			if (this.locked || this.selected)
			{
				return;
			}
			this.forgeTabContents.SetRaritySubRecipe(this.rarity);
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00097D88 File Offset: 0x00095F88
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.rarity.GetDisplayName(), 12, 8f).Text.color = this.rarity.GetColor();
			tooltip.AddSeparator(null);
			if (this.selected)
			{
				tooltip.AddTextLine("@BlueprintSelected", 12, 8f).Text.color = ColorHelper.boringGrey;
				return;
			}
			if (this.locked)
			{
				tooltip.AddTextLine("@BlueprintNotYetFound", 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			tooltip.AddTextLine("@BlueprintSelectRarity", 12, 8f).Text.color = ColorHelper.offWhite;
		}

		// Token: 0x04000F39 RID: 3897
		private Rarity rarity;

		// Token: 0x04000F3A RID: 3898
		private bool locked;

		// Token: 0x04000F3B RID: 3899
		private bool selected;

		// Token: 0x04000F3C RID: 3900
		[SerializeField]
		private TMP_Text textLabel;

		// Token: 0x04000F3D RID: 3901
		private ForgeTabContents forgeTabContents;
	}
}
