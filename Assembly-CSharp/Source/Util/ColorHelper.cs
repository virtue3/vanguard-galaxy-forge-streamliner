using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000029 RID: 41
	public static class ColorHelper
	{
		// Token: 0x06000229 RID: 553 RVA: 0x0000DB40 File Offset: 0x0000BD40
		public static Color GetEnergyColor(float fill)
		{
			if (fill <= 0.25f)
			{
				return ColorHelper.energyGreen;
			}
			if (fill <= 0.375f)
			{
				return ColorHelper.energyGreenGray;
			}
			if (fill <= 0.5f)
			{
				return ColorHelper.energyGrayGreen;
			}
			if (fill <= 0.625f)
			{
				return ColorHelper.energyLightRed;
			}
			if (fill <= 0.75f)
			{
				return ColorHelper.energyRed;
			}
			return ColorHelper.energyDarkRed;
		}

		// Token: 0x040000FA RID: 250
		public static readonly Color combatRed = new Color(1f, 0.25f, 0.25f);

		// Token: 0x040000FB RID: 251
		public static readonly Color miningBlue = new Color(0.5f, 0.5f, 1f);

		// Token: 0x040000FC RID: 252
		public static readonly Color salvageYellow = new Color(1f, 0.85f, 0f);

		// Token: 0x040000FD RID: 253
		public static readonly Color reddish = new Color(0.8207547f, 0.2593894f, 0.2593894f);

		// Token: 0x040000FE RID: 254
		public static readonly Color red90 = new Color(0.907547f, 0.1093894f, 0.1093894f);

		// Token: 0x040000FF RID: 255
		public static readonly Color greenish = new Color(0.2593894f, 0.8207547f, 0.2593894f);

		// Token: 0x04000100 RID: 256
		public static readonly Color healthbarAlpha = new Color(0.15f, 0.21f, 0.15f, 0.7f);

		// Token: 0x04000101 RID: 257
		public static readonly Color green50 = new Color(0.1f, 0.5f, 0.1f);

		// Token: 0x04000102 RID: 258
		public static readonly Color skilltreeMaxRank = new Color(0.5607843f, 0.4039216f, 0.1411765f);

		// Token: 0x04000103 RID: 259
		public static readonly Color boringGrey = new Color(0.415f, 0.415f, 0.415f);

		// Token: 0x04000104 RID: 260
		public static readonly Color fadedGrey = new Color(0.615f, 0.615f, 0.615f);

		// Token: 0x04000105 RID: 261
		public static readonly Color offWhite = new Color(0.915f, 0.915f, 0.915f);

		// Token: 0x04000106 RID: 262
		public static readonly Color skilltreeNoRank = new Color(0.03137255f, 0.2705882f, 0.3960785f);

		// Token: 0x04000107 RID: 263
		public static readonly Color skilltreeBorderNoRank = new Color(0.4622642f, 0.3129441f, 0.2289516f);

		// Token: 0x04000108 RID: 264
		public static readonly Color skilltreeBorderRank = new Color(0.1689213f, 0.2784174f, 0.490566f);

		// Token: 0x04000109 RID: 265
		public static readonly Color skilltreeBorderMaxRank = new Color(0.0509804f, 0.2156863f, 0.4784314f);

		// Token: 0x0400010A RID: 266
		public static readonly Color skilltreeBorderLockedRed = new Color(0.49f, 0.17f, 0.17f);

		// Token: 0x0400010B RID: 267
		public static readonly Color noCreditsColor = new Color(0.8207547f, 0.2593894f, 0.2593894f);

		// Token: 0x0400010C RID: 268
		public static readonly Color creditsColor = new Color(1f, 0.8196079f, 0.4666667f);

		// Token: 0x0400010D RID: 269
		public static readonly Color purple = new Color(0.45f, 0.1f, 0.5f);

		// Token: 0x0400010E RID: 270
		public static readonly Color purpleBadge = new Color(0.9f, 0.7f, 1f);

		// Token: 0x0400010F RID: 271
		public static readonly Color green05 = new Color(0.2f, 0.55f, 0.2f);

		// Token: 0x04000110 RID: 272
		public static readonly Color greenBadge = new Color(0.6f, 1f, 0.55f);

		// Token: 0x04000111 RID: 273
		public static readonly Color orange75 = new Color(0.75f, 0.6f, 0.2f);

		// Token: 0x04000112 RID: 274
		public static readonly Color orangeBadge = new Color(1f, 0.8f, 0.2f);

		// Token: 0x04000113 RID: 275
		public static readonly Color32 filthyGreenOnlyUsedForCameraBackground = new Color32(5, 13, 9, byte.MaxValue);

		// Token: 0x04000114 RID: 276
		public static readonly Color purpleBlueish = new Color(0.49f, 0.53f, 0.75f);

		// Token: 0x04000115 RID: 277
		public static readonly Color beige = new Color(0.96f, 0.92f, 0.8f);

		// Token: 0x04000116 RID: 278
		public static readonly Color softYellow = new Color(1f, 0.95f, 0.7f);

		// Token: 0x04000117 RID: 279
		public static readonly Color lightCyan = new Color(0.69f, 1f, 1f);

		// Token: 0x04000118 RID: 280
		public static readonly Color repGreen = new Color(0.388f, 0.849f, 0.388f);

		// Token: 0x04000119 RID: 281
		public static readonly Color repRed = new Color(0.85f, 0.388f, 0.406f);

		// Token: 0x0400011A RID: 282
		public static readonly Color grey = new Color(0.6698f, 0.6698f, 0.6698f);

		// Token: 0x0400011B RID: 283
		public static readonly Color RarityStandard = new Color32(230, 230, 230, byte.MaxValue);

		// Token: 0x0400011C RID: 284
		public static readonly Color RarityEnhanced = new Color32(30, byte.MaxValue, 0, byte.MaxValue);

		// Token: 0x0400011D RID: 285
		public static readonly Color RarityHighGrade = new Color32(0, 112, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400011E RID: 286
		public static readonly Color RarityExotic = new Color32(163, 53, 238, byte.MaxValue);

		// Token: 0x0400011F RID: 287
		public static readonly Color RarityLegendary = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);

		// Token: 0x04000120 RID: 288
		public static readonly Color RaritySpecial = new Color32(216, 206, 156, byte.MaxValue);

		// Token: 0x04000121 RID: 289
		public static readonly Color RarityBackgroundMultiplier = new Color(0.1f, 0.1f, 0.1f, 0.97f);

		// Token: 0x04000122 RID: 290
		public static readonly Color RarityStandardBackground = ColorHelper.RarityStandard * ColorHelper.RarityBackgroundMultiplier;

		// Token: 0x04000123 RID: 291
		public static readonly Color RarityEnhancedBackground = ColorHelper.RarityEnhanced * ColorHelper.RarityBackgroundMultiplier;

		// Token: 0x04000124 RID: 292
		public static readonly Color RarityHighGradeBackground = ColorHelper.RarityHighGrade * ColorHelper.RarityBackgroundMultiplier;

		// Token: 0x04000125 RID: 293
		public static readonly Color RarityExoticBackground = ColorHelper.RarityExotic * ColorHelper.RarityBackgroundMultiplier;

		// Token: 0x04000126 RID: 294
		public static readonly Color RarityLegendaryBackground = ColorHelper.RarityLegendary * ColorHelper.RarityBackgroundMultiplier;

		// Token: 0x04000127 RID: 295
		public static readonly Color RaritySpecialBackground = ColorHelper.RaritySpecial * ColorHelper.RarityBackgroundMultiplier;

		// Token: 0x04000128 RID: 296
		public static readonly Color detailsColor = new Color(1f, 0.8920743f, 0.6650944f);

		// Token: 0x04000129 RID: 297
		public static readonly Color modifierColor = new Color(0.7972299f, 0.5471698f, 0.3639196f);

		// Token: 0x0400012A RID: 298
		public static readonly Color middleBlue = new Color(0.27f, 0.32f, 0.47f);

		// Token: 0x0400012B RID: 299
		public static readonly Color white75 = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x0400012C RID: 300
		public static readonly Color discBlue = new Color(0.345098f, 0.3960785f, 0.9490197f);

		// Token: 0x0400012D RID: 301
		public static readonly Color discBlueLight1 = new Color(0.545f, 0.596f, 0.975f);

		// Token: 0x0400012E RID: 302
		public static readonly Color discBlueLight2 = new Color(0.675f, 0.725f, 0.985f);

		// Token: 0x0400012F RID: 303
		public static readonly Color steamBlue = new Color(0.145f, 0.396f, 0.678f);

		// Token: 0x04000130 RID: 304
		public static readonly Color steamBlueLight1 = new Color(0.275f, 0.525f, 0.785f);

		// Token: 0x04000131 RID: 305
		public static readonly Color steamBlueLight2 = new Color(0.375f, 0.625f, 0.865f);

		// Token: 0x04000132 RID: 306
		public static readonly Color brgBeige = new Color(0.8784314f, 0.8000001f, 0.6901961f);

		// Token: 0x04000133 RID: 307
		public static readonly Color brgBeigeLight = new Color(0.94f, 0.88f, 0.78f);

		// Token: 0x04000134 RID: 308
		public static readonly Color energyGreen = new Color(0.6f, 1f, 0.6f);

		// Token: 0x04000135 RID: 309
		public static readonly Color energyGreenGray = new Color(0.75f, 1f, 0.75f);

		// Token: 0x04000136 RID: 310
		public static readonly Color energyGrayGreen = new Color(0.85f, 0.9f, 0.85f);

		// Token: 0x04000137 RID: 311
		public static readonly Color energyLightRed = new Color(1f, 0.6f, 0.6f);

		// Token: 0x04000138 RID: 312
		public static readonly Color energyRed = new Color(1f, 0.4f, 0.4f);

		// Token: 0x04000139 RID: 313
		public static readonly Color energyDarkRed = new Color(0.6f, 0.2f, 0.2f);

		// Token: 0x0400013A RID: 314
		public static readonly Color buffBorder = new Color(0.4f, 0.85f, 1f);

		// Token: 0x0400013B RID: 315
		public static readonly Color debuffBorder = new Color(1f, 0.25f, 0.25f);

		// Token: 0x0400013C RID: 316
		public static readonly Color toggleAbilityBorder = new Color(0.85f, 0.6f, 1f);

		// Token: 0x0400013D RID: 317
		public static readonly Color flashExplosionMissile = new Color(1f, 0.5f, 0.3f);

		// Token: 0x0400013E RID: 318
		public static readonly Color flashExplosionUnit = new Color(0.04f, 0.53f, 0.75f);

		// Token: 0x0400013F RID: 319
		public static readonly Color shieldColor = new Color(0.185f, 0.48f, 0.85f, 1f);

		// Token: 0x04000140 RID: 320
		public static readonly Color armorColor = new Color(0.962f, 0.701f, 0.331f, 1f);

		// Token: 0x04000141 RID: 321
		public static readonly Color umbralColor = new Color32(173, 119, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000142 RID: 322
		public static readonly Color gold = new Color(1f, 0.84f, 0f);

		// Token: 0x04000143 RID: 323
		public static readonly Color silver = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x04000144 RID: 324
		public static readonly Color highlightColor = new Color32(byte.MaxValue, 209, 0, byte.MaxValue);

		// Token: 0x04000145 RID: 325
		public static readonly Color etaColorHigh = new Color32(229, 48, 32, byte.MaxValue);

		// Token: 0x04000146 RID: 326
		public static readonly Color etaColorMedium = new Color32(219, 107, 47, byte.MaxValue);

		// Token: 0x04000147 RID: 327
		public static readonly Color etaColorLow = new Color32(37, 229, 31, byte.MaxValue);

		// Token: 0x04000148 RID: 328
		public static readonly Color refineryJob = new Color32(139, 90, 43, byte.MaxValue);

		// Token: 0x04000149 RID: 329
		public static readonly Color forgeJob = new Color32(192, 192, 200, byte.MaxValue);

		// Token: 0x0400014A RID: 330
		public static readonly Color refineryJobBackground = new Color(0.15f, 0.09f, 0.04f);

		// Token: 0x0400014B RID: 331
		public static readonly Color forgeJobBackground = new Color(0.2f, 0.2f, 0.2f);
	}
}
