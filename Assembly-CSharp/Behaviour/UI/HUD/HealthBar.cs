using System;
using System.Collections.Generic;
using Behaviour.Unit;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000280 RID: 640
	public class HealthBar : MonoBehaviour
	{
		// Token: 0x06001756 RID: 5974 RVA: 0x0009365B File Offset: 0x0009185B
		private void Awake()
		{
			this.canvas = base.GetComponentInParent<Canvas>();
			this.rectTransform = (base.transform as RectTransform);
			this.gameCamera = GameplayManager.Instance.cameraMovement.gameCamera;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00093690 File Offset: 0x00091890
		private void CreateBlocks(RectTransform container, Image prefab, Image bgPrefab, int count, List<Image> list, List<Image> bgList, RectTransform bgContainer)
		{
			foreach (Image image in list)
			{
				UnityEngine.Object.Destroy(image.gameObject);
			}
			list.Clear();
			for (int i = 0; i < count; i++)
			{
				Image item = UnityEngine.Object.Instantiate<Image>(prefab, container);
				list.Add(item);
			}
			foreach (Image image2 in bgList)
			{
				UnityEngine.Object.Destroy(image2.gameObject);
			}
			bgList.Clear();
			for (int j = 0; j < count; j++)
			{
				Image item2 = UnityEngine.Object.Instantiate<Image>(bgPrefab, bgContainer);
				bgList.Add(item2);
			}
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0009376C File Offset: 0x0009196C
		private Color GetHealthColor(float ratio)
		{
			Color result;
			if (ratio > 0.66f)
			{
				result = Color.Lerp(new Color(0.8f, 0.8f, 0.2f), new Color(0.1f, 0.8f, 0.1f), (ratio - 0.66f) / 0.34f);
			}
			else if (ratio > 0.33f)
			{
				result = Color.Lerp(new Color(0.9f, 0.2f, 0.2f), new Color(0.8f, 0.8f, 0.2f), (ratio - 0.33f) / 0.33f);
			}
			else
			{
				result = new Color(0.9f, 0.2f, 0.2f);
			}
			result.a = 0.8f;
			return result;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00093828 File Offset: 0x00091A28
		public void Show(AbstractUnit unit)
		{
			float num = this.individualBarHeight;
			float x = unit.GetShipSize().x;
			float num2 = 0.4f;
			int num3 = Mathf.Clamp(Mathf.FloorToInt(x / num2), 3, 40);
			this.CreateBlocks(this.hpContainer, this.hpBlockPrefab, this.hpBlockBgPrefab, num3, this.hpBlocks, this.hpBlockBgs, this.hpBgContainer);
			if (unit.shieldGeneratorModule)
			{
				this.CreateBlocks(this.spContainer, this.hpBlockPrefab, this.spBlockBgPrefab, num3, this.shieldBlocks, this.shieldBgBlocks, this.spBgContainer);
			}
			if (unit.armorModule)
			{
				this.CreateBlocks(this.apContainer, this.hpBlockPrefab, this.apBlockBgPrefab, num3, this.armorBlocks, this.armorBgBlocks, this.apBgContainer);
			}
			if (unit.armorModule)
			{
				num += this.individualBarHeight;
			}
			if (unit.shieldGeneratorModule)
			{
				num += this.individualBarHeight;
				if (!unit.armorModule)
				{
					this.spContainer.anchoredPosition = new Vector2(this.spContainer.anchoredPosition.x, this.apContainer.anchoredPosition.y);
					this.spBgContainer.anchoredPosition = new Vector2(this.spContainer.anchoredPosition.x, this.apContainer.anchoredPosition.y);
				}
			}
			Vector2 sizeDelta = this.rectTransform.sizeDelta;
			sizeDelta.x = this.blockWidth * (float)num3;
			sizeDelta.y = num + 2f;
			this.rectTransform.sizeDelta = sizeDelta;
			this.unit = unit;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x000939CE File Offset: 0x00091BCE
		private void SetHolderColor(AbstractUnit unit)
		{
			GamePlayer.current.currentSpaceShip.IsEnemy(unit);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x000939E4 File Offset: 0x00091BE4
		private void LateUpdate()
		{
			if (!this.unit || this.unit == null)
			{
				return;
			}
			this.UpdateBlockBar(this.hpBlocks, this.unit.unitData.currentHullHP, this.unit.maxHullHP, null);
			this.UpdateBlockBar(this.shieldBlocks, this.unit.unitData.currentShieldHP, this.unit.maxShieldHP, new Color?(this.shieldColor));
			this.UpdateBlockBar(this.armorBlocks, this.unit.unitData.currentArmorHP, this.unit.maxArmorHP, new Color?(this.armorColor));
			Vector2 v = this.unit.transform.position;
			v.y += this.unit.GetBoundsY() / 2f;
			Vector2 a = this.gameCamera.WorldToScreenPoint(v);
			a.y += this.rectTransform.sizeDelta.y;
			((RectTransform)base.transform).anchoredPosition = a / this.canvas.scaleFactor;
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00093B28 File Offset: 0x00091D28
		private void UpdateBlockBar(List<Image> blocks, float current, float max, Color? overrideColor = null)
		{
			float num = current / max;
			float num2 = num * (float)blocks.Count;
			int num3 = Mathf.FloorToInt(num2);
			float num4 = num2 - (float)num3;
			Color color = overrideColor ?? this.GetHealthColor(num);
			for (int i = 0; i < blocks.Count; i++)
			{
				Image image = blocks[i];
				if (i < num3)
				{
					image.color = color;
					image.enabled = true;
				}
				else if (i == num3)
				{
					image.color = new Color(color.r, color.g, color.b, num4);
					image.enabled = (num4 > 0f);
				}
				else
				{
					image.enabled = false;
				}
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00093BDE File Offset: 0x00091DDE
		public void Destroy()
		{
			if (!this || !base.gameObject)
			{
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000E6A RID: 3690
		[SerializeField]
		private RectTransform hpContainer;

		// Token: 0x04000E6B RID: 3691
		[SerializeField]
		private RectTransform hpBgContainer;

		// Token: 0x04000E6C RID: 3692
		[SerializeField]
		private RectTransform spContainer;

		// Token: 0x04000E6D RID: 3693
		[SerializeField]
		private RectTransform spBgContainer;

		// Token: 0x04000E6E RID: 3694
		[SerializeField]
		private RectTransform apContainer;

		// Token: 0x04000E6F RID: 3695
		[SerializeField]
		private RectTransform apBgContainer;

		// Token: 0x04000E70 RID: 3696
		[SerializeField]
		private Image hpBlockPrefab;

		// Token: 0x04000E71 RID: 3697
		[SerializeField]
		private Image hpBlockBgPrefab;

		// Token: 0x04000E72 RID: 3698
		[SerializeField]
		private Image spBlockBgPrefab;

		// Token: 0x04000E73 RID: 3699
		[SerializeField]
		private Image apBlockBgPrefab;

		// Token: 0x04000E74 RID: 3700
		private AbstractUnit unit;

		// Token: 0x04000E75 RID: 3701
		private Canvas canvas;

		// Token: 0x04000E76 RID: 3702
		private RectTransform rectTransform;

		// Token: 0x04000E77 RID: 3703
		private Camera gameCamera;

		// Token: 0x04000E78 RID: 3704
		private float individualBarHeight = 8f;

		// Token: 0x04000E79 RID: 3705
		private float blockWidth = 10f;

		// Token: 0x04000E7A RID: 3706
		private List<Image> hpBlocks = new List<Image>();

		// Token: 0x04000E7B RID: 3707
		private List<Image> hpBlockBgs = new List<Image>();

		// Token: 0x04000E7C RID: 3708
		private List<Image> armorBlocks = new List<Image>();

		// Token: 0x04000E7D RID: 3709
		private List<Image> armorBgBlocks = new List<Image>();

		// Token: 0x04000E7E RID: 3710
		private List<Image> shieldBlocks = new List<Image>();

		// Token: 0x04000E7F RID: 3711
		private List<Image> shieldBgBlocks = new List<Image>();

		// Token: 0x04000E80 RID: 3712
		private Color shieldColor = ColorHelper.shieldColor;

		// Token: 0x04000E81 RID: 3713
		private Color armorColor = ColorHelper.armorColor;
	}
}
