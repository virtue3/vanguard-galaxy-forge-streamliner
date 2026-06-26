using System;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Unit;
using LightJson;

namespace Source.Data
{
	// Token: 0x0200010A RID: 266
	public class DefensiveTurretData : AbstractUnitData
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0004DD66 File Offset: 0x0004BF66
		public new DefensiveTurret unitDefinition
		{
			get
			{
				return this.unitDefinition as DefensiveTurret;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0004DD73 File Offset: 0x0004BF73
		public override string type
		{
			get
			{
				return "Turret";
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0004DD7A File Offset: 0x0004BF7A
		public AbstractTurret turretPrefab
		{
			get
			{
				EquipmentBuilder turretBuilder = this.unitDefinition.turretBuilder;
				if (turretBuilder == null)
				{
					return null;
				}
				return turretBuilder.prefab.GetComponent<AbstractTurret>();
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0004DD97 File Offset: 0x0004BF97
		public DefensiveTurretData(string guid, DefensiveTurret cls) : this(cls)
		{
			base.guid = guid;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0004DDA8 File Offset: 0x0004BFA8
		public DefensiveTurretData(DefensiveTurret cls)
		{
			base.guid = Guid.NewGuid().ToString();
			this.unitDefinition = cls;
			base.Initialize(base.guid, cls);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0004DDE8 File Offset: 0x0004BFE8
		protected override EquipmentBuilder GetHardpointDefaultEquipment(SpaceShipHardpoint hardpoint)
		{
			return this.unitDefinition.turretBuilder;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0004DDF5 File Offset: 0x0004BFF5
		internal void SetTurretItem(InventoryItemType item)
		{
			this.turretItem = item;
			base.level = item.itemLevel;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0004DE0C File Offset: 0x0004C00C
		public override JsonValue ToJson()
		{
			JsonObject jsonObject = base.ToJson();
			jsonObject["turret"] = this.unitDefinition.identifier;
			string key = "turretItem";
			InventoryItemType inventoryItemType = this.turretItem;
			jsonObject[key] = ((inventoryItemType != null) ? inventoryItemType.ToJson() : JsonValue.Null);
			return jsonObject;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0004DE68 File Offset: 0x0004C068
		public new static DefensiveTurretData FromJson(JsonValue json)
		{
			DefensiveTurretData defensiveTurretData = new DefensiveTurretData(json["turret"].AsString);
			if (json["turretItem"] != JsonValue.Null)
			{
				defensiveTurretData.turretItem = InventoryItemType.FromJson(json["turretItem"]);
			}
			AbstractUnitData.FromJson(defensiveTurretData, json);
			return defensiveTurretData;
		}

		// Token: 0x04000588 RID: 1416
		public InventoryItemType turretItem;
	}
}
