using System;
using System.Collections.Generic;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using LightJson;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000319 RID: 793
	public class SalvageClaimItem : UsableItem
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x000B165B File Offset: 0x000AF85B
		// (set) Token: 0x06001DC4 RID: 7620 RVA: 0x000B1663 File Offset: 0x000AF863
		public string systemGuid { get; private set; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x000B166C File Offset: 0x000AF86C
		// (set) Token: 0x06001DC6 RID: 7622 RVA: 0x000B1674 File Offset: 0x000AF874
		public List<PersistableData> salvageData { get; private set; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x000B167D File Offset: 0x000AF87D
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000B1680 File Offset: 0x000AF880
		public void SetClaim(string system, int level, string name, List<PersistableData> salvage = null)
		{
			this.systemGuid = system;
			this.poiName = name;
			this.salvageData = salvage;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000B1698 File Offset: 0x000AF898
		public void ActivateClaim()
		{
			SystemMapData system = GalaxyMapData.current.GetSystem(this.systemGuid);
			MapPointOfInterest salvageClaimPOI = system.GetSalvageClaimPOI();
			if (salvageClaimPOI != null)
			{
				system.RemovePointOfInterest(salvageClaimPOI);
			}
			Source.Galaxy.POI.Salvage salvage = new Source.Galaxy.POI.Salvage
			{
				name = this.poiName
			};
			system.SetupPOI(salvage, null, Faction.salvageGuild, 0);
			if (this.salvageData == null)
			{
				this.salvageData = new List<PersistableData>();
				List<Faction> list = new List<Faction>
				{
					Faction.red,
					Faction.blue,
					Faction.gold,
					Faction.miningGuild,
					Faction.tradingGuild
				};
				for (int i = 0; i < 2; i++)
				{
					SalvageData salvageData = new SalvageData
					{
						position = new Vector2((float)(8 + i * 2), (float)(2 + i * 2)),
						angle = (float)SeededRandom.Global.RandomRange(0, 360),
						shipTemplate = salvage.FindSalvageShipTemplate(SeededRandom.Global.Choose<Faction>(list))
					};
					salvageData.AddItemContent(salvage.level, 3, 1f);
					salvageData.AddScrapContent(salvage.level, 0.5f + (float)i, 2);
					salvageData.AddStructuralContent(salvage.level, 2, 1f);
					this.salvageData.Add(salvageData);
				}
			}
			foreach (PersistableData persistableData in this.salvageData)
			{
				persistableData.position += salvage.GetWorldPosition();
				salvage.AddPersistable(persistableData);
			}
			salvage.hazardFieldData = HazardFieldData.CreateRandom(0.4f);
			salvage.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + salvage.hazardFieldData.GetHazardName();
			salvage.AddCargoContainers(new Vector2(20f, 16f), 1, 0.4f);
			system.pointsOfInterest.Add(salvage);
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@MiningClaimActivated", new object[]
			{
				this.poiName
			})).WithColor(ColorHelper.greenish).Show();
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000B18E8 File Offset: 0x000AFAE8
		public override bool OnUse()
		{
			this.ActivateClaim();
			return true;
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000B18F4 File Offset: 0x000AFAF4
		public override void DataToJson(JsonObject data)
		{
			data["systemGuid"] = this.systemGuid;
			data["poiName"] = this.poiName;
			if (this.salvageData != null)
			{
				data["salvageData"] = this.salvageData.ToJsonArray<PersistableData>();
			}
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000B1950 File Offset: 0x000AFB50
		public override void DataFromJson(JsonObject data)
		{
			this.systemGuid = data["systemGuid"];
			this.poiName = data["poiName"];
			if (data["salvageData"].IsJsonArray)
			{
				this.salvageData = new List<PersistableData>();
				this.salvageData.FromJsonArray(data["salvageData"], new ClassExtensions.ParseJsonValue<PersistableData>(PersistableData.FromJson));
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000B19D0 File Offset: 0x000AFBD0
		public static bool IsSalvageClaim(Source.Galaxy.POI.Salvage poi)
		{
			return poi.name.StartsWith("Source.Galaxy.POI.Salvage Claim");
		}

		// Token: 0x04001212 RID: 4626
		public string poiName;
	}
}
