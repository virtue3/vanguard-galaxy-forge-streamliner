using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Mining;
using Source.Spacestation;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation
{
	// Token: 0x02000214 RID: 532
	public class SpaceStationActiveJob : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060013A0 RID: 5024 RVA: 0x0007F3FC File Offset: 0x0007D5FC
		private void Awake()
		{
			this.tooltip = base.GetComponent<TooltipSource>();
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0007F40C File Offset: 0x0007D60C
		private void Update()
		{
			float jobProgress = this.contained.jobProgress;
			this.progress.localScale = new Vector3(jobProgress, 1f, 1f);
			if (jobProgress < this.currentProgress)
			{
				this.UpdateLabel();
			}
			this.currentProgress = jobProgress;
			if (this.contained.remainingAmount <= 0)
			{
				SpaceStationInterior.instance.UpdateJobs();
			}
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				UITooltip.Refresh(this.tooltip);
				this.updateTimer = 0.1f;
			}
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0007F4A4 File Offset: 0x0007D6A4
		private void UpdateLabel()
		{
			this.label.text = Translation.Translate(this.contained.jobName, Array.Empty<object>()) + " x" + this.contained.remainingAmount.ToString();
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0007F4F0 File Offset: 0x0007D6F0
		public void SetJob(ISpaceStationJob job)
		{
			this.contained = job;
			this.icon.sprite = job.jobIcon;
			this.cancel.gameObject.SetActive(job.cancelAvailable);
			RepairJob repairJob = this.contained as RepairJob;
			if (repairJob != null && !repairJob.autoRepair)
			{
				this.icon.color = ColorHelper.greenish;
			}
			if (this.contained is RefineryJob)
			{
				this.backgroundImage.color = ColorHelper.refineryJobBackground.WithAlpha(0.9f);
			}
			else if (this.contained is ForgeJob)
			{
				this.backgroundImage.color = ColorHelper.forgeJobBackground.WithAlpha(0.9f);
			}
			this.Update();
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0007F5A8 File Offset: 0x0007D7A8
		public void CancelButton()
		{
			if (this.contained.cancelAvailable)
			{
				this.contained.CancelJob();
				SpaceStationInterior.instance.UpdateJobs();
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0007F5CC File Offset: 0x0007D7CC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			int initialAmount = this.contained.initialAmount;
			int num = initialAmount - this.contained.remainingAmount;
			string str = Translation.Translate("@SSRefining", Array.Empty<object>()) + " ";
			if (this.contained is ForgeJob)
			{
				str = Translation.Translate("@SSCrafting", Array.Empty<object>()) + " ";
			}
			else if (this.contained is RepairJob)
			{
				str = "";
			}
			tooltip.AddTextLine(str + Translation.Translate(this.contained.jobName, Array.Empty<object>()), 12, 8f);
			string str2 = Translation.Translate("@JRefinery", Array.Empty<object>());
			Color color = ColorHelper.refineryJob;
			if (this.contained is ForgeJob)
			{
				str2 = Translation.Translate("@JForge", Array.Empty<object>());
				color = ColorHelper.forgeJob;
			}
			else if (this.contained is RepairJob)
			{
				str2 = Translation.Translate("@JRepair", Array.Empty<object>());
				color = ColorHelper.reddish;
			}
			tooltip.AddTextLine("[" + str2 + "]", 10, 8f).Text.color = color;
			tooltip.AddTextLine(string.Concat(new string[]
			{
				Translation.Translate("@SSProgress", Array.Empty<object>()),
				": ",
				num.ToString(),
				" / ",
				initialAmount.ToString()
			}), 12, 8f).Text.color = ColorHelper.boringGrey;
			tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
			float num2 = (float)(initialAmount - num);
			RefineryJob refineryJob = this.contained as RefineryJob;
			if (refineryJob != null)
			{
				foreach (OreRefinementProduct oreRefinementProduct in refineryJob.ore.contents)
				{
					float num3 = oreRefinementProduct.yield * (float)num;
					float num4 = oreRefinementProduct.yield * (float)initialAmount;
					tooltip.AddTextLine(string.Concat(new string[]
					{
						oreRefinementProduct.product.ToString().HighlightWithColor(oreRefinementProduct.product.GetColor()),
						": ",
						GameMath.FormatNumber(num3, 2),
						" / ",
						GameMath.FormatNumber(num4, 2)
					}), 12, 8f);
				}
				num2 *= refineryJob.refineTime;
				num2 -= refineryJob.refineTime * refineryJob.jobProgress;
			}
			else
			{
				ForgeJob forgeJob = this.contained as ForgeJob;
				if (forgeJob != null)
				{
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair in forgeJob.recipe.GetResultsPreview())
					{
						int num5 = keyValuePair.Value * initialAmount;
						int num6 = keyValuePair.Value * num;
						tooltip.AddTextLine(string.Concat(new string[]
						{
							Translation.Translate(keyValuePair.Key.displayName, Array.Empty<object>()).HighlightWithColor(keyValuePair.Key.rarity.GetColor()),
							": ",
							num6.ToString(),
							" / ",
							num5.ToString()
						}), 12, 8f);
					}
					num2 *= forgeJob.craftingTime;
					num2 -= forgeJob.craftingTime * forgeJob.jobProgress;
				}
				else
				{
					RepairJob repairJob = this.contained as RepairJob;
					if (repairJob != null)
					{
						tooltip.AddTextLine(Translation.Translate("@SSShipRepair", new object[]
						{
							repairJob.spaceshipData.GetShipName()
						}), 12, 8f);
						num2 = num2 / repairJob.repairAmount * repairJob.repairTime;
					}
				}
			}
			tooltip.AddTextLine(Translation.Translate("@SSTimeRemaining", Array.Empty<object>()) + ": " + FormatString.FormatTime(num2), 12, 8f).Text.color = ColorHelper.boringGrey;
		}

		// Token: 0x04000B3C RID: 2876
		[SerializeField]
		private Image icon;

		// Token: 0x04000B3D RID: 2877
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000B3E RID: 2878
		[SerializeField]
		private Image backgroundImage;

		// Token: 0x04000B3F RID: 2879
		[SerializeField]
		private RectTransform progress;

		// Token: 0x04000B40 RID: 2880
		[SerializeField]
		private Button cancel;

		// Token: 0x04000B41 RID: 2881
		protected ISpaceStationJob contained;

		// Token: 0x04000B42 RID: 2882
		private float currentProgress = 1f;

		// Token: 0x04000B43 RID: 2883
		private float updateTimer;

		// Token: 0x04000B44 RID: 2884
		private TooltipSource tooltip;
	}
}
