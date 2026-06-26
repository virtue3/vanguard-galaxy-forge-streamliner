using System;
using System.Collections;
using Behaviour.Effects;
using Behaviour.Managers;
using Behaviour.Salvage;
using Behaviour.Util;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Salvage
{
	// Token: 0x0200034C RID: 844
	public class SalvageLaserTurret : AbstractSalvageTurret
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x000BDD52 File Offset: 0x000BBF52
		private SalvageContainer salvageTarget
		{
			get
			{
				return base.currentTarget as SalvageContainer;
			}
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000BDD5F File Offset: 0x000BBF5F
		protected override void Start()
		{
			base.Start();
			this.CreateLaserEffect();
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000BDD6D File Offset: 0x000BBF6D
		private void OnEnable()
		{
			LaserEffect laserEffect = this.laserEffect;
			if (laserEffect != null)
			{
				laserEffect.visualEffect.Reinit();
			}
			LaserEffect laserEffect2 = this.laserEffect;
			if (laserEffect2 == null)
			{
				return;
			}
			laserEffect2.Stop();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000BDD95 File Offset: 0x000BBF95
		protected override bool FireInternal()
		{
			base.StartCoroutine(this.ShootAtTarget());
			return true;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000BDDA5 File Offset: 0x000BBFA5
		private IEnumerator ShootAtTarget()
		{
			TargetableUnit currentTarget = base.currentTarget;
			GameObject trackingTarget = base.trackingTarget;
			DamageData damage = this.CreateDamageData(null, null, TargetLayer.Surface);
			damage.hitTransform = trackingTarget.transform;
			damage.hitCoordinates = trackingTarget.transform.position;
			this.overrideRotationTarget = trackingTarget;
			yield return this.PlayWeaponEffect();
			this.overrideRotationTarget = null;
			if (!trackingTarget)
			{
				yield break;
			}
			Singleton<EffectManager>.Instance.PlaySmokeEffect(trackingTarget.transform.position, 0.2f, null, 4f, 15);
			if (currentTarget != null && currentTarget.CanBeDamagedBy(this))
			{
				currentTarget.TakeDamage(damage);
			}
			yield break;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000BDDB4 File Offset: 0x000BBFB4
		private void CreateLaserEffect()
		{
			this.laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserEffectPrefab, Vector3.zero, Quaternion.identity, this.turretPieceToRotate);
			this.laserEffect.transform.localPosition = Vector2.zero;
			this.laserEffect.transform.localRotation = Quaternion.identity;
			this.laserEffect.Stop();
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000BDE1C File Offset: 0x000BC01C
		protected override IEnumerator PlayWeaponEffect()
		{
			this.laserEffect.SetObjectsToTrack(this.firePoints[this.firePointIndex].gameObject, base.trackingTarget);
			this.laserEffect.Play();
			yield return new WaitForSeconds(1f);
			this.laserEffect.Stop();
			yield break;
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000BDE2B File Offset: 0x000BC02B
		private void OnDisable()
		{
			if (!this.laserEffect)
			{
				return;
			}
			base.StopAllCoroutines();
			this.laserEffect.Stop();
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000BDE4C File Offset: 0x000BC04C
		protected override DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			DamageData damageData = base.CreateDamageData(targetTransform, hitCoordinates, targetLayer);
			damageData.hitTransform = base.trackingTarget.transform;
			damageData.hitCoordinates = base.trackingTarget.transform.position;
			return damageData;
		}

		// Token: 0x04001343 RID: 4931
		[Header("Laser Setup")]
		[SerializeField]
		private LaserEffect laserEffectPrefab;

		// Token: 0x04001344 RID: 4932
		private LaserEffect laserEffect;
	}
}
