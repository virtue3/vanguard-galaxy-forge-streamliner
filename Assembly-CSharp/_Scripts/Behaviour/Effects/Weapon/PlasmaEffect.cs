using System;
using Behaviour.Effects;

namespace _Scripts.Behaviour.Effects.Weapon
{
	// Token: 0x0200019B RID: 411
	public class PlasmaEffect : AbstractEffect
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x00068269 File Offset: 0x00066469
		public void SetAngle(float angle)
		{
			base.visualEffect.SetFloat("Angle", angle);
		}
	}
}
