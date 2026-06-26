using System;
using Behaviour.Item.Usable;
using Behaviour.Util;
using Source.NotificationAlert;
using UnityEngine;

namespace Behaviour.UI.NotificationAlert
{
	// Token: 0x02000254 RID: 596
	public class NotificationManager : Singleton<NotificationManager>
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001603 RID: 5635 RVA: 0x0008C765 File Offset: 0x0008A965
		// (set) Token: 0x06001604 RID: 5636 RVA: 0x0008C76D File Offset: 0x0008A96D
		public float displayTime { get; private set; } = 1f;

		// Token: 0x06001605 RID: 5637 RVA: 0x0008C776 File Offset: 0x0008A976
		public NotificationBuilder CreateNotification(string text)
		{
			this.DestroyNotification();
			this.CreateNotificationObject();
			return new NotificationBuilder(text, this);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0008C78C File Offset: 0x0008A98C
		private Notification CreateNotificationObject()
		{
			this.activeNotification = UnityEngine.Object.Instantiate<Notification>(this.notificationPrefab, base.transform);
			this.activeNotification.gameObject.SetActive(false);
			return this.activeNotification;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0008C7BC File Offset: 0x0008A9BC
		public Notification Get()
		{
			return this.activeNotification;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0008C7C4 File Offset: 0x0008A9C4
		public void DestroyNotification()
		{
			Notification notification = this.activeNotification;
			UnityEngine.Object.Destroy((notification != null) ? notification.gameObject : null);
			this.activeNotification = null;
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0008C7E4 File Offset: 0x0008A9E4
		public void ShowLootBox(LootBoxItem lootBoxItem)
		{
			if (this.lootBoxNotification)
			{
				UnityEngine.Object.Destroy(this.lootBoxNotification.gameObject);
			}
			this.lootBoxNotification = UnityEngine.Object.Instantiate<LootBoxNotification>(this.lootBoxNotificationPrefab, base.transform);
			this.lootBoxNotification.ShowLootBox(lootBoxItem);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0008C831 File Offset: 0x0008AA31
		public void Cleanup()
		{
			if (this.lootBoxNotification)
			{
				UnityEngine.Object.Destroy(this.lootBoxNotification.gameObject);
			}
		}

		// Token: 0x04000D40 RID: 3392
		private Notification activeNotification;

		// Token: 0x04000D41 RID: 3393
		private LootBoxNotification lootBoxNotification;

		// Token: 0x04000D42 RID: 3394
		[SerializeField]
		private Notification notificationPrefab;

		// Token: 0x04000D43 RID: 3395
		[SerializeField]
		private LootBoxNotification lootBoxNotificationPrefab;
	}
}
