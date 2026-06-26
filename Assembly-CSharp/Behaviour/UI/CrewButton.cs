using System;
using Behaviour.UI.ShipCarousel;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Item;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001EB RID: 491
	public class CrewButton : MonoBehaviour, IHighlightable, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060012A3 RID: 4771 RVA: 0x00079DA8 File Offset: 0x00077FA8
		public void SetCrew(CrewMemberData crew)
		{
			this.backgroundImage = base.GetComponent<Image>();
			this.normalMaterial = this.backgroundImage.material;
			this.currentCrew = crew;
			if (crew != null)
			{
				this.buttonIcon.sprite = crew.sprite;
				this.buttonIcon.preserveAspect = true;
				this.buttonIcon.gameObject.SetActive(true);
			}
			else
			{
				this.buttonIcon.gameObject.SetActive(false);
			}
			this.borderIcon.color = ColorHelper.boringGrey;
			this.SetRarityBackgroundColor();
			this.highlightMaterial.SetTexture("_MainTex", this.backgroundImage.sprite.texture);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00079E54 File Offset: 0x00078054
		protected void SetRarityBackgroundColor()
		{
			Color backgroundColor = Rarity.Standard.GetBackgroundColor();
			if (this.currentCrew != null)
			{
				backgroundColor = this.currentCrew.rarity.GetBackgroundColor();
			}
			this.backgroundImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.7f);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00079EA8 File Offset: 0x000780A8
		public void ShowHighlight()
		{
			this.backgroundImage.material = this.highlightMaterial;
			this.backgroundImage.color = this.highlightMaterial.GetColor("_Color");
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00079ED6 File Offset: 0x000780D6
		public void HideHighlight()
		{
			this.backgroundImage.material = this.normalMaterial;
			this.SetRarityBackgroundColor();
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00079EEF File Offset: 0x000780EF
		public void ClearCrew()
		{
			if (PersonalHangar.current && this.currentCrew != null)
			{
				PersonalHangar.current.selectedShipData.RemoveCrewMember(this.currentCrew);
				PersonalHangar.current.UpdateCrewButtons(true);
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00079F28 File Offset: 0x00078128
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			CrewMemberData crewMemberData = this.currentCrew;
			tooltip.AddTextLine(((crewMemberData != null) ? crewMemberData.GetFullName() : null) ?? "@UICrewEmpty", 14, 8f).Text.color = ColorHelper.offWhite;
			if (this.currentCrew != null)
			{
				tooltip.AddTextLine(Translation.Translate("@UICrewmemberDesc", new object[]
				{
					this.currentCrew.rarity.GetDisplayName(),
					this.currentCrew.profession.GetDisplayName()
				}), 12, 8f).Text.color = ColorHelper.modifierColor;
				tooltip.AddTextLine(Translation.Translate("@UIOnlyLevel", Array.Empty<object>()) + ": " + this.currentCrew.level.ToString().HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
				if (PersonalHangar.current)
				{
					tooltip.AddTextLine(Translation.Translate("@UIHangarCrewmemberUnequip", Array.Empty<object>()), 12, 8f);
				}
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0007A044 File Offset: 0x00078244
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				this.ClearCrew();
			}
		}

		// Token: 0x04000A6D RID: 2669
		private CrewMemberData currentCrew;

		// Token: 0x04000A6E RID: 2670
		protected Image backgroundImage;

		// Token: 0x04000A6F RID: 2671
		protected Material normalMaterial;

		// Token: 0x04000A70 RID: 2672
		[SerializeField]
		protected Image buttonIcon;

		// Token: 0x04000A71 RID: 2673
		[SerializeField]
		protected Image borderIcon;

		// Token: 0x04000A72 RID: 2674
		[SerializeField]
		protected Material highlightMaterial;

		// Token: 0x04000A73 RID: 2675
		public int index;
	}
}
