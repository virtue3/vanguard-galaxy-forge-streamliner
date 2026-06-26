using System;
using UnityEngine;

namespace Behaviour.Equipment.Builder
{
	// Token: 0x02000374 RID: 884
	public class EquipmentBuilderField : MonoBehaviour
	{
		// Token: 0x04001432 RID: 5170
		public string field;

		// Token: 0x04001433 RID: 5171
		public float minValue;

		// Token: 0x04001434 RID: 5172
		public float maxValue;

		// Token: 0x04001435 RID: 5173
		[Tooltip("Hoeveel moet dit field meeschalen met level (0 = totaal geen scaling, 1.0 = volledige scaling)")]
		public float levelScaling;

		// Token: 0x04001436 RID: 5174
		[Tooltip("Hoeveel moet dit field meeschalen met rarity (0 = totaal geen scaling, 1.0 = volledige scaling)")]
		public float rarityScaling;
	}
}
