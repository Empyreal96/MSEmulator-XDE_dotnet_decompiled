using System;
using DiscUtils.Streams;

namespace DiscUtils.Vfs
{
	// Token: 0x02000038 RID: 56
	public interface IVfsFileWithStreams : IVfsFile
	{
		// Token: 0x0600023D RID: 573
		SparseStream CreateStream(string name);

		// Token: 0x0600023E RID: 574
		SparseStream OpenExistingStream(string name);
	}
}
