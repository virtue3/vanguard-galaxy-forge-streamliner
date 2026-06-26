using System;
using System.Collections.Generic;
using System.IO;
using Behaviour.Mining;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Mining;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000025 RID: 37
	public static class AsteroidHelper
	{
		// Token: 0x0600020A RID: 522 RVA: 0x0000C624 File Offset: 0x0000A824
		public static AsteroidSize GetRandomAsteroidSize(float wealth, List<AsteroidSize> excludeSizes)
		{
			float num = SeededRandom.Global.RandomFloat() * wealth;
			if (num < 0.1f && !excludeSizes.Contains(AsteroidSize.Tiny))
			{
				return AsteroidSize.Tiny;
			}
			if (num < 0.35f && !excludeSizes.Contains(AsteroidSize.Small))
			{
				return AsteroidSize.Small;
			}
			if (num < 0.75f && !excludeSizes.Contains(AsteroidSize.Medium))
			{
				return AsteroidSize.Medium;
			}
			if ((double)num < 0.95 && !excludeSizes.Contains(AsteroidSize.Large))
			{
				return AsteroidSize.Large;
			}
			return AsteroidSize.Huge;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000C690 File Offset: 0x0000A890
		private static Vector2 GetImpulseForChunk(Asteroid asteroid, Vector2 chunkPosition, float scoreRandomSize)
		{
			Vector2 vector = chunkPosition - asteroid.transform.position;
			return new Vector2((vector.x < 0f) ? UnityEngine.Random.Range(-scoreRandomSize, vector.x) : UnityEngine.Random.Range(vector.x, scoreRandomSize), (vector.y < 0f) ? UnityEngine.Random.Range(-scoreRandomSize, vector.y) : UnityEngine.Random.Range(vector.y, scoreRandomSize));
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000C70C File Offset: 0x0000A90C
		public static float GetScoreFromAsteroidSize(AsteroidSize size)
		{
			switch (size)
			{
			case AsteroidSize.Tiny:
				return 2f;
			case AsteroidSize.Small:
				return 10f;
			case AsteroidSize.Medium:
				return 20f;
			case AsteroidSize.Large:
				return 30f;
			case AsteroidSize.Huge:
				return 40f;
			default:
				return 0f;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000C758 File Offset: 0x0000A958
		public static float GetAsteroidScale(AsteroidSize size)
		{
			switch (size)
			{
			case AsteroidSize.Tiny:
				return UnityEngine.Random.Range(0.1f, 0.2f);
			case AsteroidSize.Small:
				return UnityEngine.Random.Range(0.3f, 0.5f);
			case AsteroidSize.Medium:
				return UnityEngine.Random.Range(0.7f, 0.9f);
			case AsteroidSize.Large:
				return UnityEngine.Random.Range(1.2f, 1.5f);
			case AsteroidSize.Huge:
				return UnityEngine.Random.Range(1.8f, 2f);
			default:
				return 0.5f;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000C7D6 File Offset: 0x0000A9D6
		public static int GetMaxDronesPerSize(this AsteroidSize size)
		{
			switch (size)
			{
			case AsteroidSize.Tiny:
				return 1;
			case AsteroidSize.Small:
				return 1;
			case AsteroidSize.Medium:
				return 2;
			case AsteroidSize.Large:
				return 3;
			case AsteroidSize.Huge:
				return 4;
			default:
				return 1;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000C7FF File Offset: 0x0000A9FF
		public static int GetSurfaceAmount(float scale)
		{
			return Mathf.CeilToInt(SeededRandom.Global.RandomRange(10f * scale, 20f * scale));
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000C81E File Offset: 0x0000AA1E
		public static int GetInnerCoreAmount(float scale)
		{
			return Mathf.CeilToInt(SeededRandom.Global.RandomRange(12f * scale, 22f * scale));
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000C840 File Offset: 0x0000AA40
		public static AsteroidSize GetAsteroidSizeForPixels(int pixels)
		{
			foreach (KeyValuePair<AsteroidSize, Vector2Int> keyValuePair in AsteroidHelper.AsteroidSizes)
			{
				int num = keyValuePair.Value.x * keyValuePair.Value.x;
				int num2 = keyValuePair.Value.y * keyValuePair.Value.y;
				if (pixels <= num2 && pixels >= num)
				{
					return keyValuePair.Key;
				}
			}
			return AsteroidSize.Tiny;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
		public static Sprite CreateAsteroidSprite(AsteroidSize size, OreItemData ore, string seed = null, float cutoff = 0.1f, Color? bodyColor = null)
		{
			SeededRandom seededRandom = new SeedGenerator().Add(seed ?? SeededRandom.Global.RandomString(16)).CreateRandom();
			Vector2Int vector2Int = AsteroidHelper.AsteroidSizes[size];
			int x = vector2Int.x;
			int y = vector2Int.y;
			float[,] array = new float[x, y];
			FastNoiseLite fastNoiseLite = new FastNoiseLite(1337);
			fastNoiseLite.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
			fastNoiseLite.SetSeed((int)seededRandom.RandomInt());
			FastNoiseLite fastNoiseLite2 = new FastNoiseLite(1337);
			fastNoiseLite2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
			fastNoiseLite2.SetSeed((int)seededRandom.RandomInt());
			Vector2 vector = new Vector2((float)(x / 2), (float)(y / 2));
			int i = 0;
			float num;
			do
			{
				num = (float)seededRandom.RandomRange(-100000, 100000);
				i++;
			}
			while (fastNoiseLite.GetNoise(num, 0f) < 0.8f && i < 1000);
			for (int j = 0; j < x; j++)
			{
				for (int k = 0; k < y; k++)
				{
					array[j, k] = fastNoiseLite.GetNoise(num + ((float)j - vector.x) * 1.5f, ((float)k - vector.y) * 1.5f) + fastNoiseLite2.GetNoise((float)(j * 3), (float)(k * 3)) / 2f;
				}
			}
			for (int l = 0; l < 4; l++)
			{
				for (int m = 0; m < x; m++)
				{
					array[m, l] *= (float)l * 0.2f;
					array[m, y - l - 1] *= (float)l * 0.2f;
				}
				for (int n = 0; n < y; n++)
				{
					array[l, n] *= (float)l * 0.2f;
					array[x - l - 1, n] *= (float)l * 0.2f;
				}
			}
			for (int num2 = 0; num2 < x; num2++)
			{
				for (int num3 = 0; num3 < y; num3++)
				{
					if (array[num2, num3] <= cutoff)
					{
						array[num2, num3] = 0f;
					}
				}
			}
			AsteroidHelper.RemoveOrphanRegions(array);
			Texture2D texture2D = new Texture2D(x, y, TextureFormat.ARGB32, false);
			texture2D.filterMode = FilterMode.Point;
			Color[] pixels = texture2D.GetPixels();
			Color color = bodyColor ?? new Color32(51, 49, 52, byte.MaxValue);
			for (int num4 = 0; num4 < x; num4++)
			{
				for (int num5 = 0; num5 < y; num5++)
				{
					if (array[num4, num5] > cutoff)
					{
						pixels[num5 * x + num4] = color;
					}
					else
					{
						pixels[num5 * x + num4] = Color.clear;
					}
				}
			}
			if (ore != null)
			{
				for (int num6 = 0; num6 < x; num6++)
				{
					for (int num7 = 0; num7 < y; num7++)
					{
						if (array[num6, num7] > 0.4f)
						{
							float num8 = Mathf.Abs(fastNoiseLite2.GetNoise((float)(100 + num6 * 4), (float)(num7 * 4)));
							float num9 = 0f;
							float noise = fastNoiseLite2.GetNoise((float)(200 + num6 * 8), (float)(num7 * 8));
							if (num8 < 0.06f)
							{
								num9 = num8;
							}
							else if (noise > 0.92f)
							{
								num9 = noise;
							}
							if (num9 > 0f)
							{
								pixels[num7 * x + num6] = new Color(ore.depositColor.r, ore.depositColor.g, ore.depositColor.b);
								array[num6, num7] += 0.1f;
								for (int num10 = -1; num10 < 2; num10++)
								{
									for (int num11 = -1; num11 < 2; num11++)
									{
										if (num10 == 0 || num11 == 0)
										{
											array[num6 + num10, num7 + num11] += 0.05f;
											int num12 = (num7 + num11) * x + num6 + num10;
											if (pixels[num12] == color)
											{
												pixels[num12] = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			for (int num13 = 0; num13 < x; num13++)
			{
				for (int num14 = 0; num14 < y; num14++)
				{
					int num15 = num14 * x + num13;
					if (pixels[num15] != Color.clear)
					{
						float num16 = seededRandom.RandomRange(0.6f, 1f) * (0.5f + array[num13, num14]);
						pixels[num15] = new Color(pixels[num15].r * num16, pixels[num15].g * num16, pixels[num15].b * num16);
					}
				}
			}
			for (int num17 = 0; num17 < pixels.Length; num17++)
			{
				for (i = 0; i < AsteroidHelper.Directions.Length; i++)
				{
					if (pixels[num17].a > 0.5f)
					{
						int num18 = num17 % x + AsteroidHelper.Directions[i].x;
						int num19 = (num17 / x + AsteroidHelper.Directions[i].y) * x + num18;
						if (num19 >= 0 && num19 < pixels.Length && pixels[num19].a < 0.5f)
						{
							pixels[num17] = new Color(pixels[num17].r * 0.75f, pixels[num17].g * 0.75f, pixels[num17].b * 0.75f, pixels[num17].a);
							break;
						}
					}
				}
			}
			texture2D.SetPixels(pixels);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(new Vector2(0f, 0f), new Vector2((float)x, (float)y)), new Vector2(0.5f, 0.5f), 32f, 0U, SpriteMeshType.Tight, Vector4.zero, true, new SecondarySpriteTexture[]
			{
				new SecondarySpriteTexture
				{
					name = "_NormalMap",
					texture = AsteroidHelper.CreateNormalMap(array)
				}
			});
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000CF84 File Offset: 0x0000B184
		private static void RemoveOrphanRegions(float[,] heightmap)
		{
			int length = heightmap.GetLength(0);
			int length2 = heightmap.GetLength(1);
			bool[,] array = new bool[length, length2];
			int[,] array2 = new int[,]
			{
				{
					0,
					1
				},
				{
					1,
					0
				},
				{
					0,
					-1
				},
				{
					-1,
					0
				}
			};
			List<List<ValueTuple<int, int>>> list = new List<List<ValueTuple<int, int>>>();
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					if (!array[i, j] && heightmap[i, j] > 0f)
					{
						List<ValueTuple<int, int>> list2 = new List<ValueTuple<int, int>>();
						Queue<ValueTuple<int, int>> queue = new Queue<ValueTuple<int, int>>();
						queue.Enqueue(new ValueTuple<int, int>(i, j));
						array[i, j] = true;
						while (queue.Count > 0)
						{
							ValueTuple<int, int> valueTuple = queue.Dequeue();
							int item = valueTuple.Item1;
							int item2 = valueTuple.Item2;
							list2.Add(new ValueTuple<int, int>(item, item2));
							for (int k = 0; k < 4; k++)
							{
								int num = item + array2[k, 0];
								int num2 = item2 + array2[k, 1];
								if (num >= 0 && num < length && num2 >= 0 && num2 < length2 && !array[num, num2] && heightmap[num, num2] > 0f)
								{
									queue.Enqueue(new ValueTuple<int, int>(num, num2));
									array[num, num2] = true;
								}
							}
						}
						list.Add(list2);
					}
				}
			}
			int num3 = 0;
			foreach (List<ValueTuple<int, int>> list3 in list)
			{
				num3 = Mathf.Max(num3, list3.Count);
			}
			foreach (List<ValueTuple<int, int>> list4 in list)
			{
				if (list4.Count < num3)
				{
					foreach (ValueTuple<int, int> valueTuple2 in list4)
					{
						int item3 = valueTuple2.Item1;
						int item4 = valueTuple2.Item2;
						heightmap[item3, item4] = 0f;
					}
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		private static Texture2D vectorsToRBG(Vector3[,] vectors)
		{
			int length = vectors.GetLength(0);
			int length2 = vectors.GetLength(1);
			Texture2D texture2D = new Texture2D(length, length2);
			Color[] pixels = texture2D.GetPixels();
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					float r = (vectors[i, j].x + 1f) / 2f;
					float g = (vectors[i, j].y + 1f) / 2f;
					float b = (vectors[i, j].z + 1f) / 2f;
					pixels[j * length + i] = new Color(r, g, b);
				}
			}
			texture2D.SetPixels(pixels);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		private static Texture2D CreateNormalMap(float[,] heightmap)
		{
			AsteroidHelper.Normalize(heightmap);
			int length = heightmap.GetLength(0);
			int length2 = heightmap.GetLength(1);
			Vector3[,] partialDerivatives = AsteroidHelper.getPartialDerivatives(heightmap);
			Vector3[,] array = new Vector3[length, length2];
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					array[i, j] = new Vector3(-partialDerivatives[i, j].x, -partialDerivatives[i, j].y, 1f).normalized;
				}
			}
			return AsteroidHelper.vectorsToRBG(array);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000D348 File Offset: 0x0000B548
		private static void Normalize(float[,] heightmap)
		{
			int length = heightmap.GetLength(0);
			int length2 = heightmap.GetLength(1);
			float num = 0f;
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					num = Mathf.Max(num, heightmap[i, j]);
				}
			}
			if (num < 1f)
			{
				float num2 = 1f / num;
				for (int k = 0; k < length; k++)
				{
					for (int l = 0; l < length2; l++)
					{
						heightmap[k, l] *= num2;
					}
				}
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		private static int clamp(int val, int min, int max)
		{
			return Math.Clamp(val, min, max);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		private static Vector3[,] getPartialDerivatives(float[,] heightmap)
		{
			int length = heightmap.GetLength(0);
			int length2 = heightmap.GetLength(1);
			Vector3[,] array = new Vector3[length, length2];
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					float num = heightmap[AsteroidHelper.clamp(i - 1, 0, length - 1), j];
					float num2 = heightmap[AsteroidHelper.clamp(i - 1, 0, length - 1), AsteroidHelper.clamp(j + 1, 0, length2 - 1)];
					float num3 = heightmap[i, AsteroidHelper.clamp(j + 1, 0, length2 - 1)];
					float num4 = heightmap[AsteroidHelper.clamp(i + 1, 0, length - 1), AsteroidHelper.clamp(j + 1, 0, length2 - 1)];
					float num5 = heightmap[AsteroidHelper.clamp(i + 1, 0, length - 1), j];
					float num6 = heightmap[AsteroidHelper.clamp(i + 1, 0, length - 1), AsteroidHelper.clamp(j - 1, 0, length2 - 1)];
					float num7 = heightmap[i, AsteroidHelper.clamp(j - 1, 0, length2 - 1)];
					float num8 = heightmap[AsteroidHelper.clamp(i - 1, 0, length - 1), AsteroidHelper.clamp(j - 1, 0, length2 - 1)];
					float x = num4 + 2f * num5 + num6 - (num2 + 2f * num + num8);
					float y = num6 + 2f * num7 + num8 - (num4 + 2f * num3 + num2);
					array[i, j] = new Vector3(x, y, 0f);
				}
			}
			return array;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000D564 File Offset: 0x0000B764
		public static int GetFilledPixelCount(Sprite asteroid)
		{
			int width = asteroid.texture.width;
			int height = asteroid.texture.height;
			Color[] pixels = asteroid.texture.GetPixels();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < pixels.Length; i++)
			{
				if (pixels[i].a >= 0.5f)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
			return num;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		public static BreakDelayedSprite BreakAsteroidSprite(Sprite sprite, Vector2Int breakPosition, SpriteBreakPoint breakPoint, string seed)
		{
			int width = sprite.texture.width;
			int height = sprite.texture.height;
			Color[] pixels = sprite.texture.GetPixels();
			Color[] childPixels = new Color[pixels.Length];
			SecondarySpriteTexture[] array = new SecondarySpriteTexture[1];
			sprite.GetSecondaryTextures(array);
			Color[] childNormalPixels = new Color[pixels.Length];
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
			texture2D.filterMode = FilterMode.Point;
			Texture2D texture2D2 = new Texture2D(width, height);
			BreakDelayedSprite breakDelayedSprite = new BreakDelayedSprite
			{
				master = sprite.texture,
				masterPixels = pixels,
				masterNormalPixels = array[0].texture.GetPixels(),
				child = texture2D,
				childPixels = childPixels,
				childNormals = texture2D2,
				childNormalPixels = childNormalPixels,
				breakPosition = breakPosition,
				breakPoint = breakPoint,
				seed = seed,
				childSprite = Sprite.Create(texture2D, new Rect(new Vector2(0f, 0f), new Vector2((float)width, (float)height)), new Vector2(0.5f, 0.5f), 32f, 0U, SpriteMeshType.Tight, Vector4.zero, true, new SecondarySpriteTexture[]
				{
					new SecondarySpriteTexture
					{
						name = "_NormalMap",
						texture = texture2D2
					}
				})
			};
			Singleton<SpriteUpdater>.Current.AddSprite(breakDelayedSprite);
			return breakDelayedSprite;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000D730 File Offset: 0x0000B930
		public static void CreateAsteroidTextures(Sprite sprite, AsteroidSize initialSize)
		{
			if (initialSize == AsteroidSize.Medium || initialSize == AsteroidSize.Large)
			{
				SecondarySpriteTexture[] array = new SecondarySpriteTexture[1];
				sprite.GetSecondaryTextures(array);
				SecondarySpriteTexture secondarySpriteTexture = array[0];
				string text = Application.dataPath + "/Resources/Background/Asteroids/";
				File.WriteAllBytes(string.Concat(new string[]
				{
					text,
					initialSize.ToString(),
					"_",
					AsteroidHelper.asteroidCount.ToString(),
					".png"
				}), sprite.texture.EncodeToPNG());
				File.WriteAllBytes(string.Concat(new string[]
				{
					text,
					initialSize.ToString(),
					"_",
					AsteroidHelper.asteroidCount.ToString(),
					"_n.png"
				}), secondarySpriteTexture.texture.EncodeToPNG());
				AsteroidHelper.asteroidCount++;
				Debug.Log("Writing asteroid " + initialSize.ToString() + ", count: " + AsteroidHelper.asteroidCount.ToString());
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000D840 File Offset: 0x0000BA40
		public static GameObject SetTrackingTargetData(Collider2D collider, DamageData newDamageData, string name, Transform transform)
		{
			GameObject gameObject = new GameObject("TrackingTarget(Source: " + name + ")");
			gameObject.transform.parent = collider.transform;
			gameObject.transform.position = collider.ClosestPoint(transform.position);
			newDamageData.hitCoordinates = gameObject.transform.position;
			newDamageData.CreateHitTransform(collider.transform);
			return gameObject;
		}

		// Token: 0x040000F0 RID: 240
		public static int asteroidCount = 31;

		// Token: 0x040000F1 RID: 241
		public const float HeightCutoff = 0.1f;

		// Token: 0x040000F2 RID: 242
		public static Vector2Int[] Directions = new Vector2Int[]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};

		// Token: 0x040000F3 RID: 243
		public static Dictionary<AsteroidSize, Vector2Int> AsteroidSizes = new Dictionary<AsteroidSize, Vector2Int>
		{
			{
				AsteroidSize.Tiny,
				new Vector2Int(10, 18)
			},
			{
				AsteroidSize.Small,
				new Vector2Int(18, 36)
			},
			{
				AsteroidSize.Medium,
				new Vector2Int(36, 64)
			},
			{
				AsteroidSize.Large,
				new Vector2Int(64, 88)
			},
			{
				AsteroidSize.Huge,
				new Vector2Int(88, 128)
			}
		};
	}
}
