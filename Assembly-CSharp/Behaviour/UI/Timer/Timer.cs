using System;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Timer
{
	// Token: 0x02000211 RID: 529
	public class Timer : MonoBehaviour
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x0007EBA9 File Offset: 0x0007CDA9
		private void Start()
		{
			this.gamePlayer = GamePlayer.current;
			base.InvokeRepeating("UpdateTimerText", 0f, 1f);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0007EBCC File Offset: 0x0007CDCC
		private void UpdateTimerText()
		{
			double elapsedTime = this.gamePlayer.elapsedTime;
			this.timerText.text = GameMath.FormatTime((int)elapsedTime, true);
		}

		// Token: 0x04000B29 RID: 2857
		[SerializeField]
		private TextMeshProUGUI timerText;

		// Token: 0x04000B2A RID: 2858
		private GamePlayer gamePlayer;
	}
}
