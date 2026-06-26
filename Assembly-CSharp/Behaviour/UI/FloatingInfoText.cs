using System;
using Behaviour.Transparency;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001E6 RID: 486
	public class FloatingInfoText : MonoBehaviour
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x00079131 File Offset: 0x00077331
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x00079139 File Offset: 0x00077339
		public string prefixText { get; private set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x00079142 File Offset: 0x00077342
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x0007914A File Offset: 0x0007734A
		public string postfixText { get; private set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00079153 File Offset: 0x00077353
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x0007915B File Offset: 0x0007735B
		public string postfix { get; private set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x00079164 File Offset: 0x00077364
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x0007916C File Offset: 0x0007736C
		public bool fading { get; private set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00079175 File Offset: 0x00077375
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x0007917D File Offset: 0x0007737D
		public InfoType type { get; private set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x00079186 File Offset: 0x00077386
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x0007918E File Offset: 0x0007738E
		public GameObject originObject { get; private set; }

		// Token: 0x0600127A RID: 4730 RVA: 0x00079198 File Offset: 0x00077398
		private void Awake()
		{
			this.maxMoveSpeed = new Vector2(UnityEngine.Random.Range(1f, 1.2f) * this.maxMoveSpeed.x, UnityEngine.Random.Range(1f, 1.2f) * this.maxMoveSpeed.y);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000791E6 File Offset: 0x000773E6
		private void Start()
		{
			UnityEngine.Object.Destroy(base.gameObject, this.lifetime);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000791FC File Offset: 0x000773FC
		public void Show(GameObject originObj, float number, InfoType type, Vector2 positionOffset, string postfix = "", Color? damageColor = null)
		{
			this.postfix = postfix;
			this.positionOffset = positionOffset;
			this.type = type;
			Rigidbody2D component = originObj.GetComponent<Rigidbody2D>();
			this.velocity = (component ? component.linearVelocity : Vector2.up);
			this.lastNumber = number;
			this.SetAttributesForType(damageColor);
			this.originObject = originObj;
			this.SetText();
			this.baseFontSize = Mathf.Clamp(this.minFontSize + Mathf.Floor(this.lastNumber / GameMath.DamageMultiplier((float)GamePlayer.current.level) / 40f), this.minFontSize, this.maxFontSize);
			base.transform.position = (Vector2)originObj.transform.position + positionOffset;
			this.textColor = this.numberText.color;
			this.UpdateFontSize();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x000792DC File Offset: 0x000774DC
		protected void SetAttributesForType(Color? damageColor = null)
		{
			switch (this.type)
			{
			case InfoType.DAMAGE:
				this.numberText.color = (damageColor ?? this.defaultColor);
				return;
			case InfoType.CRITICALHIT:
				this.numberText.color = this.criticalHitColor;
				return;
			case InfoType.XP:
				this.prefixText = "+";
				this.numberText.color = this.xpColor;
				this.postfixText = " exp";
				this.numberText.color = (damageColor ?? ColorHelper.detailsColor);
				this.maxMoveSpeed *= 0.25f;
				return;
			case InfoType.PICKUP:
				this.prefixText = "+";
				this.numberText.color = this.xpColor;
				this.postfixText = " " + Translation.Translate(this.postfix, Array.Empty<object>());
				this.maxFontSize = this.minFontSize;
				this.maxMoveSpeed *= 0.25f;
				return;
			case InfoType.INFO:
				this.prefixText = "";
				this.numberText.color = (damageColor ?? ColorHelper.reddish);
				this.postfixText = Translation.Translate(this.postfix, Array.Empty<object>());
				this.maxMoveSpeed *= 0.25f;
				return;
			case InfoType.REFLECTHIT:
				this.numberText.color = ColorHelper.modifierColor;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00079474 File Offset: 0x00077674
		public void AddNumber(float number)
		{
			this.lastNumber += number;
			this.SetText();
			this.scalingDirection = -1;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00079491 File Offset: 0x00077691
		private void SetText()
		{
			this.numberText.text = this.prefixText + ((this.type == InfoType.INFO) ? "" : GameMath.FormatNumber(this.lastNumber, 0)) + this.postfixText;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x000794CB File Offset: 0x000776CB
		private void UpdateFontSize()
		{
			this.numberText.fontSize = this.baseFontSize * GameplayManager.camera.orthographicSize / (float)(ScreenSettings.fullscreen ? 10 : 4);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x000794F8 File Offset: 0x000776F8
		private void Update()
		{
			this.UpdateFontSize();
			if (!this.originObject)
			{
				return;
			}
			this.positionOffset += this.maxMoveSpeed + this.velocity * Time.deltaTime;
			base.transform.position = (Vector2)this.originObject.transform.position + this.positionOffset;
			this.life += Time.deltaTime;
			if (this.scalingDirection == -1 && base.transform.localScale.x < this.maxScale)
			{
				base.transform.localScale += new Vector3(this.scaleSpeed, this.scaleSpeed) * Time.deltaTime;
			}
			else if (base.transform.localScale.x > 1f)
			{
				this.scalingDirection = 1;
				base.transform.localScale -= new Vector3(this.scaleSpeed, this.scaleSpeed) * Time.deltaTime;
			}
			if (this.lifetime - this.life < 0.75f)
			{
				if (!this.fading)
				{
					UIInfoTextParent.instance.DeleteInfoText(this);
				}
				this.fading = true;
				this.textColor.a = this.lifetime - this.life;
				this.numberText.color = this.textColor;
			}
		}

		// Token: 0x04000A45 RID: 2629
		[SerializeField]
		private TextMeshPro numberText;

		// Token: 0x04000A46 RID: 2630
		[SerializeField]
		private Vector2 maxMoveSpeed;

		// Token: 0x04000A47 RID: 2631
		[SerializeField]
		private float lifetime;

		// Token: 0x04000A48 RID: 2632
		[SerializeField]
		private Color defaultColor;

		// Token: 0x04000A49 RID: 2633
		[SerializeField]
		private Color criticalHitColor;

		// Token: 0x04000A4A RID: 2634
		[SerializeField]
		private Color xpColor;

		// Token: 0x04000A4B RID: 2635
		[SerializeField]
		private float minFontSize;

		// Token: 0x04000A4C RID: 2636
		[SerializeField]
		private float maxFontSize;

		// Token: 0x04000A4D RID: 2637
		[SerializeField]
		private float scaleSpeed;

		// Token: 0x04000A4E RID: 2638
		[SerializeField]
		private float maxScale;

		// Token: 0x04000A4F RID: 2639
		[SerializeField]
		private SpriteRenderer prefixIcon;

		// Token: 0x04000A50 RID: 2640
		[SerializeField]
		private SpriteRenderer postfixIcon;

		// Token: 0x04000A57 RID: 2647
		private float lastNumber;

		// Token: 0x04000A58 RID: 2648
		private float life;

		// Token: 0x04000A59 RID: 2649
		public Color textColor;

		// Token: 0x04000A5A RID: 2650
		private Vector2 velocity;

		// Token: 0x04000A5B RID: 2651
		private int scalingDirection = 1;

		// Token: 0x04000A5C RID: 2652
		private float baseFontSize;

		// Token: 0x04000A5D RID: 2653
		private Vector2 positionOffset;
	}
}
