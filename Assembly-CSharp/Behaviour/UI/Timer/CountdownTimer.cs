using System;
using Source.Player;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Timer
{
	// Token: 0x02000210 RID: 528
	public class CountdownTimer : MonoBehaviour
	{
		// Token: 0x06001384 RID: 4996 RVA: 0x0007EB36 File Offset: 0x0007CD36
		private void Start()
		{
			this.gamePlayer = GamePlayer.current;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0007EB44 File Offset: 0x0007CD44
		private void Update()
		{
			int num = Mathf.FloorToInt(this.timer / 60f);
			int num2 = Mathf.FloorToInt(this.timer % 60f);
			this.timerText.text = string.Format("{0:00}:{1:00}", num, num2);
		}

		// Token: 0x04000B26 RID: 2854
		private GamePlayer gamePlayer;

		// Token: 0x04000B27 RID: 2855
		[SerializeField]
		private TextMeshProUGUI timerText;

		// Token: 0x04000B28 RID: 2856
		public float timer = 300f;
	}
}
