using System;
using System.Collections.Generic;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station;
using Source.Galaxy.POI.Station.Patrons;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Bar
{
	// Token: 0x0200023A RID: 570
	public class BarUI : MonoBehaviour
	{
		// Token: 0x0600154B RID: 5451 RVA: 0x000892F7 File Offset: 0x000874F7
		private void Start()
		{
			this.RefreshPatrons();
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00089300 File Offset: 0x00087500
		public void RefreshPatrons()
		{
			this.HideCrewInfo();
			Bar bar = SpaceStation.current.bar;
			this.buttonParent.DestroyChildren();
			bar.CheckUpdatePatrons(false);
			foreach (BarPatron barPatron in bar.availablePatrons)
			{
				barPatron.Initialize();
				List<BarPatronSprite> list = new List<BarPatronSprite>();
				foreach (BarPatronSprite barPatronSprite in this.patronSprites)
				{
					if (barPatronSprite.seatIndex == barPatron.seat && barPatronSprite.isMale == barPatron.isMale)
					{
						list.Add(barPatronSprite);
					}
				}
				if (list.Count > 0)
				{
					BarPatronImage barPatronImage = UnityEngine.Object.Instantiate<BarPatronImage>(this.patronPrefab, this.buttonParent);
					barPatronImage.SetPatronData(barPatron);
					barPatronImage.SetPatronSprite(new SeedGenerator().Add(barPatron.seed).CreateRandom().Choose<BarPatronSprite>(list), this.barBackground);
				}
			}
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00089428 File Offset: 0x00087628
		public void ShowCrewInfo(CrewMember patron)
		{
			this.crewInfo.Show(patron);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00089436 File Offset: 0x00087636
		public void HideCrewInfo()
		{
			this.crewInfo.Hide();
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00089443 File Offset: 0x00087643
		public void ShowSalesmanInfo(Salesman salesman)
		{
			this.salesmanInfo = UnityEngine.Object.Instantiate<ItemSaleInfo>(this.salesmanInfoPrefab, base.transform);
			this.salesmanInfo.Show(salesman);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00089468 File Offset: 0x00087668
		public void HideSalesmanInfo()
		{
			this.salesmanInfo.Destroy();
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00089475 File Offset: 0x00087675
		public void PatronClicked(BarPatron patron)
		{
			patron.InteractWithPatron(this);
		}

		// Token: 0x04000C97 RID: 3223
		[SerializeField]
		private RectTransform buttonParent;

		// Token: 0x04000C98 RID: 3224
		[SerializeField]
		private CrewMemberInfo crewInfo;

		// Token: 0x04000C99 RID: 3225
		[SerializeField]
		private ItemSaleInfo salesmanInfo;

		// Token: 0x04000C9A RID: 3226
		[SerializeField]
		private Image barBackground;

		// Token: 0x04000C9B RID: 3227
		[SerializeField]
		private BarPatronImage patronPrefab;

		// Token: 0x04000C9C RID: 3228
		[SerializeField]
		private List<BarPatronSprite> patronSprites;

		// Token: 0x04000C9D RID: 3229
		[SerializeField]
		private ItemSaleInfo salesmanInfoPrefab;
	}
}
