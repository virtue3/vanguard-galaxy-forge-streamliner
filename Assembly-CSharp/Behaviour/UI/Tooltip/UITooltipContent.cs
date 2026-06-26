using System;
using UnityEngine;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x0200020B RID: 523
	public abstract class UITooltipContent : MonoBehaviour
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x0007E9A8 File Offset: 0x0007CBA8
		public virtual float Spacing
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001371 RID: 4977
		public abstract float Height { get; }
	}
}
