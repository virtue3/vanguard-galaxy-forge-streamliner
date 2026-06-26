using System;
using Behaviour.Unit;
using LightJson;
using UnityEngine;

namespace Source.Data
{
	// Token: 0x0200010D RID: 269
	public class UnitPositionData : IJsonSource
	{
		// Token: 0x06000A41 RID: 2625 RVA: 0x0004DF68 File Offset: 0x0004C168
		public void SetDataFromRigidbody(AbstractUnit unit)
		{
			if (unit && unit.rigidbody)
			{
				Rigidbody2D rigidbody = unit.rigidbody;
				this.position = rigidbody.position;
				this.velocity = rigidbody.linearVelocity;
				this.rotation = rigidbody.rotation;
				this.angularVelocity = rigidbody.angularVelocity;
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0004DFC1 File Offset: 0x0004C1C1
		public void SetDataToRigidbody(Rigidbody2D rigidbody)
		{
			rigidbody.position = this.position;
			rigidbody.linearVelocity = this.velocity;
			rigidbody.rotation = this.rotation;
			rigidbody.angularVelocity = this.angularVelocity;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0004DFF4 File Offset: 0x0004C1F4
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"position",
					JsonUtil.Vector2ToJson(this.position)
				},
				{
					"velocity",
					JsonUtil.Vector2ToJson(this.velocity)
				},
				{
					"rotation",
					new double?((double)this.rotation)
				},
				{
					"angularVelocity",
					new double?((double)this.angularVelocity)
				}
			};
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0004E080 File Offset: 0x0004C280
		public static UnitPositionData FromJson(JsonValue json)
		{
			return new UnitPositionData
			{
				position = JsonUtil.JsonObjectToVector2(json["position"]),
				velocity = JsonUtil.JsonObjectToVector2(json["velocity"]),
				rotation = (float)json["rotation"].AsNumber,
				angularVelocity = (float)json["angularVelocity"].AsNumber
			};
		}

		// Token: 0x04000592 RID: 1426
		public Vector2 position = Vector2.zero;

		// Token: 0x04000593 RID: 1427
		public Vector2 velocity = Vector2.zero;

		// Token: 0x04000594 RID: 1428
		public float rotation;

		// Token: 0x04000595 RID: 1429
		public float angularVelocity;

		// Token: 0x04000596 RID: 1430
		public Vector2? overrideTarget = new Vector2?(Vector2.zero);
	}
}
