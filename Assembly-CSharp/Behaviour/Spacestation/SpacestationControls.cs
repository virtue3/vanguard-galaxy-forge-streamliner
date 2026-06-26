using System;
using Behaviour.Gameplay;
using Behaviour.UI;
using Behaviour.UI.Side_Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.Spacestation
{
	// Token: 0x020002E0 RID: 736
	public class SpacestationControls : MonoBehaviour
	{
		// Token: 0x06001AC9 RID: 6857 RVA: 0x000A537C File Offset: 0x000A357C
		private void Awake()
		{
			SpacestationControls.instance = this;
			this.controls = new PlayerControls();
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x000A5390 File Offset: 0x000A3590
		private void Update()
		{
			if (this.inputDelay > 0)
			{
				this.inputDelay--;
				if (this.inputDelay == 0)
				{
					PlayerControls playerControls = this.controls;
					if (playerControls != null)
					{
						playerControls.Enable();
					}
				}
			}
			if (GlobalControls.mousePressed)
			{
				foreach (RaycastResult raycastResult in UIHelper.GetMouseOverUIElements())
				{
					if (raycastResult.gameObject.GetComponentInParent<SideTabAutopilot>())
					{
						return;
					}
				}
				IdleManager.DelayIdleActivities(15f);
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x000A5434 File Offset: 0x000A3634
		private void OnEnable()
		{
			PlayerControls playerControls = this.controls;
			if (playerControls == null)
			{
				return;
			}
			playerControls.Enable();
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000A5446 File Offset: 0x000A3646
		private void OnDisable()
		{
			PlayerControls playerControls = this.controls;
			if (playerControls == null)
			{
				return;
			}
			playerControls.Disable();
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000A5458 File Offset: 0x000A3658
		public static void Enable()
		{
			if (SpacestationControls.instance)
			{
				SpacestationControls.instance.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000A5476 File Offset: 0x000A3676
		public static void Disable()
		{
			if (SpacestationControls.instance)
			{
				SpacestationControls.instance.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000A5494 File Offset: 0x000A3694
		public static void Delay()
		{
			if (SpacestationControls.instance && SpacestationControls.instance.controls != null)
			{
				SpacestationControls.instance.inputDelay = Mathf.Max(5, SpacestationControls.instance.inputDelay + 1);
				SpacestationControls.instance.controls.Disable();
			}
		}

		// Token: 0x040010E9 RID: 4329
		private static SpacestationControls instance;

		// Token: 0x040010EA RID: 4330
		private int inputDelay;

		// Token: 0x040010EB RID: 4331
		private PlayerControls controls;
	}
}
