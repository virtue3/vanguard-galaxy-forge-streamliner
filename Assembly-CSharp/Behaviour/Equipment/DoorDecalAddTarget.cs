using System;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Module.DroneBay.OpeningMechanism;
using Behaviour.UI;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Location;
using Behaviour.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Equipment
{
	// Token: 0x02000341 RID: 833
	public class DoorDecalAddTarget : MonoBehaviour
	{
		// Token: 0x06001FAF RID: 8111 RVA: 0x000BBAD4 File Offset: 0x000B9CD4
		public void Init(SpaceShip ship, Door door, DroneBayModule module, int bayIndex, int doorIndex)
		{
			this._ship = ship;
			this._door = door;
			this._module = module;
			this._bayIndex = bayIndex;
			this._doorIndex = doorIndex;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000BBAFC File Offset: 0x000B9CFC
		private void LateUpdate()
		{
			if (!this.InDecalMode())
			{
				return;
			}
			if (DecalSlot.AnySlotOwned)
			{
				return;
			}
			if (!Mouse.current.leftButton.wasPressedThisFrame)
			{
				return;
			}
			if (UIHelper.IsMouseOverUi)
			{
				return;
			}
			Camera camera = this.GetCamera();
			if (camera == null)
			{
				return;
			}
			Vector3 position = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			Vector2 doorLocalPos = this._door.transform.InverseTransformPoint(position);
			if (!this._door.IsPixelOnDoor(doorLocalPos))
			{
				return;
			}
			PersonalHangar current = PersonalHangar.current;
			if (current == null)
			{
				return;
			}
			current.AddDoorDecal(this._bayIndex, this._doorIndex, doorLocalPos);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000BBBA2 File Offset: 0x000B9DA2
		private Camera GetCamera()
		{
			if (!PersonalHangar.current || !PersonalHangar.current.shipSelect)
			{
				return Camera.main;
			}
			return PersonalHangar.current.shipSelect.CarouselCamera;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000BBBD8 File Offset: 0x000B9DD8
		private bool InDecalMode()
		{
			return SpaceStationInterior.instance != null && PersonalHangar.current != null && PersonalHangar.current.decalModeActive && this._ship == PersonalHangar.current.shipSelect.selectedShip;
		}

		// Token: 0x040012ED RID: 4845
		private SpaceShip _ship;

		// Token: 0x040012EE RID: 4846
		private Door _door;

		// Token: 0x040012EF RID: 4847
		private DroneBayModule _module;

		// Token: 0x040012F0 RID: 4848
		private int _bayIndex;

		// Token: 0x040012F1 RID: 4849
		private int _doorIndex;
	}
}
