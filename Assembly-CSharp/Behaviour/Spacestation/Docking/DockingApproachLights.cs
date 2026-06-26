using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Spacestation.Docking
{
	// Token: 0x020002E1 RID: 737
	public class DockingApproachLights : MonoBehaviour
	{
		// Token: 0x06001AD1 RID: 6865 RVA: 0x000A54EC File Offset: 0x000A36EC
		private void Awake()
		{
			foreach (DockingLight item in base.GetComponentsInChildren<DockingLight>())
			{
				this.lights.Add(item);
			}
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000A5520 File Offset: 0x000A3720
		private void ToggleLights(bool toggle)
		{
			foreach (DockingLight dockingLight in this.lights)
			{
				dockingLight.SetState(toggle);
			}
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000A5574 File Offset: 0x000A3774
		public void StartDockingLightsSequence()
		{
			if (this.lightSequenceCoroutine != null)
			{
				base.StopCoroutine(this.lightSequenceCoroutine);
			}
			this.lightSequenceCoroutine = base.StartCoroutine(this.DockingLightsSequence());
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000A559C File Offset: 0x000A379C
		public void StopDockingLightsSequence()
		{
			if (this.lightSequenceCoroutine != null)
			{
				base.StopCoroutine(this.lightSequenceCoroutine);
				this.lightSequenceCoroutine = null;
			}
			this.ToggleLights(false);
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000A55C0 File Offset: 0x000A37C0
		private IEnumerator DockingLightsSequence()
		{
			for (;;)
			{
				for (int i = 0; i < this.lights.Count; i += 2)
				{
					this.lights[i].SetState(true);
					if (i + 1 < this.lights.Count)
					{
						this.lights[i + 1].SetState(true);
					}
					yield return new WaitForSeconds(0.5f);
				}
				for (int i = 0; i < this.lights.Count; i += 2)
				{
					this.lights[i].SetState(false);
					if (i + 1 < this.lights.Count)
					{
						this.lights[i + 1].SetState(false);
					}
					yield return new WaitForSeconds(0.5f);
				}
			}
			yield break;
		}

		// Token: 0x040010EC RID: 4332
		private List<DockingLight> lights = new List<DockingLight>();

		// Token: 0x040010ED RID: 4333
		private Coroutine lightSequenceCoroutine;
	}
}
