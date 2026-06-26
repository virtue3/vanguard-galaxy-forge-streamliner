using System;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B7 RID: 951
	public class SmokeBackground : AbstractBackgroundLayer, ICameraTrackable
	{
		// Token: 0x0600247C RID: 9340 RVA: 0x000CD940 File Offset: 0x000CBB40
		protected override void Awake()
		{
			base.Awake();
			this.noiseTileIdentifier = Shader.PropertyToID("_FogNoiseTile");
			this.worldPositionIdentifier = Shader.PropertyToID("_WorldPosition");
			this.cameraChangeIdentifier = Shader.PropertyToID("_CameraChange");
			this.noiseScaleIdentifier = Shader.PropertyToID("_NoiseScale");
			this.maskScaleIdentifier = Shader.PropertyToID("_MaskScale");
			this.colorPowerIdentifier = Shader.PropertyToID("_ColorPower");
			this.SetShaderProperties();
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000CD9B9 File Offset: 0x000CBBB9
		protected override void SetSpriteRenderer()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			this.material = this.spriteRenderer.material;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000CD9D8 File Offset: 0x000CBBD8
		public override void SetCamera()
		{
			this.cameraToTrack = Camera.main.GetComponent<CameraMovement>().gameCamera;
			this.cameraTracker.SetCamera(this.cameraToTrack);
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000CDA00 File Offset: 0x000CBC00
		private bool TravelActiveCheck()
		{
			return this.distance == 1 && Singleton<TravelManager>.Instance && Singleton<TravelManager>.Instance.TravelActive();
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000CDA24 File Offset: 0x000CBC24
		private void Update()
		{
			Color color = this.spriteRenderer.color;
			bool flag = BasePoiManager.current != null;
			if (this.TravelActiveCheck())
			{
				bool flag2 = Singleton<TravelManager>.Instance.AreWeLeaving();
				if (flag)
				{
					if (flag2)
					{
						float travelSpeed = GamePlayer.current.currentSpaceShip.travelSpeed;
						if (travelSpeed < 0.2f)
						{
							color.a = Mathf.Clamp(1f - travelSpeed / 0.15f, 0f, 1f);
						}
					}
					else
					{
						float magnitude = BasePoiManager.current.GetLocationDifferenceForSpaceship().magnitude;
						float num = 3f;
						if (magnitude < num)
						{
							this.smokeAlpha = 1f - magnitude * 1f / num;
							color.a = Mathf.Clamp(this.smokeAlpha, 0f, 1f);
						}
						else
						{
							color.a = 0f;
							this.smokeAlpha = 0f;
						}
					}
				}
				else
				{
					color.a = 0f;
					this.smokeAlpha = 0f;
				}
			}
			else
			{
				color.a = 1f;
			}
			this.spriteRenderer.enabled = (color.a > 0f);
			this.spriteRenderer.color = color;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000CDB64 File Offset: 0x000CBD64
		private void OnValidate()
		{
			if (this.screenSize != Vector2.zero)
			{
				this.SetScreenSize(this.screenSize, this.screenSizeGame);
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000CDB8C File Offset: 0x000CBD8C
		private void SetShaderProperties()
		{
			this.spriteRenderer.material.SetVector(this.noiseTileIdentifier, this.fogNoiseTile);
			this.spriteRenderer.material.SetVector(this.worldPositionIdentifier, this.screenSizeGame);
			this.spriteRenderer.material.SetFloat(this.noiseScaleIdentifier, this.noiseScale);
			this.spriteRenderer.material.SetFloat(this.colorPowerIdentifier, this.colorPower);
			this.spriteRenderer.material.SetFloat(this.maskScaleIdentifier, this.maskScale);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000CDC30 File Offset: 0x000CBE30
		public override void SetScreenSize(Vector2 screenSize, Vector2 screenSizeGame)
		{
			base.SetScreenSize(screenSize, screenSizeGame);
			Vector2 vector = new Vector2(screenSizeGame.x / screenSizeGame.y, 1f) * this.noiseTileFactor;
			this.fogNoiseTile = vector;
			this.SetShaderProperties();
			this.spriteRenderer.transform.localScale = this.screenSizeGame / this.initialBounds;
			this.localScale = this.screenSizeGame / this.initialBounds;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000CDCB8 File Offset: 0x000CBEB8
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			if (!this.cameraToTrack)
			{
				return;
			}
			Vector2 a = this.GetLocalOffset(false);
			this.totalPositionChange += delta;
			Vector2 vector = this.GetWorldPosition();
			if (this.distance != 1)
			{
				a = this.GetLocalOffset(true);
				float d = (CameraMovement.maxZoom - this.cameraToTrack.orthographicSize) * 0.1f / (float)this.distance;
				base.transform.localScale = this.localScale * this.cameraToTrack.orthographicSize / CameraMovement.maxZoom + this.localScale * d;
				a *= 10f / (float)this.distance * (this.cameraToTrack.orthographicSize / CameraMovement.maxZoom);
				vector = vector * 0.05f / (float)(this.distance * 10);
			}
			if (this.spriteRenderer)
			{
				this.spriteRenderer.material.SetVector(this.worldPositionIdentifier, vector);
				this.spriteRenderer.material.SetVector(this.cameraChangeIdentifier, a / this.screenSizeGame);
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000CDE00 File Offset: 0x000CC000
		private Vector2 GetWorldPosition()
		{
			if (this.distance != 1)
			{
				return this.cameraToTrack.transform.position;
			}
			GamePlayer current = GamePlayer.current;
			if (((current != null) ? current.currentPointOfInterest : null) != null)
			{
				return GamePlayer.current.currentPointOfInterest.position * MapPointOfInterest.mapToLocalConversion;
			}
			GameplayManager instance = GameplayManager.Instance;
			if ((instance != null) ? instance.spaceShip : null)
			{
				return GameplayManager.Instance.spaceShip.transform.position;
			}
			return Vector2.zero;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000CDE90 File Offset: 0x000CC090
		private Vector2 GetLocalOffset(bool furtherBack = false)
		{
			if (!furtherBack)
			{
				GamePlayer current = GamePlayer.current;
				if (((current != null) ? current.currentPointOfInterest : null) != null)
				{
					return (Vector2)this.cameraToTrack.transform.position - GamePlayer.current.currentPointOfInterest.position * MapPointOfInterest.mapToLocalConversion;
				}
			}
			return Vector2.zero;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000CDEEC File Offset: 0x000CC0EC
		public SmokeBackgroundData GetData()
		{
			if (this.data == null)
			{
				this.data = new SmokeBackgroundData();
			}
			this.data.color = this.spriteRenderer.color;
			this.data.noiseTileFactor = this.noiseTileFactor;
			this.data.noiseScale = this.spriteRenderer.material.GetFloat(this.noiseScaleIdentifier);
			this.data.maskScale = this.spriteRenderer.material.GetFloat(this.maskScaleIdentifier);
			this.data.colorPower = this.spriteRenderer.material.GetFloat(this.colorPowerIdentifier);
			this.data.distance = this.distance;
			this.data.sortingLayerName = this.spriteRenderer.sortingLayerName;
			return this.data;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000CDFC4 File Offset: 0x000CC1C4
		public void SetData(SmokeBackgroundData data)
		{
			this.data = data;
			this.spriteRenderer.color = data.color;
			this.noiseTileFactor = data.noiseTileFactor;
			this.noiseScale = data.noiseScale;
			this.maskScale = data.maskScale;
			this.colorPower = data.colorPower;
			this.distance = data.distance;
			this.spriteRenderer.sortingLayerName = data.sortingLayerName;
		}

		// Token: 0x04001607 RID: 5639
		[SerializeField]
		private float noiseTileFactor;

		// Token: 0x04001608 RID: 5640
		[SerializeField]
		private int distance = 1;

		// Token: 0x04001609 RID: 5641
		private Vector2 fogNoiseTile;

		// Token: 0x0400160A RID: 5642
		private float noiseScale = 60f;

		// Token: 0x0400160B RID: 5643
		private float maskScale = 3f;

		// Token: 0x0400160C RID: 5644
		private float colorPower = 1f;

		// Token: 0x0400160D RID: 5645
		private int noiseTileIdentifier;

		// Token: 0x0400160E RID: 5646
		private int noiseScaleIdentifier;

		// Token: 0x0400160F RID: 5647
		private int maskScaleIdentifier;

		// Token: 0x04001610 RID: 5648
		private int colorPowerIdentifier;

		// Token: 0x04001611 RID: 5649
		private int worldPositionIdentifier;

		// Token: 0x04001612 RID: 5650
		private int cameraChangeIdentifier;

		// Token: 0x04001613 RID: 5651
		private Vector2 totalPositionChange = Vector2.zero;

		// Token: 0x04001614 RID: 5652
		private Vector3 localScale;

		// Token: 0x04001615 RID: 5653
		private Camera cameraToTrack;

		// Token: 0x04001616 RID: 5654
		private SmokeBackgroundData data;

		// Token: 0x04001617 RID: 5655
		private float smokeAlpha;
	}
}
