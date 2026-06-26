using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Behaviour.Effects.Data
{
	// Token: 0x0200039A RID: 922
	[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer, null)]
	public struct ShieldHitData
	{
		// Token: 0x06002300 RID: 8960 RVA: 0x000C9270 File Offset: 0x000C7470
		public ShieldHitData(Vector2 position, float duration, Vector4 color, float size, Vector2 shipSize)
		{
			this.color = color;
			this.position = position;
			this.duration = duration;
			this.size = size;
			this.shipSize = shipSize;
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000C9298 File Offset: 0x000C7498
		public new string ToString()
		{
			string[] array = new string[8];
			array[0] = "pos: ";
			int num = 1;
			Vector2 vector = this.position;
			array[num] = vector.ToString();
			array[2] = ", size: ";
			array[3] = this.size.ToString();
			array[4] = ", duration: ";
			array[5] = this.duration.ToString();
			array[6] = ", color: ";
			int num2 = 7;
			Vector4 vector2 = this.color;
			array[num2] = vector2.ToString();
			return string.Concat(array);
		}

		// Token: 0x040014E5 RID: 5349
		public Vector2 position;

		// Token: 0x040014E6 RID: 5350
		public float duration;

		// Token: 0x040014E7 RID: 5351
		public float size;

		// Token: 0x040014E8 RID: 5352
		public Vector4 color;

		// Token: 0x040014E9 RID: 5353
		public Vector2 shipSize;
	}
}
