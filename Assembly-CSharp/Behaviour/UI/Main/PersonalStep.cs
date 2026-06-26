using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000262 RID: 610
	public class PersonalStep : MonoBehaviour
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x0008EE57 File Offset: 0x0008D057
		public string firstName
		{
			get
			{
				return this.firstNameInputField.text;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x0008EE64 File Offset: 0x0008D064
		public string callsign
		{
			get
			{
				return this.callsignInputField.text;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x0008EE71 File Offset: 0x0008D071
		public string lastName
		{
			get
			{
				return this.lastNameInputField.text;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x0008EE7E File Offset: 0x0008D07E
		public CrewIcon SelectedIcon
		{
			get
			{
				return this.gobl ?? this.captainSprites[this.selectedIndex];
			}
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x0008EE9B File Offset: 0x0008D09B
		private void Start()
		{
			this.captainSprites = CrewIcons.GetAll();
			this.UpdateIconSelection();
			this.RandomName();
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0008EEB4 File Offset: 0x0008D0B4
		private void Update()
		{
			bool flag = this.firstNameInputField.text == "Gobl" && this.lastNameInputField.text == "Gobl";
			if (this.gobl == null && flag)
			{
				this.gobl = CrewIcons.Get("Gobl");
				this.UpdateIconSelection();
				return;
			}
			if (this.gobl != null && !flag)
			{
				this.gobl = null;
				this.UpdateIconSelection();
			}
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0008EF2D File Offset: 0x0008D12D
		public string GetFullPlayerName()
		{
			return string.Concat(new string[]
			{
				this.firstName,
				" \"",
				this.callsign,
				"\" ",
				this.lastName
			});
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x0008EF65 File Offset: 0x0008D165
		public void NextIcon()
		{
			this.selectedIndex = (this.selectedIndex + 1) % this.captainSprites.Count;
			this.UpdateIconSelection();
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x0008EF87 File Offset: 0x0008D187
		public void PreviousIcon()
		{
			this.selectedIndex = (this.selectedIndex - 1 + this.captainSprites.Count) % this.captainSprites.Count;
			this.UpdateIconSelection();
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x0008EFB5 File Offset: 0x0008D1B5
		private void UpdateIconSelection()
		{
			if (this.SelectedIcon != null)
			{
				this.icon.sprite = this.SelectedIcon.sprite;
			}
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x0008EFD5 File Offset: 0x0008D1D5
		public void RandomName()
		{
			this.firstNameInputField.text = NameGenerator.GetRandomFirstName(this.SelectedIcon.isMale, null);
			this.lastNameInputField.text = NameGenerator.GetRandomLastName(null);
			this.callsignInputField.ActivateInputField();
		}

		// Token: 0x04000DB3 RID: 3507
		[SerializeField]
		private TMP_InputField firstNameInputField;

		// Token: 0x04000DB4 RID: 3508
		[SerializeField]
		private TMP_InputField callsignInputField;

		// Token: 0x04000DB5 RID: 3509
		[SerializeField]
		private TMP_InputField lastNameInputField;

		// Token: 0x04000DB6 RID: 3510
		private List<CrewIcon> captainSprites;

		// Token: 0x04000DB7 RID: 3511
		private int selectedIndex;

		// Token: 0x04000DB8 RID: 3512
		private CrewIcon gobl;

		// Token: 0x04000DB9 RID: 3513
		[SerializeField]
		private Image icon;
	}
}
