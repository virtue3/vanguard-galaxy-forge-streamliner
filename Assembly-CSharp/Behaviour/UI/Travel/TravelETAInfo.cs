using System;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Travel
{
	// Token: 0x020001FE RID: 510
	public class TravelETAInfo : MonoBehaviour
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x0007DA81 File Offset: 0x0007BC81
		// (set) Token: 0x06001321 RID: 4897 RVA: 0x0007DA89 File Offset: 0x0007BC89
		public float currentETA { get; private set; }

		// Token: 0x06001322 RID: 4898 RVA: 0x0007DA94 File Offset: 0x0007BC94
		public void ShowTransition()
		{
			this.etaText.text = string.Concat(new string[]
			{
				Translation.Translate("@TravelETA", Array.Empty<object>()),
				": <color=#",
				ColorUtility.ToHtmlStringRGB(ColorHelper.etaColorLow),
				">",
				Translation.Translate("@TravelJumping", Array.Empty<object>()),
				"</color>"
			});
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0007DB00 File Offset: 0x0007BD00
		public void Init(float initialDistance, float unitsPerSecond)
		{
			this.baseWarpAcceleration = GameplayManager.Instance.spaceShip.baseWarpAcceleration;
			this.distance = initialDistance * 100f;
			this.etaText.text = string.Concat(new string[]
			{
				Translation.Translate("@TravelETA", Array.Empty<object>()),
				": <color=#",
				ColorUtility.ToHtmlStringRGB(ColorHelper.etaColorLow),
				">",
				Translation.Translate("@TravelInitiatingWarp", Array.Empty<object>()),
				"</color>"
			});
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0007DB90 File Offset: 0x0007BD90
		public void UpdateETA(float remainingDistance, float maxWarpSpeed, float unitsPerSecond, float maxWarpAcceleration)
		{
			this.maxWarpSpeed = maxWarpSpeed;
			this.baseWarpAcceleration = maxWarpAcceleration;
			this.currentETA = this.CalculateETA(remainingDistance, maxWarpSpeed, this.baseWarpAcceleration);
			float num = remainingDistance / this.distance;
			Color color = (num > 0.6f) ? ColorHelper.etaColorHigh : ((num > 0.15f) ? ColorHelper.etaColorMedium : ColorHelper.etaColorLow);
			this.etaText.text = string.Concat(new string[]
			{
				Translation.Translate("@TravelETA", Array.Empty<object>()),
				": <color=#",
				ColorUtility.ToHtmlStringRGB(color),
				">",
				GameMath.FormatTime(Mathf.RoundToInt(this.currentETA), true),
				"</color>"
			});
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0007DC4C File Offset: 0x0007BE4C
		public float CalculateETA(float remainingDistance, float maxWarpSpeed, float warpAcceleration)
		{
			float num = remainingDistance / 100f;
			if (Singleton<TravelManager>.Instance.fastLaneTravelActive)
			{
				return num / maxWarpSpeed;
			}
			float num2 = maxWarpSpeed * maxWarpSpeed / (2f * warpAcceleration);
			float result;
			if (num < 2f * num2)
			{
				result = 2f * Mathf.Sqrt(num / warpAcceleration);
			}
			else
			{
				float num3 = maxWarpSpeed / warpAcceleration;
				float num4 = (num - 2f * num2) / maxWarpSpeed;
				result = 2f * num3 + num4;
			}
			return result;
		}

		// Token: 0x04000AC8 RID: 2760
		[SerializeField]
		private TextMeshProUGUI etaText;

		// Token: 0x04000ACA RID: 2762
		private float baseWarpAcceleration;

		// Token: 0x04000ACB RID: 2763
		private float maxWarpSpeed;

		// Token: 0x04000ACC RID: 2764
		private float distance;
	}
}
