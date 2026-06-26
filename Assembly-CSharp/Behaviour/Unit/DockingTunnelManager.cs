using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001C4 RID: 452
	public class DockingTunnelManager : MonoBehaviour
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00072A9E File Offset: 0x00070C9E
		// (set) Token: 0x06001122 RID: 4386 RVA: 0x00072AA6 File Offset: 0x00070CA6
		public List<DockingTunnel> dockingTunnels { get; private set; } = new List<DockingTunnel>();

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x00072AAF File Offset: 0x00070CAF
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x00072AB7 File Offset: 0x00070CB7
		public bool tunnelsOpen { get; private set; }

		// Token: 0x06001125 RID: 4389 RVA: 0x00072AC0 File Offset: 0x00070CC0
		private void Awake()
		{
			this.SetTunnels();
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00072AC8 File Offset: 0x00070CC8
		private void SetTunnels()
		{
			foreach (DockingTunnel item in base.gameObject.GetComponentsInChildren<DockingTunnel>())
			{
				this.dockingTunnels.Add(item);
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00072AFF File Offset: 0x00070CFF
		public IEnumerator ToggleDockingTunnels(bool open, bool instant = false, Collider2D collider = null)
		{
			yield return this.ToggleDockingTunnelMechanism(open, instant, collider);
			this.tunnelsOpen = open;
			bool tunnelsOpen = this.tunnelsOpen;
			yield break;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00072B23 File Offset: 0x00070D23
		private IEnumerator ToggleDockingTunnelMechanism(bool open, bool instant = false, Collider2D collider = null)
		{
			using (List<DockingTunnel>.Enumerator enumerator = this.dockingTunnels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DockingTunnel dockingTunnel = enumerator.Current;
					dockingTunnel.ToggleDockingTunnel(open, instant, collider);
					if (this.activateInSequence)
					{
						yield return new WaitWhile(() => dockingTunnel.isMoving);
					}
				}
			}
			List<DockingTunnel>.Enumerator enumerator = default(List<DockingTunnel>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04000952 RID: 2386
		[SerializeField]
		private bool activateInSequence;
	}
}
