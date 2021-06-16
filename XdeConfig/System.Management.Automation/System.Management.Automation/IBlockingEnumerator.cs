using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000240 RID: 576
	internal interface IBlockingEnumerator<out W> : IEnumerator<!0>, IDisposable, IEnumerator
	{
		// Token: 0x06001B64 RID: 7012
		bool MoveNext(bool block);
	}
}
