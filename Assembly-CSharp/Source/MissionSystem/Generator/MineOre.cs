using System;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D9 RID: 217
	public class MineOre : MissionGenerator
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x00041A34 File Offset: 0x0003FC34
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00041A38 File Offset: 0x0003FC38
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			AsteroidFieldData systemOreData = poi.system.systemOreData;
			TargetLayer? targetLayer2;
			TargetLayer targetLayer3;
			InventoryItemType inventoryItemType;
			if (difficulty == MissionDifficulty.Easy)
			{
				targetLayer2 = targetLayer;
				targetLayer3 = TargetLayer.Core;
				if (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)
				{
					inventoryItemType = systemOreData.coreOres.commonOre.item;
				}
				else
				{
					inventoryItemType = systemOreData.surfaceOres.commonOre.item;
					targetLayer = new TargetLayer?(TargetLayer.Surface);
				}
			}
			else if (difficulty == MissionDifficulty.Normal)
			{
				if (random.RandomBool(0.1f))
				{
					targetLayer2 = targetLayer;
					targetLayer3 = TargetLayer.Core;
					if (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)
					{
						inventoryItemType = systemOreData.coreOres.rareOre.item;
					}
					else
					{
						inventoryItemType = systemOreData.surfaceOres.rareOre.item;
						targetLayer = new TargetLayer?(TargetLayer.Surface);
					}
				}
				else
				{
					if (random.RandomBool(0.1f))
					{
						targetLayer2 = targetLayer;
						targetLayer3 = TargetLayer.Surface;
						if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
						{
							inventoryItemType = systemOreData.coreOres.commonOre.item;
							targetLayer = new TargetLayer?(TargetLayer.Core);
							goto IL_33E;
						}
					}
					targetLayer2 = targetLayer;
					targetLayer3 = TargetLayer.Core;
					if (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)
					{
						inventoryItemType = systemOreData.coreOres.commonOre.item;
					}
					else
					{
						inventoryItemType = systemOreData.surfaceOres.commonOre.item;
						targetLayer = new TargetLayer?(TargetLayer.Surface);
					}
				}
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				if (random.RandomBool(0.5f))
				{
					targetLayer2 = targetLayer;
					targetLayer3 = TargetLayer.Core;
					if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
					{
						goto IL_1A6;
					}
				}
				targetLayer2 = targetLayer;
				targetLayer3 = TargetLayer.Surface;
				if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
				{
					inventoryItemType = (SeededRandom.Global.RandomBool(0.5f) ? systemOreData.coreOres.commonOre.item : systemOreData.coreOres.rareOre.item);
					targetLayer = new TargetLayer?(TargetLayer.Core);
					goto IL_33E;
				}
				IL_1A6:
				inventoryItemType = systemOreData.surfaceOres.rareOre.item;
				targetLayer = new TargetLayer?(TargetLayer.Surface);
			}
			else if (difficulty == MissionDifficulty.Insane)
			{
				if (random.RandomBool(0.5f))
				{
					targetLayer2 = targetLayer;
					targetLayer3 = TargetLayer.Surface;
					if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
					{
						goto IL_251;
					}
				}
				targetLayer2 = targetLayer;
				targetLayer3 = TargetLayer.Core;
				if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
				{
					inventoryItemType = random.Choose<OreItemData>(systemOreData.surfaceOres.wildcardOres).item;
					targetLayer = new TargetLayer?(TargetLayer.Surface);
					goto IL_33E;
				}
				IL_251:
				inventoryItemType = systemOreData.coreOres.rareOre.item;
				targetLayer = new TargetLayer?(TargetLayer.Core);
			}
			else if (difficulty == MissionDifficulty.Skull)
			{
				targetLayer2 = targetLayer;
				targetLayer3 = TargetLayer.Surface;
				if (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)
				{
					inventoryItemType = random.Choose<OreItemData>(systemOreData.surfaceOres.wildcardOres).item;
				}
				else
				{
					inventoryItemType = random.Choose<OreItemData>(systemOreData.coreOres.wildcardOres).item;
					targetLayer = new TargetLayer?(TargetLayer.Core);
				}
			}
			else
			{
				AsteroidFieldData asteroidFieldData = systemOreData;
				bool surface;
				if (targetLayer != null)
				{
					targetLayer2 = targetLayer;
					targetLayer3 = TargetLayer.Surface;
					surface = (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null);
				}
				else
				{
					surface = true;
				}
				inventoryItemType = asteroidFieldData.GetRandomOre(surface, random).item;
				targetLayer3 = targetLayer.GetValueOrDefault();
				if (targetLayer == null)
				{
					targetLayer3 = TargetLayer.Surface;
					targetLayer = new TargetLayer?(targetLayer3);
				}
			}
			IL_33E:
			float num = this.GetRewardValue(poi.level) * 0.75f;
			int requiredAmount = Mathf.CeilToInt(num / (float)inventoryItemType.cost / 1.5f);
			string text = "";
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Both;
			if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
			{
				text = " " + Translation.Translate("@MineOreMissionTargetLayer", new object[]
				{
					targetLayer
				});
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@MineOreMissionName", new object[]
				{
					inventoryItemType.displayName
				}),
				category = Translation.Translate("@MineOreMission", Array.Empty<object>()),
				description = Translation.Translate("@MineOreMissionDesc", new object[]
				{
					inventoryItemType.displayName,
					text
				}),
				completionText = Translation.Translate("@MineOreMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				canBeIdled = this.canBeIdled
			};
			MissionStep missionStep = new MissionStep();
			missionStep.objectives.Add(new TradeOffer
			{
				itemType = inventoryItemType,
				requiredAmount = requiredAmount,
				deliverTo = (poi as SpaceStation),
				gameplayType = GameplayType.Mining,
				targetLayer = targetLayer
			});
			mission.steps.Add(missionStep);
			this.AddRewards(mission, num, difficulty, random, poi.faction);
			if (poi.system.GetPointOfInterest<Source.Galaxy.POI.Mining>() == null)
			{
				poi.system.AddMiningPoi(poi.faction, null, null, 0f, false, 0.5f);
			}
			return mission;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00041F1C File Offset: 0x0004011C
		public override GameplayType GetMissionType()
		{
			return GameplayType.Mining;
		}
	}
}
