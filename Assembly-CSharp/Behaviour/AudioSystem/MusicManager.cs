using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.UI.Travel;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;
using UnityEngine;

namespace Behaviour.AudioSystem
{
	// Token: 0x020003BC RID: 956
	public class MusicManager : PersistentSingleton<MusicManager>
	{
		// Token: 0x060024C7 RID: 9415 RVA: 0x000CF1DC File Offset: 0x000CD3DC
		private void Start()
		{
			this.shuffledPlaylist = this.ShufflePlaylist(this.playlist);
			this.shuffledCombatPlaylist = this.ShufflePlaylist(this.combatPlaylist);
			this.SetMusicVolume();
			this.PlayMusic(this.playlist[0], 0f);
			this.shuffledPlaylist = new Queue<AudioClip>(from clip in this.shuffledPlaylist
			where clip != this.playlist[0]
			select clip);
			this.musicAudioSource.priority = 0;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000CF258 File Offset: 0x000CD458
		public void SetMusicVolume()
		{
			if (GameplayerPrefs.GetAudioMuted())
			{
				this.musicVolume = 0f;
			}
			else
			{
				this.musicVolume = GameplayerPrefs.GetMusicVolume();
			}
			this.UpdateVolume();
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000CF27F File Offset: 0x000CD47F
		private void UpdateVolume()
		{
			this.musicAudioSource.volume = this.musicVolume;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000CF294 File Offset: 0x000CD494
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer > 0f)
			{
				return;
			}
			this.updateTimer = 0.5f;
			if (!this.hasCombatMusic)
			{
				if (this.ShouldPlayCombatMusic() && TravelInfo.instance && TravelInfo.instance.travelETAInfo.currentETA < 30f)
				{
					this.PlayCombatTrack();
				}
				if (this.nextTrackTimer > 0f)
				{
					this.nextTrackTimer -= 0.5f;
					if (this.nextTrackTimer <= 0f)
					{
						this.PlayNextTrack();
						return;
					}
				}
				else
				{
					if (GameplayerPrefs.GetMusicVolume() == 0f)
					{
						return;
					}
					if (this.isStopped || this.musicAudioSource.isPlaying || this.playlist.Count <= 0 || this.musicAudioSource.mute)
					{
						return;
					}
					this.nextTrackTimer = 15f;
				}
				return;
			}
			if (this.ShouldPlayCombatMusic())
			{
				this.endCombatTimer = 5f;
			}
			else
			{
				this.endCombatTimer -= 0.5f;
			}
			if (this.endCombatTimer < 0f)
			{
				this.ResumeMusic();
				return;
			}
			if (!this.musicAudioSource.isPlaying)
			{
				this.PlayCombatTrack();
			}
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000CF3CF File Offset: 0x000CD5CF
		private bool ShouldPlayCombatMusic()
		{
			GamePlayer current = GamePlayer.current;
			if (((current != null) ? current.infiniteMission : null) == null)
			{
				MapPointOfInterest currentOrNext = MapPointOfInterest.currentOrNext;
				return currentOrNext != null && currentOrNext.hasCombatMusic;
			}
			return true;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000CF3F8 File Offset: 0x000CD5F8
		private Queue<AudioClip> ShufflePlaylist(List<AudioClip> playlist)
		{
			List<AudioClip> list = new List<AudioClip>(playlist);
			SeededRandom.Global.Shuffle<AudioClip>(list);
			return new Queue<AudioClip>(list);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000CF420 File Offset: 0x000CD620
		public void PlayCombatTrack()
		{
			if (this.musicAudioSource.isPlaying)
			{
				this.preCombatClip = this.musicAudioSource.clip;
				this.preCombatTime = this.musicAudioSource.time;
			}
			this.hasCombatMusic = true;
			if (this.shuffledCombatPlaylist.Count == 0)
			{
				this.shuffledCombatPlaylist = this.ShufflePlaylist(this.combatPlaylist);
			}
			this.PlayMusic(this.shuffledCombatPlaylist.Dequeue(), 0f);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000CF498 File Offset: 0x000CD698
		public void PlayNextTrack()
		{
			if (this.shuffledPlaylist.Count == 0)
			{
				this.shuffledPlaylist = this.ShufflePlaylist(this.playlist);
			}
			this.PlayMusic(this.shuffledPlaylist.Dequeue(), 0f);
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000CF4D0 File Offset: 0x000CD6D0
		public void PlayMusic(AudioClip musicClip, float progress = 0f)
		{
			if (this.musicAudioSource.isPlaying && this.musicAudioSource.clip == musicClip)
			{
				return;
			}
			if (this.fadeCoroutine != null)
			{
				base.StopCoroutine(this.fadeCoroutine);
			}
			if (this.musicAudioSource.isPlaying)
			{
				this.fadeCoroutine = base.StartCoroutine(this.FadeOutAndPlayNewTrack(musicClip, progress));
				return;
			}
			this.isStopped = false;
			this.musicAudioSource.volume = this.musicVolume;
			this.musicAudioSource.clip = musicClip;
			this.musicAudioSource.time = progress;
			this.musicAudioSource.Play();
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000CF56F File Offset: 0x000CD76F
		public void ResumeMusic()
		{
			this.hasCombatMusic = false;
			if (this.preCombatClip)
			{
				this.PlayMusic(this.preCombatClip, this.preCombatTime);
				this.preCombatClip = null;
				return;
			}
			this.PlayNextTrack();
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000CF5A5 File Offset: 0x000CD7A5
		public void StopMusic()
		{
			this.isStopped = true;
			base.StartCoroutine(this.FadeOutMusic());
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000CF5BB File Offset: 0x000CD7BB
		private IEnumerator FadeOutAndPlayNewTrack(AudioClip newClip, float progress = 0f)
		{
			yield return this.FadeOutMusic();
			this.musicAudioSource.clip = newClip;
			this.musicAudioSource.time = progress;
			this.musicAudioSource.Play();
			this.musicAudioSource.volume = this.musicVolume;
			if (progress > 0f)
			{
				yield return this.FadeInMusic();
			}
			yield break;
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000CF5D8 File Offset: 0x000CD7D8
		private IEnumerator FadeInMusic()
		{
			this.musicAudioSource.volume = 0f;
			float targetVolume = this.musicVolume;
			for (float t = 0f; t < this.fadeDuration; t += Time.deltaTime)
			{
				this.musicAudioSource.volume = Mathf.Lerp(0f, targetVolume, t / this.fadeDuration);
				yield return null;
			}
			this.musicAudioSource.volume = targetVolume;
			yield break;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000CF5E7 File Offset: 0x000CD7E7
		private IEnumerator FadeOutMusic()
		{
			float startVolume = this.musicAudioSource.volume;
			for (float t = 0f; t < this.fadeDuration; t += Time.deltaTime)
			{
				this.musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / this.fadeDuration);
				yield return null;
			}
			this.musicAudioSource.volume = 0f;
			this.musicAudioSource.Stop();
			yield break;
		}

		// Token: 0x04001656 RID: 5718
		[SerializeField]
		private AudioSource musicAudioSource;

		// Token: 0x04001657 RID: 5719
		[SerializeField]
		private List<AudioClip> playlist;

		// Token: 0x04001658 RID: 5720
		[SerializeField]
		private List<AudioClip> combatPlaylist;

		// Token: 0x04001659 RID: 5721
		private Queue<AudioClip> shuffledPlaylist;

		// Token: 0x0400165A RID: 5722
		private Queue<AudioClip> shuffledCombatPlaylist;

		// Token: 0x0400165B RID: 5723
		private readonly float fadeDuration = 2f;

		// Token: 0x0400165C RID: 5724
		private Coroutine fadeCoroutine;

		// Token: 0x0400165D RID: 5725
		private bool hasCombatMusic;

		// Token: 0x0400165E RID: 5726
		private AudioClip preCombatClip;

		// Token: 0x0400165F RID: 5727
		private float preCombatTime;

		// Token: 0x04001660 RID: 5728
		private float endCombatTimer;

		// Token: 0x04001661 RID: 5729
		private bool isStopped;

		// Token: 0x04001662 RID: 5730
		private float updateTimer;

		// Token: 0x04001663 RID: 5731
		private float nextTrackTimer;

		// Token: 0x04001664 RID: 5732
		private float musicVolume = 1f;
	}
}
