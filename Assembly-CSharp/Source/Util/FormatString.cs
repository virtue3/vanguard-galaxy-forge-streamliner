using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200002F RID: 47
	public class FormatString
	{
		// Token: 0x0600024B RID: 587 RVA: 0x00011B6C File Offset: 0x0000FD6C
		public static string FormatTime(float totalSeconds)
		{
			int num = Mathf.FloorToInt(totalSeconds / 3600f);
			int num2 = Mathf.FloorToInt(totalSeconds % 3600f / 60f);
			int num3 = Mathf.FloorToInt(totalSeconds % 60f);
			return string.Format("{0:00}:{1:00}:{2:00}", num, num2, num3);
		}
	}
}
