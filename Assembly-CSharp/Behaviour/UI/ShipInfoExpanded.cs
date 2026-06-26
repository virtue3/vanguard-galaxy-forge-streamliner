using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Equipment.Builder;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Item;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001EE RID: 494
	public class ShipInfoExpanded : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060012BE RID: 4798 RVA: 0x0007A3D8 File Offset: 0x000785D8
		public void SetShipClass(SpaceShip spaceship)
		{
			string typeName = spaceship.shipRoleType.GetTypeName();
			this.value.text = spaceship.spaceShipData.GetShipName() + " <size=12>" + Translation.Translate(typeName, Array.Empty<object>()).HighlightWithColor(ColorHelper.purpleBlueish) + "</size>";
			this.spaceship = spaceship;
			float y = this.infoBadge.anchoredPosition.y;
			this.infoBadge.anchoredPosition = new Vector2(this.value.preferredWidth / 2f + 6f, y);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0007A46C File Offset: 0x0007866C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.spaceship == null)
			{
				return;
			}
			tooltip.AddTextLine(this.spaceship.originalName + "-Class " + this.spaceship.shipRoleType.GetTypeName(), 14, 8f).Text.color = ColorHelper.detailsColor;
			tooltip.AddTextLine(this.spaceship.manufacturer.GetDisplayName(), 12, 8f).Text.color = ColorHelper.grey;
			tooltip.AddTextLine(this.spaceship.shipRoleType.GetTypeDescription(), 12, 8f).Text.color = ColorHelper.boringGrey;
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@Size", Array.Empty<object>()) + ": " + this.spaceship.shipRoleType.GetTypeSize().ToString().HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@Role", Array.Empty<object>()) + ": " + Translation.Translate(string.Format("@Role{0}", this.spaceship.shipRoleType.GetGameplayType()), Array.Empty<object>()).HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (SpaceShipHardpoint spaceShipHardpoint in this.spaceship.hardpointSlots)
			{
				if (spaceShipHardpoint.size == ModuleSize.Small)
				{
					num++;
				}
				if (spaceShipHardpoint.size == ModuleSize.Medium)
				{
					num2++;
				}
				if (spaceShipHardpoint.size == ModuleSize.Large)
				{
					num3++;
				}
			}
			List<string> list = new List<string>();
			if (num3 > 0)
			{
				list.Add(string.Format("{0}L", num3));
			}
			if (num2 > 0)
			{
				list.Add(string.Format("{0}M", num2));
			}
			if (num > 0)
			{
				list.Add(string.Format("{0}S", num));
			}
			string str = (list.Count > 0) ? (" [" + string.Join(", ", list) + "]") : string.Empty;
			string text = this.spaceship.hardpointSlots.Length.ToString() + str;
			tooltip.AddTextLine(Translation.Translate("@Hardpoints", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			SpaceShipModule spaceShipModule = this.spaceship.moduleSlots.FirstOrDefault((SpaceShipModule module) => module.slot == EquipmentSlot.DroneBay);
			if (spaceShipModule != null)
			{
				tooltip.AddTextLine(Translation.Translate("@Dronebay", Array.Empty<object>()) + ": " + Translation.Translate(string.Format("@{0}", spaceShipModule.size), Array.Empty<object>()).HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			}
			tooltip.AddTextLine(Translation.Translate("@MaxOfficer", Array.Empty<object>()) + ": " + this.spaceship.maxOfficers.ToString().HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@UIBoosters", Array.Empty<object>()) + ": " + this.spaceship.boosterSlots.Length.ToString().HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@ShipSelectTonnage", Array.Empty<object>()) + ": " + GameMath.FormatNumber((float)this.spaceship.tonnage, -1).HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Highlight("@ShipHullMultiplier", ColorHelper.lightCyan, new object[]
			{
				GameMath.FormatNumber(this.spaceship.hullHPScale, 1)
			}), 12, 8f).Text.color = ColorHelper.offWhite;
			if (this.spaceship.HasModuleSlot(EquipmentSlot.Armor))
			{
				tooltip.AddTextLine(Translation.Highlight("@ShipArmorMultiplier", ColorHelper.lightCyan, new object[]
				{
					GameMath.FormatNumber(this.spaceship.armorHPScale, 1)
				}), 12, 8f).Text.color = ColorHelper.offWhite;
			}
			if (this.spaceship.HasModuleSlot(EquipmentSlot.ShieldGenerator))
			{
				tooltip.AddTextLine(Translation.Highlight("@ShipShieldMultiplier", ColorHelper.lightCyan, new object[]
				{
					GameMath.FormatNumber(this.spaceship.shieldHPScale, 1)
				}), 12, 8f).Text.color = ColorHelper.offWhite;
			}
			tooltip.AddSeparator(null);
			int cargoCapacity = this.spaceship.cargoCapacity;
			int cargoCapacity2 = this.spaceship._cargoCapacity;
			string text2 = string.Format("{0}", cargoCapacity);
			if (cargoCapacity > cargoCapacity2)
			{
				text2 += string.Format(" ({0}: {1})", Translation.Translate("@Base", Array.Empty<object>()), cargoCapacity2);
			}
			tooltip.AddTextLine(Translation.Translate("@Cargo", Array.Empty<object>()) + ": " + text2.HighlightWithColor(ColorHelper.beige), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddSeparator(null);
			float num4 = this.spaceship.baseMaxWarpSpeed * 100f;
			float num5 = this.spaceship.baseWarpAcceleration * 100f;
			string str2 = Translation.Translate("@TravelUnits", Array.Empty<object>());
			string str3 = Translation.Translate("@TravelSecond", Array.Empty<object>());
			string str4 = str2 + "/" + str3;
			string text3 = num4.ToString() + " " + str4;
			string text4 = num5.ToString() + " " + str4;
			tooltip.AddTextLine(Translation.Translate("@TravelBaseMaxSpeed", Array.Empty<object>()) + ": " + text3.HighlightWithColor(ColorHelper.beige), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@TravelAcceleration", Array.Empty<object>()) + ": " + text4.HighlightWithColor(ColorHelper.beige), 12, 8f).Text.color = ColorHelper.offWhite;
		}

		// Token: 0x04000A7E RID: 2686
		[SerializeField]
		private TextMeshProUGUI value;

		// Token: 0x04000A7F RID: 2687
		[SerializeField]
		private RectTransform infoBadge;

		// Token: 0x04000A80 RID: 2688
		private SpaceShip spaceship;
	}
}
