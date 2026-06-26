using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Behaviour.Equipment.Aspect;
using Behaviour.Item;
using Behaviour.Unit;
using LightJson;
using Source.Item;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment
{
	// Token: 0x0200033C RID: 828
	public abstract class AbstractEquipment : InventoryItemPart, IEquipStatSource
	{
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000BA7F9 File Offset: 0x000B89F9
		// (set) Token: 0x06001F51 RID: 8017 RVA: 0x000BA801 File Offset: 0x000B8A01
		public string typeDisplayName { get; protected set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000BA80A File Offset: 0x000B8A0A
		public float energyDraw
		{
			get
			{
				return this.capacityCost;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000BA812 File Offset: 0x000B8A12
		// (set) Token: 0x06001F54 RID: 8020 RVA: 0x000BA81A File Offset: 0x000B8A1A
		public List<EquipStatLine> stats { get; private set; } = new List<EquipStatLine>();

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x000BA823 File Offset: 0x000B8A23
		// (set) Token: 0x06001F56 RID: 8022 RVA: 0x000BA82B File Offset: 0x000B8A2B
		public List<AspectSlot> aspectSlots { get; private set; } = new List<AspectSlot>();

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001F57 RID: 8023
		public abstract EquipmentSlot slot { get; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000BA834 File Offset: 0x000B8A34
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x000BA83C File Offset: 0x000B8A3C
		public AbstractUnit parent { get; protected set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x000BA845 File Offset: 0x000B8A45
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x000BA84D File Offset: 0x000B8A4D
		public bool active { get; protected set; } = true;

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x000BA856 File Offset: 0x000B8A56
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x000BA85E File Offset: 0x000B8A5E
		public bool isCrafted { get; protected set; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001F5E RID: 8030 RVA: 0x000BA867 File Offset: 0x000B8A67
		// (set) Token: 0x06001F5F RID: 8031 RVA: 0x000BA86F File Offset: 0x000B8A6F
		public int salvageWorkShopSpent { get; protected set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x000BA878 File Offset: 0x000B8A78
		// (set) Token: 0x06001F61 RID: 8033 RVA: 0x000BA880 File Offset: 0x000B8A80
		public float salvageWorkShopPenalty { get; protected set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x000BA889 File Offset: 0x000B8A89
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x000BA891 File Offset: 0x000B8A91
		public int salvageWorkShopItemChangedAmount { get; protected set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x000BA89A File Offset: 0x000B8A9A
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x000BA8A2 File Offset: 0x000B8AA2
		public bool isPlayerEquipment { get; private set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x000BA8AB File Offset: 0x000B8AAB
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x000BA8B3 File Offset: 0x000B8AB3
		public bool isPlayerAsset { get; private set; }

		// Token: 0x06001F68 RID: 8040 RVA: 0x000BA8BC File Offset: 0x000B8ABC
		protected virtual void Awake()
		{
			this.parent = base.GetComponentInParent<AbstractUnit>();
			if (this.parent == null)
			{
				Debug.LogError("Parent AbstractUnit not found!");
			}
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000BA8E4 File Offset: 0x000B8AE4
		protected virtual void Start()
		{
			this.SetMainSubStats();
			this.SetIsPlayer();
			foreach (TurretBoostStat turretBoostStat in base.GetComponentsInChildren<TurretBoostStat>())
			{
				if (this.turretStats == null)
				{
					this.turretStats = new List<EquipStatLine>();
				}
				this.turretStats.AddRange(turretBoostStat.GetStats(1));
			}
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000BA93B File Offset: 0x000B8B3B
		protected virtual void Update()
		{
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000BA93D File Offset: 0x000B8B3D
		private void SetIsPlayer()
		{
			if (this.parent.IsPlayer(true))
			{
				this.isPlayerEquipment = true;
			}
			if (this.parent.IsPlayer(false))
			{
				this.isPlayerAsset = true;
			}
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000BA969 File Offset: 0x000B8B69
		public bool IsPlayer(bool spaceshipOnly = true)
		{
			if (spaceshipOnly)
			{
				return this.isPlayerEquipment;
			}
			return this.isPlayerAsset;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x000BA97B File Offset: 0x000B8B7B
		public virtual void Activate()
		{
			this.active = true;
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000BA984 File Offset: 0x000B8B84
		public virtual void Deactivate()
		{
			this.active = false;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x000BA98D File Offset: 0x000B8B8D
		public virtual void ToggleActive()
		{
			this.active = !this.active;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000BA99E File Offset: 0x000B8B9E
		public void SetCrafted(bool crafted = true)
		{
			this.isCrafted = crafted;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000BA9A7 File Offset: 0x000B8BA7
		public void AddSalvageCreditsSpent(int amount)
		{
			this.salvageWorkShopSpent += amount;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000BA9B7 File Offset: 0x000B8BB7
		public void AddItemChanged(int amount)
		{
			this.salvageWorkShopItemChangedAmount += amount;
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x000BA9C7 File Offset: 0x000B8BC7
		public void AddWorkshopPenalty(float weight)
		{
			this.salvageWorkShopPenalty += weight;
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000BA9D8 File Offset: 0x000B8BD8
		public void SetupInWorld()
		{
			foreach (AspectSlot aspectSlot in this.aspectSlots)
			{
				if (aspectSlot.equipAspect != null)
				{
					UnityEngine.Object.Instantiate<EquipAspect>(aspectSlot.equipAspect, base.transform);
				}
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x000BAA44 File Offset: 0x000B8C44
		public EquipStatLine? GetStatLine(EquipStat stat)
		{
			foreach (EquipStatLine equipStatLine in this.stats)
			{
				if (equipStatLine.stat == stat)
				{
					return new EquipStatLine?(equipStatLine);
				}
			}
			return null;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000BAAB0 File Offset: 0x000B8CB0
		public string GetName()
		{
			return this.typeDisplayName;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000BAAB8 File Offset: 0x000B8CB8
		public void InitializeEquipment()
		{
			foreach (AspectSlot aspectSlot in this.aspectSlots)
			{
				if (!(aspectSlot.equipAspect == null))
				{
					aspectSlot.equipAspect.Initialize(this);
				}
			}
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x000BAB20 File Offset: 0x000B8D20
		public void AddAspectSlot()
		{
			AspectSlot item = new AspectSlot(null, this.aspectSlots.Count);
			this.aspectSlots.Add(item);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x000BAB4B File Offset: 0x000B8D4B
		public void SetStats(List<EquipStatLine> equipStats)
		{
			this.stats = new List<EquipStatLine>(equipStats);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x000BAB59 File Offset: 0x000B8D59
		public void SetAspectSlots(List<AspectSlot> aspects)
		{
			this.aspectSlots = new List<AspectSlot>(aspects);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x000BAB67 File Offset: 0x000B8D67
		public void ResetDynamicFields()
		{
			this.dynamicFields.Clear();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x000BAB74 File Offset: 0x000B8D74
		public void SetDynamicField(string field, float value)
		{
			FieldInfo fieldInfo = this.FindDynamicField(base.GetType(), field);
			if (fieldInfo == null)
			{
				Debug.LogWarning("ReflectField not found in " + base.GetType().Name + ": " + field);
			}
			else if (fieldInfo.FieldType == typeof(int))
			{
				fieldInfo.SetValue(this, Mathf.RoundToInt(value));
			}
			else
			{
				if (!(fieldInfo.FieldType == typeof(float)))
				{
					throw new NotImplementedException("Field in EquipmentBuilder kan niet worden ingesteld: " + field);
				}
				fieldInfo.SetValue(this, value);
			}
			this.dynamicFields[field] = value;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x000BAC29 File Offset: 0x000B8E29
		private FieldInfo FindDynamicField(Type t, string fieldName)
		{
			if (t == null)
			{
				return null;
			}
			return (t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? t.GetField("_" + fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) ?? this.FindDynamicField(t.BaseType, fieldName);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x000BAC67 File Offset: 0x000B8E67
		public virtual IEnumerable<EquipStatLine> GetStats()
		{
			return this.stats;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000BAC6F File Offset: 0x000B8E6F
		public virtual IEnumerable<ValueTuple<EquipStatLine, int>> GetStatsWithIndex(bool includeMainStat = true)
		{
			MainStat mainStat = this.GetMainStat();
			int i = 0;
			while (i < this.stats.Count)
			{
				EquipStatLine equipStatLine = this.stats[i];
				if (includeMainStat || !(equipStatLine.stat.GetDisplayName() == mainStat.mainStatName))
				{
					goto IL_E0;
				}
				if (equipStatLine.multiplier != 1f)
				{
					string b = mainStat.mainStatAmount + " " + Translation.Translate(mainStat.mainStatName, Array.Empty<object>());
					if (!(equipStatLine.ToReadableString(true) == b))
					{
						goto IL_E0;
					}
				}
				else if (!(GameMath.FormatNumber(equipStatLine.amount, -1) == mainStat.mainStatAmount))
				{
					goto IL_E0;
				}
				IL_102:
				int num = i;
				i = num + 1;
				continue;
				IL_E0:
				yield return new ValueTuple<EquipStatLine, int>(equipStatLine, i);
				goto IL_102;
			}
			yield break;
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000BAC88 File Offset: 0x000B8E88
		public virtual void DataFromJson(JsonObject data)
		{
			this.stats.FromJsonArray(data["stats"], new ClassExtensions.ParseJsonValue<EquipStatLine>(EquipStatLine.FromJson));
			if (!data["aspectSlots"].IsNull)
			{
				this.aspectSlots.FromJsonArray(data["aspectSlots"], new ClassExtensions.ParseJsonValue<AspectSlot>(AspectSlot.FromJson));
			}
			else
			{
				int num = 0;
				foreach (JsonValue jsonValue in data["aspects"].AsJsonArray)
				{
					EquipAspect equipAspect = EquipAspect.Get(jsonValue);
					this.aspectSlots.Add(new AspectSlot(equipAspect, num));
					num++;
				}
			}
			foreach (KeyValuePair<string, JsonValue> keyValuePair in data["dynamicFields"].AsJsonObject)
			{
				this.SetDynamicField(keyValuePair.Key, (float)keyValuePair.Value.AsNumber);
			}
			if (!data["salworcr"].IsNull)
			{
				this.salvageWorkShopSpent = data["salworcr"];
			}
			if (!data["salworpen"].IsNull)
			{
				this.salvageWorkShopPenalty = (float)data["salworpen"];
			}
			if (!data["salworchanam"].IsNull)
			{
				this.salvageWorkShopItemChangedAmount = data["salworchanam"];
			}
			if (!data["crafted"].IsNull)
			{
				this.isCrafted = data["crafted"].AsBoolean;
			}
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x000BAE78 File Offset: 0x000B9078
		public virtual void DataToJson(JsonObject data)
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<string, float> keyValuePair in this.dynamicFields)
			{
				jsonObject[keyValuePair.Key] = new double?((double)keyValuePair.Value);
			}
			data["stats"] = this.stats.ToJsonArray<EquipStatLine>();
			data["aspectSlots"] = this.aspectSlots.ToJsonArray<AspectSlot>();
			data["dynamicFields"] = jsonObject;
			if (this.salvageWorkShopSpent != 0)
			{
				data["salworcr"] = new double?((double)this.salvageWorkShopSpent);
			}
			if (this.salvageWorkShopPenalty != 0f)
			{
				data["salworpen"] = new double?((double)this.salvageWorkShopPenalty);
			}
			if (this.salvageWorkShopItemChangedAmount != 0)
			{
				data["salworchanam"] = new double?((double)this.salvageWorkShopItemChangedAmount);
			}
			if (this.isCrafted)
			{
				data["crafted"] = new bool?(this.isCrafted);
			}
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000BAFC4 File Offset: 0x000B91C4
		public virtual float GetStat(EquipStat s)
		{
			if (!this.parent)
			{
				return 0f;
			}
			float num = this.parent.GetStat(s);
			if (this.turretStats != null)
			{
				foreach (EquipStatLine equipStatLine in this.turretStats)
				{
					if (equipStatLine.stat == s)
					{
						num += equipStatLine.amount;
						num *= equipStatLine.multiplier;
					}
				}
			}
			return num;
		}

		// Token: 0x06001F83 RID: 8067
		public abstract MainStat GetMainStat();

		// Token: 0x06001F84 RID: 8068
		protected abstract void SetMainSubStats();

		// Token: 0x06001F85 RID: 8069 RVA: 0x000BB054 File Offset: 0x000B9254
		public MainSubStats GetMainSubStats()
		{
			if (this.mainSubStats == null || this.mainSubStats.subStatsList.Count == 0)
			{
				this.mainSubStats = new MainSubStats();
				this.SetMainSubStats();
			}
			return this.mainSubStats;
		}

		// Token: 0x040012C3 RID: 4803
		[Header("Basic Info")]
		public ModuleSize size;

		// Token: 0x040012C5 RID: 4805
		protected MainSubStats mainSubStats = new MainSubStats();

		// Token: 0x040012C6 RID: 4806
		[Header("Energy Cost")]
		public float capacityCost;

		// Token: 0x040012C9 RID: 4809
		private Dictionary<string, float> dynamicFields = new Dictionary<string, float>();

		// Token: 0x040012D2 RID: 4818
		private List<EquipStatLine> turretStats;
	}
}
