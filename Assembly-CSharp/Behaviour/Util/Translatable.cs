using System;
using TMPro;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B9 RID: 441
	public class Translatable : MonoBehaviour
	{
		// Token: 0x06000F6D RID: 3949 RVA: 0x0006B0C4 File Offset: 0x000692C4
		private void OnEnable()
		{
			if (this._translateKey == "@Missing")
			{
				return;
			}
			TMP_Text component = base.GetComponent<TMP_Text>();
			if (!component)
			{
				return;
			}
			if (!this.initialized)
			{
				this.initialized = true;
				if (string.IsNullOrEmpty(this._translateKey))
				{
					this._translateKey = component.text;
				}
				else if (this._translateKey[0] != '@')
				{
					this._translateKey = "@" + this._translateKey;
				}
			}
			if (!string.IsNullOrEmpty(this._translateKey))
			{
				component.TL(this._translateKey, Array.Empty<object>());
			}
		}

		// Token: 0x040008C5 RID: 2245
		[SerializeField]
		private string _translateKey;

		// Token: 0x040008C6 RID: 2246
		private bool initialized;
	}
}
