using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Player;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Main
{
	// Token: 0x02000268 RID: 616
	public class CrewView : MonoBehaviour
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x0008F338 File Offset: 0x0008D538
		public void PopulateCrew()
		{
			this.ship = GameplayManager.Instance.spaceShip;
			this.crewSlotHolder.DestroyChildren();
			int maxGrunts = this.ship.maxGrunts;
			this.amountLabel.text = this.ship.spaceShipData.crewData.totalCrew.ToString() + " / " + maxGrunts.ToString();
			int num = maxGrunts - this.ship.spaceShipData.crewData.totalCrew;
			foreach (KeyValuePair<string, int> keyValuePair in this.ship.spaceShipData.crewData.crew)
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					UnityEngine.Object.Instantiate<CrewSlot>(this.crewSlotPrefab, this.crewSlotHolder.transform).Init(this, keyValuePair.Key);
				}
			}
			for (int j = 0; j < num; j++)
			{
				UnityEngine.Object.Instantiate<CrewSlot>(this.crewSlotPrefab, this.crewSlotHolder.transform).Init(this, null);
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0008F478 File Offset: 0x0008D678
		public void JettisonCrew(Behaviour.Crew.Crew crew, int amount = 1)
		{
			GamePlayer.current.currentSpaceShip.RemoveCrew(crew.identifier, amount);
			Singleton<LootManager>.Instance.CreateCrewPod(crew.identifier, amount, GameplayManager.Instance.spaceShip.transform, true);
			this.PopulateCrew();
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0008F4B8 File Offset: 0x0008D6B8
		public void AddToCargo(Behaviour.Crew.Crew crew, int amount = 1)
		{
			GamePlayer.current.currentSpaceShip.RemoveCrew(crew.identifier, amount);
			InventoryItemType item = ItemBuilder.Get("CrewPod").CreateCrewPod(crew, amount);
			GamePlayer.current.currentSpaceShip.AddCargo(item, 1, false);
			this.PopulateCrew();
		}

		// Token: 0x04000DCB RID: 3531
		[SerializeField]
		private TMP_Text amountLabel;

		// Token: 0x04000DCC RID: 3532
		[SerializeField]
		private TMP_Text crewLabel;

		// Token: 0x04000DCD RID: 3533
		private SpaceShip ship;

		// Token: 0x04000DCE RID: 3534
		[SerializeField]
		private CrewSlot crewSlotPrefab;

		// Token: 0x04000DCF RID: 3535
		[SerializeField]
		private RectTransform crewSlotHolder;
	}
}
