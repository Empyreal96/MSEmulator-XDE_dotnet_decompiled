using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.PowerShell
{
	// Token: 0x02000255 RID: 597
	internal static class NativeCultureResolver
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x000A4CFC File Offset: 0x000A2EFC
		internal static CultureInfo UICulture
		{
			get
			{
				if (NativeCultureResolver.m_uiCulture == null)
				{
					lock (NativeCultureResolver.m_syncObject)
					{
						if (NativeCultureResolver.m_uiCulture == null)
						{
							NativeCultureResolver.m_uiCulture = NativeCultureResolver.GetUICulture();
						}
					}
				}
				return (CultureInfo)NativeCultureResolver.m_uiCulture.Clone();
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000A4D5C File Offset: 0x000A2F5C
		internal static CultureInfo Culture
		{
			get
			{
				if (NativeCultureResolver.m_Culture == null)
				{
					lock (NativeCultureResolver.m_syncObject)
					{
						if (NativeCultureResolver.m_Culture == null)
						{
							NativeCultureResolver.m_Culture = NativeCultureResolver.GetCulture();
						}
					}
				}
				return NativeCultureResolver.m_Culture;
			}
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000A4DB4 File Offset: 0x000A2FB4
		internal static CultureInfo GetUICulture()
		{
			return NativeCultureResolver.GetUICulture(true);
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000A4DBC File Offset: 0x000A2FBC
		internal static CultureInfo GetCulture()
		{
			return NativeCultureResolver.GetCulture(true);
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000A4DC4 File Offset: 0x000A2FC4
		internal static CultureInfo GetUICulture(bool filterOutNonConsoleCultures)
		{
			if (!NativeCultureResolver.IsVistaAndLater())
			{
				NativeCultureResolver.m_uiCulture = NativeCultureResolver.EmulateDownLevel();
				return NativeCultureResolver.m_uiCulture;
			}
			string userPreferredUILangs = NativeCultureResolver.GetUserPreferredUILangs(filterOutNonConsoleCultures);
			if (!string.IsNullOrEmpty(userPreferredUILangs))
			{
				try
				{
					string text = userPreferredUILangs;
					char[] separator = new char[1];
					string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					string name = array[0];
					string[] array2 = null;
					if (array.Length > 1)
					{
						array2 = new string[array.Length - 1];
						Array.Copy(array, 1, array2, 0, array.Length - 1);
					}
					NativeCultureResolver.m_uiCulture = new VistaCultureInfo(name, array2);
					return NativeCultureResolver.m_uiCulture;
				}
				catch (ArgumentException)
				{
				}
			}
			NativeCultureResolver.m_uiCulture = NativeCultureResolver.EmulateDownLevel();
			return NativeCultureResolver.m_uiCulture;
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000A4E68 File Offset: 0x000A3068
		internal static CultureInfo GetCulture(bool filterOutNonConsoleCultures)
		{
			CultureInfo cultureInfo;
			try
			{
				if (!NativeCultureResolver.IsVistaAndLater())
				{
					int userDefaultLCID = NativeCultureResolver.GetUserDefaultLCID();
					cultureInfo = new CultureInfo(userDefaultLCID);
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(16);
					if (NativeCultureResolver.GetUserDefaultLocaleName(stringBuilder, 16) == 0)
					{
						cultureInfo = CultureInfo.CurrentCulture;
					}
					else
					{
						cultureInfo = new CultureInfo(stringBuilder.ToString().Trim());
					}
				}
				if (filterOutNonConsoleCultures)
				{
					cultureInfo = CultureInfo.CreateSpecificCulture(cultureInfo.GetConsoleFallbackUICulture().Name);
				}
			}
			catch (ArgumentException)
			{
				cultureInfo = CultureInfo.CurrentCulture;
			}
			return cultureInfo;
		}

		// Token: 0x06001C4D RID: 7245
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern ushort GetUserDefaultUILanguage();

		// Token: 0x06001C4E RID: 7246 RVA: 0x000A4EE8 File Offset: 0x000A30E8
		private static CultureInfo EmulateDownLevel()
		{
			ushort userDefaultUILanguage = NativeCultureResolver.GetUserDefaultUILanguage();
			CultureInfo cultureInfo = new CultureInfo((int)userDefaultUILanguage);
			return cultureInfo.GetConsoleFallbackUICulture();
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000A4F08 File Offset: 0x000A3108
		private static bool IsVistaAndLater()
		{
			return Environment.OSVersion.Version.Major >= 6;
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000A4F20 File Offset: 0x000A3120
		private static string GetUserPreferredUILangs(bool filterOutNonConsoleCultures)
		{
			long num = 0L;
			int num2 = 0;
			string result = "";
			if (filterOutNonConsoleCultures && !NativeCultureResolver.SetThreadPreferredUILanguages(NativeCultureResolver.MUI_CONSOLE_FILTER, null, IntPtr.Zero))
			{
				return result;
			}
			if (!NativeCultureResolver.GetThreadPreferredUILanguages(NativeCultureResolver.MUI_LANGUAGE_NAME | NativeCultureResolver.MUI_MERGE_SYSTEM_FALLBACK | NativeCultureResolver.MUI_MERGE_USER_FALLBACK, out num, null, out num2))
			{
				return result;
			}
			byte[] array = new byte[num2 * 2];
			if (!NativeCultureResolver.GetThreadPreferredUILanguages(NativeCultureResolver.MUI_LANGUAGE_NAME | NativeCultureResolver.MUI_MERGE_SYSTEM_FALLBACK | NativeCultureResolver.MUI_MERGE_USER_FALLBACK, out num, array, out num2))
			{
				return result;
			}
			try
			{
				string @string = Encoding.Unicode.GetString(array);
				result = @string.Trim().ToLowerInvariant();
				return result;
			}
			catch (ArgumentNullException)
			{
			}
			catch (DecoderFallbackException)
			{
			}
			return result;
		}

		// Token: 0x06001C51 RID: 7249
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetUserDefaultLCID();

		// Token: 0x06001C52 RID: 7250
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetUserDefaultLocaleName([MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpLocaleName, int cchLocaleName);

		// Token: 0x06001C53 RID: 7251
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool SetThreadPreferredUILanguages(int dwFlags, StringBuilder pwszLanguagesBuffer, IntPtr pulNumLanguages);

		// Token: 0x06001C54 RID: 7252
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern bool GetThreadPreferredUILanguages(int dwFlags, out long pulNumLanguages, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pwszLanguagesBuffer, out int pcchLanguagesBuffer);

		// Token: 0x06001C55 RID: 7253
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern short SetThreadUILanguage(short langId);

		// Token: 0x04000BAF RID: 2991
		private static CultureInfo m_uiCulture = null;

		// Token: 0x04000BB0 RID: 2992
		private static CultureInfo m_Culture = null;

		// Token: 0x04000BB1 RID: 2993
		private static object m_syncObject = new object();

		// Token: 0x04000BB2 RID: 2994
		private static int MUI_LANGUAGE_NAME = 8;

		// Token: 0x04000BB3 RID: 2995
		private static int MUI_CONSOLE_FILTER = 256;

		// Token: 0x04000BB4 RID: 2996
		private static int MUI_MERGE_USER_FALLBACK = 32;

		// Token: 0x04000BB5 RID: 2997
		private static int MUI_MERGE_SYSTEM_FALLBACK = 16;
	}
}
