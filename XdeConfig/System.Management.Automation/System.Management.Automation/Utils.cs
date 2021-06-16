using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x02000430 RID: 1072
	internal static class Utils
	{
		// Token: 0x06002F3F RID: 12095 RVA: 0x001032CE File Offset: 0x001014CE
		internal static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x001032D7 File Offset: 0x001014D7
		internal static int CombineHashCodes(int h1, int h2, int h3)
		{
			return Utils.CombineHashCodes(Utils.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x001032E6 File Offset: 0x001014E6
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return Utils.CombineHashCodes(Utils.CombineHashCodes(h1, h2), Utils.CombineHashCodes(h3, h4));
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x001032FB File Offset: 0x001014FB
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return Utils.CombineHashCodes(Utils.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x0010330D File Offset: 0x0010150D
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return Utils.CombineHashCodes(Utils.CombineHashCodes(h1, h2, h3, h4), Utils.CombineHashCodes(h5, h6));
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x00103326 File Offset: 0x00101526
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return Utils.CombineHashCodes(Utils.CombineHashCodes(h1, h2, h3, h4), Utils.CombineHashCodes(h5, h6, h7));
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x00103341 File Offset: 0x00101541
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return Utils.CombineHashCodes(Utils.CombineHashCodes(h1, h2, h3, h4), Utils.CombineHashCodes(h5, h6, h7, h8));
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x00103360 File Offset: 0x00101560
		internal static void CheckKeyArg(byte[] arg, string argName)
		{
			if (arg == null)
			{
				throw PSTraceSource.NewArgumentNullException(argName);
			}
			if (arg.Length != 16 && arg.Length != 24 && arg.Length != 32)
			{
				throw PSTraceSource.NewArgumentException(argName, Serialization.InvalidKeyLength, new object[]
				{
					argName
				});
			}
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x001033A4 File Offset: 0x001015A4
		internal static void CheckArgForNullOrEmpty(string arg, string argName)
		{
			if (arg == null)
			{
				throw PSTraceSource.NewArgumentNullException(argName);
			}
			if (arg.Length == 0)
			{
				throw PSTraceSource.NewArgumentException(argName);
			}
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x001033BF File Offset: 0x001015BF
		internal static void CheckArgForNull(object arg, string argName)
		{
			if (arg == null)
			{
				throw PSTraceSource.NewArgumentNullException(argName);
			}
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x001033CB File Offset: 0x001015CB
		internal static void CheckSecureStringArg(SecureString arg, string argName)
		{
			if (arg == null)
			{
				throw PSTraceSource.NewArgumentNullException(argName);
			}
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x001033D8 File Offset: 0x001015D8
		[ArchitectureSensitive]
		internal static string GetStringFromSecureString(SecureString ss)
		{
			IntPtr intPtr = IntPtr.Zero;
			string result = null;
			try
			{
				intPtr = ClrFacade.SecureStringToCoTaskMemUnicode(ss);
				result = Marshal.PtrToStringUni(intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					ClrFacade.ZeroFreeCoTaskMemUnicode(intPtr);
				}
			}
			return result;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x00103424 File Offset: 0x00101624
		internal static TypeTable GetTypeTableFromExecutionContextTLS()
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				return null;
			}
			return executionContextFromTLS.TypeTable;
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x00103444 File Offset: 0x00101644
		internal static string GetApplicationBase(string shellId)
		{
			bool flag = shellId == Utils.DefaultPowerShellShellID;
			if (flag && Utils._pshome != null)
			{
				return Utils._pshome;
			}
			string name = "Software\\Microsoft\\PowerShell\\" + PSVersionInfo.RegistryVersionKey + "\\PowerShellEngine";
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name))
			{
				if (registryKey != null)
				{
					string text = registryKey.GetValue("ApplicationBase") as string;
					if (flag)
					{
						Interlocked.CompareExchange<string>(ref Utils._pshome, null, text);
					}
					return text;
				}
			}
			Assembly assembly = Assembly.GetEntryAssembly();
			if (assembly != null)
			{
				return Path.GetDirectoryName(assembly.Location);
			}
			assembly = typeof(PSObject).GetTypeInfo().Assembly;
			string value = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.Net\\assembly");
			if (!assembly.Location.StartsWith(value, StringComparison.OrdinalIgnoreCase))
			{
				return Path.GetDirectoryName(assembly.Location);
			}
			return "";
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x00103540 File Offset: 0x00101740
		private static string[] GetProductFolderDirectories()
		{
			if (Utils._productFolderDirectories == null)
			{
				List<string> list = new List<string>();
				string applicationBase = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID);
				if (!string.IsNullOrEmpty(applicationBase))
				{
					list.Add(applicationBase);
				}
				list.Add(Environment.GetFolderPath(Environment.SpecialFolder.System));
				list.Add(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsPowerShell", "Modules");
				list.Add(Path.Combine(path, "PackageManagement"));
				list.Add(Path.Combine(path, "PowerShellGet"));
				list.Add(Path.Combine(path, "Pester"));
				list.Add(Path.Combine(path, "PSReadline"));
				Interlocked.CompareExchange<string[]>(ref Utils._productFolderDirectories, list.ToArray(), null);
			}
			return Utils._productFolderDirectories;
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x00103604 File Offset: 0x00101804
		internal static bool IsUnderProductFolder(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			string fullName = fileInfo.FullName;
			foreach (string value in Utils.GetProductFolderDirectories())
			{
				if (fullName.StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x00103646 File Offset: 0x00101846
		internal static bool IsRunningFromSysWOW64()
		{
			return Utils.GetApplicationBase(Utils.DefaultPowerShellShellID).Contains("SysWOW64");
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x0010365C File Offset: 0x0010185C
		internal static bool IsWinPEHost()
		{
			RegistryKey registryKey = null;
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey(Utils.WinPEIdentificationRegKey);
				return registryKey != null;
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Dispose();
				}
			}
			return false;
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x001036D4 File Offset: 0x001018D4
		internal static string GetCurrentMajorVersion()
		{
			return PSVersionInfo.PSVersion.Major.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x001036F8 File Offset: 0x001018F8
		internal static Version StringToVersion(string versionString)
		{
			if (string.IsNullOrEmpty(versionString))
			{
				return null;
			}
			int num = 0;
			foreach (char c in versionString)
			{
				if (c == '.')
				{
					num++;
					if (num > 1)
					{
						break;
					}
				}
			}
			if (num == 0)
			{
				versionString += ".0";
			}
			Version result = null;
			if (Version.TryParse(versionString, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x00103760 File Offset: 0x00101960
		internal static bool IsPSVersionSupported(string ver)
		{
			Version checkVersion = Utils.StringToVersion(ver);
			return Utils.IsPSVersionSupported(checkVersion);
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x0010377C File Offset: 0x0010197C
		internal static bool IsPSVersionSupported(Version checkVersion)
		{
			if (checkVersion == null)
			{
				return false;
			}
			foreach (Version version in PSVersionInfo.PSCompatibleVersions)
			{
				if (checkVersion.Major == version.Major && checkVersion.Minor <= version.Minor)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x001037D0 File Offset: 0x001019D0
		internal static bool IsNetFrameworkVersionSupported(Version checkVersion, out bool higherThanKnownHighestVersion)
		{
			higherThanKnownHighestVersion = false;
			bool result = false;
			if (checkVersion == null)
			{
				return false;
			}
			Version version = new Version(checkVersion.Major, checkVersion.Minor, 0, 0);
			if (checkVersion > PsUtils.FrameworkRegistryInstallation.KnownHighestNetFrameworkVersion)
			{
				result = true;
				higherThanKnownHighestVersion = true;
			}
			else if (PsUtils.FrameworkRegistryInstallation.CompatibleNetFrameworkVersions.ContainsKey(version))
			{
				if (PsUtils.FrameworkRegistryInstallation.IsFrameworkInstalled(version.Major, version.Minor, 0))
				{
					result = true;
				}
				else
				{
					HashSet<Version> hashSet = PsUtils.FrameworkRegistryInstallation.CompatibleNetFrameworkVersions[version];
					foreach (Version version2 in hashSet)
					{
						if (PsUtils.FrameworkRegistryInstallation.IsFrameworkInstalled(version2.Major, version2.Minor, 0))
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x0010389C File Offset: 0x00101A9C
		internal static string GetRegistryConfigurationPrefix()
		{
			return "SOFTWARE\\Microsoft\\PowerShell\\" + PSVersionInfo.RegistryVersion1Key + "\\ShellIds";
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x001038B2 File Offset: 0x00101AB2
		internal static string GetRegistryConfigurationPath(string shellID)
		{
			return Utils.GetRegistryConfigurationPrefix() + "\\" + shellID;
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x001038C4 File Offset: 0x00101AC4
		internal static Dictionary<string, object> GetGroupPolicySetting(string settingName, params RegistryKey[] preferenceOrder)
		{
			string groupPolicyBase = "Software\\Policies\\Microsoft\\Windows\\PowerShell";
			return Utils.GetGroupPolicySetting(groupPolicyBase, settingName, preferenceOrder);
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x001038E0 File Offset: 0x00101AE0
		internal static Dictionary<string, object> GetGroupPolicySetting(string groupPolicyBase, string settingName, params RegistryKey[] preferenceOrder)
		{
			Dictionary<string, object> result;
			lock (Utils.cachedGroupPolicySettings)
			{
				Dictionary<string, object> dictionary;
				if (Utils.cachedGroupPolicySettings.TryGetValue(settingName, out dictionary) && !InternalTestHooks.BypassGroupPolicyCaching)
				{
					result = dictionary;
				}
				else
				{
					if (!string.Equals(".", settingName, StringComparison.OrdinalIgnoreCase))
					{
						groupPolicyBase = groupPolicyBase + "\\" + settingName;
					}
					dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
					foreach (RegistryKey registryKey in preferenceOrder)
					{
						try
						{
							using (RegistryKey registryKey2 = registryKey.OpenSubKey(groupPolicyBase))
							{
								if (registryKey2 != null)
								{
									foreach (string text in registryKey2.GetValueNames())
									{
										string text2 = text ?? string.Empty;
										dictionary[text2] = registryKey2.GetValue(text2);
									}
									foreach (string text3 in registryKey2.GetSubKeyNames())
									{
										string text4 = text3 ?? string.Empty;
										using (RegistryKey registryKey3 = registryKey2.OpenSubKey(text4))
										{
											if (registryKey3 != null)
											{
												dictionary[text4] = registryKey3.GetValueNames();
											}
										}
									}
									break;
								}
							}
						}
						catch (SecurityException)
						{
						}
					}
					if (dictionary.Count == 0)
					{
						dictionary = null;
					}
					if (!InternalTestHooks.BypassGroupPolicyCaching)
					{
						Utils.cachedGroupPolicySettings[settingName] = dictionary;
					}
					result = dictionary;
				}
			}
			return result;
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x00103AB4 File Offset: 0x00101CB4
		internal static IAstToWorkflowConverter GetAstToWorkflowConverterAndEnsureWorkflowModuleLoaded(ExecutionContext context)
		{
			IAstToWorkflowConverter astToWorkflowConverter = null;
			if (Utils.IsRunningFromSysWOW64())
			{
				throw new NotSupportedException(AutomationExceptions.WorkflowDoesNotSupportWOW64);
			}
			if (context != null && context.LanguageMode == PSLanguageMode.ConstrainedLanguage && SystemPolicy.GetSystemLockdownPolicy() != SystemEnforcementMode.Enforce)
			{
				throw new NotSupportedException(Modules.CannotDefineWorkflowInconsistentLanguageMode);
			}
			Utils.EnsureModuleLoaded("PSWorkflow", context);
			Type type = Type.GetType("Microsoft.PowerShell.Workflow.AstToWorkflowConverter, Microsoft.PowerShell.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
			if (type != null)
			{
				astToWorkflowConverter = (IAstToWorkflowConverter)type.GetConstructor(PSTypeExtensions.EmptyTypes).Invoke(new object[0]);
			}
			if (astToWorkflowConverter == null)
			{
				string message = StringUtil.Format(AutomationExceptions.CantLoadWorkflowType, "Microsoft.PowerShell.Workflow.AstToWorkflowConverter, Microsoft.PowerShell.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", "PSWorkflow");
				throw new NotSupportedException(message);
			}
			return astToWorkflowConverter;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x00103B54 File Offset: 0x00101D54
		internal static void EnsureModuleLoaded(string module, ExecutionContext context)
		{
			if (context != null && !context.AutoLoadingModuleInProgress.Contains(module))
			{
				List<PSModuleInfo> modules = context.Modules.GetModules(new string[]
				{
					module
				}, false);
				if (modules == null || modules.Count == 0)
				{
					CommandInfo commandInfo = new CmdletInfo("Import-Module", typeof(ImportModuleCommand), null, null, context);
					Command command = new Command(commandInfo);
					context.AutoLoadingModuleInProgress.Add(module);
					PowerShell powerShell = null;
					try
					{
						powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(command).AddParameter("Name", module).AddParameter("Scope", "GLOBAL").AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("InformationAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false).AddParameter("PassThru");
						powerShell.Invoke<PSModuleInfo>();
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
					finally
					{
						context.AutoLoadingModuleInProgress.Remove(module);
						if (powerShell != null)
						{
							powerShell.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x00103C9C File Offset: 0x00101E9C
		internal static List<PSModuleInfo> GetModules(string module, ExecutionContext context)
		{
			List<PSModuleInfo> list = context.Modules.GetModules(new string[]
			{
				module
			}, false);
			CommandInfo commandInfo = new CmdletInfo("Get-Module", typeof(GetModuleCommand), null, null, context);
			Command command = new Command(commandInfo);
			PowerShell powerShell = null;
			try
			{
				powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(command).AddParameter("Name", module).AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false).AddParameter("ListAvailable");
				Collection<PSModuleInfo> collection = powerShell.Invoke<PSModuleInfo>();
				if (collection != null)
				{
					if (list == null)
					{
						list = collection.ToList<PSModuleInfo>();
					}
					else
					{
						foreach (PSModuleInfo item in collection)
						{
							list.Add(item);
						}
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				if (powerShell != null)
				{
					powerShell.Dispose();
				}
			}
			return list;
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x00103DD8 File Offset: 0x00101FD8
		internal static List<PSModuleInfo> GetModules(ModuleSpecification fullyQualifiedName, ExecutionContext context)
		{
			List<PSModuleInfo> list = context.Modules.GetModules(new ModuleSpecification[]
			{
				fullyQualifiedName
			}, false);
			CommandInfo commandInfo = new CmdletInfo("Get-Module", typeof(GetModuleCommand), null, null, context);
			Command command = new Command(commandInfo);
			PowerShell powerShell = null;
			try
			{
				powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(command).AddParameter("FullyQualifiedName", fullyQualifiedName).AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("InformationAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false).AddParameter("ListAvailable");
				Collection<PSModuleInfo> collection = powerShell.Invoke<PSModuleInfo>();
				if (collection != null)
				{
					if (list == null)
					{
						list = collection.ToList<PSModuleInfo>();
					}
					else
					{
						list.AddRange(collection);
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				if (powerShell != null)
				{
					powerShell.Dispose();
				}
			}
			return list;
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x00103EEC File Offset: 0x001020EC
		internal static bool IsAdministrator()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x00103F14 File Offset: 0x00102114
		internal static bool NativeItemExists(string path)
		{
			bool flag;
			Exception ex;
			return Utils.NativeItemExists(path, out flag, out ex);
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x00103F2C File Offset: 0x0010212C
		internal static bool NativeItemExists(string path, out bool isDirectory, out Exception exception)
		{
			exception = null;
			if (string.IsNullOrEmpty(path))
			{
				isDirectory = false;
				return false;
			}
			if (Utils.IsReservedDeviceName(path))
			{
				isDirectory = false;
				return false;
			}
			int fileAttributes = Utils.NativeMethods.GetFileAttributes(path);
			if (fileAttributes == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 5)
				{
					Win32Exception ex = new Win32Exception(lastWin32Error);
					exception = new UnauthorizedAccessException(ex.Message, ex);
				}
				else if (lastWin32Error == 53)
				{
					Win32Exception ex2 = new Win32Exception(lastWin32Error);
					exception = new IOException(ex2.Message, ex2);
				}
				isDirectory = false;
				return false;
			}
			isDirectory = ((fileAttributes & 16) == 16);
			return true;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x00103FAC File Offset: 0x001021AC
		internal static bool NativeFileExists(string path)
		{
			bool flag2;
			Exception ex;
			bool flag = Utils.NativeItemExists(path, out flag2, out ex);
			if (ex != null)
			{
				throw ex;
			}
			return flag && !flag2;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00103FD4 File Offset: 0x001021D4
		internal static bool NativeDirectoryExists(string path)
		{
			bool flag2;
			Exception ex;
			bool flag = Utils.NativeItemExists(path, out flag2, out ex);
			if (ex != null)
			{
				throw ex;
			}
			return flag && flag2;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x00103FF8 File Offset: 0x001021F8
		internal static void NativeEnumerateDirectory(string directory, out List<string> directories, out List<string> files)
		{
			IntPtr value = new IntPtr(-1);
			files = new List<string>();
			directories = new List<string>();
			Utils.NativeMethods.WIN32_FIND_DATA win32_FIND_DATA;
			IntPtr intPtr = Utils.NativeMethods.FindFirstFile(directory + "\\*", out win32_FIND_DATA);
			if (intPtr != value)
			{
				do
				{
					if ((win32_FIND_DATA.dwFileAttributes & Utils.NativeMethods.FileAttributes.Directory) != (Utils.NativeMethods.FileAttributes)0)
					{
						if (!string.Equals(".", win32_FIND_DATA.cFileName, StringComparison.OrdinalIgnoreCase) && !string.Equals("..", win32_FIND_DATA.cFileName, StringComparison.OrdinalIgnoreCase))
						{
							directories.Add(directory + "\\" + win32_FIND_DATA.cFileName);
						}
					}
					else
					{
						files.Add(directory + "\\" + win32_FIND_DATA.cFileName);
					}
				}
				while (Utils.NativeMethods.FindNextFile(intPtr, out win32_FIND_DATA));
				Utils.NativeMethods.FindClose(intPtr);
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x001040B0 File Offset: 0x001022B0
		internal static bool IsReservedDeviceName(string destinationPath)
		{
			string[] array = new string[]
			{
				"CON",
				"PRN",
				"AUX",
				"CLOCK$",
				"NUL",
				"COM0",
				"COM1",
				"COM2",
				"COM3",
				"COM4",
				"COM5",
				"COM6",
				"COM7",
				"COM8",
				"COM9",
				"LPT0",
				"LPT1",
				"LPT2",
				"LPT3",
				"LPT4",
				"LPT5",
				"LPT6",
				"LPT7",
				"LPT8",
				"LPT9"
			};
			string fileName = Path.GetFileName(destinationPath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(destinationPath);
			if ((fileName.Length < 3 || fileName.Length > 6) && (fileNameWithoutExtension.Length < 3 || fileNameWithoutExtension.Length > 6))
			{
				return false;
			}
			foreach (string a in array)
			{
				if (string.Equals(a, fileName, StringComparison.OrdinalIgnoreCase) || string.Equals(a, fileNameWithoutExtension, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x00104228 File Offset: 0x00102428
		internal static bool IsPowerShellAssembly(string assemblyName)
		{
			if (!string.IsNullOrWhiteSpace(assemblyName))
			{
				string text = assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? Path.GetFileNameWithoutExtension(assemblyName) : assemblyName;
				if (text != null && Utils.PowerShellAssemblies.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x00104268 File Offset: 0x00102468
		internal static string GetPowerShellAssemblyStrongName(string assemblyName)
		{
			if (!string.IsNullOrWhiteSpace(assemblyName))
			{
				string text = assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? Path.GetFileNameWithoutExtension(assemblyName) : assemblyName;
				if (text != null && Utils.PowerShellAssemblies.Contains(text))
				{
					return string.Format(CultureInfo.InvariantCulture, Utils.PowerShellAssemblyStrongNameFormat, new object[]
					{
						text
					});
				}
			}
			return assemblyName;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x001042C4 File Offset: 0x001024C4
		internal static Mutex SafeWaitMutex(Mutex mutex, Utils.MutexInitializer initializer)
		{
			try
			{
				mutex.WaitOne();
			}
			catch (AbandonedMutexException)
			{
				mutex.ReleaseMutex();
				((IDisposable)mutex).Dispose();
				mutex = initializer();
				mutex.WaitOne();
			}
			return mutex;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x0010430C File Offset: 0x0010250C
		internal static bool Succeeded(int hresult)
		{
			return hresult >= 0;
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x00104318 File Offset: 0x00102518
		internal static FileSystemCmdletProviderEncoding GetEncoding(string path)
		{
			if (!File.Exists(path))
			{
				return FileSystemCmdletProviderEncoding.Default;
			}
			byte[] array = new byte[100];
			int num = 0;
			try
			{
				using (FileStream fileStream = File.OpenRead(path))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						num = binaryReader.Read(array, 0, 100);
					}
				}
			}
			catch (IOException)
			{
				return FileSystemCmdletProviderEncoding.Default;
			}
			FileSystemCmdletProviderEncoding result = FileSystemCmdletProviderEncoding.Default;
			if (num > 3)
			{
				string key = string.Join("-", new object[]
				{
					array[0],
					array[1],
					array[2],
					array[3]
				});
				if (Utils.encodingMap.TryGetValue(key, out result))
				{
					return result;
				}
			}
			if (num > 2)
			{
				string key = string.Join("-", new object[]
				{
					array[0],
					array[1],
					array[2]
				});
				if (Utils.encodingMap.TryGetValue(key, out result))
				{
					return result;
				}
			}
			if (num > 1)
			{
				string key = string.Join("-", new object[]
				{
					array[0],
					array[1]
				});
				if (Utils.encodingMap.TryGetValue(key, out result))
				{
					return result;
				}
			}
			string @string = Encoding.ASCII.GetString(array, 0, num);
			if (@string.IndexOfAny(Utils.nonPrintableCharacters) >= 0)
			{
				return FileSystemCmdletProviderEncoding.Byte;
			}
			return FileSystemCmdletProviderEncoding.Ascii;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x001044C0 File Offset: 0x001026C0
		internal static Encoding GetEncodingFromEnum(FileSystemCmdletProviderEncoding encoding)
		{
			Encoding unicode = Encoding.Unicode;
			switch (encoding)
			{
			case FileSystemCmdletProviderEncoding.String:
				return new UnicodeEncoding();
			case FileSystemCmdletProviderEncoding.Unicode:
				return new UnicodeEncoding();
			case FileSystemCmdletProviderEncoding.BigEndianUnicode:
				return new UnicodeEncoding(true, false);
			case FileSystemCmdletProviderEncoding.UTF8:
				return new UTF8Encoding();
			case FileSystemCmdletProviderEncoding.UTF7:
				return new UTF7Encoding();
			case FileSystemCmdletProviderEncoding.UTF32:
				return new UTF32Encoding();
			case FileSystemCmdletProviderEncoding.Ascii:
				return new ASCIIEncoding();
			case FileSystemCmdletProviderEncoding.Default:
				return ClrFacade.GetDefaultEncoding();
			case FileSystemCmdletProviderEncoding.Oem:
				return ClrFacade.GetOEMEncoding();
			case FileSystemCmdletProviderEncoding.BigEndianUTF32:
				return new UTF32Encoding(true, false);
			}
			return new UnicodeEncoding();
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x00104568 File Offset: 0x00102768
		internal static void QueueWorkItemWithImpersonation(WindowsIdentity identityToImpersonate, WaitCallback threadProc, object state)
		{
			object[] state2 = new object[]
			{
				identityToImpersonate,
				threadProc,
				state
			};
			ThreadPool.QueueUserWorkItem(new WaitCallback(Utils.WorkItemCallback), state2);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x0010459C File Offset: 0x0010279C
		private static void WorkItemCallback(object callBackArgs)
		{
			object[] array = callBackArgs as object[];
			WindowsIdentity windowsIdentity = array[0] as WindowsIdentity;
			WaitCallback waitCallback = array[1] as WaitCallback;
			object state = array[2];
			WindowsImpersonationContext windowsImpersonationContext = null;
			if (windowsIdentity != null && windowsIdentity.ImpersonationLevel == TokenImpersonationLevel.Impersonation)
			{
				windowsImpersonationContext = windowsIdentity.Impersonate();
			}
			try
			{
				waitCallback(state);
			}
			finally
			{
				if (windowsImpersonationContext != null)
				{
					try
					{
						windowsImpersonationContext.Undo();
						windowsImpersonationContext.Dispose();
					}
					catch (SecurityException)
					{
					}
				}
			}
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x0010461C File Offset: 0x0010281C
		internal static string ParseCommandName(string commandName, out string moduleName)
		{
			string[] array = commandName.Split(new char[]
			{
				'\\'
			}, 2);
			if (array.Length == 2)
			{
				moduleName = array[0];
				return array[1];
			}
			moduleName = null;
			return commandName;
		}

		// Token: 0x04001994 RID: 6548
		internal const string ScheduledJobModuleName = "PSScheduledJob";

		// Token: 0x04001995 RID: 6549
		internal const string WorkflowType = "Microsoft.PowerShell.Workflow.AstToWorkflowConverter, Microsoft.PowerShell.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

		// Token: 0x04001996 RID: 6550
		internal const string WorkflowModule = "PSWorkflow";

		// Token: 0x04001997 RID: 6551
		internal static string WinPEIdentificationRegKey = "System\\CurrentControlSet\\Control\\MiniNT";

		// Token: 0x04001998 RID: 6552
		private static string _pshome = null;

		// Token: 0x04001999 RID: 6553
		private static string[] _productFolderDirectories;

		// Token: 0x0400199A RID: 6554
		internal static string DefaultPowerShellShellID = "Microsoft.PowerShell";

		// Token: 0x0400199B RID: 6555
		internal static string ProductNameForDirectory = "WindowsPowerShell";

		// Token: 0x0400199C RID: 6556
		internal static string ModuleDirectory = "Modules";

		// Token: 0x0400199D RID: 6557
		internal static string DscModuleDirectory = "WindowsPowerShell\\Modules";

		// Token: 0x0400199E RID: 6558
		private static ConcurrentDictionary<string, Dictionary<string, object>> cachedGroupPolicySettings = new ConcurrentDictionary<string, Dictionary<string, object>>();

		// Token: 0x0400199F RID: 6559
		internal static char[] DirectorySeparators = new char[]
		{
			'\\',
			'/'
		};

		// Token: 0x040019A0 RID: 6560
		internal static readonly string PowerShellAssemblyStrongNameFormat = "{0}, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

		// Token: 0x040019A1 RID: 6561
		internal static readonly HashSet<string> PowerShellAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"microsoft.powershell.activities",
			"microsoft.powershell.commands.diagnostics",
			"microsoft.powershell.commands.management",
			"microsoft.powershell.commands.utility",
			"microsoft.powershell.consolehost",
			"microsoft.powershell.core.activities",
			"microsoft.powershell.diagnostics.activities",
			"microsoft.powershell.editor",
			"microsoft.powershell.gpowershell",
			"microsoft.powershell.graphicalhost",
			"microsoft.powershell.isecommon",
			"microsoft.powershell.management.activities",
			"microsoft.powershell.scheduledjob",
			"microsoft.powershell.security.activities",
			"microsoft.powershell.security",
			"microsoft.powershell.utility.activities",
			"microsoft.powershell.workflow.servicecore",
			"microsoft.wsman.management.activities",
			"microsoft.wsman.management",
			"microsoft.wsman.runtime",
			"system.management.automation"
		};

		// Token: 0x040019A2 RID: 6562
		internal static Dictionary<string, FileSystemCmdletProviderEncoding> encodingMap = new Dictionary<string, FileSystemCmdletProviderEncoding>
		{
			{
				"255-254",
				FileSystemCmdletProviderEncoding.Unicode
			},
			{
				"254-255",
				FileSystemCmdletProviderEncoding.BigEndianUnicode
			},
			{
				"255-254-0-0",
				FileSystemCmdletProviderEncoding.UTF32
			},
			{
				"0-0-254-255",
				FileSystemCmdletProviderEncoding.BigEndianUTF32
			},
			{
				"239-187-191",
				FileSystemCmdletProviderEncoding.UTF8
			}
		};

		// Token: 0x040019A3 RID: 6563
		internal static char[] nonPrintableCharacters = new char[]
		{
			'\0',
			'\u0001',
			'\u0002',
			'\u0003',
			'\u0004',
			'\u0005',
			'\u0006',
			'\a',
			'\b',
			'\v',
			'\f',
			'\u000e',
			'\u000f',
			'\u0010',
			'\u0011',
			'\u0012',
			'\u0013',
			'\u0014',
			'\u0015',
			'\u0016',
			'\u0017',
			'\u0018',
			'\u0019',
			'\u001a',
			'\u001c',
			'\u001d',
			'\u001e',
			'\u001f',
			'\u007f',
			'\u0081',
			'\u008d',
			'\u008f',
			'\u0090',
			'\u009d'
		};

		// Token: 0x02000431 RID: 1073
		internal class NativeMethods
		{
			// Token: 0x06002F6F RID: 12143
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern int GetFileAttributes(string lpFileName);

			// Token: 0x06002F70 RID: 12144
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			public static extern IntPtr FindFirstFile(string lpFileName, out Utils.NativeMethods.WIN32_FIND_DATA lpFindFileData);

			// Token: 0x06002F71 RID: 12145
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			public static extern bool FindNextFile(IntPtr hFindFile, out Utils.NativeMethods.WIN32_FIND_DATA lpFindFileData);

			// Token: 0x06002F72 RID: 12146
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			public static extern bool FindClose(IntPtr hFindFile);

			// Token: 0x040019A4 RID: 6564
			public const int MAX_PATH = 260;

			// Token: 0x040019A5 RID: 6565
			public const int MAX_ALTERNATE = 14;

			// Token: 0x02000432 RID: 1074
			[Flags]
			internal enum FileAttributes
			{
				// Token: 0x040019A7 RID: 6567
				Hidden = 2,
				// Token: 0x040019A8 RID: 6568
				Directory = 16
			}

			// Token: 0x02000433 RID: 1075
			public struct FILETIME
			{
				// Token: 0x040019A9 RID: 6569
				public uint dwLowDateTime;

				// Token: 0x040019AA RID: 6570
				public uint dwHighDateTime;
			}

			// Token: 0x02000434 RID: 1076
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct WIN32_FIND_DATA
			{
				// Token: 0x040019AB RID: 6571
				public Utils.NativeMethods.FileAttributes dwFileAttributes;

				// Token: 0x040019AC RID: 6572
				public Utils.NativeMethods.FILETIME ftCreationTime;

				// Token: 0x040019AD RID: 6573
				public Utils.NativeMethods.FILETIME ftLastAccessTime;

				// Token: 0x040019AE RID: 6574
				public Utils.NativeMethods.FILETIME ftLastWriteTime;

				// Token: 0x040019AF RID: 6575
				public uint nFileSizeHigh;

				// Token: 0x040019B0 RID: 6576
				public uint nFileSizeLow;

				// Token: 0x040019B1 RID: 6577
				public uint dwReserved0;

				// Token: 0x040019B2 RID: 6578
				public uint dwReserved1;

				// Token: 0x040019B3 RID: 6579
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
				public string cFileName;

				// Token: 0x040019B4 RID: 6580
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
				public string cAlternate;
			}
		}

		// Token: 0x02000435 RID: 1077
		// (Invoke) Token: 0x06002F75 RID: 12149
		internal delegate Mutex MutexInitializer();
	}
}
