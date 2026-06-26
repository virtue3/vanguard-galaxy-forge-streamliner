using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Managers;
using Behaviour.Unit;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.SpaceShip;
using UnityEngine;

namespace Behaviour.Travel
{
	// Token: 0x020002C8 RID: 712
	public class JumpGateManager : BasePoiManager
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x000A1F2A File Offset: 0x000A012A
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x000A1F32 File Offset: 0x000A0132
		public TheGate jumpGate { get; private set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x000A1F3B File Offset: 0x000A013B
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x000A1F43 File Offset: 0x000A0143
		public JumpGate jumpGatePoi { get; private set; }

		// Token: 0x06001A05 RID: 6661 RVA: 0x000A1F4C File Offset: 0x000A014C
		protected override void Awake()
		{
			base.Awake();
			JumpGateManager.instance = this;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000A1F5A File Offset: 0x000A015A
		protected override void Update()
		{
			base.Update();
			if (this.queueTimer < 0f)
			{
				this.NextShip();
				this.queueTimer = 2f;
			}
			this.queueTimer -= Time.deltaTime;
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x000A1F94 File Offset: 0x000A0194
		private void OnDestroy()
		{
			GamePlayer current = GamePlayer.current;
			if (current != null && current.IsInSandBox())
			{
				foreach (SpaceShip spaceShip in base.GetComponentsInChildren<SpaceShip>())
				{
					base.poi.RemoveUnit(spaceShip.spaceShipData);
				}
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000A1FE0 File Offset: 0x000A01E0
		protected override void SetWorldCoordinates()
		{
			base.SetWorldCoordinates();
			Rect rect = new Rect(base.transform.position.x - 75f, base.transform.position.y - this.screenSize.y, 150f, this.screenSize.y * 2f);
			float num = Mathf.Min(base.worldCoordinates.xMin, rect.xMin);
			float num2 = Mathf.Min(base.worldCoordinates.yMin, rect.yMin);
			float num3 = Mathf.Max(base.worldCoordinates.xMax, rect.xMax);
			float num4 = Mathf.Max(base.worldCoordinates.yMax, rect.yMax);
			base.worldCoordinates = new Rect(num, num2, num3 - num, num4 - num2);
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000A20C7 File Offset: 0x000A02C7
		protected override IEnumerator InitializePoi()
		{
			this.jumpGatePoi = (JumpGate)base.poi;
			if (this.jumpGatePoi != null && this.jumpGatePoi.lastVisitedTime == 0f && this.jumpGatePoi.GetPersistables().Count == 0)
			{
				this.jumpGatePoi.AddWindowDressing();
			}
			yield return base.InitializePoi();
			TravelDirection travelDirection = this.jumpGatePoi.GetTravelDirection();
			Debug.Log("Poi: " + base.poi.name + ", travelDirection: " + travelDirection.ToString());
			Quaternion rotation = (travelDirection == TravelDirection.Right) ? Quaternion.identity : Quaternion.Euler(new Vector3(0f, 0f, 180f));
			TheGate original = this.jumpGatePrefab;
			if (this.jumpGatePoi.system.sector.quadrant == SectorMapData.quadrantPrologue)
			{
				original = (this.jumpGatePoi.sectorJumpgate ? this.tutorialSectorGatePrefab : this.tutorialGatePrefab);
			}
			else if (base.poi is GreatGate)
			{
				original = this.greatGatePrefab;
			}
			else if (this.jumpGatePoi.sectorJumpgate)
			{
				original = this.sectorGatePrefab;
			}
			this.jumpGate = UnityEngine.Object.Instantiate<TheGate>(original, base.transform.position, rotation, base.transform);
			this.jumpGate.SetJumpGatePoi(this.jumpGatePoi);
			yield return null;
			yield break;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x000A20D8 File Offset: 0x000A02D8
		public Vector2 GetJumpGateLandingPosition(float boundsX)
		{
			Vector2 result = this.jumpGate.transform.position;
			result.x += ((this.jumpGate.travelDirection == TravelDirection.Right) ? (boundsX * 2f) : (-boundsX * 2f));
			return result;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x000A2126 File Offset: 0x000A0326
		public override void SpaceshipHasArrived()
		{
			base.SpaceshipHasArrived();
			this.InitiateTravelThroughGate();
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000A2134 File Offset: 0x000A0334
		protected override IEnumerator InitializationComplete()
		{
			yield return base.InitializationComplete();
			if (GamePlayer.current.IsInSandBox() && !base.poi.faction.IsEnemy(Faction.player))
			{
				base.StartCoroutine(this.NPCSpawner());
			}
			yield return null;
			yield break;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000A2143 File Offset: 0x000A0343
		public void InitiateTravelThroughGate()
		{
			if (GamePlayer.current.nextWaypointIsSystem(this.jumpGatePoi.targetSystemGuid))
			{
				this.jumpGate.SetJumpingShip(GameplayManager.Instance.spaceShip, JumpAction.Leaving);
			}
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000A2172 File Offset: 0x000A0372
		private IEnumerator NPCSpawner()
		{
			for (;;)
			{
				yield return new WaitForSeconds((float)UnityEngine.Random.Range(5, 60));
				this.CreatePasserbyShip();
			}
			yield break;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000A2184 File Offset: 0x000A0384
		public void CreatePasserbyShip()
		{
			if (base.poi.GetUnits(false).Count<AbstractUnitData>() + base.poi.GetTriggeredPayloads().Count<MapTriggeredPayload>() > 10)
			{
				return;
			}
			JumpAction jumpAction = SeededRandom.Global.RandomBool(0.5f) ? JumpAction.Arriving : JumpAction.Leaving;
			Vector2 spawnPosition = this.jumpGate.transform.position;
			Debug.Log("Create unit for: " + jumpAction.ToString());
			if (jumpAction == JumpAction.Arriving)
			{
				spawnPosition.x += (float)((this.jumpGate.travelDirection == TravelDirection.Left) ? -10 : 10);
				SpaceShipData spaceShipData = base.CreatePasserbyShipData(spawnPosition, null);
				spaceShipData.autoActions = "JumpGate";
				spaceShipData.positionData.rotation = (float)((this.jumpGate.travelDirection == TravelDirection.Left) ? 0 : 180);
				base.poi.AddUnit(spaceShipData, null, false);
				return;
			}
			spawnPosition.x += (float)((this.jumpGate.travelDirection == TravelDirection.Left) ? 20 : -20);
			spawnPosition.y += (float)(SeededRandom.Global.RandomBool(0.5f) ? -30 : 30);
			SpaceShipData spaceShipData2 = base.CreatePasserbyShipData(spawnPosition, null);
			spaceShipData2.autoActions = "JumpGate";
			MapTriggeredPayload mapTriggeredPayload = new MapTriggeredPayload(base.poi);
			mapTriggeredPayload.units.Add(spaceShipData2);
			mapTriggeredPayload.spawnAtPlayer = false;
			base.poi.AddPayload(mapTriggeredPayload);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000A2300 File Offset: 0x000A0500
		public IEnumerator ArriveAtGate()
		{
			GameplayerPrefs.SetBackgroundSystem(base.poi.system.name);
			yield return this.jumpGate.ArriveAtGate(GameplayManager.Instance.spaceShip);
			yield break;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000A2310 File Offset: 0x000A0510
		public Vector2 GetArrivalDestination()
		{
			int num = (this.jumpGate.travelDirection == TravelDirection.Right) ? -1 : 1;
			Vector2 result = this.jumpGate.transform.position;
			result.x += (float)(num * 20);
			return result;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000A2357 File Offset: 0x000A0557
		public IEnumerator ExecuteFastLaneTravel()
		{
			yield return null;
			yield break;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000A2360 File Offset: 0x000A0560
		private void NextShip()
		{
			if (this.jumpQueue.Count > 0 && this.jumpGate.jumpingShip == null)
			{
				KeyValuePair<SpaceShip, JumpAction> keyValuePair = this.jumpQueue.First<KeyValuePair<SpaceShip, JumpAction>>();
				Debug.Log("Grab next ship: " + keyValuePair.Key + ", " + keyValuePair.Value.ToString());
				this.jumpGate.SetJumpingShip(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000A23E8 File Offset: 0x000A05E8
		public void AddJumpingSpaceShip(SpaceShip spaceShip, JumpAction jumpAction)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Adding ship: ",
				spaceShip.name,
				" for: ",
				jumpAction.ToString(),
				", count: ",
				this.jumpQueue.Count.ToString()
			}));
			this.jumpQueue.Add(spaceShip, jumpAction);
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000A2459 File Offset: 0x000A0659
		public void RemoveJumpingSpaceShip(SpaceShip spaceShip, bool destroy = false)
		{
			Debug.Log("Removing ship from queue: " + spaceShip);
			this.jumpQueue.Remove(spaceShip);
			if (destroy)
			{
				UnityEngine.Object.Destroy(spaceShip.gameObject);
			}
		}

		// Token: 0x0400105A RID: 4186
		public static JumpGateManager instance;

		// Token: 0x0400105B RID: 4187
		[SerializeField]
		private TheGate tutorialGatePrefab;

		// Token: 0x0400105C RID: 4188
		[SerializeField]
		private TheGate tutorialSectorGatePrefab;

		// Token: 0x0400105D RID: 4189
		[SerializeField]
		private TheGate jumpGatePrefab;

		// Token: 0x0400105E RID: 4190
		[SerializeField]
		private TheGate sectorGatePrefab;

		// Token: 0x0400105F RID: 4191
		[SerializeField]
		private TheGate greatGatePrefab;

		// Token: 0x04001062 RID: 4194
		private Dictionary<SpaceShip, JumpAction> jumpQueue = new Dictionary<SpaceShip, JumpAction>();

		// Token: 0x04001063 RID: 4195
		private float queueTimer = 5f;

		// Token: 0x04001064 RID: 4196
		private bool playerNext;
	}
}
