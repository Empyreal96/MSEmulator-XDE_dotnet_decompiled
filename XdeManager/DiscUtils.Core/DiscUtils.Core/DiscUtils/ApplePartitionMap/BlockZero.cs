using System;
using DiscUtils.Streams;

namespace DiscUtils.ApplePartitionMap
{
	// Token: 0x02000091 RID: 145
	internal sealed class BlockZero : IByteArraySerializable
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000E976 File Offset: 0x0000CB76
		public int Size
		{
			get
			{
				return 512;
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000E980 File Offset: 0x0000CB80
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Signature = EndianUtilities.ToUInt16BigEndian(buffer, offset);
			this.BlockSize = EndianUtilities.ToUInt16BigEndian(buffer, offset + 2);
			this.BlockCount = EndianUtilities.ToUInt32BigEndian(buffer, offset + 4);
			this.DeviceType = EndianUtilities.ToUInt16BigEndian(buffer, offset + 8);
			this.DeviceId = EndianUtilities.ToUInt16BigEndian(buffer, offset + 10);
			this.DriverData = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12);
			this.DriverCount = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 16);
			return 512;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000E9FC File Offset: 0x0000CBFC
		public void WriteTo(byte[] buffer, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040001DD RID: 477
		public uint BlockCount;

		// Token: 0x040001DE RID: 478
		public ushort BlockSize;

		// Token: 0x040001DF RID: 479
		public ushort DeviceId;

		// Token: 0x040001E0 RID: 480
		public ushort DeviceType;

		// Token: 0x040001E1 RID: 481
		public ushort DriverCount;

		// Token: 0x040001E2 RID: 482
		public uint DriverData;

		// Token: 0x040001E3 RID: 483
		public ushort Signature;
	}
}
