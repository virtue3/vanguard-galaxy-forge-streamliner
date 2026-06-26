using System;
using Behaviour.Equipment.Aspect;
using LightJson;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200030A RID: 778
	public class AspectItem : UsableItem
	{
		// Token: 0x06001D41 RID: 7489 RVA: 0x000AFA28 File Offset: 0x000ADC28
		public override bool CanStackWith(InventoryItemType other)
		{
			AspectItem aspectItem;
			return other.TryGetComponent<AspectItem>(out aspectItem) && aspectItem.aspectName == this.aspectName;
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000AFA52 File Offset: 0x000ADC52
		public override void DataFromJson(JsonObject data)
		{
			this.aspectName = data["aspectName"];
			this.equipAspect = EquipAspect.Get(this.aspectName);
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000AFA7B File Offset: 0x000ADC7B
		public override void DataToJson(JsonObject data)
		{
			data["aspectName"] = this.aspectName;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000AFA93 File Offset: 0x000ADC93
		protected override void InitializeItem()
		{
			if (string.IsNullOrEmpty(this.aspectName))
			{
				return;
			}
			this.SetAspect();
			base.item.SetIcon(this.CreateBlueprintSprite());
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000AFABA File Offset: 0x000ADCBA
		private void SetAspect()
		{
			this.equipAspect = EquipAspect.Get(this.aspectName);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000AFACD File Offset: 0x000ADCCD
		public override bool OnUse()
		{
			Debug.Log("Ja ik wil proberen te gebruiken, hallo.");
			return false;
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000AFADC File Offset: 0x000ADCDC
		private Sprite CreateBlueprintSprite()
		{
			return SpriteCombiner.CombineSpritesGPU(base.item.icon, this.equipAspect.icon, 0.6f, new Color?(new Color(1f, 1f, 1f, 0.5f)), 1.5f, 0.65f);
		}

		// Token: 0x040011F9 RID: 4601
		public string aspectName;

		// Token: 0x040011FA RID: 4602
		public EquipAspect equipAspect;
	}
}
