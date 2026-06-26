using System;
using Behaviour.Background;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace _Scripts.Behaviour.Background.Parallax
{
	// Token: 0x0200019E RID: 414
	public class ParallaxLayer : MonoBehaviour, ICameraTrackable
	{
		// Token: 0x06000E96 RID: 3734 RVA: 0x000682BD File Offset: 0x000664BD
		private void Start()
		{
			this.cameraTracker = base.GetComponent<CameraTracker>();
			this.camera = Camera.main;
			this.gameCamera = this.camera.GetComponent<CameraMovement>().gameCamera;
			this.cameraTracker.SetCamera(this.camera);
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00068300 File Offset: 0x00066500
		private void Update()
		{
			this.aliveTimer -= Time.deltaTime;
			if (this.aliveTimer <= 0f)
			{
				if (this.poi == null || !Singleton<TravelManager>.Instance || (this.poi.leftPoi() && Singleton<TravelManager>.Instance.SpaceshipTravelPastHalfwayPoint()))
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				this.aliveTimer = 1f;
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0006836F File Offset: 0x0006656F
		public void Init(MapPointOfInterest poi, Rect size)
		{
			this.poi = poi;
			this.size = size;
			this.random = new SeedGenerator().Add(poi.backgroundSeed).CreateRandom();
			this.AddBackgroundElements();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x000683A8 File Offset: 0x000665A8
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			float d = (CameraMovement.maxZoom - this.gameCamera.orthographicSize) * 0.005f;
			Vector3 vector = Vector3.one * 0.5f * this.gameCamera.orthographicSize / CameraMovement.maxZoom + Vector3.one * d;
			Vector3 localPosition = (this.poi.position * MapPointOfInterest.mapToLocalConversion - newPosition) * vector.x;
			localPosition.z = 1f;
			base.transform.localScale = vector;
			base.transform.localPosition = localPosition;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00068458 File Offset: 0x00066658
		private Vector2 GetRandomWithinRect()
		{
			Rect rect = new Rect(this.size.x - this.size.width / 2f, this.size.y - this.size.height / 2f, this.size.width * 2f, this.size.height * 2f);
			return this.random.RandomWithinRect(rect) - new Vector2(rect.center.x, rect.center.y);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x000684F6 File Offset: 0x000666F6
		private void AddBackgroundElements()
		{
			if (this.poi.hasAsteroids)
			{
				this.AddAsteroids();
			}
			this.AddSmokePlumes();
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00068514 File Offset: 0x00066714
		private void AddSmokePlumes()
		{
			int num = (int)this.size.width / 10;
			for (int i = 0; i < num; i++)
			{
				SmokePlume smokePlume = UnityEngine.Object.Instantiate<SmokePlume>(this.smokePlumePrefab, base.transform);
				smokePlume.transform.localPosition = this.GetRandomWithinRect();
				smokePlume.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)this.random.RandomRange(0, 360)));
				smokePlume.transform.localScale = new Vector3(this.random.RandomRange(15f, 20f), this.random.RandomRange(15f, 20f), 1f);
				smokePlume.SetColor(Singleton<BackdropManager>.Instance.GetSmokeColor(this.random.RandomRange(0f, 1f)));
				smokePlume.SetTile(this.random.RandomRange(0, 15));
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00068614 File Offset: 0x00066814
		private void AddAsteroids()
		{
			int num = (int)this.size.width / 4;
			for (int i = 0; i < num; i++)
			{
				int num2 = this.random.RandomRange(0, 30);
				Sprite surface = Resources.Load<Sprite>("Background/Asteroids/Medium_" + num2.ToString());
				Texture2D normal = Resources.Load<Texture2D>("Background/Asteroids/Medium_" + num2.ToString() + "_n");
				this.CreateAsteroid(surface, normal, i);
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0006868C File Offset: 0x0006688C
		private void CreateAsteroid(Sprite surface, Texture2D normal, int i)
		{
			Sprite sprite = Sprite.Create(surface.texture, new Rect(new Vector2(0f, 0f), new Vector2((float)surface.texture.width, (float)surface.texture.height)), new Vector2(0.5f, 0.5f), 32f, 0U, SpriteMeshType.Tight, Vector4.zero, true, new SecondarySpriteTexture[]
			{
				new SecondarySpriteTexture
				{
					name = "_NormalMap",
					texture = normal
				}
			});
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = base.transform;
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.color = ColorHelper.fadedGrey;
			spriteRenderer.sortingLayerName = "Planets";
			spriteRenderer.sortingOrder = 2;
			spriteRenderer.sprite = sprite;
			gameObject.transform.localPosition = this.GetRandomWithinRect();
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)this.random.RandomRange(0, 360)));
		}

		// Token: 0x04000832 RID: 2098
		[SerializeField]
		private SmokePlume smokePlumePrefab;

		// Token: 0x04000833 RID: 2099
		private MapPointOfInterest poi;

		// Token: 0x04000834 RID: 2100
		private Rect size;

		// Token: 0x04000835 RID: 2101
		private CameraTracker cameraTracker;

		// Token: 0x04000836 RID: 2102
		private Camera gameCamera;

		// Token: 0x04000837 RID: 2103
		private Camera camera;

		// Token: 0x04000838 RID: 2104
		protected SeededRandom random;

		// Token: 0x04000839 RID: 2105
		private float aliveTimer = 1f;
	}
}
