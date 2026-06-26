using System;
using Behaviour.Crew;
using Behaviour.Unit;
using Source.Crew;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A5 RID: 677
	public class CaptainGeneral : SideTabContent
	{
		// Token: 0x06001934 RID: 6452 RVA: 0x0009CA7E File Offset: 0x0009AC7E
		private void Start()
		{
			this.commander = GamePlayer.current.commander;
			this.SetCaptainCardInfo();
			this.SetShipCardInfo();
			this.SetName();
			this.SetHomeStation();
			this.ShowBountyAndPatrolRank();
			this.ShowStats();
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0009CAB4 File Offset: 0x0009ACB4
		private void SetHomeStation()
		{
			string text = Translation.Translate("@SPHomeStation", Array.Empty<object>()) + ": ";
			this.homeStation.text = Translation.Translate("@SPHomeStation", Array.Empty<object>()) + ": ";
			if (GamePlayer.current.homeStation != null)
			{
				text += GamePlayer.current.homeStation.name.HighlightWithColor(ColorHelper.greenish);
			}
			else
			{
				text += Translation.Translate("@SPHomeStationNotSet", Array.Empty<object>()).HighlightWithColor(ColorHelper.reddish);
			}
			this.homeStation.text = text;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0009CB5C File Offset: 0x0009AD5C
		private void ShowBountyAndPatrolRank()
		{
			string text = Translation.Translate("@PatrolLvl", new object[]
			{
				GamePlayer.current.patrolRank + 1
			}) ?? "";
			text += "\n";
			text += Translation.Translate("@BountyLvl", new object[]
			{
				GamePlayer.current.bountyRank + 1
			});
			text += "\n";
			text += Translation.Translate("@IndustryLevel", new object[]
			{
				GamePlayer.current.industryRank + 1
			});
			this.ranks.text = text;
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0009CC11 File Offset: 0x0009AE11
		private void SetName()
		{
			this.captainName.text = GamePlayer.current.commander.GetFullName();
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0009CC30 File Offset: 0x0009AE30
		private void SetCaptainCardInfo()
		{
			this.captainSprite.sprite = GamePlayer.current.commander.sprite;
			this.captainLevel.text = string.Format("{0} {1}", Translation.Translate("@SPLevel", Array.Empty<object>()), this.commander.level);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0009CC8B File Offset: 0x0009AE8B
		private void SetShipCardInfo()
		{
			SpaceShip shipClass = GamePlayer.current.currentSpaceShip.shipClass;
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0009CC9D File Offset: 0x0009AE9D
		public void OpenCustomize()
		{
			UnityEngine.Object.Instantiate<CaptainCustomizePopup>(this.customizePopupPrefab, UITooltip.tooltipParent).Open(this.commander, delegate(string firstName, string callsign, string lastName, CrewIcon icon, string title, Color color)
			{
				this.commander.SetName(firstName, callsign, lastName);
				this.commander.SetIcon(icon);
				this.commander.SetTitle(title, color);
				this.SetName();
				this.SetCaptainCardInfo();
				SidePanel.instance.RefreshIfOpen();
			});
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0009CCC6 File Offset: 0x0009AEC6
		public void ClearHomeStation()
		{
			GamePlayer.current.homeStation = null;
			SidePanel.instance.RefreshIfOpen();
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0009CCE0 File Offset: 0x0009AEE0
		public void ShowStats()
		{
			int num = Register.GetCounter("SurfaceOreMined", 0) + Register.GetCounter("CoreOreMined", 0);
			this.stats.text = "Mining";
			TextMeshProUGUI textMeshProUGUI = this.stats;
			textMeshProUGUI.text += "\n";
			TextMeshProUGUI textMeshProUGUI2 = this.stats;
			textMeshProUGUI2.text = textMeshProUGUI2.text + "Total Ores Mined: " + num.ToString();
			TextMeshProUGUI textMeshProUGUI3 = this.stats;
			textMeshProUGUI3.text += "\n";
			TextMeshProUGUI textMeshProUGUI4 = this.stats;
			textMeshProUGUI4.text = textMeshProUGUI4.text + "Surface Ores Mined: " + Register.GetCounter("SurfaceOreMined", 0).ToString();
			TextMeshProUGUI textMeshProUGUI5 = this.stats;
			textMeshProUGUI5.text += "\n";
			TextMeshProUGUI textMeshProUGUI6 = this.stats;
			textMeshProUGUI6.text = textMeshProUGUI6.text + "Core Ores Mined: " + Register.GetCounter("CoreOreMined", 0).ToString();
			TextMeshProUGUI textMeshProUGUI7 = this.stats;
			textMeshProUGUI7.text += "\n";
			int counter = Register.GetCounter("SurfaceOreYieldMax", 0);
			float percentage = (counter > 0) ? ((float)Register.GetCounter("SurfaceOreMined", 0) / (float)counter) : 0f;
			TextMeshProUGUI textMeshProUGUI8 = this.stats;
			textMeshProUGUI8.text = textMeshProUGUI8.text + "Surface Ore Yield: " + GameMath.FormatPercentage(percentage, FormatPercentageMode.Default, 2);
			TextMeshProUGUI textMeshProUGUI9 = this.stats;
			textMeshProUGUI9.text += "\n";
			int counter2 = Register.GetCounter("CoreOreYieldMax", 0);
			float percentage2 = (counter2 > 0) ? ((float)Register.GetCounter("CoreOreMined", 0) / (float)counter2) : 0f;
			TextMeshProUGUI textMeshProUGUI10 = this.stats;
			textMeshProUGUI10.text = textMeshProUGUI10.text + "Core Ore Yield: " + GameMath.FormatPercentage(percentage2, FormatPercentageMode.Default, 2);
			TextMeshProUGUI textMeshProUGUI11 = this.stats;
			textMeshProUGUI11.text += "\n";
			TextMeshProUGUI textMeshProUGUI12 = this.stats;
			textMeshProUGUI12.text = textMeshProUGUI12.text + "Ores Stolen: " + Register.GetCounter("OreStolen", 0).ToString();
			TextMeshProUGUI textMeshProUGUI13 = this.stats;
			textMeshProUGUI13.text += "\n";
			TextMeshProUGUI textMeshProUGUI14 = this.stats;
			textMeshProUGUI14.text += "Salvaging";
			TextMeshProUGUI textMeshProUGUI15 = this.stats;
			textMeshProUGUI15.text += "\n";
			TextMeshProUGUI textMeshProUGUI16 = this.stats;
			textMeshProUGUI16.text = textMeshProUGUI16.text + "Scrap salvaged: " + Register.GetCounter("SalvagingScrap", 0).ToString();
			TextMeshProUGUI textMeshProUGUI17 = this.stats;
			textMeshProUGUI17.text += "\n";
			int counter3 = Register.GetCounter("SalvagingScrapYieldMax", 0);
			float percentage3 = (counter3 > 0) ? ((float)Register.GetCounter("SalvagingScrap", 0) / (float)counter3) : 0f;
			TextMeshProUGUI textMeshProUGUI18 = this.stats;
			textMeshProUGUI18.text = textMeshProUGUI18.text + "Scrap yield: " + GameMath.FormatPercentage(percentage3, FormatPercentageMode.Default, 2);
			TextMeshProUGUI textMeshProUGUI19 = this.stats;
			textMeshProUGUI19.text += "\n";
			TextMeshProUGUI textMeshProUGUI20 = this.stats;
			textMeshProUGUI20.text = textMeshProUGUI20.text + "Items salvaged: " + Register.GetCounter("SalvagingItemsRetrieved", 0).ToString();
			TextMeshProUGUI textMeshProUGUI21 = this.stats;
			textMeshProUGUI21.text += "\n";
			int counter4 = Register.GetCounter("SalvagingItemMaxYield", 0);
			float percentage4 = (counter4 > 0) ? ((float)Register.GetCounter("SalvagingItemsRetrieved", 0) / (float)counter4) : 0f;
			TextMeshProUGUI textMeshProUGUI22 = this.stats;
			textMeshProUGUI22.text = textMeshProUGUI22.text + "Item yield: " + GameMath.FormatPercentage(percentage4, FormatPercentageMode.Default, 2);
		}

		// Token: 0x04000FA4 RID: 4004
		[SerializeField]
		private Image captainSprite;

		// Token: 0x04000FA5 RID: 4005
		[SerializeField]
		private TextMeshProUGUI captainLevel;

		// Token: 0x04000FA6 RID: 4006
		[SerializeField]
		private CaptainCustomizePopup customizePopupPrefab;

		// Token: 0x04000FA7 RID: 4007
		[SerializeField]
		private TextMeshProUGUI shipName;

		// Token: 0x04000FA8 RID: 4008
		[SerializeField]
		private Image shipSprite;

		// Token: 0x04000FA9 RID: 4009
		private CommanderData commander;

		// Token: 0x04000FAA RID: 4010
		[SerializeField]
		private TextMeshProUGUI captainName;

		// Token: 0x04000FAB RID: 4011
		[SerializeField]
		private TextMeshProUGUI homeStation;

		// Token: 0x04000FAC RID: 4012
		[SerializeField]
		private TextMeshProUGUI ranks;

		// Token: 0x04000FAD RID: 4013
		[SerializeField]
		private TextMeshProUGUI stats;
	}
}
