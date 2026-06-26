using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Effects;
using Behaviour.Item.Usable;
using Behaviour.Weapons;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit.Parts
{
	// Token: 0x020001CB RID: 459
	public class ShieldPylonGenerator : MonoBehaviour
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x000734A8 File Offset: 0x000716A8
		// (set) Token: 0x0600115E RID: 4446 RVA: 0x000734B0 File Offset: 0x000716B0
		public float charge { get; private set; }

		// Token: 0x0600115F RID: 4447 RVA: 0x000734BC File Offset: 0x000716BC
		private void Start()
		{
			this.turretPart = base.GetComponent<DefensiveTurret>();
			ShieldPylonItem component = this.turretPart.defensiveTurretData.turretItem.GetComponent<ShieldPylonItem>();
			this.CreateLaserEffect();
			this.charge = component.charge;
			base.InvokeRepeating("ShieldPulse", 0.5f, 0.5f);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00073512 File Offset: 0x00071712
		private void OnEnable()
		{
			LaserEffect laserEffect = this.laserEffect;
			if (laserEffect == null)
			{
				return;
			}
			laserEffect.visualEffect.Reinit();
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0007352C File Offset: 0x0007172C
		private void CreateLaserEffect()
		{
			this.laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserPrefab, Vector3.zero, Quaternion.identity, base.transform);
			this.laserEffect.transform.localPosition = Vector2.zero;
			this.laserEffect.transform.localRotation = Quaternion.identity;
			this.laserEffect.Stop();
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00073594 File Offset: 0x00071794
		private IEnumerator PlayLaserEffect(AbstractUnit target)
		{
			this.laserEffect.SetObjectsToTrack(base.gameObject, target.gameObject);
			this.laserEffect.Play();
			yield return new WaitForSeconds(0.6f);
			this.laserEffect.Stop();
			yield break;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x000735AC File Offset: 0x000717AC
		private void ShieldPulse()
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, 20f);
			List<AbstractUnit> list = new List<AbstractUnit>();
			Collider2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				AbstractUnit abstractUnit;
				if (array2[i].gameObject.TryGetComponent<AbstractUnit>(out abstractUnit) && abstractUnit.shieldGeneratorModule && abstractUnit.currentShieldHP < abstractUnit.maxShieldHP)
				{
					AbstractUnit abstractUnit2 = abstractUnit;
					DefensiveTurret defensiveTurret = this.turretPart;
					if (!abstractUnit2.IsEnemy((defensiveTurret != null) ? defensiveTurret.faction : null))
					{
						list.Add(abstractUnit);
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			SeededRandom.Global.Shuffle<AbstractUnit>(list);
			AbstractUnit abstractUnit3 = list[0];
			int itemLevel = this.turretPart.defensiveTurretData.turretItem.itemLevel;
			float amount = 5f * GameMath.DamageMultiplier((float)itemLevel);
			abstractUnit3.unitData.RepairShieldHp(amount);
			list[0].shieldGeneratorModule.StartRechargeImmediate();
			base.StartCoroutine(this.PlayLaserEffect(abstractUnit3));
			this.charge -= 0.012f;
			this.turretPart.UpdateAmmoData();
			if (this.charge <= 0f)
			{
				this.turretPart.TakeDamage(new DamageData(base.gameObject)
				{
					damageAmount = 1E+08f,
					type = DamageType.Kinetic,
					hitTransform = base.transform
				});
			}
		}

		// Token: 0x04000981 RID: 2433
		public const float RegenPerTick = 5f;

		// Token: 0x04000982 RID: 2434
		public const float ChargePerTick = 0.012f;

		// Token: 0x04000983 RID: 2435
		public const float TickDelay = 0.5f;

		// Token: 0x04000984 RID: 2436
		public const float ScannerRange = 20f;

		// Token: 0x04000985 RID: 2437
		[SerializeField]
		private LaserEffect laserPrefab;

		// Token: 0x04000986 RID: 2438
		private LaserEffect laserEffect;

		// Token: 0x04000988 RID: 2440
		private DefensiveTurret turretPart;
	}
}
