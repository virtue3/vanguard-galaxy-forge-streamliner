using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.AudioSystem;
using Behaviour.Util;
using Source.AudioSystem;
using Source.Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Behaviour.Dialogues
{
	// Token: 0x0200039E RID: 926
	public class DialogueManager : Singleton<DialogueManager>
	{
		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x000C9542 File Offset: 0x000C7742
		public static bool isOpen
		{
			get
			{
				return Singleton<DialogueManager>.instance && Singleton<DialogueManager>.instance.dialogueContainer.gameObject.activeSelf;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000C9566 File Offset: 0x000C7766
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x000C956E File Offset: 0x000C776E
		public DialogueContainer dialogueContainer { get; private set; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000C9577 File Offset: 0x000C7777
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x000C957F File Offset: 0x000C777F
		public GameObject saleContainer { get; private set; }

		// Token: 0x06002316 RID: 8982 RVA: 0x000C9588 File Offset: 0x000C7788
		private void Update()
		{
			if (this.dialogue.Count > 0 && Input.GetKeyDown(KeyCode.Space))
			{
				this.NextOrFinish();
			}
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000C95A8 File Offset: 0x000C77A8
		public void StartDefaultDialogue(Character character = null)
		{
			this.dialogue.Clear();
			Source.Dialogues.Dialogue dialogue = (character != null) ? character.GetDefaultDialogue() : null;
			if (dialogue != null)
			{
				this.dialogue = dialogue.dialogues();
			}
			else
			{
				this.dialogue.Add(DialogueLine.cDL(Characters.captain, "Nothing to be said."));
			}
			this.dialogueContainer.gameObject.SetActive(true);
			this.LoadDialogueAndStart(this.dialogue, (dialogue != null) ? dialogue.onComplete : null);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000C9626 File Offset: 0x000C7826
		public void StartDialogue(List<DialogueLine> dialogue, Action onComplete = null)
		{
			if (!this.dialogueContainer.isActiveAndEnabled)
			{
				GameManager.Pause();
			}
			this.dialogueContainer.gameObject.SetActive(true);
			this.LoadDialogueAndStart(dialogue, onComplete);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000C9654 File Offset: 0x000C7854
		private void LoadDialogueAndStart(List<DialogueLine> dialogue, Action onComplete = null)
		{
			PersistentSingleton<SoundManager>.Instance.CreateSound().WithSoundData(this.startConvoSound).WithRandomPitch().Play();
			this.dialogue = new List<DialogueLine>(dialogue);
			this.onComplete = onComplete;
			this.currentLineIndex = 0;
			this.isDialogueOpen = true;
			this.ShowDialogueLine();
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000C96A7 File Offset: 0x000C78A7
		public void NextOrFinish()
		{
			if (this.dialogueText.textInfo.characterCount != this.dialogueText.maxVisibleCharacters)
			{
				this.skipText = true;
				return;
			}
			this.NextDialogue();
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000C96D4 File Offset: 0x000C78D4
		public void NextDialogue()
		{
			if (this.currentLineIndex < this.dialogue.Count - 1)
			{
				this.currentLineIndex++;
				this.ShowDialogueLine();
				return;
			}
			Action action = this.onComplete;
			if (action != null)
			{
				action();
			}
			this.CloseDialogue();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000C9722 File Offset: 0x000C7922
		public void PreviousDialogue()
		{
			if (this.currentLineIndex > 0)
			{
				this.currentLineIndex--;
				this.ShowDialogueLine();
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000C9741 File Offset: 0x000C7941
		public void CloseDialogue()
		{
			this.isDialogueOpen = false;
			this.SetDialogueFinished();
			GameManager.Unpause();
			this.dialogueContainer.gameObject.SetActive(false);
			this.dialogue.Clear();
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000C9774 File Offset: 0x000C7974
		private void ShowDialogueLine()
		{
			DialogueLine dialogueLine = this.dialogue[this.currentLineIndex];
			this.SetCharacterName(dialogueLine);
			this.SetPortret(dialogueLine);
			base.StartCoroutine(this.SetDialogueText(dialogueLine.text, dialogueLine.trigger));
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000C97BA File Offset: 0x000C79BA
		private void SetCharacterName(DialogueLine line)
		{
			this.characterName.text = line.character.name;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000C97D2 File Offset: 0x000C79D2
		private void SetPortret(DialogueLine line)
		{
			this.iconImage.sprite = line.character.portretSprite;
			this.iconImage.preserveAspect = true;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000C97F6 File Offset: 0x000C79F6
		private IEnumerator SetDialogueText(string text, Action trigger = null)
		{
			this.skipText = false;
			this.dialogueText.text = "\"" + text + "\"";
			this.dialogueText.ForceMeshUpdate(false, false);
			this.dialogueText.maxVisibleCharacters = 0;
			int totalCharacters = this.dialogueText.textInfo.characterCount;
			float num = 0.02f;
			float fillDuration = num * (float)totalCharacters;
			float elapsedTime = 0f;
			while (elapsedTime < fillDuration && !this.skipText)
			{
				int maxVisibleCharacters = Mathf.FloorToInt(Mathf.Clamp01(elapsedTime / fillDuration) * (float)totalCharacters);
				this.dialogueText.maxVisibleCharacters = maxVisibleCharacters;
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.dialogueText.maxVisibleCharacters = totalCharacters;
			if (!this.skipText)
			{
				yield return new WaitForSeconds(0.5f);
			}
			if (trigger != null)
			{
				trigger();
			}
			yield break;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000C9813 File Offset: 0x000C7A13
		private void SetDialogueFinished()
		{
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000C9815 File Offset: 0x000C7A15
		public static AsyncOperation LoadDialogueScene()
		{
			return SceneManager.LoadSceneAsync("UI - Dialogue", LoadSceneMode.Additive);
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000C9822 File Offset: 0x000C7A22
		public static void UnloadDialogueScene()
		{
			SceneManager.UnloadSceneAsync("UI - Dialogue");
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000C982F File Offset: 0x000C7A2F
		public bool IsDialogueOpen()
		{
			return this.isDialogueOpen;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000C9837 File Offset: 0x000C7A37
		public void Cleanup()
		{
			this.CloseDialogue();
		}

		// Token: 0x040014F6 RID: 5366
		[SerializeField]
		private TextMeshProUGUI dialogueText;

		// Token: 0x040014F7 RID: 5367
		[SerializeField]
		private TextMeshProUGUI characterName;

		// Token: 0x040014F8 RID: 5368
		[SerializeField]
		private Image iconImage;

		// Token: 0x040014F9 RID: 5369
		[SerializeField]
		private TextMeshProUGUI actionPrompt;

		// Token: 0x040014FA RID: 5370
		private List<DialogueLine> dialogue = new List<DialogueLine>();

		// Token: 0x040014FB RID: 5371
		private Action onComplete;

		// Token: 0x040014FC RID: 5372
		private int currentLineIndex;

		// Token: 0x040014FD RID: 5373
		private bool isDialogueOpen;

		// Token: 0x040014FE RID: 5374
		private bool skipText;

		// Token: 0x040014FF RID: 5375
		[SerializeField]
		private SoundData startConvoSound;
	}
}
