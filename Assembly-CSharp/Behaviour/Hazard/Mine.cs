using System;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Salvage;
using Behaviour.UI;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Galaxy;
using Source.Hazard;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Hazard
{
	// Token: 0x02000326 RID: 806
	public class Mine : LocalHazard, IDamageable
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x000B38C3 File Offset: 0x000B1AC3
		public override bool damagableByAll
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000B38C6 File Offset: 0x000B1AC6
		public new MineHazardData data
		{
			get
			{
				return (MineHazardData)this.data;
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001E40 RID: 7744 RVA: 0x000B38D3 File Offset: 0x000B1AD3
		public override string targetName
		{
			get
			{
				return "@Mine";
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000B38DA File Offset: 0x000B1ADA
		protected override void Start()
		{
			base.Start();
			this.light.enabled = false;
			base.transform.Z(ZIndex.Deployable);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000B38FC File Offset: 0x000B1AFC
		protected override void Update()
		{
			if (Singleton<TravelManager>.Instance.TravelActive())
			{
				return;
			}
			this.checkTargetTimer += Time.deltaTime;
			if (this.checkTargetInterval > this.checkTargetTimer)
			{
				this.CheckForTargets();
				this.checkTargetTimer = 0f;
			}
			if (this.currentTarget && this.data.homingSpeed > 0f)
			{
				this.MoveToTarget();
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000B396C File Offset: 0x000B1B6C
		private void OnMouseUpAsButton()
		{
			if (!UIHelper.clickTargetingAvailable)
			{
				return;
			}
			if (this.data.faction.IsEnemy(Faction.player))
			{
				GameplayManager.Instance.spaceShip.SetManualTarget(this);
			}
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000B39A0 File Offset: 0x000B1BA0
		private void CheckForTargets()
		{
			if (this.currentTarget && Vector2.Distance(this.currentTarget.transform.position, base.transform.position) > this.data.acquiringRange)
			{
				this.currentTarget = null;
			}
			else
			{
				foreach (TargetableUnit targetableUnit in PhysicsInteraction.GetUnitsWithinRange(base.transform.position, this.data.acquiringRange, this))
				{
					AbstractUnit abstractUnit = targetableUnit as AbstractUnit;
					if (abstractUnit != null && abstractUnit.IsEnemy(this.data.faction))
					{
						this.currentTarget = abstractUnit;
					}
				}
			}
			if (this.currentTarget)
			{
				this.lightTimer += Time.deltaTime;
				float num = this.lightFlickerSpeed * Vector2.Distance(this.currentTarget.transform.position, base.transform.position) / this.data.acquiringRange;
				if (!this.light.enabled && this.lightTimer > num)
				{
					this.light.enabled = true;
					this.lightTimer = 0f;
				}
				else if (this.light.enabled && this.lightTimer > this.lightDuration)
				{
					this.light.enabled = false;
					this.lightTimer = 0f;
				}
				if (Vector2.Distance(this.currentTarget.transform.position, base.transform.position) < this.data.range)
				{
					this.Detonate();
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
			}
			else
			{
				this.light.enabled = false;
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000B3B8C File Offset: 0x000B1D8C
		private void MoveToTarget()
		{
			this.currentSpeed += this.data.homingSpeed * Time.deltaTime;
			base.transform.position = Vector2.MoveTowards(base.transform.position, this.currentTarget.transform.position, this.currentSpeed);
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000B3BF8 File Offset: 0x000B1DF8
		private void Detonate()
		{
			if (base.isDestroyed)
			{
				return;
			}
			base.isDestroyed = true;
			Asteroid componentInParent = base.GetComponentInParent<Asteroid>();
			foreach (TargetableUnit targetableUnit in PhysicsInteraction.GetUnitsWithinRange(base.transform.position, this.data.range, this))
			{
				if (!(targetableUnit is Asteroid) && !(targetableUnit is SalvageContainer))
				{
					base.DealDamageToTarget(targetableUnit);
				}
			}
			Singleton<EffectManager>.Instance.PlayExplosionEffect(base.transform.position, false, 1f, null);
			if (this.data.persistable != null)
			{
				BasePoiManager.current.poi.RemovePersistable(this.data.persistable);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			if (componentInParent)
			{
				((MineHazardData)componentInParent.asteroidData.hazardData).amount--;
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000B3D10 File Offset: 0x000B1F10
		public override HazardData CreateData(int level, DamageType damageType)
		{
			float minePower = Mine.GetMinePower(level);
			MineHazardData mineHazardData = new MineHazardData();
			mineHazardData.acquiringRange = (float)SeededRandom.Global.RandomRange(3, 5);
			mineHazardData.homingSpeed = SeededRandom.Global.RandomRange(0.05f, 0.1f);
			mineHazardData.faction = Faction.marauders;
			mineHazardData.range = 1f;
			mineHazardData.damageMultiplier = SeededRandom.Global.RandomRange(minePower / 1.3f, minePower * 1.3f);
			mineHazardData.maxDamageFalloffPercentage = SeededRandom.Global.RandomRange(0.1f, 0.2f);
			mineHazardData.damageType = damageType;
			mineHazardData.amount = SeededRandom.Global.RandomRange(2, level / 5);
			base.SetStandardFields(mineHazardData);
			return mineHazardData;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000B3DC8 File Offset: 0x000B1FC8
		public override DamageType[] GetDamageTypes()
		{
			DamageType[] array = new DamageType[2];
			array[0] = DamageType.Explosive;
			return array;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000B3DD4 File Offset: 0x000B1FD4
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return true;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000B3DD7 File Offset: 0x000B1FD7
		public override void TakeDamage(DamageData data)
		{
			this.Detonate();
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000B3DDF File Offset: 0x000B1FDF
		public bool IsEnemy(AbstractUnit unit)
		{
			return unit.faction.IsEnemy(this.data.faction);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000B3DF7 File Offset: 0x000B1FF7
		public static float GetMinePower(int level)
		{
			return 270f * GameMath.DamageMultiplier((float)level);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000B3E19 File Offset: 0x000B2019
		bool IDamageable.enabled => base.enabled;

		// Token: 0x0400122D RID: 4653
		[SerializeField]
		private Light2D light;

		// Token: 0x0400122E RID: 4654
		[SerializeField]
		private float lightFlickerSpeed;

		// Token: 0x0400122F RID: 4655
		[SerializeField]
		private float lightDuration;

		// Token: 0x04001230 RID: 4656
		private float lightTimer;

		// Token: 0x04001231 RID: 4657
		private float checkTargetInterval = 0.5f;

		// Token: 0x04001232 RID: 4658
		private float checkTargetTimer;

		// Token: 0x04001233 RID: 4659
		private float currentSpeed;

		// Token: 0x04001234 RID: 4660
		private AbstractUnit currentTarget;
	}
}
