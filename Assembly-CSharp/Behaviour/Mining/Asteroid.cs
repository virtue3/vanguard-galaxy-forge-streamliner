using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Projectiles.Mining;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Crew;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Mining
{
	// Token: 0x020002FB RID: 763
	public class Asteroid : TargetableUnit, ITooltipTitleSource, ITooltipCustomSource
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000A8872 File Offset: 0x000A6A72
		public AsteroidSize asteroidSize
		{
			get
			{
				return this.asteroidData.size;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000A887F File Offset: 0x000A6A7F
		public OreItemData surfaceItem
		{
			get
			{
				return this.asteroidData.surfaceItem;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000A888C File Offset: 0x000A6A8C
		private int surfaceAmount
		{
			get
			{
				return this.asteroidData.surfaceAmount;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000A8899 File Offset: 0x000A6A99
		private float surfaceExp
		{
			get
			{
				return this.GetMiningExperience(this.surfaceItem);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x000A88A7 File Offset: 0x000A6AA7
		public bool surfaceAmountDepleted
		{
			get
			{
				return this.surfaceAmount == 0;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x000A88B2 File Offset: 0x000A6AB2
		public bool hasSurfaceOre
		{
			get
			{
				return this.surfaceAmount > 0;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x000A88BD File Offset: 0x000A6ABD
		public override int currentSurfaceHealth
		{
			get
			{
				return this.asteroidData.currentSurfaceHealth;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x000A88CA File Offset: 0x000A6ACA
		public override int maxSurfaceHealth
		{
			get
			{
				return this.asteroidData.surfaceMaxHealth;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000A88D7 File Offset: 0x000A6AD7
		private int acumulatedSurfaceDamage
		{
			get
			{
				return this.asteroidData.accumulatedSurfaceDamage;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x000A88E4 File Offset: 0x000A6AE4
		public bool hasInnerCore
		{
			get
			{
				return this.innerCoreAmount > 0;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000A88EF File Offset: 0x000A6AEF
		public OreItemData innerCoreItem
		{
			get
			{
				return this.asteroidData.innerCoreItem;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x000A88FC File Offset: 0x000A6AFC
		private int innerCoreAmount
		{
			get
			{
				return this.asteroidData.innerCoreAmount;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000A8909 File Offset: 0x000A6B09
		public override int maxCoreHealth
		{
			get
			{
				return this.asteroidData.innerCoreMaxHealth;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x000A8916 File Offset: 0x000A6B16
		private float innerCoreExp
		{
			get
			{
				return this.GetMiningExperience(this.innerCoreItem);
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x000A8924 File Offset: 0x000A6B24
		public bool innerCoreDepleted
		{
			get
			{
				return this.innerCoreAmount == 0;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x000A892F File Offset: 0x000A6B2F
		public override int currentCoreHealth
		{
			get
			{
				return this.asteroidData.currentInnerCoreHealth;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x000A893C File Offset: 0x000A6B3C
		private int acumulatedInnerCoreDamage
		{
			get
			{
				return this.asteroidData.accumulatedInnerCoreDamage;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x000A8949 File Offset: 0x000A6B49
		private float scale
		{
			get
			{
				return this.asteroidData.scale;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x000A8956 File Offset: 0x000A6B56
		// (set) Token: 0x06001BDA RID: 7130 RVA: 0x000A895E File Offset: 0x000A6B5E
		public bool isDetonating { get; private set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x000A8967 File Offset: 0x000A6B67
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x000A896F File Offset: 0x000A6B6F
		public int scanningProgress { get; protected set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x000A8978 File Offset: 0x000A6B78
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x000A8980 File Offset: 0x000A6B80
		public PolygonCollider2D coreCollider { get; private set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x000A8989 File Offset: 0x000A6B89
		public int maxAttachedDrones
		{
			get
			{
				return this.asteroidSize.GetMaxDronesPerSize();
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x000A8996 File Offset: 0x000A6B96
		public override string targetName
		{
			get
			{
				return this.GetTooltipTitle();
			}
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000A89A0 File Offset: 0x000A6BA0
		public void InitAsteroid(AsteroidData asteroidData, bool createSprites = true)
		{
			this.asteroidData = asteroidData;
			this.coreCollider = this.coreSprite.GetComponent<PolygonCollider2D>();
			base.rigidbody.mass = this.scale * 20f;
			this.scanningProgress = this.asteroidData.scanProgress;
			if (createSprites)
			{
				this.CreateSprites(asteroidData);
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000A89F7 File Offset: 0x000A6BF7
		protected override void Start()
		{
			base.transform.Z(ZIndex.Asteroid);
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000A8A08 File Offset: 0x000A6C08
		protected override void Update()
		{
			base.Update();
			if (this.preventLockedTimer >= 0f)
			{
				this.preventLockedTimer -= Time.deltaTime;
			}
			Rigidbody2D rigidbody = base.rigidbody;
			if (rigidbody != null && rigidbody.linearVelocity.magnitude > 0f)
			{
				float distanceFromBorder = base.GetDistanceFromBorder();
				if (distanceFromBorder < 2f)
				{
					base.rigidbody.linearDamping = 10f;
				}
				if (distanceFromBorder < 0f)
				{
					base.rigidbody.linearDamping = 20f;
				}
			}
			this.SlowDownRotationToMax();
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000A8A98 File Offset: 0x000A6C98
		private void SlowDownRotationToMax()
		{
			if (Mathf.Abs(base.rigidbody.angularVelocity) > this.maxAngularSpeed)
			{
				float b = Mathf.Sign(base.rigidbody.angularVelocity) * this.maxAngularSpeed;
				base.rigidbody.angularVelocity = Mathf.Lerp(base.rigidbody.angularVelocity, b, this.rotationDamping * Time.fixedDeltaTime);
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000A8B00 File Offset: 0x000A6D00
		public void CreateSprites(AsteroidData asteroidData)
		{
			AsteroidSize size = this.asteroidSize;
			if (asteroidData.parentSize != null)
			{
				size = asteroidData.parentSize.Value;
			}
			this.initialSurfaceSprite = AsteroidHelper.CreateAsteroidSprite(size, this.surfaceItem, asteroidData.seed, 0.1f, null);
			this.initialCoreSprite = AsteroidHelper.CreateAsteroidSprite(size, this.hasInnerCore ? this.innerCoreItem : null, asteroidData.seed, 0.25f, new Color?(new Color32(58, 46, 37, byte.MaxValue)));
			this.SetSurfaceSprite(this.initialSurfaceSprite);
			this.SetCoreSprite(this.initialCoreSprite);
			int index = 0;
			if (asteroidData.breakPoints.Count > 0)
			{
				this.ProcessBreakpoints(index);
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000A8BCC File Offset: 0x000A6DCC
		public void ProcessBreakpoints(int index)
		{
			if (index >= this.asteroidData.breakPoints.Count)
			{
				this.SetSurfaceSprite(this.initialSurfaceSprite);
				this.SetCoreSprite(this.initialCoreSprite);
				return;
			}
			SpriteBreakPoint breakPoint = this.asteroidData.breakPoints[index];
			BreakDelayedSprite tempCoreSprite = null;
			BreakDelayedSprite tempSurfaceSprite = null;
			tempSurfaceSprite = this.BreakOffAsteroid(this.initialSurfaceSprite, breakPoint, false);
			Action _delegate_1 = null;
			tempSurfaceSprite.onComplete = delegate()
			{
				if (breakPoint.core)
				{
					BreakDelayedSprite tempCoreSprite = this.BreakOffAsteroid(this.initialCoreSprite, breakPoint, false);
					tempCoreSprite = tempCoreSprite;
					Action onComplete;
					if ((onComplete = _delegate_1) == null)
					{
						onComplete = (_delegate_1 = delegate()
						{
							if (index == this.asteroidData.mySpriteBreakpointIndex)
							{
								if (breakPoint.core)
								{
									this.initialCoreSprite = tempCoreSprite.childSprite;
								}
								this.initialSurfaceSprite = tempSurfaceSprite.childSprite;
							}
							this.ProcessBreakpoints(index + 1);
						});
					}
					tempCoreSprite.onComplete = onComplete;
					return;
				}
				this.ProcessBreakpoints(index + 1);
			};
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000A8C78 File Offset: 0x000A6E78
		public void SetCoreSprite(Sprite sprite)
		{
			this.coreSprite.sprite = sprite;
			TargetableUnit.UpdateCollider(this.coreCollider, sprite, true);
			this.totalCorePixels = this.GetPixelCountFromSprite(this.coreSprite.sprite);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000A8CAA File Offset: 0x000A6EAA
		public void SetSurfaceSprite(Sprite sprite)
		{
			this.surfaceSprite.sprite = sprite;
			TargetableUnit.UpdateCollider(base.surfaceCollider, sprite, true);
			this.totalSurfacePixels = this.GetPixelCountFromSprite(this.surfaceSprite.sprite);
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000A8CDC File Offset: 0x000A6EDC
		public float GetMiningExperience(OreItemData oreItemData)
		{
			return (float)GameMath.GetExperienceRewardValue(oreItemData.item.rarity.GetPowerMultiplier() * 2f, SystemMapData.current.level);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000A8D04 File Offset: 0x000A6F04
		public void AddCoreExplosive(CoreExplosive explosive)
		{
			this.coreExplosives.Add(explosive);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000A8D12 File Offset: 0x000A6F12
		public void AttachDrone(Drone drone)
		{
			if (!this.attachedDrones.Contains(drone))
			{
				this.attachedDrones.Add(drone);
			}
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000A8D2E File Offset: 0x000A6F2E
		public void DetachDrone(Drone drone)
		{
			this.attachedDrones.Remove(drone);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000A8D3D File Offset: 0x000A6F3D
		public bool CanWeAttach(Drone drone)
		{
			return this.attachedDrones.Contains(drone) || this.attachedDrones.Count < this.maxAttachedDrones;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000A8D62 File Offset: 0x000A6F62
		private IEnumerator CheckCoreColliderIntegrity(DamageData data)
		{
			if (this.coreCollider.pathCount > 1)
			{
				Dictionary<Vector2[], int> dictionary = new Dictionary<Vector2[], int>();
				int totalLength = 0;
				for (int j = 0; j < this.coreCollider.pathCount; j++)
				{
					int num = this.coreCollider.GetPath(j).Length;
					Vector2[] array = new Vector2[num];
					Array.Copy(this.coreCollider.GetPath(j), array, num);
					if (array.Length != 0)
					{
						dictionary.Add(array, array.Length);
						totalLength += array.Length;
					}
				}
				if (dictionary.Count > 1)
				{
					List<KeyValuePair<Vector2[], int>> sorted = (from pair in dictionary
					orderby pair.Value
					select pair).ToList<KeyValuePair<Vector2[], int>>();
					int num2;
					for (int i = 0; i < sorted.Count; i = num2 + 1)
					{
						if (i != sorted.Count - 1)
						{
							yield return this.BreakCorePart(data, (float)sorted[i].Value / (float)totalLength, SeededRandom.Global.Choose<Vector2>(sorted[i].Key), false);
						}
						num2 = i;
					}
					sorted = null;
				}
			}
			yield break;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000A8D78 File Offset: 0x000A6F78
		public override void TakeDamage(DamageData data)
		{
			if (data.isCoreDamage)
			{
				this.TakeInnerCoreDamage(data, true);
				return;
			}
			this.TakeSurfaceDamage(data);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000A8D94 File Offset: 0x000A6F94
		public void TakeSurfaceDamage(DamageData amount)
		{
			if (amount.sourceUnit == GameplayManager.Instance.spaceShip)
			{
				BasePoiManager.current.PlayerSetMissionHostile();
			}
			if (amount.sourceUnit)
			{
				base.AddTargetedBy(amount.sourceUnit, 0f);
			}
			amount.targetUnit = this;
			amount.isCoreDamage = false;
			amount.PreDamageEvents();
			UIInfoTextParent.instance.ShowDamageNumber(amount, base.transform);
			int num = Mathf.RoundToInt(Mathf.Min(amount.damageAmount, (float)this.currentSurfaceHealth));
			this.asteroidData.currentSurfaceHealth -= num;
			this.asteroidData.accumulatedSurfaceDamage += num;
			this.lastDamageData = amount;
			AbstractUnit sourceUnit = amount.sourceUnit;
			if (sourceUnit != null && sourceUnit.IsPlayer(false))
			{
				this.playerDamagedSurface = true;
			}
			amount.PostDamageEvents();
			while (this.acumulatedSurfaceDamage >= this.surfaceItem.health)
			{
				if (this.surfaceAmount > 0)
				{
					bool chanceForInnerOre = this.innerCoreAmount > 0;
					AsteroidData asteroidData = this.asteroidData;
					int surfaceAmount = asteroidData.surfaceAmount;
					asteroidData.surfaceAmount = surfaceAmount - 1;
					this.SpawnSurfaceOre(amount.hitCoordinates, amount.yield, chanceForInnerOre);
					AbstractUnit sourceUnit2 = amount.sourceUnit;
					if (sourceUnit2 != null)
					{
						sourceUnit2.AddCrewExperience(this.surfaceExp, new CommanderSpecialization?(CommanderSpecialization.Mining), true);
					}
					if (this.asteroidSize != AsteroidSize.Tiny)
					{
						float num2 = (float)this.surfaceItem.health / (float)this.maxSurfaceHealth;
						SpriteBreakPoint spriteBreakPoint = new SpriteBreakPoint(amount.hitTransform ? amount.hitTransform.localPosition : Vector2.zero, (int)(SeededRandom.Global.RandomRange(num2 * 0.5f, num2 * 0.8f) * (float)this.totalSurfacePixels), false);
						this.BreakOffAsteroid(this.surfaceSprite.sprite, spriteBreakPoint, true).onComplete = delegate()
						{
							if (this == null)
							{
								return;
							}
							PolygonCollider2D surfaceCollider = base.surfaceCollider;
							SpriteRenderer spriteRenderer = this.surfaceSprite;
							TargetableUnit.UpdateCollider(surfaceCollider, (spriteRenderer != null) ? spriteRenderer.sprite : null, true);
						};
						this.asteroidData.breakPoints.Add(spriteBreakPoint);
					}
				}
				this.asteroidData.accumulatedSurfaceDamage -= this.surfaceItem.health;
				this.asteroidData.currentSurfaceHealth = this.surfaceItem.health * this.surfaceAmount;
			}
			if (this.currentSurfaceHealth <= 0)
			{
				this.CheckPocketSystemAchievement();
				if (this.IsAsteroidTiny())
				{
					this.Break(amount);
				}
			}
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000A8FE0 File Offset: 0x000A71E0
		private void SpawnSurfaceOre(Vector2 position, float yield, bool chanceForInnerOre)
		{
			if (this.CanWeSpawnOre(yield))
			{
				DamageData damageData = this.lastDamageData;
				Faction faction;
				if (damageData == null)
				{
					faction = null;
				}
				else
				{
					AbstractUnit sourceUnit = damageData.sourceUnit;
					faction = ((sourceUnit != null) ? sourceUnit.faction : null);
				}
				if (faction == Faction.player)
				{
					Register.AddCounter("SurfaceOreYieldMax", 1, 0);
					Register.AddCounter("SurfaceOreMined", 1, 0);
					GamePlayer.current.AddAutopilotStat(IdleStat.Ores, 1);
				}
				DamageData damageData2 = this.lastDamageData;
				bool flag;
				if (damageData2 == null)
				{
					flag = false;
				}
				else
				{
					AbstractUnit sourceUnit2 = damageData2.sourceUnit;
					bool? flag2 = (sourceUnit2 != null) ? new bool?(sourceUnit2.IsPlayer(false)) : null;
					bool flag3 = false;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				bool overrideFaction = flag && this.playerDamagedSurface && SeededRandom.Global.RandomBool(0.5f);
				this.SpawnOre(position, this.surfaceItem, yield, overrideFaction, chanceForInnerOre);
				return;
			}
			DamageData damageData3 = this.lastDamageData;
			Faction faction2;
			if (damageData3 == null)
			{
				faction2 = null;
			}
			else
			{
				AbstractUnit sourceUnit3 = damageData3.sourceUnit;
				faction2 = ((sourceUnit3 != null) ? sourceUnit3.faction : null);
			}
			if (faction2 == Faction.player)
			{
				Register.AddCounter("SurfaceOreYieldMax", 1, 0);
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000A90E8 File Offset: 0x000A72E8
		public BreakDelayedSprite BreakOffAsteroid(Sprite sprite, SpriteBreakPoint breakPoint, bool spawnChunk = true)
		{
			Vector2 hitLocation = breakPoint.position;
			Vector2 vector = new Vector2(hitLocation.x * 32f + (float)(sprite.texture.width / 2), hitLocation.y * 32f + (float)(sprite.texture.height / 2));
			BreakDelayedSprite newSprite = AsteroidHelper.BreakAsteroidSprite(sprite, new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)), breakPoint, this.asteroidData.seed + hitLocation.ToString());
			if (spawnChunk)
			{
				SpriteRenderer chunkObj = UnityEngine.Object.Instantiate<SpriteRenderer>(this.chunkPrefab, base.transform.position + new Vector3(0f, 0f, -0.03f), base.transform.rotation, base.transform.parent);
				chunkObj.sprite = newSprite.childSprite;
				PolygonCollider2D chunkCollider = chunkObj.GetComponent<PolygonCollider2D>();
				this.DisablePhysicsForObject(chunkCollider);
				newSprite.onComplete = delegate()
				{
					TargetableUnit.UpdateCollider(chunkCollider, newSprite.childSprite, false);
					this.AddEffectsForSpawnedChunk(hitLocation, chunkObj.gameObject, chunkCollider, 1f);
				};
			}
			return newSprite;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000A923F File Offset: 0x000A743F
		private void DisablePhysicsForObject(Collider2D collider)
		{
			Physics2D.IgnoreCollision(collider, base.surfaceCollider);
			Physics2D.IgnoreCollision(collider, this.coreCollider);
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000A9259 File Offset: 0x000A7459
		public bool IsSurfaceOreDepleted()
		{
			return this.surfaceAmount <= 0;
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000A9268 File Offset: 0x000A7468
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			AbstractMiningTurret abstractMiningTurret = turret as AbstractMiningTurret;
			return abstractMiningTurret != null && abstractMiningTurret.CanMineAsteroidTarget(this);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000A9288 File Offset: 0x000A7488
		public void ShowDamageNumber(DamageData damage)
		{
			UIInfoTextParent.instance.ShowDamageNumber(damage, base.transform);
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000A929C File Offset: 0x000A749C
		public void TakeInnerCoreDamage(DamageData damage, bool showDamageNumbers = true)
		{
			if (damage.sourceUnit)
			{
				base.AddTargetedBy(damage.sourceUnit, 0f);
			}
			if (damage.sourceUnit == GameplayManager.Instance.spaceShip)
			{
				BasePoiManager.current.PlayerSetMissionHostile();
			}
			damage.targetUnit = this;
			damage.isCoreDamage = true;
			damage.PreDamageEvents();
			if (showDamageNumbers)
			{
				UIInfoTextParent.instance.ShowDamageNumber(damage, base.transform);
			}
			float num = (float)this.currentCoreHealth;
			this.asteroidData.currentInnerCoreHealth = Mathf.RoundToInt(Mathf.Clamp((float)this.currentCoreHealth - damage.damageAmount, 0f, (float)this.maxCoreHealth));
			this.lastDamageData = damage;
			AbstractUnit sourceUnit = damage.sourceUnit;
			if (sourceUnit != null && sourceUnit.IsPlayer(false))
			{
				this.playerDamagedCore = true;
			}
			damage.PostDamageEvents();
			if (!this.IsAsteroidTiny() && this.innerCoreAmount > 1)
			{
				this.innerCoreCumulativeDamageWithoutOre += damage.damageAmount;
				float num2 = Mathf.Clamp(this.innerCoreCumulativeDamageWithoutOre / num, 0f, 1f);
				if ((int)((float)this.asteroidData.innerCoreAmount * num2) > 0)
				{
					base.StartCoroutine(this.HandleCoreDamage(damage, num2));
					this.innerCoreCumulativeDamageWithoutOre = 0f;
					return;
				}
			}
			else if (this.currentCoreHealth <= this.innerCoreItem.health)
			{
				this.Break(damage);
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000A93F3 File Offset: 0x000A75F3
		private IEnumerator HandleCoreDamage(DamageData damageData, float totalPercentage)
		{
			this.totalCorePixels = this.GetPixelCountFromSprite(this.coreSprite.sprite);
			int numberOfParts = (int)Mathf.Clamp(SeededRandom.Global.RandomRange((float)((totalPercentage > 0.75f) ? 2 : 1), totalPercentage / 0.25f), 1f, 3f);
			float percentageUsed = 0f;
			int num2;
			for (int i = 1; i <= numberOfParts; i = num2 + 1)
			{
				float num = (i == numberOfParts) ? (totalPercentage - percentageUsed) : (SeededRandom.Global.RandomRange(0.2f, 0.1f + 1f / (float)numberOfParts) * totalPercentage);
				percentageUsed += num;
				yield return this.BreakCorePart(damageData, num, damageData.hitTransform.localPosition, true);
				num2 = i;
			}
			if (totalPercentage > 0.95f)
			{
				this.Break(damageData);
			}
			else
			{
				yield return this.CheckCoreColliderIntegrity(damageData);
			}
			yield break;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000A9410 File Offset: 0x000A7610
		private IEnumerator BreakCorePart(DamageData source, float percentage, Vector2 hitCoordinates, bool smallerPart = true)
		{
			int num = (int)((float)this.totalCorePixels * percentage * (smallerPart ? 0.7f : 1f));
			AsteroidSize asteroidSizeForPixels = AsteroidHelper.GetAsteroidSizeForPixels(num);
			int num2 = (int)((float)this.asteroidData.surfaceAmount * percentage);
			int num3 = (int)((float)this.asteroidData.innerCoreAmount * percentage);
			this.asteroidData.innerCoreAmount -= num3;
			this.asteroidData.surfaceAmount -= num2;
			this.asteroidData.currentInnerCoreHealth = this.innerCoreItem.health * this.innerCoreAmount;
			this.asteroidData.currentSurfaceHealth = this.surfaceItem.health * this.surfaceAmount;
			int num4 = num3;
			bool flag = asteroidSizeForPixels != AsteroidSize.Tiny && asteroidSizeForPixels != AsteroidSize.Small;
			if (num3 >= 1)
			{
				if (flag)
				{
					int num5 = SeededRandom.Global.RandomRange(1, Mathf.Clamp((int)((float)num3 * 0.75f), 2, num3));
					this.SpawnInnerCoreOre(num5, source.yield);
					num4 -= num5;
				}
				else
				{
					this.SpawnInnerCoreOre(num3, source.yield);
					for (int i = 0; i < num2; i++)
					{
						this.SpawnSurfaceOre(base.transform.position, 0.1f, false);
					}
				}
			}
			if (source != null)
			{
				AbstractUnit sourceUnit = source.sourceUnit;
				if (sourceUnit != null)
				{
					sourceUnit.AddCrewExperience(this.innerCoreExp, new CommanderSpecialization?(CommanderSpecialization.Mining), true);
				}
			}
			SpriteBreakPoint spriteBreakPoint = new SpriteBreakPoint(hitCoordinates, num, true);
			BreakDelayedSprite breakDelayedSprite = this.BreakOffAsteroid(this.coreSprite.sprite, spriteBreakPoint, false);
			BreakDelayedSprite breakDelayedSprite2 = this.BreakOffAsteroid(this.surfaceSprite.sprite, spriteBreakPoint, !flag);
			bool coreComplete = false;
			bool surfaceComplete = false;
			breakDelayedSprite.onComplete = delegate()
			{
				if (this == null)
				{
					return;
				}
				PolygonCollider2D coreCollider = this.coreCollider;
				SpriteRenderer spriteRenderer = this.coreSprite;
				TargetableUnit.UpdateCollider(coreCollider, (spriteRenderer != null) ? spriteRenderer.sprite : null, true);
				coreComplete = true;
			};
			breakDelayedSprite2.onComplete = delegate()
			{
				if (this == null)
				{
					return;
				}
				PolygonCollider2D surfaceCollider = this.surfaceCollider;
				SpriteRenderer spriteRenderer = this.surfaceSprite;
				TargetableUnit.UpdateCollider(surfaceCollider, (spriteRenderer != null) ? spriteRenderer.sprite : null, true);
				surfaceComplete = true;
			};
			this.asteroidData.breakPoints.Add(spriteBreakPoint);
			if (flag)
			{
				AsteroidData asteroidDataForChild = this.asteroidData.GetAsteroidDataForChild(asteroidSizeForPixels, num2, num4);
				asteroidDataForChild.SetBreakpoints(this.asteroidData.breakPoints);
				asteroidDataForChild.SetMySpriteBreakpointIndex();
				Asteroid component = MapPointOfInterest.currentOrNext.AddPersistable(asteroidDataForChild).GetComponent<Asteroid>();
				Collider2D component2 = component.GetComponent<Collider2D>();
				PolygonCollider2D component3 = component.coreSprite.GetComponent<PolygonCollider2D>();
				this.DisablePhysicsForObject(component2);
				this.DisablePhysicsForObject(component3);
				base.AddEffectsForSpawnedChunk(hitCoordinates, component.gameObject, component2, component.scale);
				Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(component2.bounds.center, asteroidDataForChild.scale, 0.3f);
			}
			this.preventLockedTimer = 0.3f;
			yield return new WaitUntil(() => (surfaceComplete & coreComplete) || this.preventLockedTimer <= 0f);
			source.hitCoordinates = SeededRandom.Global.Choose<Vector2>(this.coreCollider.points);
			yield break;
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000A943C File Offset: 0x000A763C
		public PolygonCollider2D GetCoreCollider()
		{
			return this.coreSprite.GetComponent<PolygonCollider2D>();
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000A9449 File Offset: 0x000A7649
		private int GetPixelCountFromSprite(Sprite sprite)
		{
			return AsteroidHelper.GetFilledPixelCount(sprite);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000A9454 File Offset: 0x000A7654
		public List<InventoryItemType> TakeDrillCoreDamage(DamageData amount)
		{
			amount.hitCoordinates = base.transform.position;
			UIInfoTextParent.instance.ShowDamageNumber(amount, base.transform);
			this.asteroidData.currentInnerCoreHealth -= (int)amount.damageAmount;
			this.asteroidData.accumulatedInnerCoreDamage += (int)amount.damageAmount;
			List<InventoryItemType> result = new List<InventoryItemType>();
			this.lastDamageData = amount;
			while (this.acumulatedInnerCoreDamage >= this.innerCoreItem.health && this.innerCoreAmount > 0)
			{
				if (this.CanWeSpawnOre(amount.yield))
				{
					if (this.lastDamageData.sourceUnit.faction == Faction.player)
					{
						Register.AddCounter("CoreOreYieldMax", 1, 0);
						Register.AddCounter("CoreOreMined", 1, 0);
						GamePlayer.current.AddAutopilotStat(IdleStat.Ores, 1);
					}
					this.SpawnOre(amount.hitCoordinates, this.innerCoreItem, amount.yield, false, false);
				}
				else if (this.lastDamageData.sourceUnit.faction == Faction.player)
				{
					Register.AddCounter("CoreOreYieldMax", 1, 0);
				}
				AsteroidData asteroidData = this.asteroidData;
				int innerCoreAmount = asteroidData.innerCoreAmount;
				asteroidData.innerCoreAmount = innerCoreAmount - 1;
				this.asteroidData.accumulatedInnerCoreDamage -= this.innerCoreItem.health;
				this.asteroidData.currentInnerCoreHealth = this.innerCoreItem.health * this.innerCoreAmount;
				amount.sourceUnit.AddCrewExperience(this.innerCoreExp, new CommanderSpecialization?(CommanderSpecialization.Mining), true);
			}
			if (this.asteroidData.innerCoreAmount <= 0)
			{
				this.CheckPocketSystemAchievement();
			}
			return result;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000A95F4 File Offset: 0x000A77F4
		private void Break(DamageData source)
		{
			SpaceShip spaceShip = ((source != null) ? source.sourceUnit : null) as SpaceShip;
			if (spaceShip != null && spaceShip.IsPlayer(false))
			{
				spaceShip.AddCrewExperience(this.innerCoreExp, new CommanderSpecialization?(CommanderSpecialization.Mining), true);
			}
			foreach (Drone drone in this.attachedDrones.ToList<Drone>())
			{
				if (!drone)
				{
					this.attachedDrones.Remove(drone);
				}
				else
				{
					drone.DetachFromTarget();
				}
			}
			Vector2 vector = this.coreCollider ? this.coreCollider.bounds.center : base.surfaceCollider.bounds.center;
			Singleton<EffectManager>.Instance.PlaySmokeEffect(vector, 1f + this.scale, null, 4f, 15);
			Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(vector, this.scale * 2f, 0.3f);
			this.SpawnInnerCoreOre(this.innerCoreAmount, source.yield);
			PhysicsInteraction.ApplyShockwaveToNearbyShips(vector, this.scale, 0.62f);
			UnityEngine.Object.Destroy(base.gameObject);
			BasePoiManager.current.poi.RemovePersistable(this.asteroidData);
			this.CheckPocketSystemAchievement();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000A9760 File Offset: 0x000A7960
		private void DisableRendererAndCollider()
		{
			base.GetComponent<SpriteRenderer>().enabled = false;
			base.GetComponent<Collider2D>().enabled = false;
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000A977C File Offset: 0x000A797C
		protected bool CanWeSpawnOre(float yield = 0.6f)
		{
			if (yield == 0.6f)
			{
				DamageData damageData = this.lastDamageData;
				AbstractMiningTurret abstractMiningTurret = ((damageData != null) ? damageData.sourceTurret : null) as AbstractMiningTurret;
				if (abstractMiningTurret != null)
				{
					yield = abstractMiningTurret.yield;
				}
			}
			return SeededRandom.Global.RandomBool(yield);
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x000A97C8 File Offset: 0x000A79C8
		public void SpawnInnerCoreOre(int amount, float yield)
		{
			for (int i = 0; i < amount; i++)
			{
				if (this.CanWeSpawnOre(yield))
				{
					if (this.lastDamageData.sourceUnit.faction == Faction.player)
					{
						Register.AddCounter("CoreOreYieldMax", 1, 0);
						Register.AddCounter("CoreOreMined", 1, 0);
						GamePlayer.current.AddAutopilotStat(IdleStat.Ores, 1);
					}
					bool overrideFaction = !this.lastDamageData.sourceUnit.IsPlayer(false) && this.playerDamagedCore && SeededRandom.Global.RandomBool(0.5f);
					this.SpawnOre(base.transform.position + this.CalculateRandPosInAsteroid(), this.innerCoreItem, yield, overrideFaction, false);
				}
				else if (this.lastDamageData.sourceUnit.faction == Faction.player)
				{
					Register.AddCounter("CoreOreYieldMax", 1, 0);
				}
				float num = SkilltreeNode.miningTreasureChance.currentIncrease;
				num = (GamePlayer.current.autoPlay ? (0.33f * num) : num);
				if (this.lastDamageData.sourceUnit.IsPlayer(false) && SeededRandom.Global.RandomBool(num) && GamePlayer.current.IsInSandBox())
				{
					Singleton<LootManager>.Instance.CreateLootBox(MapPointOfInterest.current.level, base.transform.position + this.CalculateRandPosInAsteroid());
				}
			}
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000A992B File Offset: 0x000A7B2B
		public bool IsAsteroidTiny()
		{
			return this.asteroidSize == AsteroidSize.Tiny;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000A9938 File Offset: 0x000A7B38
		public Vector3 CalculateRandPosInAsteroid()
		{
			Vector3 result = UnityEngine.Random.insideUnitSphere * base.transform.localScale.x / 2f;
			result.z = 0f;
			return result;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000A9978 File Offset: 0x000A7B78
		private void SpawnOre(Vector2 position, OreItemData itemType, float yield, bool overrideFaction = false, bool innerOreChance = false)
		{
			int num = 1;
			if (SkilltreeNode.miningMegaYield.isActive)
			{
				while (yield > 1f)
				{
					yield -= 1f;
					if (yield > 1f || SeededRandom.Global.RandomBool(yield))
					{
						num++;
					}
				}
			}
			Faction faction;
			if (!overrideFaction)
			{
				DamageData damageData = this.lastDamageData;
				if (damageData == null)
				{
					faction = null;
				}
				else
				{
					AbstractUnit sourceUnit = damageData.sourceUnit;
					faction = ((sourceUnit != null) ? sourceUnit.faction : null);
				}
			}
			else
			{
				faction = Faction.player;
			}
			Faction faction2 = faction;
			Source.Galaxy.POI.Mining mining = MapPointOfInterest.current as Source.Galaxy.POI.Mining;
			if (mining != null && mining.oreOwnershipOverride != null)
			{
				faction2 = mining.oreOwnershipOverride;
			}
			for (int i = 0; i < num; i++)
			{
				Singleton<LootManager>.Instance.CreateLootItem(position, itemType.item, 1, faction2, false);
				if (faction2 == Faction.player)
				{
					MissionObjective.Trigger(MissionTrigger.MinedOre, itemType.item, null, false);
				}
			}
			if (innerOreChance && SeededRandom.Global.RandomBool(0.05f))
			{
				Singleton<LootManager>.Instance.CreateLootItem(base.transform.position + this.CalculateRandPosInAsteroid(), this.innerCoreItem.item, 1, faction2, false);
			}
			DamageData damageData2 = this.lastDamageData;
			bool flag;
			if (damageData2 == null)
			{
				flag = false;
			}
			else
			{
				AbstractUnit sourceUnit2 = damageData2.sourceUnit;
				bool? flag2 = (sourceUnit2 != null) ? new bool?(sourceUnit2.IsPlayer(false)) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag && SeededRandom.Global.RandomBool(SkilltreeNode.miningExtraOre.currentIncrease))
			{
				OreItemData randomOre = MapPointOfInterest.current.system.systemOreData.GetRandomOre(false, null);
				Singleton<LootManager>.Instance.CreateLootItem(position, randomOre.item, 1, Faction.player, false);
			}
			if (MapPointOfInterest.currentOrNext.level >= 12)
			{
				InventoryItemType inventoryItemType = InventoryItemType.PotentiallyGetCrystalItem(MapPointOfInterest.currentOrNext.level, SeededRandom.Global);
				if (inventoryItemType)
				{
					Singleton<LootManager>.Instance.CreateLootItem(base.transform.position + this.CalculateRandPosInAsteroid(), inventoryItemType, 1, Faction.player, false);
				}
			}
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000A9B6D File Offset: 0x000A7D6D
		private void OnMouseDown()
		{
			if (!UIHelper.clickTargetingAvailable)
			{
				return;
			}
			if (this.CheckForDamageNotification())
			{
				GameplayManager.Instance.spaceShip.SetManualTarget(this);
			}
			MissionObjective.Trigger(MissionTrigger.TargetAsteroid, null, null, false);
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000A9B98 File Offset: 0x000A7D98
		private bool CheckForDamageNotification()
		{
			SpaceShipData currentSpaceShip = GamePlayer.current.currentSpaceShip;
			string text = null;
			bool flag = currentSpaceShip.HasLoadout(GameplayType.Mining, TargetLayer.Surface);
			bool flag2 = currentSpaceShip.HasLoadout(GameplayType.Mining, TargetLayer.Core);
			if (!flag && !flag2)
			{
				text = "@MiningNoTurret";
			}
			else
			{
				if (!this.hasSurfaceOre && !this.hasInnerCore)
				{
					text = "@MiningNoOreRemaining";
				}
				if (this.hasSurfaceOre && !flag && !this.hasInnerCore)
				{
					text = "@MiningNoSurfaceTurret";
				}
				if (this.hasInnerCore && !flag2 && !this.hasSurfaceOre)
				{
					text = "@MiningNoCoreTurret";
				}
			}
			if (text != null)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate(text, Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			return true;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000A9C44 File Offset: 0x000A7E44
		public string GetTooltipTitle()
		{
			return Translation.Translate("@MiningAsteroid", Array.Empty<object>()) + " - " + Translation.Translate("@MiningAsteroid" + this.asteroidSize.ToString(), Array.Empty<object>());
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000A9C94 File Offset: 0x000A7E94
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			int num = 0;
			if (SkilltreeNode.miningAdvancedScanning.isActive)
			{
				num = 2;
			}
			else if (SkilltreeNode.miningBasicScanning.isActive)
			{
				num = 1;
			}
			bool flag = num == 2;
			string text = Translation.Translate("@MiningDepleted", Array.Empty<object>());
			if (!this.IsSurfaceOreDepleted())
			{
				text = (Translation.Translate(this.surfaceItem.item.displayName, Array.Empty<object>()).HighlightWithColor(this.surfaceItem.item.rarity.GetColor()) ?? "");
				if (num == 0)
				{
					tooltip.AddTextLine(Translation.Translate("@MiningSurface", Array.Empty<object>()) + ": " + text, 12, 8f);
					return;
				}
				if (flag)
				{
					text += string.Format(" ({0})", this.surfaceAmount);
				}
			}
			tooltip.AddTextLine(Translation.Translate("@MiningSurface", Array.Empty<object>()) + ": " + text, 12, 8f);
			if (num == 0)
			{
				return;
			}
			string text2 = Translation.Translate("@MiningDepleted", Array.Empty<object>());
			if (!this.innerCoreDepleted)
			{
				text2 = (Translation.Translate(this.innerCoreItem.item.displayName, Array.Empty<object>()).HighlightWithColor(this.innerCoreItem.item.rarity.GetColor()) ?? "");
				if (flag)
				{
					text2 += string.Format(" ({0})", this.innerCoreAmount);
				}
			}
			if (!this.IsSurfaceOreDepleted())
			{
				base.AddTooltipHealthbar(tooltip, TargetLayer.Surface, flag);
				tooltip.AddTextLine(this.surfaceItem.GetDescription(), 10, 8f).Text.color = ColorHelper.detailsColor;
			}
			tooltip.AddSeparator(new Color(1f, 1f, 1f, 0.5f), 2f, 0f, 8f);
			tooltip.AddTextLine(Translation.Translate("@MiningCore", Array.Empty<object>()) + ": " + text2, 12, 8f);
			if (!this.innerCoreDepleted)
			{
				base.AddTooltipHealthbar(tooltip, TargetLayer.Core, flag);
				tooltip.AddTextLine(this.innerCoreItem.GetDescription(), 10, 8f).Text.color = ColorHelper.detailsColor;
			}
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000A9ED2 File Offset: 0x000A80D2
		public int GetSurfaceAmount()
		{
			return this.surfaceAmount;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000A9EDA File Offset: 0x000A80DA
		public int GetInnerAmount()
		{
			return this.innerCoreAmount;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000A9EE4 File Offset: 0x000A80E4
		public void AddScanningProgress()
		{
			int scanningProgress = this.scanningProgress;
			this.scanningProgress = scanningProgress + 1;
			this.asteroidData.scanProgress = this.scanningProgress;
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000A9F14 File Offset: 0x000A8114
		public bool EnoughCoreExplosivesOnTarget(MiningCoreTurret source, int maxExplosives)
		{
			if (this.coreExplosives.Count == 0)
			{
				return false;
			}
			int num = 0;
			using (List<CoreExplosive>.Enumerator enumerator = this.coreExplosives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.source == source)
					{
						num++;
					}
				}
			}
			if (num >= maxExplosives)
			{
				return true;
			}
			float num2 = 0f;
			foreach (CoreExplosive coreExplosive in this.coreExplosives)
			{
				if (coreExplosive.damage.sourceTurret)
				{
					num2 += coreExplosive.damage.damageAmount;
				}
			}
			return num2 >= (float)this.currentCoreHealth;
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000A9FF8 File Offset: 0x000A81F8
		public bool AreExplosivesLatched(MiningCoreTurret source)
		{
			foreach (CoreExplosive coreExplosive in this.coreExplosives)
			{
				if (coreExplosive && coreExplosive.source == source && !coreExplosive.isLatched)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000AA06C File Offset: 0x000A826C
		public bool IsOverlappingWithOtherExplosives(Vector3 point, float spreadRadius)
		{
			foreach (CoreExplosive coreExplosive in this.coreExplosives)
			{
				if (coreExplosive != null && Vector3.Distance(point, coreExplosive.transform.position) < spreadRadius)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000AA0DC File Offset: 0x000A82DC
		public void DetonateCoreExplosives(MiningCoreTurret source)
		{
			Vector2 zero = Vector2.zero;
			float totalDamageForExplosives = this.GetTotalDamageForExplosives(source, out zero);
			AbstractTurret abstractTurret = null;
			foreach (CoreExplosive coreExplosive in this.coreExplosives)
			{
				if (coreExplosive.source == source)
				{
					abstractTurret = coreExplosive.damage.sourceTurret;
				}
			}
			if (abstractTurret != null)
			{
				base.StartCoroutine(this.DetonateCoreExplosivesWithDelay(source, new DamageData(abstractTurret)
				{
					damageAmount = totalDamageForExplosives,
					hitCoordinates = zero
				}));
			}
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000AA188 File Offset: 0x000A8388
		private float GetTotalDamageForExplosives(MiningCoreTurret source, out Vector2 randomPosition)
		{
			float num = 0f;
			List<Vector2> list = new List<Vector2>();
			foreach (CoreExplosive coreExplosive in this.coreExplosives)
			{
				if (!(coreExplosive.source != source))
				{
					num += coreExplosive.damage.damageAmount;
					list.Add(coreExplosive.transform.localPosition);
				}
			}
			randomPosition = SeededRandom.Global.Choose<Vector2>(list);
			return num;
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000AA224 File Offset: 0x000A8424
		private IEnumerator DetonateCoreExplosivesWithDelay(MiningCoreTurret source, DamageData damage)
		{
			this.isDetonating = true;
			float maxDelay = 0f;
			foreach (CoreExplosive coreExplosive in this.coreExplosives)
			{
				if (!(coreExplosive.source != source))
				{
					float num = coreExplosive.RandomExplosiveDelay();
					maxDelay = Mathf.Max(num, maxDelay);
					base.StartCoroutine(coreExplosive.Detonate(num));
					yield return new WaitForSeconds(0.1f);
				}
			}
			List<CoreExplosive>.Enumerator enumerator = default(List<CoreExplosive>.Enumerator);
			yield return new WaitForSeconds(maxDelay);
			for (int i = 0; i < this.coreExplosives.Count; i++)
			{
				if (this.coreExplosives[i].source == source)
				{
					this.coreExplosives.RemoveAt(i);
					i--;
				}
			}
			this.isDetonating = false;
			GameObject gameObject = new GameObject("TrackingTarget(CoreExplosives)");
			gameObject.transform.parent = this.coreCollider.transform;
			gameObject.transform.localPosition = damage.hitCoordinates;
			damage.hitTransform = gameObject.transform;
			damage.hitCoordinates = gameObject.transform.position;
			this.TakeInnerCoreDamage(damage, false);
			yield break;
			yield break;
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000AA244 File Offset: 0x000A8444
		private void CheckPocketSystemAchievement()
		{
			if (!SystemMapData.current.pocketSystem)
			{
				return;
			}
			if (SystemMapData.current.faction != Faction.miningGuild)
			{
				return;
			}
			if (!(MapPointOfInterest.current is Source.Galaxy.POI.Mining))
			{
				return;
			}
			if (!SystemMapData.current.IsStaticPOI(MapPointOfInterest.current))
			{
				return;
			}
			foreach (PersistableData persistableData in MapPointOfInterest.current.GetPersistables())
			{
				AsteroidData asteroidData = persistableData as AsteroidData;
				if (asteroidData != null && (asteroidData.surfaceAmount > 0 || asteroidData.innerCoreAmount > 0))
				{
					return;
				}
			}
			SteamAchievement.Trigger("MotherlodePocket");
		}

		// Token: 0x0400116D RID: 4461
		public AsteroidData asteroidData;

		// Token: 0x0400116E RID: 4462
		public SpriteRenderer surfaceSprite;

		// Token: 0x0400116F RID: 4463
		public SpriteRenderer coreSprite;

		// Token: 0x04001170 RID: 4464
		public SpriteRenderer chunkPrefab;

		// Token: 0x04001171 RID: 4465
		protected bool playerDamagedSurface;

		// Token: 0x04001172 RID: 4466
		protected bool playerDamagedCore;

		// Token: 0x04001173 RID: 4467
		public float innerCoreCumulativeDamageWithoutOre;

		// Token: 0x04001175 RID: 4469
		private List<CoreExplosive> coreExplosives = new List<CoreExplosive>();

		// Token: 0x04001178 RID: 4472
		private DamageData lastDamageData;

		// Token: 0x04001179 RID: 4473
		private List<Drone> attachedDrones = new List<Drone>();

		// Token: 0x0400117A RID: 4474
		private Sprite initialCoreSprite;

		// Token: 0x0400117B RID: 4475
		private Sprite initialSurfaceSprite;

		// Token: 0x0400117C RID: 4476
		private int totalSurfacePixels;

		// Token: 0x0400117D RID: 4477
		private int totalCorePixels;

		// Token: 0x0400117E RID: 4478
		private float preventLockedTimer = -1f;

		// Token: 0x0400117F RID: 4479
		private float maxAngularSpeed = 10f;

		// Token: 0x04001180 RID: 4480
		private float rotationDamping = 0.5f;
	}
}
