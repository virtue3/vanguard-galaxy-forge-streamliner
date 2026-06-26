using System;
using Behaviour.Managers;
using Behaviour.Persistables;
using LightJson;
using Source.Galaxy;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000119 RID: 281
	public class PatrolPortalData : PersistableData
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x0004F72C File Offset: 0x0004D92C
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			if (!PatrolPortalData.prefab)
			{
				PatrolPortalData.prefab = Resources.Load<TravelPortal>("Prefabs/PatrolPortal");
			}
			TravelPortal travelPortal = UnityEngine.Object.Instantiate<TravelPortal>(PatrolPortalData.prefab, this.position, base.rotation, parent.transform);
			travelPortal.SetTargetPoi(this.targetPoi);
			travelPortal.portalName = this.portalName;
			travelPortal.portalDesc = this.portalDesc;
			if (!this.isForwardPortal)
			{
				travelPortal.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			return travelPortal.gameObject;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0004F7C8 File Offset: 0x0004D9C8
		public override void DataToJson(JsonObject json)
		{
			json["isForwardPortal"] = new bool?(this.isForwardPortal);
			json["portalName"] = this.portalName;
			json["portalDesc"] = this.portalDesc;
			string key = "targetPoi";
			MapPointOfInterest mapPointOfInterest = this.targetPoi;
			json[key] = ((mapPointOfInterest != null) ? mapPointOfInterest.guid : null);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0004F840 File Offset: 0x0004DA40
		public override void LoadFromJson(JsonObject json)
		{
			this.isForwardPortal = json["isForwardPortal"];
			this.portalName = json["portalName"];
			this.portalDesc = json["portalDesc"];
			if (json["targetPoi"].IsString)
			{
				GalaxyMapData.current.LoadPointOfInterest(json["targetPoi"], delegate(MapPointOfInterest poi)
				{
					this.targetPoi = poi;
				});
			}
		}

		// Token: 0x040005B6 RID: 1462
		private static TravelPortal prefab;

		// Token: 0x040005B7 RID: 1463
		public MapPointOfInterest targetPoi;

		// Token: 0x040005B8 RID: 1464
		public bool isForwardPortal;

		// Token: 0x040005B9 RID: 1465
		public string portalName;

		// Token: 0x040005BA RID: 1466
		public string portalDesc;
	}
}
