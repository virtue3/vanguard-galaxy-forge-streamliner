using System;
using UnityEngine;

namespace Source.Spacestation
{
	// Token: 0x02000054 RID: 84
	public interface ISpaceStationJob
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600033F RID: 831
		bool cancelAvailable { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000340 RID: 832
		string jobName { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000341 RID: 833
		Sprite jobIcon { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000342 RID: 834
		float jobProgress { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000343 RID: 835
		int initialAmount { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000344 RID: 836
		int remainingAmount { get; }

		// Token: 0x06000345 RID: 837
		void ProgressJob(float deltaTime);

		// Token: 0x06000346 RID: 838
		void CancelJob();
	}
}
