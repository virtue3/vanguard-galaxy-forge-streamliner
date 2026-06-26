using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Util;
using Source.Galaxy;
using Source.MissionSystem;
using Source.MissionSystem.Rewards;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.NotificationAlert
{
	// Token: 0x02000253 RID: 595
	public class Notification : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x0008C066 File Offset: 0x0008A266
		private void Start()
		{
			base.StartCoroutine(this.FadeSequence());
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0008C075 File Offset: 0x0008A275
		private IEnumerator FadeSequence()
		{
			if (this.prompt)
			{
				yield break;
			}
			List<MissionReward> list = this.rewards;
			yield return new WaitForSecondsRealtime(this.displayTime);
			yield return this.FadeOut();
			Singleton<NotificationManager>.Instance.DestroyNotification();
			yield break;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0008C084 File Offset: 0x0008A284
		private IEnumerator FadeIn()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.unscaledDeltaTime;
				float alpha = Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(alpha);
				yield return null;
			}
			yield break;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0008C093 File Offset: 0x0008A293
		private IEnumerator FadeOut()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.unscaledDeltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(alpha);
				yield return null;
			}
			yield break;
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0008C0A4 File Offset: 0x0008A2A4
		private void SetAlpha(float alpha)
		{
			if (this.message != null)
			{
				this.message.color = this.message.color.WithAlpha(alpha);
			}
			if (this.subMessage != null)
			{
				this.subMessage.color = this.subMessage.color.WithAlpha(alpha);
			}
			if (this.spriteRenderer != null)
			{
				this.spriteRenderer.color = this.spriteRenderer.color.WithAlpha(alpha);
			}
			if (this.promptImage != null)
			{
				this.promptImage.color = this.promptImage.color.WithAlpha(alpha / 1.1f);
			}
			if (this.regularImage != null)
			{
				this.regularImage.color = this.regularImage.color.WithAlpha(alpha / 1.1f);
			}
			this.SetActiveRewardsAlpha(alpha);
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0008C198 File Offset: 0x0008A398
		private void SetActiveRewardsAlpha(float alpha)
		{
			foreach (NotificationReward notificationReward in this.activeNotificationRewards)
			{
				notificationReward.SetAlpha(alpha);
			}
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0008C1EC File Offset: 0x0008A3EC
		private void SetScale(float t)
		{
			base.transform.localScale = Vector3.Lerp(this.minScale, this.maxScale, t);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0008C20B File Offset: 0x0008A40B
		public void ToggleActive(bool toggle)
		{
			base.gameObject.SetActive(toggle);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0008C21C File Offset: 0x0008A41C
		public void Init(string text)
		{
			this.message.text = text;
			if (this.prompt)
			{
				this.regularImage.gameObject.SetActive(false);
				this.yesButton.gameObject.SetActive(true);
				this.noButton.gameObject.SetActive(true);
				this.promptImage.gameObject.SetActive(true);
				this.promptImage.raycastTarget = true;
				this.message.raycastTarget = true;
				return;
			}
			if (this.rewards != null)
			{
				this.InstantiateRewards();
				return;
			}
			this.regularImage.gameObject.SetActive(true);
			this.yesButton.gameObject.SetActive(false);
			this.noButton.gameObject.SetActive(false);
			this.promptImage.gameObject.SetActive(false);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0008C2ED File Offset: 0x0008A4ED
		public void SetDisplayTime(float time)
		{
			this.displayTime = time;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0008C2F6 File Offset: 0x0008A4F6
		public void SetIcon(Sprite icon)
		{
			this.spriteRenderer.sprite = icon;
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0008C304 File Offset: 0x0008A504
		public void SetPrompt(bool prompt)
		{
			this.prompt = prompt;
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0008C310 File Offset: 0x0008A510
		public void SetMissionCompleted(Mission m, List<MissionReward> rewards)
		{
			this.rewardsMission = m;
			this.rewards = rewards;
			if (this.rewardsMission == null)
			{
				return;
			}
			if (this.rewardsMission.completionText != "")
			{
				this.subMessage.text = "\"" + m.completionText + "\"";
				return;
			}
			this.subMessage.gameObject.SetActive(false);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0008C380 File Offset: 0x0008A580
		private void InstantiateRewards()
		{
			IEnumerable<MissionReward> enumerable = from reward in this.rewards
			where reward is Source.MissionSystem.Rewards.Item || reward is Credits || reward is Reputation || reward is Experience
			orderby reward is Source.MissionSystem.Rewards.Item descending, reward is Credits descending, reward is Reputation descending, reward is Experience descending
			select reward;
			float num = 10f;
			float num2 = 0f;
			List<RectTransform> list = new List<RectTransform>();
			foreach (MissionReward missionReward in enumerable)
			{
				NotificationReward notificationReward = UnityEngine.Object.Instantiate<NotificationReward>(this.notifactionRewardPrefab, this.rewardsContainer);
				notificationReward.transform.SetParent(this.rewardsContainer, false);
				Credits credits = missionReward as Credits;
				if (credits == null)
				{
					Experience experience = missionReward as Experience;
					if (experience == null)
					{
						Reputation reputation = missionReward as Reputation;
						if (reputation == null)
						{
							Source.MissionSystem.Rewards.Item item = missionReward as Source.MissionSystem.Rewards.Item;
							if (item != null)
							{
								notificationReward.Initialize(string.Format("{0} x{1}", Translation.Translate(item.item.displayName, Array.Empty<object>()), item.amount), item.item.rarity.GetColor());
								notificationReward.SetItem(item.item.icon);
							}
						}
						else
						{
							Faction faction = reputation.faction ?? this.rewardsMission.sourceFaction;
							notificationReward.Initialize(string.Format("{0} {1}", reputation.amount, Translation.Translate(faction.name, Array.Empty<object>())), ColorHelper.greenBadge);
							notificationReward.SetBadge(ColorHelper.green05, ColorHelper.greenBadge, "REP", notificationReward.badgeIcon);
						}
					}
					else
					{
						notificationReward.Initialize(string.Format("{0}", experience.amount), ColorHelper.purpleBadge);
						notificationReward.SetBadge(ColorHelper.purple, ColorHelper.purpleBadge, "EXP", notificationReward.badgeIcon);
					}
				}
				else
				{
					notificationReward.Initialize(string.Format("{0}", credits.amount), ColorHelper.orangeBadge);
					notificationReward.SetBadge(ColorHelper.orange75, ColorHelper.orangeBadge, "CR", notificationReward.badgeIcon);
				}
				RectTransform component = notificationReward.GetComponent<RectTransform>();
				num2 += component.sizeDelta.x;
				list.Add(component);
				this.activeNotificationRewards.Add(notificationReward);
			}
			num2 += num * (float)(list.Count - 1);
			float num3 = -num2 / 2f;
			for (int i = 0; i < list.Count; i++)
			{
				RectTransform rectTransform = list[i];
				float x = num3 + rectTransform.sizeDelta.x / 2f;
				rectTransform.anchoredPosition = new Vector2(x, 0f);
				num3 += rectTransform.sizeDelta.x + num;
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0008C6F0 File Offset: 0x0008A8F0
		public NotificationReward CreateNotificationReward(string text)
		{
			return new NotificationReward();
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0008C6F7 File Offset: 0x0008A8F7
		public void ShowReward()
		{
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0008C6F9 File Offset: 0x0008A8F9
		public void OnPointerDown(PointerEventData eventData)
		{
			Singleton<NotificationManager>.Instance.DestroyNotification();
		}

		// Token: 0x04000D2F RID: 3375
		[SerializeField]
		public TextMeshProUGUI message;

		// Token: 0x04000D30 RID: 3376
		[SerializeField]
		private TextMeshProUGUI subMessage;

		// Token: 0x04000D31 RID: 3377
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000D32 RID: 3378
		[SerializeField]
		private Image promptImage;

		// Token: 0x04000D33 RID: 3379
		[SerializeField]
		private Image regularImage;

		// Token: 0x04000D34 RID: 3380
		[SerializeField]
		private Button yesButton;

		// Token: 0x04000D35 RID: 3381
		[SerializeField]
		private Button noButton;

		// Token: 0x04000D36 RID: 3382
		[SerializeField]
		private RectTransform rewardsContainer;

		// Token: 0x04000D37 RID: 3383
		[SerializeField]
		private NotificationReward notifactionRewardPrefab;

		// Token: 0x04000D38 RID: 3384
		private float fadeDuration = 0.25f;

		// Token: 0x04000D39 RID: 3385
		private float displayTime;

		// Token: 0x04000D3A RID: 3386
		private bool prompt;

		// Token: 0x04000D3B RID: 3387
		private Mission rewardsMission;

		// Token: 0x04000D3C RID: 3388
		private List<MissionReward> rewards;

		// Token: 0x04000D3D RID: 3389
		private List<NotificationReward> activeNotificationRewards = new List<NotificationReward>();

		// Token: 0x04000D3E RID: 3390
		private Vector3 minScale = new Vector3(0.95f, 0.95f, 0.95f);

		// Token: 0x04000D3F RID: 3391
		private Vector3 maxScale = new Vector3(1f, 1f, 1f);
	}
}
