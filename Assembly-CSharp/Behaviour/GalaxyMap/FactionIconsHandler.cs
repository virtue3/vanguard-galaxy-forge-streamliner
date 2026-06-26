using System;
using System.Collections.Generic;
using Behaviour.UI;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Simulation.World.System;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x0200032C RID: 812
	public class FactionIconsHandler : MonoBehaviour
	{
		// Token: 0x06001EB7 RID: 7863 RVA: 0x000B6470 File Offset: 0x000B4670
		public void SetSystemFactionIcons(WorldMapStatic wms)
		{
			this.systemParent = wms;
			SystemMapData systemMapData = wms.content as SystemMapData;
			if (systemMapData != null)
			{
				foreach (SpaceStation spaceStation in systemMapData.GetPointsOfInterest<SpaceStation>())
				{
					if (!spaceStation.hidden)
					{
						bool embassy = false;
						bool hq = false;
						if (spaceStation is EmbassyStation)
						{
							embassy = true;
						}
						ConquestSystem conquestSystem = systemMapData.storyteller as ConquestSystem;
						if (conquestSystem != null && conquestSystem.headquarters)
						{
							if (conquestSystem.umbralControlLevel > 0f)
							{
								this.AddIcon(Faction.puppeteers, false, false);
							}
							if (conquestSystem.headquarters && conquestSystem.faction == spaceStation.faction)
							{
								hq = true;
							}
						}
						this.AddIcon(spaceStation.faction, embassy, hq);
					}
				}
				this.UpdateLayout();
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000B6550 File Offset: 0x000B4750
		public void SetPoiFactionIcon(WorldMapPOI wmp)
		{
			this.poiParent = wmp;
			MapPointOfInterest content = wmp.content;
			if (content != null && content.faction != null && !content.hidden)
			{
				bool embassy = false;
				bool hq = false;
				if (content is EmbassyStation)
				{
					embassy = true;
				}
				ConquestSystem conquestSystem = content.system.storyteller as ConquestSystem;
				if (conquestSystem != null)
				{
					if (conquestSystem.umbralControlLevel > 0f && content is ConquestStation)
					{
						this.AddIcon(Faction.puppeteers, false, false);
					}
					if (conquestSystem.headquarters && conquestSystem.faction == content.faction)
					{
						hq = true;
					}
				}
				this.AddIcon(content.faction, embassy, hq);
				this.UpdateLayout();
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000B65F4 File Offset: 0x000B47F4
		private void AddIcon(Faction faction, bool embassy = false, bool hq = false)
		{
			if (!this.factionsInSystem.Add(faction))
			{
				return;
			}
			FactionIconSet factionIconSet = FactionIconSet.Get(faction);
			Sprite sprite = (factionIconSet != null) ? factionIconSet.mapIcon : null;
			if (sprite)
			{
				FactionIcon component = UnityEngine.Object.Instantiate<GameObject>(this.factionIconPrefab.gameObject, base.transform).GetComponent<FactionIcon>();
				component.SetFactionIcon(sprite);
				if (embassy)
				{
					component.SetEmbassy();
				}
				if (hq)
				{
					component.SetIsHeadquarters();
				}
				component.transform.localScale = Vector3.one;
				this.icons.Add(component.transform);
			}
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000B6684 File Offset: 0x000B4884
		private void UpdateLayout()
		{
			int count = this.icons.Count;
			if (count == 0)
			{
				return;
			}
			float num = -((float)(count - 1) * this.spacing) / 2f;
			for (int i = 0; i < count; i++)
			{
				Vector3 localPosition = this.icons[i].localPosition;
				localPosition.x = num + (float)i * this.spacing;
				this.icons[i].localPosition = localPosition;
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000B66F6 File Offset: 0x000B48F6
		public bool HasFaction(Faction faction)
		{
			return this.factionsInSystem.Contains(faction);
		}

		// Token: 0x04001273 RID: 4723
		[SerializeField]
		private FactionIcon factionIconPrefab;

		// Token: 0x04001274 RID: 4724
		[SerializeField]
		private float spacing = 1.2f;

		// Token: 0x04001275 RID: 4725
		private readonly List<Transform> icons = new List<Transform>();

		// Token: 0x04001276 RID: 4726
		private readonly HashSet<Faction> factionsInSystem = new HashSet<Faction>();

		// Token: 0x04001277 RID: 4727
		private WorldMapStatic systemParent;

		// Token: 0x04001278 RID: 4728
		private WorldMapPOI poiParent;
	}
}
