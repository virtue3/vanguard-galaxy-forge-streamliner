using System;
using Behaviour.UI;
using Behaviour.Unit;
using Behaviour.Unit.Parts;
using LightJson;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200031A RID: 794
	public class ShieldPylonItem : DefensiveTurretItem
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x000B19EA File Offset: 0x000AFBEA
		// (set) Token: 0x06001DD0 RID: 7632 RVA: 0x000B19F2 File Offset: 0x000AFBF2
		public float charge { get; private set; } = 1f;

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000B19FC File Offset: 0x000AFBFC
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			Color c;
			if (this.currentHealth < 0.25f)
			{
				c = ColorHelper.reddish;
			}
			else if (this.currentHealth < 0.75f)
			{
				c = ColorHelper.orange75;
			}
			else
			{
				c = ColorHelper.greenish;
			}
			tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretHealth", c, new object[]
			{
				GameMath.FormatPercentage(this.currentHealth, FormatPercentageMode.Default, 1)
			}), 12, 8f);
			if (this.charge < 0.25f)
			{
				c = ColorHelper.reddish;
			}
			else if (this.charge < 0.75f)
			{
				c = ColorHelper.orange75;
			}
			else
			{
				c = ColorHelper.greenish;
			}
			tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretCharge", c, new object[]
			{
				GameMath.FormatPercentage(this.charge, FormatPercentageMode.Default, 1)
			}), 12, 8f);
			tooltip.AddTextLine(Translation.Translate("@DefensiveTurretRecharge", Array.Empty<object>()), 12, 8f);
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x000B1AE4 File Offset: 0x000AFCE4
		public override void UpdateAmmoData(DefensiveTurret turret)
		{
			base.UpdateAmmoData(turret);
			ShieldPylonGenerator component = turret.GetComponent<ShieldPylonGenerator>();
			this.charge = component.charge;
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000B1B0B File Offset: 0x000AFD0B
		public void SetCharge(float charge)
		{
			this.charge = charge;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000B1B14 File Offset: 0x000AFD14
		public override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["charge"] = new double?((double)this.charge);
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000B1B3C File Offset: 0x000AFD3C
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			this.charge = (float)data["charge"].AsNumber;
		}
	}
}
