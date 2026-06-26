using System;
using Behaviour.Ability;
using Behaviour.Equipment;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Util;
using LightJson;
using Source.Data;
using Source.Galaxy;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200030F RID: 783
	public class DefensiveTurretItem : UsableItem
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001D65 RID: 7525 RVA: 0x000AFF3E File Offset: 0x000AE13E
		// (set) Token: 0x06001D66 RID: 7526 RVA: 0x000AFF46 File Offset: 0x000AE146
		public string turretName { get; private set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001D67 RID: 7527 RVA: 0x000AFF4F File Offset: 0x000AE14F
		// (set) Token: 0x06001D68 RID: 7528 RVA: 0x000AFF57 File Offset: 0x000AE157
		public int maxAmmo { get; private set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x000AFF60 File Offset: 0x000AE160
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x000AFF68 File Offset: 0x000AE168
		public ActivatedAbility payload { get; private set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x000AFF71 File Offset: 0x000AE171
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x000AFF74 File Offset: 0x000AE174
		public void SetTurret(string turret)
		{
			this.turretName = turret;
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000AFF80 File Offset: 0x000AE180
		public void ResetCooldown(float resetAmount = 1f)
		{
			foreach (TemporaryEffect temporaryEffect in GameplayManager.Instance.spaceShip.commander.GetComponentsInChildren<TemporaryEffect>())
			{
				if (temporaryEffect.displayName == this.payload.displayName)
				{
					temporaryEffect.durationRemaining *= 1f - resetAmount;
				}
			}
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000AFFE0 File Offset: 0x000AE1E0
		public override bool OnUse()
		{
			if (Singleton<TravelManager>.Instance.TravelActive() || MapPointOfInterest.current == null)
			{
				return false;
			}
			bool flag = false;
			float num = 0f;
			foreach (TemporaryEffect temporaryEffect in GameplayManager.Instance.spaceShip.commander.GetComponentsInChildren<TemporaryEffect>())
			{
				if (temporaryEffect.displayName == this.payload.displayName)
				{
					flag = true;
					num = temporaryEffect.durationRemaining;
				}
			}
			if (flag)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@TurretDeploymentCooldown", new object[]
				{
					num
				})).WithColor(ColorHelper.red90).WithCustomTime(2f).Show();
				return false;
			}
			this.payload.TriggerPayload(this.payload.payload, GameplayManager.Instance.spaceShip.commander.transform, null, true);
			this.SpawnDefensiveTurret();
			return true;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000B00CC File Offset: 0x000AE2CC
		public virtual void SpawnDefensiveTurret()
		{
			DefensiveTurretData defensiveTurretData = new DefensiveTurretData(this.turretName);
			defensiveTurretData.SetTurretItem(base.GetComponent<InventoryItemType>());
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			defensiveTurretData.positionData.position = spaceShip.transform.position + spaceShip.transform.rotation * new Vector3(-2f, 0f, 0f);
			if (this.currentAmmo > 0)
			{
				defensiveTurretData.cargo.Add(defensiveTurretData.turretPrefab.ammoType, this.currentAmmo, false, false);
			}
			MapPointOfInterest current = MapPointOfInterest.current;
			if (current == null)
			{
				return;
			}
			current.AddUnit(defensiveTurretData, base.item.gameObject.name, true);
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000B0190 File Offset: 0x000AE390
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			DefensiveTurretData defensiveTurretData = new DefensiveTurretData(this.turretName);
			defensiveTurretData.SetTurretItem(base.GetComponent<InventoryItemType>());
			MainStat mainStat = defensiveTurretData.unitDefinition.turretBuilder.CreateItemType(Rarity.Standard, base.item.itemLevel, true, "preview", false, false).GetComponent<AbstractEquipment>().GetMainStat();
			tooltip.AddTextLine("<color=green>" + mainStat.mainStatAmount + "</color> " + Translation.Translate(mainStat.mainStatName, Array.Empty<object>()), 16, 8f).Text.color = CompareTooltip.detailsColor;
			tooltip.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			Color c;
			if (this.currentHealth < 0.25f)
			{
				c = ColorHelper.reddish;
			}
			else if (this.currentHealth < 0.75f)
			{
				c = ColorHelper.orange75;
			}
			else
			{
				c = ColorHelper.greenish;
			}
			tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretHealth", c, new object[]
			{
				GameMath.FormatPercentage(this.currentHealth, FormatPercentageMode.Default, 1)
			}), 12, 8f);
			if (this.maxAmmo > 0)
			{
				if (this.currentAmmo == 0)
				{
					c = ColorHelper.reddish;
				}
				else if (this.currentAmmo < this.maxAmmo / 2)
				{
					c = ColorHelper.orange75;
				}
				else
				{
					c = ColorHelper.greenish;
				}
				tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretAmmo", c, new object[]
				{
					this.currentAmmo,
					this.maxAmmo
				}), 12, 8f);
				tooltip.AddTextLine(Translation.Translate("@DefensiveTurretReload", Array.Empty<object>()), 12, 8f);
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@DefensiveTurretRepair", Array.Empty<object>()), 12, 8f);
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000B0368 File Offset: 0x000AE568
		public virtual void UpdateAmmoData(DefensiveTurret turret)
		{
			this.currentHealth = Mathf.Clamp01(turret.currentHullHP / turret.maxHullHP);
			AbstractTurret turretPrefab = turret.defensiveTurretData.turretPrefab;
			InventoryItemType inventoryItemType = (turretPrefab != null) ? turretPrefab.ammoType : null;
			if (inventoryItemType)
			{
				this.currentAmmo = turret.defensiveTurretData.cargo.GetCount(inventoryItemType);
			}
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000B03C4 File Offset: 0x000AE5C4
		public override void DataFromJson(JsonObject data)
		{
			this.turretName = data["turret"];
			this.currentAmmo = data["currentAmmo"];
			this.currentHealth = (float)data["currentHealth"].AsNumber;
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000B0418 File Offset: 0x000AE618
		public override void DataToJson(JsonObject data)
		{
			data["turret"] = this.turretName;
			data["currentHealth"] = new double?((double)this.currentHealth);
			if (this.maxAmmo > 0)
			{
				data["currentAmmo"] = new double?((double)this.currentAmmo);
			}
		}

		// Token: 0x04001202 RID: 4610
		public int currentAmmo;

		// Token: 0x04001203 RID: 4611
		public float currentHealth = 1f;
	}
}
