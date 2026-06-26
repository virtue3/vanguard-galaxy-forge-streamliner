using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.GalaxyMap.Util
{
	// Token: 0x0200033A RID: 826
	public static class PolygonMeshBuilder
	{
		// Token: 0x06001F3F RID: 7999 RVA: 0x000BA3B8 File Offset: 0x000B85B8
		public static Mesh BuildConvexPolygonMesh(IReadOnlyList<Vector2> poly, float y = 0f, bool useXZPlane = true)
		{
			if (poly == null || poly.Count < 3)
			{
				return null;
			}
			int count = poly.Count;
			Vector3[] array = new Vector3[count];
			Vector2[] array2 = new Vector2[count];
			Rect rect = PolygonMeshBuilder.ComputeAabb(poly);
			float num = (rect.width > 1E-06f) ? (1f / rect.width) : 0f;
			float num2 = (rect.height > 1E-06f) ? (1f / rect.height) : 0f;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = poly[i];
				array[i] = (useXZPlane ? new Vector3(vector.x, y, vector.y) : new Vector3(vector.x, vector.y, y));
				array2[i] = new Vector2((vector.x - rect.xMin) * num, (vector.y - rect.yMin) * num2);
			}
			if (useXZPlane && PolygonMeshBuilder.SignedArea(poly) > 0f)
			{
				Array.Reverse<Vector3>(array);
				Array.Reverse<Vector2>(array2);
			}
			int[] array3 = new int[(count - 2) * 3];
			int num3 = 0;
			for (int j = 1; j < count - 1; j++)
			{
				array3[num3++] = 0;
				array3[num3++] = j;
				array3[num3++] = j + 1;
			}
			Mesh mesh = new Mesh();
			mesh.name = "ConvexPolygon";
			mesh.SetVertices(array);
			mesh.SetTriangles(array3, 0);
			mesh.SetUVs(0, array2);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000BA550 File Offset: 0x000B8750
		private static Rect ComputeAabb(IReadOnlyList<Vector2> poly)
		{
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			float num3 = float.NegativeInfinity;
			float num4 = float.NegativeInfinity;
			for (int i = 0; i < poly.Count; i++)
			{
				Vector2 vector = poly[i];
				if (vector.x < num)
				{
					num = vector.x;
				}
				if (vector.y < num2)
				{
					num2 = vector.y;
				}
				if (vector.x > num3)
				{
					num3 = vector.x;
				}
				if (vector.y > num4)
				{
					num4 = vector.y;
				}
			}
			return Rect.MinMaxRect(num, num2, num3, num4);
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x000BA5E8 File Offset: 0x000B87E8
		private static float SignedArea(IReadOnlyList<Vector2> poly)
		{
			float num = 0f;
			for (int i = 0; i < poly.Count; i++)
			{
				Vector2 vector = poly[i];
				Vector2 vector2 = poly[(i + 1) % poly.Count];
				num += vector.x * vector2.y - vector2.x * vector.y;
			}
			return num * 0.5f;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000BA64C File Offset: 0x000B884C
		public static GameObject CreateRegionObject(List<Vector2> cellPoly, Material material, Transform parent, string name)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(parent, false);
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			Renderer renderer = gameObject.AddComponent<MeshRenderer>();
			meshFilter.sharedMesh = PolygonMeshBuilder.BuildConvexPolygonMesh(cellPoly, 0f, false);
			renderer.sharedMaterial = material;
			return gameObject;
		}
	}
}
