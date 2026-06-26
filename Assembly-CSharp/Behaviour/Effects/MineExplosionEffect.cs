using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000384 RID: 900
	public class MineExplosionEffect : ExplosionEffect
	{
		// Token: 0x06002298 RID: 8856 RVA: 0x000C7CA9 File Offset: 0x000C5EA9
		protected override void Awake()
		{
			base.Awake();
			base.EnableLight(0.5f, new Color(0.9f, 0.8f, 0.8f));
		}
	}
}
