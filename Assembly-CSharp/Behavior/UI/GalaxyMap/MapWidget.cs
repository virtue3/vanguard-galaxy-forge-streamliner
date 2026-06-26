using System;
using System.Collections.Generic;
using Behaviour.GalaxyMap;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behavior.UI.GalaxyMap
{
	// Token: 0x02000193 RID: 403
	public class MapWidget : MonoBehaviour
	{
		// Token: 0x06000E59 RID: 3673 RVA: 0x00067398 File Offset: 0x00065598
		private void OnEnable()
		{
			this.quadrants = new List<int>();
			if (GalaxyMapData.current == null)
			{
				return;
			}
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				if (!this.quadrants.Contains(sectorMapData.quadrant))
				{
					this.quadrants.Add(sectorMapData.quadrant);
				}
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0006741C File Offset: 0x0006561C
		private void Update()
		{
			this.UpdateConquestText();
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00067424 File Offset: 0x00065624
		private void UpdateConquestText()
		{
			if (this.conquestUpdate.gameObject.activeSelf)
			{
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				if (storyteller != null)
				{
					this.conquestUpdate.TL("@ConquestUpdateTimer", new object[]
					{
						GameMath.FormatTime(Mathf.RoundToInt(storyteller.conquestTickTime), true)
					});
					return;
				}
				this.conquestUpdate.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00067490 File Offset: 0x00065690
		public string GetQuadrantName()
		{
			string result = null;
			if (AbstractGalaxyMapManager.current.currentQuadrant > 0)
			{
				result = ((AbstractGalaxyMapManager.current.currentQuadrant == SectorMapData.quadrantFrontier) ? "@MapConquest" : "@MapFrontier");
			}
			return result;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000674CC File Offset: 0x000656CC
		public void ShowMapContent(MapElement content)
		{
			if (content == null)
			{
				this.SetZoomLevel(2);
				string quadrantName = this.GetQuadrantName();
				if (quadrantName != null)
				{
					this.SetQuadrantLabel(quadrantName);
					return;
				}
			}
			else
			{
				SectorMapData sectorMapData = content as SectorMapData;
				if (sectorMapData != null)
				{
					this.ShowSector(sectorMapData, 1);
					return;
				}
				SystemMapData systemMapData = content as SystemMapData;
				if (systemMapData != null)
				{
					this.ShowSystem(systemMapData, 0);
				}
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0006751A File Offset: 0x0006571A
		public void ShowSector(SectorMapData sector, int zoomLevel)
		{
			this.currentSector = sector;
			SystemMapData systemMapData = this.currentSystem;
			if (((systemMapData != null) ? systemMapData.sector : null) != sector)
			{
				this.currentSystem = null;
			}
			this.UpdateButtons();
			this.SetZoomLevel(zoomLevel);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0006754C File Offset: 0x0006574C
		public void ShowSystem(SystemMapData sys, int zoomLevel)
		{
			this.currentSystem = sys;
			this.currentSector = ((sys != null) ? sys.sector : null);
			this.UpdateButtons();
			this.SetZoomLevel(zoomLevel);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00067574 File Offset: 0x00065774
		public void ToggleQuadrant()
		{
			int num = this.quadrants.IndexOf(AbstractGalaxyMapManager.current.currentQuadrant);
			if (num >= 0)
			{
				num = (num + 1) % this.quadrants.Count;
			}
			AbstractGalaxyMapManager.current.ShowGalaxyMap(this.quadrants[num]);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x000675C1 File Offset: 0x000657C1
		public void SetQuadrantLabel(string label)
		{
			if (!this.quadrantLabel)
			{
				return;
			}
			this.quadrantLabel.SetText(Translation.Translate(label, Array.Empty<object>()));
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000675E8 File Offset: 0x000657E8
		private void UpdateButtons()
		{
			this.galaxyButton.SetLabel("Canis Majoris" + " [1]".HighlightWithColor(ColorHelper.softYellow));
			this.sectorButton.SetLabel(this.currentSector.name + " [2]".HighlightWithColor(ColorHelper.softYellow));
			MapButton mapButton = this.systemButton;
			SystemMapData systemMapData = this.currentSystem;
			mapButton.SetLabel((((systemMapData != null) ? systemMapData.name : null) ?? "(...)") + " [3]".HighlightWithColor(ColorHelper.softYellow));
			RectTransform rectTransform = (RectTransform)this.galaxyButton.transform;
			RectTransform rectTransform2 = (RectTransform)this.sectorButton.transform;
			RectTransform rectTransform3 = (RectTransform)this.systemButton.transform;
			rectTransform2.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x + 5f, 0f);
			rectTransform3.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x + rectTransform2.sizeDelta.x + 5f, 0f);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0006770C File Offset: 0x0006590C
		public void SetZoomLevel(int zoomLevel)
		{
			this.galaxyButton.SetSelected(zoomLevel == 2);
			this.sectorButton.SetSelected(zoomLevel == 1);
			this.systemButton.SetSelected(zoomLevel == 0);
			this.conquestUpdate.gameObject.SetActive(GamePlayer.current.GetStoryteller<Conquest>() != null);
			this.UpdateConquestText();
			if (this.quadrantButton)
			{
				this.quadrantButton.gameObject.SetActive(zoomLevel == 2 && this.quadrants.Count > 1);
			}
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0006779B File Offset: 0x0006599B
		public void OnChangeMapZoomLevel(int zoomLevel)
		{
			AbstractGalaxyMapManager.ChangeZoomLevel(zoomLevel);
		}

		// Token: 0x04000814 RID: 2068
		[SerializeField]
		private MapButton galaxyButton;

		// Token: 0x04000815 RID: 2069
		[SerializeField]
		private MapButton sectorButton;

		// Token: 0x04000816 RID: 2070
		[SerializeField]
		private MapButton systemButton;

		// Token: 0x04000817 RID: 2071
		[SerializeField]
		private RectTransform quadrantButton;

		// Token: 0x04000818 RID: 2072
		[SerializeField]
		private TMP_Text quadrantLabel;

		// Token: 0x04000819 RID: 2073
		[SerializeField]
		private TMP_Text conquestUpdate;

		// Token: 0x0400081A RID: 2074
		private SectorMapData currentSector;

		// Token: 0x0400081B RID: 2075
		private SystemMapData currentSystem;

		// Token: 0x0400081C RID: 2076
		private List<int> quadrants;
	}
}
