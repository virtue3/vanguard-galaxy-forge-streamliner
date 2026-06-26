using System;
using System.Collections;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001E1 RID: 481
	public class LocationManager : MonoBehaviour
	{
		// Token: 0x0600123F RID: 4671 RVA: 0x000786C9 File Offset: 0x000768C9
		private void Awake()
		{
			LocationManager.instance = this;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x000786D1 File Offset: 0x000768D1
		private void Start()
		{
			this.ShowText(0.3f);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000786DE File Offset: 0x000768DE
		public void ShowText(float waitSeconds)
		{
			if (this.currentRoutine != null)
			{
				base.StopCoroutine(this.currentRoutine);
				this.currentRoutine = null;
			}
			this.SetTextAlpha(0f);
			this.currentRoutine = base.StartCoroutine(this.WaitASecond(waitSeconds));
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00078719 File Offset: 0x00076919
		private IEnumerator WaitASecond(float seconds)
		{
			yield return new WaitForSecondsRealtime(seconds);
			GameplayManager gameplayManager = GameplayManager.Instance;
			yield return base.StartCoroutine(this.AnimateTextInSequence(this.GetSystemName() + " - " + this.GetPOIName(), string.Concat(new string[]
			{
				"Onboard ",
				gameplayManager.spaceShip.shipRoleType.GetTypeName(),
				" \"",
				gameplayManager.spaceShip.spaceShipData.GetShipName(),
				"\""
			})));
			yield break;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0007872F File Offset: 0x0007692F
		private string GetSystemName()
		{
			return Translation.Translate(GamePlayer.current.currentSystem.name, Array.Empty<object>());
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0007874C File Offset: 0x0007694C
		private string GetPOIName()
		{
			MapPointOfInterest currentPointOfInterest = GamePlayer.current.currentPointOfInterest;
			if (currentPointOfInterest == null || currentPointOfInterest.name == "" || currentPointOfInterest.name == null)
			{
				return Translation.Translate("@Unknown", Array.Empty<object>());
			}
			return Translation.Translate(currentPointOfInterest.name, Array.Empty<object>());
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000787A1 File Offset: 0x000769A1
		private IEnumerator AnimateTextInSequence(string sectorText, string shipInfo)
		{
			this.SetTextActive(true);
			yield return new WaitForSecondsRealtime(2f);
			yield return this.RevealText(this.sectorText, sectorText);
			yield return new WaitForSeconds(this.visibleDuration);
			yield return this.FadeOutText();
			yield break;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000787B7 File Offset: 0x000769B7
		private IEnumerator RevealText(TextMeshProUGUI textElement, string text)
		{
			textElement.text = text;
			textElement.ForceMeshUpdate(false, false);
			textElement.maxVisibleCharacters = 0;
			int totalCharacters = textElement.textInfo.characterCount;
			float elapsedTime = 0f;
			while (elapsedTime < this.fillDuration)
			{
				float num = elapsedTime / this.fillDuration;
				int maxVisibleCharacters = Mathf.FloorToInt(num * (float)totalCharacters);
				textElement.maxVisibleCharacters = maxVisibleCharacters;
				textElement.alpha = (this.visible ? Mathf.Lerp(0f, 1f, num) : 0f);
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			textElement.maxVisibleCharacters = totalCharacters;
			textElement.alpha = (float)(this.visible ? 1 : 0);
			yield break;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000787D4 File Offset: 0x000769D4
		private IEnumerator FadeOutText()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				float t = elapsedTime / this.fadeDuration;
				this.sectorText.alpha = (this.visible ? Mathf.Lerp(1f, 0f, t) : 0f);
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.SetTextAlpha(0f);
			this.SetTextActive(false);
			yield break;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000787E3 File Offset: 0x000769E3
		public void SetTextAlpha(float alpha)
		{
			this.sectorText.alpha = alpha;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000787F1 File Offset: 0x000769F1
		private void SetTextActive(bool active)
		{
			this.sectorText.gameObject.SetActive(active);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00078804 File Offset: 0x00076A04
		internal void SetVisibility(bool visible)
		{
			this.visible = visible;
		}

		// Token: 0x04000A1E RID: 2590
		public static LocationManager instance;

		// Token: 0x04000A1F RID: 2591
		[SerializeField]
		private TextMeshProUGUI sectorText;

		// Token: 0x04000A20 RID: 2592
		[SerializeField]
		private TextMeshProUGUI shipInfo;

		// Token: 0x04000A21 RID: 2593
		[SerializeField]
		private float fadeDuration = 1f;

		// Token: 0x04000A22 RID: 2594
		[SerializeField]
		private float visibleDuration = 4f;

		// Token: 0x04000A23 RID: 2595
		[SerializeField]
		private float fillDuration = 2f;

		// Token: 0x04000A24 RID: 2596
		private Coroutine currentRoutine;

		// Token: 0x04000A25 RID: 2597
		private bool visible = true;
	}
}
