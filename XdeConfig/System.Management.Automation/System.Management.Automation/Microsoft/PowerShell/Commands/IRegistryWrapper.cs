using System;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000476 RID: 1142
	internal interface IRegistryWrapper
	{
		// Token: 0x060032F2 RID: 13042
		void SetValue(string name, object value);

		// Token: 0x060032F3 RID: 13043
		void SetValue(string name, object value, RegistryValueKind valueKind);

		// Token: 0x060032F4 RID: 13044
		string[] GetValueNames();

		// Token: 0x060032F5 RID: 13045
		void DeleteValue(string name);

		// Token: 0x060032F6 RID: 13046
		string[] GetSubKeyNames();

		// Token: 0x060032F7 RID: 13047
		IRegistryWrapper CreateSubKey(string subkey);

		// Token: 0x060032F8 RID: 13048
		IRegistryWrapper OpenSubKey(string name, bool writable);

		// Token: 0x060032F9 RID: 13049
		void DeleteSubKeyTree(string subkey);

		// Token: 0x060032FA RID: 13050
		object GetValue(string name);

		// Token: 0x060032FB RID: 13051
		object GetValue(string name, object defaultValue, RegistryValueOptions options);

		// Token: 0x060032FC RID: 13052
		RegistryValueKind GetValueKind(string name);

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x060032FD RID: 13053
		object RegistryKey { get; }

		// Token: 0x060032FE RID: 13054
		void SetAccessControl(ObjectSecurity securityDescriptor);

		// Token: 0x060032FF RID: 13055
		ObjectSecurity GetAccessControl(AccessControlSections includeSections);

		// Token: 0x06003300 RID: 13056
		void Close();

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06003301 RID: 13057
		string Name { get; }

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06003302 RID: 13058
		int SubKeyCount { get; }
	}
}
