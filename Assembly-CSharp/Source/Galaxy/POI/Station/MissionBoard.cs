using System;
using System.Collections.Generic;
using Behaviour.Unit;
using Behaviour.Weapons;
using LightJson;
using Source.Galaxy.Factions;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000162 RID: 354
	public class MissionBoard : IJsonSource
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00062257 File Offset: 0x00060457
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0006225F File Offset: 0x0006045F
		public List<Mission> availableMissions { get; private set; } = new List<Mission>();

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00062268 File Offset: 0x00060468
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x00062270 File Offset: 0x00060470
		public Mission umbralMission { get; private set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00062279 File Offset: 0x00060479
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x00062281 File Offset: 0x00060481
		public string nextMissionSeed { get; private set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0006228A File Offset: 0x0006048A
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x00062292 File Offset: 0x00060492
		public float timer { get; private set; } = 300f;

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x0006229B File Offset: 0x0006049B
		public float remainingTime
		{
			get
			{
				return 300f - this.timer;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x000622A9 File Offset: 0x000604A9
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x000622B1 File Offset: 0x000604B1
		private Action refreshCallback { get; set; }

		// Token: 0x06000D9B RID: 3483 RVA: 0x000622BA File Offset: 0x000604BA
		public MissionBoard(SpaceStation owner)
		{
			this.owner = owner;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000622DF File Offset: 0x000604DF
		public void SetRefreshCallback(Action callback)
		{
			this.refreshCallback = callback;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000622E8 File Offset: 0x000604E8
		public void ProgressTimer(float delta)
		{
			this.timer += delta;
			if (this.timer >= 300f && SpaceStation.current == this.owner)
			{
				this.timer = 0f;
				this.RegenerateMissions(6);
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00062324 File Offset: 0x00060524
		public void RegenerateMissions(int missionCount = 6)
		{
			int bonusMissionAmount = this.owner.faction.GetReputationLevel(Faction.player).GetBonusMissionAmount();
			int num = missionCount + bonusMissionAmount;
			SeededRandom seededRandom = this.GetSeededRandom();
			this.availableMissions.Clear();
			GameplayManager instance = GameplayManager.Instance;
			GameplayType? gameplayType;
			if (instance == null)
			{
				gameplayType = null;
			}
			else
			{
				SpaceShip spaceShip = instance.spaceShip;
				gameplayType = ((spaceShip != null) ? new GameplayType?(spaceShip.GetPreferredGameplayType(false)) : null);
			}
			GameplayType? gameplayType2 = gameplayType;
			GameplayManager instance2 = GameplayManager.Instance;
			TargetLayer? targetLayer;
			if (instance2 == null)
			{
				targetLayer = null;
			}
			else
			{
				SpaceShip spaceShip2 = instance2.spaceShip;
				targetLayer = ((spaceShip2 != null) ? new TargetLayer?(spaceShip2.preferredTargetLayer) : null);
			}
			TargetLayer? targetLayer2 = targetLayer;
			if (this.owner.faction == Faction.tradingGuild)
			{
				gameplayType2 = null;
			}
			int num2 = 0;
			while (this.availableMissions.Count < num && num2 < 50)
			{
				num2++;
				this.GenerateMission(seededRandom, (num2 <= 2) ? gameplayType2 : null, false, seededRandom.RandomBool(0.75f) ? targetLayer2 : null);
			}
			seededRandom.Shuffle<Mission>(this.availableMissions);
			Action refreshCallback = this.refreshCallback;
			if (refreshCallback == null)
			{
				return;
			}
			refreshCallback();
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0006245C File Offset: 0x0006065C
		private SeededRandom GetSeededRandom()
		{
			SeededRandom seededRandom;
			if (this.nextMissionSeed != null)
			{
				seededRandom = new SeedGenerator().Add(this.nextMissionSeed).CreateRandom();
			}
			else
			{
				seededRandom = SeededRandom.Global;
			}
			this.nextMissionSeed = seededRandom.RandomItemSeed();
			return seededRandom;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0006249C File Offset: 0x0006069C
		private Mission GenerateMission(SeededRandom random, GameplayType? gameplayType, bool idle = false, TargetLayer? targetLayer = null)
		{
			Mission mission = MissionGenerator.GenerateRandomMission(this.owner, random, gameplayType, idle, targetLayer);
			if (mission != null)
			{
				this.availableMissions.Add(mission);
			}
			return mission;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000624CC File Offset: 0x000606CC
		public Mission GenerateIdleMission()
		{
			GameplayType? gameplayType = new GameplayType?(GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false));
			TargetLayer? targetLayer = new TargetLayer?(GameplayManager.Instance.spaceShip.preferredTargetLayer);
			SeededRandom seededRandom = this.GetSeededRandom();
			string str = "Try to create mission : ";
			GameplayType? gameplayType2 = gameplayType;
			string str2 = gameplayType2.ToString();
			string str3 = ", layer: ";
			TargetLayer? targetLayer2 = targetLayer;
			Debug.Log(str + str2 + str3 + targetLayer2.ToString());
			if (this.owner.faction.missionTypes.Contains("Courier") && GamePlayer.current.currentSpaceShip.GetCargoAvailable() > 60f)
			{
				gameplayType = (seededRandom.RandomBool(0.1f) ? new GameplayType?(GameplayType.Cargo) : gameplayType);
			}
			int num = 0;
			Mission mission;
			for (;;)
			{
				mission = this.GenerateMission(seededRandom, gameplayType, true, targetLayer);
				if (mission != null && (mission.enemyFaction == null || Faction.player.IsEnemy(mission.enemyFaction)))
				{
					break;
				}
				this.availableMissions.Remove(mission);
				num++;
				if (num >= 10)
				{
					goto Block_6;
				}
			}
			return mission;
			Block_6:
			return null;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x000625D3 File Offset: 0x000607D3
		public void AcceptMission(Mission mission)
		{
			this.availableMissions.Remove(mission);
			if (mission == this.umbralMission)
			{
				this.umbralMission = null;
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000625F4 File Offset: 0x000607F4
		public Mission GetUmbralMission(bool force = false)
		{
			int dayOfYear = DateTime.Now.DayOfYear;
			if (force || dayOfYear != this.umbralMissionDate)
			{
				this.umbralMission = ((Puppeteers)Faction.puppeteers).GenerateUmbralMission(this, force);
				this.umbralMissionDate = dayOfYear;
			}
			return this.umbralMission;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00062640 File Offset: 0x00060840
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject
			{
				{
					"owner",
					this.owner.guid
				},
				{
					"availableMissions",
					this.availableMissions.ToJsonArray<Mission>()
				},
				{
					"timer",
					new double?((double)this.timer)
				},
				{
					"nextMissionSeed",
					this.nextMissionSeed
				}
			};
			if (this.umbralMission != null)
			{
				jsonObject["umbralMission"] = this.umbralMission.ToJson();
			}
			if (this.umbralMissionDate > 0)
			{
				jsonObject["umbralMissionDate"] = new double?((double)this.umbralMissionDate);
			}
			return jsonObject;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00062708 File Offset: 0x00060908
		public static MissionBoard FromJson(SpaceStation owner, JsonObject data)
		{
			MissionBoard missionBoard = new MissionBoard(owner);
			missionBoard.availableMissions.FromJsonArray(data["availableMissions"], new ClassExtensions.ParseJsonValue<Mission>(Mission.FromJson));
			missionBoard.timer = (float)data["timer"].AsNumber;
			missionBoard.nextMissionSeed = data["nextMissionSeed"];
			missionBoard.umbralMissionDate = data["umbralMissionDate"];
			if (!data["umbralMission"].IsNull)
			{
				missionBoard.umbralMission = Mission.FromJson(data["umbralMission"]);
			}
			return missionBoard;
		}

		// Token: 0x04000766 RID: 1894
		public readonly SpaceStation owner;

		// Token: 0x0400076A RID: 1898
		public int umbralMissionDate;

		// Token: 0x0400076C RID: 1900
		public const float RefreshTime = 300f;

		// Token: 0x0400076D RID: 1901
		public const int MissionCount = 6;
	}
}
