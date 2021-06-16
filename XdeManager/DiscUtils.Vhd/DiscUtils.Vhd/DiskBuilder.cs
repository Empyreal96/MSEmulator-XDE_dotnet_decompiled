using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000003 RID: 3
	public sealed class DiskBuilder : DiskImageBuilder
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002808 File Offset: 0x00000A08
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002810 File Offset: 0x00000A10
		public FileType DiskType { get; set; } = FileType.Dynamic;

		// Token: 0x06000021 RID: 33 RVA: 0x0000281C File Offset: 0x00000A1C
		public override DiskImageFileSpecification[] Build(string baseName)
		{
			if (string.IsNullOrEmpty(baseName))
			{
				throw new ArgumentException("Invalid base file name", "baseName");
			}
			if (base.Content == null)
			{
				throw new InvalidOperationException("No content stream specified");
			}
			List<DiskImageFileSpecification> list = new List<DiskImageFileSpecification>();
			Footer footer = new Footer(base.Geometry ?? Geometry.FromCapacity(base.Content.Length), base.Content.Length, this.DiskType);
			if (this.DiskType == FileType.Fixed)
			{
				footer.UpdateChecksum();
				byte[] buffer = new byte[512];
				footer.ToBytes(buffer, 0);
				SparseStream sparseStream = SparseStream.FromStream(new MemoryStream(buffer, false), Ownership.None);
				Stream stream = new ConcatStream(Ownership.None, new SparseStream[]
				{
					base.Content,
					sparseStream
				});
				list.Add(new DiskImageFileSpecification(baseName + ".vhd", new PassthroughStreamBuilder(stream)));
			}
			else
			{
				if (this.DiskType != FileType.Dynamic)
				{
					throw new InvalidOperationException("Only Fixed and Dynamic disk types supported");
				}
				list.Add(new DiskImageFileSpecification(baseName + ".vhd", new DynamicDiskBuilder(base.Content, footer, 2097152U)));
			}
			return list.ToArray();
		}
	}
}
