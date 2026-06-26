using System;
using System.Collections.Generic;
using Behaviour.Ability;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x0200027A RID: 634
	public class BuffHud : MonoBehaviour
	{
		// Token: 0x0600172A RID: 5930 RVA: 0x000924AA File Offset: 0x000906AA
		private void Awake()
		{
			BuffHud.instance = this;
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x000924B4 File Offset: 0x000906B4
		private void Update()
		{
			GameplayManager gameplayManager = GameplayManager.Instance;
			if (!((gameplayManager != null) ? gameplayManager.spaceShip : null))
			{
				return;
			}
			this.refreshTimer -= Time.deltaTime;
			if (this.refreshTimer > 0f)
			{
				return;
			}
			this.refreshTimer = 0.1f;
			this.Refresh();
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0009250B File Offset: 0x0009070B
		private void Refresh()
		{
			this.RefreshEffects();
			this.RefreshAbilities();
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0009251C File Offset: 0x0009071C
		private void RefreshEffects()
		{
			HashSet<TemporaryEffect> hashSet = new HashSet<TemporaryEffect>();
			foreach (TemporaryEffect temporaryEffect in GameplayManager.Instance.spaceShip.GetComponentsInChildren<TemporaryEffect>())
			{
				if (temporaryEffect.showInBuffHud)
				{
					hashSet.Add(temporaryEffect);
				}
			}
			foreach (TemporaryEffect temporaryEffect2 in hashSet)
			{
				if (!this.activeEffectIcons.ContainsKey(temporaryEffect2))
				{
					BuffIcon buffIcon = UnityEngine.Object.Instantiate<BuffIcon>(this.buffIconPrefab, this.buffIconscontainer);
					buffIcon.SetEffect(temporaryEffect2);
					this.activeEffectIcons[temporaryEffect2] = buffIcon;
				}
			}
			List<TemporaryEffect> list = new List<TemporaryEffect>();
			foreach (KeyValuePair<TemporaryEffect, BuffIcon> keyValuePair in this.activeEffectIcons)
			{
				if (!hashSet.Contains(keyValuePair.Key))
				{
					if (keyValuePair.Value)
					{
						UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
					}
					list.Add(keyValuePair.Key);
				}
			}
			foreach (TemporaryEffect key in list)
			{
				this.activeEffectIcons.Remove(key);
			}
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000926A0 File Offset: 0x000908A0
		private void RefreshAbilities()
		{
			HashSet<ActivatedAbility> hashSet = new HashSet<ActivatedAbility>();
			foreach (ActivatedAbility activatedAbility in GameplayManager.Instance.spaceShip.GetComponentsInChildren<ActivatedAbility>())
			{
				if (activatedAbility.showInBuffHud && (activatedAbility.IsPayloadActive() || activatedAbility.toggleActive) && !this.AbilityAlreadyTrackedAsEffect(activatedAbility))
				{
					hashSet.Add(activatedAbility);
				}
			}
			foreach (ActivatedAbility activatedAbility2 in hashSet)
			{
				if (!this.activeAbilityIcons.ContainsKey(activatedAbility2))
				{
					BuffIcon buffIcon = UnityEngine.Object.Instantiate<BuffIcon>(this.buffIconPrefab, this.buffIconscontainer);
					buffIcon.SetAbility(activatedAbility2);
					this.activeAbilityIcons[activatedAbility2] = buffIcon;
				}
			}
			List<ActivatedAbility> list = new List<ActivatedAbility>();
			foreach (KeyValuePair<ActivatedAbility, BuffIcon> keyValuePair in this.activeAbilityIcons)
			{
				if (!hashSet.Contains(keyValuePair.Key))
				{
					if (keyValuePair.Value)
					{
						UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
					}
					list.Add(keyValuePair.Key);
				}
			}
			foreach (ActivatedAbility key in list)
			{
				this.activeAbilityIcons.Remove(key);
			}
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00092840 File Offset: 0x00090A40
		private bool AbilityAlreadyTrackedAsEffect(ActivatedAbility ability)
		{
			using (Dictionary<TemporaryEffect, BuffIcon>.KeyCollection.Enumerator enumerator = this.activeEffectIcons.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.abilityIdentifier == ability.identifier)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x000928AC File Offset: 0x00090AAC
		public void Clear()
		{
			foreach (KeyValuePair<TemporaryEffect, BuffIcon> keyValuePair in this.activeEffectIcons)
			{
				if (keyValuePair.Value)
				{
					UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.activeEffectIcons.Clear();
			foreach (KeyValuePair<ActivatedAbility, BuffIcon> keyValuePair2 in this.activeAbilityIcons)
			{
				if (keyValuePair2.Value)
				{
					UnityEngine.Object.Destroy(keyValuePair2.Value.gameObject);
				}
			}
			this.activeAbilityIcons.Clear();
		}

		// Token: 0x04000E41 RID: 3649
		public static BuffHud instance;

		// Token: 0x04000E42 RID: 3650
		[SerializeField]
		private BuffIcon buffIconPrefab;

		// Token: 0x04000E43 RID: 3651
		[SerializeField]
		private RectTransform buffIconscontainer;

		// Token: 0x04000E44 RID: 3652
		private readonly Dictionary<TemporaryEffect, BuffIcon> activeEffectIcons = new Dictionary<TemporaryEffect, BuffIcon>();

		// Token: 0x04000E45 RID: 3653
		private readonly Dictionary<ActivatedAbility, BuffIcon> activeAbilityIcons = new Dictionary<ActivatedAbility, BuffIcon>();

		// Token: 0x04000E46 RID: 3654
		private float refreshTimer;

		// Token: 0x04000E47 RID: 3655
		private const float RefreshInterval = 0.1f;
	}
}
