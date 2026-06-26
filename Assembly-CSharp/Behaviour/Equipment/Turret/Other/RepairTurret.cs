using System;
using System.Collections;
using Behaviour.Effects;
using Behaviour.Weapons;
using Source.Item;
using Source.Util;
using UnityEngine;
using UnityEngine.VFX;

namespace Behaviour.Equipment.Turret.Other
{
	// Token: 0x02000355 RID: 853
	public class RepairTurret : AbstractTurret
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x000BEDA0 File Offset: 0x000BCFA0
		public override EquipStat powerStat
		{
			get
			{
				return EquipStat.Power;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x000BEDA4 File Offset: 0x000BCFA4
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Generic;
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000BEDA7 File Offset: 0x000BCFA7
		protected override void Start()
		{
			base.Start();
			this.CreateLaserEffect();
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000BEDB5 File Offset: 0x000BCFB5
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

		// Token: 0x06002080 RID: 8320 RVA: 0x000BEDE0 File Offset: 0x000BCFE0
		private void CreateLaserEffect()
		{
			if (!this.laserEffect)
			{
				this.laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserEffectPrefab, Vector3.zero, Quaternion.identity, this.turretPieceToRotate);
				this.laserEffect.transform.localPosition = Vector2.zero;
				this.laserEffect.transform.localRotation = Quaternion.identity;
				this.laserEffect.Stop();
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000BEE55 File Offset: 0x000BD055
		public override void Activate()
		{
			base.Activate();
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x000BEE5D File Offset: 0x000BD05D
		public override void Deactivate()
		{
			base.Deactivate();
			base.targetingModule.ResetCurrentTargets();
			LaserEffect laserEffect = this.laserEffect;
			if (laserEffect != null)
			{
				laserEffect.Stop();
			}
			LaserEffect laserEffect2 = this.laserEffect;
			if (laserEffect2 == null)
			{
				return;
			}
			VisualEffect visualEffect = laserEffect2.visualEffect;
			if (visualEffect == null)
			{
				return;
			}
			visualEffect.Reinit();
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000BEE9B File Offset: 0x000BD09B
		private void OnDisable()
		{
			if (!this.laserEffect)
			{
				return;
			}
			base.StopAllCoroutines();
			this.laserEffect.Stop();
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000BEEBC File Offset: 0x000BD0BC
		private void SetLaserEffectStartAndSpawn()
		{
			this.laserEffect.SetTargetPosition(base.currentTarget.transform.position);
			this.laserEffect.SetSpawnPosition(base.transform.position);
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000BEEEF File Offset: 0x000BD0EF
		protected override bool FireInternal()
		{
			this.SetLaserEffectStartAndSpawn();
			base.StartCoroutine(this.FireForEffect());
			return true;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000BEF05 File Offset: 0x000BD105
		private IEnumerator FireForEffect()
		{
			TargetableUnit currentTarget = base.currentTarget;
			GameObject trackingTarget = base.trackingTarget;
			DamageData damageData = this.CreateDamageData(null, null, TargetLayer.Surface);
			damageData.hitCoordinates = trackingTarget.transform.position;
			damageData.effectColor = this.effectColor;
			this.overrideRotationTarget = trackingTarget;
			yield return this.PlayWeaponEffect();
			this.overrideRotationTarget = null;
			if (!trackingTarget || !currentTarget)
			{
				yield break;
			}
			currentTarget.TakeDamage(damageData);
			yield break;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000BEF14 File Offset: 0x000BD114
		protected override IEnumerator PlayWeaponEffect()
		{
			this.laserEffect.SetObjectsToTrack(this.firePoints[this.firePointIndex].gameObject, base.trackingTarget);
			this.laserEffect.SetPower(this.effectPower);
			this.laserEffect.SetFrequency(this.effectFrequency);
			this.laserEffect.SetSize(this.effectSize);
			this.laserEffect.SetColor(this.effectColor);
			this.laserEffect.Play();
			yield return new WaitForSeconds(this.effectDuration);
			this.laserEffect.Stop();
			yield break;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000BEF23 File Offset: 0x000BD123
		protected override void OnDestroy()
		{
			base.OnDestroy();
			LaserEffect laserEffect = this.laserEffect;
			if (laserEffect != null)
			{
				laserEffect.Stop();
			}
			UnityEngine.Object.Destroy(this.laserEffect);
		}

		// Token: 0x04001367 RID: 4967
		[Header("Laser Setup")]
		[SerializeField]
		private LaserEffect laserEffectPrefab;

		// Token: 0x04001368 RID: 4968
		[SerializeField]
		private float effectDuration;

		// Token: 0x04001369 RID: 4969
		[SerializeField]
		private Color effectColor;

		// Token: 0x0400136A RID: 4970
		[SerializeField]
		private float effectPower;

		// Token: 0x0400136B RID: 4971
		[SerializeField]
		private float effectFrequency;

		// Token: 0x0400136C RID: 4972
		[SerializeField]
		private float effectSize;

		// Token: 0x0400136D RID: 4973
		private LaserEffect laserEffect;
	}
}
