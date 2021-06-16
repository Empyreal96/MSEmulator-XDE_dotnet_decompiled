using System;
using System.IO;
using System.Management.Automation;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000478 RID: 1144
	internal class RegistryWrapper : IRegistryWrapper
	{
		// Token: 0x06003305 RID: 13061 RVA: 0x00117830 File Offset: 0x00115A30
		internal RegistryWrapper(RegistryKey regKey)
		{
			this.regKey = regKey;
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x0011783F File Offset: 0x00115A3F
		public void SetValue(string name, object value)
		{
			this.regKey.SetValue(name, value);
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x0011784E File Offset: 0x00115A4E
		public void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			value = PSObject.Base(value);
			value = RegistryWrapperUtils.ConvertUIntToValueForRegistryIfNeeded(value, valueKind);
			this.regKey.SetValue(name, value, valueKind);
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x0011786F File Offset: 0x00115A6F
		public string[] GetValueNames()
		{
			return this.regKey.GetValueNames();
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x0011787C File Offset: 0x00115A7C
		public void DeleteValue(string name)
		{
			this.regKey.DeleteValue(name);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x0011788A File Offset: 0x00115A8A
		public string[] GetSubKeyNames()
		{
			return this.regKey.GetSubKeyNames();
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00117898 File Offset: 0x00115A98
		public IRegistryWrapper CreateSubKey(string subkey)
		{
			RegistryKey registryKey = this.regKey.CreateSubKey(subkey);
			if (registryKey == null)
			{
				return null;
			}
			return new RegistryWrapper(registryKey);
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x001178C0 File Offset: 0x00115AC0
		public IRegistryWrapper OpenSubKey(string name, bool writable)
		{
			RegistryKey registryKey = this.regKey.OpenSubKey(name, writable);
			if (registryKey == null)
			{
				return null;
			}
			return new RegistryWrapper(registryKey);
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x001178E6 File Offset: 0x00115AE6
		public void DeleteSubKeyTree(string subkey)
		{
			this.regKey.DeleteSubKeyTree(subkey);
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x001178F4 File Offset: 0x00115AF4
		public object GetValue(string name)
		{
			object obj = this.regKey.GetValue(name);
			try
			{
				obj = RegistryWrapperUtils.ConvertValueToUIntFromRegistryIfNeeded(name, obj, this.GetValueKind(name));
			}
			catch (IOException)
			{
			}
			return obj;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00117934 File Offset: 0x00115B34
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			object obj = this.regKey.GetValue(name, defaultValue, options);
			try
			{
				obj = RegistryWrapperUtils.ConvertValueToUIntFromRegistryIfNeeded(name, obj, this.GetValueKind(name));
			}
			catch (IOException)
			{
			}
			return obj;
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00117978 File Offset: 0x00115B78
		public RegistryValueKind GetValueKind(string name)
		{
			return this.regKey.GetValueKind(name);
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x00117986 File Offset: 0x00115B86
		public void Close()
		{
			this.regKey.Dispose();
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x00117993 File Offset: 0x00115B93
		public string Name
		{
			get
			{
				return this.regKey.Name;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x001179A0 File Offset: 0x00115BA0
		public int SubKeyCount
		{
			get
			{
				return this.regKey.SubKeyCount;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x001179AD File Offset: 0x00115BAD
		public object RegistryKey
		{
			get
			{
				return this.regKey;
			}
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x001179B5 File Offset: 0x00115BB5
		public void SetAccessControl(ObjectSecurity securityDescriptor)
		{
			this.regKey.SetAccessControl((RegistrySecurity)securityDescriptor);
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x001179C8 File Offset: 0x00115BC8
		public ObjectSecurity GetAccessControl(AccessControlSections includeSections)
		{
			return this.regKey.GetAccessControl(includeSections);
		}

		// Token: 0x04001A8E RID: 6798
		private RegistryKey regKey;
	}
}
