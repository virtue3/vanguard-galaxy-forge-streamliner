using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.UI.Side_Menu.Loading;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Spacestation;
using Source.Player;
using Source.Simulation.Story;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x0200029D RID: 669
	public class SideTab : MonoBehaviour
	{
		// Token: 0x060018F6 RID: 6390 RVA: 0x0009B4B1 File Offset: 0x000996B1
		private void Start()
		{
			this.bgImage = base.GetComponent<Image>();
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0009B4BF File Offset: 0x000996BF
		public void ClearSideTabNav()
		{
			this.sideTabContents.Clear();
			this.sideTabNavigation.ClearButtons();
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0009B4D7 File Offset: 0x000996D7
		public void ToggleBackgroundImage(bool toggle)
		{
			this.bgImage.enabled = toggle;
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0009B4E8 File Offset: 0x000996E8
		public void ClearSideContent()
		{
			foreach (object obj in this.sideContentHolder.transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			this.currentSideTabContent = null;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0009B550 File Offset: 0x00099750
		public IEnumerator LoadContentTab(SideTabContent sideTabContent)
		{
			this.bgImage.enabled = (sideTabContent.tabName != "Equipment");
			if (this.currentSideTabContent == sideTabContent)
			{
				yield break;
			}
			this.ClearPreviousContent();
			this.SetCurrentContent(sideTabContent);
			this.DeselectLastTabMenuButton();
			this.FinalizeTabLoading(sideTabContent);
			yield break;
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0009B568 File Offset: 0x00099768
		private void SetCurrentContent(SideTabContent sideTabContent)
		{
			this.currentSideTabContent = sideTabContent;
			int sideTabIndex = this.sideTabContents.FindIndex((SideTabContent tab) => tab.tabName == this.currentSideTabContent.tabName);
			SidePanel.instance.SetSideTabIndex(sideTabIndex);
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0009B59F File Offset: 0x0009979F
		private void ClearPreviousContent()
		{
			SidePanel.instance.sideTab.ClearSideContent();
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0009B5B0 File Offset: 0x000997B0
		private void DeselectLastTabMenuButton()
		{
			if (this.lastTabMenuButton && this.currentSideTabContent != this.lastTabMenuButton.sideTabContent)
			{
				this.lastTabMenuButton.DeselectButton();
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0009B5E4 File Offset: 0x000997E4
		public void NextTab()
		{
			int count = this.sideTabContents.Count;
			if (count == 0)
			{
				return;
			}
			int num = this.sideTabContents.FindIndex((SideTabContent tab) => tab.tabName == this.currentSideTabContent.tabName);
			if (num == -1)
			{
				return;
			}
			int num2 = (num + 1) % count;
			if (!this.sideTabContents[num2].tabMenuButton.isDeactivated)
			{
				base.StartCoroutine(this.LoadContentTab(this.sideTabContents[num2]));
				this.sideTabContents[num2].tabMenuButton.SetSelectedButton();
			}
			SidePanel.instance.SetSideTabIndex(num2);
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0009B676 File Offset: 0x00099876
		private void StartLoadingBar()
		{
			this.CancelLoading();
			this.activeLoadingBar = UnityEngine.Object.Instantiate<LoadingBar>(this.loadingBar, base.transform);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0009B695 File Offset: 0x00099895
		private IEnumerator WaitForLoadingBarToFinish()
		{
			yield return this.activeLoadingBar.StartFilling();
			UnityEngine.Object.Destroy(this.activeLoadingBar.gameObject);
			yield break;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0009B6A4 File Offset: 0x000998A4
		private void FinalizeTabLoading(SideTabContent sideTabContent)
		{
			SidePanel instance = SidePanel.instance;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sideTabContent.gameObject, instance.sideTab.sideContentHolder.transform);
			if (instance.openCrew && sideTabContent is ShipEquipment)
			{
				base.StartCoroutine(gameObject.GetComponent<ShipEquipment>().ShowCrewTab());
				instance.openCrew = false;
			}
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0009B6FC File Offset: 0x000998FC
		public void SetNavButtons(SideTabContent selectedTab)
		{
			foreach (SideTabContent sideTabContent in this.sideTabContents)
			{
				bool deactivated = SideTab.TabDeactivated(sideTabContent);
				this.sideTabNavigation.CreateButton(sideTabContent, sideTabContent == selectedTab, deactivated);
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0009B764 File Offset: 0x00099964
		private static bool TabDeactivated(SideTabContent content)
		{
			return ((content.tabName == "Stash" || content.tabName == "Materials") && !SpaceStationInterior.instance) || ((content.tabName == "Autopilot" || content.tabName == "AutopilotStatistics") && !GamePlayer.current.autoPlayUnlocked) || (content.tabName == "ConquestRanks" && GamePlayer.current.GetStoryteller<Conquest>() == null);
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0009B7F8 File Offset: 0x000999F8
		public void ShowNotification(string notifyButton)
		{
			foreach (TabMenuButton tabMenuButton in this.sideTabNavigation.GetComponentsInChildren<TabMenuButton>())
			{
				if (notifyButton == tabMenuButton.sideTabContent.tabName)
				{
					tabMenuButton.ShowNotification();
				}
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0009B83C File Offset: 0x00099A3C
		public bool IsLoading()
		{
			return this.activeLoadingBar;
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0009B849 File Offset: 0x00099A49
		public void CancelLoading()
		{
			if (this.activeLoadingBar)
			{
				UnityEngine.Object.Destroy(this.activeLoadingBar.gameObject);
			}
		}

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		private SideTabNav sideTabNavigation;

		// Token: 0x04000F7A RID: 3962
		[SerializeField]
		private Image bgImage;

		// Token: 0x04000F7B RID: 3963
		public List<SideTabContent> sideTabContents;

		// Token: 0x04000F7C RID: 3964
		public GameObject sideContentHolder;

		// Token: 0x04000F7D RID: 3965
		public SideTabContent currentSideTabContent;

		// Token: 0x04000F7E RID: 3966
		public TabMenuButton lastTabMenuButton;

		// Token: 0x04000F7F RID: 3967
		[SerializeField]
		private LoadingBar loadingBar;

		// Token: 0x04000F80 RID: 3968
		private LoadingBar activeLoadingBar;
	}
}
