using System;
using System.Collections.Generic;
using System.Linq;
using Source.MissionSystem;
using Source.Player;
using UnityEngine;

namespace Source.Dialogues
{
	// Token: 0x020000FE RID: 254
	public class Character
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00048108 File Offset: 0x00046308
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x00048168 File Offset: 0x00046368
		public string givesMissionId
		{
			get
			{
				foreach (string text in this.missionIds)
				{
					if (!GamePlayer.current.HasStoryMission(text))
					{
						return text;
					}
				}
				return null;
			}
			set
			{
				this.missionIds.Add(value);
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00048178 File Offset: 0x00046378
		public bool missionAvailable
		{
			get
			{
				foreach (string text in this.missionIds)
				{
					if (!GamePlayer.current.HasStoryMission(text) && StoryMission.IsAvailable(GamePlayer.current, text))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x000481E8 File Offset: 0x000463E8
		public string missionAvailableHint
		{
			get
			{
				foreach (string text in this.missionIds)
				{
					if (!GamePlayer.current.HasStoryMission(text))
					{
						StoryMission storyMission = StoryMission.Get(text);
						if (storyMission.pickupHint != null && storyMission.IsAvailableFor(GamePlayer.current))
						{
							return storyMission.pickupHint;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0004826C File Offset: 0x0004646C
		public static Character CreateCharacter(string name)
		{
			return new Character(name);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00048274 File Offset: 0x00046474
		public Character(string name)
		{
			this.name = name;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00048299 File Offset: 0x00046499
		public Character WithPortret(Sprite portret)
		{
			this.portretSprite = portret;
			return this;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000482A3 File Offset: 0x000464A3
		public Character WithDescription(string description)
		{
			this.description = description;
			return this;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x000482B0 File Offset: 0x000464B0
		public void AddDialogue(MissionTrigger id, Func<List<DialogueLine>> dialogueFunc, Action onComplete = null, bool conditionalTrigger = false)
		{
			if (this.dialogues.Any((Dialogue d) => d.trigger == id))
			{
				return;
			}
			Dialogue item = new Dialogue
			{
				trigger = id,
				dialogues = dialogueFunc,
				onComplete = onComplete,
				conditionalTrigger = conditionalTrigger
			};
			this.dialogues.Add(item);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00048318 File Offset: 0x00046518
		public void AddDefaultDialogue(Dialogue dialogue)
		{
			this.defaultDialogue = dialogue;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00048324 File Offset: 0x00046524
		public void AddDefaultDialogue(List<DialogueLine> lines)
		{
			this.defaultDialogue = new Dialogue
			{
				dialogues = (() => lines)
			};
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0004835B File Offset: 0x0004655B
		public Dialogue GetDefaultDialogue()
		{
			if (this.createDialogue != null)
			{
				return this.createDialogue(this);
			}
			return this.defaultDialogue;
		}

		// Token: 0x04000531 RID: 1329
		public string name;

		// Token: 0x04000532 RID: 1330
		public string description;

		// Token: 0x04000533 RID: 1331
		public Sprite portretSprite;

		// Token: 0x04000534 RID: 1332
		public List<Dialogue> dialogues = new List<Dialogue>();

		// Token: 0x04000535 RID: 1333
		public List<string> missionIds = new List<string>();

		// Token: 0x04000536 RID: 1334
		public Func<Character, Dialogue> createDialogue;

		// Token: 0x04000537 RID: 1335
		public Dialogue defaultDialogue;
	}
}
