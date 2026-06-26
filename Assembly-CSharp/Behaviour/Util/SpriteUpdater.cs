using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B5 RID: 437
	public class SpriteUpdater : Singleton<SpriteUpdater>
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x0006A23C File Offset: 0x0006843C
		public void Reset()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this.threadActive = false;
				if (this.updateThread != null)
				{
					this.updateThread.Interrupt();
					this.updateThread = null;
				}
				this.sprites.Clear();
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0006A2A4 File Offset: 0x000684A4
		private void Update()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				for (int i = 0; i < this.sprites.Count; i++)
				{
					bool flag2;
					try
					{
						flag2 = this.sprites[i].Update();
					}
					catch (Exception ex)
					{
						string str = "i: ";
						string str2 = i.ToString();
						string str3 = " -- Exception: ";
						Exception ex2 = ex;
						Debug.LogWarning(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
						flag2 = true;
					}
					if (flag2)
					{
						BreakDelayedSprite breakDelayedSprite = this.sprites[i];
						this.sprites.RemoveAt(i);
						breakDelayedSprite.OnComplete();
						i--;
					}
				}
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0006A368 File Offset: 0x00068568
		public void AddSprite(BreakDelayedSprite sprite)
		{
			sprite.Initialize();
			object @lock = this._lock;
			lock (@lock)
			{
				this.sprites.Add(sprite);
				if (this.updateThread == null || !this.updateThread.IsAlive)
				{
					Debug.Log("Add Sprite, start new: " + this.sprites.Count.ToString());
					this.threadActive = true;
					this.updateThread = new Thread(new ThreadStart(this.ListenForUpdates));
					this.updateThread.IsBackground = true;
					this.updateThread.Start();
				}
				else
				{
					this.updateThread.Interrupt();
				}
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0006A430 File Offset: 0x00068630
		private void ListenForUpdates()
		{
			while (this.threadActive)
			{
				try
				{
					object @lock = this._lock;
					BreakDelayedSprite[] array;
					lock (@lock)
					{
						array = this.sprites.ToArray();
					}
					foreach (BreakDelayedSprite breakDelayedSprite in array)
					{
						if (!breakDelayedSprite.done)
						{
							try
							{
								breakDelayedSprite.RunJob();
							}
							catch (Exception ex)
							{
								breakDelayedSprite.done = true;
								string str = "Exception in RunJob: ";
								Exception ex2 = ex;
								Debug.LogWarning(str + ((ex2 != null) ? ex2.ToString() : null));
							}
						}
					}
					Thread.Sleep(1000);
				}
				catch (ThreadInterruptedException)
				{
				}
			}
		}

		// Token: 0x040008A5 RID: 2213
		private List<BreakDelayedSprite> sprites = new List<BreakDelayedSprite>();

		// Token: 0x040008A6 RID: 2214
		private readonly object _lock = new object();

		// Token: 0x040008A7 RID: 2215
		private Thread updateThread;

		// Token: 0x040008A8 RID: 2216
		private bool threadActive = true;
	}
}
