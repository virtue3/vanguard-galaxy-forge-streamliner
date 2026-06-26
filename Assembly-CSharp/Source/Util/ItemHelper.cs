using System;
using System.Text;
using Behaviour.Item;
using Behaviour.Mining;

namespace Source.Util
{
	// Token: 0x02000035 RID: 53
	public class ItemHelper
	{
		// Token: 0x06000297 RID: 663 RVA: 0x00015260 File Offset: 0x00013460
		public static string RandomItemSeed()
		{
			return DateTime.Now.Ticks.ToString();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00015282 File Offset: 0x00013482
		public static OreRefinementProduct GetRefinedProductForOre(InventoryItemType oreType)
		{
			return oreType.GetComponent<OreItemData>().contents[0];
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00015298 File Offset: 0x00013498
		public static string GetMkDesignation(int level)
		{
			if (level < 1)
			{
				return "Mk.0";
			}
			int num = (level - 1) / 4 + 1;
			if (num > 20)
			{
				num = 20;
			}
			return "Mk." + ItemHelper.ToRoman(num);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000152D0 File Offset: 0x000134D0
		private static string ToRoman(int number)
		{
			if (number < 1 || number > 3999)
			{
				return number.ToString();
			}
			ValueTuple<int, string>[] array = new ValueTuple<int, string>[]
			{
				new ValueTuple<int, string>(1000, "M"),
				new ValueTuple<int, string>(900, "CM"),
				new ValueTuple<int, string>(500, "D"),
				new ValueTuple<int, string>(400, "CD"),
				new ValueTuple<int, string>(100, "C"),
				new ValueTuple<int, string>(90, "XC"),
				new ValueTuple<int, string>(50, "L"),
				new ValueTuple<int, string>(40, "XL"),
				new ValueTuple<int, string>(10, "X"),
				new ValueTuple<int, string>(9, "IX"),
				new ValueTuple<int, string>(5, "V"),
				new ValueTuple<int, string>(4, "IV"),
				new ValueTuple<int, string>(1, "I")
			};
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValueTuple<int, string> valueTuple in array)
			{
				int item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				while (number >= item)
				{
					stringBuilder.Append(item2);
					number -= item;
				}
			}
			return stringBuilder.ToString();
		}
	}
}
