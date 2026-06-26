using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.Managers
{
	// Token: 0x02000303 RID: 771
	public class EventLogManager : Singleton<EventLogManager>
	{
		// Token: 0x06001C80 RID: 7296 RVA: 0x000AC00C File Offset: 0x000AA20C
		public void NewEvent(string key, string log)
		{
			EventLogManager.LogEntry newEntry = new EventLogManager.LogEntry(key, log, this.InstantiateEvent());
			this.AddActiveLogEntry(newEntry);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000AC02E File Offset: 0x000AA22E
		private void AddActiveLogEntry(EventLogManager.LogEntry newEntry)
		{
			this.activeLogEntries.Add(newEntry);
			if (this.logEntryPrefab == null)
			{
				return;
			}
			base.StartCoroutine(this.RemoveAfterTime(newEntry, 5f));
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000AC05E File Offset: 0x000AA25E
		private IEnumerator RemoveAfterTime(EventLogManager.LogEntry entry, float delay)
		{
			yield return new WaitForSeconds(delay);
			base.StartCoroutine(entry.FadeOut());
			yield return new WaitUntil(() => entry.IsFadedOut);
			this.activeLogEntries.Remove(entry);
			yield break;
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000AC07B File Offset: 0x000AA27B
		private TextMeshProUGUI InstantiateEvent()
		{
			if (this.logEntryPrefab == null)
			{
				return null;
			}
			return UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.logEntryPrefab, this.eventLogContainer);
		}

		// Token: 0x040011B0 RID: 4528
		[SerializeField]
		private Transform eventLogContainer;

		// Token: 0x040011B1 RID: 4529
		[SerializeField]
		private TextMeshProUGUI logEntryPrefab;

		// Token: 0x040011B2 RID: 4530
		private List<EventLogManager.LogEntry> activeLogEntries = new List<EventLogManager.LogEntry>();

		// Token: 0x040011B3 RID: 4531
		private List<EventLogManager.LogEntry> logHistory = new List<EventLogManager.LogEntry>();

		// Token: 0x0200059A RID: 1434
		private class LogEntry
		{
			// Token: 0x17000711 RID: 1809
			// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x000F3F15 File Offset: 0x000F2115
			// (set) Token: 0x06002DA4 RID: 11684 RVA: 0x000F3F1D File Offset: 0x000F211D
			public string key { get; protected set; }

			// Token: 0x17000712 RID: 1810
			// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x000F3F26 File Offset: 0x000F2126
			// (set) Token: 0x06002DA6 RID: 11686 RVA: 0x000F3F2E File Offset: 0x000F212E
			public bool IsFadedOut { get; protected set; }

			// Token: 0x06002DA7 RID: 11687 RVA: 0x000F3F37 File Offset: 0x000F2137
			public LogEntry(string key, string log, TextMeshProUGUI logText)
			{
				this.key = key;
				this.log = log;
				this.logText = logText;
				this.UpdateLogText();
			}

			// Token: 0x06002DA8 RID: 11688 RVA: 0x000F3F5A File Offset: 0x000F215A
			protected virtual void UpdateLogText()
			{
				if (this.logText == null)
				{
					return;
				}
				this.logText.text = this.log;
			}

			// Token: 0x06002DA9 RID: 11689 RVA: 0x000F3F7C File Offset: 0x000F217C
			public IEnumerator FadeOut()
			{
				this.IsFadedOut = false;
				Color originalColor = this.logText.color;
				float fadeDuration = 1f;
				float elapsed = 0f;
				while (elapsed < fadeDuration)
				{
					elapsed += Time.deltaTime;
					float a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
					this.logText.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);
					yield return null;
				}
				UnityEngine.Object.Destroy(this.logText.gameObject);
				this.IsFadedOut = true;
				yield break;
			}

			// Token: 0x04001D0D RID: 7437
			protected TextMeshProUGUI logText;

			// Token: 0x04001D0E RID: 7438
			protected string log;
		}

		// Token: 0x0200059B RID: 1435
		private class ItemLogEntry : EventLogManager.LogEntry
		{
			// Token: 0x06002DAA RID: 11690 RVA: 0x000F3F8B File Offset: 0x000F218B
			public ItemLogEntry(string key, int amount, TextMeshProUGUI logText) : base(key, "", logText)
			{
				base.key = key;
				this.amount = amount;
				this.UpdateLogText();
			}

			// Token: 0x06002DAB RID: 11691 RVA: 0x000F3FAE File Offset: 0x000F21AE
			public void UpdateAmount(int additionalAmount)
			{
				this.amount += additionalAmount;
				this.UpdateLogText();
			}

			// Token: 0x06002DAC RID: 11692 RVA: 0x000F3FC4 File Offset: 0x000F21C4
			protected override void UpdateLogText()
			{
				if (this.logText == null)
				{
					return;
				}
				this.logText.TL("@LogEntryItemPickedUp", new object[]
				{
					this.amount,
					base.key
				});
			}

			// Token: 0x04001D10 RID: 7440
			private int amount;
		}
	}
}
