using System;
using Behaviour.Effects;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Tractoring
{
	// Token: 0x020002D6 RID: 726
	public class DroneTractorBeam : MonoBehaviour
	{
		// Token: 0x06001A89 RID: 6793 RVA: 0x000A4468 File Offset: 0x000A2668
		private void Start()
		{
			this.beamEffect = base.GetComponentInChildren<BeamEffect>();
			this.beamEffect.SetPower(this.effectPower);
			this.beamEffect.SetFrequency(this.effectFrequency);
			this.beamEffect.SetSize(this.effectSize);
			this.beamEffect.Stop();
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x000A44BF File Offset: 0x000A26BF
		public void StartTractoring(Drone target)
		{
			this.target = target;
			this.beamEffect.SetColor(this.effectColor);
			this.beamEffect.SetObjectsToTrack(base.gameObject, target.gameObject);
			this.beamEffect.Play();
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000A44FB File Offset: 0x000A26FB
		public void StopTractoring()
		{
			this.target = null;
			this.beamEffect.Stop();
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000A450F File Offset: 0x000A270F
		public bool HasTarget()
		{
			return this.target != null;
		}

		// Token: 0x040010B9 RID: 4281
		[Header("Beam effect")]
		[SerializeField]
		private BeamEffect beamEffect;

		// Token: 0x040010BA RID: 4282
		[SerializeField]
		private Color effectColor;

		// Token: 0x040010BB RID: 4283
		[SerializeField]
		private Color bonusEffectColor;

		// Token: 0x040010BC RID: 4284
		[SerializeField]
		private float effectPower;

		// Token: 0x040010BD RID: 4285
		[SerializeField]
		private float effectFrequency;

		// Token: 0x040010BE RID: 4286
		[SerializeField]
		private float effectSize;

		// Token: 0x040010BF RID: 4287
		[SerializeField]
		private float effectLifetime;

		// Token: 0x040010C0 RID: 4288
		private Drone target;
	}
}
