using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using UnityEngine;

namespace Source.Galaxy.POI
{
	// Token: 0x02000156 RID: 342
	public class IndustryStation : SpaceStation
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0005D675 File Offset: 0x0005B875
		public override string sceneName
		{
			get
			{
				return "Combat";
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0005D67C File Offset: 0x0005B87C
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0005D67F File Offset: 0x0005B87F
		public override bool canBeHomeStation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x0005D682 File Offset: 0x0005B882
		public override int pointsValue
		{
			get
			{
				return Mathf.Max(34, this.level * 3);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0005D694 File Offset: 0x0005B894
		public override IEnumerable<CraftingRecipe> recipes
		{
			get
			{
				if (this.industryRecipes == null)
				{
					this.industryRecipes = new List<CraftingRecipe>
					{
						"IndustrialAmmoPack",
						"IndustrialRepairPack",
						"IndustrialSupplyPack",
						"IndustrialTurretPack"
					};
				}
				return this.industryRecipes;
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0005D6FC File Offset: 0x0005B8FC
		public override bool HasFacility(SpaceStationFacility facility)
		{
			return facility == SpaceStationFacility.Forge || facility == SpaceStationFacility.ExitSpacestation || facility == SpaceStationFacility.OutpostAirlock;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0005D728 File Offset: 0x0005B928
		public override Vector2 GetLocalOffset()
		{
			return new Vector2(this.lastVisitedX, 0f);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0005D73C File Offset: 0x0005B93C
		public override Rect GetWorldBounds()
		{
			Rect worldBounds = base.GetWorldBounds();
			worldBounds.width += 40f;
			worldBounds.height += 20f;
			worldBounds.xMin -= 20f;
			worldBounds.yMin -= 10f;
			return worldBounds;
		}

		// Token: 0x04000719 RID: 1817
		private List<CraftingRecipe> industryRecipes;
	}
}
