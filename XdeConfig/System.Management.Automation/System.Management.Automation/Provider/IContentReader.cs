using System;
using System.Collections;
using System.IO;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000468 RID: 1128
	public interface IContentReader : IDisposable
	{
		// Token: 0x0600323A RID: 12858
		IList Read(long readCount);

		// Token: 0x0600323B RID: 12859
		void Seek(long offset, SeekOrigin origin);

		// Token: 0x0600323C RID: 12860
		void Close();
	}
}
