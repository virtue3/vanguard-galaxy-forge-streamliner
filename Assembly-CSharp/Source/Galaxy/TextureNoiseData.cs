using System;
using Behaviour.Background;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x0200013D RID: 317
	public class TextureNoiseData : IJsonSource
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x00055C8C File Offset: 0x00053E8C
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"seed",
					new double?((double)this.seed)
				},
				{
					"frequency",
					new double?((double)this.frequency)
				},
				{
					"noiseType",
					this.noiseType.ToString()
				},
				{
					"fractalOctaves",
					new double?((double)this.fractalOctaves)
				},
				{
					"fractalGain",
					new double?((double)this.fractalGain)
				},
				{
					"fractalLacunarity",
					new double?((double)this.fractalLacunarity)
				},
				{
					"fractalType",
					this.fractalType.ToString()
				},
				{
					"fractalWeighedStrength",
					new double?((double)this.fractalWeighedStrength)
				},
				{
					"cellularJitter",
					new double?((double)this.cellularJitter)
				},
				{
					"cellularDistanceFunction",
					this.cellularDistanceFunction.ToString()
				},
				{
					"cellularReturnType",
					this.cellularReturnType.ToString()
				}
			};
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00055DF8 File Offset: 0x00053FF8
		public void GenerateRandom(BackgroundTextureType type)
		{
			this.seed = SeededRandom.Global.RandomRange(-1000000000, 1000000000);
			this.noiseType = SeededRandom.Global.ChooseEnum<FastNoiseLite.NoiseType>(0);
			this.fractalLacunarity = SeededRandom.Global.RandomRange(0.01f, 1f);
			this.fractalType = SeededRandom.Global.ChooseEnum<FastNoiseLite.FractalType>(0);
			this.fractalWeighedStrength = SeededRandom.Global.RandomRange(0.01f, 1f);
			if (this.noiseType == FastNoiseLite.NoiseType.Cellular)
			{
				this.cellularJitter = SeededRandom.Global.RandomRange(0.7f, 1f);
				this.cellularDistanceFunction = FastNoiseLite.CellularDistanceFunction.Hybrid;
				this.cellularReturnType = SeededRandom.Global.ChooseEnum<FastNoiseLite.CellularReturnType>(1);
				if (this.CellularDistanceIsIncompatible())
				{
					this.fractalType = (SeededRandom.Global.RandomBool(0.5f) ? FastNoiseLite.FractalType.PingPong : FastNoiseLite.FractalType.Ridged);
				}
			}
			this.frequency = this.GetRandomFrequency();
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00055EDF File Offset: 0x000540DF
		private bool CellularDistanceIsIncompatible()
		{
			return (this.cellularReturnType == FastNoiseLite.CellularReturnType.Distance2Div && this.fractalType == FastNoiseLite.FractalType.FBm) || this.fractalType == FastNoiseLite.FractalType.DomainWarpProgressive || this.fractalType == FastNoiseLite.FractalType.DomainWarpIndependent;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00055F08 File Offset: 0x00054108
		public float GetRandomFrequency()
		{
			Vector2 vector = new Vector2(0.0005f, 0.005f);
			bool flag = this.noiseType == FastNoiseLite.NoiseType.ValueCubic || this.noiseType == FastNoiseLite.NoiseType.Value || this.noiseType == FastNoiseLite.NoiseType.Perlin || this.noiseType == FastNoiseLite.NoiseType.OpenSimplex2S;
			if (this.fractalType == FastNoiseLite.FractalType.FBm || flag)
			{
				vector = new Vector2(0.005f, 0.01f);
			}
			return this.frequency = SeededRandom.Global.RandomRange(vector.x, vector.y);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00055F8C File Offset: 0x0005418C
		public static TextureNoiseData FromJson(JsonValue json)
		{
			return new TextureNoiseData
			{
				seed = json["seed"].AsInteger,
				frequency = (float)json["frequency"].AsNumber,
				noiseType = Enum.Parse<FastNoiseLite.NoiseType>(json["noiseType"]),
				fractalOctaves = json["fractalOctaves"].AsInteger,
				fractalGain = (float)json["fractalGain"].AsNumber,
				fractalLacunarity = (float)json["fractalLacunarity"].AsNumber,
				fractalType = Enum.Parse<FastNoiseLite.FractalType>(json["fractalType"]),
				fractalWeighedStrength = (float)json["fractalWeighedStrength"].AsNumber,
				cellularJitter = (float)json["cellularJitter"].AsNumber,
				cellularDistanceFunction = Enum.Parse<FastNoiseLite.CellularDistanceFunction>(json["cellularDistanceFunction"]),
				cellularReturnType = Enum.Parse<FastNoiseLite.CellularReturnType>(json["cellularReturnType"])
			};
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000560C9 File Offset: 0x000542C9
		public static TextureNoiseData FromJson(string key, JsonValue data)
		{
			return TextureNoiseData.FromJson(data);
		}

		// Token: 0x04000689 RID: 1673
		public int seed = 1337;

		// Token: 0x0400068A RID: 1674
		public float frequency = 0.01f;

		// Token: 0x0400068B RID: 1675
		public FastNoiseLite.NoiseType noiseType;

		// Token: 0x0400068C RID: 1676
		public int fractalOctaves = 1;

		// Token: 0x0400068D RID: 1677
		public float fractalGain = 0.1f;

		// Token: 0x0400068E RID: 1678
		public float fractalLacunarity = 0.1f;

		// Token: 0x0400068F RID: 1679
		public FastNoiseLite.FractalType fractalType;

		// Token: 0x04000690 RID: 1680
		public float fractalWeighedStrength = 0.1f;

		// Token: 0x04000691 RID: 1681
		public float cellularJitter = 0.1f;

		// Token: 0x04000692 RID: 1682
		public FastNoiseLite.CellularDistanceFunction cellularDistanceFunction;

		// Token: 0x04000693 RID: 1683
		public FastNoiseLite.CellularReturnType cellularReturnType;
	}
}
