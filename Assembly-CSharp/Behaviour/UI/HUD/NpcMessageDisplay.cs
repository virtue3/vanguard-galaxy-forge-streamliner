using System;
using System.Collections;
using Behaviour.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000284 RID: 644
	public class NpcMessageDisplay : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060017B0 RID: 6064 RVA: 0x00094D60 File Offset: 0x00092F60
		public void Setup(AbstractUnit unit, Sprite npcIcon, string callsign, string message, float duration = 6f)
		{
			this.targetUnit = unit;
			this.icon.sprite = npcIcon;
			this.callsignText.text = callsign;
			this.messageText.text = message;
			this.camera = Camera.main.GetComponent<CameraMovement>().gameCamera;
			this.rectTransform = base.GetComponent<RectTransform>();
			this.SetPosition();
			base.StartCoroutine(this.DisplayRoutine(duration));
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00094DCF File Offset: 0x00092FCF
		private void Update()
		{
			if (this.targetUnit == null || !this.targetUnit.gameObject.activeInHierarchy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00094DFC File Offset: 0x00092FFC
		public void OnPointerClick(PointerEventData eventData)
		{
			base.StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00094E10 File Offset: 0x00093010
		private void SetPosition()
		{
			if (this.camera == null)
			{
				return;
			}
			Vector3 position = this.camera.WorldToScreenPoint(this.targetUnit.transform.position);
			this.rectTransform.position = position;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00094E54 File Offset: 0x00093054
		private IEnumerator DisplayRoutine(float duration)
		{
			this.canvasGroup.alpha = 0f;
			float elapsed = 0f;
			while (elapsed < this.fadeInDuration)
			{
				elapsed += Time.deltaTime;
				this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / this.fadeInDuration);
				yield return null;
			}
			this.canvasGroup.alpha = 1f;
			yield return new WaitForSeconds(duration - this.fadeInDuration - this.fadeOutDuration);
			elapsed = 0f;
			while (elapsed < this.fadeOutDuration)
			{
				elapsed += Time.deltaTime;
				this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / this.fadeOutDuration);
				yield return null;
			}
			this.canvasGroup.alpha = 0f;
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x04000EA9 RID: 3753
		[SerializeField]
		private Image icon;

		// Token: 0x04000EAA RID: 3754
		[SerializeField]
		private TextMeshProUGUI callsignText;

		// Token: 0x04000EAB RID: 3755
		[SerializeField]
		private TextMeshProUGUI messageText;

		// Token: 0x04000EAC RID: 3756
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04000EAD RID: 3757
		[SerializeField]
		private float fadeInDuration = 0.5f;

		// Token: 0x04000EAE RID: 3758
		[SerializeField]
		private float fadeOutDuration = 0.5f;

		// Token: 0x04000EAF RID: 3759
		private AbstractUnit targetUnit;

		// Token: 0x04000EB0 RID: 3760
		private Camera camera;

		// Token: 0x04000EB1 RID: 3761
		private RectTransform rectTransform;
	}
}
