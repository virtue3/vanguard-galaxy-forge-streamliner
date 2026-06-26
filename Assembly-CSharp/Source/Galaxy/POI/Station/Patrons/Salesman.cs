using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Dialogues;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.UI;
using Behaviour.UI.Spacestation.Bar;
using Behaviour.Util;
using LightJson;
using Source.Data.Persistable;
using Source.Dialogues;
using Source.Item;
using Source.Mining;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI.Station.Patrons
{
	// Token: 0x02000169 RID: 361
	public class Salesman : BarPatron
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x000630BA File Offset: 0x000612BA
		public override string seed
		{
			get
			{
				return this._seed;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x000630C2 File Offset: 0x000612C2
		public override string name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x000630CA File Offset: 0x000612CA
		public override bool isMale
		{
			get
			{
				return this._isMale;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x000630D2 File Offset: 0x000612D2
		public override Sprite icon
		{
			get
			{
				return this._icon;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000630DA File Offset: 0x000612DA
		public Salesman(SpaceStation ss) : base(ss)
		{
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x000630E3 File Offset: 0x000612E3
		public Salesman(string seed, SpaceStation ss) : base(ss)
		{
			this._seed = seed;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000630F4 File Offset: 0x000612F4
		protected override void InitializeData()
		{
			SeededRandom seededRandom = new SeedGenerator().Add(this.seed).CreateRandom();
			float num = seededRandom.RandomFloat();
			if (num < 0.1f)
			{
				this.SalesmanSpaceShipPNG();
				return;
			}
			if (num < 0.3f)
			{
				this.SalesmanMiningClaim(seededRandom);
				return;
			}
			if (num < 0.5f)
			{
				this.SalesmanSalvageClaim(seededRandom);
				return;
			}
			this.SalesmanEquipment(seededRandom);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00063154 File Offset: 0x00061354
		public override void AddTooltipContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.description, 12, 8f);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0006316C File Offset: 0x0006136C
		public override void InteractWithPatron(BarUI ui)
		{
			Singleton<DialogueManager>.Instance.StartDialogue(this.dialogueLines, delegate
			{
				ui.ShowSalesmanInfo(this);
			});
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000631A9 File Offset: 0x000613A9
		public override void DataFromJson(JsonObject data)
		{
			this._seed = data["seed"];
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000631C1 File Offset: 0x000613C1
		public override void DataToJson(JsonObject data)
		{
			data["seed"] = this._seed;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000631DC File Offset: 0x000613DC
		private void SalesmanSpaceShipPNG()
		{
			this._name = "Robert Miyama";
			this._isMale = true;
			this._icon = CrewIcons.Get("Man02").sprite;
			this.description = "Slick Entrepreneur";
			this.itemForSale = "SpaceShipPng";
			this.itemCost = this.itemForSale.cost * 251;
			Character character = new Character(this.name).WithPortret(this.icon);
			this.dialogueLines = new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hi, you're just the enterprising kind of captain I'm looking for!"),
				DialogueLine.cDL(character, "Interested in a once-in-a-lifetime chance to invest in future production of space ships? Right now they're just drawings on paper, but imagine the money you'll save by getting in early!"),
				DialogueLine.cDL(Characters.captain, "Wow, that sounds like an amazing deal!")
			};
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0006329C File Offset: 0x0006149C
		private void SalesmanRandomIdentity(SeededRandom random)
		{
			this._isMale = random.RandomBool(0.5f);
			this._name = NameGenerator.GetRandomFirstName(this._isMale, random) + " " + NameGenerator.GetRandomLastName(random);
			this._icon = CrewIcons.GetRandom(this._isMale, random).sprite;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x000632F4 File Offset: 0x000614F4
		private void SalesmanEquipment(SeededRandom random)
		{
			this.SalesmanRandomIdentity(random);
			Rarity rarity;
			if (this.spaceStation.level < 5)
			{
				rarity = Rarity.Enhanced;
			}
			else if (this.spaceStation.level < 15)
			{
				rarity = Rarity.HighGrade;
			}
			else
			{
				rarity = (random.RandomBool(0.2f) ? Rarity.Exotic : Rarity.HighGrade);
			}
			this.itemForSale = random.Choose<EquipmentBuilder>(EquipmentBuilder.GetItemsForGeneralShop(this.spaceStation.level)).CreateItemType(rarity, this.spaceStation.level, true, this.seed, false, false);
			this.itemCost = this.itemForSale.cost;
			Manufacturer? manufacturer;
			string text = ((this.itemForSale.GetManufacturer() != null) ? manufacturer.GetValueOrDefault().GetDisplayName() : null) ?? "Sales";
			this.description = text + " Representative";
			Character character = new Character(this.name).WithPortret(this.icon);
			this.dialogueLines = new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hi there! Are you by any chance interested in new equipment? As a fully qualified representative for " + text + ", I can offer some deals that are not available to the general public."),
				DialogueLine.cDL(Characters.captain, "Sure, I'll take a look.")
			};
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00063418 File Offset: 0x00061618
		private void SalesmanSalvageClaim(SeededRandom random)
		{
			this.SalesmanRandomIdentity(random);
			this.description = "Salvage Scout";
			List<PersistableData> list = new List<PersistableData>();
			for (int i = 0; i < 2; i++)
			{
				SalvageData salvageData = new SalvageData
				{
					position = new Vector2((float)(8 + i * 9), (float)(2 + i * 9)),
					angle = (float)SeededRandom.Global.RandomRange(0, 360),
					shipTemplate = "AncientWreck"
				};
				salvageData.AddItemContent(this.spaceStation.level, 3, 2f);
				salvageData.AddScrapContent(this.spaceStation.level, 1f, 2);
				salvageData.AddStructuralContent(this.spaceStation.level, 2, 1f);
				list.Add(salvageData);
			}
			this.itemForSale = ItemBuilder.Get("SalvageClaim").CreateSalvageClaim(this.spaceStation.system, list);
			this.itemCost = this.itemForSale.cost;
			Character character = new Character(this.name).WithPortret(this.icon);
			this.dialogueLines = new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hey, looking for some prime quality scrap to salvage? I found the remnants of an ancient space battle, ready to be picked apart. For the right price I'll send you the coordinates."),
				DialogueLine.cDL(Characters.captain, "Hmm, that does sound interesting.")
			};
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00063558 File Offset: 0x00061758
		private void SalesmanMiningClaim(SeededRandom random)
		{
			this.SalesmanRandomIdentity(random);
			this.description = "Prospector";
			int level = this.spaceStation.level + 3;
			AsteroidFieldData asteroidFieldData = new AsteroidFieldData(random.RandomRange(3, 6), 1f, AsteroidFieldData.CalculateWealth(level, 1.5f), AsteroidFieldData.CreateOreSet(level, true), AsteroidFieldData.CreateOreSet(level, false), -1f);
			asteroidFieldData.SwapCommonRare();
			this.itemForSale = ItemBuilder.Get("MiningClaim").CreateMiningClaim(this.spaceStation.system, asteroidFieldData);
			this.itemCost = this.itemForSale.cost;
			string str = Translation.Translate(asteroidFieldData.surfaceOres.commonOre.item.displayName, Array.Empty<object>());
			Character character = new Character(this.name).WithPortret(this.icon);
			this.dialogueLines = new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hey, did ya hear? A rogue asteroid has appeared in the system recently. Word on the street is, it's the motherlode! Interested in the location of this " + str + "-rich asteroid?"),
				DialogueLine.cDL(Characters.captain, "Sure, let me see what you have.")
			};
		}

		// Token: 0x0400077E RID: 1918
		public string _seed;

		// Token: 0x0400077F RID: 1919
		public string _name;

		// Token: 0x04000780 RID: 1920
		public bool _isMale;

		// Token: 0x04000781 RID: 1921
		public Sprite _icon;

		// Token: 0x04000782 RID: 1922
		private List<DialogueLine> dialogueLines;

		// Token: 0x04000783 RID: 1923
		public string description;

		// Token: 0x04000784 RID: 1924
		public InventoryItemType itemForSale;

		// Token: 0x04000785 RID: 1925
		public int itemCost;
	}
}
