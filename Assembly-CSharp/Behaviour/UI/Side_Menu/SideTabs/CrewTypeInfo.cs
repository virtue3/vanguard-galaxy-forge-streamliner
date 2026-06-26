using System;
using Behaviour.Crew;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002AF RID: 687
	public class CrewTypeInfo : MonoBehaviour
	{
		// Token: 0x0600197B RID: 6523 RVA: 0x0009EAE4 File Offset: 0x0009CCE4
		public void Init(string crewType, int amount, int amountOnShips)
		{
			this.crew = Behaviour.Crew.Crew.Get(crewType);
			this.totalAmount = amount;
			this.amountOnShips = amountOnShips;
			this.unassignedAmount = amount - amountOnShips;
			this.Setup();
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0009EB10 File Offset: 0x0009CD10
		private void Setup()
		{
			this.typeName.text = Translation.Translate("@" + this.crew.identifier, Array.Empty<object>());
			this.amountText.text = "Total Amount:" + this.totalAmount.ToString();
			this.amountOnShipsText.text = "Active Behaviour.Crew.Crew:" + this.amountOnShips.ToString();
			this.description.text = Translation.Translate("@" + this.crew.identifier + "Desc", Array.Empty<object>());
			this.wagePerCrew.text = "Wage:" + this.crew.wage.ToString();
			this.totalWage.text = "Total wage:" + (this.crew.wage * this.totalAmount).ToString();
			this.bonus.text = "Hier komt bonus text";
			this.boardingPower.text = this.crew.boardingPower.ToString();
			this.resistance.text = this.crew.resistance.ToString();
		}

		// Token: 0x04000FE4 RID: 4068
		private Behaviour.Crew.Crew crew;

		// Token: 0x04000FE5 RID: 4069
		private int totalAmount;

		// Token: 0x04000FE6 RID: 4070
		private int amountOnShips;

		// Token: 0x04000FE7 RID: 4071
		private int unassignedAmount;

		// Token: 0x04000FE8 RID: 4072
		[SerializeField]
		private TMP_Text typeName;

		// Token: 0x04000FE9 RID: 4073
		[SerializeField]
		private TMP_Text amountText;

		// Token: 0x04000FEA RID: 4074
		[SerializeField]
		private TMP_Text amountOnShipsText;

		// Token: 0x04000FEB RID: 4075
		[SerializeField]
		private TMP_Text description;

		// Token: 0x04000FEC RID: 4076
		[SerializeField]
		private TMP_Text wagePerCrew;

		// Token: 0x04000FED RID: 4077
		[SerializeField]
		private TMP_Text totalWage;

		// Token: 0x04000FEE RID: 4078
		[SerializeField]
		private TMP_Text bonus;

		// Token: 0x04000FEF RID: 4079
		[SerializeField]
		private TMP_Text boardingPower;

		// Token: 0x04000FF0 RID: 4080
		[SerializeField]
		private TMP_Text resistance;
	}
}
