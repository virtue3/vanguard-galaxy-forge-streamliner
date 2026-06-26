using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class FieldColorAttribute : PropertyAttribute
{
	// Token: 0x06000034 RID: 52 RVA: 0x00003110 File Offset: 0x00001310
	public FieldColorAttribute(float r, float g, float b, float a = 0.15f, float labelR = -1f, float labelG = -1f, float labelB = -1f)
	{
		this.R = r;
		this.G = g;
		this.B = b;
		this.A = a;
		if (labelR >= 0f)
		{
			this.LabelR = labelR;
			this.LabelG = labelG;
			this.LabelB = labelB;
			this.HasLabelColor = true;
		}
	}

	// Token: 0x04000031 RID: 49
	public float R;

	// Token: 0x04000032 RID: 50
	public float G;

	// Token: 0x04000033 RID: 51
	public float B;

	// Token: 0x04000034 RID: 52
	public float A;

	// Token: 0x04000035 RID: 53
	public float LabelR;

	// Token: 0x04000036 RID: 54
	public float LabelG;

	// Token: 0x04000037 RID: 55
	public float LabelB;

	// Token: 0x04000038 RID: 56
	public bool HasLabelColor;
}
