using System;
using System.Linq;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Dialogues;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.Dialogues
{
	// Token: 0x0200039B RID: 923
	public class CharacterMono : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITooltipCustomSource
	{
		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000C931A File Offset: 0x000C751A
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x000C9322 File Offset: 0x000C7522
		public Character character { get; private set; }

		// Token: 0x06002304 RID: 8964 RVA: 0x000C932B File Offset: 0x000C752B
		private void Awake()
		{
			this.borderColor = this.border.color;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000C933E File Offset: 0x000C753E
		private void Update()
		{
			this.highlightTimer += Time.deltaTime;
			if (this.highlightTimer >= 0.5f)
			{
				this.UpdateHighlight();
				this.highlightTimer = 0f;
			}
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000C9370 File Offset: 0x000C7570
		public void SetCharacter(Character character)
		{
			this.character = character;
			this.characterImage.sprite = (character.portretSprite ?? null);
			this.UpdateHighlight();
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000C9398 File Offset: 0x000C7598
		private void UpdateHighlight()
		{
			this.questHighlight.gameObject.SetActive(GamePlayer.current.GetCurrentDialogueTrigger(this.character) != null || this.character.missionAvailable);
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000C93E0 File Offset: 0x000C75E0
		public void TalkTo()
		{
			this.OnPointerExit(null);
			MissionTrigger? trigger = GamePlayer.current.GetCurrentDialogueTrigger(this.character);
			if (trigger != null)
			{
				Source.Dialogues.Dialogue dialogue = this.character.dialogues.FirstOrDefault((Source.Dialogues.Dialogue d) => d.trigger == trigger.Value);
				if (dialogue != null)
				{
					if (!dialogue.conditionalTrigger)
					{
						MissionObjective.Trigger(trigger.Value, 1, null, false);
					}
					Singleton<DialogueManager>.Instance.StartDialogue(dialogue.dialogues(), dialogue.onComplete);
				}
				else
				{
					this.StartDefaultDialogue();
				}
			}
			else
			{
				this.StartDefaultDialogue();
			}
			this.UpdateHighlight();
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000C948E File Offset: 0x000C768E
		private void StartDefaultDialogue()
		{
			Singleton<DialogueManager>.Instance.StartDefaultDialogue(this.character);
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000C94A0 File Offset: 0x000C76A0
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.border.color = this.highlightColor;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000C94B3 File Offset: 0x000C76B3
		public void OnPointerExit(PointerEventData eventData)
		{
			this.border.color = this.borderColor;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000C94C8 File Offset: 0x000C76C8
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.character.name, 14, 8f);
			tooltip.AddTextLine(this.character.description, 12, 8f).Text.color = ColorHelper.boringGrey;
		}

		// Token: 0x040014EB RID: 5355
		[SerializeField]
		private Image border;

		// Token: 0x040014EC RID: 5356
		[SerializeField]
		private Image characterImage;

		// Token: 0x040014ED RID: 5357
		[SerializeField]
		private Image questHighlight;

		// Token: 0x040014EE RID: 5358
		[SerializeField]
		private Color highlightColor;

		// Token: 0x040014EF RID: 5359
		private Color borderColor;

		// Token: 0x040014F0 RID: 5360
		private float highlightTimer;
	}
}
