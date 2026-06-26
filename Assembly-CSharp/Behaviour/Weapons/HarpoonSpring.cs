using System;
using Source.Util;
using UnityEngine;
using _Scripts.Behaviour.Effects.Weapon;

namespace Behaviour.Weapons
{
	// Token: 0x020001AB RID: 427
	public class HarpoonSpring : MonoBehaviour
	{
		// Token: 0x06000EFF RID: 3839 RVA: 0x00069551 File Offset: 0x00067751
		private void Start()
		{
			this.effect = UnityEngine.Object.Instantiate<HarpoonGrinderEffect>(this.harpoonGrinderEffect, base.transform);
			base.transform.Z(ZIndex.Projectile);
			this.effect.Play();
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00069581 File Offset: 0x00067781
		public void SetPositions(Vector2 pos1, Vector2 pos2)
		{
			this.effect.SetSpawnPosition(pos1);
			this.effect.SetTargetPosition(pos2);
		}

		// Token: 0x04000884 RID: 2180
		[SerializeField]
		private HarpoonGrinderEffect harpoonGrinderEffect;

		// Token: 0x04000885 RID: 2181
		private HarpoonGrinderEffect effect;
	}
}
