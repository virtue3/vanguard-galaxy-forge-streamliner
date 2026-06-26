using System;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Hangar
{
	// Token: 0x02000234 RID: 564
	public class DecalSlotButton : MonoBehaviour
	{
		// Token: 0x06001523 RID: 5411 RVA: 0x000888E8 File Offset: 0x00086AE8
		public void Setup(int slotIndex, string equippedDecalId, Action<int> onClick)
		{
			this.slotIndex = slotIndex;
			this.onClick = onClick;
			base.GetComponent<Button>().onClick.RemoveAllListeners();
			base.GetComponent<Button>().onClick.AddListener(delegate()
			{
				Action<int> action = this.onClick;
				if (action == null)
				{
					return;
				}
				action(this.slotIndex);
			});
			DecalDefinition decalDefinition = string.IsNullOrEmpty(equippedDecalId) ? null : Decals.Get(equippedDecalId);
			if (decalDefinition != null)
			{
				Sprite sprite = decalDefinition.GetSprite();
				this.icon.sprite = sprite;
				this.icon.enabled = (sprite != null);
				if (this.label)
				{
					this.label.text = decalDefinition.displayName;
				}
			}
			else
			{
				this.icon.enabled = false;
				if (this.label)
				{
					this.label.text = "+";
				}
			}
			if (this.background)
			{
				this.background.color = Color.white;
			}
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000889CF File Offset: 0x00086BCF
		public void SetHighlight(bool on)
		{
			if (!this.background)
			{
				return;
			}
			this.background.enabled = on;
			if (on)
			{
				this.background.color = ColorHelper.buffBorder;
			}
		}

		// Token: 0x04000C78 RID: 3192
		[SerializeField]
		private Image icon;

		// Token: 0x04000C79 RID: 3193
		[SerializeField]
		private Image background;

		// Token: 0x04000C7A RID: 3194
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000C7B RID: 3195
		private int slotIndex;

		// Token: 0x04000C7C RID: 3196
		private Action<int> onClick;
	}
}
