using System;
using System.Collections;
using Behaviour.Ability;
using Behaviour.UI.Tooltip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x0200027B RID: 635
	public class BuffIcon : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001732 RID: 5938 RVA: 0x000929A6 File Offset: 0x00090BA6
		private void Awake()
		{
			this.tooltipSource = base.GetComponent<TooltipSource>();
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000929B4 File Offset: 0x00090BB4
		public void SetEffect(TemporaryEffect effect)
		{
			this.effect = effect;
			Sprite icon = effect.icon;
			if (!icon && !string.IsNullOrEmpty(effect.abilityIdentifier))
			{
				foreach (ActivatedAbility activatedAbility in GameplayManager.Instance.spaceShip.GetComponentsInChildren<ActivatedAbility>())
				{
					if (activatedAbility.identifier == effect.abilityIdentifier)
					{
						icon = activatedAbility.icon;
						break;
					}
				}
			}
			if (icon)
			{
				this.iconImage.sprite = icon;
			}
			this.previousStackSize = effect.stackSize;
			this.stackLabel.gameObject.SetActive(effect.stackSize > 1);
			this.stackLabel.text = effect.stackSize.ToString();
			Color color = effect.isDebuff ? ColorHelper.debuffBorder : ColorHelper.buffBorder;
			this.borderImage.color = color;
			this.glowImage.color = new Color(color.r, color.g, color.b, 0f);
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00092AC0 File Offset: 0x00090CC0
		public void SetAbility(ActivatedAbility ability)
		{
			this.abilitySource = ability;
			if (ability.icon)
			{
				this.iconImage.sprite = ability.icon;
			}
			this.stackLabel.gameObject.SetActive(false);
			this.durationBar.gameObject.SetActive(false);
			Color color = ability.toggledAbility ? ColorHelper.orange75 : ColorHelper.buffBorder;
			this.borderImage.color = color;
			this.glowImage.color = new Color(color.r, color.g, color.b, 0f);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00092B5C File Offset: 0x00090D5C
		private void Update()
		{
			if (this.abilitySource)
			{
				if (!this.abilitySource.IsPayloadActive() && !this.abilitySource.toggleActive)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				return;
			}
			if (!this.effect)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.durationBar.fillAmount = Mathf.Clamp01(this.effect.durationRemaining / this.effect.duration);
			this.tooltipRefreshTimer -= Time.deltaTime;
			if (this.tooltipRefreshTimer <= 0f)
			{
				this.tooltipRefreshTimer = 0.5f;
				UITooltip.Refresh(this.tooltipSource);
			}
			int stackSize = this.effect.stackSize;
			if (stackSize != this.previousStackSize)
			{
				this.previousStackSize = stackSize;
				this.stackLabel.text = stackSize.ToString();
				this.stackLabel.gameObject.SetActive(stackSize > 1);
				this.PlayStackAnimation();
				UITooltip.Refresh(this.tooltipSource);
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00092C64 File Offset: 0x00090E64
		private void PlayStackAnimation()
		{
			if (this.popCoroutine != null)
			{
				base.StopCoroutine(this.popCoroutine);
			}
			if (this.glowCoroutine != null)
			{
				base.StopCoroutine(this.glowCoroutine);
			}
			this.popCoroutine = base.StartCoroutine(this.PopLabel());
			this.glowCoroutine = base.StartCoroutine(this.GlowPulse());
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00092CBD File Offset: 0x00090EBD
		private IEnumerator PopLabel()
		{
			float punchDuration = 0.1f;
			float settleDuration = 0.2f;
			float peakScale = 1.6f;
			this.stackLabel.transform.localScale = Vector3.one;
			float t = 0f;
			while (t < punchDuration)
			{
				t += Time.deltaTime;
				float d = Mathf.Lerp(1f, peakScale, t / punchDuration);
				this.stackLabel.transform.localScale = Vector3.one * d;
				yield return null;
			}
			t = 0f;
			while (t < settleDuration)
			{
				t += Time.deltaTime;
				float d2 = Mathf.Lerp(peakScale, 1f, t / settleDuration);
				this.stackLabel.transform.localScale = Vector3.one * d2;
				yield return null;
			}
			this.stackLabel.transform.localScale = Vector3.one;
			this.popCoroutine = null;
			yield break;
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00092CCC File Offset: 0x00090ECC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.abilitySource != null)
			{
				tooltip.AddTextLine(this.abilitySource.displayName, 14, 8f).Text.color = ColorHelper.buffBorder;
				if (!string.IsNullOrEmpty(this.abilitySource.descriptionText))
				{
					tooltip.AddSeparator(null);
					tooltip.AddTextLine(this.abilitySource.descriptionText, 12, 8f);
				}
				return;
			}
			Color color = this.effect.isDebuff ? ColorHelper.debuffBorder : ColorHelper.buffBorder;
			tooltip.AddTextLine(this.effect.displayName, 14, 8f).Text.color = color;
			if (this.effect.stackSize > 1)
			{
				tooltip.AddTextLine(string.Format("x{0} stacks", this.effect.stackSize), 12, 8f).Text.color = ColorHelper.greenish;
			}
			tooltip.AddTextLine(GameMath.FormatTime((int)this.effect.durationRemaining, false), 12, 8f).Text.color = ColorHelper.fadedGrey;
			if (!string.IsNullOrEmpty(this.effect.descriptionText))
			{
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(this.effect.descriptionText, 12, 8f);
			}
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00092E34 File Offset: 0x00091034
		private IEnumerator GlowPulse()
		{
			float fadeInDuration = 0.08f;
			float fadeOutDuration = 0.4f;
			float peakAlpha = 0.65f;
			Color c = this.glowImage.color;
			float t = 0f;
			while (t < fadeInDuration)
			{
				t += Time.deltaTime;
				c.a = Mathf.Lerp(0f, peakAlpha, t / fadeInDuration);
				this.glowImage.color = c;
				yield return null;
			}
			t = 0f;
			while (t < fadeOutDuration)
			{
				t += Time.deltaTime;
				c.a = Mathf.Lerp(peakAlpha, 0f, t / fadeOutDuration);
				this.glowImage.color = c;
				yield return null;
			}
			c.a = 0f;
			this.glowImage.color = c;
			this.glowCoroutine = null;
			yield break;
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00092E43 File Offset: 0x00091043
		private void OnDestroy()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x04000E48 RID: 3656
		[SerializeField]
		private Image iconImage;

		// Token: 0x04000E49 RID: 3657
		[SerializeField]
		private Image borderImage;

		// Token: 0x04000E4A RID: 3658
		[SerializeField]
		private Image glowImage;

		// Token: 0x04000E4B RID: 3659
		[SerializeField]
		private Image durationBar;

		// Token: 0x04000E4C RID: 3660
		[SerializeField]
		private TextMeshProUGUI stackLabel;

		// Token: 0x04000E4D RID: 3661
		private TemporaryEffect effect;

		// Token: 0x04000E4E RID: 3662
		private ActivatedAbility abilitySource;

		// Token: 0x04000E4F RID: 3663
		private int previousStackSize;

		// Token: 0x04000E50 RID: 3664
		private Coroutine popCoroutine;

		// Token: 0x04000E51 RID: 3665
		private Coroutine glowCoroutine;

		// Token: 0x04000E52 RID: 3666
		private TooltipSource tooltipSource;

		// Token: 0x04000E53 RID: 3667
		private float tooltipRefreshTimer;

		// Token: 0x04000E54 RID: 3668
		private const float TooltipRefreshInterval = 0.5f;
	}
}
