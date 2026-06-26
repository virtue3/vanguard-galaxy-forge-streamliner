using System;
using System.Collections.Generic;
using Source.Galaxy;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002AA RID: 682
	public class FactionConquestRanks : MonoBehaviour
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x0009D9E2 File Offset: 0x0009BBE2
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x0009D9EA File Offset: 0x0009BBEA
		public Faction faction { get; private set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x0009D9F3 File Offset: 0x0009BBF3
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x0009D9FB File Offset: 0x0009BBFB
		public int playerContribution { get; private set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x0009DA04 File Offset: 0x0009BC04
		// (set) Token: 0x06001959 RID: 6489 RVA: 0x0009DA0C File Offset: 0x0009BC0C
		public ConquestRank conquestRank { get; private set; }

		// Token: 0x0600195A RID: 6490 RVA: 0x0009DA18 File Offset: 0x0009BC18
		public void Setup(Faction faction, Conquest conquest)
		{
			foreach (object obj in this.thresholdMarkerContainer)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (object obj2 in this.thresholdLineContainer)
			{
				UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
			}
			this.faction = faction;
			FactionIconSet factionIconSet = FactionIconSet.Get(faction);
			if (factionIconSet != null)
			{
				this.icon.sprite = factionIconSet.fullSize;
			}
			else
			{
				this.icon.gameObject.SetActive(false);
			}
			int value;
			if (faction == Faction.puppeteers)
			{
				value = conquest.umbralContribution;
			}
			else
			{
				value = conquest.GetFactionStanding(faction).playerContribution;
			}
			this.playerContribution = Mathf.Clamp(value, 0, 4500);
			this.conquestRank = ConquestRankExtension.GetConquestRankLevel(this.playerContribution);
			this.conquestRankText.text = this.conquestRank.GetConquestRankTranslation(faction.identifier);
			Color conquestColor = this.conquestRank.GetConquestColor();
			this.conquestRankText.color = conquestColor;
			this.factionName.text = Translation.Translate(faction.name, Array.Empty<object>());
			int num = ConquestRankExtension.ConquestRankThresholds[ConquestRank.None];
			int num2 = ConquestRankExtension.ConquestRankThresholds[ConquestRank.Rank6];
			this.fillImage.color = conquestColor;
			float fillAmount = Mathf.InverseLerp((float)num, (float)num2, (float)this.playerContribution);
			this.fillImage.fillAmount = fillAmount;
			this.conquestRankNumber.text = string.Format("{0} / {1}", this.playerContribution, num2);
			foreach (KeyValuePair<ConquestRank, int> keyValuePair in ConquestRankExtension.ConquestRankThresholds)
			{
				if (keyValuePair.Key != ConquestRank.None)
				{
					int value2 = keyValuePair.Value;
					float x = Mathf.InverseLerp((float)num, (float)num2, (float)value2);
					Image image = UnityEngine.Object.Instantiate<Image>(this.linePrefab, this.thresholdLineContainer);
					RectTransform rectTransform = image.rectTransform;
					rectTransform.anchorMin = new Vector2(x, 0f);
					rectTransform.anchorMax = new Vector2(x, 1f);
					rectTransform.anchoredPosition = Vector2.zero;
					image.gameObject.SetActive(false);
					FactionConquestRankThresholdMarker factionConquestRankThresholdMarker = UnityEngine.Object.Instantiate<FactionConquestRankThresholdMarker>(this.markerPrefab, this.thresholdMarkerContainer);
					RectTransform component = factionConquestRankThresholdMarker.GetComponent<RectTransform>();
					component.anchorMin = new Vector2(x, 0.5f);
					component.anchorMax = new Vector2(x, 0.5f);
					component.anchoredPosition = Vector2.zero;
					factionConquestRankThresholdMarker.Initialize(keyValuePair.Key, value2, faction.identifier);
					factionConquestRankThresholdMarker.SetLine(image);
				}
			}
		}

		// Token: 0x04000FC1 RID: 4033
		[SerializeField]
		private Image icon;

		// Token: 0x04000FC2 RID: 4034
		[SerializeField]
		private TextMeshProUGUI factionName;

		// Token: 0x04000FC3 RID: 4035
		[SerializeField]
		private TextMeshProUGUI conquestRankNumber;

		// Token: 0x04000FC4 RID: 4036
		[SerializeField]
		private TextMeshProUGUI conquestRankText;

		// Token: 0x04000FC5 RID: 4037
		[SerializeField]
		private Image fillImage;

		// Token: 0x04000FC6 RID: 4038
		[SerializeField]
		private RectTransform thresholdLineContainer;

		// Token: 0x04000FC7 RID: 4039
		[SerializeField]
		private RectTransform thresholdMarkerContainer;

		// Token: 0x04000FC8 RID: 4040
		[SerializeField]
		private FactionConquestRankThresholdMarker markerPrefab;

		// Token: 0x04000FC9 RID: 4041
		[SerializeField]
		private Image linePrefab;
	}
}
