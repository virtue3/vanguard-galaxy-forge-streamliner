using System;
using System.Collections.Generic;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Tractoring;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Unit.Parts;
using Behaviour.Util;
using Source.Ability;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Player;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Unit
{
	// Token: 0x020001BD RID: 445
	public class DefensiveTurret : AbstractUnit, ITooltipCustomSource
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0006F506 File Offset: 0x0006D706
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x0006F50E File Offset: 0x0006D70E
		public EquipmentBuilder turretBuilder { get; private set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0006F517 File Offset: 0x0006D717
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x0006F51F File Offset: 0x0006D71F
		public DefensiveTurretData defensiveTurretData { get; private set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0006F528 File Offset: 0x0006D728
		public override string targetName
		{
			get
			{
				return this.defensiveTurretData.type;
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0006F535 File Offset: 0x0006D735
		protected override void Start()
		{
			base.Start();
			this.tooltip = base.GetComponent<TooltipSource>();
			base.transform.Z(ZIndex.Deployable);
			this.isPlayer = (base.faction == Faction.player);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0006F568 File Offset: 0x0006D768
		protected override void Update()
		{
			base.Update();
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.2f;
				if (this.isPlayer)
				{
					UITooltip.Refresh(this.tooltip);
				}
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0006F5B8 File Offset: 0x0006D7B8
		private void OnEnable()
		{
			if (!base.surfaceSprite)
			{
				base.CloneBaseSprite();
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0006F5CD File Offset: 0x0006D7CD
		protected override AutoActions CreateAutoActions()
		{
			if (base.GetComponentInChildren<CombatModule>())
			{
				return new CombatActions(this);
			}
			return null;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0006F5E4 File Offset: 0x0006D7E4
		public override void CheckTriggerAbility(AbilityTrigger trigger, object source, AbstractUnit triggeredBySubordinate)
		{
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0006F5E8 File Offset: 0x0006D7E8
		public override void SetData(AbstractUnitData abstractUnitData, bool setUnitRef = true, bool applyBattleDamage = true)
		{
			DefensiveTurretData defensiveTurretData = abstractUnitData as DefensiveTurretData;
			if (defensiveTurretData != null)
			{
				this.defensiveTurretData = defensiveTurretData;
			}
			base.SetData(abstractUnitData, setUnitRef, applyBattleDamage);
			DefensiveTurretData defensiveTurretData2 = this.defensiveTurretData;
			DefensiveTurretItem defensiveTurretItem;
			if (defensiveTurretData2 == null)
			{
				defensiveTurretItem = null;
			}
			else
			{
				InventoryItemType turretItem = defensiveTurretData2.turretItem;
				defensiveTurretItem = ((turretItem != null) ? turretItem.GetComponent<DefensiveTurretItem>() : null);
			}
			DefensiveTurretItem defensiveTurretItem2 = defensiveTurretItem;
			if (defensiveTurretItem2)
			{
				base.currentHullHP = base.maxHullHP * defensiveTurretItem2.currentHealth;
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0006F649 File Offset: 0x0006D849
		public void UpdateAmmoData()
		{
			this.defensiveTurretData.turretItem.GetComponent<DefensiveTurretItem>().UpdateAmmoData(this);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0006F664 File Offset: 0x0006D864
		private void OnMouseUpAsButton()
		{
			if (base.GetComponent<TractorableItem>() || !UIHelper.clickTargetingAvailable)
			{
				return;
			}
			if (base.faction == Faction.player && this.defensiveTurretData.turretItem)
			{
				if (!GamePlayer.current.currentSpaceShip.IsCargoBayFull(this.defensiveTurretData.turretItem.m3))
				{
					TractorModule module = GameplayManager.Instance.spaceShip.GetModule<TractorModule>();
					if (((module != null) ? module.GetAvailableTractorBeam(true) : null) == null)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@NoTractorBeamAvailable", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(2f).Show();
						return;
					}
					this.UpdateAmmoData();
					TractorableItem tractorableItem = base.gameObject.AddComponent<TractorableItem>();
					tractorableItem.SetData(new TractorableItemData
					{
						itemType = this.defensiveTurretData.turretItem,
						itemAmount = 1,
						jettisoned = true
					});
					tractorableItem.OnMouseUpAsButton();
					Collider2D component = base.GetComponent<Collider2D>();
					if (component)
					{
						component.enabled = false;
						return;
					}
				}
				else
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCargoFull", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(2f).Show();
				}
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0006F7B0 File Offset: 0x0006D9B0
		public void OnMouseDown()
		{
			if (base.inShipYard || !UIHelper.clickTargetingAvailable)
			{
				return;
			}
			GameplayManager instance = GameplayManager.Instance;
			if (base.IsPlayerEnemy() || (Keyboard.current.ctrlKey.isPressed && base.CanBeForceFired()))
			{
				instance.spaceShip.SetManualTarget(this);
				if (!Faction.player.IsEnemy(base.faction))
				{
					base.unitData.playerHostile = true;
				}
			}
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0006F820 File Offset: 0x0006DA20
		public static void LoadAll()
		{
			DefensiveTurret.allDefensiveTurrets.Clear();
			DefensiveTurret[] array = Resources.LoadAll<DefensiveTurret>("DefensiveTurrets");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].identifier = array[i].name;
				DefensiveTurret.allDefensiveTurrets[array[i].identifier] = array[i];
			}
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0006F875 File Offset: 0x0006DA75
		public static DefensiveTurret Get(string name)
		{
			return DefensiveTurret.allDefensiveTurrets[name];
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0006F884 File Offset: 0x0006DA84
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddHeader(this.displayName, base.level, 0, 12, 8f).Item1.Text.color = ColorHelper.detailsColor;
			tooltip.AddSeparator(null);
			if (base.faction != null)
			{
				if (base.faction == Faction.player)
				{
					DefensiveTurretItem component = this.defensiveTurretData.turretItem.GetComponent<DefensiveTurretItem>();
					if (component.maxAmmo > 0)
					{
						Color c;
						if (component.currentAmmo == 0)
						{
							c = ColorHelper.reddish;
						}
						else if (component.currentAmmo < component.maxAmmo / 2)
						{
							c = ColorHelper.orange75;
						}
						else
						{
							c = ColorHelper.greenish;
						}
						tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretAmmo", c, new object[]
						{
							component.currentAmmo,
							component.maxAmmo
						}), 12, 8f);
					}
					ShieldPylonGenerator component2 = base.GetComponent<ShieldPylonGenerator>();
					if (component2 != null)
					{
						Color c;
						if (component2.charge < 0.25f)
						{
							c = ColorHelper.reddish;
						}
						else if (component2.charge < 0.75f)
						{
							c = ColorHelper.orange75;
						}
						else
						{
							c = ColorHelper.greenish;
						}
						tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretCharge", c, new object[]
						{
							GameMath.FormatPercentage(component2.charge, FormatPercentageMode.Default, 1)
						}), 12, 8f);
					}
				}
				string text = Translation.Translate(base.faction.name, Array.Empty<object>());
				tooltip.AddTextLine(text, 12, 8f).Text.color = (base.IsPlayerEnemy() ? ColorHelper.reddish : ColorHelper.greenish);
				if (base.CanBeForceFired())
				{
					tooltip.AddTextLine("@TooltipForceAttack", 12, 8f);
				}
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0006FA3E File Offset: 0x0006DC3E
		public static implicit operator string(DefensiveTurret iit)
		{
			return iit.identifier;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0006FA46 File Offset: 0x0006DC46
		public static implicit operator DefensiveTurret(string id)
		{
			return DefensiveTurret.Get(id);
		}

		// Token: 0x04000913 RID: 2323
		private static Dictionary<string, DefensiveTurret> allDefensiveTurrets = new Dictionary<string, DefensiveTurret>();

		// Token: 0x04000916 RID: 2326
		private TooltipSource tooltip;

		// Token: 0x04000917 RID: 2327
		private float updateTimer;

		// Token: 0x04000918 RID: 2328
		private bool isPlayer;
	}
}
