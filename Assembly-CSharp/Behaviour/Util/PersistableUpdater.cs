using System;
using Source.Data.Persistable;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B2 RID: 434
	public class PersistableUpdater : MonoBehaviour
	{
		// Token: 0x06000F3F RID: 3903 RVA: 0x00069FC8 File Offset: 0x000681C8
		private void Start()
		{
			this.rigidbody = base.GetComponent<Rigidbody2D>();
			if (this.rigidbody)
			{
				this.rigidbody.angularVelocity = this.data.angularVelocity;
				this.rigidbody.linearVelocity = this.data.velocity;
			}
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0006A01C File Offset: 0x0006821C
		private void Update()
		{
			this.data.position = base.transform.position;
			this.data.angle = base.transform.rotation.eulerAngles.z;
			if (this.rigidbody)
			{
				this.data.velocity = this.rigidbody.linearVelocity;
				this.data.angularVelocity = this.rigidbody.angularVelocity;
			}
		}

		// Token: 0x0400089F RID: 2207
		public PersistableData data;

		// Token: 0x040008A0 RID: 2208
		private Rigidbody2D rigidbody;
	}
}
