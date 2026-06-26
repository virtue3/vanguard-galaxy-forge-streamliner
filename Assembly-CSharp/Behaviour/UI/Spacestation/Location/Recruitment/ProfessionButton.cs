using System;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location.Recruitment
{
	// Token: 0x02000231 RID: 561
	public class ProfessionButton : MonoBehaviour
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x00087F60 File Offset: 0x00086160
		public void SetCallback(Action<Profession> callback, Profession profession)
		{
			this.profession = profession;
			this.callback = callback;
			TooltipSource component = base.GetComponent<TooltipSource>();
			string text = "@SSProfession";
			ColorBlock colors = this.button.colors;
			colors.normalColor = this.GetColor();
			this.button.colors = colors;
			this.text.text = profession.ToString().ToUpper()[0].ToString();
			text = text + this.text.text + profession.ToString().Substring(1, profession.ToString().Length - 1);
			component.BodyText = text;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00088018 File Offset: 0x00086218
		public Color GetColor()
		{
			Profession profession = this.profession;
			Color result;
			if (profession != Profession.Mining)
			{
				if (profession != Profession.Salvaging)
				{
					result = ColorHelper.combatRed;
				}
				else
				{
					result = ColorHelper.salvageYellow;
				}
			}
			else
			{
				result = ColorHelper.miningBlue;
			}
			return result;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0008804E File Offset: 0x0008624E
		public void OnClick()
		{
			this.callback(this.profession);
		}

		// Token: 0x04000C5F RID: 3167
		[SerializeField]
		private TMP_Text text;

		// Token: 0x04000C60 RID: 3168
		[SerializeField]
		private Button button;

		// Token: 0x04000C61 RID: 3169
		private Profession profession;

		// Token: 0x04000C62 RID: 3170
		private Action<Profession> callback;
	}
}
