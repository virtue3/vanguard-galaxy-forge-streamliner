using System;
using LightJson;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A7 RID: 167
	public abstract class MissionReward : IJsonSource
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060006CC RID: 1740
		public abstract string rewardText { get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x000395C4 File Offset: 0x000377C4
		public virtual Sprite rewardIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x000395C7 File Offset: 0x000377C7
		public virtual Color rewardColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060006CF RID: 1743
		public abstract void OnComplete(Mission m);

		// Token: 0x060006D0 RID: 1744
		public abstract void DataToJson(JsonObject data);

		// Token: 0x060006D1 RID: 1745
		public abstract void LoadFromJson(JsonObject data);

		// Token: 0x060006D2 RID: 1746 RVA: 0x000395D0 File Offset: 0x000377D0
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject
			{
				{
					"type",
					base.GetType().Name
				},
				{
					"hidden",
					new bool?(this.hidden)
				}
			};
			this.DataToJson(jsonObject);
			return jsonObject;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00039628 File Offset: 0x00037828
		public static MissionReward FromJson(JsonValue data)
		{
			MissionReward missionReward = MissionReward.Create(data["type"]);
			if (data["hidden"].IsBoolean)
			{
				missionReward.hidden = data["hidden"].AsBoolean;
			}
			missionReward.LoadFromJson(data);
			return missionReward;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00039689 File Offset: 0x00037889
		private static MissionReward Create(string name)
		{
			return (MissionReward)Type.GetType("Source.MissionSystem.Rewards." + name).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x040003C2 RID: 962
		public bool hidden;
	}
}
