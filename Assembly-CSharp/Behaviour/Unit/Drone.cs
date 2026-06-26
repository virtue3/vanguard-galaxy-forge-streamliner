using System;
using System.Collections.Generic;
using Behaviour.Ability;
using Behaviour.Crew;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.MiningTurrets;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Ability;
using Source.Crew;
using Source.Item;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001BF RID: 447
	public class Drone : AbstractUnit
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0006FA6C File Offset: 0x0006DC6C
		public override int MaxFullPowerAngle
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0006FA6F File Offset: 0x0006DC6F
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x0006FA77 File Offset: 0x0006DC77
		public AbstractUnit droneCommander { get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0006FA80 File Offset: 0x0006DC80
		public override int cargoCapacity
		{
			get
			{
				return (int)this.GetStat(EquipStat.CargoCapacity);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0006FA8B File Offset: 0x0006DC8B
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x0006FA93 File Offset: 0x0006DC93
		public int droneIndex { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0006FA9C File Offset: 0x0006DC9C
		public override string displayName
		{
			get
			{
				return Translation.TranslateOnly("@Drone" + base.name.Replace(" ", ""), Array.Empty<object>());
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x0006FAC7 File Offset: 0x0006DCC7
		public Sprite droneIcon
		{
			get
			{
				return base.GetComponent<SpriteRenderer>().sprite;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0006FAD4 File Offset: 0x0006DCD4
		public override string targetName
		{
			get
			{
				return "@Drone";
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0006FADB File Offset: 0x0006DCDB
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x0006FAE3 File Offset: 0x0006DCE3
		public Drone dronePrefab { get; set; }

		// Token: 0x0600107B RID: 4219 RVA: 0x0006FAEC File Offset: 0x0006DCEC
		private void OnEnable()
		{
			base.transform.Z(ZIndex.Drone);
			if (!base.surfaceSprite)
			{
				base.CloneBaseSprite();
			}
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0006FB10 File Offset: 0x0006DD10
		protected override void Start()
		{
			base.Start();
			base.transform.Z(ZIndex.Drone);
			SpaceShip spaceShip = this.droneCommander as SpaceShip;
			if (spaceShip != null)
			{
				base.AddHullBonus(spaceShip.shipRoleType.GetGameplayType());
			}
			this.isDriller = base.GetComponentInChildren<MiningDrillerTurret>();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0006FB60 File Offset: 0x0006DD60
		private void OnMouseUpAsButton()
		{
			if (!base.IsPlayerEnemy() || !UIHelper.clickTargetingAvailable)
			{
				return;
			}
			GameplayManager.Instance.spaceShip.SetManualTarget(this);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0006FB84 File Offset: 0x0006DD84
		protected override void Update()
		{
			base.Update();
			if (base.targetProvider && !base.targetProvider.priorityTarget)
			{
				AbstractTargetingModule targetProvider = this.droneCommander.targetProvider;
				if ((targetProvider != null) ? targetProvider.manualTarget : null)
				{
					base.targetProvider.SetManualTarget(this.droneCommander.targetProvider.manualTarget);
				}
			}
			if (!this.droneCommander)
			{
				return;
			}
			if (this.cargoIndicator)
			{
				this.SetCargoIndicatorColor();
			}
			if (!this.isReturning && this.cantFitMoreCargo)
			{
				Debug.Log("Cargo full? " + base.unitData.cargoCapacity.ToString() + ", " + base.unitData.cargoUsed.ToString());
				this.isReturning = true;
				this.DockWithCommander();
			}
			this.ReturnToDroneBay();
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0006FC70 File Offset: 0x0006DE70
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.isDeployed && base.rigidbody)
			{
				EffectManager instance = Singleton<EffectManager>.Instance;
				if (instance != null)
				{
					instance.PlayExplosionEffect(base.rigidbody.position, base.rigidbody.linearVelocity, 1f, ColorHelper.flashExplosionUnit, 0f);
				}
			}
			this.DetachFromTarget();
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0006FCD3 File Offset: 0x0006DED3
		private void OnDisable()
		{
			HudManager.Instance.RemoveHealthBar(this);
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0006FCE0 File Offset: 0x0006DEE0
		public void DetachFromTarget()
		{
			MiningDrillerTurret[] componentsInChildren = base.GetComponentsInChildren<MiningDrillerTurret>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ForceDetach(true);
			}
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0006FD0B File Offset: 0x0006DF0B
		public override void AddCrewExperience(float experience, CommanderSpecialization? skillTree = null, bool showFloating = true)
		{
			this.droneCommander.AddCrewExperience(experience, skillTree, showFloating);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0006FD1B File Offset: 0x0006DF1B
		protected override bool FreeRotation()
		{
			return true;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0006FD1E File Offset: 0x0006DF1E
		protected override AutoActions CreateAutoActions()
		{
			return null;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0006FD24 File Offset: 0x0006DF24
		public override float GetStat(EquipStat s)
		{
			float num = base.GetStat(s);
			if (this.IsDroneStat(s))
			{
				if (s.IsPercentageStat())
				{
					num += this.droneCommander.GetStat(s);
				}
				else
				{
					num += this.droneCommander.GetStat(s) / (float)this.droneCommander.droneBayModule._droneAmount;
				}
			}
			if (this.droneCommander && this.droneCommander.IsPlayer(false))
			{
				if (s == EquipStat.DamageReduction)
				{
					num += SkilltreeNode.dronesDefenses.currentIncrease;
				}
				if (s == EquipStat.Thrust || s == EquipStat.SideThrust || s == EquipStat.RotationalThrust)
				{
					num *= 1f + SkilltreeNode.dronesSpeed.currentIncrease;
				}
			}
			this.UpdateTriggeredAbilities();
			return num;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0006FDD4 File Offset: 0x0006DFD4
		private bool IsDroneStat(EquipStat s)
		{
			if (s <= EquipStat.DronePower)
			{
				switch (s)
				{
				case EquipStat.HullHP:
					return false;
				case EquipStat.ArmorHP:
					return false;
				case EquipStat.ShieldHP:
					return false;
				default:
					switch (s)
					{
					case EquipStat.Power:
						return false;
					case EquipStat.CombatPower:
						return false;
					case EquipStat.MiningPower:
						return false;
					case EquipStat.SalvagePower:
						return false;
					case EquipStat.DronePower:
						return false;
					}
					break;
				}
			}
			else
			{
				if (s == EquipStat.CriticalChance)
				{
					return false;
				}
				switch (s)
				{
				case EquipStat.Thrust:
					return false;
				case EquipStat.SideThrust:
					return false;
				case EquipStat.RotationalThrust:
					return false;
				case EquipStat.CargoCapacity:
					return false;
				case EquipStat.EnergyCapacity:
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0006FE84 File Offset: 0x0006E084
		public void UpdateTriggeredAbilities()
		{
			this.triggeredAbilities.Clear();
			foreach (TriggeredAbility triggeredAbility in base.GetComponentsInChildren<TriggeredAbility>())
			{
				if (triggeredAbility)
				{
					List<TriggeredAbility> list;
					if (!this.triggeredAbilities.TryGetValue(triggeredAbility.trigger, out list))
					{
						list = new List<TriggeredAbility>();
						this.triggeredAbilities[triggeredAbility.trigger] = list;
					}
					list.Add(triggeredAbility);
				}
			}
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0006FEF4 File Offset: 0x0006E0F4
		public override void CheckTriggerAbility(AbilityTrigger trigger, object source, AbstractUnit triggeredBySubordinate)
		{
			List<TriggeredAbility> list;
			if (this.triggeredAbilities.TryGetValue(trigger, out list))
			{
				foreach (TriggeredAbility triggeredAbility in list)
				{
					triggeredAbility.TriggerPayload(source, triggeredBySubordinate);
				}
			}
			if (this.droneCommander != null)
			{
				this.droneCommander.CheckTriggerAbility(trigger, source, this);
			}
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0006FF70 File Offset: 0x0006E170
		public void SetCargoIndicatorColor()
		{
			SpriteRenderer component = this.cargoIndicator.GetComponent<SpriteRenderer>();
			float cargoCapacity = base.unitData.cargoCapacity;
			float num = base.unitData.cargoUsed / cargoCapacity * 100f;
			if (num == 0f)
			{
				component.color = Color.green;
				return;
			}
			if (num > 0f && num < 90f)
			{
				component.color = new Color(1f, 0.65f, 0f);
				return;
			}
			if (num >= 90f && num <= 100f)
			{
				component.color = Color.red;
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00070003 File Offset: 0x0006E203
		public void SetCommander(AbstractUnit spaceship)
		{
			this.droneCommander = spaceship;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0007000C File Offset: 0x0006E20C
		public void SetAutonomousBehavior()
		{
			base.ClearOverrideDestination();
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00070014 File Offset: 0x0006E214
		public void MoveToCommander()
		{
			this.isReturning = true;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00070020 File Offset: 0x0006E220
		public void DockWithCommander()
		{
			this.isReturning = true;
			this.droneCommander.droneBayModule.DroneWantsToReturn(this);
			AbstractTurret turret = this.GetTurret();
			if (this.HasLoadout(GameplayType.Salvage, TargetLayer.Core) && turret.active)
			{
				turret.Deactivate();
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00070064 File Offset: 0x0006E264
		public void ReturnToDroneBay()
		{
			if (!this.isReturning)
			{
				return;
			}
			Vector2 dockingPosition = this.droneCommander.droneBayModule.GetDockingPosition();
			float num = Vector2.Distance(base.rigidbody.position, dockingPosition);
			float num2 = 2f + (float)this.droneCommander.droneBayModule.size * 0.5f;
			if (num < num2)
			{
				base.rigidbody.linearVelocity = Vector2.zero;
				base.rigidbody.angularVelocity = 0f;
				base.rigidbody.position = Vector2.MoveTowards(base.rigidbody.position, dockingPosition, 5f * Time.deltaTime);
			}
			else
			{
				base.SetOverrideDestination(dockingPosition, true, false, false);
			}
			if (num < 0.25f && !this.docking)
			{
				AbstractTurret turret = this.GetTurret();
				if (turret.active)
				{
					turret.Deactivate();
				}
				this.docking = true;
				base.StartCoroutine(this.droneCommander.droneBayModule.TryDockingDrone(this));
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00070154 File Offset: 0x0006E354
		public bool IsDroneFromPlayer()
		{
			return this.droneCommander == GameplayManager.Instance.spaceShip;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0007016C File Offset: 0x0006E36C
		public bool HasLoadout(GameplayType gameplayType, TargetLayer targetLayer = TargetLayer.Both)
		{
			AbstractTurret turret = this.GetTurret();
			return turret && turret.gameplayType == gameplayType && turret.CanTargetLayer(targetLayer);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0007019C File Offset: 0x0006E39C
		public bool HasDefaultLoadout(GameplayType gameplayType, TargetLayer targetLayer = TargetLayer.Both)
		{
			AbstractTurret defaultTurret = this.GetDefaultTurret();
			return defaultTurret && defaultTurret.gameplayType == gameplayType && defaultTurret.CanTargetLayer(targetLayer);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x000701CC File Offset: 0x0006E3CC
		public GameplayType GetDefaultLoadout()
		{
			AbstractTurret defaultTurret = this.GetDefaultTurret();
			if (defaultTurret)
			{
				return defaultTurret.gameplayType;
			}
			return GameplayType.Generic;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x000701F0 File Offset: 0x0006E3F0
		public AbstractTurret GetDefaultTurret()
		{
			if (base.hardpointSlots.Length == 0 || !base.hardpointSlots[0])
			{
				return null;
			}
			EquipmentBuilder defaultEquipment = base.hardpointSlots[0].defaultEquipment;
			if (defaultEquipment == null)
			{
				return null;
			}
			return defaultEquipment.prefab.GetComponent<AbstractTurret>();
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0007022C File Offset: 0x0006E42C
		private AbstractTurret GetTurret()
		{
			AbstractTurret componentInChildren = base.hardpointSlots[0].GetComponentInChildren<AbstractTurret>();
			if (!componentInChildren)
			{
				return null;
			}
			return componentInChildren;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00070252 File Offset: 0x0006E452
		protected override void TargetCleanup()
		{
			base.TargetCleanup();
			AbstractTargetingModule targetProvider = base.targetProvider;
			if (((targetProvider != null) ? targetProvider.priorityTarget : null) is Asteroid)
			{
				this.DetachFromTarget();
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00070279 File Offset: 0x0006E479
		public override void OnDestruction()
		{
			base.ShowEffectsOnDeath();
			this.TargetCleanup();
			this.droneCommander.droneBayModule.RemoveDrone(this);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x000702A4 File Offset: 0x0006E4A4
		public void ForceReset()
		{
			base.Dead(false);
			base.SetCurrentHP(true);
			this.cantFitMoreCargo = false;
			this.isReturning = false;
			this.isDeployed = false;
			base.gameObject.SetActive(false);
			base.rigidbody.position = this.droneCommander.transform.position;
			base.transform.parent = this.droneCommander.transform.parent;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0007031B File Offset: 0x0006E51B
		public override float GetEquivalentTurretsCount(EquipStat powerStat)
		{
			if (this.droneCommander)
			{
				return this.droneCommander.GetEquivalentTurretsCount(powerStat);
			}
			return base.GetEquivalentTurretsCount(powerStat);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0007033E File Offset: 0x0006E53E
		public static Drone Get(string name)
		{
			if (name == "_Base Drone")
			{
				name = "Mining Drone Laser";
			}
			return Drone.allDrones[name];
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0007035F File Offset: 0x0006E55F
		public static Dictionary<string, Drone> GetAll()
		{
			return Drone.allDrones;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00070368 File Offset: 0x0006E568
		public static void LoadAll()
		{
			Drone.allDrones.Clear();
			Drone[] array = Resources.LoadAll<Drone>("Drones");
			for (int i = 0; i < array.Length; i++)
			{
				string name = array[i].name;
				array[i].identifier = name;
				if (!name.StartsWith("_"))
				{
					Drone.allDrones[array[i].identifier] = array[i];
				}
			}
		}

		// Token: 0x04000919 RID: 2329
		private static Dictionary<string, Drone> allDrones = new Dictionary<string, Drone>();

		// Token: 0x0400091C RID: 2332
		public bool isDeployed;

		// Token: 0x0400091D RID: 2333
		public bool isReturning;

		// Token: 0x0400091E RID: 2334
		public bool docking;

		// Token: 0x0400091F RID: 2335
		public float rebuildTimer;

		// Token: 0x04000920 RID: 2336
		[SerializeField]
		public float controlRange = 30f;

		// Token: 0x04000921 RID: 2337
		public bool cantFitMoreCargo;

		// Token: 0x04000922 RID: 2338
		public GameObject cargoIndicator;

		// Token: 0x04000923 RID: 2339
		public bool keepMoving = true;

		// Token: 0x04000924 RID: 2340
		private const float DockingPullRange = 2f;

		// Token: 0x04000925 RID: 2341
		private const float DockingPullSpeed = 5f;

		// Token: 0x04000926 RID: 2342
		private Dictionary<AbilityTrigger, List<TriggeredAbility>> triggeredAbilities = new Dictionary<AbilityTrigger, List<TriggeredAbility>>();

		// Token: 0x04000927 RID: 2343
		public bool isDriller;
	}
}
