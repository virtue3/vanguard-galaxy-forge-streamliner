using System;
using System.Collections.Generic;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;

namespace Source.Simulation.World.System
{
	// Token: 0x02000078 RID: 120
	public class ConquestSystem : SystemStoryteller
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00024600 File Offset: 0x00022800
		public float totalReinforcements
		{
			get
			{
				float num = 1f;
				if (!this.headquarters && (this.system.faction == Faction.darkspacers || this.system.faction == Faction.fanatics))
				{
					num = 0.5f;
				}
				return num * (this.baseReinforcements + (this.headquarters ? 20f : 0f));
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00024662 File Offset: 0x00022862
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0002466A File Offset: 0x0002286A
		public bool headquarters { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00024673 File Offset: 0x00022873
		public Faction faction
		{
			get
			{
				return this.system.faction;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00024680 File Offset: 0x00022880
		public ConquestSystem(SystemMapData system) : base(system)
		{
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0002468C File Offset: 0x0002288C
		public override void Start()
		{
			this.station = this.system.GetPointOfInterest<ConquestStation>();
			this.connectedSystems = new List<ConquestSystem>();
			foreach (JumpGate jumpGate in this.system.GetPointsOfInterest<JumpGate>())
			{
				SystemMapData targetSystem = jumpGate.targetSystem;
				ConquestSystem conquestSystem = ((targetSystem != null) ? targetSystem.storyteller : null) as ConquestSystem;
				if (conquestSystem != null)
				{
					this.connectedSystems.Add(conquestSystem);
				}
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00024718 File Offset: 0x00022918
		public override void SetupSystem()
		{
			this.playerControlLevel = 0;
			this.controlLevel = 1;
			this.baseReinforcements = SeededRandom.Global.RandomRange(0f, 4f);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00024742 File Offset: 0x00022942
		public void SetHeadquarters(bool hq)
		{
			this.headquarters = hq;
			this.system.GetPointOfInterest<ConquestStation>().SwapConquestCommander(hq);
			this.system.GetPointOfInterest<ConquestStation>().SwapConquestShop(hq);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00024770 File Offset: 0x00022970
		public override void DataToJson(JsonObject data)
		{
			data["playerControlLevel"] = new double?((double)this.playerControlLevel);
			data["controlLevel"] = new double?((double)this.controlLevel);
			data["combatStrength"] = new double?((double)this.combatStrength);
			data["reinforcements"] = new double?((double)this.baseReinforcements);
			data["umbralControlLevel"] = new double?((double)this.umbralControlLevel);
			if (this.headquarters)
			{
				data["headquarters"] = new bool?(true);
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00024828 File Offset: 0x00022A28
		public override void DataFromJson(JsonObject data)
		{
			this.playerControlLevel = data["playerControlLevel"];
			this.controlLevel = data["controlLevel"];
			this.combatStrength = (float)data["combatStrength"].AsNumber;
			this.baseReinforcements = (float)data["reinforcements"].AsNumber;
			this.headquarters = data["headquarters"];
			this.umbralControlLevel = (float)data["umbralControlLevel"].AsNumber;
		}

		// Token: 0x04000263 RID: 611
		public const float HeadquartersReinforcements = 20f;

		// Token: 0x04000264 RID: 612
		public int playerControlLevel;

		// Token: 0x04000265 RID: 613
		public int controlLevel;

		// Token: 0x04000266 RID: 614
		public float combatStrength;

		// Token: 0x04000267 RID: 615
		public float baseReinforcements;

		// Token: 0x04000268 RID: 616
		public float umbralControlLevel;

		// Token: 0x0400026A RID: 618
		public ConquestStation station;

		// Token: 0x0400026B RID: 619
		public List<ConquestSystem> connectedSystems;
	}
}
