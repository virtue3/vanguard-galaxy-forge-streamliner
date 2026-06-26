using System;
using Behaviour.Util;
using Source.Util;

namespace Behaviour.UI.MainMenu
{
	// Token: 0x0200026C RID: 620
	public class LoadGameUI : SaveGameUI
	{
		// Token: 0x060016D8 RID: 5848 RVA: 0x00090FB8 File Offset: 0x0008F1B8
		public override void ShowSaveGame(SaveGameFile file)
		{
			this.selectedFile = file;
			foreach (SaveGameRow saveGameRow in this.rows)
			{
				saveGameRow.SetHighlighted(file);
			}
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00091010 File Offset: 0x0008F210
		public override void DoExecuteAction()
		{
			if (this.selectedFile != null)
			{
				PersistentSingleton<GameManager>.Instance.LoadGame(this.selectedFile);
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x0009102A File Offset: 0x0008F22A
		public void CancelLoadGame()
		{
			if (MainMenuUI.instance)
			{
				MainMenuUI.instance.CancelLoadGame();
			}
		}

		// Token: 0x04000E10 RID: 3600
		private SaveGameFile selectedFile;
	}
}
