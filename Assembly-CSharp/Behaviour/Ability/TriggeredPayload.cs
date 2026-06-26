using System;
using UnityEngine;

namespace Behaviour.Ability
{
	// Token: 0x020003C6 RID: 966
	public abstract class TriggeredPayload : MonoBehaviour
	{
		// Token: 0x06002558 RID: 9560
		public abstract void PayloadTriggered(object source, int stackSize = 1);

		// Token: 0x06002559 RID: 9561 RVA: 0x000D09B2 File Offset: 0x000CEBB2
		public virtual bool ShouldTrigger(object source)
		{
			return true;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000D09B5 File Offset: 0x000CEBB5
		protected virtual void Start()
		{
			if (!base.GetComponent<TemporaryEffect>())
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
