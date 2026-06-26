using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Simulation.World.Util
{
	// Token: 0x02000077 RID: 119
	public static class VoronoiCells
	{
		// Token: 0x06000464 RID: 1124 RVA: 0x00024340 File Offset: 0x00022540
		public static List<List<Vector2>> ComputeBoundedVoronoi(IReadOnlyList<Vector2> sites, Rect boundsExpanded)
		{
			List<List<Vector2>> list = new List<List<Vector2>>(sites.Count);
			List<Vector2> collection = new List<Vector2>
			{
				new Vector2(boundsExpanded.xMin, boundsExpanded.yMin),
				new Vector2(boundsExpanded.xMax, boundsExpanded.yMin),
				new Vector2(boundsExpanded.xMax, boundsExpanded.yMax),
				new Vector2(boundsExpanded.xMin, boundsExpanded.yMax)
			};
			for (int i = 0; i < sites.Count; i++)
			{
				List<Vector2> list2 = new List<Vector2>(collection);
				Vector2 vector = sites[i];
				for (int j = 0; j < sites.Count; j++)
				{
					if (j != i)
					{
						Vector2 vector2 = sites[j];
						Vector2 n = vector2 - vector;
						Vector2 m = (vector + vector2) * 0.5f;
						list2 = VoronoiCells.ClipPolygonByHalfPlane(list2, m, n);
						if (list2.Count == 0)
						{
							break;
						}
					}
				}
				list.Add(list2);
			}
			return list;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00024444 File Offset: 0x00022644
		private static List<Vector2> ClipPolygonByHalfPlane(List<Vector2> poly, Vector2 m, Vector2 n)
		{
			List<Vector2> list = new List<Vector2>(poly.Count);
			if (poly.Count == 0)
			{
				return list;
			}
			Vector2 a = poly[poly.Count - 1];
			float num = Vector2.Dot(a - m, n);
			for (int i = 0; i < poly.Count; i++)
			{
				Vector2 vector = poly[i];
				float num2 = Vector2.Dot(vector - m, n);
				bool flag = num <= 0f;
				bool flag2 = num2 <= 0f;
				if (flag && flag2)
				{
					list.Add(vector);
				}
				else if (flag && !flag2)
				{
					list.Add(VoronoiCells.LinePlaneIntersection(a, vector, m, n, num, num2));
				}
				else if (!flag && flag2)
				{
					list.Add(VoronoiCells.LinePlaneIntersection(a, vector, m, n, num, num2));
					list.Add(vector);
				}
				a = vector;
				num = num2;
			}
			return list;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0002452C File Offset: 0x0002272C
		private static Vector2 LinePlaneIntersection(Vector2 a, Vector2 b, Vector2 m, Vector2 n, float da, float db)
		{
			float d = da / (da - db);
			return a + d * (b - a);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00024558 File Offset: 0x00022758
		public static Rect ExpandedBounds(IReadOnlyList<Vector2> pts, float pad)
		{
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			float num3 = float.NegativeInfinity;
			float num4 = float.NegativeInfinity;
			foreach (Vector2 vector in pts)
			{
				num = Mathf.Min(num, vector.x);
				num2 = Mathf.Min(num2, vector.y);
				num3 = Mathf.Max(num3, vector.x);
				num4 = Mathf.Max(num4, vector.y);
			}
			return Rect.MinMaxRect(num - pad, num2 - pad, num3 + pad, num4 + pad);
		}
	}
}
