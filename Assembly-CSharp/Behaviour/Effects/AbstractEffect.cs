using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Behaviour.Effects
{
	// Token: 0x0200037E RID: 894
	public class AbstractEffect : MonoBehaviour
	{
		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06002268 RID: 8808 RVA: 0x000C73AA File Offset: 0x000C55AA
		// (set) Token: 0x06002269 RID: 8809 RVA: 0x000C73B2 File Offset: 0x000C55B2
		public VisualEffect visualEffect { get; protected set; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x000C73BB File Offset: 0x000C55BB
		// (set) Token: 0x0600226B RID: 8811 RVA: 0x000C73C3 File Offset: 0x000C55C3
		public bool canBeDestroyed { get; protected set; }

		// Token: 0x0600226C RID: 8812 RVA: 0x000C73CC File Offset: 0x000C55CC
		protected virtual void Awake()
		{
			this.visualEffect = base.GetComponent<VisualEffect>();
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000C73DC File Offset: 0x000C55DC
		protected virtual void Update()
		{
			this.playTime += Time.deltaTime;
			if (this.playing && this.maxPlayTime > 0f && this.maxPlayTime < this.playTime)
			{
				this.Stop();
				this.canBeDestroyed = true;
			}
			if (this.playing && this.followTransform)
			{
				base.transform.position = this.followTransform.position;
			}
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000C7456 File Offset: 0x000C5656
		public virtual void Play()
		{
			this.Play(0f);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000C7463 File Offset: 0x000C5663
		public virtual void Play(float delay)
		{
			if (delay > 0f)
			{
				this.Stop();
				base.StartCoroutine(this.DelayedPlay(delay));
				return;
			}
			this.StartPlaying();
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000C7488 File Offset: 0x000C5688
		protected void StartPlaying()
		{
			this.playing = true;
			this.visualEffect.Play();
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000C749C File Offset: 0x000C569C
		protected IEnumerator DelayedPlay(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.StartPlaying();
			yield break;
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000C74B2 File Offset: 0x000C56B2
		public virtual void Stop()
		{
			this.playing = false;
			if (this.visualEffect)
			{
				this.visualEffect.Stop();
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000C74D3 File Offset: 0x000C56D3
		public bool isPlaying()
		{
			return this.playing;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000C74DB File Offset: 0x000C56DB
		public void SetFollowTransform(Transform transform)
		{
			this.followTransform = transform;
		}

		// Token: 0x04001457 RID: 5207
		protected bool playing;

		// Token: 0x04001459 RID: 5209
		protected float playTime;

		// Token: 0x0400145A RID: 5210
		public float maxPlayTime;

		// Token: 0x0400145B RID: 5211
		private Transform followTransform;
	}
}
