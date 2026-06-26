using System;
using System.Collections;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001C5 RID: 453
	public class Ramp : MonoBehaviour
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00072B5A File Offset: 0x00070D5A
		// (set) Token: 0x0600112B RID: 4395 RVA: 0x00072B62 File Offset: 0x00070D62
		public Transform exitPosition { get; private set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00072B6B File Offset: 0x00070D6B
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x00072B73 File Offset: 0x00070D73
		public Transform approachPosition { get; private set; }

		// Token: 0x0600112E RID: 4398 RVA: 0x00072B7C File Offset: 0x00070D7C
		public void ToggleRamp(bool open)
		{
			if (this.isMoving)
			{
				return;
			}
			base.StartCoroutine(this.MoveRamp(open ? this.openPosition : this.closePosition, open ? this.openXScale : this.closeXScale, open ? this.openDuration : this.closeDuration));
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00072BD2 File Offset: 0x00070DD2
		private IEnumerator MoveRamp(Vector2 rampPosition, float targetXScale, float duration)
		{
			this.isMoving = true;
			Vector3 startPosition = base.transform.localPosition;
			Vector3 targetPosition = new Vector3(rampPosition.x, rampPosition.y, base.transform.localPosition.z);
			Vector3 startScale = base.transform.localScale;
			Vector3 targetScale = new Vector3(targetXScale, startScale.y, startScale.z);
			float t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime / duration;
				float t2 = Mathf.SmoothStep(0f, 1f, t);
				base.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t2);
				base.transform.localScale = Vector3.Lerp(startScale, targetScale, t2);
				yield return null;
			}
			base.transform.localPosition = targetPosition;
			base.transform.localScale = targetScale;
			this.isMoving = false;
			yield break;
		}

		// Token: 0x04000956 RID: 2390
		[SerializeField]
		private Vector2 openPosition;

		// Token: 0x04000957 RID: 2391
		[SerializeField]
		private Vector2 closePosition;

		// Token: 0x04000958 RID: 2392
		[SerializeField]
		private float openXScale;

		// Token: 0x04000959 RID: 2393
		[SerializeField]
		private float closeXScale;

		// Token: 0x0400095A RID: 2394
		[SerializeField]
		private float openDuration = 0.8f;

		// Token: 0x0400095B RID: 2395
		[SerializeField]
		private float closeDuration = 0.4f;

		// Token: 0x0400095C RID: 2396
		public bool isMoving;
	}
}
