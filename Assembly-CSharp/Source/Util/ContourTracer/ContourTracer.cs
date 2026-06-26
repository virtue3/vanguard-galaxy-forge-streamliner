using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Util.ContourTracer
{
	// Token: 0x02000053 RID: 83
	public class ContourTracer
	{
		// Token: 0x17000050 RID: 80
		public int pathCount { get; private set; }

		// Token: 0x0600032C RID: 812
		public Vector2[] GetPath(int index)
		{
			return Array.ConvertAll<Vector2Int, Vector2>(this.pixelPaths[index].ToArray(), (Vector2Int point) => point * this.pointMultiplier - this.pointOffset);
		}

		// Token: 0x0600032D RID: 813
		public int GetPath(int index, ref List<Vector2> path)
		{
			Vector2Int[] points = this.pixelPaths[index].ToArray();
			int pointIndex = 0;
			if (points.Length > path.Count)
			{
				while (pointIndex < path.Count)
				{
					path[pointIndex] = points[pointIndex] * this.pointMultiplier - this.pointOffset;
					pointIndex++;
				}
				while (pointIndex < points.Length)
				{
					path.Add(points[pointIndex] * this.pointMultiplier - this.pointOffset);
					pointIndex++;
				}
			}
			else
			{
				while (pointIndex < points.Length)
				{
					path[pointIndex] = points[pointIndex] * this.pointMultiplier - this.pointOffset;
					pointIndex++;
				}
				path.RemoveRange(pointIndex, path.Count - points.Length);
			}
			return pointIndex;
		}

		// Token: 0x0600032E RID: 814
		public void Trace(Texture2D texture, Vector2 pivot, float pixelsPerUnit, uint gapLength, float product)
		{
			TraceState state;
			state.texture = texture;
			state.gapLength = gapLength;
			state.product = product;
			this.pathCount = 0;
			this.pointMultiplier = 1f / pixelsPerUnit;
			pivot.x *= (float)state.texture.width - 1f;
			pivot.y *= (float)state.texture.height - 1f;
			this.pointOffset = pivot * this.pointMultiplier;
			state.point = Vector2Int.zero;
			state.direction = Direction.Front;
			state.pixels = state.texture.GetPixels();
			state.found = new HashSet<Vector2Int>();
			bool flag = false;
			state.point.x = 0;
			while (state.point.x < state.texture.width)
			{
				state.point.y = 0;
				int num;
				while (state.point.y < state.texture.height)
				{
					if (state.found.Contains(state.point))
					{
						flag = true;
					}
					else
					{
						bool flag2 = N0(ref state);
						if (flag)
						{
							flag = flag2;
						}
						else if (flag2)
						{
							if (this.pathCount >= this.pixelPaths.Count)
							{
								this.pixelPaths.Add(new Stack<Vector2Int>());
							}
							else
							{
								this.pixelPaths[this.pathCount].Clear();
							}
							state.stack = this.pixelPaths[this.pathCount];
							Vector2Int point = state.point;
							Direction direction = state.direction;
							state.code = Code.InnerOuter;
							state.lastLineCode = Code.Straight;
							state.lineLength = 0f;
							state.maxLineLength = float.PositiveInfinity;
							state.lastDir = Vector2.zero;
							do
							{
								if (N1(ref state))
								{
									if (N2(ref state))
									{
										Encode(Code.Inner, ref state);
										Move(-1, -1, ref state);
										Turn(Direction.Rear, ref state);
									}
									else
									{
										Encode(Code.InnerOuter, ref state);
										Move(-1, -1, ref state);
										Turn(Direction.Rear, ref state);
									}
								}
								else if (N2(ref state))
								{
									Encode(Code.Straight, ref state);
									Move(-1, 0, ref state);
									Turn(Direction.Left, ref state);
								}
								else
								{
									Encode(Code.Outer, ref state);
								}
								if (N3(ref state))
								{
									if (N4(ref state))
									{
										Encode(Code.Inner, ref state);
										Move(-1, 1, ref state);
									}
									else
									{
										Encode(Code.InnerOuter, ref state);
										Move(-1, 1, ref state);
									}
								}
								else if (N4(ref state))
								{
									Encode(Code.Straight, ref state);
									Move(0, 1, ref state);
									Turn(Direction.Right, ref state);
								}
								else
								{
									Encode(Code.Outer, ref state);
									Turn(Direction.Rear, ref state);
								}
							}
							while (state.point != point || state.direction != direction);
							if (state.code == Code.Straight && state.lastLineCode == Code.Inner)
							{
								state.stack.Pop();
							}
							Smooth(ref state);
							if (state.stack.Count >= 3)
							{
								num = this.pathCount + 1;
								this.pathCount = num;
							}
							flag = true;
						}
					}
					num = state.point.y + 1;
					state.point.y = num;
				}
				num = state.point.x + 1;
				state.point.x = num;
			}
		}

		private static bool IsBorder(int x, int y, ref TraceState state)
		{
			int num = y * state.texture.width + x;
			return state.pixels[num].a > 0f;
		}

		private static bool IsBorderSafe(int x, int y, ref TraceState state)
		{
			return y >= 0 && y < state.texture.height && x >= 0 && x < state.texture.width && IsBorder(x, y, ref state);
		}

		private static void TurnPos(ref int x, ref int y, ref TraceState state)
		{
			switch (state.direction)
			{
			case Direction.Right:
			{
				int num = x;
				x = y;
				y = -num;
				return;
			}
			case Direction.Rear:
				x = -x;
				y = -y;
				return;
			case Direction.Left:
			{
				int num = x;
				x = -y;
				y = num;
				return;
			}
			default:
				return;
			}
		}

		private static bool NOffset(int x, int y, ref TraceState state)
		{
			TurnPos(ref x, ref y, ref state);
			return IsBorderSafe(state.point.x + x, state.point.y + y, ref state);
		}

		private static bool N0(ref TraceState state) => IsBorder(state.point.x, state.point.y, ref state);
		private static bool N1(ref TraceState state) => NOffset(-1, -1, ref state);
		private static bool N2(ref TraceState state) => NOffset(-1, 0, ref state);
		private static bool N3(ref TraceState state) => NOffset(-1, 1, ref state);
		private static bool N4(ref TraceState state) => NOffset(0, 1, ref state);

		private static void Encode(Code code, ref TraceState state)
		{
			switch (code)
			{
			case Code.Inner:
				if (state.code != Code.Outer)
				{
					if (state.code == Code.Straight)
					{
						if (state.lastLineCode == Code.Inner)
						{
							Vector2Int item = state.stack.Pop();
							Smooth(ref state);
							state.stack.Push(item);
							state.lastDir = Vector2.zero;
							state.stack.Push(state.point);
						}
						else if (state.lineLength >= state.maxLineLength)
						{
							state.stack.Push(state.point);
						}
					}
					else
					{
						state.stack.Push(state.point);
					}
				}
				state.maxLineLength = state.lineLength + state.gapLength;
				state.lineLength = 0f;
				break;
			case Code.InnerOuter:
				if (state.code != Code.InnerOuter)
				{
					state.stack.Push(state.point);
				}
				break;
			case Code.Straight:
			{
				if (state.code != Code.Straight)
				{
					state.lastLineCode = state.code;
					if (state.code == Code.Outer)
					{
						if (state.stack.Peek() == state.point)
						{
							break;
						}
						Smooth(ref state);
					}
					state.stack.Push(state.point);
				}
				state.lineLength += 1f;
				break;
			}
			case Code.Outer:
				if (state.code != Code.Inner)
				{
					if (state.code == Code.Straight)
					{
						if (state.lastLineCode != Code.Inner || state.lineLength > state.maxLineLength)
						{
							state.maxLineLength = float.PositiveInfinity;
							state.lastDir = Vector2.zero;
						}
						else
						{
							state.stack.Pop();
							Smooth(ref state);
						}
					}
					else if (state.code == Code.Outer)
					{
						if (state.stack.Peek() == state.point)
						{
							break;
						}
						Smooth(ref state);
						state.lastDir = Vector2.zero;
					}
					state.stack.Push(state.point);
					state.lineLength = float.PositiveInfinity;
				}
				break;
			}
			state.code = code;
		}

		private static void Move(int x, int y, ref TraceState state)
		{
			TurnPos(ref x, ref y, ref state);
			state.point.x = state.point.x + x;
			state.point.y = state.point.y + y;
			state.found.Add(state.point);
		}

		private static void Turn(Direction direction, ref TraceState state)
		{
			state.direction = (Direction)(((int)state.direction + (int)direction) % 4);
		}

		private static void Smooth(ref TraceState state)
		{
			Vector2 normalized = ((Vector2)state.point - (Vector2)state.stack.Peek()).normalized;
			if (Vector2.Dot(normalized, state.lastDir) > state.product)
			{
				state.stack.Pop();
			}
			state.lastDir = normalized;
		}

		// Token: 0x040001D6 RID: 470
		private List<Stack<Vector2Int>> pixelPaths = new List<Stack<Vector2Int>>();

		// Token: 0x040001D8 RID: 472
		private float pointMultiplier;

		// Token: 0x040001D9 RID: 473
		private Vector2 pointOffset;

		private struct TraceState
		{
			public Texture2D texture;
			public uint gapLength;
			public float product;
			public Vector2Int point;
			public Direction direction;
			public Color[] pixels;
			public HashSet<Vector2Int> found;
			public Stack<Vector2Int> stack;
			public Code code;
			public Code lastLineCode;
			public float lineLength;
			public float maxLineLength;
			public Vector2 lastDir;
		}

		// Token: 0x0200040E RID: 1038
		private enum Direction
		{
			Front,
			Right,
			Rear,
			Left
		}

		// Token: 0x0200040F RID: 1039
		private enum Code
		{
			Inner,
			InnerOuter,
			Straight,
			Outer
		}
	}
}
