using System;
using System.Collections.Generic;
using System.Linq;
using Source.Galaxy;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.Spacestation.Cargo
{
	// Token: 0x020002E9 RID: 745
	public class CargoDock : MonoBehaviour
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000A60D0 File Offset: 0x000A42D0
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x000A60D8 File Offset: 0x000A42D8
		public CargoDock.CargoDockType dockType { get; private set; }

		// Token: 0x06001B2B RID: 6955 RVA: 0x000A60E4 File Offset: 0x000A42E4
		private void Awake()
		{
			this.occupied = new bool[this.gridWidth, this.gridHeight];
			CargoBox[] collection = Resources.LoadAll<CargoBox>("StationParts/CargoBoxes");
			this.cargoBoxesPrefabs = new List<CargoBox>(collection);
			this.SetDockNumberVisual();
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000A6128 File Offset: 0x000A4328
		public void SetDockNumberVisual()
		{
			this.textNumber.text = this.dockNumber.ToString();
			TMP_Text tmp_Text = this.textAbbr;
			string text = this.dockType.ToString().Split(new string[]
			{
				", "
			}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault<string>();
			tmp_Text.text = (((text != null) ? text.First<char>().ToString() : null) ?? "C");
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000A61A0 File Offset: 0x000A43A0
		public void SetDockType(CargoDock.CargoDockType type)
		{
			this.dockType = type;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000A61A9 File Offset: 0x000A43A9
		public void AssignItems(List<Inventory.InventoryItem> itemsToAssign)
		{
			this.items = itemsToAssign;
			this.SetCargo();
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000A61B8 File Offset: 0x000A43B8
		public void SetCargo()
		{
			this.ResetDock();
			this.totalVolume = this.items.Sum((Inventory.InventoryItem i) => i.item.m3 * (float)i.count);
			this.AddCargo(this.totalVolume);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000A6208 File Offset: 0x000A4408
		private void ResetDock()
		{
			CargoBox[] componentsInChildren = base.GetComponentsInChildren<CargoBox>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
			this.occupied = new bool[this.gridWidth, this.gridHeight];
			this.volumePlaced = 0f;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000A625C File Offset: 0x000A445C
		public void AddCargo(float remainingVolume)
		{
			SeededRandom seededRandom = new SeedGenerator().Add(MapPointOfInterest.current.guid).CreateRandom();
			foreach (int num in this.boxSizes)
			{
				while (remainingVolume >= (float)num && this.items.Count > 0)
				{
					int num2 = num;
					if (num > 4 && seededRandom.RandomBool(0.5f))
					{
						num2 = num / 2;
					}
					if (!this.CanFitBox(num2))
					{
						return;
					}
					List<Inventory.InventoryItem> list = this.TakeItemsForBox((float)num2);
					if (list.Count == 0)
					{
						break;
					}
					ItemCategory itemCategory = list[0].item.itemCategory;
					if (!this.TrySpawnBox(num2, list, itemCategory))
					{
						return;
					}
					float num3 = list.Sum((Inventory.InventoryItem i) => i.item.m3 * (float)i.count);
					this.volumePlaced += num3;
					remainingVolume -= num3;
				}
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000A6358 File Offset: 0x000A4558
		private bool CanFitBox(int size)
		{
			CargoBox prefab = this.GetPrefab(size, ItemCategory.Empty);
			if (prefab == null)
			{
				return false;
			}
			Vector2Int vector2Int = new Vector2Int(prefab.gridWidth, prefab.gridHeight);
			return this.FindPlacement(vector2Int.x, vector2Int.y, false) != null;
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000A63AC File Offset: 0x000A45AC
		private bool TrySpawnBox(int size, List<Inventory.InventoryItem> boxItems, ItemCategory category)
		{
			CargoBox prefab = this.GetPrefab(size, category);
			if (prefab == null)
			{
				return false;
			}
			Vector2Int vector2Int = new Vector2Int(prefab.gridWidth, prefab.gridHeight);
			Vector3? vector = this.FindPlacement(vector2Int.x, vector2Int.y, true);
			if (vector != null)
			{
				UnityEngine.Object.Instantiate<CargoBox>(prefab, vector.Value, base.transform.rotation, base.transform);
				return true;
			}
			return false;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000A6424 File Offset: 0x000A4624
		private List<Inventory.InventoryItem> TakeItemsForBox(float boxVolume)
		{
			List<Inventory.InventoryItem> list = new List<Inventory.InventoryItem>();
			float num = boxVolume;
			if (this.items.Count == 0)
			{
				return list;
			}
			ItemCategory itemCategory = this.items[0].item.itemCategory;
			int num2 = 0;
			while (num2 < this.items.Count && num > 0f)
			{
				Inventory.InventoryItem inventoryItem = this.items[num2];
				if (inventoryItem.item.itemCategory != itemCategory)
				{
					num2++;
				}
				else
				{
					float m = inventoryItem.item.m3;
					if (m <= 0f)
					{
						num2++;
					}
					else
					{
						int num3 = Mathf.FloorToInt(num / m);
						if (num3 <= 0)
						{
							num2++;
						}
						else
						{
							int num4 = Mathf.Min(inventoryItem.count, num3);
							list.Add(new Inventory.InventoryItem(inventoryItem.item, inventoryItem.inventory, inventoryItem.slot, num4, inventoryItem.canBuyback));
							num -= (float)num4 * m;
							if (num4 >= inventoryItem.count)
							{
								this.items.RemoveAt(num2);
							}
							else
							{
								inventoryItem.InternalRemove(num4);
								num2++;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000A6544 File Offset: 0x000A4744
		private CargoBox GetPrefab(int size, ItemCategory category = ItemCategory.Empty)
		{
			CargoBox.CargoBoxColor color = category.GetCargoBoxColor();
			return this.cargoBoxesPrefabs.FirstOrDefault((CargoBox box) => box.size == size && box.color == color);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000A6584 File Offset: 0x000A4784
		private Vector3? FindPlacement(int width, int height, bool save = true)
		{
			for (int i = 0; i <= this.gridHeight - height; i++)
			{
				for (int j = 0; j <= this.gridWidth - width; j++)
				{
					if (this.CanFit(j, i, width, height))
					{
						if (save)
						{
							this.MarkOccupied(j, i, width, height);
						}
						Vector3 position = new Vector3(((float)j + (float)width / 2f) * this.cellSize, -((float)i + (float)height / 2f) * this.cellSize, -0.01f);
						return new Vector3?(this.gridSpawn.TransformPoint(position));
					}
				}
			}
			return null;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000A661C File Offset: 0x000A481C
		private bool CanFit(int startX, int startY, int width, int height)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (this.occupied[startX + j, startY + i])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000A6658 File Offset: 0x000A4858
		private void MarkOccupied(int startX, int startY, int width, int height)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					this.occupied[startX + j, startY + i] = true;
				}
			}
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x000A6690 File Offset: 0x000A4890
		private void OnDestroy()
		{
			if (CargoDockManager.Instance == null)
			{
				return;
			}
			CargoDockManager.Instance.RemoveCargoDock(this);
		}

		// Token: 0x0400110E RID: 4366
		public int dockNumber = 1;

		// Token: 0x0400110F RID: 4367
		private readonly int[] boxSizes = new int[]
		{
			256,
			128,
			64,
			32,
			16,
			4
		};

		// Token: 0x04001110 RID: 4368
		private List<CargoBox> cargoBoxesPrefabs;

		// Token: 0x04001111 RID: 4369
		[SerializeField]
		private float cellSize = 1f;

		// Token: 0x04001112 RID: 4370
		[SerializeField]
		private int gridWidth = 8;

		// Token: 0x04001113 RID: 4371
		[SerializeField]
		private int gridHeight = 8;

		// Token: 0x04001114 RID: 4372
		[SerializeField]
		private Transform gridSpawn;

		// Token: 0x04001115 RID: 4373
		private bool[,] occupied;

		// Token: 0x04001116 RID: 4374
		private float totalVolume;

		// Token: 0x04001117 RID: 4375
		private float volumePlaced;

		// Token: 0x04001118 RID: 4376
		public List<Inventory.InventoryItem> items;

		// Token: 0x04001119 RID: 4377
		[SerializeField]
		private TextMeshPro textAbbr;

		// Token: 0x0400111A RID: 4378
		[SerializeField]
		private TextMeshPro textNumber;

		// Token: 0x0200057B RID: 1403
		[Flags]
		public enum CargoDockType
		{
			// Token: 0x04001C86 RID: 7302
			None = 0,
			// Token: 0x04001C87 RID: 7303
			GeneralShop = 1,
			// Token: 0x04001C88 RID: 7304
			MiningShop = 2,
			// Token: 0x04001C89 RID: 7305
			SalvageShop = 4,
			// Token: 0x04001C8A RID: 7306
			BountyShop = 8,
			// Token: 0x04001C8B RID: 7307
			Materials = 16,
			// Token: 0x04001C8C RID: 7308
			Global = 32,
			// Token: 0x04001C8D RID: 7309
			Offload = 64,
			// Token: 0x04001C8E RID: 7310
			Unload = 128
		}
	}
}
