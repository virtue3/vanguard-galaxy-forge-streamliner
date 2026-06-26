using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000049 RID: 73
	public class Translation
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000186B6 File Offset: 0x000168B6
		public static Translation Current
		{
			get
			{
				return Translation._current;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000302 RID: 770 RVA: 0x000186BD File Offset: 0x000168BD
		public static string CurrentLocale
		{
			get
			{
				return Translation._current.Locale;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000186C9 File Offset: 0x000168C9
		public static IEnumerable<Translation> All
		{
			get
			{
				return Translation._languages.Values;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000304 RID: 772 RVA: 0x000186D5 File Offset: 0x000168D5
		// (set) Token: 0x06000305 RID: 773 RVA: 0x000186DD File Offset: 0x000168DD
		public string Locale { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000306 RID: 774 RVA: 0x000186E6 File Offset: 0x000168E6
		// (set) Token: 0x06000307 RID: 775 RVA: 0x000186EE File Offset: 0x000168EE
		public string DisplayName { get; private set; }

		// Token: 0x06000308 RID: 776 RVA: 0x000186F8 File Offset: 0x000168F8
		public Translation(TextReader reader)
		{
			try
			{
				string input;
				while ((input = reader.ReadLine()) != null)
				{
					if (!Translation._comment.IsMatch(input))
					{
						Match match = Translation._line.Match(input);
						if (match.Success)
						{
							this._lines[match.Groups[1].Value] = match.Groups[2].Value.Trim();
						}
					}
				}
			}
			finally
			{
				if (reader != null)
				{
					((IDisposable)reader).Dispose();
				}
			}
			this.Locale = this._lines["locale"];
			this.DisplayName = this._lines["display"];
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000187C0 File Offset: 0x000169C0
		public string Get(string key)
		{
			string result;
			this._lines.TryGetValue(key, out result);
			return result;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000187DD File Offset: 0x000169DD
		public void Apply()
		{
			Translation._current = this;
			Thread.CurrentThread.CurrentCulture = new CultureInfo(this.Locale);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.Locale);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00018810 File Offset: 0x00016A10
		public static void Init()
		{
			if (Translation._default != null)
			{
				return;
			}
			Translation._languages = new Dictionary<string, Translation>();
			TextAsset[] array = Resources.LoadAll<TextAsset>("Language/");
			for (int i = 0; i < array.Length; i++)
			{
				Translation translation = new Translation(new StringReader(array[i].text));
				Translation._languages[translation.Locale] = translation;
			}
			Translation._default = Translation._languages["en-US"];
			FileInfo fileInfo = new FileInfo("language.ini");
			string key;
			if (fileInfo.Exists)
			{
				Translation._languages["override"] = new Translation(fileInfo.OpenText());
				key = "override";
			}
			else
			{
				string @string = PlayerPrefs.GetString("Locale");
				if (string.IsNullOrEmpty(@string))
				{
					string text = Thread.CurrentThread.CurrentCulture.ToString();
					if (Translation._languages.ContainsKey(text))
					{
						key = text;
					}
					else
					{
						key = "en-US";
					}
				}
				else
				{
					key = @string;
				}
			}
			Translation._languages[key].Apply();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0001890C File Offset: 0x00016B0C
		public static void Clear()
		{
			Translation._current = null;
			Translation._default = null;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0001891A File Offset: 0x00016B1A
		public static void UpdateLocale(Translation locale)
		{
			Translation.Init();
			PlayerPrefs.SetString("Locale", locale.Locale);
			locale.Apply();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00018938 File Offset: 0x00016B38
		public static string TranslateOnly(string text, params object[] values)
		{
			Translation.Init();
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (text[0] == '@')
			{
				string text2 = text.Substring(1);
				Translation current = Translation._current;
				text = ((current != null) ? current.Get(text2) : null);
				if (text == null)
				{
					Translation @default = Translation._default;
					text = ((@default != null) ? @default.Get(text2) : null);
					if (text == null)
					{
						text = "@" + text2;
					}
				}
			}
			if (values != null && values.Length != 0)
			{
				for (int i = 0; i < values.Length; i++)
				{
					string text3 = values[i] as string;
					if (text3 != null && text3.Length > 0 && text3[0] == '@')
					{
						values[i] = Translation.Translate(text3, Array.Empty<object>());
					}
					else
					{
						object obj = values[i];
						if (obj is float)
						{
							float num = (float)obj;
							values[i] = GameMath.FormatNumber(num, -1);
						}
						else
						{
							obj = values[i];
							if (obj is int)
							{
								int num2 = (int)obj;
								values[i] = GameMath.FormatNumber((float)num2, -1);
							}
							else
							{
								obj = values[i];
								if (obj is long)
								{
									long num3 = (long)obj;
									values[i] = GameMath.FormatNumber((float)num3, -1);
								}
							}
						}
					}
				}
				text = string.Format(text, values);
			}
			return text;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00018A67 File Offset: 0x00016C67
		public static string Translate(string text, params object[] values)
		{
			return Regex.Replace(Translation.TranslateOnly(text, values), "#(.*?)#", Translation._highlightPlaceholder);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00018A7F File Offset: 0x00016C7F
		public static string Highlight(string text, Color c, params object[] values)
		{
			return Regex.Replace(Translation.TranslateOnly(text, values), "#(.*?)#", "<color=#" + ColorUtility.ToHtmlStringRGB(c) + ">$1</color>");
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00018AA8 File Offset: 0x00016CA8
		public static string Plural(string v, int count)
		{
			string text = Translation.TranslateOnly(v, Array.Empty<object>());
			if (count == 1 || text.EndsWith("s"))
			{
				return text;
			}
			return text + "s";
		}

		// Token: 0x04000191 RID: 401
		public const string DefaultLanguage = "en-US";

		// Token: 0x04000192 RID: 402
		public const bool TestMode = false;

		// Token: 0x04000193 RID: 403
		private const string OverrideLanguageFile = "language.ini";

		// Token: 0x04000194 RID: 404
		private static Translation _default;

		// Token: 0x04000195 RID: 405
		private static Translation _current;

		// Token: 0x04000196 RID: 406
		private static Regex _comment = new Regex("^\\s*;");

		// Token: 0x04000197 RID: 407
		private static Regex _line = new Regex("^\\s*(.+?)\\s*=(.*)");

		// Token: 0x04000198 RID: 408
		private static string _highlightPlaceholder = "<color=#FFD100>$1</color>";

		// Token: 0x04000199 RID: 409
		public static Color highlightColor = new Color32(byte.MaxValue, 209, 0, byte.MaxValue);

		// Token: 0x0400019A RID: 410
		private static Dictionary<string, Translation> _languages;

		// Token: 0x0400019B RID: 411
		private Dictionary<string, string> _lines = new Dictionary<string, string>();
	}
}
