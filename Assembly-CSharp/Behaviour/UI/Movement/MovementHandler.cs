using System;
using Behaviour.GalaxyMap;
using Behaviour.Managers;
using Behaviour.UI.HUD;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Movement
{
	// Token: 0x02000256 RID: 598
	public class MovementHandler : Singleton<MovementHandler>
	{
		// Token: 0x06001615 RID: 5653 RVA: 0x0008C97B File Offset: 0x0008AB7B
		private void Start()
		{
			this.gameplayManager = GameplayManager.Instance;
			HudManager instance = HudManager.Instance;
			this.mask = ((instance != null) ? instance.salvageMask : null);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0008C99F File Offset: 0x0008AB9F
		private void Update()
		{
			this.HandleMouseHold();
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0008C9A8 File Offset: 0x0008ABA8
		private void HandleMouseHold()
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.startedOverUI = (UIHelper.IsMouseOverUi || AbstractGalaxyMapManager.IsShowing() || SpaceStationInterior.instance || Singleton<TravelManager>.Instance.TravelActive());
				this.isHoldingMouse = !this.startedOverUI;
				if (this.isHoldingMouse)
				{
					this.mouseHoldTimer = 0f;
					this.holdRadialImage.gameObject.SetActive(true);
					this.UpdateRadialPosition();
					this.holdRadialImage.fillAmount = 0f;
				}
			}
			if (Input.GetMouseButton(0) && this.isHoldingMouse)
			{
				this.mouseHoldTimer += Time.deltaTime;
				if (this.mouseHoldTimer > 0.15f)
				{
					float fillAmount = Mathf.Clamp01(this.mouseHoldTimer / 0.6f);
					this.holdRadialImage.fillAmount = fillAmount;
					this.UpdateRadialPosition();
				}
			}
			if (!this.movementOrderGiven && this.isHoldingMouse && this.mouseHoldTimer >= 0.6f)
			{
				if (!SpacestationExteriorManager.Instance || SpacestationExteriorManager.Instance.CancelDocking(this.gameplayManager.spaceShip, true))
				{
					Vector2 vector = this.gameplayManager.cameraMovement.gameCamera.ScreenToWorldPoint(Input.mousePosition);
					this.gameplayManager.spaceShip.SetOverrideDestination(vector, true, false, true);
					this.movementOrderGiven = true;
					this.SetNewMarker(vector, this.gameplayManager.cameraMovement.gameCamera);
				}
				this.mouseHoldTimer = 0f;
				this.holdRadialImage.gameObject.SetActive(false);
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.ResetMouseHoldState();
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0008CB4C File Offset: 0x0008AD4C
		private void UpdateRadialPosition()
		{
			Vector2 a;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.holdRadialImage.canvas.transform as RectTransform, Input.mousePosition, this.holdRadialImage.canvas.worldCamera, out a);
			Vector2 b = new Vector2(3f, -10f);
			this.holdRadialImage.rectTransform.anchoredPosition = a + b;
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0008CBB8 File Offset: 0x0008ADB8
		private void OnApplicationFocus(bool hasFocus)
		{
			if (!hasFocus)
			{
				this.ResetMouseHoldState();
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0008CBC3 File Offset: 0x0008ADC3
		private void ResetMouseHoldState()
		{
			this.isHoldingMouse = false;
			this.movementOrderGiven = false;
			this.startedOverUI = false;
			this.mouseHoldTimer = 0f;
			this.holdRadialImage.gameObject.SetActive(false);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0008CBF8 File Offset: 0x0008ADF8
		public void SetNewMarker(Vector2 worldPosition, Camera camera)
		{
			if (this.currentMoveOrderMarker != null)
			{
				UnityEngine.Object.Destroy(this.currentMoveOrderMarker);
			}
			MoveOrderMarker moveOrderMarker = UnityEngine.Object.Instantiate<MoveOrderMarker>(this.moveOrderMarkerPrefab, this.mask.transform);
			moveOrderMarker.Initialize(worldPosition, camera);
			this.currentMoveOrderMarker = moveOrderMarker.gameObject;
		}

		// Token: 0x04000D49 RID: 3401
		[SerializeField]
		private Canvas canvas;

		// Token: 0x04000D4A RID: 3402
		[SerializeField]
		private Image holdRadialImage;

		// Token: 0x04000D4B RID: 3403
		private float mouseHoldTimer;

		// Token: 0x04000D4C RID: 3404
		private const float requiredHoldTime = 0.6f;

		// Token: 0x04000D4D RID: 3405
		private bool isHoldingMouse;

		// Token: 0x04000D4E RID: 3406
		private bool startedOverUI;

		// Token: 0x04000D4F RID: 3407
		private bool movementOrderGiven;

		// Token: 0x04000D50 RID: 3408
		[SerializeField]
		private MoveOrderMarker moveOrderMarkerPrefab;

		// Token: 0x04000D51 RID: 3409
		private GameObject currentMoveOrderMarker;

		// Token: 0x04000D52 RID: 3410
		private GameplayManager gameplayManager;

		// Token: 0x04000D53 RID: 3411
		private Mask mask;
	}
}
