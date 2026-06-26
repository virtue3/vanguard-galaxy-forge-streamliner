using System;
using Behaviour.UI.Spacestation;
using LightJson;
using Source.Galaxy.POI;
using Source.Item;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000317 RID: 791
	public class RefinedMaterialsItem : UsableItem
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x000B1502 File Offset: 0x000AF702
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x000B1505 File Offset: 0x000AF705
		// (set) Token: 0x06001DB5 RID: 7605 RVA: 0x000B150D File Offset: 0x000AF70D
		public RefinedMaterial material { get; private set; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x000B1516 File Offset: 0x000AF716
		// (set) Token: 0x06001DB7 RID: 7607 RVA: 0x000B151E File Offset: 0x000AF71E
		public float amount { get; private set; }

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000B1528 File Offset: 0x000AF728
		public override void DataFromJson(JsonObject data)
		{
			this.material = Enum.Parse<RefinedMaterial>(data["material"]);
			this.amount = (float)data["amount"].AsNumber;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000B156C File Offset: 0x000AF76C
		public override void DataToJson(JsonObject data)
		{
			data["material"] = this.material.ToString();
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x000B15B9 File Offset: 0x000AF7B9
		public override bool OnUse()
		{
			if (SpaceStationInterior.instance && SpaceStation.current.refinery != null)
			{
				SpaceStation.current.refinery.AddRefinedMaterial(this.material, this.amount);
				return true;
			}
			return false;
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000B15F1 File Offset: 0x000AF7F1
		public void SetMaterial(RefinedMaterial mat, float amount)
		{
			this.material = mat;
			this.amount = amount;
		}
	}
}
