using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class SeededRandom
{
	// Token: 0x06000146 RID: 326 RVA: 0x0000A1E1 File Offset: 0x000083E1
	public SeededRandom(ulong seed)
	{
		this.State = seed;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000A1F0 File Offset: 0x000083F0
	public ulong RandomLong()
	{
		ulong num = this.State += 11400714819323198485UL;
		ulong num2 = (num ^ num >> 30) * 13787848793156543929UL;
		ulong num3 = (num2 ^ num2 >> 27) * 10723151780598845931UL;
		return num3 ^ num3 >> 31;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000A239 File Offset: 0x00008439
	public uint RandomInt()
	{
		return (uint)this.RandomLong();
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000A242 File Offset: 0x00008442
	public int RandomRange(int lo, int hi)
	{
		if (lo == hi)
		{
			return lo;
		}
		if (lo > hi)
		{
			int num = hi;
			hi = lo;
			lo = num;
		}
		return (int)((ulong)this.RandomInt() % (ulong)((long)(hi - lo))) + lo;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000A263 File Offset: 0x00008463
	public float RandomRange(float lo, float hi)
	{
		if (lo == hi)
		{
			return lo;
		}
		if (lo > hi)
		{
			float num = hi;
			hi = lo;
			lo = num;
		}
		return this.RandomFloat() * (hi - lo) + lo;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000A281 File Offset: 0x00008481
	public Vector2 RandomWithinRect(Rect rect)
	{
		return new Vector2(this.RandomRange(rect.xMin, rect.xMax), this.RandomRange(rect.yMin, rect.yMax));
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000A2B0 File Offset: 0x000084B0
	public float RandomFloat()
	{
		int num = 16777216;
		return (float)this.RandomRange(0, num) / (float)num;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000A2D0 File Offset: 0x000084D0
	public float RandomGaussian(float mean, float stdDev)
	{
		float f = 1f - this.RandomFloat();
		float num = 1f - this.RandomFloat();
		float num2 = Mathf.Sqrt(-2f * Mathf.Log(f)) * Mathf.Sin(6.28318548f * num);
		return mean + stdDev * num2;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000A31B File Offset: 0x0000851B
	internal bool RandomBool(object activitiesPerSecond)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000A322 File Offset: 0x00008522
	public float RandomScatter(float baseVal, float scatter = 0.2f)
	{
		return this.RandomGaussian(baseVal, baseVal * scatter / 2f);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000A334 File Offset: 0x00008534
	public bool RandomBool(float chanceOfTrue = 0.5f)
	{
		return this.RandomFloat() < chanceOfTrue;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000A340 File Offset: 0x00008540
	public string RandomString(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append(this.Choose<char>(SeededRandom.StringCharacters));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000A377 File Offset: 0x00008577
	public string RandomItemSeed()
	{
		return this.RandomString(8);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000A380 File Offset: 0x00008580
	public T Choose<T>(IList<T> list)
	{
		if (list == null || list.Count == 0)
		{
			return default(T);
		}
		if (list.Count == 1)
		{
			return list[0];
		}
		return list[this.RandomRange(0, list.Count)];
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000A3C6 File Offset: 0x000085C6
	public T Choose<T>(IEnumerable<T> list)
	{
		return this.Choose<T>(new List<T>(list));
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000A3D4 File Offset: 0x000085D4
	public IEnumerable<T> Choose<T>(IEnumerable<T> list, int count)
	{
		List<T> av = new List<T>(list);
		int i = 0;
		while (i < count && av.Count > 0)
		{
			yield return this.ChooseAndRemove<T>(av);
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000A3F4 File Offset: 0x000085F4
	public T ChooseAndRemove<T>(List<T> list)
	{
		T t = this.Choose<T>(list);
		list.Remove(t);
		return t;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000A414 File Offset: 0x00008614
	public T ChooseEnum<T>(int startIndex = 0)
	{
		Array values = Enum.GetValues(typeof(T));
		if (values.Length == 0)
		{
			return default(T);
		}
		if (values.Length == 1)
		{
			return (T)((object)values.GetValue(0));
		}
		return (T)((object)values.GetValue(this.RandomRange(startIndex, values.Length)));
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000A474 File Offset: 0x00008674
	public T Choose<T>(Dictionary<T, float> values)
	{
		float num = 0f;
		foreach (KeyValuePair<T, float> keyValuePair in values)
		{
			num += keyValuePair.Value;
		}
		float num2 = this.RandomRange(0f, num);
		bool flag = false;
		T result = default(T);
		foreach (KeyValuePair<T, float> keyValuePair2 in values)
		{
			if (!flag)
			{
				result = keyValuePair2.Key;
				flag = true;
			}
			num2 -= keyValuePair2.Value;
			if (num2 < 0f)
			{
				return keyValuePair2.Key;
			}
		}
		return result;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000A54C File Offset: 0x0000874C
	public void Shuffle<T>(IList<T> list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			int index = this.RandomRange(0, i + 1);
			T value = list[i];
			list[i] = list[index];
			list[index] = value;
		}
	}

	// Token: 0x040000CC RID: 204
	private static char[] StringCharacters = new char[]
	{
		'a',
		'b',
		'c',
		'd',
		'e',
		'f',
		'g',
		'h',
		'i',
		'j',
		'k',
		'l',
		'm',
		'n',
		'o',
		'p',
		'q',
		'r',
		's',
		't',
		'u',
		'v',
		'w',
		'x',
		'y',
		'z',
		'A',
		'B',
		'C',
		'D',
		'E',
		'F',
		'G',
		'H',
		'I',
		'J',
		'K',
		'L',
		'M',
		'N',
		'O',
		'P',
		'Q',
		'R',
		'S',
		'T',
		'U',
		'V',
		'W',
		'X',
		'Y',
		'Z',
		'0',
		'1',
		'2',
		'3',
		'4',
		'5',
		'6',
		'7',
		'8',
		'9'
	};

	// Token: 0x040000CD RID: 205
	public static SeededRandom Global = new SeedGenerator().Add(DateTime.Now.Ticks).CreateRandom();

	// Token: 0x040000CE RID: 206
	private ulong State;
}
