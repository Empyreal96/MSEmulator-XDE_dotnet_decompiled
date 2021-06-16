using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200001C RID: 28
	internal class FileNameRecord : IByteArraySerializable, IDiagnosticTraceable, IEquatable<FileNameRecord>
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00006FAA File Offset: 0x000051AA
		public FileNameRecord()
		{
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006FB4 File Offset: 0x000051B4
		public FileNameRecord(FileNameRecord toCopy)
		{
			this.ParentDirectory = toCopy.ParentDirectory;
			this.CreationTime = toCopy.CreationTime;
			this.ModificationTime = toCopy.ModificationTime;
			this.MftChangedTime = toCopy.MftChangedTime;
			this.LastAccessTime = toCopy.LastAccessTime;
			this.AllocatedSize = toCopy.AllocatedSize;
			this.RealSize = toCopy.RealSize;
			this.Flags = toCopy.Flags;
			this.EASizeOrReparsePointTag = toCopy.EASizeOrReparsePointTag;
			this.FileNameNamespace = toCopy.FileNameNamespace;
			this.FileName = toCopy.FileName;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000704B File Offset: 0x0000524B
		public FileAttributes FileAttributes
		{
			get
			{
				return FileNameRecord.ConvertFlags(this.Flags);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00007058 File Offset: 0x00005258
		public int Size
		{
			get
			{
				return 66 + this.FileName.Length * 2;
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000706C File Offset: 0x0000526C
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.ParentDirectory = new FileRecordReference(EndianUtilities.ToUInt64LittleEndian(buffer, offset));
			this.CreationTime = FileNameRecord.ReadDateTime(buffer, offset + 8);
			this.ModificationTime = FileNameRecord.ReadDateTime(buffer, offset + 16);
			this.MftChangedTime = FileNameRecord.ReadDateTime(buffer, offset + 24);
			this.LastAccessTime = FileNameRecord.ReadDateTime(buffer, offset + 32);
			this.AllocatedSize = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 40);
			this.RealSize = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 48);
			this.Flags = (FileAttributeFlags)EndianUtilities.ToUInt32LittleEndian(buffer, offset + 56);
			this.EASizeOrReparsePointTag = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 60);
			byte b = buffer[offset + 64];
			this.FileNameNamespace = (FileNameNamespace)buffer[offset + 65];
			this.FileName = Encoding.Unicode.GetString(buffer, offset + 66, (int)(b * 2));
			return (int)(66 + b * 2);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000713C File Offset: 0x0000533C
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.ParentDirectory.Value, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian((ulong)this.CreationTime.ToFileTimeUtc(), buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian((ulong)this.ModificationTime.ToFileTimeUtc(), buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian((ulong)this.MftChangedTime.ToFileTimeUtc(), buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian((ulong)this.LastAccessTime.ToFileTimeUtc(), buffer, offset + 32);
			EndianUtilities.WriteBytesLittleEndian(this.AllocatedSize, buffer, offset + 40);
			EndianUtilities.WriteBytesLittleEndian(this.RealSize, buffer, offset + 48);
			EndianUtilities.WriteBytesLittleEndian((uint)this.Flags, buffer, offset + 56);
			EndianUtilities.WriteBytesLittleEndian(this.EASizeOrReparsePointTag, buffer, offset + 60);
			buffer[offset + 64] = (byte)this.FileName.Length;
			buffer[offset + 65] = (byte)this.FileNameNamespace;
			Encoding.Unicode.GetBytes(this.FileName, 0, this.FileName.Length, buffer, offset + 66);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007230 File Offset: 0x00005430
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "FILE NAME RECORD");
			writer.WriteLine(indent + "   Parent Directory: " + this.ParentDirectory);
			writer.WriteLine(indent + "      Creation Time: " + this.CreationTime);
			writer.WriteLine(indent + "  Modification Time: " + this.ModificationTime);
			writer.WriteLine(indent + "   MFT Changed Time: " + this.MftChangedTime);
			writer.WriteLine(indent + "   Last Access Time: " + this.LastAccessTime);
			writer.WriteLine(indent + "     Allocated Size: " + this.AllocatedSize);
			writer.WriteLine(indent + "          Real Size: " + this.RealSize);
			writer.WriteLine(indent + "              Flags: " + this.Flags);
			if ((this.Flags & FileAttributeFlags.ReparsePoint) != FileAttributeFlags.None)
			{
				writer.WriteLine(indent + "  Reparse Point Tag: " + this.EASizeOrReparsePointTag);
			}
			else
			{
				writer.WriteLine(indent + "      Ext Attr Size: " + (this.EASizeOrReparsePointTag & 65535U));
			}
			writer.WriteLine(indent + "          Namespace: " + this.FileNameNamespace);
			writer.WriteLine(indent + "          File Name: " + this.FileName);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000073AF File Offset: 0x000055AF
		public bool Equals(FileNameRecord other)
		{
			return other != null && (this.ParentDirectory == other.ParentDirectory && this.FileNameNamespace == other.FileNameNamespace) && this.FileName == other.FileName;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000073EA File Offset: 0x000055EA
		public override string ToString()
		{
			return this.FileName;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000073F4 File Offset: 0x000055F4
		internal static FileAttributeFlags SetAttributes(FileAttributes attrs, FileAttributeFlags flags)
		{
			FileAttributes fileAttributes = (FileAttributes)65519;
			return (flags & (FileAttributeFlags)4294901760U) | (FileAttributeFlags)(attrs & fileAttributes);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007414 File Offset: 0x00005614
		internal static FileAttributes ConvertFlags(FileAttributeFlags flags)
		{
			FileAttributes fileAttributes = (FileAttributes)(flags & (FileAttributeFlags)65535U);
			if ((flags & FileAttributeFlags.Directory) != FileAttributeFlags.None)
			{
				fileAttributes |= FileAttributes.Directory;
			}
			return fileAttributes;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007438 File Offset: 0x00005638
		private static DateTime ReadDateTime(byte[] buffer, int offset)
		{
			DateTime result;
			try
			{
				result = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64LittleEndian(buffer, offset));
			}
			catch (ArgumentException)
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		// Token: 0x04000093 RID: 147
		public ulong AllocatedSize;

		// Token: 0x04000094 RID: 148
		public DateTime CreationTime;

		// Token: 0x04000095 RID: 149
		public uint EASizeOrReparsePointTag;

		// Token: 0x04000096 RID: 150
		public string FileName;

		// Token: 0x04000097 RID: 151
		public FileNameNamespace FileNameNamespace;

		// Token: 0x04000098 RID: 152
		public FileAttributeFlags Flags;

		// Token: 0x04000099 RID: 153
		public DateTime LastAccessTime;

		// Token: 0x0400009A RID: 154
		public DateTime MftChangedTime;

		// Token: 0x0400009B RID: 155
		public DateTime ModificationTime;

		// Token: 0x0400009C RID: 156
		public FileRecordReference ParentDirectory;

		// Token: 0x0400009D RID: 157
		public ulong RealSize;
	}
}
