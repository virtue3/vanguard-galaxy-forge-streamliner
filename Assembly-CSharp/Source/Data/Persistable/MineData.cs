using System;
using Behaviour.Managers;
using LightJson;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000117 RID: 279
	public class MineData : PersistableData
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x0004F6CF File Offset: 0x0004D8CF
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			return this.hazardData.AddToWorld(parent, this);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0004F6DE File Offset: 0x0004D8DE
		public override void DataToJson(JsonObject json)
		{
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0004F6E0 File Offset: 0x0004D8E0
		public override void LoadFromJson(JsonObject json)
		{
		}
	}
}
