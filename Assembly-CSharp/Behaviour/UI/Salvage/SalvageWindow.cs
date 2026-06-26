using System;
using System.Collections.Generic;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Salvage;
using Behaviour.UI.HUD;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Data.Persistable;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Salvage
{
	// Token: 0x02000248 RID: 584
	public class SalvageWindow : MonoBehaviour
	{
		// Token: 0x060015A3 RID: 5539 RVA: 0x0008A6C1 File Offset: 0x000888C1
		private void Update()
		{
			if (this.contained.itemContent.Count != this.itemCount)
			{
				this.SetSalvage(this.world, this.contained);
			}
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0008A6F0 File Offset: 0x000888F0
		public void SetSalvage(SalvageContainer world, SalvageData data)
		{
			this.world = world;
			this.contained = data;
			this.itemCount = data.itemContent.Count;
			this.label.TL("@SalvageContainer", new object[]
			{
				data.displayName
			});
			if (!GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Both))
			{
				this.desc.text = Translation.Translate("@SalvageNoTurrets", Array.Empty<object>());
				this.desc.color = ColorHelper.reddish;
			}
			this.rowParent.DestroyChildren();
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			float num = 0f;
			foreach (AbstractSalvageTurret abstractSalvageTurret in spaceShip.GetComponentsInChildren<AbstractSalvageTurret>())
			{
				num = Mathf.Max(num, abstractSalvageTurret.yield);
			}
			if (spaceShip.droneBayModule)
			{
				foreach (Drone drone in spaceShip.droneBayModule.drones)
				{
					AbstractSalvageTurret componentInChildren = drone.GetComponentInChildren<AbstractSalvageTurret>();
					if (componentInChildren)
					{
						num = Mathf.Max(num, componentInChildren.yield);
					}
				}
			}
			float num2 = 0f;
			foreach (SalvageItemData salvageItemData in data.availableItemContent)
			{
				SalvageWindowRow salvageWindowRow = UnityEngine.Object.Instantiate<SalvageWindowRow>(this.rowPrefab, this.rowParent);
				salvageWindowRow.SetItem(salvageItemData, Mathf.Clamp01(salvageItemData.baseChance * num));
				RectTransform rectTransform = salvageWindowRow.transform as RectTransform;
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, num2);
				num2 -= 32f;
			}
			InventoryItemType scrap = null;
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in data.scrapContent)
			{
				if (keyValuePair.Value > 0)
				{
					scrap = keyValuePair.Key;
				}
			}
			num2 -= 10f;
			SalvageWindowRow salvageWindowRow2 = UnityEngine.Object.Instantiate<SalvageWindowRow>(this.rowPrefab, this.rowParent);
			salvageWindowRow2.SetScrap(scrap);
			RectTransform rectTransform2 = salvageWindowRow2.transform as RectTransform;
			rectTransform2.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x, num2);
			num2 -= 32f;
			RectTransform rectTransform3 = base.transform as RectTransform;
			rectTransform3.sizeDelta = new Vector2(rectTransform3.sizeDelta.x, -num2 + 32f + 24f);
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0008A98C File Offset: 0x00088B8C
		public void SetSelectedItem(InventoryItemType ii)
		{
			if (!GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Both))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalvageNoTurrets", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
			}
			else
			{
				this.contained.SetActiveItem(ii);
				if (ii != null && this.world)
				{
					GameplayManager.Instance.spaceShip.SetManualTarget(this.world);
					HudManager.Instance.ToggleSalvageStatus(this.world, this.contained);
				}
			}
			HudManager.Instance.ToggleSalvageWindow(this.world, this.contained);
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0008AA39 File Offset: 0x00088C39
		public void CloseWindow()
		{
			HudManager.Instance.ToggleSalvageWindow(this.world, this.contained);
		}

		// Token: 0x04000CE3 RID: 3299
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000CE4 RID: 3300
		[SerializeField]
		private RectTransform rowParent;

		// Token: 0x04000CE5 RID: 3301
		[SerializeField]
		private SalvageWindowRow rowPrefab;

		// Token: 0x04000CE6 RID: 3302
		[SerializeField]
		private TMP_Text desc;

		// Token: 0x04000CE7 RID: 3303
		[SerializeField]
		private TMP_Text yieldInfo;

		// Token: 0x04000CE8 RID: 3304
		private SalvageContainer world;

		// Token: 0x04000CE9 RID: 3305
		private SalvageData contained;

		// Token: 0x04000CEA RID: 3306
		private int itemCount;
	}
}
