using System;

namespace Source.Util
{
	// Token: 0x0200002E RID: 46
	public static class FloatCompare
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00011B27 File Offset: 0x0000FD27
		public static bool ApproximatelyEqual(this float current, float compareTo)
		{
			return ((double)current).ApproximatelyEqual((double)compareTo);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00011B34 File Offset: 0x0000FD34
		public static bool ApproximatelyEqual(this double current, double compareTo)
		{
			double num = Math.Max(Math.Abs(current), Math.Abs(compareTo)) * 1E-15;
			return Math.Abs(current - compareTo) <= num;
		}
	}
}
