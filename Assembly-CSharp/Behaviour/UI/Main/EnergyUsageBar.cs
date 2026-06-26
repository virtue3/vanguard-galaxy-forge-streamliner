using System;
using System.Collections.Generic;
using Behaviour.Equipment.Module;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000269 RID: 617
	public class EnergyUsageBar : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001697 RID: 5783 RVA: 0x0008F50F File Offset: 0x0008D70F
		private void Awake()
		{
			this.GenerateEnergyStripes();
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x0008F517 File Offset: 0x0008D717
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.SetEnergyBar(this.spaceship);
				this.updateTimer = 0.1f;
			}
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0008F550 File Offset: 0x0008D750
		private void GenerateEnergyStripes()
		{
			RectTransform rectTransform = this.energyFill.rectTransform;
			foreach (float num in this.stripeThresholds)
			{
				float y = num * rectTransform.rect.height;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.stripePrefab, rectTransform);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0f, 0f);
				component.anchorMax = new Vector2(1f, 0f);
				component.pivot = new Vector2(0.5f, 0f);
				component.anchoredPosition = new Vector2(0f, y);
				component.sizeDelta = new Vector2(0f, 2f);
				gameObject.GetComponent<Image>().color = ColorHelper.GetEnergyColor(num);
			}
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x0008F624 File Offset: 0x0008D824
		public void SetEnergyBar(SpaceShip selectedShip)
		{
			if (!selectedShip)
			{
				return;
			}
			if (!selectedShip.reactorModule)
			{
				this.usedCapacity = 10000f;
				this.energyCapacity = 0f;
			}
			else
			{
				this.usedCapacity = selectedShip.reactorModule.usedCapacity;
				this.energyCapacity = selectedShip.reactorModule.energyCapacity;
				this.spaceship = selectedShip;
			}
			float num = this.usedCapacity / this.energyCapacity / 2f;
			this.energyFill.fillAmount = num;
			this.energyFill.color = ColorHelper.GetEnergyColor(num);
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0008F6BC File Offset: 0x0008D8BC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			float num = this.usedCapacity / this.energyCapacity;
			tooltip.AddTextLine(Translation.Translate("@ReactorOutput", Array.Empty<object>()), 14, 8f).Text.color = ColorHelper.purpleBadge;
			tooltip.AddTextLine(GameMath.FormatNumber(this.usedCapacity, 2).ToString().HighlightWithColor(ColorHelper.GetEnergyColor(num / 2f)) + " / " + GameMath.FormatNumber(this.energyCapacity, 2), 14, 8f).Text.color = ColorHelper.energyGrayGreen;
			tooltip.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			tooltip.AddTextLine(Translation.Translate("@EnergyUsage", Array.Empty<object>()) + " " + GameMath.FormatPercentage(num, FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.GetEnergyColor(num / 2f)), 12, 8f).Text.color = ColorHelper.detailsColor;
			foreach (KeyValuePair<float, float> keyValuePair in ReactorModule.energyThresholdModifiers)
			{
				if (num <= keyValuePair.Key)
				{
					float value = keyValuePair.Value;
					string text = Translation.Translate("@Bonus", Array.Empty<object>());
					Color color = ColorHelper.greenish;
					if (value < 0f)
					{
						text = Translation.Translate("@Penalty", Array.Empty<object>());
						color = ColorHelper.reddish;
					}
					if ((double)num <= 0.75 || num > 1f)
					{
						tooltip.AddTextLine(text, 12, 8f).Text.color = color;
						tooltip.AddTextLine(GameMath.FormatPercentage(value, FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.GetEnergyColor(num / 2f)) + " " + Translation.Translate("@EquipStatPower", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.detailsColor;
						break;
					}
					break;
				}
			}
			tooltip.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			tooltip.AddTextLine(Translation.Translate("@ReactorOutputDescription", Array.Empty<object>()), 12, 8f);
			tooltip.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			if (num > 1f)
			{
				tooltip.AddTextLine(Translation.Translate("@ReactorPenaltyDescription", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if ((double)num <= 0.75)
			{
				tooltip.AddTextLine(Translation.Translate("@ReactorBonusDescription", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@ReactorNormalDescription", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.energyGreenGray;
		}

		// Token: 0x04000DD0 RID: 3536
		[SerializeField]
		private TextMeshProUGUI energyCapacityText;

		// Token: 0x04000DD1 RID: 3537
		[SerializeField]
		private Image energyFill;

		// Token: 0x04000DD2 RID: 3538
		[SerializeField]
		private GameObject stripePrefab;

		// Token: 0x04000DD3 RID: 3539
		private SpaceShip spaceship;

		// Token: 0x04000DD4 RID: 3540
		private float usedCapacity;

		// Token: 0x04000DD5 RID: 3541
		private float energyCapacity;

		// Token: 0x04000DD6 RID: 3542
		private readonly float[] stripeThresholds = new float[]
		{
			0.25f,
			0.375f,
			0.5f,
			0.625f,
			0.75f,
			0.875f
		};

		// Token: 0x04000DD7 RID: 3543
		private float updateTimer;
	}
}
