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
	// Token: 0x02000114 RID: 276
	public class EscortDestinationData : PersistableData
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x0004F2BB File Offset: 0x0004D4BB
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			EscortDestination escortDestination = UnityEngine.Object.Instantiate<EscortDestination>(PersistentSingleton<GameManager>.Instance.escortDestinationPrefab, this.position, base.rotation, parent.transform);
			escortDestination.transform.Z(ZIndex.Station);
			return escortDestination.gameObject;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0004F2F5 File Offset: 0x0004D4F5
		public override void DataToJson(JsonObject json)
		{
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0004F2F7 File Offset: 0x0004D4F7
		public override void LoadFromJson(JsonObject json)
		{
		}
	}
}
