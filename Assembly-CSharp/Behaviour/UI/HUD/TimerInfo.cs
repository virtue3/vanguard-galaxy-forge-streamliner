using System;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000287 RID: 647
	public class TimerInfo : MonoBehaviour
	{
		// Token: 0x060017C4 RID: 6084 RVA: 0x0009529F File Offset: 0x0009349F
		public void SetTimerEvent(float totalTime, string message, Action<int> hideCallback, int priority = 1)
		{
			this.totalTime = totalTime;
			this.timerLabel.text = message;
			this.timer = totalTime;
			this.priority = priority;
			this.hideCallback = hideCallback;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x000952CC File Offset: 0x000934CC
		private void Update()
		{
			if (this.timer > 0f)
			{
				this.timer -= Time.deltaTime;
				if (this.timer <= 0f)
				{
					this.hideCallback(this.priority);
				}
				this.timerProgress.localScale = new Vector3(this.timer / this.totalTime, 1f, 1f);
				if (this.secondsLabel)
				{
					this.secondsLabel.text = GameMath.FormatNumber((float)((int)this.timer), -1) + " s";
				}
			}
		}

		// Token: 0x04000EBC RID: 3772
		[SerializeField]
		private RectTransform timerProgress;

		// Token: 0x04000EBD RID: 3773
		[SerializeField]
		private TMP_Text timerLabel;

		// Token: 0x04000EBE RID: 3774
		[SerializeField]
		private TMP_Text secondsLabel;

		// Token: 0x04000EBF RID: 3775
		private float timer;

		// Token: 0x04000EC0 RID: 3776
		private float totalTime;

		// Token: 0x04000EC1 RID: 3777
		private int priority;

		// Token: 0x04000EC2 RID: 3778
		private Action<int> hideCallback;
	}
}
