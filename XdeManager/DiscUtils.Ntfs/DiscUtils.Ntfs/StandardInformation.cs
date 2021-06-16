using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000051 RID: 81
	internal sealed class StandardInformation : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00014AAD File Offset: 0x00012CAD
		public int Size
		{
			get
			{
				if (!this._haveExtraFields)
				{
					return 48;
				}
				return 72;
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00014ABC File Offset: 0x00012CBC
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.CreationTime = StandardInformation.ReadDateTime(buffer, 0);
			this.ModificationTime = StandardInformation.ReadDateTime(buffer, 8);
			this.MftChangedTime = StandardInformation.ReadDateTime(buffer, 16);
			this.LastAccessTime = StandardInformation.ReadDateTime(buffer, 24);
			this.FileAttributes = (FileAttributeFlags)EndianUtilities.ToUInt32LittleEndian(buffer, 32);
			this.MaxVersions = EndianUtilities.ToUInt32LittleEndian(buffer, 36);
			this.Version = EndianUtilities.ToUInt32LittleEndian(buffer, 40);
			this.ClassId = EndianUtilities.ToUInt32LittleEndian(buffer, 44);
			if (buffer.Length > 48)
			{
				this.OwnerId = EndianUtilities.ToUInt32LittleEndian(buffer, 48);
				this.SecurityId = EndianUtilities.ToUInt32LittleEndian(buffer, 52);
				this.QuotaCharged = EndianUtilities.ToUInt64LittleEndian(buffer, 56);
				this.UpdateSequenceNumber = EndianUtilities.ToUInt64LittleEndian(buffer, 64);
				this._haveExtraFields = true;
				return 72;
			}
			this._haveExtraFields = false;
			return 48;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00014B8C File Offset: 0x00012D8C
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.CreationTime.ToFileTimeUtc(), buffer, 0);
			EndianUtilities.WriteBytesLittleEndian(this.ModificationTime.ToFileTimeUtc(), buffer, 8);
			EndianUtilities.WriteBytesLittleEndian(this.MftChangedTime.ToFileTimeUtc(), buffer, 16);
			EndianUtilities.WriteBytesLittleEndian(this.LastAccessTime.ToFileTimeUtc(), buffer, 24);
			EndianUtilities.WriteBytesLittleEndian((uint)this.FileAttributes, buffer, 32);
			EndianUtilities.WriteBytesLittleEndian(this.MaxVersions, buffer, 36);
			EndianUtilities.WriteBytesLittleEndian(this.Version, buffer, 40);
			EndianUtilities.WriteBytesLittleEndian(this.ClassId, buffer, 44);
			if (this._haveExtraFields)
			{
				EndianUtilities.WriteBytesLittleEndian(this.OwnerId, buffer, 48);
				EndianUtilities.WriteBytesLittleEndian(this.SecurityId, buffer, 52);
				EndianUtilities.WriteBytesLittleEndian(this.QuotaCharged, buffer, 56);
				EndianUtilities.WriteBytesLittleEndian(this.UpdateSequenceNumber, buffer, 56);
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00014C5C File Offset: 0x00012E5C
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "      Creation Time: " + this.CreationTime);
			writer.WriteLine(indent + "  Modification Time: " + this.ModificationTime);
			writer.WriteLine(indent + "   MFT Changed Time: " + this.MftChangedTime);
			writer.WriteLine(indent + "   Last Access Time: " + this.LastAccessTime);
			writer.WriteLine(indent + "   File Permissions: " + this.FileAttributes);
			writer.WriteLine(indent + "       Max Versions: " + this.MaxVersions);
			writer.WriteLine(indent + "            Version: " + this.Version);
			writer.WriteLine(indent + "           Class Id: " + this.ClassId);
			writer.WriteLine(indent + "        Security Id: " + this.SecurityId);
			writer.WriteLine(indent + "      Quota Charged: " + this.QuotaCharged);
			writer.WriteLine(indent + "     Update Seq Num: " + this.UpdateSequenceNumber);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00014DA0 File Offset: 0x00012FA0
		public static StandardInformation InitializeNewFile(File file, FileAttributeFlags flags)
		{
			DateTime utcNow = DateTime.UtcNow;
			NtfsStream ntfsStream = file.CreateStream(AttributeType.StandardInformation, null);
			StandardInformation standardInformation = new StandardInformation();
			standardInformation.CreationTime = utcNow;
			standardInformation.ModificationTime = utcNow;
			standardInformation.MftChangedTime = utcNow;
			standardInformation.LastAccessTime = utcNow;
			standardInformation.FileAttributes = flags;
			ntfsStream.SetContent<StandardInformation>(standardInformation);
			return standardInformation;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00014DEC File Offset: 0x00012FEC
		internal static FileAttributes ConvertFlags(FileAttributeFlags flags, bool isDirectory)
		{
			FileAttributes fileAttributes = (FileAttributes)(flags & (FileAttributeFlags)65535U);
			if (isDirectory)
			{
				fileAttributes |= System.IO.FileAttributes.Directory;
			}
			return fileAttributes;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00014E0A File Offset: 0x0001300A
		internal static FileAttributeFlags SetFileAttributes(FileAttributes newAttributes, FileAttributeFlags existing)
		{
			return (existing & (FileAttributeFlags)4294901760U) | (FileAttributeFlags)(newAttributes & (FileAttributes)65535);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00014E1C File Offset: 0x0001301C
		private static DateTime ReadDateTime(byte[] buffer, int offset)
		{
			DateTime result;
			try
			{
				result = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64LittleEndian(buffer, offset));
			}
			catch (ArgumentException)
			{
				result = DateTime.FromFileTimeUtc(0L);
			}
			return result;
		}

		// Token: 0x04000180 RID: 384
		private bool _haveExtraFields = true;

		// Token: 0x04000181 RID: 385
		public uint ClassId;

		// Token: 0x04000182 RID: 386
		public DateTime CreationTime;

		// Token: 0x04000183 RID: 387
		public FileAttributeFlags FileAttributes;

		// Token: 0x04000184 RID: 388
		public DateTime LastAccessTime;

		// Token: 0x04000185 RID: 389
		public uint MaxVersions;

		// Token: 0x04000186 RID: 390
		public DateTime MftChangedTime;

		// Token: 0x04000187 RID: 391
		public DateTime ModificationTime;

		// Token: 0x04000188 RID: 392
		public uint OwnerId;

		// Token: 0x04000189 RID: 393
		public ulong QuotaCharged;

		// Token: 0x0400018A RID: 394
		public uint SecurityId;

		// Token: 0x0400018B RID: 395
		public ulong UpdateSequenceNumber;

		// Token: 0x0400018C RID: 396
		public uint Version;
	}
}
