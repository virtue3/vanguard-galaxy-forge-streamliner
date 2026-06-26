using System;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Managers;
using Behaviour.Salvage;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Crew;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D8 RID: 984
	public class SalvageCreditDrone : TargetedPayload
	{
		// Token: 0x060025A5 RID: 9637 RVA: 0x000D1E5C File Offset: 0x000D005C
		private void Start()
		{
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			base.transform.SetParent(spaceShip.transform.parent);
			this.durationRemaining = this.duration;
			this.FindRandomHitLocation();
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000D1E9C File Offset: 0x000D009C
		private void Update()
		{
			this.durationRemaining -= Time.deltaTime;
			if (this.durationRemaining <= 0f || !this.target)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!this.latchedToWreck)
			{
				this.MoveToWreck();
				return;
			}
			this.retrievalTimer -= Time.deltaTime;
			if (this.retrievalTimer <= 0f)
			{
				this.GainItem();
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000D1F20 File Offset: 0x000D0120
		protected virtual void GainItem()
		{
			ItemBuilder itemBuilder = ItemBuilder.Get("Credits");
			int creditsValue = GameMath.GetCreditsValue(SeededRandom.Global.RandomRange(7.5f, 15f), SystemMapData.current.level);
			InventoryItemType item = itemBuilder.CreateCreditsItem(creditsValue);
			Singleton<LootManager>.Instance.CreateLootItem(base.transform.position, item, 1, Faction.player, false);
			this.target.data.creditsExtracted++;
			Register.AddCounter("SalvagingCreditsRetrieved", creditsValue, 0);
			GameplayManager.Instance.spaceShip.AddCrewExperience((float)(creditsValue / 50), new CommanderSpecialization?(CommanderSpecialization.Salvaging), true);
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x000D1FC4 File Offset: 0x000D01C4
		public override void SetTarget(GameObject target)
		{
			this.target = target.GetComponent<SalvageContainer>();
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000D1FD4 File Offset: 0x000D01D4
		private void MoveToWreck()
		{
			Vector3 normalized = (this.target.transform.position - base.transform.position).normalized;
			float z = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			base.transform.position += normalized * this.speed * Time.deltaTime;
			if (Vector3.Distance(base.transform.position, this.target.transform.position) <= 0.1f)
			{
				this.latchedToWreck = true;
				base.transform.SetParent(this.target.transform);
				base.transform.position = this.target.transform.position;
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000D20C8 File Offset: 0x000D02C8
		public void SetSalvageTarget(SalvageContainer wreck)
		{
			this.target = wreck;
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000D20D4 File Offset: 0x000D02D4
		private void FindRandomHitLocation()
		{
			Bounds bounds = this.target.surfaceCollider.bounds;
			int num = 0;
			Vector2 vector;
			bool flag;
			do
			{
				vector = new Vector2(SeededRandom.Global.RandomRange(bounds.min.x, bounds.max.x), SeededRandom.Global.RandomRange(bounds.min.y, bounds.max.y));
				flag = this.target.surfaceCollider.OverlapPoint(vector);
				num++;
			}
			while (!flag && num < 50);
			this.hitLocation = new GameObject();
			this.hitLocation.transform.parent = this.target.transform;
			this.hitLocation.transform.position = vector;
		}

		// Token: 0x040016E8 RID: 5864
		[SerializeField]
		private float duration;

		// Token: 0x040016E9 RID: 5865
		protected SalvageContainer target;

		// Token: 0x040016EA RID: 5866
		private float speed = 2f;

		// Token: 0x040016EB RID: 5867
		private bool latchedToWreck;

		// Token: 0x040016EC RID: 5868
		private GameObject hitLocation;

		// Token: 0x040016ED RID: 5869
		private float durationRemaining;

		// Token: 0x040016EE RID: 5870
		private float retrievalTimer = 4f;
	}
}
