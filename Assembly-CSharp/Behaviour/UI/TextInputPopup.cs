using System;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F4 RID: 500
	public class TextInputPopup : AbstractPopup
	{
		// Token: 0x060012D9 RID: 4825 RVA: 0x0007B281 File Offset: 0x00079481
		public string GetInput()
		{
			return this.input.text;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0007B290 File Offset: 0x00079490
		public void ChangeShipName(SpaceShipData shipData)
		{
			this.input.text = shipData.GetShipName();
			this.actionButton.GetComponentInChildren<TextMeshProUGUI>().text = Translation.Translate("@UIChange", Array.Empty<object>());
			this.actionButton.GetComponent<Image>().color = ColorHelper.greenish;
			this.cancelButton.GetComponent<Image>().color = ColorHelper.reddish;
			this.title.text = Translation.Translate("@UIShipName", Array.Empty<object>());
		}
	}
}
