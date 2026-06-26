using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Equipment.Module.DroneBay;
using Behaviour.Equipment.Module.DroneBay.OpeningMechanism;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Drone;
using Source.Galaxy;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000361 RID: 865
	public class DroneBayModule : AbstractModule
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x000BFE4C File Offset: 0x000BE04C
		public int droneAmount
		{
			get
			{
				int num = this._droneAmount;
				if (this.CanApplyDroneAmountSkill())
				{
					if (SkilltreeNode.dronesAmount.isActive)
					{
						num++;
					}
					if (SkilltreeNode.dronesAmount2.isActive)
					{
						num++;
					}
				}
				return num + this.droneBonusAmount;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060020D9 RID: 8409 RVA: 0x000BFE90 File Offset: 0x000BE090
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.DroneBay;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x000BFE94 File Offset: 0x000BE094
		public float rebuildTime
		{
			get
			{
				float num = 20f;
				if (base.IsPlayer(true))
				{
					num *= 1f - (SkilltreeNode.dronesRebuildIncrease.currentIncrease + SkilltreeNode.dronesRebuildIncrease2.currentIncrease);
				}
				return num;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060020DB RID: 8411 RVA: 0x000BFED0 File Offset: 0x000BE0D0
		public float transitionDuration
		{
			get
			{
				float num = 1.5f;
				if (base.IsPlayer(true))
				{
					num *= 1f - SkilltreeNode.dronesFasterDeploy.currentIncrease;
				}
				return num;
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000BFF00 File Offset: 0x000BE100
		protected override void Awake()
		{
			base.Awake();
			this.doorMechanism = base.parent.GetComponentInChildren<DoorMechanism>();
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000BFF19 File Offset: 0x000BE119
		protected override void Start()
		{
			base.Start();
			if (!base.parent.inShipYard)
			{
				this.InitializeDrones();
				base.StartCoroutine(this.CheckIfDoorsShouldOpen());
			}
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000BFF44 File Offset: 0x000BE144
		protected override void Update()
		{
			base.Update();
			if ((this.deploying || this.returning) && base.parent.travelling)
			{
				this.CancelDeployReturn();
			}
			foreach (Drone drone in this.drones)
			{
				if (drone.rebuildTimer > 0f)
				{
					drone.rebuildTimer -= Time.deltaTime;
				}
			}
			this.CheckTimedTriggers();
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000BFFE0 File Offset: 0x000BE1E0
		private bool CanApplyDroneAmountSkill()
		{
			return base.IsPlayer(true) || !base.parent || base.parent.inShipYard;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000C0008 File Offset: 0x000BE208
		private void CheckTimedTriggers()
		{
			if (this.triggerTimer < 0f)
			{
				this.triggerTimer = 0.5f;
				if (base.parent == GameplayManager.Instance.spaceShip)
				{
					this.MissionTriggerDroneBay();
				}
				for (int i = 0; i < this.drones.Count; i++)
				{
					if (!this.drones[i])
					{
						this.drones.RemoveAt(i);
						i--;
					}
				}
				if (this.drones.Count > this.droneAmount)
				{
					for (int j = this.drones.Count - 1; j >= this.droneAmount; j--)
					{
						Drone drone = this.drones[j];
						if (drone)
						{
							drone.DetachFromTarget();
							UnityEngine.Object.Destroy(drone.gameObject);
						}
						this.drones.RemoveAt(j);
					}
				}
				if (this.drones.Count < this.droneAmount && !base.parent.inShipYard)
				{
					this.RebuildDrone();
					return;
				}
			}
			else
			{
				this.triggerTimer -= Time.deltaTime;
			}
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000C0124 File Offset: 0x000BE324
		public float GetRebuildProgress()
		{
			float num = this.rebuildTime;
			foreach (Drone drone in this.drones)
			{
				if (drone.rebuildTimer > 0f)
				{
					num = ((drone.rebuildTimer < num) ? drone.rebuildTimer : num);
				}
			}
			return (this.rebuildTime - num) / this.rebuildTime;
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000C01A8 File Offset: 0x000BE3A8
		private void OnDestroy()
		{
			if (GamePlayer.current == null)
			{
				return;
			}
			foreach (Drone drone in this.drones)
			{
				if (drone && Singleton<LootManager>.Instance)
				{
					Singleton<LootManager>.Instance.DropLoot(drone, null, false);
					if (drone.rigidbody != null && drone.gameObject.activeSelf)
					{
						EffectManager instance = Singleton<EffectManager>.Instance;
						if (instance != null)
						{
							instance.PlayExplosionEffect(drone.transform.position, drone.rigidbody.linearVelocity, 1f, ColorHelper.flashExplosionUnit, 0f);
						}
					}
					UnityEngine.Object.Destroy(drone.gameObject);
				}
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000C0284 File Offset: 0x000BE484
		public float GetDronePower()
		{
			if (this.dronePower == 0f)
			{
				Drone drone = this.CreateAndInitializeDrone(0);
				InventoryItemType inventoryItemType = drone.unitData.hardpoints[0];
				if (inventoryItemType)
				{
					this.dronePower = inventoryItemType.GetComponent<AbstractTurret>().displayedPower * (float)this._droneAmount;
				}
				drone.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(drone.gameObject);
			}
			return this.dronePower;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000C02F0 File Offset: 0x000BE4F0
		private void InitializeDrones()
		{
			for (int i = 0; i < this.droneAmount; i++)
			{
				this.AddNewDrone(i);
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000C0318 File Offset: 0x000BE518
		public void ForceResetDrones()
		{
			foreach (Drone drone in this.drones)
			{
				UnityEngine.Object.Destroy(drone.gameObject);
			}
			this.drones.Clear();
			this.InitializeDrones();
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000C0380 File Offset: 0x000BE580
		private void AddNewDrone(int idx)
		{
			Drone drone = this.CreateAndInitializeDrone(idx);
			drone.gameObject.SetActive(false);
			this.drones.Add(drone);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000C03B0 File Offset: 0x000BE5B0
		private Drone GetDronePrefab(int idx, AbstractUnitData parent = null)
		{
			if (parent == null)
			{
				AbstractUnit parent2 = base.parent;
				parent = ((parent2 != null) ? parent2.unitData : null);
			}
			SeededRandom seededRandom = new SeedGenerator().Add(this.droneSeed).Add("DronePrefab").Add(idx).CreateRandom();
			if (parent != null)
			{
				return parent.GetDroneLoadout(idx, seededRandom);
			}
			return seededRandom.Choose<Drone>(GamePlayer.current.GetUnlockedDrones());
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000C041C File Offset: 0x000BE61C
		private Drone CreateAndInitializeDrone(int idx)
		{
			Drone dronePrefab = this.GetDronePrefab(idx, null);
			Transform transform = base.parent ? base.parent.transform : PersistentSingleton<GameManager>.Instance.equipmentBuilderRoot;
			Drone drone = UnityEngine.Object.Instantiate<Drone>(dronePrefab, transform.position, transform.rotation, transform.parent);
			drone.dronePrefab = dronePrefab;
			DroneData droneData = new DroneData(drone, dronePrefab);
			AbstractUnitData abstractUnitData = droneData;
			AbstractUnit parent = base.parent;
			abstractUnitData.faction = ((parent != null) ? parent.faction : null);
			droneData.LoadDefaultEquipment(base.item.itemLevel, -1f, this.droneSeed, null, null, delegate(EquipmentSlot slot)
			{
				if (slot != EquipmentSlot.Hardpoint)
				{
					return Rarity.Standard;
				}
				return base.item.rarity;
			}, false, null);
			drone.droneIndex = idx;
			drone.SetCommander(base.parent);
			drone.SetData(droneData, true, true);
			drone.InitModules();
			droneData.GiveAmmo(1000000, false);
			if (base.parent != null)
			{
				droneData.playerHostile = base.parent.unitData.playerHostile;
				droneData.playerFriendly = base.parent.unitData.playerFriendly;
				droneData.alwaysFriendly = base.parent.unitData.alwaysFriendly;
				droneData.alwaysHostile = base.parent.unitData.alwaysHostile;
			}
			drone.SetEngineState(false, true);
			drone.ToggleWeapons(false);
			return drone;
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000C057F File Offset: 0x000BE77F
		private IEnumerator ToggleDoors(bool open)
		{
			DoorMechanism doorMechanism = this.doorMechanism;
			yield return (doorMechanism != null) ? doorMechanism.ToggleDoors(open) : null;
			yield break;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000C0598 File Offset: 0x000BE798
		public void CancelDeployReturn()
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.ToggleDoors(false));
			this.shouldDeploy = false;
			this.deploying = false;
			this.returning = false;
			List<Drone> list = new List<Drone>();
			foreach (Drone drone in this.drones)
			{
				if (drone.gameObject.activeSelf)
				{
					list.Add(drone);
				}
			}
			foreach (Drone drone2 in list)
			{
				this.RemoveDrone(drone2);
				UnityEngine.Object.Destroy(drone2.gameObject);
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000C0670 File Offset: 0x000BE870
		public void DeployDrones()
		{
			if (this.deploying || this.returning || base.parent.travelling || base.parent.inShipYard)
			{
				return;
			}
			this.shouldDeploy = true;
			this.deploying = true;
			base.StartCoroutine(this.CoroutineDeployDrones());
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000C06C3 File Offset: 0x000BE8C3
		private IEnumerator CoroutineDeployDrones()
		{
			yield return this.ToggleDoors(true);
			foreach (Drone drone in new List<Drone>(this.drones))
			{
				if (this.CanDeploy(drone) && this.shouldDeploy)
				{
					yield return this.DeployDrone(drone);
				}
			}
			List<Drone>.Enumerator enumerator = default(List<Drone>.Enumerator);
			this.deploying = false;
			yield break;
			yield break;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000C06D2 File Offset: 0x000BE8D2
		private IEnumerator DeployDrone(Drone drone)
		{
			this.deploying = true;
			SpriteRenderer spriteRenderer = drone.GetComponent<SpriteRenderer>();
			Color transparentColor = spriteRenderer.color;
			transparentColor.a = 0f;
			spriteRenderer.color = transparentColor;
			yield return new WaitForSeconds(this.transitionDuration);
			if (!drone)
			{
				yield break;
			}
			drone.transform.position = this.doorMechanism.transform.position;
			drone.gameObject.SetActive(true);
			drone.isDeployed = true;
			yield return null;
			if (drone.cargoIndicator)
			{
				drone.cargoIndicator.SetActive(false);
			}
			Vector3 initialScale = new Vector3(0.75f, 0.75f, 1f);
			Vector3 targetScale = Vector3.one;
			drone.transform.localScale = initialScale;
			Vector3 right = base.parent.transform.right;
			drone.transform.rotation = Quaternion.FromToRotation(drone.transform.right, right) * drone.transform.rotation;
			float elapsedTime = 0f;
			while (elapsedTime < this.transitionDuration && drone)
			{
				float t = elapsedTime / this.transitionDuration;
				Color color = spriteRenderer.color;
				color.a = Mathf.Lerp(0f, 1f, t);
				spriteRenderer.color = color;
				drone.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
				drone.rigidbody.position = this.doorMechanism.transform.position;
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			if (!drone)
			{
				yield break;
			}
			spriteRenderer.color = new Color(transparentColor.r, transparentColor.g, transparentColor.b, 1f);
			drone.transform.localScale = targetScale;
			if (drone.cargoIndicator)
			{
				drone.cargoIndicator.SetActive(true);
			}
			this.SetDroneLocation(drone);
			if (!this.AreDronesDocked())
			{
				this.deploying = false;
			}
			drone.SetRigidbodyState(true);
			drone.SetEngineState(true, true);
			drone.ToggleWeapons(true);
			drone.docking = false;
			this.ManageClosingDroneBayDoors();
			yield break;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000C06E8 File Offset: 0x000BE8E8
		private IEnumerator CheckIfDoorsShouldOpen()
		{
			if (base.parent.travelling)
			{
				yield return new WaitUntil(() => !base.parent.travelling);
			}
			if (this.AreDronesReturning())
			{
				base.StartCoroutine(this.ToggleDoors(true));
			}
			yield return new WaitForSeconds(0.5f);
			base.StartCoroutine(this.CheckIfDoorsShouldOpen());
			yield break;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000C06F8 File Offset: 0x000BE8F8
		private void SetDroneLocation(Drone drone)
		{
			drone.rigidbody.position = this.doorMechanism.transform.position;
			drone.transform.Z(ZIndex.Drone);
			float d = 2f;
			Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
			Vector3 v = base.parent.rigidbody.position + normalized * d;
			drone.SetOverrideDestination(v, true, false, false);
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000C0776 File Offset: 0x000BE976
		private bool CanDeploy(Drone drone)
		{
			return !drone.isDeployed && drone.rebuildTimer <= 0f;
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000C0792 File Offset: 0x000BE992
		public bool AllDronesDocked()
		{
			return this.drones.All((Drone d) => !d.isDeployed);
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000C07BE File Offset: 0x000BE9BE
		public bool AreDronesDocked()
		{
			return this.drones.Any(new Func<Drone, bool>(this.CanDeploy));
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000C07D7 File Offset: 0x000BE9D7
		public bool AreDronesReturning()
		{
			return this.drones.Any((Drone d) => d.isReturning);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000C0803 File Offset: 0x000BEA03
		public bool AreDronesDeployed()
		{
			return this.drones.Any((Drone d) => d.isDeployed);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000C082F File Offset: 0x000BEA2F
		private void ManageClosingDroneBayDoors()
		{
			if (this.AreDronesReturning())
			{
				return;
			}
			if (this.deploying)
			{
				return;
			}
			if (!this.AreDronesReturning())
			{
				base.StartCoroutine(this.ToggleDoors(false));
				this.returning = false;
			}
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000C0860 File Offset: 0x000BEA60
		public Vector2 GetDockingPosition()
		{
			return this.doorMechanism ? this.doorMechanism.transform.position : base.transform.position;
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000C0891 File Offset: 0x000BEA91
		public void DroneWantsToReturn(Drone drone)
		{
			if (!this.returning)
			{
				base.StartCoroutine(this.ToggleDoors(true));
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000C08AC File Offset: 0x000BEAAC
		public void ReturnDrones()
		{
			if (this.deploying)
			{
				base.StopAllCoroutines();
				this.deploying = false;
			}
			if (this.returning)
			{
				return;
			}
			if (this.AllDronesDocked())
			{
				return;
			}
			base.StartCoroutine(this.ToggleDoors(true));
			this.shouldDeploy = false;
			this.returning = true;
			foreach (Drone drone in this.drones)
			{
				if (drone.isDeployed && !drone.isReturning)
				{
					drone.DockWithCommander();
				}
			}
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000C0954 File Offset: 0x000BEB54
		public IEnumerator TryDockingDrone(Drone drone)
		{
			if (!this.drones.Contains(drone))
			{
				Debug.Log("drones does not contain: " + ((drone != null) ? drone.ToString() : null));
				yield break;
			}
			while (!this.doorMechanism.doorOpen)
			{
				yield return null;
			}
			this.ManageClosingDroneBayDoors();
			yield return this.DockingProcedure(drone);
			yield break;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000C096A File Offset: 0x000BEB6A
		private IEnumerator DockingProcedure(Drone drone)
		{
			drone.SetRigidbodyState(false);
			SpriteRenderer spriteRenderer = drone.GetComponent<SpriteRenderer>();
			if (drone.cargoIndicator)
			{
				drone.cargoIndicator.SetActive(false);
			}
			Color initialColor = spriteRenderer.color;
			Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
			Vector3 initialScale = drone.transform.localScale;
			Vector3 targetScale = new Vector3(0.75f, 0.75f, 1f);
			drone.SetEngineState(false, true);
			drone.ToggleWeapons(false);
			Vector3 right = base.parent.transform.right;
			drone.transform.rotation = Quaternion.FromToRotation(drone.transform.right, right) * drone.transform.rotation;
			float transitionDuration = 1.5f;
			float elapsedTime = 0f;
			while (elapsedTime < transitionDuration)
			{
				float t = elapsedTime / transitionDuration;
				spriteRenderer.color = Color.Lerp(initialColor, targetColor, t);
				drone.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
				drone.rigidbody.position = this.doorMechanism.transform.position;
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			spriteRenderer.color = targetColor;
			drone.transform.localScale = targetScale;
			drone.rigidbody.position = this.doorMechanism.transform.position;
			this.HandleDroneReturn(drone);
			this.ManageClosingDroneBayDoors();
			base.StartCoroutine(this.UnloadDroneCargo(drone, ItemCategory.Ore));
			yield break;
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000C0980 File Offset: 0x000BEB80
		private void HandleDroneReturn(Drone drone)
		{
			drone.SetAutonomousBehavior();
			drone.SetEngineState(false, true);
			drone.ToggleWeapons(false);
			drone.isDeployed = false;
			drone.isReturning = false;
			drone.gameObject.SetActive(false);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000C09B1 File Offset: 0x000BEBB1
		private IEnumerator UnloadDroneCargo(Drone drone, ItemCategory type)
		{
			if (this.IsDroneCargoEmpty(drone))
			{
				yield break;
			}
			List<Inventory.InventoryItem> itemsToTransfer = this.GetItemsToTransfer(drone, type);
			foreach (Inventory.InventoryItem inventoryItem in itemsToTransfer)
			{
				if (inventoryItem.item.itemCategory != ItemCategory.Ammo)
				{
					this.TransferCargo(drone, inventoryItem.item, inventoryItem.count);
					yield return new WaitForSeconds((float)inventoryItem.count * 0.2f);
				}
			}
			List<Inventory.InventoryItem>.Enumerator enumerator = default(List<Inventory.InventoryItem>.Enumerator);
			drone.cantFitMoreCargo = false;
			Debug.Log("Unloaded cargo, handle post");
			yield return this.HandlePostUnloadBehavior(drone);
			yield break;
			yield break;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000C09D0 File Offset: 0x000BEBD0
		private bool IsDroneCargoEmpty(Drone drone)
		{
			AbstractUnitData unitData = drone.unitData;
			int? num;
			if (unitData == null)
			{
				num = null;
			}
			else
			{
				Inventory cargo = unitData.cargo;
				num = ((cargo != null) ? new int?(cargo.count) : null);
			}
			int? num2 = num;
			return num2.GetValueOrDefault() == 0;
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000C0A1C File Offset: 0x000BEC1C
		private List<Inventory.InventoryItem> GetItemsToTransfer(Drone drone, ItemCategory type)
		{
			return (from cargo in drone.unitData.cargo.items
			where cargo.item.itemCategory == type && cargo.count > 0
			select cargo).ToList<Inventory.InventoryItem>();
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000C0A5C File Offset: 0x000BEC5C
		private void TransferCargo(Drone drone, InventoryItemType item, int amount)
		{
			drone.unitData.RemoveCargo(item, amount, false);
			base.parent.unitData.AddCargo(item, amount, false);
			if (amount > 0)
			{
				MissionObjective.Trigger(MissionTrigger.ItemCollected, new ValueTuple<TractorableItemData, AbstractUnitData>(new TractorableItemData
				{
					itemType = item,
					itemAmount = amount,
					jettisoned = false,
					ownerFaction = base.parent.faction
				}, drone.unitData), null, false);
				if (item.itemCategory == ItemCategory.Ore && base.parent.faction == Faction.player)
				{
					for (int i = 0; i < amount; i++)
					{
						MissionObjective.Trigger(MissionTrigger.MinedOre, item, null, false);
					}
					return;
				}
				if (item.itemCategory == ItemCategory.Salvage && base.parent.faction == Faction.player)
				{
					for (int j = 0; j < amount; j++)
					{
						MissionObjective.Trigger(MissionTrigger.SalvagedItem, item, null, false);
					}
				}
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000C0B38 File Offset: 0x000BED38
		private IEnumerator HandlePostUnloadBehavior(Drone drone)
		{
			if (!this.returning)
			{
				yield return this.ToggleDoors(true);
				yield return this.DeployDrone(drone);
			}
			else if (this.AllDronesDocked())
			{
				yield return this.doorMechanism.ToggleDoors(false);
				this.returning = false;
			}
			yield break;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000C0B4E File Offset: 0x000BED4E
		public bool BusyWithAction()
		{
			return this.returning || this.deploying;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000C0B60 File Offset: 0x000BED60
		public void Autonomous()
		{
			foreach (Drone drone in this.drones)
			{
				drone.SetAutonomousBehavior();
			}
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000C0BB0 File Offset: 0x000BEDB0
		public void RemoveDrone(Drone drone)
		{
			this.drones.Remove(drone);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000C0BC0 File Offset: 0x000BEDC0
		private void RebuildDrone()
		{
			for (int i = 0; i < this.droneAmount; i++)
			{
				bool flag = false;
				using (List<Drone>.Enumerator enumerator = this.drones.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.droneIndex == i)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.RebuildDrone(i);
					return;
				}
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000C0C38 File Offset: 0x000BEE38
		private void RebuildDrone(int idx)
		{
			Drone drone = this.CreateAndInitializeDrone(idx);
			drone.gameObject.SetActive(false);
			drone.rebuildTimer = this.rebuildTime;
			this.drones.Add(drone);
			this.rebuilding = true;
			base.StartCoroutine(this.WaitForDroneCompletion(drone));
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000C0C86 File Offset: 0x000BEE86
		private IEnumerator WaitForDroneCompletion(Drone drone)
		{
			yield return new WaitUntil(() => drone.rebuildTimer < 0f);
			this.rebuilding = false;
			if (this.shouldDeploy && !this.returning && !base.parent.travelling && !base.parent.inShipYard)
			{
				yield return this.CoroutineDeployDrones();
			}
			yield break;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000C0C9C File Offset: 0x000BEE9C
		public override MainStat GetMainStat()
		{
			return new MainStat("Drone Power", this.GetDronePower());
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000C0CAE File Offset: 0x000BEEAE
		protected override void SetMainSubStats()
		{
			this.mainSubStats.AddMainSubStat(this._droneAmount.ToString(), "Slots");
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000C0CCB File Offset: 0x000BEECB
		public IEnumerable<Drone> GetDroneLoadout(SpaceShipData parent = null)
		{
			if (this.drones.Count == this.droneAmount)
			{
				foreach (Drone drone in this.drones)
				{
					yield return drone;
				}
				List<Drone>.Enumerator enumerator = default(List<Drone>.Enumerator);
			}
			else
			{
				int num;
				for (int i = 0; i < this.droneAmount; i = num + 1)
				{
					yield return this.GetDronePrefab(i, parent);
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000C0CE4 File Offset: 0x000BEEE4
		public bool HasLoadout(GameplayType type, TargetLayer targetLayer = TargetLayer.Both, SpaceShipData parent = null)
		{
			foreach (Drone drone in this.GetDroneLoadout(parent))
			{
				if (drone && drone.HasDefaultLoadout(type, targetLayer))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000C0D44 File Offset: 0x000BEF44
		public bool HasMiningLoadout(bool core)
		{
			foreach (Drone drone in this.GetDroneLoadout(null))
			{
				if (drone)
				{
					AbstractTurret defaultTurret = drone.GetDefaultTurret();
					if (defaultTurret)
					{
						if (defaultTurret.targetsCore && core)
						{
							return true;
						}
						if (defaultTurret.targetsSurface && !core)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000C0DC4 File Offset: 0x000BEFC4
		public bool CanDeployReturn()
		{
			return !this.BusyWithAction() && !this.doorMechanism.IsMoving() && !Singleton<TravelManager>.Instance.TravelActive() && !base.parent.inShipYard;
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000C0DF8 File Offset: 0x000BEFF8
		public void AddManualTarget(TargetableUnit target, bool overrideTarget = true)
		{
			if (!this.AreDronesDeployed())
			{
				this.DeployDrones();
			}
			bool flag = false;
			foreach (Drone drone in this.drones)
			{
				if (drone && target && !(drone.targetProvider == null) && !drone.targetProvider.manualTarget && drone.targetProvider.turrets != null && target.CanBeDamagedBy(drone.targetProvider.turrets[0]))
				{
					drone.SetManualTarget(target);
					flag = true;
				}
			}
			if (!flag)
			{
				for (int i = 0; i < this.drones.Count; i++)
				{
					Drone drone2 = this.drones[i];
					if (drone2 && target && !(drone2.targetProvider == null) && drone2.targetProvider.turrets != null && drone2.targetProvider.manualTarget != target && target.CanBeDamagedBy(drone2.targetProvider.turrets[0]) && (overrideTarget || !drone2.targetProvider.manualTarget))
					{
						drone2.SetManualTarget(target);
						this.drones.RemoveAt(i);
						this.drones.Add(drone2);
						return;
					}
				}
			}
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000C0F74 File Offset: 0x000BF174
		private void MissionTriggerDroneBay()
		{
			MissionObjective.Trigger(MissionTrigger.EquipDroneShip, null, null, false);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000C0F80 File Offset: 0x000BF180
		public void RebuildDroneLoadout()
		{
			List<Drone> droneSlots = base.parent.unitData.droneSlots;
			for (int i = 0; i < this.drones.Count; i++)
			{
				Drone drone = this.drones[i];
				if (droneSlots[drone.droneIndex] && droneSlots[drone.droneIndex] != drone.dronePrefab)
				{
					drone.DetachFromTarget();
					UnityEngine.Object.Destroy(drone.gameObject);
					this.RemoveDrone(drone);
					i--;
				}
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000C100C File Offset: 0x000BF20C
		public List<DecalPlacement> GetOrCreateDoorPlacements(int doorIndex)
		{
			DoorMechanism doorMechanism = this.doorMechanism;
			int num = (doorMechanism != null) ? doorMechanism.DoorCount : 0;
			if (num == 0)
			{
				return null;
			}
			if (this._doorDecalPlacements == null)
			{
				this._doorDecalPlacements = new List<DecalPlacement>[num];
				for (int i = 0; i < num; i++)
				{
					this._doorDecalPlacements[i] = new List<DecalPlacement>();
				}
			}
			if (doorIndex < 0 || doorIndex >= this._doorDecalPlacements.Length)
			{
				return null;
			}
			return this._doorDecalPlacements[doorIndex];
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000C1078 File Offset: 0x000BF278
		public void ResetAllDoors()
		{
			if (this.doorMechanism == null)
			{
				return;
			}
			Door[] doors = this.doorMechanism.Doors;
			for (int i = 0; i < doors.Length; i++)
			{
				doors[i].ResetSprite();
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000C10B8 File Offset: 0x000BF2B8
		public void BakeAllDoorDecals()
		{
			if (this.doorMechanism == null || this._doorDecalPlacements == null)
			{
				return;
			}
			Door[] doors = this.doorMechanism.Doors;
			int num = 0;
			while (num < doors.Length && num < this._doorDecalPlacements.Length)
			{
				if (this._doorDecalPlacements[num].Count > 0)
				{
					doors[num].ApplyDecals(this._doorDecalPlacements[num]);
				}
				num++;
			}
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000C1124 File Offset: 0x000BF324
		public override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["droneSeed"] = this.droneSeed;
			if (this._doorDecalPlacements != null)
			{
				JsonArray jsonArray = new JsonArray();
				foreach (List<DecalPlacement> list in this._doorDecalPlacements)
				{
					JsonArray jsonArray2 = new JsonArray();
					foreach (DecalPlacement decalPlacement in list)
					{
						jsonArray2.Add(decalPlacement.ToJson());
					}
					jsonArray.Add(jsonArray2);
				}
				data["doorDecals"] = jsonArray;
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000C11E4 File Offset: 0x000BF3E4
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			if (data["droneSeed"].IsString)
			{
				this.droneSeed = data["droneSeed"];
			}
			else
			{
				this.droneSeed = SeededRandom.Global.RandomItemSeed();
			}
			if (data["doorDecals"].IsJsonArray)
			{
				JsonArray asJsonArray = data["doorDecals"].AsJsonArray;
				this._doorDecalPlacements = new List<DecalPlacement>[asJsonArray.Count];
				for (int i = 0; i < asJsonArray.Count; i++)
				{
					this._doorDecalPlacements[i] = new List<DecalPlacement>();
					if (asJsonArray[i].IsJsonArray)
					{
						foreach (JsonValue json in asJsonArray[i].AsJsonArray)
						{
							this._doorDecalPlacements[i].Add(DecalPlacement.FromJson(json));
						}
					}
				}
			}
		}

		// Token: 0x04001391 RID: 5009
		[Header("Module Specific")]
		private const float RebuildBase = 20f;

		// Token: 0x04001392 RID: 5010
		private const float TransitionDurationBase = 1.5f;

		// Token: 0x04001393 RID: 5011
		public int _droneAmount;

		// Token: 0x04001394 RID: 5012
		public int droneBonusAmount;

		// Token: 0x04001395 RID: 5013
		public string droneSeed;

		// Token: 0x04001396 RID: 5014
		public List<Drone> drones = new List<Drone>();

		// Token: 0x04001397 RID: 5015
		public bool deploying;

		// Token: 0x04001398 RID: 5016
		public bool returning;

		// Token: 0x04001399 RID: 5017
		public bool rebuilding;

		// Token: 0x0400139A RID: 5018
		public bool shouldDeploy;

		// Token: 0x0400139B RID: 5019
		public DoorMechanism doorMechanism;

		// Token: 0x0400139C RID: 5020
		private float dronePower;

		// Token: 0x0400139D RID: 5021
		private float triggerTimer = 0.5f;

		// Token: 0x0400139E RID: 5022
		private List<DecalPlacement>[] _doorDecalPlacements;
	}
}
