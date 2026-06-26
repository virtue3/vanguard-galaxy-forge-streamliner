using System;
using System.Collections.Generic;
using Behaviour.Dialogues;
using Behaviour.UI;
using Behaviour.UI.Spacestation.Bar;
using Behaviour.Util;
using LightJson;
using Source.Crew;
using Source.Dialogues;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI.Station.Patrons
{
	// Token: 0x02000168 RID: 360
	public class CrewMember : BarPatron
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00062F60 File Offset: 0x00061160
		public override string seed
		{
			get
			{
				return this.crewMember.guid;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00062F6D File Offset: 0x0006116D
		public override string name
		{
			get
			{
				return this.crewMember.GetFullName();
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00062F7A File Offset: 0x0006117A
		public override bool isMale
		{
			get
			{
				return this.crewMember.gender == Gender.Male;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00062F8A File Offset: 0x0006118A
		public override Sprite icon
		{
			get
			{
				return this.crewMember.icon.sprite;
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00062F9C File Offset: 0x0006119C
		public CrewMember(SpaceStation ss) : base(ss)
		{
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00062FA8 File Offset: 0x000611A8
		public override void AddTooltipContent(UITooltip tooltip)
		{
			tooltip.AddTextLine("@UIBarCrewmember", 12, 8f);
			tooltip.AddTextLine(Translation.Translate("@UIBarCrewmemberDesc", new object[]
			{
				this.crewMember.rarity.GetDisplayName(),
				this.crewMember.profession.GetDisplayName()
			}), 12, 8f);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0006300C File Offset: 0x0006120C
		public override void InteractWithPatron(BarUI ui)
		{
			Character character = new Character(this.crewMember.firstName).WithPortret(this.icon);
			Singleton<DialogueManager>.Instance.StartDialogue(new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Heya, heard you're looking for crew?"),
				DialogueLine.cDL(Characters.captain, "You bet!")
			}, delegate
			{
				ui.ShowCrewInfo(this);
			});
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0006308A File Offset: 0x0006128A
		public override void DataFromJson(JsonObject data)
		{
			this.crewMember = CrewMemberData.FromJson(data["crewMember"]);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x000630A2 File Offset: 0x000612A2
		public override void DataToJson(JsonObject data)
		{
			data["crewMember"] = this.crewMember.ToJson();
		}

		// Token: 0x0400077D RID: 1917
		public CrewMemberData crewMember;
	}
}
