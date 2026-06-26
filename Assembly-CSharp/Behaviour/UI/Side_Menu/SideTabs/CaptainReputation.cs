using System;
using Source.Galaxy;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A7 RID: 679
	public class CaptainReputation : SideTabContent
	{
		// Token: 0x06001940 RID: 6464 RVA: 0x0009D0FC File Offset: 0x0009B2FC
		private void Start()
		{
			foreach (Faction faction in Faction.all)
			{
				if (faction != Faction.player && faction != Faction.amalgam && faction != Faction.fanatics && faction != Faction.holyRadicals && faction.GetReputation(Faction.player) != 0)
				{
					UnityEngine.Object.Instantiate<FactionReputation>(this.factionReputationPrefab, this.container).Setup(faction);
				}
			}
			base.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
		}

		// Token: 0x04000FAE RID: 4014
		[SerializeField]
		private FactionReputation factionReputationPrefab;

		// Token: 0x04000FAF RID: 4015
		[SerializeField]
		private Transform container;
	}
}
