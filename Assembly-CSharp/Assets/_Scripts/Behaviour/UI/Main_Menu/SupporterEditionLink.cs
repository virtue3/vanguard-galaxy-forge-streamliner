using System;
using UnityEngine;

namespace Assets._Scripts.Behaviour.UI.Main_Menu
{
	// Token: 0x020001A1 RID: 417
	public class SupporterEditionLink : MonoBehaviour
	{
		// Token: 0x06000EB1 RID: 3761 RVA: 0x00068A90 File Offset: 0x00066C90
		public void OpenSteamSupporterURL()
		{
			Application.OpenURL("https://store.steampowered.com/app/4621360/Vanguard_Galaxy__Supporter_Pack/");
		}
	}
}
