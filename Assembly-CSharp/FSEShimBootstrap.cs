using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using UnityEngine;

// Token: 0x0200000A RID: 10
public static class FSEShimBootstrap
{
	// Token: 0x0600007D RID: 125
	[DllImport("ntdll.dll", CharSet = CharSet.Ansi, EntryPoint = "wine_get_version")]
	private static extern IntPtr WineGetVersion();

	// Token: 0x0600007E RID: 126 RVA: 0x00004544 File Offset: 0x00002744
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void EnsureDisableFSEAndMaybeRelaunch()
	{
		if (!FSEShimBootstrap.IsRealWindows())
		{
			return;
		}
		try
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (!commandLineArgs.Any((string a) => string.Equals(a, "--fse-shim-applied", StringComparison.Ordinal)))
			{
				string ownExecutablePath = FSEShimBootstrap.GetOwnExecutablePath();
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", false))
				{
					if (string.Equals(((registryKey != null) ? registryKey.GetValue(ownExecutablePath) : null) as string, "~ DISABLEDXMAXIMIZEDWINDOWEDMODE", StringComparison.Ordinal))
					{
						UnityEngine.Debug.Log("FSE Shim Value already set!");
						return;
					}
				}
				using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true))
				{
					registryKey2.SetValue(ownExecutablePath, "~ DISABLEDXMAXIMIZEDWINDOWEDMODE", RegistryValueKind.String);
				}
				UnityEngine.Debug.Log("Added FSE Shim Value, restarting!");
				FSEShimBootstrap.RelaunchSelfWithMarker(commandLineArgs, ownExecutablePath);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.Log("Exception while setting FSE Shim Value!");
			UnityEngine.Debug.LogException(exception);
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00004650 File Offset: 0x00002850
	private static bool IsRealWindows()
	{
		if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
		{
			return false;
		}
		try
		{
			if (FSEShimBootstrap.WineGetVersion() != IntPtr.Zero)
			{
				return false;
			}
		}
		catch
		{
		}
		try
		{
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Wine"))
			{
				if (registryKey != null)
				{
					return false;
				}
			}
		}
		catch
		{
		}
		IDictionary environmentVariables = Environment.GetEnvironmentVariables();
		if (environmentVariables.Contains("SteamGameId"))
		{
			environmentVariables.Contains("STEAM_COMPAT_CLIENT_INSTALL_PATH");
		}
		return true;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00004700 File Offset: 0x00002900
	private static string GetOwnExecutablePath()
	{
		PropertyInfo property = typeof(Environment).GetProperty("ProcessPath", BindingFlags.Static | BindingFlags.Public);
		string text = ((property != null) ? property.GetValue(null) : null) as string;
		if (text != null && !string.IsNullOrEmpty(text))
		{
			return text;
		}
		return Process.GetCurrentProcess().MainModule.FileName;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00004754 File Offset: 0x00002954
	private static void RelaunchSelfWithMarker(string[] args, string exePath)
	{
		IEnumerable<string> source = from a in args.Skip(1)
		where !string.Equals(a, "--fse-shim-applied", StringComparison.Ordinal)
		select a;
		string text = string.Join(" ", source.Select(new Func<string, string>(FSEShimBootstrap.Quote))) + " --fse-shim-applied";
		Process.Start(new ProcessStartInfo(exePath)
		{
			Arguments = text.Trim(),
			WorkingDirectory = Path.GetDirectoryName(exePath),
			UseShellExecute = true
		});
		Application.Quit(0);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000047E8 File Offset: 0x000029E8
	private static string Quote(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return "\"\"";
		}
		if (!s.Contains(" ") && !s.Contains("\""))
		{
			return s;
		}
		return "\"" + s.Replace("\"", "\\\"") + "\"";
	}

	// Token: 0x04000060 RID: 96
	private const string LayersKeyPath = "Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers";

	// Token: 0x04000061 RID: 97
	private const string ShimValue = "~ DISABLEDXMAXIMIZEDWINDOWEDMODE";

	// Token: 0x04000062 RID: 98
	private const string RelaunchMarker = "--fse-shim-applied";
}
