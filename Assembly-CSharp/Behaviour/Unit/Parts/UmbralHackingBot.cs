using System;
using System.Collections;
using Behaviour.Effects;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Data;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit.Parts
{
	// Token: 0x020001CE RID: 462
	public class UmbralHackingBot : MonoBehaviour
	{
		// Token: 0x06001175 RID: 4469 RVA: 0x00073D9C File Offset: 0x00071F9C
		private void Start()
		{
			foreach (CombatStationPart combatStationPart in BasePoiManager.current.GetComponentsInChildren<CombatStationPart>())
			{
				if (combatStationPart.partType == CombatStationPartType.Core)
				{
					this.target = combatStationPart;
					break;
				}
			}
			MissionObjective.Trigger(MissionTrigger.UmbralStationInfected, null, null, false);
			this.CreateLaserEffect();
			ConquestSystem conquestSystem = MapPointOfInterest.current.system.storyteller as ConquestSystem;
			if (conquestSystem != null)
			{
				conquestSystem.umbralControlLevel = this.controlPercentage;
				GamePlayer.current.GetStoryteller<Conquest>().umbralContribution += 5;
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralHackingBotDeployed", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00073E54 File Offset: 0x00072054
		private void OnEnable()
		{
			LaserEffect laserEffect = this.laserEffect;
			if (laserEffect == null)
			{
				return;
			}
			laserEffect.visualEffect.Reinit();
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00073E6C File Offset: 0x0007206C
		private void CreateLaserEffect()
		{
			this.laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserPrefab, Vector3.zero, Quaternion.identity, base.transform);
			this.laserEffect.transform.localPosition = Vector2.zero;
			this.laserEffect.transform.localRotation = Quaternion.identity;
			this.laserEffect.Stop();
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00073ED4 File Offset: 0x000720D4
		private IEnumerator PlayLaserEffect(AbstractUnit target)
		{
			this.laserEffect.SetObjectsToTrack(base.gameObject, target.gameObject);
			this.laserEffect.Play();
			yield return new WaitForSeconds(4.5f);
			this.laserEffect.Stop();
			this.SetupSystemControl();
			yield break;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00073EEC File Offset: 0x000720EC
		private void Update()
		{
			if (!this.target)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Vector3 normalized = (this.target.transform.position - base.transform.position).normalized;
			float z = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			if (Vector2.Distance(base.transform.position, this.target.transform.position) > 2f)
			{
				base.transform.position = Vector2.MoveTowards(base.transform.position, this.target.transform.position, Time.deltaTime * 2f);
				return;
			}
			if (!this.laserFired)
			{
				this.laserFired = true;
				base.StartCoroutine(this.PlayLaserEffect(this.target));
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00074007 File Offset: 0x00072207
		public void SetupSystemControl()
		{
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralHackingBotSuccess", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000993 RID: 2451
		[SerializeField]
		private LaserEffect laserPrefab;

		// Token: 0x04000994 RID: 2452
		public float controlPercentage = 0.05f;

		// Token: 0x04000995 RID: 2453
		private LaserEffect laserEffect;

		// Token: 0x04000996 RID: 2454
		private CombatStationPart target;

		// Token: 0x04000997 RID: 2455
		private bool laserFired;
	}
}
