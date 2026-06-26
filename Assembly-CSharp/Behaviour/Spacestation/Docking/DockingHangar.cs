using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Spacestation.Docking
{
	// Token: 0x020002E2 RID: 738
	public class DockingHangar : DockingOption
	{
		// Token: 0x06001AD7 RID: 6871 RVA: 0x000A55E2 File Offset: 0x000A37E2
		private void Awake()
		{
			this.spriteMask.enabled = false;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000A55F0 File Offset: 0x000A37F0
		public void ToggleMask(bool isActive)
		{
			if (this.spriteMask != null)
			{
				this.spriteMask.enabled = isActive;
				if (base.dockingSpaceship != null)
				{
					SpriteRenderer[] componentsInChildren = base.dockingSpaceship.GetComponentsInChildren<SpriteRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].maskInteraction = (isActive ? SpriteMaskInteraction.VisibleOutsideMask : SpriteMaskInteraction.None);
					}
				}
			}
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x000A564E File Offset: 0x000A384E
		protected override IEnumerator DockingProcedure(bool skipCoroutine = false)
		{
			yield return null;
			yield break;
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x000A5656 File Offset: 0x000A3856
		protected override IEnumerator UndockingProcedure()
		{
			yield return null;
			yield break;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x000A565E File Offset: 0x000A385E
		protected override void PrepareApproach()
		{
			this.dockingApproachLights.StartDockingLightsSequence();
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000A566B File Offset: 0x000A386B
		protected override void DockingProcedureQuick()
		{
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000A566D File Offset: 0x000A386D
		protected override void PrepareLanding()
		{
			this.ToggleMask(true);
			this.dockingApproachLights.StopDockingLightsSequence();
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000A5684 File Offset: 0x000A3884
		protected override void DockingShipSetup()
		{
			if (base.dockingSpaceship != null)
			{
				SpriteRenderer[] componentsInChildren = base.dockingSpaceship.GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
				}
			}
		}

		// Token: 0x040010EE RID: 4334
		private List<DockingLight> doorWarningLights = new List<DockingLight>();

		// Token: 0x040010EF RID: 4335
		[SerializeField]
		private SpriteMask spriteMask;
	}
}
