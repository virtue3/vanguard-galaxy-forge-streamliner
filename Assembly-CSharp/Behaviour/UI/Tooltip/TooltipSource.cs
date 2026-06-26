using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x02000207 RID: 519
	public class TooltipSource : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x0007E85C File Offset: 0x0007CA5C
		public virtual UITooltip Prefab
		{
			get
			{
				return UITooltipParent.TooltipPrefab;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0007E863 File Offset: 0x0007CA63
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x0007E86B File Offset: 0x0007CA6B
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0007E874 File Offset: 0x0007CA74
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x0007E87C File Offset: 0x0007CA7C
		public string BodyText
		{
			get
			{
				return this._bodyText;
			}
			set
			{
				this._bodyText = value;
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0007E885 File Offset: 0x0007CA85
		public virtual string GetTitle()
		{
			if (!string.IsNullOrEmpty(this._title))
			{
				return this._title;
			}
			ITooltipTitleSource componentInParent = base.GetComponentInParent<ITooltipTitleSource>();
			if (componentInParent == null)
			{
				return null;
			}
			return componentInParent.GetTooltipTitle();
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0007E8AC File Offset: 0x0007CAAC
		public virtual string GetBodyText()
		{
			if (!string.IsNullOrEmpty(this._bodyText))
			{
				return this._bodyText;
			}
			ITooltipTextSource componentInParent = base.GetComponentInParent<ITooltipTextSource>();
			if (componentInParent == null)
			{
				return null;
			}
			return componentInParent.GetTooltipText();
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0007E8D4 File Offset: 0x0007CAD4
		public virtual void AddCustomContent(UITooltip tooltip)
		{
			ITooltipCustomSource tooltipCustomSource;
			if (base.TryGetComponent<ITooltipCustomSource>(out tooltipCustomSource))
			{
				tooltipCustomSource.AddTooltipCustomContent(tooltip);
			}
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0007E8F2 File Offset: 0x0007CAF2
		private void OnDisable()
		{
			this.CancelCoroutine();
			UITooltip.Hide(this);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0007E900 File Offset: 0x0007CB00
		private void OnDestroy()
		{
			this.CancelCoroutine();
			UITooltip.Hide(this);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0007E90E File Offset: 0x0007CB0E
		private void OnMouseEnter()
		{
			if (base.isActiveAndEnabled && !UIHelper.IsMouseOverUi)
			{
				this.showTooltipCoroutine = base.StartCoroutine(this.ShowTooltipAfterDelay(this.delay));
			}
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x0007E937 File Offset: 0x0007CB37
		private IEnumerator ShowTooltipAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			UITooltip.Show(this, this.Prefab);
			yield break;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x0007E94D File Offset: 0x0007CB4D
		private void OnMouseExit()
		{
			this.CancelCoroutine();
			UITooltip.Hide(this);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0007E95B File Offset: 0x0007CB5B
		private void CancelCoroutine()
		{
			if (this.showTooltipCoroutine != null)
			{
				base.StopCoroutine(this.showTooltipCoroutine);
				this.showTooltipCoroutine = null;
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0007E978 File Offset: 0x0007CB78
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.showTooltipCoroutine = base.StartCoroutine(this.ShowTooltipAfterDelay(this.delay));
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0007E992 File Offset: 0x0007CB92
		public void OnPointerExit(PointerEventData eventData)
		{
			this.CancelCoroutine();
			UITooltip.Hide(this);
		}

		// Token: 0x04000B17 RID: 2839
		[SerializeField]
		private string _title;

		// Token: 0x04000B18 RID: 2840
		[SerializeField]
		private string _bodyText;

		// Token: 0x04000B19 RID: 2841
		[SerializeField]
		private float delay;

		// Token: 0x04000B1A RID: 2842
		private Coroutine showTooltipCoroutine;
	}
}
