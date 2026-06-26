using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000028 RID: 40
	public static class ColorExtensions
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000DB26 File Offset: 0x0000BD26
		public static Color WithAlpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
	}
}
