using System;
using Source.Galaxy;
using Source.Simulation.World.System;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000332 RID: 818
	public class WorldMapCombatStation : MonoBehaviour
	{
		// Token: 0x06001EF6 RID: 7926 RVA: 0x000B8E68 File Offset: 0x000B7068
		private void Start()
		{
			MapPointOfInterest content = base.GetComponent<WorldMapPOI>().content;
			bool flag = content.faction.IsEnemy(Faction.player);
			FactionSkirmish factionSkirmish = content.system.storyteller as FactionSkirmish;
			if (((factionSkirmish != null) ? factionSkirmish.helpingFaction : null) != null && factionSkirmish.helpingFaction != content.faction)
			{
				flag = true;
			}
			if (!flag)
			{
				this.sprite.sprite = this.friendlySprite;
			}
		}

		// Token: 0x0400128E RID: 4750
		[SerializeField]
		private SpriteRenderer sprite;

		// Token: 0x0400128F RID: 4751
		[SerializeField]
		private Sprite friendlySprite;
	}
}
