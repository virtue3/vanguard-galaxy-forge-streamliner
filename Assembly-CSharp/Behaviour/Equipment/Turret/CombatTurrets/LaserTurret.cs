using System;
using System.Collections;
using Behaviour.Effects;
using Behaviour.Weapons;
using UnityEngine;
using UnityEngine.VFX;

namespace Behaviour.Equipment.Turret.CombatTurrets
{
	// Token: 0x0200035B RID: 859
	public class LaserTurret : AbstractCombatTurret
	{
		// Token: 0x060020A9 RID: 8361 RVA: 0x000BF6E3 File Offset: 0x000BD8E3
		protected override void Start()
		{
			base.Start();
			this.CreateLaserEffect();
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000BF6F1 File Offset: 0x000BD8F1
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

		// Token: 0x060020AB RID: 8363 RVA: 0x000BF71C File Offset: 0x000BD91C
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

		// Token: 0x060020AC RID: 8364 RVA: 0x000BF791 File Offset: 0x000BD991
		public override void Activate()
		{
			base.Activate();
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000BF799 File Offset: 0x000BD999
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

		// Token: 0x060020AE RID: 8366 RVA: 0x000BF7D7 File Offset: 0x000BD9D7
		private void OnDisable()
		{
			if (!this.laserEffect)
			{
				return;
			}
			base.StopAllCoroutines();
			this.laserEffect.Stop();
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000BF7F8 File Offset: 0x000BD9F8
		private void SetLaserEffectStartAndSpawn()
		{
			this.laserEffect.SetTargetPosition(base.currentTarget.transform.position);
			this.laserEffect.SetSpawnPosition(base.transform.position);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000BF82B File Offset: 0x000BDA2B
		protected override bool FireInternal()
		{
			this.SetLaserEffectStartAndSpawn();
			base.StartCoroutine(this.FireForEffect());
			return true;
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000BF841 File Offset: 0x000BDA41
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

		// Token: 0x060020B2 RID: 8370 RVA: 0x000BF850 File Offset: 0x000BDA50
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

		// Token: 0x060020B3 RID: 8371 RVA: 0x000BF85F File Offset: 0x000BDA5F
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

		// Token: 0x0400137A RID: 4986
		[Header("Laser Setup")]
		[SerializeField]
		private LaserEffect laserEffectPrefab;

		// Token: 0x0400137B RID: 4987
		[SerializeField]
		private float effectDuration;

		// Token: 0x0400137C RID: 4988
		[SerializeField]
		private Color effectColor;

		// Token: 0x0400137D RID: 4989
		[SerializeField]
		private float effectPower;

		// Token: 0x0400137E RID: 4990
		[SerializeField]
		private float effectFrequency;

		// Token: 0x0400137F RID: 4991
		[SerializeField]
		private float effectSize;

		// Token: 0x04001380 RID: 4992
		private LaserEffect laserEffect;
	}
}
