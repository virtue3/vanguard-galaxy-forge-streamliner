using System;
using Behaviour.UI.Spacestation;
using LightJson;
using Source.Galaxy;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Spacestation
{
	// Token: 0x02000056 RID: 86
	public class RepairJob : IJsonSource, ISpaceStationJob
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0001CA8A File Offset: 0x0001AC8A
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0001CA92 File Offset: 0x0001AC92
		public int initialAmount { get; protected set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0001CA9B File Offset: 0x0001AC9B
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0001CAA3 File Offset: 0x0001ACA3
		public int remainingAmount { get; protected set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0001CAAC File Offset: 0x0001ACAC
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		public float repairTime { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0001CABD File Offset: 0x0001ACBD
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0001CAC5 File Offset: 0x0001ACC5
		public float repairAmount { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0001CACE File Offset: 0x0001ACCE
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0001CAD6 File Offset: 0x0001ACD6
		public bool autoRepair { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0001CADF File Offset: 0x0001ACDF
		public bool cancelAvailable
		{
			get
			{
				return !this.autoRepair;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0001CAEA File Offset: 0x0001ACEA
		public string jobName
		{
			get
			{
				if (!this.autoRepair)
				{
					return "@SSRepair";
				}
				return "@SSAutoRepair";
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0001CAFF File Offset: 0x0001ACFF
		public Sprite jobIcon
		{
			get
			{
				return SpaceStationInterior.instance.repairIcon;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0001CB0B File Offset: 0x0001AD0B
		public float jobProgress
		{
			get
			{
				return this.progress / this.repairTime;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001CB1C File Offset: 0x0001AD1C
		public RepairJob(int initialAmount, string spaceshipGuid, bool autoRepair = false, Faction faction = null)
		{
			this.initialAmount = initialAmount;
			this.remainingAmount = initialAmount;
			this.repairAmount = Mathf.Max(1f, (float)initialAmount / 200f);
			this.spaceshipGuid = spaceshipGuid;
			foreach (SpaceShipData spaceShipData in GamePlayer.current.spaceShips)
			{
				if (spaceShipData.guid == spaceshipGuid)
				{
					this.spaceshipData = spaceShipData;
					break;
				}
			}
			float num = 1f;
			if (faction != null)
			{
				num = faction.GetReputationLevel(Faction.player).GetRepairSpeedMultiplier();
			}
			this.autoRepair = autoRepair;
			this.repairTime = (autoRepair ? 1f : 0.1f);
			this.repairTime *= num;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001CBFC File Offset: 0x0001ADFC
		public void ProgressJob(float deltaTime)
		{
			this.progress += deltaTime;
			if (this.progress >= this.repairTime)
			{
				if (this.spaceshipData == null)
				{
					this.CancelJob();
				}
				this.spaceshipData.RepairHullHp(this.repairAmount);
				this.spaceshipData.RepairArmorHP(this.repairAmount);
				if (this.spaceshipData.unit)
				{
					this.spaceshipData.unit.RepairSpritePixels(0.02f);
				}
				this.progress -= this.repairTime;
				this.remainingAmount = Mathf.CeilToInt(Mathf.Max(this.spaceshipData.HullDamageTaken(), this.spaceshipData.ArmorDamageTaken()));
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001CCB8 File Offset: 0x0001AEB8
		public void CancelJob()
		{
			PersonalHangar.current.CancelJob(this, true);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"progress",
					new double?((double)this.progress)
				},
				{
					"initialAmount",
					new double?((double)this.initialAmount)
				},
				{
					"repairAmount",
					new double?((double)this.repairAmount)
				},
				{
					"remainingAmount",
					new double?((double)this.remainingAmount)
				},
				{
					"autoRepair",
					new bool?(this.autoRepair)
				},
				{
					"repairTime",
					new double?((double)this.repairTime)
				},
				{
					"shipguid",
					this.spaceshipGuid
				}
			};
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
		public static RepairJob FromJson(JsonValue json)
		{
			if (json.IsNull || json["shipguid"].IsNull)
			{
				return null;
			}
			RepairJob repairJob = new RepairJob(json["initialAmount"], json["shipguid"], json["autoRepair"].AsBoolean, null);
			repairJob.repairAmount = Mathf.Max(1f, (float)json["repairAmount"].AsNumber);
			repairJob.remainingAmount = json["remainingAmount"];
			repairJob.progress = (float)json["progress"].AsNumber;
			if (!json["repairTime"].IsNull)
			{
				repairJob.repairTime = (float)json["repairTime"].AsNumber;
			}
			return repairJob;
		}

		// Token: 0x040001DE RID: 478
		private float progress;

		// Token: 0x040001E2 RID: 482
		public SpaceShipData spaceshipData;

		// Token: 0x040001E3 RID: 483
		public string spaceshipGuid;
	}
}
