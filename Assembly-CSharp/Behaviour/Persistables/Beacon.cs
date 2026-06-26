using System;
using System.Collections;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Source.MissionSystem;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Persistables
{
	// Token: 0x020002F1 RID: 753
	public class Beacon : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001B86 RID: 7046 RVA: 0x000A7C5C File Offset: 0x000A5E5C
		private void Start()
		{
			base.StartCoroutine(this.FlickerRoutine());
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000A7C6C File Offset: 0x000A5E6C
		private void Update()
		{
			float intensity = Mathf.Lerp(this.minIntensity, this.maxIntensity, (Mathf.Sin(Time.time * this.humSpeed) + 1f) / 2f);
			this.light2D.intensity = intensity;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000A7CB4 File Offset: 0x000A5EB4
		private IEnumerator FlickerRoutine()
		{
			for (;;)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 6f));
				int num;
				for (int i = 0; i < UnityEngine.Random.Range(2, 4); i = num + 1)
				{
					this.light2D.intensity = 0.3f;
					yield return new WaitForSeconds(0.05f);
					this.light2D.intensity = this.maxIntensity;
					yield return new WaitForSeconds(0.07f);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000A7CC3 File Offset: 0x000A5EC3
		private void OnMouseDown()
		{
			MissionObjective.Trigger(MissionTrigger.InteractWithUmbralBeacon, null, null, false);
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000A7CCF File Offset: 0x000A5ECF
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@InteractWithBeacon", Array.Empty<object>()), 12, 8f);
		}

		// Token: 0x04001146 RID: 4422
		[SerializeField]
		private Light2D light2D;

		// Token: 0x04001147 RID: 4423
		[SerializeField]
		private float minIntensity = 0.8f;

		// Token: 0x04001148 RID: 4424
		[SerializeField]
		private float maxIntensity = 1.2f;

		// Token: 0x04001149 RID: 4425
		[SerializeField]
		private float humSpeed = 2f;
	}
}
