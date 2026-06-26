using System;
using Behaviour.Weapons;

namespace Source.Util
{
	// Token: 0x02000048 RID: 72
	public static class TargetLayerExtensions
	{
		// Token: 0x060002FD RID: 765 RVA: 0x00018625 File Offset: 0x00016825
		public static string GetName(this TargetLayer layer)
		{
			switch (layer)
			{
			case TargetLayer.Surface:
				return Translation.Translate("@Surface", Array.Empty<object>());
			case TargetLayer.Core:
				return Translation.Translate("@Core", Array.Empty<object>());
			}
			return string.Empty;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00018660 File Offset: 0x00016860
		public static string GetSalvageName(this TargetLayer layer)
		{
			if (layer == TargetLayer.Core)
			{
				return Translation.Translate("@Structural", Array.Empty<object>());
			}
			return layer.GetName();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001867C File Offset: 0x0001687C
		public static string GetName(this TargetLayer? layer)
		{
			if (layer == null)
			{
				return string.Empty;
			}
			return layer.Value.GetName();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00018699 File Offset: 0x00016899
		public static string GetSalvageName(this TargetLayer? layer)
		{
			if (layer == null)
			{
				return string.Empty;
			}
			return layer.Value.GetSalvageName();
		}
	}
}
