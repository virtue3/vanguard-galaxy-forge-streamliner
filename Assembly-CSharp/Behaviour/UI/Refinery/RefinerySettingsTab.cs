using System;
using Source.Galaxy.POI;
using Source.Mining;
using Source.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Refinery
{
	// Token: 0x0200024E RID: 590
	public class RefinerySettingsTab : MonoBehaviour
	{
		// Token: 0x060015CD RID: 5581 RVA: 0x0008B606 File Offset: 0x00089806
		private void OnEnable()
		{
			this.RefreshSettings();
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0008B610 File Offset: 0x00089810
		public void RefreshSettings()
		{
			this.autoRefine.isOn = SpaceStation.current.refinery.autoRefine;
			this.autoSell.isOn = Register.HasFlag("AutoSell", false);
			this.UpdateToggleState(this.autoRefineOptionShipCargo, this.autoRefine.isOn);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0008B664 File Offset: 0x00089864
		private void UpdateToggleState(Toggle toggle, bool condition)
		{
			toggle.interactable = condition;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0008B66D File Offset: 0x0008986D
		public void UpdateAutoRefine()
		{
			SpaceStation.current.refinery.autoRefine = this.autoRefine.isOn;
			this.UpdateToggleState(this.autoRefineOptionShipCargo, this.autoRefine.isOn);
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0008B6A0 File Offset: 0x000898A0
		public void UpdateAutoSell()
		{
			Register.SetFlag("AutoSell", this.autoSell.isOn);
			Refinery.autoSell = this.autoSell.isOn;
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0008B6C7 File Offset: 0x000898C7
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0008B6D5 File Offset: 0x000898D5
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000D10 RID: 3344
		[SerializeField]
		private Toggle autoRefine;

		// Token: 0x04000D11 RID: 3345
		[SerializeField]
		private Toggle autoRefineOptionShipCargo;

		// Token: 0x04000D12 RID: 3346
		[SerializeField]
		private Toggle autoSell;
	}
}
