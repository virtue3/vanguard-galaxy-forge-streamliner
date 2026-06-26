using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.UI.Main;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station;
using Source.Player;
using Source.Simulation.Story;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x0200022C RID: 556
	public class Shipyard : MonoBehaviour
	{
		// Token: 0x060014E1 RID: 5345 RVA: 0x00086C52 File Offset: 0x00084E52
		private void Start()
		{
			this.spacestation = (GamePlayer.current.currentPointOfInterest as SpaceStation);
			this.shipSelect.HideChangeNameButton();
			this.SetShipsForSpacestation();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00086C7C File Offset: 0x00084E7C
		private void Update()
		{
			if (this.selectedShip == this.shipSelect.selectedShip)
			{
				return;
			}
			this.selectedShip = this.shipSelect.selectedShip;
			float totalCost = this.shipSelect.selectedShip.totalCost;
			if (this.shipSelect.selectedShip.shopItemData.conquestCommendations)
			{
				this.commendationPrice.gameObject.SetActive(true);
				TextMeshProUGUI componentInChildren = this.commendationPrice.GetComponentInChildren<TextMeshProUGUI>();
				componentInChildren.text = (GameMath.FormatNumber((float)this.GetShipCommendationCost(), -1) ?? "");
				componentInChildren.color = (GamePlayer.current.CanAfford((float)this.GetShipCommendationCost()) ? ColorHelper.offWhite : ColorHelper.noCreditsColor);
				this.creditPrice.alignment = TextAlignmentOptions.Left;
			}
			else
			{
				this.commendationPrice.gameObject.SetActive(false);
				this.creditPrice.alignment = TextAlignmentOptions.Center;
			}
			this.creditPrice.gameObject.SetActive(true);
			this.creditPrice.text = "$ " + GameMath.FormatNumber(totalCost, -1);
			this.creditPrice.color = (GamePlayer.current.CanAfford(totalCost) ? ColorHelper.creditsColor : ColorHelper.noCreditsColor);
			this.reqHolder.SetActive(false);
			this.buyButton.gameObject.SetActive(true);
			this.SetRequirements();
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00086DDE File Offset: 0x00084FDE
		private int GetShipCommendationCost()
		{
			return (int)this.shipSelect.selectedShip.shipCommendationCost;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00086DF1 File Offset: 0x00084FF1
		private void CheckShipCount()
		{
			if (this.shipSelect.ships.Count == 0)
			{
				this.reqHolder.SetActive(true);
				this.repRequirement.text = Translation.Translate("@SSNoShipsToBuy", Array.Empty<object>());
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00086E2C File Offset: 0x0008502C
		private void SetRequirements()
		{
			foreach (object obj in this.reqHolder.transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			if (!this.FactionRequirementsMet())
			{
				this.reqHolder.SetActive(true);
				string text = this.shipSelect.selectedShip.shopItemData.factionPrereq[0].faction;
				if (string.IsNullOrEmpty(text))
				{
					text = this.spacestation.faction.identifier;
				}
				UnityEngine.Object.Instantiate<GameObject>(this.reqPrefab, this.reqHolder.transform).GetComponentInChildren<TextMeshProUGUI>().text = Translation.Translate("@SSShipyardRepRequirement", new object[]
				{
					this.GetTranslatedRepLevel(),
					Faction.Get(text).name
				});
			}
			if (!this.ConquestRankMet())
			{
				this.reqHolder.SetActive(true);
				FactionPrerequisites factionPrerequisites = this.shipSelect.selectedShip.shopItemData.factionPrereq[0];
				string text2 = factionPrerequisites.faction;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = this.spacestation.faction.identifier;
				}
				Faction faction = Faction.Get(text2);
				UnityEngine.Object.Instantiate<GameObject>(this.reqPrefab, this.reqHolder.transform).GetComponentInChildren<TextMeshProUGUI>().text = Translation.Translate("@SSShipyardConquestRankRequirement", new object[]
				{
					factionPrerequisites.conquestRank.GetConquestRankTranslation(faction.identifier),
					faction.name
				});
			}
			if (!this.LevelRequirementsMet())
			{
				this.reqHolder.SetActive(true);
				UnityEngine.Object.Instantiate<GameObject>(this.reqPrefab, this.reqHolder.transform).GetComponentInChildren<TextMeshProUGUI>().text = Translation.Translate("@SSLevelRequirementNotMet", new object[]
				{
					this.shipSelect.selectedShip.shopItemData.levelRequirement
				});
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00087034 File Offset: 0x00085234
		private void SetShipsForSpacestation()
		{
			if (this.spacestation.shipyard.spaceShips.Count == 0)
			{
				this.ShowShips();
			}
			else
			{
				this.shipSelect.SetNewShips(this.spacestation.shipyard.spaceShips, this.spacestation.name);
			}
			this.CheckShipCount();
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0008708C File Offset: 0x0008528C
		public void ShowShips()
		{
			List<ShipyardShip> list = new List<ShipyardShip>();
			this.AddGeneralShips(list);
			this.spacestation.shipyard.spaceShips = list;
			this.shipSelect.SetNewShips(list, this.spacestation.name);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x000870D0 File Offset: 0x000852D0
		private void AddGeneralShips(List<ShipyardShip> ships)
		{
			Faction faction = this.spacestation.faction;
			if (faction == null)
			{
				return;
			}
			foreach (KeyValuePair<string, SpaceShip> keyValuePair in from ship in SpaceShip.GetAll()
			orderby ship.Value.shipCost
			select ship)
			{
				if (keyValuePair.Value.hasCommander)
				{
					ShopItemData shopItemData = keyValuePair.Value.shopItemData;
					int num = (this.spacestation is EmbassyStation) ? 60 : this.spacestation.level;
					foreach (FactionPrerequisites factionPrerequisites in shopItemData.factionPrereq)
					{
						if ((string.IsNullOrWhiteSpace(factionPrerequisites.faction) || faction.identifier == factionPrerequisites.faction) && shopItemData.minAreaLevelRequirement <= num && (shopItemData.maxAreaLevelRequirement == 0 || shopItemData.maxAreaLevelRequirement >= this.spacestation.level))
						{
							ships.Add(new ShipyardShip(keyValuePair.Value.name, 1));
						}
					}
				}
			}
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00087228 File Offset: 0x00085428
		public void TryBuySelectedShip()
		{
			if (this.shipSelect.selectedShip == null)
			{
				return;
			}
			GamePlayer current = GamePlayer.current;
			SpaceShip ship = this.shipSelect.selectedShip;
			float shipCost = ship.totalCost;
			ShopItemData shopItemData = this.shipSelect.selectedShip.shopItemData;
			if (!this.FactionRequirementsMet())
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSFactionRequirementNotMet", new object[]
				{
					this.GetTranslatedRepLevel(),
					this.spacestation.faction.name
				})).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!this.ConquestRankMet())
			{
				ConquestRank conquestRank = shopItemData.factionPrereq[0].conquestRank;
				Faction faction = Faction.Get(shopItemData.factionPrereq[0].faction);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSConquestRankRequirementNotMet", new object[]
				{
					conquestRank.GetConquestRankTranslation(faction.identifier),
					this.spacestation.faction.name
				})).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!this.LevelRequirementsMet())
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSLevelRequirementNotMet", new object[]
				{
					shopItemData.levelRequirement
				})).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!current.CanAfford(shipCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (shopItemData.conquestCommendations)
			{
				InventoryItemType type = InventoryItemType.Get("ConquestCurrency");
				if (GamePlayer.current.CountAvailableItems(type) < this.GetShipCommendationCost())
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCommendations", new object[]
					{
						this.GetShipCommendationCost()
					})).WithColor(ColorHelper.red90).Show();
					return;
				}
			}
			string msg = Translation.Translate("@SSShipyardBuyShipQuery", new object[]
			{
				("$ " + GameMath.FormatNumber(shipCost, -1)).HighlightWithColor(ColorHelper.creditsColor)
			});
			if (this.shipSelect.selectedShip.shopItemData.conquestCommendations)
			{
				msg = Translation.TranslateOnly("@SSShipyardBuyShipQueryCommendation", new object[]
				{
					("$ " + GameMath.FormatNumber(shipCost, -1)).HighlightWithColor(ColorHelper.creditsColor),
					GameMath.FormatNumber((float)this.GetShipCommendationCost(), -1) ?? ""
				});
			}
			AlertPopup.ShowQuery(msg, "Purchase", "Cancel", delegate
			{
				this.BuyShip(ship, shipCost);
			}, null, null, null);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x000874EC File Offset: 0x000856EC
		private void BuyShip(SpaceShip ship, float shipCost)
		{
			GamePlayer.current.RemoveCredits(shipCost);
			if (this.shipSelect.selectedShip.shopItemData.conquestCommendations)
			{
				InventoryItemType type = InventoryItemType.Get("ConquestCurrency");
				GamePlayer.current.ConsumeAvailableItems(type, this.GetShipCommendationCost());
			}
			this.AddSpaceshipToFleet(ship.spaceShipData);
			this.spacestation.shipyard.RemoveShip(ship.name);
			this.shipSelect.RemoveShip(ship.spaceShipData);
			if (ship.shipRoleType.GetRole() == SpaceShipRole.Combat && ship.shipRoleType.GetShipType() == SpaceShipType.Size4)
			{
				SteamAchievement.Trigger("BuyFrigate");
			}
			this.CheckShipCount();
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00087597 File Offset: 0x00085797
		private bool CanAfford()
		{
			return true;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0008759C File Offset: 0x0008579C
		private bool FactionRequirementsMet()
		{
			List<FactionPrerequisites> factionPrereq = this.shipSelect.selectedShip.shopItemData.factionPrereq;
			if (factionPrereq.Count == 0)
			{
				return true;
			}
			Faction faction = this.spacestation.faction;
			foreach (FactionPrerequisites factionPrerequisites in factionPrereq)
			{
				ReputationLevel reputationLevel = factionPrerequisites.reputationLevel;
				Faction faction2 = string.IsNullOrEmpty(factionPrerequisites.faction) ? faction : Faction.Get(factionPrerequisites.faction);
				if (faction2 == null)
				{
					Debug.LogWarning("Ship in shipyard probably doesn't have the correct shopdata faction: " + factionPrerequisites.faction);
				}
				else if (faction == faction2)
				{
					return faction2.GetReputation(Faction.player) >= reputationLevel.GetReputationThreshold();
				}
			}
			return true;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00087674 File Offset: 0x00085874
		private bool ConquestRankMet()
		{
			List<FactionPrerequisites> factionPrereq = this.shipSelect.selectedShip.shopItemData.factionPrereq;
			if (factionPrereq.Count == 0)
			{
				return true;
			}
			Faction faction = this.spacestation.faction;
			foreach (FactionPrerequisites factionPrerequisites in factionPrereq)
			{
				Faction faction2 = string.IsNullOrEmpty(factionPrerequisites.faction) ? faction : Faction.Get(factionPrerequisites.faction);
				if (faction2 == null)
				{
					Debug.LogWarning("Ship in shipyard probably doesn't have the correct shopdata faction: " + factionPrerequisites.faction);
				}
				else
				{
					ConquestRank conquestRank = factionPrerequisites.conquestRank;
					if (conquestRank == ConquestRank.None)
					{
						return true;
					}
					if (faction != faction2)
					{
						return true;
					}
					Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
					if (storyteller == null)
					{
						return false;
					}
					return ConquestRankExtension.GetConquestRankLevel(storyteller.GetFactionStanding(faction2).playerContribution) >= conquestRank;
				}
			}
			return true;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00087778 File Offset: 0x00085978
		private bool LevelRequirementsMet()
		{
			int levelRequirement = this.shipSelect.selectedShip.shopItemData.levelRequirement;
			return GamePlayer.current.level >= levelRequirement;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x000877AB File Offset: 0x000859AB
		private string GetTranslatedRepLevel()
		{
			return Translation.Translate(string.Format("@{0}", this.shipSelect.selectedShip.shopItemData.factionPrereq[0].reputationLevel), Array.Empty<object>());
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x000877E8 File Offset: 0x000859E8
		public void AddSpaceshipToFleet(SpaceShipData spaceShipData)
		{
			spaceShipData.LoadDefaultEquipment(MapPointOfInterest.current.level, -1f, null, null, null, null, false, null);
			if (!GamePlayer.current.spaceShips.Contains(spaceShipData))
			{
				GamePlayer.current.spaceShips.Add(spaceShipData);
			}
		}

		// Token: 0x04000C37 RID: 3127
		[SerializeField]
		private Behaviour.UI.Main.ShipCarousel shipSelect;

		// Token: 0x04000C38 RID: 3128
		[SerializeField]
		private TextMeshProUGUI creditPrice;

		// Token: 0x04000C39 RID: 3129
		[SerializeField]
		private GameObject commendationPrice;

		// Token: 0x04000C3A RID: 3130
		[SerializeField]
		private TextMeshProUGUI repRequirement;

		// Token: 0x04000C3B RID: 3131
		[SerializeField]
		private GameObject reqHolder;

		// Token: 0x04000C3C RID: 3132
		[SerializeField]
		private GameObject reqPrefab;

		// Token: 0x04000C3D RID: 3133
		[SerializeField]
		private Button buyButton;

		// Token: 0x04000C3E RID: 3134
		public SpaceStation spacestation;

		// Token: 0x04000C3F RID: 3135
		private SpaceShip selectedShip;

		// Token: 0x04000C40 RID: 3136
		private float reqTimer;
	}
}
