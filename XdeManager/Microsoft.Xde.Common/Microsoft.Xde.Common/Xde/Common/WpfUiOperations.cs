using System;
using System.Windows.Threading;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000077 RID: 119
	public class WpfUiOperations : IUiOperations
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x00007A9E File Offset: 0x00005C9E
		public WpfUiOperations(Dispatcher dispatcher)
		{
			this.dispatcher = dispatcher;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00007AAD File Offset: 0x00005CAD
		public void ExecuteOnUiThread(Action action)
		{
			this.dispatcher.Invoke(action);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00007ABB File Offset: 0x00005CBB
		public void AsyncExecuteOnUiThread(Action action)
		{
			this.dispatcher.BeginInvoke(action, Array.Empty<object>());
		}

		// Token: 0x040001B3 RID: 435
		private Dispatcher dispatcher;
	}
}
