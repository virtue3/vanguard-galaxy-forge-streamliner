using System;
using Source.Crew;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000260 RID: 608
	public class PersonalHistoryButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x0008E9E4 File Offset: 0x0008CBE4
		private void Awake()
		{
			this.defaultColor = this.background.color;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0008E9F7 File Offset: 0x0008CBF7
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.disabled)
			{
				return;
			}
			base.GetComponentInParent<PersonalHistoryStep>().OnChangePersonalHistory(this.personalHistory);
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0008EA13 File Offset: 0x0008CC13
		internal void SetPersonalHistory(PersonalHistory personalHistory)
		{
			this.personalHistory = personalHistory;
			this.text.TL(personalHistory.GetDisplayName(), Array.Empty<object>());
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0008EA32 File Offset: 0x0008CC32
		public void SetLocked(bool locked)
		{
			this.text.color = (locked ? ColorHelper.reddish : ColorHelper.greenish);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0008EA4E File Offset: 0x0008CC4E
		public void SetSelected(PersonalHistory selected)
		{
			this.background.color = ((this.personalHistory == selected) ? this.selectedColor : this.defaultColor);
		}

		// Token: 0x04000DA2 RID: 3490
		[SerializeField]
		private Image background;

		// Token: 0x04000DA3 RID: 3491
		[SerializeField]
		private TMP_Text text;

		// Token: 0x04000DA4 RID: 3492
		[SerializeField]
		private Color selectedColor;

		// Token: 0x04000DA5 RID: 3493
		private Color defaultColor;

		// Token: 0x04000DA6 RID: 3494
		private PersonalHistory personalHistory;

		// Token: 0x04000DA7 RID: 3495
		public bool disabled;
	}
}
