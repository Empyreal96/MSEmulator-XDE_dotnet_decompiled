using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200003C RID: 60
	public interface IUiOperations
	{
		// Token: 0x06000170 RID: 368
		void ExecuteOnUiThread(Action action);

		// Token: 0x06000171 RID: 369
		void AsyncExecuteOnUiThread(Action action);
	}
}
