using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Managers;
using Source.Galaxy.POI;
using Source.Player;

namespace Behaviour.Mining
{
	// Token: 0x020002FC RID: 764
	public class MiningManager : BasePoiManager
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000AA35D File Offset: 0x000A855D
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x000AA364 File Offset: 0x000A8564
		public static MiningManager Instance { get; private set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000AA36C File Offset: 0x000A856C
		public IEnumerable<Asteroid> asteroids
		{
			get
			{
				return base.GetComponentsInChildren<Asteroid>();
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000AA374 File Offset: 0x000A8574
		protected override void Awake()
		{
			base.Awake();
			MiningManager.Instance = this;
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000AA382 File Offset: 0x000A8582
		protected override IEnumerator InitializePoi()
		{
			yield return base.InitializePoi();
			this.miningPoi = (Source.Galaxy.POI.Mining)base.poi;
			yield break;
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000AA391 File Offset: 0x000A8591
		public override void SpaceshipHasArrived()
		{
			base.SpaceshipHasArrived();
			GamePlayer.current.lastVisitedMiningPOI = this.miningPoi;
		}

		// Token: 0x04001182 RID: 4482
		private Source.Galaxy.POI.Mining miningPoi;
	}
}
