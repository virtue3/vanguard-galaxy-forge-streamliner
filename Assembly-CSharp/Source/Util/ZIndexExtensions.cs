using System;

namespace Source.Util
{
	// Token: 0x0200004D RID: 77
	public static class ZIndexExtensions
	{
		// Token: 0x0600031C RID: 796 RVA: 0x0001911C File Offset: 0x0001731C
		public static float GetIndex(this ZIndex index)
		{
			float result;
			switch (index)
			{
			case ZIndex.Jumpgate:
				result = -1f;
				break;
			case ZIndex.Projectile:
				result = -0.6f;
				break;
			case ZIndex.Drone:
				result = -0.5f;
				break;
			case ZIndex.Player:
				result = 0f;
				break;
			case ZIndex.Deployable:
				result = 0.3f;
				break;
			case ZIndex.NPC:
				result = 0.5f;
				break;
			case ZIndex.TractorableItem:
				result = 0.6f;
				break;
			case ZIndex.Salvage:
				result = 0.9f;
				break;
			case ZIndex.DockedShip:
				result = 0.95f;
				break;
			case ZIndex.Station:
				result = 1f;
				break;
			case ZIndex.AttachedToAsteroid:
				result = 1.05f;
				break;
			case ZIndex.Asteroid:
				result = 1.1f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}
	}
}
