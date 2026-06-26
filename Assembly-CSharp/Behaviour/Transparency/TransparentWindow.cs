using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Behaviour.Bootstrap;
using Behaviour.GalaxyMap;
using Behaviour.UI;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.Transparency
{
	// Token: 0x020002D4 RID: 724
	public class TransparentWindow : AbstractWindow
	{
		// Token: 0x06001A64 RID: 6756
		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		// Token: 0x06001A65 RID: 6757
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		// Token: 0x06001A66 RID: 6758
		[DllImport("user32.dll")]
		private static extern bool BringWindowToTop(IntPtr window);

		// Token: 0x06001A67 RID: 6759
		[DllImport("user32.dll")]
		private static extern IntPtr SetActiveWindow(IntPtr window);

		// Token: 0x06001A68 RID: 6760
		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr window, uint dwFlags);

		// Token: 0x06001A69 RID: 6761
		[DllImport("user32.dll")]
		private static extern int SetLayeredWindowAttributes(IntPtr window, uint crKey, byte bAlpha, uint dwFlags);

		// Token: 0x06001A6A RID: 6762
		[DllImport("user32.dll")]
		public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

		// Token: 0x06001A6B RID: 6763
		[DllImport("user32.dll")]
		private static extern int SetWindowLongPtrA(IntPtr window, int nIndex, uint dwNewLong);

		// Token: 0x06001A6C RID: 6764
		[DllImport("user32.dll")]
		private static extern int RedrawWindow(IntPtr window, IntPtr? lprcUpdate, IntPtr? hrgnUpdate, uint flags);

		// Token: 0x06001A6D RID: 6765
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool SetWindowPos(IntPtr window, IntPtr windowInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		// Token: 0x06001A6E RID: 6766
		[DllImport("Dwmapi.dll")]
		private static extern uint DwmExtendFrameIntoClientArea(IntPtr window, ref TransparentWindow.MARGINS margins);

		// Token: 0x06001A6F RID: 6767
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06001A70 RID: 6768
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowRect(IntPtr hwnd, out TransparentWindow.RECT lpTaskbarRect);

		// Token: 0x06001A71 RID: 6769 RVA: 0x000A3CB4 File Offset: 0x000A1EB4
		protected override void Awake()
		{
			Debug.Log(base.name + " -- Awake");
			base.Awake();
			if (GameplayerPrefs.GetAutoScale() > ScreenSettings.maxScale)
			{
				GameplayerPrefs.SetAutoScale(ScreenSettings.maxScale);
			}
			ScreenSettings.SetScreenPercentage();
			this.pointerData = new PointerEventData(EventSystem.current);
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000A3D07 File Offset: 0x000A1F07
		protected void Start()
		{
			Debug.Log(base.name + " -- Start");
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x000A3D20 File Offset: 0x000A1F20
		protected void Update()
		{
			if (!this.windowEnabled)
			{
				return;
			}
			if (!ScreenSettings.allowScaleUpdate || SteamCommunication.overlayActivated)
			{
				return;
			}
			this.monitorForGame = TransparentWindow.MonitorFromWindow(this.WindowHandle, 2U);
			this.monitorForTaskbar = TransparentWindow.MonitorFromWindow(this.TaskBarHandle, 2U);
			this.weDealWithTaskbar = (this.monitorForGame == this.monitorForTaskbar);
			TransparentWindow.GetWindowRect(this.TaskBarHandle, out this.TaskbarRect);
			TransparentWindow.GetWindowRect(this.WindowHandle, out this.WindowRect);
			float num = (this.WindowRect.Bottom <= 0) ? MathF.Abs((float)this.WindowRect.Top) : ((float)this.WindowRect.Bottom);
			float num2 = num * ScreenSettings.clickableScreenPercentage + ScreenSettings.minimumY;
			if (this.monitorForGame == this.monitorForTaskbar)
			{
				TransparentWindow.RECT taskbarRect = this.TaskbarRect;
			}
			float y = Input.mousePosition.y;
			this.MouseOverUICheck();
			bool flag = y > num2 || y < ScreenSettings.minimumY;
			base.isClickThrough = (flag && !UIHelper.IsMouseOverUi && !this.isMouseOverUI && !this.isMouseOverCollider);
			this.SetClickThrough();
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x000A3E48 File Offset: 0x000A2048
		private void MouseOverUICheck()
		{
			if (EventSystem.current == null)
			{
				this.isMouseOverUI = false;
				return;
			}
			this.pointerData.position = Input.mousePosition;
			this.results.Clear();
			EventSystem.current.RaycastAll(this.pointerData, this.results);
			this.isMouseOverUI = (this.results.Count > 0);
			this.isMouseOverCollider = false;
			if (AbstractGalaxyMapManager.current)
			{
				Ray ray = AbstractGalaxyMapManager.current.mapCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
				this.isMouseOverCollider = hit;
			}
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x000A3F06 File Offset: 0x000A2106
		private void SetClickThrough()
		{
			if (base.isClickThrough)
			{
				TransparentWindow.SetWindowLongPtrA(this.WindowHandle, -20, 524320U);
				return;
			}
			TransparentWindow.SetWindowLongPtrA(this.WindowHandle, -20, 524288U);
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x000A3F38 File Offset: 0x000A2138
		private void OnApplicationFocus(bool hasFocus)
		{
			if (GameplayerPrefs.GetAlwaysOnTop() && !hasFocus)
			{
				TransparentWindow.SetWindowPos(this.WindowHandle, TransparentWindow.WINDOW_TOPMOST, 0, 0, 0, 0, 3U);
				return;
			}
			if (!GameplayerPrefs.GetAlwaysOnTop() && !hasFocus)
			{
				TransparentWindow.SetWindowPos(this.WindowHandle, TransparentWindow.WINDOW_NOTTOPMOST, 0, 0, 0, 0, 3U);
			}
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x000A3F86 File Offset: 0x000A2186
		private uint GetTransparencyFlag(bool enabled)
		{
			if (!enabled)
			{
				return 2U;
			}
			return 1U;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000A3F8E File Offset: 0x000A218E
		protected override void OnEnable()
		{
			base.OnEnable();
			Debug.Log(base.name + " -- OnEnable");
			this.TaskBarHandle = TransparentWindow.FindWindow("Shell_TrayWnd", "");
			this.SwitchTransparencyMode();
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000A3FC8 File Offset: 0x000A21C8
		public void SwitchTransparencyMode()
		{
			Application.runInBackground = true;
			this.WindowHandle = TransparentWindow.GetActiveWindow();
			this.SetDefaultColorMode();
			TransparentWindow.GetWindowRect(this.TaskBarHandle, out this.TaskbarRect);
			TransparentWindow.GetWindowRect(this.WindowHandle, out this.WindowRect);
			base.StartCoroutine(this.EnableCoroutine());
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000A4020 File Offset: 0x000A2220
		public void SetDefaultColorMode()
		{
			this.defaultColorKey = ((GameplayerPrefs.GetTransparencyMode() == TransparencyMode.Transparent.ToString()) ? 1U : 2U);
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x000A4052 File Offset: 0x000A2252
		public override void ToggleSteamOverlay(bool enabled)
		{
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000A4054 File Offset: 0x000A2254
		public override float GetMinimumY()
		{
			return (float)(this.weDealWithTaskbar ? Mathf.Clamp(this.TaskbarRect.Bottom - this.TaskbarRect.Top, 0, 72) : 0);
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000A4084 File Offset: 0x000A2284
		private void SetTransparentWindow(uint colorKeyOrAlpha)
		{
			Color backgroundColor = (colorKeyOrAlpha == 1U) ? ColorHelper.filthyGreenOnlyUsedForCameraBackground : Color.black;
			backgroundColor.a = 0f;
			this.fullScreenCamera.backgroundColor = backgroundColor;
			this.SetTransparencyToColorKeyOrAlpha(colorKeyOrAlpha);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000A40C8 File Offset: 0x000A22C8
		private void SetTransparencyToColorKeyOrAlpha(uint colorKeyOrAlpha)
		{
			Debug.Log("Setting transparencyMode with mode: " + colorKeyOrAlpha.ToString());
			TransparentWindow.MARGINS margins = new TransparentWindow.MARGINS
			{
				cxLeftWidth = -1
			};
			TransparentWindow.DwmExtendFrameIntoClientArea(this.WindowHandle, ref margins);
			TransparentWindow.SetWindowLongPtrA(this.WindowHandle, -20, 524320U);
			byte bAlpha = (colorKeyOrAlpha == 2U) ? byte.MaxValue : (byte)0;
			TransparentWindow.SetLayeredWindowAttributes(this.WindowHandle, 593157U, bAlpha, colorKeyOrAlpha);
			TransparentWindow.SetWindowPos(this.WindowHandle, GameplayerPrefs.GetAlwaysOnTop() ? TransparentWindow.WINDOW_TOPMOST : TransparentWindow.WINDOW_TOP, 0, 0, 0, 0, 3U);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000A4164 File Offset: 0x000A2364
		public void RemoveTransparency()
		{
			uint windowLong = TransparentWindow.GetWindowLong(this.WindowHandle, -20);
			TransparentWindow.MARGINS margins = default(TransparentWindow.MARGINS);
			TransparentWindow.DwmExtendFrameIntoClientArea(this.WindowHandle, ref margins);
			TransparentWindow.SetWindowLongPtrA(this.WindowHandle, -20, windowLong & 4294443007U & 4294967263U);
			TransparentWindow.SetWindowPos(this.WindowHandle, TransparentWindow.WINDOW_NOTTOPMOST, 0, 0, 0, 0, 3U);
			TransparentWindow.RedrawWindow(this.WindowHandle, null, null, 1157U);
			ScreenSettings.minimumY = 0f;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000A41F0 File Offset: 0x000A23F0
		private IEnumerator EnableCoroutine()
		{
			yield return null;
			this.SetTransparentWindow(this.defaultColorKey);
			yield return base.ResolutionThereAndBackAgain();
			yield return null;
			yield break;
		}

		// Token: 0x04001095 RID: 4245
		[SerializeField]
		private TextMeshProUGUI debugText;

		// Token: 0x04001096 RID: 4246
		private IntPtr monitorForGame;

		// Token: 0x04001097 RID: 4247
		private IntPtr monitorForTaskbar;

		// Token: 0x04001098 RID: 4248
		private const uint RDW_ERASE = 4U;

		// Token: 0x04001099 RID: 4249
		private const uint RDW_INVALIDATE = 1U;

		// Token: 0x0400109A RID: 4250
		private const uint RDW_FRAME = 1024U;

		// Token: 0x0400109B RID: 4251
		private const uint RDW_ALLCHILDREN = 128U;

		// Token: 0x0400109C RID: 4252
		private const int GWL_EXSTYLE = -20;

		// Token: 0x0400109D RID: 4253
		private const uint WS_EX_LAYERED = 524288U;

		// Token: 0x0400109E RID: 4254
		private const uint WS_EX_TRANSPARANT = 32U;

		// Token: 0x0400109F RID: 4255
		private const uint SWP_NOMOVE = 2U;

		// Token: 0x040010A0 RID: 4256
		private const uint SWP_NOSIZE = 1U;

		// Token: 0x040010A1 RID: 4257
		public const uint LWA_COLORKEY = 1U;

		// Token: 0x040010A2 RID: 4258
		public const uint LWA_ALPHA = 2U;

		// Token: 0x040010A3 RID: 4259
		private uint defaultColorKey = 2U;

		// Token: 0x040010A4 RID: 4260
		private static readonly IntPtr WINDOW_BOTTOM = new IntPtr(1);

		// Token: 0x040010A5 RID: 4261
		private static readonly IntPtr WINDOW_TOP = new IntPtr(0);

		// Token: 0x040010A6 RID: 4262
		private static readonly IntPtr WINDOW_TOPMOST = new IntPtr(-1);

		// Token: 0x040010A7 RID: 4263
		private static readonly IntPtr WINDOW_NOTTOPMOST = new IntPtr(-2);

		// Token: 0x040010A8 RID: 4264
		private IntPtr WindowHandle;

		// Token: 0x040010A9 RID: 4265
		private TransparentWindow.RECT WindowRect;

		// Token: 0x040010AA RID: 4266
		private IntPtr TaskBarHandle;

		// Token: 0x040010AB RID: 4267
		private TransparentWindow.RECT TaskbarRect;

		// Token: 0x040010AC RID: 4268
		private bool weDealWithTaskbar;

		// Token: 0x040010AD RID: 4269
		private bool isMouseOverUI;

		// Token: 0x040010AE RID: 4270
		private bool isMouseOverCollider;

		// Token: 0x040010AF RID: 4271
		private PointerEventData pointerData;

		// Token: 0x040010B0 RID: 4272
		private readonly List<RaycastResult> results = new List<RaycastResult>();

		// Token: 0x040010B1 RID: 4273
		private float debugTimer;

		// Token: 0x02000566 RID: 1382
		private struct MARGINS
		{
			// Token: 0x04001C33 RID: 7219
			public int cxLeftWidth;

			// Token: 0x04001C34 RID: 7220
			public int cxRightWidth;

			// Token: 0x04001C35 RID: 7221
			public int cyTopHeight;

			// Token: 0x04001C36 RID: 7222
			public int cyBottomHeight;
		}

		// Token: 0x02000567 RID: 1383
		public struct RECT
		{
			// Token: 0x06002CB1 RID: 11441 RVA: 0x000F0FDC File Offset: 0x000EF1DC
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"Left: ",
					this.Left.ToString(),
					", Top: ",
					this.Top.ToString(),
					", Right: ",
					this.Right.ToString(),
					", Bottom: ",
					this.Bottom.ToString()
				});
			}

			// Token: 0x04001C37 RID: 7223
			public int Left;

			// Token: 0x04001C38 RID: 7224
			public int Top;

			// Token: 0x04001C39 RID: 7225
			public int Right;

			// Token: 0x04001C3A RID: 7226
			public int Bottom;
		}
	}
}
