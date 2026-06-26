using System;
using System.Collections.Generic;
using Behaviour.Tractoring;
using UnityEngine;

namespace Behaviour.Persistables
{
	// Token: 0x020002F3 RID: 755
	public class EscortDestination : MonoBehaviour, ITractorParent
	{
		// Token: 0x06001B92 RID: 7058 RVA: 0x000A7E90 File Offset: 0x000A6090
		private void Start()
		{
			TractorBeam item = UnityEngine.Object.Instantiate<TractorBeam>(this.tractorBeamPrefab, base.transform);
			this.tractorBeams.Add(item);
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 1f);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000A7EF0 File Offset: 0x000A60F0
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

		// Token: 0x06001B94 RID: 7060 RVA: 0x000A7F8C File Offset: 0x000A618C
		protected virtual bool CanTractorItem(TractorableItem tItem)
		{
			return tItem.data.itemType.displayName == "@EscortMissionItem0";
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000A7FB0 File Offset: 0x000A61B0
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

		// Token: 0x06001B96 RID: 7062 RVA: 0x000A802D File Offset: 0x000A622D
		private void OnDestroy()
		{
			base.CancelInvoke("ScanForItems");
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000A803A File Offset: 0x000A623A
		public virtual void ItemTractored(TractorableItem item)
		{
		}

		// Token: 0x0400114D RID: 4429
		[SerializeField]
		private TractorBeam tractorBeamPrefab;

		// Token: 0x0400114E RID: 4430
		private List<TractorBeam> tractorBeams = new List<TractorBeam>();

		// Token: 0x0400114F RID: 4431
		private float scanDelay;
	}
}
