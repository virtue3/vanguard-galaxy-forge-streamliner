using System;
using Source.MissionSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000220 RID: 544
	public class BountyMissionRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001429 RID: 5161 RVA: 0x00082065 File Offset: 0x00080265
		public void SetMission(BountyMission m)
		{
			this.SetMission(m, m.bountyLevel);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00082074 File Offset: 0x00080274
		public void SetMission(PatrolMission m)
		{
			this.SetMission(m, m.patrolLevel);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00082083 File Offset: 0x00080283
		public void SetMission(IndustryMission m)
		{
			this.SetMission(m, m.industryLevel);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00082092 File Offset: 0x00080292
		private void SetMission(Mission m, int level)
		{
			this.contained = m;
			this.label.text = m.category;
			this.icon.sprite = this.levelSprites[level];
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000820C0 File Offset: 0x000802C0
		public void OnPointerClick(PointerEventData eventData)
		{
			BountyMission bountyMission = this.contained as BountyMission;
			if (bountyMission != null)
			{
				base.GetComponentInParent<BountyBoard>().ShowMission(bountyMission);
				return;
			}
			PatrolMission patrolMission = this.contained as PatrolMission;
			if (patrolMission != null)
			{
				base.GetComponentInParent<PatrolBoard>().ShowMission(patrolMission);
				return;
			}
			IndustryMission industryMission = this.contained as IndustryMission;
			if (industryMission != null)
			{
				base.GetComponentInParent<IndustryBoard>().ShowMission(industryMission);
			}
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00082120 File Offset: 0x00080320
		public void UpdateSelectedMission(Mission m)
		{
			this.background.color = ((this.contained == m) ? this.highlightColor : this.defaultColor);
		}

		// Token: 0x04000BAF RID: 2991
		[SerializeField]
		private Image background;

		// Token: 0x04000BB0 RID: 2992
		[SerializeField]
		private Image icon;

		// Token: 0x04000BB1 RID: 2993
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000BB2 RID: 2994
		[SerializeField]
		private Sprite[] levelSprites;

		// Token: 0x04000BB3 RID: 2995
		private Mission contained;

		// Token: 0x04000BB4 RID: 2996
		private Color defaultColor = new Color(0f, 0f, 0f, 0.2f);

		// Token: 0x04000BB5 RID: 2997
		private Color highlightColor = new Color(0f, 0f, 0f, 0.4f);
	}
}
