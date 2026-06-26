using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Combat;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Ability;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World.POI;
using Source.Simulation.World.System;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Unit
{
	// Token: 0x020001BC RID: 444
	public class CombatStationPart : AbstractUnit, ITooltipCustomSource
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0006EEF3 File Offset: 0x0006D0F3
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x0006EEFB File Offset: 0x0006D0FB
		public CombatStationPartType partType { get; private set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0006EF04 File Offset: 0x0006D104
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x0006EF0C File Offset: 0x0006D10C
		public FixedJoint2D connectionJoint { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0006EF18 File Offset: 0x0006D118
		public override string targetName
		{
			get
			{
				return "@StationPart" + this.partType.ToString();
			}
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0006EF43 File Offset: 0x0006D143
		private void OnEnable()
		{
			if (!base.surfaceSprite)
			{
				base.CloneBaseSprite();
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0006EF58 File Offset: 0x0006D158
		protected override void Start()
		{
			base.Start();
			base.transform.Z(ZIndex.Station);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0006EF70 File Offset: 0x0006D170
		private void OnMouseUpAsButton()
		{
			if (!UIHelper.clickTargetingAvailable || !base.enabled)
			{
				return;
			}
			if (base.IsPlayerEnemy() || (Keyboard.current.ctrlKey.isPressed && base.CanBeForceFired()))
			{
				GameplayManager.Instance.spaceShip.SetManualTarget(this);
				if (!Faction.player.IsEnemy(base.faction))
				{
					base.unitData.playerHostile = true;
				}
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0006EFDC File Offset: 0x0006D1DC
		protected override void AddStatModifiers(float[] calcedStats, float[] statMultipliers)
		{
			ConquestStation conquestStation = MapPointOfInterest.currentOrNext as ConquestStation;
			if (conquestStation != null)
			{
				ConquestSystem conquestSystem = conquestStation.system.storyteller as ConquestSystem;
				if (conquestSystem != null)
				{
					foreach (EquipStat equipStat in AbstractUnit.npcHealthAffectedStats)
					{
						float num = Mathf.Clamp(2f, 10f, Mathf.Sqrt(conquestSystem.combatStrength));
						if (conquestSystem.headquarters)
						{
							num += 2f;
						}
						statMultipliers[(int)equipStat] *= num;
					}
				}
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0006F060 File Offset: 0x0006D260
		public override void CheckTriggerAbility(AbilityTrigger trigger, object source, AbstractUnit triggeredBySubordinate)
		{
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0006F062 File Offset: 0x0006D262
		protected override AutoActions CreateAutoActions()
		{
			if (base.unitData.autoActions != null)
			{
				return AutoActions.Create(base.unitData.autoActions, this);
			}
			if (base.hardpointSlots.Length != 0)
			{
				return new CombatActions(this);
			}
			return null;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0006F094 File Offset: 0x0006D294
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.connectionJoint)
			{
				UnityEngine.Object.Destroy(this.connectionJoint);
			}
			if (base.unitData.currentHullHP <= 0f && base.unitData.damageTaken > 0f)
			{
				PhysicsInteraction.ApplyShockwaveToNearbyShips(base.transform.position, 0.6f, 0.62f);
				CombatManager combatManager;
				if (this.TryGetComponentInParent(out combatManager))
				{
					if ((from c in combatManager.GetComponentsInChildren<CombatStationPart>()
					where c.partType != CombatStationPartType.Connector
					select c).ToArray<CombatStationPart>().Length == 0)
					{
						MissionObjective.Trigger(MissionTrigger.CombatStationDestroyed, null, null, false);
						if (SystemMapData.current.pocketSystem && base.faction == Faction.marauders)
						{
							SteamAchievement.Trigger("PiratePocket");
						}
						else if (SystemMapData.current.pocketSystem)
						{
							SteamAchievement.Trigger("WarzonePocket");
						}
					}
				}
				ConquestSystem conquestSystem = SystemMapData.current.storyteller as ConquestSystem;
				if (conquestSystem != null)
				{
					SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
					if (spaceStation != null && spaceStation.faction == SystemMapData.current.faction)
					{
						int num = this.partType.GetFleetReinforcementReduction();
						if (num > 0)
						{
							if ((float)num > conquestSystem.combatStrength)
							{
								num = Mathf.RoundToInt(conquestSystem.combatStrength);
								conquestSystem.combatStrength = 0f;
							}
							else
							{
								conquestSystem.combatStrength -= (float)num;
							}
							if (conquestSystem.combatStrength > 0f)
							{
								Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@DestroyedStation", new object[]
								{
									-num
								})).WithColor(ColorHelper.greenish).WithCustomTime(5f).Show();
								return;
							}
							Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@ReinforcementsReducedToZero", Array.Empty<object>())).WithColor(ColorHelper.greenish).WithCustomTime(5f).Show();
						}
					}
				}
			}
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0006F287 File Offset: 0x0006D487
		public static CombatStationPart Get(string name)
		{
			return CombatStationPart.allParts[name];
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0006F294 File Offset: 0x0006D494
		public static void LoadAll()
		{
			CombatStationPart.allParts.Clear();
			CombatStationPart[] array = Resources.LoadAll<CombatStationPart>("StationParts");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].identifier = array[i].name;
				CombatStationPart.allParts[array[i].identifier] = array[i];
				array[i].GetComponentsInChildren<CombatStationConnector>(array[i].connectors);
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0006F2F9 File Offset: 0x0006D4F9
		public static implicit operator string(CombatStationPart iit)
		{
			return iit.identifier;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0006F301 File Offset: 0x0006D501
		public static implicit operator CombatStationPart(string id)
		{
			return CombatStationPart.Get(id);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0006F30C File Offset: 0x0006D50C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddHeader(Translation.Translate("@StationPart" + this.partType.ToString(), Array.Empty<object>()), base.level, 0, 12, 8f).Item1.Text.color = ColorHelper.detailsColor;
			if (base.faction != null)
			{
				string text = Translation.Translate(base.faction.name, Array.Empty<object>());
				tooltip.AddTextLine(text, 12, 8f).Text.color = (base.IsPlayerEnemy() ? ColorHelper.reddish : ColorHelper.greenish);
				if (base.CanBeForceFired())
				{
					tooltip.AddTextLine("@TooltipForceAttackStation", 12, 8f);
				}
			}
			if (MapPointOfInterest.current is IndustryStation)
			{
				IndustrialOutpost industrialOutpost = MapPointOfInterest.current.storyteller as IndustrialOutpost;
				tooltip.AddSeparator(null);
				tooltip.AddTextLine("@IndustryMissionPOI", 12, 8f);
				float num = industrialOutpost.repairAmount / industrialOutpost.repairMax;
				tooltip.AddTextLine(Translation.Highlight("@IndustryRepairStatus", ((double)num < 0.1) ? ColorHelper.reddish : ColorHelper.greenish, new object[]
				{
					GameMath.FormatPercentage(num, FormatPercentageMode.Default, 0)
				}), 12, 8f);
				float ammoAmount = industrialOutpost.ammoAmount;
				tooltip.AddTextLine(Translation.Highlight("@IndustryAmmoStatus", ((double)ammoAmount < 0.25) ? ColorHelper.reddish : ColorHelper.greenish, new object[]
				{
					GameMath.FormatPercentage(ammoAmount, FormatPercentageMode.Default, 0)
				}), 12, 8f);
				tooltip.AddTextLine(Translation.Translate("@IndustryTurretStatus", new object[]
				{
					industrialOutpost.currentTurrets,
					industrialOutpost.maxTurrets
				}), 12, 8f);
			}
		}

		// Token: 0x0400090F RID: 2319
		public static Dictionary<string, CombatStationPart> allParts = new Dictionary<string, CombatStationPart>();

		// Token: 0x04000911 RID: 2321
		public List<CombatStationConnector> connectors = new List<CombatStationConnector>();
	}
}
