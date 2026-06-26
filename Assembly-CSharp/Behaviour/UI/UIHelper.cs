using System;
using System.Collections.Generic;
using Behaviour.Ability;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI
{
	// Token: 0x020001FB RID: 507
	public class UIHelper
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x0007D668 File Offset: 0x0007B868
		public static bool IsMouseOverUi
		{
			get
			{
				EventSystem current = EventSystem.current;
				return current != null && current.IsPointerOverGameObject();
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x0007D67A File Offset: 0x0007B87A
		public static bool clickTargetingAvailable
		{
			get
			{
				return !UIHelper.IsMouseOverUi && !ActivatedAbility.targetingActive;
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0007D690 File Offset: 0x0007B890
		public static GameObject GetMouseOverGameObject(Camera cam = null)
		{
			if (cam == null)
			{
				cam = Camera.main;
			}
			Vector3 v = cam.ScreenToWorldPoint(GlobalControls.mousePosition);
			v.z = cam.transform.position.z;
			Collider2D collider = Physics2D.Raycast(v, Vector2.zero).collider;
			if (collider == null)
			{
				return null;
			}
			return collider.gameObject;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0007D6F4 File Offset: 0x0007B8F4
		public static List<RaycastResult> GetMouseOverUIElements()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = GlobalControls.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			return list;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0007D72A File Offset: 0x0007B92A
		public static string HighlightText(string text)
		{
			return "<color=#FFD100>" + text + "</color>";
		}
	}
}
