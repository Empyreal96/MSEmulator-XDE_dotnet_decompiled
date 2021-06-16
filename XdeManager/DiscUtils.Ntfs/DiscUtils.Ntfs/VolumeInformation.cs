using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000054 RID: 84
	internal sealed class VolumeInformation : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x060003C2 RID: 962 RVA: 0x000151FC File Offset: 0x000133FC
		public VolumeInformation()
		{
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00015204 File Offset: 0x00013404
		public VolumeInformation(byte major, byte minor, VolumeInformationFlags flags)
		{
			this._majorVersion = major;
			this._minorVersion = minor;
			this.Flags = flags;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00015221 File Offset: 0x00013421
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00015229 File Offset: 0x00013429
		public VolumeInformationFlags Flags { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00015232 File Offset: 0x00013432
		public int Version
		{
			get
			{
				return (int)this._majorVersion << 8 | (int)this._minorVersion;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00015243 File Offset: 0x00013443
		public int Size
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00015247 File Offset: 0x00013447
		public int ReadFrom(byte[] buffer, int offset)
		{
			this._majorVersion = buffer[offset + 8];
			this._minorVersion = buffer[offset + 9];
			this.Flags = (VolumeInformationFlags)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 10);
			return 12;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00015272 File Offset: 0x00013472
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(0UL, buffer, offset);
			buffer[offset + 8] = this._majorVersion;
			buffer[offset + 9] = this._minorVersion;
			EndianUtilities.WriteBytesLittleEndian((ushort)this.Flags, buffer, offset + 10);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000152A4 File Offset: 0x000134A4
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(string.Concat(new object[]
			{
				indent,
				"  Version: ",
				this._majorVersion,
				".",
				this._minorVersion
			}));
			writer.WriteLine(indent + "    Flags: " + this.Flags);
		}

		// Token: 0x04000191 RID: 401
		public const int VersionNt4 = 258;

		// Token: 0x04000192 RID: 402
		public const int VersionW2k = 768;

		// Token: 0x04000193 RID: 403
		public const int VersionXp = 769;

		// Token: 0x04000194 RID: 404
		private byte _majorVersion;

		// Token: 0x04000195 RID: 405
		private byte _minorVersion;
	}
}
