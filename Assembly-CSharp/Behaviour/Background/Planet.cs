using System;
using Source.Galaxy;
using Source.Galaxy.Statics;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B5 RID: 949
	public class Planet : MonoBehaviour, ICameraTrackable
	{
		// Token: 0x0600246D RID: 9325 RVA: 0x000CD463 File Offset: 0x000CB663
		private void Awake()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000CD471 File Offset: 0x000CB671
		private void Start()
		{
			this.cameraTracker = base.GetComponent<CameraTracker>();
			this.camera = Camera.main;
			this.gameCamera = this.camera.GetComponent<CameraMovement>().gameCamera;
			this.cameraTracker.SetCamera(this.camera);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000CD4B4 File Offset: 0x000CB6B4
		public void SetPlanetData(Source.Galaxy.Statics.Planet planetData)
		{
			this.planetData = planetData;
			Texture2D value = Resources.Load<Texture2D>("Background/PlanetTexture/" + planetData.planetType.ToString() + "/" + planetData.GetTextureName());
			this.spriteRenderer.material.SetTexture("_PlanetTexture", value);
			this.spriteRenderer.material.SetInt("_TotalPixels", (int)(planetData.scale / 2f * (float)Planet.planetPPU));
			this.atmosphereRenderer.gameObject.SetActive(planetData.atmosphere);
			if (planetData.atmosphere)
			{
				this.atmosphereRenderer.material.SetInt("_TotalPixels", (int)(planetData.scale / 5f * (float)Planet.planetPPU));
			}
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, planetData.tilt.Value));
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000CD5A8 File Offset: 0x000CB7A8
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			Vector3 a = this.planetData.position * MapPointOfInterest.mapToLocalConversion - newPosition;
			this.relativePosition = a / (this.distanceFactor / (this.gameCamera.orthographicSize / CameraMovement.maxZoom));
			this.relativePosition.z = 1f;
			float d = (CameraMovement.maxZoom - this.gameCamera.orthographicSize) * 0.1f;
			this.localScale = Vector3.one * this.planetData.scale * this.gameCamera.orthographicSize / CameraMovement.maxZoom + Vector3.one * d;
			base.transform.localScale = this.localScale;
			base.transform.localPosition = this.relativePosition;
		}

		// Token: 0x040015F3 RID: 5619
		private static int planetPPU = 64;

		// Token: 0x040015F4 RID: 5620
		private CameraTracker cameraTracker;

		// Token: 0x040015F5 RID: 5621
		private Source.Galaxy.Statics.Planet planetData;

		// Token: 0x040015F6 RID: 5622
		private float distanceFactor = 5f;

		// Token: 0x040015F7 RID: 5623
		private Camera camera;

		// Token: 0x040015F8 RID: 5624
		private Camera gameCamera;

		// Token: 0x040015F9 RID: 5625
		private Vector3 relativePosition;

		// Token: 0x040015FA RID: 5626
		private Vector3 localScale;

		// Token: 0x040015FB RID: 5627
		private SpriteRenderer spriteRenderer;

		// Token: 0x040015FC RID: 5628
		[SerializeField]
		private SpriteRenderer atmosphereRenderer;
	}
}
