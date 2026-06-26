using System;
using System.Collections.Generic;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B8 RID: 696
	public class AutopilotSessionStatsPanel : MonoBehaviour
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0009FF4C File Offset: 0x0009E14C
		// (set) Token: 0x060019B5 RID: 6581 RVA: 0x0009FF54 File Offset: 0x0009E154
		public float height { get; private set; }

		// Token: 0x060019B6 RID: 6582 RVA: 0x0009FF5D File Offset: 0x0009E15D
		private void Awake()
		{
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0009FF5F File Offset: 0x0009E15F
		public void SetSessionStats(AutopilotSessionStats stats)
		{
			this.statistics = stats;
			this.UpdateStatistics();
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0009FF70 File Offset: 0x0009E170
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				base.transform.DestroyChildren();
				this.height = 0f;
				this.UpdateStatistics();
				this.updateTimer = 0.5f;
			}
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0009FFC4 File Offset: 0x0009E1C4
		private void UpdateStatistics()
		{
			foreach (KeyValuePair<IdleStat, int> keyValuePair in this.statistics.stats)
			{
				this.AddRow(Translation.Translate("@AutopilotStat" + keyValuePair.Key.ToString(), Array.Empty<object>()), GameMath.FormatNumber((float)keyValuePair.Value, -1));
			}
			Vector2 sizeDelta = (base.transform as RectTransform).sizeDelta;
			sizeDelta.y = this.height;
			(base.transform as RectTransform).sizeDelta = sizeDelta;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x000A0084 File Offset: 0x0009E284
		private void AddRow(string key, string value)
		{
			AutopilotStatRow autopilotStatRow = UnityEngine.Object.Instantiate<AutopilotStatRow>(this.statRowPrefab, base.transform);
			autopilotStatRow.SetInfo(key, value);
			autopilotStatRow.SetPosition(new Vector2(0f, -this.height));
			this.height += 30f;
		}

		// Token: 0x04001026 RID: 4134
		[SerializeField]
		private AutopilotStatRow statRowPrefab;

		// Token: 0x04001027 RID: 4135
		private AutopilotSessionStats statistics;

		// Token: 0x04001028 RID: 4136
		private float updateTimer = 0.5f;
	}
}
