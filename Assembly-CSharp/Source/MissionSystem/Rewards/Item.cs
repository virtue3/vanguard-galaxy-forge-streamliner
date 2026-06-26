using System;
using Behaviour.Item;
using LightJson;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B6 RID: 182
	public class Item : MissionReward
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0003DAD8 File Offset: 0x0003BCD8
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly(this.item.displayName, Array.Empty<object>()) + ((this.amount > 1) ? (" x" + this.amount.ToString()) : "");
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0003DB24 File Offset: 0x0003BD24
		public override Sprite rewardIcon
		{
			get
			{
				return this.item.icon;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0003DB31 File Offset: 0x0003BD31
		public override Color rewardColor
		{
			get
			{
				return this.item.rarity.GetColor();
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0003DB43 File Offset: 0x0003BD43
		public override void DataToJson(JsonObject data)
		{
			data["item"] = this.item.ToJson();
			data["count"] = new double?((double)this.amount);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0003DB77 File Offset: 0x0003BD77
		public override void LoadFromJson(JsonObject data)
		{
			this.item = InventoryItemType.FromJson(data["item"]);
			if (data.ContainsKey("count"))
			{
				this.amount = data["count"];
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0003DBB2 File Offset: 0x0003BDB2
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.currentSpaceShip.AddCargo(this.item, this.amount, true);
		}

		// Token: 0x04000443 RID: 1091
		public InventoryItemType item;

		// Token: 0x04000444 RID: 1092
		public int amount = 1;
	}
}
