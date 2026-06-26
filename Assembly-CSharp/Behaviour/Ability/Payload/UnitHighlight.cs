using System;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003DC RID: 988
	public class UnitHighlight : MonoBehaviour
	{
		// Token: 0x060025B4 RID: 9652 RVA: 0x000D2301 File Offset: 0x000D0501
		private void Start()
		{
			this.unit = base.GetComponentInParent<AbstractUnit>();
			this.unit.spriteRenderer.color = this.highlightColor;
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000D2325 File Offset: 0x000D0525
		private void OnDestroy()
		{
			if (this.unit != null)
			{
				this.unit.spriteRenderer.color = this.highlightColor;
			}
		}

		// Token: 0x040016F0 RID: 5872
		[SerializeField]
		private Color highlightColor = Color.blue;

		// Token: 0x040016F1 RID: 5873
		private AbstractUnit unit;
	}
}
