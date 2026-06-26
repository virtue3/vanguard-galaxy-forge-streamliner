using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Effects
{
	// Token: 0x02000381 RID: 897
	public class FreeFormLightLine
	{
		// Token: 0x0600228F RID: 8847 RVA: 0x000C7A4C File Offset: 0x000C5C4C
		public FreeFormLightLine(Light2D light)
		{
			this.light2D = light;
			this.lightType = this.light2D.GetType();
			this.field = this.lightType.GetField("m_ShapePath", this.privateInstanceFlags);
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000C7A9C File Offset: 0x000C5C9C
		public void SetShape()
		{
			this.rect = new Rect(new Vector2(0f, -0.2f), new Vector2(this.length, 0.4f));
			this.path = new Vector3[]
			{
				this.rect.min,
				new Vector2(this.rect.max.x, this.rect.min.y),
				this.rect.max,
				new Vector2(this.rect.min.x, this.rect.max.y)
			};
			this.field.SetValue(this.light2D, this.path);
			object[] parameters = new object[]
			{
				true
			};
			this.lightType.GetMethod("UpdateMesh", this.privateInstanceFlags).Invoke(this.light2D, parameters);
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000C7BBC File Offset: 0x000C5DBC
		public void UpdateFreeformLight(Vector2 from, Vector2 to)
		{
			float num = Vector2.Distance(from, to) * 1.5f;
			if (Mathf.Abs(this.length - num) > 0.5f)
			{
				this.length = num;
				this.SetShape();
			}
		}

		// Token: 0x04001470 RID: 5232
		private float length;

		// Token: 0x04001471 RID: 5233
		private Light2D light2D;

		// Token: 0x04001472 RID: 5234
		private FieldInfo field;

		// Token: 0x04001473 RID: 5235
		private Type lightType;

		// Token: 0x04001474 RID: 5236
		private BindingFlags privateInstanceFlags = BindingFlags.Instance | BindingFlags.NonPublic;

		// Token: 0x04001475 RID: 5237
		private Rect rect;

		// Token: 0x04001476 RID: 5238
		private Vector3[] path;
	}
}
