using System;
using System.Collections.Generic;
using Source.Crew;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location.Recruitment
{
	// Token: 0x02000232 RID: 562
	public class ProfessionFilter : MonoBehaviour
	{
		// Token: 0x0600150D RID: 5389 RVA: 0x0008806C File Offset: 0x0008626C
		private void Start()
		{
			foreach (Profession profession in this.professions)
			{
				UnityEngine.Object.Instantiate<ProfessionButton>(this.professionButtonPrefab, base.transform).SetCallback(new Action<Profession>(this.recruitmentCenter.SetProfessionFilter), profession);
			}
		}

		// Token: 0x04000C63 RID: 3171
		[SerializeField]
		private ProfessionButton professionButtonPrefab;

		// Token: 0x04000C64 RID: 3172
		[SerializeField]
		private List<Profession> professions;

		// Token: 0x04000C65 RID: 3173
		[SerializeField]
		private RecruitmentCenterUI recruitmentCenter;
	}
}
