using System;

namespace CSharpx
{
	// Token: 0x02000028 RID: 40
	internal sealed class Nothing<T> : Maybe<T>
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00004CD1 File Offset: 0x00002ED1
		internal Nothing() : base(MaybeType.Nothing)
		{
		}
	}
}
