using System;

namespace DiscUtils.Streams
{
	// Token: 0x0200001A RID: 26
	public interface IByteArraySerializable
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000CD RID: 205
		int Size { get; }

		// Token: 0x060000CE RID: 206
		int ReadFrom(byte[] buffer, int offset);

		// Token: 0x060000CF RID: 207
		void WriteTo(byte[] buffer, int offset);
	}
}
