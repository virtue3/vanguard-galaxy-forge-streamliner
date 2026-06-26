using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Spacestation
{
	// Token: 0x020002DF RID: 735
	public class WaypointPath
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000A52FE File Offset: 0x000A34FE
		// (set) Token: 0x06001AC2 RID: 6850 RVA: 0x000A5306 File Offset: 0x000A3506
		public List<Transform> points { get; private set; } = new List<Transform>();

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000A530F File Offset: 0x000A350F
		public Vector2 GetPoint(int index)
		{
			return this.points[index].position;
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x000A5327 File Offset: 0x000A3527
		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000A5334 File Offset: 0x000A3534
		public WaypointPath()
		{
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000A5347 File Offset: 0x000A3547
		public WaypointPath(List<Transform> points)
		{
			this.points = points;
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000A5361 File Offset: 0x000A3561
		public void AddWaypoint(Transform transform)
		{
			this.points.Add(transform);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000A536F File Offset: 0x000A356F
		public void ReversePoints()
		{
			this.points.Reverse();
		}
	}
}
