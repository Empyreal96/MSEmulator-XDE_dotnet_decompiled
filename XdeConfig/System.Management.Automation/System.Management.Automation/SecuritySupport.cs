using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.PowerShell;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x02000808 RID: 2056
	internal static class SecuritySupport
	{
		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06004F53 RID: 20307 RVA: 0x001A48AC File Offset: 0x001A2AAC
		internal static ExecutionPolicyScope[] ExecutionPolicyScopePreferences
		{
			get
			{
				return new ExecutionPolicyScope[]
				{
					ExecutionPolicyScope.MachinePolicy,
					ExecutionPolicyScope.UserPolicy,
					ExecutionPolicyScope.Process,
					ExecutionPolicyScope.CurrentUser,
					ExecutionPolicyScope.LocalMachine
				};
			}
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x001A48D4 File Offset: 0x001A2AD4
		internal static void SetExecutionPolicy(ExecutionPolicyScope scope, ExecutionPolicy policy, string shellId)
		{
			string value = "Restricted";
			string registryConfigurationPath = Utils.GetRegistryConfigurationPath(shellId);
			switch (policy)
			{
			case ExecutionPolicy.Unrestricted:
				value = "Unrestricted";
				break;
			case ExecutionPolicy.RemoteSigned:
				value = "RemoteSigned";
				break;
			case ExecutionPolicy.AllSigned:
				value = "AllSigned";
				break;
			case ExecutionPolicy.Restricted:
				value = "Restricted";
				break;
			case ExecutionPolicy.Bypass:
				value = "Bypass";
				break;
			}
			switch (scope)
			{
			case ExecutionPolicyScope.Process:
				if (policy == ExecutionPolicy.Undefined)
				{
					value = null;
				}
				Environment.SetEnvironmentVariable("PSExecutionPolicyPreference", value);
				return;
			case ExecutionPolicyScope.CurrentUser:
				if (policy == ExecutionPolicy.Undefined)
				{
					using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(registryConfigurationPath, true))
					{
						if (registryKey != null && registryKey.GetValue("ExecutionPolicy") != null)
						{
							registryKey.DeleteValue("ExecutionPolicy");
						}
					}
					SecuritySupport.CleanKeyParents(Registry.CurrentUser, registryConfigurationPath);
					return;
				}
				using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(registryConfigurationPath))
				{
					registryKey2.SetValue("ExecutionPolicy", value, RegistryValueKind.String);
					return;
				}
				break;
			case ExecutionPolicyScope.LocalMachine:
				break;
			default:
				return;
			}
			if (policy == ExecutionPolicy.Undefined)
			{
				using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey(registryConfigurationPath, true))
				{
					if (registryKey3 != null && registryKey3.GetValue("ExecutionPolicy") != null)
					{
						registryKey3.DeleteValue("ExecutionPolicy");
					}
				}
				SecuritySupport.CleanKeyParents(Registry.LocalMachine, registryConfigurationPath);
				return;
			}
			using (RegistryKey registryKey4 = Registry.LocalMachine.CreateSubKey(registryConfigurationPath))
			{
				registryKey4.SetValue("ExecutionPolicy", value, RegistryValueKind.String);
			}
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x001A4A70 File Offset: 0x001A2C70
		private static void CleanKeyParents(RegistryKey baseKey, string keyPath)
		{
			using (RegistryKey registryKey = baseKey.OpenSubKey(keyPath, true))
			{
				if (registryKey == null || (registryKey.ValueCount == 0 && registryKey.SubKeyCount == 0))
				{
					string[] array = keyPath.Split(new char[]
					{
						'\\'
					});
					if (array.Length > 2)
					{
						string text = array[array.Length - 1];
						string text2 = keyPath.Remove(keyPath.Length - text.Length - 1);
						if (registryKey != null)
						{
							using (RegistryKey registryKey2 = baseKey.OpenSubKey(text2, true))
							{
								registryKey2.DeleteSubKey(text, true);
							}
						}
						SecuritySupport.CleanKeyParents(baseKey, text2);
					}
				}
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x001A4B30 File Offset: 0x001A2D30
		internal static ExecutionPolicy GetExecutionPolicy(string shellId)
		{
			foreach (ExecutionPolicyScope scope in SecuritySupport.ExecutionPolicyScopePreferences)
			{
				ExecutionPolicy executionPolicy = SecuritySupport.GetExecutionPolicy(shellId, scope);
				if (executionPolicy != ExecutionPolicy.Undefined)
				{
					return executionPolicy;
				}
			}
			return ExecutionPolicy.Restricted;
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x001A4B70 File Offset: 0x001A2D70
		internal static ExecutionPolicy GetExecutionPolicy(string shellId, ExecutionPolicyScope scope)
		{
			switch (scope)
			{
			case ExecutionPolicyScope.Process:
			{
				string environmentVariable = Environment.GetEnvironmentVariable("PSExecutionPolicyPreference");
				if (!string.IsNullOrEmpty(environmentVariable))
				{
					return SecuritySupport.ParseExecutionPolicy(environmentVariable);
				}
				return ExecutionPolicy.Undefined;
			}
			case ExecutionPolicyScope.CurrentUser:
			case ExecutionPolicyScope.LocalMachine:
			{
				string localPreferenceValue = SecuritySupport.GetLocalPreferenceValue(shellId, scope);
				if (!string.IsNullOrEmpty(localPreferenceValue))
				{
					return SecuritySupport.ParseExecutionPolicy(localPreferenceValue);
				}
				return ExecutionPolicy.Undefined;
			}
			case ExecutionPolicyScope.UserPolicy:
			case ExecutionPolicyScope.MachinePolicy:
			{
				string groupPolicyValue = SecuritySupport.GetGroupPolicyValue(shellId, scope);
				if (!string.IsNullOrEmpty(groupPolicyValue))
				{
					Process process = Process.GetCurrentProcess();
					string a = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "gpscript.exe");
					bool flag = false;
					try
					{
						while (process != null)
						{
							if (string.Equals(a, PsUtils.GetMainModule(process).FileName, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
								break;
							}
							process = PsUtils.GetParentProcess(process);
						}
					}
					catch (Win32Exception)
					{
					}
					if (!flag)
					{
						return SecuritySupport.ParseExecutionPolicy(groupPolicyValue);
					}
				}
				return ExecutionPolicy.Undefined;
			}
			default:
				return ExecutionPolicy.Restricted;
			}
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001A4C4C File Offset: 0x001A2E4C
		internal static ExecutionPolicy ParseExecutionPolicy(string policy)
		{
			if (string.Equals(policy, "Bypass", StringComparison.OrdinalIgnoreCase))
			{
				return ExecutionPolicy.Bypass;
			}
			if (string.Equals(policy, "Unrestricted", StringComparison.OrdinalIgnoreCase))
			{
				return ExecutionPolicy.Unrestricted;
			}
			if (string.Equals(policy, "RemoteSigned", StringComparison.OrdinalIgnoreCase))
			{
				return ExecutionPolicy.RemoteSigned;
			}
			if (string.Equals(policy, "AllSigned", StringComparison.OrdinalIgnoreCase))
			{
				return ExecutionPolicy.AllSigned;
			}
			if (string.Equals(policy, "Restricted", StringComparison.OrdinalIgnoreCase))
			{
				return ExecutionPolicy.Restricted;
			}
			return ExecutionPolicy.Restricted;
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001A4CAC File Offset: 0x001A2EAC
		internal static string GetExecutionPolicy(ExecutionPolicy policy)
		{
			switch (policy)
			{
			case ExecutionPolicy.Unrestricted:
				return "Unrestricted";
			case ExecutionPolicy.RemoteSigned:
				return "RemoteSigned";
			case ExecutionPolicy.AllSigned:
				return "AllSigned";
			case ExecutionPolicy.Restricted:
				return "Restricted";
			case ExecutionPolicy.Bypass:
				return "Bypass";
			default:
				return "Restricted";
			}
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x001A4CFC File Offset: 0x001A2EFC
		internal static bool IsProductBinary(string file)
		{
			if (string.IsNullOrEmpty(file) || !File.Exists(file))
			{
				return false;
			}
			if (!Utils.IsUnderProductFolder(file))
			{
				return false;
			}
			Signature signature = SignatureHelper.GetSignature(file, null);
			return (signature != null && signature.IsOSBinary) || (Signature.CatalogApiAvailable != null && !Signature.CatalogApiAvailable.Value);
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x001A4D58 File Offset: 0x001A2F58
		[ArchitectureSensitive]
		internal static SaferPolicy GetSaferPolicy(string path, SafeHandle handle)
		{
			SaferPolicy result = SaferPolicy.Allowed;
			SAFER_CODE_PROPERTIES safer_CODE_PROPERTIES = default(SAFER_CODE_PROPERTIES);
			safer_CODE_PROPERTIES.cbSize = (uint)Marshal.SizeOf(typeof(SAFER_CODE_PROPERTIES));
			safer_CODE_PROPERTIES.dwCheckFlags = 13U;
			safer_CODE_PROPERTIES.ImagePath = path;
			if (handle != null)
			{
				safer_CODE_PROPERTIES.hImageFileHandle = handle.DangerousGetHandle();
			}
			safer_CODE_PROPERTIES.dwWVTUIChoice = 2U;
			IntPtr intPtr;
			if (NativeMethods.SaferIdentifyLevel(1U, ref safer_CODE_PROPERTIES, out intPtr, "SCRIPT"))
			{
				IntPtr zero = IntPtr.Zero;
				try
				{
					if (!NativeMethods.SaferComputeTokenFromLevel(intPtr, IntPtr.Zero, ref zero, 1U, IntPtr.Zero))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if (lastWin32Error != 1260 && lastWin32Error != 786)
						{
							throw new Win32Exception();
						}
						result = SaferPolicy.Disallowed;
					}
					else if (zero == IntPtr.Zero)
					{
						result = SaferPolicy.Allowed;
					}
					else
					{
						result = SaferPolicy.Disallowed;
						NativeMethods.CloseHandle(zero);
					}
					return result;
				}
				finally
				{
					NativeMethods.SaferCloseLevel(intPtr);
				}
			}
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x001A4E40 File Offset: 0x001A3040
		private static string GetGroupPolicyValue(string shellId, ExecutionPolicyScope scope)
		{
			RegistryKey registryKey = null;
			switch (scope)
			{
			case ExecutionPolicyScope.UserPolicy:
				registryKey = Registry.CurrentUser;
				break;
			case ExecutionPolicyScope.MachinePolicy:
				registryKey = Registry.LocalMachine;
				break;
			}
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting(".", new RegistryKey[]
			{
				registryKey
			});
			if (groupPolicySetting == null)
			{
				return null;
			}
			object obj = null;
			if (groupPolicySetting.TryGetValue("EnableScripts", out obj))
			{
				if (string.Equals(obj.ToString(), "0", StringComparison.OrdinalIgnoreCase))
				{
					return "Restricted";
				}
				if (string.Equals(obj.ToString(), "1", StringComparison.OrdinalIgnoreCase))
				{
					object obj2 = null;
					if (groupPolicySetting.TryGetValue("ExecutionPolicy", out obj2))
					{
						return obj2.ToString();
					}
				}
			}
			return null;
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x001A4EE8 File Offset: 0x001A30E8
		private static string GetLocalPreferenceValue(string shellId, ExecutionPolicyScope scope)
		{
			string registryConfigurationPath = Utils.GetRegistryConfigurationPath(shellId);
			switch (scope)
			{
			case ExecutionPolicyScope.CurrentUser:
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(registryConfigurationPath))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue("ExecutionPolicy");
						string result = value as string;
						registryKey.Dispose();
						return result;
					}
					goto IL_99;
				}
				break;
			case ExecutionPolicyScope.LocalMachine:
				break;
			default:
				goto IL_99;
			}
			using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(registryConfigurationPath))
			{
				if (registryKey2 != null)
				{
					object value2 = registryKey2.GetValue("ExecutionPolicy");
					string result2 = value2 as string;
					registryKey2.Dispose();
					return result2;
				}
			}
			IL_99:
			return null;
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x001A4FB0 File Offset: 0x001A31B0
		internal static void CheckIfFileExists(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(filePath);
			}
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x001A4FC1 File Offset: 0x001A31C1
		internal static bool CertIsGoodForSigning(X509Certificate2 c)
		{
			return SecuritySupport.CertHasPrivatekey(c) && SecuritySupport.CertHasOid(c, "1.3.6.1.5.5.7.3.3");
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x001A4FD8 File Offset: 0x001A31D8
		internal static bool CertIsGoodForEncryption(X509Certificate2 c)
		{
			return SecuritySupport.CertHasOid(c, "1.3.6.1.4.1.311.80.1") && (SecuritySupport.CertHasKeyUsage(c, X509KeyUsageFlags.DataEncipherment) || SecuritySupport.CertHasKeyUsage(c, X509KeyUsageFlags.KeyEncipherment));
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x001A5000 File Offset: 0x001A3200
		private static bool CertHasOid(X509Certificate2 c, string oid)
		{
			Collection<string> certEKU = SecuritySupport.GetCertEKU(c);
			foreach (string a in certEKU)
			{
				if (a == oid)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001A5058 File Offset: 0x001A3258
		private static bool CertHasKeyUsage(X509Certificate2 c, X509KeyUsageFlags keyUsage)
		{
			foreach (X509Extension x509Extension in c.Extensions)
			{
				X509KeyUsageExtension x509KeyUsageExtension = x509Extension as X509KeyUsageExtension;
				if (x509KeyUsageExtension != null)
				{
					if ((x509KeyUsageExtension.KeyUsages & keyUsage) == keyUsage)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x001A509E File Offset: 0x001A329E
		internal static bool CertHasPrivatekey(X509Certificate2 cert)
		{
			return cert.HasPrivateKey;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x001A50A8 File Offset: 0x001A32A8
		[ArchitectureSensitive]
		internal static Collection<string> GetCertEKU(X509Certificate2 cert)
		{
			Collection<string> collection = new Collection<string>();
			IntPtr handle = cert.Handle;
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			if (NativeMethods.CertGetEnhancedKeyUsage(handle, 0U, zero, out num))
			{
				if (num > 0)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(num);
					try
					{
						if (NativeMethods.CertGetEnhancedKeyUsage(handle, 0U, intPtr, out num))
						{
							NativeMethods.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = ClrFacade.PtrToStructure<NativeMethods.CERT_ENHKEY_USAGE>(intPtr);
							IntPtr rgpszUsageIdentifier = cert_ENHKEY_USAGE.rgpszUsageIdentifier;
							int num2 = 0;
							while ((long)num2 < (long)((ulong)cert_ENHKEY_USAGE.cUsageIdentifier))
							{
								IntPtr ptr = Marshal.ReadIntPtr(rgpszUsageIdentifier, num2 * Marshal.SizeOf(rgpszUsageIdentifier));
								string item = Marshal.PtrToStringAnsi(ptr);
								collection.Add(item);
								num2++;
							}
							return collection;
						}
						throw new Win32Exception(Marshal.GetLastWin32Error());
					}
					finally
					{
						Marshal.FreeHGlobal(intPtr);
					}
					goto IL_AA;
				}
				return collection;
			}
			IL_AA:
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x001A517C File Offset: 0x001A337C
		internal static uint GetDWORDFromInt(int n)
		{
			return BitConverter.ToUInt32(BitConverter.GetBytes(n), 0);
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x001A5198 File Offset: 0x001A3398
		internal static int GetIntFromDWORD(uint n)
		{
			long num = (long)((ulong)n - 4294967296UL);
			return (int)num;
		}
	}
}
