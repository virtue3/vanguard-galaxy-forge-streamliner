using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Bar;
using Behaviour.UI.Spacestation.Location;
using Behaviour.Util;
using Source.Crew;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B0 RID: 688
	public class CrewLocal : SideTabContent
	{
		// Token: 0x0600197E RID: 6526 RVA: 0x0009EC58 File Offset: 0x0009CE58
		private void Start()
		{
			this.SetupCrewMembers(0);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0009EC61 File Offset: 0x0009CE61
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.1f;
				this.UpdateActionText();
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0009EC94 File Offset: 0x0009CE94
		private void UpdateActionText()
		{
			if (!PersonalHangar.current)
			{
				if (GamePlayer.current.currentSpaceShip.crewMembers.Contains(this.currentCrew))
				{
					this.actionText.TL("@CrewEquipInSS", Array.Empty<object>());
				}
				else
				{
					this.actionText.TL("@CrewEquipInSS2", Array.Empty<object>());
				}
				this.actionText.color = Color.white;
				return;
			}
			SpaceShipData selectedShipData = PersonalHangar.current.selectedShipData;
			if (selectedShipData.crewMembers.Contains(this.currentCrew))
			{
				this.actionText.TL("@CrewSlotUnequip", Array.Empty<object>());
				this.actionText.color = Color.white;
				return;
			}
			bool flag = false;
			for (int i = 0; i < selectedShipData.crewMembers.Length; i++)
			{
				if (selectedShipData.crewMembers[i] == null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.actionText.TL("@CrewSlotEquip", Array.Empty<object>());
				this.actionText.color = Color.white;
				return;
			}
			this.actionText.TL("@CrewSlotsFull", Array.Empty<object>());
			this.actionText.color = ColorHelper.reddish;
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0009EDC0 File Offset: 0x0009CFC0
		public void SetupCrewMembers(int index = 0)
		{
			this.crewList.DestroyChildren();
			this.rows.Clear();
			float num = 0f;
			List<CrewMemberData> crewMembers = GamePlayer.current.crewMembers;
			crewMembers.Sort((CrewMemberData a, CrewMemberData b) => b.level.CompareTo(a.level));
			for (int i = 0; i < crewMembers.Count; i++)
			{
				CrewMemberData crewMemberData = crewMembers[i];
				CrewRowItem crewRowItem = UnityEngine.Object.Instantiate<CrewRowItem>(this.crewRowItem, this.crewList);
				crewRowItem.SetCrewMemberData(crewMemberData);
				this.rows.Add(crewRowItem);
				(crewRowItem.transform as RectTransform).anchoredPosition = new Vector2(0f, -num);
				num += 36f;
				if (i == index)
				{
					this.ShowCrewMember(crewMemberData);
				}
			}
			if (crewMembers.Count == 0)
			{
				this.ShowNoCrew();
			}
			this.crewList.sizeDelta = new Vector2(this.crewList.sizeDelta.x, num);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0009EEBC File Offset: 0x0009D0BC
		public void ShowCrewMember(CrewMemberData crew)
		{
			this.currentCrew = crew;
			bool active = SpaceStationInterior.instance != null && !this.currentCrew.critical;
			this.removeCrew.SetActive(active);
			this.renameCrew.SetActive(!this.currentCrew.critical);
			this.infoName.text = crew.GetFullName();
			this.infoName.color = ColorHelper.offWhite;
			this.infoIcon.sprite = crew.sprite;
			this.infoProfession.TL("@UIBarCrewmemberDesc", new object[]
			{
				crew.rarity.GetDisplayName(),
				crew.profession.GetDisplayName()
			});
			this.infoProfession.color = ColorHelper.modifierColor;
			this.infoLevel.text = Translation.Translate("@UIOnlyLevel", Array.Empty<object>()) + ": " + this.currentCrew.level.ToString().HighlightWithColor(ColorHelper.lightCyan);
			this.infoLevel.color = ColorHelper.offWhite;
			this.progressImage.SetCrewMember(this.currentCrew);
			this.skillsParent.DestroyChildren();
			List<SkilltreeNode> list = new List<SkilltreeNode>(crew.unlockedNodes);
			foreach (SkilltreeNode skilltreeNode in crew.skillNodes)
			{
				UnityEngine.Object.Instantiate<BarCrewSkillButton>(this.skillPrefab, this.skillsParent).SetSkill(skilltreeNode, list.Contains(skilltreeNode));
			}
			this.crewInfoParent.gameObject.SetActive(true);
			this.noCrewParent.gameObject.SetActive(false);
			foreach (CrewRowItem crewRowItem in this.rows)
			{
				crewRowItem.SetSelected(crewRowItem.contained == crew);
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0009F0CC File Offset: 0x0009D2CC
		public void ShowNoCrew()
		{
			this.crewInfoParent.gameObject.SetActive(false);
			this.noCrewParent.gameObject.SetActive(true);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0009F0F0 File Offset: 0x0009D2F0
		public void InteractSelectedCrew()
		{
			if (PersonalHangar.current)
			{
				SpaceShipData selectedShipData = PersonalHangar.current.selectedShipData;
				if (selectedShipData.crewMembers.Contains(this.currentCrew))
				{
					selectedShipData.RemoveCrewMember(this.currentCrew);
				}
				else
				{
					selectedShipData.AssignCrewMember(this.currentCrew);
				}
				PersonalHangar.current.UpdateCrewButtons(true);
			}
			this.ShowCrewMember(this.currentCrew);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0009F15C File Offset: 0x0009D35C
		private void TryRemoveCrew()
		{
			GamePlayer current = GamePlayer.current;
			int num = current.crewMembers.IndexOf(this.currentCrew);
			foreach (SpaceShipData spaceShipData in current.spaceShips)
			{
				spaceShipData.RemoveCrewMember(this.currentCrew);
			}
			current.crewMembers.Remove(this.currentCrew);
			int index = Mathf.Clamp(num - 1, 0, current.crewMembers.Count - 1);
			this.SetupCrewMembers(index);
			if (PersonalHangar.current)
			{
				PersonalHangar.current.UpdateCrewButtons(true);
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@RemovedOfficer", Array.Empty<object>())).WithColor(ColorHelper.orange75).Show();
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0009F23C File Offset: 0x0009D43C
		public void RemoveCrew()
		{
			if (this.currentCrew.critical)
			{
				return;
			}
			if (SpaceStationInterior.instance == null)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@RemoveOfficerNotDocked", Array.Empty<object>())).WithColor(ColorHelper.reddish).Show();
				return;
			}
			AlertPopup.ShowQuery("@RemoveOfficerQuery", "@UIYes", "@UINo", delegate
			{
				this.TryRemoveCrew();
			}, null, null, null);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0009F2B0 File Offset: 0x0009D4B0
		public void RenameCrew()
		{
			if (this.currentCrew.critical)
			{
				return;
			}
			UnityEngine.Object.Instantiate<OfficerRenamePopup>(this.renamePopupPrefab, UITooltip.tooltipParent).Open(this.currentCrew, delegate(string firstName, string callsign, string lastName)
			{
				this.currentCrew.SetName(firstName, callsign, lastName);
				this.ShowCrewMember(this.currentCrew);
				foreach (CrewRowItem crewRowItem in this.rows)
				{
					if (crewRowItem.contained == this.currentCrew)
					{
						crewRowItem.SetCrewMemberData(this.currentCrew);
						break;
					}
				}
			});
		}

		// Token: 0x04000FF1 RID: 4081
		[SerializeField]
		private RectTransform crewList;

		// Token: 0x04000FF2 RID: 4082
		[SerializeField]
		private CrewRowItem crewRowItem;

		// Token: 0x04000FF3 RID: 4083
		[SerializeField]
		private RectTransform crewInfoParent;

		// Token: 0x04000FF4 RID: 4084
		[SerializeField]
		private RectTransform noCrewParent;

		// Token: 0x04000FF5 RID: 4085
		[SerializeField]
		private TMP_Text infoName;

		// Token: 0x04000FF6 RID: 4086
		[SerializeField]
		private TMP_Text infoProfession;

		// Token: 0x04000FF7 RID: 4087
		[SerializeField]
		private TMP_Text infoLevel;

		// Token: 0x04000FF8 RID: 4088
		[SerializeField]
		public LevelProgressImage progressImage;

		// Token: 0x04000FF9 RID: 4089
		[SerializeField]
		private Image infoIcon;

		// Token: 0x04000FFA RID: 4090
		[SerializeField]
		private RectTransform skillsParent;

		// Token: 0x04000FFB RID: 4091
		[SerializeField]
		private BarCrewSkillButton skillPrefab;

		// Token: 0x04000FFC RID: 4092
		[SerializeField]
		private TMP_Text actionText;

		// Token: 0x04000FFD RID: 4093
		[SerializeField]
		private GameObject removeCrew;

		// Token: 0x04000FFE RID: 4094
		[SerializeField]
		private GameObject renameCrew;

		// Token: 0x04000FFF RID: 4095
		[SerializeField]
		private OfficerRenamePopup renamePopupPrefab;

		// Token: 0x04001000 RID: 4096
		private CrewMemberData currentCrew;

		// Token: 0x04001001 RID: 4097
		private List<CrewRowItem> rows = new List<CrewRowItem>();

		// Token: 0x04001002 RID: 4098
		private float updateTimer;
	}
}
