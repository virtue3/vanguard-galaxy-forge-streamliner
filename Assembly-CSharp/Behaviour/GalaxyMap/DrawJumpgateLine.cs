using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.GalaxyMap
{
	// Token: 0x0200032A RID: 810
	public class DrawJumpgateLine : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x000B6040 File Offset: 0x000B4240
		// (set) Token: 0x06001EA5 RID: 7845 RVA: 0x000B6048 File Offset: 0x000B4248
		public bool mouseInteraction { get; private set; } = true;

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x000B6051 File Offset: 0x000B4251
		// (set) Token: 0x06001EA7 RID: 7847 RVA: 0x000B6059 File Offset: 0x000B4259
		public LineRenderer lineRenderer { get; private set; }

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000B6062 File Offset: 0x000B4262
		private void Awake()
		{
			if (!this.lineRenderer)
			{
				this.SetLineRenderer(ColorHelper.boringGrey);
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000B607C File Offset: 0x000B427C
		public void SetLineRenderer(Color startColor)
		{
			this.lineRenderer = (base.GetComponent<LineRenderer>() ?? base.gameObject.AddComponent<LineRenderer>());
			this.lineRenderer.startColor = startColor;
			this.lineRenderer.endColor = Color.black;
			this.lineRenderer.positionCount = 2;
			this.lineRenderer.sortingOrder = -1;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000B60D8 File Offset: 0x000B42D8
		public void SetMouseInteraction(bool interaction)
		{
			this.mouseInteraction = interaction;
			if (!interaction)
			{
				UnityEngine.Object.Destroy(this.tooltipSource);
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000B60F0 File Offset: 0x000B42F0
		public void SetPositions(Vector2 objA, Vector2 objB, Color startColor, GalaxyMapCameraMovement cameraMovement, float thickness)
		{
			if (cameraMovement == null)
			{
				this.mouseInteraction = false;
			}
			if (!this.lineRenderer)
			{
				this.SetLineRenderer(startColor);
			}
			this.lineRenderer.startColor = startColor;
			this.objectA = objA;
			this.jumpToA = objA;
			this.objectB = objB;
			this.jumpToB = objB;
			this.lineRenderer.startWidth = thickness;
			this.lineRenderer.endWidth = thickness;
			Vector2 vector = this.objectA;
			Vector2 vector2 = this.objectB;
			this.lineRenderer.SetPosition(0, new Vector3(this.objectA.x, this.objectA.y, -10f));
			this.lineRenderer.SetPosition(1, new Vector3(this.objectB.x, this.objectB.y, -10f));
			if (this.mouseInteraction)
			{
				this.UpdateCollider();
			}
			else
			{
				UnityEngine.Object.Destroy(this.polyCollider);
			}
			this.cameraMovement = cameraMovement;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000B61F4 File Offset: 0x000B43F4
		private void UpdateCollider()
		{
			Mesh mesh = new Mesh();
			this.lineRenderer.BakeMesh(mesh, true);
			Vector3[] vertices = mesh.vertices;
			Vector2[] array = new Vector2[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = new Vector2(vertices[i].x, vertices[i].y);
			}
			this.polyCollider.pathCount = 1;
			this.polyCollider.SetPath(0, array);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000B626F File Offset: 0x000B446F
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.mouseInteraction)
			{
				return;
			}
			if (this.IsBCloser())
			{
				this.cameraMovement.JumpTo(this.jumpToA);
				return;
			}
			this.cameraMovement.JumpTo(this.jumpToB);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000B62A8 File Offset: 0x000B44A8
		private bool IsBCloser()
		{
			float num = Vector2.Distance(this.cameraMovement.transform.position, this.jumpToA);
			float num2 = Vector2.Distance(this.cameraMovement.transform.position, this.jumpToB);
			return num > num2;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000B62FC File Offset: 0x000B44FC
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.mouseInteraction)
			{
				return;
			}
			this.lineRenderer.startColor = this.ChangeColorAlpha(this.lineRenderer.startColor, 0.3f);
			this.lineRenderer.endColor = this.ChangeColorAlpha(this.lineRenderer.endColor, 0.3f);
			if (this.IsBCloser())
			{
				this.tooltipSource.BodyText = this.nameB + " >> " + this.nameA;
				return;
			}
			this.tooltipSource.BodyText = this.nameA + " >> " + this.nameB;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000B639F File Offset: 0x000B459F
		private Color ChangeColorAlpha(Color color, float alpha)
		{
			color.a = alpha;
			return color;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000B63AC File Offset: 0x000B45AC
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this.mouseInteraction)
			{
				return;
			}
			this.lineRenderer.startColor = this.ChangeColorAlpha(this.lineRenderer.startColor, 1f);
			this.lineRenderer.endColor = this.ChangeColorAlpha(this.lineRenderer.endColor, 1f);
		}

		// Token: 0x04001266 RID: 4710
		public Vector2 objectA;

		// Token: 0x04001267 RID: 4711
		public Vector2 objectB;

		// Token: 0x04001268 RID: 4712
		public Vector2 jumpToA;

		// Token: 0x04001269 RID: 4713
		public Vector2 jumpToB;

		// Token: 0x0400126A RID: 4714
		public string nameA;

		// Token: 0x0400126B RID: 4715
		public string nameB;

		// Token: 0x0400126E RID: 4718
		[SerializeField]
		private PolygonCollider2D polyCollider;

		// Token: 0x0400126F RID: 4719
		[SerializeField]
		private TooltipSource tooltipSource;

		// Token: 0x04001270 RID: 4720
		private GalaxyMapCameraMovement cameraMovement;
	}
}
