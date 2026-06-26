using System;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Missions
{
	// Token: 0x0200025C RID: 604
	public class MissionRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x0008DC65 File Offset: 0x0008BE65
		// (set) Token: 0x06001644 RID: 5700 RVA: 0x0008DC6D File Offset: 0x0008BE6D
		public Mission contained { get; private set; }

		// Token: 0x06001645 RID: 5701 RVA: 0x0008DC76 File Offset: 0x0008BE76
		private void Awake()
		{
			this.background = base.GetComponent<Image>();
			this.normalColor = this.background.color;
			this.sidePanelParent = base.GetComponentInParent<MissionUI>();
			this.spaceStationParent = base.GetComponentInParent<MissionBoard>();
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0008DCAD File Offset: 0x0008BEAD
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.UpdateStatusIcon();
				this.updateTimer = 0.5f;
			}
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0008DCE0 File Offset: 0x0008BEE0
		public void SetMission(Mission m)
		{
			this.contained = m;
			if (m.GetIcon())
			{
				this.missionIcon.sprite = m.GetIcon();
			}
			else
			{
				this.missionIcon.gameObject.SetActive(false);
			}
			this.text.text = m.name;
			this.text.color = (m.failed ? ColorHelper.reddish : m.difficulty.GetColor());
			this.background.color = (this.normalColor = m.difficulty.GetBackgroundColor());
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0008DD7C File Offset: 0x0008BF7C
		public void UpdateStatusIcon()
		{
			if (this.contained.CanClaimRewards())
			{
				this.turnInIcon.gameObject.SetActive(true);
				this.statusIcon.gameObject.SetActive(false);
			}
			else
			{
				this.turnInIcon.gameObject.SetActive(false);
				if (this.sidePanelParent)
				{
					this.statusIcon.sprite = this.completedIcon;
					this.statusIcon.gameObject.SetActive(this.contained.isComplete);
				}
				else if (GamePlayer.current.missions.Contains(this.contained))
				{
					this.statusIcon.sprite = (this.contained.isComplete ? this.completedIcon : this.busyIcon);
					this.statusIcon.gameObject.SetActive(true);
				}
				else
				{
					this.statusIcon.gameObject.SetActive(false);
				}
			}
			this.completedBorder.gameObject.SetActive(this.contained.isComplete);
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0008DE85 File Offset: 0x0008C085
		public void SetSelected(bool sel)
		{
			this.selected = sel;
			this.background.color = (sel ? this.selectedColor : this.normalColor);
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0008DEAA File Offset: 0x0008C0AA
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.sidePanelParent)
			{
				this.sidePanelParent.ShowMission(this.contained);
				return;
			}
			if (this.spaceStationParent)
			{
				this.spaceStationParent.ShowMission(this.contained, true);
			}
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0008DEEA File Offset: 0x0008C0EA
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.background.color = this.highlightColor;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0008DEFD File Offset: 0x0008C0FD
		public void OnPointerExit(PointerEventData eventData)
		{
			this.background.color = (this.selected ? this.selectedColor : this.normalColor);
		}

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		private Image missionIcon;

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		private TMP_Text text;

		// Token: 0x04000D7B RID: 3451
		[SerializeField]
		private Image statusIcon;

		// Token: 0x04000D7C RID: 3452
		[SerializeField]
		private Image turnInIcon;

		// Token: 0x04000D7D RID: 3453
		[SerializeField]
		private Image completedBorder;

		// Token: 0x04000D7E RID: 3454
		[SerializeField]
		private Sprite busyIcon;

		// Token: 0x04000D7F RID: 3455
		[SerializeField]
		private Sprite completedIcon;

		// Token: 0x04000D80 RID: 3456
		[SerializeField]
		private Color selectedColor;

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		private Color highlightColor;

		// Token: 0x04000D82 RID: 3458
		private bool selected;

		// Token: 0x04000D83 RID: 3459
		private Image background;

		// Token: 0x04000D84 RID: 3460
		private Color normalColor;

		// Token: 0x04000D86 RID: 3462
		private MissionUI sidePanelParent;

		// Token: 0x04000D87 RID: 3463
		private MissionBoard spaceStationParent;

		// Token: 0x04000D88 RID: 3464
		private float updateTimer;
	}
}
