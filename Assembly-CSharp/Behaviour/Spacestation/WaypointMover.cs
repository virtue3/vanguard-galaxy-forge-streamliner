using System;
using System.Collections;
using UnityEngine;

namespace Behaviour.Spacestation
{
	// Token: 0x020002DE RID: 734
	public class WaypointMover : MonoBehaviour
	{
		// Token: 0x06001ABF RID: 6847 RVA: 0x000A52BF File Offset: 0x000A34BF
		public IEnumerator FollowPath(WaypointPath path)
		{
			float z = base.transform.position.z;
			int num;
			for (int i = 0; i < path.Count; i = num + 1)
			{
				Vector2 target = path.GetPoint(i);
				while (Vector2.Distance(base.transform.position, target) > this.arriveThreshold)
				{
					Vector2 vector = Vector2.MoveTowards(base.transform.position, target, this.moveSpeed * Time.deltaTime);
					base.transform.position = new Vector3(vector.x, vector.y, z);
					Vector2 vector2 = target - (Vector2)base.transform.position;
					if (vector2.sqrMagnitude > 0.0001f)
					{
						float z2 = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f - 90f;
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(0f, 0f, z2), this.rotationSpeed * Time.deltaTime);
					}
					yield return null;
				}
				Vector3 position = base.transform.position;
				position.x = target.x;
				position.y = target.y;
				base.transform.position = position;
				target = default(Vector2);
				num = i;
			}
			yield break;
		}

		// Token: 0x040010E5 RID: 4325
		[SerializeField]
		private float moveSpeed = 1.5f;

		// Token: 0x040010E6 RID: 4326
		[SerializeField]
		private float rotationSpeed = 8f;

		// Token: 0x040010E7 RID: 4327
		[SerializeField]
		private float arriveThreshold = 0.02f;
	}
}
