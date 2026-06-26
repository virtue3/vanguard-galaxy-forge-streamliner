using System;
using Behaviour;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Util;
using LightJson;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000118 RID: 280
	public class MoveToAreaData : PersistableData
	{
		// Token: 0x06000AA9 RID: 2729 RVA: 0x0004F6EA File Offset: 0x0004D8EA
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			MoveToArea moveToArea = UnityEngine.Object.Instantiate<MoveToArea>(PersistentSingleton<GameManager>.Instance.moveToAreaPrefab, this.position, base.rotation, parent.transform);
			moveToArea.InitObject(this);
			return moveToArea.gameObject;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0004F71E File Offset: 0x0004D91E
		public override void DataToJson(JsonObject json)
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0004F720 File Offset: 0x0004D920
		public override void LoadFromJson(JsonObject json)
		{
		}
	}
}
