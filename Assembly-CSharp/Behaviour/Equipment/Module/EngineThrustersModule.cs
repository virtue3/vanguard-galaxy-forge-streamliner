using System;
using System.Linq;
using Behaviour.Effects;
using Behaviour.Unit;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000362 RID: 866
	public class EngineThrustersModule : AbstractModule
	{
		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x000C133A File Offset: 0x000BF53A
		public float thrust
		{
			get
			{
				return this._thrust * this.thrustMultiplier;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06002119 RID: 8473 RVA: 0x000C1349 File Offset: 0x000BF549
		public float sideThrust
		{
			get
			{
				return this._sideThrust * this.thrustMultiplier * (1f + this.GetStat(EquipStat.SideThrust));
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x000C1367 File Offset: 0x000BF567
		public float rotationalThrust
		{
			get
			{
				return this._rotationalThrust * this.thrustMultiplier * (1f + this.GetStat(EquipStat.RotationalThrust));
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x000C1385 File Offset: 0x000BF585
		// (set) Token: 0x0600211C RID: 8476 RVA: 0x000C138D File Offset: 0x000BF58D
		public Rigidbody2D rigidbody { get; protected set; }

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x000C1396 File Offset: 0x000BF596
		// (set) Token: 0x0600211E RID: 8478 RVA: 0x000C139E File Offset: 0x000BF59E
		public float radius { get; protected set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x000C13A7 File Offset: 0x000BF5A7
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Engine;
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000C13AC File Offset: 0x000BF5AC
		protected override void Update()
		{
			base.Update();
			this.thrustTimer -= Time.deltaTime;
			if (this.thrustTimer < 0f)
			{
				this.thrustTimer = 0.5f;
				float stat = this.GetStat(EquipStat.Thrust);
				int num = Mathf.RoundToInt(25f * GameMath.DamageMultiplier((float)base.parent.unitData.level));
				if (num == 0)
				{
					return;
				}
				this.thrustMultiplier = 1f;
				if (stat < (float)num)
				{
					this.thrustMultiplier = Mathf.Max(0.8f, Mathf.Sqrt(stat / (float)num));
					return;
				}
				this.thrustMultiplier += Mathf.Sqrt(stat / (float)num);
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000C1458 File Offset: 0x000BF658
		public void SetThrusters()
		{
			ThrusterEffect[] componentsInChildren = base.parent.GetComponentsInChildren<ThrusterEffect>();
			this.mainThrusters = (from te in componentsInChildren
			where te.isMain
			select te).ToArray<ThrusterEffect>();
			ThrusterEffect[] array = this.mainThrusters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetSoundModifier(base.parent.IsPlayer(false));
			}
			foreach (SideThrusterEffect sideThrusterEffect in base.parent.GetComponentsInChildren<SideThrusterEffect>())
			{
				switch (sideThrusterEffect.GetSide())
				{
				case Side.TopLeft:
					this.topLeftThruster = sideThrusterEffect;
					this.topLeftThruster.SetSoundModifier(base.parent.IsPlayer(false));
					break;
				case Side.BottomLeft:
					this.bottomLeftThruster = sideThrusterEffect;
					this.bottomLeftThruster.SetSoundModifier(base.parent.IsPlayer(false));
					break;
				case Side.TopRight:
					this.topRightThruster = sideThrusterEffect;
					this.topRightThruster.SetSoundModifier(base.parent.IsPlayer(false));
					break;
				case Side.BottomRight:
					this.bottomRightThruster = sideThrusterEffect;
					this.bottomRightThruster.SetSoundModifier(base.parent.IsPlayer(false));
					break;
				}
			}
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000C1596 File Offset: 0x000BF796
		public void SetNoSlowdown()
		{
			this.slowDownAllowed = false;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000C15A0 File Offset: 0x000BF7A0
		public void ApplyManualThrust(Vector2 manualInput, float rotate)
		{
			Vector2 vector = new Vector2(manualInput.y, -manualInput.x);
			Vector2 relativeForce = new Vector2(Mathf.Clamp(vector.x, -0.3f, 1f) * this.thrust, Mathf.Clamp(vector.y, -0.5f, 0.5f) * this.sideThrust);
			float num = Vector2.Dot(this.rigidbody.linearVelocity.normalized, base.transform.right);
			bool flag = false;
			if (this.rigidbody.linearVelocity.magnitude < base.parent.maxSpeed || num < -0.5f)
			{
				this.rigidbody.AddRelativeForce(relativeForce);
				flag = true;
			}
			if (rotate != 0f)
			{
				this.rigidbody.AddTorque(-rotate * this.rotationalThrust);
				this.SetSideThrustersForRotation(-rotate, 1f);
			}
			else if (flag)
			{
				this.SetSideThrusters(-manualInput.x);
			}
			if (flag)
			{
				this.SetThrusterPower(manualInput.y);
			}
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000C16B0 File Offset: 0x000BF8B0
		public void ForwardThrust(Vector2 vectorToDestination, Vector2 targetSpeedVector, bool braking)
		{
			float forwardSpeed = this.GetForwardSpeed();
			Vector2 vector = this.rigidbody.transform.right;
			float num = Vector2.Dot(targetSpeedVector.normalized, vector) * targetSpeedVector.magnitude;
			this.forwardThrottle = this.ApplyDirectionalThrust(vectorToDestination, vector, forwardSpeed - num, this.thrust, "MOVE", Vector2.right, this.slowDownAllowed, this.forwardThrottle, braking);
			this.SetThrusterPower(Mathf.Abs(this.forwardThrottle));
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000C1730 File Offset: 0x000BF930
		public float GetForwardSpeed()
		{
			Vector2 rhs = this.rigidbody.transform.right;
			return Vector2.Dot(this.rigidbody.linearVelocity.normalized, rhs) * this.rigidbody.linearVelocity.magnitude;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000C1780 File Offset: 0x000BF980
		public void SetThrusterPower(float power)
		{
			ThrusterEffect[] array = this.mainThrusters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetPowerFactor(power);
			}
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000C17AC File Offset: 0x000BF9AC
		public void SideThrust(Vector2 vectorToDestination, bool applySidewaysMovement)
		{
			Vector2 vector = this.rigidbody.transform.up;
			float num = Vector2.Dot(this.rigidbody.linearVelocity.normalized, vector);
			float num2 = this.rigidbody.linearVelocity.magnitude * num;
			if (num2 != 0f && !applySidewaysMovement)
			{
				if (this.debugThisShit)
				{
					Debug.Log(string.Concat(new string[]
					{
						base.parent.name,
						" SIDE -- normalized: ",
						num.ToString(),
						", sidespeed: ",
						num2.ToString()
					}));
				}
				float num3 = this.sideThrust / this.rigidbody.mass;
				float num4 = -num2 / num3;
				num4 = Mathf.Clamp(num4, -1f, 1f);
				if (this.debugThisShit)
				{
					string[] array = new string[9];
					array[0] = base.parent.name;
					array[1] = " SIDE -- thrustFactor: ";
					array[2] = num4.ToString();
					array[3] = ", impulse: ";
					int num5 = 4;
					Vector2 vector2 = vector;
					array[num5] = vector2.ToString();
					array[5] = ", accel";
					array[6] = num3.ToString();
					array[7] = ", side speed ";
					array[8] = num2.ToString();
					Debug.Log(string.Concat(array));
				}
				this.rigidbody.AddRelativeForce(Vector2.up * num4 * this.sideThrust);
				this.SetSideThrusters(num4);
				return;
			}
			this.sideThrottle = this.ApplyDirectionalThrust(vectorToDestination, vector, num2, this.sideThrust, "SIDE", Vector2.up, true, this.sideThrottle, false);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000C1954 File Offset: 0x000BFB54
		private float ApplyDirectionalThrust(Vector2 vectorToDestination, Vector2 impulseVector, float currentSpeed, float thrustForDirection, string debug, Vector2 localImpulseVector, bool slowDownAllowed, float throttle, bool braking = false)
		{
			float num = Vector2.Dot(vectorToDestination.normalized, impulseVector);
			float num2 = Mathf.Abs(num * vectorToDestination.magnitude);
			float num3 = (currentSpeed > 0.2f || currentSpeed < -0.2f) ? Mathf.Abs(currentSpeed) : 0f;
			float num4 = thrustForDirection / this.rigidbody.mass;
			float num5 = thrustForDirection * throttle / this.rigidbody.mass;
			float num6 = Mathf.Abs(Mathf.Pow(currentSpeed, 2f) / (2f * ((throttle != 0f) ? num5 : num4))) + num3;
			float num7 = (float)((currentSpeed == 0f || num > 0f) ? 1 : -1);
			bool flag = Mathf.Abs(num2) < 0.01f;
			bool flag2 = Mathf.Abs(currentSpeed) < 0.01f;
			if (this.debugThisShit)
			{
				string[] array = new string[21];
				array[0] = base.parent.name;
				array[1] = " ";
				array[2] = debug;
				array[3] = " -- impulseVector: ";
				int num8 = 4;
				Vector2 vector = impulseVector;
				array[num8] = vector.ToString();
				array[5] = ", vectorToDest: ";
				int num9 = 6;
				vector = vectorToDestination;
				array[num9] = vector.ToString();
				array[7] = ", vectorToDestNorm: ";
				array[8] = vectorToDestination.normalized.ToString();
				array[9] = ", normalized: ";
				array[10] = num.ToString();
				array[11] = ", distance: ";
				array[12] = num2.ToString();
				array[13] = ", to stop: ";
				array[14] = num6.ToString();
				array[15] = ", directionCounter: ";
				array[16] = num7.ToString();
				array[17] = ", accel: ";
				array[18] = num4.ToString();
				array[19] = " current speed: ";
				array[20] = currentSpeed.ToString();
				Debug.Log(string.Concat(array));
			}
			if (slowDownAllowed && num2 < num6 && !EngineThrustersModule.MovingInOpositeDirection(num > 0f, currentSpeed))
			{
				num7 = -num7;
				if (this.debugThisShit)
				{
					Debug.Log(base.parent.name + " " + debug + " -- Breaking");
				}
			}
			if (flag && flag2)
			{
				if (this.debugThisShit)
				{
					Debug.Log(string.Concat(new string[]
					{
						base.parent.name,
						" ",
						debug,
						" -- CloseEnough: ",
						flag.ToString(),
						", slowEnough: ",
						flag2.ToString()
					}));
				}
				throttle = 0f;
			}
			else if ((num2 > num4 && num2 > num6) || currentSpeed > num4)
			{
				if (this.debugThisShit)
				{
					Debug.Log(base.parent.name + " " + debug + " -- Setting throttle to 1");
				}
				throttle = 1f;
			}
			else if (currentSpeed < num4 && num2 < num4 * 3f)
			{
				throttle = 0.3f;
				if (this.debugThisShit)
				{
					Debug.Log(string.Concat(new string[]
					{
						base.parent.name,
						" ",
						debug,
						" -- Setting throttle to ",
						throttle.ToString(),
						" distanceToTarget / accelerationPerSecond: ",
						(num2 / num4).ToString()
					}));
				}
			}
			else
			{
				throttle = 1f;
				if (this.debugThisShit)
				{
					Debug.Log(base.parent.name + " " + debug + " -- Setting throttle to 1 because else... :D ");
				}
			}
			throttle *= num7;
			if (this.debugThisShit)
			{
				Debug.Log(string.Concat(new string[]
				{
					base.parent.name,
					" ",
					debug,
					" -- adding thrust: ",
					(thrustForDirection * throttle).ToString(),
					", thrustForDirection: ",
					thrustForDirection.ToString(),
					", speedInDirection: ",
					currentSpeed.ToString(),
					", throttle: ",
					throttle.ToString(),
					", directionCounter: ",
					num7.ToString()
				}));
			}
			if (this.debugThisShit)
			{
				string[] array2 = new string[11];
				array2[0] = base.parent.name;
				array2[1] = " ";
				array2[2] = debug;
				array2[3] = " -- AddRelativeForce: ";
				array2[4] = (localImpulseVector * thrustForDirection * throttle).ToString();
				array2[5] = ", local: ";
				int num10 = 6;
				Vector2 vector = localImpulseVector;
				array2[num10] = vector.ToString();
				array2[7] = " ";
				array2[8] = thrustForDirection.ToString();
				array2[9] = " ";
				array2[10] = throttle.ToString();
				Debug.Log(string.Concat(array2));
			}
			if (base.parent.takingDamage && debug == "SIDE" && !braking && currentSpeed < 0.5f)
			{
				throttle = Mathf.Max(0.3f, throttle) * (float)(base.parent.evadeLeft ? -1 : 1);
			}
			Drone drone = base.parent as Drone;
			if (drone != null && drone.keepMoving && !drone.isReturning && !braking)
			{
				throttle = Mathf.Max(0.02f * Time.fixedDeltaTime, throttle);
			}
			this.rigidbody.AddRelativeForce(localImpulseVector * thrustForDirection * throttle);
			if (debug == "SIDE")
			{
				this.SetSideThrusters(throttle);
			}
			return throttle;
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000C1EB8 File Offset: 0x000C00B8
		private void SetSideThrusters(float throttle)
		{
			float num = Mathf.Abs(throttle);
			if (!base.active || num == 0f)
			{
				this.SetSideThrusterEffectPower(0f, 0f, 0f, 0f);
				return;
			}
			if (throttle < 0f)
			{
				this.SetSideThrusterEffectPower(num, num, 0f, 0f);
				return;
			}
			if (throttle > 0f)
			{
				this.SetSideThrusterEffectPower(0f, 0f, num, num);
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000C1F2C File Offset: 0x000C012C
		private void SetSideThrusterEffectPower(float topLeft, float bottomLeft, float topRight, float bottomRight)
		{
			this.topLeftThruster.SetPowerFactor(topLeft);
			this.bottomLeftThruster.SetPowerFactor(bottomLeft);
			this.topRightThruster.SetPowerFactor(topRight);
			this.bottomRightThruster.SetPowerFactor(bottomRight);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000C1F5F File Offset: 0x000C015F
		private static bool MovingInOpositeDirection(bool forward, float forwardSpeed)
		{
			return (!forward && forwardSpeed > 0f) || (forward && forwardSpeed < 0f);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000C1F7C File Offset: 0x000C017C
		public void RotateShip(float angle)
		{
			if (Mathf.Abs(angle) < 0.5f)
			{
				return;
			}
			bool flag = angle < 0f;
			bool flag2 = false;
			float num = 1f;
			if ((flag && this.rigidbody.angularVelocity < 0f) || (!flag && this.rigidbody.angularVelocity > 0f))
			{
				float num2 = Mathf.Abs(this.rigidbody.angularVelocity * this.rigidbody.inertia);
				this.calculatedRotationthrust = (flag ? this.rotationalThrust : (-this.rotationalThrust));
				this.calculatedRotationthrust = Mathf.Clamp(this.calculatedRotationthrust, -num2, num2);
				if (this.debugThisShit)
				{
					Debug.Log(string.Concat(new string[]
					{
						base.parent.name,
						" ROTATE -- moving opposite direction, braking: ",
						this.calculatedRotationthrust.ToString(),
						", angVel: ",
						this.rigidbody.angularVelocity.ToString()
					}));
				}
				this.rigidbody.AddTorque(this.calculatedRotationthrust);
				this.SetSideThrustersForRotation(this.calculatedRotationthrust, 1f);
				return;
			}
			if (this.rigidbody.angularVelocity != 0f)
			{
				if (this.debugThisShit)
				{
					Debug.Log(string.Concat(new string[]
					{
						base.parent.name,
						" ROTATE -- check slowdown factor: rotationThrust: ",
						this.rotationalThrust.ToString(),
						", mass: ",
						this.rigidbody.mass.ToString(),
						", radius: ",
						this.radius.ToString(),
						", angle: ",
						angle.ToString()
					}));
				}
				flag2 = PhysicsCalculations.CalculateRotationalThrustFactor(angle, ref num, this.rigidbody, this.rotationalThrust);
			}
			this.calculatedRotationthrust = (flag ? this.rotationalThrust : (-this.rotationalThrust)) * num;
			this.calculatedRotationthrust = (flag2 ? (-this.calculatedRotationthrust) : this.calculatedRotationthrust);
			if (flag2 && this.rigidbody.angularVelocity != 0f)
			{
				float num3 = Mathf.Abs(this.rigidbody.angularVelocity * this.rigidbody.inertia);
				this.calculatedRotationthrust = Mathf.Clamp(this.calculatedRotationthrust, -num3, num3);
			}
			if (this.debugThisShit)
			{
				Debug.Log(string.Concat(new string[]
				{
					base.parent.name,
					" ROTATE -- Add rotational thrust, engine: ",
					this.rotationalThrust.ToString(),
					", adding: ",
					this.calculatedRotationthrust.ToString(),
					", slowDown: ",
					flag2.ToString(),
					", slowDownFactor: ",
					num.ToString(),
					", actual toruq: ",
					(this.calculatedRotationthrust * Time.fixedDeltaTime / this.rigidbody.mass).ToString(),
					" ang vel: ",
					this.rigidbody.angularVelocity.ToString()
				}));
			}
			this.rigidbody.AddTorque(this.calculatedRotationthrust);
			this.SetSideThrustersForRotation(this.calculatedRotationthrust, num);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000C22C0 File Offset: 0x000C04C0
		private void SetSideThrustersForRotation(float thrust, float slowDownFactor = 1f)
		{
			if (thrust > 0f)
			{
				this.SetSideThrusterEffectPower(0f, Mathf.Abs(slowDownFactor), Mathf.Abs(slowDownFactor), 0f);
				return;
			}
			if (thrust < 0f)
			{
				this.SetSideThrusterEffectPower(slowDownFactor, 0f, 0f, slowDownFactor);
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000C230C File Offset: 0x000C050C
		public float GetForwardAccelerationPerSecond()
		{
			return this.thrust / this.rigidbody.mass;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000C2320 File Offset: 0x000C0520
		public void SetRadius(float radius)
		{
			this.radius = radius;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000C2329 File Offset: 0x000C0529
		public void SetRigidBody(Rigidbody2D rigidbody)
		{
			this.rigidbody = rigidbody;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000C2334 File Offset: 0x000C0534
		public void SetThrusterState(bool isEnabled)
		{
			base.active = isEnabled;
			ThrusterEffect[] array = this.mainThrusters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetPowerFactor(0f);
			}
			this.topLeftThruster.gameObject.SetActive(isEnabled);
			this.bottomLeftThruster.gameObject.SetActive(isEnabled);
			this.topRightThruster.gameObject.SetActive(isEnabled);
			this.bottomRightThruster.gameObject.SetActive(isEnabled);
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000C23B0 File Offset: 0x000C05B0
		public override MainStat GetMainStat()
		{
			EquipStatLine? equipStatLine;
			return new MainStat("Thrust", (base.GetStatLine(EquipStat.Thrust) != null) ? equipStatLine.GetValueOrDefault().amount : 0f);
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000C23EC File Offset: 0x000C05EC
		protected override void SetMainSubStats()
		{
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000C23EE File Offset: 0x000C05EE
		public void SetDummyThrust()
		{
			this._thrust = 300f;
		}

		// Token: 0x0400139F RID: 5023
		[Header("Module Specific")]
		[SerializeField]
		private float _thrust;

		// Token: 0x040013A0 RID: 5024
		private float thrustMultiplier = 1f;

		// Token: 0x040013A1 RID: 5025
		[SerializeField]
		protected float _sideThrust;

		// Token: 0x040013A2 RID: 5026
		[SerializeField]
		protected float _rotationalThrust;

		// Token: 0x040013A3 RID: 5027
		protected float forwardThrottle;

		// Token: 0x040013A4 RID: 5028
		protected float sideThrottle;

		// Token: 0x040013A5 RID: 5029
		protected float rotationThrottle;

		// Token: 0x040013A6 RID: 5030
		protected float throttleIncrement = 0.01f;

		// Token: 0x040013A7 RID: 5031
		public string fuelType;

		// Token: 0x040013AA RID: 5034
		protected bool slowDownAllowed = true;

		// Token: 0x040013AB RID: 5035
		protected ThrusterEffect[] mainThrusters = new ThrusterEffect[2];

		// Token: 0x040013AC RID: 5036
		protected SideThrusterEffect topLeftThruster;

		// Token: 0x040013AD RID: 5037
		protected SideThrusterEffect bottomLeftThruster;

		// Token: 0x040013AE RID: 5038
		protected SideThrusterEffect topRightThruster;

		// Token: 0x040013AF RID: 5039
		protected SideThrusterEffect bottomRightThruster;

		// Token: 0x040013B0 RID: 5040
		private float calculatedRotationthrust;

		// Token: 0x040013B1 RID: 5041
		protected bool debugThisShit;

		// Token: 0x040013B2 RID: 5042
		private float thrustTimer;
	}
}
