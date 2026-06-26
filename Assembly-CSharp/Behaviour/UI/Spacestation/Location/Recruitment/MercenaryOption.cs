using System;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Crew;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location.Recruitment
{
	// Token: 0x0200022E RID: 558
	public class MercenaryOption : MonoBehaviour
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00087AFC File Offset: 0x00085CFC
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x00087B04 File Offset: 0x00085D04
		public Mercenary mercenary { get; private set; }

		// Token: 0x060014FA RID: 5370 RVA: 0x00087B0D File Offset: 0x00085D0D
		public void Update()
		{
			if (this.current && this.mercenary != null)
			{
				this.timeLeftText.text = GameMath.FormatTime((int)this.mercenary.timeLeft, true);
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00087B3C File Offset: 0x00085D3C
		public void SetMercenary(Mercenary mercenary, Action refreshCallback, bool current = false)
		{
			this.current = current;
			this.mercenary = mercenary;
			this.refreshCallback = refreshCallback;
			this.portrait.SetMercenary(mercenary);
			this.ship.SetShip(mercenary.ship, SpaceShip.Get(mercenary.ship).GetComponent<SpriteRenderer>().sprite);
			this.border.color = mercenary.rarity.GetColor();
			this.hireButton.onClick.RemoveAllListeners();
			if (current)
			{
				this.costs.text = Translation.Translate("@SSHired", Array.Empty<object>());
				this.hireText.text = Translation.Translate("@SSFire", Array.Empty<object>());
				this.hireButton.onClick.AddListener(new UnityAction(this.OnFire));
				this.extendButton.GetComponent<TooltipSource>().BodyText = Translation.Translate("@SSMercAddTime", new object[]
				{
					1,
					mercenary.GetFullName()
				});
			}
			else
			{
				this.costs.text = "$ " + GameMath.FormatNumber((float)mercenary.creditCost, -1) + " / h";
				this.hireText.text = Translation.Translate("@SSHire", Array.Empty<object>());
				this.hireButton.onClick.AddListener(new UnityAction(this.OnHire));
			}
			this.extendButton.gameObject.SetActive(current);
			this.timeLeftText.gameObject.SetActive(current);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00087CBF File Offset: 0x00085EBF
		public void OnFire()
		{
			AlertPopup.ShowQuery(Translation.Translate("@SSSureFire", new object[]
			{
				this.mercenary.GetFullName()
			}), null, null, new Action(this.ExecuteFire), null, null, null);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00087CF5 File Offset: 0x00085EF5
		public void ExecuteFire()
		{
			GamePlayer.current.RemoveHiredMercenary(true);
			this.refreshCallback();
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00087D10 File Offset: 0x00085F10
		public void OnHire()
		{
			if (!GamePlayer.current.CanAfford((float)this.mercenary.creditCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (GamePlayer.current.hiredMercenary != null)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSMercAlreadyHired", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			GamePlayer.current.RemoveCredits((float)this.mercenary.creditCost);
			Vector2 vector = GameplayManager.Instance.spaceShip.transform.position;
			vector += new Vector2((float)SeededRandom.Global.RandomRange(5, 10), (float)SeededRandom.Global.RandomRange(5, 8));
			this.mercenary.timeLeft = 3600f;
			GameplayManager.Instance.CreateMercenary(vector, this.mercenary);
			this.refreshCallback();
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00087E18 File Offset: 0x00086018
		public void OnExtendHire()
		{
			if (!GamePlayer.current.CanAfford((float)this.mercenary.creditCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			GamePlayer.current.RemoveCredits((float)this.mercenary.creditCost);
			this.mercenary.AddMercTime(1);
		}

		// Token: 0x04000C4B RID: 3147
		[SerializeField]
		private MercenaryPortrait portrait;

		// Token: 0x04000C4C RID: 3148
		[SerializeField]
		private MercenaryShip ship;

		// Token: 0x04000C4D RID: 3149
		[SerializeField]
		private TMP_Text costs;

		// Token: 0x04000C4E RID: 3150
		[SerializeField]
		private TMP_Text hireText;

		// Token: 0x04000C4F RID: 3151
		[SerializeField]
		private TMP_Text extendText;

		// Token: 0x04000C50 RID: 3152
		[SerializeField]
		private TMP_Text timeLeftText;

		// Token: 0x04000C51 RID: 3153
		[SerializeField]
		private Button hireButton;

		// Token: 0x04000C52 RID: 3154
		[SerializeField]
		private Button extendButton;

		// Token: 0x04000C53 RID: 3155
		[SerializeField]
		private Image border;

		// Token: 0x04000C55 RID: 3157
		public bool current;

		// Token: 0x04000C56 RID: 3158
		private Action refreshCallback;
	}
}
