using System;
using Behaviour.UI.Spacestation;
using Source.Galaxy.POI;
using UnityEngine;

namespace Behaviour.Crew.Skills
{
	// Token: 0x020003A8 RID: 936
	public class SkillRefreshTradeTerminal : MonoBehaviour
	{
		// Token: 0x0600240C RID: 9228 RVA: 0x000CB67D File Offset: 0x000C987D
		private void Start()
		{
			this.RefreshTradeTerminal();
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000CB685 File Offset: 0x000C9885
		private void OnDestroy()
		{
			this.RefreshTradeTerminal();
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000CB68D File Offset: 0x000C988D
		private void RefreshTradeTerminal()
		{
			if (SpaceStationInterior.instance && SpaceStationInterior.instance.currentTab == SpaceStationFacility.TradeTerminal)
			{
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.TradeTerminal, true);
			}
		}
	}
}
