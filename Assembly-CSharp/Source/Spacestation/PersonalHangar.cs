using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Spacestation
{
	// Token: 0x02000055 RID: 85
	public class PersonalHangar
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0001C6BD File Offset: 0x0001A8BD
		public static PersonalHangar current
		{
			get
			{
				SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
				if (spaceStation == null)
				{
					return null;
				}
				return spaceStation.personalHangar;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001C6D4 File Offset: 0x0001A8D4
		public PersonalHangar(SpaceStation owner)
		{
			this.owner = owner;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001C6F0 File Offset: 0x0001A8F0
		public void TryStartAutoRepair(float amount, string shipGuid)
		{
			if (this.jobs.FirstOrDefault((RepairJob job) => job.spaceshipGuid == shipGuid) == null)
			{
				this.jobs.Add(new RepairJob(Mathf.CeilToInt(amount), shipGuid, true, this.owner.faction));
				SpaceStationInterior.instance.UpdateJobs();
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0001C758 File Offset: 0x0001A958
		public void StartRepair(float amount, string shipGuid, bool autoRepair = false, int repairCost = 0)
		{
			RepairJob repairJob = this.jobs.FirstOrDefault((RepairJob job) => job.spaceshipGuid == shipGuid);
			if (repairJob != null && !repairJob.autoRepair && !autoRepair)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSRepairInProgress", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			GamePlayer.current.credits -= (long)repairCost;
			if (repairJob != null && repairJob.autoRepair)
			{
				this.CancelJob(repairJob, true);
			}
			this.jobs.Add(new RepairJob(Mathf.CeilToInt(amount), shipGuid, autoRepair, this.owner.faction));
			SpaceStationInterior.instance.UpdateJobs();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0001C81C File Offset: 0x0001AA1C
		public void ProgressJobs(float deltaTime)
		{
			List<RepairJob> list = new List<RepairJob>();
			foreach (RepairJob repairJob in this.jobs)
			{
				repairJob.ProgressJob(deltaTime);
				if (repairJob.remainingAmount <= 0)
				{
					list.Add(repairJob);
					MissionObjective.Trigger(MissionTrigger.PersonalHangarRepair, null, null, false);
					if (SidePanel.instance.currentTab == SidePanel.SideTabType.Ship)
					{
						SidePanel.instance.RefreshIfOpen();
					}
					SpaceStationInterior instance = SpaceStationInterior.instance;
					if (instance)
					{
						instance.UpdateJobs();
						if (instance.currentTab == SpaceStationFacility.PersonalHangar)
						{
							instance.GoToLocation(SpaceStationFacility.PersonalHangar, false);
						}
					}
				}
			}
			foreach (RepairJob item in list)
			{
				this.jobs.Remove(item);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001C914 File Offset: 0x0001AB14
		public bool CancelJob(RepairJob job, bool autoRepair = true)
		{
			if (job == null)
			{
				return false;
			}
			this.jobs.Remove(job);
			if (!job.autoRepair)
			{
				GamePlayer.current.credits += (long)job.remainingAmount;
				if (job.spaceshipData != null)
				{
					SpaceShipData spaceshipData = job.spaceshipData;
					float num = Mathf.Max(spaceshipData.HullDamageTaken(), spaceshipData.ArmorDamageTaken());
					if (!job.autoRepair && num > 0f)
					{
						this.StartRepair(num, spaceshipData.guid, true, 0);
					}
				}
			}
			return true;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001C994 File Offset: 0x0001AB94
		public bool CancelJobLeavingStation(string shipGuid, bool autoRepair = true)
		{
			RepairJob repairJob = this.jobs.FirstOrDefault((RepairJob job) => job.spaceshipGuid == shipGuid);
			if (repairJob != null)
			{
				this.jobs.Remove(repairJob);
				if (!repairJob.autoRepair)
				{
					GamePlayer.current.credits += (long)repairJob.remainingAmount;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001C9F9 File Offset: 0x0001ABF9
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"jobs",
					this.jobs.ToJsonArray<RepairJob>()
				}
			};
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001CA24 File Offset: 0x0001AC24
		public static PersonalHangar FromJson(SpaceStation parent, JsonValue json)
		{
			PersonalHangar personalHangar = new PersonalHangar(parent);
			if (json2["jobs"].IsJsonArray)
			{
				personalHangar.jobs.FromJsonArray(json2["jobs"], (JsonValue json) => RepairJob.FromJson(json));
			}
			return personalHangar;
		}

		// Token: 0x040001DA RID: 474
		public readonly SpaceStation owner;

		// Token: 0x040001DB RID: 475
		public List<RepairJob> jobs = new List<RepairJob>();
	}
}
