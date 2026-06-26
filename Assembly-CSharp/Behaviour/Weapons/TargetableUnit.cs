using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Util.ContourTracer;
using UnityEngine;

namespace Behaviour.Weapons
{
	// Token: 0x020001AC RID: 428
	public abstract class TargetableUnit : MonoBehaviour
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000F02 RID: 3842 RVA: 0x000695AD File Offset: 0x000677AD
		// (set) Token: 0x06000F03 RID: 3843 RVA: 0x000695B5 File Offset: 0x000677B5
		public Rigidbody2D rigidbody { get; protected set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x000695BE File Offset: 0x000677BE
		public virtual float mass
		{
			get
			{
				return 100f;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x000695C5 File Offset: 0x000677C5
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x000695CD File Offset: 0x000677CD
		public PolygonCollider2D surfaceCollider { get; protected set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x000695D6 File Offset: 0x000677D6
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x000695DE File Offset: 0x000677DE
		public float radius { get; protected set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x000695E7 File Offset: 0x000677E7
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x000695EF File Offset: 0x000677EF
		public float height { get; protected set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x000695F8 File Offset: 0x000677F8
		public virtual bool bouncyBouncy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x000695FB File Offset: 0x000677FB
		public int targetedCount
		{
			get
			{
				return this.targetedBy.Count;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x00069608 File Offset: 0x00067808
		public virtual int currentSurfaceHealth
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0006960B File Offset: 0x0006780B
		public virtual int maxSurfaceHealth
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0006960E File Offset: 0x0006780E
		public virtual int currentCoreHealth
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00069611 File Offset: 0x00067811
		public virtual int maxCoreHealth
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x00069614 File Offset: 0x00067814
		public Vector2 targetablePosition
		{
			get
			{
				if (!this.rigidbody)
				{
					return base.transform.position;
				}
				return this.rigidbody.position;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0006963F File Offset: 0x0006783F
		public Vector2 targetableVelocity
		{
			get
			{
				if (!this.rigidbody)
				{
					return Vector2.zero;
				}
				return this.rigidbody.linearVelocity;
			}
		}

		// Token: 0x06000F13 RID: 3859
		public abstract bool CanBeDamagedBy(AbstractTurret turret);

		// Token: 0x06000F14 RID: 3860
		public abstract void TakeDamage(DamageData data);

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0006965F File Offset: 0x0006785F
		public virtual bool hasHealthBar
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x00069662 File Offset: 0x00067862
		public virtual bool damagableByAll
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00069665 File Offset: 0x00067865
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0006966D File Offset: 0x0006786D
		public bool isDestroyed { get; protected set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000F19 RID: 3865
		public abstract string targetName { get; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x00069676 File Offset: 0x00067876
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x0006967E File Offset: 0x0006787E
		public SpriteRenderer spriteRenderer { get; protected set; }

		// Token: 0x06000F1C RID: 3868 RVA: 0x00069687 File Offset: 0x00067887
		protected virtual void Awake()
		{
			this.surfaceCollider = base.GetComponent<PolygonCollider2D>();
			this.rigidbody = base.GetComponent<Rigidbody2D>();
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			this.isSalvage = false;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x000696B4 File Offset: 0x000678B4
		protected virtual void Start()
		{
			this.SetDimensions();
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000696BC File Offset: 0x000678BC
		public void SetDimensions()
		{
			if (!this.spriteRenderer)
			{
				return;
			}
			this.radius = this.spriteRenderer.sprite.bounds.extents.x;
			this.height = this.spriteRenderer.sprite.bounds.extents.y;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00069720 File Offset: 0x00067920
		protected virtual void Update()
		{
			this.trackerTimer -= Time.deltaTime;
			if (this.trackerTimer < 0f)
			{
				this.trackerTimer = 0.5f;
				List<AbstractUnit> list = new List<AbstractUnit>();
				foreach (KeyValuePair<AbstractUnit, TargetableTracker> keyValuePair in this.targetedBy)
				{
					keyValuePair.Value.timer -= 0.5f;
					if (keyValuePair.Value.timer < 0f)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (AbstractUnit key in list)
				{
					this.targetedBy.Remove(key);
				}
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0006981C File Offset: 0x00067A1C
		protected virtual void FixedUpdate()
		{
			if (!this.rigidbody)
			{
				return;
			}
			this.CheckBorderDistance();
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00069834 File Offset: 0x00067A34
		private void CheckBorderDistance()
		{
			if (this.bouncyBouncy && BasePoiManager.current && BasePoiManager.current.initializedAndReady)
			{
				TravelManager instance = Singleton<TravelManager>.Instance;
				if (instance == null || !instance.TravelActive())
				{
					float num = this.GetDistanceFromBorder();
					if (num < 0f)
					{
						num = Mathf.Abs(num);
						Vector2 lhs = (Vector2)BasePoiManager.current.transform.position - this.rigidbody.position;
						if (Vector2.Dot(lhs, this.rigidbody.linearVelocity) < 0f || this.rigidbody.linearVelocity.magnitude < 3f)
						{
							this.rigidbody.AddForce(num * lhs.normalized * this.mass);
						}
					}
				}
			}
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00069916 File Offset: 0x00067B16
		public bool IsTargetedBy(AbstractUnit unit)
		{
			return this.targetedBy.ContainsKey(unit);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00069924 File Offset: 0x00067B24
		public void AddTargetedBy(AbstractUnit sourceUnit, float damage = 0f)
		{
			if (!this.targetedBy.ContainsKey(sourceUnit))
			{
				this.targetedBy.Add(sourceUnit, new TargetableTracker());
			}
			this.targetedBy[sourceUnit].timer += 3f;
			this.targetedBy[sourceUnit].damage += damage;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00069988 File Offset: 0x00067B88
		public AbstractUnit GetHighestDamageDealer()
		{
			float num = 0f;
			AbstractUnit result = null;
			foreach (KeyValuePair<AbstractUnit, TargetableTracker> keyValuePair in this.targetedBy)
			{
				if (keyValuePair.Value.damage >= num)
				{
					result = keyValuePair.Key;
					num = keyValuePair.Value.damage;
				}
			}
			return result;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00069A04 File Offset: 0x00067C04
		protected void AddEffectsForSpawnedChunk(Vector2 hitLocation, GameObject chunkObj, Collider2D chunkCollider, float scale = 1f)
		{
			Rigidbody2D component = chunkObj.GetComponent<Rigidbody2D>();
			if (!component)
			{
				return;
			}
			Vector2 b = (this.surfaceCollider != null) ? this.surfaceCollider.bounds.center : chunkObj.transform.position;
			Vector2 normalized = (Vector2)(((chunkCollider != null) ? chunkCollider.bounds.center : chunkObj.transform.position) - (Vector3)b).normalized;
			component.AddForce(normalized * SeededRandom.Global.RandomRange(50f, 100f) * scale);
			component.AddTorque(SeededRandom.Global.RandomRange(-5f, 5f) * scale);
			if (this.rigidbody != null)
			{
				this.rigidbody.AddForce(normalized * -30f);
			}
			Singleton<EffectManager>.Instance.PlaySmokeTrailEffect(chunkObj.gameObject, 0f, 0.1f, hitLocation, (float)UnityEngine.Random.Range(7, 10), 50f);
			if (this != null && base.gameObject)
			{
				Singleton<EffectManager>.Instance.PlaySmokeTrailEffect(base.gameObject, 0f, 0.1f, hitLocation, (float)UnityEngine.Random.Range(4, 6), 30f);
			}
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00069B58 File Offset: 0x00067D58
		public static void UpdateCollider(PolygonCollider2D collider, Sprite sprite, bool keepMinimumCollider = true)
		{
			if (!collider || !sprite)
			{
				return;
			}
			ContourTracer contourTracer = new ContourTracer();
			contourTracer.Trace(sprite.texture, new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit, 3U, 0.99f);
			List<Vector2> points = new List<Vector2>();
			List<Vector2> list = new List<Vector2>();
			if (contourTracer.pathCount > 0 || !keepMinimumCollider)
			{
				collider.pathCount = contourTracer.pathCount;
				for (int i = 0; i < collider.pathCount; i++)
				{
					contourTracer.GetPath(i, ref points);
					LineUtility.Simplify(points, 0.1f, list);
					if (list.Count > 2)
					{
						collider.SetPath(i, list);
					}
					else
					{
						collider.SetPath(i, points);
					}
				}
			}
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00069C0C File Offset: 0x00067E0C
		protected void OnTargetedHandling()
		{
			if (SpacestationExteriorManager.Instance)
			{
				if (SpacestationExteriorManager.Instance.GetDockingOption(GameplayManager.Instance.spaceShip))
				{
					SpacestationExteriorManager.Instance.CancelDocking(GameplayManager.Instance.spaceShip, true);
				}
				GameplayManager.Instance.spaceShip.ClearOverrideDestination();
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00069C68 File Offset: 0x00067E68
		public float GetDistanceFromBorder()
		{
			Rect worldCoordinates = BasePoiManager.current.worldCoordinates;
			float num = base.transform.position.x - worldCoordinates.xMin;
			float num2 = worldCoordinates.xMax - base.transform.position.x;
			float num3 = base.transform.position.y - worldCoordinates.yMin;
			float num4 = worldCoordinates.yMax - base.transform.position.y;
			return new float[]
			{
				num,
				num2,
				num3,
				num4
			}.Min();
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00069D04 File Offset: 0x00067F04
		public float GetBoundsX()
		{
			if (!this.surfaceCollider)
			{
				return 2f;
			}
			return this.surfaceCollider.bounds.size.x;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00069D3C File Offset: 0x00067F3C
		public float GetBoundsY()
		{
			if (!this.surfaceCollider)
			{
				return 2f;
			}
			return this.surfaceCollider.bounds.size.y;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00069D74 File Offset: 0x00067F74
		protected virtual void OnDestroy()
		{
			if (HudManager.Instance)
			{
				HudManager.Instance.RemoveTargetIndicator(this);
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00069D90 File Offset: 0x00067F90
		protected void AddTooltipHealthbar(UITooltip tooltip, TargetLayer layer, bool advanced = false)
		{
			TargetableHealthBar targetableHealthBar = UnityEngine.Object.Instantiate<TargetableHealthBar>(GameplayManager.Instance.healthBarPrefab, tooltip.Content);
			targetableHealthBar.SetTargetableUnit(this, layer, advanced);
			tooltip.AddContent(targetableHealthBar);
		}

		// Token: 0x0400088A RID: 2186
		private Dictionary<AbstractUnit, TargetableTracker> targetedBy = new Dictionary<AbstractUnit, TargetableTracker>();

		// Token: 0x0400088B RID: 2187
		private float trackerTimer;

		// Token: 0x0400088C RID: 2188
		protected bool hitByPlayer;

		// Token: 0x0400088E RID: 2190
		public bool isSalvage;
	}
}
