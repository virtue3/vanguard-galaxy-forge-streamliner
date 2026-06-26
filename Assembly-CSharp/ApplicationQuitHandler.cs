using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class ApplicationQuitHandler : MonoBehaviour
{
	// Token: 0x060000BC RID: 188 RVA: 0x00005849 File Offset: 0x00003A49
	private void OnApplicationQuit()
	{
		if (!Application.isEditor)
		{
			Process.GetCurrentProcess().Kill();
		}
	}
}
