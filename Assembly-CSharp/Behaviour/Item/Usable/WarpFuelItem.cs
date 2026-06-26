using System;
using Behaviour.Item.Builder;
using Behaviour.Managers;
using Behaviour.UI;
using LightJson;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000321 RID: 801
	public class WarpFuelItem : UsableItem
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x000B2023 File Offset: 0x000B0223
		// (set) Token: 0x06001DF9 RID: 7673 RVA: 0x000B202B File Offset: 0x000B022B
		public float efficiency { get; private set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x000B2034 File Offset: 0x000B0234
		// (set) Token: 0x06001DFB RID: 7675 RVA: 0x000B203C File Offset: 0x000B023C
		public float multiplier { get; private set; }

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x000B2045 File Offset: 0x000B0245
		// (set) Token: 0x06001DFD RID: 7677 RVA: 0x000B204D File Offset: 0x000B024D
		public float capacity { get; private set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001DFE RID: 7678 RVA: 0x000B2056 File Offset: 0x000B0256
		// (set) Token: 0x06001DFF RID: 7679 RVA: 0x000B205E File Offset: 0x000B025E
		public float remaining { get; private set; } = 1f;

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001E00 RID: 7680 RVA: 0x000B2067 File Offset: 0x000B0267
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x000B206A File Offset: 0x000B026A
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000B2070 File Offset: 0x000B0270
		public override void DataToJson(JsonObject data)
		{
			data["efficiency"] = new double?((double)this.efficiency);
			data["multiplier"] = new double?((double)this.multiplier);
			data["capacity"] = new double?((double)this.capacity);
			data["remaining"] = new double?((double)this.remaining);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000B20F0 File Offset: 0x000B02F0
		public override void DataFromJson(JsonObject data)
		{
			this.efficiency = (float)data["efficiency"].AsNumber;
			this.multiplier = (float)data["multiplier"].AsNumber;
			this.capacity = (float)data["capacity"].AsNumber;
			this.remaining = (float)data["remaining"].AsNumber;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000B2165 File Offset: 0x000B0365
		public override bool OnUse()
		{
			GamePlayer.current.useWarpFuel = !GamePlayer.current.useWarpFuel;
			return false;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000B217F File Offset: 0x000B037F
		public void SetWarpFuel(float efficiency, float multiplier, float capacity, float remaining = 1f)
		{
			this.efficiency = efficiency;
			this.multiplier = multiplier;
			this.capacity = capacity;
			this.remaining = remaining;
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x000B21A0 File Offset: 0x000B03A0
		public void UseWarpFuel(float baseMaxWarpSpeed, float deltaTime)
		{
			float num = this.remaining * this.capacity;
			float num2 = this.multiplier * baseMaxWarpSpeed / this.efficiency * deltaTime;
			num = Mathf.Max(num - num2, 0f);
			this.remaining = num / this.capacity;
			if (this.remaining <= 0f)
			{
				this.remaining = 1f;
				GamePlayer.current.currentSpaceShip.AddCargo(InventoryItemType.Get("EmptyCell"), 1, false);
				GamePlayer.current.currentSpaceShip.RemoveCargo(base.item, 1, false);
			}
			base.item.ResetDynamicValue();
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000B2240 File Offset: 0x000B0440
		public override int GetDynamicValue()
		{
			return Mathf.CeilToInt((float)base.item.baseCost * Mathf.Max(0.1f, this.remaining));
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000B2264 File Offset: 0x000B0464
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			base.name = "@Remaining";
			tooltip.AddTextLine(string.Concat(new string[]
			{
				"<color=",
				RandomStuffHelper.ColorToHex(ColorHelper.greenish),
				">",
				Translation.Translate(base.name, Array.Empty<object>()),
				"</color> ",
				GameMath.FormatPercentage(this.remaining, FormatPercentageMode.Default, 1)
			}), 12, 8f).Text.color = CompareTooltip.detailsColor;
			base.name = "@Allowed";
			if (!GamePlayer.current.useWarpFuel)
			{
				tooltip.AddTextLine("@WarpFuelDisabled", 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if (TravelManager.GetWarpFuelAutopilotMultiplier() == 0f)
			{
				tooltip.AddTextLine("@WarpFuelDisabledAutopilot", 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			tooltip.AddTextLine("@WarpFuelEnabled", 12, 8f).Text.color = ColorHelper.greenish;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000B2374 File Offset: 0x000B0574
		public override bool CanStackWith(InventoryItemType other)
		{
			WarpFuelItem component = other.GetComponent<WarpFuelItem>();
			return component && (component.efficiency == this.efficiency && component.multiplier == this.multiplier) && component.capacity == this.capacity;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000B23C0 File Offset: 0x000B05C0
		public override int AddStackToSelf(int ownCount, InventoryItemType other, int otherCount)
		{
			WarpFuelItem component = other.GetComponent<WarpFuelItem>();
			if (otherCount < 0)
			{
				this.remaining = 1f;
			}
			else
			{
				float num = this.remaining + component.remaining;
				if (num < 1f)
				{
					this.remaining = num;
					otherCount--;
				}
				else
				{
					this.remaining = num - 1f;
				}
			}
			return base.AddStackToSelf(ownCount, other, otherCount);
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000B2420 File Offset: 0x000B0620
		public override bool SplitStack(Inventory inventory, int slot1, int amount1)
		{
			if (inventory.hasSearchFilter)
			{
				inventory.Add(base.item.itemBuilder.CreateWarpFuel(this.GetWarpFuelType(), 1f), amount1, false, true);
			}
			else
			{
				inventory.Set(slot1, base.item.itemBuilder.CreateWarpFuel(this.GetWarpFuelType(), 1f), amount1, false);
			}
			return true;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000B2484 File Offset: 0x000B0684
		public WarpFuelItem.WarpFuelType GetWarpFuelType()
		{
			foreach (object obj in Enum.GetValues(typeof(WarpFuelItem.WarpFuelType)))
			{
				WarpFuelItem.WarpFuelType warpFuelType = (WarpFuelItem.WarpFuelType)obj;
				ValueTuple<string, string, Rarity, float, float, int, float> warpFuelConfig = ItemBuilder.GetWarpFuelConfig(warpFuelType);
				if (warpFuelConfig.Item5 == this.capacity && warpFuelConfig.Item4 == this.multiplier)
				{
					return warpFuelType;
				}
			}
			return WarpFuelItem.WarpFuelType.IonCell;
		}

		// Token: 0x020005AA RID: 1450
		public enum WarpFuelType
		{
			// Token: 0x04001D45 RID: 7493
			PlasmaCell,
			// Token: 0x04001D46 RID: 7494
			IonCell,
			// Token: 0x04001D47 RID: 7495
			HyperCell
		}
	}
}
