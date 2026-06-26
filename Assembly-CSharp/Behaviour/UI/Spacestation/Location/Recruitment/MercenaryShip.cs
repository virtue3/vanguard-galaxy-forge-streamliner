using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location.Recruitment
{
	// Token: 0x02000230 RID: 560
	public class MercenaryShip : MonoBehaviour
	{
		// Token: 0x06001507 RID: 5383 RVA: 0x00087F3D File Offset: 0x0008613D
		public void SetShip(string name, Sprite ship)
		{
			this.image.sprite = ship;
			this.shipName.text = name;
		}

		// Token: 0x04000C5D RID: 3165
		[SerializeField]
		private Image image;

		// Token: 0x04000C5E RID: 3166
		[SerializeField]
		private TMP_Text shipName;
	}
}
