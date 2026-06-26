using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000390 RID: 912
	public class JumpGateEffect : AbstractEffect
	{
		// Token: 0x060022D4 RID: 8916 RVA: 0x000C8804 File Offset: 0x000C6A04
		protected override void Awake()
		{
			base.Awake();
			base.visualEffect.SetVector2("StartSize", this.startSize);
			base.visualEffect.SetFloat("CircleSize", this.circleSize);
			base.visualEffect.SetFloat("ParticleSize", this.particleSize);
			base.visualEffect.SetFloat("Lifetime", this.lifetime);
			base.visualEffect.SetFloat("AttractionSpeed", this.attractionSpeed);
			base.visualEffect.SetFloat("AttractionForce", this.attractionForce);
			base.visualEffect.SetVector2("AttractionCenter", this.attractionCenter);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000C88B1 File Offset: 0x000C6AB1
		public void SetAttractionSpeed(float attractionSpeed)
		{
			this.attractionSpeed = attractionSpeed;
			base.visualEffect.SetFloat("AttractionSpeed", attractionSpeed);
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000C88CC File Offset: 0x000C6ACC
		public void SetShipPosition(Vector2 shipPosition, float size)
		{
			this.followingShip = true;
			base.visualEffect.SetVector2("ShipPosition", shipPosition);
			base.visualEffect.SetFloat("ShipSize", size);
			base.visualEffect.SetVector2("ShipAttraction", new Vector2(800f, 1000f));
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000C8921 File Offset: 0x000C6B21
		public void StopFollowingShip()
		{
			if (this.followingShip)
			{
				base.visualEffect.SetVector2("ShipAttraction", Vector2.zero);
			}
			this.followingShip = false;
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000C8947 File Offset: 0x000C6B47
		public void SetAttractionCenter(Vector2 attractionCenter)
		{
			this.attractionCenter = attractionCenter;
			base.visualEffect.SetVector2("AttractionCenter", attractionCenter);
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000C8961 File Offset: 0x000C6B61
		public void SetParticleColor(Gradient particleColor)
		{
			this.particleColor = particleColor;
			base.visualEffect.SetGradient("ParticleColor", particleColor);
		}

		// Token: 0x040014B3 RID: 5299
		[SerializeField]
		private Vector2 startSize;

		// Token: 0x040014B4 RID: 5300
		[SerializeField]
		private float circleSize;

		// Token: 0x040014B5 RID: 5301
		[SerializeField]
		private float particleSize;

		// Token: 0x040014B6 RID: 5302
		[SerializeField]
		private float lifetime;

		// Token: 0x040014B7 RID: 5303
		[SerializeField]
		private float attractionSpeed;

		// Token: 0x040014B8 RID: 5304
		[SerializeField]
		private float attractionForce;

		// Token: 0x040014B9 RID: 5305
		[SerializeField]
		private Vector2 attractionCenter;

		// Token: 0x040014BA RID: 5306
		[SerializeField]
		private Gradient particleColor;

		// Token: 0x040014BB RID: 5307
		private bool followingShip;
	}
}
