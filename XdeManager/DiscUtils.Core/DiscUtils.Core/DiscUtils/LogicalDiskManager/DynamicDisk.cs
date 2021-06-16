using System;
using System.IO;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x02000060 RID: 96
	internal class DynamicDisk : IDiagnosticTraceable
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x0000A6B8 File Offset: 0x000088B8
		internal DynamicDisk(VirtualDisk disk)
		{
			this._disk = disk;
			this._header = DynamicDisk.GetPrivateHeader(this._disk);
			TocBlock tableOfContents = this.GetTableOfContents();
			long position = this._header.ConfigurationStartLba * 512L + tableOfContents.Item1Start * 512L;
			this._disk.Content.Position = position;
			this.Database = new Database(this._disk.Content);
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000A732 File Offset: 0x00008932
		public SparseStream Content
		{
			get
			{
				return this._disk.Content;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000A73F File Offset: 0x0000893F
		public Database Database { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000A747 File Offset: 0x00008947
		public long DataOffset
		{
			get
			{
				return this._header.DataStartLba;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000A754 File Offset: 0x00008954
		public Guid GroupId
		{
			get
			{
				if (!string.IsNullOrEmpty(this._header.DiskGroupId))
				{
					return new Guid(this._header.DiskGroupId);
				}
				return Guid.Empty;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000A77E File Offset: 0x0000897E
		public Guid Id
		{
			get
			{
				return new Guid(this._header.DiskId);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000A790 File Offset: 0x00008990
		public void Dump(TextWriter writer, string linePrefix)
		{
			writer.WriteLine(linePrefix + "DISK (" + this._header.DiskId + ")");
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"      Metadata Version: ",
				this._header.Version >> 16 & 65535U,
				".",
				this._header.Version & 65535U
			}));
			writer.WriteLine(linePrefix + "             Timestamp: " + this._header.Timestamp);
			writer.WriteLine(linePrefix + "               Disk Id: " + this._header.DiskId);
			writer.WriteLine(linePrefix + "               Host Id: " + this._header.HostId);
			writer.WriteLine(linePrefix + "         Disk Group Id: " + this._header.DiskGroupId);
			writer.WriteLine(linePrefix + "       Disk Group Name: " + this._header.DiskGroupName);
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"            Data Start: ",
				this._header.DataStartLba,
				" (Sectors)"
			}));
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"             Data Size: ",
				this._header.DataSizeLba,
				" (Sectors)"
			}));
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"   Configuration Start: ",
				this._header.ConfigurationStartLba,
				" (Sectors)"
			}));
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"    Configuration Size: ",
				this._header.ConfigurationSizeLba,
				" (Sectors)"
			}));
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"              TOC Size: ",
				this._header.TocSizeLba,
				" (Sectors)"
			}));
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"              Next TOC: ",
				this._header.NextTocLba,
				" (Sectors)"
			}));
			writer.WriteLine(linePrefix + "     Number of Configs: " + this._header.NumberOfConfigs);
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"           Config Size: ",
				this._header.ConfigurationSizeLba,
				" (Sectors)"
			}));
			writer.WriteLine(linePrefix + "        Number of Logs: " + this._header.NumberOfLogs);
			writer.WriteLine(string.Concat(new object[]
			{
				linePrefix,
				"              Log Size: ",
				this._header.LogSizeLba,
				" (Sectors)"
			}));
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000AAAC File Offset: 0x00008CAC
		internal static PrivateHeader GetPrivateHeader(VirtualDisk disk)
		{
			if (disk.IsPartitioned)
			{
				long num = 0L;
				PartitionTable partitions = disk.Partitions;
				if (partitions is BiosPartitionTable)
				{
					num = 3072L;
				}
				else
				{
					foreach (PartitionInfo partitionInfo in partitions.Partitions)
					{
						if (partitionInfo.GuidType == GuidPartitionTypes.WindowsLdmMetadata)
						{
							num = partitionInfo.LastSector * 512L;
						}
					}
				}
				if (num != 0L)
				{
					disk.Content.Position = num;
					byte[] array = new byte[512];
					disk.Content.Read(array, 0, array.Length);
					PrivateHeader privateHeader = new PrivateHeader();
					privateHeader.ReadFrom(array, 0);
					return privateHeader;
				}
			}
			return null;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000AB78 File Offset: 0x00008D78
		private TocBlock GetTableOfContents()
		{
			byte[] array = new byte[this._header.TocSizeLba * 512L];
			this._disk.Content.Position = this._header.ConfigurationStartLba * 512L + this._header.TocSizeLba * 512L;
			this._disk.Content.Read(array, 0, array.Length);
			TocBlock tocBlock = new TocBlock();
			tocBlock.ReadFrom(array, 0);
			if (tocBlock.Signature == "TOCBLOCK")
			{
				return tocBlock;
			}
			return null;
		}

		// Token: 0x04000127 RID: 295
		private readonly VirtualDisk _disk;

		// Token: 0x04000128 RID: 296
		private readonly PrivateHeader _header;
	}
}
