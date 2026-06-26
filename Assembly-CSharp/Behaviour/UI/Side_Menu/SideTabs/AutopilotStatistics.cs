using System;
using System.Collections.Generic;
using System.Linq;
using Source.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002BA RID: 698
	public class AutopilotStatistics : SideTabContent
	{
		// Token: 0x060019C0 RID: 6592 RVA: 0x000A0180 File Offset: 0x0009E380
		private void Awake()
		{
			float y = this.content.anchoredPosition.y;
			this.SetStats();
			Vector2 sizeDelta = (this.content.transform as RectTransform).sizeDelta;
			sizeDelta.y = this.contentHeight;
			(this.content.transform as RectTransform).sizeDelta = sizeDelta;
			this.content.anchoredPosition = new Vector2(this.content.anchoredPosition.x, -sizeDelta.y - y);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000A0206 File Offset: 0x0009E406
		private void Update()
		{
			if (this.trackAutopilotStatus != GamePlayer.current.autoPlay)
			{
				this.SetStats();
			}
			this.trackAutopilotStatus = GamePlayer.current.autoPlay;
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x000A0230 File Offset: 0x0009E430
		public void SetStats()
		{
			this.content.DestroyChildren();
			if (GamePlayer.current.currentAutopilotSessionStats != null)
			{
				this.CreateSessionStats(GamePlayer.current.currentAutopilotSessionStats, true);
			}
			for (int i = GamePlayer.current.autopilotSessionStats.Count - 1; i >= 0; i--)
			{
				AutopilotSessionStats stats = GamePlayer.current.autopilotSessionStats[i];
				this.CreateSessionStats(stats, GamePlayer.current.currentAutopilotSessionStats == null && i == GamePlayer.current.autopilotSessionStats.Count<AutopilotSessionStats>() - 1);
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000A02BC File Offset: 0x0009E4BC
		private void CreateSessionStats(AutopilotSessionStats stats, bool show = true)
		{
			AutopilotStatHeader autopilotStatHeader = UnityEngine.Object.Instantiate<AutopilotStatHeader>(this.headerButtonPrefab, this.content);
			autopilotStatHeader.SetStats(stats);
			autopilotStatHeader.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.ToggleStats(stats);
			});
			AutopilotSessionStatsPanel autopilotSessionStatsPanel = UnityEngine.Object.Instantiate<AutopilotSessionStatsPanel>(this.sessionStatsPanelPrefab, this.content);
			autopilotSessionStatsPanel.SetSessionStats(stats);
			autopilotSessionStatsPanel.gameObject.SetActive(show);
			AutopilotStatistics.sessionPanels[stats] = autopilotSessionStatsPanel;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x000A0350 File Offset: 0x0009E550
		private void ToggleStats(AutopilotSessionStats stats)
		{
			AutopilotStatistics.sessionPanels[stats].gameObject.SetActive(!AutopilotStatistics.sessionPanels[stats].gameObject.activeSelf);
		}

		// Token: 0x0400102C RID: 4140
		[SerializeField]
		private AutopilotStatHeader headerButtonPrefab;

		// Token: 0x0400102D RID: 4141
		[SerializeField]
		private AutopilotSessionStatsPanel sessionStatsPanelPrefab;

		// Token: 0x0400102E RID: 4142
		[SerializeField]
		private RectTransform content;

		// Token: 0x0400102F RID: 4143
		private float contentHeight;

		// Token: 0x04001030 RID: 4144
		private static Dictionary<AutopilotSessionStats, AutopilotSessionStatsPanel> sessionPanels = new Dictionary<AutopilotSessionStats, AutopilotSessionStatsPanel>();

		// Token: 0x04001031 RID: 4145
		private bool trackAutopilotStatus;
	}
}
