using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DiscUtils.Internal;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000017 RID: 23
	internal class Directory : File
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00004F38 File Offset: 0x00003138
		public Directory(INtfsContext context, FileRecord baseRecord) : base(context, baseRecord)
		{
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004F42 File Offset: 0x00003142
		private IndexView<FileNameRecord, FileRecordReference> Index
		{
			get
			{
				if (this._index == null && base.StreamExists(AttributeType.IndexRoot, "$I30"))
				{
					this._index = new IndexView<FileNameRecord, FileRecordReference>(base.GetIndex("$I30"));
				}
				return this._index;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004F7A File Offset: 0x0000317A
		public bool IsEmpty
		{
			get
			{
				return this.Index.Count == 0;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004F8A File Offset: 0x0000318A
		public IEnumerable<DirectoryEntry> GetAllEntries(bool filter)
		{
			IEnumerable<KeyValuePair<FileNameRecord, FileRecordReference>> enumerable;
			if (!filter)
			{
				enumerable = this.Index.Entries;
			}
			else
			{
				IEnumerable<KeyValuePair<FileNameRecord, FileRecordReference>> enumerable2 = this.FilterEntries(this.Index.Entries);
				enumerable = enumerable2;
			}
			IEnumerable<KeyValuePair<FileNameRecord, FileRecordReference>> enumerable3 = enumerable;
			foreach (KeyValuePair<FileNameRecord, FileRecordReference> keyValuePair in enumerable3)
			{
				yield return new DirectoryEntry(this, keyValuePair.Value, keyValuePair.Key);
			}
			IEnumerator<KeyValuePair<FileNameRecord, FileRecordReference>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004FA1 File Offset: 0x000031A1
		public void UpdateEntry(DirectoryEntry entry)
		{
			this.Index[entry.Details] = entry.Reference;
			base.UpdateRecordInMft();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004FC0 File Offset: 0x000031C0
		public override void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "DIRECTORY (" + base.ToString() + ")");
			writer.WriteLine(indent + "  File Number: " + base.IndexInMft);
			if (this.Index != null)
			{
				foreach (KeyValuePair<FileNameRecord, FileRecordReference> keyValuePair in this.Index.Entries)
				{
					writer.WriteLine(indent + "  DIRECTORY ENTRY (" + keyValuePair.Key.FileName + ")");
					writer.WriteLine(indent + "    MFT Ref: " + keyValuePair.Value);
					keyValuePair.Key.Dump(writer, indent + "    ");
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000050A8 File Offset: 0x000032A8
		public override string ToString()
		{
			return base.ToString() + "\\";
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000050BA File Offset: 0x000032BA
		internal new static Directory CreateNew(INtfsContext context, FileAttributeFlags parentDirFlags)
		{
			Directory directory = (Directory)context.AllocateFile(FileRecordFlags.IsDirectory);
			StandardInformation.InitializeNewFile(directory, FileAttributeFlags.Archive | (parentDirFlags & FileAttributeFlags.Compressed));
			directory.CreateIndex("$I30", AttributeType.FileName, AttributeCollationRule.Filename);
			directory.UpdateRecordInMft();
			return directory;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000050F4 File Offset: 0x000032F4
		internal DirectoryEntry GetEntryByName(string name)
		{
			string query = name;
			int num = name.IndexOf(':');
			if (num >= 0)
			{
				query = name.Substring(0, num);
			}
			KeyValuePair<FileNameRecord, FileRecordReference> keyValuePair = this.Index.FindFirst(new Directory.FileNameQuery(query, this._context.UpperCase));
			if (keyValuePair.Key != null)
			{
				return new DirectoryEntry(this, keyValuePair.Value, keyValuePair.Key);
			}
			return null;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005158 File Offset: 0x00003358
		internal DirectoryEntry AddEntry(File file, string name, FileNameNamespace nameNamespace)
		{
			if (name.Length > 255)
			{
				throw new IOException("Invalid file name, more than 255 characters: " + name);
			}
			if (name.IndexOfAny(new char[]
			{
				'\0',
				'/'
			}) != -1)
			{
				throw new IOException("Invalid file name, contains '\\0' or '/': " + name);
			}
			FileNameRecord fileNameRecord = file.GetFileNameRecord(null, true);
			fileNameRecord.FileNameNamespace = nameNamespace;
			fileNameRecord.FileName = name;
			fileNameRecord.ParentDirectory = base.MftReference;
			file.CreateStream(AttributeType.FileName, null).SetContent<FileNameRecord>(fileNameRecord);
			ushort hardLinkCount = file.HardLinkCount;
			file.HardLinkCount = hardLinkCount + 1;
			file.UpdateRecordInMft();
			this.Index[fileNameRecord] = file.MftReference;
			base.Modified();
			base.UpdateRecordInMft();
			return new DirectoryEntry(this, file.MftReference, fileNameRecord);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000521C File Offset: 0x0000341C
		internal void RemoveEntry(DirectoryEntry dirEntry)
		{
			File file = this._context.GetFileByRef(dirEntry.Reference);
			FileNameRecord details = dirEntry.Details;
			this.Index.Remove(dirEntry.Details);
			foreach (NtfsStream ntfsStream in file.GetStreams(AttributeType.FileName, null))
			{
				FileNameRecord content = ntfsStream.GetContent<FileNameRecord>();
				if (details.Equals(content))
				{
					file.RemoveStream(ntfsStream);
					break;
				}
			}
			File file2 = file;
			ushort hardLinkCount = file2.HardLinkCount;
			file2.HardLinkCount = hardLinkCount - 1;
			file.UpdateRecordInMft();
			base.Modified();
			base.UpdateRecordInMft();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000052D4 File Offset: 0x000034D4
		internal string CreateShortName(string name)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			int num = name.LastIndexOf('.');
			int num2 = 0;
			while (text.Length < 6 && num2 < name.Length && num2 != num)
			{
				char ch = char.ToUpperInvariant(name[num2]);
				if (Utilities.Is8Dot3Char(ch))
				{
					text += ch.ToString();
				}
				num2++;
			}
			if (num >= 0)
			{
				num2 = num + 1;
				while (text2.Length < 3 && num2 < name.Length)
				{
					char ch2 = char.ToUpperInvariant(name[num2]);
					if (Utilities.Is8Dot3Char(ch2))
					{
						text2 += ch2.ToString();
					}
					num2++;
				}
			}
			num2 = 1;
			string text4;
			do
			{
				string text3 = string.Format(CultureInfo.InvariantCulture, "~{0}", new object[]
				{
					num2
				});
				text4 = text.Substring(0, Math.Min(8 - text3.Length, text.Length)) + text3 + ((text2.Length > 0) ? ("." + text2) : string.Empty);
				num2++;
			}
			while (this.GetEntryByName(text4) != null);
			return text4;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000053F0 File Offset: 0x000035F0
		private List<KeyValuePair<FileNameRecord, FileRecordReference>> FilterEntries(IEnumerable<KeyValuePair<FileNameRecord, FileRecordReference>> entriesIter)
		{
			List<KeyValuePair<FileNameRecord, FileRecordReference>> list = new List<KeyValuePair<FileNameRecord, FileRecordReference>>();
			foreach (KeyValuePair<FileNameRecord, FileRecordReference> item in entriesIter)
			{
				if (((item.Key.Flags & FileAttributeFlags.Hidden) == FileAttributeFlags.None || !this._context.Options.HideHiddenFiles) && ((item.Key.Flags & FileAttributeFlags.System) == FileAttributeFlags.None || !this._context.Options.HideSystemFiles) && (item.Value.MftIndex >= 24L || !this._context.Options.HideMetafiles) && (item.Key.FileNameNamespace != FileNameNamespace.Dos || !this._context.Options.HideDosFileNames))
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x04000073 RID: 115
		private IndexView<FileNameRecord, FileRecordReference> _index;

		// Token: 0x02000069 RID: 105
		private sealed class FileNameQuery : IComparable<byte[]>
		{
			// Token: 0x0600041A RID: 1050 RVA: 0x00015951 File Offset: 0x00013B51
			public FileNameQuery(string query, UpperCase upperCase)
			{
				this._query = Encoding.Unicode.GetBytes(query);
				this._upperCase = upperCase;
			}

			// Token: 0x0600041B RID: 1051 RVA: 0x00015974 File Offset: 0x00013B74
			public int CompareTo(byte[] buffer)
			{
				byte b = buffer[64];
				return this._upperCase.Compare(this._query, 0, this._query.Length, buffer, 66, (int)(b * 2));
			}

			// Token: 0x040001EC RID: 492
			private readonly byte[] _query;

			// Token: 0x040001ED RID: 493
			private readonly UpperCase _upperCase;
		}
	}
}
