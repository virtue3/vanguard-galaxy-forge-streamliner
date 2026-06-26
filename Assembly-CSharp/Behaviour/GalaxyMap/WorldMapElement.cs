using System;
using Source.MissionSystem;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000333 RID: 819
	public class WorldMapElement : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x000B8EDD File Offset: 0x000B70DD
		// (set) Token: 0x06001EF9 RID: 7929 RVA: 0x000B8EE5 File Offset: 0x000B70E5
		public SpriteRenderer backgroundRenderer { get; private set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x000B8EEE File Offset: 0x000B70EE
		// (set) Token: 0x06001EFB RID: 7931 RVA: 0x000B8EF6 File Offset: 0x000B70F6
		public Color highlightColor { get; private set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x000B8EFF File Offset: 0x000B70FF
		// (set) Token: 0x06001EFD RID: 7933 RVA: 0x000B8F07 File Offset: 0x000B7107
		public Color playerColor { get; private set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x000B8F10 File Offset: 0x000B7110
		// (set) Token: 0x06001EFF RID: 7935 RVA: 0x000B8F18 File Offset: 0x000B7118
		public float highlightSpeed { get; private set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x000B8F21 File Offset: 0x000B7121
		// (set) Token: 0x06001F01 RID: 7937 RVA: 0x000B8F29 File Offset: 0x000B7129
		public SpriteRenderer missionLocation { get; private set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000B8F32 File Offset: 0x000B7132
		// (set) Token: 0x06001F03 RID: 7939 RVA: 0x000B8F3A File Offset: 0x000B713A
		public SpriteRenderer missionLocation2 { get; private set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000B8F43 File Offset: 0x000B7143
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x000B8F4B File Offset: 0x000B714B
		public SpriteRenderer homebaseLocation { get; private set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x000B8F54 File Offset: 0x000B7154
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x000B8F5C File Offset: 0x000B715C
		public LineRenderer travelLine { get; private set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x000B8F65 File Offset: 0x000B7165
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x000B8F6D File Offset: 0x000B716D
		public FactionIconsHandler factionIcons { get; private set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x000B8F76 File Offset: 0x000B7176
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x000B8F7E File Offset: 0x000B717E
		public bool highlightMouse { get; set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x000B8F87 File Offset: 0x000B7187
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x000B8F8F File Offset: 0x000B718F
		public bool highlightPlayer { get; set; }

		// Token: 0x06001F0E RID: 7950 RVA: 0x000B8F98 File Offset: 0x000B7198
		private void Awake()
		{
			this.originalBackgroundColor = this.backgroundRenderer.color;
			this.originalBackgroundScale = this.backgroundRenderer.transform.localScale;
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x000B8FC4 File Offset: 0x000B71C4
		protected virtual void Update()
		{
			if (AbstractGalaxyMapManager.current.tweening)
			{
				return;
			}
			if (this.activeTravelShip)
			{
				this.activeTravelLine.SetPositions(new Vector3[]
				{
					base.transform.position,
					this.activeTravelShip.transform.position
				});
			}
			else if (this.activeTravelLine)
			{
				UnityEngine.Object.Destroy(this.activeTravelLine.gameObject);
			}
			if (this.highlightMouse || this.highlightPlayer)
			{
				this.backgroundRenderer.color = (this.highlightMouse ? this.highlightColor : this.playerColor);
				float x = this.backgroundRenderer.transform.localScale.x;
				if (x < this.minScale)
				{
					this.directionSmaller = false;
				}
				else if (x > this.minScale + 0.5f)
				{
					this.directionSmaller = true;
				}
				int num = this.directionSmaller ? -1 : 1;
				this.backgroundRenderer.transform.localScale += (float)num * Time.deltaTime * (this.highlightMouse ? this.highlightSpeed : (this.highlightSpeed / 3f)) * Vector3.one;
				return;
			}
			this.backgroundRenderer.color = this.originalBackgroundColor;
			this.backgroundRenderer.transform.localScale = this.originalBackgroundScale;
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x000B9149 File Offset: 0x000B7349
		public void SetMissionMarker(Mission mission)
		{
			if (mission.difficulty != MissionDifficulty.Tutorial && mission.difficulty != MissionDifficulty.Story)
			{
				this.missionLocation.color = ColorHelper.green50;
			}
			this.missionLocation.gameObject.SetActive(true);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x000B917E File Offset: 0x000B737E
		public void SetHomebaseMarker()
		{
			this.homebaseLocation.gameObject.SetActive(true);
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000B9191 File Offset: 0x000B7391
		public void SetAvailableMissionMarker(SpriteRenderer renderer)
		{
			renderer.color = new Color32(102, 216, byte.MaxValue, byte.MaxValue);
			renderer.gameObject.SetActive(true);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x000B91C0 File Offset: 0x000B73C0
		private void OnMouseEnter()
		{
			this.highlightMouse = true;
			if (this.travelLine)
			{
				ShipLocation componentInChildren = base.transform.parent.GetComponentInChildren<ShipLocation>();
				if (componentInChildren)
				{
					this.activeTravelShip = componentInChildren;
					this.activeTravelLine = UnityEngine.Object.Instantiate<LineRenderer>(this.travelLine, base.transform.parent);
				}
			}
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x000B921D File Offset: 0x000B741D
		private void OnMouseExit()
		{
			this.highlightMouse = false;
			if (this.activeTravelLine)
			{
				this.activeTravelShip = null;
				UnityEngine.Object.Destroy(this.activeTravelLine.gameObject);
			}
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x000B924A File Offset: 0x000B744A
		public void Highlight(bool highlight)
		{
			this.highlightMouse = highlight;
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x000B9253 File Offset: 0x000B7453
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.OnMouseEnter();
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000B925B File Offset: 0x000B745B
		public void OnPointerExit(PointerEventData eventData)
		{
			this.OnMouseExit();
		}

		// Token: 0x04001294 RID: 4756
		[SerializeField]
		public SpriteRenderer spriteRenderer;

		// Token: 0x0400129C RID: 4764
		protected Color originalBackgroundColor;

		// Token: 0x0400129D RID: 4765
		protected Vector3 originalBackgroundScale;

		// Token: 0x0400129E RID: 4766
		protected float minScale = 1.5f;

		// Token: 0x0400129F RID: 4767
		private bool directionSmaller;

		// Token: 0x040012A0 RID: 4768
		private LineRenderer activeTravelLine;

		// Token: 0x040012A1 RID: 4769
		private ShipLocation activeTravelShip;
	}
}
