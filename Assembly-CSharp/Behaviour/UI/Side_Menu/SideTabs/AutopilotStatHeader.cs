using System;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B9 RID: 697
	public class AutopilotStatHeader : MonoBehaviour
	{
		// Token: 0x060019BC RID: 6588 RVA: 0x000A00E5 File Offset: 0x0009E2E5
		public void SetStats(AutopilotSessionStats stats)
		{
			this.statistics = stats;
			this.UpdateHeader();
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000A00F4 File Offset: 0x0009E2F4
		private void UpdateHeader()
		{
			string str = (this.statistics.shipActivity != null) ? (" - " + this.statistics.shipActivity) : "";
			this.header.text = this.statistics.shipName + str + " - " + GameMath.FormatTime(this.statistics.GetTotalTime(), true);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000A015D File Offset: 0x0009E35D
		private void Update()
		{
			if (this.statistics == GamePlayer.current.currentAutopilotSessionStats)
			{
				this.UpdateHeader();
			}
		}

		// Token: 0x0400102A RID: 4138
		[SerializeField]
		private TMP_Text header;

		// Token: 0x0400102B RID: 4139
		private AutopilotSessionStats statistics;
	}
}
