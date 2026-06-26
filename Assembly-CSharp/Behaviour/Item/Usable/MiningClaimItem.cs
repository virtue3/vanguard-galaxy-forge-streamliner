using System;
using System.Collections.Generic;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.Mining;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000314 RID: 788
	public class MiningClaimItem : UsableItem
	{
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001D98 RID: 7576 RVA: 0x000B0D13 File Offset: 0x000AEF13
		// (set) Token: 0x06001D99 RID: 7577 RVA: 0x000B0D1B File Offset: 0x000AEF1B
		public string systemGuid { get; private set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x000B0D24 File Offset: 0x000AEF24
		// (set) Token: 0x06001D9B RID: 7579 RVA: 0x000B0D2C File Offset: 0x000AEF2C
		public AsteroidFieldData asteroidFieldData { get; private set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000B0D35 File Offset: 0x000AEF35
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000B0D38 File Offset: 0x000AEF38
		public void SetClaim(string system, int level, string name, AsteroidFieldData customFieldData = null)
		{
			this.systemGuid = system;
			this.poiName = name;
			this.asteroidFieldData = (customFieldData ?? new AsteroidFieldData(UnityEngine.Random.Range(8, 17), 1f, AsteroidFieldData.CalculateWealth(level, 1f), AsteroidFieldData.CreateOreSet(level, true), AsteroidFieldData.CreateOreSet(level, false), -1f));
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000B0D90 File Offset: 0x000AEF90
		public void ActivateClaim()
		{
			SystemMapData system = GalaxyMapData.current.GetSystem(this.systemGuid);
			MapPointOfInterest miningClaimPOI = system.GetMiningClaimPOI();
			if (miningClaimPOI != null)
			{
				system.RemovePointOfInterest(miningClaimPOI);
			}
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining
			{
				name = this.poiName
			};
			mining.SetAsteroidFieldData(this.asteroidFieldData, 0);
			system.SetupPOI(mining, null, Faction.miningGuild, 0);
			mining.hazardFieldData = HazardFieldData.CreateRandom(0.4f);
			mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
			mining.AddCargoContainers(new Vector2(30f, 16f), 2, 0.4f);
			system.pointsOfInterest.Add(mining);
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@MiningClaimActivated", new object[]
			{
				this.poiName
			})).WithColor(ColorHelper.greenish).Show();
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000B0E88 File Offset: 0x000AF088
		public override bool OnUse()
		{
			this.ActivateClaim();
			return true;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000B0E94 File Offset: 0x000AF094
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			Color modifierColor = CompareTooltip.modifierColor;
			Color detailsColor = CompareTooltip.detailsColor;
			SeededRandom seededRandom = new SeedGenerator().Add(this.asteroidFieldData.ToString() + this.asteroidFieldData.wealth.ToString() + this.asteroidFieldData.amount.ToString()).CreateRandom();
			int amount = this.asteroidFieldData.amount;
			int hi = Mathf.Max(1, Mathf.RoundToInt((float)amount * 0.2f));
			float num = (float)Mathf.Max(0, amount - seededRandom.RandomRange(1, hi));
			int num2 = amount + seededRandom.RandomRange(0, hi);
			string text = GameMath.FormatNumber(num, -1) + " - " + GameMath.FormatNumber((float)num2, -1);
			string text2 = "@Asteroids";
			tooltip.AddTextLine(string.Concat(new string[]
			{
				"<color=",
				RandomStuffHelper.ColorToHex(modifierColor),
				">",
				Translation.Translate(text2, Array.Empty<object>()),
				"</color> ",
				text
			}), 16, 8f).Text.color = detailsColor;
			text2 = "@Ores";
			tooltip.AddTextLine(string.Concat(new string[]
			{
				"<color=",
				RandomStuffHelper.ColorToHex(modifierColor),
				">",
				Translation.Translate(text2, Array.Empty<object>()),
				"</color> ",
				this.GetAllOreNamesInField()
			}), 12, 8f).Text.color = detailsColor;
			text2 = "@Wealth";
			tooltip.AddTextLine(string.Concat(new string[]
			{
				"<color=",
				RandomStuffHelper.ColorToHex(modifierColor),
				">",
				Translation.Translate(text2, Array.Empty<object>()),
				"</color> ",
				GameMath.FormatPercentage(this.asteroidFieldData.wealth, FormatPercentageMode.Default, 1)
			}), 12, 8f).Text.color = detailsColor;
			tooltip.AddTextLine(string.Concat(new string[]
			{
				"<color=",
				RandomStuffHelper.ColorToHex(ColorHelper.greenish),
				">",
				Translation.Translate("@MapStaticSystem", Array.Empty<object>()),
				"</color> ",
				this.GetSystemName()
			}), 12, 8f).Text.color = detailsColor;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000B10E4 File Offset: 0x000AF2E4
		public string GetSystemName()
		{
			SystemMapData system = GalaxyMapData.current.GetSystem(this.systemGuid);
			return ((system != null) ? system.name : null) ?? "";
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000B110C File Offset: 0x000AF30C
		public string GetAllOreNamesInField()
		{
			if (this.asteroidFieldData == null)
			{
				return "";
			}
			List<OreItemData> majorOres = this.asteroidFieldData.GetMajorOres();
			int num = majorOres.Count - 1;
			string text = "";
			for (int i = 0; i < majorOres.Count; i++)
			{
				OreItemData oreItemData = majorOres[i];
				if (i > 0)
				{
					text += Translation.Translate(oreItemData.item.displayName, Array.Empty<object>());
				}
				else
				{
					text += Translation.Translate(oreItemData.item.displayName, Array.Empty<object>());
				}
				if (i < num)
				{
					text += ", ";
				}
			}
			return text;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000B11B0 File Offset: 0x000AF3B0
		public override void DataToJson(JsonObject data)
		{
			data["systemGuid"] = this.systemGuid;
			data["asteroidFieldData"] = this.asteroidFieldData.ToJson();
			data["poiName"] = this.poiName;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000B1200 File Offset: 0x000AF400
		public override void DataFromJson(JsonObject data)
		{
			this.systemGuid = data["systemGuid"];
			this.asteroidFieldData = AsteroidFieldData.FromJson(data["asteroidFieldData"]);
			this.poiName = data["poiName"];
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000B124F File Offset: 0x000AF44F
		public static bool IsMiningClaim(Source.Galaxy.POI.Mining poi)
		{
			return poi.name.StartsWith("Source.Galaxy.POI.Mining Claim");
		}

		// Token: 0x0400120B RID: 4619
		public string poiName;
	}
}
