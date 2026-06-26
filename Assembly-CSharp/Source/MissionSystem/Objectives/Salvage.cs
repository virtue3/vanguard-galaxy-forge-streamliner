using System;
using Behaviour.Equipment.Module;
using Behaviour.Unit;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000CC RID: 204
	public class Salvage : Mining
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0003F3EB File Offset: 0x0003D5EB
		public override string coreName
		{
			get
			{
				return "Structural";
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0003F3F4 File Offset: 0x0003D5F4
		public override bool LoadoutCanRetrieveItem(MapPointOfInterest poi)
		{
			GameplayManager instance = GameplayManager.Instance;
			UnityEngine.Object exists;
			if (instance == null)
			{
				exists = null;
			}
			else
			{
				SpaceShip spaceShip = instance.spaceShip;
				exists = ((spaceShip != null) ? spaceShip.GetModule<SalvageModule>() : null);
			}
			if (!exists)
			{
				return false;
			}
			bool flag = false;
			foreach (PersistableData persistableData in poi.GetPersistables())
			{
				SalvageData salvageData = persistableData as SalvageData;
				if (salvageData != null)
				{
					flag = (salvageData.HasItems() || salvageData.HasScrap());
				}
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0003F484 File Offset: 0x0003D684
		public override string statusText
		{
			get
			{
				string text;
				if (this.itemType)
				{
					text = Translation.Translate(this.itemType.displayName, Array.Empty<object>());
				}
				else if (this.itemCategory != null)
				{
					text = Translation.Translate("@ItemCategory" + this.itemCategory.ToString(), Array.Empty<object>());
				}
				else
				{
					text = "Items";
				}
				return string.Concat(new string[]
				{
					text,
					" collected: ",
					base.currentAmount.ToString(),
					"/",
					this.requiredAmount.ToString()
				});
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0003F52F File Offset: 0x0003D72F
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0003F537 File Offset: 0x0003D737
		public override GameplayType gameplayType { get; set; } = GameplayType.Salvage;
	}
}
