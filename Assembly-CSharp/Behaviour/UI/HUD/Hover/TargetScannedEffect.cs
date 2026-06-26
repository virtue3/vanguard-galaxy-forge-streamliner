using System;
using System.Collections;
using Behaviour.Mining;
using UnityEngine;

namespace Behaviour.UI.HUD.Hover
{
	// Token: 0x0200028F RID: 655
	public class TargetScannedEffect : MonoBehaviour
	{
		// Token: 0x060017F7 RID: 6135 RVA: 0x0009629C File Offset: 0x0009449C
		private void Start()
		{
			this.asteroid = base.GetComponent<Asteroid>();
			this.outlineColorIdentifier = Shader.PropertyToID("_OutlineColor");
			this.itemColorIdentifier = Shader.PropertyToID("_ItemColor");
			this.scannerOverlayIdentifier = Shader.PropertyToID("_ScannerOverlay");
			this.scannerPowerIdentifier = Shader.PropertyToID("_ScannerPower");
			this.scannerFlickerSpeedIdentifier = Shader.PropertyToID("_ScannerFlickerSpeed");
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			if (this.spriteRenderer != null)
			{
				this.originalMaterial = this.spriteRenderer.material;
			}
			this.originalFinalScanningColor = this.finalScanningColor;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0009633C File Offset: 0x0009453C
		public void StartScanning(float totalTime, float delay = 0f)
		{
			if (this.runningRoutine != null)
			{
				return;
			}
			this.totalScanTime = totalTime;
			this.runningRoutine = base.StartCoroutine(this.RunningScan(delay));
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00096361 File Offset: 0x00094561
		public Color GetLowResourceColor()
		{
			return this.lowResourceColor;
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00096369 File Offset: 0x00094569
		public Color GetOriginalFinalScanningColor()
		{
			return this.originalFinalScanningColor;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00096371 File Offset: 0x00094571
		public void SetFinalScanningColor(Color color)
		{
			this.finalScanningColor = color;
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0009637A File Offset: 0x0009457A
		private IEnumerator RunningScan(float delay)
		{
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			if (this.asteroid.scanningProgress == 0)
			{
				this.ApplyOutlineMaterial(this.outlineMaterial, this.initialScanningColor, this.scannerFlickerSpeed * this.totalScanTime / 2f);
				yield return new WaitForSeconds(this.totalScanTime / 3f);
				this.asteroid.AddScanningProgress();
			}
			if (this.asteroid.scanningProgress == 1)
			{
				this.ApplyNewSettings(this.inconclusiveScanningColor, this.scannerFlickerSpeed * this.totalScanTime / 4f);
				yield return new WaitForSeconds(this.totalScanTime / 3f);
				this.asteroid.AddScanningProgress();
			}
			if (this.asteroid.scanningProgress == 2)
			{
				this.ApplyNewSettings(this.finalScanningColor, 0f);
				yield return new WaitForSeconds(this.totalScanTime / 3f * 2f);
				this.asteroid.AddScanningProgress();
			}
			if (this.asteroid.scanningProgress == 3 && !this.hovering)
			{
				this.ApplyOutlineMaterial(this.originalMaterial, Color.white, this.scannerFlickerSpeed);
			}
			this.runningRoutine = null;
			yield break;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00096390 File Offset: 0x00094590
		private void OnMouseEnter()
		{
			if (UIHelper.IsMouseOverUi)
			{
				return;
			}
			if (!this.asteroid)
			{
				return;
			}
			this.hovering = true;
			if (this.asteroid.scanningProgress < 3)
			{
				this.StartScanning(3f, 0f);
				return;
			}
			this.ApplyOutlineMaterial(this.outlineMaterial, this.finalScanningColor, 0f);
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x000963F0 File Offset: 0x000945F0
		private void OnMouseExit()
		{
			this.hovering = false;
			if (this.runningRoutine != null)
			{
				return;
			}
			this.ApplyOutlineMaterial(this.originalMaterial, Color.white, this.scannerFlickerSpeed);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0009641C File Offset: 0x0009461C
		private void ApplyOutlineMaterial(Material material, Color color, float flickerSpeed)
		{
			if (this.spriteRenderer != null)
			{
				this.spriteRenderer.material = material;
				this.spriteRenderer.material.SetColor(this.outlineColorIdentifier, color);
				if (material == this.outlineMaterial)
				{
					this.spriteRenderer.material.SetColor(this.itemColorIdentifier, color);
					this.spriteRenderer.material.SetTexture(this.scannerOverlayIdentifier, this.scannerOverlay);
					this.spriteRenderer.material.SetFloat(this.scannerPowerIdentifier, this.scannerPower);
					this.spriteRenderer.material.SetFloat(this.scannerFlickerSpeedIdentifier, flickerSpeed);
				}
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000964D1 File Offset: 0x000946D1
		private void ApplyNewSettings(Color color, float flickerSpeed)
		{
			this.spriteRenderer.material.SetColor(this.itemColorIdentifier, color);
			this.spriteRenderer.material.SetFloat(this.scannerFlickerSpeedIdentifier, flickerSpeed);
		}

		// Token: 0x04000EEA RID: 3818
		[SerializeField]
		private Material outlineMaterial;

		// Token: 0x04000EEB RID: 3819
		[SerializeField]
		private Color hoverOutlineColor = Color.gray;

		// Token: 0x04000EEC RID: 3820
		[SerializeField]
		private Color initialScanningColor = Color.gray;

		// Token: 0x04000EED RID: 3821
		[SerializeField]
		private Color inconclusiveScanningColor = Color.gray;

		// Token: 0x04000EEE RID: 3822
		[SerializeField]
		private Color finalScanningColor = Color.green;

		// Token: 0x04000EEF RID: 3823
		[SerializeField]
		private Color lowResourceColor = Color.green;

		// Token: 0x04000EF0 RID: 3824
		[SerializeField]
		private Texture2D scannerOverlay;

		// Token: 0x04000EF1 RID: 3825
		[SerializeField]
		private float scannerPower;

		// Token: 0x04000EF2 RID: 3826
		[SerializeField]
		private float scannerFlickerSpeed;

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		private Material originalMaterial;

		// Token: 0x04000EF4 RID: 3828
		private int outlineColorIdentifier;

		// Token: 0x04000EF5 RID: 3829
		private int itemColorIdentifier;

		// Token: 0x04000EF6 RID: 3830
		private int scannerOverlayIdentifier;

		// Token: 0x04000EF7 RID: 3831
		private int scannerPowerIdentifier;

		// Token: 0x04000EF8 RID: 3832
		private int scannerFlickerSpeedIdentifier;

		// Token: 0x04000EF9 RID: 3833
		private Color originalFinalScanningColor;

		// Token: 0x04000EFA RID: 3834
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000EFB RID: 3835
		private float totalScanTime = 3f;

		// Token: 0x04000EFC RID: 3836
		private Coroutine runningRoutine;

		// Token: 0x04000EFD RID: 3837
		private Asteroid asteroid;

		// Token: 0x04000EFE RID: 3838
		private bool hovering;
	}
}
