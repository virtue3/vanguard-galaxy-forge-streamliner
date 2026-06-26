using System;
using System.Collections.Generic;
using LightJson;
using Source.Crew;
using Source.Galaxy.POI.Station.Patrons;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x0200015E RID: 350
	public class Bar : IJsonSource
	{
		// Token: 0x06000D72 RID: 3442 RVA: 0x0006160B File Offset: 0x0005F80B
		public Bar(SpaceStation ss)
		{
			this.spaceStation = ss;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00061628 File Offset: 0x0005F828
		public void CheckUpdatePatrons(bool force = false)
		{
			if (force || new DateTime(this.lastUpdateTime).DayOfYear != DateTime.Now.DayOfYear)
			{
				this.availablePatrons.Clear();
				SeededRandom seededRandom;
				if (this.nextUpdateSeed != null)
				{
					seededRandom = new SeedGenerator().Add(this.nextUpdateSeed).CreateRandom();
				}
				else
				{
					seededRandom = SeededRandom.Global;
				}
				for (int i = 0; i < 5; i++)
				{
					bool flag;
					BarPatron barPatron;
					do
					{
						flag = false;
						barPatron = this.CreateBarPatron(i + 1, seededRandom);
						barPatron.Initialize();
						using (List<BarPatron>.Enumerator enumerator = this.availablePatrons.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.ConflictsWith(barPatron))
								{
									flag = true;
									break;
								}
							}
						}
					}
					while (flag);
					this.availablePatrons.Add(barPatron);
				}
				float num = 0.75f;
				while (seededRandom.RandomBool(num))
				{
					this.availablePatrons.Remove(seededRandom.Choose<BarPatron>(this.availablePatrons));
					num -= 0.25f;
				}
				this.nextUpdateSeed = seededRandom.RandomItemSeed();
				this.lastUpdateTime = DateTime.Now.Ticks;
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00061760 File Offset: 0x0005F960
		private BarPatron CreateBarPatron(int seat, SeededRandom random)
		{
			if (random.RandomFloat() < 0.3f)
			{
				return new CrewMember(this.spaceStation)
				{
					crewMember = CrewMemberData.CreateRandomCrewMember(random),
					seat = seat
				};
			}
			return new Salesman(random.RandomItemSeed(), this.spaceStation)
			{
				seat = seat
			};
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000617B4 File Offset: 0x0005F9B4
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"availablePatrons",
					this.availablePatrons.ToJsonArray<BarPatron>()
				},
				{
					"lastUpdateTime",
					this.lastUpdateTime.ToString()
				},
				{
					"nextUpdateSeed",
					this.nextUpdateSeed
				}
			};
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0006181C File Offset: 0x0005FA1C
		public static Bar FromJson(SpaceStation ss, JsonObject data)
		{
			Bar bar = new Bar(ss);
			if (data["availablePatrons"].IsJsonArray)
			{
				bar.availablePatrons.FromJsonArray(data["availablePatrons"], (JsonValue val) => BarPatron.FromJson(val, ss));
				bar.lastUpdateTime = long.Parse(data["lastUpdateTime"]);
			}
			bar.nextUpdateSeed = data["nextUpdateSeed"];
			return bar;
		}

		// Token: 0x0400075A RID: 1882
		public List<BarPatron> availablePatrons = new List<BarPatron>();

		// Token: 0x0400075B RID: 1883
		private long lastUpdateTime;

		// Token: 0x0400075C RID: 1884
		private string nextUpdateSeed;

		// Token: 0x0400075D RID: 1885
		private SpaceStation spaceStation;
	}
}
