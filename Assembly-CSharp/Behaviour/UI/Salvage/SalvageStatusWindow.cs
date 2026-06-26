using System;
using System.Collections;
using Behaviour.Salvage;
using Source.Data.Persistable;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Salvage
{
	// Token: 0x02000247 RID: 583
	public class SalvageStatusWindow : MonoBehaviour
	{
		// Token: 0x0600159C RID: 5532 RVA: 0x0008A461 File Offset: 0x00088661
		private void Awake()
		{
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0008A464 File Offset: 0x00088664
		private void Update()
		{
			if (!this.CheckActive() || !this.world)
			{
				return;
			}
			Camera gameCamera = GameplayManager.Instance.cameraMovement.gameCamera;
			Vector2 v = this.world.transform.position;
			v.y += 1.5f;
			((RectTransform)base.transform).anchoredPosition = gameCamera.WorldToScreenPoint(v) / this.label.canvas.scaleFactor;
			if (this.activeItem.extracted)
			{
				this.statusText.TL(this.activeItem.extractionSuccessful ? "@SalvageExtractSuccess" : "@SalvageExtractFail", Array.Empty<object>());
				this.statusText.color = (this.activeItem.extractionSuccessful ? ColorHelper.greenish : ColorHelper.reddish);
				this.progressHolder.gameObject.SetActive(false);
				this.statusText.gameObject.SetActive(true);
				base.StartCoroutine(this.ClearStatus());
				return;
			}
			this.progress.transform.localScale = new Vector3((float)this.data.damageTaken / (float)this.data.healthPerItem, 1f, 1f);
			this.progressHolder.gameObject.SetActive(true);
			this.statusText.gameObject.SetActive(false);
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0008A5D7 File Offset: 0x000887D7
		public void SetSalvageData(SalvageContainer inWorld, SalvageData data)
		{
			this.data = data;
			this.world = inWorld;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0008A5E8 File Offset: 0x000887E8
		private bool CheckActive()
		{
			SalvageItemData salvageItemData = this.data.activeItem;
			if (this.activeItem != null && salvageItemData == null)
			{
				return true;
			}
			bool flag = this.activeItem != salvageItemData;
			this.activeItem = salvageItemData;
			if (flag)
			{
				this.label.text = Translation.Highlight(Translation.TranslateOnly("@SalvageExtractText", new object[]
				{
					this.activeItem.item.displayName
				}), this.activeItem.item.rarity.GetColor(), Array.Empty<object>());
			}
			if (this.activeItem == null || !this.world)
			{
				this.Hide();
				return false;
			}
			return true;
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0008A68E File Offset: 0x0008888E
		public void Hide()
		{
			this.activeItem = null;
			this.world = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0008A6AA File Offset: 0x000888AA
		private IEnumerator ClearStatus()
		{
			yield return new WaitForSeconds(10f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000CDC RID: 3292
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000CDD RID: 3293
		[SerializeField]
		private TMP_Text statusText;

		// Token: 0x04000CDE RID: 3294
		[SerializeField]
		private RectTransform progressHolder;

		// Token: 0x04000CDF RID: 3295
		[SerializeField]
		private RectTransform progress;

		// Token: 0x04000CE0 RID: 3296
		private SalvageContainer world;

		// Token: 0x04000CE1 RID: 3297
		private SalvageData data;

		// Token: 0x04000CE2 RID: 3298
		private SalvageItemData activeItem;
	}
}
