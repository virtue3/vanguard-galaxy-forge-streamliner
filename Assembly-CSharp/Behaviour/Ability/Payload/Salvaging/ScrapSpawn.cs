using System;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Util;

namespace Behaviour.Ability.Payload.Salvaging
{
	// Token: 0x020003DD RID: 989
	public class ScrapSpawn : TriggeredPayload
	{
		// Token: 0x060025B7 RID: 9655 RVA: 0x000D2360 File Offset: 0x000D0560
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			AbstractUnit abstractUnit = damageData.targetUnit as AbstractUnit;
			if (abstractUnit != null)
			{
				int tier = SalvageHelper.RollTier(abstractUnit.level);
				InventoryItemType inventoryItemType = InventoryItemType.Get(SalvageHelper.BuildScrapItemName(SeededRandom.Global.Choose<string>(SalvageHelper.surfaceMaterials), tier));
				if (!inventoryItemType)
				{
					return;
				}
				Singleton<LootManager>.Instance.CreateLootItem(damageData.hitCoordinates, inventoryItemType, 1, Faction.player, false);
			}
		}
	}
}
