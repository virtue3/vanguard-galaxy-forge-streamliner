using System;
using System.Collections.Generic;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;
using Source.Hazard;
using UnityEngine;

namespace Behaviour.Hazard
{
	// Token: 0x02000324 RID: 804
	public abstract class LocalHazard : TargetableUnit
	{
		// Token: 0x06001E32 RID: 7730 RVA: 0x000B35A6 File Offset: 0x000B17A6
		public virtual void Init(GameObject source, HazardData data, float rangeMultiplier = 1f)
		{
			this.source = (source ?? base.gameObject);
			this.data = data;
			this.rangeMultiplier = rangeMultiplier;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000B35C8 File Offset: 0x000B17C8
		protected void DealDamageToTarget(TargetableUnit unit)
		{
			Vector2 v = unit.transform.position;
			foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(base.transform.position, (unit.transform.position - base.transform.position).normalized, this.data.range * 5f))
			{
				if (raycastHit2D.transform.GetComponentInParent<AbstractUnit>() == unit)
				{
					v = raycastHit2D.point;
					break;
				}
			}
			DamageData damageData = new DamageData(base.gameObject)
			{
				type = this.data.damageType
			};
			GameObject gameObject = null;
			if (!this.trackingTargets.TryGetValue(unit, out gameObject) || !gameObject)
			{
				gameObject = new GameObject("TrackingTarget");
				gameObject.transform.parent = unit.transform;
				gameObject.transform.position = v;
				this.trackingTargets[unit] = gameObject;
			}
			damageData.hitTransform = gameObject.transform;
			damageData.hitCoordinates = gameObject.transform.position;
			float num = Vector2.Distance(unit.transform.position, damageData.hitCoordinates);
			damageData.power = this.data.damageMultiplier - this.data.damageMultiplier * this.data.maxDamageFalloffPercentage * num / this.data.range;
			unit.TakeDamage(damageData);
		}

		// Token: 0x06001E34 RID: 7732
		public abstract HazardData CreateData(int level, DamageType damageType);

		// Token: 0x06001E35 RID: 7733 RVA: 0x000B3760 File Offset: 0x000B1960
		protected void SetStandardFields(HazardData data)
		{
			data.hazard = this;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000B376C File Offset: 0x000B196C
		public static DamageType GetRandomDamageTypeForHazard(HazardName hazardName)
		{
			if (hazardName == HazardName.DamageInRadius)
			{
				return SeededRandom.Global.Choose<DamageType>(new List<DamageType>
				{
					DamageType.Cold,
					DamageType.Corrosion,
					DamageType.Heat,
					DamageType.Radiation
				});
			}
			if (hazardName == HazardName.Mine)
			{
				return SeededRandom.Global.Choose<DamageType>(new List<DamageType>
				{
					DamageType.Explosive,
					DamageType.Kinetic
				});
			}
			return DamageType.Explosive;
		}

		// Token: 0x06001E37 RID: 7735
		public abstract DamageType[] GetDamageTypes();

		// Token: 0x06001E38 RID: 7736 RVA: 0x000B37CC File Offset: 0x000B19CC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			foreach (GameObject gameObject in this.trackingTargets.Values)
			{
				if (gameObject)
				{
					UnityEngine.Object.Destroy(gameObject.gameObject);
				}
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x000B3838 File Offset: 0x000B1A38
		public static HazardName GetRandom()
		{
			return SeededRandom.Global.ChooseEnum<HazardName>(0);
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000B3845 File Offset: 0x000B1A45
		public static LocalHazard Get(string name)
		{
			return LocalHazard.allHazards[name];
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000B3854 File Offset: 0x000B1A54
		public static void LoadAll()
		{
			LocalHazard.allHazards.Clear();
			LocalHazard[] array = Resources.LoadAll<LocalHazard>("Hazards");
			for (int i = 0; i < array.Length; i++)
			{
				LocalHazard.allHazards[array[i].name] = array[i];
			}
		}

		// Token: 0x04001224 RID: 4644
		private static Dictionary<string, LocalHazard> allHazards = new Dictionary<string, LocalHazard>();

		// Token: 0x04001225 RID: 4645
		public bool attachToGameObject;

		// Token: 0x04001226 RID: 4646
		public GameObject source;

		// Token: 0x04001227 RID: 4647
		public HazardData data;

		// Token: 0x04001228 RID: 4648
		public float rangeMultiplier = 1f;

		// Token: 0x04001229 RID: 4649
		private Dictionary<TargetableUnit, GameObject> trackingTargets = new Dictionary<TargetableUnit, GameObject>();
	}
}
