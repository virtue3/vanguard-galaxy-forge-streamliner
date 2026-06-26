using System;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x0200027F RID: 639
	public class EnemyIndicatorManager : MonoBehaviour
	{
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x0009361A File Offset: 0x0009181A
		// (set) Token: 0x06001751 RID: 5969 RVA: 0x00093621 File Offset: 0x00091821
		public static EnemyIndicatorManager Instance { get; private set; }

		// Token: 0x06001752 RID: 5970 RVA: 0x00093629 File Offset: 0x00091829
		private void Awake()
		{
			EnemyIndicatorManager.Instance = this;
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00093631 File Offset: 0x00091831
		private void OnDestroy()
		{
			EnemyIndicatorManager.Instance = null;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00093639 File Offset: 0x00091839
		public EnemyIndicator ShowIndicator(SpaceShip unit)
		{
			EnemyIndicator enemyIndicator = UnityEngine.Object.Instantiate<EnemyIndicator>(this.indicatorPrefab, base.transform);
			enemyIndicator.SetUnit(unit);
			return enemyIndicator;
		}

		// Token: 0x04000E69 RID: 3689
		[SerializeField]
		private EnemyIndicator indicatorPrefab;
	}
}
