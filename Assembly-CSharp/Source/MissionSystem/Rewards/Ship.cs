using System;
using Behaviour.Unit;
using LightJson;
using Source.Player;
using Source.SpaceShip;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000BA RID: 186
	public class Ship : MissionReward
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0003DD51 File Offset: 0x0003BF51
		public override string rewardText
		{
			get
			{
				return this.ship.displayName;
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0003DD5E File Offset: 0x0003BF5E
		public override void DataToJson(JsonObject data)
		{
			data["ship"] = this.ship.identifier;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0003DD7C File Offset: 0x0003BF7C
		public override void LoadFromJson(JsonObject data)
		{
			this.ship = data["ship"].AsString;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0003DDA8 File Offset: 0x0003BFA8
		public override void OnComplete(Mission m)
		{
			SpaceShipData item = new SpaceShipData(this.ship, true, null);
			GamePlayer.current.spaceShips.Add(item);
		}

		// Token: 0x04000449 RID: 1097
		public Behaviour.Unit.SpaceShip ship;
	}
}
