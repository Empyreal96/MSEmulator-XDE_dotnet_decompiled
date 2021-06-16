using System;
using System.Security.AccessControl;

namespace System.Management.Automation.Provider
{
	// Token: 0x0200046C RID: 1132
	public interface ISecurityDescriptorCmdletProvider
	{
		// Token: 0x06003250 RID: 12880
		void GetSecurityDescriptor(string path, AccessControlSections includeSections);

		// Token: 0x06003251 RID: 12881
		void SetSecurityDescriptor(string path, ObjectSecurity securityDescriptor);

		// Token: 0x06003252 RID: 12882
		ObjectSecurity NewSecurityDescriptorFromPath(string path, AccessControlSections includeSections);

		// Token: 0x06003253 RID: 12883
		ObjectSecurity NewSecurityDescriptorOfType(string type, AccessControlSections includeSections);
	}
}
