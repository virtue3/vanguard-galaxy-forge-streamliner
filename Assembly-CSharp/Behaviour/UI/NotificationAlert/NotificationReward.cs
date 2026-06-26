using System;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.NotificationAlert
{
	// Token: 0x02000255 RID: 597
	public class NotificationReward : MonoBehaviour
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x0008C863 File Offset: 0x0008AA63
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x0008C86B File Offset: 0x0008AA6B
		public Sprite badgeIcon { get; private set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x0008C874 File Offset: 0x0008AA74
		// (set) Token: 0x0600160F RID: 5647 RVA: 0x0008C87C File Offset: 0x0008AA7C
		public TextMeshProUGUI badgeText { get; private set; }

		// Token: 0x06001610 RID: 5648 RVA: 0x0008C885 File Offset: 0x0008AA85
		public void Initialize(string text, Color color)
		{
			this.rewardText.text = text;
			this.rewardText.color = color;
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0008C8A0 File Offset: 0x0008AAA0
		public void SetBadge(Color badgeColor, Color badgeTextColor, string badgeText, Sprite sprite = null)
		{
			this.rewardImage.sprite = sprite;
			this.rewardImage.color = badgeColor;
			this.badgeText.gameObject.SetActive(true);
			this.badgeText.text = badgeText;
			this.badgeText.color = badgeTextColor;
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0008C8EF File Offset: 0x0008AAEF
		public void SetItem(Sprite sprite)
		{
			this.rewardImage.sprite = sprite;
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0008C900 File Offset: 0x0008AB00
		public void SetAlpha(float alpha)
		{
			this.rewardImage.color = this.rewardImage.color.WithAlpha(alpha / 1.1f);
			this.rewardText.color = this.rewardText.color.WithAlpha(alpha / 1.1f);
			this.badgeText.color = this.badgeText.color.WithAlpha(alpha / 1.1f);
		}

		// Token: 0x04000D45 RID: 3397
		[SerializeField]
		private Image rewardImage;

		// Token: 0x04000D46 RID: 3398
		[SerializeField]
		private TextMeshProUGUI rewardText;
	}
}
