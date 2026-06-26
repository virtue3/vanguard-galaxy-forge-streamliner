using System;
using System.Collections.Generic;
using Source.Player;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A3 RID: 931
	public class CrewIcons : MonoBehaviour
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x000C9CE4 File Offset: 0x000C7EE4
		private void Awake()
		{
			CrewIcons.instance = this;
			foreach (Sprite sprite in this.maleIcons)
			{
				this.icons[sprite.name] = new CrewIcon(sprite, true);
			}
			foreach (Sprite sprite2 in this.femaleIcons)
			{
				this.icons[sprite2.name] = new CrewIcon(sprite2, false);
			}
			foreach (Sprite sprite3 in this.hiddenIcons)
			{
				this.icons[sprite3.name] = new CrewIcon(sprite3, false)
				{
					hidden = true
				};
			}
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000C9DFC File Offset: 0x000C7FFC
		public static CrewIcon Get(string id)
		{
			return CrewIcons.instance.icons[id];
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000C9E10 File Offset: 0x000C8010
		public static CrewIcon GetRandom(bool male, SeededRandom random = null)
		{
			List<CrewIcon> list = new List<CrewIcon>();
			foreach (CrewIcon crewIcon in CrewIcons.instance.icons.Values)
			{
				if (!crewIcon.hidden && crewIcon.isMale == male && GamePlayer.current.commander.icon != crewIcon)
				{
					list.Add(crewIcon);
				}
			}
			return (random ?? SeededRandom.Global).Choose<CrewIcon>(list);
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000C9EA8 File Offset: 0x000C80A8
		public static List<CrewIcon> GetAll()
		{
			List<CrewIcon> list = new List<CrewIcon>();
			List<CrewIcon> list2 = new List<CrewIcon>();
			foreach (CrewIcon crewIcon in CrewIcons.instance.icons.Values)
			{
				if (!crewIcon.hidden)
				{
					(crewIcon.isMale ? list : list2).Add(crewIcon);
				}
			}
			SeededRandom.Global.Shuffle<CrewIcon>(list);
			SeededRandom.Global.Shuffle<CrewIcon>(list2);
			List<CrewIcon> list3 = new List<CrewIcon>();
			int count = list.Count;
			if (list.Count != list2.Count)
			{
				Debug.LogWarning("Aantal male en female crew icons moet hetzelfde zijn!");
			}
			for (int i = 0; i < list.Count; i++)
			{
				list3.Add(list[i]);
				list3.Add(list2[i]);
			}
			return list3;
		}

		// Token: 0x04001515 RID: 5397
		private static CrewIcons instance;

		// Token: 0x04001516 RID: 5398
		[SerializeField]
		private List<Sprite> maleIcons;

		// Token: 0x04001517 RID: 5399
		[SerializeField]
		private List<Sprite> femaleIcons;

		// Token: 0x04001518 RID: 5400
		[SerializeField]
		private List<Sprite> hiddenIcons;

		// Token: 0x04001519 RID: 5401
		private Dictionary<string, CrewIcon> icons = new Dictionary<string, CrewIcon>();
	}
}
