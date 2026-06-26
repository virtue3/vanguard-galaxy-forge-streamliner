using System;
using System.Collections;
using Behaviour.Effects;
using Behaviour.Unit;
using Source.Util;
using UnityEngine;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003CA RID: 970
	public class RepairDrone : MonoBehaviour
	{
		// Token: 0x06002563 RID: 9571 RVA: 0x000D0BB4 File Offset: 0x000CEDB4
		public void Initialize(AbstractUnit target, float lifespan, float healAmount, float? startAngle = null)
		{
			this.target = target;
			this.lifespan = lifespan;
			this.healMultiplier = healAmount;
			Vector2 shipSize = target.GetShipSize();
			this.orbitRadius = Mathf.Max(shipSize.x, shipSize.y) * 0.55f;
			this.angle = (startAngle ?? UnityEngine.Random.Range(0f, 6.28318548f));
			this.orbitSpeed = SeededRandom.Global.RandomRange(this.orbitSpeed * 0.8f, this.orbitSpeed * 1.2f);
			this.orbitClockwise = SeededRandom.Global.RandomBool(0.5f);
			this.CreateLaserEffect();
			base.StartCoroutine(this.DroneRoutine());
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000D0C74 File Offset: 0x000CEE74
		private void CreateLaserEffect()
		{
			this.laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserEffectPrefab, Vector3.zero, Quaternion.identity);
			this.laserEffect.transform.localPosition = Vector2.zero;
			this.laserEffect.transform.localRotation = Quaternion.identity;
			this.laserEffect.Stop();
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000D0CD6 File Offset: 0x000CEED6
		private IEnumerator DroneRoutine()
		{
			float timer = 0f;
			float healTimer = this.healInterval;
			SpriteRenderer sr = base.GetComponent<SpriteRenderer>();
			float fadeDuration = 0.5f;
			float fadeTimer = 0f;
			Color c = sr.color;
			c.a = 0f;
			sr.color = c;
			while (fadeTimer < fadeDuration)
			{
				fadeTimer += Time.deltaTime;
				float t = Mathf.Clamp01(fadeTimer / fadeDuration);
				c.a = Mathf.Lerp(0f, 1f, t);
				sr.color = c;
				this.OrbitTarget();
				yield return null;
			}
			while (timer < this.lifespan && this.target != null)
			{
				timer += Time.deltaTime;
				healTimer += Time.deltaTime;
				this.OrbitTarget();
				if (healTimer >= this.healInterval)
				{
					this.HealTarget();
					healTimer = 0f;
				}
				yield return null;
			}
			fadeTimer = 0f;
			while (fadeTimer < fadeDuration)
			{
				fadeTimer += Time.deltaTime;
				float t2 = Mathf.Clamp01(fadeTimer / fadeDuration);
				c.a = Mathf.Lerp(1f, 0f, t2);
				sr.color = c;
				this.OrbitTarget();
				yield return null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000D0CE8 File Offset: 0x000CEEE8
		private void OrbitTarget()
		{
			if (this.target == null)
			{
				return;
			}
			float num = this.orbitClockwise ? 1f : -1f;
			this.angle += num * this.orbitSpeed * 0.0174532924f * Time.deltaTime;
			float x = Mathf.Sin(Time.time * 2f) * 0.1f * this.orbitRadius;
			float y = Mathf.Cos(Time.time * 2.3f) * 0.1f * this.orbitRadius;
			Vector2 vector = new Vector2(Mathf.Cos(this.angle), Mathf.Sin(this.angle)) * this.orbitRadius;
			vector += new Vector2(x, y);
			base.transform.position = new Vector3(this.target.transform.position.x + vector.x, this.target.transform.position.y + vector.y, -1f);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000D0DF8 File Offset: 0x000CEFF8
		private Vector2 GetRandomPointOnShip()
		{
			if (this.target == null)
			{
				return this.target.transform.position;
			}
			Collider2D component = this.target.GetComponent<Collider2D>();
			if (component == null)
			{
				return this.target.transform.position;
			}
			Bounds bounds = component.bounds;
			int num = 0;
			Vector2 vector;
			do
			{
				vector = new Vector2(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y));
				num++;
			}
			while (!component.OverlapPoint(vector) && num < 10);
			return vector;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000D0EB1 File Offset: 0x000CF0B1
		private void HealTarget()
		{
			base.StartCoroutine(this.PlayLaserEffect());
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000D0EC0 File Offset: 0x000CF0C0
		protected IEnumerator PlayLaserEffect()
		{
			if (this.target != null && this.target.armorModule)
			{
				this.randomTarget = new GameObject("ArmorRepairBotTrackingTarget");
				this.randomTarget.transform.parent = this.target.transform;
				this.randomTarget.transform.position = this.GetRandomPointOnShip();
				this.laserEffect.SetObjectsToTrack(base.gameObject, this.randomTarget);
				this.laserEffect.SetPower(0.01f);
				this.laserEffect.SetFrequency(60f);
				this.laserEffect.SetSize(0.02f);
				this.laserEffect.SetColor(ColorHelper.greenBadge);
				this.laserEffect.Play();
				yield return new WaitForSeconds(0.25f);
				this.laserEffect.Stop();
				float amount = this.target.maxArmorHP * this.healMultiplier;
				this.target.unitData.RepairArmorHP(amount);
				UnityEngine.Object.Destroy(this.randomTarget);
			}
			yield break;
		}

		// Token: 0x040016B0 RID: 5808
		[Header("Laser Setup")]
		[SerializeField]
		private LaserEffect laserEffectPrefab;

		// Token: 0x040016B1 RID: 5809
		private LaserEffect laserEffect;

		// Token: 0x040016B2 RID: 5810
		private GameObject randomTarget;

		// Token: 0x040016B3 RID: 5811
		private AbstractUnit target;

		// Token: 0x040016B4 RID: 5812
		private float lifespan;

		// Token: 0x040016B5 RID: 5813
		private float healInterval = 2f;

		// Token: 0x040016B6 RID: 5814
		private float healMultiplier = 0.1f;

		// Token: 0x040016B7 RID: 5815
		private float orbitRadius = 2.5f;

		// Token: 0x040016B8 RID: 5816
		private float orbitSpeed = 40f;

		// Token: 0x040016B9 RID: 5817
		private bool orbitClockwise = true;

		// Token: 0x040016BA RID: 5818
		private float angle;
	}
}
