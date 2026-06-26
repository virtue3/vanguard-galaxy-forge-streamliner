using System;
using Behaviour.Equipment.Builder;
using Behaviour.Unit;
using LightJson;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;

namespace Source.Simulation
{
	// Token: 0x02000070 RID: 112
	public abstract class Storyteller : IJsonSource
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0001FF35 File Offset: 0x0001E135
		public string identifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001FF42 File Offset: 0x0001E142
		public virtual int maxPlayerLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001FF45 File Offset: 0x0001E145
		public virtual int maxReputation
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0001FF48 File Offset: 0x0001E148
		public virtual int maxBonusSkillpoints
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001FF4B File Offset: 0x0001E14B
		public virtual int maxLootBoxSkillPoints
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001FF4E File Offset: 0x0001E14E
		public Storyteller(GamePlayer ply)
		{
			this.player = ply;
		}

		// Token: 0x0600040D RID: 1037
		public abstract void SetupNewGame();

		// Token: 0x0600040E RID: 1038
		public abstract void Start();

		// Token: 0x0600040F RID: 1039
		public abstract void StoryUpdate(float delta);

		// Token: 0x06000410 RID: 1040 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual TravelDynamicEvent TriggerDynamicEvent()
		{
			return null;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001FF60 File Offset: 0x0001E160
		public virtual void Cleanup()
		{
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001FF62 File Offset: 0x0001E162
		public virtual void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001FF64 File Offset: 0x0001E164
		public virtual void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001FF66 File Offset: 0x0001E166
		public virtual void OnStoryMissionComplete(string storyId)
		{
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001FF68 File Offset: 0x0001E168
		public virtual void AddItemsToShop(SpaceStation parent, ShopInventory shop)
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001FF6A File Offset: 0x0001E16A
		public virtual bool ItemIsPlayerAvailable(EquipmentBuilder item)
		{
			return true;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001FF6D File Offset: 0x0001E16D
		public virtual bool ShipIsPlayerAvailable(Behaviour.Unit.SpaceShip ss)
		{
			return true;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001FF70 File Offset: 0x0001E170
		public static Storyteller FromJson(JsonValue data, GamePlayer player)
		{
			string str;
			if (data.IsString)
			{
				str = data;
			}
			else
			{
				str = data["identifier"];
			}
			Storyteller storyteller = (Storyteller)Type.GetType("Source.Simulation.Story." + str).GetConstructor(new Type[]
			{
				typeof(GamePlayer)
			}).Invoke(new object[]
			{
				player
			});
			if (data.IsJsonObject)
			{
				storyteller.DataFromJson(data);
			}
			return storyteller;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001FFF4 File Offset: 0x0001E1F4
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			this.DataToJson(jsonObject);
			jsonObject["identifier"] = this.identifier;
			return jsonObject;
		}

		// Token: 0x04000242 RID: 578
		public readonly GamePlayer player;
	}
}
