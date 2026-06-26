using System;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000365 RID: 869
	public class ReactorModuleLight : MonoBehaviour
	{
		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x000C267F File Offset: 0x000C087F
		// (set) Token: 0x06002149 RID: 8521 RVA: 0x000C2687 File Offset: 0x000C0887
		[Header("References")]
		public Light2D reactorLight { get; private set; }

		// Token: 0x0600214A RID: 8522 RVA: 0x000C2690 File Offset: 0x000C0890
		private void Awake()
		{
			this.randomOffset = UnityEngine.Random.Range(0f, 999f);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000C26A7 File Offset: 0x000C08A7
		private void Update()
		{
			if (!this.reactorLight.gameObject.activeSelf)
			{
				return;
			}
			this.ApplyLightState();
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000C26C4 File Offset: 0x000C08C4
		public void Init(ReactorModule module, bool deactivate = false)
		{
			if (deactivate)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.reactorLight.gameObject.SetActive(true);
			if (module == null)
			{
				this.energyPercentage = 2f;
			}
			else
			{
				this.energyPercentage = module.usedCapacity / module.energyCapacity;
			}
			this.ApplyLightState();
			this.UpdateColorAndFlickerSettings();
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000C2728 File Offset: 0x000C0928
		private void ApplyLightState()
		{
			float num = this.energyPercentage;
			ReactorModuleLight.LightState lightState;
			if (num < 0.75f)
			{
				lightState = ReactorModuleLight.LightState.Low;
			}
			else if (num < 1f)
			{
				lightState = ReactorModuleLight.LightState.Mid;
			}
			else if (num <= 1.25f)
			{
				lightState = ReactorModuleLight.LightState.OverLow;
			}
			else if (num <= 1.5f)
			{
				lightState = ReactorModuleLight.LightState.OverMed;
			}
			else
			{
				lightState = ReactorModuleLight.LightState.OverHigh;
			}
			if (lightState != this.currentState)
			{
				this.currentState = lightState;
				this.UpdateColorAndFlickerSettings();
			}
			if (!this.reactorLight.gameObject.activeSelf)
			{
				return;
			}
			float num2 = Mathf.PerlinNoise(Time.time * this.activeSpeed + this.randomOffset, 0f) - 0.5f;
			this.reactorLight.intensity = this.baseIntensity + num2 * this.activeAmp;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000C27D8 File Offset: 0x000C09D8
		private void UpdateColorAndFlickerSettings()
		{
			switch (this.currentState)
			{
			case ReactorModuleLight.LightState.Low:
				this.reactorLight.gameObject.SetActive(false);
				this.reactorLight.color = ColorHelper.energyGreen;
				this.activeSpeed = 0f;
				this.activeAmp = 0f;
				this.activeRadius = this.baseRadius;
				this.reactorLight.intensity = this.baseIntensity;
				break;
			case ReactorModuleLight.LightState.Mid:
				this.reactorLight.gameObject.SetActive(false);
				this.reactorLight.color = ColorHelper.energyGreenGray;
				this.activeSpeed = 0f;
				this.activeAmp = 0f;
				this.activeRadius = this.baseRadius;
				this.reactorLight.intensity = this.baseIntensity;
				break;
			case ReactorModuleLight.LightState.OverLow:
				this.reactorLight.gameObject.SetActive(true);
				this.reactorLight.color = Color.darkRed;
				this.activeSpeed = this.flickerSpeedLow;
				this.activeAmp = this.flickerAmpLow;
				this.activeRadius = this.radiusOverLow;
				break;
			case ReactorModuleLight.LightState.OverMed:
				this.reactorLight.gameObject.SetActive(true);
				this.reactorLight.color = Color.darkRed;
				this.activeSpeed = this.flickerSpeedMed;
				this.activeAmp = this.flickerAmpMed;
				this.activeRadius = this.radiusOverMed;
				break;
			case ReactorModuleLight.LightState.OverHigh:
				this.reactorLight.gameObject.SetActive(true);
				this.reactorLight.color = Color.red;
				this.activeSpeed = this.flickerSpeedHigh;
				this.activeAmp = this.flickerAmpHigh;
				this.activeRadius = this.radiusOverHigh;
				break;
			}
			this.reactorLight.pointLightOuterRadius = this.activeRadius;
		}

		// Token: 0x040013B6 RID: 5046
		[Header("Intensity")]
		[SerializeField]
		private float baseIntensity = 1f;

		// Token: 0x040013B7 RID: 5047
		[Header("Flicker Settings")]
		[SerializeField]
		private float flickerSpeedLow = 2f;

		// Token: 0x040013B8 RID: 5048
		[SerializeField]
		private float flickerSpeedMed = 5f;

		// Token: 0x040013B9 RID: 5049
		[SerializeField]
		private float flickerSpeedHigh = 12f;

		// Token: 0x040013BA RID: 5050
		[SerializeField]
		private float flickerAmpLow = 0.05f;

		// Token: 0x040013BB RID: 5051
		[SerializeField]
		private float flickerAmpMed = 0.12f;

		// Token: 0x040013BC RID: 5052
		[SerializeField]
		private float flickerAmpHigh = 0.25f;

		// Token: 0x040013BD RID: 5053
		[Header("Radius")]
		[SerializeField]
		private float baseRadius = 1f;

		// Token: 0x040013BE RID: 5054
		[SerializeField]
		private float radiusOverLow = 1.1f;

		// Token: 0x040013BF RID: 5055
		[SerializeField]
		private float radiusOverMed = 1.25f;

		// Token: 0x040013C0 RID: 5056
		[SerializeField]
		private float radiusOverHigh = 1.45f;

		// Token: 0x040013C1 RID: 5057
		private float activeRadius;

		// Token: 0x040013C2 RID: 5058
		private float randomOffset;

		// Token: 0x040013C3 RID: 5059
		[SerializeField]
		private float energyPercentage;

		// Token: 0x040013C4 RID: 5060
		private ReactorModuleLight.LightState currentState;

		// Token: 0x040013C5 RID: 5061
		private float activeAmp;

		// Token: 0x040013C6 RID: 5062
		private float activeSpeed;

		// Token: 0x020005CF RID: 1487
		private enum LightState
		{
			// Token: 0x04001DF9 RID: 7673
			Low,
			// Token: 0x04001DFA RID: 7674
			Mid,
			// Token: 0x04001DFB RID: 7675
			OverLow,
			// Token: 0x04001DFC RID: 7676
			OverMed,
			// Token: 0x04001DFD RID: 7677
			OverHigh
		}
	}
}
