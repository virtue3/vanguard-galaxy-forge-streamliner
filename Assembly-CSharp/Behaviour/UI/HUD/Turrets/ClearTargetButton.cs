using System;
using System.Collections.Generic;
using Behaviour.Equipment.Module;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.HUD.Turrets
{
	// Token: 0x02000289 RID: 649
	public class ClearTargetButton : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x060017CB RID: 6091 RVA: 0x000954AC File Offset: 0x000936AC
		public void Init(TurretControl turretControl)
		{
			this.turretControl = turretControl;
			if (turretControl.NoTurrets())
			{
				base.gameObject.SetActive(false);
			}
			else
			{
				base.gameObject.SetActive(true);
			}
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			MiningModule module = spaceShip.GetModule<MiningModule>();
			CombatModule module2 = spaceShip.GetModule<CombatModule>();
			SalvageModule module3 = spaceShip.GetModule<SalvageModule>();
			if (module != null)
			{
				this.targetingModules["@MiningScanner"] = module;
			}
			if (module2 != null)
			{
				this.targetingModules["@CombatScanner"] = module2;
			}
			if (module3 != null)
			{
				this.targetingModules["@SalvageScanner"] = module3;
			}
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00095550 File Offset: 0x00093750
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@ScannerTargets", Array.Empty<object>()) + ":", 12, 8f).Text.color = ColorHelper.offWhite;
			bool flag = true;
			foreach (KeyValuePair<string, AbstractTargetingModule> keyValuePair in this.targetingModules)
			{
				string str = Translation.Translate("@None", Array.Empty<object>()).HighlightWithColor(ColorHelper.boringGrey);
				if (keyValuePair.Value.priorityTarget != null)
				{
					str = Translation.Translate(keyValuePair.Value.priorityTarget.targetName, Array.Empty<object>()).HighlightWithColor(ColorHelper.red90);
					flag = false;
				}
				string str2 = Translation.TranslateOnly(keyValuePair.Key, Array.Empty<object>()) + ": ";
				tooltip.AddTextLine(str2 + str, 12, 8f);
			}
			if (!flag)
			{
				tooltip.AddTextLine(Translation.Translate("@TCClearTargets", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.offWhite;
			}
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00095690 File Offset: 0x00093890
		public void OnPointerClick(PointerEventData eventData)
		{
			this.turretControl.ClearTargets();
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0009569D File Offset: 0x0009389D
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.turretControl.ShowRange();
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x000956AA File Offset: 0x000938AA
		public void OnPointerExit(PointerEventData eventData)
		{
			this.turretControl.HideRange();
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x000956B7 File Offset: 0x000938B7
		private void OnDisable()
		{
			if (!this.turretControl)
			{
				return;
			}
			this.turretControl.HideRange();
		}

		// Token: 0x04000EC3 RID: 3779
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000EC4 RID: 3780
		private TurretControl turretControl;

		// Token: 0x04000EC5 RID: 3781
		private Dictionary<string, AbstractTargetingModule> targetingModules = new Dictionary<string, AbstractTargetingModule>();
	}
}
