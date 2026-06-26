using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Equipment.Module.DroneBay.OpeningMechanism;
using UnityEngine;

namespace Behaviour.Equipment.Module.DroneBay
{
	// Token: 0x0200036F RID: 879
	public class DoorMechanism : MonoBehaviour
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060021F1 RID: 8689 RVA: 0x000C4D61 File Offset: 0x000C2F61
		public Door[] Doors
		{
			get
			{
				return this.doors;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000C4D69 File Offset: 0x000C2F69
		public int DoorCount
		{
			get
			{
				Door[] array = this.doors;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060021F3 RID: 8691 RVA: 0x000C4D79 File Offset: 0x000C2F79
		// (set) Token: 0x060021F4 RID: 8692 RVA: 0x000C4D81 File Offset: 0x000C2F81
		public bool doorOpen { get; private set; }

		// Token: 0x060021F5 RID: 8693 RVA: 0x000C4D8A File Offset: 0x000C2F8A
		public IEnumerator ToggleDoors(bool open)
		{
			yield return this.ToggleDoorMechanism(open);
			this.doorOpen = open;
			yield break;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000C4DA0 File Offset: 0x000C2FA0
		private IEnumerator ToggleDoorMechanism(bool open)
		{
			if (open == this.doorOpen)
			{
				yield break;
			}
			IEnumerable<Door> enumerable;
			if (!open)
			{
				enumerable = this.doors.Reverse<Door>();
			}
			else
			{
				IEnumerable<Door> enumerable2 = this.doors;
				enumerable = enumerable2;
			}
			IEnumerable<Door> enumerable3 = enumerable;
			using (IEnumerator<Door> enumerator = enumerable3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Door door = enumerator.Current;
					door.ToggleDoor(open, this.speedModifier);
					if (this.activateInSequence)
					{
						yield return new WaitWhile(() => door.isMoving);
					}
				}
			}
			yield break;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000C4DB8 File Offset: 0x000C2FB8
		public bool IsMoving()
		{
			Door[] array = this.doors;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].isMoving)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001403 RID: 5123
		[SerializeField]
		private bool activateInSequence;

		// Token: 0x04001404 RID: 5124
		[SerializeField]
		private Door[] doors;

		// Token: 0x04001405 RID: 5125
		[SerializeField]
		private float speedModifier = 1f;
	}
}
