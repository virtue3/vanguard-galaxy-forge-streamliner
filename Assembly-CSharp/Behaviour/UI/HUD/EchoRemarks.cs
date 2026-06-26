using System;
using System.Collections.Generic;
using Behaviour.GalaxyMap;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.HUD
{
	// Token: 0x0200027D RID: 637
	public class EchoRemarks : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001742 RID: 5954 RVA: 0x0009302C File Offset: 0x0009122C
		private void Awake()
		{
			if (GamePlayer.current.HasStoryteller<Conquest>())
			{
				Debug.Log("Add tips for conquest");
				this.AddTips(this.tips, "@ECHOTravelTipConquest", this.maxTravelTipConquestIndex);
			}
			if (GamePlayer.current.IsInSandBox())
			{
				this.AddTips(this.tips, "@ECHOTravelTipSandbox", this.maxTravelTipSandboxIndex);
			}
			this.AddTips(this.tips, "@ECHOTravelTip", this.maxTravelTipIndex);
			this.container = base.GetComponent<RectTransform>();
			this.canvasGroup = base.GetComponent<CanvasGroup>();
			this.canvasGroup.alpha = 0f;
			this.delayTimer = this.tipsStartTime;
			this.travelTipTimer = 0f;
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x000930E0 File Offset: 0x000912E0
		public void ChangeVisibilityStatus(bool enabled)
		{
			this.pause = !enabled;
			if (!this.canvasGroup)
			{
				return;
			}
			this.canvasGroup.interactable = enabled;
			this.canvasGroup.blocksRaycasts = enabled;
			this.canvasGroup.alpha = (float)(enabled ? 1 : 0);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00093130 File Offset: 0x00091330
		private void OnEnable()
		{
			this.delayTimer = this.tipsStartTime;
			this.travelTipTimer = 0f;
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00093159 File Offset: 0x00091359
		private void OnDisable()
		{
			this.showTip = false;
			this.message.text = "";
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00093172 File Offset: 0x00091372
		public void SetMessage(string text)
		{
			this.message.text = text;
			this.container.sizeDelta = new Vector2(this.message.preferredWidth + 54f, this.container.sizeDelta.y);
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000931B1 File Offset: 0x000913B1
		public void HideMessage()
		{
			this.showTip = false;
			this.travelTipTimer = 0f;
			this.delayTimer = this.tipDelayTime;
			this.message.text = "";
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x000931E4 File Offset: 0x000913E4
		private void Update()
		{
			if (this.pause || AbstractGalaxyMapManager.IsShowing())
			{
				return;
			}
			if (this.travelTipTimer <= 0f && this.delayTimer <= 0f)
			{
				this.showTip = true;
				string text = SeededRandom.Global.Choose<string>(this.tips);
				this.SetMessage(Translation.Translate(text, Array.Empty<object>()));
				this.travelTipTimer = this.tipShowTime;
			}
			if (this.showTip)
			{
				this.travelTipTimer -= Time.deltaTime;
				if (this.canvasGroup.alpha < 1f)
				{
					this.canvasGroup.alpha = Mathf.Clamp(this.canvasGroup.alpha + Time.deltaTime, 0f, 1f);
				}
				if (this.travelTipTimer < 0f)
				{
					this.HideMessage();
				}
			}
			else if (this.canvasGroup.alpha > 0f)
			{
				this.canvasGroup.alpha = Mathf.Clamp(this.canvasGroup.alpha - Time.deltaTime, 0f, 1f);
			}
			if (this.delayTimer > 0f)
			{
				this.delayTimer -= Time.deltaTime;
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00093318 File Offset: 0x00091518
		private void AddTips(List<string> tips, string name, int count)
		{
			for (int i = 1; i <= count; i++)
			{
				tips.Add(name + i.ToString());
			}
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00093344 File Offset: 0x00091544
		public void OnPointerClick(PointerEventData eventData)
		{
			this.HideMessage();
		}

		// Token: 0x04000E58 RID: 3672
		[SerializeField]
		private TMP_Text message;

		// Token: 0x04000E59 RID: 3673
		private RectTransform container;

		// Token: 0x04000E5A RID: 3674
		[SerializeField]
		private float tipShowTime;

		// Token: 0x04000E5B RID: 3675
		[SerializeField]
		private float tipDelayTime;

		// Token: 0x04000E5C RID: 3676
		[SerializeField]
		private float tipsStartTime;

		// Token: 0x04000E5D RID: 3677
		private float travelTipTimer;

		// Token: 0x04000E5E RID: 3678
		private float delayTimer;

		// Token: 0x04000E5F RID: 3679
		private bool showTip;

		// Token: 0x04000E60 RID: 3680
		private int maxTravelTipIndex = 42;

		// Token: 0x04000E61 RID: 3681
		private int maxTravelTipSandboxIndex = 23;

		// Token: 0x04000E62 RID: 3682
		private int maxTravelTipConquestIndex = 1;

		// Token: 0x04000E63 RID: 3683
		private CanvasGroup canvasGroup;

		// Token: 0x04000E64 RID: 3684
		private List<string> tips = new List<string>();

		// Token: 0x04000E65 RID: 3685
		private bool pause;
	}
}
