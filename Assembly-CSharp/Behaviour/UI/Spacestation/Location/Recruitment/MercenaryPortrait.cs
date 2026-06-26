using System;
using Source.Crew;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location.Recruitment
{
	// Token: 0x0200022F RID: 559
	public class MercenaryPortrait : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x06001501 RID: 5377 RVA: 0x00087E90 File Offset: 0x00086090
		public void SetMercenary(Mercenary mercenary)
		{
			this.mercenary = mercenary;
			this.icon.sprite = mercenary.icon.sprite;
			this.callSign.text = mercenary.callsign;
			this.border.color = this.mercenary.rarity.GetColor();
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00087EE6 File Offset: 0x000860E6
		public void SetRightClickCallback(Action callback)
		{
			this.rightClickCallback = callback;
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00087EEF File Offset: 0x000860EF
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.border.color = this.highlightColor;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00087F02 File Offset: 0x00086102
		public void OnPointerExit(PointerEventData eventData)
		{
			this.border.color = this.mercenary.rarity.GetColor();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00087F1F File Offset: 0x0008611F
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				this.rightClickCallback();
			}
		}

		// Token: 0x04000C57 RID: 3159
		[SerializeField]
		private Image icon;

		// Token: 0x04000C58 RID: 3160
		[SerializeField]
		private Image border;

		// Token: 0x04000C59 RID: 3161
		[SerializeField]
		private TMP_Text callSign;

		// Token: 0x04000C5A RID: 3162
		[SerializeField]
		private Color highlightColor;

		// Token: 0x04000C5B RID: 3163
		private Mercenary mercenary;

		// Token: 0x04000C5C RID: 3164
		private Action rightClickCallback;
	}
}
