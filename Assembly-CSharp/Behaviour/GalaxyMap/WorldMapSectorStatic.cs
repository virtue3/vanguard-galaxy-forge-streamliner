using System;
using System.Collections.Generic;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000337 RID: 823
	public class WorldMapSectorStatic : WorldMapStatic
	{
		// Token: 0x06001F2F RID: 7983 RVA: 0x000B9AE8 File Offset: 0x000B7CE8
		public override void SetContent(MapElement stc)
		{
			this.content = stc;
			this.sectorMapData = (SectorMapData)stc;
			SectorBackgroundCompositeData sectorBackgroundCompositeData = SectorBackgroundCompositeData.CreateForSector(this.sectorMapData);
			this.stars = new List<SpriteRenderer>();
			foreach (SystemMapData systemMapData in this.sectorMapData.allSystems)
			{
				Vector3 localPosition = systemMapData.position / (float)(this.sectorMapData.conquestSector ? 7 : 13);
				localPosition.z = -1f;
				SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(this.spritePrefab, base.transform);
				spriteRenderer.material.SetColor("_Color", systemMapData.GetColor());
				spriteRenderer.material.SetFloat("_Size", 0.03f);
				spriteRenderer.transform.localScale = new Vector3(systemMapData.GetSize(), systemMapData.GetSize(), 1f);
				spriteRenderer.transform.localPosition = localPosition;
				spriteRenderer.gameObject.SetActive(true);
				this.stars.Add(spriteRenderer);
			}
			this.spriteRenderer.material.SetColor("_BackgroundColor", sectorBackgroundCompositeData.starLayerPerformantData.backgroundColor);
			this.spriteRenderer.material.SetInt("_FadeEdges", 1);
			this.spriteRenderer.transform.localScale = new Vector3(20f, 10f, 1f);
			base.backgroundRenderer.transform.localScale = new Vector3(20f, 10f, 1f) / 2f;
			this.originalBackgroundScale = base.backgroundRenderer.transform.localScale;
			this.minScale = base.backgroundRenderer.transform.localScale.x;
			SectorMapData sectorMapData = this.content as SectorMapData;
			if (sectorMapData != null)
			{
				foreach (Mission mission in GamePlayer.current.missions)
				{
					if (!mission.isComplete || mission.turnIn == null || mission.turnIn.system.sector != this.content)
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
				foreach (SystemMapData systemMapData2 in sectorMapData.allSystems)
				{
					using (IEnumerator<SpaceStation> enumerator3 = systemMapData2.GetPointsOfInterest<SpaceStation>().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.HasAvailableMission())
							{
								base.SetAvailableMissionMarker(base.missionLocation.gameObject.activeSelf ? base.missionLocation2 : base.missionLocation);
								break;
							}
						}
					}
				}
				SpaceStation homeStation = GamePlayer.current.homeStation;
				MapElement mapElement;
				if (homeStation == null)
				{
					mapElement = null;
				}
				else
				{
					SystemMapData system = homeStation.system;
					mapElement = ((system != null) ? system.sector : null);
				}
				if (mapElement == this.content)
				{
					base.SetHomebaseMarker();
				}
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000B9E48 File Offset: 0x000B8048
		public override List<SpriteRenderer> GetSpriteRenderers()
		{
			List<SpriteRenderer> spriteRenderers = base.GetSpriteRenderers();
			spriteRenderers.AddRange(this.stars);
			return spriteRenderers;
		}

		// Token: 0x040012AF RID: 4783
		[SerializeField]
		private SpriteRenderer spritePrefab;

		// Token: 0x040012B0 RID: 4784
		private SectorMapData sectorMapData;

		// Token: 0x040012B1 RID: 4785
		private List<SpriteRenderer> stars;
	}
}
