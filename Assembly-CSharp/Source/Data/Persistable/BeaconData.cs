using System;
using Behaviour;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Util;
using LightJson;
using Source.Util;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000111 RID: 273
	public class BeaconData : PersistableData
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x0004EA5B File Offset: 0x0004CC5B
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			Beacon beacon = UnityEngine.Object.Instantiate<Beacon>(PersistentSingleton<GameManager>.Instance.beaconPrefab, this.position, base.rotation, parent.transform);
			beacon.transform.Z(ZIndex.Station);
			return beacon.gameObject;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0004EA95 File Offset: 0x0004CC95
		public override void DataToJson(JsonObject json)
		{
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0004EA97 File Offset: 0x0004CC97
		public override void LoadFromJson(JsonObject json)
		{
		}
	}
}
