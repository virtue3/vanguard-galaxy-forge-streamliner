using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Equipment.Turret;
using UnityEngine;

namespace Behaviour.UI.HUD.Turrets
{
	// Token: 0x0200028B RID: 651
	public class TurretControl : MonoBehaviour
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060017DB RID: 6107 RVA: 0x00095BF7 File Offset: 0x00093DF7
		// (set) Token: 0x060017DC RID: 6108 RVA: 0x00095BFF File Offset: 0x00093DFF
		public TurretStatus turretStatus { get; private set; }

		// Token: 0x060017DD RID: 6109 RVA: 0x00095C08 File Offset: 0x00093E08
		private void Start()
		{
			this.shownPos = this.hud.anchoredPosition;
			this.hiddenPos = this.shownPos - new Vector2(0f, this.moveDistance);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00095C3C File Offset: 0x00093E3C
		public void ResetHud()
		{
			this.ClearTurrets();
			this.CreateTurretButtons();
			this.activateTurrets.Init(this);
			this.deactivateTurrets.Init(this);
			this.clearTargets.Init(this);
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00095C6E File Offset: 0x00093E6E
		private void ClearTurrets()
		{
			this.turretContainer.transform.DestroyChildren();
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00095C80 File Offset: 0x00093E80
		protected void CreateTurretButtons()
		{
			this.turrets.Clear();
			foreach (AbstractTurret abstractTurret in GameplayManager.Instance.spaceShip.GetTurrets())
			{
				UnityEngine.Object.Instantiate<TurretButton>(this.turretButton, this.turretContainer.transform).SetTurret(abstractTurret, this);
				this.turrets.Add(abstractTurret);
			}
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00095D04 File Offset: 0x00093F04
		public void PressSlideButton()
		{
			base.StopAllCoroutines();
			if (this.up)
			{
				base.StartCoroutine(this.MoveControlsDown());
				return;
			}
			base.StartCoroutine(this.MoveControlsUp());
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00095D2F File Offset: 0x00093F2F
		public IEnumerator MoveControlsDown()
		{
			this.up = false;
			yield return base.StartCoroutine(this.MoveTo(this.hiddenPos));
			this.ClearTurrets();
			yield break;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00095D3E File Offset: 0x00093F3E
		public IEnumerator MoveControlsUp()
		{
			this.up = true;
			this.ResetHud();
			yield return base.StartCoroutine(this.MoveTo(this.shownPos));
			yield break;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00095D4D File Offset: 0x00093F4D
		private IEnumerator MoveTo(Vector2 target)
		{
			while (Vector2.Distance(this.hud.anchoredPosition, target) > 0.1f)
			{
				this.hud.anchoredPosition = Vector2.Lerp(this.hud.anchoredPosition, target, Time.deltaTime * this.moveSpeed);
				yield return null;
			}
			this.hud.anchoredPosition = target;
			yield break;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00095D64 File Offset: 0x00093F64
		public void CheckTurretStatus()
		{
			bool flag = false;
			bool flag2 = false;
			using (List<AbstractTurret>.Enumerator enumerator = this.turrets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.active)
					{
						flag = true;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			if (flag && flag2)
			{
				this.turretStatus = TurretStatus.Mixed;
			}
			else if (flag)
			{
				this.turretStatus = TurretStatus.On;
			}
			else
			{
				this.turretStatus = TurretStatus.Off;
			}
			this.activateTurrets.SetColor(this.turretStatus);
			this.deactivateTurrets.SetColor(this.turretStatus);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00095E04 File Offset: 0x00094004
		public void ActivateTurrets()
		{
			foreach (AbstractTurret abstractTurret in this.turrets)
			{
				abstractTurret.Activate();
			}
			this.CheckTurretStatus();
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00095E5C File Offset: 0x0009405C
		public void DeactivateTurrets()
		{
			foreach (AbstractTurret abstractTurret in this.turrets)
			{
				abstractTurret.Deactivate();
			}
			this.CheckTurretStatus();
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00095EB4 File Offset: 0x000940B4
		public void ClearTargets()
		{
			foreach (AbstractTurret abstractTurret in this.turrets)
			{
				abstractTurret.ClearTarget();
			}
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00095F04 File Offset: 0x00094104
		public void ShowRange()
		{
			foreach (AbstractTurret abstractTurret in this.turrets)
			{
				abstractTurret.ShowRange();
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00095F54 File Offset: 0x00094154
		public void HideRange()
		{
			foreach (AbstractTurret abstractTurret in this.turrets)
			{
				abstractTurret.HideRange();
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00095FA4 File Offset: 0x000941A4
		public bool NoTurrets()
		{
			return this.turrets.Count == 0;
		}

		// Token: 0x04000ED3 RID: 3795
		[SerializeField]
		private TurretButton turretButton;

		// Token: 0x04000ED4 RID: 3796
		[SerializeField]
		private RectTransform turretContainer;

		// Token: 0x04000ED5 RID: 3797
		[SerializeField]
		private TurretsActivateButton activateTurrets;

		// Token: 0x04000ED6 RID: 3798
		[SerializeField]
		private TurretsDeactivateButton deactivateTurrets;

		// Token: 0x04000ED7 RID: 3799
		[SerializeField]
		private ClearTargetButton clearTargets;

		// Token: 0x04000ED8 RID: 3800
		public RectTransform hud;

		// Token: 0x04000ED9 RID: 3801
		[SerializeField]
		private float moveDistance = 40f;

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		private float moveSpeed = 4f;

		// Token: 0x04000EDB RID: 3803
		private Vector2 hiddenPos;

		// Token: 0x04000EDC RID: 3804
		private Vector2 shownPos;

		// Token: 0x04000EDE RID: 3806
		public bool up = true;

		// Token: 0x04000EDF RID: 3807
		private List<AbstractTurret> turrets = new List<AbstractTurret>();
	}
}
