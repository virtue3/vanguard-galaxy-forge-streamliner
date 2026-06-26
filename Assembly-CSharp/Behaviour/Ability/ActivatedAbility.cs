using System;
using UnityEngine;

namespace Behaviour.Ability
{
	// Token: 0x020003C1 RID: 961
	public class ActivatedAbility : AbstractAbility
	{
		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x000D048D File Offset: 0x000CE68D
		protected override bool persistCooldown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x000D0490 File Offset: 0x000CE690
		// (set) Token: 0x06002520 RID: 9504 RVA: 0x000D0498 File Offset: 0x000CE698
		public Sprite icon { get; private set; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x000D04A1 File Offset: 0x000CE6A1
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x000D04A9 File Offset: 0x000CE6A9
		public string displayName { get; private set; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x000D04B2 File Offset: 0x000CE6B2
		// (set) Token: 0x06002524 RID: 9508 RVA: 0x000D04BA File Offset: 0x000CE6BA
		public bool showInBuffHud { get; private set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x000D04C3 File Offset: 0x000CE6C3
		// (set) Token: 0x06002526 RID: 9510 RVA: 0x000D04CB File Offset: 0x000CE6CB
		public AbilityTargetType targetType { get; private set; }

		// Token: 0x06002527 RID: 9511 RVA: 0x000D04D4 File Offset: 0x000CE6D4
		public GameObject TriggerPayload(GameObject target)
		{
			GameObject gameObject = base.TriggerPayload(null, null, null, false);
			if (gameObject == null)
			{
				return null;
			}
			TargetedPayload[] componentsInChildren = gameObject.GetComponentsInChildren<TargetedPayload>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetTarget(target);
			}
			return gameObject;
		}

		// Token: 0x04001694 RID: 5780
		public static bool targetingActive;

		// Token: 0x04001697 RID: 5783
		public string descriptionText;
	}
}
