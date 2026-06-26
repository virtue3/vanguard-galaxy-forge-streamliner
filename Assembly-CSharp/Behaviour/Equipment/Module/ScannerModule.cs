using System;
using System.Collections.Generic;
using Behaviour.UI.HUD.Hover;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Item;
using Source.Player;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200036C RID: 876
	public class ScannerModule : AbstractModule
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x000C3F70 File Offset: 0x000C2170
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Scanner;
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000C3F73 File Offset: 0x000C2173
		protected override void Start()
		{
			base.Start();
			this.targetModules = base.parent.GetComponentsInChildren<AbstractTargetingModule>();
			this.scanTimer = this.checkInterval;
			this.SetScannerCenter();
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000C3FA0 File Offset: 0x000C21A0
		protected override void Update()
		{
			if (base.parent.inShipYard || !base.active || (base.parent.IsPlayer(false) && GamePlayer.current.waypoints.Count > 0))
			{
				return;
			}
			base.Update();
			if (!this.checkForTargets)
			{
				return;
			}
			this.scanTimer -= Time.deltaTime;
			if (this.scanTimer < 0f)
			{
				this.scanTimer = this.checkInterval;
				this.SelectClosestTargetInRange(this.overlapRadius);
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000C402C File Offset: 0x000C222C
		public void SelectClosestTargetInRange(float overlapRadius)
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(this.scannerCenter.position, overlapRadius);
			List<TargetableUnit> list = new List<TargetableUnit>();
			foreach (Collider2D collider2D in array)
			{
				TargetableUnit component;
				if (collider2D.gameObject.activeInHierarchy && (component = collider2D.GetComponent<TargetableUnit>()))
				{
					list.Add(component);
					TargetScannedEffect targetScannedEffect;
					if (collider2D.gameObject.TryGetComponent<TargetScannedEffect>(out targetScannedEffect))
					{
						float delay = Vector2.Distance(this.scannerCenter.position, collider2D.gameObject.transform.position) / overlapRadius * 2f;
						targetScannedEffect.StartScanning(this.scanDuration, delay);
						targetScannedEffect.SetFinalScanningColor(Color.red);
					}
				}
			}
			list.Sort((TargetableUnit a, TargetableUnit b) => Vector2.Distance(this.scannerCenter.position, a.transform.position).CompareTo(Vector2.Distance(this.scannerCenter.position, b.transform.position)));
			foreach (AbstractTargetingModule abstractTargetingModule in this.targetModules)
			{
				abstractTargetingModule.UpdateAvailableTargets(list);
				abstractTargetingModule.UpdateAutoTarget();
			}
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000C412A File Offset: 0x000C232A
		private void OnDisable()
		{
			this.scanTimer = this.checkInterval;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000C4138 File Offset: 0x000C2338
		public void SetSensorRange(float range)
		{
			this.overlapRadius = range;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000C4141 File Offset: 0x000C2341
		public void SetScanInterval(float interval)
		{
			this.checkInterval = interval;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000C414A File Offset: 0x000C234A
		public void SetCheckForTargets(bool checkForTargets)
		{
			this.checkForTargets = checkForTargets;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000C4154 File Offset: 0x000C2354
		private void SetScannerCenter()
		{
			Drone drone = base.parent as Drone;
			if (drone != null)
			{
				this.scannerCenter = drone.droneCommander.transform;
				return;
			}
			this.scannerCenter = base.transform;
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000C4190 File Offset: 0x000C2390
		public override MainStat GetMainStat()
		{
			EquipStatLine? equipStatLine;
			float amount = (base.GetStatLine(EquipStat.Precision) != null) ? equipStatLine.GetValueOrDefault().amount : 0f;
			return new MainStat("Precision", amount);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000C41CE File Offset: 0x000C23CE
		protected override void SetMainSubStats()
		{
		}

		// Token: 0x040013EA RID: 5098
		[Header("Scanner")]
		[SerializeField]
		private float checkInterval;

		// Token: 0x040013EB RID: 5099
		[SerializeField]
		private float scanDuration;

		// Token: 0x040013EC RID: 5100
		[SerializeField]
		private float overlapRadius;

		// Token: 0x040013ED RID: 5101
		private bool checkForTargets = true;

		// Token: 0x040013EE RID: 5102
		private Transform scannerCenter;

		// Token: 0x040013EF RID: 5103
		private AbstractTargetingModule[] targetModules;

		// Token: 0x040013F0 RID: 5104
		private float scanTimer;
	}
}
