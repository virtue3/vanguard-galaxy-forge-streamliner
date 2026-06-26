using System;
using System.Collections.Generic;
using Behaviour.Ability;
using Source.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000276 RID: 630
	public class AbilityHud : MonoBehaviour
	{
		// Token: 0x06001713 RID: 5907 RVA: 0x00091F99 File Offset: 0x00090199
		private void Awake()
		{
			AbilityHud.instance = this;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00091FA1 File Offset: 0x000901A1
		private void Update()
		{
			if (!this.initialized && GameplayManager.Instance && GameplayManager.Instance.spaceShip)
			{
				this.CreateAbilityButtons();
				this.initialized = true;
			}
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00091FD8 File Offset: 0x000901D8
		private ActivatedAbility GetAbility(string id)
		{
			foreach (ActivatedAbility activatedAbility in this.availableAbilities)
			{
				if (activatedAbility.identifier == id)
				{
					return activatedAbility;
				}
			}
			return null;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0009203C File Offset: 0x0009023C
		public void ResetHud(bool hardReset)
		{
			if (hardReset)
			{
				this.abilityButtons.Clear();
				base.transform.DestroyChildren();
			}
			this.CreateAbilityButtons();
			this.initialized = true;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00092064 File Offset: 0x00090264
		public bool HasAbilityButton(ActivatedAbility aa)
		{
			using (List<AbilityButton>.Enumerator enumerator = this.abilityButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ability == aa)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x000920C4 File Offset: 0x000902C4
		protected void CreateAbilityButtons()
		{
			this.availableAbilities.Clear();
			this.availableAbilities.AddRange(GameplayManager.Instance.spaceShip.GetActivatedAbilities());
			List<string> abilitySlots = GamePlayer.current.abilitySlots;
			for (int i = 0; i < abilitySlots.Count; i++)
			{
				bool flag = false;
				using (HashSet<ActivatedAbility>.Enumerator enumerator = this.availableAbilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.identifier == abilitySlots[i])
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					abilitySlots.RemoveAt(i);
					i--;
				}
			}
			while (abilitySlots.Count > 4)
			{
				abilitySlots.RemoveAt(abilitySlots.Count - 1);
			}
			foreach (string id in abilitySlots)
			{
				ActivatedAbility ability = this.GetAbility(id);
				if (ability)
				{
					this.AddAbilityButton(ability);
				}
			}
			foreach (ActivatedAbility activatedAbility in this.availableAbilities)
			{
				if (this.abilityButtons.Count < 4 && !this.HasAbilityButton(activatedAbility))
				{
					this.AddAbilityButton(activatedAbility);
					abilitySlots.Add(activatedAbility.identifier);
				}
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0009224C File Offset: 0x0009044C
		private void AddAbilityButton(ActivatedAbility aa)
		{
			if (!this.HasAbilityButton(aa))
			{
				AbilityButton abilityButton = UnityEngine.Object.Instantiate<AbilityButton>(this.abilityButton, base.transform);
				abilityButton.SetAbility(aa);
				this.abilityButtons.Add(abilityButton);
			}
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00092288 File Offset: 0x00090488
		public List<ActivatedAbility> GetSelectableAbilities()
		{
			List<ActivatedAbility> list = new List<ActivatedAbility>(this.availableAbilities);
			foreach (AbilityButton abilityButton in this.abilityButtons)
			{
				list.Remove(abilityButton.ability);
			}
			return list;
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x000922F0 File Offset: 0x000904F0
		public void HideOtherAbilitySelect(AbilityButton button)
		{
			foreach (AbilityButton abilityButton in this.abilityButtons)
			{
				if (abilityButton != button)
				{
					abilityButton.ToggleAbilitySelect(new bool?(false));
				}
			}
		}

		// Token: 0x04000E30 RID: 3632
		public const int AbilitySlotCount = 4;

		// Token: 0x04000E31 RID: 3633
		public static AbilityHud instance;

		// Token: 0x04000E32 RID: 3634
		[SerializeField]
		private AbilityButton abilityButton;

		// Token: 0x04000E33 RID: 3635
		private bool initialized;

		// Token: 0x04000E34 RID: 3636
		private List<AbilityButton> abilityButtons = new List<AbilityButton>();

		// Token: 0x04000E35 RID: 3637
		private HashSet<ActivatedAbility> availableAbilities = new HashSet<ActivatedAbility>();
	}
}
