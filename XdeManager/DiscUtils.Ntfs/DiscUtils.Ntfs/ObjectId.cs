using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000044 RID: 68
	internal sealed class ObjectId : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00012C28 File Offset: 0x00010E28
		public int Size
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00012C2C File Offset: 0x00010E2C
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Id = EndianUtilities.ToGuidLittleEndian(buffer, offset);
			return 16;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00012C3D File Offset: 0x00010E3D
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00012C4C File Offset: 0x00010E4C
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "  Object ID: " + this.Id);
		}

		// Token: 0x0400015A RID: 346
		public Guid Id;
	}
}
