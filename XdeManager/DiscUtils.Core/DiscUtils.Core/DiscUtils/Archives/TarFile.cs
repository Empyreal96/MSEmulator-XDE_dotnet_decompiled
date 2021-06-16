using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Archives
{
	// Token: 0x0200008C RID: 140
	public sealed class TarFile
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x0000E230 File Offset: 0x0000C430
		public TarFile(Stream fileStream)
		{
			this._fileStream = fileStream;
			this._files = new Dictionary<string, FileRecord>();
			TarHeader tarHeader = new TarHeader();
			byte[] buffer = StreamUtilities.ReadExact(this._fileStream, 512);
			tarHeader.ReadFrom(buffer, 0);
			while (tarHeader.FileLength != 0L || !string.IsNullOrEmpty(tarHeader.FileName))
			{
				FileRecord fileRecord = new FileRecord(tarHeader.FileName, this._fileStream.Position, tarHeader.FileLength);
				this._files.Add(fileRecord.Name, fileRecord);
				this._fileStream.Position += (tarHeader.FileLength + 511L) / 512L * 512L;
				buffer = StreamUtilities.ReadExact(this._fileStream, 512);
				tarHeader.ReadFrom(buffer, 0);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000E304 File Offset: 0x0000C504
		public bool TryOpenFile(string path, out Stream stream)
		{
			if (this._files.ContainsKey(path))
			{
				FileRecord fileRecord = this._files[path];
				stream = new SubStream(this._fileStream, fileRecord.Start, fileRecord.Length);
				return true;
			}
			stream = null;
			return false;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000E34C File Offset: 0x0000C54C
		public Stream OpenFile(string path)
		{
			if (this._files.ContainsKey(path))
			{
				FileRecord fileRecord = this._files[path];
				return new SubStream(this._fileStream, fileRecord.Start, fileRecord.Length);
			}
			throw new FileNotFoundException("File is not in archive", path);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000E397 File Offset: 0x0000C597
		public bool FileExists(string path)
		{
			return this._files.ContainsKey(path);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		public bool DirExists(string path)
		{
			string text = path.Replace("\\", "/");
			text = (text.EndsWith("/", StringComparison.Ordinal) ? text : (text + "/"));
			using (Dictionary<string, FileRecord>.KeyCollection.Enumerator enumerator = this._files.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.StartsWith(text, StringComparison.Ordinal))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000E438 File Offset: 0x0000C638
		internal IEnumerable<FileRecord> GetFiles(string dir)
		{
			string searchStr = dir;
			searchStr = searchStr.Replace("\\", "/");
			searchStr = (searchStr.EndsWith("/", StringComparison.Ordinal) ? searchStr : (searchStr + "/"));
			foreach (string text in this._files.Keys)
			{
				if (text.StartsWith(searchStr, StringComparison.Ordinal))
				{
					yield return this._files[text];
				}
			}
			Dictionary<string, FileRecord>.KeyCollection.Enumerator enumerator = default(Dictionary<string, FileRecord>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x040001C7 RID: 455
		private readonly Dictionary<string, FileRecord> _files;

		// Token: 0x040001C8 RID: 456
		private readonly Stream _fileStream;
	}
}
