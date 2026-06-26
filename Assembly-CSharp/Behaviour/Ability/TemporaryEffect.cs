using System;
using System.Collections.Generic;
using Behaviour.Unit;
using Source.Player;
using UnityEngine;

namespace Behaviour.Ability
{
	// Token: 0x020003C4 RID: 964
	public class TemporaryEffect : MonoBehaviour
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x000D05A8 File Offset: 0x000CE7A8
		// (set) Token: 0x06002533 RID: 9523 RVA: 0x000D05B0 File Offset: 0x000CE7B0
		public Sprite icon { get; private set; }

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x000D05B9 File Offset: 0x000CE7B9
		// (set) Token: 0x06002535 RID: 9525 RVA: 0x000D05C1 File Offset: 0x000CE7C1
		public string displayName { get; private set; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002536 RID: 9526 RVA: 0x000D05CA File Offset: 0x000CE7CA
		// (set) Token: 0x06002537 RID: 9527 RVA: 0x000D05D2 File Offset: 0x000CE7D2
		public string descriptionText { get; private set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06002538 RID: 9528 RVA: 0x000D05DB File Offset: 0x000CE7DB
		// (set) Token: 0x06002539 RID: 9529 RVA: 0x000D05E3 File Offset: 0x000CE7E3
		public bool showInBuffHud { get; private set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600253A RID: 9530 RVA: 0x000D05EC File Offset: 0x000CE7EC
		// (set) Token: 0x0600253B RID: 9531 RVA: 0x000D05F4 File Offset: 0x000CE7F4
		public bool isDebuff { get; private set; }

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600253C RID: 9532 RVA: 0x000D05FD File Offset: 0x000CE7FD
		// (set) Token: 0x0600253D RID: 9533 RVA: 0x000D0605 File Offset: 0x000CE805
		public float duration { get; private set; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600253E RID: 9534 RVA: 0x000D060E File Offset: 0x000CE80E
		// (set) Token: 0x0600253F RID: 9535 RVA: 0x000D0616 File Offset: 0x000CE816
		public string abilityIdentifier { get; private set; }

		// Token: 0x06002540 RID: 9536 RVA: 0x000D061F File Offset: 0x000CE81F
		public void SetAbilityIdentifier(string id)
		{
			this.abilityIdentifier = id;
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x000D0628 File Offset: 0x000CE828
		public int stackSize
		{
			get
			{
				StackableEffect stackableEffect;
				if (base.TryGetComponent<StackableEffect>(out stackableEffect))
				{
					return stackableEffect.stackSize;
				}
				return 1;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x000D0647 File Offset: 0x000CE847
		// (set) Token: 0x06002543 RID: 9539 RVA: 0x000D064F File Offset: 0x000CE84F
		public float durationRemaining { get; set; }

		// Token: 0x06002544 RID: 9540 RVA: 0x000D0658 File Offset: 0x000CE858
		private void Start()
		{
			this.durationRemaining = this.duration;
			this.parentUnit = base.GetComponentInParent<AbstractUnit>();
			this.isPlayerEffect = (this.parentUnit != null && this.parentUnit.IsPlayer(true));
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000D0698 File Offset: 0x000CE898
		private void Update()
		{
			this.durationRemaining -= Time.deltaTime;
			if (this.durationRemaining <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (this.showInBuffHud && this.isPlayerEffect && !string.IsNullOrEmpty(this.abilityIdentifier) && GamePlayer.current != null)
			{
				GamePlayer.current.activeEffects[this.abilityIdentifier] = new ActiveEffectData
				{
					durationRemaining = this.durationRemaining,
					stackSize = this.stackSize
				};
			}
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000D0728 File Offset: 0x000CE928
		private void OnDestroy()
		{
			if (this.showInBuffHud && this.isPlayerEffect && !string.IsNullOrEmpty(this.abilityIdentifier))
			{
				GamePlayer current = GamePlayer.current;
				if (current != null)
				{
					current.activeEffects.Remove(this.abilityIdentifier);
				}
			}
			AbstractUnit componentInParent = base.GetComponentInParent<AbstractUnit>();
			if (componentInParent && !GameManager.isQuitting)
			{
				componentInParent.CalculateStats();
			}
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000D078C File Offset: 0x000CE98C
		public void AddStack(TemporaryEffect stack)
		{
			List<StackableEffect> list = new List<StackableEffect>();
			List<StackableEffect> list2 = new List<StackableEffect>();
			base.GetComponents<StackableEffect>(list);
			stack.GetComponents<StackableEffect>(list2);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].AddStack(list2[i]);
			}
			this.durationRemaining = Mathf.Max(this.durationRemaining, (stack.durationRemaining > 0f) ? stack.durationRemaining : stack.duration);
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000D0804 File Offset: 0x000CEA04
		public void CheckStackability()
		{
			foreach (TemporaryEffect temporaryEffect in base.transform.parent.GetComponentsInChildren<TemporaryEffect>())
			{
				if (temporaryEffect != this && temporaryEffect.gameObject.name == base.gameObject.name)
				{
					temporaryEffect.AddStack(this);
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000D086D File Offset: 0x000CEA6D
		public void SetDuration(float duration)
		{
			this.duration = duration;
		}

		// Token: 0x040016A4 RID: 5796
		private AbstractUnit parentUnit;

		// Token: 0x040016A5 RID: 5797
		private bool isPlayerEffect;
	}
}
