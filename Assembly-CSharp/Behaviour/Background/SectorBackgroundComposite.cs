using System;
using System.Collections.Generic;
using Source.Galaxy;
using Source.Player;
using UnityEngine;
using _Scripts.Behaviour.Background;

namespace Behaviour.Background
{
	// Token: 0x020003B6 RID: 950
	public class SectorBackgroundComposite : AbstractGalaxyMapBackground, ICameraTrackable
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x000CD6A8 File Offset: 0x000CB8A8
		public void LoadBackgroundData(SectorMapData sector, bool isGalaxyMap = false)
		{
			base.transform.DestroyChildren();
			this.spriteRenderers.Clear();
			this.isGalaxyMap = isGalaxyMap;
			if (GamePlayer.current != null)
			{
				this.sectorData = sector;
				this.data = SectorBackgroundCompositeData.CreateForSector(sector);
			}
			else
			{
				this.sectorData = sector;
				this.data = SectorBackgroundCompositeData.CreateForSector(sector);
			}
			if (this.setPresetCoroutine != null)
			{
				base.StopCoroutine(this.setPresetCoroutine);
			}
			this.SetBackground();
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000CD71C File Offset: 0x000CB91C
		public override void SetBackgroundAlpha(float alpha)
		{
			foreach (SpriteRenderer spriteRenderer in this.spriteRenderers)
			{
				Color color = spriteRenderer.color;
				color.a = alpha;
				spriteRenderer.color = color;
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000CD77C File Offset: 0x000CB97C
		public void SetScreenSize(Vector2 screenSize, Vector2 screenSizeGame)
		{
			this.screenSize = screenSize;
			this.screenSizeGame = screenSizeGame;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000CD78C File Offset: 0x000CB98C
		private void SetBackground()
		{
			base.transform.DestroyChildren();
			foreach (NebulaData nebulaData in this.data.nebulae)
			{
				Nebula nebula = this.InstantiateNebula();
				nebula.SetData(nebulaData);
				nebula.transform.localScale = new Vector2(0.5f, 0.5f);
				this.spriteRenderers.Add(nebula.spriteRenderer);
			}
			this.InstantiateStarBackground();
			this.starBackground.SetData(this.data.starLayerPerformantData);
			this.spriteRenderers.Add(this.starBackground.background);
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000CD858 File Offset: 0x000CBA58
		private void InstantiateStarBackground()
		{
			this.starBackground = UnityEngine.Object.Instantiate<StarLayerPerformant>(this.starLayerPerformantPrefab, base.transform);
			StarLayerPerformant starLayerPerformant = this.starBackground;
			Vector2 vector = this.screenSize;
			SectorMapData sectorMapData = this.sectorData;
			starLayerPerformant.SetScreenSize(vector, (sectorMapData != null) ? sectorMapData.position : Vector2.zero);
			this.starBackground.gameObject.layer = base.gameObject.layer;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000CD8BE File Offset: 0x000CBABE
		private Nebula InstantiateNebula()
		{
			return UnityEngine.Object.Instantiate<Nebula>(this.nebulaPrefab, base.transform);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000CD8D4 File Offset: 0x000CBAD4
		public void SetLayer(int layer)
		{
			base.gameObject.layer = layer;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.layer = layer;
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000CD91A File Offset: 0x000CBB1A
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			base.transform.position = newPosition;
		}

		// Token: 0x040015FD RID: 5629
		[SerializeField]
		private Nebula nebulaPrefab;

		// Token: 0x040015FE RID: 5630
		[SerializeField]
		private StarLayerPerformant starLayerPerformantPrefab;

		// Token: 0x040015FF RID: 5631
		private Vector2 screenSize;

		// Token: 0x04001600 RID: 5632
		private Vector2 screenSizeGame;

		// Token: 0x04001601 RID: 5633
		public SectorBackgroundCompositeData data;

		// Token: 0x04001602 RID: 5634
		public SectorMapData sectorData;

		// Token: 0x04001603 RID: 5635
		private StarLayerPerformant starBackground;

		// Token: 0x04001604 RID: 5636
		private bool isGalaxyMap;

		// Token: 0x04001605 RID: 5637
		private Coroutine setPresetCoroutine;

		// Token: 0x04001606 RID: 5638
		private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
	}
}
