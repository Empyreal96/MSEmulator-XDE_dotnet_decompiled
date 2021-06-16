using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000037 RID: 55
	internal sealed class NonResidentAttributeRecord : AttributeRecord
	{
		// Token: 0x06000229 RID: 553 RVA: 0x0000BD7C File Offset: 0x00009F7C
		public NonResidentAttributeRecord(byte[] buffer, int offset, out int length)
		{
			this.Read(buffer, offset, out length);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000BD90 File Offset: 0x00009F90
		public NonResidentAttributeRecord(AttributeType type, string name, ushort id, AttributeFlags flags, long firstCluster, ulong numClusters, uint bytesPerCluster) : base(type, name, id, flags)
		{
			this._nonResidentFlag = 1;
			this.DataRuns = new List<DataRun>();
			this.DataRuns.Add(new DataRun(firstCluster, (long)numClusters, false));
			this._lastVCN = numClusters - 1UL;
			this._dataAllocatedSize = (ulong)bytesPerCluster * numClusters;
			this._dataRealSize = (ulong)bytesPerCluster * numClusters;
			this._initializedDataSize = (ulong)bytesPerCluster * numClusters;
			if ((flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				this._compressionUnitSize = 4;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000BE10 File Offset: 0x0000A010
		public NonResidentAttributeRecord(AttributeType type, string name, ushort id, AttributeFlags flags, long startVcn, List<DataRun> dataRuns) : base(type, name, id, flags)
		{
			this._nonResidentFlag = 1;
			this.DataRuns = dataRuns;
			this._startingVCN = (ulong)startVcn;
			if ((flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				this._compressionUnitSize = 4;
			}
			if (dataRuns != null && dataRuns.Count != 0)
			{
				this._lastVCN = this._startingVCN;
				foreach (DataRun dataRun in dataRuns)
				{
					this._lastVCN += (ulong)dataRun.RunLength;
				}
				this._lastVCN -= 1UL;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000BED0 File Offset: 0x0000A0D0
		public override long AllocatedLength
		{
			get
			{
				return (long)this._dataAllocatedSize;
			}
			set
			{
				this._dataAllocatedSize = (ulong)value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000BED9 File Offset: 0x0000A0D9
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000BEE1 File Offset: 0x0000A0E1
		public long CompressedDataSize
		{
			get
			{
				return (long)this._compressedSize;
			}
			set
			{
				this._compressedSize = (ulong)value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000BEEA File Offset: 0x0000A0EA
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000BEF7 File Offset: 0x0000A0F7
		public int CompressionUnitSize
		{
			get
			{
				return 1 << (int)this._compressionUnitSize;
			}
			set
			{
				this._compressionUnitSize = (ushort)MathUtilities.Log2(value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000BF06 File Offset: 0x0000A106
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000BF0E File Offset: 0x0000A10E
		public override long DataLength
		{
			get
			{
				return (long)this._dataRealSize;
			}
			set
			{
				this._dataRealSize = (ulong)value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000BF17 File Offset: 0x0000A117
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000BF1F File Offset: 0x0000A11F
		public List<DataRun> DataRuns { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000BF28 File Offset: 0x0000A128
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000BF30 File Offset: 0x0000A130
		public override long InitializedDataLength
		{
			get
			{
				return (long)this._initializedDataSize;
			}
			set
			{
				this._initializedDataSize = (ulong)value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000BF39 File Offset: 0x0000A139
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000BF41 File Offset: 0x0000A141
		public long LastVcn
		{
			get
			{
				return (long)this._lastVCN;
			}
			set
			{
				this._lastVCN = (ulong)value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000BF4C File Offset: 0x0000A14C
		public override int Size
		{
			get
			{
				byte b = 0;
				int num = (int)(((base.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None) ? 72 : 64);
				if (base.Name != null)
				{
					b = (byte)base.Name.Length;
				}
				ushort num2 = (ushort)MathUtilities.RoundUp(num + (int)(b * 2), 8);
				int num3 = 0;
				foreach (DataRun dataRun in this.DataRuns)
				{
					num3 += dataRun.Size;
				}
				num3++;
				return MathUtilities.RoundUp((int)num2 + num3, 8);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000BFEC File Offset: 0x0000A1EC
		public override long StartVcn
		{
			get
			{
				return (long)this._startingVCN;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
		public void ReplaceRun(DataRun oldRun, DataRun newRun)
		{
			int num = this.DataRuns.IndexOf(oldRun);
			if (num < 0)
			{
				throw new ArgumentException("Attempt to replace non-existant run", "oldRun");
			}
			this.DataRuns[num] = newRun;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000C030 File Offset: 0x0000A230
		public int RemoveRun(DataRun run)
		{
			int num = this.DataRuns.IndexOf(run);
			if (num < 0)
			{
				throw new ArgumentException("Attempt to remove non-existant run", "run");
			}
			this.DataRuns.RemoveAt(num);
			return num;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C06C File Offset: 0x0000A26C
		public void InsertRun(DataRun existingRun, DataRun newRun)
		{
			int num = this.DataRuns.IndexOf(existingRun);
			if (num < 0)
			{
				throw new ArgumentException("Attempt to replace non-existant run", "existingRun");
			}
			this.DataRuns.Insert(num + 1, newRun);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C0A9 File Offset: 0x0000A2A9
		public void InsertRun(int index, DataRun newRun)
		{
			this.DataRuns.Insert(index, newRun);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
		public override Range<long, long>[] GetClusters()
		{
			List<DataRun> dataRuns = this.DataRuns;
			long num = 0L;
			List<Range<long, long>> list = new List<Range<long, long>>(this.DataRuns.Count);
			foreach (DataRun dataRun in dataRuns)
			{
				if (!dataRun.IsSparse)
				{
					num += dataRun.RunOffset;
					list.Add(new Range<long, long>(num, dataRun.RunLength));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C144 File Offset: 0x0000A344
		public override IBuffer GetReadOnlyDataBuffer(INtfsContext context)
		{
			return new NonResidentDataBuffer(context, this);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C150 File Offset: 0x0000A350
		public override int Write(byte[] buffer, int offset)
		{
			ushort num = 64;
			if ((base.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				num += 8;
			}
			byte b = 0;
			ushort num2 = num;
			if (base.Name != null)
			{
				b = (byte)base.Name.Length;
			}
			ushort num3 = (ushort)MathUtilities.RoundUp((int)(num + (ushort)(b * 2)), 8);
			int num4 = 0;
			foreach (DataRun dataRun in this.DataRuns)
			{
				num4 += dataRun.Write(buffer, offset + (int)num3 + num4);
			}
			buffer[offset + (int)num3 + num4] = 0;
			num4++;
			int num5 = MathUtilities.RoundUp((int)num3 + num4, 8);
			EndianUtilities.WriteBytesLittleEndian((uint)this._type, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(num5, buffer, offset + 4);
			buffer[offset + 8] = this._nonResidentFlag;
			buffer[offset + 9] = b;
			EndianUtilities.WriteBytesLittleEndian(num2, buffer, offset + 10);
			EndianUtilities.WriteBytesLittleEndian((ushort)this._flags, buffer, offset + 12);
			EndianUtilities.WriteBytesLittleEndian(this._attributeId, buffer, offset + 14);
			EndianUtilities.WriteBytesLittleEndian(this._startingVCN, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this._lastVCN, buffer, offset + 24);
			EndianUtilities.WriteBytesLittleEndian(num3, buffer, offset + 32);
			EndianUtilities.WriteBytesLittleEndian(this._compressionUnitSize, buffer, offset + 34);
			EndianUtilities.WriteBytesLittleEndian(0U, buffer, offset + 36);
			EndianUtilities.WriteBytesLittleEndian(this._dataAllocatedSize, buffer, offset + 40);
			EndianUtilities.WriteBytesLittleEndian(this._dataRealSize, buffer, offset + 48);
			EndianUtilities.WriteBytesLittleEndian(this._initializedDataSize, buffer, offset + 56);
			if ((base.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				EndianUtilities.WriteBytesLittleEndian(this._compressedSize, buffer, offset + 64);
			}
			if (base.Name != null)
			{
				Array.Copy(Encoding.Unicode.GetBytes(base.Name), 0, buffer, offset + (int)num2, (int)(b * 2));
			}
			return num5;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000C318 File Offset: 0x0000A518
		public AttributeRecord Split(int suggestedSplitIdx)
		{
			int num;
			if (suggestedSplitIdx <= 0 || suggestedSplitIdx >= this.DataRuns.Count)
			{
				num = this.DataRuns.Count / 2;
			}
			else
			{
				num = suggestedSplitIdx;
			}
			long num2 = (long)this._startingVCN;
			long num3 = 0L;
			for (int i = 0; i < num; i++)
			{
				num2 += this.DataRuns[i].RunLength;
				num3 += this.DataRuns[i].RunOffset;
			}
			List<DataRun> list = new List<DataRun>();
			while (this.DataRuns.Count > num)
			{
				DataRun item = this.DataRuns[num];
				this.DataRuns.RemoveAt(num);
				list.Add(item);
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!list[j].IsSparse)
				{
					list[j].RunOffset += num3;
					break;
				}
			}
			this._lastVCN = (ulong)(num2 - 1L);
			return new NonResidentAttributeRecord(this._type, this._name, 0, this._flags, num2, list);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000C424 File Offset: 0x0000A624
		public override void Dump(TextWriter writer, string indent)
		{
			base.Dump(writer, indent);
			writer.WriteLine(indent + "     Starting VCN: " + this._startingVCN);
			writer.WriteLine(indent + "         Last VCN: " + this._lastVCN);
			writer.WriteLine(indent + "   Comp Unit Size: " + this._compressionUnitSize);
			writer.WriteLine(indent + "   Allocated Size: " + this._dataAllocatedSize);
			writer.WriteLine(indent + "        Real Size: " + this._dataRealSize);
			writer.WriteLine(indent + "   Init Data Size: " + this._initializedDataSize);
			if ((base.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None)
			{
				writer.WriteLine(indent + "  Compressed Size: " + this._compressedSize);
			}
			string text = string.Empty;
			foreach (DataRun arg in this.DataRuns)
			{
				text = text + " " + arg;
			}
			writer.WriteLine(indent + "        Data Runs:" + text);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C570 File Offset: 0x0000A770
		protected override void Read(byte[] buffer, int offset, out int length)
		{
			this.DataRuns = null;
			base.Read(buffer, offset, out length);
			this._startingVCN = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 16);
			this._lastVCN = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 24);
			this._dataRunsOffset = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 32);
			this._compressionUnitSize = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 34);
			this._dataAllocatedSize = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 40);
			this._dataRealSize = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 48);
			this._initializedDataSize = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 56);
			if ((base.Flags & (AttributeFlags.Compressed | AttributeFlags.Sparse)) != AttributeFlags.None && this._dataRunsOffset > 64)
			{
				this._compressedSize = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 64);
			}
			this.DataRuns = new List<DataRun>();
			int num;
			for (int i = (int)this._dataRunsOffset; i < length; i += num)
			{
				DataRun dataRun = new DataRun();
				num = dataRun.Read(buffer, offset + i);
				if (num == 1)
				{
					break;
				}
				this.DataRuns.Add(dataRun);
			}
		}

		// Token: 0x04000107 RID: 263
		private const ushort DefaultCompressionUnitSize = 4;

		// Token: 0x04000108 RID: 264
		private ulong _compressedSize;

		// Token: 0x04000109 RID: 265
		private ushort _compressionUnitSize;

		// Token: 0x0400010A RID: 266
		private ulong _dataAllocatedSize;

		// Token: 0x0400010B RID: 267
		private ulong _dataRealSize;

		// Token: 0x0400010C RID: 268
		private ushort _dataRunsOffset;

		// Token: 0x0400010D RID: 269
		private ulong _initializedDataSize;

		// Token: 0x0400010E RID: 270
		private ulong _lastVCN;

		// Token: 0x0400010F RID: 271
		private ulong _startingVCN;
	}
}
