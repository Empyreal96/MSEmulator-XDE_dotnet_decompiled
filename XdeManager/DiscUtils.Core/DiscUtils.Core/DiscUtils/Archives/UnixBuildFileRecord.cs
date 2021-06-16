using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Archives
{
	// Token: 0x02000090 RID: 144
	internal sealed class UnixBuildFileRecord
	{
		// Token: 0x060004E6 RID: 1254 RVA: 0x0000E8B1 File Offset: 0x0000CAB1
		public UnixBuildFileRecord(string name, byte[] buffer) : this(name, new BuilderBufferExtentSource(buffer), UnixFilePermissions.None, 0, 0, DateTimeOffsetExtensions.UnixEpoch)
		{
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
		public UnixBuildFileRecord(string name, Stream stream) : this(name, new BuilderStreamExtentSource(stream), UnixFilePermissions.None, 0, 0, DateTimeOffsetExtensions.UnixEpoch)
		{
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000E8DF File Offset: 0x0000CADF
		public UnixBuildFileRecord(string name, byte[] buffer, UnixFilePermissions fileMode, int ownerId, int groupId, DateTime modificationTime) : this(name, new BuilderBufferExtentSource(buffer), fileMode, ownerId, groupId, modificationTime)
		{
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000E8F5 File Offset: 0x0000CAF5
		public UnixBuildFileRecord(string name, Stream stream, UnixFilePermissions fileMode, int ownerId, int groupId, DateTime modificationTime) : this(name, new BuilderStreamExtentSource(stream), fileMode, ownerId, groupId, modificationTime)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000E90B File Offset: 0x0000CB0B
		public UnixBuildFileRecord(string name, BuilderExtentSource fileSource, UnixFilePermissions fileMode, int ownerId, int groupId, DateTime modificationTime)
		{
			this.Name = name;
			this._source = fileSource;
			this.FileMode = fileMode;
			this.OwnerId = ownerId;
			this.GroupId = groupId;
			this.ModificationTime = modificationTime;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000E940 File Offset: 0x0000CB40
		public UnixFilePermissions FileMode { get; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000E948 File Offset: 0x0000CB48
		public int GroupId { get; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000E950 File Offset: 0x0000CB50
		public DateTime ModificationTime { get; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0000E958 File Offset: 0x0000CB58
		public string Name { get; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0000E960 File Offset: 0x0000CB60
		public int OwnerId { get; }

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000E968 File Offset: 0x0000CB68
		public BuilderExtent Fix(long pos)
		{
			return this._source.Fix(pos);
		}

		// Token: 0x040001D7 RID: 471
		private readonly BuilderExtentSource _source;
	}
}
