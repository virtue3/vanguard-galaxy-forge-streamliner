using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x0200038E RID: 910
	public class TrailEffect : AbstractEffect
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x000C8702 File Offset: 0x000C6902
		// (set) Token: 0x060022CC RID: 8908 RVA: 0x000C870A File Offset: 0x000C690A
		public GameObject followObject { get; private set; }

		// Token: 0x060022CD RID: 8909 RVA: 0x000C8713 File Offset: 0x000C6913
		public void SetFollowObject(GameObject follow)
		{
			this.followObject = follow;
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000C871C File Offset: 0x000C691C
		public void SetFollowObject(GameObject follow, Vector2 localOffset)
		{
			this.followObject = follow;
			this.localOffset = localOffset;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000C872C File Offset: 0x000C692C
		protected override void Update()
		{
			base.Update();
			if (this.followObject)
			{
				Vector2 v = this.followObject.transform.position;
				if (this.localOffset != Vector2.zero)
				{
					v = this.followObject.transform.TransformPoint(this.localOffset);
				}
				base.visualEffect.SetVector2("Position", v);
				return;
			}
			if (this.playing)
			{
				this.Stop();
				base.canBeDestroyed = true;
			}
		}

		// Token: 0x040014B1 RID: 5297
		public Vector2 localOffset;
	}
}
