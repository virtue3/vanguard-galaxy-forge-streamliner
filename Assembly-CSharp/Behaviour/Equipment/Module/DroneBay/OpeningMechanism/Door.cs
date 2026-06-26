using System;
using System.Collections;
using System.Collections.Generic;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module.DroneBay.OpeningMechanism
{
	// Token: 0x02000370 RID: 880
	public class Door : MonoBehaviour
	{
		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000C4DFA File Offset: 0x000C2FFA
		// (set) Token: 0x060021FA RID: 8698 RVA: 0x000C4E02 File Offset: 0x000C3002
		public SpriteRenderer spriteRenderer { get; private set; }

		// Token: 0x060021FB RID: 8699 RVA: 0x000C4E0B File Offset: 0x000C300B
		public void Awake()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000C4E1C File Offset: 0x000C301C
		private void EnsureCloned()
		{
			if (this._originalSprite != null)
			{
				return;
			}
			if (this.spriteRenderer == null || this.spriteRenderer.sprite == null)
			{
				return;
			}
			this._originalSprite = this.spriteRenderer.sprite;
			Sprite sprite = SpriteHelper.CopySprite(this._originalSprite);
			if (sprite != null)
			{
				this.spriteRenderer.sprite = sprite;
			}
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000C4E8C File Offset: 0x000C308C
		public void ResetSprite()
		{
			if (this.spriteRenderer == null)
			{
				return;
			}
			if (this._originalSprite != null)
			{
				this.spriteRenderer.sprite = this._originalSprite;
			}
			this._originalSprite = null;
			this.EnsureCloned();
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000C4EC9 File Offset: 0x000C30C9
		public void ApplyDecals(List<DecalPlacement> placements)
		{
			if (placements == null || placements.Count == 0)
			{
				return;
			}
			this.EnsureCloned();
			this.BakeDecals(placements);
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000C4EE4 File Offset: 0x000C30E4
		private void BakeDecals(List<DecalPlacement> placements)
		{
			if (this.spriteRenderer == null || this.spriteRenderer.sprite == null)
			{
				return;
			}
			Sprite sprite = this.spriteRenderer.sprite;
			Texture2D texture = sprite.texture;
			Rect rect = sprite.rect;
			float pixelsPerUnit = sprite.pixelsPerUnit;
			float x = sprite.pivot.x;
			float y = sprite.pivot.y;
			bool flag = false;
			foreach (DecalPlacement decalPlacement in placements)
			{
				if (!string.IsNullOrEmpty(decalPlacement.decalId))
				{
					DecalDefinition decalDefinition = Decals.Get(decalPlacement.decalId);
					if (decalDefinition != null)
					{
						Sprite sprite2 = decalDefinition.GetSprite();
						if (!(sprite2 == null) && sprite2.texture.isReadable)
						{
							Texture2D texture2 = sprite2.texture;
							float scale = decalPlacement.scale;
							int num = Mathf.Max(1, Mathf.RoundToInt((float)texture2.width * scale));
							int num2 = Mathf.Max(1, Mathf.RoundToInt((float)texture2.height * scale));
							int num3 = Mathf.RoundToInt(rect.x + x + decalPlacement.position.x * pixelsPerUnit);
							int num4 = Mathf.RoundToInt(rect.y + y + decalPlacement.position.y * pixelsPerUnit);
							float num5 = (float)num / (float)texture2.width;
							float num6 = (float)num2 / (float)texture2.height;
							float num7 = sprite2.pivot.x * num5;
							float num8 = sprite2.pivot.y * num6;
							float f = decalPlacement.rotation * 0.0174532924f;
							float num9 = Mathf.Cos(f);
							float num10 = Mathf.Sin(f);
							int num11 = Mathf.CeilToInt((Mathf.Abs(num9) * (float)num + Mathf.Abs(num10) * (float)num2) * 0.5f) + 1;
							int num12 = Mathf.CeilToInt((Mathf.Abs(num10) * (float)num + Mathf.Abs(num9) * (float)num2) * 0.5f) + 1;
							for (int i = -num12; i <= num12; i++)
							{
								for (int j = -num11; j <= num11; j++)
								{
									float num13 = num9 * (float)j + num10 * (float)i;
									float num14 = -num10 * (float)j + num9 * (float)i;
									float num15 = num13 + num7;
									float num16 = num14 + num8;
									if (num15 >= 0f && num15 < (float)num && num16 >= 0f && num16 < (float)num2)
									{
										int x2 = Mathf.Clamp(Mathf.RoundToInt(num15 / (float)(num - 1) * (float)(texture2.width - 1)), 0, texture2.width - 1);
										int y2 = Mathf.Clamp(Mathf.RoundToInt(num16 / (float)(num2 - 1) * (float)(texture2.height - 1)), 0, texture2.height - 1);
										Color pixel = texture2.GetPixel(x2, y2);
										if (pixel.a >= 0.01f)
										{
											Color color = new Color(pixel.r * decalPlacement.color.r, pixel.g * decalPlacement.color.g, pixel.b * decalPlacement.color.b, pixel.a);
											int num17 = num3 + j;
											int num18 = num4 + i;
											if (num17 >= 0 && num17 < texture.width && num18 >= 0 && num18 < texture.height)
											{
												Color pixel2 = texture.GetPixel(num17, num18);
												if (pixel2.a >= 0.01f && num17 - 1 >= 0 && texture.GetPixel(num17 - 1, num18).a >= 0.01f && num17 + 1 < texture.width && texture.GetPixel(num17 + 1, num18).a >= 0.01f && num18 - 1 >= 0 && texture.GetPixel(num17, num18 - 1).a >= 0.01f && num18 + 1 < texture.height && texture.GetPixel(num17, num18 + 1).a >= 0.01f)
												{
													Color color2 = Color.Lerp(pixel2, color, color.a * decalPlacement.opacity);
													color2.a = pixel2.a;
													texture.SetPixel(num17, num18, color2);
													flag = true;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				texture.Apply();
			}
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000C5378 File Offset: 0x000C3578
		public bool IsPixelOnDoor(Vector2 doorLocalPos)
		{
			Sprite sprite = this.spriteRenderer ? this.spriteRenderer.sprite : null;
			if (sprite == null || !sprite.texture.isReadable)
			{
				return false;
			}
			Texture2D texture = sprite.texture;
			float pixelsPerUnit = sprite.pixelsPerUnit;
			int num = Mathf.RoundToInt(sprite.rect.x + sprite.pivot.x + doorLocalPos.x * pixelsPerUnit);
			int num2 = Mathf.RoundToInt(sprite.rect.y + sprite.pivot.y + doorLocalPos.y * pixelsPerUnit);
			return num >= 0 && num < texture.width && num2 >= 0 && num2 < texture.height && texture.GetPixel(num, num2).a > 0.01f;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000C5450 File Offset: 0x000C3650
		public bool TryBakePixel(Vector2 doorLocalPos, Color decalColor, float opacity)
		{
			this.EnsureCloned();
			if (this.spriteRenderer == null)
			{
				return false;
			}
			Sprite sprite = this.spriteRenderer.sprite;
			if (sprite == null || !sprite.texture.isReadable)
			{
				return false;
			}
			Texture2D texture = sprite.texture;
			float pixelsPerUnit = sprite.pixelsPerUnit;
			int num = Mathf.RoundToInt(sprite.rect.x + sprite.pivot.x + doorLocalPos.x * pixelsPerUnit);
			int num2 = Mathf.RoundToInt(sprite.rect.y + sprite.pivot.y + doorLocalPos.y * pixelsPerUnit);
			if (num < 0 || num >= texture.width || num2 < 0 || num2 >= texture.height)
			{
				return false;
			}
			Color pixel = texture.GetPixel(num, num2);
			if (pixel.a < 0.01f)
			{
				return false;
			}
			if (num - 1 < 0 || texture.GetPixel(num - 1, num2).a < 0.01f || num + 1 >= texture.width || texture.GetPixel(num + 1, num2).a < 0.01f || num2 - 1 < 0 || texture.GetPixel(num, num2 - 1).a < 0.01f || num2 + 1 >= texture.height || texture.GetPixel(num, num2 + 1).a < 0.01f)
			{
				return false;
			}
			Color color = Color.Lerp(pixel, decalColor, decalColor.a * opacity);
			color.a = pixel.a;
			texture.SetPixel(num, num2, color);
			return true;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000C55E6 File Offset: 0x000C37E6
		public void FlushTexture()
		{
			if (this.spriteRenderer && this.spriteRenderer.sprite)
			{
				this.spriteRenderer.sprite.texture.Apply();
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000C561C File Offset: 0x000C381C
		public void ToggleDoor(bool open, float speedModifier)
		{
			if (this.isMoving)
			{
				return;
			}
			this.speedModifier = speedModifier;
			base.StartCoroutine(this.MoveDoor(open ? this.openPosition : this.closePosition));
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000C564C File Offset: 0x000C384C
		private IEnumerator MoveDoor(Vector2 doorPosition)
		{
			this.isMoving = true;
			Vector3 targetPosition = new Vector3(doorPosition.x, doorPosition.y, base.transform.localPosition.z);
			float modifiedSpeed = this.moveSpeed * this.speedModifier;
			while (Vector3.Distance(base.transform.localPosition, targetPosition) > Mathf.Epsilon)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, targetPosition, modifiedSpeed * Time.deltaTime);
				yield return null;
			}
			base.transform.localPosition = targetPosition;
			this.isMoving = false;
			yield break;
		}

		// Token: 0x04001407 RID: 5127
		[SerializeField]
		private Vector2 openPosition;

		// Token: 0x04001408 RID: 5128
		[SerializeField]
		private Vector2 closePosition;

		// Token: 0x04001409 RID: 5129
		private readonly float moveSpeed = 1.5f;

		// Token: 0x0400140A RID: 5130
		public bool isMoving;

		// Token: 0x0400140B RID: 5131
		private float speedModifier = 1f;

		// Token: 0x0400140C RID: 5132
		private Sprite _originalSprite;
	}
}
