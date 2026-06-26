using System;
using Behaviour.Managers;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Behaviour.Persistables
{
	// Token: 0x020002F9 RID: 761
	public class TravelPortal : MonoBehaviour, ITooltipTitleSource, ITooltipTextSource
	{
		// Token: 0x06001BC1 RID: 7105 RVA: 0x000A87F0 File Offset: 0x000A69F0
		public string GetTooltipTitle()
		{
			return this.portalName;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000A87F8 File Offset: 0x000A69F8
		public void SetTargetPoi(MapPointOfInterest target)
		{
			this.targetPoi = target;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000A8801 File Offset: 0x000A6A01
		public string GetTooltipText()
		{
			return this.portalDesc;
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000A8809 File Offset: 0x000A6A09
		private void OnDestroy()
		{
			EffectManager instance = Singleton<EffectManager>.Instance;
			if (instance == null)
			{
				return;
			}
			instance.PlayExplosionEffect(base.transform.position, false, 5f, new Color?(ColorHelper.flashExplosionUnit));
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000A883A File Offset: 0x000A6A3A
		public void OnMouseUpAsButton()
		{
			if (this.targetPoi == null)
			{
				return;
			}
			GameplayManager.Instance.spaceShip.manualInputTimer = 0f;
			Singleton<TravelManager>.Instance.TryInitiateTravel(this.targetPoi);
		}

		// Token: 0x04001163 RID: 4451
		private MapPointOfInterest targetPoi;

		// Token: 0x04001164 RID: 4452
		public string portalName;

		// Token: 0x04001165 RID: 4453
		public string portalDesc;
	}
}
