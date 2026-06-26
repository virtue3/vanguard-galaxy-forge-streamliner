using System;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Persistables
{
	// Token: 0x020002F8 RID: 760
	public class PoiBeacon : MonoBehaviour, ITooltipTitleSource, ITooltipCustomSource
	{
		// Token: 0x06001BBC RID: 7100 RVA: 0x000A86B8 File Offset: 0x000A68B8
		private void Update()
		{
			float intensity = Mathf.Lerp(this.minIntensity, this.maxIntensity, (Mathf.Sin(Time.time * this.humSpeed) + 1f) / 2f);
			this.light2D.intensity = intensity;
			if (this.currentTooltip)
			{
				TMP_Text t = this.currentTooltip;
				string s = "@PoiBeaconTimer";
				object[] array = new object[1];
				int num = 0;
				MapPointOfInterest current = MapPointOfInterest.current;
				array[num] = GameMath.FormatTime(Mathf.CeilToInt((current != null) ? current.timeLeft : 0f), true);
				t.TL(s, array);
			}
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000A8746 File Offset: 0x000A6946
		private void OnMouseDown()
		{
			if (MapPointOfInterest.current != null && MapPointOfInterest.current.timeLeft > -1f)
			{
				MapPointOfInterest.current.timeLeft = 1800f;
			}
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000A8770 File Offset: 0x000A6970
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			this.currentTooltip = tooltip.AddTextLine(Translation.Translate("@PoiBeaconTimer", new object[]
			{
				"00:00:00"
			}), 12, 8f).Text;
			tooltip.AddTextLine("@PoiBeaconInteract", 12, 8f);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000A87C0 File Offset: 0x000A69C0
		public string GetTooltipTitle()
		{
			return "@PoiBeaconName";
		}

		// Token: 0x0400115E RID: 4446
		[SerializeField]
		private Light2D light2D;

		// Token: 0x0400115F RID: 4447
		[SerializeField]
		private float minIntensity = 0.8f;

		// Token: 0x04001160 RID: 4448
		[SerializeField]
		private float maxIntensity = 1.2f;

		// Token: 0x04001161 RID: 4449
		[SerializeField]
		private float humSpeed = 2f;

		// Token: 0x04001162 RID: 4450
		private TMP_Text currentTooltip;
	}
}
