using System;
using Source.Item;
using UnityEngine;

namespace Behaviour.Equipment.Builder
{
	// Token: 0x02000375 RID: 885
	public class EquipmentBuilderStat : MonoBehaviour
	{
		// Token: 0x04001437 RID: 5175
		public EquipStat stat;

		// Token: 0x04001438 RID: 5176
		public bool isMainStat;

		// Token: 0x04001439 RID: 5177
		public bool isMultiplier;

		// Token: 0x0400143A RID: 5178
		public float minValue;

		// Token: 0x0400143B RID: 5179
		public float maxValue;

		// Token: 0x0400143C RID: 5180
		public float spawnWeight = 1f;

		// Token: 0x0400143D RID: 5181
		[Tooltip("Hoeveel moet dit field meeschalen met level (0 = totaal geen scaling, 1.0 = volledige scaling)")]
		public float levelScaling;

		// Token: 0x0400143E RID: 5182
		[Tooltip("Hoeveel moet dit field meeschalen met rarity (0 = totaal geen scaling, 1.0 = volledige scaling)")]
		public float rarityScaling;

		// Token: 0x0400143F RID: 5183
		[Tooltip("From what level will this stat be available")]
		public float minSpawnLevel;
	}
}
