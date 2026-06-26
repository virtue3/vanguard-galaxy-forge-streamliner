using System;
using System.Collections.Generic;
using Behaviour.UI.Spacestation.Location.Recruitment;
using Behaviour.Unit;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000224 RID: 548
	public class RecruitmentCenterUI : MonoBehaviour
	{
		// Token: 0x06001471 RID: 5233 RVA: 0x00084208 File Offset: 0x00082408
		private void Awake()
		{
			this.title.SetText(Translation.Translate("@UIRecruitmentTitle", new object[]
			{
				MapPointOfInterest.current.faction.name
			}));
			this.subTitle.SetText(Translation.Translate("@UIRecruitmentSubTitle", new object[]
			{
				MapPointOfInterest.current.faction.name
			}));
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0008426F File Offset: 0x0008246F
		public void Start()
		{
			this.ShowMercenaries();
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00084277 File Offset: 0x00082477
		public void SetProfessionFilter(Profession profession)
		{
			this.selectedProfession = profession;
			this.ShowMercenaries();
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00084286 File Offset: 0x00082486
		public void RefreshMercenaries()
		{
			this.ShowMercenaries();
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00084290 File Offset: 0x00082490
		private void ShowMercenaries()
		{
			this.currentMerc = GamePlayer.current.hiredMercenary;
			SpaceStation.current.recruitmentCenter.CreateRecruits(this.selectedProfession, false);
			List<Mercenary> mercenaries = SpaceStation.current.recruitmentCenter.GetMercenaries(this.selectedProfession);
			this.recruitContainer.DestroyChildren();
			this.totalHeight = 0f;
			if (this.currentMerc != null)
			{
				this.AddMercenaryOption(this.currentMerc, true);
			}
			for (int i = mercenaries.Count - 1; i >= 0; i--)
			{
				Mercenary mercenary = mercenaries[i];
				SpaceShip spaceShip = mercenary.GetSpaceShip();
				if ((this.currentMerc == null || !(mercenary.seed == this.currentMerc.seed)) && this.CanUseRarity(mercenary.rarity) && this.CanUseShipType(spaceShip.shipRoleType.GetRole(), spaceShip.shipRoleType.GetShipType()))
				{
					this.AddMercenaryOption(mercenary, false);
				}
			}
			Vector2 sizeDelta = this.recruitContainer.sizeDelta;
			sizeDelta.y = this.totalHeight;
			this.recruitContainer.sizeDelta = sizeDelta;
			this.recruitContainer.anchoredPosition = new Vector2(this.recruitContainer.anchoredPosition.x, sizeDelta.y - this.totalHeight);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000843CF File Offset: 0x000825CF
		private void AddMercenaryOption(Mercenary mercenary, bool current = false)
		{
			UnityEngine.Object.Instantiate<MercenaryOption>(this.mercenaryOptionPrefab, this.recruitContainer).SetMercenary(mercenary, new Action(this.RefreshMercenaries), current);
			this.totalHeight += this.optionHeight;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00084408 File Offset: 0x00082608
		private bool CanUseRarity(Rarity rarity)
		{
			ReputationLevel reputationLevel = MapPointOfInterest.current.faction.GetReputationLevel(Faction.player);
			bool result;
			switch (rarity)
			{
			case Rarity.HighGrade:
				result = (reputationLevel >= ReputationLevel.Cordial);
				break;
			case Rarity.Exotic:
				result = (reputationLevel >= ReputationLevel.Friendly);
				break;
			case Rarity.Legendary:
				result = (reputationLevel >= ReputationLevel.Respected);
				break;
			default:
				result = true;
				break;
			}
			return result;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00084464 File Offset: 0x00082664
		private bool CanUseShipType(SpaceShipRole role, SpaceShipType shipType)
		{
			ReputationLevel reputationLevel = MapPointOfInterest.current.faction.GetReputationLevel(Faction.player);
			bool result;
			switch (shipType)
			{
			case SpaceShipType.Size3:
				result = (reputationLevel >= ReputationLevel.Cordial);
				break;
			case SpaceShipType.Size4:
				result = (reputationLevel >= ReputationLevel.Friendly);
				break;
			case SpaceShipType.Size5:
				result = (reputationLevel >= ReputationLevel.Respected);
				break;
			case SpaceShipType.Size6:
				result = (reputationLevel >= ReputationLevel.Respected);
				break;
			case SpaceShipType.Size7:
				result = (reputationLevel >= ReputationLevel.Distinguished);
				break;
			case SpaceShipType.Size8:
				result = (reputationLevel >= ReputationLevel.Distinguished);
				break;
			default:
				result = true;
				break;
			}
			return result;
		}

		// Token: 0x04000BE1 RID: 3041
		[SerializeField]
		private MercenaryOption mercenaryOptionPrefab;

		// Token: 0x04000BE2 RID: 3042
		[SerializeField]
		private RectTransform recruitContainer;

		// Token: 0x04000BE3 RID: 3043
		[SerializeField]
		private TMP_Text title;

		// Token: 0x04000BE4 RID: 3044
		[SerializeField]
		private TMP_Text subTitle;

		// Token: 0x04000BE5 RID: 3045
		private float optionHeight = 86f;

		// Token: 0x04000BE6 RID: 3046
		private float totalHeight;

		// Token: 0x04000BE7 RID: 3047
		private Mercenary currentMerc;

		// Token: 0x04000BE8 RID: 3048
		private Profession selectedProfession = Profession.Combat;
	}
}
