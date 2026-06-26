using System;
using Behaviour.Effects.Weapon;
using Behaviour.Managers;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Equipment.Turret.CombatTurrets
{
	// Token: 0x0200035A RID: 858
	public class GatlingTurret : AbstractCombatTurret
	{
		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x000BF435 File Offset: 0x000BD635
		public override int shotsPerAmmo
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000BF438 File Offset: 0x000BD638
		protected override bool FireInternal()
		{
			DamageData damageData = this.CreateDamageData(null, null, TargetLayer.Surface);
			float num = this.accuracyAngle * Vector3.Distance(this.firePoints[this.firePointIndex].position, base.trackingTarget.transform.position) * 0.015f;
			Vector3 vector = base.trackingTarget.transform.position + new Vector3(SeededRandom.Global.RandomRange(-num, num), SeededRandom.Global.RandomRange(-num, num));
			damageData.hitCoordinates = vector;
			Vector3 normalized = (vector - this.firePoints[this.firePointIndex].position).normalized;
			vector += normalized * 5f;
			foreach (RaycastHit2D hit in Physics2D.LinecastAll(this.firePoints[this.firePointIndex].position, vector))
			{
				if (hit)
				{
					Collider2D collider = hit.collider;
					if ((collider != null) ? collider.gameObject : null)
					{
						Collider2D collider2 = hit.collider;
						UnityEngine.Object x = (collider2 != null) ? collider2.gameObject : null;
						TargetableUnit currentTarget = base.currentTarget;
						if (x == ((currentTarget != null) ? currentTarget.gameObject : null))
						{
							damageData.targetUnit = base.currentTarget;
							damageData.hitCoordinates = hit.point;
							this.UpdateHitIndicator(damageData);
							base.currentTarget.GetComponent<IDamageable>().TakeDamage(damageData);
							vector = hit.point;
							Singleton<EffectManager>.Instance.PlayFlashEffect(hit.point, this.tracerColor, 5f);
						}
					}
				}
			}
			UnityEngine.Object.Instantiate<HitscanTracerEffect>(this.tracerEffect, base.parent.transform.parent).ShowTracer(this.tracerColor, this.firePoints[this.firePointIndex].position, vector);
			base.PlayFiringSound();
			base.StartCoroutine(this.PlayWeaponEffect());
			return true;
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000BF648 File Offset: 0x000BD848
		private void UpdateHitIndicator(DamageData damageData)
		{
			if (!this.lastHitIndicator || this.lastHitIndicator.transform.parent != damageData.targetUnit.transform)
			{
				this.lastHitIndicator = new GameObject("Gatling Turret Hit");
				this.lastHitIndicator.transform.parent = damageData.targetUnit.transform;
			}
			this.lastHitIndicator.transform.position = damageData.hitCoordinates;
			damageData.hitTransform = this.lastHitIndicator.transform;
		}

		// Token: 0x04001376 RID: 4982
		private const float ScatterMultiplier = 0.015f;

		// Token: 0x04001377 RID: 4983
		[SerializeField]
		private HitscanTracerEffect tracerEffect;

		// Token: 0x04001378 RID: 4984
		public Color tracerColor;

		// Token: 0x04001379 RID: 4985
		private GameObject lastHitIndicator;
	}
}
