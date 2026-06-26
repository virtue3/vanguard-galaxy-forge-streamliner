using System;
using Behaviour.UI.Side_Menu.SideTabs;
using Source.Crew;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D1 RID: 465
	public class CrewRowItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x000747F2 File Offset: 0x000729F2
		// (set) Token: 0x0600118F RID: 4495 RVA: 0x000747FA File Offset: 0x000729FA
		public CrewMemberData contained { get; private set; }

		// Token: 0x06001190 RID: 4496 RVA: 0x00074804 File Offset: 0x00072A04
		public void SetCrewMemberData(CrewMemberData data)
		{
			this.contained = data;
			this.icon.sprite = this.contained.sprite;
			this.nameText.text = "(" + this.contained.level.ToString() + ") " + data.GetFullName();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00074864 File Offset: 0x00072A64
		public void OnPointerClick(PointerEventData eventData)
		{
			CrewLocal componentInParent = base.GetComponentInParent<CrewLocal>();
			componentInParent.ShowCrewMember(this.contained);
			if (eventData.button == PointerEventData.InputButton.Right || (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2))
			{
				componentInParent.InteractSelectedCrew();
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x000748A4 File Offset: 0x00072AA4
		public void SetSelected(bool sel)
		{
			float num = sel ? 0.5f : 0f;
			this.background.color = new Color(num, num, num, 0.8f);
		}

		// Token: 0x040009A2 RID: 2466
		[SerializeField]
		private Image background;

		// Token: 0x040009A3 RID: 2467
		[SerializeField]
		private Image icon;

		// Token: 0x040009A4 RID: 2468
		[SerializeField]
		private TMP_Text nameText;
	}
}
