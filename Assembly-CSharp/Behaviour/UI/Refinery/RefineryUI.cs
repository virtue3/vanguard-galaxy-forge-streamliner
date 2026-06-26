using System;
using Source.Item;
using Source.Mining;
using UnityEngine;

namespace Behaviour.UI.Refinery
{
	// Token: 0x0200024F RID: 591
	public class RefineryUI : MonoBehaviour
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x0008B6EB File Offset: 0x000898EB
		// (set) Token: 0x060015D6 RID: 5590 RVA: 0x0008B6F2 File Offset: 0x000898F2
		public static RefineryUI current { get; private set; }

		// Token: 0x060015D7 RID: 5591 RVA: 0x0008B6FC File Offset: 0x000898FC
		private void Awake()
		{
			RefineryUI.current = this;
			this.materialsParent.DestroyChildren();
			Refinery current = Refinery.current;
			int length = Enum.GetValues(typeof(RefinedMaterial)).Length;
			for (int i = 0; i < length; i++)
			{
				RefineryMaterialBadge refineryMaterialBadge = UnityEngine.Object.Instantiate<RefineryMaterialBadge>(this.materialPrefab, this.materialsParent);
				refineryMaterialBadge.SetMaterial((RefinedMaterial)i);
				((RectTransform)refineryMaterialBadge.transform).anchoredPosition = new Vector2(4f, (float)(-4 - i * 34));
			}
			this.ShowRefinery();
			this.settings.RefreshSettings();
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0008B78B File Offset: 0x0008998B
		public void ShowRefinery()
		{
			this.contents.Show();
			this.settings.Hide();
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0008B7A3 File Offset: 0x000899A3
		public void UpdateContent()
		{
			this.contents.Show();
		}

		// Token: 0x04000D14 RID: 3348
		[SerializeField]
		private RectTransform materialsParent;

		// Token: 0x04000D15 RID: 3349
		[SerializeField]
		private RefineryMaterialBadge materialPrefab;

		// Token: 0x04000D16 RID: 3350
		[SerializeField]
		private RefineryJobTabContents contents;

		// Token: 0x04000D17 RID: 3351
		[SerializeField]
		private RefinerySettingsTab settings;
	}
}
