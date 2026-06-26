using System;
using Behaviour.Equipment.Turret;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D7 RID: 983
	public class PayloadTurretExtraDamage : TriggeredPayload
	{
		// Token: 0x060025A2 RID: 9634 RVA: 0x000D1CF4 File Offset: 0x000CFEF4
		public override bool ShouldTrigger(object source)
		{
			AbstractTurret componentInParent = base.GetComponentInParent<AbstractTurret>();
			if (componentInParent)
			{
				DamageData damageData = source as DamageData;
				if (damageData != null && damageData.targetUnit)
				{
					return damageData.sourceTurret == componentInParent;
				}
			}
			return false;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000D1D38 File Offset: 0x000CFF38
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			if (this.damagePercentage > 0f)
			{
				DamageData copy = damageData.GetCopy(damageData.totalDamageAmount * this.damagePercentage, damageData.hitCoordinates, false);
				copy.type = this.damageType;
				copy.targetUnit = damageData.targetUnit;
				DamageOverTime damageOverTime;
				if (base.TryGetComponent<DamageOverTime>(out damageOverTime))
				{
					copy.isDamageOverTime = true;
					damageOverTime.SetupDamageTemplate(copy);
				}
				else
				{
					damageData.targetUnit.TakeDamage(copy);
				}
			}
			if (this.additionalPayload && SeededRandom.Global.RandomBool(this.additionalPayloadChance))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.additionalPayload, this.additionalPayloadParentToTarget ? damageData.targetUnit.transform : damageData.sourceUnit.transform);
				TemporaryEffect[] components = gameObject.GetComponents<TemporaryEffect>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].CheckStackability();
				}
				if (!gameObject.activeSelf)
				{
					gameObject.gameObject.SetActive(true);
				}
				AbstractUnit componentInParent = gameObject.GetComponentInParent<AbstractUnit>();
				if (componentInParent)
				{
					componentInParent.CalculateStats();
				}
			}
		}

		// Token: 0x040016E3 RID: 5859
		[SerializeField]
		private float damagePercentage;

		// Token: 0x040016E4 RID: 5860
		[SerializeField]
		private DamageType damageType;

		// Token: 0x040016E5 RID: 5861
		[SerializeField]
		private GameObject additionalPayload;

		// Token: 0x040016E6 RID: 5862
		[SerializeField]
		private float additionalPayloadChance;

		// Token: 0x040016E7 RID: 5863
		[SerializeField]
		private bool additionalPayloadParentToTarget;
	}
}
