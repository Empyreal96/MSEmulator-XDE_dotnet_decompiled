using System;
using Microsoft.Win32;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200005F RID: 95
	public static class RegistryHelper
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00004F43 File Offset: 0x00003143
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00004F5E File Offset: 0x0000315E
		public static bool RestartPending
		{
			get
			{
				return RegistryHelper.GetValue<int>(RegistryView.Default, RegistryHive.LocalMachine, "Software\\Microsoft\\Xde\\Volatile", "RequireReboot", 0) != 0;
			}
			set
			{
				if (value)
				{
					RegistryHelper.SetValue(RegistryView.Default, RegistryHive.LocalMachine, "Software\\Microsoft\\Xde\\Volatile", "RequireReboot", 1, true);
					return;
				}
				throw new ArgumentException(Strings.InvalidParameterValue, "value");
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00004F8F File Offset: 0x0000318F
		public static int TelemetryLevel
		{
			get
			{
				return RegistryHelper.GetValue<int>(RegistryView.Default, RegistryHive.LocalMachine, "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection", "AllowTelemetry", 1);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00004FA7 File Offset: 0x000031A7
		public static bool KitsCeipOptedIn
		{
			get
			{
				return RegistryHelper.GetValue<int>(RegistryView.Registry32, RegistryHive.LocalMachine, "Software\\Microsoft\\Windows Kits\\CEIP", "OptIn", 0) != 0;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00004FC6 File Offset: 0x000031C6
		public static bool TelemetryDisabled
		{
			get
			{
				return RegistryHelper.GetValue<int>("TelemetryDisabled", 0) != 0;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00004FD8 File Offset: 0x000031D8
		public static T GetValue<T>(string valueKey)
		{
			return RegistryHelper.GetValue<T>(RegistryHelper.XdeRegistryRoot, valueKey, default(T));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00004FF9 File Offset: 0x000031F9
		public static T GetValue<T>(string valueKey, T defaultValue)
		{
			return RegistryHelper.GetValue<T>(RegistryHelper.XdeRegistryRoot, valueKey, defaultValue);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00005008 File Offset: 0x00003208
		public static T GetValue<T>(string keyName, string valueName, T defaultValue)
		{
			object value = Registry.GetValue(keyName, valueName, defaultValue);
			if (value != null)
			{
				return (T)((object)value);
			}
			return defaultValue;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00005030 File Offset: 0x00003230
		public static T GetValue<T>(RegistryView view, RegistryHive hive, string subKeyName, string valueName, T defaultValue)
		{
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(hive, view))
			{
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(subKeyName))
				{
					if (registryKey2 != null)
					{
						object value = registryKey2.GetValue(valueName, defaultValue);
						if (value != null)
						{
							return (T)((object)value);
						}
					}
				}
			}
			return defaultValue;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000050A4 File Offset: 0x000032A4
		public static void SetValue(RegistryView view, RegistryHive hive, string subKeyName, string valueName, object value)
		{
			RegistryHelper.SetValue(view, hive, subKeyName, valueName, value, false);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000050B4 File Offset: 0x000032B4
		public static void SetValue(RegistryView view, RegistryHive hive, string subKeyName, string valueName, object value, bool volatileKey)
		{
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(hive, view))
			{
				using (RegistryKey registryKey2 = registryKey.CreateSubKey(subKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree, volatileKey ? RegistryOptions.Volatile : RegistryOptions.None))
				{
					registryKey2.SetValue(valueName, value);
				}
			}
		}

		// Token: 0x0400014D RID: 333
		public const string DisableWP81NetworkStackValueName = "DisableWP81NetworkStack";

		// Token: 0x0400014E RID: 334
		public const string DisableNatValueName = "DisableNAT";

		// Token: 0x0400014F RID: 335
		public const string DisableDefaultSwitchValueName = "DisableDefaultSwitch";

		// Token: 0x04000150 RID: 336
		public const string DisableGpuValueName = "DisableGpu";

		// Token: 0x04000151 RID: 337
		public const int SettingOffValue = 0;

		// Token: 0x04000152 RID: 338
		public const int SettingOnValue = 1;

		// Token: 0x04000153 RID: 339
		public const int TelemetryLevelOff = 1;

		// Token: 0x04000154 RID: 340
		public const int TelemetryLevelOn = 3;

		// Token: 0x04000155 RID: 341
		public static readonly string XdeRegistryRoot = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\XDE\\" + Globals.XdeVersionShort;

		// Token: 0x04000156 RID: 342
		public static readonly string XdeWowRegistryRoot = "HKEY_LOCAL_MACHINE\\Software\\Wow6432Node\\Microsoft\\XDE\\" + Globals.XdeVersionShort;
	}
}
