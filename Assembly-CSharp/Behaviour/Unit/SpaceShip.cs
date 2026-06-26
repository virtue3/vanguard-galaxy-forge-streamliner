using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Ability;
using Behaviour.Crew;
using Behaviour.Effects;
using Behaviour.Equipment;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Module.DroneBay.OpeningMechanism;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Salvage;
using Behaviour.Travel;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Ability;
using Source.Crew;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Unit
{
	// Token: 0x020001C2 RID: 450
	public class SpaceShip : AbstractUnit, ITooltipCustomSource
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00070575 File Offset: 0x0006E775
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x0007057D File Offset: 0x0006E77D
		public ShopItemData shopItemData { get; private set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00070586 File Offset: 0x0006E786
		public float shipCost
		{
			get
			{
				return 65000f * this.costMultiplier;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00070594 File Offset: 0x0006E794
		public float shipCommendationCost
		{
			get
			{
				return 3.5f * this.costMultiplier;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x000705A2 File Offset: 0x0006E7A2
		public float shipSellValue
		{
			get
			{
				return 65000f * this.costMultiplier * 0.4f;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x000705B6 File Offset: 0x0006E7B6
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x000705BE File Offset: 0x0006E7BE
		public SpaceShipRoleType shipRoleType { get; private set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x000705C7 File Offset: 0x0006E7C7
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x000705CF File Offset: 0x0006E7CF
		public bool dockSideways { get; private set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x000705D8 File Offset: 0x0006E7D8
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x000705E0 File Offset: 0x0006E7E0
		public float shipyardScale { get; private set; } = 1f;

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x000705E9 File Offset: 0x0006E7E9
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x000705F1 File Offset: 0x0006E7F1
		public float costMultiplier { get; private set; } = 1f;

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000705FA File Offset: 0x0006E7FA
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x00070602 File Offset: 0x0006E802
		public int maxOfficers { get; private set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0007060B File Offset: 0x0006E80B
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x00070613 File Offset: 0x0006E813
		public int maxGrunts { get; private set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x0007061C File Offset: 0x0006E81C
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x00070624 File Offset: 0x0006E824
		public Commander commander { get; protected set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0007062D File Offset: 0x0006E82D
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x00070635 File Offset: 0x0006E835
		public bool mint { get; private set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x0007063E File Offset: 0x0006E83E
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x00070646 File Offset: 0x0006E846
		public bool hasCommander { get; private set; } = true;

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0007064F File Offset: 0x0006E84F
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x00070657 File Offset: 0x0006E857
		public int maxDecals { get; private set; } = 20;

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00070660 File Offset: 0x0006E860
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00070668 File Offset: 0x0006E868
		public int maxDecalsPerDoor { get; private set; } = 3;

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00070671 File Offset: 0x0006E871
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x00070679 File Offset: 0x0006E879
		public SpaceShipData spaceShipData { get; private set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00070682 File Offset: 0x0006E882
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x0007068A File Offset: 0x0006E88A
		public bool jumpingProcedureEngaged { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00070693 File Offset: 0x0006E893
		public override bool shouldAutoFire
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x00070696 File Offset: 0x0006E896
		public override string displayName
		{
			get
			{
				SpaceShipData spaceShipData = this.spaceShipData;
				if (string.IsNullOrWhiteSpace((spaceShipData != null) ? spaceShipData.customShipName : null))
				{
					return base.displayName;
				}
				SpaceShipData spaceShipData2 = this.spaceShipData;
				if (spaceShipData2 == null)
				{
					return null;
				}
				return spaceShipData2.customShipName;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x000706C9 File Offset: 0x0006E8C9
		public string originalName
		{
			get
			{
				return base.displayName;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x000706D4 File Offset: 0x0006E8D4
		public float totalCost
		{
			get
			{
				float num = this.shipCost;
				float num2 = Mathf.Pow(this.costMultiplier, 0.333333343f);
				foreach (InventoryItemType inventoryItemType in base.unitData.equippedItems)
				{
					num += (float)inventoryItemType.shipComponentSellValue * num2;
				}
				return num;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00070744 File Offset: 0x0006E944
		public float totalSellValue
		{
			get
			{
				float num = this.shipSellValue;
				foreach (InventoryItemType inventoryItemType in base.unitData.equippedItems)
				{
					num += (float)inventoryItemType.sellValue;
				}
				return num;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x000707A4 File Offset: 0x0006E9A4
		public override string targetName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x000707AC File Offset: 0x0006E9AC
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x000707B4 File Offset: 0x0006E9B4
		public RampManager rampManager { get; private set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x000707BD File Offset: 0x0006E9BD
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x000707C5 File Offset: 0x0006E9C5
		public DockingTunnelManager dockingTunnelManager { get; private set; }

		// Token: 0x060010CE RID: 4302 RVA: 0x000707CE File Offset: 0x0006E9CE
		private void OnEnable()
		{
			if (!base.surfaceSprite)
			{
				base.CloneBaseSprite();
				this.ApplyDecals(null);
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000707EC File Offset: 0x0006E9EC
		protected override void Start()
		{
			base.Start();
			base.identifier = this.spaceShipData.unitDefinition.identifier;
			if (base.faction == Faction.player && base.unitData.autoActions == null)
			{
				base.GetComponent<TooltipSource>().enabled = false;
			}
			this.rampManager = base.GetComponentInChildren<RampManager>();
			this.dockingTunnelManager = base.GetComponentInChildren<DockingTunnelManager>();
			this.SetReactorLight();
			if (base.IsPlayerEnemy())
			{
				EnemyIndicatorManager.Instance.ShowIndicator(this);
			}
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0007086D File Offset: 0x0006EA6D
		protected override void Update()
		{
			base.Update();
			if (base.IsPlayer(true))
			{
				base.holdPosition = GamePlayer.current.holdPosition;
			}
			if (this.travelAngleTimer > 0f)
			{
				this.travelAngleTimer -= Time.deltaTime;
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x000708B0 File Offset: 0x0006EAB0
		private void SetReactorLight()
		{
			ReactorModuleLight componentInChildren = base.GetComponentInChildren<ReactorModuleLight>();
			if (componentInChildren != null)
			{
				componentInChildren.Init(base.reactorModule, !base.IsPlayer(true));
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x000708E4 File Offset: 0x0006EAE4
		protected override AutoActions CreateAutoActions()
		{
			if (base.IsPlayer(true))
			{
				return null;
			}
			if (base.unitData.autoActions != null)
			{
				return AutoActions.Create(base.unitData.autoActions, this);
			}
			if (MapPointOfInterest.current is SpaceStation)
			{
				return new SpaceStationActions(this);
			}
			if (base.GetComponentInChildren<CombatModule>())
			{
				return new CombatActions(this);
			}
			return null;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00070944 File Offset: 0x0006EB44
		public void InitSpacestationAutoActions()
		{
			SpaceStationActions spaceStationActions = base.autoActions as SpaceStationActions;
			if (spaceStationActions != null)
			{
				spaceStationActions.CheckDockingState();
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00070966 File Offset: 0x0006EB66
		public override bool MoveOnCombatDamage()
		{
			return this.shipRoleType.GetTypeSize() < 3;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00070978 File Offset: 0x0006EB78
		public override void SetData(AbstractUnitData abstractUnitData, bool setUnitRef = true, bool applyBattleDamage = true)
		{
			base.SetData(abstractUnitData, setUnitRef, applyBattleDamage);
			SpaceShipData spaceShipData = abstractUnitData as SpaceShipData;
			if (spaceShipData != null)
			{
				this.spaceShipData = spaceShipData;
				CommanderData commanderData;
				if (!spaceShipData.isPlayer)
				{
					commanderData = spaceShipData.commanderData;
				}
				else
				{
					GamePlayer current = GamePlayer.current;
					commanderData = ((current != null) ? current.commander : null);
				}
				this.SetCrewMembers(commanderData, this.spaceShipData.crewMembers);
				this.ApplyDecals(spaceShipData);
			}
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000709D8 File Offset: 0x0006EBD8
		public void ApplyDecals(SpaceShipData data = null)
		{
			if (data == null)
			{
				data = this.spaceShipData;
			}
			if (base.surfaceSprite != null && data != null && data.decalPlacements.Count > 0)
			{
				if (this._cleanOriginalSprite == null)
				{
					this._cleanOriginalSprite = this.originalSprite;
				}
				this.BakeDecals(data);
				this.originalSprite = SpriteHelper.CopySprite(base.surfaceSprite.sprite);
			}
			DroneBayModule[] droneBayModules = this.GetDroneBayModules();
			for (int i = 0; i < droneBayModules.Length; i++)
			{
				droneBayModules[i].BakeAllDoorDecals();
			}
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00070A64 File Offset: 0x0006EC64
		public void ChangeDecals(SpaceShipData data)
		{
			if (this._cleanOriginalSprite != null)
			{
				this.originalSprite = this._cleanOriginalSprite;
			}
			this._cleanOriginalSprite = null;
			base.spriteRenderer.sprite = this.originalSprite;
			base.CloneBaseSprite();
			if (this._cachedDoors == null)
			{
				this._cachedDoors = base.GetComponentsInChildren<Door>();
			}
			Door[] cachedDoors = this._cachedDoors;
			for (int i = 0; i < cachedDoors.Length; i++)
			{
				cachedDoors[i].ResetSprite();
			}
			DroneBayModule[] droneBayModules = this.GetDroneBayModules();
			for (int i = 0; i < droneBayModules.Length; i++)
			{
				droneBayModules[i].ResetAllDoors();
			}
			this.ApplyDecals(data);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00070B00 File Offset: 0x0006ED00
		private DroneBayModule[] GetDroneBayModules()
		{
			DroneBayModule[] result;
			if ((result = this._cachedDroneBayModules) == null)
			{
				result = (this._cachedDroneBayModules = base.GetComponentsInChildren<DroneBayModule>());
			}
			return result;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00070B28 File Offset: 0x0006ED28
		private void BakeDecals(SpaceShipData data)
		{
			Texture2D texture = base.surfaceSprite.sprite.texture;
			Rect rect = base.surfaceSprite.sprite.rect;
			float pixelsPerUnit = base.surfaceSprite.sprite.pixelsPerUnit;
			float x = base.surfaceSprite.sprite.pivot.x;
			float y = base.surfaceSprite.sprite.pivot.y;
			if (this._cachedDoors == null)
			{
				this._cachedDoors = base.GetComponentsInChildren<Door>();
			}
			bool flag = false;
			HashSet<Door> hashSet = new HashSet<Door>();
			foreach (DecalPlacement decalPlacement in data.decalPlacements)
			{
				if (!string.IsNullOrEmpty(decalPlacement.decalId))
				{
					DecalDefinition decalDefinition = Decals.Get(decalPlacement.decalId);
					if (decalDefinition != null)
					{
						Sprite sprite = decalDefinition.GetSprite();
						if (!(sprite == null) && sprite.texture.isReadable)
						{
							Texture2D texture2 = sprite.texture;
							float scale = decalPlacement.scale;
							int num = Mathf.Max(1, Mathf.RoundToInt((float)texture2.width * scale));
							int num2 = Mathf.Max(1, Mathf.RoundToInt((float)texture2.height * scale));
							int num3 = Mathf.RoundToInt(rect.x + x + decalPlacement.position.x * pixelsPerUnit);
							int num4 = Mathf.RoundToInt(rect.y + y + decalPlacement.position.y * pixelsPerUnit);
							float num5 = (float)num / (float)texture2.width;
							float num6 = (float)num2 / (float)texture2.height;
							float num7 = sprite.pivot.x * num5;
							float num8 = sprite.pivot.y * num6;
							float f = decalPlacement.rotation * 0.0174532924f;
							float num9 = Mathf.Cos(f);
							float num10 = Mathf.Sin(f);
							int num11 = Mathf.CeilToInt((Mathf.Abs(num9) * (float)num + Mathf.Abs(num10) * (float)num2) * 0.5f) + 1;
							int num12 = Mathf.CeilToInt((Mathf.Abs(num10) * (float)num + Mathf.Abs(num9) * (float)num2) * 0.5f) + 1;
							for (int i = -num12; i <= num12; i++)
							{
								for (int j = -num11; j <= num11; j++)
								{
									float num13 = num9 * (float)j + num10 * (float)i;
									float num14 = -num10 * (float)j + num9 * (float)i;
									float num15 = num13 + num7;
									float num16 = num14 + num8;
									if (num15 >= 0f && num15 < (float)num && num16 >= 0f && num16 < (float)num2)
									{
										int x2 = Mathf.Clamp(Mathf.RoundToInt(num15 / (float)(num - 1) * (float)(texture2.width - 1)), 0, texture2.width - 1);
										int y2 = Mathf.Clamp(Mathf.RoundToInt(num16 / (float)(num2 - 1) * (float)(texture2.height - 1)), 0, texture2.height - 1);
										Color pixel = texture2.GetPixel(x2, y2);
										if (pixel.a >= 0.01f)
										{
											Color color = new Color(pixel.r * decalPlacement.color.r, pixel.g * decalPlacement.color.g, pixel.b * decalPlacement.color.b, pixel.a);
											int num17 = num3 + j;
											int num18 = num4 + i;
											if (num17 >= 0 && num17 < texture.width && num18 >= 0 && num18 < texture.height)
											{
												Color pixel2 = texture.GetPixel(num17, num18);
												if (pixel2.a < 0.01f)
												{
													float x3 = ((float)num17 - rect.x - x) / pixelsPerUnit;
													float y3 = ((float)num18 - rect.y - y) / pixelsPerUnit;
													Vector3 position = base.transform.TransformPoint(new Vector2(x3, y3));
													foreach (Door door in this._cachedDoors)
													{
														Vector2 doorLocalPos = door.transform.InverseTransformPoint(position);
														if (door.TryBakePixel(doorLocalPos, color, decalPlacement.opacity))
														{
															hashSet.Add(door);
														}
													}
												}
												else if (num17 - 1 >= 0 && texture.GetPixel(num17 - 1, num18).a >= 0.01f && num17 + 1 < texture.width && texture.GetPixel(num17 + 1, num18).a >= 0.01f && num18 - 1 >= 0 && texture.GetPixel(num17, num18 - 1).a >= 0.01f && num18 + 1 < texture.height && texture.GetPixel(num17, num18 + 1).a >= 0.01f)
												{
													Color color2 = Color.Lerp(pixel2, color, color.a * decalPlacement.opacity);
													color2.a = pixel2.a;
													texture.SetPixel(num17, num18, color2);
													flag = true;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				texture.Apply();
			}
			foreach (Door door2 in hashSet)
			{
				door2.FlushTexture();
			}
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x000710C0 File Offset: 0x0006F2C0
		public bool IsPixelOnShip(Vector2 localPos)
		{
			if (base.surfaceSprite != null)
			{
				Sprite sprite = base.surfaceSprite.sprite;
				if (sprite != null && sprite.texture.isReadable)
				{
					Texture2D texture = sprite.texture;
					float pixelsPerUnit = sprite.pixelsPerUnit;
					int num = Mathf.RoundToInt(sprite.rect.x + sprite.pivot.x + localPos.x * pixelsPerUnit);
					int num2 = Mathf.RoundToInt(sprite.rect.y + sprite.pivot.y + localPos.y * pixelsPerUnit);
					if (num >= 0 && num < texture.width && num2 >= 0 && num2 < texture.height && texture.GetPixel(num, num2).a > 0.01f)
					{
						return true;
					}
				}
			}
			Vector3 position = base.transform.TransformPoint(localPos);
			if (this._cachedDoors == null)
			{
				this._cachedDoors = base.GetComponentsInChildren<Door>();
			}
			foreach (Door door in this._cachedDoors)
			{
				Vector2 doorLocalPos = door.transform.InverseTransformPoint(position);
				if (door.IsPixelOnDoor(doorLocalPos))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00071205 File Offset: 0x0006F405
		public void ConnectModulesToReactor()
		{
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00071207 File Offset: 0x0006F407
		public void UnloadEquipment(InventoryItemType item)
		{
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00071209 File Offset: 0x0006F409
		public int AddItemToCargo(InventoryItemType inventoryItemType, int i, bool force = false)
		{
			return this.spaceShipData.AddCargo(inventoryItemType, i, force);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00071219 File Offset: 0x0006F419
		public bool CanAddItemToCargo(InventoryItemType iit, int count)
		{
			return this.spaceShipData.GetCargoAvailable() >= iit.m3 * (float)count;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00071234 File Offset: 0x0006F434
		public void ToggleDockingTunnel(bool open)
		{
			if (this.dockingTunnelManager)
			{
				base.StartCoroutine(this.dockingTunnelManager.ToggleDockingTunnels(open, false, null));
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00071258 File Offset: 0x0006F458
		public void ToggleRamp(bool open)
		{
			if (this.rampManager)
			{
				base.StartCoroutine(this.rampManager.ToggleRamps(open));
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0007127A File Offset: 0x0006F47A
		public void DeployDockingTunnel()
		{
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0007127C File Offset: 0x0006F47C
		public void SetCrewMembers(CommanderData commanderData, IEnumerable<CrewMemberData> crewSeats)
		{
			CrewMember[] componentsInChildren = base.GetComponentsInChildren<CrewMember>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
			if (commanderData != null)
			{
				this.commander = new GameObject("Commander " + commanderData.GetFullName())
				{
					transform = 
					{
						parent = base.transform,
						localPosition = Vector3.zero,
						localRotation = Quaternion.identity
					}
				}.AddComponent<Commander>();
				this.commander.SetCommanderData(commanderData, crewSeats);
				this.spaceShipData.commanderData = commanderData;
			}
			this.crewMembers.Clear();
			foreach (CrewMemberData crewMemberData in crewSeats)
			{
				if (crewMemberData != null)
				{
					CrewMember crewMember = new GameObject("CrewMember " + crewMemberData.GetFullName())
					{
						transform = 
						{
							parent = base.transform,
							localPosition = Vector3.zero,
							localRotation = Quaternion.identity
						}
					}.AddComponent<CrewMember>();
					crewMember.SetCrewMemberData(crewMemberData);
					this.crewMembers.Add(crewMember);
				}
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x000713C4 File Offset: 0x0006F5C4
		public CrewMember GetCrewMember(CrewMemberData data)
		{
			foreach (CrewMember crewMember in this.crewMembers)
			{
				if (crewMember.crewMemberData == data)
				{
					return crewMember;
				}
			}
			return null;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00071420 File Offset: 0x0006F620
		public IEnumerable<ActivatedAbility> GetActivatedAbilities()
		{
			return base.GetComponentsInChildren<ActivatedAbility>();
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00071428 File Offset: 0x0006F628
		public IEnumerable<AbstractTurret> GetTurrets()
		{
			return base.GetComponentsInChildren<AbstractTurret>();
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00071430 File Offset: 0x0006F630
		public void SetTurretState(bool active)
		{
			foreach (AbstractTurret abstractTurret in this.GetTurrets())
			{
				if (active)
				{
					abstractTurret.Activate();
				}
				else
				{
					abstractTurret.Deactivate();
				}
			}
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00071488 File Offset: 0x0006F688
		public void UpdateCommanderSkills()
		{
			if (!this.commander)
			{
				return;
			}
			this.commander.CheckExperience();
			foreach (CrewMember crewMember in this.crewMembers)
			{
				crewMember.CheckExperience();
			}
			this.CalculateStats();
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000714F8 File Offset: 0x0006F6F8
		public override void CalculateStats()
		{
			base.CalculateStats();
			this.UpdateTriggeredAbilities();
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00071508 File Offset: 0x0006F708
		public void UpdateTriggeredAbilities()
		{
			this.triggeredAbilities.Clear();
			foreach (TriggeredAbility triggeredAbility in base.GetComponentsInChildren<TriggeredAbility>())
			{
				if (triggeredAbility)
				{
					List<TriggeredAbility> list;
					if (!this.triggeredAbilities.TryGetValue(triggeredAbility.trigger, out list))
					{
						list = new List<TriggeredAbility>();
						this.triggeredAbilities[triggeredAbility.trigger] = list;
					}
					list.Add(triggeredAbility);
				}
			}
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00071578 File Offset: 0x0006F778
		public override void CheckTriggerAbility(AbilityTrigger trigger, object source, AbstractUnit triggeredBySubordinate)
		{
			List<TriggeredAbility> list;
			if (this.triggeredAbilities.TryGetValue(trigger, out list))
			{
				foreach (TriggeredAbility triggeredAbility in list)
				{
					if ((triggeredAbility.triggeredBySelf && !triggeredBySubordinate) || (triggeredAbility.triggeredBySubordinate && triggeredBySubordinate))
					{
						triggeredAbility.TriggerPayload(source, triggeredBySubordinate);
					}
				}
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x000715F8 File Offset: 0x0006F7F8
		public override void AddCrewExperience(float experience, CommanderSpecialization? skillTree = null, bool showFloating = true)
		{
			if (GameplayManager.Instance.spaceShip != this)
			{
				return;
			}
			float num = this.spaceShipData.AddCrewExperience(experience, skillTree);
			if (showFloating && num > 0f && base.IsPlayer(false))
			{
				UIInfoTextParent.instance.ShowExperienceNumber(num);
			}
			if (base.droneBayModule)
			{
				CommanderSpecialization? commanderSpecialization = skillTree;
				CommanderSpecialization commanderSpecialization2 = CommanderSpecialization.Offense;
				if (!(commanderSpecialization.GetValueOrDefault() == commanderSpecialization2 & commanderSpecialization != null))
				{
					commanderSpecialization = skillTree;
					commanderSpecialization2 = CommanderSpecialization.Mining;
					if (!(commanderSpecialization.GetValueOrDefault() == commanderSpecialization2 & commanderSpecialization != null))
					{
						commanderSpecialization = skillTree;
						commanderSpecialization2 = CommanderSpecialization.Salvaging;
						if (!(commanderSpecialization.GetValueOrDefault() == commanderSpecialization2 & commanderSpecialization != null))
						{
							return;
						}
					}
				}
				Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Drones));
				SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(tree, false);
				if (skillTreeData == null)
				{
					return;
				}
				skillTreeData.AddMasteryXp(experience);
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000716C4 File Offset: 0x0006F8C4
		public void CancelJumpAway(Vector2? position)
		{
			if (position != null)
			{
				base.SetOverrideDestination(position.Value, true, false, false);
			}
			else
			{
				base.SetBrakeDestination();
			}
			if (this.travelEffect)
			{
				this.travelEffect.Stop();
				UnityEngine.Object.Destroy(this.travelEffect.gameObject);
			}
			this.forceWorldAngle = null;
			base.unitData.travelling = false;
			base.unitData.travelSpeed = 0f;
			this.collider.isTrigger = false;
			base.structureCollider.isTrigger = false;
			AbstractEquipment[] componentsInChildren = base.GetComponentsInChildren<AbstractEquipment>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Activate();
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00071777 File Offset: 0x0006F977
		public void EnableCollision(bool enabled)
		{
			this.collider.isTrigger = !enabled;
			base.structureCollider.isTrigger = !enabled;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00071798 File Offset: 0x0006F998
		public void DisableCollisionWith(SpaceShip spaceShip)
		{
			Physics2D.IgnoreCollision(base.structureCollider, spaceShip.surfaceCollider);
			Physics2D.IgnoreCollision(base.structureCollider, spaceShip.structureCollider);
			Physics2D.IgnoreCollision(base.surfaceCollider, spaceShip.surfaceCollider);
			Physics2D.IgnoreCollision(base.surfaceCollider, spaceShip.structureCollider);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x000717E9 File Offset: 0x0006F9E9
		public IEnumerator PrepareForInSystemTravel(Vector2 worldPositionToTravelTo, bool fastLaneTravel = false)
		{
			this.forceWorldAngle = new Quaternion?(this.GetShipLookRotationForTravel(worldPositionToTravelTo));
			if (!base.unitData.travelling)
			{
				yield return this.ExitProtocol(worldPositionToTravelTo);
			}
			base.unitData.travelling = true;
			this.SetTravelEffect();
			this.travelEffect.SetBasicVars(base.height, 2f, fastLaneTravel, base.radius / base.height);
			this.travelEffect.Travel();
			if (base.droneBayModule)
			{
				using (List<Drone>.Enumerator enumerator = base.droneBayModule.drones.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Drone drone = enumerator.Current;
						if (drone.gameObject.activeSelf)
						{
							drone.ForceReset();
						}
					}
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00071808 File Offset: 0x0006FA08
		public void CompleteTravel()
		{
			AbstractEquipment[] componentsInChildren = base.GetComponentsInChildren<AbstractEquipment>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Activate();
			}
			this.forceWorldAngle = null;
			this.collider.isTrigger = false;
			base.structureCollider.isTrigger = false;
			this.SetTravelEffect();
			this.travelEffect.transform.parent = GameplayManager.Instance.transform;
			this.travelEffect.Land();
			UnityEngine.Object.Destroy(this.travelEffect.gameObject, 3f);
			base.unitData.travelling = false;
			base.unitData.travelSpeed = 0f;
			base.ResetPosition(new Vector2?(base.rigidbody.position));
			base.SetEngineState(true, true);
			SpaceShipData spaceShipData = this.spaceShipData;
			CommanderData commanderData = (spaceShipData != null) ? spaceShipData.commanderData : null;
			if (commanderData != null && !string.IsNullOrEmpty(commanderData.pendingNpcMessage))
			{
				HudManager.Instance.ShowNpcMessage(this, commanderData, commanderData.pendingNpcMessage, 6f);
				commanderData.pendingNpcMessage = null;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00071911 File Offset: 0x0006FB11
		public IEnumerator WaitForDrones(bool waitForDrones = true)
		{
			if (base.droneBayModule != null)
			{
				if (waitForDrones)
				{
					base.droneBayModule.ReturnDrones();
					yield return this.WaitForAllDronesToDock();
				}
				else
				{
					base.droneBayModule.CancelDeployReturn();
				}
			}
			yield break;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00071927 File Offset: 0x0006FB27
		private IEnumerator ExitProtocol(Vector2 worldPositionToTravelTo)
		{
			base.SetBrakeDestination();
			SalvageModule module = base.GetModule<SalvageModule>();
			if (module)
			{
				module.ReleaseTarget();
				this.forceWorldAngle = new Quaternion?(this.GetShipLookRotationForTravel(worldPositionToTravelTo));
			}
			bool waitForDrones = !base.IsPlayer(false) || !GamePlayer.current.emergencyJump;
			yield return this.WaitForDrones(waitForDrones);
			TractorModule module2 = base.GetModule<TractorModule>();
			if (module2 && waitForDrones)
			{
				yield return module2.WaitForIdle();
			}
			this.collider.isTrigger = true;
			base.structureCollider.isTrigger = true;
			this.travelAngleTimer = 10f;
			yield return new WaitUntil(delegate()
			{
				float currentAngleToTarget = base.GetCurrentAngleToTarget();
				return (currentAngleToTarget < 5f && currentAngleToTarget > -5f) || this.travelAngleTimer < 0f;
			});
			this.SetTravelEffect();
			this.travelEffect.Charge();
			this.DisableEquipment();
			yield return new WaitForSeconds(1f);
			yield return null;
			yield break;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00071940 File Offset: 0x0006FB40
		public void GiveJumpGateImpulse(TravelDirection travelDirection)
		{
			base.rigidbody.rotation = (float)((travelDirection == TravelDirection.Right) ? 180 : 0);
			float num = UnityEngine.Random.Range(2f, 0.5f);
			num = (SeededRandom.Global.RandomBool(0.5f) ? num : (-num));
			int num2 = (travelDirection == TravelDirection.Right) ? -1 : 1;
			if (base.rigidbody.mass < 20000f)
			{
				base.GiveImpulse(new Vector2((float)this.shipRoleType.GetTypeSize() / 2f * (float)num2 * base.rigidbody.mass * (float)SeededRandom.Global.RandomRange(5, 7), 0f), base.rigidbody.inertia * num, 0f);
				return;
			}
			Vector2 force = new Vector2((float)this.shipRoleType.GetTypeSize() / 2f * (float)num2 * base.rigidbody.mass * SeededRandom.Global.RandomRange(3f, 4.5f), 0f);
			base.GiveImpulse(force, 0f, 0f);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00071A4C File Offset: 0x0006FC4C
		public void DisableEquipment()
		{
			AbstractEquipment[] componentsInChildren = base.GetComponentsInChildren<AbstractEquipment>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Deactivate();
			}
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00071A76 File Offset: 0x0006FC76
		public void StartLandNpcAtPoiCoroutine(Vector2 targetPosition)
		{
			base.StartCoroutine(this.LandNpcAtPoi(targetPosition));
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00071A86 File Offset: 0x0006FC86
		private IEnumerator LandNpcAtPoi(Vector2 targetPosition)
		{
			yield return new WaitForEndOfFrame();
			base.unitData.travelling = true;
			this.collider.isTrigger = true;
			base.structureCollider.isTrigger = true;
			base.unitData.travelSpeed = this.baseMaxWarpSpeed / 10f;
			base.transform.rotation = this.GetShipLookRotationForTravel(targetPosition);
			this.SetTravelEffect();
			this.travelEffect.SetBasicVars(base.height, 2f, false, 1f);
			this.travelEffect.Travel();
			while (base.gameObject && Vector2.Distance(base.rigidbody.position, targetPosition) > 0.5f)
			{
				float num = Vector2.Distance(base.rigidbody.position, targetPosition);
				if (0.5 * (double)this.baseWarpAcceleration * (double)Mathf.Pow(base.unitData.travelSpeed / this.baseWarpAcceleration, 2f) >= (double)num)
				{
					base.unitData.travelSpeed = Mathf.Clamp(base.unitData.travelSpeed - this.baseWarpAcceleration * Time.deltaTime, 0f, this.baseWarpAcceleration);
				}
				this.MoveTowards(targetPosition);
				yield return null;
			}
			this.CompleteTravel();
			yield return null;
			yield break;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00071A9C File Offset: 0x0006FC9C
		private void SetTravelEffect()
		{
			if (!this.travelEffect)
			{
				this.CreateTravelEffect();
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00071AB4 File Offset: 0x0006FCB4
		private void CreateTravelEffect()
		{
			this.travelEffect = UnityEngine.Object.Instantiate<TravelEffect>(this.travelEffectPrefab, base.transform);
			this.travelEffect.transform.localPosition = new Vector2(base.radius * 1.5f, 0f);
			this.travelEffect.transform.rotation = base.transform.rotation;
			this.travelEffect.SetBasicVars(base.height, 2f, Singleton<TravelManager>.Instance.fastLaneTravelActive, base.radius / base.height);
			this.travelEffect.Play();
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00071B56 File Offset: 0x0006FD56
		private IEnumerator WaitForAllDronesToDock()
		{
			float timeout = 10f;
			float timer = 0f;
			while (!base.droneBayModule.AllDronesDocked())
			{
				timer += Time.deltaTime;
				if (timer > timeout)
				{
					Debug.LogWarning("Drones zijn traag of wat dan ook, dus we laten ze achter.");
					break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00071B68 File Offset: 0x0006FD68
		public void MoveTowards(Vector2 worldLocationToTravelTo)
		{
			float maxDistanceDelta = base.unitData.travelSpeed * Time.fixedDeltaTime * MapPointOfInterest.mapToLocalConversion;
			base.rigidbody.position = Vector2.MoveTowards(base.rigidbody.position, worldLocationToTravelTo, maxDistanceDelta);
			Quaternion rotation = Quaternion.RotateTowards(base.transform.rotation, this.GetShipLookRotationForTravel(worldLocationToTravelTo), Time.fixedDeltaTime * 200f);
			if (Vector2.Distance(base.rigidbody.position, worldLocationToTravelTo) > 3f)
			{
				base.transform.rotation = rotation;
			}
			if (this.travelEffect)
			{
				this.travelEffect.SetTravelSpeed(base.unitData.travelSpeed);
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00071C14 File Offset: 0x0006FE14
		public Quaternion GetShipLookRotationForTravel(Vector2 worldLocation)
		{
			if (base.IsPlayer(true) && !Singleton<TravelManager>.Instance && Singleton<TravelManager>.Instance.localTarget != null)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0f, 0f, 90f) * (worldLocation - base.rigidbody.position));
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00071C84 File Offset: 0x0006FE84
		public void OnMouseDown()
		{
			if (base.inShipYard || !UIHelper.clickTargetingAvailable || !base.enabled)
			{
				return;
			}
			GameplayManager instance = GameplayManager.Instance;
			if (base.IsPlayerEnemy() || (Keyboard.current.ctrlKey.isPressed && base.CanBeForceFired()))
			{
				instance.spaceShip.SetManualTarget(this);
				if (!Faction.player.IsEnemy(base.faction))
				{
					base.unitData.playerHostile = true;
					return;
				}
			}
			else if (this == instance.spaceShip)
			{
				instance.cameraMovement.SetTarget(this, true);
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00071D18 File Offset: 0x0006FF18
		private void OnBecameInvisible()
		{
			if (base.IsPlayer(true))
			{
				TravelManager instance = Singleton<TravelManager>.Instance;
				if (instance != null && !instance.TravelActive())
				{
					Debug.Log("We just became invisible: " + Singleton<TravelManager>.Instance.TravelActive().ToString());
					HudManager.Instance.ToggleTrackShipText(true);
				}
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00071D6D File Offset: 0x0006FF6D
		private void OnBecameVisible()
		{
			if (base.IsPlayer(true))
			{
				if (!HudManager.Instance)
				{
					return;
				}
				HudManager.Instance.ToggleTrackShipText(false);
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00071D90 File Offset: 0x0006FF90
		public void SetHullHP(int amount)
		{
			base.currentHullHP = (float)Mathf.Clamp(amount, 0, (int)base.maxHullHP);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00071DA7 File Offset: 0x0006FFA7
		public void RepairHullHP(float amount)
		{
			base.currentHullHP = Mathf.Min(base.currentHullHP + amount, base.maxHullHP);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00071DC2 File Offset: 0x0006FFC2
		public float HullDamageTaken()
		{
			return base.maxHullHP - base.currentHullHP;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00071DD1 File Offset: 0x0006FFD1
		public float ArmorDamageTaken()
		{
			return base.maxArmorHP - base.currentArmorHP;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00071DE0 File Offset: 0x0006FFE0
		public void SetArmorHP(int amount)
		{
			base.currentArmorHP = (float)Mathf.Clamp(amount, 0, (int)base.maxArmorHP);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00071DF7 File Offset: 0x0006FFF7
		public float GetCurrentX()
		{
			return base.rigidbody.position.x;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00071E0C File Offset: 0x0007000C
		public override void TakeDamage(DamageData damageData)
		{
			base.TakeDamage(damageData);
			if (this == GameplayManager.Instance.spaceShip && damageData.totalDamageAmount > 0f)
			{
				MissionObjective.Trigger(MissionTrigger.TakeDamage, (int)damageData.totalDamageAmount, null, false);
			}
			if (base.isDestroyed)
			{
				return;
			}
			CombatModule module = base.GetModule<CombatModule>();
			if (module && !module.priorityTarget && damageData.sourceUnit && damageData.sourceUnit.IsEnemy(this))
			{
				base.SetManualTarget(damageData.sourceUnit);
			}
			if (base.droneBayModule && damageData.sourceUnit && damageData.sourceUnit.IsEnemy(this))
			{
				base.droneBayModule.AddManualTarget(damageData.sourceUnit, false);
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00071EDC File Offset: 0x000700DC
		public GameplayType GetPreferredGameplayType(bool autoDetect = false)
		{
			if (!autoDetect && GamePlayer.current.autopilotSettings.preferredGameplayType != GameplayType.Generic)
			{
				this.preferredTargetLayer = GamePlayer.current.autopilotSettings.preferredTargetLayer;
				return GamePlayer.current.autopilotSettings.preferredGameplayType;
			}
			if (this.preferredGameplayType != GameplayType.Generic)
			{
				if (this.preferredGameplayType == GameplayType.Combat || this.preferredGameplayType == GameplayType.Cargo)
				{
					this.preferredTargetLayer = TargetLayer.Both;
				}
				return this.preferredGameplayType;
			}
			if (!this)
			{
				return GameplayType.Generic;
			}
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			AbstractTurret[] componentsInChildren = base.GetComponentsInChildren<AbstractTurret>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				SpaceShip.AddPowerForMissionType(componentsInChildren[i], dictionary);
			}
			DroneBayModule componentInChildren = base.GetComponentInChildren<DroneBayModule>();
			if (componentInChildren)
			{
				foreach (Drone drone in componentInChildren.drones)
				{
					componentsInChildren = drone.GetComponentsInChildren<AbstractTurret>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						SpaceShip.AddPowerForMissionType(componentsInChildren[i], dictionary);
					}
				}
			}
			if (dictionary.Count<KeyValuePair<string, float>>() == 0)
			{
				return GameplayType.Generic;
			}
			string[] array = dictionary.Aggregate(delegate(KeyValuePair<string, float> l, KeyValuePair<string, float> r)
			{
				if (l.Value <= r.Value)
				{
					return r;
				}
				return l;
			}).Key.Split("_", StringSplitOptions.None);
			GameplayType result = Enum.Parse<GameplayType>(array[0]);
			this.preferredTargetLayer = Enum.Parse<TargetLayer>(array[1]);
			Debug.Log("Determined prefered mission type: " + result.ToString() + ", targetLayer: " + this.preferredTargetLayer.ToString());
			this.preferredGameplayType = result;
			return result;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0007208C File Offset: 0x0007028C
		private static void AddPowerForMissionType(AbstractTurret turret, Dictionary<string, float> powerPerType)
		{
			string text = turret.gameplayType.ToString() + "_" + turret.targetLayer.ToString();
			float attackPower = turret.GetAttackPower();
			if (!powerPerType.TryAdd(text, attackPower))
			{
				string key = text;
				powerPerType[key] += attackPower;
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000720F8 File Offset: 0x000702F8
		public string GetPreferredActivityName(bool autoDetect = false)
		{
			GameplayType? gameplayType = new GameplayType?(this.GetPreferredGameplayType(autoDetect));
			string targetLayerName = SpaceShip.GetTargetLayerName(gameplayType, new TargetLayer?(this.preferredTargetLayer));
			GameplayType? gameplayType2 = gameplayType;
			return gameplayType2.ToString() + ((targetLayerName == "") ? "" : ("(" + targetLayerName + ")"));
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0007215C File Offset: 0x0007035C
		public static string GetTargetLayerName(GameplayType? missionType, TargetLayer? targetLayer)
		{
			if (missionType != null)
			{
				GameplayType valueOrDefault = missionType.GetValueOrDefault();
				if (valueOrDefault == GameplayType.Mining)
				{
					TargetLayer? targetLayer2 = targetLayer;
					TargetLayer targetLayer3 = TargetLayer.Core;
					return (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null) ? "Core" : "Surface";
				}
				if (valueOrDefault == GameplayType.Salvage)
				{
					TargetLayer? targetLayer2 = targetLayer;
					TargetLayer targetLayer3 = TargetLayer.Core;
					return (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null) ? "Structure" : "Surface";
				}
			}
			return "";
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000721DC File Offset: 0x000703DC
		public bool SalvagingItem()
		{
			SalvageModule componentInChildren = base.GetComponentInChildren<SalvageModule>();
			if (componentInChildren && componentInChildren.priorityTarget is SalvageContainer)
			{
				SalvageContainer salvageContainer = componentInChildren.priorityTarget as SalvageContainer;
				object obj;
				if (salvageContainer == null)
				{
					obj = null;
				}
				else
				{
					SalvageData data = salvageContainer.data;
					obj = ((data != null) ? data.activeItem : null);
				}
				return obj != null;
			}
			return false;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00072230 File Offset: 0x00070430
		public SalvageContainer GetCurrentSalvageTarget()
		{
			SalvageModule componentInChildren = base.GetComponentInChildren<SalvageModule>();
			if (componentInChildren && componentInChildren.priorityTarget is SalvageContainer)
			{
				return componentInChildren.priorityTarget as SalvageContainer;
			}
			return null;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00072268 File Offset: 0x00070468
		public bool CanDock()
		{
			DockingState? dockingState = this.spaceShipData.dockingState;
			DockingState dockingState2 = DockingState.Docking;
			return !(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00072298 File Offset: 0x00070498
		public Vector2 GetFormationPos(SpaceShip leader)
		{
			int num = GamePlayer.current.activeFleet.IndexOf(this.spaceShipData);
			if (num < 0)
			{
				return -new Vector2(leader.radius / 2f, leader.height + base.height + 1f);
			}
			float num2 = (float)((num % 2 == 0) ? 1 : -1);
			float x = (float)(num / 2 + 1) * (leader.radius / 2f + base.radius / 2f);
			float y = num2 * (leader.height + base.height + 1f);
			return -new Vector2(x, y);
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00072338 File Offset: 0x00070538
		public void CleanUpTravel()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name == "TrackingTarget" || transform.gameObject.name == "Hit Target")
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			if (base.droneBayModule)
			{
				foreach (Drone drone in base.droneBayModule.drones)
				{
					foreach (object obj2 in drone.transform)
					{
						Transform transform2 = (Transform)obj2;
						if (transform2.gameObject.name == "TrackingTarget" || transform2.gameObject.name == "Hit Target")
						{
							UnityEngine.Object.Destroy(transform2.gameObject);
						}
					}
				}
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00072494 File Offset: 0x00070694
		public void SetTemporaryActions(AutoActions aa)
		{
			base.autoActions = aa;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0007249D File Offset: 0x0007069D
		public static SpaceShip Get(string name)
		{
			return SpaceShip.allShips[name];
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x000724AA File Offset: 0x000706AA
		public static bool SpaceShipExists(string name)
		{
			return SpaceShip.allShips.ContainsKey(name);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x000724B7 File Offset: 0x000706B7
		public static Dictionary<string, SpaceShip> GetAll()
		{
			return SpaceShip.allShips;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000724C0 File Offset: 0x000706C0
		public static void LoadAll()
		{
			SpaceShip.allShips.Clear();
			SpaceShip[] array = Resources.LoadAll<SpaceShip>("SpaceShips");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].identifier = array[i].name;
				SpaceShip.allShips[array[i].identifier] = array[i];
				foreach (SpaceShip spaceShip in SpaceShip.allShips.Values)
				{
					bool flag = spaceShip.HasModuleSlot(EquipmentSlot.Armor);
					bool flag2 = spaceShip.HasModuleSlot(EquipmentSlot.ShieldGenerator);
					if (spaceShip.armorHPScale == 0f && flag)
					{
						Debug.LogError("SpaceShip " + spaceShip.displayName + " heeft wel een armor slot, maar Armor Scale == 0");
					}
					if (spaceShip.shieldHPScale == 0f && flag2)
					{
						Debug.LogError("SpaceShip " + spaceShip.displayName + " heeft wel een shield slot, maar Shield Scale == 0");
					}
				}
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x000725C8 File Offset: 0x000707C8
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			SalvageContainer salvageContainer;
			if (base.TryGetComponent<SalvageContainer>(out salvageContainer))
			{
				salvageContainer.AddTooltipCustomContent(tooltip);
				return;
			}
			if (this.spaceShipData.commanderData == null)
			{
				tooltip.AddHeader(this.displayName, base.level, 0, 12, 8f);
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(UnitRankHelper.GetRankName(base.unitData.unitRank).HighlightWithColor(UnitRankHelper.GetColor(base.unitData.unitRank)), 12, 8f);
			}
			else if (this.spaceShipData.commanderData != null)
			{
				string text = this.spaceShipData.customShipName;
				if (base.faction != null)
				{
					text = FactionInfo.GetAbbreviation(base.faction) + " " + text;
				}
				tooltip.AddHeader(text, base.level, 0, 12, 8f).Item1.Text.color = ColorHelper.detailsColor;
				tooltip.AddSeparator(null);
				string typeName = this.shipRoleType.GetTypeName();
				string str = base.displayName + "-Class " + typeName;
				string str2 = UnitRankHelper.GetRankName(base.unitData.unitRank).HighlightWithColor(UnitRankHelper.GetColor(base.unitData.unitRank));
				tooltip.AddTextLine(str2 + " " + str, 12, 8f);
				if (this.spaceShipData.commanderData.lastName == null)
				{
					tooltip.AddTextLine(this.spaceShipData.commanderData.firstName, 12, 8f).Text.color = ColorHelper.boringGrey;
				}
				else
				{
					tooltip.AddTextLine("Cpt. " + this.spaceShipData.commanderData.GetFullName(), 12, 8f).Text.color = ColorHelper.boringGrey;
				}
			}
			if (base.faction != null)
			{
				string text2 = Translation.Translate(base.faction.name, Array.Empty<object>());
				tooltip.AddTextLine(text2, 12, 8f).Text.color = (base.IsPlayerEnemy() ? ColorHelper.reddish : ColorHelper.greenish);
				if (base.CanBeForceFired())
				{
					tooltip.AddTextLine("@TooltipForceAttack", 12, 8f);
				}
			}
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0007280A File Offset: 0x00070A0A
		public static IEnumerable<SpaceShip> GetNPCShipTypes(int level, int minPoints, int maxPoints, GameplayType? gType)
		{
			foreach (SpaceShip spaceShip in SpaceShip.allShips.Values)
			{
				if (spaceShip.pointValue >= minPoints && spaceShip.pointValue <= maxPoints && SpaceShip.IsNPCShipAvailable(level, spaceShip, gType))
				{
					yield return spaceShip;
				}
			}
			Dictionary<string, SpaceShip>.ValueCollection.Enumerator enumerator = default(Dictionary<string, SpaceShip>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0007282F File Offset: 0x00070A2F
		public static bool IsNPCShipAvailable(int level, SpaceShip ship, GameplayType? activity)
		{
			return activity == null || activity.Value == ship.shipRoleType.GetGameplayType();
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00072850 File Offset: 0x00070A50
		public static implicit operator string(SpaceShip iit)
		{
			return iit.identifier;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00072858 File Offset: 0x00070A58
		public static implicit operator SpaceShip(string id)
		{
			return SpaceShip.Get(id);
		}

		// Token: 0x0400092A RID: 2346
		public static Dictionary<string, SpaceShip> allShips = new Dictionary<string, SpaceShip>();

		// Token: 0x04000932 RID: 2354
		[FieldColor(0f, 0f, 0.8f, 0.15f, 1f, 0.8f, 0.8f)]
		public float baseMaxWarpSpeed;

		// Token: 0x04000933 RID: 2355
		[FieldColor(0f, 0f, 0.8f, 0.15f, 1f, 0.8f, 0.8f)]
		public float baseWarpAcceleration;

		// Token: 0x04000934 RID: 2356
		private List<CrewMember> crewMembers = new List<CrewMember>();

		// Token: 0x04000936 RID: 2358
		private Dictionary<AbilityTrigger, List<TriggeredAbility>> triggeredAbilities = new Dictionary<AbilityTrigger, List<TriggeredAbility>>();

		// Token: 0x0400093B RID: 2363
		[SerializeField]
		private TravelEffect travelEffectPrefab;

		// Token: 0x0400093C RID: 2364
		private TravelEffect travelEffect;

		// Token: 0x0400093F RID: 2367
		public TargetLayer preferredTargetLayer;

		// Token: 0x04000940 RID: 2368
		private GameplayType preferredGameplayType;

		// Token: 0x04000941 RID: 2369
		private float travelAngleTimer;

		// Token: 0x04000944 RID: 2372
		private Sprite _cleanOriginalSprite;

		// Token: 0x04000945 RID: 2373
		private DroneBayModule[] _cachedDroneBayModules;

		// Token: 0x04000946 RID: 2374
		private Door[] _cachedDoors;
	}
}
