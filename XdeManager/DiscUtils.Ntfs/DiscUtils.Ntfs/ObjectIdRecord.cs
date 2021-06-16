using System;
using System.Globalization;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000045 RID: 69
	internal sealed class ObjectIdRecord : IByteArraySerializable
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00012C72 File Offset: 0x00010E72
		public int Size
		{
			get
			{
				return 56;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00012C78 File Offset: 0x00010E78
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.MftReference = default(FileRecordReference);
			this.MftReference.ReadFrom(buffer, offset);
			this.BirthVolumeId = EndianUtilities.ToGuidLittleEndian(buffer, offset + 8);
			this.BirthObjectId = EndianUtilities.ToGuidLittleEndian(buffer, offset + 24);
			this.BirthDomainId = EndianUtilities.ToGuidLittleEndian(buffer, offset + 40);
			return 56;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012CD0 File Offset: 0x00010ED0
		public void WriteTo(byte[] buffer, int offset)
		{
			this.MftReference.WriteTo(buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.BirthVolumeId, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.BirthObjectId, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(this.BirthDomainId, buffer, offset + 40);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00012D10 File Offset: 0x00010F10
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[Data-MftRef:{0},BirthVolId:{1},BirthObjId:{2},BirthDomId:{3}]", new object[]
			{
				this.MftReference,
				this.BirthVolumeId,
				this.BirthObjectId,
				this.BirthDomainId
			});
		}

		// Token: 0x0400015B RID: 347
		public Guid BirthDomainId;

		// Token: 0x0400015C RID: 348
		public Guid BirthObjectId;

		// Token: 0x0400015D RID: 349
		public Guid BirthVolumeId;

		// Token: 0x0400015E RID: 350
		public FileRecordReference MftReference;
	}
}
