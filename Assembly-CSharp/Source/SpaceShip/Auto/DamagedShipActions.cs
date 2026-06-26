using System;
using Behaviour.Unit;
using Behaviour.Weapons;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000067 RID: 103
	public class DamagedShipActions : AutoActions
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0001EE50 File Offset: 0x0001D050
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001EE53 File Offset: 0x0001D053
		public DamagedShipActions(AbstractUnit parent) : base(parent)
		{
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001EE5C File Offset: 0x0001D05C
		public override void Update(float delta)
		{
			base.Update(delta);
			if (this.parent.currentShieldHP == this.parent.maxShieldHP)
			{
				base.StartExitCoroutine();
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001EE83 File Offset: 0x0001D083
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001EE86 File Offset: 0x0001D086
		public override void OnDamageTaken(DamageData data)
		{
		}
	}
}
