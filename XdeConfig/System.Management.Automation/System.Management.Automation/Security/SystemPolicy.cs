using System;
using System.IO;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32;

namespace System.Management.Automation.Security
{
	// Token: 0x02000816 RID: 2070
	public sealed class SystemPolicy
	{
		// Token: 0x06004FAC RID: 20396 RVA: 0x001A6E1A File Offset: 0x001A501A
		private SystemPolicy()
		{
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x001A6E24 File Offset: 0x001A5024
		public static SystemEnforcementMode GetSystemLockdownPolicy()
		{
			if (SystemPolicy.wasSystemPolicyDebugPolicy || SystemPolicy.systemLockdownPolicy == null)
			{
				lock (SystemPolicy.systemLockdownPolicyLock)
				{
					if (SystemPolicy.wasSystemPolicyDebugPolicy || SystemPolicy.systemLockdownPolicy == null)
					{
						SystemPolicy.systemLockdownPolicy = new SystemEnforcementMode?(SystemPolicy.GetLockdownPolicy(null, null));
					}
				}
			}
			return SystemPolicy.systemLockdownPolicy.Value;
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x001A6EA0 File Offset: 0x001A50A0
		public static SystemEnforcementMode GetLockdownPolicy(string path, SafeHandle handle)
		{
			SystemEnforcementMode systemEnforcementMode = SystemPolicy.GetWldpPolicy(path, handle);
			if (systemEnforcementMode == SystemEnforcementMode.Enforce)
			{
				return systemEnforcementMode;
			}
			systemEnforcementMode = SystemPolicy.GetAppLockerPolicy(path, handle);
			if (systemEnforcementMode == SystemEnforcementMode.Enforce)
			{
				return systemEnforcementMode;
			}
			if (SystemPolicy.cachedSaferSystemPolicy.GetValueOrDefault(SaferPolicy.Allowed) == SaferPolicy.Disallowed)
			{
				return systemEnforcementMode;
			}
			return SystemPolicy.GetDebugLockdownPolicy(path);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x001A6EE0 File Offset: 0x001A50E0
		private static SystemEnforcementMode GetWldpPolicy(string path, SafeHandle handle)
		{
			if (SystemPolicy.hadMissingWldpAssembly || !File.Exists(Path.Combine(Environment.SystemDirectory, "wldp.dll")))
			{
				SystemPolicy.hadMissingWldpAssembly = true;
				return SystemPolicy.GetDebugLockdownPolicy(path);
			}
			SystemEnforcementMode result;
			try
			{
				SystemPolicy.WLDP_HOST_INFORMATION wldp_HOST_INFORMATION = default(SystemPolicy.WLDP_HOST_INFORMATION);
				wldp_HOST_INFORMATION.dwRevision = 1U;
				wldp_HOST_INFORMATION.dwHostId = SystemPolicy.WLDP_HOST_ID.WLDP_HOST_ID_POWERSHELL;
				if (!string.IsNullOrEmpty(path))
				{
					wldp_HOST_INFORMATION.szSource = path;
					if (handle != null)
					{
						IntPtr hSource = IntPtr.Zero;
						hSource = handle.DangerousGetHandle();
						wldp_HOST_INFORMATION.hSource = hSource;
					}
				}
				uint pdwLockdownState = 0U;
				int num = SystemPolicy.WldpNativeMethods.WldpGetLockdownPolicy(ref wldp_HOST_INFORMATION, ref pdwLockdownState, 0U);
				if (num >= 0)
				{
					result = SystemPolicy.GetLockdownPolicyForResult(pdwLockdownState);
				}
				else
				{
					result = SystemEnforcementMode.Enforce;
				}
			}
			catch (DllNotFoundException)
			{
				SystemPolicy.hadMissingWldpAssembly = true;
				result = SystemPolicy.GetDebugLockdownPolicy(path);
			}
			return result;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x001A6F9C File Offset: 0x001A519C
		private static SystemEnforcementMode GetAppLockerPolicy(string path, SafeHandle handle)
		{
			SaferPolicy saferPolicy = SaferPolicy.Disallowed;
			if (string.IsNullOrEmpty(path))
			{
				if (SystemPolicy.cachedSaferSystemPolicy != null && !InternalTestHooks.BypassAppLockerPolicyCaching)
				{
					saferPolicy = SystemPolicy.cachedSaferSystemPolicy.Value;
				}
				else
				{
					string text = null;
					string text2 = null;
					try
					{
						string text3 = Path.GetTempPath();
						int num = 0;
						while (num++ < 2)
						{
							bool flag = false;
							try
							{
								if (!Directory.Exists(text3))
								{
									Directory.CreateDirectory(text3);
								}
								text = Path.Combine(text3, Path.GetRandomFileName() + ".ps1");
								text2 = Path.Combine(text3, Path.GetRandomFileName() + ".psm1");
								File.WriteAllText(text, "1");
								File.WriteAllText(text2, "1");
							}
							catch (IOException)
							{
								if (num == 2)
								{
									throw;
								}
								flag = true;
							}
							catch (UnauthorizedAccessException)
							{
								if (num == 2)
								{
									throw;
								}
								flag = true;
							}
							catch (SecurityException)
							{
								if (num == 2)
								{
									throw;
								}
								flag = true;
							}
							if (!flag)
							{
								break;
							}
							Guid knownFolderId = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
							text3 = SystemPolicy.GetKnownFolderPath(knownFolderId) + "\\Temp";
						}
						saferPolicy = SystemPolicy.TestSaferPolicy(text, text2);
					}
					catch (IOException)
					{
						saferPolicy = SaferPolicy.Disallowed;
					}
					catch (UnauthorizedAccessException)
					{
						saferPolicy = ((WindowsIdentity.GetCurrent().ImpersonationLevel == TokenImpersonationLevel.Impersonation) ? SaferPolicy.Allowed : SaferPolicy.Disallowed);
					}
					finally
					{
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						if (File.Exists(text2))
						{
							File.Delete(text2);
						}
					}
					SystemPolicy.cachedSaferSystemPolicy = new SaferPolicy?(saferPolicy);
				}
				if (saferPolicy == SaferPolicy.Disallowed)
				{
					return SystemEnforcementMode.Enforce;
				}
				return SystemEnforcementMode.None;
			}
			else
			{
				saferPolicy = SecuritySupport.GetSaferPolicy(path, handle);
				if (saferPolicy == SaferPolicy.Disallowed)
				{
					return SystemEnforcementMode.Enforce;
				}
				return SystemEnforcementMode.None;
			}
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x001A7148 File Offset: 0x001A5348
		private static string GetKnownFolderPath(Guid knownFolderId)
		{
			IntPtr zero = IntPtr.Zero;
			string result;
			try
			{
				int num = SystemPolicy.WldpNativeMethods.SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out zero);
				if (num < 0)
				{
					throw new IOException();
				}
				result = Marshal.PtrToStringAuto(zero);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(zero);
				}
			}
			return result;
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001A71A4 File Offset: 0x001A53A4
		private static SaferPolicy TestSaferPolicy(string testPathScript, string testPathModule)
		{
			SaferPolicy saferPolicy = SecuritySupport.GetSaferPolicy(testPathScript, null);
			if (saferPolicy == SaferPolicy.Disallowed)
			{
				saferPolicy = SecuritySupport.GetSaferPolicy(testPathModule, null);
			}
			return saferPolicy;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x001A71C8 File Offset: 0x001A53C8
		private static SystemEnforcementMode GetDebugLockdownPolicy(string path)
		{
			if (PsUtils.IsRunningOnProcessorArchitectureARM())
			{
				return SystemEnforcementMode.Enforce;
			}
			SystemPolicy.wasSystemPolicyDebugPolicy = true;
			if (path != null)
			{
				if (path.IndexOf("System32", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return SystemEnforcementMode.None;
				}
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\CI\\TRSData"))
					{
						if (registryKey2 != null)
						{
							object value = registryKey2.GetValue("TestPath");
							registryKey2.Close();
							registryKey.Close();
							if (value != null)
							{
								string[] array = (string[])value;
								foreach (string value2 in array)
								{
									if (path.IndexOf(value2, StringComparison.OrdinalIgnoreCase) >= 0)
									{
										return SystemEnforcementMode.None;
									}
								}
							}
						}
					}
				}
				return SystemPolicy.systemLockdownPolicy.GetValueOrDefault(SystemEnforcementMode.None);
			}
			else
			{
				object environmentVariable = Environment.GetEnvironmentVariable("__PSLockdownPolicy", EnvironmentVariableTarget.Machine);
				if (environmentVariable != null)
				{
					uint pdwLockdownState = LanguagePrimitives.ConvertTo<uint>(environmentVariable);
					return SystemPolicy.GetLockdownPolicyForResult(pdwLockdownState);
				}
				return SystemEnforcementMode.None;
			}
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x001A72D0 File Offset: 0x001A54D0
		internal static bool IsClassInApprovedList(Guid clsid)
		{
			bool result;
			try
			{
				SystemPolicy.WLDP_HOST_INFORMATION wldp_HOST_INFORMATION = default(SystemPolicy.WLDP_HOST_INFORMATION);
				wldp_HOST_INFORMATION.dwRevision = 1U;
				wldp_HOST_INFORMATION.dwHostId = SystemPolicy.WLDP_HOST_ID.WLDP_HOST_ID_POWERSHELL;
				int num = 0;
				int num2 = SystemPolicy.WldpNativeMethods.WldpIsClassInApprovedList(ref clsid, ref wldp_HOST_INFORMATION, ref num, 0U);
				if (num2 >= 0 && num == 1)
				{
					if (SystemPolicy.wasSystemPolicyDebugPolicy && string.Equals(clsid.ToString(), "0000050b-0000-0010-8000-00aa006d2ea4", StringComparison.OrdinalIgnoreCase))
					{
						result = false;
					}
					else
					{
						result = true;
					}
				}
				else
				{
					result = false;
				}
			}
			catch (DllNotFoundException)
			{
				if (string.Equals(clsid.ToString(), "f6d90f11-9c73-11d3-b32e-00c04f990bb4", StringComparison.OrdinalIgnoreCase))
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x001A7370 File Offset: 0x001A5570
		private static SystemEnforcementMode GetLockdownPolicyForResult(uint pdwLockdownState)
		{
			if ((pdwLockdownState & 8U) == 8U)
			{
				return SystemEnforcementMode.Audit;
			}
			if ((pdwLockdownState & 4U) == 4U)
			{
				return SystemEnforcementMode.Enforce;
			}
			return SystemEnforcementMode.None;
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001A7384 File Offset: 0x001A5584
		internal static string DumpLockdownState(uint pdwLockdownState)
		{
			string text = "";
			if ((pdwLockdownState & 2147483648U) == 2147483648U)
			{
				text += "WLDP_LOCKDOWN_DEFINED_FLAG\r\n";
			}
			if ((pdwLockdownState & 1U) == 1U)
			{
				text += "WLDP_LOCKDOWN_SECUREBOOT_FLAG\r\n";
			}
			if ((pdwLockdownState & 2U) == 2U)
			{
				text += "WLDP_LOCKDOWN_DEBUGPOLICY_FLAG\r\n";
			}
			if ((pdwLockdownState & 4U) == 4U)
			{
				text += "WLDP_LOCKDOWN_UMCIENFORCE_FLAG\r\n";
			}
			if ((pdwLockdownState & 8U) == 8U)
			{
				text += "WLDP_LOCKDOWN_UMCIAUDIT_FLAG\r\n";
			}
			return text;
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06004FB7 RID: 20407 RVA: 0x001A73FA File Offset: 0x001A55FA
		// (set) Token: 0x06004FB8 RID: 20408 RVA: 0x001A7401 File Offset: 0x001A5601
		internal static bool XamlWorkflowSupported { get; set; }

		// Token: 0x040028BD RID: 10429
		private static object systemLockdownPolicyLock = new object();

		// Token: 0x040028BE RID: 10430
		private static SystemEnforcementMode? systemLockdownPolicy = null;

		// Token: 0x040028BF RID: 10431
		private static bool wasSystemPolicyDebugPolicy = false;

		// Token: 0x040028C0 RID: 10432
		private static SaferPolicy? cachedSaferSystemPolicy = null;

		// Token: 0x040028C1 RID: 10433
		private static bool hadMissingWldpAssembly = false;

		// Token: 0x02000817 RID: 2071
		internal class WldpNativeConstants
		{
			// Token: 0x040028C3 RID: 10435
			internal const uint WLDP_HOST_INFORMATION_REVISION = 1U;

			// Token: 0x040028C4 RID: 10436
			internal const uint WLDP_LOCKDOWN_UNDEFINED = 0U;

			// Token: 0x040028C5 RID: 10437
			internal const uint WLDP_LOCKDOWN_DEFINED_FLAG = 2147483648U;

			// Token: 0x040028C6 RID: 10438
			internal const uint WLDP_LOCKDOWN_SECUREBOOT_FLAG = 1U;

			// Token: 0x040028C7 RID: 10439
			internal const uint WLDP_LOCKDOWN_DEBUGPOLICY_FLAG = 2U;

			// Token: 0x040028C8 RID: 10440
			internal const uint WLDP_LOCKDOWN_UMCIENFORCE_FLAG = 4U;

			// Token: 0x040028C9 RID: 10441
			internal const uint WLDP_LOCKDOWN_UMCIAUDIT_FLAG = 8U;
		}

		// Token: 0x02000818 RID: 2072
		internal enum WLDP_HOST_ID
		{
			// Token: 0x040028CB RID: 10443
			WLDP_HOST_ID_UNKNOWN,
			// Token: 0x040028CC RID: 10444
			WLDP_HOST_ID_GLOBAL,
			// Token: 0x040028CD RID: 10445
			WLDP_HOST_ID_VBA,
			// Token: 0x040028CE RID: 10446
			WLDP_HOST_ID_WSH,
			// Token: 0x040028CF RID: 10447
			WLDP_HOST_ID_POWERSHELL,
			// Token: 0x040028D0 RID: 10448
			WLDP_HOST_ID_IE,
			// Token: 0x040028D1 RID: 10449
			WLDP_HOST_ID_MSI,
			// Token: 0x040028D2 RID: 10450
			WLDP_HOST_ID_MAX
		}

		// Token: 0x02000819 RID: 2073
		internal struct WLDP_HOST_INFORMATION
		{
			// Token: 0x040028D3 RID: 10451
			internal uint dwRevision;

			// Token: 0x040028D4 RID: 10452
			internal SystemPolicy.WLDP_HOST_ID dwHostId;

			// Token: 0x040028D5 RID: 10453
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string szSource;

			// Token: 0x040028D6 RID: 10454
			internal IntPtr hSource;
		}

		// Token: 0x0200081A RID: 2074
		internal class WldpNativeMethods
		{
			// Token: 0x06004FBB RID: 20411
			[DllImport("wldp.dll")]
			internal static extern int WldpGetLockdownPolicy(ref SystemPolicy.WLDP_HOST_INFORMATION pHostInformation, ref uint pdwLockdownState, uint dwFlags);

			// Token: 0x06004FBC RID: 20412
			[DllImport("wldp.dll")]
			internal static extern int WldpIsClassInApprovedList(ref Guid rclsid, ref SystemPolicy.WLDP_HOST_INFORMATION pHostInformation, ref int ptIsApproved, uint dwFlags);

			// Token: 0x06004FBD RID: 20413
			[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, int dwFlags, IntPtr hToken, out IntPtr pszPath);
		}
	}
}
