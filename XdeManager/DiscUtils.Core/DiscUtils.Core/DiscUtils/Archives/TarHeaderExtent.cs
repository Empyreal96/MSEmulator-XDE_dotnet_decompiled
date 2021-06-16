using System;
using DiscUtils.Streams;

namespace DiscUtils.Archives
{
	// Token: 0x0200008F RID: 143
	internal sealed class TarHeaderExtent : BuilderBufferExtent
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x0000E7F1 File Offset: 0x0000C9F1
		public TarHeaderExtent(long start, string fileName, long fileLength, UnixFilePermissions mode, int ownerId, int groupId, DateTime modificationTime) : base(start, 512L)
		{
			this._fileName = fileName;
			this._fileLength = fileLength;
			this._mode = mode;
			this._ownerId = ownerId;
			this._groupId = groupId;
			this._modificationTime = modificationTime;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000E82E File Offset: 0x0000CA2E
		public TarHeaderExtent(long start, string fileName, long fileLength) : this(start, fileName, fileLength, UnixFilePermissions.None, 0, 0, DateTimeOffsetExtensions.UnixEpoch)
		{
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000E844 File Offset: 0x0000CA44
		protected override byte[] GetBuffer()
		{
			byte[] array = new byte[512];
			new TarHeader
			{
				FileName = this._fileName,
				FileLength = this._fileLength,
				FileMode = this._mode,
				OwnerId = this._ownerId,
				GroupId = this._groupId,
				ModificationTime = this._modificationTime
			}.WriteTo(array, 0);
			return array;
		}

		// Token: 0x040001D1 RID: 465
		private readonly long _fileLength;

		// Token: 0x040001D2 RID: 466
		private readonly string _fileName;

		// Token: 0x040001D3 RID: 467
		private readonly int _groupId;

		// Token: 0x040001D4 RID: 468
		private readonly UnixFilePermissions _mode;

		// Token: 0x040001D5 RID: 469
		private readonly DateTime _modificationTime;

		// Token: 0x040001D6 RID: 470
		private readonly int _ownerId;
	}
}
