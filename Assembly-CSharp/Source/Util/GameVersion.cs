using System;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000034 RID: 52
	public class GameVersion
	{
		// Token: 0x06000290 RID: 656 RVA: 0x00015140 File Offset: 0x00013340
		public static string GetVersion()
		{
			return "v" + Application.version;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00015160 File Offset: 0x00013360
		public static bool IsAtLeast(string version, int major, int minor, int patch = 0, int bugfix = 0)
		{
			int num = GameVersion.SplitVersion(version);
			int versionMask = GameVersion.GetVersionMask(major, minor, patch, bugfix);
			return num >= versionMask;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00015184 File Offset: 0x00013384
		public static bool IsBelow(string version, int major, int minor, int patch = 0, int bugfix = 0)
		{
			int num = GameVersion.SplitVersion(version);
			int versionMask = GameVersion.GetVersionMask(major, minor, patch, bugfix);
			return num < versionMask;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000151A8 File Offset: 0x000133A8
		public static bool IsFuture(string version)
		{
			int num = GameVersion.SplitVersion(Application.version);
			return GameVersion.SplitVersion(version) > num;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000151CC File Offset: 0x000133CC
		private static int SplitVersion(string version)
		{
			string[] array = version.Split('.', StringSplitOptions.None);
			if (array.Length < 2 || array.Length > 4)
			{
				throw new FormatException("GameVersion moet het formaat hebben: X.X.X.X; minimaal 2 en maximaal 4 groepen.");
			}
			int[] array2 = new int[4];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
				if (array2[i] > 99)
				{
					throw new FormatException("Elk cijfer in GameVersion mag max. 2 digits zijn.");
				}
			}
			return GameVersion.GetVersionMask(array2[0], array2[1], array2[2], array2[3]);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001523D File Offset: 0x0001343D
		private static int GetVersionMask(int major, int minor, int patch, int bugfix)
		{
			return major * 1000000 + minor * 10000 + patch * 100 + bugfix;
		}

		// Token: 0x0400016F RID: 367
		public const bool OpenBeta = false;
	}
}
