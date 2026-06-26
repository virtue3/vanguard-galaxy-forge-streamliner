using System;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B0 RID: 432
	public class Materials : MonoBehaviour
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00069EFC File Offset: 0x000680FC
		public static Material Grayscale
		{
			get
			{
				return Materials._instance._grayscale;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00069F08 File Offset: 0x00068108
		public static Material Grayscale75
		{
			get
			{
				return Materials._instance._grayscale75;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x00069F14 File Offset: 0x00068114
		public static Material PoiBorder
		{
			get
			{
				return Materials._instance.poiBorder;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x00069F20 File Offset: 0x00068120
		public static Material Default
		{
			get
			{
				return Materials._instance._default;
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00069F2C File Offset: 0x0006812C
		private void Awake()
		{
			Materials._instance = this;
		}

		// Token: 0x04000897 RID: 2199
		private static Materials _instance;

		// Token: 0x04000898 RID: 2200
		[SerializeField]
		private Material _grayscale;

		// Token: 0x04000899 RID: 2201
		[SerializeField]
		private Material _grayscale75;

		// Token: 0x0400089A RID: 2202
		[SerializeField]
		private Material _default;

		// Token: 0x0400089B RID: 2203
		[SerializeField]
		private Material poiBorder;
	}
}
