using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200004A RID: 74
	internal class ReparsePoints
	{
		// Token: 0x06000378 RID: 888 RVA: 0x00013B45 File Offset: 0x00011D45
		public ReparsePoints(File file)
		{
			this._file = file;
			this._index = new IndexView<ReparsePoints.Key, ReparsePoints.Data>(file.GetIndex("$R"));
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00013B6C File Offset: 0x00011D6C
		internal void Add(uint tag, FileRecordReference file)
		{
			ReparsePoints.Key key = new ReparsePoints.Key();
			key.Tag = tag;
			key.File = file;
			ReparsePoints.Data value = new ReparsePoints.Data();
			this._index[key] = value;
			this._file.UpdateRecordInMft();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00013BAC File Offset: 0x00011DAC
		internal void Remove(uint tag, FileRecordReference file)
		{
			ReparsePoints.Key key = new ReparsePoints.Key();
			key.Tag = tag;
			key.File = file;
			this._index.Remove(key);
			this._file.UpdateRecordInMft();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00013BE4 File Offset: 0x00011DE4
		internal void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "REPARSE POINT INDEX");
			foreach (KeyValuePair<ReparsePoints.Key, ReparsePoints.Data> keyValuePair in this._index.Entries)
			{
				writer.WriteLine(indent + "  REPARSE POINT INDEX ENTRY");
				writer.WriteLine(indent + "            Tag: " + keyValuePair.Key.Tag.ToString("x", CultureInfo.InvariantCulture));
				writer.WriteLine(indent + "  MFT Reference: " + keyValuePair.Key.File);
			}
		}

		// Token: 0x0400016A RID: 362
		private readonly File _file;

		// Token: 0x0400016B RID: 363
		private readonly IndexView<ReparsePoints.Key, ReparsePoints.Data> _index;

		// Token: 0x0200008A RID: 138
		internal sealed class Key : IByteArraySerializable
		{
			// Token: 0x17000158 RID: 344
			// (get) Token: 0x060004D1 RID: 1233 RVA: 0x000178B8 File Offset: 0x00015AB8
			public int Size
			{
				get
				{
					return 12;
				}
			}

			// Token: 0x060004D2 RID: 1234 RVA: 0x000178BC File Offset: 0x00015ABC
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Tag = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
				this.File = new FileRecordReference(EndianUtilities.ToUInt64LittleEndian(buffer, offset + 4));
				return 12;
			}

			// Token: 0x060004D3 RID: 1235 RVA: 0x000178E1 File Offset: 0x00015AE1
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Tag, buffer, offset);
				EndianUtilities.WriteBytesLittleEndian(this.File.Value, buffer, offset + 4);
			}

			// Token: 0x060004D4 RID: 1236 RVA: 0x00017904 File Offset: 0x00015B04
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "{0:x}:", new object[]
				{
					this.Tag
				}) + this.File;
			}

			// Token: 0x0400025A RID: 602
			public FileRecordReference File;

			// Token: 0x0400025B RID: 603
			public uint Tag;
		}

		// Token: 0x0200008B RID: 139
		internal sealed class Data : IByteArraySerializable
		{
			// Token: 0x17000159 RID: 345
			// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00017941 File Offset: 0x00015B41
			public int Size
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x060004D7 RID: 1239 RVA: 0x00017944 File Offset: 0x00015B44
			public int ReadFrom(byte[] buffer, int offset)
			{
				return 0;
			}

			// Token: 0x060004D8 RID: 1240 RVA: 0x00017947 File Offset: 0x00015B47
			public void WriteTo(byte[] buffer, int offset)
			{
			}

			// Token: 0x060004D9 RID: 1241 RVA: 0x00017949 File Offset: 0x00015B49
			public override string ToString()
			{
				return "<no data>";
			}
		}
	}
}
