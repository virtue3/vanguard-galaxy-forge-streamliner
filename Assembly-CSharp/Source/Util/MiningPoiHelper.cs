using System;
using System.Collections.Generic;
using System.Linq;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Mining;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000036 RID: 54
	public static class MiningPoiHelper
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00015448 File Offset: 0x00013648
		public static void InitializeAsteroids(MapPointOfInterest poi, AsteroidFieldData data, bool reverse = false, bool keepMiddleClear = true)
		{
			poi.asteroidsInitialized = true;
			poi.SetAsteroidFieldData(data, 0);
			MiningPoiHelper.CreateNewAsteroids(poi, data, reverse, keepMiddleClear);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00015464 File Offset: 0x00013664
		public static void CreateNewAsteroids(MapPointOfInterest poi, AsteroidFieldData data, bool reverse = false, bool keepMiddleClear = true)
		{
			for (int i = 0; i < data.amount; i++)
			{
				Vector2 randomSpawnPosition = MiningPoiHelper.GetRandomSpawnPosition(poi, data, reverse, keepMiddleClear);
				AsteroidData asteroidData = new AsteroidData("");
				asteroidData.InitAsteroid(poi, data, randomSpawnPosition);
				poi.AddPersistable(asteroidData);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000154A8 File Offset: 0x000136A8
		private static Vector2 GetRandomSpawnPosition(MapPointOfInterest poi, AsteroidFieldData data, bool reverse = false, bool keepMiddleClear = true)
		{
			List<AsteroidData> list = poi.GetPersistables().OfType<AsteroidData>().ToList<AsteroidData>();
			Vector2 worldPosition = poi.GetWorldPosition();
			Vector2 vector;
			if (list.Count == 0)
			{
				float num = (float)(keepMiddleClear ? 15 : 5);
				vector = new Vector2(reverse ? (-num) : num, 0f);
			}
			else
			{
				Vector2 a = list.Last<AsteroidData>().position - worldPosition;
				float d = 5f / data.density * SeededRandom.Global.RandomRange(0.75f, 1.25f);
				do
				{
					Vector2 normalized = new Vector2(SeededRandom.Global.RandomRange(0f, 1f), SeededRandom.Global.RandomRange(-1f, 1f)).normalized;
					if (reverse)
					{
						vector = a - normalized * d;
					}
					else
					{
						vector = a + normalized * d;
					}
				}
				while (Mathf.Abs(vector.y) > 8f);
			}
			return worldPosition + vector;
		}

		// Token: 0x04000170 RID: 368
		public const float AverageAsteroidDistance = 5f;
	}
}
