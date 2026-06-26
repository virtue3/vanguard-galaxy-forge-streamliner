using System;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A2 RID: 674
	public class CaptainConquestRanks : SideTabContent
	{
		// Token: 0x0600191E RID: 6430 RVA: 0x0009BF40 File Offset: 0x0009A140
		private void Start()
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller == null)
			{
				Debug.LogWarning("No Conquest.");
				return;
			}
			foreach (Faction faction in Faction.all)
			{
				if (faction != Faction.player && faction != Faction.amalgam && faction != Faction.fanatics && faction != Faction.holyRadicals && (Conquest.conquestFactions.Contains(faction) || faction == Faction.puppeteers))
				{
					UnityEngine.Object.Instantiate<FactionConquestRanks>(this.factionConquestRanksPrefab, this.container).Setup(faction, storyteller);
				}
			}
			base.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
		}

		// Token: 0x04000F8D RID: 3981
		[SerializeField]
		private FactionConquestRanks factionConquestRanksPrefab;

		// Token: 0x04000F8E RID: 3982
		[SerializeField]
		private Transform container;
	}
}
