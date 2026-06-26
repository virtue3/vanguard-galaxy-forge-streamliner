using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.UI.Main
{
	// Token: 0x02000264 RID: 612
	public class StepProgression : MonoBehaviour
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x0008F040 File Offset: 0x0008D240
		private void Start()
		{
			foreach (StepProgressionImage stepProgressionImage in this.stepProgressionImagesList)
			{
				this.stepProgressionImages.Add(stepProgressionImage.step, stepProgressionImage);
				if (stepProgressionImage.step == 1)
				{
					stepProgressionImage.SetCurrent();
				}
				else
				{
					stepProgressionImage.SetIncomplete();
				}
			}
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0008F0B8 File Offset: 0x0008D2B8
		public void SetStepState(string currentStep, StepState state)
		{
			int num;
			if (!int.TryParse(currentStep, out num))
			{
				Debug.LogWarning("Invalid step format, must be an integer.");
				return;
			}
			StepProgressionImage stepProgressionImage;
			if (!this.stepProgressionImages.TryGetValue(num, out stepProgressionImage))
			{
				Debug.LogWarning(string.Format("Step {0} does not exist.", num));
				return;
			}
			switch (state)
			{
			case StepState.Completed:
				stepProgressionImage.SetCompleted();
				return;
			case StepState.Incomplete:
				stepProgressionImage.SetIncomplete();
				return;
			case StepState.Current:
				stepProgressionImage.SetCurrent();
				return;
			default:
				Debug.LogWarning("Unknown step state.");
				return;
			}
		}

		// Token: 0x04000DBB RID: 3515
		[SerializeField]
		private List<StepProgressionImage> stepProgressionImagesList;

		// Token: 0x04000DBC RID: 3516
		private Dictionary<int, StepProgressionImage> stepProgressionImages = new Dictionary<int, StepProgressionImage>();
	}
}
