using System;
using Behaviour.Unit;
using Source.Data;

namespace Source.Drone
{
	// Token: 0x02000106 RID: 262
	public class TorpedoData : AbstractUnitData
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0004C013 File Offset: 0x0004A213
		public override string type
		{
			get
			{
				return "Torpedo";
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0004C01A File Offset: 0x0004A21A
		public TorpedoData(Torpedo torpedo, Torpedo definition)
		{
			this.unit = torpedo;
			base.Initialize(torpedo.identifier, definition);
		}
	}
}
