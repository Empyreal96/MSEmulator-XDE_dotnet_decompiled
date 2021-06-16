using System;
using System.Runtime.InteropServices;

namespace System.Threading.Tasks.Dataflow.Internal
{
	// Token: 0x0200007F RID: 127
	[StructLayout(LayoutKind.Explicit, Size = 256)]
	internal struct PaddedInt64
	{
		// Token: 0x0400018B RID: 395
		[FieldOffset(128)]
		internal long Value;
	}
}
