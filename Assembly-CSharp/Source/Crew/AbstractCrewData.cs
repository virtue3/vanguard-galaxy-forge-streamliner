using System;
using Behaviour.Crew;
using Behaviour.Managers;
using Behaviour.UI.Side_Menu;
using Behaviour.Util;
using LightJson;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Crew
{
	// Token: 0x02000120 RID: 288
	public abstract class AbstractCrewData : IJsonSource
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00051A0C File Offset: 0x0004FC0C
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x00051A14 File Offset: 0x0004FC14
		public string firstName { get; protected set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00051A1D File Offset: 0x0004FC1D
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x00051A25 File Offset: 0x0004FC25
		public string callsign { get; protected set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00051A2E File Offset: 0x0004FC2E
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x00051A36 File Offset: 0x0004FC36
		public string lastName { get; protected set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00051A3F File Offset: 0x0004FC3F
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x00051A47 File Offset: 0x0004FC47
		public CrewIcon icon { get; private set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00051A50 File Offset: 0x0004FC50
		public Sprite sprite
		{
			get
			{
				return this.icon.sprite;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00051A5D File Offset: 0x0004FC5D
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x00051A65 File Offset: 0x0004FC65
		public float age { get; protected set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00051A6E File Offset: 0x0004FC6E
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x00051A76 File Offset: 0x0004FC76
		public string description { get; protected set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00051A7F File Offset: 0x0004FC7F
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x00051A87 File Offset: 0x0004FC87
		public int level { get; private set; } = 1;

		// Token: 0x06000B0F RID: 2831 RVA: 0x00051A90 File Offset: 0x0004FC90
		public bool IsMaxLevel()
		{
			return this.level >= GameMath.maxLevel;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00051AA2 File Offset: 0x0004FCA2
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00051AAA File Offset: 0x0004FCAA
		public float experience { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00051AB3 File Offset: 0x0004FCB3
		public virtual float maxExperience
		{
			get
			{
				return GameMath.GetMaxExperienceForLevel(this.level);
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00051AC0 File Offset: 0x0004FCC0
		public string GetExperienceProgressString()
		{
			return Mathf.RoundToInt(this.experience).ToString() + "/" + Mathf.RoundToInt(this.maxExperience).ToString();
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00051B00 File Offset: 0x0004FD00
		public float GiveExperience(float exp)
		{
			if (this.IsMaxLevel())
			{
				return 0f;
			}
			if (GamePlayer.current.autoPlay)
			{
				float currentIncrease = SkilltreeNode.PromptEngineeringExperiencePenaltyReduction.currentIncrease;
				exp *= 0.5f + GamePlayer.current.commander.GetAutopilotPenaltyReductionModifier() + currentIncrease;
				GamePlayer.current.AddAutopilotStat(IdleStat.Experience, (int)exp);
			}
			this.experience += exp;
			while (this.experience >= this.maxExperience)
			{
				this.experience -= this.maxExperience;
				int level = this.level;
				this.level = level + 1;
				Singleton<EventLogManager>.Instance.NewEvent(this.callsign, Translation.Translate("@LogLevelUp", new object[]
				{
					string.IsNullOrEmpty(this.callsign) ? this.firstName : this.callsign
				}));
				SidePanel.instance.NotifyTab((this is CommanderData) ? SidePanel.SideTabType.Captain : SidePanel.SideTabType.Crew, (this is CommanderData) ? "Skilltree" : "");
				if (GamePlayer.current.commander == this)
				{
					SteamStatsManager.Set(SteamStatType.Level, this.level);
				}
				else
				{
					CrewMemberData crewMemberData = this as CrewMemberData;
					if (crewMemberData != null)
					{
						CrewMember crewMember = GameplayManager.Instance.spaceShip.GetCrewMember(crewMemberData);
						if (crewMember)
						{
							crewMember.CheckExperience();
						}
					}
				}
			}
			return exp;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00051C4E File Offset: 0x0004FE4E
		public void SetName(string firstName, string callsign, string lastName)
		{
			this.firstName = firstName;
			this.callsign = callsign;
			this.lastName = lastName;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00051C65 File Offset: 0x0004FE65
		public void SetCallsign(string callsign)
		{
			this.callsign = callsign;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00051C6E File Offset: 0x0004FE6E
		public void SetIcon(CrewIcon icon)
		{
			this.icon = icon;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00051C78 File Offset: 0x0004FE78
		public string GetFullName()
		{
			string text = this.firstName;
			if (this.callsign != "")
			{
				text = text + " \"" + this.callsign + "\"";
			}
			if (this.lastName != "")
			{
				text = text + " " + this.lastName;
			}
			return text;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00051CDA File Offset: 0x0004FEDA
		public new string ToString()
		{
			return string.Concat(new string[]
			{
				this.firstName,
				" (",
				this.callsign,
				") ",
				this.lastName
			});
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00051D14 File Offset: 0x0004FF14
		public virtual JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"firstName",
					this.firstName
				},
				{
					"callsign",
					this.callsign
				},
				{
					"lastName",
					this.lastName
				},
				{
					"icon",
					(this.icon != null) ? this.icon.identifier : ""
				},
				{
					"age",
					new double?((double)this.age)
				},
				{
					"gender",
					this.gender.ToString()
				},
				{
					"description",
					this.description
				},
				{
					"experience",
					new double?((double)Mathf.RoundToInt(this.experience))
				},
				{
					"level",
					new double?((double)this.level)
				},
				{
					"pendingNpcMessage",
					this.pendingNpcMessage
				}
			};
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00051E48 File Offset: 0x00050048
		public virtual void DataFromJson(JsonObject data)
		{
			this.firstName = data["firstName"];
			this.callsign = data["callsign"];
			this.lastName = data["lastName"];
			if (!string.IsNullOrEmpty(data["icon"]))
			{
				this.icon = CrewIcons.Get(data["icon"]);
				this.gender = Enum.Parse<Gender>(data["gender"]);
				if (this.icon != null && this.icon.isMale != (this.gender == Gender.Male))
				{
					this.gender = (this.icon.isMale ? Gender.Male : Gender.Female);
				}
			}
			this.age = (float)data["age"];
			this.description = data["description"];
			this.experience = (float)data["experience"];
			this.level = data["level"];
			if (data.ContainsKey("pendingNpcMessage"))
			{
				this.pendingNpcMessage = data["pendingNpcMessage"];
			}
		}

		// Token: 0x040005E7 RID: 1511
		public Gender gender;

		// Token: 0x040005EB RID: 1515
		public string pendingNpcMessage;
	}
}
