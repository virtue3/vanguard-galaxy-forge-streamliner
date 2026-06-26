using System;
using Behaviour.Weapons;
using LightJson;
using Source.Util;

namespace Source.Player
{
	// Token: 0x02000091 RID: 145
	public class AutopilotSettings : IJsonSource
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00031493 File Offset: 0x0002F693
		public int ammoSeconds
		{
			get
			{
				return this.ammoMinutes * 60;
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000314A0 File Offset: 0x0002F6A0
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"prioritizeHomestation",
					new bool?(this.prioritizeHomestation)
				},
				{
					"runMissions",
					new bool?(this.runMissions)
				},
				{
					"preferMissions",
					new bool?(this.preferMissions)
				},
				{
					"noTravel",
					new bool?(this.noTravel)
				},
				{
					"autoSell",
					new bool?(this.autoSell)
				},
				{
					"ammoMinutes",
					new double?((double)this.ammoMinutes)
				},
				{
					"gameplayType",
					this.preferredGameplayType.ToString()
				},
				{
					"targetLayer",
					this.preferredTargetLayer.ToString()
				}
			};
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000315A4 File Offset: 0x0002F7A4
		public static AutopilotSettings FromJson(JsonObject val)
		{
			AutopilotSettings autopilotSettings = new AutopilotSettings();
			autopilotSettings.prioritizeHomestation = val["prioritizeHomestation"].AsBoolean;
			autopilotSettings.runMissions = (!val.ContainsKey("runMissions") || val["runMissions"].AsBoolean);
			autopilotSettings.preferMissions = val["preferMissions"].AsBoolean;
			autopilotSettings.noTravel = val["noTravel"].AsBoolean;
			autopilotSettings.autoSell = val["autoSell"].AsBoolean;
			if (val["ammoMinutes"].IsNull)
			{
				autopilotSettings.ammoMinutes = 3;
			}
			else
			{
				autopilotSettings.ammoMinutes = val["ammoMinutes"].AsInteger;
			}
			GameplayType gameplayType;
			if (!val["gameplayType"].IsNull && Enum.TryParse<GameplayType>(val["gameplayType"], out gameplayType))
			{
				autopilotSettings.preferredGameplayType = gameplayType;
			}
			TargetLayer targetLayer;
			if (!val["targetLayer"].IsNull && Enum.TryParse<TargetLayer>(val["targetLayer"], out targetLayer))
			{
				autopilotSettings.preferredTargetLayer = targetLayer;
			}
			return autopilotSettings;
		}

		// Token: 0x040002CA RID: 714
		public const string LOADOUT_AUTODETECT = "Autodetect";

		// Token: 0x040002CB RID: 715
		public bool prioritizeHomestation = true;

		// Token: 0x040002CC RID: 716
		public bool runMissions = true;

		// Token: 0x040002CD RID: 717
		public bool preferMissions;

		// Token: 0x040002CE RID: 718
		public bool noTravel;

		// Token: 0x040002CF RID: 719
		public bool autoSell;

		// Token: 0x040002D0 RID: 720
		public int ammoMinutes = 3;

		// Token: 0x040002D1 RID: 721
		public GameplayType preferredGameplayType;

		// Token: 0x040002D2 RID: 722
		public TargetLayer preferredTargetLayer;
	}
}
