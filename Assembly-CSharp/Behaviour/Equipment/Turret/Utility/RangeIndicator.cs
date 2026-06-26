using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Utility
{
	// Token: 0x0200034B RID: 843
	[RequireComponent(typeof(LineRenderer), typeof(MeshFilter), typeof(MeshRenderer))]
	public class RangeIndicator : MonoBehaviour
	{
		// Token: 0x06002031 RID: 8241 RVA: 0x000BD6F8 File Offset: 0x000BB8F8
		private void Awake()
		{
			this.line.material = new Material(Shader.Find("Sprites/Default"));
			this.line.loop = true;
			this.line.positionCount = this.segments;
			this.line.startWidth = 0.1f;
			this.line.endWidth = 0.1f;
			this.line.useWorldSpace = false;
			this.fillMesh = new Mesh();
			this.meshFilter.mesh = this.fillMesh;
			this.meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
			this.meshRenderer.material.renderQueue = 4000;
			this.UpdateVisualAlpha(0f);
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x000BD7C0 File Offset: 0x000BB9C0
		private void Update()
		{
			if (!Mathf.Approximately(this.currentAlpha, this.targetAlpha))
			{
				this.currentAlpha = Mathf.MoveTowards(this.currentAlpha, this.targetAlpha, this.fadeSpeed * Time.deltaTime);
				this.UpdateVisualAlpha(this.currentAlpha);
			}
			this.rotationSpeed = Mathf.Lerp(this.rotationMin, this.rotationMax, (Mathf.Sin(Time.time * this.rotationOscillationSpeed) + 1f) / 2f);
			base.transform.Rotate(0f, 0f, this.rotationSpeed * Time.deltaTime);
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000BD864 File Offset: 0x000BBA64
		public void SetColorIndicator(TurretType type)
		{
			this.turretType = type;
			ValueTuple<Color, Color> colorsForTurret = this.GetColorsForTurret(type);
			Color item = colorsForTurret.Item1;
			Color item2 = colorsForTurret.Item2;
			this.ringColor = item;
			this.fillColor = item2;
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000BD89C File Offset: 0x000BBA9C
		public void CheckForColor(bool active)
		{
			if (active)
			{
				this.SetColorIndicator(this.turretType);
			}
			else
			{
				this.ringColor = new Color(0.25f, 0.25f, 0.25f, 0.02f);
				this.fillColor = new Color(0.25f, 0.25f, 0.25f, 0.01f);
			}
			this.UpdateVisualAlpha(this.currentAlpha);
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000BD904 File Offset: 0x000BBB04
		public ValueTuple<Color, Color> GetColorsForTurret(TurretType type)
		{
			ValueTuple<Color, Color> result;
			switch (type)
			{
			case TurretType.Combat:
				result = new ValueTuple<Color, Color>(new Color(0.6f, 0.18f, 0.18f, 0.02f), new Color(0.6f, 0.18f, 0.18f, 0.01f));
				break;
			case TurretType.Mining:
				result = new ValueTuple<Color, Color>(new Color(0.12f, 0.3f, 0.4f, 0.02f), new Color(0.12f, 0.3f, 0.4f, 0.01f));
				break;
			case TurretType.Salvaging:
				result = new ValueTuple<Color, Color>(new Color(0.4f, 0.2f, 0.12f, 0.02f), new Color(0.4f, 0.2f, 0.12f, 0.01f));
				break;
			default:
				result = new ValueTuple<Color, Color>(new Color(0.3f, 0.5f, 1f, 0.02f), new Color(1f, 1f, 1f, 0.01f));
				break;
			}
			return result;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000BDA14 File Offset: 0x000BBC14
		public void Show(float range, bool active = true)
		{
			this.SetRange(range);
			if (this.active != active)
			{
				this.CheckForColor(active);
			}
			this.active = active;
			if (this.showCoroutine != null)
			{
				base.StopCoroutine(this.showCoroutine);
			}
			this.showCoroutine = base.StartCoroutine(this.ShowDelayed());
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000BDA65 File Offset: 0x000BBC65
		private IEnumerator ShowDelayed()
		{
			yield return new WaitForSeconds(this.showDelay);
			this.line.enabled = true;
			this.meshRenderer.enabled = true;
			this.targetAlpha = 1f;
			this.showCoroutine = null;
			yield break;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000BDA74 File Offset: 0x000BBC74
		public void Hide()
		{
			if (this.showCoroutine != null)
			{
				base.StopCoroutine(this.showCoroutine);
				this.showCoroutine = null;
			}
			this.targetAlpha = 0f;
			if (this.hideCoroutine == null)
			{
				base.StartCoroutine(this.HideAfterFade());
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000BDAB1 File Offset: 0x000BBCB1
		public void SetRange(float newRange)
		{
			this.range = newRange;
			this.GenerateCircleOutline();
			this.GenerateFilledCircle();
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000BDAC6 File Offset: 0x000BBCC6
		public void SetColor(Color ring, Color fill)
		{
			this.ringColor = ring;
			this.fillColor = fill;
			this.UpdateVisualAlpha(this.currentAlpha);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000BDAE4 File Offset: 0x000BBCE4
		private void GenerateCircleOutline()
		{
			float num = 360f / (float)this.segments;
			float num2 = 0f;
			for (int i = 0; i < this.segments; i++)
			{
				float f = num2 * 0.0174532924f;
				float x = Mathf.Cos(f) * this.range;
				float y = Mathf.Sin(f) * this.range;
				this.line.SetPosition(i, new Vector3(x, y, 0f));
				num2 += num;
			}
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000BDB58 File Offset: 0x000BBD58
		private void GenerateFilledCircle()
		{
			this.fillMesh.Clear();
			Vector3[] array = new Vector3[this.segments + 1];
			int[] array2 = new int[this.segments * 3];
			array[0] = Vector3.zero;
			for (int i = 0; i < this.segments; i++)
			{
				float f = (float)i * 3.14159274f * 2f / (float)this.segments;
				array[i + 1] = new Vector3(Mathf.Cos(f) * this.range, Mathf.Sin(f) * this.range, 0f);
			}
			for (int j = 0; j < this.segments; j++)
			{
				int num = j * 3;
				array2[num] = 0;
				array2[num + 1] = j + 1;
				array2[num + 2] = ((j == this.segments - 1) ? 1 : (j + 2));
			}
			this.fillMesh.vertices = array;
			this.fillMesh.triangles = array2;
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000BDC48 File Offset: 0x000BBE48
		private void UpdateVisualAlpha(float alpha)
		{
			Color color = this.ringColor;
			color.a = alpha;
			this.line.startColor = color;
			this.line.endColor = color;
			Color color2 = this.fillColor;
			color2.a = this.fillColor.a * alpha;
			this.meshRenderer.material.color = color2;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000BDCA8 File Offset: 0x000BBEA8
		private IEnumerator HideAfterFade()
		{
			while (this.currentAlpha > 0.01f)
			{
				yield return null;
			}
			this.hideCoroutine = null;
			this.line.enabled = false;
			this.meshRenderer.enabled = false;
			yield break;
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000BDCB7 File Offset: 0x000BBEB7
		private void OnDisable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x0400132F RID: 4911
		public float range = 5f;

		// Token: 0x04001330 RID: 4912
		public int segments = 64;

		// Token: 0x04001331 RID: 4913
		[Header("Visual")]
		public Color ringColor = Color.cyan;

		// Token: 0x04001332 RID: 4914
		public Color fillColor = new Color(0f, 1f, 1f, 0.15f);

		// Token: 0x04001333 RID: 4915
		public float fadeSpeed = 4f;

		// Token: 0x04001334 RID: 4916
		public float rotationSpeed = 40f;

		// Token: 0x04001335 RID: 4917
		public float rotationMin = 30f;

		// Token: 0x04001336 RID: 4918
		public float rotationMax = 70f;

		// Token: 0x04001337 RID: 4919
		public float rotationOscillationSpeed = 1f;

		// Token: 0x04001338 RID: 4920
		[SerializeField]
		private LineRenderer line;

		// Token: 0x04001339 RID: 4921
		[SerializeField]
		private MeshFilter meshFilter;

		// Token: 0x0400133A RID: 4922
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x0400133B RID: 4923
		private float currentAlpha;

		// Token: 0x0400133C RID: 4924
		private float targetAlpha;

		// Token: 0x0400133D RID: 4925
		private Mesh fillMesh;

		// Token: 0x0400133E RID: 4926
		public float showDelay = 1f;

		// Token: 0x0400133F RID: 4927
		private Coroutine showCoroutine;

		// Token: 0x04001340 RID: 4928
		private Coroutine hideCoroutine;

		// Token: 0x04001341 RID: 4929
		private bool active;

		// Token: 0x04001342 RID: 4930
		private TurretType turretType;
	}
}
