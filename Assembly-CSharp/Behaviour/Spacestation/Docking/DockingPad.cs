using System;
using System.Collections;
using System.Linq;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Spacestation.Docking
{
	// Token: 0x020002E6 RID: 742
	public class DockingPad : DockingOption
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x000A5D79 File Offset: 0x000A3F79
		// (set) Token: 0x06001B07 RID: 6919 RVA: 0x000A5D81 File Offset: 0x000A3F81
		public WaypointPath waypointPath { get; private set; } = new WaypointPath();

		// Token: 0x06001B08 RID: 6920 RVA: 0x000A5D8C File Offset: 0x000A3F8C
		protected override void DockingProcedureQuick()
		{
			Vector3 localScale = new Vector3(0.95f, 0.95f, 1f);
			base.dockingSpaceship.transform.localScale = localScale;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000A5DC0 File Offset: 0x000A3FC0
		protected override IEnumerator DockingProcedure(bool skipCoroutine = false)
		{
			Vector3 finalScale = new Vector3(0.95f, 0.95f, 1f);
			if (this.scaleCoroutine != null)
			{
				base.StopCoroutine(this.scaleCoroutine);
			}
			this.scaleCoroutine = base.StartCoroutine(this.ScaleOverTime(base.dockingSpaceship.transform, finalScale, 2f));
			yield return this.scaleCoroutine;
			if (!base.dockingSpaceship)
			{
				yield break;
			}
			this.SetWayPointPath();
			yield break;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000A5DCF File Offset: 0x000A3FCF
		protected override void DockingShipSetup()
		{
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000A5DD1 File Offset: 0x000A3FD1
		protected override void PrepareApproach()
		{
			this.dockingApproachLights.StartDockingLightsSequence();
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000A5DDE File Offset: 0x000A3FDE
		protected override void PrepareLanding()
		{
			this.dockingApproachLights.StopDockingLightsSequence();
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000A5DEB File Offset: 0x000A3FEB
		protected override IEnumerator UndockingProcedure()
		{
			if (base.dockingSpaceship == null)
			{
				Debug.LogWarning("DockingSpaceship is null, but should not be null");
				yield return null;
			}
			yield return base.StartCoroutine(this.ScaleOverTime(base.dockingSpaceship.transform, new Vector3(1f, 1f, 1f), 1f));
			yield break;
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000A5DFA File Offset: 0x000A3FFA
		private IEnumerator ScaleOverTime(Transform target, Vector3 finalScale, float duration)
		{
			if (!target)
			{
				yield break;
			}
			Vector3 initialScale = target.localScale;
			float elapsed = 0f;
			do
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / duration);
				target.localScale = Vector3.Lerp(initialScale, finalScale, t);
				yield return null;
			}
			while (elapsed < duration && target);
			if (target)
			{
				target.localScale = finalScale;
			}
			yield break;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000A5E18 File Offset: 0x000A4018
		public void SetWayPointPath()
		{
			this.waypointPath.points.Clear();
			if (base.dockingSpaceship && base.dockingSpaceship.rampManager)
			{
				if (base.dockingSpaceship.rampManager != null)
				{
					Ramp ramp = base.dockingSpaceship.rampManager.ramps.FirstOrDefault<Ramp>();
					this.waypointPath.AddWaypoint(ramp.exitPosition);
					this.waypointPath.AddWaypoint(ramp.approachPosition);
				}
				else
				{
					this.waypointPath.AddWaypoint(base.dockingSpaceship.transform);
				}
			}
			foreach (Waypoint waypoint in base.GetComponentsInChildren<Waypoint>())
			{
				this.waypointPath.AddWaypoint(waypoint.transform);
			}
		}

		// Token: 0x04001104 RID: 4356
		private Coroutine scaleCoroutine;
	}
}
