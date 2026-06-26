using System;
using Behaviour.Ability;
using Behaviour.UI.Tooltip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000277 RID: 631
	public class AbilitySelectable : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600171D RID: 5917 RVA: 0x00092372 File Offset: 0x00090572
		public void SetAbility(ActivatedAbility aa)
		{
			this.ability = aa;
			this.icon.sprite = aa.icon;
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0009238C File Offset: 0x0009058C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.ability.displayName, 14, 8f);
			tooltip.AddTextLine(this.ability.descriptionText, 12, 8f);
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000923C0 File Offset: 0x000905C0
		public void OnPointerClick(PointerEventData eventData)
		{
			base.GetComponentInParent<AbilityButton>().NewAbilitySelected(this.ability);
		}

		// Token: 0x04000E36 RID: 3638
		[SerializeField]
		private Image icon;

		// Token: 0x04000E37 RID: 3639
		private ActivatedAbility ability;
	}
}
