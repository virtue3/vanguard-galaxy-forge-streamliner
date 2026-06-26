using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200004B RID: 75
	public static class VectorCalculationsHelper
	{
		// Token: 0x0600031B RID: 795 RVA: 0x000190E8 File Offset: 0x000172E8
		public static Vector2 PredictFuturePosition(Vector2 startPosition, Vector2 targetPosition, Vector2 targetDirection, float targetSpeed, float startSpeed)
		{
			float d = (startPosition - targetPosition).magnitude / startSpeed;
			return targetPosition + targetDirection * targetSpeed * d;
		}
	}
}
