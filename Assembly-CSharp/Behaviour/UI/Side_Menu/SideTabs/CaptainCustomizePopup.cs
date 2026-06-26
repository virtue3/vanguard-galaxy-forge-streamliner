using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.Side_Menu.SideTabs.Captain;
using Behaviour.Util;
using Source.Crew;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A3 RID: 675
	public class CaptainCustomizePopup : MonoBehaviour
	{
		// Token: 0x06001920 RID: 6432 RVA: 0x0009C004 File Offset: 0x0009A204
		private void Start()
		{
			GameManager.Pause();
			this.firstNameInput.Select();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0009C016 File Offset: 0x0009A216
		private void OnDestroy()
		{
			GameManager.Unpause();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0009C020 File Offset: 0x0009A220
		public void Open(CommanderData data, Action<string, string, string, CrewIcon, string, Color> onConfirm)
		{
			this.onConfirm = onConfirm;
			this.icons = CrewIcons.GetAll();
			this.selectedIndex = this.icons.FindIndex((CrewIcon i) => i == data.icon);
			if (this.selectedIndex < 0)
			{
				this.selectedIndex = 0;
			}
			this.firstNameInput.text = data.firstName;
			this.callsignInput.text = data.callsign;
			this.lastNameInput.text = data.lastName;
			this.selectedTitle = data.selectedTitle;
			this.selectedTitleColor = this.GetColorForTitle(this.selectedTitle);
			this.UpdateCurrentTitleLabel();
			this.UpdateIcon();
			this.PopulateFactions();
			this.ShowGeneralTitles();
			this.confirmButton.onClick.RemoveAllListeners();
			this.confirmButton.onClick.AddListener(new UnityAction(this.Confirm));
			this.cancelButton.onClick.RemoveAllListeners();
			this.cancelButton.onClick.AddListener(new UnityAction(this.Close));
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0009C14E File Offset: 0x0009A34E
		public void NextIcon()
		{
			this.selectedIndex = (this.selectedIndex + 1) % this.icons.Count;
			this.UpdateIcon();
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0009C170 File Offset: 0x0009A370
		public void PreviousIcon()
		{
			this.selectedIndex = (this.selectedIndex - 1 + this.icons.Count) % this.icons.Count;
			this.UpdateIcon();
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0009C19E File Offset: 0x0009A39E
		private void UpdateIcon()
		{
			if (this.icons != null && this.icons.Count > 0)
			{
				this.iconPreview.sprite = this.icons[this.selectedIndex].sprite;
			}
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0009C1D8 File Offset: 0x0009A3D8
		private void PopulateFactions()
		{
			this.titleFactionContainer.DestroyChildren();
			this.titleListContainer.DestroyChildren();
			bool flag = GamePlayer.current.unlockedTitles.Count > 0 || SteamCommunication.supporterEdition;
			if (flag && this.generalTitlesButton)
			{
				UnityEngine.Object.Instantiate<Button>(this.generalTitlesButton, this.titleFactionContainer).onClick.AddListener(new UnityAction(this.ShowGeneralTitles));
			}
			bool flag2 = false;
			if (GamePlayer.current.GetStoryteller<Conquest>() != null)
			{
				foreach (Faction faction in Faction.all)
				{
					if (faction != Faction.player && faction != Faction.amalgam && faction != Faction.fanatics && faction != Faction.holyRadicals && (Conquest.conquestFactions.Contains(faction) || faction == Faction.puppeteers) && faction.GetConquestRank() != ConquestRank.None)
					{
						flag2 = true;
						Button button = UnityEngine.Object.Instantiate<Button>(this.factionButtonPrefab, this.titleFactionContainer);
						Image componentInChildren = button.GetComponentInChildren<Image>();
						FactionIconSet factionIconSet = FactionIconSet.Get(faction);
						if (componentInChildren && factionIconSet != null)
						{
							componentInChildren.sprite = ((factionIconSet.tinySize != null) ? factionIconSet.tinySize : factionIconSet.fullSize);
						}
						FactionTitleButton component = button.GetComponent<FactionTitleButton>();
						if (component)
						{
							component.Setup(faction);
						}
						Faction f = faction;
						button.onClick.AddListener(delegate()
						{
							this.ShowTitlesForFaction(f);
						});
					}
				}
			}
			this.titleSection.SetActive(flag2 || flag);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0009C3A8 File Offset: 0x0009A5A8
		private void ShowGeneralTitles()
		{
			this.titleListContainer.DestroyChildren();
			this.selectedTitleButton = null;
			if (this.titleScrollRect)
			{
				this.titleScrollRect.verticalNormalizedPosition = 1f;
			}
			List<TitleDefinition> list = new List<TitleDefinition>();
			foreach (string identifier in GamePlayer.current.unlockedTitles)
			{
				TitleDefinition titleDefinition = Titles.Get(identifier);
				if (titleDefinition != null)
				{
					list.Add(titleDefinition);
				}
			}
			if (SteamCommunication.supporterEdition)
			{
				list.Add(Titles.SupporterCombat);
				list.Add(Titles.SupporterMining);
				list.Add(Titles.SupporterSalvaging);
				list.Add(Titles.SupporterTrading);
			}
			foreach (TitleDefinition titleDefinition2 in list)
			{
				string displayName = titleDefinition2.GetDisplayName();
				Button button = UnityEngine.Object.Instantiate<Button>(this.titleRowPrefab, this.titleListContainer);
				TextMeshProUGUI componentInChildren = button.GetComponentInChildren<TextMeshProUGUI>();
				if (componentInChildren != null)
				{
					componentInChildren.text = displayName;
					componentInChildren.color = titleDefinition2.color;
				}
				string t = displayName;
				Button b = button;
				Color c = titleDefinition2.color;
				button.onClick.AddListener(delegate()
				{
					this.SelectTitle(t, b, c);
				});
				if (displayName == this.selectedTitle)
				{
					this.HighlightButton(button);
				}
			}
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0009C554 File Offset: 0x0009A754
		private void ShowTitlesForFaction(Faction faction)
		{
			this.titleListContainer.DestroyChildren();
			this.selectedTitleButton = null;
			if (this.titleScrollRect)
			{
				this.titleScrollRect.verticalNormalizedPosition = 1f;
			}
			ConquestRank conquestRank = faction.GetConquestRank();
			foreach (object obj in Enum.GetValues(typeof(ConquestRank)))
			{
				ConquestRank conquestRank2 = (ConquestRank)obj;
				if (conquestRank2 != ConquestRank.None)
				{
					if (conquestRank2 > conquestRank)
					{
						break;
					}
					string conquestRankTranslation = conquestRank2.GetConquestRankTranslation(faction.identifier);
					Button button = UnityEngine.Object.Instantiate<Button>(this.titleRowPrefab, this.titleListContainer);
					TextMeshProUGUI componentInChildren = button.GetComponentInChildren<TextMeshProUGUI>();
					if (componentInChildren != null)
					{
						componentInChildren.text = conquestRankTranslation;
						componentInChildren.color = conquestRank2.GetConquestColor();
					}
					string t = conquestRankTranslation;
					Button b = button;
					Color c = conquestRank2.GetConquestColor();
					button.onClick.AddListener(delegate()
					{
						this.SelectTitle(t, b, c);
					});
					if (conquestRankTranslation == this.selectedTitle)
					{
						this.HighlightButton(button);
					}
				}
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0009C6A0 File Offset: 0x0009A8A0
		private void HighlightButton(Button btn)
		{
			if (this.selectedTitleButton != null)
			{
				this.selectedTitleButton.GetComponent<Image>().color = Color.white;
			}
			this.selectedTitleButton = btn;
			btn.GetComponent<Image>().color = ColorHelper.buffBorder;
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0009C6DC File Offset: 0x0009A8DC
		private void SelectTitle(string title, Button btn, Color color)
		{
			this.selectedTitle = title;
			this.selectedTitleColor = color;
			this.HighlightButton(btn);
			this.UpdateCurrentTitleLabel();
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0009C6FC File Offset: 0x0009A8FC
		private void UpdateCurrentTitleLabel()
		{
			if (this.currentTitleLabel == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.selectedTitle))
			{
				Translation.Translate("@UINoTitle", Array.Empty<object>());
				this.currentTitleLabel.text = Translation.Translate("@CurrentTitle", new object[]
				{
					Translation.Translate("@UINoTitle", Array.Empty<object>()).HighlightWithColor(ColorHelper.fadedGrey)
				});
				return;
			}
			this.currentTitleLabel.text = Translation.Translate("@CurrentTitle", new object[]
			{
				this.selectedTitle.HighlightWithColor(this.selectedTitleColor)
			});
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0009C79C File Offset: 0x0009A99C
		private Color GetColorForTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				return Color.white;
			}
			foreach (TitleDefinition titleDefinition in Titles.GetAll())
			{
				if (titleDefinition.GetDisplayName() == title)
				{
					return titleDefinition.color;
				}
			}
			foreach (Faction faction in Faction.all)
			{
				if (faction != Faction.player && faction != Faction.amalgam && faction != Faction.fanatics && faction != Faction.holyRadicals && (Conquest.conquestFactions.Contains(faction) || faction == Faction.puppeteers))
				{
					foreach (object obj in Enum.GetValues(typeof(ConquestRank)))
					{
						ConquestRank conquestRank = (ConquestRank)obj;
						if (conquestRank != ConquestRank.None && conquestRank.GetConquestRankTranslation(faction.identifier) == title)
						{
							return conquestRank.GetConquestColor();
						}
					}
				}
			}
			return Color.white;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0009C908 File Offset: 0x0009AB08
		private void Confirm()
		{
			Action<string, string, string, CrewIcon, string, Color> action = this.onConfirm;
			if (action != null)
			{
				action(this.firstNameInput.text, this.callsignInput.text, this.lastNameInput.text, this.icons[this.selectedIndex], this.selectedTitle, this.selectedTitleColor);
			}
			this.Close();
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0009C96A File Offset: 0x0009AB6A
		private void Close()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000F8F RID: 3983
		[Header("Identity")]
		[SerializeField]
		private TMP_InputField firstNameInput;

		// Token: 0x04000F90 RID: 3984
		[SerializeField]
		private TMP_InputField callsignInput;

		// Token: 0x04000F91 RID: 3985
		[SerializeField]
		private TMP_InputField lastNameInput;

		// Token: 0x04000F92 RID: 3986
		[SerializeField]
		private Image iconPreview;

		// Token: 0x04000F93 RID: 3987
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04000F94 RID: 3988
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04000F95 RID: 3989
		[Header("Title")]
		[SerializeField]
		private GameObject titleSection;

		// Token: 0x04000F96 RID: 3990
		[SerializeField]
		private Transform titleFactionContainer;

		// Token: 0x04000F97 RID: 3991
		[SerializeField]
		private Transform titleListContainer;

		// Token: 0x04000F98 RID: 3992
		[SerializeField]
		private ScrollRect titleScrollRect;

		// Token: 0x04000F99 RID: 3993
		[SerializeField]
		private TextMeshProUGUI currentTitleLabel;

		// Token: 0x04000F9A RID: 3994
		[SerializeField]
		private Button generalTitlesButton;

		// Token: 0x04000F9B RID: 3995
		[SerializeField]
		private Button factionButtonPrefab;

		// Token: 0x04000F9C RID: 3996
		[SerializeField]
		private Button titleRowPrefab;

		// Token: 0x04000F9D RID: 3997
		private List<CrewIcon> icons;

		// Token: 0x04000F9E RID: 3998
		private int selectedIndex;

		// Token: 0x04000F9F RID: 3999
		private string selectedTitle;

		// Token: 0x04000FA0 RID: 4000
		private Color selectedTitleColor = Color.white;

		// Token: 0x04000FA1 RID: 4001
		private Button selectedTitleButton;

		// Token: 0x04000FA2 RID: 4002
		private Action<string, string, string, CrewIcon, string, Color> onConfirm;
	}
}
