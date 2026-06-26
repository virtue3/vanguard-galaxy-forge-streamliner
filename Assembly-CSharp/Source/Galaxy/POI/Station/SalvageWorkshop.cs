using System;
using LightJson;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000165 RID: 357
	public class SalvageWorkshop : IJsonSource
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x00062E18 File Offset: 0x00061018
		public SalvageWorkshop(SpaceStation parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00062E27 File Offset: 0x00061027
		public JsonValue ToJson()
		{
			return new JsonObject();
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00062E33 File Offset: 0x00061033
		public static SalvageWorkshop FromJson(JsonObject json, SpaceStation parent)
		{
			return new SalvageWorkshop(parent);
		}

		// Token: 0x04000779 RID: 1913
		private readonly SpaceStation parent;
	}
}
