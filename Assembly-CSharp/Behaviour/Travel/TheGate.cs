using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Effects;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Travel
{
	// Token: 0x020002CC RID: 716
	public class TheGate : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x000A272A File Offset: 0x000A092A
		// (set) Token: 0x06001A21 RID: 6689 RVA: 0x000A2732 File Offset: 0x000A0932
		public TravelDirection travelDirection { get; private set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x000A273B File Offset: 0x000A093B
		// (set) Token: 0x06001A23 RID: 6691 RVA: 0x000A2743 File Offset: 0x000A0943
		public SpaceShip jumpingShip { get; private set; }

		// Token: 0x06001A24 RID: 6692 RVA: 0x000A274C File Offset: 0x000A094C
		private void Start()
		{
			base.transform.Z(ZIndex.Jumpgate);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x000A275C File Offset: 0x000A095C
		public void SetJumpGatePoi(JumpGate jumpGate)
		{
			this.jumpGatePoi = jumpGate;
			this.travelDirection = this.jumpGatePoi.GetTravelDirection();
			if (GameplayManager.Instance.spaceShip && GameplayManager.Instance.spaceShip.jumpingProcedureEngaged)
			{
				this.jumpingShip = GameplayManager.Instance.spaceShip;
				Debug.Log("Setting jumping ship to player: " + this.jumpingShip);
				this.CheckActivation();
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000A27D4 File Offset: 0x000A09D4
		public void CheckActivation()
		{
			this.activated = ((this.jumpingShip.IsPlayer(false) && GamePlayer.current.nextWaypointIsSystem(this.jumpGatePoi.targetSystemGuid)) || this.jumpingShip.jumpingProcedureEngaged || !this.jumpingShip.IsPlayer(false));
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x000A2830 File Offset: 0x000A0A30
		public void SetJumpingShip(SpaceShip spaceShip, JumpAction jumpAction)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Set jumping ship: ",
				spaceShip,
				" / ",
				jumpAction.ToString(),
				", isplayer: ",
				spaceShip.IsPlayer(false).ToString()
			}));
			this.jumpingShip = spaceShip;
			base.StopAllCoroutines();
			if (jumpAction == JumpAction.Leaving)
			{
				base.StartCoroutine(this.MoveToJumpGatePoint());
				return;
			}
			base.StartCoroutine(this.ArriveAtGate(spaceShip));
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x000A28BC File Offset: 0x000A0ABC
		public void ClearJumpingShip()
		{
			this.jumpingShip = null;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000A28C5 File Offset: 0x000A0AC5
		private IEnumerator MoveToJumpGatePoint()
		{
			TheGate.ClosureClass21_0 _locals_1 = new TheGate.ClosureClass21_0();
			_locals_1._thisRef = this;
			yield return this.jumpingShip.WaitForDrones(true);
			_locals_1.gatePos = base.transform.position;
			TheGate.ClosureClass21_0 _locals_2 = _locals_1;
			_locals_2.gatePos.x = _locals_2.gatePos.x + (float)((this.travelDirection == TravelDirection.Right) ? -5 : 5);
			SpaceShip jumpingShip = this.jumpingShip;
			if (jumpingShip != null)
			{
				jumpingShip.SetOverrideDestination(_locals_1.gatePos, true, true, false);
			}
			yield return new WaitUntil(() => _locals_1._thisRef.jumpingShip == null || Vector2.Distance(_locals_1._thisRef.jumpingShip.rigidbody.position, _locals_1.gatePos) < 3f);
			this.TravelThroughGate();
			yield break;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000A28D4 File Offset: 0x000A0AD4
		private void TravelThroughGate()
		{
			if (!this.jumpingShip)
			{
				return;
			}
			Vector2 position = base.transform.position;
			position.x += (float)((this.travelDirection == TravelDirection.Right) ? 50 : -50);
			this.CheckActivation();
			this.jumpingShip.SetOverrideDestination(position, true, false, false);
			if (this.jumpingShip.IsPlayer(true))
			{
				GameplayManager.Instance.SetFleetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
				return;
			}
			this.jumpingShip.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000A2956 File Offset: 0x000A0B56
		public IEnumerator ChargeFastLaneTravelToNextGate(SpaceShip spaceShip, JumpGate nextPoi)
		{
			this.jumpingShip = spaceShip;
			this.attractionSpeedModifier = 3f;
			this.effect.SetParticleColor(this.fastLaneChargeColor);
			yield return new WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x000A296C File Offset: 0x000A0B6C
		public IEnumerator ArriveAtGate(SpaceShip spaceShip)
		{
			Debug.Log("Set jumping ship arrive at gate: " + spaceShip + ", is player: " + spaceShip.IsPlayer(false).ToString());
			this.jumpingShip = spaceShip;
			this.CheckActivation();
			if (this.jumpingShip.IsPlayer(true))
			{
				GameplayManager.Instance.GiveFleetJumpGateImpulse(this.travelDirection);
			}
			else
			{
				this.jumpingShip.GiveJumpGateImpulse(this.travelDirection);
			}
			yield return new WaitForSeconds(0.5f);
			if (this.jumpingShip.IsPlayer(false))
			{
				LocationManager.instance.ShowText(0.3f);
			}
			yield return new WaitForSeconds(2f);
			if (this.jumpingShip.IsPlayer(true))
			{
				GameplayManager.Instance.SetFleetEngineState(true, true);
				GameplayManager.Instance.SetFleetTurretState(true);
				GameplayManager.Instance.SetFleetMaskInteraction(SpriteMaskInteraction.None);
			}
			else
			{
				this.jumpingShip.SetEngineState(true, true);
				this.jumpingShip.SetTurretState(true);
				this.jumpingShip.SetMaskInteraction(SpriteMaskInteraction.None);
			}
			this.jumpingShip.jumpingProcedureEngaged = false;
			this.DetermineDestination();
			if (!this.jumpingShip.IsPlayer(false))
			{
				this.jumpingShip.autoActions.ExitPOI();
				JumpGateManager.instance.RemoveJumpingSpaceShip(this.jumpingShip, false);
			}
			this.jumpingShip = null;
			yield break;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x000A2984 File Offset: 0x000A0B84
		private void DetermineDestination()
		{
			if (!this.jumpingShip.IsPlayer(false) || (this.jumpingShip.IsPlayer(false) && !GamePlayer.current.HasWaypointsAfterCurrent()))
			{
				this.jumpingShip.SetOverrideDestination(JumpGateManager.instance.GetArrivalDestination(), true, false, false);
			}
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x000A29D4 File Offset: 0x000A0BD4
		private void Update()
		{
			if (this.jumpingShip == null && this.effect)
			{
				this.effect.StopFollowingShip();
			}
			if (this.jumpingShip == null || this.jumpGatePoi == null || !BasePoiManager.current.initializedAndReady)
			{
				return;
			}
			this.SetLockedStatusLight();
			bool flag = !this.jumpingShip.IsPlayer(false) || (GamePlayer.current.nextWaypointIsSystem(this.jumpGatePoi.targetSystemGuid) && this.jumpGatePoi.canUseJumpGate);
			if (!this.jumpingShip.jumpingProcedureEngaged && flag && this.jumpingShip.CompletelyCrossedThreshold(base.transform.position.x, this.travelDirection == TravelDirection.Right))
			{
				Debug.Log(this.jumpingShip + " -- Crossed threshold, jumping! " + this.travelDirection.ToString());
				if (this.jumpingShip.IsPlayer(true))
				{
					GameplayManager.Instance.SetFleetEngineState(false, false);
				}
				else
				{
					this.jumpingShip.SetEngineState(false, false);
				}
				int num = (this.travelDirection == TravelDirection.Right) ? 1 : -1;
				this.jumpingShip.GiveImpulse(new Vector2((float)num * this.jumpingShip.rigidbody.mass * 3f, 0f), 0f, 0f);
				if (this.jumpingShip.IsPlayer(false))
				{
					this.jumpingShip.jumpingProcedureEngaged = true;
					Singleton<TravelManager>.Instance.JumpToPOIFrom(this.jumpGatePoi);
				}
				else
				{
					Debug.Log("Removing: " + this.jumpingShip);
					this.jumpGatePoi.RemoveUnit(this.jumpingShip.spaceShipData);
					JumpGateManager.instance.RemoveJumpingSpaceShip(this.jumpingShip, true);
				}
			}
			this.UpdateEffect();
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000A2BB8 File Offset: 0x000A0DB8
		private void UpdateEffect()
		{
			Vector2 vector = this.jumpingShip.transform.position;
			float num = this.jumpingShip.GetBoundsX();
			num = Mathf.Clamp(num, 3f, 20f);
			float num2 = -0.1f;
			if (this.CloseEnoughToGate(vector, num * 3f))
			{
				Vector2 vector2 = vector - (Vector2)base.transform.position;
				vector2 = ((this.travelDirection == TravelDirection.Right) ? vector2 : (-vector2));
				this.effect.SetShipPosition(vector2, num);
			}
			else
			{
				this.effect.StopFollowingShip();
			}
			if (this.CloseEnoughToGate(vector, num * 2f))
			{
				float num3 = Mathf.Abs(base.transform.position.x - vector.x);
				num2 = -(6f - num3) * 10f;
			}
			this.effect.SetAttractionSpeed(num2 * this.attractionSpeedModifier);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x000A2CA3 File Offset: 0x000A0EA3
		private bool CloseEnoughToGate(Vector2 shipPosition, float distance)
		{
			if (this.travelDirection != TravelDirection.Right)
			{
				return shipPosition.x < base.transform.position.x + distance;
			}
			return shipPosition.x > base.transform.position.x - distance;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000A2CE4 File Offset: 0x000A0EE4
		private void SetLockedStatusLight()
		{
			bool flag = true;
			if (this.jumpingShip.IsPlayer(false))
			{
				flag = this.jumpGatePoi.canUseJumpGate;
			}
			if (flag)
			{
				using (List<Light2D>.Enumerator enumerator = this.lights.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Light2D light2D = enumerator.Current;
						light2D.color = ColorHelper.greenish;
					}
					return;
				}
			}
			foreach (Light2D light2D2 in this.lights)
			{
				light2D2.color = ColorHelper.reddish;
			}
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x000A2D9C File Offset: 0x000A0F9C
		private void OnMouseDown()
		{
			if (this.jumpGatePoi.targetPoiGuid == null || !UIHelper.clickTargetingAvailable)
			{
				return;
			}
			if (!this.jumpGatePoi.canUseJumpGate)
			{
				string text = "@TravelGateLocked";
				if (this.jumpGatePoi.sectorJumpgate)
				{
					SystemMapData targetSystem = this.jumpGatePoi.targetSystem;
					if (((targetSystem != null) ? targetSystem.level : 0) < 55 || GamePlayer.current.level >= 55)
					{
						text = "@TravelSectorGateLocked";
					}
				}
				using (IEnumerator<Inventory.InventoryItem> enumerator = GamePlayer.current.currentSpaceShip.cargo.items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JumpgatePassItem jumpgatePassItem;
						if (enumerator.Current.item.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem))
						{
							string jumpgateGuid = jumpgatePassItem.jumpgateGuid;
							MapPointOfInterest current = MapPointOfInterest.current;
							if (jumpgateGuid == ((current != null) ? current.guid : null))
							{
								text = "@TravelGateLockedPass";
								break;
							}
						}
					}
				}
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate(text, Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(6f).Show();
				return;
			}
			if (this.jumpGatePoi.targetSystem.name == "Canis Majoris")
			{
				AlertPopup.ShowQuery("Are you sure you want to leave the prologue area? \n\n- You will not be able to return.\n- Items stored in the Materials tab will be lost.\n- Refined materials will be lost if you don't take them with you.\n", null, null, delegate
				{
					Singleton<TravelManager>.Instance.SetRouteToPOI(this.jumpGatePoi.GetTargetPOI());
				}, null, null, null);
				return;
			}
			Singleton<TravelManager>.Instance.SetRouteToPOI(this.jumpGatePoi.GetTargetPOI());
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x000A2F04 File Offset: 0x000A1104
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			SystemMapData targetSystem = this.jumpGatePoi.targetSystem;
			if (targetSystem != null)
			{
				if (this.jumpGatePoi.sectorJumpgate)
				{
					tooltip.AddTextLine(Translation.TranslateOnly("@Gate2Sector", new object[]
					{
						targetSystem.sector.name,
						targetSystem.name
					}), 12, 8f);
				}
				else
				{
					tooltip.AddTextLine(Translation.TranslateOnly("@Gate2", new object[]
					{
						targetSystem.name
					}), 12, 8f);
				}
				tooltip.AddSeparator(null);
			}
			string text = Translation.Translate("@Locked", Array.Empty<object>());
			Color color = ColorHelper.reddish;
			bool flag = true;
			if (this.jumpGatePoi.canUseJumpGate)
			{
				flag = false;
				text = Translation.Translate("@Open", Array.Empty<object>());
				color = ColorHelper.greenish;
			}
			tooltip.AddTextLine(Translation.Translate("@GateStatus", Array.Empty<object>()) + ": " + text.HighlightWithColor(color), 12, 8f);
			if (!flag)
			{
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@ClickToJump", Array.Empty<object>()), 12, 8f);
			}
		}

		// Token: 0x0400106E RID: 4206
		[SerializeField]
		private JumpGateEffect effect;

		// Token: 0x0400106F RID: 4207
		[SerializeField]
		private List<Light2D> lights;

		// Token: 0x04001070 RID: 4208
		[SerializeField]
		private SpriteMask mask;

		// Token: 0x04001071 RID: 4209
		[SerializeField]
		private JumpgateTractor jumpgateTractor;

		// Token: 0x04001072 RID: 4210
		[SerializeField]
		private Gradient fastLaneChargeColor;

		// Token: 0x04001073 RID: 4211
		private bool activated;

		// Token: 0x04001074 RID: 4212
		private JumpGate jumpGatePoi;

		// Token: 0x04001077 RID: 4215
		private float attractionSpeedModifier = 1f;

		private sealed class ClosureClass21_0
		{
			public TheGate _thisRef;
			public Vector3 gatePos;
		}
	}
}
