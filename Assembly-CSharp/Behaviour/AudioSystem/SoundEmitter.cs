using System;
using System.Collections;
using Behaviour.Util;
using Source.AudioSystem;
using UnityEngine;

namespace Behaviour.AudioSystem
{
	// Token: 0x020003BD RID: 957
	[RequireComponent(typeof(AudioSource))]
	public class SoundEmitter : MonoBehaviour
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x000CF628 File Offset: 0x000CD828
		// (set) Token: 0x060024D8 RID: 9432 RVA: 0x000CF630 File Offset: 0x000CD830
		public SoundData soundData { get; private set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x000CF639 File Offset: 0x000CD839
		// (set) Token: 0x060024DA RID: 9434 RVA: 0x000CF641 File Offset: 0x000CD841
		public bool looping { get; private set; }

		// Token: 0x060024DB RID: 9435 RVA: 0x000CF64A File Offset: 0x000CD84A
		private void Awake()
		{
			this.audioSource = base.GetComponent<AudioSource>();
			this.gameCamera = PersistentSingleton<SoundManager>.Instance.gameCamera;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000CF668 File Offset: 0x000CD868
		public void UpdateDynamic()
		{
			this.SetFollowPosition();
			float zoomFactor = this.GetZoomFactor();
			float distanceFactor = this.GetDistanceFactor();
			float num = this.baseVolume * zoomFactor * distanceFactor * this.gameVolume * this.powerVolume;
			if (this.thrusterSound)
			{
				num *= PersistentSingleton<SoundManager>.Instance.thrusterVolume;
			}
			this.audioSource.volume = num * this.powerVolume;
			this.ApplyStereoPan();
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000CF6D0 File Offset: 0x000CD8D0
		private void SetFollowPosition()
		{
			if (this.followPosition && this.followTarget != null)
			{
				base.transform.position = this.followTarget.position;
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000CF700 File Offset: 0x000CD900
		private float GetZoomFactor()
		{
			float orthographicSize = this.gameCamera.orthographicSize;
			float b = 5f;
			float t = Mathf.InverseLerp(20f, b, orthographicSize);
			return Mathf.Lerp(0.5f, 1f, t);
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000CF740 File Offset: 0x000CD940
		private float GetDistanceFactor()
		{
			float t = Mathf.Clamp01(Vector2.Distance(this.gameCamera.transform.position, base.transform.position) / this.maxCameraDistance);
			return 1f - Mathf.SmoothStep(0f, 1f, t);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000CF79C File Offset: 0x000CD99C
		public void ApplyStereoPan()
		{
			float num = this.gameCamera.orthographicSize * this.gameCamera.aspect;
			float panStereo = Mathf.Clamp((base.transform.position.x - this.gameCamera.transform.position.x) / num, -1f, 1f);
			this.audioSource.panStereo = panStereo;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000CF808 File Offset: 0x000CDA08
		public void Play()
		{
			this.isReturning = false;
			if (this.playingCoroutine != null)
			{
				base.StopCoroutine(this.playingCoroutine);
			}
			this.audioSource.Play();
			if (!this.audioSource.loop)
			{
				this.playingCoroutine = base.StartCoroutine(this.WaitForSoundToEnd());
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000CF85A File Offset: 0x000CDA5A
		public void ChangeVolume(float volume)
		{
			this.gameVolume = volume;
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000CF863 File Offset: 0x000CDA63
		public void ChangePowerVolume(float volume)
		{
			this.powerVolume = volume;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000CF86C File Offset: 0x000CDA6C
		private IEnumerator WaitForSoundToEnd()
		{
			yield return new WaitWhile(() => this.audioSource.isPlaying);
			if (!this.isReturning)
			{
				base.StartCoroutine(this.FadeOutAndReturn(0.1f));
			}
			yield break;
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000CF87B File Offset: 0x000CDA7B
		private IEnumerator FadeOutAndReturn(float duration)
		{
			if (this.isReturning)
			{
				yield break;
			}
			this.isReturning = true;
			float startVol = this.audioSource.volume;
			for (float t = 0f; t < duration; t += Time.deltaTime)
			{
				this.audioSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
				yield return null;
			}
			this.audioSource.Stop();
			this.ReturnToPool();
			yield break;
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000CF894 File Offset: 0x000CDA94
		public void Stop()
		{
			if (this.isReturning)
			{
				return;
			}
			if (this.playingCoroutine != null)
			{
				base.StopCoroutine(this.playingCoroutine);
				this.playingCoroutine = null;
			}
			if (this.audioSource.isPlaying)
			{
				base.StartCoroutine(this.FadeOutAndReturn(0.1f));
				return;
			}
			this.ReturnToPool();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000CF8EB File Offset: 0x000CDAEB
		public void Pause()
		{
			this.audioSource.Pause();
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000CF8F8 File Offset: 0x000CDAF8
		public void Unpause()
		{
			this.audioSource.UnPause();
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000CF908 File Offset: 0x000CDB08
		public void Init(SoundData data)
		{
			this.soundData = data;
			this.thrusterSound = data.thrusterSound;
			this.followTarget = null;
			this.followPosition = false;
			this.isReturning = false;
			this.audioSource.clip = data.clip;
			this.audioSource.outputAudioMixerGroup = data.mixerGroup;
			this.audioSource.loop = data.loop;
			if (data.loop)
			{
				this.looping = true;
			}
			this.audioSource.playOnAwake = data.playOnAwake;
			this.audioSource.pitch = 1f;
			this.baseVolume = data.volume;
			this.maxCameraDistance = 90f;
			this.gameVolume = PersistentSingleton<SoundManager>.Instance.gameVolume;
			float num = this.baseVolume * this.gameVolume;
			if (this.thrusterSound)
			{
				float thrusterVolume = PersistentSingleton<SoundManager>.Instance.thrusterVolume;
				num *= thrusterVolume;
			}
			this.audioSource.volume = num;
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000CF9F8 File Offset: 0x000CDBF8
		public void WithRandomPitch(SoundData data)
		{
			this.audioSource.pitch += UnityEngine.Random.Range(data.minPitch, data.maxPitch);
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000CFA1D File Offset: 0x000CDC1D
		public void SetFollowTarget(Transform target)
		{
			this.followTarget = target;
			this.followPosition = (target != null);
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000CFA33 File Offset: 0x000CDC33
		private void ReturnToPool()
		{
			if (!this.isReturning)
			{
				this.isReturning = true;
			}
			PersistentSingleton<SoundManager>.Instance.ReturnToPool(this);
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000CFA4F File Offset: 0x000CDC4F
		public void ResetEmitterState()
		{
			this.looping = false;
			this.followTarget = null;
			this.followPosition = false;
			this.audioSource.panStereo = 0f;
			this.thrusterSound = false;
			base.StopAllCoroutines();
			this.ChangePowerVolume(1f);
		}

		// Token: 0x04001666 RID: 5734
		private AudioSource audioSource;

		// Token: 0x04001667 RID: 5735
		public float gameVolume = 1f;

		// Token: 0x04001668 RID: 5736
		public float baseVolume = 1f;

		// Token: 0x04001669 RID: 5737
		private Coroutine playingCoroutine;

		// Token: 0x0400166A RID: 5738
		private bool isReturning;

		// Token: 0x0400166B RID: 5739
		private Transform followTarget;

		// Token: 0x0400166C RID: 5740
		private bool followPosition;

		// Token: 0x0400166D RID: 5741
		private Camera gameCamera;

		// Token: 0x0400166E RID: 5742
		private float maxCameraDistance;

		// Token: 0x0400166F RID: 5743
		private float minZoomVolume;

		// Token: 0x04001671 RID: 5745
		private float powerVolume = 1f;

		// Token: 0x04001672 RID: 5746
		private bool thrusterSound;
	}
}
