using System;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit.Parts
{
	// Token: 0x020001CF RID: 463
	public class UmbralTrackingBot : MonoBehaviour
	{
		// Token: 0x0600117C RID: 4476 RVA: 0x0007405C File Offset: 0x0007225C
		private void SetupTarget()
		{
			SpaceShip spaceShip = null;
			float num = 0f;
			foreach (SpaceShip spaceShip2 in BasePoiManager.current.GetComponentsInChildren<SpaceShip>())
			{
				if (!spaceShip2.IsPlayerEnemy() && !(spaceShip2.autoActions is TrackingTargetActions))
				{
					float num2 = Vector2.Distance(base.transform.position, spaceShip2.transform.position);
					if (spaceShip == null || num > num2)
					{
						spaceShip = spaceShip2;
						num = num2;
					}
				}
			}
			if (spaceShip)
			{
				this.target = spaceShip;
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralTrackingBeaconDeployed", new object[]
				{
					FactionInfo.GetAbbreviation(this.target.faction) + " " + this.target.displayName
				})).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0007414C File Offset: 0x0007234C
		private void Update()
		{
			if (!this.target)
			{
				this.targetTimer -= Time.deltaTime;
				if (this.targetTimer < 0f)
				{
					this.SetupTarget();
					this.targetTimer = 0.5f;
				}
				return;
			}
			Vector3 normalized = (this.target.transform.position - base.transform.position).normalized;
			float z = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			if (Vector2.Distance(base.transform.position, this.target.transform.position) < 1f)
			{
				this.StartTracking(this.target);
				return;
			}
			base.transform.position = Vector2.MoveTowards(base.transform.position, this.target.transform.position, Time.deltaTime * 5f);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0007427C File Offset: 0x0007247C
		private void StartTracking(SpaceShip target)
		{
			if (target.unitData.canBeTracked)
			{
				UIInfoTextParent.instance.ShowWarningText("@UmbralTrackingBeaconInstalled", base.transform, new Color?(Color.white));
				MissionObjective.Trigger(MissionTrigger.PlaceTracker, null, null, false);
				Register.AddCounter("TrackerPlaced", 1, 0);
				target.spaceShipData.autoActions = "TrackingTarget";
				target.ResetAutoActions();
			}
			else
			{
				UIInfoTextParent.instance.ShowWarningText("@UmbralTrackingNotPossible", base.transform, new Color?(Color.white));
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000998 RID: 2456
		private SpaceShip target;

		// Token: 0x04000999 RID: 2457
		private float targetTimer;
	}
}
