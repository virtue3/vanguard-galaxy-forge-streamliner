using System;
using UnityEngine;

namespace Behaviour.Weapons
{
	// Token: 0x020001AE RID: 430
	public class TorpedoLatchPoint : MonoBehaviour
	{
		// Token: 0x06000F2F RID: 3887 RVA: 0x00069DDE File Offset: 0x00067FDE
		public Vector2 TorpedoPosition()
		{
			return this.torpedoCenter.transform.position;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00069DF5 File Offset: 0x00067FF5
		public void ToggleActive()
		{
			this.torpedoCenter.SetActive(!this.torpedoCenter.activeSelf);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00069E10 File Offset: 0x00068010
		public void SetReadyToFire(bool fire)
		{
			this.isDeployed = !fire;
			this.isReloading = !fire;
			this.isReadyToFire = fire;
			this.torpedoCenter.SetActive(fire);
		}

		// Token: 0x04000892 RID: 2194
		[SerializeField]
		private GameObject torpedoCenter;

		// Token: 0x04000893 RID: 2195
		public bool isDeployed;

		// Token: 0x04000894 RID: 2196
		public bool isReloading;

		// Token: 0x04000895 RID: 2197
		public bool isReadyToFire;
	}
}
