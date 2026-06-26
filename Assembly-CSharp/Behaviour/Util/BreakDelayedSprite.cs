using System;
using System.Collections.Generic;
using Source.Mining;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B6 RID: 438
	public class BreakDelayedSprite
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000F53 RID: 3923 RVA: 0x0006A530 File Offset: 0x00068730
		// (remove) Token: 0x06000F54 RID: 3924 RVA: 0x0006A568 File Offset: 0x00068768
		private event Action _onComplete;

		// Token: 0x1700028E RID: 654
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0006A59D File Offset: 0x0006879D
		public Action onComplete
		{
			set
			{
				if (this.done)
				{
					this.UpdateSprites();
					value();
					return;
				}
				this._onComplete += value;
			}
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0006A5BD File Offset: 0x000687BD
		public void Initialize()
		{
			this.UpdateSprites();
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0006A5C5 File Offset: 0x000687C5
		public bool Update()
		{
			if (this.done)
			{
				this.UpdateSprites();
			}
			return this.done;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0006A5E0 File Offset: 0x000687E0
		private void UpdateSprites()
		{
			this.master.SetPixels(this.masterPixels);
			this.master.Apply();
			this.child.SetPixels(this.childPixels);
			this.child.Apply();
			this.childNormals.SetPixels(this.childNormalPixels);
			this.childNormals.Apply();
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0006A644 File Offset: 0x00068844
		public void RunJob()
		{
			SeededRandom seededRandom = new SeedGenerator().Add(this.seed ?? SeededRandom.Global.RandomString(16)).CreateRandom();
			int num = this.breakPoint.size;
			int width = this.master.width;
			int height = this.master.height;
			List<Vector2Int> list = new List<Vector2Int>(this.breakPoint.brokenPixels);
			if (list.Count == 0)
			{
				bool startFilled = this.masterPixels[this.breakPosition.y * width + this.breakPosition.x].a > 0.5f;
				Vector2Int edge = BreakDelayedSprite.FindEdge(this.breakPosition, this.masterPixels, width, height, startFilled);
				list = BreakDelayedSprite.GetBoxBetweenPoints(this.breakPosition, edge, this.masterPixels, width);
			}
			List<Vector2Int> list2 = new List<Vector2Int>();
			while (list.Count > 0 && num > 0)
			{
				Vector2Int item = list[0];
				list.RemoveAt(0);
				int num2 = item.y * width + item.x;
				if (this.masterPixels[num2].a > 0.5f)
				{
					list2.Add(item);
					this.childPixels[num2] = this.masterPixels[num2];
					this.childNormalPixels[num2] = this.masterNormalPixels[num2];
					this.masterPixels[num2] = Color.clear;
					num--;
				}
				if (list.Count < num)
				{
					for (int i = 0; i < BreakDelayedSprite.Directions.Length; i++)
					{
						int num3 = item.x + BreakDelayedSprite.Directions[i].x;
						int num4 = item.y + BreakDelayedSprite.Directions[i].y;
						if (num3 >= 0 && num3 < width && num4 >= 0 && num4 < height && this.masterPixels[num4 * width + num3].a > 0.5f)
						{
							list.Add(new Vector2Int(num3, num4));
						}
					}
					if (list.Count == 0)
					{
						break;
					}
					seededRandom.Shuffle<Vector2Int>(list);
				}
			}
			foreach (Vector2Int vector2Int in list2)
			{
				for (int j = 0; j < BreakDelayedSprite.Directions.Length; j++)
				{
					int num5 = vector2Int.x + BreakDelayedSprite.Directions[j].x;
					int num6 = (vector2Int.y + BreakDelayedSprite.Directions[j].y) * width + num5;
					if (num6 >= 0 && num6 < this.masterPixels.Length && this.masterPixels[num6].a > 0.5f)
					{
						this.masterPixels[num6] = new Color(this.masterPixels[num6].r / 3f, this.masterPixels[num6].g / 3f, this.masterPixels[num6].b / 3f, this.masterPixels[num6].a);
					}
				}
			}
			for (int k = 0; k < this.childPixels.Length; k++)
			{
				for (int l = 0; l < BreakDelayedSprite.Directions.Length; l++)
				{
					if (this.childPixels[k].a > 0.5f)
					{
						int num7 = k % width + BreakDelayedSprite.Directions[l].x;
						int num8 = (k / width + BreakDelayedSprite.Directions[l].y) * width + num7;
						if (num8 >= 0 && num8 < this.childPixels.Length && this.childPixels[num8].a < 0.5f)
						{
							this.childPixels[k] = new Color(this.childPixels[k].r / 3f, this.childPixels[k].g / 3f, this.childPixels[k].b / 3f, this.childPixels[k].a);
							break;
						}
					}
				}
			}
			this.done = true;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0006AAE8 File Offset: 0x00068CE8
		private static List<Vector2Int> GetBoxBetweenPoints(Vector2Int breakPosition, Vector2Int edge, Color[] pixels, int width)
		{
			int num = (breakPosition.x < edge.x) ? breakPosition.x : edge.x;
			int num2 = (breakPosition.x < edge.x) ? edge.x : breakPosition.x;
			int num3 = (breakPosition.y < edge.y) ? breakPosition.y : edge.y;
			int num4 = (breakPosition.y < edge.y) ? edge.y : breakPosition.y;
			List<Vector2Int> list = new List<Vector2Int>();
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					if (pixels[j * width + i].a > 0.5f)
					{
						list.Add(new Vector2Int(i, j));
					}
				}
			}
			return list;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0006ABC8 File Offset: 0x00068DC8
		private static Vector2Int FindEdge(Vector2Int start, Color[] pixels, int width, int height, bool startFilled)
		{
			int num = start.x;
			int num2 = start.y;
			int[] array = new int[4];
			array[0] = 1;
			array[2] = -1;
			int[] array2 = array;
			int[] array3 = new int[]
			{
				0,
				1,
				0,
				-1
			};
			int i = 1;
			while (i < Math.Max(width, height))
			{
				for (int j = 0; j < 4; j++)
				{
					for (int k = 0; k < i; k++)
					{
						num += array2[j];
						num2 += array3[j];
						if (num >= 0 && num < width && num2 >= 0 && num2 < height)
						{
							if (startFilled && pixels[num2 * width + num].a < 0.5f)
							{
								return new Vector2Int(num, num2);
							}
							if (!startFilled && pixels[num2 * width + num].a > 0.5f)
							{
								return new Vector2Int(num, num2);
							}
						}
					}
					if (j == 1 || j == 3)
					{
						i++;
					}
				}
			}
			return Vector2Int.zero;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0006ACB9 File Offset: 0x00068EB9
		public void OnComplete()
		{
			Action onComplete = this._onComplete;
			if (onComplete == null)
			{
				return;
			}
			onComplete();
		}

		// Token: 0x040008A9 RID: 2217
		public static Vector2Int[] Directions = new Vector2Int[]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};

		// Token: 0x040008AA RID: 2218
		public Texture2D master;

		// Token: 0x040008AB RID: 2219
		public Texture2D child;

		// Token: 0x040008AC RID: 2220
		public Texture2D childNormals;

		// Token: 0x040008AD RID: 2221
		public Sprite childSprite;

		// Token: 0x040008AE RID: 2222
		public PolygonCollider2D spawnedChunkCollider;

		// Token: 0x040008AF RID: 2223
		public Color[] masterPixels;

		// Token: 0x040008B0 RID: 2224
		public Color[] masterNormalPixels;

		// Token: 0x040008B1 RID: 2225
		public Color[] childPixels;

		// Token: 0x040008B2 RID: 2226
		public Color[] childNormalPixels;

		// Token: 0x040008B3 RID: 2227
		public Vector2Int breakPosition;

		// Token: 0x040008B4 RID: 2228
		public SpriteBreakPoint breakPoint;

		// Token: 0x040008B5 RID: 2229
		public string seed;

		// Token: 0x040008B6 RID: 2230
		public volatile bool done;
	}
}
