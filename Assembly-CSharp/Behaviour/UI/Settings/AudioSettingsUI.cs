using System;
using Behaviour.AudioSystem;
using Behaviour.Util;
using Source.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Settings
{
	// Token: 0x02000240 RID: 576
	public class AudioSettingsUI : SettingsSubMenu
	{
		// Token: 0x06001563 RID: 5475 RVA: 0x00089993 File Offset: 0x00087B93
		private void Start()
		{
			this.SetMusicVolume();
			this.SetSoundEffectVolume();
			this.SetThrusterVolume();
			this.SetAudioToggle();
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x000899AD File Offset: 0x00087BAD
		private void Update()
		{
			this.SetAudioToggle();
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x000899B5 File Offset: 0x00087BB5
		public void OnChangeMusicVolume()
		{
			GameplayerPrefs.SetMusicVolume(this.musicSlider.value);
			PersistentSingleton<MusicManager>.Instance.SetMusicVolume();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x000899D1 File Offset: 0x00087BD1
		private void SetMusicVolume()
		{
			this.musicSlider.value = GameplayerPrefs.GetMusicVolume();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x000899E3 File Offset: 0x00087BE3
		public void OnChangeSoundEffectVolume()
		{
			GameplayerPrefs.SetSoundEffectVolume(this.soundEffectSlider.value);
			PersistentSingleton<SoundManager>.Instance.SetGameVolume();
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000899FF File Offset: 0x00087BFF
		private void SetSoundEffectVolume()
		{
			this.soundEffectSlider.value = GameplayerPrefs.GetSoundEffectVolume();
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00089A11 File Offset: 0x00087C11
		public void OnChangeThrusterVolume()
		{
			GameplayerPrefs.SetThrusterVolume(this.thrusterSlider.value);
			PersistentSingleton<SoundManager>.Instance.SetThrusterVolume();
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00089A2D File Offset: 0x00087C2D
		private void SetThrusterVolume()
		{
			this.thrusterSlider.value = GameplayerPrefs.GetThrusterVolume();
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00089A3F File Offset: 0x00087C3F
		public void OnChangeAudioMuted()
		{
			GameplayerPrefs.SetAudioMuted(this.audioMuteToggle.isOn);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00089A51 File Offset: 0x00087C51
		private void SetAudioToggle()
		{
			this.audioMuteToggle.SetIsOnWithoutNotify(GameplayerPrefs.GetAudioMuted());
		}

		// Token: 0x04000CB6 RID: 3254
		[SerializeField]
		private Slider musicSlider;

		// Token: 0x04000CB7 RID: 3255
		[SerializeField]
		private Slider soundEffectSlider;

		// Token: 0x04000CB8 RID: 3256
		[SerializeField]
		private Slider thrusterSlider;

		// Token: 0x04000CB9 RID: 3257
		[SerializeField]
		private Toggle audioMuteToggle;
	}
}
