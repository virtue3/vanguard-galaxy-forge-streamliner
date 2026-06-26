using System;
using Behaviour.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x0200027E RID: 638
	public class EnemyIndicator : MonoBehaviour
	{
		// Token: 0x0600174C RID: 5964 RVA: 0x00093378 File Offset: 0x00091578
		private void Update()
		{
			if (!this.unit)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Vector3 vector = GameplayManager.camera.WorldToViewportPoint(this.unit.transform.position);
			if (vector.x > -0.02f && vector.x <= 1.02f && vector.y > -0.02f && vector.y < 1.02f)
			{
				this.sprite.enabled = false;
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			Vector2 vector2 = EnemyIndicator.IntersectViewportEdge(vector);
			rectTransform.anchorMin = vector2;
			rectTransform.anchorMax = vector2;
			float num = Vector2.Distance(vector2, vector);
			Color color = this.sprite.color;
			color.a = Mathf.Clamp(num * 2f, 0.2f, 0.8f);
			this.sprite.color = color;
			this.sprite.enabled = true;
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0009346C File Offset: 0x0009166C
		public void SetUnit(SpaceShip unit)
		{
			this.unit = unit;
			RectTransform rectTransform = base.transform as RectTransform;
			int typeSize = unit.shipRoleType.GetTypeSize();
			if (typeSize < 3)
			{
				rectTransform.sizeDelta = new Vector2(80f, 80f);
				return;
			}
			if (typeSize < 5)
			{
				rectTransform.sizeDelta = new Vector2(120f, 120f);
				return;
			}
			rectTransform.sizeDelta = new Vector2(160f, 160f);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x000934E4 File Offset: 0x000916E4
		public static Vector2 IntersectViewportEdge(Vector2 targetViewport)
		{
			Vector2 vector = new Vector2(0.5f, 0.5f);
			Vector2 vector2 = targetViewport - vector;
			if (vector2.sqrMagnitude < 1E-08f)
			{
				return vector;
			}
			float num = (Mathf.Abs(vector2.x) > 1E-08f) ? (1f / vector2.x) : float.PositiveInfinity;
			float num2 = (Mathf.Abs(vector2.y) > 1E-08f) ? (1f / vector2.y) : float.PositiveInfinity;
			float a = (0f - vector.x) * num;
			float b = (1f - vector.x) * num;
			float a2 = Mathf.Min(a, b);
			float a3 = Mathf.Max(a, b);
			float a4 = (0f - vector.y) * num2;
			float b2 = (1f - vector.y) * num2;
			float b3 = Mathf.Min(a4, b2);
			float b4 = Mathf.Max(a4, b2);
			a2 = Mathf.Max(a2, b3);
			float d = Mathf.Min(a3, b4);
			Vector2 vector3 = vector + vector2 * d;
			vector3.x = Mathf.Clamp01(vector3.x);
			vector3.y = Mathf.Clamp01(vector3.y);
			return vector3;
		}

		// Token: 0x04000E66 RID: 3686
		[SerializeField]
		private Image sprite;

		// Token: 0x04000E67 RID: 3687
		private AbstractUnit unit;
	}
}
