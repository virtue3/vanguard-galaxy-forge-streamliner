using System;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;
using Source.Item;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003CE RID: 974
	public class CrackshotDrone : TargetedPayload
	{
		// Token: 0x06002572 RID: 9586 RVA: 0x000D1068 File Offset: 0x000CF268
		private void Start()
		{
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			base.transform.SetParent(spaceShip.transform.parent);
			this.durationRemaining = this.duration;
			this.FindRandomHitLocation();
			this.CreateDamageData();
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000D10B0 File Offset: 0x000CF2B0
		private void Update()
		{
			this.durationRemaining -= Time.deltaTime;
			if (this.durationRemaining <= 0f || !this.target)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!this.latchedToAsteroid)
			{
				this.MoveToAsteroid();
				return;
			}
			if (!this.target.IsSurfaceOreDepleted())
			{
				this.ApplyDamageOverTime();
			}
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000D1117 File Offset: 0x000CF317
		public override void SetTarget(GameObject target)
		{
			this.target = target.GetComponent<Asteroid>();
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000D1128 File Offset: 0x000CF328
		private void CreateDamageData()
		{
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			this.damageData = new DamageData(spaceShip.gameObject)
			{
				power = 2f * spaceShip.GetNormalizedPower(EquipStat.MiningPower),
				type = DamageType.Kinetic,
				hitTransform = this.hitLocation.transform,
				hitCoordinates = this.hitLocation.transform.position
			};
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000D1198 File Offset: 0x000CF398
		private void ApplyDamageOverTime()
		{
			this.damageTimer += Time.deltaTime;
			if (this.damageTimer >= this.damageInterval)
			{
				this.FindRandomHitLocation();
				this.target.TakeSurfaceDamage(this.damageData);
				this.damageTimer = 0f;
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000D11E8 File Offset: 0x000CF3E8
		private void MoveToAsteroid()
		{
			Vector3 normalized = (this.target.transform.position - base.transform.position).normalized;
			float z = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			base.transform.position += normalized * this.speed * Time.deltaTime;
			if (Vector3.Distance(base.transform.position, this.target.transform.position) <= 0.1f)
			{
				this.latchedToAsteroid = true;
				base.transform.SetParent(this.target.transform);
				base.transform.position = this.target.transform.position;
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000D12DC File Offset: 0x000CF4DC
		public void SetAsteroidTarget(Asteroid asteroid)
		{
			this.target = asteroid;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000D12E8 File Offset: 0x000CF4E8
		private void FindRandomHitLocation()
		{
			Bounds bounds = this.target.surfaceCollider.bounds;
			int num = 0;
			Vector2 vector;
			bool flag;
			do
			{
				vector = new Vector2(SeededRandom.Global.RandomRange(bounds.min.x, bounds.max.x), SeededRandom.Global.RandomRange(bounds.min.y, bounds.max.y));
				flag = this.target.surfaceCollider.OverlapPoint(vector);
				num++;
			}
			while (!flag && num < 50);
			this.hitLocation = new GameObject();
			this.hitLocation.transform.parent = this.target.transform;
			this.hitLocation.transform.position = vector;
		}

		// Token: 0x040016C2 RID: 5826
		[SerializeField]
		private float duration;

		// Token: 0x040016C3 RID: 5827
		private Asteroid target;

		// Token: 0x040016C4 RID: 5828
		private float speed = 5f;

		// Token: 0x040016C5 RID: 5829
		private bool latchedToAsteroid;

		// Token: 0x040016C6 RID: 5830
		private DamageData damageData;

		// Token: 0x040016C7 RID: 5831
		private float damageInterval = 1f;

		// Token: 0x040016C8 RID: 5832
		private float damageTimer;

		// Token: 0x040016C9 RID: 5833
		private GameObject hitLocation;

		// Token: 0x040016CA RID: 5834
		private float durationRemaining;
	}
}
