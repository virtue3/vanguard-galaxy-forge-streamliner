using System;
using System.Collections.Generic;
using Behaviour.Hazard;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;
using Source.Galaxy;
using Source.Hazard;
using Source.Player;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001E9 RID: 489
	public class UIInfoTextParent : MonoBehaviour
	{
		// Token: 0x06001294 RID: 4756 RVA: 0x000799A4 File Offset: 0x00077BA4
		private void Awake()
		{
			UIInfoTextParent.instance = this;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x000799AC File Offset: 0x00077BAC
		private FloatingInfoText GetInfoText(GameObject originObj, InfoType infoType, string postFix = "", Color? color = null)
		{
			foreach (FloatingInfoText floatingInfoText in this.infoTexts)
			{
				if (UIInfoTextParent.IsSameInfo(originObj, infoType, postFix, floatingInfoText, color) && !floatingInfoText.fading)
				{
					return floatingInfoText;
				}
			}
			return null;
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00079A14 File Offset: 0x00077C14
		private static bool IsSameInfo(GameObject originObj, InfoType infoType, string postFix, FloatingInfoText infoText, Color? color = null)
		{
			bool flag = true;
			if (infoText.postfix != "")
			{
				flag = (infoText.postfix == postFix);
			}
			bool flag2 = infoText.textColor == color;
			return infoText.originObject.Equals(originObj) && infoText.type == infoType && flag && flag2;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00079A84 File Offset: 0x00077C84
		public void ShowDamageNumber(DamageData data, Transform parent)
		{
			if (data.damageAmount <= 0f || !GameplayerPrefs.GetShowDamageNumbers())
			{
				return;
			}
			LocalHazard localHazard = null;
			if (!data.sourceUnit || !data.sourceUnit.IsPlayer(false))
			{
				if (data.targetUnit)
				{
					AbstractUnit abstractUnit = data.targetUnit as AbstractUnit;
					if (abstractUnit != null && abstractUnit.IsPlayer(false))
					{
						goto IL_A1;
					}
				}
				if (!data.source || !data.source.TryGetComponent<LocalHazard>(out localHazard))
				{
					return;
				}
				MineHazardData mineHazardData = ((localHazard != null) ? localHazard.data : null) as MineHazardData;
				if (mineHazardData == null || mineHazardData.faction != Faction.player)
				{
					return;
				}
			}
			IL_A1:
			Vector2 a = data.hitCoordinates - (Vector2)parent.position;
			float num = 0.4f;
			Vector2 b = new Vector2(SeededRandom.Global.RandomRange(-num, num), SeededRandom.Global.RandomRange(-num, num));
			Vector2 positionOffset = a + b;
			InfoType type = data.IsCriticalHit() ? InfoType.CRITICALHIT : InfoType.DAMAGE;
			if (data.reflectedDamage)
			{
				type = InfoType.REFLECTHIT;
			}
			this.CreateOrUpdateInfoText(parent, data.damageAmount, type, positionOffset, "", new Color?(data.type.GetColor()));
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00079BB8 File Offset: 0x00077DB8
		public void ShowExperienceNumber(float experience)
		{
			Transform transform = GameplayManager.Instance.spaceShip.transform;
			this.CreateOrUpdateInfoText(transform, experience, InfoType.XP, Vector2.zero, "", null);
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00079BF4 File Offset: 0x00077DF4
		public void ShowPickupText(string itemName, int count)
		{
			Transform transform = GameplayManager.Instance.spaceShip.transform;
			this.CreateOrUpdateInfoText(transform, (float)count, InfoType.PICKUP, Vector2.zero, itemName, null);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00079C2A File Offset: 0x00077E2A
		public void ShowWarningText(string warning, Transform position = null, Color? c = null)
		{
			this.CreateOrUpdateInfoText(position ?? GameplayManager.Instance.spaceShip.transform, 0f, InfoType.INFO, Vector2.zero, warning, c);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00079C54 File Offset: 0x00077E54
		private void CreateOrUpdateInfoText(Transform parent, float number, InfoType type, Vector2 positionOffset, string postFix = "", Color? damageColor = null)
		{
			FloatingInfoText floatingInfoText = this.GetInfoText(parent.gameObject, type, postFix, damageColor);
			if (floatingInfoText)
			{
				floatingInfoText.AddNumber(number);
				return;
			}
			floatingInfoText = UnityEngine.Object.Instantiate<FloatingInfoText>(this.floatingInfoTextPrefab, base.transform);
			floatingInfoText.Show(parent.gameObject, Mathf.Ceil(number), type, positionOffset, postFix, damageColor);
			this.infoTexts.Add(floatingInfoText);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00079CBA File Offset: 0x00077EBA
		public void DeleteInfoText(FloatingInfoText floatingInfoText)
		{
			this.infoTexts.Remove(floatingInfoText);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00079CCC File Offset: 0x00077ECC
		public void ClearInfoTexts()
		{
			foreach (FloatingInfoText floatingInfoText in this.infoTexts)
			{
				if (floatingInfoText)
				{
					UnityEngine.Object.Destroy(floatingInfoText.gameObject);
				}
			}
			this.infoTexts.Clear();
		}

		// Token: 0x04000A68 RID: 2664
		public static UIInfoTextParent instance;

		// Token: 0x04000A69 RID: 2665
		[SerializeField]
		private FloatingInfoText floatingInfoTextPrefab;

		// Token: 0x04000A6A RID: 2666
		private List<FloatingInfoText> infoTexts = new List<FloatingInfoText>();
	}
}
