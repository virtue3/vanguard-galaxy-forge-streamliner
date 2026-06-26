using System;
using Behaviour.Spacestation.Cargo;
using Source.Galaxy.POI;
using UnityEngine;

namespace Behavior.Spacestation
{
	// Token: 0x02000194 RID: 404
	public class Spacestation : MonoBehaviour
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x000677AB File Offset: 0x000659AB
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x000677B3 File Offset: 0x000659B3
		public Transform leftWarpInTransform { get; private set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x000677BC File Offset: 0x000659BC
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x000677C4 File Offset: 0x000659C4
		public Transform righttWarpInTransform { get; private set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x000677CD File Offset: 0x000659CD
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x000677D5 File Offset: 0x000659D5
		public SpaceStation.StationVariants variant { get; private set; }

		// Token: 0x06000E6C RID: 3692 RVA: 0x000677DE File Offset: 0x000659DE
		private void Awake()
		{
			this.cargoDockManager = base.GetComponentInChildren<CargoDockManager>();
		}

		// Token: 0x0400081F RID: 2079
		private bool inputDisabled;

		// Token: 0x04000821 RID: 2081
		private CargoDockManager cargoDockManager;
	}
}
