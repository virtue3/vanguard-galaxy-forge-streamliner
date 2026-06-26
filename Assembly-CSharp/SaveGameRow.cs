using System;
using Behaviour.UI.MainMenu;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class SaveGameRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060000B7 RID: 183 RVA: 0x0000576C File Offset: 0x0000396C
	private void Start()
	{
		this._parent = base.GetComponentInParent<SaveGameUI>();
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0000577A File Offset: 0x0000397A
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.clickCount == 2)
		{
			this._parent.DoExecuteAction();
			return;
		}
		this._parent.ShowSaveGame(this._file);
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000057A4 File Offset: 0x000039A4
	public void SetSave(SaveGameFile file)
	{
		this._file = file;
		this._nameText.text = file.Name;
		this._dateText.text = file.File.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000057EC File Offset: 0x000039EC
	public void SetHighlighted(SaveGameFile file)
	{
		this._background.color = ((this._file == file) ? new Color(0.5f, 0.5f, 0.5f, 0.8f) : new Color(0f, 0f, 0f, 0.8f));
	}

	// Token: 0x04000079 RID: 121
	[SerializeField]
	private TMP_Text _nameText;

	// Token: 0x0400007A RID: 122
	[SerializeField]
	private TMP_Text _dateText;

	// Token: 0x0400007B RID: 123
	[SerializeField]
	private Image _background;

	// Token: 0x0400007C RID: 124
	private SaveGameUI _parent;

	// Token: 0x0400007D RID: 125
	private SaveGameFile _file;
}
