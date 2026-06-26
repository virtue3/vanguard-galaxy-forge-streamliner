using System;
using System.Collections;
using Behaviour.Unit;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Util;
using UnityEngine;

namespace Behaviour.Persistables
{
	// Token: 0x020002F7 RID: 759
	public class MoveToArea : MonoBehaviour
	{
		// Token: 0x06001BB6 RID: 7094 RVA: 0x000A85BA File Offset: 0x000A67BA
		public void InitObject(MoveToAreaData moveToAreaData)
		{
			this.data = moveToAreaData;
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000A85C4 File Offset: 0x000A67C4
		private void Update()
		{
			float z = Mathf.Sin(Time.time * this.oscillationSpeed) * this.rotationAmount;
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000A8608 File Offset: 0x000A6808
		private void OnTriggerEnter2D(Collider2D collision)
		{
			SpaceShip component = collision.GetComponent<SpaceShip>();
			if (component != null && component == GameplayManager.Instance.spaceShip)
			{
				MissionObjective.Trigger(MissionTrigger.MoveToArea, null, null, false);
				MapPointOfInterest.current.RemovePersistable(this.data);
				if (this.fadeCoroutine == null)
				{
					this.fadeCoroutine = base.StartCoroutine(this.FadeAndDestroyCoroutine());
				}
			}
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000A866B File Offset: 0x000A686B
		private IEnumerator FadeAndDestroyCoroutine()
		{
			float elapsedTime = 0f;
			while (elapsedTime < 0.5f)
			{
				elapsedTime += Time.deltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / 0.5f);
				this.SetAlpha(alpha);
				yield return null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000A867A File Offset: 0x000A687A
		private void SetAlpha(float t)
		{
			this.moveIndicator.color = this.moveIndicator.color.WithAlpha(t);
		}

		// Token: 0x04001159 RID: 4441
		private MoveToAreaData data;

		// Token: 0x0400115A RID: 4442
		[SerializeField]
		private SpriteRenderer moveIndicator;

		// Token: 0x0400115B RID: 4443
		[SerializeField]
		private float rotationAmount = 15f;

		// Token: 0x0400115C RID: 4444
		[SerializeField]
		private float oscillationSpeed = 2f;

		// Token: 0x0400115D RID: 4445
		private Coroutine fadeCoroutine;
	}
}
