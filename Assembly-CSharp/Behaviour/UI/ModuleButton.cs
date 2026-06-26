using System;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI
{
	// Token: 0x020001ED RID: 493
	public class ModuleButton : EquipmentButton
	{
		// Token: 0x060012B7 RID: 4791 RVA: 0x0007A2E8 File Offset: 0x000784E8
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this.sizeText.gameObject.SetActive(true);
			this.SetSizeText();
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0007A308 File Offset: 0x00078508
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			this.sizeText.gameObject.SetActive(false);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0007A322 File Offset: 0x00078522
		private void SetSizeText()
		{
			this.sizeText.text = string.Format("{0}: {1}", Translation.Translate("@Size", Array.Empty<object>()), this.spaceShipModule.size);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0007A358 File Offset: 0x00078558
		protected override void SetHoveringButton(bool toggle)
		{
			InventoryInteractionManager.Instance.hoveringModuleButton = (toggle ? this : null);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0007A36C File Offset: 0x0007856C
		protected override void AssignSlot<T>(T slot)
		{
			SpaceShipModule spaceShipModule = slot as SpaceShipModule;
			if (spaceShipModule != null)
			{
				this.spaceShipModule = spaceShipModule;
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0007A390 File Offset: 0x00078590
		public override string GetSlotText<T>(T slot)
		{
			SpaceShipModule spaceShipModule = slot as SpaceShipModule;
			if (spaceShipModule != null)
			{
				return Translation.Translate("@SPEquipEmpty", new object[]
				{
					spaceShipModule.slot
				});
			}
			return "";
		}

		// Token: 0x04000A7C RID: 2684
		public SpaceShipModule spaceShipModule;

		// Token: 0x04000A7D RID: 2685
		[SerializeField]
		protected TextMeshProUGUI sizeText;
	}
}
