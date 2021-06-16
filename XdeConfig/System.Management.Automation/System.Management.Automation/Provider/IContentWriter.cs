using System;
using System.Collections;
using System.IO;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000469 RID: 1129
	public interface IContentWriter : IDisposable
	{
		// Token: 0x0600323D RID: 12861
		IList Write(IList content);

		// Token: 0x0600323E RID: 12862
		void Seek(long offset, SeekOrigin origin);

		// Token: 0x0600323F RID: 12863
		void Close();
	}
}
