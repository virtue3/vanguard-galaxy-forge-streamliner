using System;
using Behaviour.Equipment.Aspect;
using LightJson;

namespace Source.Item
{
	// Token: 0x020000EB RID: 235
	[Serializable]
	public class AspectSlot : IJsonSource
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x000463EA File Offset: 0x000445EA
		public AspectSlot(EquipAspect equipAspect, int indexSlot)
		{
			this.equipAspect = equipAspect;
			this.indexSlot = indexSlot;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00046400 File Offset: 0x00044600
		public bool IsAspectEmpty()
		{
			return this.equipAspect;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0004640D File Offset: 0x0004460D
		public void SetEquipAspect(EquipAspect equipAspect)
		{
			this.equipAspect = equipAspect;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00046418 File Offset: 0x00044618
		public static AspectSlot FromJson(JsonValue data)
		{
			EquipAspect equipAspect = null;
			if (!data["equipAspect"].IsNull)
			{
				equipAspect = EquipAspect.Get(data["equipAspect"]);
			}
			int num = (int)data["index"].AsNumber;
			return new AspectSlot(equipAspect, num);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00046474 File Offset: 0x00044674
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"equipAspect",
					(this.equipAspect != null) ? this.equipAspect.identifier : null
				},
				{
					"index",
					this.indexSlot.ToString()
				}
			};
		}

		// Token: 0x040004AC RID: 1196
		public EquipAspect equipAspect;

		// Token: 0x040004AD RID: 1197
		public int indexSlot;
	}
}
