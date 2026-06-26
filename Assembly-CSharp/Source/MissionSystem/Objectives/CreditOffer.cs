using System;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C5 RID: 197
	public class CreditOffer : MissionObjective
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0003E8F5 File Offset: 0x0003CAF5
		public int currentAmount
		{
			get
			{
				return Mathf.RoundToInt((float)GamePlayer.current.credits);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0003E907 File Offset: 0x0003CB07
		public override string statusText
		{
			get
			{
				return Translation.TranslateOnly("@MissionObjectiveCreditOffer", new object[]
				{
					this.requiredAmount,
					this.deliverTo.name
				});
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0003E935 File Offset: 0x0003CB35
		public override bool IsComplete()
		{
			return GamePlayer.current != null && this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0003E951 File Offset: 0x0003CB51
		public override void OnMissionTurnedIn()
		{
			GamePlayer.current.RemoveCredits((float)this.requiredAmount);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0003E964 File Offset: 0x0003CB64
		public override MapPointOfInterest GetPoi()
		{
			return this.deliverTo;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0003E96C File Offset: 0x0003CB6C
		protected override void DataToJson(JsonObject data)
		{
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			data["deliverTo"] = this.deliverTo.guid;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0003E9A5 File Offset: 0x0003CBA5
		protected override void LoadFromJson(JsonObject data)
		{
			this.requiredAmount = data["requiredAmount"];
			GalaxyMapData.current.LoadPointOfInterest(data["deliverTo"], delegate(MapPointOfInterest poi)
			{
				this.deliverTo = (poi as SpaceStation);
			});
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0003E9E3 File Offset: 0x0003CBE3
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x0400045B RID: 1115
		public int requiredAmount;

		// Token: 0x0400045C RID: 1116
		public SpaceStation deliverTo;
	}
}
