using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x02000299 RID: 665
	public abstract class BaseMenuButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x0009A74C File Offset: 0x0009894C
		public bool isDeactivated
		{
			get
			{
				return this.deactivated;
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0009A754 File Offset: 0x00098954
		protected virtual void Awake()
		{
			this.CacheUIComponents();
			this.CacheOriginalColors();
			if (this.notificationMaterial)
			{
				this.originalHighlightedButtonColor = this.notificationImage.color;
				this.originalMaterial = this.notificationImage.material;
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0009A791 File Offset: 0x00098991
		protected virtual void Start()
		{
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0009A793 File Offset: 0x00098993
		private void Update()
		{
			if (this.hoverTime > 0f)
			{
				this.hoverTime -= Time.deltaTime;
			}
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0009A7B4 File Offset: 0x000989B4
		protected void CacheUIComponents()
		{
			this.buttonText = base.GetComponentInChildren<TextMeshProUGUI>();
			this.buttonImage = base.GetComponent<Image>();
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0009A7CE File Offset: 0x000989CE
		private void CacheOriginalColors()
		{
			this.originalButtonColor = this.buttonImage.color;
			this.originalTextColor = this.buttonText.color;
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0009A7F2 File Offset: 0x000989F2
		public void SetButtonColor()
		{
			this.UpdateButtonAppearance(new Color(0.6f, 0.6f, 0.6f, 0.2f), new Color?(new Color(1f, 1f, 1f, 0.15f)));
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0009A831 File Offset: 0x00098A31
		protected void SetButtonText(string text)
		{
			this.buttonText.text = text;
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0009A840 File Offset: 0x00098A40
		public void SelectButton()
		{
			this.isSelected = true;
			this.UpdateButtonAppearance(new Color(0.233001f, 0.3733063f, 0.6415094f), new Color?(new Color(0.9056604f, 0.9056604f, 0.9056604f)));
			this.Highlight(true);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0009A88E File Offset: 0x00098A8E
		public void DeselectButton()
		{
			this.isSelected = false;
			this.ResetButtonAppearance();
			this.Highlight(false);
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0009A8A4 File Offset: 0x00098AA4
		protected void HoverButton()
		{
			this.UpdateButtonAppearance(new Color(0.2715379f, 0.3808415f, 0.509434f), null);
			this.Highlight(true);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0009A8DC File Offset: 0x00098ADC
		protected void ButtonDown()
		{
			this.isDown = true;
			this.UpdateButtonAppearance(new Color(0.3175952f, 0.3407195f, 0.3679245f), null);
			this.Highlight(true);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0009A91A File Offset: 0x00098B1A
		protected void ResetButtonAppearance()
		{
			this.UpdateButtonAppearance(this.originalButtonColor, new Color?(this.originalTextColor));
			this.Highlight(false);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0009A93A File Offset: 0x00098B3A
		protected void UpdateButtonAppearance(Color buttonColor, Color? textColor = null)
		{
			this.buttonImage.color = buttonColor;
			if (textColor != null)
			{
				this.buttonText.color = textColor.Value;
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0009A963 File Offset: 0x00098B63
		public void Highlight(bool isActive)
		{
			if (!this.highlightedImage)
			{
				return;
			}
			this.highlightedImage.gameObject.SetActive(isActive);
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0009A984 File Offset: 0x00098B84
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Deactivated())
			{
				return;
			}
			if (!this.isSelected && !SidePanel.instance.IsTabMoving())
			{
				if (this.isDown)
				{
					this.ButtonDown();
				}
				else
				{
					this.HoverButton();
				}
			}
			this.hoverTime = 0.5f;
			this.isHovering = true;
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0009A9D6 File Offset: 0x00098BD6
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Deactivated())
			{
				return;
			}
			if (!this.isSelected && !SidePanel.instance.IsTabMoving())
			{
				this.ResetButtonAppearance();
			}
			if (this.hoverTime <= 0f)
			{
				this.HideNotification();
			}
			this.isHovering = false;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0009AA15 File Offset: 0x00098C15
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Deactivated())
			{
				return;
			}
			if (!this.isSelected && !SidePanel.instance.IsTabMoving())
			{
				this.ButtonDown();
			}
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0009AA3A File Offset: 0x00098C3A
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.Deactivated())
			{
				return;
			}
			if (this.isHovering && !SidePanel.instance.IsTabMoving())
			{
				this.OnClick();
			}
			this.isDown = false;
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0009AA66 File Offset: 0x00098C66
		public bool Deactivated()
		{
			return this.deactivated;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0009AA6E File Offset: 0x00098C6E
		public void SetDeactivated(bool value)
		{
			this.deactivated = value;
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0009AA77 File Offset: 0x00098C77
		public void ShowNotification()
		{
			if (this.showingNotification)
			{
				return;
			}
			this.showingNotification = true;
			this.notificationImage.color = Color.white;
			this.notificationImage.material = this.notificationMaterial;
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0009AAAA File Offset: 0x00098CAA
		public void HideNotification()
		{
			if (!this.showingNotification)
			{
				return;
			}
			this.showingNotification = false;
			this.notificationImage.color = this.originalHighlightedButtonColor;
			this.notificationImage.material = this.originalMaterial;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0009AADE File Offset: 0x00098CDE
		protected virtual void OnClick()
		{
			this.HideNotification();
		}

		// Token: 0x04000F4C RID: 3916
		[SerializeField]
		private bool deactivated;

		// Token: 0x04000F4D RID: 3917
		protected TextMeshProUGUI buttonText;

		// Token: 0x04000F4E RID: 3918
		protected Image buttonImage;

		// Token: 0x04000F4F RID: 3919
		[SerializeField]
		protected Image highlightedImage;

		// Token: 0x04000F50 RID: 3920
		[SerializeField]
		protected Image notificationImage;

		// Token: 0x04000F51 RID: 3921
		[SerializeField]
		protected Material notificationMaterial;

		// Token: 0x04000F52 RID: 3922
		protected float hoverTime;

		// Token: 0x04000F53 RID: 3923
		protected bool isSelected;

		// Token: 0x04000F54 RID: 3924
		protected bool isHovering;

		// Token: 0x04000F55 RID: 3925
		protected bool isDown;

		// Token: 0x04000F56 RID: 3926
		protected Color originalButtonColor;

		// Token: 0x04000F57 RID: 3927
		protected Color originalTextColor;

		// Token: 0x04000F58 RID: 3928
		protected bool showingNotification;

		// Token: 0x04000F59 RID: 3929
		protected Material originalMaterial;

		// Token: 0x04000F5A RID: 3930
		protected Color originalHighlightedButtonColor;
	}
}
