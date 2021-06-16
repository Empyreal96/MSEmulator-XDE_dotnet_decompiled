using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Archives
{
	// Token: 0x0200008D RID: 141
	public sealed class TarFileBuilder : StreamBuilder
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x0000E44F File Offset: 0x0000C64F
		public TarFileBuilder()
		{
			this._files = new List<UnixBuildFileRecord>();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000E462 File Offset: 0x0000C662
		public void AddFile(string name, byte[] buffer)
		{
			this._files.Add(new UnixBuildFileRecord(name, buffer));
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000E476 File Offset: 0x0000C676
		public void AddFile(string name, byte[] buffer, UnixFilePermissions fileMode, int ownerId, int groupId, DateTime modificationTime)
		{
			this._files.Add(new UnixBuildFileRecord(name, buffer, fileMode, ownerId, groupId, modificationTime));
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000E491 File Offset: 0x0000C691
		public void AddFile(string name, Stream stream)
		{
			this._files.Add(new UnixBuildFileRecord(name, stream));
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000E4A5 File Offset: 0x0000C6A5
		public void AddFile(string name, Stream stream, UnixFilePermissions fileMode, int ownerId, int groupId, DateTime modificationTime)
		{
			this._files.Add(new UnixBuildFileRecord(name, stream, fileMode, ownerId, groupId, modificationTime));
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		protected override List<BuilderExtent> FixExtents(out long totalLength)
		{
			List<BuilderExtent> list = new List<BuilderExtent>(this._files.Count * 2 + 2);
			long num = 0L;
			foreach (UnixBuildFileRecord unixBuildFileRecord in this._files)
			{
				BuilderExtent builderExtent = unixBuildFileRecord.Fix(num + 512L);
				list.Add(new TarHeaderExtent(num, unixBuildFileRecord.Name, builderExtent.Length, unixBuildFileRecord.FileMode, unixBuildFileRecord.OwnerId, unixBuildFileRecord.GroupId, unixBuildFileRecord.ModificationTime));
				num += 512L;
				list.Add(builderExtent);
				num += MathUtilities.RoundUp(builderExtent.Length, 512L);
			}
			list.Add(new BuilderBufferExtent(num, new byte[1024]));
			totalLength = num + 1024L;
			return list;
		}

		// Token: 0x040001C9 RID: 457
		private readonly List<UnixBuildFileRecord> _files;
	}
}
