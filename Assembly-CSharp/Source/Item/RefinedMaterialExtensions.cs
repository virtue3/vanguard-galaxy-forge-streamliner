using System;
using UnityEngine;

namespace Source.Item
{
	// Token: 0x020000F8 RID: 248
	public static class RefinedMaterialExtensions
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x0004784C File Offset: 0x00045A4C
		public static float GetValue(this RefinedMaterial self)
		{
			float num;
			switch (self)
			{
			case RefinedMaterial.Titanium:
				num = 1.6f;
				break;
			case RefinedMaterial.Oxide:
				num = 1.4f;
				break;
			case RefinedMaterial.Silicon:
				num = 1.8f;
				break;
			case RefinedMaterial.Tungsten:
				num = 2.4f;
				break;
			case RefinedMaterial.Carbon:
				num = 0.6f;
				break;
			case RefinedMaterial.Iridium:
				num = 3.4f;
				break;
			case RefinedMaterial.Platinum:
				num = 5.2f;
				break;
			case RefinedMaterial.Astatine:
				num = 6.2f;
				break;
			default:
				num = 0f;
				break;
			}
			return 100f * num;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000478D0 File Offset: 0x00045AD0
		public static Color GetColor(this RefinedMaterial self)
		{
			Color result;
			switch (self)
			{
			case RefinedMaterial.Titanium:
				result = new Color(0.09803922f, 0.5686275f, 0.7764707f);
				break;
			case RefinedMaterial.Oxide:
				result = new Color(0f, 0.7529413f, 0.6627451f);
				break;
			case RefinedMaterial.Silicon:
				result = new Color(0.4313726f, 0.3098039f, 0.2392157f);
				break;
			case RefinedMaterial.Tungsten:
				result = new Color(0.7607844f, 0.1921569f, 0.9725491f);
				break;
			case RefinedMaterial.Carbon:
				result = new Color(0.3215686f, 0.3254902f, 0.3411765f);
				break;
			case RefinedMaterial.Iridium:
				result = new Color(0.9058824f, 0.1803922f, 0.172549f);
				break;
			case RefinedMaterial.Platinum:
				result = new Color(0.6588235f, 0.6941177f, 0.7294118f);
				break;
			case RefinedMaterial.Astatine:
				result = new Color(0.1960784f, 0.854902f, 0.509804f);
				break;
			default:
				result = Color.white;
				break;
			}
			return result;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000479CD File Offset: 0x00045BCD
		public static Sprite GetIcon(this RefinedMaterial self)
		{
			return Resources.Load<Sprite>("Sprites/Refined/" + self.ToString());
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000479EB File Offset: 0x00045BEB
		public static string GetDisplayName(this RefinedMaterial self)
		{
			return self.ToString();
		}
	}
}
