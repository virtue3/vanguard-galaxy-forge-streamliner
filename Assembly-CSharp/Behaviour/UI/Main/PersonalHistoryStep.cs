using System;
using System.Collections.Generic;
using Source.Crew;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000261 RID: 609
	public class PersonalHistoryStep : MonoBehaviour
	{
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x0008EA7A File Offset: 0x0008CC7A
		// (set) Token: 0x06001670 RID: 5744 RVA: 0x0008EA82 File Offset: 0x0008CC82
		public PersonalHistory selectedPersonalHistory { get; private set; }

		// Token: 0x06001671 RID: 5745 RVA: 0x0008EA8C File Offset: 0x0008CC8C
		private void Start()
		{
			float num = 0f;
			foreach (object obj in Enum.GetValues(typeof(PersonalHistory)))
			{
				PersonalHistory personalHistory = (PersonalHistory)obj;
				PersonalHistoryButton personalHistoryButton = UnityEngine.Object.Instantiate<PersonalHistoryButton>(this.personalHistoryButton, this.container);
				personalHistoryButton.SetPersonalHistory(personalHistory);
				((RectTransform)personalHistoryButton.transform).anchoredPosition = new Vector2(0f, num);
				personalHistoryButton.SetLocked(new PersonalHistoryData(personalHistory).locked);
				this.buttons.Add(personalHistoryButton);
				num -= 24f;
			}
			this.container.sizeDelta = new Vector2(this.container.sizeDelta.x, -num);
			this.OnChangePersonalHistory(PersonalHistory.Miner);
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0008EB74 File Offset: 0x0008CD74
		public void OnChangePersonalHistory(PersonalHistory personalHistory)
		{
			this.selectedPersonalHistory = personalHistory;
			foreach (PersonalHistoryButton personalHistoryButton in this.buttons)
			{
				personalHistoryButton.SetSelected(personalHistory);
			}
			this.PopulateInformationPanel();
			this.nextButton.gameObject.SetActive(!this.personalHistoryData.locked);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x0008EBF0 File Offset: 0x0008CDF0
		private void OnEnable()
		{
			this.nextButton.gameObject.SetActive(this.personalHistoryData == null || !this.personalHistoryData.locked);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0008EC1B File Offset: 0x0008CE1B
		private void OnDisable()
		{
			this.nextButton.gameObject.SetActive(true);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0008EC30 File Offset: 0x0008CE30
		private void PopulateInformationPanel()
		{
			this.personalHistoryData = new PersonalHistoryData(this.selectedPersonalHistory);
			this.credits.text = Translation.Translate("@NGStartingCredits", Array.Empty<object>()) + ": " + this.personalHistoryData.starterCredits.ToString().HighlightWithColor(ColorHelper.creditsColor);
			this.crewmates.text = Translation.Translate("@UICrew", Array.Empty<object>()) + ": " + this.personalHistoryData.starterCrewMembersAmount.ToString().HighlightWithColor((this.personalHistoryData.starterCrewMembersAmount > 0) ? ColorHelper.greenish : ColorHelper.reddish);
			this.skilltrees.text = Translation.Translate("@NGStarterSkilltree", Array.Empty<object>()) + ": " + this.personalHistoryData.starterSpecialization.ToString().HighlightWithColor(ColorHelper.greenish);
			this.starterShipsAmount.text = Translation.Translate("@NGShipChoices", Array.Empty<object>()) + ": ";
			int count = this.personalHistoryData.starterShips.Count;
			int num = 1;
			foreach (string text in this.personalHistoryData.starterShips)
			{
				if (num == count)
				{
					TextMeshProUGUI textMeshProUGUI = this.starterShipsAmount;
					textMeshProUGUI.text += text.HighlightWithColor(ColorHelper.greenish);
				}
				else
				{
					TextMeshProUGUI textMeshProUGUI2 = this.starterShipsAmount;
					textMeshProUGUI2.text = textMeshProUGUI2.text + text.HighlightWithColor(ColorHelper.greenish) + ", ".HighlightWithColor(ColorHelper.greenish);
				}
				num++;
			}
			this.description.text = Translation.Translate(this.personalHistoryData.description, Array.Empty<object>()).HighlightWithColor(this.personalHistoryData.locked ? ColorHelper.reddish : ColorHelper.white75);
		}

		// Token: 0x04000DA8 RID: 3496
		[SerializeField]
		private PersonalHistoryButton personalHistoryButton;

		// Token: 0x04000DA9 RID: 3497
		[SerializeField]
		private RectTransform container;

		// Token: 0x04000DAA RID: 3498
		[SerializeField]
		private TextMeshProUGUI credits;

		// Token: 0x04000DAB RID: 3499
		[SerializeField]
		private TextMeshProUGUI crewmates;

		// Token: 0x04000DAC RID: 3500
		[SerializeField]
		private TextMeshProUGUI skilltrees;

		// Token: 0x04000DAD RID: 3501
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x04000DAE RID: 3502
		[SerializeField]
		private TextMeshProUGUI starterShipsAmount;

		// Token: 0x04000DAF RID: 3503
		private PersonalHistoryData personalHistoryData;

		// Token: 0x04000DB0 RID: 3504
		private List<PersonalHistoryButton> buttons = new List<PersonalHistoryButton>();

		// Token: 0x04000DB1 RID: 3505
		[SerializeField]
		private Button nextButton;
	}
}
