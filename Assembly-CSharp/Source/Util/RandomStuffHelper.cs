using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200003B RID: 59
	public class RandomStuffHelper
	{
		// Token: 0x060002AC RID: 684 RVA: 0x000167B0 File Offset: 0x000149B0
		public static Vector2 GetRandomVector2NearVector2(Vector2 center, float range)
		{
			float x = center.x + UnityEngine.Random.Range(-range, range);
			float y = center.y + UnityEngine.Random.Range(-range, range);
			return new Vector2(x, y);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000167E4 File Offset: 0x000149E4
		public static string ColorToHex(Color color)
		{
			Color32 color2 = color;
			return "#" + color2.r.ToString("X2") + color2.g.ToString("X2") + color2.b.ToString("X2");
		}
	}
}
