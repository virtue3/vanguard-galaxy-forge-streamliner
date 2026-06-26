using System;
using Behaviour.Unit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000285 RID: 645
	public class ShipHealthMonitor : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x060017B6 RID: 6070 RVA: 0x00094E88 File Offset: 0x00093088
		private void Awake()
		{
			ShipHealthMonitor.instance = this;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00094E90 File Offset: 0x00093090
		private void Update()
		{
			if (!GameplayManager.Instance || !GameplayManager.Instance.spaceShip)
			{
				return;
			}
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f && GameplayManager.Instance.spaceShip)
			{
				this.updateTimer = 0.1f;
				this.spaceShip = GameplayManager.Instance.spaceShip;
				this.SetMaxStatusAmounts(this.spaceShip.maxHullHP, this.spaceShip.currentHullHP, this.spaceShip.hullHPScale, this.spaceShip.maxShieldHP, this.spaceShip.currentShieldHP, this.spaceShip.shieldHPScale, this.spaceShip.maxArmorHP, this.spaceShip.currentArmorHP, this.spaceShip.armorHPScale);
				if (this.healthBar.isActiveAndEnabled)
				{
					this.UpdateHealth();
				}
				if (this.shieldBar.isActiveAndEnabled)
				{
					this.UpdateShield();
				}
				if (this.armorBar.isActiveAndEnabled)
				{
					this.UpdateArmor();
				}
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00094FB0 File Offset: 0x000931B0
		public void SetMaxStatusAmounts(float maxHealth = 0f, float currentHealth = 0f, float hpModifier = 0f, float maxShield = 0f, float currentShield = 0f, float shieldModifier = 0f, float maxArmor = 0f, float currentArmor = 0f, float armorModifier = 0f)
		{
			if (maxHealth > 0f)
			{
				this.healthBar.InitBar(maxHealth, currentHealth, hpModifier, BarType.Hull);
			}
			if (maxShield > 0f)
			{
				if (!this.shieldBar.gameObject.activeSelf)
				{
					this.shieldBar.gameObject.SetActive(true);
				}
				this.shieldBar.InitBar(maxShield, currentShield, shieldModifier, BarType.Shield);
			}
			else if (this.shieldBar && this.shieldBar.gameObject.activeSelf)
			{
				this.shieldBar.gameObject.SetActive(false);
			}
			if (maxArmor > 0f)
			{
				if (!this.armorBar.gameObject.activeSelf)
				{
					this.armorBar.gameObject.SetActive(true);
				}
				this.armorBar.InitBar(maxArmor, currentArmor, armorModifier, BarType.Armor);
			}
			else if (this.armorBar && this.armorBar.gameObject.activeSelf)
			{
				this.armorBar.gameObject.SetActive(false);
			}
			HudManager.Instance.RefreshFleetLayout();
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x000950BE File Offset: 0x000932BE
		public void UpdateHealth()
		{
			this.healthBar.SetHealth(this.spaceShip.currentHullHP, this.spaceShip.maxHullHP);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000950E1 File Offset: 0x000932E1
		public void UpdateShield()
		{
			this.shieldBar.SetHealth(this.spaceShip.currentShieldHP, this.spaceShip.maxShieldHP);
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00095104 File Offset: 0x00093304
		public void UpdateArmor()
		{
			this.armorBar.SetHealth(this.spaceShip.currentArmorHP, this.spaceShip.maxArmorHP);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00095127 File Offset: 0x00093327
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.healthBar.ShowLabel(true);
			this.armorBar.ShowLabel(true);
			this.shieldBar.ShowLabel(true);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0009514D File Offset: 0x0009334D
		public void OnPointerExit(PointerEventData eventData)
		{
			this.healthBar.ShowLabel(false);
			this.armorBar.ShowLabel(false);
			this.shieldBar.ShowLabel(false);
		}

		// Token: 0x04000EB2 RID: 3762
		public static ShipHealthMonitor instance;

		// Token: 0x04000EB3 RID: 3763
		public AmountBar healthBar;

		// Token: 0x04000EB4 RID: 3764
		public AmountBar shieldBar;

		// Token: 0x04000EB5 RID: 3765
		public AmountBar armorBar;

		// Token: 0x04000EB6 RID: 3766
		private float updateTimer;

		// Token: 0x04000EB7 RID: 3767
		private SpaceShip spaceShip;
	}
}
