using System;
using Behaviour.Crew;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001D5 RID: 469
	public class SkillTreeRowItem : MonoBehaviour
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00075520 File Offset: 0x00073720
		// (set) Token: 0x060011A7 RID: 4519 RVA: 0x00075528 File Offset: 0x00073728
		public float width { get; private set; }

		// Token: 0x060011A8 RID: 4520 RVA: 0x00075531 File Offset: 0x00073731
		public void SetSkillTree(Skilltree skillTree, Action callback)
		{
			this.skillTree = skillTree;
			this.callback = callback;
			this.DrawSkillTree();
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00075547 File Offset: 0x00073747
		private void SkillTreeUpdate()
		{
			this.callback();
			this.DrawSkillTree();
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0007555C File Offset: 0x0007375C
		private void DrawSkillTree()
		{
			this.panel.DestroyChildren();
			foreach (SkilltreeNode skilltreeNode in this.skillTree.allNodes)
			{
				CrewSkillNodeItem crewSkillNodeItem = UnityEngine.Object.Instantiate<CrewSkillNodeItem>(this.skillNodeItem, this.panel.transform);
				crewSkillNodeItem.SetSkillNode(skilltreeNode, new Action(this.SkillTreeUpdate));
				((RectTransform)crewSkillNodeItem.transform).anchoredPosition = new Vector2((float)(66 * (skilltreeNode.tier - 1)), (float)(-66 * skilltreeNode.row));
			}
		}

		// Token: 0x040009B8 RID: 2488
		[SerializeField]
		private CrewSkillNodeItem skillNodeItem;

		// Token: 0x040009B9 RID: 2489
		[SerializeField]
		private RectTransform panel;

		// Token: 0x040009BA RID: 2490
		private Skilltree skillTree;

		// Token: 0x040009BB RID: 2491
		private Action callback;
	}
}
