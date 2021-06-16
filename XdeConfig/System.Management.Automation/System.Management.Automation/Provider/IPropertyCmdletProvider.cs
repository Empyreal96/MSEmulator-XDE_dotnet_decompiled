using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Provider
{
	// Token: 0x0200046A RID: 1130
	public interface IPropertyCmdletProvider
	{
		// Token: 0x06003240 RID: 12864
		void GetProperty(string path, Collection<string> providerSpecificPickList);

		// Token: 0x06003241 RID: 12865
		object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList);

		// Token: 0x06003242 RID: 12866
		void SetProperty(string path, PSObject propertyValue);

		// Token: 0x06003243 RID: 12867
		object SetPropertyDynamicParameters(string path, PSObject propertyValue);

		// Token: 0x06003244 RID: 12868
		void ClearProperty(string path, Collection<string> propertyToClear);

		// Token: 0x06003245 RID: 12869
		object ClearPropertyDynamicParameters(string path, Collection<string> propertyToClear);
	}
}
