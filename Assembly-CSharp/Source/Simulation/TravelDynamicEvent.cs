using System;
using Source.Data.Persistable;
using Source.Galaxy;
using UnityEngine;

namespace Source.Simulation
{
	// Token: 0x02000071 RID: 113
	public abstract class TravelDynamicEvent
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0002002A File Offset: 0x0001E22A
		public virtual string actionLabel
		{
			get
			{
				return "@TravelInvestigate";
			}
		}

		// Token: 0x0600041B RID: 1051
		public abstract MapPointOfInterest CreateDynamicPOI(SystemMapData system);

		// Token: 0x0600041C RID: 1052 RVA: 0x00020034 File Offset: 0x0001E234
		public void AddBeacon(MapPointOfInterest poi)
		{
			if (poi.level < 10)
			{
				LootContainerData lootContainerData = new LootContainerData
				{
					position = poi.GetWorldPosition() + new Vector2(SeededRandom.Global.RandomRange(-2f, 2f), SeededRandom.Global.RandomRange(2f, 5f) * (float)(SeededRandom.Global.RandomBool(0.5f) ? 1 : -1)),
					name = "@LCNameOldCargoContainer"
				};
				lootContainerData.AddLoot("PoiBeacon", 1);
				poi.AddPersistable(lootContainerData);
			}
		}
	}
}
