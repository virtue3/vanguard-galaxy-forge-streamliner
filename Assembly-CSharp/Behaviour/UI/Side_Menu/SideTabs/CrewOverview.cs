using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Source.Player;
using Source.SpaceShip;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B1 RID: 689
	public class CrewOverview : SideTabContent
	{
		// Token: 0x0600198B RID: 6539 RVA: 0x0009F388 File Offset: 0x0009D588
		private void Start()
		{
			GamePlayer current = GamePlayer.current;
			foreach (Crew crew in Crew.all)
			{
				this.crewInShips[crew.identifier] = 0;
				this.crewNotAssigned[crew.identifier] = 0;
			}
			foreach (SpaceShipData spaceShipData in GamePlayer.current.spaceShips)
			{
				foreach (KeyValuePair<string, int> keyValuePair in spaceShipData.crewData.crew)
				{
					int num;
					if (this.crewInShips.TryGetValue(keyValuePair.Key, out num))
					{
						this.crewInShips[keyValuePair.Key] = num + keyValuePair.Value;
					}
					else
					{
						this.crewInShips[keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
			foreach (KeyValuePair<string, int> keyValuePair2 in this.crewNotAssigned)
			{
				UnityEngine.Object.Instantiate<CrewTypeInfo>(this.crewTypeInfo, this.crewTypeInfoHolder).Init(keyValuePair2.Key, this.GetTotalCrewCount(keyValuePair2.Key), this.GetActiveCrewCount(keyValuePair2.Key));
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0009F53C File Offset: 0x0009D73C
		private int GetUnassignedCrewCount(string type)
		{
			return this.crewNotAssigned[type];
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0009F54A File Offset: 0x0009D74A
		private int GetActiveCrewCount(string type)
		{
			return this.crewInShips[type];
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0009F558 File Offset: 0x0009D758
		private int GetTotalCrewCount(string type)
		{
			return this.crewInShips[type] + this.crewNotAssigned[type];
		}

		// Token: 0x04001003 RID: 4099
		[SerializeField]
		private TMP_Text totalCrew;

		// Token: 0x04001004 RID: 4100
		[SerializeField]
		private CrewTypeInfo crewTypeInfo;

		// Token: 0x04001005 RID: 4101
		[SerializeField]
		private RectTransform crewTypeInfoHolder;

		// Token: 0x04001006 RID: 4102
		private Dictionary<string, int> crewInShips = new Dictionary<string, int>();

		// Token: 0x04001007 RID: 4103
		private Dictionary<string, int> crewNotAssigned = new Dictionary<string, int>();
	}
}
