using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.AudioSystem;
using Behaviour.Crew;
using Behaviour.Effects;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret.Projectile;
using Behaviour.Equipment.Turret.Utility;
using Behaviour.Item;
using Behaviour.UI;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Ability;
using Source.AudioSystem;
using Source.Combat;
using Source.Item;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret
{
	// Token: 0x02000342 RID: 834
	[Serializable]
	public abstract class AbstractTurret : AbstractEquipment
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x000BBC30 File Offset: 0x000B9E30
		public float displayedPower
		{
			get
			{
				foreach (EquipStatLine equipStatLine in base.stats)
				{
					if (equipStatLine.stat == this.powerStat)
					{
						return equipStatLine.amount;
					}
				}
				return 0f;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001FB5 RID: 8117
		public abstract EquipStat powerStat { get; }

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x000BBC9C File Offset: 0x000B9E9C
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x000BBCA4 File Offset: 0x000B9EA4
		public float customApproachDistance { get; protected set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x000BBCAD File Offset: 0x000B9EAD
		public float range
		{
			get
			{
				return this._range * (1f + this.GetStat(EquipStat.WeaponRange));
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001FB9 RID: 8121 RVA: 0x000BBCC4 File Offset: 0x000B9EC4
		public float reloadDelay
		{
			get
			{
				return this._reloadDelay / (1f + this.GetStat(EquipStat.ReloadSpeed));
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x000BBCDB File Offset: 0x000B9EDB
		public float fireDelay
		{
			get
			{
				return this._fireDelay / (1f + this.GetStat(EquipStat.AttackSpeed));
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x000BBCF2 File Offset: 0x000B9EF2
		public float baseFireRate
		{
			get
			{
				return 1f / this._fireDelay;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x000BBD00 File Offset: 0x000B9F00
		public float projectileSpeed
		{
			get
			{
				return this._projectileSpeed * (1f + this.GetStat(EquipStat.ProjectileSpeed));
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x000BBD17 File Offset: 0x000B9F17
		public int maxMagSize
		{
			get
			{
				return Mathf.RoundToInt((float)this._maxMagSize * (1f + this.GetStat(EquipStat.MagazineSize)));
			}
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000BBD34 File Offset: 0x000B9F34
		public int GetMaxMagSize()
		{
			return this._maxMagSize;
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001FBF RID: 8127 RVA: 0x000BBD3C File Offset: 0x000B9F3C
		public float defaultAttacksPerSecond
		{
			get
			{
				float num = (float)(this.burstAmount - 1) * this.burstDelay + this._fireDelay;
				float num2 = (float)Mathf.CeilToInt((float)this._maxMagSize / (float)this.burstAmount) * num + this._reloadDelay;
				return (float)this._maxMagSize / num2;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x000BBD89 File Offset: 0x000B9F89
		// (set) Token: 0x06001FC1 RID: 8129 RVA: 0x000BBD91 File Offset: 0x000B9F91
		public float currentBurstAmount { get; private set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x000BBD9A File Offset: 0x000B9F9A
		public float rotationSpeed
		{
			get
			{
				return this._rotationSpeed * (1f + this.GetStat(EquipStat.TurretRotationSpeed));
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x000BBDB1 File Offset: 0x000B9FB1
		public bool canRotate
		{
			get
			{
				return this._rotationSpeed > 0f;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x000BBDC0 File Offset: 0x000B9FC0
		// (set) Token: 0x06001FC5 RID: 8133 RVA: 0x000BBDC8 File Offset: 0x000B9FC8
		public float currentFireDelay { get; private set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x000BBDD1 File Offset: 0x000B9FD1
		// (set) Token: 0x06001FC7 RID: 8135 RVA: 0x000BBDD9 File Offset: 0x000B9FD9
		public int currentMagSize { get; private set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x000BBDE2 File Offset: 0x000B9FE2
		// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x000BBDEA File Offset: 0x000B9FEA
		public float currentReloadDelay { get; private set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x000BBDF3 File Offset: 0x000B9FF3
		public bool isReloading
		{
			get
			{
				return this.currentReloadDelay > 0f;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x000BBE02 File Offset: 0x000BA002
		// (set) Token: 0x06001FCC RID: 8140 RVA: 0x000BBE0A File Offset: 0x000BA00A
		public AbstractTargetingModule targetingModule { get; private set; }

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x000BBE13 File Offset: 0x000BA013
		public TargetableUnit currentTarget
		{
			get
			{
				AbstractTargetingModule targetingModule = this.targetingModule;
				if (targetingModule == null)
				{
					return null;
				}
				return targetingModule.priorityTarget;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x000BBE26 File Offset: 0x000BA026
		// (set) Token: 0x06001FCF RID: 8143 RVA: 0x000BBE2E File Offset: 0x000BA02E
		public GameObject trackingTarget { get; private set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x000BBE37 File Offset: 0x000BA037
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x000BBE3F File Offset: 0x000BA03F
		public bool targetInCrosshairs { get; private set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x000BBE48 File Offset: 0x000BA048
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x000BBE50 File Offset: 0x000BA050
		public bool targetInRange { get; private set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000BBE59 File Offset: 0x000BA059
		public virtual int shotsPerAmmo
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x000BBE5C File Offset: 0x000BA05C
		public bool targetsSurface
		{
			get
			{
				return this.targetLayer == TargetLayer.Surface || this.targetLayer == TargetLayer.Both;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x000BBE71 File Offset: 0x000BA071
		public bool targetsCore
		{
			get
			{
				return this.targetLayer == TargetLayer.Core || this.targetLayer == TargetLayer.Both;
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000BBE87 File Offset: 0x000BA087
		public bool CanTargetLayer(TargetLayer targetLayer)
		{
			return targetLayer == TargetLayer.Both || targetLayer == this.targetLayer;
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000BBE98 File Offset: 0x000BA098
		public virtual TargetLayer targetLayer
		{
			get
			{
				return TargetLayer.Surface;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000BBE9B File Offset: 0x000BA09B
		public virtual GameplayType gameplayType
		{
			get
			{
				return GameplayType.Generic;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000BBEA0 File Offset: 0x000BA0A0
		public virtual float turretEquivalentRating
		{
			get
			{
				switch (this.size)
				{
				case ModuleSize.Tiny:
					return 0.45f;
				case ModuleSize.Medium:
					return 2f;
				case ModuleSize.Large:
					return 3f;
				}
				return 1f;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x000BBEEB File Offset: 0x000BA0EB
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Hardpoint;
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000BBEF0 File Offset: 0x000BA0F0
		protected override void Start()
		{
			base.Start();
			this.SetBarrelExitPoints();
			this.Reload(false, false, 1f);
			if (this.turretPieceToRotate == null)
			{
				this.turretPieceToRotate = base.transform;
			}
			this.defaultRotation = this.turretPieceToRotate.localRotation;
			this.rangeIndicator = base.GetComponentInChildren<RangeIndicator>(true);
			this.rangeIndicator.SetColorIndicator(this.turretType);
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000BBF60 File Offset: 0x000BA160
		protected override void Update()
		{
			bool flag = this.currentTarget && this.currentTarget.CanBeDamagedBy(this);
			if (this.hadTargetLastFrame && !flag)
			{
				this.rotationResetTimer = this.rotationResetInterval;
			}
			this.calcAttackTime -= Time.deltaTime;
			bool flag2 = true;
			if (this.currentReloadDelay > 0f)
			{
				this.currentReloadDelay -= Time.deltaTime;
				flag2 = false;
			}
			else if (this.currentFireDelay > 0f)
			{
				this.currentFireDelay -= Time.deltaTime;
				flag2 = false;
			}
			if (!flag)
			{
				this.rotationResetTimer -= Time.deltaTime;
				if (this.rotationResetTimer <= 0f && this.canRotate)
				{
					this.ResetRotation();
					if (!this.isReloading)
					{
						this.Reload(false, false, 1f);
					}
					if (Quaternion.Angle(this.turretPieceToRotate.localRotation, this.defaultRotation) < 0.01f)
					{
						this.rotationResetTimer = this.rotationResetInterval;
					}
				}
				this.hadTargetLastFrame = false;
				return;
			}
			this.hadTargetLastFrame = true;
			this.UpdateTrackingTarget();
			if (this.canRotate)
			{
				this.RotateTurret();
			}
			if (!base.active || !flag2)
			{
				return;
			}
			if (this.shotReset)
			{
				this.RandomizeTrackingTarget();
				return;
			}
			if (this.targetInRange && this.targetInCrosshairs)
			{
				this.Fire();
				return;
			}
			if (this.targetInRange && !this.canRotate)
			{
				this.Fire();
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000BC0D2 File Offset: 0x000BA2D2
		private void ResetRotation()
		{
			this.turretPieceToRotate.localRotation = Quaternion.RotateTowards(this.turretPieceToRotate.localRotation, this.defaultRotation, this.rotationSpeed * Time.deltaTime);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000BC104 File Offset: 0x000BA304
		private void SetBarrelExitPoints()
		{
			BarrelExitPoint[] componentsInChildren = base.GetComponentsInChildren<BarrelExitPoint>(true);
			this.firePoints = new Transform[componentsInChildren.Length];
			this.muzzleFlashEffects = new MuzzleFlashEffect[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.firePoints[i] = componentsInChildren[i].firepoint.transform;
				this.muzzleFlashEffects[i] = componentsInChildren[i].muzzleFlashEffect;
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000BC168 File Offset: 0x000BA368
		protected virtual void OnDestroy()
		{
			if (this.trackingTarget)
			{
				UnityEngine.Object.Destroy(this.trackingTarget);
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000BC182 File Offset: 0x000BA382
		public void TriggerFireProjectile(AbstractProjectile projectile)
		{
			if (base.parent)
			{
				base.parent.CheckTriggerAbility(AbilityTrigger.OnFireProjectile, projectile, null);
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000BC1A0 File Offset: 0x000BA3A0
		protected void PlayFiringSound()
		{
			if (this.fireSoundData != null)
			{
				PersistentSingleton<SoundManager>.Instance.CreateSound().WithSoundData(this.fireSoundData).WithPosition(base.transform.position).WithCustomVolume(base.IsPlayer(true) ? 1f : 0.6f).WithRandomPitch().Play();
			}
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000BC203 File Offset: 0x000BA403
		public void SetTargetingModule(AbstractTargetingModule module)
		{
			this.targetingModule = module;
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000BC20C File Offset: 0x000BA40C
		public virtual void ClearTarget()
		{
			this.targetingModule.ResetCurrentTargets();
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000BC21C File Offset: 0x000BA41C
		private void UpdateTrackingTarget()
		{
			Vector2 targetablePosition = this.currentTarget.targetablePosition;
			if (!this.trackingTarget || this.trackingTarget.transform.parent != this.currentTarget.transform)
			{
				if (this.trackingTarget)
				{
					UnityEngine.Object.Destroy(this.trackingTarget.gameObject);
				}
				this.trackingTarget = new GameObject("TrackingTarget");
				this.trackingTarget.transform.parent = this.currentTarget.transform;
				this.trackingTarget.transform.localPosition = Vector3.zero;
				this.shotReset = true;
			}
			if (this.projectileSpeed > 0f && this.currentTarget.targetableVelocity.magnitude > 0.1f)
			{
				float d = (1f + Vector2.Dot(this.currentTarget.targetableVelocity.normalized, (targetablePosition - (Vector2)this.firePoints[this.firePointIndex].transform.position).normalized)) * (Vector2.Distance(this.firePoints[this.firePointIndex].transform.position, targetablePosition) / this.projectileSpeed);
				this.trackingTarget.transform.position = targetablePosition + this.currentTarget.targetableVelocity * d;
			}
			this.UpdateForCollider();
			this.targetInRange = (Vector2.Distance(this.firePoints[this.firePointIndex].transform.position, this.trackingTarget.transform.position) <= this.range);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000BC3E1 File Offset: 0x000BA5E1
		protected virtual void UpdateForCollider()
		{
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000BC3E4 File Offset: 0x000BA5E4
		protected virtual void RandomizeTrackingTarget()
		{
			this.shotReset = false;
			if (this.projectileSpeed > 0f)
			{
				return;
			}
			PolygonCollider2D surfaceCollider = this.currentTarget.surfaceCollider;
			Bounds bounds = surfaceCollider.bounds;
			int num = 0;
			Vector2 vector;
			bool flag;
			do
			{
				vector = new Vector2(SeededRandom.Global.RandomRange(bounds.min.x, bounds.max.x), SeededRandom.Global.RandomRange(bounds.min.y, bounds.max.y));
				flag = surfaceCollider.OverlapPoint(vector);
				num++;
			}
			while (!flag && num < 50);
			this.trackingTarget.transform.position = vector;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000BC494 File Offset: 0x000BA694
		public bool CanTarget(TargetableUnit targetable)
		{
			return targetable.CanBeDamagedBy(this);
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000BC4A0 File Offset: 0x000BA6A0
		protected void Fire()
		{
			if (!this.HasAmmo())
			{
				this.Reload(false, false, 1f);
				this.currentBurstAmount = 0f;
				return;
			}
			float currentBurstAmount = this.currentBurstAmount;
			this.currentBurstAmount = currentBurstAmount + 1f;
			if (this.burstAmount > 1 && this.currentBurstAmount < (float)this.burstAmount)
			{
				this.currentFireDelay = this.burstDelay;
			}
			else
			{
				this.currentFireDelay = this.fireDelay;
				this.currentBurstAmount = 0f;
			}
			base.parent.CheckTriggerAbility(AbilityTrigger.OnFire, this, null);
			if (this.FireInternal())
			{
				this.firePointIndex++;
				this.firePointIndex %= this.firePoints.Length;
				this.shotReset = true;
				this.currentMagSize -= this.ammoPerShot;
				if (this.ammoType)
				{
					this.ammoCounter++;
					if (this.ammoCounter >= this.shotsPerAmmo)
					{
						base.parent.unitData.RemoveCargo(this.ammoType, this.ammoPerShot, false);
						this.ammoCounter = 0;
					}
				}
				if (!this.HasAmmo())
				{
					this.Reload(false, false, 1f);
				}
			}
		}

		// Token: 0x06001FEA RID: 8170
		protected abstract bool FireInternal();

		// Token: 0x06001FEB RID: 8171 RVA: 0x000BC5D8 File Offset: 0x000BA7D8
		public virtual void FireExtraShot()
		{
			this.Fire();
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000BC5E0 File Offset: 0x000BA7E0
		private void RotateTurret()
		{
			if (!this.overrideRotationTarget)
			{
				this.overrideRotationTarget = null;
			}
			Vector3 position = (this.overrideRotationTarget ?? this.trackingTarget).transform.position;
			float num = (this.overrideRotationTarget != null) ? 9999f : this.rotationSpeed;
			Vector3 vector = position - this.turretPieceToRotate.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			Quaternion quaternion = Quaternion.Euler(new Vector3(0f, 0f, z));
			float num2 = Quaternion.Angle(this.turretPieceToRotate.rotation, quaternion);
			this.rotatingTurret = (num2 > 0.01f);
			Quaternion rotation = Quaternion.RotateTowards(this.turretPieceToRotate.rotation, quaternion, num * Time.deltaTime);
			this.turretPieceToRotate.rotation = rotation;
			this.targetInCrosshairs = (num2 < this.maxFiringAngle);
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000BC6D0 File Offset: 0x000BA8D0
		public bool DoWeNeedToMove()
		{
			float magnitude = (this.trackingTarget.transform.position - base.transform.position).magnitude;
			bool flag = magnitude > this.range / 2f;
			float num = 1f;
			bool flag2 = magnitude < num;
			return flag || flag2;
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000BC726 File Offset: 0x000BA926
		public int AmmoAmountAvailable()
		{
			return base.parent.unitData.ItemAmountInCargoHold(this.ammoType) * this.shotsPerAmmo;
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000BC745 File Offset: 0x000BA945
		public bool HasAmmoType()
		{
			return this.ammoType;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000BC752 File Offset: 0x000BA952
		public virtual bool HasAmmo()
		{
			return (!this.ammoType || base.parent.unitData.ItemAmountInCargoHold(this.ammoType) != 0) && this.currentMagSize > 0;
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000BC784 File Offset: 0x000BA984
		public void Reload(bool instant = false, bool forceReload = false, float reloadModifier = 1f)
		{
			if (this.currentMagSize == this.maxMagSize && !forceReload)
			{
				return;
			}
			if (instant)
			{
				this.DoReloadInternal();
				return;
			}
			if (base.IsPlayer(true) && SeededRandom.Global.RandomBool(SkilltreeNode.combatInstantReload.currentIncrease))
			{
				this.DoReloadInternal();
				return;
			}
			base.StartCoroutine(this.ReloadCoroutine(reloadModifier));
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000BC7E1 File Offset: 0x000BA9E1
		private IEnumerator ReloadCoroutine(float reloadModifier = 1f)
		{
			float num = this.reloadDelay / reloadModifier;
			this.currentReloadDelay = num;
			yield return new WaitForSeconds(num * 0.8f);
			this.DoReloadInternal();
			yield break;
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000BC7F8 File Offset: 0x000BA9F8
		private void DoReloadInternal()
		{
			int num;
			if (this.ammoType)
			{
				num = Math.Min(this.AmmoAmountAvailable(), this.maxMagSize);
			}
			else
			{
				num = this.maxMagSize;
			}
			if (num == 0 && base.IsPlayer(true))
			{
				UIInfoTextParent instance = UIInfoTextParent.instance;
				if (instance != null)
				{
					instance.ShowWarningText("@OutOfAmmo", null, null);
				}
			}
			this.currentMagSize = num;
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x000BC85F File Offset: 0x000BAA5F
		protected virtual IEnumerator PlayWeaponEffect()
		{
			if (this.muzzleFlashEffects.Length != 0)
			{
				this.muzzleFlashEffects[this.muzzleFlashEffectIndex].Play();
				this.muzzleFlashEffectIndex++;
				this.muzzleFlashEffectIndex %= this.muzzleFlashEffects.Length;
			}
			yield return null;
			yield break;
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000BC870 File Offset: 0x000BAA70
		protected virtual float CalculateAttackPower()
		{
			float num = base.parent.GetStat(this.powerStat);
			if (base.parent.droneBayModule)
			{
				using (List<Drone>.Enumerator enumerator = base.parent.droneBayModule.drones.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Drone drone = enumerator.Current;
						if (drone && drone.GetComponentInChildren<AbstractTurret>().powerStat == this.powerStat)
						{
							num += drone.GetStat(this.powerStat);
						}
					}
					goto IL_12A;
				}
			}
			Drone drone2 = base.parent as Drone;
			if (drone2 != null && drone2.droneCommander)
			{
				num = drone2.droneCommander.GetStat(this.powerStat);
				num += drone2.droneCommander.GetStat(EquipStat.DronePower);
				foreach (Drone drone3 in drone2.droneCommander.droneBayModule.drones)
				{
					if (drone3)
					{
						num += drone3.GetStat(this.powerStat);
					}
				}
			}
			IL_12A:
			float num2 = this.GetPowerMultiplier();
			float num3 = Mathf.Max(0.45f, base.parent.GetEquivalentTurretsCount(this.powerStat));
			float num4 = num / num3;
			if (num3 > 5.9f)
			{
				num4 *= 0.65f;
			}
			else if (num3 > 4.9f)
			{
				num4 *= 0.68f;
			}
			else if (num3 > 3.9f)
			{
				num4 *= 0.72f;
			}
			else if (num3 > 2.9f)
			{
				num4 *= 0.78f;
			}
			else if (num3 > 1.9f)
			{
				num4 *= 0.85f;
			}
			return num4 * this.turretEquivalentRating * num2;
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000BCA4C File Offset: 0x000BAC4C
		public float GetAttackPower()
		{
			if (this.calcAttackTime <= 0f)
			{
				this.calcedAttackPower = this.CalculateAttackPower();
				this.calcAttackTime = 0.2f;
			}
			return this.calcedAttackPower;
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000BCA78 File Offset: 0x000BAC78
		protected virtual float GetPowerMultiplier()
		{
			return this.powerMultiplier;
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000BCA80 File Offset: 0x000BAC80
		public void ShowRange()
		{
			if (this.rangeIndicator != null && this.rangeIndicator.isActiveAndEnabled)
			{
				Transform transform = base.transform;
				if (this.firePoints.Length != 0)
				{
					transform = this.firePoints[this.firePointIndex];
				}
				this.rangeIndicator.Show(this.range / transform.localScale.y, base.active);
			}
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000BCAE9 File Offset: 0x000BACE9
		public void ChangeRangeIndicatorColor()
		{
			this.rangeIndicator.CheckForColor(base.active);
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000BCAFC File Offset: 0x000BACFC
		public void HideRange()
		{
			if (this.rangeIndicator != null && this.rangeIndicator.isActiveAndEnabled)
			{
				this.rangeIndicator.Hide();
			}
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000BCB24 File Offset: 0x000BAD24
		protected virtual DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			return new DamageData(this)
			{
				power = this.GetAttackPower(),
				criticalChance = this.GetStat(EquipStat.CriticalChance),
				type = this.damageType,
				hitTransform = (targetTransform ?? this.trackingTarget.transform),
				hitCoordinates = (hitCoordinates ?? this.trackingTarget.transform.position),
				targetLayer = targetLayer
			};
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000BCBA9 File Offset: 0x000BADA9
		public override MainStat GetMainStat()
		{
			return new MainStat(this.powerStat.GetDisplayName(), this.displayedPower);
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000BCBC4 File Offset: 0x000BADC4
		protected override void SetMainSubStats()
		{
			float defaultAttacksPerSecond = this.defaultAttacksPerSecond;
			this.mainSubStats.AddMainSubStat(GameMath.FormatNumber(defaultAttacksPerSecond, (defaultAttacksPerSecond < 100f) ? 1 : 0), "attacks per second");
			this.mainSubStats.AddMainSubStat(this.damageType.ToString(), "damage");
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000BCC1C File Offset: 0x000BAE1C
		public override void DataFromJson(JsonObject data)
		{
			if (data["dynamicFields"].IsJsonObject)
			{
				JsonObject asJsonObject = data["dynamicFields"].AsJsonObject;
				foreach (KeyValuePair<string, JsonValue> keyValuePair in asJsonObject)
				{
					if (keyValuePair.Key == "power")
					{
						base.stats.Add(new EquipStatLine(this.powerStat, (float)keyValuePair.Value.AsNumber, 1f, true));
					}
				}
				asJsonObject.Remove("power");
			}
			base.DataFromJson(data);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000BCCDC File Offset: 0x000BAEDC
		public float GetBaseRange()
		{
			return this._range;
		}

		// Token: 0x040012F2 RID: 4850
		[Header("Turret Info")]
		public DamageType damageType;

		// Token: 0x040012F3 RID: 4851
		[SerializeField]
		public float powerMultiplier = 1f;

		// Token: 0x040012F5 RID: 4853
		[SerializeField]
		protected float _range;

		// Token: 0x040012F6 RID: 4854
		[SerializeField]
		private float _reloadDelay;

		// Token: 0x040012F7 RID: 4855
		[SerializeField]
		protected float _fireDelay;

		// Token: 0x040012F8 RID: 4856
		[SerializeField]
		protected float _projectileSpeed;

		// Token: 0x040012F9 RID: 4857
		public float maxFiringAngle;

		// Token: 0x040012FA RID: 4858
		[SerializeField]
		protected int _maxMagSize;

		// Token: 0x040012FB RID: 4859
		public int ammoPerShot = 1;

		// Token: 0x040012FC RID: 4860
		public int burstAmount = 1;

		// Token: 0x040012FD RID: 4861
		public float burstDelay = 0.2f;

		// Token: 0x040012FF RID: 4863
		[SerializeField]
		private float _rotationSpeed;

		// Token: 0x04001300 RID: 4864
		protected GameObject overrideRotationTarget;

		// Token: 0x04001301 RID: 4865
		public float accuracyAngle;

		// Token: 0x04001302 RID: 4866
		public TurretType turretType;

		// Token: 0x04001303 RID: 4867
		public Transform turretPieceToRotate;

		// Token: 0x04001304 RID: 4868
		[SerializeField]
		public InventoryItemType ammoType;

		// Token: 0x04001308 RID: 4872
		protected bool shotReset;

		// Token: 0x04001309 RID: 4873
		private MuzzleFlashEffect[] muzzleFlashEffects;

		// Token: 0x0400130A RID: 4874
		protected Transform[] firePoints;

		// Token: 0x0400130B RID: 4875
		protected int firePointIndex;

		// Token: 0x0400130C RID: 4876
		private int muzzleFlashEffectIndex;

		// Token: 0x04001311 RID: 4881
		private bool rotatingTurret;

		// Token: 0x04001312 RID: 4882
		private int ammoCounter;

		// Token: 0x04001313 RID: 4883
		[SerializeField]
		protected SoundData fireSoundData;

		// Token: 0x04001314 RID: 4884
		private float calcedAttackPower;

		// Token: 0x04001315 RID: 4885
		private float calcAttackTime;

		// Token: 0x04001316 RID: 4886
		public RangeIndicator rangeIndicator;

		// Token: 0x04001317 RID: 4887
		private float rotationResetTimer;

		// Token: 0x04001318 RID: 4888
		private float rotationResetInterval = 10f;

		// Token: 0x04001319 RID: 4889
		private Quaternion defaultRotation;

		// Token: 0x0400131A RID: 4890
		private bool hadTargetLastFrame;
	}
}
