using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Security.AccessControl;
using Microsoft.PowerShell.Commands.Internal;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000479 RID: 1145
	internal class TransactedRegistryWrapper : IRegistryWrapper
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x001179D6 File Offset: 0x00115BD6
		internal TransactedRegistryWrapper(TransactedRegistryKey txRegKey, CmdletProvider provider)
		{
			this.txRegKey = txRegKey;
			this.provider = provider;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x001179EC File Offset: 0x00115BEC
		public void SetValue(string name, object value)
		{
			using (this.provider.CurrentPSTransaction)
			{
				this.txRegKey.SetValue(name, value);
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x00117A30 File Offset: 0x00115C30
		public void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			using (this.provider.CurrentPSTransaction)
			{
				value = PSObject.Base(value);
				value = RegistryWrapperUtils.ConvertUIntToValueForRegistryIfNeeded(value, valueKind);
				this.txRegKey.SetValue(name, value, valueKind);
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00117A84 File Offset: 0x00115C84
		public string[] GetValueNames()
		{
			string[] valueNames;
			using (this.provider.CurrentPSTransaction)
			{
				valueNames = this.txRegKey.GetValueNames();
			}
			return valueNames;
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00117AC8 File Offset: 0x00115CC8
		public void DeleteValue(string name)
		{
			using (this.provider.CurrentPSTransaction)
			{
				this.txRegKey.DeleteValue(name);
			}
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00117B0C File Offset: 0x00115D0C
		public string[] GetSubKeyNames()
		{
			string[] subKeyNames;
			using (this.provider.CurrentPSTransaction)
			{
				subKeyNames = this.txRegKey.GetSubKeyNames();
			}
			return subKeyNames;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x00117B50 File Offset: 0x00115D50
		public IRegistryWrapper CreateSubKey(string subkey)
		{
			IRegistryWrapper result;
			using (this.provider.CurrentPSTransaction)
			{
				TransactedRegistryKey transactedRegistryKey = this.txRegKey.CreateSubKey(subkey);
				if (transactedRegistryKey == null)
				{
					result = null;
				}
				else
				{
					result = new TransactedRegistryWrapper(transactedRegistryKey, this.provider);
				}
			}
			return result;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x00117BA8 File Offset: 0x00115DA8
		public IRegistryWrapper OpenSubKey(string name, bool writable)
		{
			IRegistryWrapper result;
			using (this.provider.CurrentPSTransaction)
			{
				TransactedRegistryKey transactedRegistryKey = this.txRegKey.OpenSubKey(name, writable);
				if (transactedRegistryKey == null)
				{
					result = null;
				}
				else
				{
					result = new TransactedRegistryWrapper(transactedRegistryKey, this.provider);
				}
			}
			return result;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x00117C00 File Offset: 0x00115E00
		public void DeleteSubKeyTree(string subkey)
		{
			using (this.provider.CurrentPSTransaction)
			{
				this.txRegKey.DeleteSubKeyTree(subkey);
			}
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x00117C44 File Offset: 0x00115E44
		public object GetValue(string name)
		{
			object result;
			using (this.provider.CurrentPSTransaction)
			{
				object obj = this.txRegKey.GetValue(name);
				try
				{
					obj = RegistryWrapperUtils.ConvertValueToUIntFromRegistryIfNeeded(name, obj, this.GetValueKind(name));
				}
				catch (IOException)
				{
				}
				result = obj;
			}
			return result;
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x00117CAC File Offset: 0x00115EAC
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			object result;
			using (this.provider.CurrentPSTransaction)
			{
				object obj = this.txRegKey.GetValue(name, defaultValue, options);
				try
				{
					obj = RegistryWrapperUtils.ConvertValueToUIntFromRegistryIfNeeded(name, obj, this.GetValueKind(name));
				}
				catch (IOException)
				{
				}
				result = obj;
			}
			return result;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00117D14 File Offset: 0x00115F14
		public RegistryValueKind GetValueKind(string name)
		{
			RegistryValueKind valueKind;
			using (this.provider.CurrentPSTransaction)
			{
				valueKind = this.txRegKey.GetValueKind(name);
			}
			return valueKind;
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x00117D58 File Offset: 0x00115F58
		public void Close()
		{
			using (this.provider.CurrentPSTransaction)
			{
				this.txRegKey.Close();
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06003324 RID: 13092 RVA: 0x00117D98 File Offset: 0x00115F98
		public string Name
		{
			get
			{
				string name;
				using (this.provider.CurrentPSTransaction)
				{
					name = this.txRegKey.Name;
				}
				return name;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06003325 RID: 13093 RVA: 0x00117DDC File Offset: 0x00115FDC
		public int SubKeyCount
		{
			get
			{
				int subKeyCount;
				using (this.provider.CurrentPSTransaction)
				{
					subKeyCount = this.txRegKey.SubKeyCount;
				}
				return subKeyCount;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06003326 RID: 13094 RVA: 0x00117E20 File Offset: 0x00116020
		public object RegistryKey
		{
			get
			{
				return this.txRegKey;
			}
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x00117E28 File Offset: 0x00116028
		public void SetAccessControl(ObjectSecurity securityDescriptor)
		{
			using (this.provider.CurrentPSTransaction)
			{
				this.txRegKey.SetAccessControl((TransactedRegistrySecurity)securityDescriptor);
			}
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x00117E70 File Offset: 0x00116070
		public ObjectSecurity GetAccessControl(AccessControlSections includeSections)
		{
			ObjectSecurity accessControl;
			using (this.provider.CurrentPSTransaction)
			{
				accessControl = this.txRegKey.GetAccessControl(includeSections);
			}
			return accessControl;
		}

		// Token: 0x04001A8F RID: 6799
		private TransactedRegistryKey txRegKey;

		// Token: 0x04001A90 RID: 6800
		private CmdletProvider provider;
	}
}
