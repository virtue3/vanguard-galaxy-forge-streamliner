using System;
using System.Collections.Generic;
using Behaviour.UI.NotificationAlert;
using Source.MissionSystem;
using UnityEngine;

namespace Source.NotificationAlert
{
	// Token: 0x020000A0 RID: 160
	public class NotificationBuilder
	{
		// Token: 0x06000672 RID: 1650 RVA: 0x00036FD2 File Offset: 0x000351D2
		public NotificationBuilder(string text, NotificationManager notificationManager)
		{
			this.text = text;
			this.notificationManager = notificationManager;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00036FE8 File Offset: 0x000351E8
		public NotificationBuilder WithColor(Color color)
		{
			this.color = color;
			return this;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00036FF2 File Offset: 0x000351F2
		public NotificationBuilder WithIcon(Sprite icon)
		{
			this.icon = icon;
			return this;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00036FFC File Offset: 0x000351FC
		public NotificationBuilder WithCustomTime(float time)
		{
			this.displayTime = new float?(time);
			return this;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0003700B File Offset: 0x0003520B
		public NotificationBuilder WithPrompt()
		{
			this.isPrompt = true;
			return this;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00037015 File Offset: 0x00035215
		public NotificationBuilder WithMissionRewards(Mission m, List<MissionReward> rewards)
		{
			this.rewardsMission = m;
			this.rewards = rewards;
			return this;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00037028 File Offset: 0x00035228
		public void Show()
		{
			Notification notification = this.notificationManager.Get();
			notification.SetPrompt(this.isPrompt);
			notification.SetMissionCompleted(this.rewardsMission, this.rewards);
			notification.Init(this.text);
			notification.SetDisplayTime(this.displayTime ?? this.notificationManager.displayTime);
			Color color = this.color;
			notification.message.color = this.color;
			if (this.icon != null)
			{
				notification.SetIcon(this.icon);
			}
			notification.ToggleActive(true);
		}

		// Token: 0x04000390 RID: 912
		private readonly NotificationManager notificationManager;

		// Token: 0x04000391 RID: 913
		private string text;

		// Token: 0x04000392 RID: 914
		private Color color;

		// Token: 0x04000393 RID: 915
		private Sprite icon;

		// Token: 0x04000394 RID: 916
		private float? displayTime;

		// Token: 0x04000395 RID: 917
		private bool isPrompt;

		// Token: 0x04000396 RID: 918
		private Mission rewardsMission;

		// Token: 0x04000397 RID: 919
		private List<MissionReward> rewards;
	}
}
