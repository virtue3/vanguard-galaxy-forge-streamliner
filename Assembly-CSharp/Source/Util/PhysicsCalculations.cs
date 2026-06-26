using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200003A RID: 58
	public static class PhysicsCalculations
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00016734 File Offset: 0x00014934
		public static bool CalculateRotationalThrustFactor(float angle, ref float slowDownFactor, Rigidbody2D rigidbody, float rotationalThrust)
		{
			bool result = false;
			float f = angle * 0.0174532924f;
			float num = rigidbody.angularVelocity * 0.0174532924f;
			float num2 = rotationalThrust / rigidbody.inertia;
			float f2 = Mathf.Abs(num / num2) + 0.0001f * rotationalThrust / rigidbody.inertia;
			float num3 = num2 * Mathf.Pow(f2, 2f) / 2f;
			if (num3 >= Mathf.Abs(f))
			{
				result = true;
			}
			if (Mathf.Abs(f) < num3)
			{
				slowDownFactor = Mathf.Abs(f) / num3;
			}
			return result;
		}
	}
}
