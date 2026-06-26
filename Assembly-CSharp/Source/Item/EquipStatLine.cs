using System;
using LightJson;
using Source.Util;

namespace Source.Item
{
	// Token: 0x020000F0 RID: 240
	[Serializable]
	public struct EquipStatLine : IJsonSource
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x000464D4 File Offset: 0x000446D4
		public EquipStatLine(EquipStat stat, float amt, float multiplier = 1f, bool canReroll = true)
		{
			this.stat = stat;
			this.amount = amt;
			this.multiplier = multiplier;
			this.canReroll = canReroll;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000464F4 File Offset: 0x000446F4
		public string ToReadableString(bool noDecimal = false)
		{
			string str = this.ToString(noDecimal);
			if (this.multiplier != 1f)
			{
				return ((this.multiplier > 1f) ? "+" : "") + str + " " + this.stat.GetDisplayName();
			}
			return ((this.amount > 0f) ? "+" : "") + str + " " + this.stat.GetDisplayName();
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00046578 File Offset: 0x00044778
		public string ToString(bool noDecimal = false)
		{
			if (this.multiplier != 1f)
			{
				return GameMath.FormatPercentage(this.multiplier - 1f, FormatPercentageMode.Default, noDecimal ? 0 : 1);
			}
			if (this.stat.IsPercentageStat())
			{
				return GameMath.FormatPercentage(this.amount, FormatPercentageMode.Default, noDecimal ? 0 : 1);
			}
			return GameMath.FormatNumber(this.amount, -1);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000465DC File Offset: 0x000447DC
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject
			{
				{
					"stat",
					this.stat.ToString()
				}
			};
			if (this.multiplier != 1f)
			{
				jsonObject["multiplier"] = new double?((double)this.multiplier);
			}
			else
			{
				jsonObject["amount"] = new double?((double)this.amount);
			}
			jsonObject["cr"] = new bool?(this.canReroll);
			return jsonObject;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0004667C File Offset: 0x0004487C
		public static EquipStatLine FromJson(JsonValue data)
		{
			float amt = 0f;
			float num = 1f;
			bool flag = true;
			if (data["amount"].IsNumber)
			{
				amt = (float)data["amount"].AsNumber;
			}
			else
			{
				num = (float)data["multiplier"].AsNumber;
			}
			if (!data["cr"].IsNull)
			{
				flag = data["cr"];
			}
			return new EquipStatLine(Enum.Parse<EquipStat>(data["stat"]), amt, num, flag);
		}

		// Token: 0x040004F0 RID: 1264
		public EquipStat stat;

		// Token: 0x040004F1 RID: 1265
		public float amount;

		// Token: 0x040004F2 RID: 1266
		public float multiplier;

		// Token: 0x040004F3 RID: 1267
		public bool canReroll;
	}
}
