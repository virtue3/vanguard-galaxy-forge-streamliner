using System;
using Behaviour.Managers;
using Behaviour.UI.HUD;

namespace Behaviour.Space
{
	// Token: 0x020002EC RID: 748
	public class SpaceSceneManager : BasePoiManager
	{
		// Token: 0x06001B50 RID: 6992 RVA: 0x000A6C9E File Offset: 0x000A4E9E
		public override void SpaceshipHasArrived()
		{
			base.SpaceshipHasArrived();
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000A6CA6 File Offset: 0x000A4EA6
		private void OnDestroy()
		{
			HudManager.Instance.HideSalvageHUD();
		}
	}
}
