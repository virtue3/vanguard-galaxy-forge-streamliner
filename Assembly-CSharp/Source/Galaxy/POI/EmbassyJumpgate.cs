using System;
using Source.Player;

namespace Source.Galaxy.POI
{
	// Token: 0x02000152 RID: 338
	public class EmbassyJumpgate : JumpGate
	{
		// Token: 0x06000CF6 RID: 3318 RVA: 0x0005D4A8 File Offset: 0x0005B6A8
		public void LinkJumpgate(EmbassyJumpgate other, bool unlock)
		{
			this.targetSystemGuid = other.system.guid;
			this.targetPoiGuid = other.guid;
			this.hidden = false;
			other.targetSystemGuid = this.system.guid;
			other.targetPoiGuid = base.guid;
			other.hidden = false;
			if (Faction.player.IsEnemy(this.faction) || (other.faction == Faction.darkspacers && GamePlayer.current.level < 55))
			{
				this.jumpgateOpen = false;
				EmbassyJumpgate embassyJumpgate = base.GetTargetPOI() as EmbassyJumpgate;
				if (embassyJumpgate != null)
				{
					embassyJumpgate.jumpgateOpen = false;
					return;
				}
			}
			else if (unlock)
			{
				base.UnlockJumpgate();
			}
		}
	}
}
