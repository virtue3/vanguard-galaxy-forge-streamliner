using System;
using System.Collections;
using Behaviour.Managers;
using Behaviour.UI.HUD;
using Source.Galaxy;
using Source.Simulation.World.POI;

namespace Behaviour.Combat
{
	// Token: 0x020003AB RID: 939
	public class CombatManager : BasePoiManager
	{
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x000CC42E File Offset: 0x000CA62E
		protected override float securityPatrolMultiplier
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000CC435 File Offset: 0x000CA635
		public override bool xRestrictionOnSpaceship
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000CC438 File Offset: 0x000CA638
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000CC440 File Offset: 0x000CA640
		protected override IEnumerator InitializePoi()
		{
			yield return base.InitializePoi();
			yield return null;
			yield break;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000CC44F File Offset: 0x000CA64F
		public override void SpaceshipHasArrived()
		{
			base.SpaceshipHasArrived();
			MapPointOfInterest current = MapPointOfInterest.current;
			if (((current != null) ? current.storyteller : null) is IndustrialOutpost)
			{
				HudManager.Instance.ToggleIndustryOps(true);
			}
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000CC47A File Offset: 0x000CA67A
		protected override IEnumerator InitializationComplete()
		{
			yield return base.InitializationComplete();
			yield break;
		}
	}
}
