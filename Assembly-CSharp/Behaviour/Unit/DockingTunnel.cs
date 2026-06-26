using System;
using System.Collections;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001C3 RID: 451
	public class DockingTunnel : MonoBehaviour
	{
		// Token: 0x0600111C RID: 4380 RVA: 0x000728F8 File Offset: 0x00070AF8
		private void Start()
		{
			this.dockingCollider = base.GetComponent<BoxCollider2D>();
			if (this.dockingCollider == null)
			{
				return;
			}
			this.baseLength = this.dockingCollider.bounds.size.x / base.transform.lossyScale.x;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00072950 File Offset: 0x00070B50
		public void ToggleDockingTunnel(bool open, bool instant = false, Collider2D targetCollider = null)
		{
			if (this.isMoving)
			{
				return;
			}
			if (open && targetCollider != null)
			{
				float targetXScale = this.CalculateRequiredScale(targetCollider);
				base.StartCoroutine(this.MoveTunnel(this.openPosition, targetXScale, this.openDuration, instant));
				return;
			}
			base.StartCoroutine(this.MoveTunnel(this.closePosition, this.closeXScale, this.closeDuration, instant));
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x000729B8 File Offset: 0x00070BB8
		private float CalculateRequiredScale(Collider2D targetCollider)
		{
			Vector3 right = base.transform.right;
			float num = Vector3.Dot(targetCollider.ClosestPoint(base.transform.position) - base.transform.position, right);
			if (num <= 0f)
			{
				return this.openXScale;
			}
			float b = (num + this.dockingInset) / this.baseLength;
			return Mathf.Max(this.openXScale, b);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00072A33 File Offset: 0x00070C33
		private IEnumerator MoveTunnel(Vector2 tunnelPosition, float targetXScale, float duration, bool instant = false)
		{
			this.isMoving = true;
			Vector3 startPosition = base.transform.localPosition;
			Vector3 targetPosition = new Vector3(tunnelPosition.x, tunnelPosition.y, base.transform.localPosition.z);
			Vector3 startScale = base.transform.localScale;
			Vector3 targetScale = new Vector3(targetXScale, startScale.y, startScale.z);
			float t = 0f;
			if (!instant)
			{
				while (t < 1f)
				{
					t += Time.deltaTime / duration;
					float t2 = Mathf.SmoothStep(0f, 1f, t);
					base.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t2);
					base.transform.localScale = Vector3.Lerp(startScale, targetScale, t2);
					yield return null;
				}
			}
			base.transform.localPosition = targetPosition;
			base.transform.localScale = targetScale;
			this.isMoving = false;
			yield break;
		}

		// Token: 0x04000947 RID: 2375
		[SerializeField]
		private Vector2 openPosition;

		// Token: 0x04000948 RID: 2376
		[SerializeField]
		private Vector2 closePosition;

		// Token: 0x04000949 RID: 2377
		[SerializeField]
		private float openXScale = 1f;

		// Token: 0x0400094A RID: 2378
		[SerializeField]
		private float closeXScale = 1f;

		// Token: 0x0400094B RID: 2379
		[SerializeField]
		private float openDuration = 0.8f;

		// Token: 0x0400094C RID: 2380
		[SerializeField]
		private float closeDuration = 0.4f;

		// Token: 0x0400094D RID: 2381
		[SerializeField]
		private float dockingInset = 0.2f;

		// Token: 0x0400094E RID: 2382
		public bool isMoving;

		// Token: 0x0400094F RID: 2383
		private BoxCollider2D dockingCollider;

		// Token: 0x04000950 RID: 2384
		private float baseLength;
	}
}
