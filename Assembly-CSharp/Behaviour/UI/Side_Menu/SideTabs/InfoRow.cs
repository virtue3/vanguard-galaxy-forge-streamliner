using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Equipment.Module;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Item;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002BD RID: 701
	public class InfoRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, ITooltipCustomSource
	{
		// Token: 0x060019CE RID: 6606 RVA: 0x000A04D0 File Offset: 0x0009E6D0
		public void SetInfo(float baseStat, EquipStat equipStat, string stats, string addendum = "", bool noHoverInfo = false)
		{
			this.baseStat = baseStat;
			this.equipStat = equipStat;
			this.addendum = addendum;
			this.noHoverInfo = noHoverInfo;
			this.field.text = Translation.Translate("@EquipStat" + equipStat.ToString(), Array.Empty<object>()) + " " + Translation.Translate(addendum, Array.Empty<object>());
			this.stat.text = stats;
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x000A0549 File Offset: 0x0009E749
		public void SetCustomString(string customString, int customAmount)
		{
			this.customString = customString;
			this.customAmount = customAmount;
			this.field.text = this.customString;
			this.stat.text = customAmount.ToString();
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x000A057C File Offset: 0x0009E77C
		public void SetPosition(Vector2 position)
		{
			(base.transform as RectTransform).anchoredPosition = position;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x000A058F File Offset: 0x0009E78F
		public void OnPointerClick(PointerEventData eventData)
		{
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000A0591 File Offset: 0x0009E791
		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000A0593 File Offset: 0x0009E793
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x000A0598 File Offset: 0x0009E798
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (!string.IsNullOrEmpty(this.customString))
			{
				tooltip.AddTextLine(this.customString + " " + this.customAmount.ToString(), 12, 8f);
				return;
			}
			tooltip.SetWidth(300f);
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			List<StatsInfoItem> statsInfoItems = spaceShip.GetStatsInfoItems(this.equipStat);
			tooltip.AddTextLine(string.Concat(new string[]
			{
				Translation.Translate("@EquipStat" + this.equipStat.ToString(), Array.Empty<object>()),
				" ",
				Translation.Translate(this.addendum, Array.Empty<object>()),
				" ",
				this.equipStat.IsPercentageStat() ? GameMath.FormatPercentage(this.baseStat, FormatPercentageMode.Default, 1) : GameMath.FormatNumber(this.baseStat, -1)
			}), 14, 8f).Text.color = ColorHelper.detailsColor;
			if (this.noHoverInfo)
			{
				return;
			}
			if (statsInfoItems.Count > 0)
			{
				tooltip.AddSeparator(null);
			}
			SpaceShip spaceShip2 = GameplayManager.Instance.spaceShip;
			if (this.equipStat == EquipStat.CriticalChance)
			{
				tooltip.AddTextLine(Translation.Translate("@BaseCritChance", Array.Empty<object>()) + " " + GameMath.FormatPercentage(0.03f, FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.greenish), 12, 8f);
				tooltip.AddTextLine(Translation.Translate("@EquipStatPrecision", Array.Empty<object>()) + " " + GameMath.FormatPercentage(spaceShip2.GetPrecisionCrit(), FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			else if (this.equipStat == EquipStat.CriticalDamage)
			{
				tooltip.AddTextLine(Translation.Translate("@BaseCritDamage", Array.Empty<object>()) + " " + GameMath.FormatPercentage(1f, FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			else if (this.equipStat == EquipStat.ShieldHP)
			{
				tooltip.AddTextLine(Translation.Translate("@BaseShieldHP", Array.Empty<object>()) + " " + spaceShip2.baseShieldHP.ToString().HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			else if (this.equipStat == EquipStat.ArmorHP)
			{
				tooltip.AddTextLine(Translation.Translate("@BaseArmorHP", Array.Empty<object>()) + " " + spaceShip2.baseArmorHP.ToString().HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			else if (this.equipStat == EquipStat.HullHP)
			{
				tooltip.AddTextLine(Translation.Translate("@BaseHullHP", Array.Empty<object>()) + " " + spaceShip2.baseHullHP.ToString().HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			else if (this.equipStat == EquipStat.CargoCapacity)
			{
				tooltip.AddTextLine(Translation.Translate("@BaseCargoCapacity", Array.Empty<object>()) + " " + spaceShip2._cargoCapacity.ToString().HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			else if (this.equipStat == EquipStat.DronePower)
			{
				float dronePower = spaceShip2.spaceShipData.GetEquippedItem(EquipmentSlot.DroneBay).GetComponent<DroneBayModule>().GetDronePower();
				tooltip.AddTextLine(Translation.Translate("@BaseDronePower", Array.Empty<object>()) + " " + GameMath.FormatNumber(dronePower, -1).HighlightWithColor(ColorHelper.greenish), 12, 8f);
			}
			List<string> list = new List<string>();
			float num = 0f;
			float num2 = 1f;
			foreach (StatsInfoItem statsInfoItem in statsInfoItems)
			{
				if (statsInfoItem.stat.multiplier == 1f)
				{
					num += statsInfoItem.stat.amount;
					string text = statsInfoItem.stat.ToString(false);
					tooltip.AddTextLine(Translation.Translate(statsInfoItem.source, Array.Empty<object>()) + " " + text.HighlightWithColor(ColorHelper.greenish), 12, 8f);
				}
				else if (statsInfoItem.stat.multiplier != 0f)
				{
					string text2 = statsInfoItem.stat.ToString(false);
					list.Add(Translation.Translate(statsInfoItem.source, Array.Empty<object>()) + " " + text2.HighlightWithColor(ColorHelper.greenish));
					num2 *= statsInfoItem.stat.multiplier;
				}
			}
			if (num > 0f && num2 != 1f)
			{
				string text3 = this.equipStat.IsPercentageStat() ? GameMath.FormatPercentage(num, FormatPercentageMode.Default, 1) : GameMath.FormatNumber(num, -1);
				tooltip.AddTextLine(string.Concat(new string[]
				{
					Translation.Translate(Translation.Translate("@StatTotal", Array.Empty<object>()), Array.Empty<object>()),
					" ",
					Translation.Translate("@EquipStat" + this.equipStat.ToString(), Array.Empty<object>()),
					" ",
					text3.HighlightWithColor(ColorHelper.steamBlueLight2)
				}), 12, 8f);
			}
			tooltip.AddSeparator(new Color?(ColorHelper.white75));
			if (AbstractUnit.reactorAffectedStats.Contains(this.equipStat) && spaceShip.reactorModule != null)
			{
				float energyBonusOrPenalty = spaceShip.reactorModule.energyBonusOrPenalty;
				num2 *= 1f + energyBonusOrPenalty;
				Color color = (energyBonusOrPenalty < 0f) ? ColorHelper.reddish : ColorHelper.greenish;
				tooltip.AddTextLine(string.Concat(new string[]
				{
					Translation.Translate("@ShipInfoReactorBonus", Array.Empty<object>()),
					" ",
					Translation.Translate(this.addendum, Array.Empty<object>()),
					" ",
					GameMath.FormatPercentage(energyBonusOrPenalty, FormatPercentageMode.Default, 1).HighlightWithColor(color)
				}), 12, 8f);
			}
			foreach (string text4 in list)
			{
				tooltip.AddTextLine(text4, 12, 8f);
			}
			if (num2 != 1f)
			{
				tooltip.AddTextLine(string.Concat(new string[]
				{
					Translation.Translate(Translation.Translate("@StatTotalMultiplier", Array.Empty<object>()), Array.Empty<object>()),
					" ",
					Translation.Translate("@EquipStat" + this.equipStat.ToString(), Array.Empty<object>()),
					" ",
					GameMath.FormatNumber(num2, 2).HighlightWithColor(ColorHelper.steamBlueLight2)
				}), 12, 8f);
			}
		}

		// Token: 0x0400103A RID: 4154
		[SerializeField]
		private Image icon;

		// Token: 0x0400103B RID: 4155
		[SerializeField]
		private TMP_Text field;

		// Token: 0x0400103C RID: 4156
		[SerializeField]
		private TMP_Text stat;

		// Token: 0x0400103D RID: 4157
		private float baseStat;

		// Token: 0x0400103E RID: 4158
		private EquipStat equipStat;

		// Token: 0x0400103F RID: 4159
		private string addendum;

		// Token: 0x04001040 RID: 4160
		private bool noHoverInfo;

		// Token: 0x04001041 RID: 4161
		private string customString;

		// Token: 0x04001042 RID: 4162
		private int customAmount;
	}
}
