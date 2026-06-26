using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.UI;
using Behaviour.UI.Forge;
using Behaviour.UI.Spacestation;
using LightJson;
using Source.Galaxy.POI;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200030B RID: 779
	public class BlueprintItem : UsableItem
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x000AFB39 File Offset: 0x000ADD39
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000AFB3C File Offset: 0x000ADD3C
		public override void DataFromJson(JsonObject data)
		{
			this.blueprintName = data["blueprintName"];
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x000AFB54 File Offset: 0x000ADD54
		public override void DataToJson(JsonObject data)
		{
			data["blueprintName"] = this.blueprintName;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x000AFB6C File Offset: 0x000ADD6C
		protected override void InitializeItem()
		{
			if (string.IsNullOrEmpty(this.blueprintName))
			{
				return;
			}
			base.item.SetIcon(this.CreateBlueprintSprite());
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000AFB90 File Offset: 0x000ADD90
		public override bool OnUse()
		{
			bool flag = GamePlayer.current.AddBlueprint(this.blueprintName);
			if (flag && SpaceStationInterior.instance)
			{
				SpaceStation current = SpaceStation.current;
				if (((current != null) ? current.forge : null) != null)
				{
					ForgeUI.preselectRecipe = CraftingRecipe.Get(this.blueprintName);
					SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.Forge, true);
				}
			}
			return flag;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000AFBEB File Offset: 0x000ADDEB
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			if (GamePlayer.current.blueprints.Contains(this.blueprintName))
			{
				tooltip.AddTextLine("@BlueprintItemAlreadyKnown", 12, 8f).Text.color = ColorHelper.reddish;
			}
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000AFC28 File Offset: 0x000ADE28
		private Sprite CreateBlueprintSprite()
		{
			CraftingRecipe craftingRecipe = CraftingRecipe.Get(this.blueprintName);
			InventoryItemType inventoryItemType = null;
			using (IEnumerator<KeyValuePair<InventoryItemType, int>> enumerator = craftingRecipe.GetResultsPreview().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<InventoryItemType, int> keyValuePair = enumerator.Current;
					inventoryItemType = keyValuePair.Key;
				}
			}
			if (inventoryItemType == null)
			{
				return base.item.icon;
			}
			return SpriteCombiner.CombineSpritesGPU(base.item.icon, inventoryItemType.icon, 0.7f, null, 1.5f, 0.65f);
		}

		// Token: 0x040011FB RID: 4603
		public string blueprintName;
	}
}
