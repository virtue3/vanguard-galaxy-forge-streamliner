using System;
using Behaviour.UI.Tooltip;
using Source.MissionSystem;
using Source.MissionSystem.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Missions
{
	// Token: 0x0200025B RID: 603
	public class MissionRewardRow : MonoBehaviour
	{
		// Token: 0x06001641 RID: 5697 RVA: 0x0008DBC8 File Offset: 0x0008BDC8
		public void SetReward(MissionReward mr)
		{
			Sprite rewardIcon = mr.rewardIcon;
			if (rewardIcon != null)
			{
				this.icon.sprite = rewardIcon;
			}
			else
			{
				this.icon.gameObject.SetActive(false);
			}
			this.text.TL(mr.rewardText, Array.Empty<object>());
			this.text.color = mr.rewardColor;
			Source.MissionSystem.Rewards.Item item = mr as Source.MissionSystem.Rewards.Item;
			if (item != null)
			{
				base.GetComponent<ItemTooltipSource>().SetItem(item.item, item.amount, true, ItemTooltipContext.InCarousel, false, null);
				return;
			}
			base.GetComponent<ItemTooltipSource>().enabled = false;
		}

		// Token: 0x04000D77 RID: 3447
		[SerializeField]
		private Image icon;

		// Token: 0x04000D78 RID: 3448
		[SerializeField]
		private TMP_Text text;
	}
}
