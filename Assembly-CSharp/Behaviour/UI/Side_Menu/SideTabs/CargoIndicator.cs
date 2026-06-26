using System;
using Source.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B3 RID: 691
	public class CargoIndicator : MonoBehaviour
	{
		// Token: 0x06001996 RID: 6550 RVA: 0x0009F6B9 File Offset: 0x0009D8B9
		private void Start()
		{
			base.InvokeRepeating("UpdateCargoIndicator", 0.5f, 0.5f);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0009F6D0 File Offset: 0x0009D8D0
		private void UpdateCargoIndicator()
		{
			if (GamePlayer.current != null)
			{
				GameplayManager instance = GameplayManager.Instance;
				if ((instance != null) ? instance.spaceShip : null)
				{
					float cargoUsed = GamePlayer.current.currentSpaceShip.cargoUsed;
					float cargoCapacity = GamePlayer.current.currentSpaceShip.cargoCapacity;
					float num = cargoUsed / cargoCapacity;
					this.cargoIndicatorImage.fillAmount = Mathf.Clamp01(num);
					this.SetCargoIndicatorColor(num);
					return;
				}
			}
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0009F738 File Offset: 0x0009D938
		private void SetCargoIndicatorColor(float availableSpacePercentage)
		{
			Color color;
			if (availableSpacePercentage < 0.7f)
			{
				color = new Color(0.2593894f, 0.8207547f, 0.2593894f);
			}
			else if (availableSpacePercentage < 1f)
			{
				color = new Color(0.8207547f, 0.6254426f, 0.2593894f);
			}
			else
			{
				color = new Color(0.8207547f, 0.2593894f, 0.2593894f);
			}
			this.cargoIndicatorImage.color = color;
		}

		// Token: 0x0400100E RID: 4110
		[SerializeField]
		private Image cargoIndicatorImage;
	}
}
