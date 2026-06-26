using System;
using System.Collections;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003C7 RID: 967
	public class AbilityShieldOvercharge : MonoBehaviour
	{
		// Token: 0x0600255C RID: 9564 RVA: 0x000D09D8 File Offset: 0x000CEBD8
		private void Start()
		{
			AbstractUnit componentInParent = base.GetComponentInParent<AbstractUnit>();
			if (componentInParent != null && componentInParent.shieldGeneratorModule != null)
			{
				float totalHeal = componentInParent.maxShieldHP * this.healFraction;
				base.StartCoroutine(this.HealRoutine(componentInParent, totalHeal));
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000D0A2C File Offset: 0x000CEC2C
		private IEnumerator HealRoutine(AbstractUnit parent, float totalHeal)
		{
			int ticks = Mathf.CeilToInt(this.duration / this.tickInterval);
			float healPerTick = totalHeal / (float)ticks;
			int num;
			for (int i = 0; i < ticks; i = num + 1)
			{
				parent.unitData.RepairShieldHp(healPerTick);
				yield return new WaitForSeconds(this.tickInterval);
				num = i;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x040016AC RID: 5804
		[SerializeField]
		private float healFraction = 0.5f;

		// Token: 0x040016AD RID: 5805
		[SerializeField]
		private float duration = 0.25f;

		// Token: 0x040016AE RID: 5806
		[SerializeField]
		private float tickInterval = 0.05f;
	}
}
