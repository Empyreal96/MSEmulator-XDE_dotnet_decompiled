using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Microsoft.VisualStudio.Deployment.Internal
{
	// Token: 0x02000002 RID: 2
	internal static class TokenExpander
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		internal static string ExpandSpecialPaths(string originalPath)
		{
			Dictionary<string, TokenExpander.ShellFolderEntry> dictionary = new Dictionary<string, TokenExpander.ShellFolderEntry>
			{
				{
					"CSIDL_APPDATA",
					new TokenExpander.ShellFolderEntry("CSIDL_APPDATA", Environment.SpecialFolder.ApplicationData, "\\Application Data")
				},
				{
					"CSIDL_COMMON_APPDATA",
					new TokenExpander.ShellFolderEntry("CSIDL_COMMON_APPDATA", Environment.SpecialFolder.CommonApplicationData, "\\")
				},
				{
					"CSIDL_DESKTOPDIRECTORY",
					new TokenExpander.ShellFolderEntry("CSIDL_DESKTOPDIRECTORY", Environment.SpecialFolder.DesktopDirectory, "\\Windows\\Desktop")
				},
				{
					"CSIDL_FAVORITES",
					new TokenExpander.ShellFolderEntry("CSIDL_FAVORITES", Environment.SpecialFolder.Favorites, "\\Windows\\Favorites")
				},
				{
					"CSIDL_FONTS",
					new TokenExpander.ShellFolderEntry("CSIDL_FONTS", Environment.SpecialFolder.Fonts, "\\Windows\\Fonts")
				},
				{
					"CSIDL_PERSONAL",
					new TokenExpander.ShellFolderEntry("CSIDL_PERSONAL", Environment.SpecialFolder.Personal, "\\")
				},
				{
					"CSIDL_PROFILE",
					new TokenExpander.ShellFolderEntry("CSIDL_PROFILE", Environment.SpecialFolder.UserProfile, "\\Profiles\\Default")
				},
				{
					"CSIDL_PROGRAM_FILES",
					new TokenExpander.ShellFolderEntry("CSIDL_PROGRAM_FILES", Environment.SpecialFolder.ProgramFiles, "\\Program Files")
				},
				{
					"CSIDL_PROGRAMS",
					new TokenExpander.ShellFolderEntry("CSIDL_PROGRAMS", Environment.SpecialFolder.Programs, "\\Windows\\Programs")
				},
				{
					"CSIDL_STARTMENU",
					new TokenExpander.ShellFolderEntry("CSIDL_STARTMENU", Environment.SpecialFolder.StartMenu, "\\")
				},
				{
					"CSIDL_STARTUP",
					new TokenExpander.ShellFolderEntry("CSIDL_STARTUP", Environment.SpecialFolder.Startup, "\\Windows\\Startup")
				},
				{
					"CSIDL_WINDOWS",
					new TokenExpander.ShellFolderEntry("CSIDL_WINDOWS", Environment.SpecialFolder.Windows, "\\Windows")
				},
				{
					"CSIDL_LOCAL_APPDATA",
					new TokenExpander.ShellFolderEntry("CSIDL_LOCAL_APPDATA", Environment.SpecialFolder.LocalApplicationData, "\\")
				},
				{
					"CSIDL_PROGRAM_FILESX86",
					new TokenExpander.ShellFolderEntry("CSIDL_PROGRAM_FILESX86", Environment.SpecialFolder.ProgramFilesX86, "\\")
				}
			};
			string text = "";
			if (string.IsNullOrEmpty(originalPath))
			{
				return text;
			}
			for (;;)
			{
				string text2;
				int specialFolderToken = TokenExpander.GetSpecialFolderToken(originalPath, out text2);
				if (specialFolderToken < 0)
				{
					break;
				}
				TokenExpander.ShellFolderEntry shellFolderEntry;
				if (dictionary.TryGetValue(text2, out shellFolderEntry))
				{
					string str = shellFolderEntry.Default;
					try
					{
						str = Environment.GetFolderPath(shellFolderEntry.CsIdl);
					}
					catch
					{
					}
					text += str;
				}
				else if (text2.Equals("FOLDERID_Windows10SDK", StringComparison.OrdinalIgnoreCase))
				{
					if (TokenExpander.WinsdkRoot == null)
					{
						TokenExpander.WinsdkRoot = (Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows Kits\\Installed Roots", "KitsRoot10", null) as string);
						if (!string.IsNullOrEmpty(TokenExpander.WinsdkRoot) || TokenExpander.WinsdkRoot.EndsWith("\\"))
						{
							TokenExpander.WinsdkRoot = TokenExpander.WinsdkRoot.Substring(0, TokenExpander.WinsdkRoot.Length - 1);
						}
					}
					if (!string.IsNullOrEmpty(TokenExpander.WinsdkRoot))
					{
						text += TokenExpander.WinsdkRoot;
					}
					else
					{
						text = text + "%" + text2 + "%";
					}
				}
				else
				{
					text = text + "%" + text2 + "%";
				}
				originalPath = originalPath.Substring(specialFolderToken);
			}
			text += originalPath;
			return text;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002308 File Offset: 0x00000508
		private static int GetSpecialFolderToken(string originalPath, out string token)
		{
			token = null;
			int num = originalPath.IndexOf('%');
			if (num >= 0)
			{
				int num2 = originalPath.IndexOf('%', num + 1);
				if (num2 > num)
				{
					token = originalPath.Substring(num + 1, num2 - num - 1);
					return num2 + 1;
				}
			}
			return -1;
		}

		// Token: 0x04000001 RID: 1
		private const string Windows10SDKToken = "FOLDERID_Windows10SDK";

		// Token: 0x04000002 RID: 2
		private static string WinsdkRoot;

		// Token: 0x02000014 RID: 20
		internal struct ShellFolderEntry
		{
			// Token: 0x06000164 RID: 356 RVA: 0x00004E78 File Offset: 0x00003078
			public ShellFolderEntry(string csIdlString, Environment.SpecialFolder csIdl, string defaultPath)
			{
				this.CsIdlString = csIdlString;
				this.CsIdl = csIdl;
				this.Default = defaultPath;
			}

			// Token: 0x04000053 RID: 83
			public string CsIdlString;

			// Token: 0x04000054 RID: 84
			public Environment.SpecialFolder CsIdl;

			// Token: 0x04000055 RID: 85
			public string Default;
		}
	}
}
