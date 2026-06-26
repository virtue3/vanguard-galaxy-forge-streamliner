using System;
using System.Collections.Generic;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Galaxy.Statics;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000338 RID: 824
	public class WorldMapStatic : WorldMapElement, ITooltipCustomSource, IWorldMapVisual, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001F32 RID: 7986 RVA: 0x000B9E64 File Offset: 0x000B8064
		public static WorldMapStatic GetPrefab(string name)
		{
			if (WorldMapStatic.prefabs == null)
			{
				WorldMapStatic.prefabs = new Dictionary<string, WorldMapStatic>();
				foreach (WorldMapStatic worldMapStatic in Resources.LoadAll<WorldMapStatic>("GalaxyMap/Statics"))
				{
					WorldMapStatic.prefabs[worldMapStatic.gameObject.name] = worldMapStatic;
				}
			}
			return WorldMapStatic.prefabs[name];
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000B9EC0 File Offset: 0x000B80C0
		protected override void Update()
		{
			if (this.content is Star || this.content is Planet)
			{
				return;
			}
			SystemMapData systemMapData = this.content as SystemMapData;
			bool highlightPlayer;
			if (systemMapData == null || SystemMapData.current != systemMapData)
			{
				SectorMapData sectorMapData = this.content as SectorMapData;
				highlightPlayer = (sectorMapData != null && SectorMapData.current == sectorMapData);
			}
			else
			{
				highlightPlayer = true;
			}
			base.highlightPlayer = highlightPlayer;
			base.Update();
			base.backgroundRenderer.gameObject.SetActive((!AbstractGalaxyMapManager.current.tweening && base.highlightMouse) || base.highlightPlayer);
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000B9F55 File Offset: 0x000B8155
		private void Start()
		{
			if (this.content is Planet)
			{
				base.GetComponent<TooltipSource>().enabled = false;
			}
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000B9F70 File Offset: 0x000B8170
		private void OnMouseUpAsButton()
		{
			this.LeftClickHandler();
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000B9F78 File Offset: 0x000B8178
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				this.LeftClickHandler();
			}
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000B9F88 File Offset: 0x000B8188
		private void LeftClickHandler()
		{
			SystemMapData systemMapData = this.content as SystemMapData;
			if (systemMapData != null)
			{
				AbstractGalaxyMapManager.current.ShowSystemMap(systemMapData);
				return;
			}
			SectorMapData sectorMapData = this.content as SectorMapData;
			if (sectorMapData != null)
			{
				AbstractGalaxyMapManager.current.ShowSectorMap(sectorMapData);
			}
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000B9FCC File Offset: 0x000B81CC
		public virtual void SetContent(MapElement stc)
		{
			this.content = stc;
			Color value = this.content.GetColor();
			this.spriteRenderer.material.SetColor("_Color", value);
			float num = (float)((stc is Star) ? 2 : 1);
			this.spriteRenderer.transform.localScale = new Vector3(this.content.GetSize() * num, this.content.GetSize() * num, 1f);
			base.backgroundRenderer.transform.localScale = this.spriteRenderer.transform.localScale / 2f * this.backgroundScale;
			this.minScale = base.backgroundRenderer.transform.localScale.x;
			SystemMapData systemMapData = this.content as SystemMapData;
			if (systemMapData != null)
			{
				foreach (Mission mission in GamePlayer.current.allMissions)
				{
					if (!mission.isComplete || mission.turnIn == null || mission.turnIn.system != this.content)
					{
						MissionStep currentStep = mission.currentStep;
						if (currentStep == null || !currentStep.IsPointOfInterest(this.content))
						{
							continue;
						}
					}
					base.SetMissionMarker(mission);
					break;
				}
				using (IEnumerator<SpaceStation> enumerator2 = systemMapData.GetPointsOfInterest<SpaceStation>().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.HasAvailableMission())
						{
							base.SetAvailableMissionMarker(base.missionLocation.gameObject.activeSelf ? base.missionLocation2 : base.missionLocation);
							break;
						}
					}
				}
				SystemMapData systemMapData2 = (SystemMapData)this.content;
				SpaceStation homeStation = GamePlayer.current.homeStation;
				if (systemMapData2 == ((homeStation != null) ? homeStation.system : null))
				{
					base.SetHomebaseMarker();
				}
			}
			if (base.factionIcons != null)
			{
				base.factionIcons.SetSystemFactionIcons(this);
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000BA1D8 File Offset: 0x000B83D8
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.content is Star)
			{
				tooltip.AddTextLine(this.content.name, 16, 8f);
			}
			else
			{
				SectorMapData sectorMapData = this.content as SectorMapData;
				if (sectorMapData != null)
				{
					ValueTuple<int, int> sectorLevel = sectorMapData.sectorLevel;
					tooltip.AddHeader(sectorMapData.name, sectorLevel.Item1, (sectorLevel.Item2 == 0) ? 9999 : sectorLevel.Item2, 16, 8f);
				}
				else
				{
					tooltip.AddHeader(this.content.name, this.content.level, 0, 16, 8f);
				}
			}
			MapStatic mapStatic = this.content as MapStatic;
			string text;
			if (mapStatic != null)
			{
				text = mapStatic.typeName;
			}
			else if (this.content is SectorMapData)
			{
				text = "@MapStaticSector";
			}
			else if (this.content is SystemMapData)
			{
				text = "@MapStaticSystem";
			}
			else
			{
				text = null;
			}
			if (text != null)
			{
				tooltip.AddTextLine(text, 12, 8f).Text.color = ColorHelper.boringGrey;
			}
			this.content.AddTooltipInfo(tooltip);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000BA2EC File Offset: 0x000B84EC
		public virtual List<SpriteRenderer> GetSpriteRenderers()
		{
			List<SpriteRenderer> list = new List<SpriteRenderer>
			{
				this.spriteRenderer,
				base.backgroundRenderer
			};
			if (base.missionLocation)
			{
				list.Add(base.missionLocation);
			}
			return list;
		}

		// Token: 0x040012B2 RID: 4786
		private static Dictionary<string, WorldMapStatic> prefabs;

		// Token: 0x040012B3 RID: 4787
		public MapElement content;

		// Token: 0x040012B4 RID: 4788
		[SerializeField]
		private Color color;

		// Token: 0x040012B5 RID: 4789
		[SerializeField]
		private float backgroundScale = 1f;
	}
}
