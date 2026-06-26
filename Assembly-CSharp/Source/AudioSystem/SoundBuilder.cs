using System;
using Behaviour.AudioSystem;
using UnityEngine;

namespace Source.AudioSystem
{
	// Token: 0x0200018D RID: 397
	public class SoundBuilder
	{
		// Token: 0x06000E35 RID: 3637 RVA: 0x00066D82 File Offset: 0x00064F82
		public SoundBuilder(SoundManager soundManager)
		{
			this.soundManager = soundManager;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00066D9C File Offset: 0x00064F9C
		public SoundBuilder WithSoundData(SoundData soundData)
		{
			this.soundData = soundData;
			return this;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00066DA6 File Offset: 0x00064FA6
		public SoundBuilder WithPosition(Vector2 position)
		{
			this.position = position;
			return this;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00066DB0 File Offset: 0x00064FB0
		public SoundBuilder WithFollow(Transform target)
		{
			this.followTarget = target;
			return this;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00066DBA File Offset: 0x00064FBA
		public SoundBuilder WithRandomPitch()
		{
			this.randomPitch = true;
			return this;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00066DC4 File Offset: 0x00064FC4
		public SoundBuilder WithCustomVolume(float volume)
		{
			this.hasCustomVolume = true;
			this.customVolume = Mathf.Max(0f, volume);
			return this;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00066DDF File Offset: 0x00064FDF
		public SoundBuilder WithPowerVolume(float volume)
		{
			this.hasPowerVolume = true;
			this.powerVolume = Mathf.Max(0f, volume);
			return this;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00066DFA File Offset: 0x00064FFA
		public void Play()
		{
			SoundEmitter soundEmitter = this.BuildEmitter();
			soundEmitter.UpdateDynamic();
			soundEmitter.Play();
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00066E0D File Offset: 0x0006500D
		public SoundEmitter PlayReturn()
		{
			SoundEmitter soundEmitter = this.BuildEmitter();
			soundEmitter.UpdateDynamic();
			soundEmitter.Play();
			return soundEmitter;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00066E24 File Offset: 0x00065024
		private SoundEmitter BuildEmitter()
		{
			if (!this.soundManager.CanPlaySound(this.soundData))
			{
				return null;
			}
			SoundEmitter soundEmitter = this.soundManager.Get();
			soundEmitter.Init(this.soundData);
			if (this.hasCustomVolume)
			{
				soundEmitter.baseVolume = this.customVolume;
			}
			if (this.hasPowerVolume)
			{
				soundEmitter.ChangePowerVolume(this.powerVolume);
			}
			if (this.position != Vector2.zero)
			{
				soundEmitter.transform.position = this.position;
			}
			else
			{
				soundEmitter.transform.position = this.soundManager.gameCamera.transform.position;
			}
			if (this.randomPitch)
			{
				soundEmitter.WithRandomPitch(this.soundData);
			}
			if (this.soundData.frequentSound)
			{
				this.soundManager.FrequentSoundEmitters.Enqueue(soundEmitter);
			}
			if (this.followTarget != null)
			{
				soundEmitter.SetFollowTarget(this.followTarget);
			}
			return soundEmitter;
		}

		// Token: 0x040007DC RID: 2012
		private readonly SoundManager soundManager;

		// Token: 0x040007DD RID: 2013
		private SoundData soundData;

		// Token: 0x040007DE RID: 2014
		private bool randomPitch;

		// Token: 0x040007DF RID: 2015
		private Vector2 position = Vector2.zero;

		// Token: 0x040007E0 RID: 2016
		private Transform followTarget;

		// Token: 0x040007E1 RID: 2017
		private float customVolume;

		// Token: 0x040007E2 RID: 2018
		private bool hasCustomVolume;

		// Token: 0x040007E3 RID: 2019
		private bool hasPowerVolume;

		// Token: 0x040007E4 RID: 2020
		private float powerVolume;
	}
}
