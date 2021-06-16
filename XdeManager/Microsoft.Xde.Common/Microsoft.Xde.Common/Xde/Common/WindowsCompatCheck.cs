using System;
using System.Management;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000076 RID: 118
	public class WindowsCompatCheck : IWindowsCompatCheck
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x0000787B File Offset: 0x00005A7B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public bool IsWindows10OrBetter()
		{
			return Environment.OSVersion.Version >= WindowsCompatCheck.Windows10Version;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00007894 File Offset: 0x00005A94
		public bool IsBuildEqualOrGreater(int buildNumber)
		{
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"))
			{
				if (registryKey != null)
				{
					return int.Parse((string)registryKey.GetValue("CurrentBuild")) >= buildNumber;
				}
			}
			return false;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000078F4 File Offset: 0x00005AF4
		public bool IsBuildEqualOrGreater(MinBuildVersion minBuildVersion)
		{
			return this.IsBuildEqualOrGreater((int)minBuildVersion);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000078FD File Offset: 0x00005AFD
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public bool Is64BitWindows()
		{
			return Environment.Is64BitOperatingSystem;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00007904 File Offset: 0x00005B04
		public bool IsSlatPresent()
		{
			if (this.IsHypervisorPresent())
			{
				return true;
			}
			bool result;
			using (ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), "select * from win32_processor", 0, 100))
			{
				result = (instanceHelper != null && (bool)instanceHelper["SecondLevelAddressTranslationExtensions"]);
			}
			return result;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00007964 File Offset: 0x00005B64
		public bool IsHypervisorPresent()
		{
			bool result;
			using (ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), "select * from win32_computersystem", 0, 100))
			{
				result = (instanceHelper != null && (bool)instanceHelper["HypervisorPresent"]);
			}
			return result;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000079B8 File Offset: 0x00005BB8
		public bool IsHyperVManagementServiceRunning()
		{
			bool result;
			using (ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), "select * from win32_service where name='vmms'", 0, 100))
			{
				result = (instanceHelper != null && (bool)instanceHelper["Started"]);
			}
			return result;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00007A0C File Offset: 0x00005C0C
		public bool IsHyperVFeatureInstalled()
		{
			return this.GetHyperVFeatureState() == WindowsCompatCheck.OptionalFeatureState.Enabled;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00007A18 File Offset: 0x00005C18
		public bool IsHyperVFeatureOptionAvailable()
		{
			WindowsCompatCheck.OptionalFeatureState hyperVFeatureState = this.GetHyperVFeatureState();
			return hyperVFeatureState == WindowsCompatCheck.OptionalFeatureState.Enabled || hyperVFeatureState == WindowsCompatCheck.OptionalFeatureState.Disabled;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00007A38 File Offset: 0x00005C38
		private WindowsCompatCheck.OptionalFeatureState GetHyperVFeatureState()
		{
			using (ManagementObject instanceHelper = WmiBaseUtils.GetInstanceHelper(WmiBaseUtils.GetCimv2Scope(), "select * from Win32_OptionalFeature where caption = 'Hyper-V'", 5, 250))
			{
				if (instanceHelper != null)
				{
					return (WindowsCompatCheck.OptionalFeatureState)((uint)instanceHelper["InstallState"]);
				}
			}
			return WindowsCompatCheck.OptionalFeatureState.Absent;
		}

		// Token: 0x040001B2 RID: 434
		private static readonly Version Windows10Version = new Version(6, 4);

		// Token: 0x0200008A RID: 138
		private enum OptionalFeatureState
		{
			// Token: 0x040001DF RID: 479
			Enabled = 1,
			// Token: 0x040001E0 RID: 480
			Disabled,
			// Token: 0x040001E1 RID: 481
			Absent,
			// Token: 0x040001E2 RID: 482
			Unknown
		}
	}
}
