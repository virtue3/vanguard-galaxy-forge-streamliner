using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Effects;
using Behaviour.Managers;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Equipment.Turret.CombatTurrets
{
	// Token: 0x0200035D RID: 861
	public class RailCannonTurret : AbstractCombatTurret
	{
		// Token: 0x060020B9 RID: 8377 RVA: 0x000BF8CE File Offset: 0x000BDACE
		protected override void Start()
		{
			base.Start();
			this.CreateRailGunEffects();
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000BF8DC File Offset: 0x000BDADC
		private void CreateRailGunEffects()
		{
			for (int i = 0; i < this.firePoints.Length; i++)
			{
				this.CreateRailgunEffect();
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000BF904 File Offset: 0x000BDB04
		public void CreateRailgunEffect()
		{
			RailgunEffect railgunEffect = UnityEngine.Object.Instantiate<RailgunEffect>(this.railgunEffectPrefab, Vector3.zero, Quaternion.identity, this.turretPieceToRotate);
			railgunEffect.transform.localPosition = Vector2.zero;
			railgunEffect.transform.localRotation = Quaternion.identity;
			railgunEffect.SetChargeTime(this.chargeTime);
			railgunEffect.SetDischargeTime(this.dischargeTime);
			railgunEffect.Stop();
			this.railgunEffects.Add(railgunEffect);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000BF97C File Offset: 0x000BDB7C
		protected override void Update()
		{
			base.Update();
			if (this.railgunEffects[this.firePointIndex].isPlaying() && base.trackingTarget)
			{
				this.UpdateEffectPosition();
			}
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000BF9AF File Offset: 0x000BDBAF
		protected override bool FireInternal()
		{
			base.StartCoroutine(this.FireForEffect());
			return true;
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000BF9BF File Offset: 0x000BDBBF
		private IEnumerator FireForEffect()
		{
			base.StartCoroutine(this.PlayWeaponEffect());
			yield return new WaitForSeconds(this.railgunEffects[this.firePointIndex].chargeTime - this.dischargeTime);
			if (!base.trackingTarget)
			{
				this.CancelEffects();
				yield break;
			}
			if (this.recoil > 0f)
			{
				Vector2 a = -this.firePoints[this.firePointIndex].right;
				base.parent.rigidbody.AddForce(a * this.recoil, ForceMode2D.Impulse);
			}
			Ray2D ray2D = new Ray2D(this.firePoints[this.firePointIndex].position, this.firePoints[this.firePointIndex].right);
			this.UpdateEffectPosition();
			Vector3 position = this.firePoints[this.firePointIndex].position;
			RaycastHit2D[] array = Physics2D.RaycastAll(ray2D.origin, ray2D.direction, base.range);
			int num = 0;
			foreach (RaycastHit2D hit in array)
			{
				if (!hit || !hit.collider)
				{
					yield break;
				}
				IDamageable damageable;
				if (hit.collider.TryGetComponent<IDamageable>(out damageable) && damageable.enabled && damageable.IsEnemy(base.parent))
				{
					DamageData damageData = this.CreateDamageData(hit.transform, new Vector2?(hit.point), TargetLayer.Surface);
					if (num > 0 && this.penetrationDropoff)
					{
						float power = damageData.power * Mathf.Pow(this.penetrationMultiplier, (float)num);
						damageData.power = power;
					}
					damageData.effectColor = Color.blue;
					damageable.TakeDamage(damageData);
					Singleton<EffectManager>.Instance.PlaySparksEffect(hit.point, 0.1f, 0f, 0.2f, null, null);
					Singleton<EffectManager>.Instance.PlaySmokeEffect(hit.point, 0.5f, null, 4f, 15);
					num++;
					if (!this.penetration || num > 5)
					{
						this.railgunEffects[this.firePointIndex].SetTargetPosition(hit.point);
						break;
					}
				}
			}
			yield break;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000BF9CE File Offset: 0x000BDBCE
		protected override IEnumerator PlayWeaponEffect()
		{
			RailgunEffect railgunEffect = this.railgunEffects[this.firePointIndex];
			this.UpdateEffectPosition();
			railgunEffect.visualEffect.Reinit();
			railgunEffect.Play();
			yield return new WaitForSeconds(railgunEffect.GetPlayTime());
			railgunEffect.Stop();
			yield break;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000BF9E0 File Offset: 0x000BDBE0
		private void UpdateEffectPosition()
		{
			this.railgunEffects[this.firePointIndex].SetSpawnPosition(this.firePoints[this.firePointIndex].position);
			Vector2 vector = this.firePoints[this.firePointIndex].right;
			Vector2 targetPosition;
			if (this.penetration)
			{
				targetPosition = (Vector2)this.firePoints[this.firePointIndex].position + vector.normalized * 70f;
			}
			else
			{
				targetPosition = base.trackingTarget.transform.position;
			}
			this.railgunEffects[this.firePointIndex].SetTargetPosition(targetPosition);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000BFA97 File Offset: 0x000BDC97
		private void OnDisable()
		{
			if (this.railgunEffects.Count <= 0)
			{
				return;
			}
			this.CancelEffects();
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000BFAB0 File Offset: 0x000BDCB0
		private void CancelEffects()
		{
			base.StopAllCoroutines();
			foreach (RailgunEffect railgunEffect in this.railgunEffects)
			{
				railgunEffect.Stop();
			}
		}

		// Token: 0x04001383 RID: 4995
		[Header("Railgun Specific")]
		public float chargeTime;

		// Token: 0x04001384 RID: 4996
		public float dischargeTime;

		// Token: 0x04001385 RID: 4997
		public RailgunEffect railgunEffectPrefab;

		// Token: 0x04001386 RID: 4998
		public LayerMask layeroMask;

		// Token: 0x04001387 RID: 4999
		public bool penetration = true;

		// Token: 0x04001388 RID: 5000
		public bool penetrationDropoff = true;

		// Token: 0x04001389 RID: 5001
		[Range(0f, 1f)]
		public float penetrationMultiplier = 0.5f;

		// Token: 0x0400138A RID: 5002
		protected List<RailgunEffect> railgunEffects = new List<RailgunEffect>();

		// Token: 0x0400138B RID: 5003
		[SerializeField]
		protected float recoil = 800f;
	}
}
