using System;

namespace Source.SpaceShip
{
	// Token: 0x02000059 RID: 89
	public static class SizesExtensions
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0001D1EE File Offset: 0x0001B3EE
		public static string GetDisplayName(this ModuleSize size)
		{
			return "@ModuleSize" + size.ToString();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001D207 File Offset: 0x0001B407
		public static string GetShortDisplayName(this ModuleSize size)
		{
			return "@ModuleSizeShort" + size.ToString();
		}
	}
}
