using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000069 RID: 105
	public class FlyByActions : AutoActions
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001EF84 File Offset: 0x0001D184
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001EF87 File Offset: 0x0001D187
		public FlyByActions(AbstractUnit parent) : base(parent)
		{
			this.spaceShip.targetProvider.Deactivate();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
		public override void Update(float delta)
		{
			base.Update(delta);
			if (Vector2.Distance(this.spaceShip.currentDestination, this.spaceShip.transform.position) < 2f)
			{
				base.StartExitCoroutine();
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001EFDB File Offset: 0x0001D1DB
		protected override void RemoveUnit()
		{
			UnityEngine.Object.Destroy(this.spaceShip.gameObject);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001EFED File Offset: 0x0001D1ED
		public override void OnDamageTaken(DamageData data)
		{
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001EFEF File Offset: 0x0001D1EF
		public override bool DoWeRespawn()
		{
			return false;
		}
	}
}
