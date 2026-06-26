using System;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x02000094 RID: 148
	public static class Decals
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x000319BC File Offset: 0x0002FBBC
		private static void EnsureLoaded()
		{
			if (Decals._all != null)
			{
				return;
			}
			Decals._all = new Dictionary<string, DecalDefinition>();
			DecalAsset[] array = Resources.LoadAll<DecalAsset>("Decals");
			for (int i = 0; i < array.Length; i++)
			{
				DecalDefinition decalDefinition = new DecalDefinition(array[i]);
				Decals._all[decalDefinition.identifier] = decalDefinition;
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00031A10 File Offset: 0x0002FC10
		public static DecalDefinition Get(string identifier)
		{
			Decals.EnsureLoaded();
			DecalDefinition result;
			Decals._all.TryGetValue(identifier, out result);
			return result;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00031A31 File Offset: 0x0002FC31
		public static IEnumerable<DecalDefinition> GetAll()
		{
			Decals.EnsureLoaded();
			return Decals._all.Values;
		}

		// Token: 0x040002E1 RID: 737
		private static Dictionary<string, DecalDefinition> _all;
	}
}
