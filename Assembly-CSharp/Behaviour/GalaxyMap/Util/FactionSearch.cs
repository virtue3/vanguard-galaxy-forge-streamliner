using System;
using Source.Galaxy;
using UnityEngine;

namespace Behaviour.GalaxyMap.Util
{
	// Token: 0x02000339 RID: 825
	public class FactionSearch : MonoBehaviour
	{
		// Token: 0x06001F3C RID: 7996 RVA: 0x000BA344 File Offset: 0x000B8544
		public void FilterByFaction(Faction faction)
		{
			foreach (WorldMapStatic worldMapStatic in AbstractGalaxyMapManager.current.GetWorldMapStatics())
			{
				bool highlight = worldMapStatic.factionIcons.HasFaction(faction);
				worldMapStatic.Highlight(highlight);
			}
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x000BA380 File Offset: 0x000B8580
		public void ClearFilter()
		{
			WorldMapStatic[] worldMapStatics = AbstractGalaxyMapManager.current.GetWorldMapStatics();
			for (int i = 0; i < worldMapStatics.Length; i++)
			{
				worldMapStatics[i].Highlight(false);
			}
		}
	}
}
