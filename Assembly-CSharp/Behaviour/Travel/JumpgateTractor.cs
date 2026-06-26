using System;
using System.Collections.Generic;
using Behaviour.Item.Usable;
using Behaviour.Tractoring;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Util;
using UnityEngine;

namespace Behaviour.Travel
{
	// Token: 0x020002CA RID: 714
	public class JumpgateTractor : MonoBehaviour, ITractorParent
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x000A24BC File Offset: 0x000A06BC
		private void Start()
		{
			TractorBeam item = UnityEngine.Object.Instantiate<TractorBeam>(this.tractorBeamPrefab, base.transform);
			this.tractorBeams.Add(item);
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000A24E8 File Offset: 0x000A06E8
		private void Update()
		{
			this.scanDelay -= Time.deltaTime;
			if (this.scanDelay < 0f)
			{
				this.scanDelay = 0.5f;
				this.ScanForItems();
			}
			foreach (TractorBeam tractorBeam in this.tractorBeams)
			{
				if (tractorBeam.HasTarget())
				{
					tractorBeam.MoveTargetTowardsObject(base.transform.position, this);
				}
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000A2584 File Offset: 0x000A0784
		protected virtual bool CanTractorItem(TractorableItem tItem)
		{
			JumpGate jumpGatePoi = JumpGateManager.instance.jumpGatePoi;
			if (jumpGatePoi.canUseJumpGate)
			{
				return false;
			}
			JumpgatePassItem jumpgatePassItem;
			if (tItem.data.itemType.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem))
			{
				JumpGate jumpGate = GalaxyMapData.current.GetPointOfInterest(jumpgatePassItem.jumpgateGuid) as JumpGate;
				JumpGate jumpGate2 = ((jumpGate != null) ? jumpGate.GetTargetPOI() : null) as JumpGate;
				if (jumpgatePassItem.jumpgateGuid == jumpGatePoi.guid || jumpGatePoi.targetPoiGuid == ((jumpGate2 != null) ? jumpGate2.targetPoiGuid : null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x000A2610 File Offset: 0x000A0810
		private void ScanForItems()
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, 20f);
			for (int i = 0; i < array.Length; i++)
			{
				TractorableItem tractorableItem;
				if (array[i].TryGetComponent<TractorableItem>(out tractorableItem) && !tractorableItem.isTractored && !this.tractorBeams[0].HasTarget() && this.CanTractorItem(tractorableItem))
				{
					this.tractorBeams[0].StartTractoring(tractorableItem, 2f);
				}
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000A268D File Offset: 0x000A088D
		private void OnDestroy()
		{
			base.CancelInvoke("ScanForItems");
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000A269C File Offset: 0x000A089C
		public virtual void ItemTractored(TractorableItem item)
		{
			JumpgatePassItem jumpgatePassItem;
			if (item.data.itemType.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem))
			{
				jumpgatePassItem.UnlockJumpgate();
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSUnlockJumpGate", new object[]
				{
					GalaxyMapData.current.GetSystem(JumpGateManager.instance.jumpGatePoi.targetSystemGuid).name
				})).WithColor(ColorHelper.green50).WithCustomTime(2f).Show();
			}
		}

		// Token: 0x04001068 RID: 4200
		[SerializeField]
		private TractorBeam tractorBeamPrefab;

		// Token: 0x04001069 RID: 4201
		private List<TractorBeam> tractorBeams = new List<TractorBeam>();

		// Token: 0x0400106A RID: 4202
		private float scanDelay;
	}
}
