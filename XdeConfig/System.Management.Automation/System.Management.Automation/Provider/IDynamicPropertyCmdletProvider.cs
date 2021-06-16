using System;

namespace System.Management.Automation.Provider
{
	// Token: 0x0200046B RID: 1131
	public interface IDynamicPropertyCmdletProvider : IPropertyCmdletProvider
	{
		// Token: 0x06003246 RID: 12870
		void NewProperty(string path, string propertyName, string propertyTypeName, object value);

		// Token: 0x06003247 RID: 12871
		object NewPropertyDynamicParameters(string path, string propertyName, string propertyTypeName, object value);

		// Token: 0x06003248 RID: 12872
		void RemoveProperty(string path, string propertyName);

		// Token: 0x06003249 RID: 12873
		object RemovePropertyDynamicParameters(string path, string propertyName);

		// Token: 0x0600324A RID: 12874
		void RenameProperty(string path, string sourceProperty, string destinationProperty);

		// Token: 0x0600324B RID: 12875
		object RenamePropertyDynamicParameters(string path, string sourceProperty, string destinationProperty);

		// Token: 0x0600324C RID: 12876
		void CopyProperty(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty);

		// Token: 0x0600324D RID: 12877
		object CopyPropertyDynamicParameters(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty);

		// Token: 0x0600324E RID: 12878
		void MoveProperty(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty);

		// Token: 0x0600324F RID: 12879
		object MovePropertyDynamicParameters(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty);
	}
}
