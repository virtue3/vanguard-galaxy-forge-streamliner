using System;
using System.Collections.Generic;
using Behaviour.Dialogues;
using Behaviour.Equipment.Builder;
using Behaviour.Util;
using LightJson;
using Source.Data;
using Source.Dialogues;
using Source.Galaxy;
using Source.Item;
using Source.MissionSystem;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.SpaceShip;
using Source.Util;

namespace Source.Simulation.World.POI
{
	// Token: 0x0200007D RID: 125
	public class DistressCombat : PoiStoryteller
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x0002635E File Offset: 0x0002455E
		public DistressCombat(MapPointOfInterest poi) : base(poi)
		{
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00026368 File Offset: 0x00024568
		public override void UpdateActive(float deltaTime)
		{
			if (this.rewardClaimed)
			{
				return;
			}
			using (IEnumerator<AbstractUnitData> enumerator = this.poi.GetUnits(false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsPlayerEnemy())
					{
						return;
					}
				}
			}
			AbstractUnitData friend = null;
			foreach (AbstractUnitData abstractUnitData in this.poi.GetUnits(false))
			{
				if (!abstractUnitData.IsPlayerEnemy())
				{
					friend = abstractUnitData;
					break;
				}
			}
			if (friend != null)
			{
				SpaceShipData spaceShipData = friend as SpaceShipData;
				if (spaceShipData != null)
				{
					this.rewardClaimed = true;
					Character character = Character.CreateCharacter(spaceShipData.commanderData.firstName).WithPortret(spaceShipData.commanderData.sprite);
					Singleton<DialogueManager>.Instance.StartDialogue(new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Phew, that was a close call! Those pirates disabled our warp drive, they would've had us for lunch! Thanks for the help!"),
						DialogueLine.cDL(Characters.captain, "No worries. Now, did I hear you mention anything about a reward...?"),
						DialogueLine.cDL(character, "Err, yes, of course. Here you go.")
					}, delegate
					{
						SeededRandom global = SeededRandom.Global;
						Mission mission = new Mission
						{
							name = Translation.Translate(this.poi.name, Array.Empty<object>()),
							completionText = Translation.Translate("@DistressCombatCompletion", Array.Empty<object>()),
							difficulty = MissionDifficulty.Hard,
							sourcePoi = this.poi,
							sourceFaction = friend.faction,
							autoComplete = true
						};
						Rarity rarity = global.RandomBool(0.5f) ? Rarity.Enhanced : Rarity.HighGrade;
						mission.rewards.Add(new Item
						{
							item = global.Choose<EquipmentBuilder>(EquipmentBuilder.GetItemsForGeneralShop(this.poi.level)).CreateItemType(rarity, this.poi.level, true, global.RandomItemSeed(), false, false)
						});
						mission.rewards.Add(new Credits
						{
							amount = GameMath.GetCreditsValue(25f, this.poi.level)
						});
						mission.rewards.Add(new Reputation
						{
							faction = friend.faction,
							amount = 150
						});
						GamePlayer.current.AddMissionWithLog(mission);
					});
				}
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000264C0 File Offset: 0x000246C0
		public override void DataFromJson(JsonObject data)
		{
			this.rewardClaimed = data["rewardClaimed"];
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000264D8 File Offset: 0x000246D8
		public override void DataToJson(JsonObject data)
		{
			data["rewardClaimed"] = new bool?(this.rewardClaimed);
		}

		// Token: 0x04000277 RID: 631
		public bool rewardClaimed;
	}
}
