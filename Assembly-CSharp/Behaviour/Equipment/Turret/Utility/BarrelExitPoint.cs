using System;
using Behaviour.Effects;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Utility
{
	// Token: 0x02000349 RID: 841
	public class BarrelExitPoint : MonoBehaviour
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x000BD6C6 File Offset: 0x000BB8C6
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x000BD6CE File Offset: 0x000BB8CE
		public FirePoint firepoint { get; private set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x000BD6D7 File Offset: 0x000BB8D7
		// (set) Token: 0x0600202E RID: 8238 RVA: 0x000BD6DF File Offset: 0x000BB8DF
		public MuzzleFlashEffect muzzleFlashEffect { get; private set; }
	}
}
