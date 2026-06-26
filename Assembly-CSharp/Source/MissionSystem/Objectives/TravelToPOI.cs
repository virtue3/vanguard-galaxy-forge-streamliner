using System;
using LightJson;
using Source.Galaxy;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000D0 RID: 208
	public class TravelToPOI : MissionObjective
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0003FBEF File Offset: 0x0003DDEF
		public override string statusText
		{
			get
			{
				string result;
				if ((result = this.customDescription) == null)
				{
					string str = "Travel to: ";
					GalaxyMapData current = GalaxyMapData.current;
					result = str + ((current != null) ? current.GetPointOfInterest(this.targetPOI).name : null);
				}
				return result;
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0003FC24 File Offset: 0x0003DE24
		public override bool IsComplete()
		{
			GalaxyMapData current = GalaxyMapData.current;
			MapPointOfInterest mapPointOfInterest = (current != null) ? current.GetPointOfInterest(this.targetPOI) : null;
			if (mapPointOfInterest == null)
			{
				Debug.LogWarning("Can't find target POI in the galaxy: " + this.targetPOI + ".");
				return false;
			}
			return mapPointOfInterest.lastVisitedTime > 0f;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0003FC75 File Offset: 0x0003DE75
		public override MapPointOfInterest GetPoi()
		{
			return GalaxyMapData.current.GetPointOfInterest(this.targetPOI);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0003FC87 File Offset: 0x0003DE87
		protected override void DataToJson(JsonObject data)
		{
			data["poiGuid"] = this.targetPOI;
			data["cusDes"] = this.customDescription;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0003FCB5 File Offset: 0x0003DEB5
		protected override void LoadFromJson(JsonObject data)
		{
			this.targetPOI = data["poiGuid"];
			this.customDescription = data["cusDes"];
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0003FCE3 File Offset: 0x0003DEE3
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x0400047D RID: 1149
		public string targetPOI;
	}
}
