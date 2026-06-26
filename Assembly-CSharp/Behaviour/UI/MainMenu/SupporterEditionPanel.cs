using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Behaviour.UI.MainMenu
{
	// Token: 0x0200026E RID: 622
	public class SupporterEditionPanel : MonoBehaviour
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x000912D8 File Offset: 0x0008F4D8
		private void Start()
		{
			if (SteamManager.Initialized)
			{
				this.callsignInput.text = SteamFriends.GetPersonaName();
			}
			this.PopulateFactionDropdown();
			this.submitButton.onClick.AddListener(new UnityAction(this.OnSubmit));
			this.UpdateSentState();
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00091324 File Offset: 0x0008F524
		private void UpdateSentState()
		{
			bool flag = GameplayerPrefs.SentSEName();
			string text = flag ? "@MMSupporterEditionCallsignSent" : "@MMSupporterEditionCallsign";
			this.thankYouText.text = Translation.Translate("@MMSupporterEditionThankYou", Array.Empty<object>()) + "\n\n" + Translation.Translate(text, Array.Empty<object>());
			this.callsignInput.gameObject.SetActive(!flag);
			this.factionDropdown.gameObject.SetActive(!flag);
			this.submitButton.gameObject.SetActive(!flag);
			if (flag)
			{
				RectTransform rectTransform = this.thankYouText.rectTransform;
				RectTransform rectTransform2 = rectTransform.parent as RectTransform;
				float y = -(((rectTransform2 != null) ? rectTransform2.rect.height : 0f) / 2f - rectTransform.rect.height / 2f);
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00091424 File Offset: 0x0008F624
		private void PopulateFactionDropdown()
		{
			this.factionDropdown.ClearOptions();
			this.factionIdentifiers.Clear();
			List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
			this.factionIdentifiers.Add("Any");
			list.Add(new TMP_Dropdown.OptionData(Translation.Translate("@MMSupporterEditionAnyFaction", Array.Empty<object>())));
			foreach (Faction faction in Faction.all)
			{
				if (!SupporterEditionPanel.excludedFactions.Contains(faction.identifier))
				{
					this.factionIdentifiers.Add(faction.identifier);
					list.Add(new TMP_Dropdown.OptionData(Translation.Translate(faction.name, Array.Empty<object>())));
				}
			}
			this.factionDropdown.AddOptions(list);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x000914FC File Offset: 0x0008F6FC
		private void OnSubmit()
		{
			base.StartCoroutine(this.PostSupporterDataRoutine(this.callsignInput.text, this.factionIdentifiers[this.factionDropdown.value]));
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x0009152C File Offset: 0x0008F72C
		private IEnumerator PostSupporterDataRoutine(string name, string faction)
		{
			if (!SteamManager.Initialized)
			{
				yield break;
			}
			this.submitButton.interactable = false;
			CSteamID steamID = SteamUser.GetSteamID();
			string s = string.Format("{0}:{1}", steamID, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			string value = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("steamId", steamID.ToString());
			wwwform.AddField("token", value);
			wwwform.AddField("displayName", name);
			wwwform.AddField("faction", faction);
			UnityWebRequest request = UnityWebRequest.Post(this.postUrl, wwwform);
			yield return request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(request.error);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@MMSupporterEditionError", Array.Empty<object>())).WithColor(ColorHelper.orange75).Show();
				this.submitButton.interactable = true;
			}
			else
			{
				GameplayerPrefs.SetSentSEName(true);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@MMSupporterEditionSuccess", Array.Empty<object>())).WithColor(ColorHelper.green50).Show();
				this.UpdateSentState();
			}
			yield break;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00091549 File Offset: 0x0008F749
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E17 RID: 3607
		private readonly string postUrl = "https://script.google.com/macros/s/AKfycbx6nFLT_XF4PoPT9eL49FXmp4N4Xwh5UEgtGZZt8YV46_W68a3V2hBMx_8dXpMuwnn-/exec";

		// Token: 0x04000E18 RID: 3608
		[SerializeField]
		private TMP_Text thankYouText;

		// Token: 0x04000E19 RID: 3609
		[SerializeField]
		private TMP_InputField callsignInput;

		// Token: 0x04000E1A RID: 3610
		[SerializeField]
		private TMP_Dropdown factionDropdown;

		// Token: 0x04000E1B RID: 3611
		[SerializeField]
		private Button submitButton;

		// Token: 0x04000E1C RID: 3612
		private static readonly HashSet<string> excludedFactions = new HashSet<string>
		{
			"Player",
			"Fanatics",
			"Amalgam",
			"HolyRadicals",
			"Puppeteers",
			"Smugglers"
		};

		// Token: 0x04000E1D RID: 3613
		private readonly List<string> factionIdentifiers = new List<string>();
	}
}
