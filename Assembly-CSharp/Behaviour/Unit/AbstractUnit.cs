using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment;
using Behaviour.Equipment.Aspect;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Salvage;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Ability;
using Source.Combat;
using Source.Crew;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001BA RID: 442
	public abstract class AbstractUnit : TargetableUnit, IDamageable
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0006B16A File Offset: 0x0006936A
		public virtual int MaxFullPowerAngle
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0006B16E File Offset: 0x0006936E
		// (set) Token: 0x06000F71 RID: 3953 RVA: 0x0006B176 File Offset: 0x00069376
		public string identifier { get; protected set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0006B17F File Offset: 0x0006937F
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0006B187 File Offset: 0x00069387
		public AbstractUnitData unitData { get; private set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0006B190 File Offset: 0x00069390
		public float baseHullHP
		{
			get
			{
				return this.hullHPScale * 150f;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0006B19E File Offset: 0x0006939E
		public float baseShieldHP
		{
			get
			{
				return this.shieldHPScale * 300f;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0006B1AC File Offset: 0x000693AC
		public float baseArmorHP
		{
			get
			{
				return this.armorHPScale * 300f;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0006B1BA File Offset: 0x000693BA
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0006B1C2 File Offset: 0x000693C2
		public float hullHPScale { get; private set; } = 1f;

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0006B1CB File Offset: 0x000693CB
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x0006B1D3 File Offset: 0x000693D3
		public float armorHPScale { get; private set; } = 1f;

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0006B1DC File Offset: 0x000693DC
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x0006B1E4 File Offset: 0x000693E4
		public float shieldHPScale { get; private set; } = 1f;

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x0006B1ED File Offset: 0x000693ED
		// (set) Token: 0x06000F7E RID: 3966 RVA: 0x0006B1F5 File Offset: 0x000693F5
		public int _cargoCapacity { get; private set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x0006B1FE File Offset: 0x000693FE
		public virtual int cargoCapacity
		{
			get
			{
				if (this.faction != Faction.player)
				{
					return 10000000;
				}
				return (int)this.GetStat(EquipStat.CargoCapacity);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0006B21C File Offset: 0x0006941C
		// (set) Token: 0x06000F81 RID: 3969 RVA: 0x0006B224 File Offset: 0x00069424
		public int tonnage { get; private set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0006B22D File Offset: 0x0006942D
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x0006B235 File Offset: 0x00069435
		public virtual string displayName { get; private set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0006B23E File Offset: 0x0006943E
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x0006B246 File Offset: 0x00069446
		public Manufacturer manufacturer { get; private set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0006B24F File Offset: 0x0006944F
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0006B257 File Offset: 0x00069457
		public SpaceShipModule[] moduleSlots { get; private set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0006B260 File Offset: 0x00069460
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x0006B268 File Offset: 0x00069468
		public SpaceShipHardpoint[] hardpointSlots { get; private set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0006B271 File Offset: 0x00069471
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x0006B279 File Offset: 0x00069479
		public SpaceShipBooster[] boosterSlots { get; private set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0006B282 File Offset: 0x00069482
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x0006B28A File Offset: 0x0006948A
		public float baseExperienceReward { get; private set; } = 10f;

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0006B293 File Offset: 0x00069493
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x0006B29B File Offset: 0x0006949B
		public int pointValue { get; private set; } = 10;

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0006B2A4 File Offset: 0x000694A4
		public override bool hasHealthBar
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0006B2A7 File Offset: 0x000694A7
		public int level
		{
			get
			{
				return this.unitData.level;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0006B2B4 File Offset: 0x000694B4
		public float maxHullHP
		{
			get
			{
				return this.GetStat(EquipStat.HullHP);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0006B2BD File Offset: 0x000694BD
		public float healthPerBattleDamage
		{
			get
			{
				return this.maxHullHP / 10f;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0006B2CC File Offset: 0x000694CC
		public float fractionHullHP
		{
			get
			{
				return this.currentHullHP / this.maxHullHP;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0006B2DB File Offset: 0x000694DB
		public float maxArmorHP
		{
			get
			{
				return this.GetStat(EquipStat.ArmorHP);
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0006B2E4 File Offset: 0x000694E4
		public float maxShieldHP
		{
			get
			{
				return this.GetStat(EquipStat.ShieldHP);
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0006B2ED File Offset: 0x000694ED
		public float maxTotalHP
		{
			get
			{
				return this.maxHullHP + this.maxArmorHP + this.maxShieldHP;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0006B303 File Offset: 0x00069503
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x0006B30B File Offset: 0x0006950B
		public SpriteRenderer surfaceSprite { get; protected set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0006B314 File Offset: 0x00069514
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x0006B31C File Offset: 0x0006951C
		public Sprite structureSprite { get; protected set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0006B325 File Offset: 0x00069525
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x0006B32D File Offset: 0x0006952D
		public PolygonCollider2D structureCollider { get; protected set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0006B336 File Offset: 0x00069536
		public override float mass
		{
			get
			{
				return (float)(this.unitData.unitDefinition.tonnage * 10);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0006B34C File Offset: 0x0006954C
		public Faction faction
		{
			get
			{
				AbstractUnitData unitData = this.unitData;
				if (unitData == null)
				{
					return null;
				}
				return unitData.faction;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0006B35F File Offset: 0x0006955F
		// (set) Token: 0x06000FA1 RID: 4001 RVA: 0x0006B367 File Offset: 0x00069567
		public float speed { get; protected set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0006B370 File Offset: 0x00069570
		public Vector2? overrideTarget
		{
			get
			{
				return this.unitData.overrideTarget;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0006B37D File Offset: 0x0006957D
		public bool clearOverrideWhenReachedDestination
		{
			get
			{
				return this.unitData.clearOverrideWhenReachedDestination;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0006B38A File Offset: 0x0006958A
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x0006B392 File Offset: 0x00069592
		public AbstractTargetingModule targetProvider { get; private set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0006B39B File Offset: 0x0006959B
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0006B3A3 File Offset: 0x000695A3
		public Vector2 currentDestination { get; private set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0006B3AC File Offset: 0x000695AC
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x0006B3B4 File Offset: 0x000695B4
		public bool holdPosition { get; protected set; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0006B3BD File Offset: 0x000695BD
		public virtual bool shouldAutoFire
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0006B3C0 File Offset: 0x000695C0
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x0006B3C8 File Offset: 0x000695C8
		public EngineThrustersModule engine { get; protected set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0006B3D1 File Offset: 0x000695D1
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x0006B3D9 File Offset: 0x000695D9
		public ShieldGeneratorModule shieldGeneratorModule { get; protected set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0006B3E2 File Offset: 0x000695E2
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x0006B3EA File Offset: 0x000695EA
		public ArmorModule armorModule { get; protected set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0006B3F3 File Offset: 0x000695F3
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x0006B3FB File Offset: 0x000695FB
		public ReactorModule reactorModule { get; private set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0006B404 File Offset: 0x00069604
		// (set) Token: 0x06000FB4 RID: 4020 RVA: 0x0006B40C File Offset: 0x0006960C
		public DroneBayModule droneBayModule { get; private set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0006B415 File Offset: 0x00069615
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x0006B41D File Offset: 0x0006961D
		public TorpedoBayModule torpedoBayModule { get; private set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0006B426 File Offset: 0x00069626
		// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x0006B42E File Offset: 0x0006962E
		public bool inShipYard { get; protected set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0006B437 File Offset: 0x00069637
		public override bool bouncyBouncy
		{
			get
			{
				return !this.inShipYard;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0006B442 File Offset: 0x00069642
		public bool travelling
		{
			get
			{
				return this.unitData.travelling;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0006B44F File Offset: 0x0006964F
		// (set) Token: 0x06000FBC RID: 4028 RVA: 0x0006B457 File Offset: 0x00069657
		public AutoActions autoActions { get; protected set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0006B460 File Offset: 0x00069660
		public bool takingDamage
		{
			get
			{
				return this.takingDamageTimer > 0f && !this.holdPosition;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0006B47A File Offset: 0x0006967A
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0006B482 File Offset: 0x00069682
		public SalvageContainer collisionIsSalvage { get; protected set; }

		// Token: 0x06000FC0 RID: 4032
		protected abstract AutoActions CreateAutoActions();

		// Token: 0x06000FC1 RID: 4033
		public abstract void CheckTriggerAbility(AbilityTrigger trigger, object source, AbstractUnit triggeredBySubordinate = null);

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0006B48B File Offset: 0x0006968B
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x0006B498 File Offset: 0x00069698
		public float currentHullHP
		{
			get
			{
				return this.unitData.currentHullHP;
			}
			set
			{
				this.unitData.currentHullHP = value;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0006B4A6 File Offset: 0x000696A6
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x0006B4B3 File Offset: 0x000696B3
		public float currentArmorHP
		{
			get
			{
				return this.unitData.currentArmorHP;
			}
			set
			{
				this.unitData.currentArmorHP = value;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0006B4C1 File Offset: 0x000696C1
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x0006B4CE File Offset: 0x000696CE
		public float currentShieldHP
		{
			get
			{
				return this.unitData.currentShieldHP;
			}
			set
			{
				this.unitData.currentShieldHP = value;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0006B4DC File Offset: 0x000696DC
		public ZIndex defaultZIndex
		{
			get
			{
				GameplayManager instance = GameplayManager.Instance;
				if (!(((instance != null) ? instance.spaceShip : null) == this))
				{
					return ZIndex.NPC;
				}
				return ZIndex.Player;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0006B4FA File Offset: 0x000696FA
		// (set) Token: 0x06000FCA RID: 4042 RVA: 0x0006B502 File Offset: 0x00069702
		public bool dummyEngine { get; private set; }

		// Token: 0x06000FCB RID: 4043 RVA: 0x0006B50B File Offset: 0x0006970B
		protected override void Awake()
		{
			base.Awake();
			this.AddHullBonus();
			this.AddRangeBonus();
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0006B520 File Offset: 0x00069720
		protected override void Start()
		{
			base.Start();
			base.transform.Z(this.defaultZIndex);
			this.collider = base.GetComponent<Collider2D>();
			this.AddRigidBody((this is Drone || this is DefensiveTurret) ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic);
			if (this.inShipYard)
			{
				this.SetShipyardConfig();
			}
			this.SetEngine();
			this.SetCurrentHP(false);
			this.SetMaskInteraction(SpriteMaskInteraction.None);
			if (!this.inShipYard)
			{
				base.StartCoroutine(this.StartAutoActions());
			}
			if (this.IsPlayer(true))
			{
				if (this.maxShieldHP > 0f && !this.shieldGeneratorModule)
				{
					ShieldGeneratorModule component = UnityEngine.Object.Instantiate<InventoryItemType>(InventoryItemType.Get("SmallShield"), base.transform).GetComponent<ShieldGeneratorModule>();
					component.overrideBaseCapacity = 0f;
					this.SetShieldGeneratorModule(component);
				}
				if (this.maxArmorHP > 0f && !this.armorModule)
				{
					ArmorModule component2 = UnityEngine.Object.Instantiate<InventoryItemType>(InventoryItemType.Get("SmallArmor"), base.transform).GetComponent<ArmorModule>();
					component2.overrideBaseCapacity = 0f;
					this.SetArmorModule(component2);
				}
			}
			this.turrets = base.GetComponentsInChildren<AbstractTurret>();
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0006B648 File Offset: 0x00069848
		public void AddRigidBody(RigidbodyType2D type = RigidbodyType2D.Dynamic)
		{
			base.rigidbody = base.gameObject.GetComponent<Rigidbody2D>();
			if (!base.rigidbody)
			{
				base.rigidbody = base.gameObject.AddComponent<Rigidbody2D>();
			}
			base.rigidbody.mass = this.mass;
			base.rigidbody.inertia = this.mass;
			base.rigidbody.linearDamping = 0.1f;
			base.rigidbody.angularDamping = 1f;
			base.rigidbody.centerOfMass = Vector2.zero;
			base.rigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
			base.rigidbody.bodyType = type;
			if (!this.inShipYard && base.rigidbody && this.IsPlayer(true) && GamePlayer.current != null && GamePlayer.current.UseLastShipPosition())
			{
				this.unitData.positionData.SetDataToRigidbody(base.rigidbody);
			}
			this.currentDestination = base.rigidbody.position;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0006B746 File Offset: 0x00069946
		private IEnumerator StartAutoActions()
		{
			yield return new WaitForSeconds(0.1f);
			this.ResetAutoActions();
			yield break;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0006B755 File Offset: 0x00069955
		public void ResetAutoActions()
		{
			this.autoActions = this.CreateAutoActions();
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0006B764 File Offset: 0x00069964
		private void AddHullBonus()
		{
			SpaceShip spaceShip = this as SpaceShip;
			if (spaceShip != null && !spaceShip.mint)
			{
				this.AddHullBonus(spaceShip.shipRoleType.GetGameplayType());
			}
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0006B794 File Offset: 0x00069994
		protected void AddHullBonus(GameplayType bonus)
		{
			if (bonus != GameplayType.Generic && bonus != GameplayType.Cargo)
			{
				UnityEngine.Object.Instantiate<BonusBadge>(Resources.Load<BonusBadge>("Bonus/HullBonus/" + bonus.ToString() + "HullBonus"), base.transform).SetBonusBadge(BonusType.HullBonus);
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0006B7D0 File Offset: 0x000699D0
		private void AddRangeBonus()
		{
			SpaceShip spaceShip = this as SpaceShip;
			if (spaceShip != null)
			{
				float shipRangeBonus = spaceShip.shipRoleType.GetShipRangeBonus();
				if (shipRangeBonus > 0f)
				{
					BonusBadge bonusBadge = UnityEngine.Object.Instantiate<BonusBadge>(Resources.Load<BonusBadge>("Bonus/RangeBonus/RangeBonus"), base.transform);
					bonusBadge.GetComponent<BoostStat>().SetStatBoost(shipRangeBonus);
					bonusBadge.SetBonusBadge(BonusType.RangeBonus);
				}
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0006B823 File Offset: 0x00069A23
		private void AddManufacturerBonus()
		{
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0006B828 File Offset: 0x00069A28
		public float GetAngleToTarget()
		{
			if (this.forceWorldAngle == null && !this.travelling && this.targetProvider && this.targetProvider.priorityTarget && !this.takingDamage)
			{
				return this.angleToTarget;
			}
			return 0f;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0006B880 File Offset: 0x00069A80
		public void SetMaskInteraction(SpriteMaskInteraction interaction = SpriteMaskInteraction.None)
		{
			SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].maskInteraction = interaction;
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0006B8AB File Offset: 0x00069AAB
		public void SetInShipyard(Vector2 shipyardPosition)
		{
			this.inShipYard = true;
			this.shipyardPosition = shipyardPosition;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0006B8BC File Offset: 0x00069ABC
		private void SetShipyardConfig()
		{
			this.EnableHardpointColliders(true);
			this.SetEngineState(false, false);
			this.ResetPosition(null);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0006B8E7 File Offset: 0x00069AE7
		public void ToggleStateForDocking(bool enabled)
		{
			this.collider.isTrigger = !enabled;
			this.structureCollider.isTrigger = !enabled;
			base.transform.Z(enabled ? this.defaultZIndex : ZIndex.DockedShip);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0006B920 File Offset: 0x00069B20
		public void EnableHardpointColliders(bool enabled)
		{
			SpaceShipHardpoint[] hardpointSlots = this.hardpointSlots;
			for (int i = 0; i < hardpointSlots.Length; i++)
			{
				hardpointSlots[i].GetComponent<BoxCollider2D>().enabled = enabled;
			}
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0006B950 File Offset: 0x00069B50
		private void SetEngine()
		{
			if (this is DefensiveTurret || this is CombatStationPart)
			{
				return;
			}
			this.engine = base.GetComponentInChildren<EngineThrustersModule>();
			if (this.engine)
			{
				this.engine.SetRadius(base.radius);
				this.engine.SetRigidBody(base.rigidbody);
				this.engine.SetThrusters();
				this.dummyEngine = false;
				return;
			}
			if (this.IsPlayer(true))
			{
				EngineThrustersModule component = EquipmentBuilder.Get("SmallEngine").CreateItemType(Rarity.Standard, 10, true, null, false, false).GetComponent<EngineThrustersModule>();
				component.SetDummyThrust();
				this.engine = UnityEngine.Object.Instantiate<EngineThrustersModule>(component, base.transform);
				this.engine.SetRadius(base.radius);
				this.engine.SetRigidBody(base.rigidbody);
				this.engine.SetThrusters();
				this.dummyEngine = true;
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0006BA2D File Offset: 0x00069C2D
		public void SetHoldPosition(bool holdPosition)
		{
			if (this.IsPlayer(true) && GamePlayer.current != null)
			{
				GamePlayer.current.holdPosition = holdPosition;
			}
			this.holdPosition = holdPosition;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0006BA54 File Offset: 0x00069C54
		public void SetCurrentHP(bool force = false)
		{
			if (this.currentHullHP < 0f || force)
			{
				this.currentHullHP = this.maxHullHP;
			}
			if (this.currentArmorHP < 0f || force)
			{
				this.currentArmorHP = this.maxArmorHP;
			}
			if (this.currentShieldHP < 0f || force)
			{
				this.currentShieldHP = this.maxShieldHP;
			}
			if (force)
			{
				this.TryRestoreSpriteToOriginal();
			}
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0006BAC4 File Offset: 0x00069CC4
		protected override void Update()
		{
			base.Update();
			if (this.takingDamageTimer > 0f)
			{
				this.takingDamageTimer -= Time.deltaTime;
			}
			this.collisionTimer -= Time.deltaTime;
			if (this.collisionTimer <= 0f)
			{
				this.collisionTimer = 3f;
				this.collisionContactTime = 0f;
				this.collisionIsSalvage = null;
			}
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0006BB34 File Offset: 0x00069D34
		protected void UpdateTarget()
		{
			if (this.inShipYard || this.travelling || (this.targetProvider && !this.targetProvider.active))
			{
				return;
			}
			this.targetTimer -= Time.deltaTime;
			if (this.targetTimer < 0f)
			{
				this.targetProvider = this.GetPriorityTargetModule();
				this.targetTimer = 0.25f;
			}
			Vector2? vector = this.overrideTarget;
			Vector2? vector2 = this.overrideTarget;
			this.approachDistance = 0f;
			this.targetSpeed = Vector2.zero;
			float num = (float)((this.overrideTarget != null) ? 0 : 2);
			bool flag = this.overrideTarget != null;
			this.targetVector = null;
			if (vector == null && this.targetProvider && !this.holdPosition)
			{
				TargetableUnit priorityTarget = this.targetProvider.priorityTarget;
				if (priorityTarget)
				{
					AbstractUnit abstractUnit = priorityTarget as AbstractUnit;
					if (abstractUnit && this.DoWeChase(abstractUnit) && !(this is DefensiveTurret))
					{
						this.targetSpeed = abstractUnit.rigidbody.linearVelocity;
						float startSpeed = (this.speed > this.engine.GetForwardAccelerationPerSecond()) ? this.speed : this.engine.GetForwardAccelerationPerSecond();
						vector = new Vector2?(VectorCalculationsHelper.PredictFuturePosition(base.rigidbody.position, abstractUnit.rigidbody.position, this.targetSpeed.normalized, abstractUnit.speed, startSpeed));
						if (this.targetProvider.trackingTargetPos != null)
						{
							Vector2 b = this.targetProvider.trackingTargetPos.Value - vector.Value;
							vector += b;
						}
						vector2 = vector;
					}
					else
					{
						vector = new Vector2?(this.targetProvider.trackingTargetPos ?? priorityTarget.transform.position);
						vector2 = new Vector2?(priorityTarget.transform.position);
						this.targetSpeed = Vector2.zero;
					}
					if (this.targetProvider.maintainTargetAngle)
					{
						this.targetVector = new Vector2?((vector2.Value - base.rigidbody.position).normalized);
					}
					this.approachDistance = this.targetProvider.approachDistance;
				}
			}
			if (vector != null && Vector2.Distance(base.rigidbody.position, vector.Value) > this.approachDistance)
			{
				Vector2 a = vector.Value - base.rigidbody.position;
				Vector2 moveTo = (a.magnitude - this.approachDistance) / a.magnitude * a + base.rigidbody.position;
				this.MoveTo(moveTo);
				return;
			}
			if (vector != null && Vector2.Distance(base.rigidbody.position, vector.Value) < this.approachDistance - num && this.targetProvider != null && this.targetProvider.runFromTarget)
			{
				Vector2 a2 = base.rigidbody.position - vector.Value;
				float d = this.approachDistance / a2.magnitude;
				Vector2 moveTo2 = vector.Value + a2 * d;
				this.MoveTo(moveTo2);
			}
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0006BEE0 File Offset: 0x0006A0E0
		public void SetManualTarget(TargetableUnit target)
		{
			bool damagableByAll = target.damagableByAll;
			MiningModule module = this.GetModule<MiningModule>();
			if (((damagableByAll && module != null && module.canMineSurface) || target is Asteroid) && module != null)
			{
				module.SetManualTarget(target);
			}
			if (damagableByAll || target is AbstractUnit)
			{
				CombatModule module2 = this.GetModule<CombatModule>();
				if (module2 != null)
				{
					module2.SetManualTarget(target);
				}
			}
			if (damagableByAll || target is SalvageContainer)
			{
				SalvageModule module3 = this.GetModule<SalvageModule>();
				if (module3 != null)
				{
					module3.SetManualTarget(target);
				}
			}
			DroneBayModule droneBayModule = this.droneBayModule;
			if (droneBayModule != null)
			{
				droneBayModule.AddManualTarget(target, true);
			}
			TorpedoBayModule torpedoBayModule = this.torpedoBayModule;
			if (torpedoBayModule == null)
			{
				return;
			}
			torpedoBayModule.AddManualTarget(target, true);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0006BF78 File Offset: 0x0006A178
		public T GetModule<T>() where T : AbstractModule
		{
			return base.GetComponentInChildren<T>();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0006BF80 File Offset: 0x0006A180
		public bool HasModuleSlot(EquipmentSlot slot)
		{
			for (int i = 0; i < this.moduleSlots.Length; i++)
			{
				if (this.moduleSlots[i].slot == slot)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0006BFB4 File Offset: 0x0006A1B4
		public virtual void SetData(AbstractUnitData abstractUnitData, bool setUnitRef = true, bool applyBattleDamage = true)
		{
			this.unitData = abstractUnitData;
			if (setUnitRef)
			{
				abstractUnitData.unit = this;
			}
			foreach (InventoryItemType inventoryItemType in abstractUnitData.equippedModules)
			{
				if (inventoryItemType.GetComponent<ReactorModule>())
				{
					this.LoadEquipment(inventoryItemType);
					break;
				}
			}
			foreach (InventoryItemType inventoryItemType2 in abstractUnitData.equippedModules)
			{
				if (!inventoryItemType2.GetComponent<ReactorModule>())
				{
					this.LoadEquipment(inventoryItemType2);
				}
			}
			foreach (InventoryItemType inventoryItemType3 in abstractUnitData.boosters)
			{
				if (inventoryItemType3 != null)
				{
					this.LoadEquipment(inventoryItemType3);
				}
			}
			for (int j = 0; j < abstractUnitData.hardpoints.Length; j++)
			{
				SpaceShipHardpoint spaceShipHardpoint = this.hardpointSlots[j];
				spaceShipHardpoint.index = j;
				this.LoadHardpoint(abstractUnitData.hardpoints[j], spaceShipHardpoint.transform);
			}
			if (setUnitRef)
			{
				this.unitData.SetCargoCapacity(this.cargoCapacity);
				this.currentHullHP = Mathf.Min(this.currentHullHP, this.maxHullHP);
				this.currentShieldHP = Mathf.Min(this.currentShieldHP, this.maxShieldHP);
				this.currentArmorHP = Mathf.Min(this.currentArmorHP, this.maxArmorHP);
			}
			this.CreateStructure(this.structureSprite);
			if (applyBattleDamage)
			{
				if (this.unitData.battleDamage.Count == 0)
				{
					this.TryRestoreSpriteToOriginal();
					return;
				}
				this.ProcessBattleDamage(0);
			}
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0006C164 File Offset: 0x0006A364
		public void ProcessBattleDamage(int index)
		{
			if (index >= this.unitData.battleDamage.Count || !this.surfaceSprite)
			{
				return;
			}
			SpriteBreakPoint spriteBreakPoint = this.unitData.battleDamage[index];
			BreakDelayedSprite breakDelayedSprite;
			if (spriteBreakPoint.core)
			{
				breakDelayedSprite = this.CreateCoreDamage(spriteBreakPoint);
			}
			else
			{
				breakDelayedSprite = this.ShowBattleDamage(this.surfaceSprite.sprite, spriteBreakPoint, false, false);
			}
			if (breakDelayedSprite != null)
			{
				breakDelayedSprite.onComplete = delegate()
				{
					this.ProcessBattleDamage(index + 1);
				};
			}
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0006C200 File Offset: 0x0006A400
		public void LoadEquipment(InventoryItemType item)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(item, base.transform);
			inventoryItemType.SetEquipmentInitialized(item.equipmentInitialized);
			inventoryItemType.InitializeItem(item);
			AbstractEquipment component = inventoryItemType.GetComponent<AbstractEquipment>();
			component.SetupInWorld();
			ReactorModule reactorModule = component as ReactorModule;
			if (reactorModule == null)
			{
				ShieldGeneratorModule shieldGeneratorModule = component as ShieldGeneratorModule;
				if (shieldGeneratorModule == null)
				{
					ArmorModule armorModule = component as ArmorModule;
					if (armorModule == null)
					{
						DroneBayModule droneBayModule = component as DroneBayModule;
						if (droneBayModule == null)
						{
							TorpedoBayModule torpedoBayModule = component as TorpedoBayModule;
							if (torpedoBayModule != null)
							{
								this.SetTorpedoBayModule(torpedoBayModule);
							}
						}
						else
						{
							this.SetDroneBayModule(droneBayModule);
						}
					}
					else
					{
						this.SetArmorModule(armorModule);
					}
				}
				else
				{
					this.SetShieldGeneratorModule(shieldGeneratorModule);
				}
			}
			else
			{
				this.SetReactorModule(reactorModule);
			}
			if (!(component is ReactorModule))
			{
				this.ConnectEquipmentToReactor(component);
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0006C2AC File Offset: 0x0006A4AC
		public virtual float GetStat(EquipStat s)
		{
			if (this.statsTimestamp == 0f || Time.time > this.statsTimestamp + 0.5f)
			{
				this.CalculateStats();
			}
			float num;
			if (s <= EquipStat.ShieldRechargeDelay)
			{
				switch (s)
				{
				case EquipStat.HullHP:
					num = this.unitData.unitDefinition.baseHullHP;
					goto IL_19D;
				case EquipStat.ArmorHP:
				{
					float num2;
					if (!this.armorModule || this.armorModule.overrideBaseCapacity >= 0f)
					{
						ArmorModule armorModule = this.armorModule;
						num2 = ((armorModule != null) ? armorModule.overrideBaseCapacity : 0f);
					}
					else
					{
						num2 = this.unitData.unitDefinition.baseArmorHP;
					}
					num = num2;
					goto IL_19D;
				}
				case EquipStat.ShieldHP:
				{
					float num3;
					if (!this.shieldGeneratorModule || this.shieldGeneratorModule.overrideBaseCapacity >= 0f)
					{
						ShieldGeneratorModule shieldGeneratorModule = this.shieldGeneratorModule;
						num3 = ((shieldGeneratorModule != null) ? shieldGeneratorModule.overrideBaseCapacity : 0f);
					}
					else
					{
						num3 = this.unitData.unitDefinition.baseShieldHP;
					}
					num = num3;
					goto IL_19D;
				}
				default:
					if (s == EquipStat.ShieldRegen)
					{
						num = (this.shieldGeneratorModule ? this.shieldGeneratorModule.regen : 0f);
						goto IL_19D;
					}
					if (s == EquipStat.ShieldRechargeDelay)
					{
						num = (this.shieldGeneratorModule ? this.shieldGeneratorModule.rechargeDelay : 0f);
						goto IL_19D;
					}
					break;
				}
			}
			else
			{
				if (s == EquipStat.ShieldRechargeRate)
				{
					num = (this.shieldGeneratorModule ? this.shieldGeneratorModule.rechargeRate : 0f);
					goto IL_19D;
				}
				if (s == EquipStat.CriticalChance)
				{
					num = this.GetBaseCritChance();
					goto IL_19D;
				}
				if (s == EquipStat.CargoCapacity)
				{
					num = (float)this._cargoCapacity;
					goto IL_19D;
				}
			}
			num = 0f;
			IL_19D:
			return (num + this.calcedStats[(int)s]) * this.statMultipliers[(int)s];
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0006C46C File Offset: 0x0006A66C
		public virtual float GetNormalizedPower(EquipStat s)
		{
			float equivalentTurretsCount = this.GetEquivalentTurretsCount(s);
			if (equivalentTurretsCount != 0f)
			{
				return this.GetStat(s) / equivalentTurretsCount;
			}
			return this.GetStat(s);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0006C49A File Offset: 0x0006A69A
		private float GetBaseCritChance()
		{
			return 0.03f + this.GetPrecisionCrit();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0006C4A8 File Offset: 0x0006A6A8
		public float GetPrecisionCrit()
		{
			float stat = this.GetStat(EquipStat.Precision);
			int num = Mathf.RoundToInt(25f * GameMath.DamageMultiplier((float)this.unitData.level));
			float num2 = 0.05f * (stat / (float)num);
			if (num2 > 0.05f)
			{
				float f = num2 - 0.05f + 1f;
				num2 = 0.05f + Mathf.Pow(f, 0.75f) - 1f;
			}
			return num2;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0006C515 File Offset: 0x0006A715
		public float GetStatMultiplier(EquipStat s)
		{
			if (this.statsTimestamp == 0f || Time.time > this.statsTimestamp + 0.5f)
			{
				this.CalculateStats();
			}
			return this.statMultipliers[(int)s];
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0006C548 File Offset: 0x0006A748
		public virtual void CalculateStats()
		{
			if (this.turrets != null)
			{
				this.combatEquivalentTurrets = 0f;
				this.miningEquivalentTurrets = 0f;
				this.salvageEquivalentTurrets = 0f;
				foreach (AbstractTurret abstractTurret in this.turrets)
				{
					if (abstractTurret.active)
					{
						if (abstractTurret is AbstractCombatTurret)
						{
							this.combatEquivalentTurrets += abstractTurret.turretEquivalentRating;
						}
						else if (abstractTurret is AbstractMiningTurret)
						{
							this.miningEquivalentTurrets += abstractTurret.turretEquivalentRating;
						}
						else if (abstractTurret is AbstractSalvageTurret)
						{
							this.salvageEquivalentTurrets += abstractTurret.turretEquivalentRating;
						}
					}
				}
				if (this.droneBayModule)
				{
					for (int j = 0; j < this.droneBayModule.droneAmount; j++)
					{
						Drone droneLoadout = this.GetDroneLoadout(j, SeededRandom.Global);
						if (droneLoadout)
						{
							AbstractTurret abstractTurret2 = (droneLoadout != null) ? droneLoadout.GetDefaultTurret() : null;
							if (abstractTurret2 is AbstractCombatTurret)
							{
								this.combatEquivalentTurrets += abstractTurret2.turretEquivalentRating;
							}
							else if (abstractTurret2 is AbstractMiningTurret)
							{
								this.miningEquivalentTurrets += abstractTurret2.turretEquivalentRating;
							}
							else if (abstractTurret2 is AbstractSalvageTurret)
							{
								this.salvageEquivalentTurrets += abstractTurret2.turretEquivalentRating;
							}
						}
					}
				}
			}
			if (this.calcedStats == null)
			{
				int num = 0;
				foreach (object obj in Enum.GetValues(typeof(EquipStat)))
				{
					num = Math.Max(num, (int)obj + 1);
				}
				this.calcedStats = new float[num];
				this.statMultipliers = new float[num];
			}
			else
			{
				Array.Clear(this.calcedStats, 0, this.calcedStats.Length);
			}
			Array.Fill<float>(this.statMultipliers, 1f);
			this.statsTimestamp = Time.time;
			foreach (IEquipStatSource equipStatSource in base.GetComponentsInChildren<IEquipStatSource>())
			{
				foreach (EquipStatLine equipStatLine in equipStatSource.GetStats())
				{
					if (!(this is Drone))
					{
						AbstractTurret abstractTurret3 = equipStatSource as AbstractTurret;
						if (abstractTurret3 != null && abstractTurret3.powerStat == equipStatLine.stat && !abstractTurret3.active)
						{
							continue;
						}
					}
					if (!(this is Torpedo))
					{
						AbstractEquipment abstractEquipment = equipStatSource as AbstractEquipment;
						if (abstractEquipment != null && abstractEquipment.parent is Torpedo)
						{
							continue;
						}
					}
					this.calcedStats[(int)equipStatLine.stat] += equipStatLine.amount;
					this.statMultipliers[(int)equipStatLine.stat] *= equipStatLine.multiplier;
					if (equipStatLine.stat == EquipStat.Power)
					{
						this.calcedStats[21] += equipStatLine.amount;
						this.calcedStats[22] += equipStatLine.amount;
						this.calcedStats[23] += equipStatLine.amount;
						this.statMultipliers[21] *= equipStatLine.multiplier;
						this.statMultipliers[22] *= equipStatLine.multiplier;
						this.statMultipliers[23] *= equipStatLine.multiplier;
					}
				}
			}
			if (this.reactorModule != null && this.IsPlayer(true))
			{
				float energyBonusOrPenalty = this.reactorModule.energyBonusOrPenalty;
				float num2 = 1f + energyBonusOrPenalty;
				foreach (int num3 in AbstractUnit.reactorAffectedStats)
				{
					if (num3 == 21 && this.reactorModule.energyUsage <= 0.5f)
					{
						this.statMultipliers[num3] *= num2 + SkilltreeNode.combatReactorOutputCP.currentIncrease;
					}
					else
					{
						this.statMultipliers[num3] *= num2;
					}
				}
			}
			this.IsPlayer(true);
			if (this.faction != Faction.player)
			{
				foreach (EquipStat equipStat in AbstractUnit.npcAffectedStats)
				{
					if (equipStat.IsPercentageStat())
					{
						this.calcedStats[(int)equipStat] += this.unitData.unitRank.GetStatAddition();
					}
					else
					{
						this.statMultipliers[(int)equipStat] *= this.unitData.unitRank.GetStatMultiplier();
					}
				}
				foreach (EquipStat equipStat2 in AbstractUnit.npcHealthAffectedStats)
				{
					this.statMultipliers[(int)equipStat2] *= this.unitData.unitRank.GetStatMultiplier();
				}
			}
			this.AddStatModifiers(this.calcedStats, this.statMultipliers);
			if (!this.inShipYard)
			{
				AbstractUnitData unitData = this.unitData;
				if (unitData != null)
				{
					unitData.SetCargoCapacity(this.cargoCapacity);
				}
				this.unitData.maxHullHP = this.maxHullHP;
				this.unitData.maxShieldHP = this.maxShieldHP;
				this.unitData.maxArmorHP = this.maxArmorHP;
			}
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0006CAC4 File Offset: 0x0006ACC4
		protected virtual void AddStatModifiers(float[] calcedStats, float[] statMultipliers)
		{
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0006CAC8 File Offset: 0x0006ACC8
		public List<StatsInfoItem> GetStatsInfoItems(EquipStat stat)
		{
			List<StatsInfoItem> list = new List<StatsInfoItem>();
			foreach (IEquipStatSource child in base.GetComponentsInChildren<IEquipStatSource>())
			{
				StatsInfoItem statsInfoItem = AbstractUnit.GetStatLine(stat, child);
				if (stat.IsPowerStat())
				{
					StatsInfoItem statLine = AbstractUnit.GetStatLine(EquipStat.Power, child);
					if (statsInfoItem == null)
					{
						statsInfoItem = statLine;
					}
					else if (statLine != null)
					{
						StatsInfoItem statsInfoItem2 = statsInfoItem;
						statsInfoItem2.stat.amount = statsInfoItem2.stat.amount + statLine.stat.amount;
						StatsInfoItem statsInfoItem3 = statsInfoItem;
						statsInfoItem3.stat.multiplier = statsInfoItem3.stat.multiplier * statLine.stat.multiplier;
					}
				}
				if (statsInfoItem != null)
				{
					list.Add(statsInfoItem);
				}
			}
			return list;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0006CB64 File Offset: 0x0006AD64
		private static StatsInfoItem GetStatLine(EquipStat stat, IEquipStatSource child)
		{
			EquipStatLine? statLine = child.GetStatLine(stat);
			if (statLine != null)
			{
				return new StatsInfoItem
				{
					source = child.GetName(),
					stat = statLine.Value
				};
			}
			return null;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0006CBA4 File Offset: 0x0006ADA4
		public void LoadHardpoint(InventoryItemType item, Transform hardpoint)
		{
			SpaceShipHardpoint component = hardpoint.GetComponent<SpaceShipHardpoint>();
			hardpoint.DestroyChildren();
			if (!item)
			{
				component.SetItem(null);
				return;
			}
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(item, hardpoint);
			inventoryItemType.SetEquipmentInitialized(item.equipmentInitialized);
			inventoryItemType.InitializeItem(item);
			AbstractTurret component2 = item.GetComponent<AbstractTurret>();
			component.SetItem(item);
			if (!component2.canRotate)
			{
				inventoryItemType.transform.Rotate(Vector3.forward, component.rotate);
			}
			AbstractEquipment component3 = inventoryItemType.GetComponent<AbstractEquipment>();
			this.ConnectEquipmentToReactor(component3);
			component3.SetupInWorld();
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0006CC27 File Offset: 0x0006AE27
		private void SetReactorModule(ReactorModule reactor)
		{
			this.reactorModule = reactor;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0006CC30 File Offset: 0x0006AE30
		private void SetShieldGeneratorModule(ShieldGeneratorModule shieldGeneratorModule)
		{
			this.shieldGeneratorModule = shieldGeneratorModule;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0006CC39 File Offset: 0x0006AE39
		private void SetArmorModule(ArmorModule armorModule)
		{
			this.armorModule = armorModule;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0006CC42 File Offset: 0x0006AE42
		private void SetDroneBayModule(DroneBayModule droneBayModule)
		{
			this.droneBayModule = droneBayModule;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0006CC4B File Offset: 0x0006AE4B
		private void SetTorpedoBayModule(TorpedoBayModule torpedoBayModule)
		{
			this.torpedoBayModule = torpedoBayModule;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0006CC54 File Offset: 0x0006AE54
		private void ConnectEquipmentToReactor(AbstractEquipment equipment)
		{
			if (this.reactorModule)
			{
				this.reactorModule.ConnectEquipment(equipment);
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0006CC70 File Offset: 0x0006AE70
		public void CreateStructure(Sprite sprite)
		{
			if (!this.structure)
			{
				GameObject gameObject = new GameObject("Structure" + base.name);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0.05f);
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = new Vector3(0.97f, 0.97f, 1f);
				gameObject.layer = base.gameObject.layer;
				this.structure = gameObject.AddComponent<SpriteRenderer>();
				this.structure.sprite = sprite;
				this.GiveSkeletonTreatment(this.structure);
				this.structureCollider = gameObject.AddComponent<PolygonCollider2D>();
				TargetableUnit.UpdateCollider(this.structureCollider, this.structureSprite, true);
				Physics2D.IgnoreCollision(this.structureCollider, base.surfaceCollider);
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0006CD68 File Offset: 0x0006AF68
		private void GiveSkeletonTreatment(SpriteRenderer renderer)
		{
			renderer.color = new Color(0.25f, 0.25f, 0.25f, 1f);
			renderer.material = Materials.Grayscale;
			SpriteRenderer spriteRenderer = base.spriteRenderer;
			renderer.sortingLayerName = (((spriteRenderer != null) ? spriteRenderer.sortingLayerName : null) ?? "Player");
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0006CDC0 File Offset: 0x0006AFC0
		public void CloneBaseSprite()
		{
			this.surfaceSprite = base.GetComponent<SpriteRenderer>();
			this.originalSprite = this.surfaceSprite.sprite;
			if (this.surfaceSprite)
			{
				Sprite sprite = SpriteHelper.CopySprite(this.surfaceSprite.sprite);
				Sprite sprite2 = SpriteHelper.CopySprite(this.surfaceSprite.sprite);
				if (sprite != null)
				{
					this.surfaceSprite.sprite = sprite;
				}
				else
				{
					this.surfaceSprite = null;
				}
				if (sprite2 != null)
				{
					this.structureSprite = sprite2;
					return;
				}
			}
			else
			{
				this.surfaceSprite = null;
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0006CE50 File Offset: 0x0006B050
		public SpriteBreakPoint BreakSpriteOnDamage(Sprite sprite, Vector2 hitPosition, int breakAmount, bool core = false, bool spawnChunk = true)
		{
			if (sprite)
			{
				SpriteBreakPoint spriteBreakPoint = new SpriteBreakPoint(new Vector2(hitPosition.x / base.transform.localScale.x, hitPosition.y / base.transform.localScale.y), breakAmount, core);
				this.unitData.battleDamage.Add(spriteBreakPoint);
				if (spawnChunk)
				{
					if (core)
					{
						this.ShowBattleDamage(this.structureSprite, spriteBreakPoint, true, true);
					}
					spriteBreakPoint.surfaceDelayedSprite = this.ShowBattleDamage(sprite, spriteBreakPoint, true, false);
				}
				return spriteBreakPoint;
			}
			return null;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0006CEE0 File Offset: 0x0006B0E0
		public BreakDelayedSprite ShowBattleDamage(Sprite sprite, SpriteBreakPoint breakPoint, bool spawnChunk, bool isStructural = false)
		{
			float pixelsPerUnit = sprite.pixelsPerUnit;
			Vector2 position = breakPoint.position;
			Vector2Int breakPosition = new Vector2Int(Mathf.RoundToInt(position.x * pixelsPerUnit + (float)(sprite.texture.width / 2)), Mathf.RoundToInt(position.y * pixelsPerUnit + (float)(sprite.texture.height / 2)));
			breakPosition.x = Mathf.Clamp(breakPosition.x, 1, sprite.texture.width - 2);
			breakPosition.y = Mathf.Clamp(breakPosition.y, 1, sprite.texture.height - 2);
			BreakDelayedSprite breakDelayedSprite = AsteroidHelper.BreakAsteroidSprite(sprite, breakPosition, breakPoint, null);
			if (spawnChunk && breakDelayedSprite.childSprite)
			{
				this.CreateChunk(breakDelayedSprite, position, isStructural);
			}
			return breakDelayedSprite;
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0006CFA4 File Offset: 0x0006B1A4
		public void CreateChunk(BreakDelayedSprite chunkSprite, Vector2 hitLocation, bool isStructural = false)
		{
			SpriteRenderer chunkObj = this.CreateSpriteChunk(chunkSprite.childSprite, isStructural);
			chunkSprite.spawnedChunkCollider = chunkObj.GetComponent<PolygonCollider2D>();
			chunkSprite.onComplete = delegate()
			{
				TargetableUnit.UpdateCollider(chunkSprite.spawnedChunkCollider, chunkSprite.childSprite, false);
				if (!isStructural && this.surfaceCollider && this.surfaceSprite && !this.isActiveAndEnabled)
				{
					TargetableUnit.UpdateCollider(this.surfaceCollider, this.surfaceSprite.sprite, true);
				}
				this.AddEffectsForSpawnedChunk(hitLocation, chunkObj.gameObject, chunkSprite.spawnedChunkCollider, 1f);
			};
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0006D020 File Offset: 0x0006B220
		public SpriteRenderer CreateSpriteChunk(Sprite sprite, bool isStructural)
		{
			SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(this.damageChunkPrefab, base.transform.position + new Vector3(0f, 0f, -1f), base.transform.rotation, BasePoiManager.current ? BasePoiManager.current.transform : base.transform.parent);
			spriteRenderer.transform.localScale = base.transform.localScale;
			spriteRenderer.sprite = sprite;
			if (isStructural)
			{
				this.GiveSkeletonTreatment(spriteRenderer);
			}
			PolygonCollider2D component = spriteRenderer.GetComponent<PolygonCollider2D>();
			this.DisablePhysicsForObject(component);
			return spriteRenderer;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0006D0C1 File Offset: 0x0006B2C1
		private void DisablePhysicsForObject(Collider2D collider)
		{
			Physics2D.IgnoreCollision(collider, base.surfaceCollider);
			Physics2D.IgnoreCollision(collider, this.structureCollider);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0006D0DB File Offset: 0x0006B2DB
		public virtual void TryRestoreSpriteToOriginal()
		{
			this.unitData.battleDamage.Clear();
			if (base.spriteRenderer != null)
			{
				base.spriteRenderer.sprite = this.originalSprite;
				this.CloneBaseSprite();
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0006D114 File Offset: 0x0006B314
		public void RepairSpritePixels(float frac)
		{
			int num = Mathf.RoundToInt((float)this.GetSurfacePixels() * frac);
			int width = this.originalSprite.texture.width;
			int height = this.originalSprite.texture.height;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (flag)
				{
					switch (SeededRandom.Global.RandomRange(0, 4))
					{
					case 0:
						num2++;
						break;
					case 1:
						num2--;
						break;
					case 2:
						num3++;
						break;
					case 3:
						num3--;
						break;
					}
				}
				else
				{
					num2 = SeededRandom.Global.RandomRange(0, width);
					num3 = SeededRandom.Global.RandomRange(0, height);
				}
				flag = false;
				if (base.spriteRenderer.sprite.texture.GetPixel(num2, num3).a == 0f)
				{
					Color pixel = this.originalSprite.texture.GetPixel(num2, num3);
					if (pixel.a != 0f)
					{
						base.spriteRenderer.sprite.texture.SetPixel(num2, num3, pixel);
						flag = true;
					}
				}
			}
			base.spriteRenderer.sprite.texture.Apply();
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0006D250 File Offset: 0x0006B450
		public bool DoWeUseAmmo(InventoryItemType ammo)
		{
			if (this.torpedoBayModule && this.torpedoBayModule.ammoType == ammo)
			{
				return true;
			}
			foreach (AbstractTurret abstractTurret in base.GetComponentsInChildren<AbstractTurret>())
			{
				if (abstractTurret.ammoType && abstractTurret.ammoType == ammo)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0006D2B8 File Offset: 0x0006B4B8
		public bool AmmoInCargoForTurrets(bool idleCheck = false)
		{
			if (this.torpedoBayModule && this.torpedoBayModule.ammoType)
			{
				int count = idleCheck ? (GamePlayer.current.autopilotSettings.ammoSeconds / 2) : 5;
				if (!this.unitData.cargo.HasItem(this.torpedoBayModule.ammoType, count))
				{
					return false;
				}
			}
			foreach (AbstractTurret abstractTurret in base.GetComponentsInChildren<AbstractTurret>())
			{
				if (abstractTurret.ammoType)
				{
					int count2 = idleCheck ? ((int)(abstractTurret.defaultAttacksPerSecond * (float)GamePlayer.current.autopilotSettings.ammoSeconds / (float)abstractTurret.shotsPerAmmo)) : abstractTurret.maxMagSize;
					if (!this.unitData.cargo.HasItem(abstractTurret.ammoType, count2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0006D38C File Offset: 0x0006B58C
		public Dictionary<InventoryItemType, int> GetAmmoTypesRequired()
		{
			Dictionary<InventoryItemType, int> dictionary = new Dictionary<InventoryItemType, int>();
			if (this.torpedoBayModule)
			{
				int count = GamePlayer.current.autopilotSettings.ammoSeconds / 2;
				if (!this.unitData.cargo.HasItem(this.torpedoBayModule.ammoType, count))
				{
					dictionary.Add(this.torpedoBayModule.ammoType, 1);
				}
			}
			foreach (AbstractTurret abstractTurret in base.GetComponentsInChildren<AbstractTurret>())
			{
				if (abstractTurret.ammoType)
				{
					int count2 = (int)(abstractTurret.defaultAttacksPerSecond * (float)GamePlayer.current.autopilotSettings.ammoSeconds / (float)abstractTurret.shotsPerAmmo);
					if (!this.unitData.cargo.HasItem(abstractTurret.ammoType, count2))
					{
						if (dictionary.ContainsKey(abstractTurret.ammoType))
						{
							Dictionary<InventoryItemType, int> dictionary2 = dictionary;
							InventoryItemType ammoType = abstractTurret.ammoType;
							dictionary2[ammoType] += abstractTurret.maxMagSize;
						}
						else
						{
							dictionary.Add(abstractTurret.ammoType, abstractTurret.maxMagSize);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0006D4AC File Offset: 0x0006B6AC
		private bool DoWeChase(AbstractUnit target)
		{
			if (!this.engine)
			{
				return false;
			}
			bool flag = (target.rigidbody.linearVelocity.x > 0f && target.rigidbody.position.x > base.rigidbody.position.x) || (target.rigidbody.linearVelocity.x < 0f && target.rigidbody.position.x < base.rigidbody.position.x);
			return target.speed != 0f && this.speed != 0f && flag;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0006D564 File Offset: 0x0006B764
		private AbstractTargetingModule GetPriorityTargetModule()
		{
			if (this.targetingModules != null)
			{
				this.targetingModules.Sort((AbstractTargetingModule a, AbstractTargetingModule b) => a.targetingPriority.CompareTo(b.targetingPriority));
				foreach (AbstractTargetingModule abstractTargetingModule in this.targetingModules)
				{
					if (abstractTargetingModule.priorityTarget)
					{
						return abstractTargetingModule;
					}
				}
				using (List<AbstractTargetingModule>.Enumerator enumerator = this.targetingModules.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			return null;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0006D638 File Offset: 0x0006B838
		public bool IsEnemy(AbstractUnit unit)
		{
			return unit && this && !this.isSalvage && this.unitData.IsEnemy(unit);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0006D660 File Offset: 0x0006B860
		public bool IsEnemy(Faction f)
		{
			return this.unitData.IsEnemy(f);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0006D66E File Offset: 0x0006B86E
		public bool IsPlayerEnemy()
		{
			return this.unitData.IsPlayerEnemy();
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0006D67B File Offset: 0x0006B87B
		public virtual bool MoveOnCombatDamage()
		{
			return true;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0006D680 File Offset: 0x0006B880
		public override void TakeDamage(DamageData damageData)
		{
			if (!base.enabled)
			{
				return;
			}
			if (base.isDestroyed)
			{
				return;
			}
			if (damageData.sourceUnit)
			{
				base.AddTargetedBy(damageData.sourceUnit, damageData.damageAmount);
				Faction faction = this.faction;
				if (faction != null && !faction.IsEnemy(damageData.sourceUnit.faction) && damageData.sourceUnit.faction == Faction.player)
				{
					foreach (AbstractUnit abstractUnit in BasePoiManager.current.GetComponentsInChildren<AbstractUnit>())
					{
						AbstractUnitData unitData = abstractUnit.unitData;
						if (((unitData != null) ? unitData.faction : null) == this.faction)
						{
							abstractUnit.unitData.playerHostile = true;
						}
					}
				}
			}
			damageData.targetUnit = this;
			AbstractUnit sourceUnit = damageData.sourceUnit;
			if (sourceUnit != null && sourceUnit.IsPlayer(false))
			{
				this.hitByPlayer = true;
			}
			damageData.PreDamageEvents();
			float num = Mathf.Max(0.2f, 1f - this.GetStat(EquipStat.DamageReduction) - this.GetStat(damageData.type.GetResistStat()));
			if (Singleton<TravelManager>.Instance.TravelActive())
			{
				num *= 0.1f;
			}
			damageData.OverrideDamageAmount(damageData.damageAmount * num);
			this.CheckTriggerAbility(AbilityTrigger.BeforeAllDamageTaken, damageData, null);
			UIInfoTextParent.instance.ShowDamageNumber(damageData, base.transform);
			if (this.MoveOnCombatDamage() && damageData.damageForceMovement && this.takingDamageTimer <= 0f)
			{
				this.takingDamageTimer = 3f;
				this.evadeLeft = SeededRandom.Global.RandomBool(0.5f);
			}
			this.lastDamageTime = GamePlayer.current.elapsedTime;
			this.DamagePrevention(damageData);
			if (!this.IsPlayer(true))
			{
				HudManager.Instance.ShowHealthBar(this);
			}
			if (damageData.damageAmount == 0f)
			{
				damageData.PostDamageEvents();
				AutoActions autoActions = this.autoActions;
				if (autoActions == null)
				{
					return;
				}
				autoActions.OnDamageTaken(damageData);
				return;
			}
			else
			{
				this.CheckTriggerAbility(AbilityTrigger.BeforeHullDamageDealt, damageData, null);
				float currentHullHP = Mathf.Clamp(this.currentHullHP - damageData.damageAmount, 0f, this.maxHullHP);
				this.currentHullHP = currentHullHP;
				this.DetermineBattleDamage(damageData);
				if (base.isDestroyed)
				{
					return;
				}
				if (this.currentHullHP <= 0f)
				{
					base.isDestroyed = true;
				}
				damageData.PostDamageEvents();
				AutoActions autoActions2 = this.autoActions;
				if (autoActions2 != null)
				{
					autoActions2.OnDamageTaken(damageData);
				}
				if (this.currentHullHP <= 0f)
				{
					this.CheckTriggerAbility(AbilityTrigger.OnDeath, damageData, null);
					if (this == GameplayManager.Instance.spaceShip)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@EmergencyJump", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
						this.currentHullHP = 0.1f;
						AbstractTurret[] componentsInChildren2 = base.GetComponentsInChildren<AbstractTurret>();
						for (int i = 0; i < componentsInChildren2.Length; i++)
						{
							componentsInChildren2[i].Deactivate();
						}
						if (this.torpedoBayModule)
						{
							this.torpedoBayModule.Deactivate();
						}
						Singleton<TravelManager>.Instance.TravelToClosestSpacestation();
						Register.AddCounter("EmergencyJumps", 1, 0);
						GamePlayer.current.AddAutopilotStat(IdleStat.EmergencyJumps, 1);
						GamePlayer.current.emergencyJump = true;
					}
					else if (this is Drone)
					{
						Singleton<LootManager>.Instance.DropLoot(this, damageData, this.hitByPlayer);
						this.OnDestruction();
					}
					else if (this.IsPlayer(false) && this is SpaceShip)
					{
						this.currentHullHP = 0.1f;
					}
					else
					{
						if (!(this is Torpedo))
						{
							MissionObjective.Trigger(MissionTrigger.UnitDestroyed, new ValueTuple<AbstractUnit, DamageData>(this, damageData), null, false);
						}
						if (this.unitData.deathTrigger != null)
						{
							MissionObjective.Trigger(this.unitData.deathTrigger.Value, new ValueTuple<AbstractUnit, DamageData>(this, damageData), null, false);
						}
						this.ShowEffectsOnDeath();
						Singleton<LootManager>.Instance.DropLoot(this, damageData, this.hitByPlayer);
						MapPointOfInterest current = MapPointOfInterest.current;
						if (current != null)
						{
							current.RemoveUnit(this.unitData);
						}
						if (this.autoActions is AmbientMinerActions)
						{
							MissionObjective.Trigger(MissionTrigger.MinerChasedOff, this.unitData, null, false);
						}
						else if (this.autoActions is AmbientSalvagerActions)
						{
							MissionObjective.Trigger(MissionTrigger.SalvagerChasedOff, this.unitData, null, false);
						}
						base.StartCoroutine(this.CreateRemainsWithPotentialSalvage());
					}
					if (this.hitByPlayer)
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Kills, 1);
						return;
					}
				}
				else
				{
					this.CheckTriggerAbility(AbilityTrigger.OnHullDamageTaken, damageData, null);
				}
				return;
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0006DAC3 File Offset: 0x0006BCC3
		protected virtual void TargetCleanup()
		{
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0006DAC5 File Offset: 0x0006BCC5
		public virtual void OnDestruction()
		{
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0006DAC8 File Offset: 0x0006BCC8
		public void AddDefenseMasteryXp(float exp)
		{
			float num = 0.1f;
			Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Defense));
			SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(tree, false);
			if (skillTreeData == null)
			{
				return;
			}
			skillTreeData.AddMasteryXp(exp * num);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0006DB06 File Offset: 0x0006BD06
		private int GetSurfacePixels()
		{
			if (this.surfacePixels == 0)
			{
				this.surfacePixels = AsteroidHelper.GetFilledPixelCount(this.surfaceSprite.sprite);
			}
			return this.surfacePixels;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0006DB2C File Offset: 0x0006BD2C
		private IEnumerator CreateRemainsWithPotentialSalvage()
		{
			if (this.remainsCreated)
			{
				yield break;
			}
			this.remainsCreated = true;
			float chanceOfTrue = 0.2f * (1f + SkilltreeNode.salvagingDebrisChance.currentIncrease);
			bool salvage = this is SpaceShip && this.tonnage > 300 && SeededRandom.Global.RandomBool(chanceOfTrue) && !(MapPointOfInterest.current is IndustryStation);
			Vector2 hitPosition = new Vector2((float)(SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * base.GetBoundsX() / 2f, (float)(SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * base.GetBoundsY() / 2f);
			int num = this.GetSurfacePixels();
			int breakAmount = SeededRandom.Global.RandomRange(num / 3, num / 2);
			yield return null;
			SpriteBreakPoint breakPoint = this.BreakSpriteOnDamage(this.surfaceSprite.sprite, hitPosition, breakAmount, true, false);
			BreakDelayedSprite surfaceBattleDamage = this.ShowBattleDamage(this.surfaceSprite.sprite, breakPoint, false, false);
			yield return null;
			BreakDelayedSprite coreBattleDamage = this.ShowBattleDamage(this.structureSprite, breakPoint, false, true);
			yield return null;
			UnitWreckage unitWreckage = this.CreateUnitWreckage(hitPosition, surfaceBattleDamage.childSprite, coreBattleDamage.childSprite, surfaceBattleDamage);
			if (!salvage)
			{
				UnitWreckage unitWreckage2 = this.CreateUnitWreckage(hitPosition, this.surfaceSprite.sprite, this.structureSprite, surfaceBattleDamage);
				Physics2D.IgnoreCollision(unitWreckage.collider, unitWreckage2.collider);
			}
			else
			{
				Debug.Log("Create salvage");
				SpaceShipData spaceShipData = new SpaceShipData(this.identifier, false, null);
				SpriteBreakPoint[] array = new SpriteBreakPoint[this.unitData.battleDamage.Count];
				this.unitData.battleDamage.CopyTo(array);
				spaceShipData.battleDamage = new List<SpriteBreakPoint>(array);
				SalvageData salvageData = new SalvageData
				{
					position = base.transform.position,
					angle = base.rigidbody.rotation,
					velocity = base.rigidbody.linearVelocity,
					angularVelocity = base.rigidbody.angularVelocity,
					shipTemplate = this.identifier,
					shipData = spaceShipData,
					showOutline = true
				};
				this.AddSalvageItemsFromLoot(this.unitData as SpaceShipData, salvageData);
				salvageData.AddScrapContent(this.unitData.level, 0.3f, 2);
				salvageData.AddStructuralContent(this.unitData.level, 2, 0.6f);
				SpaceShip component = BasePoiManager.current.poi.AddPersistable(salvageData).GetComponent<SpaceShip>();
				Physics2D.IgnoreCollision(unitWreckage.collider, component.surfaceCollider);
				Physics2D.IgnoreCollision(unitWreckage.collider, component.structureCollider);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0006DB3C File Offset: 0x0006BD3C
		private void AddSalvageItemsFromLoot(SpaceShipData unitData, SalvageData salvageData)
		{
			foreach (InventoryItemType item in unitData.CreateSalvageTable(SeededRandom.Global.RandomRange(1, 3)))
			{
				salvageData.AddItemContent(item);
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0006DB98 File Offset: 0x0006BD98
		protected BreakDelayedSprite CreateCoreDamage(SpriteBreakPoint breakPoint)
		{
			this.ShowBattleDamage(this.structureSprite, breakPoint, false, true);
			return this.ShowBattleDamage(this.surfaceSprite.sprite, breakPoint, false, false);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0006DBC0 File Offset: 0x0006BDC0
		private UnitWreckage CreateUnitWreckage(Vector2 hitPosition, Sprite surfaceSprite, Sprite coreSprite, BreakDelayedSprite delayedAction)
		{
			UnitWreckage wreckage = UnityEngine.Object.Instantiate<UnitWreckage>(this.wreckagePrefab, base.transform.position + new Vector3(0f, 0f, -0.1f), base.transform.rotation, BasePoiManager.current ? BasePoiManager.current.transform : base.transform.parent);
			wreckage.transform.localScale = base.transform.localScale;
			wreckage.surface.sprite = surfaceSprite;
			wreckage.core.sprite = coreSprite;
			this.GiveSkeletonTreatment(wreckage.core);
			wreckage.rigidbody2D.mass = this.mass * 0.4f;
			wreckage.rigidbody2D.angularVelocity = base.rigidbody.angularVelocity;
			wreckage.rigidbody2D.linearVelocity = base.rigidbody.linearVelocity;
			delayedAction.onComplete = delegate()
			{
				TargetableUnit.UpdateCollider(wreckage.collider, surfaceSprite, true);
				this.AddEffectsForSpawnedChunk(hitPosition, wreckage.gameObject, wreckage.collider, 1f);
			};
			return wreckage;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0006DD0C File Offset: 0x0006BF0C
		protected void ShowEffectsOnDeath()
		{
			int num = Mathf.Clamp(Mathf.CeilToInt(base.radius / 0.7f), 1, 8);
			for (int i = 0; i < num; i++)
			{
				float scale = Mathf.Clamp(SeededRandom.Global.RandomRange(base.radius / 1.5f / (float)(num - i), base.radius * 1.5f / (float)(num - i)), 0.7f, 5f);
				float delay = (float)i * SeededRandom.Global.RandomRange(0.05f, 0.2f);
				Vector2 position = base.rigidbody.position;
				position.x += (float)(SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * SeededRandom.Global.RandomRange(0f, base.GetBoundsX() / 2f);
				position.y += (float)(SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * SeededRandom.Global.RandomRange(0f, base.GetBoundsY() / 3f);
				Singleton<EffectManager>.Instance.PlayExplosionEffect(position, base.rigidbody.linearVelocity, scale, ColorHelper.flashExplosionUnit, delay);
			}
			Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(base.rigidbody.position, base.GetBoundsX() / 8f, 0f);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0006DE6C File Offset: 0x0006C06C
		private void DetermineBattleDamage(DamageData damageData)
		{
			this.unitData.damageTaken += damageData.damageAmount;
			if (this.unitData.damageTaken >= this.healthPerBattleDamage)
			{
				int num = this.GetSurfacePixels();
				int num2 = (int)SeededRandom.Global.RandomRange(Mathf.Max(5f, (float)num / 40f), Mathf.Max(10f, (float)num / 20f));
				num2 = Mathf.Clamp(num2, 5, 180);
				this.unitData.damageTaken -= this.healthPerBattleDamage;
				SpriteRenderer surfaceSprite = this.surfaceSprite;
				this.BreakSpriteOnDamage((surfaceSprite != null) ? surfaceSprite.sprite : null, damageData.hitTransform.localPosition, num2, false, true);
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0006DF30 File Offset: 0x0006C130
		protected void DamagePrevention(DamageData damageData)
		{
			if (this.shieldGeneratorModule)
			{
				this.shieldGeneratorModule.ResetDelayTimer();
				if (this.shieldGeneratorModule.currentShieldCapacity > 0f)
				{
					this.shieldGeneratorModule.TakeDamage(damageData);
				}
			}
			if (damageData.damageAmount > 0f && this.armorModule && this.armorModule.currentArmor > 0f)
			{
				this.armorModule.TakeDamage(damageData);
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0006DFAB File Offset: 0x0006C1AB
		public void Dead(bool status)
		{
			base.isDestroyed = status;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0006DFB4 File Offset: 0x0006C1B4
		private void OnDrawGizmos()
		{
			if (base.rigidbody)
			{
				Vector2 currentDestination = this.currentDestination;
				Gizmos.color = Color.red;
				Gizmos.DrawLine(base.rigidbody.position, this.currentDestination);
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0006DFF4 File Offset: 0x0006C1F4
		public void SetOverrideDestination(Vector2 position, bool clear = false, bool freeRotation = false, bool cancelManualMovement = false)
		{
			this.unitData.overrideTarget = new Vector2?(position);
			this.unitData.clearOverrideWhenReachedDestination = clear;
			this.freeRotation = freeRotation;
			if (cancelManualMovement)
			{
				this.manualInputTimer = 0f;
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0006E029 File Offset: 0x0006C229
		public void ClearOverrideDestination()
		{
			this.unitData.clearOverrideWhenReachedDestination = false;
			this.unitData.overrideTarget = null;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0006E048 File Offset: 0x0006C248
		public void ResetPosition(Vector2? overridePosition = null)
		{
			float x = (GamePlayer.current != null) ? GamePlayer.current.GetLastX() : 0f;
			Vector2 value = new Vector2(x, 0f);
			if (overridePosition != null)
			{
				value = overridePosition.Value;
			}
			base.rigidbody.position = (this.inShipYard ? this.shipyardPosition : value);
			base.transform.position = base.rigidbody.position;
			base.rigidbody.totalForce = Vector2.zero;
			base.rigidbody.totalTorque = 0f;
			base.rigidbody.linearVelocity = Vector2.zero;
			base.rigidbody.angularVelocity = 0f;
			base.isDestroyed = false;
			if (this.inShipYard)
			{
				base.transform.rotation = Quaternion.identity;
			}
			this.currentDestination = base.rigidbody.position;
			base.transform.Z(this.defaultZIndex);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0006E144 File Offset: 0x0006C344
		private void MoveTo(Vector2 moveTo)
		{
			if (!BasePoiManager.current || !BasePoiManager.current.initializedAndReady || (this == GameplayManager.Instance.spaceShip && Singleton<TravelManager>.Instance.TravelActive()))
			{
				return;
			}
			this.currentDestination = moveTo;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0006E184 File Offset: 0x0006C384
		private Vector2 ClampDestination(Vector2 moveTo)
		{
			if (!(this.autoActions is FlyByActions))
			{
				AutoActions autoActions = this.autoActions;
				if (autoActions == null || !autoActions.leaving)
				{
					BasePoiManager localPoiManager = Singleton<TravelManager>.Instance.localPoiManager;
					Rect rect = (localPoiManager != null) ? localPoiManager.worldCoordinates : default(Rect);
					Vector2 center = rect.center;
					float num = rect.height / 2f;
					float num2 = rect.width / 2f;
					return new Vector2(Mathf.Clamp(moveTo.x, center.x - num2, center.x + num2), Mathf.Clamp(moveTo.y, center.y - num, center.y + num));
				}
			}
			return moveTo;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0006E234 File Offset: 0x0006C434
		public void GiveImpulse(Vector2 force, float torque, float delay)
		{
			base.StartCoroutine(this.HandleImpulse(force, torque, delay));
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0006E246 File Offset: 0x0006C446
		public IEnumerator HandleImpulse(Vector2 force, float torque, float delay)
		{
			yield return new WaitForSeconds(delay);
			base.rigidbody.AddForce(force, ForceMode2D.Impulse);
			base.rigidbody.AddTorque(torque, ForceMode2D.Impulse);
			yield break;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0006E26C File Offset: 0x0006C46C
		public void SetBrakeDestination()
		{
			Vector2 a = base.rigidbody.transform.right;
			Vector2 position = base.rigidbody.position + a * this.speed;
			this.SetOverrideDestination(position, true, false, false);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0006E2B8 File Offset: 0x0006C4B8
		public void ApplyManualMovement(Vector2 input, float rotate)
		{
			if (!this.engine || this.inShipYard || this.unitData.travelling || !this.enginesEnabled || Singleton<TravelManager>.Instance.TravelActive() || (BasePoiManager.current != null && !BasePoiManager.current.initializedAndReady))
			{
				return;
			}
			this.manualInputTimer = 10f;
			this.ClearOverrideDestination();
			GamePlayer.current.holdPosition = true;
			GamePlayer.current.EndAutopilotSession();
			this.engine.ApplyManualThrust(input, rotate);
			this.currentDestination = base.rigidbody.position;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0006E35C File Offset: 0x0006C55C
		protected override void FixedUpdate()
		{
			AutoActions autoActions = this.autoActions;
			if (autoActions != null)
			{
				autoActions.Update(Time.deltaTime);
			}
			base.FixedUpdate();
			if (this.inShipYard || this.unitData.travelling || !this.enginesEnabled || (BasePoiManager.current != null && !BasePoiManager.current.initializedAndReady))
			{
				return;
			}
			this.UpdateTarget();
			this.speed = base.rigidbody.linearVelocity.magnitude;
			if (this.manualInputTimer > 0f)
			{
				this.manualInputTimer -= Time.fixedDeltaTime;
			}
			if (this.engine && this.manualInputTimer <= 0f)
			{
				this.ApplyThrust();
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0006E41A File Offset: 0x0006C61A
		protected virtual bool FreeRotation()
		{
			return this.freeRotation;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0006E424 File Offset: 0x0006C624
		private void ApplyThrust()
		{
			if (!BasePoiManager.current || !BasePoiManager.current.initializedAndReady)
			{
				return;
			}
			this.currentDestination = this.ClampDestination(this.currentDestination);
			Vector2 vector = this.currentDestination - base.rigidbody.position;
			float num = Vector3.SignedAngle(vector, base.rigidbody.transform.right, Vector3.forward);
			float magnitude = vector.magnitude;
			bool flag = magnitude < base.radius * 2f;
			bool braking = false;
			if (!this.CanWeApplyPower(num, magnitude) && this.speed > 3f)
			{
				num = 0f;
				vector = base.rigidbody.transform.right * this.speed;
				braking = true;
			}
			bool flag2 = false;
			if (this.forceWorldAngle != null || (this.targetVector != null && flag))
			{
				float num2 = this.GetCurrentAngleToTarget();
				if (!Mathf.Approximately(num2, this.GetAngleToTarget()))
				{
					int num3 = (num2 > 0f) ? -1 : 1;
					num2 += this.GetAngleToTarget() * (float)num3;
					num = num2;
					flag2 = true;
				}
			}
			if (this.FreeRotation() || (num != 0f && !flag) || flag2)
			{
				this.engine.RotateShip(num);
			}
			if (magnitude != 0f && (flag || this.CanWeApplyPower(num, magnitude)))
			{
				this.engine.ForwardThrust(vector, this.targetSpeed, braking);
			}
			if (Mathf.Abs(num) < 2f || flag || this.forceWorldAngle != null)
			{
				this.engine.SideThrust(vector, flag || this.forceWorldAngle != null);
			}
			if (magnitude < 0.5f && this.clearOverrideWhenReachedDestination)
			{
				this.ClearOverrideDestination();
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0006E5F0 File Offset: 0x0006C7F0
		public float GetCurrentAngleToTarget()
		{
			float result = 0f;
			if (this.forceWorldAngle != null)
			{
				return Mathf.DeltaAngle(this.forceWorldAngle.Value.eulerAngles.z, base.rigidbody.rotation);
			}
			if (this.targetVector != null)
			{
				result = Vector3.SignedAngle(this.targetVector.Value, base.rigidbody.transform.right, Vector3.forward);
			}
			return result;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0006E674 File Offset: 0x0006C874
		private bool CanWeApplyPower(float angle, float distanceToDestination)
		{
			return (this.maxSpeed < 0f || (this.maxSpeed > 0f && this.engine.GetForwardSpeed() < this.maxSpeed)) && ((distanceToDestination < base.radius * 5f && Mathf.Abs(angle) < 5f) || (distanceToDestination > base.radius * 5f && Mathf.Abs(angle) < (float)this.MaxFullPowerAngle));
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0006E6EE File Offset: 0x0006C8EE
		public AbstractMiningTurret[] GetMiningTurrets()
		{
			return base.GetComponentsInChildren<AbstractMiningTurret>();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0006E6F6 File Offset: 0x0006C8F6
		public AbstractCombatTurret[] GetCombatTurrets()
		{
			return base.GetComponentsInChildren<AbstractCombatTurret>();
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0006E6FE File Offset: 0x0006C8FE
		public AbstractSalvageTurret[] GetSalvageTurrets()
		{
			return base.GetComponentsInChildren<AbstractSalvageTurret>();
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0006E708 File Offset: 0x0006C908
		public float GetMaxCombatRange()
		{
			float num = 8f;
			foreach (AbstractCombatTurret abstractCombatTurret in this.GetCombatTurrets())
			{
				num = ((abstractCombatTurret.range > num) ? abstractCombatTurret.range : num);
			}
			return num;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0006E748 File Offset: 0x0006C948
		public void InitModules()
		{
			AbstractMiningTurret[] miningTurrets = this.GetMiningTurrets();
			if (miningTurrets.Length == 0)
			{
				DroneBayModule droneBayModule = this.droneBayModule;
				if ((droneBayModule == null || !droneBayModule.HasLoadout(GameplayType.Mining, TargetLayer.Both, null)) && !(this.torpedoBayModule != null))
				{
					goto IL_90;
				}
			}
			MiningModule miningModule = base.GetComponentInChildren<MiningModule>() ?? (UnityEngine.Object.Instantiate<AbstractModule>(AbstractModule.GetDefaultModule("MiningScanner"), base.transform) as MiningModule);
			miningModule.SetMiningTurrets(miningTurrets);
			if (this.droneBayModule != null)
			{
				miningModule.SetDroneBay(this.droneBayModule);
			}
			if (this.torpedoBayModule != null)
			{
				miningModule.SetTorpedoBay(this.torpedoBayModule);
			}
			IL_90:
			AbstractCombatTurret[] combatTurrets = this.GetCombatTurrets();
			if (combatTurrets.Length == 0)
			{
				DroneBayModule droneBayModule2 = this.droneBayModule;
				if ((droneBayModule2 == null || !droneBayModule2.HasLoadout(GameplayType.Combat, TargetLayer.Both, null)) && !(this.torpedoBayModule != null))
				{
					goto IL_124;
				}
			}
			CombatModule combatModule = base.GetComponentInChildren<CombatModule>() ?? (UnityEngine.Object.Instantiate<AbstractModule>(AbstractModule.GetDefaultModule("CombatScanner"), base.transform) as CombatModule);
			combatModule.SetCombatTurrets(combatTurrets);
			if (this.droneBayModule != null)
			{
				combatModule.SetDroneBay(this.droneBayModule);
			}
			if (this.torpedoBayModule != null)
			{
				combatModule.SetTorpedoBay(this.torpedoBayModule);
			}
			IL_124:
			AbstractSalvageTurret[] salvageTurrets = this.GetSalvageTurrets();
			if (salvageTurrets.Length == 0)
			{
				DroneBayModule droneBayModule3 = this.droneBayModule;
				if ((droneBayModule3 == null || !droneBayModule3.HasLoadout(GameplayType.Salvage, TargetLayer.Both, null)) && !(this.torpedoBayModule != null))
				{
					goto IL_1B8;
				}
			}
			SalvageModule salvageModule = base.GetComponentInChildren<SalvageModule>() ?? (UnityEngine.Object.Instantiate<AbstractModule>(AbstractModule.GetDefaultModule("SalvageScanner"), base.transform) as SalvageModule);
			salvageModule.SetSalvageTurrets(salvageTurrets);
			if (this.droneBayModule != null)
			{
				salvageModule.SetDroneBay(this.droneBayModule);
			}
			if (this.torpedoBayModule != null)
			{
				salvageModule.SetTorpedoBay(this.torpedoBayModule);
			}
			IL_1B8:
			this.targetingModules = new List<AbstractTargetingModule>(base.GetComponentsInChildren<AbstractTargetingModule>());
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0006E920 File Offset: 0x0006CB20
		public bool IsPlayer(bool spaceShipOnly = false)
		{
			if (GameplayManager.Instance && this == GameplayManager.Instance.spaceShip)
			{
				return true;
			}
			if (!spaceShipOnly && this.unitData.faction == Faction.player)
			{
				return true;
			}
			Drone drone = this as Drone;
			return drone != null && !spaceShipOnly && drone.IsDroneFromPlayer();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0006E97C File Offset: 0x0006CB7C
		public Vector2 GetShipSize()
		{
			return new Vector2(base.radius, base.height) * 2f;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0006E99C File Offset: 0x0006CB9C
		public float GetSpaceshipSizeWithBuffer(float buffer = 0.5f)
		{
			if (base.spriteRenderer != null)
			{
				Vector2 vector = base.spriteRenderer.bounds.size;
				return Mathf.Max(vector.x, vector.y) + buffer;
			}
			return buffer;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0006E9E5 File Offset: 0x0006CBE5
		public void SetEngineState(bool isEnabled, bool thrusterEnabled = true)
		{
			if (!isEnabled)
			{
				this.StopMovement();
			}
			this.enginesEnabled = isEnabled;
			if (this.engine == null)
			{
				this.SetEngine();
			}
			EngineThrustersModule engine = this.engine;
			if (engine == null)
			{
				return;
			}
			engine.SetThrusterState(thrusterEnabled);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0006EA1C File Offset: 0x0006CC1C
		public void ToggleWeapons(bool enabled)
		{
			foreach (AbstractTurret abstractTurret in base.GetComponentsInChildren<AbstractTurret>())
			{
				if (enabled)
				{
					abstractTurret.Activate();
				}
				else
				{
					abstractTurret.Deactivate();
				}
			}
			if (this.droneBayModule)
			{
				if (enabled)
				{
					this.droneBayModule.Activate();
				}
				else
				{
					this.droneBayModule.Deactivate();
				}
			}
			if (this.torpedoBayModule)
			{
				if (enabled)
				{
					this.torpedoBayModule.Activate();
					return;
				}
				this.torpedoBayModule.Deactivate();
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0006EAA2 File Offset: 0x0006CCA2
		public void SetRigidbodyState(bool isEnabled)
		{
			if (isEnabled)
			{
				base.rigidbody.bodyType = RigidbodyType2D.Dynamic;
				return;
			}
			base.rigidbody.bodyType = RigidbodyType2D.Kinematic;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0006EAC0 File Offset: 0x0006CCC0
		public void StopMovement()
		{
			if (!base.rigidbody)
			{
				return;
			}
			base.rigidbody.linearVelocity = Vector2.zero;
			base.rigidbody.angularVelocity = 0f;
			base.rigidbody.totalForce = Vector2.zero;
			base.rigidbody.totalTorque = 0f;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0006EB1C File Offset: 0x0006CD1C
		public bool IsNearWorldPosition(Vector2 worldPosition)
		{
			float num = 0.5f;
			return base.rigidbody.position.x < worldPosition.x + num && base.rigidbody.position.x > worldPosition.x - num && base.rigidbody.position.y < worldPosition.y + num && base.rigidbody.position.y > worldPosition.y - num;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0006EB99 File Offset: 0x0006CD99
		public void SetDisplayName(string name)
		{
			this.displayName = name;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0006EBA2 File Offset: 0x0006CDA2
		public bool CompletelyCrossedThreshold(float x, bool right)
		{
			if (right)
			{
				return x < base.transform.position.x - base.GetBoundsX();
			}
			return !right && x > base.transform.position.x + base.GetBoundsX();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0006EBE1 File Offset: 0x0006CDE1
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return !(turret is AbstractMiningTurret) && !(turret is AbstractSalvageTurret);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0006EBF9 File Offset: 0x0006CDF9
		public virtual void AddCrewExperience(float experience, CommanderSpecialization? skillTree = null, bool showFloating = true)
		{
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0006EBFB File Offset: 0x0006CDFB
		public Collider2D GetSurfaceCollider()
		{
			return this.collider;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0006EC03 File Offset: 0x0006CE03
		public Vector2 GetTargetablePosition()
		{
			return base.transform.position;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0006EC15 File Offset: 0x0006CE15
		public Vector2 GetTargetableVelocity()
		{
			return base.rigidbody.linearVelocity;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0006EC22 File Offset: 0x0006CE22
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (HudManager.Instance)
			{
				HudManager.Instance.RemoveHealthBar(this);
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0006EC44 File Offset: 0x0006CE44
		private void OnCollisionStay2D(Collision2D collision)
		{
			this.collisionContactTime += Time.deltaTime;
			if (this.collisionContactTime > 2f)
			{
				bool flag = false;
				if (this.IgnoreCollision(collision))
				{
					flag = true;
				}
				else if (collision.gameObject.layer == LayerMask.NameToLayer("AsteroidChunk"))
				{
					flag = true;
				}
				if (flag)
				{
					Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
					base.StartCoroutine(this.RestoreCollision(collision.collider, collision.otherCollider));
				}
				this.collisionContactTime = 0f;
			}
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0006ECD0 File Offset: 0x0006CED0
		private bool IgnoreCollision(Collision2D collision)
		{
			AbstractUnit abstractUnit;
			collision.gameObject.TryGetComponent<AbstractUnit>(out abstractUnit);
			bool flag = this.IsPlayer(false) || abstractUnit == null || abstractUnit.IsPlayer(false);
			bool flag2 = (abstractUnit && !abstractUnit.IsEnemy(this) && flag) || (flag && Singleton<TravelManager>.Instance.TravelActive());
			this.collisionIsSalvage = ((abstractUnit != null) ? abstractUnit.IsSalvage() : null);
			return flag2 && !this.collisionIsSalvage && !this.IsSalvage();
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0006ED64 File Offset: 0x0006CF64
		public SalvageContainer IsSalvage()
		{
			SalvageContainer result;
			base.TryGetComponent<SalvageContainer>(out result);
			return result;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0006ED7B File Offset: 0x0006CF7B
		private IEnumerator RestoreCollision(Collider2D collider, Collider2D otherCollider)
		{
			yield return new WaitForSeconds(10f);
			if (collider && otherCollider)
			{
				Physics2D.IgnoreCollision(collider, otherCollider, false);
			}
			yield break;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0006ED94 File Offset: 0x0006CF94
		public virtual float GetEquivalentTurretsCount(EquipStat powerStat)
		{
			float result;
			if (powerStat != EquipStat.MiningPower)
			{
				if (powerStat != EquipStat.SalvagePower)
				{
					result = this.combatEquivalentTurrets;
				}
				else
				{
					result = this.salvageEquivalentTurrets;
				}
			}
			else
			{
				result = this.miningEquivalentTurrets;
			}
			return result;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0006EDC7 File Offset: 0x0006CFC7
		public Drone GetDroneLoadout(int idx, SeededRandom random)
		{
			return this.unitData.GetDroneLoadout(idx, random);
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0006EDD6 File Offset: 0x0006CFD6
		public bool CanBeForceFired()
		{
			return !this.IsPlayerEnemy() && !this.IsPlayer(false) && SystemMapData.current.sector.quadrant > 1 && !(MapPointOfInterest.current is SpaceStation);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0006EED2 File Offset: 0x0006D0D2
		bool IDamageable.enabled => base.enabled;

		// Token: 0x040008C7 RID: 2247
		public const float BaseCritChance = 0.03f;

		// Token: 0x040008C8 RID: 2248
		public const float BaseCritDamage = 1f;

		// Token: 0x040008C9 RID: 2249
		public const float StatRecalcTimer = 0.5f;

		// Token: 0x040008CA RID: 2250
		public const float StoppingMargin = 0.5f;

		// Token: 0x040008CB RID: 2251
		public const int MinimumTonnageForSalvage = 300;

		// Token: 0x040008CC RID: 2252
		public static EquipStat[] reactorAffectedStats = new EquipStat[]
		{
			EquipStat.Power,
			EquipStat.CombatPower,
			EquipStat.MiningPower,
			EquipStat.SalvagePower
		};

		// Token: 0x040008CD RID: 2253
		public static EquipStat[] npcAffectedStats = new EquipStat[]
		{
			EquipStat.ReloadSpeed,
			EquipStat.AttackSpeed
		};

		// Token: 0x040008CE RID: 2254
		public static EquipStat[] npcHealthAffectedStats = new EquipStat[]
		{
			EquipStat.HullHP,
			EquipStat.ArmorHP,
			EquipStat.ShieldHP
		};

		// Token: 0x040008DD RID: 2269
		private float combatEquivalentTurrets;

		// Token: 0x040008DE RID: 2270
		private float miningEquivalentTurrets;

		// Token: 0x040008DF RID: 2271
		private float salvageEquivalentTurrets;

		// Token: 0x040008E0 RID: 2272
		[SerializeField]
		protected SpriteRenderer damageChunkPrefab;

		// Token: 0x040008E1 RID: 2273
		[SerializeField]
		protected UnitWreckage wreckagePrefab;

		// Token: 0x040008E3 RID: 2275
		public int surfacePixels;

		// Token: 0x040008E4 RID: 2276
		protected Sprite originalSprite;

		// Token: 0x040008E6 RID: 2278
		protected SpriteRenderer structure;

		// Token: 0x040008E8 RID: 2280
		[FieldColor(0f, 0f, 0.8f, 0.15f, 1f, 0.8f, 0.8f)]
		public float maxSpeed = -1f;

		// Token: 0x040008EA RID: 2282
		private GameObject followTarget;

		// Token: 0x040008ED RID: 2285
		protected List<AbstractTargetingModule> targetingModules;

		// Token: 0x040008EE RID: 2286
		[FieldColor(0f, 0f, 0.8f, 0.15f, 1f, 0.8f, 0.8f)]
		public float angleToTarget;

		// Token: 0x040008EF RID: 2287
		private float targetTimer;

		// Token: 0x040008F0 RID: 2288
		public Quaternion? forceWorldAngle;

		// Token: 0x040008F2 RID: 2290
		public bool freeRotation;

		// Token: 0x040008F3 RID: 2291
		protected Vector2? targetVector;

		// Token: 0x040008F4 RID: 2292
		private Vector2 targetSpeed;

		// Token: 0x040008F5 RID: 2293
		private float approachDistance = 2f;

		// Token: 0x040008F6 RID: 2294
		protected Collider2D collider;

		// Token: 0x040008FD RID: 2301
		private bool enginesEnabled = true;

		// Token: 0x040008FE RID: 2302
		private float statsTimestamp;

		// Token: 0x040008FF RID: 2303
		private float[] calcedStats;

		// Token: 0x04000900 RID: 2304
		private float[] statMultipliers;

		// Token: 0x04000902 RID: 2306
		private Vector2 shipyardPosition;

		// Token: 0x04000904 RID: 2308
		private float takingDamageTimer;

		// Token: 0x04000905 RID: 2309
		public bool evadeLeft;

		// Token: 0x04000906 RID: 2310
		private float collisionTimer;

		// Token: 0x04000907 RID: 2311
		private float collisionContactTime;

		// Token: 0x04000909 RID: 2313
		public float manualInputTimer;

		// Token: 0x0400090A RID: 2314
		public double lastDamageTime = -1000.0;

		// Token: 0x0400090B RID: 2315
		private AbstractTurret[] turrets;

		// Token: 0x0400090D RID: 2317
		private bool remainsCreated;
	}
}
