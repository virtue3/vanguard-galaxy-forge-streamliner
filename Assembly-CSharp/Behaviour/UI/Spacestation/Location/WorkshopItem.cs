using System;
using System.Collections.Generic;
using System.Linq;
using Behavior.Equipment.Booster;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x0200022A RID: 554
	public class WorkshopItem : MonoBehaviour
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x00086081 File Offset: 0x00084281
		// (set) Token: 0x060014CE RID: 5326 RVA: 0x00086089 File Offset: 0x00084289
		public SalvageWorkshop salvageWorkshop { get; private set; }

		// Token: 0x060014CF RID: 5327 RVA: 0x00086094 File Offset: 0x00084294
		public void SetContent(InventoryItemType item, SalvageWorkshop parent)
		{
			this.salvageWorkshop = parent;
			this.Refresh();
			Color detailsColor = ColorHelper.detailsColor;
			Color modifierColor = ColorHelper.modifierColor;
			base.GetComponent<Image>().color = item.rarity.GetBackgroundColor();
			this.header.GetComponent<Image>().color = item.rarity.GetBackgroundColor();
			this.icon.sprite = item.icon;
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			AbstractBooster component2 = item.GetComponent<AbstractBooster>();
			if (component && !component2)
			{
				this.headerLabel.TL("@EquipmentTooltipHeader", new object[]
				{
					component.size.GetDisplayName(),
					component.typeDisplayName
				});
			}
			else
			{
				this.headerLabel.TL(item.itemCategory.GetDisplayName(), Array.Empty<object>());
			}
			if (item.itemCategory.CanBeEquiped() && GamePlayer.current != null)
			{
				this.headerLabel.color = (item.CanBeEquippedOn(GamePlayer.current.currentSpaceShip) ? Color.white : ColorHelper.reddish);
			}
			this.header.sizeDelta = new Vector2(this.headerLabel.preferredWidth + 48f, this.header.sizeDelta.y);
			UITooltipText uitooltipText = this.AddTextLine(item.displayName, 16, 8f);
			uitooltipText.Text.rectTransform.offsetMax = new Vector2(-64f, 0f);
			uitooltipText.Text.color = item.rarity.GetColor();
			Manufacturer? manufacturer = item.GetManufacturer();
			if (manufacturer != null)
			{
				UITooltipText uitooltipText2 = this.AddTextLine(manufacturer.Value.GetDisplayName(), 12, 8f);
				uitooltipText2.Text.color = detailsColor;
				uitooltipText2.Text.rectTransform.offsetMax = new Vector2(-64f, 0f);
			}
			if (!item.missionItem && (item.equipmentBuilder != null || item.itemLevel > 0))
			{
				this.levelLabel.TL("@TooltipItemLevel", new object[]
				{
					item.itemLevel
				});
			}
			else
			{
				this.levelLabel.gameObject.SetActive(false);
			}
			if (component && !(component is ReactorModule) && !component2)
			{
				this.energyLabel.text = GameMath.FormatNumber(component.energyDraw, 1);
				this.energyLabel.color = ColorHelper.purpleBadge;
				this.energyLabel.rectTransform.sizeDelta = new Vector2(this.energyLabel.preferredWidth, 12f);
				this.energyLabel.gameObject.SetActive(true);
			}
			else
			{
				this.energyLabel.gameObject.SetActive(false);
			}
			if (component)
			{
				AbstractTurret abstractTurret = component as AbstractTurret;
				if (abstractTurret != null)
				{
					this.rangeLabel.text = GameMath.FormatNumber(abstractTurret.range, 1);
					this.rangeLabel.color = ColorHelper.greenBadge;
					this.rangeLabel.rectTransform.sizeDelta = new Vector2(this.rangeLabel.preferredWidth, 12f);
					this.rangeLabel.gameObject.SetActive(true);
					goto IL_32E;
				}
			}
			this.rangeLabel.gameObject.SetActive(false);
			IL_32E:
			this.AddMinHeightSpacer(60f);
			this.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			if (component)
			{
				MainStat mainStat = component.GetMainStat();
				if (mainStat != null)
				{
					this.AddTextLine("<color=green>" + mainStat.mainStatAmount + "</color> " + Translation.Translate(mainStat.mainStatName, Array.Empty<object>()), 16, 8f).Text.color = detailsColor;
				}
				foreach (SubStat subStat in component.GetMainSubStats().subStatsList)
				{
					this.AddTextLine("  " + Translation.Translate(subStat.mainSubStatName, Array.Empty<object>()) + " " + subStat.mainSubStatAmount, 12, 8f).Text.color = detailsColor;
				}
				this.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
				bool rerolledBefore = component.GetStats().Any((EquipStatLine s) => !s.canReroll);
				bool flag = false;
				foreach (ValueTuple<EquipStatLine, int> valueTuple in component.GetStatsWithIndex(false))
				{
					WorkshopStat workshopStat = UnityEngine.Object.Instantiate<WorkshopStat>(this.statPrefab, this._contentParent);
					workshopStat.SetStat(valueTuple.Item1, valueTuple.Item2, this.salvageWorkshop, rerolledBefore);
					this.AddContent(workshopStat);
					flag = true;
				}
				AbstractTurret abstractTurret2 = component as AbstractTurret;
				if (abstractTurret2 != null && abstractTurret2.ammoType != null)
				{
					this.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
					this.AddTextLine("  " + Translation.Highlight("@AmmoType", ColorHelper.white75, new object[]
					{
						abstractTurret2.ammoType.displayName
					}), 12, 8f).Text.color = ColorHelper.boringGrey;
					flag = true;
				}
				for (int i = 0; i < component.aspectSlots.Count; i++)
				{
					flag = false;
					AspectSlot aspectSlot = component.aspectSlots[i];
					WorkshopAspect workshopAspect = UnityEngine.Object.Instantiate<WorkshopAspect>(this.aspectPrefab, this._contentParent);
					workshopAspect.SetAspect(aspectSlot, this.salvageWorkshop, i);
					this.AddContent(workshopAspect);
				}
				if (flag)
				{
					this.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
				}
			}
			this.SetContentList();
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000866E8 File Offset: 0x000848E8
		private void SetContentList()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			float num = 0f;
			float num2 = 0f;
			foreach (UITooltipContent uitooltipContent in this._contentList)
			{
				RectTransform rectTransform2 = uitooltipContent.transform as RectTransform;
				rectTransform2.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x, -num);
				if (uitooltipContent is UITooltipSpacer)
				{
					num = Mathf.Max(uitooltipContent.Height, num);
				}
				else
				{
					num += uitooltipContent.Height;
				}
				num2 = uitooltipContent.Spacing;
			}
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num - num2 + 20f);
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000867BC File Offset: 0x000849BC
		public void Refresh()
		{
			this._contentParent.DestroyChildren();
			this._contentList.Clear();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000867D4 File Offset: 0x000849D4
		public UITooltipText AddTextLine(string text, int size = 12, float margin = 8f)
		{
			UITooltipText uitooltipText = UnityEngine.Object.Instantiate<UITooltipText>(this._textPrefab, this._contentParent);
			uitooltipText.SetText(text, size, margin);
			this.AddContent(uitooltipText);
			return uitooltipText;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00086804 File Offset: 0x00084A04
		public UITooltipSpacer AddMinHeightSpacer(float minHeight)
		{
			UITooltipSpacer uitooltipSpacer = UnityEngine.Object.Instantiate<UITooltipSpacer>(this._spacerPrefab, this._contentParent);
			uitooltipSpacer.height = minHeight;
			this.AddContent(uitooltipSpacer);
			return uitooltipSpacer;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00086834 File Offset: 0x00084A34
		public UITooltipLine AddSeparator(Color c, float thickness, float spacingTop, float spacingBottom)
		{
			if (this._contentList[this._contentList.Count - 1] is UITooltipLine)
			{
				return null;
			}
			UITooltipLine uitooltipLine = UnityEngine.Object.Instantiate<UITooltipLine>(this._linePrefab, this._contentParent);
			uitooltipLine.SetLine(c, thickness, spacingTop, spacingBottom);
			this.AddContent(uitooltipLine);
			return uitooltipLine;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00086887 File Offset: 0x00084A87
		public void AddContent(UITooltipContent content)
		{
			this._contentList.Add(content);
		}

		// Token: 0x04000C1E RID: 3102
		[SerializeField]
		private RectTransform _contentParent;

		// Token: 0x04000C1F RID: 3103
		[SerializeField]
		private UITooltipText _textPrefab;

		// Token: 0x04000C20 RID: 3104
		[SerializeField]
		private UITooltipLine _linePrefab;

		// Token: 0x04000C21 RID: 3105
		[SerializeField]
		private UITooltipSpacer _spacerPrefab;

		// Token: 0x04000C22 RID: 3106
		[SerializeField]
		private Image icon;

		// Token: 0x04000C23 RID: 3107
		[SerializeField]
		private TMP_Text levelLabel;

		// Token: 0x04000C24 RID: 3108
		[SerializeField]
		private TMP_Text energyLabel;

		// Token: 0x04000C25 RID: 3109
		[SerializeField]
		private TMP_Text rangeLabel;

		// Token: 0x04000C26 RID: 3110
		[SerializeField]
		private TMP_Text headerLabel;

		// Token: 0x04000C27 RID: 3111
		[SerializeField]
		private RectTransform header;

		// Token: 0x04000C28 RID: 3112
		[SerializeField]
		private WorkshopAspect aspectPrefab;

		// Token: 0x04000C29 RID: 3113
		[SerializeField]
		private WorkshopStat statPrefab;

		// Token: 0x04000C2A RID: 3114
		private List<UITooltipContent> _contentList = new List<UITooltipContent>();
	}
}
