using System;
using Behaviour.Travel;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x0200006A RID: 106
	public class JumpGateActions : AutoActions
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0001EFF2 File Offset: 0x0001D1F2
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001EFF5 File Offset: 0x0001D1F5
		public override bool skipLeavePosition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001EFF8 File Offset: 0x0001D1F8
		public JumpGateActions(AbstractUnit parent) : base(parent)
		{
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001F004 File Offset: 0x0001D204
		private JumpAction GetJumpAction()
		{
			TravelDirection travelDirection = JumpGateManager.instance.jumpGatePoi.GetTravelDirection();
			float x = JumpGateManager.instance.jumpGate.transform.position.x;
			float z = this.parent.transform.rotation.eulerAngles.z;
			if ((travelDirection == TravelDirection.Left && (z < 90f || z > 270f) && this.parent.transform.position.x < x) || (travelDirection == TravelDirection.Right && z > 90f && z < 270f && this.parent.transform.position.x > x))
			{
				return JumpAction.Arriving;
			}
			return JumpAction.Leaving;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
		public void InitJumpAction()
		{
			this.jumpAction = this.GetJumpAction();
			if (this.jumpAction == JumpAction.Arriving)
			{
				this.jumpAction = JumpAction.Arriving;
				this.spaceShip.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
				this.spaceShip.SetEngineState(false, false);
				this.spaceShip.EnableCollision(false);
				this.spaceShip.jumpingProcedureEngaged = true;
				Debug.Log(this.parent.name + " -- Set jumpaction to: " + this.jumpAction.ToString());
			}
			JumpGateManager.instance.AddJumpingSpaceShip((SpaceShip)this.parent, this.jumpAction);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001F154 File Offset: 0x0001D354
		public override void Update(float delta)
		{
			base.Update(delta);
			if (!this.inited && JumpGateManager.instance && JumpGateManager.instance.initializedAndReady && this.parent.rigidbody)
			{
				string str = "Init JumpGateActions: ";
				AbstractUnit parent = this.parent;
				Debug.Log(str + ((parent != null) ? parent.ToString() : null));
				this.InitJumpAction();
				this.inited = true;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001F1C8 File Offset: 0x0001D3C8
		public override void OnDamageTaken(DamageData data)
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001F1CA File Offset: 0x0001D3CA
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x0400022C RID: 556
		protected JumpAction jumpAction;

		// Token: 0x0400022D RID: 557
		private bool inited;
	}
}
