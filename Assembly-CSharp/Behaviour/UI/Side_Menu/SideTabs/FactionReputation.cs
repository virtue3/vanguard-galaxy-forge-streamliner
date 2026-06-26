using System;
using System.Collections.Generic;
using Behaviour.UI.Spacestation;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A8 RID: 680
	public class FactionReputation : MonoBehaviour
	{
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001942 RID: 6466 RVA: 0x0009D1A0 File Offset: 0x0009B3A0
		// (set) Token: 0x06001943 RID: 6467 RVA: 0x0009D1A8 File Offset: 0x0009B3A8
		public Faction faction { get; private set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x0009D1B1 File Offset: 0x0009B3B1
		// (set) Token: 0x06001945 RID: 6469 RVA: 0x0009D1B9 File Offset: 0x0009B3B9
		public int reputationLevel { get; private set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x0009D1C2 File Offset: 0x0009B3C2
		// (set) Token: 0x06001947 RID: 6471 RVA: 0x0009D1CA File Offset: 0x0009B3CA
		public ReputationLevel reputation { get; private set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x0009D1D3 File Offset: 0x0009B3D3
		// (set) Token: 0x06001949 RID: 6473 RVA: 0x0009D1DB File Offset: 0x0009B3DB
		public Color reputationColor { get; private set; }

		// Token: 0x0600194A RID: 6474 RVA: 0x0009D1E4 File Offset: 0x0009B3E4
		public void Setup(Faction faction)
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
			this.factionName.text = Translation.Translate(faction.name, Array.Empty<object>());
			this.reputationLevel = faction.GetReputation(Faction.player);
			this.reputation = ReputationLevelExtensions.GetReputationLevel(this.reputationLevel);
			int num = ReputationLevelExtensions.ReputationThresholds[ReputationLevel.Wary];
			int num2 = ReputationLevelExtensions.ReputationThresholds[ReputationLevel.Exalted];
			int num3 = ReputationLevelExtensions.ReputationThresholds[ReputationLevel.AbsoluteThreat];
			this.fillImage.color = this.reputation.GetReputationColor();
			bool flag = this.reputationLevel >= num;
			if (flag)
			{
				this.factionName.color = ColorHelper.repGreen;
				float fillAmount = Mathf.InverseLerp((float)num, (float)num2, (float)this.reputationLevel);
				this.fillImage.fillAmount = fillAmount;
				this.factionReputationNumber.text = string.Format("{0} / {1}", this.reputationLevel, num2);
			}
			else
			{
				this.factionName.color = ColorHelper.repRed;
				float fillAmount2 = Mathf.InverseLerp((float)num, (float)num3, (float)this.reputationLevel);
				this.fillImage.fillAmount = fillAmount2;
				this.factionReputationNumber.text = string.Format("{0} / {1}", this.reputationLevel, num3);
			}
			int num4 = num;
			int num5 = flag ? num2 : num3;
			foreach (KeyValuePair<ReputationLevel, int> keyValuePair in ReputationLevelExtensions.ReputationThresholds)
			{
				int value = keyValuePair.Value;
				if ((!flag || value >= num) && (flag || value <= num))
				{
					float x = Mathf.InverseLerp((float)num4, (float)num5, (float)value);
					Image image = UnityEngine.Object.Instantiate<Image>(this.linePrefab, this.thresholdLineContainer);
					RectTransform rectTransform = image.rectTransform;
					rectTransform.anchorMin = new Vector2(x, 0f);
					rectTransform.anchorMax = new Vector2(x, 1f);
					rectTransform.anchoredPosition = Vector2.zero;
					image.gameObject.SetActive(false);
					ReputationThresholdMarker reputationThresholdMarker = UnityEngine.Object.Instantiate<ReputationThresholdMarker>(this.markerPrefab, this.thresholdMarkerContainer);
					RectTransform component = reputationThresholdMarker.GetComponent<RectTransform>();
					component.anchorMin = new Vector2(x, 0.5f);
					component.anchorMax = new Vector2(x, 0.5f);
					component.anchoredPosition = Vector2.zero;
					reputationThresholdMarker.Initialize(keyValuePair.Key, value);
					reputationThresholdMarker.SetLine(image);
				}
			}
			this.UpdateAtWarStatus();
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0009D540 File Offset: 0x0009B740
		public void ToggleWarStatus()
		{
			if (SpaceStationInterior.instance && MapPointOfInterest.current.faction == this.faction && !(MapPointOfInterest.current is EmbassyStation))
			{
				this.UpdateAtWarStatus();
				return;
			}
			if ((float)Faction.player.GetReputation(this.faction) < -500f)
			{
				this.UpdateAtWarStatus();
				return;
			}
			if (GamePlayer.current.atWar.Contains(this.faction) && SpaceStationInterior.instance)
			{
				GamePlayer.current.atWar.Remove(this.faction);
			}
			else if (this.warToggle.isOn)
			{
				GamePlayer.current.atWar.Add(this.faction);
			}
			this.UpdateAtWarStatus();
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0009D604 File Offset: 0x0009B804
		private void UpdateAtWarStatus()
		{
			if (GamePlayer.current.GetStoryteller<Conquest>() != null)
			{
				this.warToggle.SetIsOnWithoutNotify(GamePlayer.current.atWar.Contains(this.faction) || (float)Faction.player.GetReputation(this.faction) < -500f);
				this.warToggle.gameObject.SetActive(true);
			}
			else
			{
				this.warToggle.gameObject.SetActive(false);
			}
			if (GamePlayer.current.atWar.Contains(this.faction))
			{
				this.factionReputationLevel.TL("@AtWar", Array.Empty<object>());
				this.factionReputationLevel.color = ReputationLevel.Hated.GetReputationColor();
			}
			else
			{
				this.factionReputationLevel.text = Translation.Translate("@" + this.reputation.ToString(), Array.Empty<object>());
				this.reputationColor = this.reputation.GetReputationColor();
				this.factionReputationLevel.color = this.reputationColor;
			}
			UITooltip.Refresh();
		}

		// Token: 0x04000FB0 RID: 4016
		[SerializeField]
		private Image icon;

		// Token: 0x04000FB1 RID: 4017
		[SerializeField]
		private TextMeshProUGUI factionName;

		// Token: 0x04000FB2 RID: 4018
		[SerializeField]
		private TextMeshProUGUI factionReputationNumber;

		// Token: 0x04000FB3 RID: 4019
		[SerializeField]
		private TextMeshProUGUI factionReputationLevel;

		// Token: 0x04000FB4 RID: 4020
		[SerializeField]
		private Image fillImage;

		// Token: 0x04000FB5 RID: 4021
		[SerializeField]
		private Toggle warToggle;

		// Token: 0x04000FB6 RID: 4022
		[SerializeField]
		private RectTransform thresholdLineContainer;

		// Token: 0x04000FB7 RID: 4023
		[SerializeField]
		private RectTransform thresholdMarkerContainer;

		// Token: 0x04000FB8 RID: 4024
		[SerializeField]
		private ReputationThresholdMarker markerPrefab;

		// Token: 0x04000FB9 RID: 4025
		[SerializeField]
		private Image linePrefab;
	}
}
