using System;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000467 RID: 1127
	public interface IContentCmdletProvider
	{
		// Token: 0x06003234 RID: 12852
		IContentReader GetContentReader(string path);

		// Token: 0x06003235 RID: 12853
		object GetContentReaderDynamicParameters(string path);

		// Token: 0x06003236 RID: 12854
		IContentWriter GetContentWriter(string path);

		// Token: 0x06003237 RID: 12855
		object GetContentWriterDynamicParameters(string path);

		// Token: 0x06003238 RID: 12856
		void ClearContent(string path);

		// Token: 0x06003239 RID: 12857
		object ClearContentDynamicParameters(string path);
	}
}
