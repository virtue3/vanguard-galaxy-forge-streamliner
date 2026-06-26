using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000045 RID: 69
	public class SpriteHelper
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x00018060 File Offset: 0x00016260
		public static Sprite CopySprite(Sprite sprite)
		{
			if (sprite && sprite.texture.isReadable)
			{
				Texture2D texture = sprite.texture;
				Texture2D texture2D = new Texture2D(texture.width, texture.height, texture.format, false);
				texture2D.filterMode = texture.filterMode;
				Graphics.CopyTexture(texture, texture2D);
				SecondarySpriteTexture[] array = new SecondarySpriteTexture[sprite.GetSecondaryTextureCount()];
				sprite.GetSecondaryTextures(array);
				if (array.Length != 0)
				{
					Texture2D texture2 = array[0].texture;
					Texture2D texture2D2 = new Texture2D(texture2.width, texture2.height, texture2.format, false);
					texture2D2.filterMode = texture2.filterMode;
					Graphics.CopyTexture(texture2, texture2D2);
					array[0] = new SecondarySpriteTexture
					{
						name = "_NormalMap",
						texture = texture2D2
					};
				}
				return Sprite.Create(texture2D, sprite.rect, sprite.pivot / new Vector2(sprite.rect.width, sprite.rect.height), sprite.pixelsPerUnit, 0U, SpriteMeshType.Tight, Vector4.zero, true, array);
			}
			Debug.LogWarning("Sprite texture is niet Read/Write, nog even instellen: " + (((sprite != null) ? sprite.name : null) ?? "NULL"));
			return null;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000181A4 File Offset: 0x000163A4
		public static Sprite CreateSpriteFromShader(Material material, int width, int height)
		{
			RenderTexture renderTexture = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);
			Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
			Graphics.SetRenderTarget(renderTexture);
			Graphics.Blit(texture2D, renderTexture, material, 0);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			texture2D.Apply();
			Sprite result = Sprite.Create(texture2D, new Rect(new Vector2(0f, 0f), new Vector2((float)renderTexture.width, (float)renderTexture.height)), new Vector2(0.5f, 0.5f));
			RenderTexture.active = null;
			renderTexture.Release();
			return result;
		}
	}
}
