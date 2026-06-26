using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Main_Menu
{
	// Token: 0x02000273 RID: 627
	[CreateAssetMenu(fileName = "InputValidator - FileName.asset", menuName = "TextMeshPro/Input Validators/FileName", order = 100)]
	[Serializable]
	public class SaveNameFilter : TMP_InputValidator
	{
		// Token: 0x06001700 RID: 5888 RVA: 0x00091849 File Offset: 0x0008FA49
		public override char Validate(ref string text, ref int pos, char ch)
		{
			if (SaveNameFilter.forbidden.Contains(ch))
			{
				return '\0';
			}
			text = text.Insert(pos, ch.ToString());
			pos++;
			return ch;
		}

		// Token: 0x04000E24 RID: 3620
		private static HashSet<char> forbidden = new HashSet<char>
		{
			'<',
			'>',
			':',
			'"',
			'/',
			'\\',
			'|',
			'?',
			'*'
		};
	}
}
