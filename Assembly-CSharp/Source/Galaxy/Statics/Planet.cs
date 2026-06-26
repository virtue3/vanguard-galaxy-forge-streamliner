using System;
using System.Collections.Generic;
using Behaviour.GalaxyMap;
using LightJson;

namespace Source.Galaxy.Statics
{
	// Token: 0x0200014C RID: 332
	public class Planet : MapStatic
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0005C910 File Offset: 0x0005AB10
		public override WorldMapStatic Prefab
		{
			get
			{
				return WorldMapStatic.GetPrefab("Planet");
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0005C91C File Offset: 0x0005AB1C
		public Planet()
		{
			this.planetTypeTextures = new Dictionary<PlanetType, List<string>>
			{
				{
					PlanetType.GasGiant,
					new List<string>
					{
						"lightblue",
						"pinky",
						"red",
						"greenblue",
						"yellowy",
						"greenyellow"
					}
				},
				{
					PlanetType.EarthLike,
					new List<string>
					{
						"one",
						"two",
						"three",
						"four",
						"five",
						"six"
					}
				},
				{
					PlanetType.Rock,
					new List<string>
					{
						"one"
					}
				},
				{
					PlanetType.Desert,
					new List<string>
					{
						"one",
						"two",
						"three"
					}
				}
			};
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0005CA1A File Offset: 0x0005AC1A
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0005CA24 File Offset: 0x0005AC24
		public PlanetType pType
		{
			get
			{
				return this.planetType;
			}
			set
			{
				this.planetType = value;
				int value2 = this.textureIndex.GetValueOrDefault();
				if (this.textureIndex == null)
				{
					value2 = this.planetTypeTextures[this.planetType].IndexOf(SeededRandom.Global.Choose<string>(this.planetTypeTextures[this.planetType]));
					this.textureIndex = new int?(value2);
				}
				PlanetType planetType = this.planetType;
				this.atmosphere = (planetType == PlanetType.EarthLike || planetType == PlanetType.Desert);
				float value3 = this.tilt.GetValueOrDefault();
				if (this.tilt == null)
				{
					value3 = (float)SeededRandom.Global.RandomRange(-45, 45);
					this.tilt = new float?(value3);
				}
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0005CAE2 File Offset: 0x0005ACE2
		public string GetTextureName()
		{
			return this.planetTypeTextures[this.planetType][this.textureIndex.Value];
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0005CB05 File Offset: 0x0005AD05
		public override void ActiveUpdate(float delta)
		{
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0005CB08 File Offset: 0x0005AD08
		public override void DataToJson(JsonObject json)
		{
			base.DataToJson(json);
			json["scale"] = new double?((double)this.scale);
			json["rotationSpeed"] = new double?((double)this.rotationSpeed);
			string key = "tilt";
			float? num = this.tilt;
			json[key] = ((num != null) ? new double?((double)num.GetValueOrDefault()) : null);
			json["planetType"] = this.planetType.ToString();
			string key2 = "texture";
			int? num2 = this.textureIndex;
			json[key2] = ((num2 != null) ? new double?((double)num2.GetValueOrDefault()) : null);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0005CBE8 File Offset: 0x0005ADE8
		public override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data);
			this.scale = (float)data["scale"].AsNumber;
			this.rotationSpeed = (float)data["rotationSpeed"].AsNumber;
			this.tilt = new float?((float)data["tilt"].AsNumber);
			this.textureIndex = new int?(data["texture"].AsInteger);
			this.pType = Enum.Parse<PlanetType>(data["planetType"]);
		}

		// Token: 0x0400070E RID: 1806
		public float scale;

		// Token: 0x0400070F RID: 1807
		public float rotationSpeed;

		// Token: 0x04000710 RID: 1808
		public PlanetType planetType;

		// Token: 0x04000711 RID: 1809
		private int? textureIndex;

		// Token: 0x04000712 RID: 1810
		public bool atmosphere;

		// Token: 0x04000713 RID: 1811
		public float? tilt;

		// Token: 0x04000714 RID: 1812
		private Dictionary<PlanetType, List<string>> planetTypeTextures;
	}
}
