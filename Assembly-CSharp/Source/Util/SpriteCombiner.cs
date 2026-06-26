using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000044 RID: 68
	public static class SpriteCombiner
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00017CEC File Offset: 0x00015EEC
		private static Material Mat
		{
			get
			{
				if (SpriteCombiner._mat == null)
				{
					Shader shader = Shader.Find("Hidden/BlueprintCombine");
					if (shader == null)
					{
						Debug.LogError("SpriteCombinerGPU: Shader 'Hidden/BlueprintCombine' not found.");
						return null;
					}
					SpriteCombiner._mat = new Material(shader)
					{
						hideFlags = HideFlags.HideAndDontSave
					};
				}
				return SpriteCombiner._mat;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00017D40 File Offset: 0x00015F40
		public static Sprite CombineSpritesGPU(Sprite background, Sprite overlay, float overlayScale = 0.5f, Color? overlayColor = null, float overlayBoost = 1.5f, float tintStrength = 0.65f)
		{
			if (background == null || overlay == null)
			{
				Debug.LogError("SpriteCombinerGPU: background and overlay must be non-null.");
				return null;
			}
			if (SpriteCombiner.Mat == null)
			{
				return null;
			}
			Texture2D texture = background.texture;
			Texture2D texture2 = overlay.texture;
			Rect rect = new Rect(0f, 0f, 96f, 96f);
			Rect textureRect = overlay.textureRect;
			int num = Mathf.RoundToInt(rect.width);
			int num2 = Mathf.RoundToInt(rect.height);
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			temporary.filterMode = FilterMode.Bilinear;
			temporary.wrapMode = TextureWrapMode.Clamp;
			Vector4 value = new Vector4(rect.x / (float)texture.width, rect.y / (float)texture.height, rect.width / (float)texture.width, rect.height / (float)texture.height);
			Vector4 value2 = new Vector4(textureRect.x / (float)texture2.width, textureRect.y / (float)texture2.height, textureRect.width / (float)texture2.width, textureRect.height / (float)texture2.height);
			Vector2 vector = new Vector2((float)num, (float)num2);
			Vector2 vector2 = new Vector2(textureRect.width, textureRect.height);
			Vector2 b = vector2 * overlayScale;
			Vector2 v = 0.5f * (vector - b);
			SpriteCombiner.Mat.SetTexture("_BgTex", texture);
			SpriteCombiner.Mat.SetTexture("_OvTex", texture2);
			SpriteCombiner.Mat.SetVector("_BgRect", value);
			SpriteCombiner.Mat.SetVector("_OvRect", value2);
			SpriteCombiner.Mat.SetVector("_OutSize", vector);
			SpriteCombiner.Mat.SetVector("_OvSizePx", vector2);
			SpriteCombiner.Mat.SetVector("_OvStartPx", v);
			SpriteCombiner.Mat.SetFloat("_Scale", overlayScale);
			SpriteCombiner.Mat.SetFloat("_BlueBoost", overlayBoost);
			SpriteCombiner.Mat.SetColor("_OverlayTintColor", overlayColor ?? SpriteCombiner.blueprintBlue);
			SpriteCombiner.Mat.SetFloat("_TintStrength", tintStrength);
			RenderTexture active = RenderTexture.active;
			Graphics.Blit(null, temporary, SpriteCombiner.Mat);
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, false, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)num, (float)num2), 0, 0, false);
			texture2D.Apply(false, false);
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
			Vector2 pivot = new Vector2(background.pivot.x / (float)num, background.pivot.y / (float)num2);
			Sprite result = Sprite.Create(texture2D, new Rect(0f, 0f, (float)num, (float)num2), pivot, background.pixelsPerUnit);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.filterMode = FilterMode.Bilinear;
			return result;
		}

		// Token: 0x0400018C RID: 396
		private static Color blueprintBlue = new Color(0.2f, 0.6f, 1f, 1f);

		// Token: 0x0400018D RID: 397
		private static Material _mat;
	}
}
