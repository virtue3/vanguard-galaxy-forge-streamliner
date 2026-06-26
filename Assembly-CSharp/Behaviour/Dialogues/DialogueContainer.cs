using System;
using Behaviour.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.Dialogues
{
	// Token: 0x0200039D RID: 925
	public class DialogueContainer : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x0600230F RID: 8975 RVA: 0x000C9525 File Offset: 0x000C7725
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			Singleton<DialogueManager>.Instance.NextOrFinish();
		}
	}
}
