using System;
using Behaviour.Unit;
using Source.Data;

namespace Source.Drone
{
	// Token: 0x02000105 RID: 261
	public class DroneData : AbstractUnitData
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0004BFF0 File Offset: 0x0004A1F0
		public override string type
		{
			get
			{
				return "Behaviour.Unit.Drone";
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0004BFF7 File Offset: 0x0004A1F7
		public DroneData(Behaviour.Unit.Drone drone, Behaviour.Unit.Drone droneDefinition)
		{
			this.unit = drone;
			base.Initialize(drone.identifier, droneDefinition);
		}
	}
}
