using System;
using System.Collections;
using Behaviour.Bootstrap;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using UnityEngine;

namespace Behaviour.Persistables
{
	// Token: 0x020002F4 RID: 756
	public class IndustryMissionDock : MonoBehaviour
	{
		// Token: 0x06001B99 RID: 7065 RVA: 0x000A804F File Offset: 0x000A624F
		private void Start()
		{
			this.dockCollider = base.GetComponent<Collider2D>();
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000A8060 File Offset: 0x000A6260
		private void Update()
		{
			float z = Mathf.Sin(Time.time * this.oscillationSpeed) * this.rotationAmount;
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000A80A4 File Offset: 0x000A62A4
		private void OnTriggerEnter2D(Collider2D collision)
		{
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			if (collision.gameObject == spaceShip.gameObject && !Singleton<TravelManager>.Instance.TravelActive())
			{
				this.DockWithStation();
			}
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000A80E1 File Offset: 0x000A62E1
		public void DockWithStation()
		{
			PersistentSingleton<SceneLoader>.Instance.ToggleSpaceStationInterior(true, false);
			GameplayManager.Instance.spaceShip.SetBrakeDestination();
			base.StartCoroutine(this.DisableEngines());
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000A810C File Offset: 0x000A630C
		public bool OverlapsCollider(Collider2D collider2D)
		{
			return this.dockCollider.bounds.Intersects(collider2D.bounds);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000A8132 File Offset: 0x000A6332
		private IEnumerator DisableEngines()
		{
			yield return new WaitForSeconds(1f);
			GameplayManager.Instance.spaceShip.SetEngineState(false, false);
			yield break;
		}

		// Token: 0x04001150 RID: 4432
		[SerializeField]
		private float rotationAmount = 15f;

		// Token: 0x04001151 RID: 4433
		[SerializeField]
		private float oscillationSpeed = 2f;

		// Token: 0x04001152 RID: 4434
		private Collider2D dockCollider;
	}
}
