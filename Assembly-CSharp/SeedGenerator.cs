using System;
using System.Text;

// Token: 0x02000016 RID: 22
public class SeedGenerator
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600015B RID: 347 RVA: 0x0000A5E2 File Offset: 0x000087E2
	// (set) Token: 0x0600015C RID: 348 RVA: 0x0000A5EA File Offset: 0x000087EA
	public ulong Hash { get; private set; }

	// Token: 0x0600015D RID: 349 RVA: 0x0000A5F3 File Offset: 0x000087F3
	public SeedGenerator()
	{
		this.Hash = SeedGenerator.FnvOffset64;
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000A606 File Offset: 0x00008806
	public SeedGenerator Add(string data)
	{
		if (data == null)
		{
			data = "null";
		}
		return this.Add(Encoding.ASCII.GetBytes(data)).Add(SeedGenerator.SeparatorBytes);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000A62D File Offset: 0x0000882D
	public SeedGenerator Add(object data)
	{
		return this.Add((data != null) ? data.ToString() : null);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000A644 File Offset: 0x00008844
	public SeedGenerator Add(byte[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			this.Hash ^= (ulong)data[i];
			this.Hash *= SeedGenerator.FnvPrime64;
		}
		return this;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000A683 File Offset: 0x00008883
	public SeededRandom CreateRandom()
	{
		return new SeededRandom(this.Hash);
	}

	// Token: 0x040000CF RID: 207
	private static readonly byte[] SeparatorBytes = new byte[]
	{
		251,
		45,
		49
	};

	// Token: 0x040000D0 RID: 208
	public static readonly ulong FnvPrime64 = 1099511628211UL;

	// Token: 0x040000D1 RID: 209
	public static readonly ulong FnvOffset64 = 14695981039346656037UL;
}
