using System;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002BC RID: 700
	public class InfoPanel : MonoBehaviour
	{
		// Token: 0x060019CA RID: 6602 RVA: 0x000A03C8 File Offset: 0x0009E5C8
		public void SetHeader(string header, Color? color = null)
		{
			this.header.text = header;
			if (color != null)
			{
				Color value = color.Value;
				value.a = 0.1f;
				this.headerBackground.color = value;
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000A040C File Offset: 0x0009E60C
		public void AddInfoRow(float baseStat, EquipStat equipStat, float stat, string addendum = "", bool noHoverInfo = false)
		{
			this.currentHeight -= 30f;
			InfoRow infoRow = UnityEngine.Object.Instantiate<InfoRow>(this.infoRowPrefab, base.transform);
			infoRow.SetInfo(baseStat, equipStat, equipStat.IsPercentageStat() ? GameMath.FormatPercentage(stat, FormatPercentageMode.Default, 1) : GameMath.FormatNumber(stat, -1), addendum, noHoverInfo);
			infoRow.SetPosition(new Vector2(0f, this.currentHeight));
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000A0478 File Offset: 0x0009E678
		public void AddInfoRowCustomString(string customString, int customAmount)
		{
			this.currentHeight -= 30f;
			InfoRow infoRow = UnityEngine.Object.Instantiate<InfoRow>(this.infoRowPrefab, base.transform);
			infoRow.SetCustomString(customString, customAmount);
			infoRow.SetPosition(new Vector2(0f, this.currentHeight));
		}

		// Token: 0x04001035 RID: 4149
		[SerializeField]
		private InfoRow infoRowPrefab;

		// Token: 0x04001036 RID: 4150
		[SerializeField]
		private TMP_Text header;

		// Token: 0x04001037 RID: 4151
		[SerializeField]
		private Image headerBackground;

		// Token: 0x04001038 RID: 4152
		private float currentHeight;

		// Token: 0x04001039 RID: 4153
		public const float infoRowHeight = 30f;
	}
}
