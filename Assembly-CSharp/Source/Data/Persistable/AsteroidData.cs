using System;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Equipment.Module;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Mining;
using Source.Util;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000110 RID: 272
	public class AsteroidData : PersistableData
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0004E13E File Offset: 0x0004C33E
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x0004E146 File Offset: 0x0004C346
		public float scale { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0004E14F File Offset: 0x0004C34F
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x0004E157 File Offset: 0x0004C357
		public AsteroidSize size { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0004E160 File Offset: 0x0004C360
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x0004E168 File Offset: 0x0004C368
		public OreItemData surfaceItem { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0004E171 File Offset: 0x0004C371
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x0004E179 File Offset: 0x0004C379
		public int surfaceAmount { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0004E182 File Offset: 0x0004C382
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0004E18A File Offset: 0x0004C38A
		public int surfaceMaxHealth { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0004E193 File Offset: 0x0004C393
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x0004E19B File Offset: 0x0004C39B
		public int currentSurfaceHealth { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0004E1A4 File Offset: 0x0004C3A4
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0004E1AC File Offset: 0x0004C3AC
		public int accumulatedSurfaceDamage { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0004E1B5 File Offset: 0x0004C3B5
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0004E1BD File Offset: 0x0004C3BD
		public bool hasInnerCore { get; private set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0004E1C6 File Offset: 0x0004C3C6
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0004E1CE File Offset: 0x0004C3CE
		public OreItemData innerCoreItem { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0004E1D7 File Offset: 0x0004C3D7
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0004E1DF File Offset: 0x0004C3DF
		public int innerCoreAmount { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0004E1E8 File Offset: 0x0004C3E8
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x0004E1F0 File Offset: 0x0004C3F0
		public int innerCoreMaxHealth { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0004E1F9 File Offset: 0x0004C3F9
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0004E201 File Offset: 0x0004C401
		public int currentInnerCoreHealth { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0004E20A File Offset: 0x0004C40A
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x0004E212 File Offset: 0x0004C412
		public int accumulatedInnerCoreDamage { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0004E21B File Offset: 0x0004C41B
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x0004E223 File Offset: 0x0004C423
		public string seed { get; private set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0004E22C File Offset: 0x0004C42C
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x0004E234 File Offset: 0x0004C434
		public int scanProgress { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0004E23D File Offset: 0x0004C43D
		// (set) Token: 0x06000A67 RID: 2663 RVA: 0x0004E245 File Offset: 0x0004C445
		public AsteroidSize? parentSize { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0004E24E File Offset: 0x0004C44E
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x0004E256 File Offset: 0x0004C456
		public List<SpriteBreakPoint> breakPoints { get; private set; } = new List<SpriteBreakPoint>();

		// Token: 0x06000A6A RID: 2666 RVA: 0x0004E25F File Offset: 0x0004C45F
		public AsteroidData() : this("")
		{
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0004E26C File Offset: 0x0004C46C
		public AsteroidData(string seed)
		{
			if (seed == "")
			{
				seed = SeededRandom.Global.RandomString(16);
			}
			this.seed = seed;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0004E2CC File Offset: 0x0004C4CC
		public void InitAsteroid(MapPointOfInterest poi, AsteroidFieldData asteroidFieldData, Vector2 position)
		{
			SeededRandom global = SeededRandom.Global;
			this.position = position;
			this.angle = (float)global.RandomRange(0, 360);
			this.velocity = new Vector2(global.RandomRange(-0.05f, 0.05f), global.RandomRange(-0.05f, 0.05f));
			this.angularVelocity = global.RandomRange(-10f, 10f);
			this.surfaceItem = asteroidFieldData.GetRandomOre(true, null);
			int num = 0;
			do
			{
				this.innerCoreItem = asteroidFieldData.GetRandomOre(false, null);
				num++;
			}
			while (this.surfaceItem == this.innerCoreItem && num < 5);
			this.size = AsteroidHelper.GetRandomAsteroidSize(asteroidFieldData.wealth, asteroidFieldData.excludeSizes);
			this.scale = AsteroidHelper.GetAsteroidScale(this.size);
			this.hasInnerCore = global.RandomBool(asteroidFieldData.GetCoreChance(this.size));
			if (this.hasInnerCore)
			{
				this.innerCoreAmount = AsteroidHelper.GetInnerCoreAmount(this.scale);
			}
			this.surfaceAmount = AsteroidHelper.GetSurfaceAmount(this.scale);
			this.UpdateAsteroidHealth();
			if (poi.CanSpawnHazard())
			{
				this.hazardData = poi.CreateHazardData();
			}
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0004E3F8 File Offset: 0x0004C5F8
		private void UpdateAsteroidHealth()
		{
			if (this.hasInnerCore)
			{
				this.innerCoreMaxHealth = Mathf.Max(this.innerCoreMaxHealth, this.innerCoreItem.health * this.innerCoreAmount);
				this.currentInnerCoreHealth = this.innerCoreItem.health * this.innerCoreAmount;
			}
			this.surfaceMaxHealth = Mathf.Max(this.surfaceMaxHealth, this.surfaceItem.health * this.surfaceAmount);
			this.currentSurfaceHealth = this.surfaceItem.health * this.surfaceAmount;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0004E484 File Offset: 0x0004C684
		public AsteroidData GetAsteroidDataForChild(AsteroidSize childSize, int childSurfaceAmount, int childCoreAmount)
		{
			AsteroidData asteroidData = new AsteroidData(this.seed);
			asteroidData.position = new Vector3(this.position.x, this.position.y, -1f);
			asteroidData.angle = this.angle;
			asteroidData.surfaceItem = this.surfaceItem;
			asteroidData.surfaceAmount = childSurfaceAmount;
			asteroidData.innerCoreItem = this.innerCoreItem;
			asteroidData.innerCoreAmount = childCoreAmount;
			asteroidData.hasInnerCore = (childCoreAmount > 0);
			asteroidData.size = childSize;
			asteroidData.scale = AsteroidHelper.GetAsteroidScale(childSize);
			asteroidData.UpdateAsteroidHealth();
			if (this.parentSize != null)
			{
				asteroidData.parentSize = this.parentSize;
			}
			else
			{
				asteroidData.parentSize = new AsteroidSize?(this.size);
			}
			return asteroidData;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0004E550 File Offset: 0x0004C750
		public override bool ItemCanBeObtained(InventoryItemType itemType)
		{
			if (this.surfaceItem.item == itemType)
			{
				if (this.surfaceAmount == 0)
				{
					return false;
				}
				GameplayManager instance = GameplayManager.Instance;
				MiningModule miningModule;
				if (instance == null)
				{
					miningModule = null;
				}
				else
				{
					SpaceShip spaceShip = instance.spaceShip;
					miningModule = ((spaceShip != null) ? spaceShip.GetModule<MiningModule>() : null);
				}
				MiningModule miningModule2 = miningModule;
				return miningModule2 && miningModule2.canMineSurface;
			}
			else
			{
				if (!(this.innerCoreItem.item == itemType))
				{
					return false;
				}
				if (this.innerCoreAmount == 0)
				{
					return false;
				}
				GameplayManager instance2 = GameplayManager.Instance;
				MiningModule miningModule3;
				if (instance2 == null)
				{
					miningModule3 = null;
				}
				else
				{
					SpaceShip spaceShip2 = instance2.spaceShip;
					miningModule3 = ((spaceShip2 != null) ? spaceShip2.GetModule<MiningModule>() : null);
				}
				MiningModule miningModule4 = miningModule3;
				return miningModule4 && miningModule4.canMineCore;
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0004E5F6 File Offset: 0x0004C7F6
		public void SetBreakpoints(List<SpriteBreakPoint> breakPoints)
		{
			this.breakPoints = new List<SpriteBreakPoint>(breakPoints);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0004E604 File Offset: 0x0004C804
		public void SetMySpriteBreakpointIndex()
		{
			this.mySpriteBreakpointIndex = this.breakPoints.Count - 1;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0004E619 File Offset: 0x0004C819
		public override bool ShouldCleanUp()
		{
			return true;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0004E61C File Offset: 0x0004C81C
		public override void DataToJson(JsonObject data)
		{
			data["seed"] = this.seed;
			data["scale"] = new double?((double)this.scale);
			data["position"] = JsonUtil.Vector2ToJson(this.position);
			data["size"] = this.size.ToString();
			data["surfaceOre"] = this.surfaceItem.item.identifier;
			data["surfaceAmount"] = new double?((double)this.surfaceAmount);
			data["accumulatedSurfaceDamage"] = new double?((double)this.accumulatedSurfaceDamage);
			if (this.innerCoreItem)
			{
				data["innerCoreOre"] = this.innerCoreItem.item.identifier;
			}
			data["innerCoreAmount"] = new double?((double)this.innerCoreAmount);
			data["hasInnerCore"] = new bool?(this.hasInnerCore);
			data["accumulatedInnerCoreDamage"] = new double?((double)this.accumulatedInnerCoreDamage);
			data["breakpoints"] = this.breakPoints.ToJsonArray<SpriteBreakPoint>();
			data["scanProgress"] = new double?((double)this.scanProgress);
			data["mySpriteBreakpointIndex"] = new double?((double)this.mySpriteBreakpointIndex);
			if (this.parentSize != null)
			{
				data["parentSize"] = this.parentSize.ToString();
			}
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0004E7FC File Offset: 0x0004C9FC
		public override void LoadFromJson(JsonObject json)
		{
			this.seed = json["seed"];
			this.size = Enum.Parse<AsteroidSize>(json["size"]);
			this.scale = (float)json["scale"].AsNumber;
			this.surfaceItem = json["surfaceOre"].AsString;
			this.surfaceAmount = json["surfaceAmount"].AsInteger;
			this.accumulatedSurfaceDamage = json["accumulatedSurfaceDamage"];
			if (!json["innerCoreOre"].IsNull)
			{
				this.innerCoreItem = json["innerCoreOre"].AsString;
			}
			this.innerCoreAmount = json["innerCoreAmount"].AsInteger;
			this.accumulatedInnerCoreDamage = json["accumulatedInnerCoreDamage"];
			this.hasInnerCore = json["hasInnerCore"].AsBoolean;
			this.scanProgress = json["scanProgress"].AsInteger;
			this.breakPoints.FromJsonArray(json["breakpoints"], new ClassExtensions.ParseJsonValue<SpriteBreakPoint>(SpriteBreakPoint.FromJson));
			this.mySpriteBreakpointIndex = json["mySpriteBreakpointIndex"].AsInteger;
			if (!json["parentSize"].IsNull)
			{
				this.parentSize = new AsteroidSize?(Enum.Parse<AsteroidSize>(json["parentSize"]));
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0004E9AC File Offset: 0x0004CBAC
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			this.UpdateAsteroidHealth();
			Asteroid asteroid = UnityEngine.Object.Instantiate<Asteroid>(PersistentSingleton<GameManager>.Instance.asteroidPrefab, this.position, base.rotation, parent.transform);
			asteroid.InitAsteroid(this, true);
			base.AddHazardToWorld(asteroid.gameObject, this.GetRangeMultiplier());
			return asteroid.gameObject;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0004EA08 File Offset: 0x0004CC08
		public float GetRangeMultiplier()
		{
			switch (this.size)
			{
			case AsteroidSize.Tiny:
				return 0.5f;
			case AsteroidSize.Small:
				return 0.8f;
			case AsteroidSize.Medium:
				return 1f;
			case AsteroidSize.Large:
				return 1.2f;
			case AsteroidSize.Huge:
				return 1.5f;
			default:
				return 1f;
			}
		}

		// Token: 0x040005AC RID: 1452
		public int mySpriteBreakpointIndex = -1;

		// Token: 0x040005AE RID: 1454
		public float minRotationSpeed = 5f;

		// Token: 0x040005AF RID: 1455
		public float maxRotationSpeed = 10f;
	}
}
