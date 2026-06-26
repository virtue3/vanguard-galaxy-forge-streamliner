using System;
using System.Collections.Generic;
using Behaviour.UI;
using Behaviour.Util;
using Source.AudioSystem;
using Source.Player;
using UnityEngine;
using UnityEngine.Pool;

namespace Behaviour.AudioSystem
{
	// Token: 0x020003BE RID: 958
	public class SoundManager : PersistentSingleton<SoundManager>
	{
		// Token: 0x060024F0 RID: 9456 RVA: 0x000CFAC4 File Offset: 0x000CDCC4
		private void Start()
		{
			this.InitializePool();
			this.gameCamera = Camera.main.GetComponent<CameraMovement>().gameCamera;
			this.SetGameVolume();
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000CFAE7 File Offset: 0x000CDCE7
		public void SetGameVolume()
		{
			if (GameplayerPrefs.GetAudioMuted())
			{
				this.gameVolume = 0f;
			}
			else
			{
				this.gameVolume = GameplayerPrefs.GetSoundEffectVolume();
			}
			this.SetThrusterVolume();
			this.UpdateVolume();
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000CFB14 File Offset: 0x000CDD14
		public void SetThrusterVolume()
		{
			this.thrusterVolume = GameplayerPrefs.GetThrusterVolume() * this.gameVolume;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000CFB28 File Offset: 0x000CDD28
		private void UpdateVolume()
		{
			foreach (SoundEmitter soundEmitter in this.activeSoundEmitter)
			{
				soundEmitter.ChangeVolume(this.gameVolume);
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000CFB80 File Offset: 0x000CDD80
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) && UIHelper.IsMouseOverUi)
			{
				this.CreateSound().WithSoundData(this.mouseClickSoundData).WithRandomPitch().Play();
			}
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.UpdateActiveEmitters();
				this.updateTimer = this.updateRate;
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000CFBEC File Offset: 0x000CDDEC
		private void UpdateActiveEmitters()
		{
			foreach (SoundEmitter soundEmitter in this.activeSoundEmitter)
			{
				if (soundEmitter.looping)
				{
					soundEmitter.UpdateDynamic();
				}
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000CFC48 File Offset: 0x000CDE48
		public SoundBuilder CreateSound()
		{
			return new SoundBuilder(this);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000CFC50 File Offset: 0x000CDE50
		public SoundEmitter Get()
		{
			return this.soundEmitterPool.Get();
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000CFC60 File Offset: 0x000CDE60
		public void OnPause(bool pause)
		{
			foreach (SoundEmitter soundEmitter in this.activeSoundEmitter)
			{
				if (soundEmitter.looping)
				{
					if (pause)
					{
						soundEmitter.Pause();
					}
					if (!pause)
					{
						soundEmitter.Unpause();
					}
				}
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000CFCC8 File Offset: 0x000CDEC8
		public void ReturnToPool(SoundEmitter soundEmitter)
		{
			this.soundEmitterPool.Release(soundEmitter);
			soundEmitter.ResetEmitterState();
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000CFCDC File Offset: 0x000CDEDC
		public bool CanPlaySound(SoundData data)
		{
			if (!data.frequentSound)
			{
				return true;
			}
			SoundEmitter soundEmitter;
			if (this.FrequentSoundEmitters.Count >= this.maxSoundInstances && this.FrequentSoundEmitters.TryDequeue(out soundEmitter))
			{
				try
				{
					soundEmitter.Stop();
					return true;
				}
				catch
				{
					MonoBehaviour.print("SoundEmitter already released");
				}
				return false;
			}
			return true;
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000CFD44 File Offset: 0x000CDF44
		private SoundEmitter CreateSoundEmitter()
		{
			SoundEmitter soundEmitter = UnityEngine.Object.Instantiate<SoundEmitter>(this.soundEmitterPrefab, base.transform);
			soundEmitter.gameObject.SetActive(false);
			return soundEmitter;
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000CFD63 File Offset: 0x000CDF63
		private void OnTakeFromPool(SoundEmitter soundEmitter)
		{
			soundEmitter.gameObject.SetActive(true);
			this.activeSoundEmitter.Add(soundEmitter);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000CFD7D File Offset: 0x000CDF7D
		private void OnReturnedToPool(SoundEmitter soundEmitter)
		{
			soundEmitter.gameObject.SetActive(false);
			this.activeSoundEmitter.Remove(soundEmitter);
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000CFD98 File Offset: 0x000CDF98
		private void OnDestroyPoolObject(SoundEmitter soundEmitter)
		{
			if (soundEmitter)
			{
				UnityEngine.Object.Destroy(soundEmitter.gameObject);
			}
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000CFDB0 File Offset: 0x000CDFB0
		private void InitializePool()
		{
			this.soundEmitterPool = new ObjectPool<SoundEmitter>(new Func<SoundEmitter>(this.CreateSoundEmitter), new Action<SoundEmitter>(this.OnTakeFromPool), new Action<SoundEmitter>(this.OnReturnedToPool), new Action<SoundEmitter>(this.OnDestroyPoolObject), this.collectionCheck, this.defaultCapacity, this.maxPoolSize);
		}

		// Token: 0x04001673 RID: 5747
		private IObjectPool<SoundEmitter> soundEmitterPool;

		// Token: 0x04001674 RID: 5748
		private readonly List<SoundEmitter> activeSoundEmitter = new List<SoundEmitter>();

		// Token: 0x04001675 RID: 5749
		public readonly Queue<SoundEmitter> FrequentSoundEmitters = new Queue<SoundEmitter>();

		// Token: 0x04001676 RID: 5750
		[SerializeField]
		private SoundEmitter soundEmitterPrefab;

		// Token: 0x04001677 RID: 5751
		[SerializeField]
		private bool collectionCheck = true;

		// Token: 0x04001678 RID: 5752
		[SerializeField]
		private int defaultCapacity = 10;

		// Token: 0x04001679 RID: 5753
		[SerializeField]
		private int maxPoolSize = 100;

		// Token: 0x0400167A RID: 5754
		[SerializeField]
		private int maxSoundInstances = 30;

		// Token: 0x0400167B RID: 5755
		[SerializeField]
		private SoundData mouseClickSoundData;

		// Token: 0x0400167C RID: 5756
		public Camera gameCamera;

		// Token: 0x0400167D RID: 5757
		private float updateTimer;

		// Token: 0x0400167E RID: 5758
		private float updateRate = 0.1f;

		// Token: 0x0400167F RID: 5759
		public float gameVolume = 1f;

		// Token: 0x04001680 RID: 5760
		public float thrusterVolume = 1f;
	}
}
