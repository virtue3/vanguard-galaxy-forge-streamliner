using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Spacestation.Location;
using LightJson;
using UnityEngine;

namespace Source.Crew
{
	// Token: 0x02000125 RID: 293
	[Serializable]
	public class CrewData : IJsonSource
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00052A3D File Offset: 0x00050C3D
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00052A45 File Offset: 0x00050C45
		public Dictionary<string, int> crew { get; private set; } = new Dictionary<string, int>();

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00052A4E File Offset: 0x00050C4E
		public int totalCrew
		{
			get
			{
				return this.crew.Values.Sum();
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00052A60 File Offset: 0x00050C60
		public int GetCrewCount(string type)
		{
			return this.crew[type];
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00052A70 File Offset: 0x00050C70
		public void AddCrew(string type, int amount)
		{
			if (amount <= 0)
			{
				return;
			}
			int num;
			if (this.crew.TryGetValue(type, out num))
			{
				this.crew[type] = num + amount;
			}
			else
			{
				this.crew[type] = amount;
			}
			this.Refresh();
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00052AB8 File Offset: 0x00050CB8
		public int RemoveCrew(string type, int amount)
		{
			if (amount <= 0)
			{
				return 0;
			}
			int num;
			if (!this.crew.TryGetValue(type, out num))
			{
				Debug.LogWarning("Crew is trying to be removed: " + type + ". But it is not available in this dict.");
				return 0;
			}
			int b = this.crew[type];
			int num2 = Mathf.Min(amount, b);
			Dictionary<string, int> crew = this.crew;
			crew[type] -= num2;
			this.Refresh();
			return num2;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00052B2C File Offset: 0x00050D2C
		public int RemoveAnyCrew(int amount)
		{
			int num = 0;
			foreach (string text in this.crew.Keys.ToList<string>())
			{
				if (num >= amount)
				{
					break;
				}
				int num2 = Mathf.Min(amount - num, this.crew[text]);
				Dictionary<string, int> crew = this.crew;
				string key = text;
				crew[key] -= num2;
				num += num2;
			}
			this.Refresh();
			return num;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00052BCC File Offset: 0x00050DCC
		private void Refresh()
		{
			if (PersonalHangar.current && PersonalHangar.current.shipSelect.crewViewOpen)
			{
				PersonalHangar.current.shipSelect.PopulateCrewView();
			}
			ShipEquipment shipEquipment = SidePanel.instance.ShipEquipment();
			if (SidePanel.instance != null && shipEquipment)
			{
				shipEquipment.RefreshCrewOverview();
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00052C2B File Offset: 0x00050E2B
		public float GetMiningBonus()
		{
			return 1f;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00052C32 File Offset: 0x00050E32
		public float GetSalvageBonus()
		{
			return 1f;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00052C39 File Offset: 0x00050E39
		public float GetBoardingPower()
		{
			return 1f;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00052C40 File Offset: 0x00050E40
		public float GetCombatBonus()
		{
			return 1f;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00052C48 File Offset: 0x00050E48
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<string, int> keyValuePair in this.crew)
			{
				jsonObject[keyValuePair.Key.ToString()] = new double?((double)keyValuePair.Value);
			}
			return new JsonObject
			{
				{
					"crew",
					jsonObject
				}
			};
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00052CDC File Offset: 0x00050EDC
		public static CrewData FromJson(JsonValue data)
		{
			CrewData crewData = new CrewData();
			if (data["crew"].AsJsonObject != null)
			{
				foreach (KeyValuePair<string, JsonValue> keyValuePair in data["crew"].AsJsonObject)
				{
					int num = (int)keyValuePair.Value.AsNumber;
					if (num > 0)
					{
						crewData.crew[keyValuePair.Key] = num;
					}
				}
			}
			return crewData;
		}
	}
}
