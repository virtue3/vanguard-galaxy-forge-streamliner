using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001C6 RID: 454
	public class RampManager : MonoBehaviour
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x00072C14 File Offset: 0x00070E14
		// (set) Token: 0x06001132 RID: 4402 RVA: 0x00072C1C File Offset: 0x00070E1C
		public List<Ramp> ramps { get; private set; } = new List<Ramp>();

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x00072C25 File Offset: 0x00070E25
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x00072C2D File Offset: 0x00070E2D
		public bool rampsOpen { get; private set; }

		// Token: 0x06001135 RID: 4405 RVA: 0x00072C36 File Offset: 0x00070E36
		private void Awake()
		{
			this.SetRamps();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00072C40 File Offset: 0x00070E40
		private void SetRamps()
		{
			foreach (Ramp item in base.gameObject.GetComponentsInChildren<Ramp>())
			{
				this.ramps.Add(item);
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00072C77 File Offset: 0x00070E77
		public IEnumerator ToggleRamps(bool open)
		{
			yield return this.ToggleRampMechanism(open);
			this.rampsOpen = open;
			yield break;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00072C8D File Offset: 0x00070E8D
		private IEnumerator ToggleRampMechanism(bool open)
		{
			if (open == this.rampsOpen)
			{
				yield break;
			}
			using (List<Ramp>.Enumerator enumerator = this.ramps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ramp ramp = enumerator.Current;
					ramp.ToggleRamp(open);
					if (this.activateInSequence)
					{
						yield return new WaitWhile(() => ramp.isMoving);
					}
				}
			}
			List<Ramp>.Enumerator enumerator = default(List<Ramp>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0400095E RID: 2398
		[SerializeField]
		private bool activateInSequence;
	}
}
