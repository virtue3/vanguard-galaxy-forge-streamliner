using System;
using Behaviour.Managers;
using LightJson;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x0200011B RID: 283
	public class PrefabData : PersistableData
	{
		// Token: 0x06000ABF RID: 2751 RVA: 0x0004FAF0 File Offset: 0x0004DCF0
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			return UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/" + this.prefabName), this.position, base.rotation, parent.transform);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0004FB23 File Offset: 0x0004DD23
		public override void DataToJson(JsonObject json)
		{
			json["prefabName"] = this.prefabName;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0004FB3B File Offset: 0x0004DD3B
		public override void LoadFromJson(JsonObject json)
		{
			this.prefabName = json["prefabName"];
		}

		// Token: 0x040005C0 RID: 1472
		public string prefabName;
	}
}
