using System;
using Behaviour.GalaxyMap;
using Behaviour.Util;
using LightJson;
using UnityEngine;

namespace Source.Galaxy.Statics
{
	// Token: 0x0200014D RID: 333
	public class Star : MapStatic
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0005CC88 File Offset: 0x0005AE88
		public override WorldMapStatic Prefab
		{
			get
			{
				return WorldMapStatic.GetPrefab("Star");
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0005CC94 File Offset: 0x0005AE94
		public override void ActiveUpdate(float delta)
		{
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0005CC96 File Offset: 0x0005AE96
		public override Color GetColor()
		{
			return Singleton<BackdropManager>.Instance.GetStarColor(this.color);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0005CCA8 File Offset: 0x0005AEA8
		public override float GetSize()
		{
			return this.intensity;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0005CCB0 File Offset: 0x0005AEB0
		public override void DataToJson(JsonObject json)
		{
			base.DataToJson(json);
			json["color"] = new double?((double)this.color);
			json["intensity"] = new double?((double)this.intensity);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0005CCFC File Offset: 0x0005AEFC
		public override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data);
			if (!data.ContainsKey("color"))
			{
				this.color = Star.GetRandomColorForType(base.name);
				this.intensity = SeededRandom.Global.RandomRange(2f, 7f);
				return;
			}
			this.color = (float)data["color"].AsNumber;
			this.intensity = (float)data["intensity"].AsNumber;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0005CD80 File Offset: 0x0005AF80
		public static float GetRandomColorForType(string type)
		{
			uint num = (uint)type.GetHashCode();
			if (num <= 733639457U)
			{
				if (num != 36174992U)
				{
					if (num != 41091819U)
					{
						if (num == 733639457U)
						{
							if (type == "O-Type")
							{
								return SeededRandom.Global.RandomRange(0f, 0.1f);
							}
						}
					}
					else if (type == "A-Type")
					{
						return SeededRandom.Global.RandomRange(0.15f, 0.33f);
					}
				}
				else if (type == "B-Type")
				{
					return SeededRandom.Global.RandomRange(0.05f, 0.15f);
				}
			}
			else if (num <= 1868302809U)
			{
				if (num != 1241283661U)
				{
					if (num == 1868302809U)
					{
						if (type == "G-Type")
						{
							return SeededRandom.Global.RandomRange(0.45f, 0.6f);
						}
					}
				}
				else if (type == "K-Type")
				{
					return SeededRandom.Global.RandomRange(0.95f, 1f);
				}
			}
			else if (num != 1931756532U)
			{
				if (num == 3959502391U)
				{
					if (type == "M-Type")
					{
						return SeededRandom.Global.RandomRange(0.75f, 0.9f);
					}
				}
			}
			else if (type == "F-Type")
			{
				return SeededRandom.Global.RandomRange(0.33f, 0.5f);
			}
			return (float)SeededRandom.Global.RandomRange(0, 1);
		}

		// Token: 0x04000715 RID: 1813
		public static readonly string[] StarType = new string[]
		{
			"O-Type",
			"B-Type",
			"A-Type",
			"F-Type",
			"G-Type",
			"K-Type",
			"M-Type"
		};

		// Token: 0x04000716 RID: 1814
		public float color;

		// Token: 0x04000717 RID: 1815
		public float intensity;
	}
}
