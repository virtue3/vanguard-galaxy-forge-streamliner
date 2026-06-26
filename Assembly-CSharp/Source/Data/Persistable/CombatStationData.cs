using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Unit;
using LightJson;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000112 RID: 274
	public class CombatStationData : PersistableData
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0004EAA1 File Offset: 0x0004CCA1
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x0004EAC4 File Offset: 0x0004CCC4
		public bool playerFriendly
		{
			get
			{
				return this.parts.Count > 0 && this.parts[0].playerFriendly;
			}
			set
			{
				foreach (CombatStationPartData combatStationPartData in this.stationParts)
				{
					combatStationPartData.playerFriendly = value;
				}
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0004EB10 File Offset: 0x0004CD10
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0004EB34 File Offset: 0x0004CD34
		public bool playerHostile
		{
			get
			{
				return this.parts.Count > 0 && this.parts[0].playerHostile;
			}
			set
			{
				foreach (CombatStationPartData combatStationPartData in this.stationParts)
				{
					combatStationPartData.playerHostile = value;
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0004EB80 File Offset: 0x0004CD80
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0004EBA4 File Offset: 0x0004CDA4
		public bool alwaysHostile
		{
			get
			{
				return this.parts.Count > 0 && this.parts[0].alwaysHostile;
			}
			set
			{
				foreach (CombatStationPartData combatStationPartData in this.stationParts)
				{
					combatStationPartData.alwaysHostile = value;
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0004EBF0 File Offset: 0x0004CDF0
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0004EC14 File Offset: 0x0004CE14
		public bool noReputationLoss
		{
			get
			{
				return this.parts.Count > 0 && this.parts[0].noReputationLoss;
			}
			set
			{
				foreach (CombatStationPartData combatStationPartData in this.stationParts)
				{
					combatStationPartData.noReputationLoss = value;
				}
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0004EC60 File Offset: 0x0004CE60
		public override bool ShouldKeepPoiAlive()
		{
			return true;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0004EC63 File Offset: 0x0004CE63
		public IEnumerable<CombatStationPartData> stationParts
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0004EC6B File Offset: 0x0004CE6B
		public void AddPart(CombatStationPartData part)
		{
			this.parts.Add(part);
			part.positionData.position += this.position;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0004EC98 File Offset: 0x0004CE98
		public override void OffsetPosition(Vector2 diff)
		{
			base.OffsetPosition(diff);
			foreach (CombatStationPartData combatStationPartData in this.parts)
			{
				combatStationPartData.positionData.position += diff;
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0004ED00 File Offset: 0x0004CF00
		private void CleanupParts()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].currentHullHP <= 0f && this.parts[i].damageTaken > 0f)
				{
					this.parts.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0004ED64 File Offset: 0x0004CF64
		public void ConnectParts(CombatStationPartData part1, CombatStationConnector connector1, CombatStationPartData part2, CombatStationConnector connector2)
		{
			this.connections.Add(new ValueTuple<string, string>(part1.guid, part2.guid));
			Quaternion rotation = Quaternion.Euler(0f, 0f, part1.positionData.rotation);
			Vector2 a = part1.positionData.position + rotation * connector1.transform.position;
			float num = part1.positionData.rotation + connector1.transform.localEulerAngles.z;
			part2.positionData.rotation = num + 180f - connector2.transform.localEulerAngles.z;
			Quaternion rotation2 = Quaternion.Euler(0f, 0f, part2.positionData.rotation);
			part2.positionData.position = a - rotation2 * connector2.transform.position;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0004EE54 File Offset: 0x0004D054
		public void ConnectParts(int part1, int connector1, int part2, int connector2)
		{
			CombatStationPartData combatStationPartData = this.parts[part1];
			CombatStationPartData combatStationPartData2 = this.parts[part2];
			this.ConnectParts(combatStationPartData, combatStationPartData.connectors[connector1], combatStationPartData2, combatStationPartData2.connectors[connector2]);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0004EE9C File Offset: 0x0004D09C
		public CombatStationPartData GetPart(CombatStationPartType type)
		{
			foreach (CombatStationPartData combatStationPartData in this.parts)
			{
				if (combatStationPartData.partPrefab.partType == type)
				{
					return combatStationPartData;
				}
			}
			return null;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0004EF00 File Offset: 0x0004D100
		public void AddLoot(InventoryItemType iit, int count = 1)
		{
			List<CombatStationPartData> list = new List<CombatStationPartData>();
			foreach (CombatStationPartData combatStationPartData in this.parts)
			{
				if (combatStationPartData.partPrefab.partType != CombatStationPartType.Connector)
				{
					list.Add(combatStationPartData);
				}
			}
			SeededRandom.Global.Choose<CombatStationPartData>(list).AddLoot(iit, count);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0004EF7C File Offset: 0x0004D17C
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			GameObject gameObject = null;
			Dictionary<string, AbstractUnit> dictionary = new Dictionary<string, AbstractUnit>();
			this.CleanupParts();
			foreach (CombatStationPartData combatStationPartData in this.parts)
			{
				AbstractUnit abstractUnit = parent.AddToWorld(combatStationPartData, null, false);
				abstractUnit.AddRigidBody(RigidbodyType2D.Dynamic);
				dictionary[combatStationPartData.guid] = abstractUnit;
				if (gameObject == null)
				{
					gameObject = abstractUnit.gameObject;
				}
			}
			foreach (ValueTuple<string, string> valueTuple in this.connections)
			{
				if (dictionary.ContainsKey(valueTuple.Item1) && dictionary.ContainsKey(valueTuple.Item2))
				{
					FixedJoint2D fixedJoint2D = dictionary[valueTuple.Item1].gameObject.AddComponent<FixedJoint2D>();
					fixedJoint2D.connectedBody = dictionary[valueTuple.Item2].rigidbody;
					((CombatStationPart)dictionary[valueTuple.Item2]).connectionJoint = fixedJoint2D;
				}
			}
			foreach (AbstractUnit abstractUnit2 in dictionary.Values)
			{
				abstractUnit2.surfaceCollider.enabled = true;
			}
			return gameObject;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0004F0F8 File Offset: 0x0004D2F8
		public override void DataToJson(JsonObject json)
		{
			this.CleanupParts();
			json["parts"] = this.parts.ToJsonArray<CombatStationPartData>();
			JsonArray jsonArray = new JsonArray();
			foreach (ValueTuple<string, string> valueTuple in this.connections)
			{
				jsonArray.Add(new JsonArray
				{
					valueTuple.Item1,
					valueTuple.Item2
				});
			}
			json["connections"] = jsonArray;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0004F1B4 File Offset: 0x0004D3B4
		public override void LoadFromJson(JsonObject json)
		{
			this.parts.FromJsonArray(json["parts"], new ClassExtensions.ParseJsonValue<CombatStationPartData>(CombatStationPartData.FromJson));
			foreach (JsonValue jsonValue in json["connections"].AsJsonArray)
			{
				this.connections.Add(new ValueTuple<string, string>(jsonValue[0], jsonValue[1]));
			}
		}

		// Token: 0x040005B0 RID: 1456
		private List<CombatStationPartData> parts = new List<CombatStationPartData>();

		// Token: 0x040005B1 RID: 1457
		private List<ValueTuple<string, string>> connections = new List<ValueTuple<string, string>>();
	}
}
