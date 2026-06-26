using System;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000011 RID: 17
public static class AsyncOperationExtensions
{
	// Token: 0x06000118 RID: 280 RVA: 0x0000938C File Offset: 0x0000758C
	public static Task AsTask(this AsyncOperation asyncOperation)
	{
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		asyncOperation.completed += delegate(AsyncOperation _)
		{
			tcs.SetResult(true);
		};
		return tcs.Task;
	}
}
