using System;
using Source.Galaxy;
using Source.Simulation.World.POI;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000283 RID: 643
	public class IndustrialOpsUI : MonoBehaviour
	{
		// Token: 0x060017AC RID: 6060 RVA: 0x00094BEC File Offset: 0x00092DEC
		private void Update()
		{
			MapPointOfInterest current = MapPointOfInterest.current;
			IndustrialOutpost industrialOutpost = ((current != null) ? current.storyteller : null) as IndustrialOutpost;
			if (industrialOutpost == null)
			{
				HudManager.Instance.ToggleIndustryOps(false);
				this._statusText.text = "";
				return;
			}
			float num = industrialOutpost.repairAmount / industrialOutpost.repairMax;
			float ammoAmount = industrialOutpost.ammoAmount;
			string text = Translation.Highlight("@IndustryRepairStatus", ((double)num < 0.1) ? ColorHelper.reddish : ColorHelper.greenish, new object[]
			{
				GameMath.FormatPercentage(num, FormatPercentageMode.Default, 0)
			}) + "\n" + Translation.Highlight("@IndustryAmmoStatus", ((double)ammoAmount < 0.25) ? ColorHelper.reddish : ColorHelper.greenish, new object[]
			{
				GameMath.FormatPercentage(ammoAmount, FormatPercentageMode.Default, 0)
			});
			this._statusText.text = text;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00094CC4 File Offset: 0x00092EC4
		public void SetActive(bool active = true)
		{
			if (!this || !base.gameObject)
			{
				return;
			}
			MapPointOfInterest current = MapPointOfInterest.current;
			IndustrialOutpost industrialOutpost = ((current != null) ? current.storyteller : null) as IndustrialOutpost;
			if (active && industrialOutpost != null)
			{
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00094D20 File Offset: 0x00092F20
		public void SetAnchoredPosition(float y)
		{
			Vector2 anchoredPosition = (base.transform as RectTransform).anchoredPosition;
			anchoredPosition.y = y;
			(base.transform as RectTransform).anchoredPosition = anchoredPosition;
		}

		// Token: 0x04000EA8 RID: 3752
		[SerializeField]
		private TMP_Text _statusText;
	}
}
