using System;
using Source.Galaxy;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000336 RID: 822
	public class WorldMapPOITimer : MonoBehaviour
	{
		// Token: 0x06001F2C RID: 7980 RVA: 0x000B9A6B File Offset: 0x000B7C6B
		private void Awake()
		{
			this.text = base.GetComponent<TMP_Text>();
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000B9A7C File Offset: 0x000B7C7C
		private void Update()
		{
			int num = (int)this.poi.timeLeft;
			Color c;
			if (num < 60)
			{
				c = ColorHelper.reddish;
			}
			else if (num < 600)
			{
				c = Translation.highlightColor;
			}
			else
			{
				c = ColorHelper.greenish;
			}
			this.text.text = Translation.Highlight("@MapPOIExpires", c, new object[]
			{
				GameMath.FormatTime(num, true)
			});
		}

		// Token: 0x040012AD RID: 4781
		private TMP_Text text;

		// Token: 0x040012AE RID: 4782
		public MapPointOfInterest poi;
	}
}
