using System;
using Behaviour;
using Behaviour.Managers;
using Behaviour.Travel;
using Behaviour.Util;
using LightJson;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x0200011F RID: 287
	public class TutorialJumpgateData : PersistableData
	{
		// Token: 0x06000AFC RID: 2812 RVA: 0x000519D3 File Offset: 0x0004FBD3
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			return UnityEngine.Object.Instantiate<TutorialJumpgate>(PersistentSingleton<GameManager>.Instance.tutorialJumpgatePrefab, this.position, base.rotation, parent.transform).gameObject;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00051A00 File Offset: 0x0004FC00
		public override void DataToJson(JsonObject json)
		{
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00051A02 File Offset: 0x0004FC02
		public override void LoadFromJson(JsonObject json)
		{
		}
	}
}
