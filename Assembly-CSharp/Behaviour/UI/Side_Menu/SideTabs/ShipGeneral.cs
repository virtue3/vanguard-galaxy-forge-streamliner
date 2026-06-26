using System;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.Unit;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002BF RID: 703
	public class ShipGeneral : SideTabContent
	{
		// Token: 0x060019DB RID: 6619 RVA: 0x000A0CFC File Offset: 0x0009EEFC
		private void Awake()
		{
			float y = this.content.anchoredPosition.y;
			this.currentShip = GameplayManager.Instance.spaceShip;
			this.shipName.text = this.currentShip.displayName;
			this.AddInfoPanelGeneral();
			this.CheckCombatStats();
			this.CheckDroneStats();
			this.CheckMiningStats();
			this.CheckSalvagingStats();
			this.AddInfoPanelDefense();
			Vector2 sizeDelta = (this.content.transform as RectTransform).sizeDelta;
			sizeDelta.y = Math.Max(this.leftColumnHeight, this.rightColumnHeight);
			(this.content.transform as RectTransform).sizeDelta = sizeDelta;
			this.content.anchoredPosition = new Vector2(this.content.anchoredPosition.x, -sizeDelta.y - y);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000A0DD4 File Offset: 0x0009EFD4
		private void CheckMiningStats()
		{
			AbstractMiningTurret[] miningTurrets = this.currentShip.GetMiningTurrets();
			float num = 0f;
			float num2 = 0f;
			float num3 = this.currentShip.GetStat(EquipStat.MiningPower);
			float num4 = 0f;
			float num5 = this.currentShip.GetStat(EquipStat.MiningPower);
			float num6 = 0f;
			foreach (AbstractMiningTurret abstractMiningTurret in miningTurrets)
			{
				if (abstractMiningTurret.targetsCore)
				{
					num6 += (float)Math.Ceiling((double)abstractMiningTurret.displayedPower);
				}
				if (abstractMiningTurret.targetsSurface)
				{
					num4 += (float)Math.Ceiling((double)abstractMiningTurret.displayedPower);
				}
				num += abstractMiningTurret._yield;
				num2 += abstractMiningTurret.yield;
			}
			if (num4 == 0f)
			{
				num3 = 0f;
			}
			if (num6 == 0f)
			{
				num5 = 0f;
			}
			num2 /= (float)miningTurrets.Length;
			if ((num3 > 0f || num5 > 0f) && miningTurrets.Length != 0)
			{
				this.AddInfoPanelMining(num3, num4, num5, num6, num, num2);
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000A0ED8 File Offset: 0x0009F0D8
		private void CheckCombatStats()
		{
			AbstractCombatTurret[] combatTurrets = this.currentShip.GetCombatTurrets();
			float stat = this.currentShip.GetStat(EquipStat.CombatPower);
			float num = 0f;
			foreach (AbstractCombatTurret abstractCombatTurret in combatTurrets)
			{
				num += (float)Math.Ceiling((double)abstractCombatTurret.displayedPower);
			}
			num += this.currentShip.GetStat(EquipStat.Power);
			if (stat > 0f)
			{
				this.AddInfoPanelCombat(stat, num);
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000A0F4C File Offset: 0x0009F14C
		private void CheckDroneStats()
		{
			if (this.currentShip.droneBayModule == null)
			{
				return;
			}
			float num = this.currentShip.GetStat(EquipStat.DronePower);
			num += this.currentShip.spaceShipData.GetEquippedItem(EquipmentSlot.DroneBay).GetComponent<DroneBayModule>().GetDronePower();
			float dronePower = 0f;
			if (num > 0f)
			{
				this.AddInfoPanelDrone(num, dronePower);
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000A0FB0 File Offset: 0x0009F1B0
		private void CheckSalvagingStats()
		{
			AbstractSalvageTurret[] salvageTurrets = this.currentShip.GetSalvageTurrets();
			float stat = this.currentShip.GetStat(EquipStat.SalvagePower);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			foreach (AbstractSalvageTurret abstractSalvageTurret in salvageTurrets)
			{
				num += (float)Math.Ceiling((double)abstractSalvageTurret.displayedPower);
				num2 += abstractSalvageTurret._yield;
				num3 += abstractSalvageTurret.yield;
			}
			num3 /= (float)salvageTurrets.Length;
			if (stat > 0f && salvageTurrets.Length != 0)
			{
				this.AddInfoPanelSalvage(stat, num, num2, num3);
			}
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000A1050 File Offset: 0x0009F250
		public void AddInfoPanelGeneral()
		{
			InfoPanel infoPanel = UnityEngine.Object.Instantiate<InfoPanel>(this.infoPanelPrefab, this.content);
			(infoPanel.transform as RectTransform).anchoredPosition = new Vector2(0f, -this.yOffset);
			infoPanel.SetHeader(Translation.Translate("@ShipInfoGeneral", Array.Empty<object>()), null);
			float num = this.currentShip.GetStat(EquipStat.EnergyCapacity);
			infoPanel.AddInfoRow(num, EquipStat.EnergyCapacity, num, "", false);
			num = this.currentShip.GetStat(EquipStat.HullHP);
			infoPanel.AddInfoRow(num, EquipStat.HullHP, num, "", false);
			num = this.currentShip.GetStat(EquipStat.HullRegen);
			infoPanel.AddInfoRow(num, EquipStat.HullRegen, num, "", false);
			num = this.currentShip.GetStat(EquipStat.CargoCapacity);
			infoPanel.AddInfoRow(num, EquipStat.CargoCapacity, num, "", false);
			num = ((this.currentShip.engine != null) ? this.currentShip.engine.thrust : 0f);
			infoPanel.AddInfoRow(num, EquipStat.Thrust, num, "", false);
			num = this.currentShip.GetStat(EquipStat.Precision);
			infoPanel.AddInfoRow(num, EquipStat.Precision, num, "", false);
			num = this.currentShip.GetStat(EquipStat.CriticalChance);
			infoPanel.AddInfoRow(num, EquipStat.CriticalChance, num, "", false);
			num = 1f + this.currentShip.GetStat(EquipStat.CriticalDamage);
			infoPanel.AddInfoRow(num, EquipStat.CriticalDamage, num, "", false);
			this.leftColumnHeight += 275f;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000A11D0 File Offset: 0x0009F3D0
		public void AddInfoPanelDefense()
		{
			InfoPanel infoPanel = UnityEngine.Object.Instantiate<InfoPanel>(this.infoPanelPrefab, this.content);
			(infoPanel.transform as RectTransform).anchoredPosition = new Vector2(this.xOffset, -this.yOffset - this.rightColumnHeight);
			infoPanel.SetHeader(Translation.Translate("@ShipInfoDefense", Array.Empty<object>()), new Color?(ColorHelper.greenish));
			if (this.currentShip.maxShieldHP > 0f && this.currentShip.shieldGeneratorModule)
			{
				float num = this.currentShip.GetStat(EquipStat.ShieldHP);
				infoPanel.AddInfoRow(num, EquipStat.ShieldHP, num, "", false);
				num = this.currentShip.GetStat(EquipStat.ShieldRegen);
				infoPanel.AddInfoRow(num, EquipStat.ShieldRegen, this.currentShip.GetStat(EquipStat.ShieldRegen), "", false);
				num = this.currentShip.GetStat(EquipStat.ShieldRechargeDelay);
				infoPanel.AddInfoRow(num, EquipStat.ShieldRechargeDelay, num, "", false);
				num = this.currentShip.GetStat(EquipStat.ShieldRechargeRate);
				infoPanel.AddInfoRow(num, EquipStat.ShieldRechargeRate, num, "", false);
				this.rightColumnHeight += 120f;
			}
			if (this.currentShip.maxArmorHP > 0f && this.currentShip.armorModule)
			{
				float num = this.currentShip.GetStat(EquipStat.ArmorHP);
				infoPanel.AddInfoRow(num, EquipStat.ArmorHP, num, "", false);
				num = this.currentShip.GetStat(EquipStat.ArmorRegen);
				infoPanel.AddInfoRow(num, EquipStat.ArmorRegen, num, "", false);
				num = this.currentShip.armorModule.resistAmount;
				infoPanel.AddInfoRow(num, EquipStat.DamageReduction, num, "", false);
				this.rightColumnHeight += 90f;
			}
			foreach (EquipStat equipStat in new EquipStat[]
			{
				EquipStat.ColdResist,
				EquipStat.CorrosionResist,
				EquipStat.EnergyResist,
				EquipStat.ExplosiveResist,
				EquipStat.HeatResist,
				EquipStat.KineticResist,
				EquipStat.RadiationResist
			})
			{
				float num = this.currentShip.GetStat(equipStat);
				infoPanel.AddInfoRow(num, equipStat, num, "", false);
			}
			this.rightColumnHeight += 245f;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000A13E4 File Offset: 0x0009F5E4
		public void AddInfoPanelMining(float baseSurfacePower, float surfacePower, float baseCorePower, float corePower, float baseYield, float yield)
		{
			InfoPanel infoPanel = UnityEngine.Object.Instantiate<InfoPanel>(this.infoPanelPrefab, this.content);
			(infoPanel.transform as RectTransform).anchoredPosition = new Vector2(0f, -this.yOffset - this.leftColumnHeight);
			infoPanel.SetHeader(Translation.Translate("@ShipInfoMining", Array.Empty<object>()), new Color?(ColorHelper.discBlue));
			infoPanel.AddInfoRow(baseSurfacePower, EquipStat.MiningPower, baseSurfacePower, "@MiningSurface", false);
			infoPanel.AddInfoRow(baseCorePower, EquipStat.MiningPower, baseCorePower, "@MiningCore", false);
			infoPanel.AddInfoRow(yield, EquipStat.Yield, yield, "", false);
			this.leftColumnHeight += 125f;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000A148C File Offset: 0x0009F68C
		public void AddInfoPanelCombat(float baseCombatPower, float combatPower)
		{
			InfoPanel infoPanel = UnityEngine.Object.Instantiate<InfoPanel>(this.infoPanelPrefab, this.content);
			(infoPanel.transform as RectTransform).anchoredPosition = new Vector2(this.xOffset, -this.yOffset - this.rightColumnHeight);
			infoPanel.SetHeader(Translation.Translate("@ShipInfoCombat", Array.Empty<object>()), new Color?(ColorHelper.reddish));
			infoPanel.AddInfoRow(baseCombatPower, EquipStat.CombatPower, baseCombatPower, "", false);
			this.rightColumnHeight += 65f;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x000A1514 File Offset: 0x0009F714
		public void AddInfoPanelDrone(float baseDronePower, float dronePower)
		{
			InfoPanel infoPanel = UnityEngine.Object.Instantiate<InfoPanel>(this.infoPanelPrefab, this.content);
			(infoPanel.transform as RectTransform).anchoredPosition = new Vector2(this.xOffset, -this.yOffset - this.rightColumnHeight);
			infoPanel.SetHeader(Translation.Translate("@ShipInfoDrones", Array.Empty<object>()), new Color?(ColorHelper.lightCyan));
			infoPanel.AddInfoRow(baseDronePower, EquipStat.DronePower, baseDronePower, "", false);
			int droneAmount = this.currentShip.droneBayModule.droneAmount;
			string customString = Translation.Translate("@DroneAmount", Array.Empty<object>()) ?? "";
			infoPanel.AddInfoRowCustomString(customString, droneAmount);
			this.rightColumnHeight += 95f;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x000A15D0 File Offset: 0x0009F7D0
		public void AddInfoPanelSalvage(float baseSalvagePower, float salvagePower, float baseYield, float yield)
		{
			InfoPanel infoPanel = UnityEngine.Object.Instantiate<InfoPanel>(this.infoPanelPrefab, this.content);
			(infoPanel.transform as RectTransform).anchoredPosition = new Vector2(0f, -this.yOffset - this.leftColumnHeight);
			infoPanel.SetHeader(Translation.Translate("@ShipInfoSalvage", Array.Empty<object>()), new Color?(ColorHelper.orange75));
			infoPanel.AddInfoRow(yield, EquipStat.Yield, yield, "", false);
			infoPanel.AddInfoRow(baseSalvagePower, EquipStat.SalvagePower, baseSalvagePower, "", false);
			this.leftColumnHeight += 95f;
		}

		// Token: 0x04001044 RID: 4164
		[SerializeField]
		private InfoPanel infoPanelPrefab;

		// Token: 0x04001045 RID: 4165
		[SerializeField]
		private RectTransform content;

		// Token: 0x04001046 RID: 4166
		[SerializeField]
		private TMP_Text shipName;

		// Token: 0x04001047 RID: 4167
		private float xOffset = 360f;

		// Token: 0x04001048 RID: 4168
		private float yOffset;

		// Token: 0x04001049 RID: 4169
		private float leftColumnHeight;

		// Token: 0x0400104A RID: 4170
		private float rightColumnHeight;

		// Token: 0x0400104B RID: 4171
		private SpaceShip currentShip;
	}
}
