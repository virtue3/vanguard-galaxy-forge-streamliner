using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Data;
using Source.Galaxy;
using Source.Item;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000061 RID: 97
	public class AmbientEscortActions : DefenseSubjectActions
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x0001E16C File Offset: 0x0001C36C
		public AmbientEscortActions(AbstractUnit parent) : base(parent)
		{
			this.destinationTarget = BasePoiManager.current.GetComponentInChildren<EscortDestination>().transform.position;
			this.destinationTarget.x = this.destinationTarget.x - 3f;
			parent.SetOverrideDestination(this.destinationTarget, false, false, false);
			parent.maxSpeed = 0f;
			this.SetEnemyFaction();
			this.SetWaveIntervalAndStrength();
			this.escortActive = !Singleton<TravelManager>.Instance.TravelActive();
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001E218 File Offset: 0x0001C418
		public override void Update(float delta)
		{
			base.Update(delta);
			this.SetEngineSpeed();
			this.WaveSpawner();
			if (this.reachedDestination)
			{
				this.unloadTimer -= Time.deltaTime;
				if (this.unloadTimer < 0f && !this.unloadedAllCargo)
				{
					this.unloadTimer = this.unloadInterval;
					this.UnloadCargo();
					return;
				}
			}
			else if (Vector3.Distance(this.parent.rigidbody.position, this.destinationTarget) < 0.75f)
			{
				this.reachedDestination = true;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001E2AD File Offset: 0x0001C4AD
		public override void SpaceShipHasArrived()
		{
			this.escortActive = true;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
		private void SetEngineSpeed()
		{
			this.moveTimer -= Time.deltaTime;
			if (this.moveTimer < 0f)
			{
				if (this.escortActive)
				{
					this.parent.maxSpeed = 1.2f;
				}
				else
				{
					this.parent.maxSpeed = 0f;
				}
				this.moveTimer = 1f;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001E31C File Offset: 0x0001C51C
		private void WaveSpawner()
		{
			this.waveTimer -= Time.deltaTime;
			if (this.waveTimer < 0f)
			{
				if (!this.escortActive)
				{
					return;
				}
				if (!this.unloadedAllCargo)
				{
					MapPointOfInterest current = MapPointOfInterest.current;
					float pointsScale = this.waveStrength;
					Faction f = this.enemyFaction;
					List<AbstractUnitData> list = current.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), f, 0, 0, 1, 5, null);
					foreach (AbstractUnitData abstractUnitData in list)
					{
						abstractUnitData.alwaysHostile = true;
						abstractUnitData.noReputationLoss = this.hostileNoRepLoss;
						abstractUnitData.positionData.position = this.parent.transform.position + new Vector2(SeededRandom.Global.RandomRange(-10f, 10f), SeededRandom.Global.RandomRange(-4f, 4f));
					}
					MapPointOfInterest.current.AddTriggeredSpawn(list, this.waveInterval, 0, false, false);
				}
				this.waveTimer = this.waveInterval;
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001E44C File Offset: 0x0001C64C
		private void SetEnemyFaction()
		{
			if (this.mission != null)
			{
				ProtectUnit protectUnit = this.mission.currentStep.objectives.OfType<ProtectUnit>().FirstOrDefault<ProtectUnit>();
				if (protectUnit != null)
				{
					this.enemyFaction = protectUnit.enemyFaction;
					this.hostileNoRepLoss = protectUnit.hostileNoRepLoss;
				}
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001E498 File Offset: 0x0001C698
		private void SetWaveIntervalAndStrength()
		{
			if (this.mission == null)
			{
				this.waveTimer = this.waveInterval;
				return;
			}
			MissionDifficulty difficulty = this.mission.difficulty;
			SeededRandom global = SeededRandom.Global;
			if (difficulty == MissionDifficulty.Easy || difficulty == MissionDifficulty.Story)
			{
				this.waveInterval = 25f;
				this.waveStrength = global.RandomRange(0.3f, 0.35f);
				return;
			}
			if (difficulty == MissionDifficulty.Normal)
			{
				this.waveInterval = 22f;
				this.waveStrength = global.RandomRange(0.35f, 0.4f);
				return;
			}
			if (difficulty == MissionDifficulty.Hard)
			{
				this.waveInterval = 18f;
				this.waveStrength = global.RandomRange(0.4f, 0.5f);
				return;
			}
			if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				this.waveInterval = 16f;
				this.waveStrength = global.RandomRange(0.7f, 0.75f);
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001E570 File Offset: 0x0001C770
		private void UnloadCargo()
		{
			AbstractUnit parent = this.parent;
			Inventory.InventoryItem inventoryItem;
			if (parent == null)
			{
				inventoryItem = null;
			}
			else
			{
				AbstractUnitData unitData = parent.unitData;
				if (unitData == null)
				{
					inventoryItem = null;
				}
				else
				{
					Inventory cargo = unitData.cargo;
					if (cargo == null)
					{
						inventoryItem = null;
					}
					else
					{
						IEnumerable<Inventory.InventoryItem> items = cargo.items;
						if (items == null)
						{
							inventoryItem = null;
						}
						else
						{
							inventoryItem = items.FirstOrDefault(delegate(Inventory.InventoryItem i)
							{
								if (i == null)
								{
									return false;
								}
								InventoryItemType item = i.item;
								bool? flag = (item != null) ? new bool?(item.missionItem) : null;
								bool flag2 = true;
								return flag.GetValueOrDefault() == flag2 & flag != null;
							});
						}
					}
				}
			}
			Inventory.InventoryItem inventoryItem2 = inventoryItem;
			if (inventoryItem2 != null)
			{
				if (this.parent.unitData.cargo.Remove(inventoryItem2, 1))
				{
					Singleton<LootManager>.Instance.CreateLootItem(this.parent.transform.position, inventoryItem2.item, 1, this.parent.faction, true);
					MissionObjective.Trigger(MissionTrigger.EscortUnitCargoUnloaded, null, null, false);
				}
				return;
			}
			MissionObjective.Trigger(MissionTrigger.UnitProtected, null, null, false);
			this.unloadedAllCargo = true;
			this.OnEscortComplete();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001E642 File Offset: 0x0001C842
		public void OnEscortComplete()
		{
			base.StartExitCoroutine();
		}

		// Token: 0x04000210 RID: 528
		private Faction enemyFaction = Faction.marauders;

		// Token: 0x04000211 RID: 529
		public const float MaxSpeed = 1.2f;

		// Token: 0x04000212 RID: 530
		public Vector2 destinationTarget;

		// Token: 0x04000213 RID: 531
		private bool reachedDestination;

		// Token: 0x04000214 RID: 532
		private bool unloadedAllCargo;

		// Token: 0x04000215 RID: 533
		private float unloadTimer;

		// Token: 0x04000216 RID: 534
		private float unloadInterval = 3f;

		// Token: 0x04000217 RID: 535
		private float waveTimer;

		// Token: 0x04000218 RID: 536
		private float waveInterval = 3f;

		// Token: 0x04000219 RID: 537
		private float waveStrength = 0.5f;

		// Token: 0x0400021A RID: 538
		private float moveTimer;

		// Token: 0x0400021B RID: 539
		private bool escortActive;

		// Token: 0x0400021C RID: 540
		private bool hostileNoRepLoss;
	}
}
