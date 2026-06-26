using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000014 RID: 20
public class PlayerControls : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600012E RID: 302 RVA: 0x00009C00 File Offset: 0x00007E00
	public InputActionAsset asset { get; }

	// Token: 0x0600012F RID: 303 RVA: 0x00009C08 File Offset: 0x00007E08
	public PlayerControls()
	{
		this.asset = InputActionAsset.FromJson("{\n    \"version\": 1,\n    \"name\": \"InputSystem_Actions\",\n    \"maps\": [\n        {\n            \"name\": \"Player\",\n            \"id\": \"df70fa95-8a34-4494-b137-73ab6b9c7d37\",\n            \"actions\": [\n                {\n                    \"name\": \"Move\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"351f2ccd-1f9f-44bf-9bec-d62ac5c5f408\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Look\",\n                    \"type\": \"Value\",\n                    \"id\": \"6b444451-8a00-4d00-a97e-f47457f736a8\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"6c2ab1b8-8984-453a-af3d-a3c78ae1679a\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Interact\",\n                    \"type\": \"Button\",\n                    \"id\": \"852140f2-7766-474d-8707-702459ba45f3\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"Hold\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Crouch\",\n                    \"type\": \"Button\",\n                    \"id\": \"27c5f898-bc57-4ee1-8800-db469aca5fe3\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Jump\",\n                    \"type\": \"Button\",\n                    \"id\": \"f1ba0d36-48eb-4cd5-b651-1c94a6531f70\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Previous\",\n                    \"type\": \"Button\",\n                    \"id\": \"2776c80d-3c14-4091-8c56-d04ced07a2b0\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Next\",\n                    \"type\": \"Button\",\n                    \"id\": \"b7230bb6-fc9b-4f52-8b25-f5e19cb2c2ba\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Sprint\",\n                    \"type\": \"Button\",\n                    \"id\": \"641cd816-40e6-41b4-8c3d-04687c349290\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Follow\",\n                    \"type\": \"Button\",\n                    \"id\": \"74c0a4f9-1cf5-41ed-81f6-1bc09ca54cd8\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Hold Position\",\n                    \"type\": \"Button\",\n                    \"id\": \"ce445111-8802-4d81-a859-e379c6f18359\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Toggle Autopilot\",\n                    \"type\": \"Button\",\n                    \"id\": \"d834a9fe-e216-407c-bbc7-8f9b4e447d8f\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ToggleEchoTravel\",\n                    \"type\": \"Button\",\n                    \"id\": \"ce23382c-3c92-4078-a3f6-edb32a84452c\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Rotate\",\n                    \"type\": \"PassThrough\",\n                    \"id\": \"1118d138-7448-4f00-82c8-920f746052a1\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"978bfe49-cc26-4a3d-ab7b-7d7a29327403\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"WASD\",\n                    \"id\": \"00ca640b-d935-4593-8157-c05846ea39b3\",\n                    \"path\": \"Dpad(mode=1)\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"e2062cb9-1b15-46a2-838c-2f8d72a0bdd9\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"8180e8bd-4097-4f4e-ab88-4523101a6ce9\",\n                    \"path\": \"<Keyboard>/upArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"320bffee-a40b-4347-ac70-c210eb8bc73a\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"1c5327b5-f71c-4f60-99c7-4e737386f1d1\",\n                    \"path\": \"<Keyboard>/downArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"d2581a9b-1d11-4566-b27d-b92aff5fabbc\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"2e46982e-44cc-431b-9f0b-c11910bf467a\",\n                    \"path\": \"<Keyboard>/leftArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"fcfe95b8-67b9-4526-84b5-5d0bc98d6400\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"77bff152-3580-4b21-b6de-dcd0c7e41164\",\n                    \"path\": \"<Keyboard>/rightArrow\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1635d3fe-58b6-4ba9-a4e2-f4b964f6b5c8\",\n                    \"path\": \"<XRController>/{Primary2DAxis}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"One Modifier\",\n                    \"id\": \"b0d1f8f5-050c-4ac0-8d35-bc426474cc02\",\n                    \"path\": \"OneModifier\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ToggleEchoTravel\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"modifier\",\n                    \"id\": \"72784e37-e548-4d61-8bab-700ede0d9485\",\n                    \"path\": \"<Keyboard>/ctrl\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ToggleEchoTravel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"binding\",\n                    \"id\": \"73695c50-070e-48f8-aa2d-3f5c48245a89\",\n                    \"path\": \"<Keyboard>/t\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ToggleEchoTravel\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c1f7a91b-d0fd-4a62-997e-7fb9b69bf235\",\n                    \"path\": \"<Gamepad>/rightStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8c8e490b-c610-4785-884f-f04217b23ca4\",\n                    \"path\": \"<Pointer>/delta\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse;Touch\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3e5f5442-8668-4b27-a940-df99bad7e831\",\n                    \"path\": \"<Joystick>/{Hatswitch}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"143bb1cd-cc10-4eca-a2f0-a3664166fe91\",\n                    \"path\": \"<Gamepad>/buttonWest\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"05f6913d-c316-48b2-a6bb-e225f14c7960\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"886e731e-7071-4ae4-95c0-e61739dad6fd\",\n                    \"path\": \"<Touchscreen>/primaryTouch/tap\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Touch\",\n                    \"action\": \"Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"ee3d0cd2-254e-47a7-a8cb-bc94d9658c54\",\n                    \"path\": \"<Joystick>/trigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Joystick\",\n                    \"action\": \"Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8255d333-5683-4943-a58a-ccb207ff1dce\",\n                    \"path\": \"<XRController>/{PrimaryAction}\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b3c1c7f0-bd20-4ee7-a0f1-899b24bca6d7\",\n                    \"path\": \"<Keyboard>/enter\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"cbac6039-9c09-46a1-b5f2-4e5124ccb5ed\",\n                    \"path\": \"<Keyboard>/2\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Next\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"e15ca19d-e649-4852-97d5-7fe8ccc44e94\",\n                    \"path\": \"<Gamepad>/dpad/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Next\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f2e9ba44-c423-42a7-ad56-f20975884794\",\n                    \"path\": \"<Keyboard>/leftShift\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8cbb2f4b-a784-49cc-8d5e-c010b8c7f4e6\",\n                    \"path\": \"<Gamepad>/leftStickPress\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d8bf24bf-3f2f-4160-a97c-38ec1eb520ba\",\n                    \"path\": \"<XRController>/trigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"eb40bb66-4559-4dfa-9a2f-820438abb426\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"daba33a1-ad0c-4742-a909-43ad1cdfbeb6\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"603f3daf-40bd-4854-8724-93e8017f59e3\",\n                    \"path\": \"<XRController>/secondaryButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"XR\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1534dc16-a6aa-499d-9c3a-22b47347b52a\",\n                    \"path\": \"<Keyboard>/1\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Previous\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"25060bbd-a3a6-476e-8fba-45ae484aad05\",\n                    \"path\": \"<Gamepad>/dpad/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Previous\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"1c04ea5f-b012-41d1-a6f7-02e963b52893\",\n                    \"path\": \"<Keyboard>/e\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Interact\",\n                    \"isComposite\": false,\n               [...string is too long...]");
		this.m_Player = this.asset.FindActionMap("Player", true);
		this.m_Player_Move = this.m_Player.FindAction("Move", true);
		this.m_Player_Look = this.m_Player.FindAction("Look", true);
		this.m_Player_Attack = this.m_Player.FindAction("Attack", true);
		this.m_Player_Interact = this.m_Player.FindAction("Interact", true);
		this.m_Player_Crouch = this.m_Player.FindAction("Crouch", true);
		this.m_Player_Jump = this.m_Player.FindAction("Jump", true);
		this.m_Player_Previous = this.m_Player.FindAction("Previous", true);
		this.m_Player_Next = this.m_Player.FindAction("Next", true);
		this.m_Player_Sprint = this.m_Player.FindAction("Sprint", true);
		this.m_Player_Follow = this.m_Player.FindAction("Follow", true);
		this.m_Player_HoldPosition = this.m_Player.FindAction("Hold Position", true);
		this.m_Player_ToggleAutopilot = this.m_Player.FindAction("Toggle Autopilot", true);
		this.m_Player_ToggleEchoTravel = this.m_Player.FindAction("ToggleEchoTravel", true);
		this.m_Player_Rotate = this.m_Player.FindAction("Rotate", true);
		this.m_UI = this.asset.FindActionMap("UI", true);
		this.m_UI_Navigate = this.m_UI.FindAction("Navigate", true);
		this.m_UI_Submit = this.m_UI.FindAction("Submit", true);
		this.m_UI_Cancel = this.m_UI.FindAction("Cancel", true);
		this.m_UI_Point = this.m_UI.FindAction("Point", true);
		this.m_UI_Click = this.m_UI.FindAction("Click", true);
		this.m_UI_RightClick = this.m_UI.FindAction("RightClick", true);
		this.m_UI_MiddleClick = this.m_UI.FindAction("MiddleClick", true);
		this.m_UI_ScrollWheel = this.m_UI.FindAction("ScrollWheel", true);
		this.m_UI_TrackedDevicePosition = this.m_UI.FindAction("TrackedDevicePosition", true);
		this.m_UI_TrackedDeviceOrientation = this.m_UI.FindAction("TrackedDeviceOrientation", true);
		this.m_UI_ToggleMap = this.m_UI.FindAction("ToggleMap", true);
		this.m_UI_ToggleInventory = this.m_UI.FindAction("ToggleInventory", true);
		this.m_UI_Tab = this.m_UI.FindAction("Tab", true);
		this.m_UI_Console = this.m_UI.FindAction("Console", true);
		this.m_UI_LeftShift = this.m_UI.FindAction("LeftShift", true);
		this.m_UI_Escape = this.m_UI.FindAction("Escape", true);
		this.m_UI_ToggleAudio = this.m_UI.FindAction("ToggleAudio", true);
		this.m_UI_HideUI = this.m_UI.FindAction("Hide UI", true);
		this.m_UI_ToggleHPBars = this.m_UI.FindAction("ToggleHPBars", true);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00009F8C File Offset: 0x0000818C
	~PlayerControls()
	{
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00009FB4 File Offset: 0x000081B4
	public void Dispose()
	{
		UnityEngine.Object.Destroy(this.asset);
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000132 RID: 306 RVA: 0x00009FC1 File Offset: 0x000081C1
	// (set) Token: 0x06000133 RID: 307 RVA: 0x00009FCE File Offset: 0x000081CE
	public InputBinding? bindingMask
	{
		get
		{
			return this.asset.bindingMask;
		}
		set
		{
			this.asset.bindingMask = value;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000134 RID: 308 RVA: 0x00009FDC File Offset: 0x000081DC
	// (set) Token: 0x06000135 RID: 309 RVA: 0x00009FE9 File Offset: 0x000081E9
	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return this.asset.devices;
		}
		set
		{
			this.asset.devices = value;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000136 RID: 310 RVA: 0x00009FF7 File Offset: 0x000081F7
	public ReadOnlyArray<InputControlScheme> controlSchemes
	{
		get
		{
			return this.asset.controlSchemes;
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000A004 File Offset: 0x00008204
	public bool Contains(InputAction action)
	{
		return this.asset.Contains(action);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000A012 File Offset: 0x00008212
	public IEnumerator<InputAction> GetEnumerator()
	{
		return this.asset.GetEnumerator();
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000A01F File Offset: 0x0000821F
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000A027 File Offset: 0x00008227
	public void Enable()
	{
		this.asset.Enable();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000A034 File Offset: 0x00008234
	public void Disable()
	{
		this.asset.Disable();
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600013C RID: 316 RVA: 0x0000A041 File Offset: 0x00008241
	public IEnumerable<InputBinding> bindings
	{
		get
		{
			return this.asset.bindings;
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000A04E File Offset: 0x0000824E
	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return this.asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000A05D File Offset: 0x0000825D
	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return this.asset.FindBinding(bindingMask, out action);
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600013F RID: 319 RVA: 0x0000A06C File Offset: 0x0000826C
	public PlayerControls.PlayerActions Player
	{
		get
		{
			return new PlayerControls.PlayerActions(this);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000140 RID: 320 RVA: 0x0000A074 File Offset: 0x00008274
	public PlayerControls.UIActions UI
	{
		get
		{
			return new PlayerControls.UIActions(this);
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000141 RID: 321 RVA: 0x0000A07C File Offset: 0x0000827C
	public InputControlScheme KeyboardMouseScheme
	{
		get
		{
			if (this.m_KeyboardMouseSchemeIndex == -1)
			{
				this.m_KeyboardMouseSchemeIndex = this.asset.FindControlSchemeIndex("Keyboard&Mouse");
			}
			return this.asset.controlSchemes[this.m_KeyboardMouseSchemeIndex];
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000142 RID: 322 RVA: 0x0000A0C4 File Offset: 0x000082C4
	public InputControlScheme GamepadScheme
	{
		get
		{
			if (this.m_GamepadSchemeIndex == -1)
			{
				this.m_GamepadSchemeIndex = this.asset.FindControlSchemeIndex("Gamepad");
			}
			return this.asset.controlSchemes[this.m_GamepadSchemeIndex];
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000143 RID: 323 RVA: 0x0000A10C File Offset: 0x0000830C
	public InputControlScheme TouchScheme
	{
		get
		{
			if (this.m_TouchSchemeIndex == -1)
			{
				this.m_TouchSchemeIndex = this.asset.FindControlSchemeIndex("Touch");
			}
			return this.asset.controlSchemes[this.m_TouchSchemeIndex];
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000144 RID: 324 RVA: 0x0000A154 File Offset: 0x00008354
	public InputControlScheme JoystickScheme
	{
		get
		{
			if (this.m_JoystickSchemeIndex == -1)
			{
				this.m_JoystickSchemeIndex = this.asset.FindControlSchemeIndex("Joystick");
			}
			return this.asset.controlSchemes[this.m_JoystickSchemeIndex];
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000145 RID: 325 RVA: 0x0000A19C File Offset: 0x0000839C
	public InputControlScheme XRScheme
	{
		get
		{
			if (this.m_XRSchemeIndex == -1)
			{
				this.m_XRSchemeIndex = this.asset.FindControlSchemeIndex("XR");
			}
			return this.asset.controlSchemes[this.m_XRSchemeIndex];
		}
	}

	// Token: 0x040000A2 RID: 162
	private readonly InputActionMap m_Player;

	// Token: 0x040000A3 RID: 163
	private List<PlayerControls.IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<PlayerControls.IPlayerActions>();

	// Token: 0x040000A4 RID: 164
	private readonly InputAction m_Player_Move;

	// Token: 0x040000A5 RID: 165
	private readonly InputAction m_Player_Look;

	// Token: 0x040000A6 RID: 166
	private readonly InputAction m_Player_Attack;

	// Token: 0x040000A7 RID: 167
	private readonly InputAction m_Player_Interact;

	// Token: 0x040000A8 RID: 168
	private readonly InputAction m_Player_Crouch;

	// Token: 0x040000A9 RID: 169
	private readonly InputAction m_Player_Jump;

	// Token: 0x040000AA RID: 170
	private readonly InputAction m_Player_Previous;

	// Token: 0x040000AB RID: 171
	private readonly InputAction m_Player_Next;

	// Token: 0x040000AC RID: 172
	private readonly InputAction m_Player_Sprint;

	// Token: 0x040000AD RID: 173
	private readonly InputAction m_Player_Follow;

	// Token: 0x040000AE RID: 174
	private readonly InputAction m_Player_HoldPosition;

	// Token: 0x040000AF RID: 175
	private readonly InputAction m_Player_ToggleAutopilot;

	// Token: 0x040000B0 RID: 176
	private readonly InputAction m_Player_ToggleEchoTravel;

	// Token: 0x040000B1 RID: 177
	private readonly InputAction m_Player_Rotate;

	// Token: 0x040000B2 RID: 178
	private readonly InputActionMap m_UI;

	// Token: 0x040000B3 RID: 179
	private List<PlayerControls.IUIActions> m_UIActionsCallbackInterfaces = new List<PlayerControls.IUIActions>();

	// Token: 0x040000B4 RID: 180
	private readonly InputAction m_UI_Navigate;

	// Token: 0x040000B5 RID: 181
	private readonly InputAction m_UI_Submit;

	// Token: 0x040000B6 RID: 182
	private readonly InputAction m_UI_Cancel;

	// Token: 0x040000B7 RID: 183
	private readonly InputAction m_UI_Point;

	// Token: 0x040000B8 RID: 184
	private readonly InputAction m_UI_Click;

	// Token: 0x040000B9 RID: 185
	private readonly InputAction m_UI_RightClick;

	// Token: 0x040000BA RID: 186
	private readonly InputAction m_UI_MiddleClick;

	// Token: 0x040000BB RID: 187
	private readonly InputAction m_UI_ScrollWheel;

	// Token: 0x040000BC RID: 188
	private readonly InputAction m_UI_TrackedDevicePosition;

	// Token: 0x040000BD RID: 189
	private readonly InputAction m_UI_TrackedDeviceOrientation;

	// Token: 0x040000BE RID: 190
	private readonly InputAction m_UI_ToggleMap;

	// Token: 0x040000BF RID: 191
	private readonly InputAction m_UI_ToggleInventory;

	// Token: 0x040000C0 RID: 192
	private readonly InputAction m_UI_Tab;

	// Token: 0x040000C1 RID: 193
	private readonly InputAction m_UI_Console;

	// Token: 0x040000C2 RID: 194
	private readonly InputAction m_UI_LeftShift;

	// Token: 0x040000C3 RID: 195
	private readonly InputAction m_UI_Escape;

	// Token: 0x040000C4 RID: 196
	private readonly InputAction m_UI_ToggleAudio;

	// Token: 0x040000C5 RID: 197
	private readonly InputAction m_UI_HideUI;

	// Token: 0x040000C6 RID: 198
	private readonly InputAction m_UI_ToggleHPBars;

	// Token: 0x040000C7 RID: 199
	private int m_KeyboardMouseSchemeIndex = -1;

	// Token: 0x040000C8 RID: 200
	private int m_GamepadSchemeIndex = -1;

	// Token: 0x040000C9 RID: 201
	private int m_TouchSchemeIndex = -1;

	// Token: 0x040000CA RID: 202
	private int m_JoystickSchemeIndex = -1;

	// Token: 0x040000CB RID: 203
	private int m_XRSchemeIndex = -1;

	// Token: 0x020003FB RID: 1019
	public struct PlayerActions
	{
		// Token: 0x0600260E RID: 9742 RVA: 0x000D34B1 File Offset: 0x000D16B1
		public PlayerActions(PlayerControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x000D34BA File Offset: 0x000D16BA
		public InputAction Move
		{
			get
			{
				return this.m_Wrapper.m_Player_Move;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x000D34C7 File Offset: 0x000D16C7
		public InputAction Look
		{
			get
			{
				return this.m_Wrapper.m_Player_Look;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x000D34D4 File Offset: 0x000D16D4
		public InputAction Attack
		{
			get
			{
				return this.m_Wrapper.m_Player_Attack;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x000D34E1 File Offset: 0x000D16E1
		public InputAction Interact
		{
			get
			{
				return this.m_Wrapper.m_Player_Interact;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x000D34EE File Offset: 0x000D16EE
		public InputAction Crouch
		{
			get
			{
				return this.m_Wrapper.m_Player_Crouch;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x000D34FB File Offset: 0x000D16FB
		public InputAction Jump
		{
			get
			{
				return this.m_Wrapper.m_Player_Jump;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x000D3508 File Offset: 0x000D1708
		public InputAction Previous
		{
			get
			{
				return this.m_Wrapper.m_Player_Previous;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x000D3515 File Offset: 0x000D1715
		public InputAction Next
		{
			get
			{
				return this.m_Wrapper.m_Player_Next;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x000D3522 File Offset: 0x000D1722
		public InputAction Sprint
		{
			get
			{
				return this.m_Wrapper.m_Player_Sprint;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000D352F File Offset: 0x000D172F
		public InputAction Follow
		{
			get
			{
				return this.m_Wrapper.m_Player_Follow;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x000D353C File Offset: 0x000D173C
		public InputAction HoldPosition
		{
			get
			{
				return this.m_Wrapper.m_Player_HoldPosition;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x000D3549 File Offset: 0x000D1749
		public InputAction ToggleAutopilot
		{
			get
			{
				return this.m_Wrapper.m_Player_ToggleAutopilot;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x000D3556 File Offset: 0x000D1756
		public InputAction ToggleEchoTravel
		{
			get
			{
				return this.m_Wrapper.m_Player_ToggleEchoTravel;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600261C RID: 9756 RVA: 0x000D3563 File Offset: 0x000D1763
		public InputAction Rotate
		{
			get
			{
				return this.m_Wrapper.m_Player_Rotate;
			}
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000D3570 File Offset: 0x000D1770
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_Player;
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000D357D File Offset: 0x000D177D
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000D358A File Offset: 0x000D178A
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000D3597 File Offset: 0x000D1797
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000D35A4 File Offset: 0x000D17A4
		public static implicit operator InputActionMap(PlayerControls.PlayerActions set)
		{
			return set.Get();
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000D35B0 File Offset: 0x000D17B0
		public void AddCallbacks(PlayerControls.IPlayerActions instance)
		{
			if (instance == null || this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
			this.Move.started += instance.OnMove;
			this.Move.performed += instance.OnMove;
			this.Move.canceled += instance.OnMove;
			this.Look.started += instance.OnLook;
			this.Look.performed += instance.OnLook;
			this.Look.canceled += instance.OnLook;
			this.Attack.started += instance.OnAttack;
			this.Attack.performed += instance.OnAttack;
			this.Attack.canceled += instance.OnAttack;
			this.Interact.started += instance.OnInteract;
			this.Interact.performed += instance.OnInteract;
			this.Interact.canceled += instance.OnInteract;
			this.Crouch.started += instance.OnCrouch;
			this.Crouch.performed += instance.OnCrouch;
			this.Crouch.canceled += instance.OnCrouch;
			this.Jump.started += instance.OnJump;
			this.Jump.performed += instance.OnJump;
			this.Jump.canceled += instance.OnJump;
			this.Previous.started += instance.OnPrevious;
			this.Previous.performed += instance.OnPrevious;
			this.Previous.canceled += instance.OnPrevious;
			this.Next.started += instance.OnNext;
			this.Next.performed += instance.OnNext;
			this.Next.canceled += instance.OnNext;
			this.Sprint.started += instance.OnSprint;
			this.Sprint.performed += instance.OnSprint;
			this.Sprint.canceled += instance.OnSprint;
			this.Follow.started += instance.OnFollow;
			this.Follow.performed += instance.OnFollow;
			this.Follow.canceled += instance.OnFollow;
			this.HoldPosition.started += instance.OnHoldPosition;
			this.HoldPosition.performed += instance.OnHoldPosition;
			this.HoldPosition.canceled += instance.OnHoldPosition;
			this.ToggleAutopilot.started += instance.OnToggleAutopilot;
			this.ToggleAutopilot.performed += instance.OnToggleAutopilot;
			this.ToggleAutopilot.canceled += instance.OnToggleAutopilot;
			this.ToggleEchoTravel.started += instance.OnToggleEchoTravel;
			this.ToggleEchoTravel.performed += instance.OnToggleEchoTravel;
			this.ToggleEchoTravel.canceled += instance.OnToggleEchoTravel;
			this.Rotate.started += instance.OnRotate;
			this.Rotate.performed += instance.OnRotate;
			this.Rotate.canceled += instance.OnRotate;
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000D39D8 File Offset: 0x000D1BD8
		private void UnregisterCallbacks(PlayerControls.IPlayerActions instance)
		{
			this.Move.started -= instance.OnMove;
			this.Move.performed -= instance.OnMove;
			this.Move.canceled -= instance.OnMove;
			this.Look.started -= instance.OnLook;
			this.Look.performed -= instance.OnLook;
			this.Look.canceled -= instance.OnLook;
			this.Attack.started -= instance.OnAttack;
			this.Attack.performed -= instance.OnAttack;
			this.Attack.canceled -= instance.OnAttack;
			this.Interact.started -= instance.OnInteract;
			this.Interact.performed -= instance.OnInteract;
			this.Interact.canceled -= instance.OnInteract;
			this.Crouch.started -= instance.OnCrouch;
			this.Crouch.performed -= instance.OnCrouch;
			this.Crouch.canceled -= instance.OnCrouch;
			this.Jump.started -= instance.OnJump;
			this.Jump.performed -= instance.OnJump;
			this.Jump.canceled -= instance.OnJump;
			this.Previous.started -= instance.OnPrevious;
			this.Previous.performed -= instance.OnPrevious;
			this.Previous.canceled -= instance.OnPrevious;
			this.Next.started -= instance.OnNext;
			this.Next.performed -= instance.OnNext;
			this.Next.canceled -= instance.OnNext;
			this.Sprint.started -= instance.OnSprint;
			this.Sprint.performed -= instance.OnSprint;
			this.Sprint.canceled -= instance.OnSprint;
			this.Follow.started -= instance.OnFollow;
			this.Follow.performed -= instance.OnFollow;
			this.Follow.canceled -= instance.OnFollow;
			this.HoldPosition.started -= instance.OnHoldPosition;
			this.HoldPosition.performed -= instance.OnHoldPosition;
			this.HoldPosition.canceled -= instance.OnHoldPosition;
			this.ToggleAutopilot.started -= instance.OnToggleAutopilot;
			this.ToggleAutopilot.performed -= instance.OnToggleAutopilot;
			this.ToggleAutopilot.canceled -= instance.OnToggleAutopilot;
			this.ToggleEchoTravel.started -= instance.OnToggleEchoTravel;
			this.ToggleEchoTravel.performed -= instance.OnToggleEchoTravel;
			this.ToggleEchoTravel.canceled -= instance.OnToggleEchoTravel;
			this.Rotate.started -= instance.OnRotate;
			this.Rotate.performed -= instance.OnRotate;
			this.Rotate.canceled -= instance.OnRotate;
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000D3DD5 File Offset: 0x000D1FD5
		public void RemoveCallbacks(PlayerControls.IPlayerActions instance)
		{
			if (this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000D3DF4 File Offset: 0x000D1FF4
		public void SetCallbacks(PlayerControls.IPlayerActions instance)
		{
			foreach (PlayerControls.IPlayerActions instance2 in this.m_Wrapper.m_PlayerActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(instance2);
			}
			this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x04001764 RID: 5988
		private PlayerControls m_Wrapper;
	}

	// Token: 0x020003FC RID: 1020
	public struct UIActions
	{
		// Token: 0x06002626 RID: 9766 RVA: 0x000D3E64 File Offset: 0x000D2064
		public UIActions(PlayerControls wrapper)
		{
			this.m_Wrapper = wrapper;
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x000D3E6D File Offset: 0x000D206D
		public InputAction Navigate
		{
			get
			{
				return this.m_Wrapper.m_UI_Navigate;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x000D3E7A File Offset: 0x000D207A
		public InputAction Submit
		{
			get
			{
				return this.m_Wrapper.m_UI_Submit;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x000D3E87 File Offset: 0x000D2087
		public InputAction Cancel
		{
			get
			{
				return this.m_Wrapper.m_UI_Cancel;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x000D3E94 File Offset: 0x000D2094
		public InputAction Point
		{
			get
			{
				return this.m_Wrapper.m_UI_Point;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x000D3EA1 File Offset: 0x000D20A1
		public InputAction Click
		{
			get
			{
				return this.m_Wrapper.m_UI_Click;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600262C RID: 9772 RVA: 0x000D3EAE File Offset: 0x000D20AE
		public InputAction RightClick
		{
			get
			{
				return this.m_Wrapper.m_UI_RightClick;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x000D3EBB File Offset: 0x000D20BB
		public InputAction MiddleClick
		{
			get
			{
				return this.m_Wrapper.m_UI_MiddleClick;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x000D3EC8 File Offset: 0x000D20C8
		public InputAction ScrollWheel
		{
			get
			{
				return this.m_Wrapper.m_UI_ScrollWheel;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x000D3ED5 File Offset: 0x000D20D5
		public InputAction TrackedDevicePosition
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDevicePosition;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x000D3EE2 File Offset: 0x000D20E2
		public InputAction TrackedDeviceOrientation
		{
			get
			{
				return this.m_Wrapper.m_UI_TrackedDeviceOrientation;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002631 RID: 9777 RVA: 0x000D3EEF File Offset: 0x000D20EF
		public InputAction ToggleMap
		{
			get
			{
				return this.m_Wrapper.m_UI_ToggleMap;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000D3EFC File Offset: 0x000D20FC
		public InputAction ToggleInventory
		{
			get
			{
				return this.m_Wrapper.m_UI_ToggleInventory;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002633 RID: 9779 RVA: 0x000D3F09 File Offset: 0x000D2109
		public InputAction Tab
		{
			get
			{
				return this.m_Wrapper.m_UI_Tab;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000D3F16 File Offset: 0x000D2116
		public InputAction Console
		{
			get
			{
				return this.m_Wrapper.m_UI_Console;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002635 RID: 9781 RVA: 0x000D3F23 File Offset: 0x000D2123
		public InputAction LeftShift
		{
			get
			{
				return this.m_Wrapper.m_UI_LeftShift;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x000D3F30 File Offset: 0x000D2130
		public InputAction Escape
		{
			get
			{
				return this.m_Wrapper.m_UI_Escape;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x000D3F3D File Offset: 0x000D213D
		public InputAction ToggleAudio
		{
			get
			{
				return this.m_Wrapper.m_UI_ToggleAudio;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x000D3F4A File Offset: 0x000D214A
		public InputAction HideUI
		{
			get
			{
				return this.m_Wrapper.m_UI_HideUI;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x000D3F57 File Offset: 0x000D2157
		public InputAction ToggleHPBars
		{
			get
			{
				return this.m_Wrapper.m_UI_ToggleHPBars;
			}
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000D3F64 File Offset: 0x000D2164
		public InputActionMap Get()
		{
			return this.m_Wrapper.m_UI;
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000D3F71 File Offset: 0x000D2171
		public void Enable()
		{
			this.Get().Enable();
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000D3F7E File Offset: 0x000D217E
		public void Disable()
		{
			this.Get().Disable();
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x000D3F8B File Offset: 0x000D218B
		public bool enabled
		{
			get
			{
				return this.Get().enabled;
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000D3F98 File Offset: 0x000D2198
		public static implicit operator InputActionMap(PlayerControls.UIActions set)
		{
			return set.Get();
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000D3FA4 File Offset: 0x000D21A4
		public void AddCallbacks(PlayerControls.IUIActions instance)
		{
			if (instance == null || this.m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance))
			{
				return;
			}
			this.m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
			this.Navigate.started += instance.OnNavigate;
			this.Navigate.performed += instance.OnNavigate;
			this.Navigate.canceled += instance.OnNavigate;
			this.Submit.started += instance.OnSubmit;
			this.Submit.performed += instance.OnSubmit;
			this.Submit.canceled += instance.OnSubmit;
			this.Cancel.started += instance.OnCancel;
			this.Cancel.performed += instance.OnCancel;
			this.Cancel.canceled += instance.OnCancel;
			this.Point.started += instance.OnPoint;
			this.Point.performed += instance.OnPoint;
			this.Point.canceled += instance.OnPoint;
			this.Click.started += instance.OnClick;
			this.Click.performed += instance.OnClick;
			this.Click.canceled += instance.OnClick;
			this.RightClick.started += instance.OnRightClick;
			this.RightClick.performed += instance.OnRightClick;
			this.RightClick.canceled += instance.OnRightClick;
			this.MiddleClick.started += instance.OnMiddleClick;
			this.MiddleClick.performed += instance.OnMiddleClick;
			this.MiddleClick.canceled += instance.OnMiddleClick;
			this.ScrollWheel.started += instance.OnScrollWheel;
			this.ScrollWheel.performed += instance.OnScrollWheel;
			this.ScrollWheel.canceled += instance.OnScrollWheel;
			this.TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
			this.TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
			this.ToggleMap.started += instance.OnToggleMap;
			this.ToggleMap.performed += instance.OnToggleMap;
			this.ToggleMap.canceled += instance.OnToggleMap;
			this.ToggleInventory.started += instance.OnToggleInventory;
			this.ToggleInventory.performed += instance.OnToggleInventory;
			this.ToggleInventory.canceled += instance.OnToggleInventory;
			this.Tab.started += instance.OnTab;
			this.Tab.performed += instance.OnTab;
			this.Tab.canceled += instance.OnTab;
			this.Console.started += instance.OnConsole;
			this.Console.performed += instance.OnConsole;
			this.Console.canceled += instance.OnConsole;
			this.LeftShift.started += instance.OnLeftShift;
			this.LeftShift.performed += instance.OnLeftShift;
			this.LeftShift.canceled += instance.OnLeftShift;
			this.Escape.started += instance.OnEscape;
			this.Escape.performed += instance.OnEscape;
			this.Escape.canceled += instance.OnEscape;
			this.ToggleAudio.started += instance.OnToggleAudio;
			this.ToggleAudio.performed += instance.OnToggleAudio;
			this.ToggleAudio.canceled += instance.OnToggleAudio;
			this.HideUI.started += instance.OnHideUI;
			this.HideUI.performed += instance.OnHideUI;
			this.HideUI.canceled += instance.OnHideUI;
			this.ToggleHPBars.started += instance.OnToggleHPBars;
			this.ToggleHPBars.performed += instance.OnToggleHPBars;
			this.ToggleHPBars.canceled += instance.OnToggleHPBars;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000D4534 File Offset: 0x000D2734
		private void UnregisterCallbacks(PlayerControls.IUIActions instance)
		{
			this.Navigate.started -= instance.OnNavigate;
			this.Navigate.performed -= instance.OnNavigate;
			this.Navigate.canceled -= instance.OnNavigate;
			this.Submit.started -= instance.OnSubmit;
			this.Submit.performed -= instance.OnSubmit;
			this.Submit.canceled -= instance.OnSubmit;
			this.Cancel.started -= instance.OnCancel;
			this.Cancel.performed -= instance.OnCancel;
			this.Cancel.canceled -= instance.OnCancel;
			this.Point.started -= instance.OnPoint;
			this.Point.performed -= instance.OnPoint;
			this.Point.canceled -= instance.OnPoint;
			this.Click.started -= instance.OnClick;
			this.Click.performed -= instance.OnClick;
			this.Click.canceled -= instance.OnClick;
			this.RightClick.started -= instance.OnRightClick;
			this.RightClick.performed -= instance.OnRightClick;
			this.RightClick.canceled -= instance.OnRightClick;
			this.MiddleClick.started -= instance.OnMiddleClick;
			this.MiddleClick.performed -= instance.OnMiddleClick;
			this.MiddleClick.canceled -= instance.OnMiddleClick;
			this.ScrollWheel.started -= instance.OnScrollWheel;
			this.ScrollWheel.performed -= instance.OnScrollWheel;
			this.ScrollWheel.canceled -= instance.OnScrollWheel;
			this.TrackedDevicePosition.started -= instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.performed -= instance.OnTrackedDevicePosition;
			this.TrackedDevicePosition.canceled -= instance.OnTrackedDevicePosition;
			this.TrackedDeviceOrientation.started -= instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.performed -= instance.OnTrackedDeviceOrientation;
			this.TrackedDeviceOrientation.canceled -= instance.OnTrackedDeviceOrientation;
			this.ToggleMap.started -= instance.OnToggleMap;
			this.ToggleMap.performed -= instance.OnToggleMap;
			this.ToggleMap.canceled -= instance.OnToggleMap;
			this.ToggleInventory.started -= instance.OnToggleInventory;
			this.ToggleInventory.performed -= instance.OnToggleInventory;
			this.ToggleInventory.canceled -= instance.OnToggleInventory;
			this.Tab.started -= instance.OnTab;
			this.Tab.performed -= instance.OnTab;
			this.Tab.canceled -= instance.OnTab;
			this.Console.started -= instance.OnConsole;
			this.Console.performed -= instance.OnConsole;
			this.Console.canceled -= instance.OnConsole;
			this.LeftShift.started -= instance.OnLeftShift;
			this.LeftShift.performed -= instance.OnLeftShift;
			this.LeftShift.canceled -= instance.OnLeftShift;
			this.Escape.started -= instance.OnEscape;
			this.Escape.performed -= instance.OnEscape;
			this.Escape.canceled -= instance.OnEscape;
			this.ToggleAudio.started -= instance.OnToggleAudio;
			this.ToggleAudio.performed -= instance.OnToggleAudio;
			this.ToggleAudio.canceled -= instance.OnToggleAudio;
			this.HideUI.started -= instance.OnHideUI;
			this.HideUI.performed -= instance.OnHideUI;
			this.HideUI.canceled -= instance.OnHideUI;
			this.ToggleHPBars.started -= instance.OnToggleHPBars;
			this.ToggleHPBars.performed -= instance.OnToggleHPBars;
			this.ToggleHPBars.canceled -= instance.OnToggleHPBars;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000D4A99 File Offset: 0x000D2C99
		public void RemoveCallbacks(PlayerControls.IUIActions instance)
		{
			if (this.m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
			{
				this.UnregisterCallbacks(instance);
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000D4AB8 File Offset: 0x000D2CB8
		public void SetCallbacks(PlayerControls.IUIActions instance)
		{
			foreach (PlayerControls.IUIActions instance2 in this.m_Wrapper.m_UIActionsCallbackInterfaces)
			{
				this.UnregisterCallbacks(instance2);
			}
			this.m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
			this.AddCallbacks(instance);
		}

		// Token: 0x04001765 RID: 5989
		private PlayerControls m_Wrapper;
	}

	// Token: 0x020003FD RID: 1021
	public interface IPlayerActions
	{
		// Token: 0x06002643 RID: 9795
		void OnMove(InputAction.CallbackContext context);

		// Token: 0x06002644 RID: 9796
		void OnLook(InputAction.CallbackContext context);

		// Token: 0x06002645 RID: 9797
		void OnAttack(InputAction.CallbackContext context);

		// Token: 0x06002646 RID: 9798
		void OnInteract(InputAction.CallbackContext context);

		// Token: 0x06002647 RID: 9799
		void OnCrouch(InputAction.CallbackContext context);

		// Token: 0x06002648 RID: 9800
		void OnJump(InputAction.CallbackContext context);

		// Token: 0x06002649 RID: 9801
		void OnPrevious(InputAction.CallbackContext context);

		// Token: 0x0600264A RID: 9802
		void OnNext(InputAction.CallbackContext context);

		// Token: 0x0600264B RID: 9803
		void OnSprint(InputAction.CallbackContext context);

		// Token: 0x0600264C RID: 9804
		void OnFollow(InputAction.CallbackContext context);

		// Token: 0x0600264D RID: 9805
		void OnHoldPosition(InputAction.CallbackContext context);

		// Token: 0x0600264E RID: 9806
		void OnToggleAutopilot(InputAction.CallbackContext context);

		// Token: 0x0600264F RID: 9807
		void OnToggleEchoTravel(InputAction.CallbackContext context);

		// Token: 0x06002650 RID: 9808
		void OnRotate(InputAction.CallbackContext context);
	}

	// Token: 0x020003FE RID: 1022
	public interface IUIActions
	{
		// Token: 0x06002651 RID: 9809
		void OnNavigate(InputAction.CallbackContext context);

		// Token: 0x06002652 RID: 9810
		void OnSubmit(InputAction.CallbackContext context);

		// Token: 0x06002653 RID: 9811
		void OnCancel(InputAction.CallbackContext context);

		// Token: 0x06002654 RID: 9812
		void OnPoint(InputAction.CallbackContext context);

		// Token: 0x06002655 RID: 9813
		void OnClick(InputAction.CallbackContext context);

		// Token: 0x06002656 RID: 9814
		void OnRightClick(InputAction.CallbackContext context);

		// Token: 0x06002657 RID: 9815
		void OnMiddleClick(InputAction.CallbackContext context);

		// Token: 0x06002658 RID: 9816
		void OnScrollWheel(InputAction.CallbackContext context);

		// Token: 0x06002659 RID: 9817
		void OnTrackedDevicePosition(InputAction.CallbackContext context);

		// Token: 0x0600265A RID: 9818
		void OnTrackedDeviceOrientation(InputAction.CallbackContext context);

		// Token: 0x0600265B RID: 9819
		void OnToggleMap(InputAction.CallbackContext context);

		// Token: 0x0600265C RID: 9820
		void OnToggleInventory(InputAction.CallbackContext context);

		// Token: 0x0600265D RID: 9821
		void OnTab(InputAction.CallbackContext context);

		// Token: 0x0600265E RID: 9822
		void OnConsole(InputAction.CallbackContext context);

		// Token: 0x0600265F RID: 9823
		void OnLeftShift(InputAction.CallbackContext context);

		// Token: 0x06002660 RID: 9824
		void OnEscape(InputAction.CallbackContext context);

		// Token: 0x06002661 RID: 9825
		void OnToggleAudio(InputAction.CallbackContext context);

		// Token: 0x06002662 RID: 9826
		void OnHideUI(InputAction.CallbackContext context);

		// Token: 0x06002663 RID: 9827
		void OnToggleHPBars(InputAction.CallbackContext context);
	}
}
