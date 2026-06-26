using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200035E RID: 862
	public abstract class AbstractModule : AbstractEquipment
	{
		// Token: 0x060020C4 RID: 8388 RVA: 0x000BFB3F File Offset: 0x000BDD3F
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000BFB48 File Offset: 0x000BDD48
		public static void LoadDefaultModules()
		{
			foreach (AbstractModule abstractModule in Resources.LoadAll<AbstractModule>("ShipModules"))
			{
				AbstractModule.defaultModules[abstractModule.gameObject.name] = abstractModule;
			}
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000BFB88 File Offset: 0x000BDD88
		public static AbstractModule GetDefaultModule(string name)
		{
			return AbstractModule.defaultModules[name];
		}

		// Token: 0x0400138C RID: 5004
		private static Dictionary<string, AbstractModule> defaultModules = new Dictionary<string, AbstractModule>();
	}
}
