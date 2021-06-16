using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200004B RID: 75
	internal sealed class ResidentAttributeRecord : AttributeRecord
	{
		// Token: 0x0600037C RID: 892 RVA: 0x00013CA0 File Offset: 0x00011EA0
		public ResidentAttributeRecord(byte[] buffer, int offset, out int length)
		{
			this.Read(buffer, offset, out length);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00013CB1 File Offset: 0x00011EB1
		public ResidentAttributeRecord(AttributeType type, string name, ushort id, bool indexed, AttributeFlags flags) : base(type, name, id, flags)
		{
			this._nonResidentFlag = 0;
			this._indexedFlag = (indexed ? 1 : 0);
			this._memoryBuffer = new SparseMemoryBuffer(1024);
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00013CE4 File Offset: 0x00011EE4
		// (set) Token: 0x0600037F RID: 895 RVA: 0x00013CF3 File Offset: 0x00011EF3
		public override long AllocatedLength
		{
			get
			{
				return MathUtilities.RoundUp(this.DataLength, 8L);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00013CFA File Offset: 0x00011EFA
		public IBuffer DataBuffer
		{
			get
			{
				return this._memoryBuffer;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00013D02 File Offset: 0x00011F02
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00013D0F File Offset: 0x00011F0F
		public override long DataLength
		{
			get
			{
				return this._memoryBuffer.Capacity;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00013D18 File Offset: 0x00011F18
		public int DataOffset
		{
			get
			{
				byte b = 0;
				if (base.Name != null)
				{
					b = (byte)base.Name.Length;
				}
				return MathUtilities.RoundUp((int)(24 + b * 2), 8);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00013D48 File Offset: 0x00011F48
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00013D50 File Offset: 0x00011F50
		public override long InitializedDataLength
		{
			get
			{
				return this.DataLength;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00013D58 File Offset: 0x00011F58
		public override int Size
		{
			get
			{
				byte b = 0;
				int num = 24;
				if (base.Name != null)
				{
					b = (byte)base.Name.Length;
				}
				return (int)MathUtilities.RoundUp((long)((ulong)((ushort)MathUtilities.RoundUp(num + (int)(b * 2), 8)) + (ulong)this._memoryBuffer.Capacity), 8L);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00013D9E File Offset: 0x00011F9E
		public override long StartVcn
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00013DA2 File Offset: 0x00011FA2
		public override IBuffer GetReadOnlyDataBuffer(INtfsContext context)
		{
			return this._memoryBuffer;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00013DAA File Offset: 0x00011FAA
		public override Range<long, long>[] GetClusters()
		{
			return new Range<long, long>[0];
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00013DB4 File Offset: 0x00011FB4
		public override int Write(byte[] buffer, int offset)
		{
			byte b = 0;
			ushort num = 0;
			if (base.Name != null)
			{
				num = 24;
				b = (byte)base.Name.Length;
			}
			ushort num2 = (ushort)MathUtilities.RoundUp((int)(24 + b * 2), 8);
			int num3 = (int)MathUtilities.RoundUp((long)((ulong)num2 + (ulong)this._memoryBuffer.Capacity), 8L);
			EndianUtilities.WriteBytesLittleEndian((uint)this._type, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(num3, buffer, offset + 4);
			buffer[offset + 8] = this._nonResidentFlag;
			buffer[offset + 9] = b;
			EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 10);
			EndianUtilities.WriteBytesLittleEndian((ushort)this._flags, buffer, offset + 12);
			EndianUtilities.WriteBytesLittleEndian(this._attributeId, buffer, offset + 14);
			EndianUtilities.WriteBytesLittleEndian((int)this._memoryBuffer.Capacity, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(num2, buffer, offset + 20);
			buffer[offset + 22] = this._indexedFlag;
			buffer[offset + 23] = 0;
			if (base.Name != null)
			{
				Array.Copy(Encoding.Unicode.GetBytes(base.Name), 0, buffer, offset + (int)num, (int)(b * 2));
			}
			this._memoryBuffer.Read(0L, buffer, offset + (int)num2, (int)this._memoryBuffer.Capacity);
			return num3;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00013ECC File Offset: 0x000120CC
		public override void Dump(TextWriter writer, string indent)
		{
			base.Dump(writer, indent);
			writer.WriteLine(indent + "     Data Length: " + this.DataLength);
			writer.WriteLine(indent + "         Indexed: " + this._indexedFlag);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00013F1C File Offset: 0x0001211C
		protected override void Read(byte[] buffer, int offset, out int length)
		{
			base.Read(buffer, offset, out length);
			uint num = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 16);
			ushort num2 = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 20);
			this._indexedFlag = buffer[offset + 22];
			if ((ulong)((uint)num2 + num) > (ulong)((long)length))
			{
				throw new IOException("Corrupt attribute, data outside of attribute");
			}
			this._memoryBuffer = new SparseMemoryBuffer(1024);
			this._memoryBuffer.Write(0L, buffer, offset + (int)num2, (int)num);
		}

		// Token: 0x0400016C RID: 364
		private byte _indexedFlag;

		// Token: 0x0400016D RID: 365
		private SparseMemoryBuffer _memoryBuffer;
	}
}
