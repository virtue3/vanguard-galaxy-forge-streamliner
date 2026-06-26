using System;
using System.Collections;
using Behaviour.AudioSystem;
using Behaviour.Effects;
using Behaviour.Managers;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.AudioSystem;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile
{
	// Token: 0x02000350 RID: 848
	public class Missile : AbstractProjectile
	{
		// Token: 0x0600205C RID: 8284 RVA: 0x000BE344 File Offset: 0x000BC544
		protected override void Awake()
		{
			base.Awake();
			this.missileIgnitionEffect = UnityEngine.Object.Instantiate<MissileIgnitionEffect>(this.ignitionEffectPrefab, base.transform);
			this.missileIgnitionEffect.transform.localPosition = new Vector3(-0.31f, 0f, 0f);
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000BE394 File Offset: 0x000BC594
		public void Initialize(Transform target, DamageData damageData, float speed, bool seekingMissile, Vector2 initialSpeed)
		{
			this.rigidbody = base.gameObject.AddComponent<Rigidbody2D>();
			this.rigidbody.mass = this.mass;
			this.rigidbody.inertia = this.mass;
			this.rigidbody.linearVelocity = initialSpeed;
			this.rigidbody.angularDamping = 0f;
			this.rigidbody.AddRelativeForce(this.initialImpulse * Vector2.right, ForceMode2D.Impulse);
			this.rigidbody.interpolation = RigidbodyInterpolation2D.Extrapolate;
			this.target = target;
			this.maxSpeed = speed;
			this.damageData = damageData;
			this.parentTurret = damageData.source.GetComponent<AbstractTurret>();
			this.seeking = seekingMissile;
			this.acceleration = UnityEngine.Random.Range(this.acceleration * 0.9f, this.acceleration * 1.1f);
			Singleton<EffectManager>.Instance.PlayMissileTrailEffect(base.gameObject, this.ignitionDelay, this.missileTrailEffect);
			this.missileIgnitionEffect.Play(this.ignitionDelay);
			UnityEngine.Object.Destroy(base.gameObject, this.lifetime);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000BE4A8 File Offset: 0x000BC6A8
		protected override void Start()
		{
			base.StartCoroutine(this.Wait(this.ignitionDelay));
			base.transform.Z(ZIndex.Projectile);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000BE4CC File Offset: 0x000BC6CC
		private void UpdateTrackingTarget()
		{
			if (!this.parentTurret)
			{
				return;
			}
			if (!this.parentTurret.currentTarget)
			{
				return;
			}
			float d = Vector2.Distance(base.transform.position, this.parentTurret.currentTarget.transform.position) / this.maxSpeed;
			this.targetPosition = (Vector2)this.parentTurret.currentTarget.transform.position + this.parentTurret.currentTarget.targetableVelocity * d;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000BE56C File Offset: 0x000BC76C
		private void FixedUpdate()
		{
			if (!this.startTracking)
			{
				return;
			}
			if (this.target != null)
			{
				this.UpdateTrackingTarget();
				Vector3 vector = this.targetPosition - this.rigidbody.position;
				float value = Vector3.SignedAngle(vector, this.rigidbody.transform.right, Vector3.forward);
				float num = Vector2.Dot(this.rigidbody.linearVelocity.normalized, this.rigidbody.transform.right);
				float num2 = Vector2.Dot(this.rigidbody.linearVelocity.normalized, this.rigidbody.transform.up);
				if (num * this.rigidbody.linearVelocity.magnitude > this.minSpeed && Math.Abs(value) > 10f)
				{
					this.rigidbody.AddRelativeForce(Vector2.right * -this.thrust * 0.5f * Time.fixedDeltaTime);
				}
				else if (this.rigidbody.linearVelocity.magnitude < this.maxSpeed)
				{
					this.rigidbody.AddRelativeForce(Vector2.right * this.thrust * Time.fixedDeltaTime);
				}
				if (Math.Abs(num2) > 0.1f)
				{
					this.rigidbody.AddRelativeForce(Vector2.up * this.thrust * Time.fixedDeltaTime * -num2);
				}
				Quaternion to = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0f, 0f, 90f) * vector);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotateSpeed * Time.fixedDeltaTime);
				return;
			}
			if (this.seeking)
			{
				this.SeekNewTarget();
			}
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000BE760 File Offset: 0x000BC960
		protected override void OnHit(Collider2D collision)
		{
			IDamageable component = collision.GetComponent<IDamageable>();
			if (component != null)
			{
				component.TakeDamage(this.damageData);
			}
			if (this.fireSoundData != null)
			{
				PersistentSingleton<SoundManager>.Instance.CreateSound().WithSoundData(this.fireSoundData).WithPosition(base.transform.position).WithCustomVolume(0.6f).WithRandomPitch().Play();
			}
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000BE7CA File Offset: 0x000BC9CA
		private IEnumerator Wait(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			this.startTracking = true;
			yield break;
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000BE7E0 File Offset: 0x000BC9E0
		private void SeekNewTarget()
		{
			if (this.parentTurret.trackingTarget)
			{
				this.target = this.parentTurret.trackingTarget.transform;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000BE80A File Offset: 0x000BCA0A
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Singleton<EffectManager>.Instance.PlayExplosionEffect(base.gameObject.transform.position, Vector2.zero, 0.3f, ColorHelper.flashExplosionMissile, 0f);
		}

		// Token: 0x0400134E RID: 4942
		[SerializeField]
		private float ignitionDelay;

		// Token: 0x0400134F RID: 4943
		[SerializeField]
		private float mass;

		// Token: 0x04001350 RID: 4944
		[SerializeField]
		private float thrust;

		// Token: 0x04001351 RID: 4945
		[SerializeField]
		private float initialImpulse;

		// Token: 0x04001352 RID: 4946
		[SerializeField]
		private float rotateSpeed;

		// Token: 0x04001353 RID: 4947
		[SerializeField]
		private MissileIgnitionEffect ignitionEffectPrefab;

		// Token: 0x04001354 RID: 4948
		[SerializeField]
		private MissileTrailEffect missileTrailEffect;

		// Token: 0x04001355 RID: 4949
		[SerializeField]
		private TrailRenderer fixedTrail;

		// Token: 0x04001356 RID: 4950
		private Transform target;

		// Token: 0x04001357 RID: 4951
		private Vector2 targetPosition;

		// Token: 0x04001358 RID: 4952
		private float maxSpeed;

		// Token: 0x04001359 RID: 4953
		private float minSpeed = 0.5f;

		// Token: 0x0400135A RID: 4954
		private float acceleration = 0.8f;

		// Token: 0x0400135B RID: 4955
		private Rigidbody2D rigidbody;

		// Token: 0x0400135C RID: 4956
		private AbstractTurret parentTurret;

		// Token: 0x0400135D RID: 4957
		private bool seeking;

		// Token: 0x0400135E RID: 4958
		private bool startTracking;

		// Token: 0x0400135F RID: 4959
		private MissileIgnitionEffect missileIgnitionEffect;

		// Token: 0x04001360 RID: 4960
		[SerializeField]
		private SoundData fireSoundData;
	}
}
