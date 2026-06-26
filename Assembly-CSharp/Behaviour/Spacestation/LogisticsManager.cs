using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Spacestation.Docking;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Spacestation
{
	// Token: 0x020002DC RID: 732
	public class LogisticsManager : MonoBehaviour
	{
		// Token: 0x06001AB8 RID: 6840 RVA: 0x000A51A9 File Offset: 0x000A33A9
		private void Awake()
		{
			LogisticsManager.Instance = this;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000A51B4 File Offset: 0x000A33B4
		public void TransferToShip(DockingPad dockingPad)
		{
			if (dockingPad.waypointPath.Count == 0)
			{
				dockingPad.SetWayPointPath();
			}
			this.ship = dockingPad.dockingSpaceship;
			WaypointPath waypointPath = new WaypointPath(new List<Transform>(dockingPad.waypointPath.points));
			waypointPath.ReversePoints();
			this.waypointPath = waypointPath;
			this.spawnPosition = this.waypointPath.points.First<Transform>();
			base.StartCoroutine(this.SpawnAndMove(this.crewPrefab));
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000A522C File Offset: 0x000A342C
		public void TransferFromShip(GameObject prefab, DockingPad dockingPad)
		{
			if (dockingPad.waypointPath.Count == 0)
			{
				dockingPad.SetWayPointPath();
			}
			this.waypointPath = dockingPad.waypointPath;
			this.spawnPosition = this.waypointPath.points.First<Transform>();
			base.StartCoroutine(this.SpawnAndMove(prefab));
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000A527C File Offset: 0x000A347C
		private IEnumerator SpawnAndMove(GameObject prefab)
		{
			if (this.ship.rampManager && !this.ship.rampManager.rampsOpen)
			{
				base.StartCoroutine(this.ship.rampManager.ToggleRamps(true));
			}
			yield return new WaitUntil(() => this.ship.rampManager.rampsOpen);
			Vector3 position = this.spawnPosition.position;
			position.z = prefab.transform.position.z;
			this.spawnPosition.transform.position = position;
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(prefab, this.spawnPosition.position, Quaternion.identity);
			WaypointMover component = obj.GetComponent<WaypointMover>();
			yield return component.FollowPath(this.waypointPath);
			UnityEngine.Object.Destroy(obj);
			yield break;
		}

		// Token: 0x040010DF RID: 4319
		public static LogisticsManager Instance;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private GameObject cargoPrefab;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private GameObject crewPrefab;

		// Token: 0x040010E2 RID: 4322
		private WaypointPath waypointPath = new WaypointPath();

		// Token: 0x040010E3 RID: 4323
		private Transform spawnPosition;

		// Token: 0x040010E4 RID: 4324
		private SpaceShip ship;
	}
}
