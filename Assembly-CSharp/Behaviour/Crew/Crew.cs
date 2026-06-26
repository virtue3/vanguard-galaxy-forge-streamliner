using System;
using System.Collections.Generic;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A2 RID: 930
	public class Crew : MonoBehaviour
	{
		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06002330 RID: 9008 RVA: 0x000C9A9C File Offset: 0x000C7C9C
		public static IEnumerable<Crew> all
		{
			get
			{
				return Crew.allCrew.Values;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x000C9AA8 File Offset: 0x000C7CA8
		// (set) Token: 0x06002332 RID: 9010 RVA: 0x000C9AB0 File Offset: 0x000C7CB0
		public string identifier { get; private set; }

		// Token: 0x06002333 RID: 9011 RVA: 0x000C9AB9 File Offset: 0x000C7CB9
		public static implicit operator string(Crew cr)
		{
			return cr.identifier;
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000C9AC1 File Offset: 0x000C7CC1
		public static implicit operator Crew(string id)
		{
			return Crew.Get(id);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x000C9ACC File Offset: 0x000C7CCC
		public string GetBonus()
		{
			string text = "";
			if (this.miningBonus > 0f)
			{
				text = text + Translation.Translate("@EquipStatMiningPower", Array.Empty<object>()) + ": " + GameMath.FormatPercentage(this.miningBonus, FormatPercentageMode.Default, 2);
				text += "\n";
			}
			if (this.salvageBonus > 0f)
			{
				text = text + Translation.Translate("@EquipStatSalvagePower", Array.Empty<object>()) + ": " + GameMath.FormatPercentage(this.salvageBonus, FormatPercentageMode.Default, 2);
				text += "\n";
			}
			if (this.combatBonus > 0f)
			{
				text = text + Translation.Translate("@EquipStatCombatPower", Array.Empty<object>()) + ": " + GameMath.FormatPercentage(this.combatBonus, FormatPercentageMode.Default, 2);
				text += "\n";
			}
			if (this.hullRepair > 0f)
			{
				text = text + Translation.Translate("@EquipStatHullRegen", Array.Empty<object>()) + ": " + GameMath.FormatNumber(this.hullRepair, -1);
			}
			return text;
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000C9BD7 File Offset: 0x000C7DD7
		public static Crew Get(string name)
		{
			return Crew.allCrew[name];
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000C9BE4 File Offset: 0x000C7DE4
		public static void LoadAll()
		{
			Crew.allCrew.Clear();
			Crew[] array = Resources.LoadAll<Crew>("Crew");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					array[i].identifier = array[i].gameObject.name;
					Crew.allCrew[array[i].identifier] = array[i];
				}
			}
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000C9C54 File Offset: 0x000C7E54
		public static string GetRandom(int level, bool playerOnly = true)
		{
			List<string> list = new List<string>();
			foreach (Crew crew in Crew.all)
			{
				if (crew.availableForPlayer && level >= crew.level)
				{
					list.Add(crew.identifier);
				}
			}
			return SeededRandom.Global.Choose<string>(list);
		}

		// Token: 0x04001507 RID: 5383
		private static Dictionary<string, Crew> allCrew = new Dictionary<string, Crew>();

		// Token: 0x04001509 RID: 5385
		public Rarity rarity;

		// Token: 0x0400150A RID: 5386
		public int level;

		// Token: 0x0400150B RID: 5387
		public bool availableForPlayer = true;

		// Token: 0x0400150C RID: 5388
		[Header("Economy")]
		public int wage;

		// Token: 0x0400150D RID: 5389
		[Header("Combat")]
		public int resistance;

		// Token: 0x0400150E RID: 5390
		public int boardingPower;

		// Token: 0x0400150F RID: 5391
		[Header("Bonuses")]
		public float miningBonus;

		// Token: 0x04001510 RID: 5392
		public float salvageBonus;

		// Token: 0x04001511 RID: 5393
		public float combatBonus;

		// Token: 0x04001512 RID: 5394
		public float hullRepair;

		// Token: 0x04001513 RID: 5395
		[Header("Flavor")]
		public string displayName;

		// Token: 0x04001514 RID: 5396
		public Sprite icon;
	}
}
