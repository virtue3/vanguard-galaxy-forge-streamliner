using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Effects;
using Behaviour.Equipment;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Data.Persistable;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Salvage
{
	// Token: 0x020002F0 RID: 752
	public class SalvageContainer : TargetableUnit, ITooltipTitleSource, ITooltipCustomSource
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x000A71F7 File Offset: 0x000A53F7
		public string displayName
		{
			get
			{
				return this.shipBase.displayName;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x000A7204 File Offset: 0x000A5404
		// (set) Token: 0x06001B6E RID: 7022 RVA: 0x000A720C File Offset: 0x000A540C
		public SpaceShip shipBase { get; private set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x000A7215 File Offset: 0x000A5415
		public override int currentSurfaceHealth
		{
			get
			{
				return this.data.surfaceHealth;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001B70 RID: 7024 RVA: 0x000A7222 File Offset: 0x000A5422
		public override int maxSurfaceHealth
		{
			get
			{
				return this.data.maxHealth;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x000A722F File Offset: 0x000A542F
		public override int currentCoreHealth
		{
			get
			{
				return this.data.structureHealth;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x000A723C File Offset: 0x000A543C
		public override int maxCoreHealth
		{
			get
			{
				return this.data.maxStructuralHealth;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001B73 RID: 7027 RVA: 0x000A7249 File Offset: 0x000A5449
		public override float mass
		{
			get
			{
				return this.shipBase.mass;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x000A7256 File Offset: 0x000A5456
		public override string targetName
		{
			get
			{
				return this.GetTooltipTitle();
			}
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x000A7260 File Offset: 0x000A5460
		protected override void Awake()
		{
			this.shipBase = base.GetComponent<SpaceShip>();
			this.shipBase.AddRigidBody(RigidbodyType2D.Dynamic);
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Asteroid"));
			base.gameObject.AddComponent<TooltipSource>();
			SpaceShipHardpoint[] componentsInChildren = base.GetComponentsInChildren<SpaceShipHardpoint>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
			SpriteRenderer[] componentsInChildren2 = base.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].color *= new Color(0.75f, 0.75f, 0.75f, 1f);
			}
			base.Awake();
			this.isSalvage = true;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x000A7316 File Offset: 0x000A5516
		protected override void Start()
		{
			base.Start();
			this.data.InitHealth();
			base.StartCoroutine(this.WaitForBattleDamage());
			base.transform.Z(ZIndex.Salvage);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x000A7342 File Offset: 0x000A5542
		public IEnumerator WaitForBattleDamage()
		{
			yield return new WaitForSeconds(2f);
			TargetableUnit.UpdateCollider(this.shipBase.surfaceCollider, this.shipBase.surfaceSprite.sprite, true);
			this.pixelsSurface = AsteroidHelper.GetFilledPixelCount(this.shipBase.surfaceSprite.sprite);
			this.pixelsStructure = AsteroidHelper.GetFilledPixelCount(this.shipBase.structureSprite);
			if (this.data.showOutline && this.HasSalvage(TargetLayer.Both))
			{
				this.SetSalvageEffect();
			}
			this.shipBase.SetDimensions();
			yield break;
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x000A7351 File Offset: 0x000A5551
		private void SetSalvageEffect()
		{
			this.salvageEffect = UnityEngine.Object.Instantiate<SalvageEffect>(Singleton<EffectManager>.Instance.salvageEffect, base.transform);
			this.UpdateSalvageEffectProperties();
			this.salvageEffect.Play();
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000A7380 File Offset: 0x000A5580
		private void UpdateSalvageEffectProperties()
		{
			this.salvageEffect.SetMainTexture(this.shipBase.surfaceSprite.sprite.texture);
			this.salvageEffect.SetScaleFactor(Vector2.one / 5f);
			this.salvageEffect.SetColor(this.GetMaxRarityColor());
			this.salvageEffect.SetTime(1f, false);
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000A73E9 File Offset: 0x000A55E9
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return turret is AbstractSalvageTurret && this.HasSalvage(turret.targetLayer);
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000A7406 File Offset: 0x000A5606
		private bool HasSalvage(TargetLayer targetLayer)
		{
			return this.data.HasSalvage(targetLayer);
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000A741C File Offset: 0x000A561C
		private Color GetMaxRarityColor()
		{
			Rarity rarity = Rarity.Standard;
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.data.scrapContent)
			{
				if (keyValuePair.Value > 0 && keyValuePair.Key.rarity > rarity)
				{
					rarity = keyValuePair.Key.rarity;
				}
			}
			foreach (SalvageItemData salvageItemData in this.data.itemContent)
			{
				if (salvageItemData.item.rarity > rarity)
				{
					rarity = salvageItemData.item.rarity;
				}
			}
			return rarity.GetColor();
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000A74F8 File Offset: 0x000A56F8
		public Collider2D GetSurfaceCollider()
		{
			return base.surfaceCollider;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x000A7500 File Offset: 0x000A5700
		public Vector2 GetTargetablePosition()
		{
			return base.transform.position;
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000A7512 File Offset: 0x000A5712
		public Vector2 GetTargetableVelocity()
		{
			return base.rigidbody.linearVelocity;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000A7520 File Offset: 0x000A5720
		private void OnMouseUpAsButton()
		{
			if (!UIHelper.clickTargetingAvailable)
			{
				return;
			}
			if (GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Both))
			{
				GameplayManager.Instance.spaceShip.SetManualTarget(this);
			}
			base.OnTargetedHandling();
			if (this.data.itemContent.Count > 0)
			{
				HudManager.Instance.ToggleSalvageWindow(this, this.data);
			}
			MissionObjective.Trigger(MissionTrigger.TargetWreckage, null, null, false);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000A758C File Offset: 0x000A578C
		public override void TakeDamage(DamageData amount)
		{
			if (amount.sourceUnit)
			{
				base.AddTargetedBy(amount.sourceUnit, 0f);
			}
			if (amount.sourceUnit == GameplayManager.Instance.spaceShip)
			{
				BasePoiManager.current.PlayerSetMissionHostile();
			}
			UIInfoTextParent.instance.ShowDamageNumber(amount, base.transform);
			amount.targetUnit = this;
			amount.PreDamageEvents();
			if (amount.sourceUnit.IsPlayer(false))
			{
				this.hitByPlayer = true;
			}
			Sprite sprite = this.shipBase.surfaceSprite.sprite;
			bool flag = false;
			bool flag2 = this.data.HasStructuralContent() || this.data.HasScrap();
			if (this.data.TakeDamage(amount, this.hitByPlayer))
			{
				if (flag2)
				{
					float num = (float)((amount.targetLayer == TargetLayer.Core) ? this.data.structuralDamageBeforeChunk : this.data.scrapDamageBeforeChunk) / (float)((amount.targetLayer == TargetLayer.Core) ? this.data.maxStructuralHealth : this.data.maxHealth);
					int num2 = (int)(SeededRandom.Global.RandomRange(num / 1.5f, num / 1.1f) * (float)((amount.targetLayer == TargetLayer.Core) ? this.pixelsSurface : this.pixelsStructure));
					num2 = Mathf.Clamp(num2, 5, 180);
					this.shipBase.BreakSpriteOnDamage(sprite, amount.hitTransform.localPosition, num2, amount.targetLayer == TargetLayer.Core, true);
				}
				if (!this.data.HasSalvage(TargetLayer.Both))
				{
					flag = true;
				}
				else if (this.data.showOutline)
				{
					this.UpdateSalvageEffectProperties();
				}
				amount.PostDamageEvents();
			}
			if (flag)
			{
				int breakAmount = AsteroidHelper.GetFilledPixelCount(this.shipBase.structureSprite) / 2;
				SpriteBreakPoint leftoverBreakPoint = this.shipBase.BreakSpriteOnDamage(this.shipBase.surfaceSprite.sprite, amount.hitTransform.localPosition, breakAmount, true, true);
				BasePoiManager.current.poi.RemovePersistable(this.data);
				leftoverBreakPoint.surfaceDelayedSprite.onComplete = delegate()
				{
					SpriteRenderer spriteRenderer = this.shipBase.CreateSpriteChunk(sprite, false);
					TargetableUnit.UpdateCollider(spriteRenderer.GetComponent<PolygonCollider2D>(), sprite, false);
					Collider2D component = spriteRenderer.GetComponent<Collider2D>();
					SpriteRenderer spriteRenderer2 = this.shipBase.CreateSpriteChunk(this.shipBase.structureSprite, true);
					TargetableUnit.UpdateCollider(spriteRenderer2.GetComponent<PolygonCollider2D>(), this.shipBase.structureSprite, false);
					Collider2D component2 = spriteRenderer2.GetComponent<Collider2D>();
					Physics2D.IgnoreCollision(component, component2);
					PolygonCollider2D spawnedChunkCollider = leftoverBreakPoint.surfaceDelayedSprite.spawnedChunkCollider;
					if (spawnedChunkCollider)
					{
						Physics2D.IgnoreCollision(spawnedChunkCollider, component);
						Physics2D.IgnoreCollision(spawnedChunkCollider, component2);
					}
					this.IgnoreOverlappingColliders(component);
					this.IgnoreOverlappingColliders(component2);
					this.AddEffectsForSpawnedChunk(leftoverBreakPoint.position, spriteRenderer.gameObject, component, 1f);
					this.AddEffectsForSpawnedChunk(leftoverBreakPoint.position, spriteRenderer2.gameObject, component2, 1f);
					UnityEngine.Object.Destroy(this.gameObject);
				};
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000A77C8 File Offset: 0x000A59C8
		private void IgnoreOverlappingColliders(Collider2D collider)
		{
			List<Collider2D> list = new List<Collider2D>();
			Physics2D.OverlapCollider(collider, list);
			foreach (Collider2D collider2 in list)
			{
				Physics2D.IgnoreCollision(collider2, collider);
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000A7824 File Offset: 0x000A5A24
		public string GetTooltipTitle()
		{
			return Translation.Translate("@SalvageContainer", new object[]
			{
				this.shipBase.displayName
			});
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000A7844 File Offset: 0x000A5A44
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine("@SalvageItems", 12, 8f);
			bool flag = false;
			if (this.data.availableItemContent.Count > 0)
			{
				using (List<SalvageItemData>.Enumerator enumerator = this.data.availableItemContent.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SalvageItemData salvageItemData = enumerator.Current;
						AbstractEquipment component = salvageItemData.item.GetComponent<AbstractEquipment>();
						string str = "";
						if (component != null)
						{
							str = " [" + Translation.Translate(component.size.GetShortDisplayName(), Array.Empty<object>()) + "]";
						}
						tooltip.AddTextLine(Translation.Translate(salvageItemData.item.displayName, Array.Empty<object>()) + str, 12, 8f).Text.color = salvageItemData.item.rarity.GetColor();
						flag = true;
					}
					goto IL_116;
				}
			}
			tooltip.AddTextLine("@SalvageEmptyItems", 12, 8f).Text.color = Rarity.Standard.GetColor();
			IL_116:
			int unreachableItemCount = this.data.GetUnreachableItemCount();
			if (unreachableItemCount > 0)
			{
				string text = (unreachableItemCount == 1) ? Translation.Translate("@SalvageUnreachableItem", Array.Empty<object>()) : Translation.Translate("@SalvageUnreachableItems", new object[]
				{
					unreachableItemCount
				});
				tooltip.AddTextLine(text, 12, 8f).Text.color = ColorHelper.reddish;
			}
			tooltip.AddTextLine("", 12, 8f);
			tooltip.AddTextLine("@SalvageScrap", 12, 8f);
			bool flag2 = false;
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.data.scrapContent)
			{
				if (keyValuePair.Value > 0)
				{
					tooltip.AddTextLine(keyValuePair.Key.displayName, 12, 8f).Text.color = keyValuePair.Key.rarity.GetColor();
					flag2 = true;
				}
			}
			if (!flag2)
			{
				tooltip.AddTextLine("@SalvageEmptyScrap", 12, 8f).Text.color = Rarity.Standard.GetColor();
			}
			else
			{
				base.AddTooltipHealthbar(tooltip, TargetLayer.Surface, false);
			}
			tooltip.AddTextLine("", 12, 8f);
			tooltip.AddTextLine("@SalvageStructure", 12, 8f);
			bool flag3 = false;
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair2 in this.data.structuralContent)
			{
				if (keyValuePair2.Value > 0)
				{
					tooltip.AddTextLine(keyValuePair2.Key.displayName, 12, 8f).Text.color = keyValuePair2.Key.rarity.GetColor();
					flag3 = true;
				}
			}
			if (!flag3)
			{
				tooltip.AddTextLine("@SalvageEmptyScrap", 12, 8f).Text.color = Rarity.Standard.GetColor();
			}
			else
			{
				base.AddTooltipHealthbar(tooltip, TargetLayer.Core, false);
			}
			if (flag2 && !flag3 && !GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Surface))
			{
				tooltip.AddTextLine(Translation.Translate("@SalvageNoTurretsSurface", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if (flag3 && !flag2 && !GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Core))
			{
				tooltip.AddTextLine(Translation.Translate("@SalvageNoTurretsStructure", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if (flag && !GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Both))
			{
				tooltip.AddTextLine(Translation.Translate("@SalvageNoTurrets", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
			}
		}

		// Token: 0x04001141 RID: 4417
		public SalvageData data;

		// Token: 0x04001143 RID: 4419
		private SalvageEffect salvageEffect;

		// Token: 0x04001144 RID: 4420
		private int pixelsSurface;

		// Token: 0x04001145 RID: 4421
		private int pixelsStructure;
	}
}
