using System;
using System.Collections.Generic;
using Behaviour.GalaxyMap;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Data;
using Source.Data.Persistable;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI
{
	// Token: 0x02000150 RID: 336
	public class CombatStation : Combat
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0005D16D File Offset: 0x0005B36D
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("CombatStation");
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0005D17C File Offset: 0x0005B37C
		public override bool hasCombatMusic
		{
			get
			{
				foreach (PersistableData persistableData in base.GetPersistables())
				{
					CombatStationData combatStationData = persistableData as CombatStationData;
					if (combatStationData != null)
					{
						foreach (CombatStationPartData combatStationPartData in combatStationData.stationParts)
						{
							if (combatStationPartData.currentHullHP > 0f && combatStationPartData.IsPlayerEnemy())
							{
								return true;
							}
						}
					}
				}
				return base.hasCombatMusic;
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0005D224 File Offset: 0x0005B424
		public override bool CanTravelHere()
		{
			if (this.system.storyteller is PirateHideout)
			{
				bool flag = true;
				foreach (MapPointOfInterest mapPointOfInterest in this.system.allPointsOfInterest)
				{
					if (mapPointOfInterest != this && mapPointOfInterest is Combat)
					{
						using (IEnumerator<AbstractUnitData> enumerator2 = mapPointOfInterest.GetUnits(false).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.IsPlayerEnemy())
								{
									flag = false;
									break;
								}
							}
						}
						using (IEnumerator<MapTriggeredPayload> enumerator3 = mapPointOfInterest.GetTriggeredPayloads().GetEnumerator())
						{
							if (enumerator3.MoveNext())
							{
								MapTriggeredPayload mapTriggeredPayload = enumerator3.Current;
								flag = false;
							}
						}
					}
					if (!flag)
					{
						break;
					}
				}
				if (!flag)
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@MapPOIDefeatPatrols", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				}
				return flag;
			}
			return true;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0005D348 File Offset: 0x0005B548
		protected override string GenerateDefaultName()
		{
			return this.faction.GenerateStationName(this);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0005D358 File Offset: 0x0005B558
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			FactionSkirmish factionSkirmish = this.system.storyteller as FactionSkirmish;
			Color color;
			if (((factionSkirmish != null) ? factionSkirmish.helpingFaction : null) != null && factionSkirmish.helpingFaction != this.faction)
			{
				color = ColorHelper.reddish;
			}
			else
			{
				color = this.faction.relationColor;
			}
			tooltip.AddTextLine(this.faction.name, 12, 8f).Text.color = color;
			if (((factionSkirmish != null) ? factionSkirmish.helpingFaction : null) != null && factionSkirmish.helpingFaction != this.faction)
			{
				tooltip.AddTextLine(Translation.Translate("@FactionSkirmishStrength", new object[]
				{
					GameMath.FormatPercentage((float)factionSkirmish.enemyStrength / 5f, FormatPercentageMode.Default, 1)
				}), 12, 8f);
			}
			if (factionSkirmish == null || factionSkirmish.helpingFaction != this.faction)
			{
				base.AddTooltipInfo(tooltip);
			}
		}
	}
}
