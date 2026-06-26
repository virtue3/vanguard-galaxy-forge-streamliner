using System;
using System.Collections.Generic;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Aspect
{
	// Token: 0x02000379 RID: 889
	public class EquipAspect : MonoBehaviour
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x000C7147 File Offset: 0x000C5347
		public string identifier
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x000C714F File Offset: 0x000C534F
		public string displayName
		{
			get
			{
				return "@Aspect" + this.identifier;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000C7164 File Offset: 0x000C5364
		public virtual string description
		{
			get
			{
				List<object> list = new List<object>();
				BoostStat[] components = base.GetComponents<BoostStat>();
				for (int i = 0; i < components.Length; i++)
				{
					foreach (EquipStatLine equipStatLine in components[i].GetStats(1))
					{
						list.Add(equipStatLine.ToString(false));
					}
				}
				return Translation.Translate("@Aspect" + this.identifier + "Desc", list.ToArray());
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000C71F8 File Offset: 0x000C53F8
		protected virtual void Start()
		{
			this.parent = base.GetComponentInParent<AbstractEquipment>();
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000C7206 File Offset: 0x000C5406
		public virtual void Initialize(AbstractEquipment equipment)
		{
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000C7208 File Offset: 0x000C5408
		public static EquipAspect Get(string name)
		{
			return EquipAspect.allAspects[name];
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000C7218 File Offset: 0x000C5418
		public static void LoadAll()
		{
			EquipAspect.allAspects.Clear();
			EquipAspect[] array = Resources.LoadAll<EquipAspect>("EquipAspects");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					EquipAspect.allAspects[array[i].identifier] = array[i];
				}
			}
		}

		// Token: 0x0400144A RID: 5194
		private static Dictionary<string, EquipAspect> allAspects = new Dictionary<string, EquipAspect>();

		// Token: 0x0400144B RID: 5195
		public Sprite icon;

		// Token: 0x0400144C RID: 5196
		public bool common;

		// Token: 0x0400144D RID: 5197
		protected AbstractEquipment parent;
	}
}
