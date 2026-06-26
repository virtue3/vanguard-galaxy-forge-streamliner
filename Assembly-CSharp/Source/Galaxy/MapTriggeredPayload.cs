using System;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.Unit;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Player;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000148 RID: 328
	public class MapTriggeredPayload : IJsonSource
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x00059565 File Offset: 0x00057765
		public MapTriggeredPayload(MapPointOfInterest parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0005958C File Offset: 0x0005778C
		public bool Update(float delta)
		{
			if (this.UpdateTimer())
			{
				BasePoiManager current = BasePoiManager.current;
				if (current != null && current.initializedAndReady)
				{
					this.triggerTime -= delta;
					if (this.triggerTime < 0f)
					{
						this.TriggerPayload();
						return true;
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000595DD File Offset: 0x000577DD
		private bool UpdateTimer()
		{
			return this.triggerSequence <= 0 && (!this.waitForNoEnemies || !this.EnemiesStillPresent());
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00059600 File Offset: 0x00057800
		private bool EnemiesStillPresent()
		{
			foreach (AbstractUnitData abstractUnitData in this.parent.GetUnits(false))
			{
				if (abstractUnitData.IsPlayerEnemy() && !(abstractUnitData is CombatStationPartData))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00059664 File Offset: 0x00057864
		public bool HasEnemyReinforcements()
		{
			if (!this.UpdateTimer())
			{
				return false;
			}
			if (this.waitForNoEnemies && this.EnemiesStillPresent())
			{
				return false;
			}
			foreach (AbstractUnitData abstractUnitData in this.units)
			{
				if (abstractUnitData.IsPlayerEnemy() && abstractUnitData.IsCombatant())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x000596E4 File Offset: 0x000578E4
		public void TriggerPayload()
		{
			foreach (AbstractUnitData abstractUnitData in this.units)
			{
				if (this.spawnAtPlayer)
				{
					abstractUnitData.positionData.position += GamePlayer.current.currentSpaceShip.positionData.position;
				}
				SpaceShip spaceShip = this.parent.AddUnit(abstractUnitData, null, false) as SpaceShip;
				if (spaceShip != null)
				{
					Vector2 position = abstractUnitData.positionData.position;
					if (this.spawnAtPlayer)
					{
						float num = (float)((SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * SeededRandom.Global.RandomRange(6, 15));
						float num2 = (float)((SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * SeededRandom.Global.RandomRange(8, 20));
						position.y = GamePlayer.current.currentSpaceShip.positionData.position.y + num;
						position.x = GamePlayer.current.currentSpaceShip.positionData.position.x + num2;
					}
					else
					{
						float num3 = (float)SeededRandom.Global.RandomRange(15, 25);
						float num4 = (float)SeededRandom.Global.RandomRange(1, 3);
						position.y += ((position.y > MapPointOfInterest.current.GetWorldPosition().y) ? (-num3) : num3);
						position.x += (SeededRandom.Global.RandomBool(0.5f) ? (-num4) : num4);
					}
					spaceShip.StartLandNpcAtPoiCoroutine(position);
				}
			}
			foreach (PersistableData persistableData in this.persistables)
			{
				if (this.spawnAtPlayer)
				{
					persistableData.position += GamePlayer.current.currentSpaceShip.positionData.position;
				}
				this.parent.AddPersistable(persistableData);
			}
			foreach (MapTriggeredPayload mapTriggeredPayload in this.parent.GetTriggeredPayloads())
			{
				if (mapTriggeredPayload.triggerSequence > 0)
				{
					mapTriggeredPayload.triggerSequence--;
				}
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00059998 File Offset: 0x00057B98
		public void AddUnit(AbstractUnitData unit)
		{
			this.units.Add(unit);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000599A6 File Offset: 0x00057BA6
		public void AddPersistable(PersistableData data)
		{
			this.persistables.Add(data);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000599B4 File Offset: 0x00057BB4
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"triggerSequence",
					new double?((double)this.triggerSequence)
				},
				{
					"waitForNoEnemies",
					new bool?(this.waitForNoEnemies)
				},
				{
					"triggerTime",
					new double?((double)this.triggerTime)
				},
				{
					"spawnAtPlayer",
					new bool?(this.spawnAtPlayer)
				},
				{
					"persistables",
					this.persistables.ToJsonArray<PersistableData>()
				},
				{
					"units",
					this.units.ToJsonArray<AbstractUnitData>()
				}
			};
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00059A78 File Offset: 0x00057C78
		public static MapTriggeredPayload FromJson(MapPointOfInterest parent, JsonValue data)
		{
			MapTriggeredPayload mapTriggeredPayload = new MapTriggeredPayload(parent);
			mapTriggeredPayload.triggerSequence = data["triggerSequence"];
			mapTriggeredPayload.waitForNoEnemies = data["waitForNoEnemies"];
			mapTriggeredPayload.triggerTime = (float)data["triggerTime"].AsNumber;
			mapTriggeredPayload.spawnAtPlayer = data["spawnAtPlayer"];
			mapTriggeredPayload.persistables.FromJsonArray(data["persistables"], new ClassExtensions.ParseJsonValue<PersistableData>(PersistableData.FromJson));
			mapTriggeredPayload.units.FromJsonArray(data["units"], new ClassExtensions.ParseJsonValue<AbstractUnitData>(AbstractUnitData.FromJson));
			return mapTriggeredPayload;
		}

		// Token: 0x040006EE RID: 1774
		public readonly MapPointOfInterest parent;

		// Token: 0x040006EF RID: 1775
		public float triggerTime;

		// Token: 0x040006F0 RID: 1776
		public int triggerSequence;

		// Token: 0x040006F1 RID: 1777
		public bool waitForNoEnemies;

		// Token: 0x040006F2 RID: 1778
		public bool spawnAtPlayer;

		// Token: 0x040006F3 RID: 1779
		public readonly List<PersistableData> persistables = new List<PersistableData>();

		// Token: 0x040006F4 RID: 1780
		public readonly List<AbstractUnitData> units = new List<AbstractUnitData>();
	}
}
