using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Crew;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station.Patrons;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Bar
{
	// Token: 0x0200023C RID: 572
	public class CrewMemberInfo : MonoBehaviour
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x00089488 File Offset: 0x00087688
		public void Show(Source.Galaxy.POI.Station.Patrons.CrewMember patron)
		{
			this.currentCrew = patron;
			CrewMemberData crewMember = patron.crewMember;
			this.infoName.text = crewMember.GetFullName();
			this.infoIcon.sprite = crewMember.sprite;
			this.infoProfession.TL("@UIBarCrewmemberDesc", new object[]
			{
				crewMember.rarity.GetDisplayName(),
				crewMember.profession.GetDisplayName()
			});
			this.skillsParent.DestroyChildren();
			List<SkilltreeNode> list = new List<SkilltreeNode>(crewMember.unlockedNodes);
			foreach (SkilltreeNode skilltreeNode in crewMember.skillNodes)
			{
				UnityEngine.Object.Instantiate<BarCrewSkillButton>(this.skillPrefab, this.skillsParent).SetSkill(skilltreeNode, list.Contains(skilltreeNode));
			}
			this.infoCost.TL("@UIBarCost", new object[]
			{
				crewMember.purchaseCost
			});
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0008959C File Offset: 0x0008779C
		public void Hide()
		{
			this.currentCrew = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x000895B4 File Offset: 0x000877B4
		public void ButtonHire()
		{
			if (this.currentCrew != null)
			{
				int purchaseCost = this.currentCrew.crewMember.purchaseCost;
				if (!GamePlayer.current.CanAfford((float)purchaseCost))
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
					return;
				}
				GamePlayer.current.RemoveCredits((float)purchaseCost);
				GamePlayer.current.crewMembers.Add(this.currentCrew.crewMember);
				SpaceStation.current.bar.availablePatrons.Remove(this.currentCrew);
				base.GetComponentInParent<BarUI>().RefreshPatrons();
			}
		}

		// Token: 0x04000CA2 RID: 3234
		private Source.Galaxy.POI.Station.Patrons.CrewMember currentCrew;

		// Token: 0x04000CA3 RID: 3235
		[SerializeField]
		private TMP_Text infoName;

		// Token: 0x04000CA4 RID: 3236
		[SerializeField]
		private TMP_Text infoProfession;

		// Token: 0x04000CA5 RID: 3237
		[SerializeField]
		private TMP_Text infoCost;

		// Token: 0x04000CA6 RID: 3238
		[SerializeField]
		private Image infoIcon;

		// Token: 0x04000CA7 RID: 3239
		[SerializeField]
		private RectTransform skillsParent;

		// Token: 0x04000CA8 RID: 3240
		[SerializeField]
		private BarCrewSkillButton skillPrefab;
	}
}
