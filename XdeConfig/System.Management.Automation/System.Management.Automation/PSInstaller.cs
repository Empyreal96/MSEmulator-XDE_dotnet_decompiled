using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x0200085A RID: 2138
	public abstract class PSInstaller : Installer
	{
		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06005246 RID: 21062 RVA: 0x001B7724 File Offset: 0x001B5924
		private static string[] MshRegistryRoots
		{
			get
			{
				return new string[]
				{
					"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\PowerShell\\" + PSVersionInfo.RegistryVersion1Key + "\\"
				};
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06005247 RID: 21063
		internal abstract string RegKey { get; }

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06005248 RID: 21064
		internal abstract Dictionary<string, object> RegValues { get; }

		// Token: 0x06005249 RID: 21065 RVA: 0x001B7750 File Offset: 0x001B5950
		public sealed override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);
			this.WriteRegistry();
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x001B7760 File Offset: 0x001B5960
		private void WriteRegistry()
		{
			foreach (string str in PSInstaller.MshRegistryRoots)
			{
				RegistryKey registryKey = this.GetRegistryKey(str + this.RegKey);
				foreach (string text in this.RegValues.Keys)
				{
					registryKey.SetValue(text, this.RegValues[text]);
				}
			}
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x001B77F8 File Offset: 0x001B59F8
		private RegistryKey GetRegistryKey(string keyPath)
		{
			RegistryKey rootHive = PSInstaller.GetRootHive(keyPath);
			if (rootHive == null)
			{
				return null;
			}
			return rootHive.CreateSubKey(PSInstaller.GetSubkeyPath(keyPath));
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x001B7820 File Offset: 0x001B5A20
		private static string GetSubkeyPath(string keyPath)
		{
			int num = keyPath.IndexOf('\\');
			if (num > 0)
			{
				return keyPath.Substring(num + 1);
			}
			return null;
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x001B7848 File Offset: 0x001B5A48
		private static RegistryKey GetRootHive(string keyPath)
		{
			int num = keyPath.IndexOf('\\');
			string text;
			if (num > 0)
			{
				text = keyPath.Substring(0, num);
			}
			else
			{
				text = keyPath;
			}
			string a;
			if ((a = text.ToUpperInvariant()) != null)
			{
				if (a == "HKEY_CURRENT_USER")
				{
					return Registry.CurrentUser;
				}
				if (a == "HKEY_LOCAL_MACHINE")
				{
					return Registry.LocalMachine;
				}
				if (a == "HKEY_CLASSES_ROOT")
				{
					return Registry.ClassesRoot;
				}
				if (a == "HKEY_CURRENT_CONFIG")
				{
					return Registry.CurrentConfig;
				}
				if (a == "HKEY_PERFORMANCE_DATA")
				{
					return Registry.PerformanceData;
				}
				if (a == "HKEY_USERS")
				{
					return Registry.Users;
				}
			}
			return null;
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x001B78F0 File Offset: 0x001B5AF0
		public sealed override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
			if (base.Context != null && base.Context.Parameters != null && base.Context.Parameters.ContainsKey("RegFile"))
			{
				string value = base.Context.Parameters["RegFile"];
				if (!string.IsNullOrEmpty(value))
				{
					return;
				}
			}
			int num = this.RegKey.LastIndexOf('\\');
			string str;
			string subkey;
			if (num >= 0)
			{
				str = this.RegKey.Substring(0, num);
				subkey = this.RegKey.Substring(num + 1);
			}
			else
			{
				str = "";
				subkey = this.RegKey;
			}
			foreach (string str2 in PSInstaller.MshRegistryRoots)
			{
				RegistryKey registryKey = this.GetRegistryKey(str2 + str);
				registryKey.DeleteSubKey(subkey);
			}
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x001B79C6 File Offset: 0x001B5BC6
		public sealed override void Rollback(IDictionary savedState)
		{
			this.Uninstall(savedState);
		}
	}
}
