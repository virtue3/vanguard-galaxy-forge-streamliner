using System;
using System.Collections.Generic;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Spacestation;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.MainMenu
{
	// Token: 0x0200026D RID: 621
	public class SaveGameUI : MonoBehaviour
	{
		// Token: 0x060016DC RID: 5852 RVA: 0x0009104A File Offset: 0x0008F24A
		private void OnEnable()
		{
			((RectTransform)base.transform).anchoredPosition = Vector2.zero;
			this.RefreshSaveGames();
			if (this.openBetaMessage)
			{
				this.openBetaMessage.SetActive(false);
			}
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00091080 File Offset: 0x0008F280
		public void SetSideMenuOptions(SideMenuOptions sideMenuOptions)
		{
			this.sideMenuOptions = sideMenuOptions;
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0009108C File Offset: 0x0008F28C
		private void RefreshSaveGames()
		{
			this.rows.Clear();
			this.savesParent.DestroyChildren();
			List<SaveGameFile> saveGames = SaveGame.GetSaveGames();
			saveGames.Sort((SaveGameFile a, SaveGameFile b) => b.Timestamp.CompareTo(a.Timestamp));
			float num = 0f;
			foreach (SaveGameFile save in saveGames)
			{
				SaveGameRow saveGameRow = UnityEngine.Object.Instantiate<SaveGameRow>(this.savePrefab, this.savesParent);
				saveGameRow.SetSave(save);
				this.rows.Add(saveGameRow);
				(saveGameRow.transform as RectTransform).anchoredPosition = new Vector2(0f, num);
				num -= 24f;
			}
			this.savesParent.sizeDelta = new Vector2(this.savesParent.sizeDelta.x, -num);
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00091184 File Offset: 0x0008F384
		private void OnDisable()
		{
			if (this.textInput)
			{
				this.textInput.text = "";
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x000911A3 File Offset: 0x0008F3A3
		private void Update()
		{
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x000911A8 File Offset: 0x0008F3A8
		public virtual void ShowSaveGame(SaveGameFile file)
		{
			this.textInput.text = file.Name;
			foreach (SaveGameRow saveGameRow in this.rows)
			{
				saveGameRow.SetHighlighted(file);
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x0009120C File Offset: 0x0008F40C
		public virtual void DoExecuteAction()
		{
			if (string.IsNullOrEmpty(this.textInput.text))
			{
				return;
			}
			try
			{
				SaveGame.DoSave(this.textInput.text);
				if (SpaceStationInterior.instance)
				{
					SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Settings, 0);
				}
				else
				{
					SidePanel.instance.CloseTab();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x0009127C File Offset: 0x0008F47C
		public void CancelSaveGame()
		{
			this.sideMenuOptions.SetMenuActive(true);
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x0009128A File Offset: 0x0008F48A
		public void EndEdit()
		{
			((RectTransform)base.transform.root.GetChild(0)).anchoredPosition = Vector2.zero;
			if (Input.GetKey(KeyCode.Return))
			{
				Debug.Log("Calling execute");
				this.DoExecuteAction();
			}
		}

		// Token: 0x04000E11 RID: 3601
		[SerializeField]
		private RectTransform savesParent;

		// Token: 0x04000E12 RID: 3602
		[SerializeField]
		private SaveGameRow savePrefab;

		// Token: 0x04000E13 RID: 3603
		[SerializeField]
		private TMP_InputField textInput;

		// Token: 0x04000E14 RID: 3604
		[SerializeField]
		private GameObject openBetaMessage;

		// Token: 0x04000E15 RID: 3605
		protected List<SaveGameRow> rows = new List<SaveGameRow>();

		// Token: 0x04000E16 RID: 3606
		protected SideMenuOptions sideMenuOptions;
	}
}
