using System;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.Util;
using LightJson;
using Source.Data.Persistable;
using Source.Galaxy;

namespace Source.Simulation.World.POI
{
	// Token: 0x02000082 RID: 130
	public class UmbralDeadDrop : PoiStoryteller
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0002738C File Offset: 0x0002558C
		public UmbralDeadDrop(MapPointOfInterest poi) : base(poi)
		{
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000273A0 File Offset: 0x000255A0
		public override void UpdateActive(float deltaTime)
		{
			if (this.dropped)
			{
				return;
			}
			this.updateTimer -= deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.1f;
				TractorableItemData tractorableItemData = null;
				foreach (PersistableData persistableData in this.poi.GetPersistables())
				{
					TractorableItemData tractorableItemData2 = persistableData as TractorableItemData;
					if (tractorableItemData2 != null && tractorableItemData2.ownerFaction == Faction.player && this.itemsChecked.Add(tractorableItemData2))
					{
						if (SeededRandom.Global.RandomBool(this.dropChance))
						{
							tractorableItemData = tractorableItemData2;
							break;
						}
						this.dropChance += 0.01f;
					}
				}
				if (tractorableItemData != null)
				{
					this.dropped = true;
					Singleton<LootManager>.Instance.CreateLootItem(tractorableItemData.position, "MiningDeadDrop", 1, Faction.player, false);
				}
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00027498 File Offset: 0x00025698
		public override void DataToJson(JsonObject data)
		{
			data["dropped"] = new bool?(this.dropped);
			data["dropChance"] = new double?((double)this.dropChance);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000274D4 File Offset: 0x000256D4
		public override void DataFromJson(JsonObject data)
		{
			this.dropped = data["dropped"];
			this.dropChance = (float)data["dropChance"].AsNumber;
		}

		// Token: 0x04000287 RID: 647
		public bool dropped;

		// Token: 0x04000288 RID: 648
		public float dropChance;

		// Token: 0x04000289 RID: 649
		private HashSet<TractorableItemData> itemsChecked = new HashSet<TractorableItemData>();

		// Token: 0x0400028A RID: 650
		private float updateTimer;
	}
}
