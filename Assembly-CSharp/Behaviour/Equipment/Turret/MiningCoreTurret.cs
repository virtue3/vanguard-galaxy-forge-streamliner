using System;
using Behaviour.Mining;
using Behaviour.Projectiles.Mining;
using Behaviour.Weapons;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret
{
	// Token: 0x02000346 RID: 838
	public class MiningCoreTurret : AbstractMiningTurret
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x000BD10E File Offset: 0x000BB30E
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Mining;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000BD111 File Offset: 0x000BB311
		public override TargetLayer targetLayer
		{
			get
			{
				return TargetLayer.Core;
			}
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000BD114 File Offset: 0x000BB314
		public override bool CanMineAsteroidTarget(Asteroid target)
		{
			return base.targetsCore && !target.innerCoreDepleted;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000BD12C File Offset: 0x000BB32C
		protected override bool FireInternal()
		{
			if ((base.asteroidTarget && (base.asteroidTarget.isDetonating || base.asteroidTarget.EnoughCoreExplosivesOnTarget(this, this._maxMagSize))) || !base.asteroidTarget)
			{
				return false;
			}
			CoreExplosive component = UnityEngine.Object.Instantiate<GameObject>(this.explosivePrefab, this.firePoints[this.firePointIndex].position, this.firePoints[this.firePointIndex].rotation, base.asteroidTarget.transform).GetComponent<CoreExplosive>();
			Vector2 vector = this.GetRandomPointWithinCollider(base.asteroidTarget.GetCoreCollider());
			component.Initialize(this, base.asteroidTarget, vector);
			component.damage = this.CreateDamageData(vector, component.transform);
			base.asteroidTarget.AddCoreExplosive(component);
			return true;
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000BD200 File Offset: 0x000BB400
		protected override void Update()
		{
			if (base.asteroidTarget && !base.asteroidTarget.isDetonating && base.asteroidTarget.EnoughCoreExplosivesOnTarget(this, this._maxMagSize))
			{
				if (base.asteroidTarget.AreExplosivesLatched(this))
				{
					base.asteroidTarget.DetonateCoreExplosives(this);
					return;
				}
			}
			else
			{
				base.Update();
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000BD25C File Offset: 0x000BB45C
		private Vector3 GetRandomPointWithinCollider(PolygonCollider2D collider)
		{
			Bounds bounds = collider.bounds;
			int num = 100;
			int num2 = 0;
			Vector3 vector;
			bool flag;
			do
			{
				vector = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), bounds.center.z);
				flag = (collider.OverlapPoint(vector) && !base.asteroidTarget.IsOverlappingWithOtherExplosives(vector, this.spreadRadius));
				num2++;
			}
			while (!flag && num2 < num);
			if (num2 >= num)
			{
				Debug.LogWarning("Could not find a non-overlapping point within the collider bounds.");
			}
			return vector;
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000BD30B File Offset: 0x000BB50B
		protected override float GetPowerMultiplier()
		{
			return 1.2f;
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x000BD314 File Offset: 0x000BB514
		public DamageData CreateDamageData(Vector2 coordinates, Transform hitTransform)
		{
			DamageData damageData = base.CreateDamageData(null, null, TargetLayer.Surface);
			damageData.hitCoordinates = coordinates;
			damageData.hitTransform = base.transform;
			return damageData;
		}

		// Token: 0x04001322 RID: 4898
		public GameObject explosivePrefab;

		// Token: 0x04001323 RID: 4899
		private float spreadRadius = 0.25f;

		// Token: 0x04001324 RID: 4900
		[Header("Core Explosives")]
		public float explosiveDelay = 0.25f;
	}
}
