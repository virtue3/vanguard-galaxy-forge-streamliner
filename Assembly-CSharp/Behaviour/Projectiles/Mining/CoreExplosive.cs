using System;
using System.Collections;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Projectiles.Mining
{
	// Token: 0x0200033B RID: 827
	public class CoreExplosive : MonoBehaviour
	{
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x000BA691 File Offset: 0x000B8891
		// (set) Token: 0x06001F44 RID: 8004 RVA: 0x000BA699 File Offset: 0x000B8899
		public Transform asteroid { get; private set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x000BA6A2 File Offset: 0x000B88A2
		// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000BA6AA File Offset: 0x000B88AA
		public MiningCoreTurret source { get; private set; }

		// Token: 0x06001F47 RID: 8007 RVA: 0x000BA6B3 File Offset: 0x000B88B3
		private void Awake()
		{
			this.detonationLight = base.GetComponent<Light2D>();
			this.detonationLight.enabled = false;
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000BA6D9 File Offset: 0x000B88D9
		private void Start()
		{
			base.transform.Z(ZIndex.Projectile);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000BA6E8 File Offset: 0x000B88E8
		public void Initialize(MiningCoreTurret source, Asteroid targetAsteroid, Vector3 targetPosition)
		{
			this.asteroid = targetAsteroid.transform;
			this.asteroidCollider = this.asteroid.GetComponent<Collider2D>();
			this.source = source;
			this.targetPosition = targetPosition;
			this.targetPositionInAsteroid = this.asteroid.InverseTransformPoint(targetPosition);
			this.LatchOntoAsteroid();
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000BA73D File Offset: 0x000B893D
		public float RandomExplosiveDelay()
		{
			return UnityEngine.Random.Range(this.explosiveDelay, this.explosiveDelay * 2f);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000BA756 File Offset: 0x000B8956
		private void LatchOntoAsteroid()
		{
			if (this.asteroid == null || this.asteroidCollider == null)
			{
				Debug.LogWarning("Asteroid target or collider not set.");
				return;
			}
			base.StartCoroutine(this.SpreadOut());
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000BA78C File Offset: 0x000B898C
		private IEnumerator SpreadOut()
		{
			float elapsedTime = 0f;
			Vector3 startPosition = base.transform.position;
			float oscillationFrequency = 1.25f;
			float maxOscillationAmplitude = 0.5f;
			float randomSpreadTime = this.RandomSpreadTime();
			while (elapsedTime < randomSpreadTime)
			{
				float t = elapsedTime / randomSpreadTime;
				Vector3 position = Vector3.Lerp(startPosition, this.asteroid.TransformPoint(this.targetPositionInAsteroid), t);
				float num = Mathf.Lerp(maxOscillationAmplitude, 0f, t);
				float num2 = Mathf.Sin(elapsedTime * oscillationFrequency) * num;
				position.y += num2;
				position.z = base.transform.position.z;
				base.transform.position = position;
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			base.transform.position = this.asteroid.TransformPoint(this.targetPositionInAsteroid);
			base.transform.Z(ZIndex.AttachedToAsteroid);
			this.isLatched = true;
			this.detonationLight.enabled = true;
			yield break;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000BA79B File Offset: 0x000B899B
		private float RandomSpreadTime()
		{
			return UnityEngine.Random.Range(this.spreadTime * 0.75f, this.spreadTime * 1.25f);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000BA7BA File Offset: 0x000B89BA
		public IEnumerator Detonate(float detonateTime)
		{
			if (this.asteroid)
			{
				Asteroid mineable = this.asteroid.GetComponent<Asteroid>();
				float timePerFlash = detonateTime / 3f;
				int num;
				for (int i = 0; i < 3; i = num + 1)
				{
					this.detonationLight.enabled = false;
					yield return new WaitForSeconds(timePerFlash);
					this.detonationLight.enabled = true;
					yield return new WaitForSeconds(0.1f);
					num = i;
				}
				Singleton<EffectManager>.Instance.PlayExplosionEffect(base.transform.position, true, 1f, null);
				mineable.ShowDamageNumber(this.damage);
				this.spriteRenderer.enabled = false;
				UnityEngine.Object.Destroy(base.gameObject, 2f);
				mineable = null;
			}
			yield break;
		}

		// Token: 0x040012B6 RID: 4790
		public float latchDistance = 0.5f;

		// Token: 0x040012B7 RID: 4791
		public float spreadRadius = 2f;

		// Token: 0x040012B8 RID: 4792
		public float spreadTime = 1f;

		// Token: 0x040012B9 RID: 4793
		public DamageData damage;

		// Token: 0x040012BA RID: 4794
		public bool isLatched;

		// Token: 0x040012BD RID: 4797
		[SerializeField]
		private float explosiveDelay;

		// Token: 0x040012BE RID: 4798
		private Collider2D asteroidCollider;

		// Token: 0x040012BF RID: 4799
		private Vector3 targetPosition;

		// Token: 0x040012C0 RID: 4800
		private Vector2 targetPositionInAsteroid;

		// Token: 0x040012C1 RID: 4801
		private Light2D detonationLight;

		// Token: 0x040012C2 RID: 4802
		private SpriteRenderer spriteRenderer;
	}
}
