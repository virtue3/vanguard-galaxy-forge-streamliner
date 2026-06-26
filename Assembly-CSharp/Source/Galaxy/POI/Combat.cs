using System;
using System.Collections.Generic;
using Behaviour.GalaxyMap;
using Behaviour.UI;
using Behaviour.Weapons;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy.NameGenerator;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI
{
	// Token: 0x0200014F RID: 335
	public class Combat : MapPointOfInterest
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0005CFAF File Offset: 0x0005B1AF
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("Combat");
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0005CFBB File Offset: 0x0005B1BB
		public override string sceneName
		{
			get
			{
				return "Combat";
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0005CFC2 File Offset: 0x0005B1C2
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0005CFC5 File Offset: 0x0005B1C5
		protected override string GenerateDefaultName()
		{
			return CombatArea.GenerateCombatAreaName();
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0005CFCC File Offset: 0x0005B1CC
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			if (!GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Combat, TargetLayer.Both))
			{
				tooltip.AddTextLine("@CombatNoTurrets", 12, 8f).Text.color = ColorHelper.reddish;
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0005D004 File Offset: 0x0005B204
		public override void AmbientUpdate(float delta)
		{
			base.AmbientUpdate(delta);
			if (MapPointOfInterest.current == this)
			{
				return;
			}
			if (this.lastVisitedTime == 0f)
			{
				return;
			}
			if (this.timeLeft > 0f)
			{
				return;
			}
			using (IEnumerator<MapTriggeredPayload> enumerator = base.GetTriggeredPayloads().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					MapTriggeredPayload mapTriggeredPayload = enumerator.Current;
					return;
				}
			}
			using (IEnumerator<PersistableData> enumerator2 = base.GetPersistables().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.ShouldKeepPoiAlive())
					{
						return;
					}
				}
			}
			using (IEnumerator<AbstractUnitData> enumerator3 = base.GetUnits(false).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.IsPlayerEnemy())
					{
						return;
					}
				}
			}
			this.system.RemovePointOfInterest(this);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0005D104 File Offset: 0x0005B304
		public override Rect GetWorldBounds()
		{
			Rect worldBounds = base.GetWorldBounds();
			worldBounds.width += 160f;
			worldBounds.height += 40f;
			worldBounds.xMin -= 40f;
			worldBounds.yMin -= 20f;
			return worldBounds;
		}
	}
}
