using System;
using System.Collections.Generic;
using Behaviour.Unit;

namespace Source.Galaxy
{
	// Token: 0x02000142 RID: 322
	public class FactionShipCollection
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00056BC4 File Offset: 0x00054DC4
		public int minPointsPerUnit
		{
			get
			{
				if (this._minPointsPerUnit == 0)
				{
					this._minPointsPerUnit = (this.GetMinPointsPerUnit(this.ownShips) ?? (this.GetMinPointsPerUnit(this.alliedShips) ?? this.GetMinPointsPerUnit(this.fallbackShips).GetValueOrDefault()));
				}
				return this._minPointsPerUnit;
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00056C38 File Offset: 0x00054E38
		public Behaviour.Unit.SpaceShip GetUnit(int pointsRemaining, int unitsRemaining)
		{
			if (unitsRemaining == 1)
			{
				Behaviour.Unit.SpaceShip result;
				if ((result = this.GetBiggestUnit(this.ownShips, pointsRemaining)) == null)
				{
					result = (this.GetBiggestUnit(this.alliedShips, pointsRemaining) ?? this.GetBiggestUnit(this.fallbackShips, pointsRemaining));
				}
				return result;
			}
			SeededRandom global = SeededRandom.Global;
			List<Behaviour.Unit.SpaceShip> list;
			if ((list = this.FilterShips(this.ownShips, pointsRemaining)) == null)
			{
				list = (this.FilterShips(this.alliedShips, pointsRemaining) ?? this.FilterShips(this.fallbackShips, pointsRemaining));
			}
			return global.Choose<Behaviour.Unit.SpaceShip>(list);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00056CB4 File Offset: 0x00054EB4
		private List<Behaviour.Unit.SpaceShip> FilterShips(List<Behaviour.Unit.SpaceShip> ships, int pointsRemaining)
		{
			this.unitCache.Clear();
			foreach (Behaviour.Unit.SpaceShip spaceShip in ships)
			{
				if (spaceShip.pointValue <= pointsRemaining)
				{
					this.unitCache.Add(spaceShip);
				}
			}
			if (this.unitCache.Count <= 0)
			{
				return null;
			}
			return this.unitCache;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00056D34 File Offset: 0x00054F34
		private Behaviour.Unit.SpaceShip GetBiggestUnit(List<Behaviour.Unit.SpaceShip> list, int points)
		{
			Behaviour.Unit.SpaceShip spaceShip = null;
			foreach (Behaviour.Unit.SpaceShip spaceShip2 in list)
			{
				if (spaceShip2.pointValue <= points && (spaceShip == null || spaceShip2.pointValue > points))
				{
					int pointValue = spaceShip2.pointValue;
					spaceShip = spaceShip2;
				}
			}
			return spaceShip;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00056DA4 File Offset: 0x00054FA4
		private int? GetMinPointsPerUnit(List<Behaviour.Unit.SpaceShip> ships)
		{
			int num = 9999999;
			foreach (Behaviour.Unit.SpaceShip spaceShip in ships)
			{
				num = Math.Min(num, spaceShip.pointValue);
			}
			if (num != 9999999)
			{
				return new int?(num);
			}
			return null;
		}

		// Token: 0x040006C0 RID: 1728
		private const int Max = 9999999;

		// Token: 0x040006C1 RID: 1729
		public List<Behaviour.Unit.SpaceShip> ownShips = new List<Behaviour.Unit.SpaceShip>();

		// Token: 0x040006C2 RID: 1730
		public List<Behaviour.Unit.SpaceShip> alliedShips = new List<Behaviour.Unit.SpaceShip>();

		// Token: 0x040006C3 RID: 1731
		public List<Behaviour.Unit.SpaceShip> fallbackShips = new List<Behaviour.Unit.SpaceShip>();

		// Token: 0x040006C4 RID: 1732
		private List<Behaviour.Unit.SpaceShip> unitCache = new List<Behaviour.Unit.SpaceShip>();

		// Token: 0x040006C5 RID: 1733
		private int _minPointsPerUnit;
	}
}
