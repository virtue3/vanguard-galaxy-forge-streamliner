using System;
using Behaviour.UI;
using Behaviour.UI.DebugScreen;
using Behaviour.UI.Side_Menu;
using Behaviour.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour
{
	// Token: 0x020001A8 RID: 424
	public class GlobalControls : PersistentSingleton<GlobalControls>
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x00068F4E File Offset: 0x0006714E
		// (set) Token: 0x06000EDF RID: 3807 RVA: 0x00068F55 File Offset: 0x00067155
		public static Vector2 mousePosition { get; private set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x00068F5D File Offset: 0x0006715D
		// (set) Token: 0x06000EE1 RID: 3809 RVA: 0x00068F64 File Offset: 0x00067164
		public static Vector2 mouseWorld { get; private set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x00068F6C File Offset: 0x0006716C
		public static bool mousePressed
		{
			get
			{
				return Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00068F90 File Offset: 0x00067190
		public static bool mouseReleased
		{
			get
			{
				return Mouse.current.leftButton.wasReleasedThisFrame;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x00068FA1 File Offset: 0x000671A1
		public static bool mouseDown
		{
			get
			{
				return Mouse.current.leftButton.isPressed;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00068FB2 File Offset: 0x000671B2
		public static bool modifierShift
		{
			get
			{
				return Keyboard.current.shiftKey.isPressed;
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00068FC4 File Offset: 0x000671C4
		protected override void Awake()
		{
			base.Awake();
			this.controls = new PlayerControls();
			this.controls.UI.Cancel.performed += this.UICancel;
			this.controls.UI.Tab.performed += this.UITab;
			this.controls.UI.Console.performed += this.UIConsole;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00069050 File Offset: 0x00067250
		private unsafe void Update()
		{
			GlobalControls.mousePosition = Mouse.current.position.value;
			if (GameplayManager.camera)
			{
				GlobalControls.mouseWorld = GameplayManager.camera.ScreenToWorldPoint(GlobalControls.mousePosition);
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x000690A0 File Offset: 0x000672A0
		private void OnEnable()
		{
			PlayerControls playerControls = this.controls;
			if (playerControls == null)
			{
				return;
			}
			playerControls.Enable();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x000690B2 File Offset: 0x000672B2
		private void OnDisable()
		{
			PlayerControls playerControls = this.controls;
			if (playerControls == null)
			{
				return;
			}
			playerControls.Disable();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000690C4 File Offset: 0x000672C4
		private void UICancel(InputAction.CallbackContext obj)
		{
			if (InventoryInteractionManager.Instance && InventoryInteractionManager.Instance.ClearSelectedItem())
			{
				return;
			}
			if (SidePanel.instance && SidePanel.instance.IsSideMenuOpen())
			{
				SidePanel.instance.CloseTab();
				return;
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00069102 File Offset: 0x00067302
		private void UITab(InputAction.CallbackContext obj)
		{
			if (SmartTextField.current)
			{
				SmartTextField.current.Tab();
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0006911A File Offset: 0x0006731A
		private void UIConsole(InputAction.CallbackContext obj)
		{
			if (Singleton<ConsoleScreen>.Instance)
			{
				Singleton<ConsoleScreen>.Instance.Toggle();
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00069132 File Offset: 0x00067332
		public static void Enable()
		{
			if (PersistentSingleton<GlobalControls>.Instance)
			{
				PersistentSingleton<GlobalControls>.Instance.gameObject.SetActive(true);
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00069150 File Offset: 0x00067350
		public static void Disable()
		{
			if (PersistentSingleton<GlobalControls>.Instance)
			{
				PersistentSingleton<GlobalControls>.Instance.gameObject.SetActive(false);
			}
		}

		// Token: 0x0400086B RID: 2155
		private PlayerControls controls;
	}
}
