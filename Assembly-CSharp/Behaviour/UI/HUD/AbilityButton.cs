using System;
using System.Collections;
using Behaviour.Ability;
using Behaviour.Ability.Payload;
using Behaviour.Mining;
using Behaviour.Salvage;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000275 RID: 629
	public class AbilityButton : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x000919A3 File Offset: 0x0008FBA3
		// (set) Token: 0x06001707 RID: 5895 RVA: 0x000919AB File Offset: 0x0008FBAB
		public ActivatedAbility ability { get; private set; }

		// Token: 0x06001708 RID: 5896 RVA: 0x000919B4 File Offset: 0x0008FBB4
		private void Awake()
		{
			this.parent = base.GetComponentInParent<AbilityHud>();
			this.tooltip = base.GetComponent<TooltipSource>();
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000919CE File Offset: 0x0008FBCE
		public void SetAbility(ActivatedAbility ability)
		{
			this.ability = ability;
			this.image.sprite = this.ability.icon;
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x000919F0 File Offset: 0x0008FBF0
		public void ToggleAbilitySelect(bool? toggle = null)
		{
			bool flag = toggle ?? (!this.abilitySelectParent.gameObject.activeSelf);
			if (flag && this.ability.isReady)
			{
				this.parent.HideOtherAbilitySelect(this);
				this.abilitySelectParent.DestroyActiveChildren();
				int num = 2;
				foreach (ActivatedAbility ability in this.parent.GetSelectableAbilities())
				{
					AbilitySelectable abilitySelectable = UnityEngine.Object.Instantiate<AbilitySelectable>(this.abilitySelectIcon, this.abilitySelectParent);
					abilitySelectable.SetAbility(ability);
					((RectTransform)abilitySelectable.transform).anchoredPosition = new Vector2(0f, (float)num);
					num += 42;
					abilitySelectable.gameObject.SetActive(true);
				}
				this.abilitySelectParent.sizeDelta = new Vector2(this.abilitySelectParent.sizeDelta.x, (float)num);
			}
			this.abilitySelectParent.gameObject.SetActive(flag);
			this.tooltip.enabled = !flag;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00091B20 File Offset: 0x0008FD20
		public void NewAbilitySelected(ActivatedAbility newAbility)
		{
			this.ToggleAbilitySelect(new bool?(false));
			if (!this.ability.isReady)
			{
				return;
			}
			for (int i = 0; i < GamePlayer.current.abilitySlots.Count; i++)
			{
				if (GamePlayer.current.abilitySlots[i] == this.ability.identifier)
				{
					GamePlayer.current.abilitySlots[i] = newAbility.identifier;
					break;
				}
			}
			this.SetAbility(newAbility);
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00091BA4 File Offset: 0x0008FDA4
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.2f;
				UITooltip.Refresh(this.tooltip);
				if (!this.ability.toggleActive && !this.ability.isReady)
				{
					this.fillImage.gameObject.SetActive(true);
					this.fillImage.fillAmount = this.ability.GetCooldownProgress();
				}
				if (this.targetingAbility)
				{
					this.borderImage.color = ColorHelper.purpleBlueish;
					return;
				}
				if (this.ability.toggleActive)
				{
					this.borderImage.color = ColorHelper.orange75;
					return;
				}
				if (this.ability.isReady)
				{
					this.fillImage.gameObject.SetActive(false);
					this.borderImage.color = ColorHelper.greenish;
					return;
				}
				if (this.ability.IsPayloadActive())
				{
					this.borderImage.color = ColorHelper.discBlueLight2;
					return;
				}
				this.borderImage.color = ColorHelper.reddish;
			}
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00091CC0 File Offset: 0x0008FEC0
		public void ActivateAbility()
		{
			this.updateTimer = 0f;
			if (!this.ability.isReady)
			{
				Debug.Log("Not available, sorry: " + this.ability.name);
				return;
			}
			if (this.ability.targetType != AbilityTargetType.None)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.SelectTargetAndActivate());
				return;
			}
			Debug.Log("Activate ability: " + this.ability.name);
			this.ability.TriggerPayload(null, null, null, false);
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00091D4B File Offset: 0x0008FF4B
		private IEnumerator SelectTargetAndActivate()
		{
			ActivatedAbility.targetingActive = true;
			this.targetingAbility = true;
			this.borderImage.color = ColorHelper.purpleBlueish;
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@AbilitySelectTarget", Array.Empty<object>())).WithColor(Color.white).WithCustomTime(999f).Show();
			Camera gameCamera = Camera.main.GetComponent<CameraMovement>().gameCamera;
			GameObject target = null;
			while (!Input.GetMouseButtonDown(0))
			{
				yield return null;
			}
			Collider2D collider2D = Physics2D.OverlapPoint(gameCamera.ScreenToWorldPoint(Input.mousePosition));
			if (collider2D != null)
			{
				if (this.ability.targetType == AbilityTargetType.Asteroid)
				{
					Asteroid componentInParent = collider2D.GetComponentInParent<Asteroid>();
					if (componentInParent != null)
					{
						target = componentInParent.gameObject;
					}
				}
				else if (this.ability.targetType == AbilityTargetType.Salvage)
				{
					SalvageContainer componentInParent2 = collider2D.GetComponentInParent<SalvageContainer>();
					if (componentInParent2 != null)
					{
						SalvageCargoLootboxBot salvageCargoLootboxBot;
						SalvageCreditDrone salvageCreditDrone;
						if (this.ability.payload.TryGetComponent<SalvageCargoLootboxBot>(out salvageCargoLootboxBot))
						{
							if (componentInParent2.data.lootboxExtracted == 0)
							{
								target = componentInParent2.gameObject;
							}
						}
						else if (this.ability.payload.TryGetComponent<SalvageCreditDrone>(out salvageCreditDrone) && componentInParent2.data.creditsExtracted == 0)
						{
							target = componentInParent2.gameObject;
						}
					}
				}
				else if (this.ability.targetType == AbilityTargetType.FriendlyShip)
				{
					AbstractUnit componentInParent3 = collider2D.GetComponentInParent<AbstractUnit>();
					if (componentInParent3 != null && !GameplayManager.Instance.spaceShip.IsEnemy(componentInParent3))
					{
						target = componentInParent3.gameObject;
					}
				}
			}
			if (target)
			{
				this.ability.TriggerPayload(target);
				Singleton<NotificationManager>.Instance.DestroyNotification();
			}
			else
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@AbilityInvalidTarget", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
			}
			yield return new WaitForSeconds(0.1f);
			this.targetingAbility = false;
			ActivatedAbility.targetingActive = false;
			yield break;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00091D5C File Offset: 0x0008FF5C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.ability.displayName, 14, 8f);
			if (this.ability.targetType != AbilityTargetType.None)
			{
				tooltip.AddTextLine(Translation.Translate("@AbilityTargetType", new object[]
				{
					"@AbilityTargetType" + this.ability.targetType.ToString()
				}), 12, 8f);
			}
			if (this.ability.toggleActive)
			{
				tooltip.AddTextLine("@AbilityToggleActive", 12, 8f).Text.color = ColorHelper.orange75;
			}
			else if (this.ability.isReady)
			{
				tooltip.AddTextLine(Translation.Translate("@AbilityReady", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
			}
			else if (this.ability.IsPayloadActive())
			{
				tooltip.AddTextLine("@AbilityActive", 12, 8f).Text.color = ColorHelper.energyGrayGreen;
			}
			else if (this.ability.cooldownRemaining > 0f)
			{
				tooltip.AddTextLine("@AbilityCooldown", 12, 8f).Text.color = ColorHelper.reddish;
				tooltip.AddTextLine(GameMath.FormatTime((int)this.ability.cooldownRemaining, false), 12, 8f).Text.color = ColorHelper.reddish;
			}
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(this.ability.descriptionText, 12, 8f);
			if (this.ability.isReady && this.parent.GetSelectableAbilities().Count > 0)
			{
				tooltip.AddTextLine("", 12, 8f);
				tooltip.AddTextLine("@AbilityChangeSlot", 12, 8f);
			}
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00091F44 File Offset: 0x00090144
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right && this.ability.isReady && this.parent.GetSelectableAbilities().Count > 0)
			{
				this.ToggleAbilitySelect(null);
			}
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00091F89 File Offset: 0x00090189
		private void OnDisable()
		{
			ActivatedAbility.targetingActive = false;
		}

		// Token: 0x04000E25 RID: 3621
		[SerializeField]
		private Image borderImage;

		// Token: 0x04000E26 RID: 3622
		[SerializeField]
		private Image image;

		// Token: 0x04000E27 RID: 3623
		[SerializeField]
		private TextMeshProUGUI timer;

		// Token: 0x04000E28 RID: 3624
		[SerializeField]
		private Image fillImage;

		// Token: 0x04000E29 RID: 3625
		[SerializeField]
		private RectTransform abilitySelectParent;

		// Token: 0x04000E2A RID: 3626
		[SerializeField]
		private AbilitySelectable abilitySelectIcon;

		// Token: 0x04000E2C RID: 3628
		private AbilityHud parent;

		// Token: 0x04000E2D RID: 3629
		private TooltipSource tooltip;

		// Token: 0x04000E2E RID: 3630
		private bool targetingAbility;

		// Token: 0x04000E2F RID: 3631
		private float updateTimer;
	}
}
