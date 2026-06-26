using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Persistables
{
	// Token: 0x020002F6 RID: 758
	public class LootContainer : TargetableUnit, ITooltipCustomSource, IDamageable
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x000A8176 File Offset: 0x000A6376
		public override bool damagableByAll
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x000A8179 File Offset: 0x000A6379
		public override string targetName
		{
			get
			{
				return "@LootContainer";
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000A8180 File Offset: 0x000A6380
		protected override void Start()
		{
			base.Start();
			if (this.sr == null)
			{
				this.sr = base.GetComponent<SpriteRenderer>();
			}
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000A81A4 File Offset: 0x000A63A4
		protected override void Update()
		{
			base.Update();
			if (this.data.loot.Count > 0)
			{
				this.light.enabled = true;
				this.light.transform.eulerAngles = new Vector3(0f, 0f, Time.time * 200f);
				return;
			}
			this.light.enabled = false;
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000A820D File Offset: 0x000A640D
		public bool IsEnemy(AbstractUnit target)
		{
			return true;
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000A8210 File Offset: 0x000A6410
		public void InitObject(LootContainerData data)
		{
			this.data = data;
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000A821C File Offset: 0x000A641C
		private void OnCollisionEnter2D(Collision2D collision)
		{
			SpaceShip component = collision.gameObject.GetComponent<SpaceShip>();
			if (component != null && MapPointOfInterest.current != null)
			{
				this.data.damageTaken = this.data.maxHealth;
				if (this.data.damageTaken >= this.data.maxHealth)
				{
					if (!this.damageTaken)
					{
						DamageData damageData = new DamageData(base.gameObject)
						{
							power = 4f,
							hitCoordinates = component.transform.position
						};
						component.TakeDamage(damageData);
						this.damageTaken = true;
					}
					this.CrateOpened();
				}
			}
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x000A82BC File Offset: 0x000A64BC
		private void CrateOpened()
		{
			if (base.isDestroyed)
			{
				return;
			}
			base.isDestroyed = true;
			this.DropLoot();
			BasePoiManager.current.poi.RemovePersistable(this.data);
			if (this.fadeCoroutine == null)
			{
				this.fadeCoroutine = base.StartCoroutine(this.FadeAndDestroyCoroutine());
			}
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			foreach (AbstractTargetingModule abstractTargetingModule in new AbstractTargetingModule[]
			{
				spaceShip.GetModule<MiningModule>(),
				spaceShip.GetModule<CombatModule>(),
				spaceShip.GetModule<SalvageModule>()
			})
			{
				if (((abstractTargetingModule != null) ? abstractTargetingModule.manualTarget : null) == this)
				{
					abstractTargetingModule.ResetCurrentTargets();
				}
			}
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000A8368 File Offset: 0x000A6568
		private void DropLoot()
		{
			LootManager instance = Singleton<LootManager>.Instance;
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.data.loot)
			{
				instance.CreateLootItem(base.transform.position, keyValuePair.Key, keyValuePair.Value, Faction.player, false);
			}
			MissionObjective.Trigger(MissionTrigger.LootContainerOpened, this.data, null, false);
			this.data.loot.Clear();
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000A8408 File Offset: 0x000A6608
		private IEnumerator FadeAndDestroyCoroutine()
		{
			yield return new WaitForSeconds(10f);
			float elapsedTime = 0f;
			while (elapsedTime < 0.5f)
			{
				elapsedTime += Time.deltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / 0.5f);
				this.SetAlpha(alpha);
				yield return null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000A8417 File Offset: 0x000A6617
		private void SetAlpha(float t)
		{
			this.sr.color = this.sr.color.WithAlpha(t);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000A8438 File Offset: 0x000A6638
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate(this.data.name, Array.Empty<object>()), 16, 8f).Text.color = ColorHelper.detailsColor;
			if (this.data.loot.Count <= 0)
			{
				tooltip.AddTextLine(Translation.Translate("@LCEmpty", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
				return;
			}
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.data.loot)
			{
				tooltip.AddTextLine(string.Format("{0} x{1}", Translation.Translate(keyValuePair.Key.displayName, Array.Empty<object>()), keyValuePair.Value), 12, 8f).Text.color = keyValuePair.Key.rarity.GetColor();
			}
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000A8550 File Offset: 0x000A6750
		private void OnMouseDown()
		{
			if (!UIHelper.clickTargetingAvailable)
			{
				return;
			}
			GameplayManager.Instance.spaceShip.SetManualTarget(this);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x000A856A File Offset: 0x000A676A
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return true;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x000A856D File Offset: 0x000A676D
		public override void TakeDamage(DamageData damageData)
		{
			this.data.damageTaken += Mathf.CeilToInt(damageData.damageAmount);
			if (this.data.damageTaken >= this.data.maxHealth)
			{
				this.CrateOpened();
			}
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000A85B2 File Offset: 0x000A67B2
		bool IDamageable.enabled => base.enabled;

		// Token: 0x04001154 RID: 4436
		private LootContainerData data;

		// Token: 0x04001155 RID: 4437
		private bool damageTaken;

		// Token: 0x04001156 RID: 4438
		[SerializeField]
		private SpriteRenderer sr;

		// Token: 0x04001157 RID: 4439
		[SerializeField]
		private Light2D light;

		// Token: 0x04001158 RID: 4440
		private Coroutine fadeCoroutine;
	}
}
