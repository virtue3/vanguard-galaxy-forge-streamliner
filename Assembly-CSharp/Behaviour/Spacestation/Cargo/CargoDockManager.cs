using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Source.Galaxy.POI;
using Source.Item;
using UnityEngine;

namespace Behaviour.Spacestation.Cargo
{
	// Token: 0x020002EA RID: 746
	public class CargoDockManager : MonoBehaviour
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x000A66EA File Offset: 0x000A48EA
		// (set) Token: 0x06001B3C RID: 6972 RVA: 0x000A66F1 File Offset: 0x000A48F1
		public static CargoDockManager Instance { get; private set; }

		// Token: 0x06001B3D RID: 6973 RVA: 0x000A66F9 File Offset: 0x000A48F9
		private void Awake()
		{
			this.SetDocks();
			CargoDockManager.Instance = this;
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000A6707 File Offset: 0x000A4907
		private void Update()
		{
			this.HandleDockUpdateTimer();
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x000A6710 File Offset: 0x000A4910
		public void SetDockTypes()
		{
			CargoDock[] componentsInChildren = base.GetComponentsInChildren<CargoDock>();
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			componentsInChildren[0].SetDockType(CargoDock.CargoDockType.Materials);
			if (componentsInChildren.Length > 1)
			{
				componentsInChildren[1].SetDockType(CargoDock.CargoDockType.GeneralShop);
			}
			CargoDock.CargoDockType[] array = new CargoDock.CargoDockType[]
			{
				CargoDock.CargoDockType.MiningShop,
				CargoDock.CargoDockType.SalvageShop,
				CargoDock.CargoDockType.BountyShop
			};
			for (int i = 2; i < componentsInChildren.Length; i++)
			{
				CargoDock.CargoDockType dockType = array[SeededRandom.Global.RandomRange(0, array.Length)];
				componentsInChildren[i].SetDockType(dockType);
			}
			CargoDock[] array2 = componentsInChildren;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].SetDockNumberVisual();
			}
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000A679C File Offset: 0x000A499C
		public void SetDocks()
		{
			foreach (CargoDock cargoDock in base.GetComponentsInChildren<CargoDock>())
			{
				foreach (object obj in Enum.GetValues(typeof(CargoDock.CargoDockType)))
				{
					CargoDock.CargoDockType cargoDockType = (CargoDock.CargoDockType)obj;
					if (cargoDockType != CargoDock.CargoDockType.None && (cargoDock.dockType & cargoDockType) != CargoDock.CargoDockType.None)
					{
						if (!this.docksByType.ContainsKey(cargoDockType))
						{
							this.docksByType[cargoDockType] = new List<CargoDock>();
						}
						this.docksByType[cargoDockType].Add(cargoDock);
					}
				}
				cargoDock.dockNumber = this.docksByType.SelectMany((KeyValuePair<CargoDock.CargoDockType, List<CargoDock>> kvp) => kvp.Value).Distinct<CargoDock>().ToList<CargoDock>().IndexOf(cargoDock) + 1;
			}
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x000A68A0 File Offset: 0x000A4AA0
		private void HandleDockUpdateTimer()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.DistributeAndResetTimer();
			}
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x000A68C8 File Offset: 0x000A4AC8
		public void Distribute(CargoDock.CargoDockType type, IEnumerable<Inventory.InventoryItem> items)
		{
			if (items == null)
			{
				return;
			}
			List<CargoDock> list = new List<CargoDock>();
			foreach (KeyValuePair<CargoDock.CargoDockType, List<CargoDock>> keyValuePair in this.docksByType)
			{
				if ((type & keyValuePair.Key) != CargoDock.CargoDockType.None)
				{
					list.AddRange(keyValuePair.Value);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			List<Inventory.InventoryItem> list2 = (from i in items
			group i by i.item into g
			select new Inventory.InventoryItem(g.Key, g.First<Inventory.InventoryItem>().inventory, g.First<Inventory.InventoryItem>().slot, g.Sum((Inventory.InventoryItem i) => i.count), g.First<Inventory.InventoryItem>().canBuyback)).ToList<Inventory.InventoryItem>();
			foreach (CargoDock cargoDock in list)
			{
				if (!list2.Any<Inventory.InventoryItem>())
				{
					break;
				}
				cargoDock.AssignItems(list2);
			}
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x000A69D8 File Offset: 0x000A4BD8
		public void DistributeAndResetTimer()
		{
			this.DistributeDocks();
			this.updateTimer = 2f;
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000A69EC File Offset: 0x000A4BEC
		private void DistributeDocks()
		{
			CargoDock.CargoDockType type = CargoDock.CargoDockType.Materials;
			Inventory materials = Inventory.materials;
			this.Distribute(type, (materials != null) ? materials.items : null);
			CargoDock.CargoDockType type2 = CargoDock.CargoDockType.Global;
			Inventory global = Inventory.global;
			this.Distribute(type2, (global != null) ? global.items : null);
			CargoDock.CargoDockType type3 = CargoDock.CargoDockType.GeneralShop;
			SpaceStation current = SpaceStation.current;
			IEnumerable<Inventory.InventoryItem> items;
			if (current == null)
			{
				items = null;
			}
			else
			{
				ShopInventory generalShopInventory = current.generalShopInventory;
				items = ((generalShopInventory != null) ? generalShopInventory.items : null);
			}
			this.Distribute(type3, items);
			CargoDock.CargoDockType type4 = CargoDock.CargoDockType.MiningShop;
			SpaceStation current2 = SpaceStation.current;
			IEnumerable<Inventory.InventoryItem> items2;
			if (current2 == null)
			{
				items2 = null;
			}
			else
			{
				ShopInventory miningShopInventory = current2.miningShopInventory;
				items2 = ((miningShopInventory != null) ? miningShopInventory.items : null);
			}
			this.Distribute(type4, items2);
			CargoDock.CargoDockType type5 = CargoDock.CargoDockType.SalvageShop;
			SpaceStation current3 = SpaceStation.current;
			IEnumerable<Inventory.InventoryItem> items3;
			if (current3 == null)
			{
				items3 = null;
			}
			else
			{
				ShopInventory salvageShopInventory = current3.salvageShopInventory;
				items3 = ((salvageShopInventory != null) ? salvageShopInventory.items : null);
			}
			this.Distribute(type5, items3);
			CargoDock.CargoDockType type6 = CargoDock.CargoDockType.BountyShop;
			SpaceStation current4 = SpaceStation.current;
			IEnumerable<Inventory.InventoryItem> items4;
			if (current4 == null)
			{
				items4 = null;
			}
			else
			{
				ShopInventory bountyShopInventory = current4.bountyShopInventory;
				items4 = ((bountyShopInventory != null) ? bountyShopInventory.items : null);
			}
			this.Distribute(type6, items4);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x000A6ABB File Offset: 0x000A4CBB
		private void OnDestroy()
		{
			if (CargoDockManager.Instance == this)
			{
				CargoDockManager.Instance = null;
			}
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000A6AD0 File Offset: 0x000A4CD0
		public void RemoveCargoDock(CargoDock cargoDock)
		{
			List<CargoDock> list;
			if (this.docksByType.TryGetValue(cargoDock.dockType, out list))
			{
				list.Remove(cargoDock);
				if (list.Count == 0)
				{
					this.docksByType.Remove(cargoDock.dockType);
				}
			}
		}

		// Token: 0x0400111B RID: 4379
		private Dictionary<CargoDock.CargoDockType, List<CargoDock>> docksByType = new Dictionary<CargoDock.CargoDockType, List<CargoDock>>();

		// Token: 0x0400111D RID: 4381
		private float updateTimer;
	}
}
