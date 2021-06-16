using System;
using System.Collections.Generic;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x0200004A RID: 74
	public class BiosPartitionedDiskBuilder : StreamBuilder
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00006B3C File Offset: 0x00004D3C
		public BiosPartitionedDiskBuilder(long capacity, Geometry biosGeometry)
		{
			this._capacity = capacity;
			this._biosGeometry = biosGeometry;
			this._bootSectors = new SparseMemoryStream();
			this._bootSectors.SetLength(capacity);
			this.PartitionTable = BiosPartitionTable.Initialize(this._bootSectors, this._biosGeometry);
			this._partitionContents = new Dictionary<int, BuilderExtent>();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00006B98 File Offset: 0x00004D98
		[Obsolete("Use the variant that takes VirtualDisk, this method breaks for disks with extended partitions", false)]
		public BiosPartitionedDiskBuilder(long capacity, byte[] bootSectors, Geometry biosGeometry)
		{
			if (bootSectors == null)
			{
				throw new ArgumentNullException("bootSectors");
			}
			this._capacity = capacity;
			this._biosGeometry = biosGeometry;
			this._bootSectors = new SparseMemoryStream();
			this._bootSectors.SetLength(capacity);
			this._bootSectors.Write(bootSectors, 0, bootSectors.Length);
			this.PartitionTable = new BiosPartitionTable(this._bootSectors, biosGeometry);
			this._partitionContents = new Dictionary<int, BuilderExtent>();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00006C0C File Offset: 0x00004E0C
		public BiosPartitionedDiskBuilder(VirtualDisk sourceDisk)
		{
			if (sourceDisk == null)
			{
				throw new ArgumentNullException("sourceDisk");
			}
			this._capacity = sourceDisk.Capacity;
			this._biosGeometry = sourceDisk.BiosGeometry;
			this._bootSectors = new SparseMemoryStream();
			this._bootSectors.SetLength(this._capacity);
			foreach (StreamExtent streamExtent in new BiosPartitionTable(sourceDisk).GetMetadataDiskExtents())
			{
				sourceDisk.Content.Position = streamExtent.Start;
				byte[] array = StreamUtilities.ReadExact(sourceDisk.Content, (int)streamExtent.Length);
				this._bootSectors.Position = streamExtent.Start;
				this._bootSectors.Write(array, 0, array.Length);
			}
			this.PartitionTable = new BiosPartitionTable(this._bootSectors, this._biosGeometry);
			this._partitionContents = new Dictionary<int, BuilderExtent>();
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00006D08 File Offset: 0x00004F08
		public BiosPartitionTable PartitionTable { get; }

		// Token: 0x0600030C RID: 780 RVA: 0x00006D10 File Offset: 0x00004F10
		public void SetPartitionContent(int index, SparseStream stream)
		{
			this._partitionContents[index] = new BuilderSparseStreamExtent(this.PartitionTable[index].FirstSector * 512L, stream);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00006D3C File Offset: 0x00004F3C
		public void UpdateBiosGeometry(Geometry geometry)
		{
			this.PartitionTable.UpdateBiosGeometry(geometry);
			this._biosGeometry = geometry;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00006D54 File Offset: 0x00004F54
		protected override List<BuilderExtent> FixExtents(out long totalLength)
		{
			totalLength = this._capacity;
			List<BuilderExtent> list = new List<BuilderExtent>();
			foreach (StreamExtent streamExtent in this.PartitionTable.GetMetadataDiskExtents())
			{
				this._bootSectors.Position = streamExtent.Start;
				byte[] buffer = StreamUtilities.ReadExact(this._bootSectors, (int)streamExtent.Length);
				list.Add(new BuilderBufferExtent(streamExtent.Start, buffer));
			}
			list.AddRange(this._partitionContents.Values);
			return list;
		}

		// Token: 0x040000A8 RID: 168
		private Geometry _biosGeometry;

		// Token: 0x040000A9 RID: 169
		private readonly SparseMemoryStream _bootSectors;

		// Token: 0x040000AA RID: 170
		private readonly long _capacity;

		// Token: 0x040000AB RID: 171
		private readonly Dictionary<int, BuilderExtent> _partitionContents;
	}
}
