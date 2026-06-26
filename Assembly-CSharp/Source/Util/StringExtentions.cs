using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000046 RID: 70
	public static class StringExtentions
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x00018258 File Offset: 0x00016458
		public static string HighlightWithColor(this string text, Color color)
		{
			return string.Concat(new string[]
			{
				"<color=",
				RandomStuffHelper.ColorToHex(color),
				">",
				text,
				"</color>"
			});
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0001828A File Offset: 0x0001648A
		public static string WithBold(this string text)
		{
			return "<b>" + text + "</b>";
		}
	}
}
