using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Item;
using Source.Galaxy;
using Source.Item;
using Source.Mining;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Mining
{
	// Token: 0x020002FD RID: 765
	public class OreItemData : InventoryItemPart
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x000AA3BC File Offset: 0x000A85BC
		public static IEnumerable<OreItemData> regularOres
		{
			get
			{
				if (OreItemData.allOres == null)
				{
					OreItemData.allOres = new List<OreItemData>();
					foreach (InventoryItemType inventoryItemType in InventoryItemType.all)
					{
						if (inventoryItemType.itemCategory == ItemCategory.Ore)
						{
							OreItemData component = inventoryItemType.GetComponent<OreItemData>();
							if (component != null && component.occurrence > 0f)
							{
								OreItemData.allOres.Add(component);
							}
						}
					}
				}
				return OreItemData.allOres;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x000AA448 File Offset: 0x000A8648
		// (set) Token: 0x06001C1E RID: 7198 RVA: 0x000AA450 File Offset: 0x000A8650
		public int customHealth { get; private set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x000AA459 File Offset: 0x000A8659
		// (set) Token: 0x06001C20 RID: 7200 RVA: 0x000AA461 File Offset: 0x000A8661
		public float occurrence { get; private set; } = 1f;

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x000AA46A File Offset: 0x000A866A
		// (set) Token: 0x06001C22 RID: 7202 RVA: 0x000AA472 File Offset: 0x000A8672
		[Tooltip("Influences the time it takes to refine ore")]
		[Range(0f, 5f)]
		public float refinementDifficulty { get; private set; } = 1f;

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x000AA47B File Offset: 0x000A867B
		// (set) Token: 0x06001C24 RID: 7204 RVA: 0x000AA483 File Offset: 0x000A8683
		public Color depositColor { get; private set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x000AA48C File Offset: 0x000A868C
		// (set) Token: 0x06001C26 RID: 7206 RVA: 0x000AA494 File Offset: 0x000A8694
		public bool ignoreExtraRewards { get; private set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001C27 RID: 7207 RVA: 0x000AA49D File Offset: 0x000A869D
		// (set) Token: 0x06001C28 RID: 7208 RVA: 0x000AA4A5 File Offset: 0x000A86A5
		public bool disableAutoRefine { get; private set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x000AA4AE File Offset: 0x000A86AE
		// (set) Token: 0x06001C2A RID: 7210 RVA: 0x000AA4B6 File Offset: 0x000A86B6
		public bool disableDynamicValue { get; private set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x000AA4C0 File Offset: 0x000A86C0
		public int health
		{
			get
			{
				float num = (float)((this.customHealth > 0) ? this.customHealth : this.dynamicHealth);
				float num2 = 1f;
				GamePlayer current = GamePlayer.current;
				int num3 = (current != null) ? current.level : 1;
				MapPointOfInterest currentOrNext = MapPointOfInterest.currentOrNext;
				int num4;
				if (currentOrNext == null)
				{
					SystemMapData current2 = SystemMapData.current;
					num4 = ((current2 != null) ? current2.level : 1);
				}
				else
				{
					num4 = currentOrNext.level;
				}
				int num5 = num4;
				if (num3 > num5)
				{
					num5 = Math.Max(num5, num3 - 5);
				}
				if (num5 > 12)
				{
					num5 += Math.Min(10, num5 - 12);
				}
				int level = GamePlayer.current.level;
				return Mathf.RoundToInt(num * GameMath.DamageMultiplier((float)num5) * num2);
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000AA55D File Offset: 0x000A875D
		public int refinementCost
		{
			get
			{
				return Mathf.CeilToInt((float)base.item.cost * 0.1f * Mathf.Max(0f, 1f - SkilltreeNode.industrialCraftCreditReduction.currentIncrease) * this.refinementDifficulty);
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001C2D RID: 7213 RVA: 0x000AA598 File Offset: 0x000A8798
		public float refinementTime
		{
			get
			{
				return Mathf.Sqrt((float)base.item.itemLevel) * base.item.rarity.GetCostMultiplier() * (this.refinementDifficulty * 7.5f) / Refinery.refinedPerSecond;
			}
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000AA5D0 File Offset: 0x000A87D0
		protected override void InitializeItem()
		{
			if (string.IsNullOrEmpty(base.item.displayName))
			{
				base.item.SetDisplayName("@" + base.item.identifier.Replace(' ', '_'));
			}
			float num = GameMath.DamageMultiplier((float)base.item.itemLevel) - 1f;
			num /= 10f;
			this.dynamicHealth = Mathf.RoundToInt(40f * (1f + num) * base.item.rarity.GetPowerMultiplier());
			if (base.item.itemLevel > 10)
			{
				int num2 = Mathf.Clamp(base.item.itemLevel - 10, 1, 40);
				base.item.sellValueMultiplier = 1f - 0.01f * (float)num2;
			}
			if (this.contents.Count > 0 && this.contents[0].customYield == 0f)
			{
				float num3 = (float)this.GetContentValue();
				SeedGenerator seedGenerator = new SeedGenerator().Add("OreItemData Value");
				foreach (OreRefinementProduct oreRefinementProduct in this.contents)
				{
					seedGenerator.Add(oreRefinementProduct.product.ToString());
				}
				SeededRandom seededRandom = seedGenerator.CreateRandom();
				if (this.contents.Count == 1)
				{
					this.contents[0].UpdateDynamicYield(num3 / this.contents[0].product.GetValue());
					return;
				}
				if (this.contents.Count == 2)
				{
					float num4 = seededRandom.RandomRange(0.5f, 0.75f);
					this.contents[0].UpdateDynamicYield(num3 / this.contents[0].product.GetValue() * num4);
					this.contents[1].UpdateDynamicYield(num3 / this.contents[1].product.GetValue() * (1f - num4));
					return;
				}
				if (this.contents.Count == 3)
				{
					float num5 = seededRandom.RandomRange(0.4f, 0.6f);
					float num6 = seededRandom.RandomRange(0.25f, 0.35f);
					this.contents[0].UpdateDynamicYield(num3 / this.contents[0].product.GetValue() * num5);
					this.contents[1].UpdateDynamicYield(num3 / this.contents[1].product.GetValue() * num6);
					this.contents[2].UpdateDynamicYield(num3 / this.contents[2].product.GetValue() * (1f - num5 - num6));
					return;
				}
				Debug.LogWarning("Dynamic yield could not be determined for " + base.item.displayName);
			}
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000AA8E4 File Offset: 0x000A8AE4
		public override int GetDynamicValue()
		{
			if (!this.disableDynamicValue)
			{
				return this.GetContentValue();
			}
			return 0;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000AA8F8 File Offset: 0x000A8AF8
		public int GetContentValue()
		{
			if (this.contents.Count > 0 && this.contents[0].customYield > 0f)
			{
				float num = 0f;
				foreach (OreRefinementProduct oreRefinementProduct in this.contents)
				{
					num += oreRefinementProduct.product.GetValue() * oreRefinementProduct.customYield;
				}
				return Mathf.CeilToInt(num);
			}
			return Mathf.CeilToInt(300f * GameMath.CostMultiplier(base.item.itemLevel) * base.item.rarity.GetCostMultiplier() * 0.5f);
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000AA9C0 File Offset: 0x000A8BC0
		public static OreItemData Get(string name)
		{
			return InventoryItemType.Get(name).GetComponent<OreItemData>();
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000AA9CD File Offset: 0x000A8BCD
		public static implicit operator OreItemData(string name)
		{
			return OreItemData.Get(name);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000AA9D8 File Offset: 0x000A8BD8
		public string GetDescription()
		{
			int count = this.contents.Count;
			if (count == 0)
			{
				return Translation.Translate("@OreUnknown", Array.Empty<object>());
			}
			string[] array = (from c in this.contents
			select c.product.GetDisplayName()).ToArray<string>();
			string text = "@OreDescription" + count.ToString();
			object[] values = array;
			return Translation.Translate(text, values);
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x000AAA50 File Offset: 0x000A8C50
		public bool HasMaterial(RefinedMaterial mat)
		{
			using (List<OreRefinementProduct>.Enumerator enumerator = this.contents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.product == mat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001183 RID: 4483
		private static List<OreItemData> allOres;

		// Token: 0x04001188 RID: 4488
		[SerializeField]
		public List<OreRefinementProduct> contents;

		// Token: 0x0400118C RID: 4492
		private int dynamicHealth;
	}
}
