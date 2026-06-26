using System;
using Steamworks;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x0200009C RID: 156
	public class SteamStat
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00036BD2 File Offset: 0x00034DD2
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00036BDA File Offset: 0x00034DDA
		public SteamStatType Stat { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00036BE3 File Offset: 0x00034DE3
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00036BEB File Offset: 0x00034DEB
		public string StatName { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00036BF4 File Offset: 0x00034DF4
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x00036BFC File Offset: 0x00034DFC
		public int[] Tiers { get; private set; }

		// Token: 0x06000669 RID: 1641 RVA: 0x00036C05 File Offset: 0x00034E05
		public SteamStat(SteamStatType type, int[] tiers)
		{
			this.Stat = type;
			this.StatName = Enum.GetName(typeof(SteamStatType), type);
			this.Tiers = tiers;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00036C41 File Offset: 0x00034E41
		public void Add(int count)
		{
			this._addedAmount += count;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00036C51 File Offset: 0x00034E51
		public void Set(int count)
		{
			if (this._savedAmount < count)
			{
				this._setAmount = count;
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00036C64 File Offset: 0x00034E64
		public void Update(float delta)
		{
			this._updateTimer -= delta;
			if (this._updateTimer > 0f)
			{
				return;
			}
			this._updateTimer = 1f;
			if (this._addedAmount == 0 && this._setAmount == 0)
			{
				return;
			}
			if (this._savedAmount == 0 && !SteamUserStats.GetStat(this.StatName, out this._savedAmount))
			{
				return;
			}
			int num = (this._setAmount != 0) ? this._setAmount : (this._savedAmount + this._addedAmount);
			SteamUserStats.SetStat(this.StatName, num);
			bool flag = false;
			for (int i = 0; i < this.Tiers.Length; i++)
			{
				if (this._savedAmount < this.Tiers[i] && num >= this.Tiers[i])
				{
					flag = true;
					break;
				}
			}
			this._savedAmount = num;
			this._addedAmount = 0;
			this._setAmount = 0;
			if (flag)
			{
				Debug.Log("[SteamUserStats] Storing stats data!");
				SteamUserStats.StoreStats();
			}
		}

		// Token: 0x0400036E RID: 878
		public const float UpdateInterval = 1f;

		// Token: 0x04000372 RID: 882
		protected int _savedAmount;

		// Token: 0x04000373 RID: 883
		protected int _addedAmount;

		// Token: 0x04000374 RID: 884
		protected int _setAmount;

		// Token: 0x04000375 RID: 885
		private float _updateTimer = 1f;
	}
}
