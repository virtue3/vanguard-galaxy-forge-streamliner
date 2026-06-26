using System;
using System.Collections;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Spacestation.Docking
{
	// Token: 0x020002E7 RID: 743
	public class DockingTunnel : DockingOption
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x000A5EF4 File Offset: 0x000A40F4
		// (set) Token: 0x06001B12 RID: 6930 RVA: 0x000A5EFC File Offset: 0x000A40FC
		public DockingTunnelManager dockingTunnelManager { get; private set; }

		// Token: 0x06001B13 RID: 6931 RVA: 0x000A5F05 File Offset: 0x000A4105
		protected override IEnumerator DockingProcedure(bool skipCoroutine = false)
		{
			base.StartCoroutine(this.dockingTunnelManager.ToggleDockingTunnels(true, skipCoroutine, base.dockingSpaceship.surfaceCollider));
			yield return new WaitUntil(() => this.dockingTunnelManager.tunnelsOpen);
			yield return null;
			yield break;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x000A5F1B File Offset: 0x000A411B
		protected override void DockingProcedureQuick()
		{
			base.StartCoroutine(this.dockingTunnelManager.ToggleDockingTunnels(true, true, base.dockingSpaceship.surfaceCollider));
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000A5F3C File Offset: 0x000A413C
		protected override void DockingShipSetup()
		{
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000A5F3E File Offset: 0x000A413E
		protected override void PrepareApproach()
		{
			this.dockingApproachLights.StartDockingLightsSequence();
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000A5F4B File Offset: 0x000A414B
		protected override void PrepareLanding()
		{
			this.dockingApproachLights.StopDockingLightsSequence();
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000A5F58 File Offset: 0x000A4158
		protected override IEnumerator UndockingProcedure()
		{
			base.StartCoroutine(this.dockingTunnelManager.ToggleDockingTunnels(false, false, null));
			if (base.dockingSpaceship.dockingTunnelManager)
			{
				base.dockingSpaceship.ToggleDockingTunnel(false);
				yield return new WaitUntil(() => !base.dockingSpaceship.dockingTunnelManager.tunnelsOpen);
			}
			yield return new WaitUntil(() => !this.dockingTunnelManager.tunnelsOpen);
			yield return null;
			yield break;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000A5F67 File Offset: 0x000A4167
		public override void ResetDockingOption()
		{
			base.ResetDockingOption();
			base.StartCoroutine(this.dockingTunnelManager.ToggleDockingTunnels(false, false, null));
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000A5F84 File Offset: 0x000A4184
		protected override Quaternion GetBestDockingRotation()
		{
			if (!this.bidirectional)
			{
				return base.dockingTransform.rotation;
			}
			Quaternion rotation = base.dockingTransform.rotation;
			Quaternion result = base.dockingTransform.rotation * Quaternion.Euler(0f, 0f, 180f);
			float z = base.dockingSpaceship.transform.eulerAngles.z;
			float num = Mathf.Abs(Mathf.DeltaAngle(z, rotation.eulerAngles.z));
			float num2 = Mathf.Abs(Mathf.DeltaAngle(z, result.eulerAngles.z));
			if (num > num2)
			{
				return result;
			}
			return rotation;
		}

		// Token: 0x04001107 RID: 4359
		[SerializeField]
		private bool bidirectional = true;
	}
}
