using System;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x02000082 RID: 130
	internal interface IReorderingBuffer
	{
		// Token: 0x060003FA RID: 1018
		void IgnoreItem(long id);
	}
}
