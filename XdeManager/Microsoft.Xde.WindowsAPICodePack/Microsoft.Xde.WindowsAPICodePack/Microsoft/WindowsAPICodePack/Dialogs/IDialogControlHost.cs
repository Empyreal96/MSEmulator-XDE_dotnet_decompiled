using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200001F RID: 31
	public interface IDialogControlHost
	{
		// Token: 0x0600011F RID: 287
		bool IsCollectionChangeAllowed();

		// Token: 0x06000120 RID: 288
		void ApplyCollectionChanged();

		// Token: 0x06000121 RID: 289
		bool IsControlPropertyChangeAllowed(string propertyName, DialogControl control);

		// Token: 0x06000122 RID: 290
		void ApplyControlPropertyChange(string propertyName, DialogControl control);
	}
}
