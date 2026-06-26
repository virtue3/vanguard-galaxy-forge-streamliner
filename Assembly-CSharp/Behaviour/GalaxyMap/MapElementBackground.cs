using System;
using TMPro;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000330 RID: 816
	public class MapElementBackground : MonoBehaviour
	{
		// Token: 0x06001EEF RID: 7919 RVA: 0x000B8D24 File Offset: 0x000B6F24
		public void SetSize(Vector2 size)
		{
			size *= 2f;
			this.background.transform.localScale = size / 2f;
			Vector2 v = this.label.transform.localPosition;
			v.y = size.y - 1f;
			this.label.transform.localPosition = v;
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000B8D9D File Offset: 0x000B6F9D
		public void SetColor(Color color)
		{
			this.background.color = Color.Lerp(this.background.color, color, 0.2f);
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000B8DC0 File Offset: 0x000B6FC0
		public void SetLabel(string content)
		{
			this.label.text = content;
		}

		// Token: 0x0400128A RID: 4746
		[SerializeField]
		private SpriteRenderer background;

		// Token: 0x0400128B RID: 4747
		[SerializeField]
		private TextMeshPro label;
	}
}
