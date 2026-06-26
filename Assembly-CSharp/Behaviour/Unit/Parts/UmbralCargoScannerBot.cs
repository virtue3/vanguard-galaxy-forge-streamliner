using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.Effects;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Galaxy;
using Source.Item;
using Source.MissionSystem;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit.Parts
{
	// Token: 0x020001CD RID: 461
	public class UmbralCargoScannerBot : MonoBehaviour
	{
		// Token: 0x06001168 RID: 4456 RVA: 0x00073802 File Offset: 0x00071A02
		private void Start()
		{
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralCargoScannerDeployed", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
			this.CreateLaserEffect();
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0007383C File Offset: 0x00071A3C
		private void OnEnable()
		{
			LaserEffect laserEffect = this.laserEffect;
			if (laserEffect == null)
			{
				return;
			}
			laserEffect.visualEffect.Reinit();
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00073854 File Offset: 0x00071A54
		private void CreateLaserEffect()
		{
			this.laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserPrefab, Vector3.zero, Quaternion.identity, base.transform);
			this.laserEffect.transform.localPosition = Vector2.zero;
			this.laserEffect.transform.localRotation = Quaternion.identity;
			this.laserEffect.Stop();
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000738BC File Offset: 0x00071ABC
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f && !this.target && !this.scannerFiring)
			{
				this.updateTimer = 2f;
				SpaceShip x = null;
				float num = 0f;
				foreach (SpaceShip spaceShip in BasePoiManager.current.GetComponentsInChildren<SpaceShip>())
				{
					if (!spaceShip.IsPlayerEnemy() && !this.scanned.Contains(spaceShip))
					{
						float num2 = Vector2.Distance(base.transform.position, spaceShip.transform.position);
						if (x == null || num > num2)
						{
							x = spaceShip;
							num = num2;
						}
					}
				}
				if (x != null)
				{
					this.target = x;
				}
			}
			if (this.target)
			{
				Vector3 normalized = (this.target.transform.position - base.transform.position).normalized;
				float z = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
				base.transform.rotation = Quaternion.Euler(0f, 0f, z);
				float num3 = Vector2.Distance(base.transform.position, this.target.transform.position);
				if (num3 < 3f && !this.scannerFiring)
				{
					base.StartCoroutine(this.DoScan(this.target));
					return;
				}
				if (num3 > 3f)
				{
					base.transform.position = Vector2.MoveTowards(base.transform.position, this.target.transform.position, Time.deltaTime * 2f);
				}
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00073AAC File Offset: 0x00071CAC
		private IEnumerator DoScan(SpaceShip target)
		{
			this.scannerFiring = true;
			this.scanned.Add(target);
			this.laserEffect.SetObjectsToTrack(base.gameObject, target.gameObject);
			this.laserEffect.Play();
			yield return new WaitForSeconds(2.5f);
			this.laserEffect.Stop();
			if (target && SeededRandom.Global.RandomBool(0.25f))
			{
				ValueTuple<InventoryItemType, int> valueTuple = this.CreateCargo(target);
				InventoryItemType item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				List<MissionReward> rewards = new List<MissionReward>
				{
					new Item
					{
						item = item,
						amount = item2
					}
				};
				target.spaceShipData.AddLoot(item, item2);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralCargoScannerFoundCargo", new object[]
				{
					FactionInfo.GetAbbreviation(target.faction) + " " + target.displayName
				})).WithColor(ColorHelper.umbralColor).WithMissionRewards(null, rewards).WithCustomTime(5f).Show();
				Register.AddCounter("CargoScannerHit", 1, 0);
				MissionObjective.Trigger(MissionTrigger.FindCargoWithScanner, null, null, false);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				UIInfoTextParent.instance.ShowWarningText("@UmbralCargoScannerNoCargo", base.transform, new Color?(Color.white));
			}
			this.target = null;
			this.scannerFiring = false;
			yield break;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00073AC4 File Offset: 0x00071CC4
		private ValueTuple<InventoryItemType, int> CreateCargo(SpaceShip target)
		{
			int creditsValue = GameMath.GetCreditsValue(80f, MapPointOfInterest.current.level);
			return SeededRandom.Global.Choose<UmbralCargoScannerBot.CargoCreator>(UmbralCargoScannerBot.cargoOdds)(MapPointOfInterest.current.level, creditsValue);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00073B08 File Offset: 0x00071D08
		private static ValueTuple<InventoryItemType, int> CreateEquipmentCargo(int level, int value)
		{
			SeededRandom global = SeededRandom.Global;
			Rarity rarity = Rarity.HighGrade;
			if (level >= 58 && global.RandomBool(0.05f))
			{
				rarity = Rarity.Legendary;
			}
			else if (global.RandomBool(0.15f))
			{
				rarity = Rarity.Exotic;
			}
			return new ValueTuple<InventoryItemType, int>(global.Choose<EquipmentBuilder>(EquipmentBuilder.GetItemsForGeneralShop(level)).CreateItemType(rarity, level, false, null, false, false), 1);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00073B60 File Offset: 0x00071D60
		private static ValueTuple<InventoryItemType, int> CreateTradeCargo(int level, int value)
		{
			List<InventoryItemType> list = new List<InventoryItemType>();
			list.AddRange(Economy.defaultTradeGoods[Rarity.HighGrade]);
			list.AddRange(Economy.defaultTradeGoods[Rarity.Exotic]);
			InventoryItemType inventoryItemType = SeededRandom.Global.Choose<InventoryItemType>(list);
			return new ValueTuple<InventoryItemType, int>(inventoryItemType, Mathf.CeilToInt((float)value / (float)inventoryItemType.cost));
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00073BB6 File Offset: 0x00071DB6
		private static ValueTuple<InventoryItemType, int> CreateSkillpointCargo(int level, int value)
		{
			return new ValueTuple<InventoryItemType, int>("BonusSkillPointTemplate", 1);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00073BC8 File Offset: 0x00071DC8
		private static ValueTuple<InventoryItemType, int> CreateSalvageCargo(int level, int value)
		{
			Dictionary<RefinedMaterial, float> dictionary = new Dictionary<RefinedMaterial, float>();
			foreach (RefinedMaterial refinedMaterial in (RefinedMaterial[])Enum.GetValues(typeof(RefinedMaterial)))
			{
				dictionary.Add(refinedMaterial, Mathf.Sqrt(refinedMaterial.GetValue()));
			}
			int tier = SalvageHelper.RollTier(level);
			InventoryItemType inventoryItemType = InventoryItemType.Get(SalvageHelper.BuildScrapItemName(SeededRandom.Global.Choose<RefinedMaterial>(dictionary).ToString(), tier));
			return new ValueTuple<InventoryItemType, int>(inventoryItemType, Mathf.CeilToInt((float)value / (float)inventoryItemType.cost));
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00073C60 File Offset: 0x00071E60
		private static ValueTuple<InventoryItemType, int> CreateOreCargo(int level, int value)
		{
			List<InventoryItemType> list = new List<InventoryItemType>();
			foreach (OreItemData oreItemData in OreItemData.regularOres)
			{
				if (oreItemData.item.itemLevel >= level - 10 && oreItemData.item.itemLevel <= level + 10)
				{
					list.Add(oreItemData.item);
				}
			}
			InventoryItemType inventoryItemType = SeededRandom.Global.Choose<InventoryItemType>(list);
			return new ValueTuple<InventoryItemType, int>(inventoryItemType, Mathf.CeilToInt((float)value / (float)inventoryItemType.cost));
		}

		// Token: 0x0400098C RID: 2444
		private static Dictionary<UmbralCargoScannerBot.CargoCreator, float> cargoOdds = new Dictionary<UmbralCargoScannerBot.CargoCreator, float>
		{
			{
				new UmbralCargoScannerBot.CargoCreator(UmbralCargoScannerBot.CreateEquipmentCargo),
				1f
			},
			{
				new UmbralCargoScannerBot.CargoCreator(UmbralCargoScannerBot.CreateTradeCargo),
				1f
			},
			{
				new UmbralCargoScannerBot.CargoCreator(UmbralCargoScannerBot.CreateSkillpointCargo),
				0.25f
			},
			{
				new UmbralCargoScannerBot.CargoCreator(UmbralCargoScannerBot.CreateSalvageCargo),
				1f
			},
			{
				new UmbralCargoScannerBot.CargoCreator(UmbralCargoScannerBot.CreateOreCargo),
				1f
			}
		};

		// Token: 0x0400098D RID: 2445
		[SerializeField]
		private LaserEffect laserPrefab;

		// Token: 0x0400098E RID: 2446
		private LaserEffect laserEffect;

		// Token: 0x0400098F RID: 2447
		private SpaceShip target;

		// Token: 0x04000990 RID: 2448
		private float updateTimer;

		// Token: 0x04000991 RID: 2449
		private HashSet<SpaceShip> scanned = new HashSet<SpaceShip>();

		// Token: 0x04000992 RID: 2450
		private bool scannerFiring;

		// Token: 0x020004F5 RID: 1269
		// (Invoke) Token: 0x06002AB3 RID: 10931
		private delegate ValueTuple<InventoryItemType, int> CargoCreator(int level, int value);
	}
}
