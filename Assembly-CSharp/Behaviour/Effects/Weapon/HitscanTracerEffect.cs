using System;
using System.Collections;
using UnityEngine;

namespace Behaviour.Effects.Weapon
{
	// Token: 0x02000399 RID: 921
	public class HitscanTracerEffect : MonoBehaviour
	{
		// Token: 0x060022FD RID: 8957 RVA: 0x000C9194 File Offset: 0x000C7394
		public void ShowTracer(Color c, Vector3 start, Vector3 end)
		{
			base.transform.position = Vector3.zero;
			float num = SeededRandom.Global.RandomRange(0.6f, 1.8f);
			c = new Color(c.r * num, c.g * num, c.b * num, SeededRandom.Global.RandomRange(c.a * 0.6f, c.a * 1.8f));
			this.line.startColor = c;
			this.line.endColor = c;
			this.line.SetPositions(new Vector3[]
			{
				start,
				end
			});
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.DoTracer());
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000C9259 File Offset: 0x000C7459
		private IEnumerator DoTracer()
		{
			float time = 0f;
			float driftSpeed = SeededRandom.Global.RandomRange(-0.2f, 0.2f);
			while (time < 0.3f)
			{
				float deltaTime = Time.deltaTime;
				time += deltaTime;
				base.transform.position += new Vector3(0f, deltaTime * driftSpeed, 0f);
				this.line.startColor *= new Color(1f, 1f, 1f, 1f - time * 0.3f);
				this.line.startColor = this.line.startColor;
				yield return null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x040014E4 RID: 5348
		[SerializeField]
		private LineRenderer line;
	}
}
