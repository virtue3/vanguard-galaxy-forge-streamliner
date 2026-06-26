using System;
using Behaviour.Bootstrap;
using Behaviour.Util;
using Source.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Settings
{
	// Token: 0x02000242 RID: 578
	public class FullscreenToggle : MonoBehaviour
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x00089CA0 File Offset: 0x00087EA0
		private void Awake()
		{
			if (SteamCommunication.runningOnSteamDeck)
			{
				base.gameObject.SetActive(false);
			}
			this.toggle.SetIsOnWithoutNotify(GameplayerPrefs.GetTransparencyMode() == TransparencyMode.FullScreen.ToString());
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00089CE4 File Offset: 0x00087EE4
		private void Update()
		{
			this.toggle.SetIsOnWithoutNotify(GameplayerPrefs.GetTransparencyMode() == TransparencyMode.FullScreen.ToString());
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00089D18 File Offset: 0x00087F18
		public void OnToggleChange()
		{
			TransparencyMode transparencyMode;
			if (this.toggle.isOn)
			{
				transparencyMode = TransparencyMode.FullScreen;
			}
			else
			{
				transparencyMode = Enum.Parse<TransparencyMode>(GameplayerPrefs.GetPreferredTransparencyMode());
			}
			GameplayerPrefs.SetTransparencyMode(transparencyMode.ToString());
			PersistentSingleton<Bootstrapper>.Instance.SetTransparencyMode(transparencyMode);
		}

		// Token: 0x04000CC2 RID: 3266
		[SerializeField]
		private Toggle toggle;

		// Token: 0x04000CC3 RID: 3267
		[SerializeField]
		private TMP_Text text;
	}
}
