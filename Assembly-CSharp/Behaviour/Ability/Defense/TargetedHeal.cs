using System;
using System.Collections;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003CD RID: 973
	public class TargetedHeal : TargetedPayload
	{
		// Token: 0x0600256F RID: 9583 RVA: 0x000D0FD4 File Offset: 0x000CF1D4
		public override void SetTarget(GameObject targetGO)
		{
			this.target = targetGO.GetComponent<AbstractUnit>();
			if (this.target != null)
			{
				float totalHeal = this.target.maxHullHP * this.healFraction;
				base.StartCoroutine(this.HealRoutine(this.target, totalHeal));
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000D1022 File Offset: 0x000CF222
		private IEnumerator HealRoutine(AbstractUnit target, float totalHeal)
		{
			int ticks = Mathf.CeilToInt(this.duration / this.tickInterval);
			float healPerTick = totalHeal / (float)ticks;
			SpriteRenderer[] renderers = target.GetComponentsInChildren<SpriteRenderer>();
			Color[] originalColors = new Color[renderers.Length];
			for (int j = 0; j < renderers.Length; j++)
			{
				originalColors[j] = renderers[j].color;
			}
			int num;
			for (int i = 0; i < ticks; i = num + 1)
			{
				if (target == null)
				{
					yield break;
				}
				target.unitData.RepairHullHp(healPerTick);
				for (int k = 0; k < renderers.Length; k++)
				{
					renderers[k].color = Color.Lerp(originalColors[k], Color.green, 0.6f);
				}
				yield return new WaitForSeconds(0.05f);
				for (int l = 0; l < renderers.Length; l++)
				{
					renderers[l].color = originalColors[l];
				}
				yield return new WaitForSeconds(this.tickInterval - 0.05f);
				num = i;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x040016BE RID: 5822
		[SerializeField]
		private float healFraction = 0.75f;

		// Token: 0x040016BF RID: 5823
		[SerializeField]
		private float duration = 0.5f;

		// Token: 0x040016C0 RID: 5824
		[SerializeField]
		private float tickInterval = 0.1f;

		// Token: 0x040016C1 RID: 5825
		private AbstractUnit target;
	}
}
