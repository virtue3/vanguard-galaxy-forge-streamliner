using System;
using System.Runtime.CompilerServices;

// Token: 0x02000010 RID: 16
public class FastNoiseLite
{
	// Token: 0x060000C7 RID: 199 RVA: 0x00005A44 File Offset: 0x00003C44
	public FastNoiseLite(int seed = 1337)
	{
		this.SetSeed(seed);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005AD9 File Offset: 0x00003CD9
	public void SetSeed(int seed)
	{
		this.mSeed = seed;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005AE2 File Offset: 0x00003CE2
	public void SetFrequency(float frequency)
	{
		this.mFrequency = frequency;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005AEB File Offset: 0x00003CEB
	public void SetNoiseType(FastNoiseLite.NoiseType noiseType)
	{
		this.mNoiseType = noiseType;
		this.UpdateTransformType3D();
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005AFA File Offset: 0x00003CFA
	public void SetRotationType3D(FastNoiseLite.RotationType3D rotationType3D)
	{
		this.mRotationType3D = rotationType3D;
		this.UpdateTransformType3D();
		this.UpdateWarpTransformType3D();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005B0F File Offset: 0x00003D0F
	public void SetFractalType(FastNoiseLite.FractalType fractalType)
	{
		this.mFractalType = fractalType;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005B18 File Offset: 0x00003D18
	public void SetFractalOctaves(int octaves)
	{
		this.mOctaves = octaves;
		this.CalculateFractalBounding();
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005B27 File Offset: 0x00003D27
	public void SetFractalLacunarity(float lacunarity)
	{
		this.mLacunarity = lacunarity;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005B30 File Offset: 0x00003D30
	public void SetFractalGain(float gain)
	{
		this.mGain = gain;
		this.CalculateFractalBounding();
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005B3F File Offset: 0x00003D3F
	public void SetFractalWeightedStrength(float weightedStrength)
	{
		this.mWeightedStrength = weightedStrength;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005B48 File Offset: 0x00003D48
	public void SetFractalPingPongStrength(float pingPongStrength)
	{
		this.mPingPongStrength = pingPongStrength;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005B51 File Offset: 0x00003D51
	public void SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction cellularDistanceFunction)
	{
		this.mCellularDistanceFunction = cellularDistanceFunction;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005B5A File Offset: 0x00003D5A
	public void SetCellularReturnType(FastNoiseLite.CellularReturnType cellularReturnType)
	{
		this.mCellularReturnType = cellularReturnType;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005B63 File Offset: 0x00003D63
	public void SetCellularJitter(float cellularJitter)
	{
		this.mCellularJitterModifier = cellularJitter;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00005B6C File Offset: 0x00003D6C
	public void SetDomainWarpType(FastNoiseLite.DomainWarpType domainWarpType)
	{
		this.mDomainWarpType = domainWarpType;
		this.UpdateWarpTransformType3D();
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005B7B File Offset: 0x00003D7B
	public void SetDomainWarpAmp(float domainWarpAmp)
	{
		this.mDomainWarpAmp = domainWarpAmp;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00005B84 File Offset: 0x00003D84
	[MethodImpl((MethodImplOptions)512)]
	public float GetNoise(float x, float y)
	{
		this.TransformNoiseCoordinate(ref x, ref y);
		switch (this.mFractalType)
		{
		case FastNoiseLite.FractalType.FBm:
			return this.GenFractalFBm(x, y);
		case FastNoiseLite.FractalType.Ridged:
			return this.GenFractalRidged(x, y);
		case FastNoiseLite.FractalType.PingPong:
			return this.GenFractalPingPong(x, y);
		default:
			return this.GenNoiseSingle(this.mSeed, x, y);
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00005BE0 File Offset: 0x00003DE0
	[MethodImpl((MethodImplOptions)512)]
	public float GetNoise(float x, float y, float z)
	{
		this.TransformNoiseCoordinate(ref x, ref y, ref z);
		switch (this.mFractalType)
		{
		case FastNoiseLite.FractalType.FBm:
			return this.GenFractalFBm(x, y, z);
		case FastNoiseLite.FractalType.Ridged:
			return this.GenFractalRidged(x, y, z);
		case FastNoiseLite.FractalType.PingPong:
			return this.GenFractalPingPong(x, y, z);
		default:
			return this.GenNoiseSingle(this.mSeed, x, y, z);
		}
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00005C44 File Offset: 0x00003E44
	[MethodImpl((MethodImplOptions)512)]
	public void DomainWarp(ref float x, ref float y)
	{
		FastNoiseLite.FractalType fractalType = this.mFractalType;
		if (fractalType == FastNoiseLite.FractalType.DomainWarpProgressive)
		{
			this.DomainWarpFractalProgressive(ref x, ref y);
			return;
		}
		if (fractalType != FastNoiseLite.FractalType.DomainWarpIndependent)
		{
			this.DomainWarpSingle(ref x, ref y);
			return;
		}
		this.DomainWarpFractalIndependent(ref x, ref y);
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00005C7C File Offset: 0x00003E7C
	[MethodImpl((MethodImplOptions)512)]
	public void DomainWarp(ref float x, ref float y, ref float z)
	{
		FastNoiseLite.FractalType fractalType = this.mFractalType;
		if (fractalType == FastNoiseLite.FractalType.DomainWarpProgressive)
		{
			this.DomainWarpFractalProgressive(ref x, ref y, ref z);
			return;
		}
		if (fractalType != FastNoiseLite.FractalType.DomainWarpIndependent)
		{
			this.DomainWarpSingle(ref x, ref y, ref z);
			return;
		}
		this.DomainWarpFractalIndependent(ref x, ref y, ref z);
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005CB5 File Offset: 0x00003EB5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float FastMin(float a, float b)
	{
		if (a >= b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00005CBE File Offset: 0x00003EBE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float FastMax(float a, float b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00005CC7 File Offset: 0x00003EC7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float FastAbs(float f)
	{
		if (f >= 0f)
		{
			return f;
		}
		return -f;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00005CD5 File Offset: 0x00003ED5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float FastSqrt(float f)
	{
		return (float)Math.Sqrt((double)f);
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00005CDF File Offset: 0x00003EDF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int FastFloor(float f)
	{
		if (f < 0f)
		{
			return (int)f - 1;
		}
		return (int)f;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005CF0 File Offset: 0x00003EF0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int FastRound(float f)
	{
		if (f < 0f)
		{
			return (int)(f - 0.5f);
		}
		return (int)(f + 0.5f);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005D0B File Offset: 0x00003F0B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float Lerp(float a, float b, float t)
	{
		return a + t * (b - a);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005D14 File Offset: 0x00003F14
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float InterpHermite(float t)
	{
		return t * t * (3f - 2f * t);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00005D27 File Offset: 0x00003F27
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float InterpQuintic(float t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00005D44 File Offset: 0x00003F44
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float CubicLerp(float a, float b, float c, float d, float t)
	{
		float num = d - c - (a - b);
		return t * t * t * num + t * t * (a - b - num) + t * (c - a) + b;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00005D78 File Offset: 0x00003F78
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float PingPong(float t)
	{
		t -= (float)((int)(t * 0.5f) * 2);
		if (t >= 1f)
		{
			return 2f - t;
		}
		return t;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00005D9C File Offset: 0x00003F9C
	private void CalculateFractalBounding()
	{
		float num = FastNoiseLite.FastAbs(this.mGain);
		float num2 = num;
		float num3 = 1f;
		for (int i = 1; i < this.mOctaves; i++)
		{
			num3 += num2;
			num2 *= num;
		}
		this.mFractalBounding = 1f / num3;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00005DE3 File Offset: 0x00003FE3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Hash(int seed, int xPrimed, int yPrimed)
	{
		return (seed ^ xPrimed ^ yPrimed) * 668265261;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00005DF0 File Offset: 0x00003FF0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Hash(int seed, int xPrimed, int yPrimed, int zPrimed)
	{
		return (seed ^ xPrimed ^ yPrimed ^ zPrimed) * 668265261;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00005DFF File Offset: 0x00003FFF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float ValCoord(int seed, int xPrimed, int yPrimed)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed);
		int num2 = num * num;
		return (float)(num2 ^ num2 << 19) * 4.656613E-10f;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00005E17 File Offset: 0x00004017
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float ValCoord(int seed, int xPrimed, int yPrimed, int zPrimed)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed, zPrimed);
		int num2 = num * num;
		return (float)(num2 ^ num2 << 19) * 4.656613E-10f;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005E30 File Offset: 0x00004030
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GradCoord(int seed, int xPrimed, int yPrimed, float xd, float yd)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed);
		num ^= num >> 15;
		num &= 254;
		float num2 = FastNoiseLite.Gradients2D[num];
		float num3 = FastNoiseLite.Gradients2D[num | 1];
		return xd * num2 + yd * num3;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00005E70 File Offset: 0x00004070
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GradCoord(int seed, int xPrimed, int yPrimed, int zPrimed, float xd, float yd, float zd)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed, zPrimed);
		num ^= num >> 15;
		num &= 252;
		float num2 = FastNoiseLite.Gradients3D[num];
		float num3 = FastNoiseLite.Gradients3D[num | 1];
		float num4 = FastNoiseLite.Gradients3D[num | 2];
		return xd * num2 + yd * num3 + zd * num4;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00005EC0 File Offset: 0x000040C0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void GradCoordOut(int seed, int xPrimed, int yPrimed, out float xo, out float yo)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed) & 510;
		xo = FastNoiseLite.RandVecs2D[num];
		yo = FastNoiseLite.RandVecs2D[num | 1];
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00005EF4 File Offset: 0x000040F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void GradCoordOut(int seed, int xPrimed, int yPrimed, int zPrimed, out float xo, out float yo, out float zo)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed, zPrimed) & 1020;
		xo = FastNoiseLite.RandVecs3D[num];
		yo = FastNoiseLite.RandVecs3D[num | 1];
		zo = FastNoiseLite.RandVecs3D[num | 2];
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00005F34 File Offset: 0x00004134
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void GradCoordDual(int seed, int xPrimed, int yPrimed, float xd, float yd, out float xo, out float yo)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed);
		int num2 = num & 254;
		int num3 = num >> 7 & 510;
		float num4 = FastNoiseLite.Gradients2D[num2];
		float num5 = FastNoiseLite.Gradients2D[num2 | 1];
		float num6 = xd * num4 + yd * num5;
		float num7 = FastNoiseLite.RandVecs2D[num3];
		float num8 = FastNoiseLite.RandVecs2D[num3 | 1];
		xo = num6 * num7;
		yo = num6 * num8;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00005F9C File Offset: 0x0000419C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void GradCoordDual(int seed, int xPrimed, int yPrimed, int zPrimed, float xd, float yd, float zd, out float xo, out float yo, out float zo)
	{
		int num = FastNoiseLite.Hash(seed, xPrimed, yPrimed, zPrimed);
		int num2 = num & 252;
		int num3 = num >> 6 & 1020;
		float num4 = FastNoiseLite.Gradients3D[num2];
		float num5 = FastNoiseLite.Gradients3D[num2 | 1];
		float num6 = FastNoiseLite.Gradients3D[num2 | 2];
		float num7 = xd * num4 + yd * num5 + zd * num6;
		float num8 = FastNoiseLite.RandVecs3D[num3];
		float num9 = FastNoiseLite.RandVecs3D[num3 | 1];
		float num10 = FastNoiseLite.RandVecs3D[num3 | 2];
		xo = num7 * num8;
		yo = num7 * num9;
		zo = num7 * num10;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006028 File Offset: 0x00004228
	private float GenNoiseSingle(int seed, float x, float y)
	{
		switch (this.mNoiseType)
		{
		case FastNoiseLite.NoiseType.OpenSimplex2:
			return this.SingleSimplex(seed, x, y);
		case FastNoiseLite.NoiseType.OpenSimplex2S:
			return this.SingleOpenSimplex2S(seed, x, y);
		case FastNoiseLite.NoiseType.Cellular:
			return this.SingleCellular(seed, x, y);
		case FastNoiseLite.NoiseType.Perlin:
			return this.SinglePerlin(seed, x, y);
		case FastNoiseLite.NoiseType.ValueCubic:
			return this.SingleValueCubic(seed, x, y);
		case FastNoiseLite.NoiseType.Value:
			return this.SingleValue(seed, x, y);
		default:
			return 0f;
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000060A0 File Offset: 0x000042A0
	private float GenNoiseSingle(int seed, float x, float y, float z)
	{
		switch (this.mNoiseType)
		{
		case FastNoiseLite.NoiseType.OpenSimplex2:
			return this.SingleOpenSimplex2(seed, x, y, z);
		case FastNoiseLite.NoiseType.OpenSimplex2S:
			return this.SingleOpenSimplex2S(seed, x, y, z);
		case FastNoiseLite.NoiseType.Cellular:
			return this.SingleCellular(seed, x, y, z);
		case FastNoiseLite.NoiseType.Perlin:
			return this.SinglePerlin(seed, x, y, z);
		case FastNoiseLite.NoiseType.ValueCubic:
			return this.SingleValueCubic(seed, x, y, z);
		case FastNoiseLite.NoiseType.Value:
			return this.SingleValue(seed, x, y, z);
		default:
			return 0f;
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00006124 File Offset: 0x00004324
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void TransformNoiseCoordinate(ref float x, ref float y)
	{
		x *= this.mFrequency;
		y *= this.mFrequency;
		FastNoiseLite.NoiseType noiseType = this.mNoiseType;
		if (noiseType <= FastNoiseLite.NoiseType.OpenSimplex2S)
		{
			float num = (x + y) * 0.3660254f;
			x += num;
			y += num;
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0000616C File Offset: 0x0000436C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void TransformNoiseCoordinate(ref float x, ref float y, ref float z)
	{
		x *= this.mFrequency;
		y *= this.mFrequency;
		z *= this.mFrequency;
		switch (this.mTransformType3D)
		{
		case FastNoiseLite.TransformType3D.ImproveXYPlanes:
		{
			float num = x + y;
			float num2 = num * -0.211324871f;
			z *= 0.577350259f;
			x += num2 - z;
			y = y + num2 - z;
			z += num * 0.577350259f;
			return;
		}
		case FastNoiseLite.TransformType3D.ImproveXZPlanes:
		{
			float num3 = x + z;
			float num4 = num3 * -0.211324871f;
			y *= 0.577350259f;
			x += num4 - y;
			z += num4 - y;
			y += num3 * 0.577350259f;
			return;
		}
		case FastNoiseLite.TransformType3D.DefaultOpenSimplex2:
		{
			float num5 = (x + y + z) * 0.6666667f;
			x = num5 - x;
			y = num5 - y;
			z = num5 - z;
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000624C File Offset: 0x0000444C
	private void UpdateTransformType3D()
	{
		FastNoiseLite.RotationType3D rotationType3D = this.mRotationType3D;
		if (rotationType3D == FastNoiseLite.RotationType3D.ImproveXYPlanes)
		{
			this.mTransformType3D = FastNoiseLite.TransformType3D.ImproveXYPlanes;
			return;
		}
		if (rotationType3D == FastNoiseLite.RotationType3D.ImproveXZPlanes)
		{
			this.mTransformType3D = FastNoiseLite.TransformType3D.ImproveXZPlanes;
			return;
		}
		FastNoiseLite.NoiseType noiseType = this.mNoiseType;
		if (noiseType <= FastNoiseLite.NoiseType.OpenSimplex2S)
		{
			this.mTransformType3D = FastNoiseLite.TransformType3D.DefaultOpenSimplex2;
			return;
		}
		this.mTransformType3D = FastNoiseLite.TransformType3D.None;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006294 File Offset: 0x00004494
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void TransformDomainWarpCoordinate(ref float x, ref float y)
	{
		FastNoiseLite.DomainWarpType domainWarpType = this.mDomainWarpType;
		if (domainWarpType <= FastNoiseLite.DomainWarpType.OpenSimplex2Reduced)
		{
			float num = (x + y) * 0.3660254f;
			x += num;
			y += num;
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000062C4 File Offset: 0x000044C4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void TransformDomainWarpCoordinate(ref float x, ref float y, ref float z)
	{
		switch (this.mWarpTransformType3D)
		{
		case FastNoiseLite.TransformType3D.ImproveXYPlanes:
		{
			float num = x + y;
			float num2 = num * -0.211324871f;
			z *= 0.577350259f;
			x += num2 - z;
			y = y + num2 - z;
			z += num * 0.577350259f;
			return;
		}
		case FastNoiseLite.TransformType3D.ImproveXZPlanes:
		{
			float num3 = x + z;
			float num4 = num3 * -0.211324871f;
			y *= 0.577350259f;
			x += num4 - y;
			z += num4 - y;
			y += num3 * 0.577350259f;
			return;
		}
		case FastNoiseLite.TransformType3D.DefaultOpenSimplex2:
		{
			float num5 = (x + y + z) * 0.6666667f;
			x = num5 - x;
			y = num5 - y;
			z = num5 - z;
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006384 File Offset: 0x00004584
	private void UpdateWarpTransformType3D()
	{
		FastNoiseLite.RotationType3D rotationType3D = this.mRotationType3D;
		if (rotationType3D == FastNoiseLite.RotationType3D.ImproveXYPlanes)
		{
			this.mWarpTransformType3D = FastNoiseLite.TransformType3D.ImproveXYPlanes;
			return;
		}
		if (rotationType3D == FastNoiseLite.RotationType3D.ImproveXZPlanes)
		{
			this.mWarpTransformType3D = FastNoiseLite.TransformType3D.ImproveXZPlanes;
			return;
		}
		FastNoiseLite.DomainWarpType domainWarpType = this.mDomainWarpType;
		if (domainWarpType <= FastNoiseLite.DomainWarpType.OpenSimplex2Reduced)
		{
			this.mWarpTransformType3D = FastNoiseLite.TransformType3D.DefaultOpenSimplex2;
			return;
		}
		this.mWarpTransformType3D = FastNoiseLite.TransformType3D.None;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x000063CC File Offset: 0x000045CC
	private float GenFractalFBm(float x, float y)
	{
		int num = this.mSeed;
		float num2 = 0f;
		float num3 = this.mFractalBounding;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float num4 = this.GenNoiseSingle(num++, x, y);
			num2 += num4 * num3;
			num3 *= FastNoiseLite.Lerp(1f, FastNoiseLite.FastMin(num4 + 1f, 2f) * 0.5f, this.mWeightedStrength);
			x *= this.mLacunarity;
			y *= this.mLacunarity;
			num3 *= this.mGain;
		}
		return num2;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006460 File Offset: 0x00004660
	private float GenFractalFBm(float x, float y, float z)
	{
		int num = this.mSeed;
		float num2 = 0f;
		float num3 = this.mFractalBounding;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float num4 = this.GenNoiseSingle(num++, x, y, z);
			num2 += num4 * num3;
			num3 *= FastNoiseLite.Lerp(1f, (num4 + 1f) * 0.5f, this.mWeightedStrength);
			x *= this.mLacunarity;
			y *= this.mLacunarity;
			z *= this.mLacunarity;
			num3 *= this.mGain;
		}
		return num2;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x000064F4 File Offset: 0x000046F4
	private float GenFractalRidged(float x, float y)
	{
		int num = this.mSeed;
		float num2 = 0f;
		float num3 = this.mFractalBounding;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float num4 = FastNoiseLite.FastAbs(this.GenNoiseSingle(num++, x, y));
			num2 += (num4 * -2f + 1f) * num3;
			num3 *= FastNoiseLite.Lerp(1f, 1f - num4, this.mWeightedStrength);
			x *= this.mLacunarity;
			y *= this.mLacunarity;
			num3 *= this.mGain;
		}
		return num2;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006588 File Offset: 0x00004788
	private float GenFractalRidged(float x, float y, float z)
	{
		int num = this.mSeed;
		float num2 = 0f;
		float num3 = this.mFractalBounding;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float num4 = FastNoiseLite.FastAbs(this.GenNoiseSingle(num++, x, y, z));
			num2 += (num4 * -2f + 1f) * num3;
			num3 *= FastNoiseLite.Lerp(1f, 1f - num4, this.mWeightedStrength);
			x *= this.mLacunarity;
			y *= this.mLacunarity;
			z *= this.mLacunarity;
			num3 *= this.mGain;
		}
		return num2;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00006628 File Offset: 0x00004828
	private float GenFractalPingPong(float x, float y)
	{
		int num = this.mSeed;
		float num2 = 0f;
		float num3 = this.mFractalBounding;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float num4 = FastNoiseLite.PingPong((this.GenNoiseSingle(num++, x, y) + 1f) * this.mPingPongStrength);
			num2 += (num4 - 0.5f) * 2f * num3;
			num3 *= FastNoiseLite.Lerp(1f, num4, this.mWeightedStrength);
			x *= this.mLacunarity;
			y *= this.mLacunarity;
			num3 *= this.mGain;
		}
		return num2;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000066C4 File Offset: 0x000048C4
	private float GenFractalPingPong(float x, float y, float z)
	{
		int num = this.mSeed;
		float num2 = 0f;
		float num3 = this.mFractalBounding;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float num4 = FastNoiseLite.PingPong((this.GenNoiseSingle(num++, x, y, z) + 1f) * this.mPingPongStrength);
			num2 += (num4 - 0.5f) * 2f * num3;
			num3 *= FastNoiseLite.Lerp(1f, num4, this.mWeightedStrength);
			x *= this.mLacunarity;
			y *= this.mLacunarity;
			z *= this.mLacunarity;
			num3 *= this.mGain;
		}
		return num2;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006768 File Offset: 0x00004968
	private float SingleSimplex(int seed, float x, float y)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		float num3 = x - (float)num;
		float num4 = y - (float)num2;
		float num5 = (num3 + num4) * 0.211324871f;
		float num6 = num3 - num5;
		float num7 = num4 - num5;
		num *= 501125321;
		num2 *= 1136930381;
		float num8 = 0.5f - num6 * num6 - num7 * num7;
		float num9;
		if (num8 <= 0f)
		{
			num9 = 0f;
		}
		else
		{
			num9 = num8 * num8 * (num8 * num8) * FastNoiseLite.GradCoord(seed, num, num2, num6, num7);
		}
		float num10 = 3.15470052f * num5 + (-0.6666666f + num8);
		float num11;
		if (num10 <= 0f)
		{
			num11 = 0f;
		}
		else
		{
			float xd = num6 + -0.577350259f;
			float yd = num7 + -0.577350259f;
			num11 = num10 * num10 * (num10 * num10) * FastNoiseLite.GradCoord(seed, num + 501125321, num2 + 1136930381, xd, yd);
		}
		float num15;
		if (num7 > num6)
		{
			float num12 = num6 + 0.211324871f;
			float num13 = num7 + -0.7886751f;
			float num14 = 0.5f - num12 * num12 - num13 * num13;
			if (num14 <= 0f)
			{
				num15 = 0f;
			}
			else
			{
				num15 = num14 * num14 * (num14 * num14) * FastNoiseLite.GradCoord(seed, num, num2 + 1136930381, num12, num13);
			}
		}
		else
		{
			float num16 = num6 + -0.7886751f;
			float num17 = num7 + 0.211324871f;
			float num18 = 0.5f - num16 * num16 - num17 * num17;
			if (num18 <= 0f)
			{
				num15 = 0f;
			}
			else
			{
				num15 = num18 * num18 * (num18 * num18) * FastNoiseLite.GradCoord(seed, num + 501125321, num2, num16, num17);
			}
		}
		return (num9 + num15 + num11) * 99.83685f;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006918 File Offset: 0x00004B18
	private float SingleOpenSimplex2(int seed, float x, float y, float z)
	{
		int num = FastNoiseLite.FastRound(x);
		int num2 = FastNoiseLite.FastRound(y);
		int num3 = FastNoiseLite.FastRound(z);
		float num4 = x - (float)num;
		float num5 = y - (float)num2;
		float num6 = z - (float)num3;
		int num7 = (int)(-1f - num4) | 1;
		int num8 = (int)(-1f - num5) | 1;
		int num9 = (int)(-1f - num6) | 1;
		float num10 = (float)num7 * -num4;
		float num11 = (float)num8 * -num5;
		float num12 = (float)num9 * -num6;
		num *= 501125321;
		num2 *= 1136930381;
		num3 *= 1720413743;
		float num13 = 0f;
		float num14 = 0.6f - num4 * num4 - (num5 * num5 + num6 * num6);
		int num15 = 0;
		for (;;)
		{
			if (num14 > 0f)
			{
				num13 += num14 * num14 * (num14 * num14) * FastNoiseLite.GradCoord(seed, num, num2, num3, num4, num5, num6);
			}
			if (num10 >= num11 && num10 >= num12)
			{
				float num16 = num14 + num10 + num10;
				if (num16 > 1f)
				{
					num16 -= 1f;
					num13 += num16 * num16 * (num16 * num16) * FastNoiseLite.GradCoord(seed, num - num7 * 501125321, num2, num3, num4 + (float)num7, num5, num6);
				}
			}
			else if (num11 > num10 && num11 >= num12)
			{
				float num17 = num14 + num11 + num11;
				if (num17 > 1f)
				{
					num17 -= 1f;
					num13 += num17 * num17 * (num17 * num17) * FastNoiseLite.GradCoord(seed, num, num2 - num8 * 1136930381, num3, num4, num5 + (float)num8, num6);
				}
			}
			else
			{
				float num18 = num14 + num12 + num12;
				if (num18 > 1f)
				{
					num18 -= 1f;
					num13 += num18 * num18 * (num18 * num18) * FastNoiseLite.GradCoord(seed, num, num2, num3 - num9 * 1720413743, num4, num5, num6 + (float)num9);
				}
			}
			if (num15 == 1)
			{
				break;
			}
			num10 = 0.5f - num10;
			num11 = 0.5f - num11;
			num12 = 0.5f - num12;
			num4 = (float)num7 * num10;
			num5 = (float)num8 * num11;
			num6 = (float)num9 * num12;
			num14 += 0.75f - num10 - (num11 + num12);
			num += (num7 >> 1 & 501125321);
			num2 += (num8 >> 1 & 1136930381);
			num3 += (num9 >> 1 & 1720413743);
			num7 = -num7;
			num8 = -num8;
			num9 = -num9;
			seed = ~seed;
			num15++;
		}
		return num13 * 32.6942825f;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006B90 File Offset: 0x00004D90
	private float SingleOpenSimplex2S(int seed, float x, float y)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		float num3 = x - (float)num;
		float num4 = y - (float)num2;
		num *= 501125321;
		num2 *= 1136930381;
		int xPrimed = num + 501125321;
		int yPrimed = num2 + 1136930381;
		float num5 = (num3 + num4) * 0.211324871f;
		float num6 = num3 - num5;
		float num7 = num4 - num5;
		float num8 = 0.6666667f - num6 * num6 - num7 * num7;
		float num9 = num8 * num8 * (num8 * num8) * FastNoiseLite.GradCoord(seed, num, num2, num6, num7);
		float num10 = 3.15470052f * num5 + (-0.6666666f + num8);
		float xd = num6 - 0.577350259f;
		float yd = num7 - 0.577350259f;
		num9 += num10 * num10 * (num10 * num10) * FastNoiseLite.GradCoord(seed, xPrimed, yPrimed, xd, yd);
		float num11 = num3 - num4;
		if (num5 > 0.211324871f)
		{
			if (num3 + num11 > 1f)
			{
				float num12 = num6 + -1.36602545f;
				float num13 = num7 + -0.3660254f;
				float num14 = 0.6666667f - num12 * num12 - num13 * num13;
				if (num14 > 0f)
				{
					num9 += num14 * num14 * (num14 * num14) * FastNoiseLite.GradCoord(seed, num + 1002250642, num2 + 1136930381, num12, num13);
				}
			}
			else
			{
				float num15 = num6 + 0.211324871f;
				float num16 = num7 + -0.7886751f;
				float num17 = 0.6666667f - num15 * num15 - num16 * num16;
				if (num17 > 0f)
				{
					num9 += num17 * num17 * (num17 * num17) * FastNoiseLite.GradCoord(seed, num, num2 + 1136930381, num15, num16);
				}
			}
			if (num4 - num11 > 1f)
			{
				float num18 = num6 + -0.3660254f;
				float num19 = num7 + -1.36602545f;
				float num20 = 0.6666667f - num18 * num18 - num19 * num19;
				if (num20 > 0f)
				{
					num9 += num20 * num20 * (num20 * num20) * FastNoiseLite.GradCoord(seed, num + 501125321, num2 + -2021106534, num18, num19);
				}
			}
			else
			{
				float num21 = num6 + -0.7886751f;
				float num22 = num7 + 0.211324871f;
				float num23 = 0.6666667f - num21 * num21 - num22 * num22;
				if (num23 > 0f)
				{
					num9 += num23 * num23 * (num23 * num23) * FastNoiseLite.GradCoord(seed, num + 501125321, num2, num21, num22);
				}
			}
		}
		else
		{
			if (num3 + num11 < 0f)
			{
				float num24 = num6 + 0.7886751f;
				float num25 = num7 - 0.211324871f;
				float num26 = 0.6666667f - num24 * num24 - num25 * num25;
				if (num26 > 0f)
				{
					num9 += num26 * num26 * (num26 * num26) * FastNoiseLite.GradCoord(seed, num - 501125321, num2, num24, num25);
				}
			}
			else
			{
				float num27 = num6 + -0.7886751f;
				float num28 = num7 + 0.211324871f;
				float num29 = 0.6666667f - num27 * num27 - num28 * num28;
				if (num29 > 0f)
				{
					num9 += num29 * num29 * (num29 * num29) * FastNoiseLite.GradCoord(seed, num + 501125321, num2, num27, num28);
				}
			}
			if (num4 < num11)
			{
				float num30 = num6 - 0.211324871f;
				float num31 = num7 - -0.7886751f;
				float num32 = 0.6666667f - num30 * num30 - num31 * num31;
				if (num32 > 0f)
				{
					num9 += num32 * num32 * (num32 * num32) * FastNoiseLite.GradCoord(seed, num, num2 - 1136930381, num30, num31);
				}
			}
			else
			{
				float num33 = num6 + 0.211324871f;
				float num34 = num7 + -0.7886751f;
				float num35 = 0.6666667f - num33 * num33 - num34 * num34;
				if (num35 > 0f)
				{
					num9 += num35 * num35 * (num35 * num35) * FastNoiseLite.GradCoord(seed, num, num2 + 1136930381, num33, num34);
				}
			}
		}
		return num9 * 18.2419624f;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006F5C File Offset: 0x0000515C
	private float SingleOpenSimplex2S(int seed, float x, float y, float z)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		int num3 = FastNoiseLite.FastFloor(z);
		float num4 = x - (float)num;
		float num5 = y - (float)num2;
		float num6 = z - (float)num3;
		num *= 501125321;
		num2 *= 1136930381;
		num3 *= 1720413743;
		int seed2 = seed + 1293373;
		int num7 = (int)(-0.5f - num4);
		int num8 = (int)(-0.5f - num5);
		int num9 = (int)(-0.5f - num6);
		float num10 = num4 + (float)num7;
		float num11 = num5 + (float)num8;
		float num12 = num6 + (float)num9;
		float num13 = 0.75f - num10 * num10 - num11 * num11 - num12 * num12;
		float num14 = num13 * num13 * (num13 * num13) * FastNoiseLite.GradCoord(seed, num + (num7 & 501125321), num2 + (num8 & 1136930381), num3 + (num9 & 1720413743), num10, num11, num12);
		float num15 = num4 - 0.5f;
		float num16 = num5 - 0.5f;
		float num17 = num6 - 0.5f;
		float num18 = 0.75f - num15 * num15 - num16 * num16 - num17 * num17;
		num14 += num18 * num18 * (num18 * num18) * FastNoiseLite.GradCoord(seed2, num + 501125321, num2 + 1136930381, num3 + 1720413743, num15, num16, num17);
		float num19 = (float)((num7 | 1) << 1) * num15;
		float num20 = (float)((num8 | 1) << 1) * num16;
		float num21 = (float)((num9 | 1) << 1) * num17;
		float num22 = (float)(-2 - (num7 << 2)) * num15 - 1f;
		float num23 = (float)(-2 - (num8 << 2)) * num16 - 1f;
		float num24 = (float)(-2 - (num9 << 2)) * num17 - 1f;
		bool flag = false;
		float num25 = num19 + num13;
		if (num25 > 0f)
		{
			float xd = num10 - (float)(num7 | 1);
			float yd = num11;
			float zd = num12;
			num14 += num25 * num25 * (num25 * num25) * FastNoiseLite.GradCoord(seed, num + (~num7 & 501125321), num2 + (num8 & 1136930381), num3 + (num9 & 1720413743), xd, yd, zd);
		}
		else
		{
			float num26 = num20 + num21 + num13;
			if (num26 > 0f)
			{
				float xd2 = num10;
				float yd2 = num11 - (float)(num8 | 1);
				float zd2 = num12 - (float)(num9 | 1);
				num14 += num26 * num26 * (num26 * num26) * FastNoiseLite.GradCoord(seed, num + (num7 & 501125321), num2 + (~num8 & 1136930381), num3 + (~num9 & 1720413743), xd2, yd2, zd2);
			}
			float num27 = num22 + num18;
			if (num27 > 0f)
			{
				float xd3 = (float)(num7 | 1) + num15;
				float yd3 = num16;
				float zd3 = num17;
				num14 += num27 * num27 * (num27 * num27) * FastNoiseLite.GradCoord(seed2, num + (num7 & 1002250642), num2 + 1136930381, num3 + 1720413743, xd3, yd3, zd3);
				flag = true;
			}
		}
		bool flag2 = false;
		float num28 = num20 + num13;
		if (num28 > 0f)
		{
			float xd4 = num10;
			float yd4 = num11 - (float)(num8 | 1);
			float zd4 = num12;
			num14 += num28 * num28 * (num28 * num28) * FastNoiseLite.GradCoord(seed, num + (num7 & 501125321), num2 + (~num8 & 1136930381), num3 + (num9 & 1720413743), xd4, yd4, zd4);
		}
		else
		{
			float num29 = num19 + num21 + num13;
			if (num29 > 0f)
			{
				float xd5 = num10 - (float)(num7 | 1);
				float yd5 = num11;
				float zd5 = num12 - (float)(num9 | 1);
				num14 += num29 * num29 * (num29 * num29) * FastNoiseLite.GradCoord(seed, num + (~num7 & 501125321), num2 + (num8 & 1136930381), num3 + (~num9 & 1720413743), xd5, yd5, zd5);
			}
			float num30 = num23 + num18;
			if (num30 > 0f)
			{
				float xd6 = num15;
				float yd6 = (float)(num8 | 1) + num16;
				float zd6 = num17;
				num14 += num30 * num30 * (num30 * num30) * FastNoiseLite.GradCoord(seed2, num + 501125321, num2 + (num8 & -2021106534), num3 + 1720413743, xd6, yd6, zd6);
				flag2 = true;
			}
		}
		bool flag3 = false;
		float num31 = num21 + num13;
		if (num31 > 0f)
		{
			float xd7 = num10;
			float yd7 = num11;
			float zd7 = num12 - (float)(num9 | 1);
			num14 += num31 * num31 * (num31 * num31) * FastNoiseLite.GradCoord(seed, num + (num7 & 501125321), num2 + (num8 & 1136930381), num3 + (~num9 & 1720413743), xd7, yd7, zd7);
		}
		else
		{
			float num32 = num19 + num20 + num13;
			if (num32 > 0f)
			{
				float xd8 = num10 - (float)(num7 | 1);
				float yd8 = num11 - (float)(num8 | 1);
				float zd8 = num12;
				num14 += num32 * num32 * (num32 * num32) * FastNoiseLite.GradCoord(seed, num + (~num7 & 501125321), num2 + (~num8 & 1136930381), num3 + (num9 & 1720413743), xd8, yd8, zd8);
			}
			float num33 = num24 + num18;
			if (num33 > 0f)
			{
				float xd9 = num15;
				float yd9 = num16;
				float zd9 = (float)(num9 | 1) + num17;
				num14 += num33 * num33 * (num33 * num33) * FastNoiseLite.GradCoord(seed2, num + 501125321, num2 + 1136930381, num3 + (num9 & -854139810), xd9, yd9, zd9);
				flag3 = true;
			}
		}
		if (!flag)
		{
			float num34 = num23 + num24 + num18;
			if (num34 > 0f)
			{
				float xd10 = num15;
				float yd10 = (float)(num8 | 1) + num16;
				float zd10 = (float)(num9 | 1) + num17;
				num14 += num34 * num34 * (num34 * num34) * FastNoiseLite.GradCoord(seed2, num + 501125321, num2 + (num8 & -2021106534), num3 + (num9 & -854139810), xd10, yd10, zd10);
			}
		}
		if (!flag2)
		{
			float num35 = num22 + num24 + num18;
			if (num35 > 0f)
			{
				float xd11 = (float)(num7 | 1) + num15;
				float yd11 = num16;
				float zd11 = (float)(num9 | 1) + num17;
				num14 += num35 * num35 * (num35 * num35) * FastNoiseLite.GradCoord(seed2, num + (num7 & 1002250642), num2 + 1136930381, num3 + (num9 & -854139810), xd11, yd11, zd11);
			}
		}
		if (!flag3)
		{
			float num36 = num22 + num23 + num18;
			if (num36 > 0f)
			{
				float xd12 = (float)(num7 | 1) + num15;
				float yd12 = (float)(num8 | 1) + num16;
				float zd12 = num17;
				num14 += num36 * num36 * (num36 * num36) * FastNoiseLite.GradCoord(seed2, num + (num7 & 1002250642), num2 + (num8 & -2021106534), num3 + 1720413743, xd12, yd12, zd12);
			}
		}
		return num14 * 9.046026f;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x000075C0 File Offset: 0x000057C0
	private float SingleCellular(int seed, float x, float y)
	{
		int num = FastNoiseLite.FastRound(x);
		int num2 = FastNoiseLite.FastRound(y);
		float num3 = float.MaxValue;
		float num4 = float.MaxValue;
		int num5 = 0;
		float num6 = 0.437015951f * this.mCellularJitterModifier;
		int num7 = (num - 1) * 501125321;
		int num8 = (num2 - 1) * 1136930381;
		switch (this.mCellularDistanceFunction)
		{
		default:
			for (int i = num - 1; i <= num + 1; i++)
			{
				int num9 = num8;
				for (int j = num2 - 1; j <= num2 + 1; j++)
				{
					int num10 = FastNoiseLite.Hash(seed, num7, num9);
					int num11 = num10 & 510;
					float num12 = (float)i - x + FastNoiseLite.RandVecs2D[num11] * num6;
					float num13 = (float)j - y + FastNoiseLite.RandVecs2D[num11 | 1] * num6;
					float num14 = num12 * num12 + num13 * num13;
					num4 = FastNoiseLite.FastMax(FastNoiseLite.FastMin(num4, num14), num3);
					if (num14 < num3)
					{
						num3 = num14;
						num5 = num10;
					}
					num9 += 1136930381;
				}
				num7 += 501125321;
			}
			break;
		case FastNoiseLite.CellularDistanceFunction.Manhattan:
			for (int k = num - 1; k <= num + 1; k++)
			{
				int num15 = num8;
				for (int l = num2 - 1; l <= num2 + 1; l++)
				{
					int num16 = FastNoiseLite.Hash(seed, num7, num15);
					int num17 = num16 & 510;
					float f = (float)k - x + FastNoiseLite.RandVecs2D[num17] * num6;
					float f2 = (float)l - y + FastNoiseLite.RandVecs2D[num17 | 1] * num6;
					float num18 = FastNoiseLite.FastAbs(f) + FastNoiseLite.FastAbs(f2);
					num4 = FastNoiseLite.FastMax(FastNoiseLite.FastMin(num4, num18), num3);
					if (num18 < num3)
					{
						num3 = num18;
						num5 = num16;
					}
					num15 += 1136930381;
				}
				num7 += 501125321;
			}
			break;
		case FastNoiseLite.CellularDistanceFunction.Hybrid:
			for (int m = num - 1; m <= num + 1; m++)
			{
				int num19 = num8;
				for (int n = num2 - 1; n <= num2 + 1; n++)
				{
					int num20 = FastNoiseLite.Hash(seed, num7, num19);
					int num21 = num20 & 510;
					float num22 = (float)m - x + FastNoiseLite.RandVecs2D[num21] * num6;
					float num23 = (float)n - y + FastNoiseLite.RandVecs2D[num21 | 1] * num6;
					float num24 = FastNoiseLite.FastAbs(num22) + FastNoiseLite.FastAbs(num23) + (num22 * num22 + num23 * num23);
					num4 = FastNoiseLite.FastMax(FastNoiseLite.FastMin(num4, num24), num3);
					if (num24 < num3)
					{
						num3 = num24;
						num5 = num20;
					}
					num19 += 1136930381;
				}
				num7 += 501125321;
			}
			break;
		}
		if (this.mCellularDistanceFunction == FastNoiseLite.CellularDistanceFunction.Euclidean && this.mCellularReturnType >= FastNoiseLite.CellularReturnType.Distance)
		{
			num3 = FastNoiseLite.FastSqrt(num3);
			if (this.mCellularReturnType >= FastNoiseLite.CellularReturnType.Distance2)
			{
				num4 = FastNoiseLite.FastSqrt(num4);
			}
		}
		switch (this.mCellularReturnType)
		{
		case FastNoiseLite.CellularReturnType.CellValue:
			return (float)num5 * 4.656613E-10f;
		case FastNoiseLite.CellularReturnType.Distance:
			return num3 - 1f;
		case FastNoiseLite.CellularReturnType.Distance2:
			return num4 - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Add:
			return (num4 + num3) * 0.5f - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Sub:
			return num4 - num3 - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Mul:
			return num4 * num3 * 0.5f - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Div:
			return num3 / num4 - 1f;
		default:
			return 0f;
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000078FC File Offset: 0x00005AFC
	private float SingleCellular(int seed, float x, float y, float z)
	{
		int num = FastNoiseLite.FastRound(x);
		int num2 = FastNoiseLite.FastRound(y);
		int num3 = FastNoiseLite.FastRound(z);
		float num4 = float.MaxValue;
		float num5 = float.MaxValue;
		int num6 = 0;
		float num7 = 0.396143526f * this.mCellularJitterModifier;
		int num8 = (num - 1) * 501125321;
		int num9 = (num2 - 1) * 1136930381;
		int num10 = (num3 - 1) * 1720413743;
		switch (this.mCellularDistanceFunction)
		{
		case FastNoiseLite.CellularDistanceFunction.Euclidean:
		case FastNoiseLite.CellularDistanceFunction.EuclideanSq:
			for (int i = num - 1; i <= num + 1; i++)
			{
				int num11 = num9;
				for (int j = num2 - 1; j <= num2 + 1; j++)
				{
					int num12 = num10;
					for (int k = num3 - 1; k <= num3 + 1; k++)
					{
						int num13 = FastNoiseLite.Hash(seed, num8, num11, num12);
						int num14 = num13 & 1020;
						float num15 = (float)i - x + FastNoiseLite.RandVecs3D[num14] * num7;
						float num16 = (float)j - y + FastNoiseLite.RandVecs3D[num14 | 1] * num7;
						float num17 = (float)k - z + FastNoiseLite.RandVecs3D[num14 | 2] * num7;
						float num18 = num15 * num15 + num16 * num16 + num17 * num17;
						num5 = FastNoiseLite.FastMax(FastNoiseLite.FastMin(num5, num18), num4);
						if (num18 < num4)
						{
							num4 = num18;
							num6 = num13;
						}
						num12 += 1720413743;
					}
					num11 += 1136930381;
				}
				num8 += 501125321;
			}
			break;
		case FastNoiseLite.CellularDistanceFunction.Manhattan:
			for (int l = num - 1; l <= num + 1; l++)
			{
				int num19 = num9;
				for (int m = num2 - 1; m <= num2 + 1; m++)
				{
					int num20 = num10;
					for (int n = num3 - 1; n <= num3 + 1; n++)
					{
						int num21 = FastNoiseLite.Hash(seed, num8, num19, num20);
						int num22 = num21 & 1020;
						float f = (float)l - x + FastNoiseLite.RandVecs3D[num22] * num7;
						float f2 = (float)m - y + FastNoiseLite.RandVecs3D[num22 | 1] * num7;
						float f3 = (float)n - z + FastNoiseLite.RandVecs3D[num22 | 2] * num7;
						float num23 = FastNoiseLite.FastAbs(f) + FastNoiseLite.FastAbs(f2) + FastNoiseLite.FastAbs(f3);
						num5 = FastNoiseLite.FastMax(FastNoiseLite.FastMin(num5, num23), num4);
						if (num23 < num4)
						{
							num4 = num23;
							num6 = num21;
						}
						num20 += 1720413743;
					}
					num19 += 1136930381;
				}
				num8 += 501125321;
			}
			break;
		case FastNoiseLite.CellularDistanceFunction.Hybrid:
			for (int num24 = num - 1; num24 <= num + 1; num24++)
			{
				int num25 = num9;
				for (int num26 = num2 - 1; num26 <= num2 + 1; num26++)
				{
					int num27 = num10;
					for (int num28 = num3 - 1; num28 <= num3 + 1; num28++)
					{
						int num29 = FastNoiseLite.Hash(seed, num8, num25, num27);
						int num30 = num29 & 1020;
						float num31 = (float)num24 - x + FastNoiseLite.RandVecs3D[num30] * num7;
						float num32 = (float)num26 - y + FastNoiseLite.RandVecs3D[num30 | 1] * num7;
						float num33 = (float)num28 - z + FastNoiseLite.RandVecs3D[num30 | 2] * num7;
						float num34 = FastNoiseLite.FastAbs(num31) + FastNoiseLite.FastAbs(num32) + FastNoiseLite.FastAbs(num33) + (num31 * num31 + num32 * num32 + num33 * num33);
						num5 = FastNoiseLite.FastMax(FastNoiseLite.FastMin(num5, num34), num4);
						if (num34 < num4)
						{
							num4 = num34;
							num6 = num29;
						}
						num27 += 1720413743;
					}
					num25 += 1136930381;
				}
				num8 += 501125321;
			}
			break;
		}
		if (this.mCellularDistanceFunction == FastNoiseLite.CellularDistanceFunction.Euclidean && this.mCellularReturnType >= FastNoiseLite.CellularReturnType.Distance)
		{
			num4 = FastNoiseLite.FastSqrt(num4);
			if (this.mCellularReturnType >= FastNoiseLite.CellularReturnType.Distance2)
			{
				num5 = FastNoiseLite.FastSqrt(num5);
			}
		}
		switch (this.mCellularReturnType)
		{
		case FastNoiseLite.CellularReturnType.CellValue:
			return (float)num6 * 4.656613E-10f;
		case FastNoiseLite.CellularReturnType.Distance:
			return num4 - 1f;
		case FastNoiseLite.CellularReturnType.Distance2:
			return num5 - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Add:
			return (num5 + num4) * 0.5f - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Sub:
			return num5 - num4 - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Mul:
			return num5 * num4 * 0.5f - 1f;
		case FastNoiseLite.CellularReturnType.Distance2Div:
			return num4 / num5 - 1f;
		default:
			return 0f;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00007D48 File Offset: 0x00005F48
	private float SinglePerlin(int seed, float x, float y)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		float num3 = x - (float)num;
		float num4 = y - (float)num2;
		float xd = num3 - 1f;
		float yd = num4 - 1f;
		float t = FastNoiseLite.InterpQuintic(num3);
		float t2 = FastNoiseLite.InterpQuintic(num4);
		num *= 501125321;
		num2 *= 1136930381;
		int xPrimed = num + 501125321;
		int yPrimed = num2 + 1136930381;
		float a = FastNoiseLite.Lerp(FastNoiseLite.GradCoord(seed, num, num2, num3, num4), FastNoiseLite.GradCoord(seed, xPrimed, num2, xd, num4), t);
		float b = FastNoiseLite.Lerp(FastNoiseLite.GradCoord(seed, num, yPrimed, num3, yd), FastNoiseLite.GradCoord(seed, xPrimed, yPrimed, xd, yd), t);
		return FastNoiseLite.Lerp(a, b, t2) * 1.42476916f;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00007E04 File Offset: 0x00006004
	private float SinglePerlin(int seed, float x, float y, float z)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		int num3 = FastNoiseLite.FastFloor(z);
		float num4 = x - (float)num;
		float num5 = y - (float)num2;
		float num6 = z - (float)num3;
		float xd = num4 - 1f;
		float yd = num5 - 1f;
		float zd = num6 - 1f;
		float t = FastNoiseLite.InterpQuintic(num4);
		float t2 = FastNoiseLite.InterpQuintic(num5);
		float t3 = FastNoiseLite.InterpQuintic(num6);
		num *= 501125321;
		num2 *= 1136930381;
		num3 *= 1720413743;
		int xPrimed = num + 501125321;
		int yPrimed = num2 + 1136930381;
		int zPrimed = num3 + 1720413743;
		float a = FastNoiseLite.Lerp(FastNoiseLite.GradCoord(seed, num, num2, num3, num4, num5, num6), FastNoiseLite.GradCoord(seed, xPrimed, num2, num3, xd, num5, num6), t);
		float b = FastNoiseLite.Lerp(FastNoiseLite.GradCoord(seed, num, yPrimed, num3, num4, yd, num6), FastNoiseLite.GradCoord(seed, xPrimed, yPrimed, num3, xd, yd, num6), t);
		float a2 = FastNoiseLite.Lerp(FastNoiseLite.GradCoord(seed, num, num2, zPrimed, num4, num5, zd), FastNoiseLite.GradCoord(seed, xPrimed, num2, zPrimed, xd, num5, zd), t);
		float b2 = FastNoiseLite.Lerp(FastNoiseLite.GradCoord(seed, num, yPrimed, zPrimed, num4, yd, zd), FastNoiseLite.GradCoord(seed, xPrimed, yPrimed, zPrimed, xd, yd, zd), t);
		float a3 = FastNoiseLite.Lerp(a, b, t2);
		float b3 = FastNoiseLite.Lerp(a2, b2, t2);
		return FastNoiseLite.Lerp(a3, b3, t3) * 0.9649214f;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00007F70 File Offset: 0x00006170
	private float SingleValueCubic(int seed, float x, float y)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		float t = x - (float)num;
		float t2 = y - (float)num2;
		num *= 501125321;
		num2 *= 1136930381;
		int xPrimed = num - 501125321;
		int yPrimed = num2 - 1136930381;
		int xPrimed2 = num + 501125321;
		int yPrimed2 = num2 + 1136930381;
		int xPrimed3 = num + 1002250642;
		int yPrimed3 = num2 + -2021106534;
		return FastNoiseLite.CubicLerp(FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed), FastNoiseLite.ValCoord(seed, num, yPrimed), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, num2), FastNoiseLite.ValCoord(seed, num, num2), FastNoiseLite.ValCoord(seed, xPrimed2, num2), FastNoiseLite.ValCoord(seed, xPrimed3, num2), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed2), FastNoiseLite.ValCoord(seed, num, yPrimed2), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed2), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed2), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed3), FastNoiseLite.ValCoord(seed, num, yPrimed3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed3), t), t2) * 0.444444448f;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0000809C File Offset: 0x0000629C
	private float SingleValueCubic(int seed, float x, float y, float z)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		int num3 = FastNoiseLite.FastFloor(z);
		float t = x - (float)num;
		float t2 = y - (float)num2;
		float t3 = z - (float)num3;
		num *= 501125321;
		num2 *= 1136930381;
		num3 *= 1720413743;
		int xPrimed = num - 501125321;
		int yPrimed = num2 - 1136930381;
		int zPrimed = num3 - 1720413743;
		int xPrimed2 = num + 501125321;
		int yPrimed2 = num2 + 1136930381;
		int zPrimed2 = num3 + 1720413743;
		int xPrimed3 = num + 1002250642;
		int yPrimed3 = num2 + -2021106534;
		int zPrimed3 = num3 + -854139810;
		return FastNoiseLite.CubicLerp(FastNoiseLite.CubicLerp(FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed, zPrimed), FastNoiseLite.ValCoord(seed, num, yPrimed, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed, zPrimed), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, num2, zPrimed), FastNoiseLite.ValCoord(seed, num, num2, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed2, num2, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed3, num2, zPrimed), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed2, zPrimed), FastNoiseLite.ValCoord(seed, num, yPrimed2, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed2, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed2, zPrimed), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed3, zPrimed), FastNoiseLite.ValCoord(seed, num, yPrimed3, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed3, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed3, zPrimed), t), t2), FastNoiseLite.CubicLerp(FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed, num3), FastNoiseLite.ValCoord(seed, num, yPrimed, num3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed, num3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed, num3), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, num2, num3), FastNoiseLite.ValCoord(seed, num, num2, num3), FastNoiseLite.ValCoord(seed, xPrimed2, num2, num3), FastNoiseLite.ValCoord(seed, xPrimed3, num2, num3), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed2, num3), FastNoiseLite.ValCoord(seed, num, yPrimed2, num3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed2, num3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed2, num3), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed3, num3), FastNoiseLite.ValCoord(seed, num, yPrimed3, num3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed3, num3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed3, num3), t), t2), FastNoiseLite.CubicLerp(FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed, zPrimed2), FastNoiseLite.ValCoord(seed, num, yPrimed, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed, zPrimed2), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, num2, zPrimed2), FastNoiseLite.ValCoord(seed, num, num2, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed2, num2, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed3, num2, zPrimed2), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed2, zPrimed2), FastNoiseLite.ValCoord(seed, num, yPrimed2, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed2, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed2, zPrimed2), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed3, zPrimed2), FastNoiseLite.ValCoord(seed, num, yPrimed3, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed3, zPrimed2), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed3, zPrimed2), t), t2), FastNoiseLite.CubicLerp(FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed, zPrimed3), FastNoiseLite.ValCoord(seed, num, yPrimed, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed, zPrimed3), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, num2, zPrimed3), FastNoiseLite.ValCoord(seed, num, num2, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed2, num2, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed3, num2, zPrimed3), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed2, zPrimed3), FastNoiseLite.ValCoord(seed, num, yPrimed2, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed2, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed2, zPrimed3), t), FastNoiseLite.CubicLerp(FastNoiseLite.ValCoord(seed, xPrimed, yPrimed3, zPrimed3), FastNoiseLite.ValCoord(seed, num, yPrimed3, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed2, yPrimed3, zPrimed3), FastNoiseLite.ValCoord(seed, xPrimed3, yPrimed3, zPrimed3), t), t2), t3) * 0.2962963f;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00008498 File Offset: 0x00006698
	private float SingleValue(int seed, float x, float y)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		float t = FastNoiseLite.InterpHermite(x - (float)num);
		float t2 = FastNoiseLite.InterpHermite(y - (float)num2);
		num *= 501125321;
		num2 *= 1136930381;
		int xPrimed = num + 501125321;
		int yPrimed = num2 + 1136930381;
		float a = FastNoiseLite.Lerp(FastNoiseLite.ValCoord(seed, num, num2), FastNoiseLite.ValCoord(seed, xPrimed, num2), t);
		float b = FastNoiseLite.Lerp(FastNoiseLite.ValCoord(seed, num, yPrimed), FastNoiseLite.ValCoord(seed, xPrimed, yPrimed), t);
		return FastNoiseLite.Lerp(a, b, t2);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00008528 File Offset: 0x00006728
	private float SingleValue(int seed, float x, float y, float z)
	{
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		int num3 = FastNoiseLite.FastFloor(z);
		float t = FastNoiseLite.InterpHermite(x - (float)num);
		float t2 = FastNoiseLite.InterpHermite(y - (float)num2);
		float t3 = FastNoiseLite.InterpHermite(z - (float)num3);
		num *= 501125321;
		num2 *= 1136930381;
		num3 *= 1720413743;
		int xPrimed = num + 501125321;
		int yPrimed = num2 + 1136930381;
		int zPrimed = num3 + 1720413743;
		float a = FastNoiseLite.Lerp(FastNoiseLite.ValCoord(seed, num, num2, num3), FastNoiseLite.ValCoord(seed, xPrimed, num2, num3), t);
		float b = FastNoiseLite.Lerp(FastNoiseLite.ValCoord(seed, num, yPrimed, num3), FastNoiseLite.ValCoord(seed, xPrimed, yPrimed, num3), t);
		float a2 = FastNoiseLite.Lerp(FastNoiseLite.ValCoord(seed, num, num2, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed, num2, zPrimed), t);
		float b2 = FastNoiseLite.Lerp(FastNoiseLite.ValCoord(seed, num, yPrimed, zPrimed), FastNoiseLite.ValCoord(seed, xPrimed, yPrimed, zPrimed), t);
		float a3 = FastNoiseLite.Lerp(a, b, t2);
		float b3 = FastNoiseLite.Lerp(a2, b2, t2);
		return FastNoiseLite.Lerp(a3, b3, t3);
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00008634 File Offset: 0x00006834
	private void DoSingleDomainWarp(int seed, float amp, float freq, float x, float y, ref float xr, ref float yr)
	{
		switch (this.mDomainWarpType)
		{
		case FastNoiseLite.DomainWarpType.OpenSimplex2:
			this.SingleDomainWarpSimplexGradient(seed, amp * 38.2836876f, freq, x, y, ref xr, ref yr, false);
			return;
		case FastNoiseLite.DomainWarpType.OpenSimplex2Reduced:
			this.SingleDomainWarpSimplexGradient(seed, amp * 16f, freq, x, y, ref xr, ref yr, true);
			return;
		case FastNoiseLite.DomainWarpType.BasicGrid:
			this.SingleDomainWarpBasicGrid(seed, amp, freq, x, y, ref xr, ref yr);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000086A0 File Offset: 0x000068A0
	private void DoSingleDomainWarp(int seed, float amp, float freq, float x, float y, float z, ref float xr, ref float yr, ref float zr)
	{
		switch (this.mDomainWarpType)
		{
		case FastNoiseLite.DomainWarpType.OpenSimplex2:
			this.SingleDomainWarpOpenSimplex2Gradient(seed, amp * 32.6942825f, freq, x, y, z, ref xr, ref yr, ref zr, false);
			return;
		case FastNoiseLite.DomainWarpType.OpenSimplex2Reduced:
			this.SingleDomainWarpOpenSimplex2Gradient(seed, amp * 7.716049f, freq, x, y, z, ref xr, ref yr, ref zr, true);
			return;
		case FastNoiseLite.DomainWarpType.BasicGrid:
			this.SingleDomainWarpBasicGrid(seed, amp, freq, x, y, z, ref xr, ref yr, ref zr);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00008718 File Offset: 0x00006918
	private void DomainWarpSingle(ref float x, ref float y)
	{
		int seed = this.mSeed;
		float amp = this.mDomainWarpAmp * this.mFractalBounding;
		float freq = this.mFrequency;
		float x2 = x;
		float y2 = y;
		this.TransformDomainWarpCoordinate(ref x2, ref y2);
		this.DoSingleDomainWarp(seed, amp, freq, x2, y2, ref x, ref y);
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00008760 File Offset: 0x00006960
	private void DomainWarpSingle(ref float x, ref float y, ref float z)
	{
		int seed = this.mSeed;
		float amp = this.mDomainWarpAmp * this.mFractalBounding;
		float freq = this.mFrequency;
		float x2 = x;
		float y2 = y;
		float z2 = z;
		this.TransformDomainWarpCoordinate(ref x2, ref y2, ref z2);
		this.DoSingleDomainWarp(seed, amp, freq, x2, y2, z2, ref x, ref y, ref z);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000087B4 File Offset: 0x000069B4
	private void DomainWarpFractalProgressive(ref float x, ref float y)
	{
		int num = this.mSeed;
		float num2 = this.mDomainWarpAmp * this.mFractalBounding;
		float num3 = this.mFrequency;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float x2 = x;
			float y2 = y;
			this.TransformDomainWarpCoordinate(ref x2, ref y2);
			this.DoSingleDomainWarp(num, num2, num3, x2, y2, ref x, ref y);
			num++;
			num2 *= this.mGain;
			num3 *= this.mLacunarity;
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00008828 File Offset: 0x00006A28
	private void DomainWarpFractalProgressive(ref float x, ref float y, ref float z)
	{
		int num = this.mSeed;
		float num2 = this.mDomainWarpAmp * this.mFractalBounding;
		float num3 = this.mFrequency;
		for (int i = 0; i < this.mOctaves; i++)
		{
			float x2 = x;
			float y2 = y;
			float z2 = z;
			this.TransformDomainWarpCoordinate(ref x2, ref y2, ref z2);
			this.DoSingleDomainWarp(num, num2, num3, x2, y2, z2, ref x, ref y, ref z);
			num++;
			num2 *= this.mGain;
			num3 *= this.mLacunarity;
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000088A4 File Offset: 0x00006AA4
	private void DomainWarpFractalIndependent(ref float x, ref float y)
	{
		float x2 = x;
		float y2 = y;
		this.TransformDomainWarpCoordinate(ref x2, ref y2);
		int num = this.mSeed;
		float num2 = this.mDomainWarpAmp * this.mFractalBounding;
		float num3 = this.mFrequency;
		for (int i = 0; i < this.mOctaves; i++)
		{
			this.DoSingleDomainWarp(num, num2, num3, x2, y2, ref x, ref y);
			num++;
			num2 *= this.mGain;
			num3 *= this.mLacunarity;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0000891C File Offset: 0x00006B1C
	private void DomainWarpFractalIndependent(ref float x, ref float y, ref float z)
	{
		float x2 = x;
		float y2 = y;
		float z2 = z;
		this.TransformDomainWarpCoordinate(ref x2, ref y2, ref z2);
		int num = this.mSeed;
		float num2 = this.mDomainWarpAmp * this.mFractalBounding;
		float num3 = this.mFrequency;
		for (int i = 0; i < this.mOctaves; i++)
		{
			this.DoSingleDomainWarp(num, num2, num3, x2, y2, z2, ref x, ref y, ref z);
			num++;
			num2 *= this.mGain;
			num3 *= this.mLacunarity;
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0000899C File Offset: 0x00006B9C
	private void SingleDomainWarpBasicGrid(int seed, float warpAmp, float frequency, float x, float y, ref float xr, ref float yr)
	{
		float num = x * frequency;
		float num2 = y * frequency;
		int num3 = FastNoiseLite.FastFloor(num);
		int num4 = FastNoiseLite.FastFloor(num2);
		float t = FastNoiseLite.InterpHermite(num - (float)num3);
		float t2 = FastNoiseLite.InterpHermite(num2 - (float)num4);
		num3 *= 501125321;
		num4 *= 1136930381;
		int xPrimed = num3 + 501125321;
		int yPrimed = num4 + 1136930381;
		int num5 = FastNoiseLite.Hash(seed, num3, num4) & 510;
		int num6 = FastNoiseLite.Hash(seed, xPrimed, num4) & 510;
		float a = FastNoiseLite.Lerp(FastNoiseLite.RandVecs2D[num5], FastNoiseLite.RandVecs2D[num6], t);
		float a2 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs2D[num5 | 1], FastNoiseLite.RandVecs2D[num6 | 1], t);
		num5 = (FastNoiseLite.Hash(seed, num3, yPrimed) & 510);
		num6 = (FastNoiseLite.Hash(seed, xPrimed, yPrimed) & 510);
		float b = FastNoiseLite.Lerp(FastNoiseLite.RandVecs2D[num5], FastNoiseLite.RandVecs2D[num6], t);
		float b2 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs2D[num5 | 1], FastNoiseLite.RandVecs2D[num6 | 1], t);
		xr += FastNoiseLite.Lerp(a, b, t2) * warpAmp;
		yr += FastNoiseLite.Lerp(a2, b2, t2) * warpAmp;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00008ACC File Offset: 0x00006CCC
	private void SingleDomainWarpBasicGrid(int seed, float warpAmp, float frequency, float x, float y, float z, ref float xr, ref float yr, ref float zr)
	{
		float num = x * frequency;
		float num2 = y * frequency;
		float num3 = z * frequency;
		int num4 = FastNoiseLite.FastFloor(num);
		int num5 = FastNoiseLite.FastFloor(num2);
		int num6 = FastNoiseLite.FastFloor(num3);
		float t = FastNoiseLite.InterpHermite(num - (float)num4);
		float t2 = FastNoiseLite.InterpHermite(num2 - (float)num5);
		float t3 = FastNoiseLite.InterpHermite(num3 - (float)num6);
		num4 *= 501125321;
		num5 *= 1136930381;
		num6 *= 1720413743;
		int xPrimed = num4 + 501125321;
		int yPrimed = num5 + 1136930381;
		int zPrimed = num6 + 1720413743;
		int num7 = FastNoiseLite.Hash(seed, num4, num5, num6) & 1020;
		int num8 = FastNoiseLite.Hash(seed, xPrimed, num5, num6) & 1020;
		float a = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7], FastNoiseLite.RandVecs3D[num8], t);
		float a2 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 1], FastNoiseLite.RandVecs3D[num8 | 1], t);
		float a3 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 2], FastNoiseLite.RandVecs3D[num8 | 2], t);
		num7 = (FastNoiseLite.Hash(seed, num4, yPrimed, num6) & 1020);
		num8 = (FastNoiseLite.Hash(seed, xPrimed, yPrimed, num6) & 1020);
		float b = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7], FastNoiseLite.RandVecs3D[num8], t);
		float b2 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 1], FastNoiseLite.RandVecs3D[num8 | 1], t);
		float b3 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 2], FastNoiseLite.RandVecs3D[num8 | 2], t);
		float a4 = FastNoiseLite.Lerp(a, b, t2);
		float a5 = FastNoiseLite.Lerp(a2, b2, t2);
		float a6 = FastNoiseLite.Lerp(a3, b3, t2);
		num7 = (FastNoiseLite.Hash(seed, num4, num5, zPrimed) & 1020);
		num8 = (FastNoiseLite.Hash(seed, xPrimed, num5, zPrimed) & 1020);
		a = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7], FastNoiseLite.RandVecs3D[num8], t);
		a2 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 1], FastNoiseLite.RandVecs3D[num8 | 1], t);
		a3 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 2], FastNoiseLite.RandVecs3D[num8 | 2], t);
		num7 = (FastNoiseLite.Hash(seed, num4, yPrimed, zPrimed) & 1020);
		num8 = (FastNoiseLite.Hash(seed, xPrimed, yPrimed, zPrimed) & 1020);
		b = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7], FastNoiseLite.RandVecs3D[num8], t);
		b2 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 1], FastNoiseLite.RandVecs3D[num8 | 1], t);
		b3 = FastNoiseLite.Lerp(FastNoiseLite.RandVecs3D[num7 | 2], FastNoiseLite.RandVecs3D[num8 | 2], t);
		xr += FastNoiseLite.Lerp(a4, FastNoiseLite.Lerp(a, b, t2), t3) * warpAmp;
		yr += FastNoiseLite.Lerp(a5, FastNoiseLite.Lerp(a2, b2, t2), t3) * warpAmp;
		zr += FastNoiseLite.Lerp(a6, FastNoiseLite.Lerp(a3, b3, t2), t3) * warpAmp;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00008DBC File Offset: 0x00006FBC
	private void SingleDomainWarpSimplexGradient(int seed, float warpAmp, float frequency, float x, float y, ref float xr, ref float yr, bool outGradOnly)
	{
		x *= frequency;
		y *= frequency;
		int num = FastNoiseLite.FastFloor(x);
		int num2 = FastNoiseLite.FastFloor(y);
		float num3 = x - (float)num;
		float num4 = y - (float)num2;
		float num5 = (num3 + num4) * 0.211324871f;
		float num6 = num3 - num5;
		float num7 = num4 - num5;
		num *= 501125321;
		num2 *= 1136930381;
		float num9;
		float num8 = num9 = 0f;
		float num10 = 0.5f - num6 * num6 - num7 * num7;
		if (num10 > 0f)
		{
			float num11 = num10 * num10 * (num10 * num10);
			float num12;
			float num13;
			if (outGradOnly)
			{
				FastNoiseLite.GradCoordOut(seed, num, num2, out num12, out num13);
			}
			else
			{
				FastNoiseLite.GradCoordDual(seed, num, num2, num6, num7, out num12, out num13);
			}
			num9 += num11 * num12;
			num8 += num11 * num13;
		}
		float num14 = 3.15470052f * num5 + (-0.6666666f + num10);
		if (num14 > 0f)
		{
			float xd = num6 + -0.577350259f;
			float yd = num7 + -0.577350259f;
			float num15 = num14 * num14 * (num14 * num14);
			float num16;
			float num17;
			if (outGradOnly)
			{
				FastNoiseLite.GradCoordOut(seed, num + 501125321, num2 + 1136930381, out num16, out num17);
			}
			else
			{
				FastNoiseLite.GradCoordDual(seed, num + 501125321, num2 + 1136930381, xd, yd, out num16, out num17);
			}
			num9 += num15 * num16;
			num8 += num15 * num17;
		}
		if (num7 > num6)
		{
			float num18 = num6 + 0.211324871f;
			float num19 = num7 + -0.7886751f;
			float num20 = 0.5f - num18 * num18 - num19 * num19;
			if (num20 > 0f)
			{
				float num21 = num20 * num20 * (num20 * num20);
				float num22;
				float num23;
				if (outGradOnly)
				{
					FastNoiseLite.GradCoordOut(seed, num, num2 + 1136930381, out num22, out num23);
				}
				else
				{
					FastNoiseLite.GradCoordDual(seed, num, num2 + 1136930381, num18, num19, out num22, out num23);
				}
				num9 += num21 * num22;
				num8 += num21 * num23;
			}
		}
		else
		{
			float num24 = num6 + -0.7886751f;
			float num25 = num7 + 0.211324871f;
			float num26 = 0.5f - num24 * num24 - num25 * num25;
			if (num26 > 0f)
			{
				float num27 = num26 * num26 * (num26 * num26);
				float num28;
				float num29;
				if (outGradOnly)
				{
					FastNoiseLite.GradCoordOut(seed, num + 501125321, num2, out num28, out num29);
				}
				else
				{
					FastNoiseLite.GradCoordDual(seed, num + 501125321, num2, num24, num25, out num28, out num29);
				}
				num9 += num27 * num28;
				num8 += num27 * num29;
			}
		}
		xr += num9 * warpAmp;
		yr += num8 * warpAmp;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000902C File Offset: 0x0000722C
	private void SingleDomainWarpOpenSimplex2Gradient(int seed, float warpAmp, float frequency, float x, float y, float z, ref float xr, ref float yr, ref float zr, bool outGradOnly)
	{
		x *= frequency;
		y *= frequency;
		z *= frequency;
		int num = FastNoiseLite.FastRound(x);
		int num2 = FastNoiseLite.FastRound(y);
		int num3 = FastNoiseLite.FastRound(z);
		float num4 = x - (float)num;
		float num5 = y - (float)num2;
		float num6 = z - (float)num3;
		int num7 = (int)(-num4 - 1f) | 1;
		int num8 = (int)(-num5 - 1f) | 1;
		int num9 = (int)(-num6 - 1f) | 1;
		float num10 = (float)num7 * -num4;
		float num11 = (float)num8 * -num5;
		float num12 = (float)num9 * -num6;
		num *= 501125321;
		num2 *= 1136930381;
		num3 *= 1720413743;
		float num15;
		float num14;
		float num13 = num14 = (num15 = 0f);
		float num16 = 0.6f - num4 * num4 - (num5 * num5 + num6 * num6);
		int num17 = 0;
		for (;;)
		{
			if (num16 > 0f)
			{
				float num18 = num16 * num16 * (num16 * num16);
				float num19;
				float num20;
				float num21;
				if (outGradOnly)
				{
					FastNoiseLite.GradCoordOut(seed, num, num2, num3, out num19, out num20, out num21);
				}
				else
				{
					FastNoiseLite.GradCoordDual(seed, num, num2, num3, num4, num5, num6, out num19, out num20, out num21);
				}
				num14 += num18 * num19;
				num13 += num18 * num20;
				num15 += num18 * num21;
			}
			float num22 = num16;
			int num23 = num;
			int num24 = num2;
			int num25 = num3;
			float num26 = num4;
			float num27 = num5;
			float num28 = num6;
			if (num10 >= num11 && num10 >= num12)
			{
				num26 += (float)num7;
				num22 = num22 + num10 + num10;
				num23 -= num7 * 501125321;
			}
			else if (num11 > num10 && num11 >= num12)
			{
				num27 += (float)num8;
				num22 = num22 + num11 + num11;
				num24 -= num8 * 1136930381;
			}
			else
			{
				num28 += (float)num9;
				num22 = num22 + num12 + num12;
				num25 -= num9 * 1720413743;
			}
			if (num22 > 1f)
			{
				num22 -= 1f;
				float num29 = num22 * num22 * (num22 * num22);
				float num30;
				float num31;
				float num32;
				if (outGradOnly)
				{
					FastNoiseLite.GradCoordOut(seed, num23, num24, num25, out num30, out num31, out num32);
				}
				else
				{
					FastNoiseLite.GradCoordDual(seed, num23, num24, num25, num26, num27, num28, out num30, out num31, out num32);
				}
				num14 += num29 * num30;
				num13 += num29 * num31;
				num15 += num29 * num32;
			}
			if (num17 == 1)
			{
				break;
			}
			num10 = 0.5f - num10;
			num11 = 0.5f - num11;
			num12 = 0.5f - num12;
			num4 = (float)num7 * num10;
			num5 = (float)num8 * num11;
			num6 = (float)num9 * num12;
			num16 += 0.75f - num10 - (num11 + num12);
			num += (num7 >> 1 & 501125321);
			num2 += (num8 >> 1 & 1136930381);
			num3 += (num9 >> 1 & 1720413743);
			num7 = -num7;
			num8 = -num8;
			num9 = -num9;
			seed += 1293373;
			num17++;
		}
		xr += num14 * warpAmp;
		yr += num13 * warpAmp;
		zr += num15 * warpAmp;
	}

	// Token: 0x04000085 RID: 133
	private const short INLINE = 256;

	// Token: 0x04000086 RID: 134
	private const short OPTIMISE = 512;

	// Token: 0x04000087 RID: 135
	private int mSeed = 1337;

	// Token: 0x04000088 RID: 136
	private float mFrequency = 0.01f;

	// Token: 0x04000089 RID: 137
	private FastNoiseLite.NoiseType mNoiseType;

	// Token: 0x0400008A RID: 138
	private FastNoiseLite.RotationType3D mRotationType3D;

	// Token: 0x0400008B RID: 139
	private FastNoiseLite.TransformType3D mTransformType3D = FastNoiseLite.TransformType3D.DefaultOpenSimplex2;

	// Token: 0x0400008C RID: 140
	private FastNoiseLite.FractalType mFractalType;

	// Token: 0x0400008D RID: 141
	private int mOctaves = 3;

	// Token: 0x0400008E RID: 142
	private float mLacunarity = 2f;

	// Token: 0x0400008F RID: 143
	private float mGain = 0.5f;

	// Token: 0x04000090 RID: 144
	private float mWeightedStrength;

	// Token: 0x04000091 RID: 145
	private float mPingPongStrength = 2f;

	// Token: 0x04000092 RID: 146
	private float mFractalBounding = 0.5714286f;

	// Token: 0x04000093 RID: 147
	private FastNoiseLite.CellularDistanceFunction mCellularDistanceFunction = FastNoiseLite.CellularDistanceFunction.EuclideanSq;

	// Token: 0x04000094 RID: 148
	private FastNoiseLite.CellularReturnType mCellularReturnType = FastNoiseLite.CellularReturnType.Distance;

	// Token: 0x04000095 RID: 149
	private float mCellularJitterModifier = 1f;

	// Token: 0x04000096 RID: 150
	private FastNoiseLite.DomainWarpType mDomainWarpType;

	// Token: 0x04000097 RID: 151
	private FastNoiseLite.TransformType3D mWarpTransformType3D = FastNoiseLite.TransformType3D.DefaultOpenSimplex2;

	// Token: 0x04000098 RID: 152
	private float mDomainWarpAmp = 1f;

	// Token: 0x04000099 RID: 153
	private static readonly float[] Gradients2D = new float[]
	{
		0.130526185f,
		0.9914449f,
		0.382683426f,
		0.9238795f,
		0.6087614f,
		0.7933533f,
		0.7933533f,
		0.6087614f,
		0.9238795f,
		0.382683426f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		-0.130526185f,
		0.9238795f,
		-0.382683426f,
		0.7933533f,
		-0.6087614f,
		0.6087614f,
		-0.7933533f,
		0.382683426f,
		-0.9238795f,
		0.130526185f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		-0.382683426f,
		-0.9238795f,
		-0.6087614f,
		-0.7933533f,
		-0.7933533f,
		-0.6087614f,
		-0.9238795f,
		-0.382683426f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		0.130526185f,
		-0.9238795f,
		0.382683426f,
		-0.7933533f,
		0.6087614f,
		-0.6087614f,
		0.7933533f,
		-0.382683426f,
		0.9238795f,
		-0.130526185f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		0.382683426f,
		0.9238795f,
		0.6087614f,
		0.7933533f,
		0.7933533f,
		0.6087614f,
		0.9238795f,
		0.382683426f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		-0.130526185f,
		0.9238795f,
		-0.382683426f,
		0.7933533f,
		-0.6087614f,
		0.6087614f,
		-0.7933533f,
		0.382683426f,
		-0.9238795f,
		0.130526185f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		-0.382683426f,
		-0.9238795f,
		-0.6087614f,
		-0.7933533f,
		-0.7933533f,
		-0.6087614f,
		-0.9238795f,
		-0.382683426f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		0.130526185f,
		-0.9238795f,
		0.382683426f,
		-0.7933533f,
		0.6087614f,
		-0.6087614f,
		0.7933533f,
		-0.382683426f,
		0.9238795f,
		-0.130526185f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		0.382683426f,
		0.9238795f,
		0.6087614f,
		0.7933533f,
		0.7933533f,
		0.6087614f,
		0.9238795f,
		0.382683426f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		-0.130526185f,
		0.9238795f,
		-0.382683426f,
		0.7933533f,
		-0.6087614f,
		0.6087614f,
		-0.7933533f,
		0.382683426f,
		-0.9238795f,
		0.130526185f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		-0.382683426f,
		-0.9238795f,
		-0.6087614f,
		-0.7933533f,
		-0.7933533f,
		-0.6087614f,
		-0.9238795f,
		-0.382683426f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		0.130526185f,
		-0.9238795f,
		0.382683426f,
		-0.7933533f,
		0.6087614f,
		-0.6087614f,
		0.7933533f,
		-0.382683426f,
		0.9238795f,
		-0.130526185f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		0.382683426f,
		0.9238795f,
		0.6087614f,
		0.7933533f,
		0.7933533f,
		0.6087614f,
		0.9238795f,
		0.382683426f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		-0.130526185f,
		0.9238795f,
		-0.382683426f,
		0.7933533f,
		-0.6087614f,
		0.6087614f,
		-0.7933533f,
		0.382683426f,
		-0.9238795f,
		0.130526185f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		-0.382683426f,
		-0.9238795f,
		-0.6087614f,
		-0.7933533f,
		-0.7933533f,
		-0.6087614f,
		-0.9238795f,
		-0.382683426f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		0.130526185f,
		-0.9238795f,
		0.382683426f,
		-0.7933533f,
		0.6087614f,
		-0.6087614f,
		0.7933533f,
		-0.382683426f,
		0.9238795f,
		-0.130526185f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		0.382683426f,
		0.9238795f,
		0.6087614f,
		0.7933533f,
		0.7933533f,
		0.6087614f,
		0.9238795f,
		0.382683426f,
		0.9914449f,
		0.130526185f,
		0.9914449f,
		-0.130526185f,
		0.9238795f,
		-0.382683426f,
		0.7933533f,
		-0.6087614f,
		0.6087614f,
		-0.7933533f,
		0.382683426f,
		-0.9238795f,
		0.130526185f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		-0.382683426f,
		-0.9238795f,
		-0.6087614f,
		-0.7933533f,
		-0.7933533f,
		-0.6087614f,
		-0.9238795f,
		-0.382683426f,
		-0.9914449f,
		-0.130526185f,
		-0.9914449f,
		0.130526185f,
		-0.9238795f,
		0.382683426f,
		-0.7933533f,
		0.6087614f,
		-0.6087614f,
		0.7933533f,
		-0.382683426f,
		0.9238795f,
		-0.130526185f,
		0.9914449f,
		0.382683426f,
		0.9238795f,
		0.9238795f,
		0.382683426f,
		0.9238795f,
		-0.382683426f,
		0.382683426f,
		-0.9238795f,
		-0.382683426f,
		-0.9238795f,
		-0.9238795f,
		-0.382683426f,
		-0.9238795f,
		0.382683426f,
		-0.382683426f,
		0.9238795f
	};

	// Token: 0x0400009A RID: 154
	private static readonly float[] RandVecs2D = new float[]
	{
		-0.2700222f,
		-0.9628541f,
		0.386309266f,
		-0.9223693f,
		0.04444859f,
		-0.9990117f,
		-0.599252343f,
		-0.800560236f,
		-0.781928f,
		0.62336874f,
		0.9464672f,
		0.322799921f,
		-0.6514147f,
		-0.7587219f,
		0.937847257f,
		0.347048372f,
		-0.8497876f,
		-0.527125239f,
		-0.879042566f,
		0.476743251f,
		-0.8923003f,
		-0.451442361f,
		-0.379844427f,
		-0.9250504f,
		-0.9951651f,
		0.09821638f,
		0.7724398f,
		-0.635088f,
		0.757328331f,
		-0.6530343f,
		-0.9928005f,
		-0.119780056f,
		-0.05326657f,
		0.998580337f,
		0.975425363f,
		-0.220330074f,
		-0.766501844f,
		0.642242134f,
		0.9916367f,
		0.129060611f,
		-0.994696856f,
		0.102850378f,
		-0.537920535f,
		-0.8429955f,
		0.502281547f,
		-0.864704132f,
		0.455982149f,
		-0.8899889f,
		-0.8659131f,
		-0.50019443f,
		0.08794584f,
		-0.9961253f,
		-0.5051685f,
		0.8630207f,
		0.7753185f,
		-0.6315704f,
		-0.692194462f,
		0.72171104f,
		-0.519165933f,
		-0.854673445f,
		0.8978623f,
		-0.4402764f,
		-0.170677409f,
		0.985326946f,
		-0.935343f,
		-0.353742063f,
		-0.999240458f,
		0.0389674678f,
		-0.2882064f,
		-0.9575683f,
		-0.966381133f,
		0.2571138f,
		-0.875971437f,
		-0.482363015f,
		-0.8303123f,
		-0.557298362f,
		0.0511013381f,
		-0.998693466f,
		-0.855837345f,
		-0.517245054f,
		0.0988702551f,
		0.9951003f,
		0.9189016f,
		0.394486785f,
		-0.243937582f,
		-0.969790936f,
		-0.812140942f,
		-0.5834613f,
		-0.99104315f,
		0.133542135f,
		0.8492424f,
		-0.528003156f,
		-0.9717839f,
		-0.235872954f,
		0.9949457f,
		0.100414209f,
		0.6241065f,
		-0.7813392f,
		0.6629103f,
		0.748698831f,
		-0.7197418f,
		0.6942418f,
		-0.8143371f,
		-0.580392241f,
		0.104521051f,
		-0.9945227f,
		-0.10659261f,
		-0.99430275f,
		0.445799679f,
		-0.8951328f,
		0.105547406f,
		0.99441427f,
		-0.9927903f,
		0.119864449f,
		-0.833436668f,
		0.552615047f,
		0.9115562f,
		-0.4111756f,
		0.8285545f,
		-0.55990845f,
		0.7217098f,
		-0.6921958f,
		0.494049281f,
		-0.8694339f,
		-0.36523214f,
		-0.9309165f,
		-0.9696607f,
		0.244454846f,
		0.0892550945f,
		-0.9960088f,
		0.5354071f,
		-0.8445941f,
		-0.105357617f,
		0.9944344f,
		-0.989028454f,
		0.1477251f,
		0.004856105f,
		0.9999882f,
		0.988559842f,
		0.150829136f,
		0.928612947f,
		-0.371049821f,
		-0.5832394f,
		-0.8123003f,
		0.301520765f,
		0.9534596f,
		-0.957511067f,
		0.288396567f,
		0.9715802f,
		-0.236710548f,
		0.2299818f,
		0.973194957f,
		0.9557638f,
		-0.2941352f,
		0.7409561f,
		0.671553433f,
		-0.9971514f,
		-0.07542631f,
		0.69057107f,
		-0.7232645f,
		-0.2907137f,
		-0.9568101f,
		0.5912778f,
		-0.80646795f,
		-0.945459247f,
		-0.3257405f,
		0.666445553f,
		0.7455537f,
		0.6236135f,
		0.781732857f,
		0.9126994f,
		-0.408631653f,
		-0.8191762f,
		0.573541939f,
		-0.8812746f,
		-0.4726046f,
		0.995331347f,
		0.09651673f,
		0.985565066f,
		-0.169296965f,
		-0.8495981f,
		0.527430654f,
		0.6174854f,
		-0.786582351f,
		0.850815654f,
		0.5254643f,
		0.998503268f,
		-0.0546925f,
		0.197137162f,
		-0.980375946f,
		0.660785556f,
		-0.7505747f,
		-0.0309749413f,
		0.9995202f,
		-0.6731661f,
		0.739491343f,
		-0.719501853f,
		-0.694490552f,
		0.972751141f,
		0.2318516f,
		0.9997059f,
		-0.02425069f,
		0.442178756f,
		-0.896926939f,
		0.9981351f,
		-0.0610436723f,
		-0.9173661f,
		-0.398044556f,
		-0.81500566f,
		-0.579453f,
		-0.878933132f,
		0.476945f,
		0.0158605836f,
		0.999874234f,
		-0.8095465f,
		0.5870558f,
		-0.9165899f,
		-0.399828672f,
		-0.8023543f,
		0.5968481f,
		-0.5176738f,
		0.855578065f,
		-0.8154407f,
		-0.578840554f,
		0.402201027f,
		-0.915551364f,
		-0.9052557f,
		-0.4248672f,
		0.7317446f,
		0.681579f,
		-0.564763248f,
		-0.825253f,
		-0.8403276f,
		-0.542078853f,
		-0.931428134f,
		0.363925248f,
		0.523819864f,
		0.851829052f,
		0.7432804f,
		-0.66898f,
		-0.9853716f,
		-0.170419738f,
		0.460146874f,
		0.887842834f,
		0.8258554f,
		0.563881934f,
		0.6182366f,
		0.785992f,
		0.833150268f,
		-0.553046644f,
		0.150030747f,
		0.9886813f,
		-0.6623304f,
		-0.7492119f,
		-0.668598652f,
		0.743623435f,
		0.7025606f,
		0.7116239f,
		-0.541938961f,
		-0.840417862f,
		-0.338861644f,
		0.9408362f,
		0.833153f,
		0.553042531f,
		-0.29897207f,
		-0.954261839f,
		0.2638523f,
		0.9645631f,
		0.124108739f,
		-0.9922686f,
		-0.7282649f,
		-0.6852957f,
		0.69625f,
		0.717799366f,
		-0.918353558f,
		0.395761f,
		-0.6326102f,
		-0.7744703f,
		-0.9331892f,
		-0.35938552f,
		-0.115377933f,
		-0.993321657f,
		0.9514975f,
		-0.307656556f,
		-0.08987977f,
		-0.9959526f,
		0.6678497f,
		0.7442962f,
		0.795240045f,
		-0.6062947f,
		-0.6462007f,
		-0.7631675f,
		-0.273359865f,
		0.961911857f,
		0.966959f,
		-0.254931837f,
		-0.9792895f,
		0.202465191f,
		-0.5369503f,
		-0.843613863f,
		-0.270036459f,
		-0.9628501f,
		-0.6400277f,
		0.768351853f,
		-0.785453737f,
		-0.6189204f,
		0.0600590557f,
		-0.9981948f,
		-0.0245577041f,
		0.9996984f,
		-0.659836233f,
		0.7514095f,
		-0.625389457f,
		-0.7803128f,
		-0.6210409f,
		-0.7837782f,
		0.8348889f,
		0.550418556f,
		-0.15922752f,
		0.9872419f,
		0.836762249f,
		0.547566354f,
		-0.8675754f,
		-0.4973057f,
		-0.202266261f,
		-0.97933054f,
		0.939919f,
		0.341397554f,
		0.987740457f,
		-0.1561049f,
		-0.903445542f,
		0.428702831f,
		0.126980424f,
		-0.9919052f,
		-0.3819601f,
		0.924178839f,
		0.9754626f,
		0.220165253f,
		-0.320401579f,
		-0.947281837f,
		-0.9874761f,
		0.157768741f,
		0.0253534839f,
		-0.999678552f,
		0.4835131f,
		-0.8753371f,
		-0.28508f,
		-0.9585037f,
		-0.06805516f,
		-0.997681558f,
		-0.7885244f,
		-0.615003467f,
		0.3185392f,
		-0.9479097f,
		0.8880043f,
		0.459835142f,
		0.647692144f,
		-0.761902153f,
		0.982024133f,
		0.188755423f,
		0.935727537f,
		-0.352723718f,
		-0.889489532f,
		0.456955522f,
		0.7922791f,
		0.6101588f,
		0.748381853f,
		0.663268149f,
		-0.728893f,
		-0.684627652f,
		0.8729033f,
		-0.487893283f,
		0.8288346f,
		0.5594937f,
		0.08074567f,
		0.996734738f,
		0.979914844f,
		-0.1994165f,
		-0.5807307f,
		-0.814095736f,
		-0.470004976f,
		-0.8826638f,
		0.2409493f,
		0.9705377f,
		0.9437817f,
		-0.330569416f,
		-0.892799854f,
		-0.45045355f,
		-0.806962252f,
		0.590603054f,
		0.0625897348f,
		0.998039365f,
		-0.931259751f,
		0.364355981f,
		0.577744961f,
		0.816217363f,
		-0.3360096f,
		-0.9418586f,
		0.697932065f,
		-0.716163933f,
		-0.00200815732f,
		-0.999998f,
		-0.182729438f,
		-0.983163238f,
		-0.6523912f,
		0.7578824f,
		-0.430262685f,
		-0.9027037f,
		-0.9985126f,
		-0.0545209125f,
		-0.0102810217f,
		-0.999947131f,
		-0.494607121f,
		0.869116664f,
		-0.299935f,
		0.953959644f,
		0.8165472f,
		0.5772787f,
		0.269746035f,
		0.9629315f,
		-0.7306287f,
		-0.682774961f,
		-0.7590952f,
		-0.650979638f,
		-0.9070538f,
		0.4210146f,
		-0.5104861f,
		-0.859886f,
		0.861335039f,
		0.5080373f,
		0.500788152f,
		-0.8655699f,
		-0.6541582f,
		0.7563578f,
		-0.838275552f,
		-0.54524684f,
		0.6940071f,
		0.7199682f,
		0.06950936f,
		0.9975813f,
		0.170294225f,
		-0.9853933f,
		0.269597322f,
		0.9629731f,
		0.551961243f,
		-0.833869755f,
		0.2256575f,
		-0.9742067f,
		0.421526283f,
		-0.9068162f,
		0.488187343f,
		-0.872738838f,
		-0.3683855f,
		-0.929673135f,
		-0.982539058f,
		0.18605645f,
		0.812564731f,
		0.582871f,
		0.3196461f,
		-0.947537f,
		0.9570914f,
		0.289786249f,
		-0.6876655f,
		-0.7260276f,
		-0.9988771f,
		-0.04737673f,
		-0.1250179f,
		0.9921545f,
		-0.828013361f,
		0.560708344f,
		0.932486355f,
		-0.361205131f,
		0.639465332f,
		0.7688199f,
		-0.0162384715f,
		-0.999868155f,
		-0.995501459f,
		-0.0947461352f,
		-0.8145332f,
		0.580117f,
		0.4037328f,
		-0.914876938f,
		0.9944263f,
		0.10543368f,
		-0.16247116f,
		0.9867133f,
		-0.9949488f,
		-0.100383878f,
		-0.699530244f,
		0.714603f,
		0.5263415f,
		-0.850273252f,
		-0.5395222f,
		0.8419714f,
		0.65793705f,
		0.7530729f,
		0.014267588f,
		-0.9998982f,
		-0.6734384f,
		0.7392433f,
		0.6394121f,
		-0.7688642f,
		0.9211571f,
		0.389190853f,
		-0.146637216f,
		-0.98919034f,
		-0.7823181f,
		0.6228791f,
		-0.5039611f,
		-0.8637264f,
		-0.774312f,
		-0.632804f
	};

	// Token: 0x0400009B RID: 155
	private static readonly float[] Gradients3D = new float[]
	{
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		0f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		0f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		0f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		0f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		1f,
		0f,
		1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		-1f,
		0f,
		1f,
		1f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		1f,
		-1f,
		0f,
		0f,
		-1f,
		-1f,
		0f,
		0f,
		1f,
		1f,
		0f,
		0f,
		0f,
		-1f,
		1f,
		0f,
		-1f,
		1f,
		0f,
		0f,
		0f,
		-1f,
		-1f,
		0f
	};

	// Token: 0x0400009C RID: 156
	private static readonly float[] RandVecs3D = new float[]
	{
		-0.7292737f,
		-0.661843956f,
		0.17355819f,
		0f,
		0.7902921f,
		-0.5480887f,
		-0.2739291f,
		0f,
		0.7217579f,
		0.622621238f,
		-0.3023381f,
		0f,
		0.5656831f,
		-0.8208298f,
		-0.079000026f,
		0f,
		0.760049045f,
		-0.555597961f,
		-0.337099969f,
		0f,
		0.371394575f,
		0.501126468f,
		0.78162545f,
		0f,
		-0.127706245f,
		-0.4254439f,
		-0.8959289f,
		0f,
		-0.2881561f,
		-0.5815839f,
		0.7607406f,
		0f,
		0.5849561f,
		-0.6628202f,
		-0.4674352f,
		0f,
		0.330717117f,
		0.0391653739f,
		0.94291687f,
		0f,
		0.8712122f,
		-0.411337435f,
		-0.267938167f,
		0f,
		0.580981f,
		0.7021916f,
		0.411567777f,
		0f,
		0.5037569f,
		0.6330057f,
		-0.5878204f,
		0f,
		0.449371219f,
		0.6013902f,
		0.6606023f,
		0f,
		-0.6878404f,
		0.0901889056f,
		-0.7202372f,
		0f,
		-0.595895648f,
		-0.646935046f,
		0.475797653f,
		0f,
		-0.5127052f,
		0.1946922f,
		-0.836198747f,
		0f,
		-0.991150737f,
		-0.0541027635f,
		-0.121215314f,
		0f,
		-0.214972109f,
		0.9720882f,
		-0.09397608f,
		0f,
		-0.7518651f,
		-0.542805731f,
		0.374246955f,
		0f,
		0.5237069f,
		0.8516377f,
		-0.0210781787f,
		0f,
		0.6333505f,
		0.192616716f,
		-0.749510467f,
		0f,
		-0.06788242f,
		0.39983058f,
		0.9140719f,
		0f,
		-0.55386287f,
		-0.472989678f,
		-0.6852129f,
		0f,
		-0.726145566f,
		-0.5911991f,
		0.350993335f,
		0f,
		-0.9229275f,
		-0.178280875f,
		0.341204941f,
		0f,
		-0.6968815f,
		0.651127458f,
		0.300648034f,
		0f,
		0.960804462f,
		-0.209836319f,
		-0.18117249f,
		0f,
		0.0681714639f,
		-0.9743405f,
		0.214506909f,
		0f,
		-0.3577285f,
		-0.6697087f,
		-0.650784552f,
		0f,
		-0.186862111f,
		0.7648617f,
		-0.616497457f,
		0f,
		-0.654169738f,
		0.3967915f,
		0.643908739f,
		0f,
		0.699334f,
		-0.6164538f,
		0.361823916f,
		0f,
		-0.154666573f,
		0.6291284f,
		0.7617583f,
		0f,
		-0.6841613f,
		-0.2580482f,
		-0.682154238f,
		0f,
		0.5383981f,
		0.4258655f,
		0.727163f,
		0f,
		-0.5026988f,
		-0.7939833f,
		-0.3418837f,
		0f,
		0.320297182f,
		0.283441544f,
		0.9039196f,
		0f,
		0.86832273f,
		-0.000376265642f,
		-0.495999515f,
		0f,
		0.791120052f,
		-0.0851104558f,
		0.605710566f,
		0f,
		-0.04011016f,
		-0.439724863f,
		0.8972364f,
		0f,
		0.914512f,
		0.357934624f,
		-0.188548759f,
		0f,
		-0.961203933f,
		-0.275648415f,
		0.0102466689f,
		0f,
		0.651036143f,
		-0.287779927f,
		-0.702377856f,
		0f,
		-0.204178631f,
		0.736523747f,
		0.6448596f,
		0f,
		-0.7718264f,
		0.379062682f,
		0.5104856f,
		0f,
		-0.306008279f,
		-0.7692988f,
		0.56083715f,
		0f,
		0.454007328f,
		-0.5024843f,
		0.735789955f,
		0f,
		0.481679559f,
		0.6021208f,
		-0.636738f,
		0f,
		0.696198046f,
		-0.322219729f,
		0.6414692f,
		0f,
		-0.653216064f,
		-0.6781149f,
		0.336851567f,
		0f,
		0.508930147f,
		-0.615466237f,
		-0.601823449f,
		0f,
		-0.163591981f,
		-0.9133605f,
		-0.372840881f,
		0f,
		0.5240802f,
		-0.8437664f,
		0.115750588f,
		0f,
		0.5902587f,
		0.4983818f,
		-0.634988368f,
		0f,
		0.5863228f,
		0.494764745f,
		0.6414308f,
		0f,
		0.6779335f,
		0.234134525f,
		0.6968409f,
		0f,
		0.7177054f,
		-0.685897946f,
		0.120178632f,
		0f,
		-0.532882f,
		-0.5205125f,
		0.6671608f,
		0f,
		-0.8654874f,
		-0.07007271f,
		-0.4960054f,
		0f,
		-0.286181f,
		0.795208931f,
		0.534549534f,
		0f,
		-0.0484952964f,
		0.981083632f,
		-0.187411562f,
		0f,
		-0.635852158f,
		0.605834842f,
		0.478180021f,
		0f,
		0.62547946f,
		-0.286161959f,
		0.725869656f,
		0f,
		-0.258526f,
		0.506194949f,
		-0.8227582f,
		0f,
		0.0213630684f,
		0.506401658f,
		-0.862033f,
		0f,
		0.200111777f,
		0.859926343f,
		0.46955505f,
		0f,
		0.474356145f,
		0.6014985f,
		-0.6427953f,
		0f,
		0.6622994f,
		-0.520247459f,
		-0.539168f,
		0f,
		0.08084973f,
		-0.653272033f,
		0.7527941f,
		0f,
		-0.6893687f,
		0.0592860356f,
		0.7219805f,
		0f,
		-0.112188712f,
		-0.967318535f,
		0.227395251f,
		0f,
		0.7344116f,
		0.59796685f,
		-0.3210533f,
		0f,
		0.5789393f,
		-0.248884976f,
		0.776457f,
		0f,
		0.698818266f,
		0.355716974f,
		-0.6205791f,
		0f,
		-0.863684535f,
		-0.274877131f,
		-0.4224826f,
		0f,
		-0.4247028f,
		-0.464088082f,
		0.777335048f,
		0f,
		0.5257723f,
		-0.842701733f,
		0.115832992f,
		0f,
		0.934383035f,
		0.316302478f,
		-0.163954392f,
		0f,
		-0.101683639f,
		-0.8057303f,
		-0.583488762f,
		0f,
		-0.6529239f,
		0.506021261f,
		-0.5635893f,
		0f,
		-0.246528611f,
		-0.9668206f,
		-0.06694497f,
		0f,
		-0.9776897f,
		-0.209925056f,
		-0.00736882538f,
		0f,
		0.7736893f,
		0.573424459f,
		0.2694238f,
		0f,
		-0.6095088f,
		0.4995679f,
		0.6155737f,
		0f,
		0.5794535f,
		0.7434547f,
		0.333929241f,
		0f,
		-0.8226211f,
		0.0814258158f,
		0.562729359f,
		0f,
		-0.510385454f,
		0.470366776f,
		0.719904f,
		0f,
		-0.5764972f,
		-0.0723165646f,
		-0.813892663f,
		0f,
		0.7250629f,
		0.39499715f,
		-0.56414634f,
		0f,
		-0.1525424f,
		0.486084074f,
		-0.8604958f,
		0f,
		-0.55509764f,
		-0.495782077f,
		0.6678823f,
		0f,
		-0.188361436f,
		0.914586961f,
		0.35784173f,
		0f,
		0.762555659f,
		-0.541440845f,
		-0.354048967f,
		0f,
		-0.5870232f,
		-0.3226498f,
		-0.7424964f,
		0f,
		0.305112422f,
		0.2262544f,
		-0.9250488f,
		0f,
		0.637957633f,
		0.577242434f,
		-0.509707034f,
		0f,
		-0.5966776f,
		0.145485237f,
		-0.7891831f,
		0f,
		-0.65833056f,
		0.655548751f,
		-0.369941473f,
		0f,
		0.743489265f,
		0.235108465f,
		0.6260573f,
		0f,
		0.5562114f,
		0.826436043f,
		-0.08736329f,
		0f,
		-0.302894f,
		-0.8251527f,
		0.476841927f,
		0f,
		0.112934381f,
		-0.9858884f,
		-0.123571075f,
		0f,
		0.5937653f,
		-0.5896814f,
		0.5474657f,
		0f,
		0.6757964f,
		-0.583575845f,
		-0.450264841f,
		0f,
		0.7242303f,
		-0.115271978f,
		0.679855049f,
		0f,
		-0.9511914f,
		0.0753624f,
		-0.299258083f,
		0f,
		0.2539471f,
		-0.188633934f,
		0.9486454f,
		0f,
		0.5714336f,
		-0.167945087f,
		-0.8032796f,
		0f,
		-0.06778235f,
		0.39782694f,
		0.9149532f,
		0f,
		0.6074973f,
		0.73306f,
		-0.305892259f,
		0f,
		-0.543547869f,
		0.167582244f,
		0.8224791f,
		0f,
		-0.5876678f,
		-0.3380045f,
		-0.7351187f,
		0f,
		-0.796756268f,
		0.0409782268f,
		-0.602909863f,
		0f,
		-0.199635088f,
		0.8706295f,
		0.4496111f,
		0f,
		-0.0278766025f,
		-0.910623252f,
		-0.4122962f,
		0f,
		-0.7797626f,
		-0.6257635f,
		0.0197577551f,
		0f,
		-0.5211233f,
		0.740164459f,
		-0.424955457f,
		0f,
		0.8575425f,
		0.4053273f,
		-0.316750169f,
		0f,
		0.104522333f,
		0.8390196f,
		-0.533967435f,
		0f,
		0.3501823f,
		0.9242524f,
		-0.152085021f,
		0f,
		0.198784992f,
		0.0764761344f,
		0.9770547f,
		0f,
		0.784599662f,
		0.6066257f,
		-0.128096417f,
		0f,
		0.09006737f,
		-0.975098968f,
		-0.20265691f,
		0f,
		-0.827434361f,
		-0.542299569f,
		0.145820364f,
		0f,
		-0.348579764f,
		-0.41580227f,
		0.8400004f,
		0f,
		-0.2471779f,
		-0.730482f,
		-0.6366311f,
		0f,
		-0.3700155f,
		0.8577948f,
		0.356758446f,
		0f,
		0.591339469f,
		-0.548311949f,
		-0.591330349f,
		0f,
		0.120487355f,
		-0.7626472f,
		-0.6354935f,
		0f,
		0.6169593f,
		0.03079648f,
		0.7863923f,
		0f,
		0.12581569f,
		-0.664083f,
		-0.73699677f,
		0f,
		-0.6477565f,
		-0.174014732f,
		-0.741707742f,
		0f,
		0.6217889f,
		-0.7804431f,
		-0.06547655f,
		0f,
		0.6589943f,
		-0.6096988f,
		0.44044736f,
		0f,
		-0.268983752f,
		-0.6732403f,
		-0.688763559f,
		0f,
		-0.38497752f,
		0.567654252f,
		0.7277094f,
		0f,
		0.57544446f,
		0.811047137f,
		-0.105196349f,
		0f,
		0.914159358f,
		0.3832948f,
		0.131900564f,
		0f,
		-0.107925318f,
		0.9245494f,
		0.365459353f,
		0f,
		0.3779771f,
		0.304314882f,
		0.874371648f,
		0f,
		-0.214288518f,
		-0.8259286f,
		0.5214617f,
		0f,
		0.580254436f,
		0.414809853f,
		-0.7008834f,
		0f,
		-0.198266089f,
		0.856716156f,
		-0.476159662f,
		0f,
		-0.0338155366f,
		0.377318084f,
		-0.9254661f,
		0f,
		-0.686792254f,
		-0.6656598f,
		0.29191336f,
		0f,
		0.7731743f,
		-0.287579358f,
		-0.565243f,
		0f,
		-0.09655942f,
		0.91937083f,
		-0.3813575f,
		0f,
		0.271570235f,
		-0.957791f,
		-0.09426606f,
		0f,
		0.245101571f,
		-0.6917999f,
		-0.6792188f,
		0f,
		0.97770077f,
		-0.175385535f,
		0.115503654f,
		0f,
		-0.522474f,
		0.8521607f,
		0.0290361587f,
		0f,
		-0.773488045f,
		-0.526129246f,
		0.353417963f,
		0f,
		-0.71344924f,
		-0.269547254f,
		0.6467878f,
		0f,
		0.164403722f,
		0.5105846f,
		-0.843963742f,
		0f,
		0.6494636f,
		0.0558561124f,
		0.7583384f,
		0f,
		-0.4711971f,
		0.501728058f,
		-0.7254256f,
		0f,
		-0.633576453f,
		-0.238168627f,
		-0.7361091f,
		0f,
		-0.9021533f,
		-0.2709478f,
		-0.335718185f,
		0f,
		-0.3793711f,
		0.8722581f,
		0.3086152f,
		0f,
		-0.685559869f,
		-0.325014323f,
		0.6514394f,
		0f,
		0.290094227f,
		-0.7799058f,
		-0.5546101f,
		0f,
		-0.209831938f,
		0.8503707f,
		0.482535154f,
		0f,
		-0.459260374f,
		0.6598504f,
		-0.5947077f,
		0f,
		0.871594548f,
		0.09616365f,
		-0.480703115f,
		0f,
		-0.6776666f,
		0.711850464f,
		-0.1844907f,
		0f,
		0.7044378f,
		0.3124276f,
		0.637304f,
		0f,
		-0.7052319f,
		-0.240109324f,
		-0.6670798f,
		0f,
		0.0819210038f,
		-0.720733643f,
		-0.688354552f,
		0f,
		-0.6993681f,
		-0.5875763f,
		-0.4069869f,
		0f,
		-0.128145441f,
		0.6419896f,
		0.755928636f,
		0f,
		-0.6337388f,
		-0.678547144f,
		-0.3714147f,
		0f,
		0.5565052f,
		-0.216888756f,
		-0.8020357f,
		0f,
		-0.579155445f,
		0.7244372f,
		-0.3738579f,
		0f,
		0.11757791f,
		-0.7096451f,
		0.69467926f,
		0f,
		-0.613462f,
		0.132363111f,
		0.7785528f,
		0f,
		0.698463559f,
		-0.0298051629f,
		-0.7150247f,
		0f,
		0.831808269f,
		-0.3930172f,
		0.391959757f,
		0f,
		0.146957636f,
		0.055416517f,
		-0.98758924f,
		0f,
		0.708868563f,
		-0.2690504f,
		0.652010143f,
		0f,
		0.27260533f,
		0.67369765f,
		-0.686889946f,
		0f,
		-0.65912956f,
		0.303545862f,
		-0.688046634f,
		0f,
		0.481513143f,
		-0.752827f,
		0.4487723f,
		0f,
		0.943001f,
		0.167564735f,
		-0.287526131f,
		0f,
		0.434802949f,
		0.7695305f,
		-0.46772778f,
		0f,
		0.393199623f,
		0.5944736f,
		0.701423645f,
		0f,
		0.725433648f,
		-0.603925645f,
		0.330181479f,
		0f,
		0.759023547f,
		-0.6506083f,
		0.0243331324f,
		0f,
		-0.8552769f,
		-0.3430043f,
		0.388393581f,
		0f,
		-0.6139747f,
		0.6981725f,
		0.368225753f,
		0f,
		-0.746590555f,
		-0.575201f,
		0.334284931f,
		0f,
		0.5730066f,
		0.8105555f,
		-0.121091679f,
		0f,
		-0.922587752f,
		-0.3475211f,
		-0.167514041f,
		0f,
		-0.71058166f,
		-0.471969217f,
		-0.5218417f,
		0f,
		-0.0856461f,
		0.358300149f,
		0.9296697f,
		0f,
		-0.8279698f,
		-0.2043157f,
		0.5222271f,
		0f,
		0.427944034f,
		0.278166f,
		0.8599346f,
		0f,
		0.539908f,
		-0.785712063f,
		-0.3019204f,
		0f,
		0.5678404f,
		-0.5495414f,
		-0.612830758f,
		0f,
		-0.9896071f,
		0.136563912f,
		-0.0450341851f,
		0f,
		-0.6154343f,
		-0.644087553f,
		0.454303741f,
		0f,
		0.107420437f,
		-0.794634044f,
		0.597509444f,
		0f,
		-0.359545f,
		-0.888553f,
		0.284957826f,
		0f,
		-0.218040526f,
		0.1529889f,
		0.9638738f,
		0f,
		-0.7277432f,
		-0.61640507f,
		-0.300723463f,
		0f,
		0.7249729f,
		-0.00669719465f,
		0.688744843f,
		0f,
		-0.5553659f,
		-0.5336586f,
		0.6377908f,
		0f,
		0.5137558f,
		0.797620833f,
		-0.316f,
		0f,
		-0.3794025f,
		0.924560845f,
		-0.0352275148f,
		0f,
		0.822924852f,
		0.27453658f,
		-0.497417659f,
		0f,
		-0.5404114f,
		0.60911417f,
		0.5804614f,
		0f,
		0.8036582f,
		-0.270302951f,
		0.5301602f,
		0f,
		0.604431868f,
		0.683296859f,
		0.409594327f,
		0f,
		0.06389989f,
		0.965820849f,
		-0.2512108f,
		0f,
		0.108711332f,
		0.74024713f,
		-0.6634878f,
		0f,
		-0.7134277f,
		-0.6926784f,
		0.105912849f,
		0f,
		0.645889759f,
		-0.57245487f,
		-0.50509584f,
		0f,
		-0.6553931f,
		0.73814714f,
		0.159995615f,
		0f,
		0.391096145f,
		0.918887138f,
		-0.05186756f,
		0f,
		-0.487902254f,
		-0.5904377f,
		0.642911136f,
		0f,
		0.601479f,
		0.770744145f,
		-0.210182011f,
		0f,
		-0.5677173f,
		0.7511361f,
		0.336885184f,
		0f,
		0.7858574f,
		0.226674661f,
		0.5753667f,
		0f,
		-0.452034563f,
		-0.6042227f,
		-0.656185746f,
		0f,
		0.00227211625f,
		0.4132844f,
		-0.9105992f,
		0f,
		-0.581575155f,
		-0.5162926f,
		0.6286591f,
		0f,
		-0.03703705f,
		0.8273786f,
		0.5604221f,
		0f,
		-0.511969268f,
		0.795354366f,
		-0.324498f,
		0f,
		-0.268241733f,
		-0.957229f,
		-0.10843876f,
		0f,
		-0.232248276f,
		-0.9679131f,
		-0.09594243f,
		0f,
		0.3554329f,
		-0.8881506f,
		0.291300625f,
		0f,
		0.734652042f,
		-0.4371373f,
		0.5188423f,
		0f,
		0.998512f,
		0.0465901121f,
		-0.0283394456f,
		0f,
		-0.37276876f,
		-0.9082481f,
		0.190075725f,
		0f,
		0.9173738f,
		-0.3483642f,
		0.192529842f,
		0f,
		0.2714911f,
		0.41475296f,
		-0.868488669f,
		0f,
		0.5131763f,
		-0.711633444f,
		0.4798207f,
		0f,
		-0.873735368f,
		0.188869923f,
		-0.448235065f,
		0f,
		0.846004367f,
		-0.3725218f,
		0.38145f,
		0f,
		0.897872746f,
		-0.178020909f,
		-0.402657539f,
		0f,
		0.217806563f,
		-0.9698323f,
		-0.109478951f,
		0f,
		-0.151803136f,
		-0.7788918f,
		-0.6085091f,
		0f,
		-0.2600385f,
		-0.4755398f,
		-0.840382f,
		0f,
		0.5723135f,
		-0.7474341f,
		-0.337341845f,
		0f,
		-0.7174141f,
		0.169901714f,
		-0.675611138f,
		0f,
		-0.6841808f,
		0.0214570761f,
		-0.728996754f,
		0f,
		-0.2007448f,
		0.06555606f,
		-0.9774477f,
		0f,
		-0.114880368f,
		-0.8044887f,
		0.5827524f,
		0f,
		-0.787035f,
		0.03447489f,
		0.6159443f,
		0f,
		-0.201559648f,
		0.685987234f,
		0.699138939f,
		0f,
		-0.0858108252f,
		-0.10920836f,
		-0.990308046f,
		0f,
		0.5532693f,
		0.732525051f,
		-0.396610767f,
		0f,
		-0.184248939f,
		-0.9777375f,
		-0.100407675f,
		0f,
		0.07754738f,
		-0.9111506f,
		0.404711038f,
		0f,
		0.139983848f,
		0.7601631f,
		-0.634473443f,
		0f,
		0.448441923f,
		-0.84528923f,
		0.290492535f,
		0f
	};

	// Token: 0x0400009D RID: 157
	private const int PrimeX = 501125321;

	// Token: 0x0400009E RID: 158
	private const int PrimeY = 1136930381;

	// Token: 0x0400009F RID: 159
	private const int PrimeZ = 1720413743;

	// Token: 0x020003F1 RID: 1009
	public enum NoiseType
	{
		// Token: 0x0400173C RID: 5948
		OpenSimplex2,
		// Token: 0x0400173D RID: 5949
		OpenSimplex2S,
		// Token: 0x0400173E RID: 5950
		Cellular,
		// Token: 0x0400173F RID: 5951
		Perlin,
		// Token: 0x04001740 RID: 5952
		ValueCubic,
		// Token: 0x04001741 RID: 5953
		Value
	}

	// Token: 0x020003F2 RID: 1010
	public enum RotationType3D
	{
		// Token: 0x04001743 RID: 5955
		None,
		// Token: 0x04001744 RID: 5956
		ImproveXYPlanes,
		// Token: 0x04001745 RID: 5957
		ImproveXZPlanes
	}

	// Token: 0x020003F3 RID: 1011
	public enum FractalType
	{
		// Token: 0x04001747 RID: 5959
		None,
		// Token: 0x04001748 RID: 5960
		FBm,
		// Token: 0x04001749 RID: 5961
		Ridged,
		// Token: 0x0400174A RID: 5962
		PingPong,
		// Token: 0x0400174B RID: 5963
		DomainWarpProgressive,
		// Token: 0x0400174C RID: 5964
		DomainWarpIndependent
	}

	// Token: 0x020003F4 RID: 1012
	public enum CellularDistanceFunction
	{
		// Token: 0x0400174E RID: 5966
		Euclidean,
		// Token: 0x0400174F RID: 5967
		EuclideanSq,
		// Token: 0x04001750 RID: 5968
		Manhattan,
		// Token: 0x04001751 RID: 5969
		Hybrid
	}

	// Token: 0x020003F5 RID: 1013
	public enum CellularReturnType
	{
		// Token: 0x04001753 RID: 5971
		CellValue,
		// Token: 0x04001754 RID: 5972
		Distance,
		// Token: 0x04001755 RID: 5973
		Distance2,
		// Token: 0x04001756 RID: 5974
		Distance2Add,
		// Token: 0x04001757 RID: 5975
		Distance2Sub,
		// Token: 0x04001758 RID: 5976
		Distance2Mul,
		// Token: 0x04001759 RID: 5977
		Distance2Div
	}

	// Token: 0x020003F6 RID: 1014
	public enum DomainWarpType
	{
		// Token: 0x0400175B RID: 5979
		OpenSimplex2,
		// Token: 0x0400175C RID: 5980
		OpenSimplex2Reduced,
		// Token: 0x0400175D RID: 5981
		BasicGrid
	}

	// Token: 0x020003F7 RID: 1015
	private enum TransformType3D
	{
		// Token: 0x0400175F RID: 5983
		None,
		// Token: 0x04001760 RID: 5984
		ImproveXYPlanes,
		// Token: 0x04001761 RID: 5985
		ImproveXZPlanes,
		// Token: 0x04001762 RID: 5986
		DefaultOpenSimplex2
	}
}
