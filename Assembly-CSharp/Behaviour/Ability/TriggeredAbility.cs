using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Ability;
using UnityEngine;

namespace Behaviour.Ability
{
	// Token: 0x020003C5 RID: 965
	public class TriggeredAbility : AbstractAbility
	{
		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x000D087E File Offset: 0x000CEA7E
		// (set) Token: 0x0600254C RID: 9548 RVA: 0x000D0886 File Offset: 0x000CEA86
		public AbilityTrigger trigger { get; private set; }

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x000D088F File Offset: 0x000CEA8F
		// (set) Token: 0x0600254E RID: 9550 RVA: 0x000D0897 File Offset: 0x000CEA97
		public bool stackTriggerChance { get; private set; }

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600254F RID: 9551 RVA: 0x000D08A0 File Offset: 0x000CEAA0
		// (set) Token: 0x06002550 RID: 9552 RVA: 0x000D08A8 File Offset: 0x000CEAA8
		public bool attachPayloadToTarget { get; private set; }

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x000D08B1 File Offset: 0x000CEAB1
		// (set) Token: 0x06002552 RID: 9554 RVA: 0x000D08B9 File Offset: 0x000CEAB9
		public bool triggeredBySelf { get; private set; } = true;

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x000D08C2 File Offset: 0x000CEAC2
		// (set) Token: 0x06002554 RID: 9556 RVA: 0x000D08CA File Offset: 0x000CEACA
		public bool triggeredBySubordinate { get; private set; } = true;

		// Token: 0x06002555 RID: 9557 RVA: 0x000D08D3 File Offset: 0x000CEAD3
		public float GetTriggerChance(int stackSize = -1)
		{
			if (this.stackTriggerChance)
			{
				return this.triggerChance * (float)((stackSize < 0) ? base.stackSize : stackSize);
			}
			return this.triggerChance;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000D08FC File Offset: 0x000CEAFC
		public GameObject TriggerPayload(object source, AbstractUnit triggeredBySubordinate)
		{
			if (base.isReady && SeededRandom.Global.RandomBool(this.GetTriggerChance(-1)))
			{
				bool flag = true;
				foreach (TriggeredPayload triggeredPayload in base.payload.GetComponents<TriggeredPayload>())
				{
					flag = (flag && triggeredPayload.ShouldTrigger(source));
				}
				if (flag)
				{
					Transform payloadParent = null;
					if (this.attachPayloadToTarget)
					{
						DamageData damageData = source as DamageData;
						if (damageData != null)
						{
							payloadParent = damageData.targetUnit.transform;
							goto IL_86;
						}
					}
					if (triggeredBySubordinate)
					{
						payloadParent = triggeredBySubordinate.transform;
					}
					IL_86:
					return base.TriggerPayload(null, payloadParent, source, false);
				}
			}
			return null;
		}

		// Token: 0x040016A6 RID: 5798
		[SerializeField]
		private float triggerChance;
	}
}
