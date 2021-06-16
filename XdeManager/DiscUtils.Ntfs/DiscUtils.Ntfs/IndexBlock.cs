using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000029 RID: 41
	internal class IndexBlock : FixupRecordBase
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00008E10 File Offset: 0x00007010
		public IndexBlock(Index index, bool isRoot, IndexEntry parentEntry, BiosParameterBlock bpb) : base("INDX", (int)bpb.BytesPerSector)
		{
			this._index = index;
			this._isRoot = isRoot;
			Stream allocationStream = index.AllocationStream;
			this._streamPosition = index.IndexBlockVcnToPosition(parentEntry.ChildrenVirtualCluster);
			allocationStream.Position = this._streamPosition;
			byte[] buffer = StreamUtilities.ReadExact(allocationStream, (int)index.IndexBufferSize);
			base.FromBytes(buffer, 0);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008E78 File Offset: 0x00007078
		private IndexBlock(Index index, bool isRoot, long vcn, BiosParameterBlock bpb) : base("INDX", (int)bpb.BytesPerSector, bpb.IndexBufferSize)
		{
			this._index = index;
			this._isRoot = isRoot;
			this._indexBlockVcn = (ulong)vcn;
			this._streamPosition = vcn * (long)((ulong)bpb.BytesPerSector) * (long)((ulong)bpb.SectorsPerCluster);
			this.Node = new IndexNode(new IndexNodeSaveFn(this.WriteToDisk), base.UpdateSequenceSize, this._index, isRoot, (uint)(bpb.IndexBufferSize - 24));
			this.WriteToDisk();
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008F00 File Offset: 0x00007100
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00008F08 File Offset: 0x00007108
		public IndexNode Node { get; private set; }

		// Token: 0x06000191 RID: 401 RVA: 0x00008F11 File Offset: 0x00007111
		internal static IndexBlock Initialize(Index index, bool isRoot, IndexEntry parentEntry, BiosParameterBlock bpb)
		{
			return new IndexBlock(index, isRoot, parentEntry.ChildrenVirtualCluster, bpb);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008F24 File Offset: 0x00007124
		internal void WriteToDisk()
		{
			byte[] array = new byte[this._index.IndexBufferSize];
			base.ToBytes(array, 0);
			Stream allocationStream = this._index.AllocationStream;
			allocationStream.Position = this._streamPosition;
			allocationStream.Write(array, 0, array.Length);
			allocationStream.Flush();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008F74 File Offset: 0x00007174
		protected override void Read(byte[] buffer, int offset)
		{
			this._logSequenceNumber = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 8);
			this._indexBlockVcn = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 16);
			this.Node = new IndexNode(new IndexNodeSaveFn(this.WriteToDisk), base.UpdateSequenceSize, this._index, this._isRoot, buffer, offset + 24);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008FCE File Offset: 0x000071CE
		protected override ushort Write(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this._logSequenceNumber, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this._indexBlockVcn, buffer, offset + 16);
			return 24 + this.Node.WriteTo(buffer, offset + 24);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009003 File Offset: 0x00007203
		protected override int CalcSize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000C4 RID: 196
		private const int FieldSize = 24;

		// Token: 0x040000C5 RID: 197
		private readonly Index _index;

		// Token: 0x040000C6 RID: 198
		private ulong _indexBlockVcn;

		// Token: 0x040000C7 RID: 199
		private readonly bool _isRoot;

		// Token: 0x040000C8 RID: 200
		private ulong _logSequenceNumber;

		// Token: 0x040000C9 RID: 201
		private readonly long _streamPosition;
	}
}
