using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x02000007 RID: 7
	public class DiskImageFileInfo
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000035C4 File Offset: 0x000017C4
		internal DiskImageFileInfo(Footer footer, DynamicHeader header, Stream vhdStream)
		{
			this._footer = footer;
			this._header = header;
			this._vhdStream = vhdStream;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000035E1 File Offset: 0x000017E1
		public string Cookie
		{
			get
			{
				return this._footer.Cookie;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000035EE File Offset: 0x000017EE
		public DateTime CreationTimestamp
		{
			get
			{
				return this._footer.Timestamp;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000035FB File Offset: 0x000017FB
		public string CreatorApp
		{
			get
			{
				return this._footer.CreatorApp;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003608 File Offset: 0x00001808
		public string CreatorHostOS
		{
			get
			{
				return this._footer.CreatorHostOS;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003615 File Offset: 0x00001815
		public int CreatorVersion
		{
			get
			{
				return (int)this._footer.CreatorVersion;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003622 File Offset: 0x00001822
		public long CurrentSize
		{
			get
			{
				return this._footer.CurrentSize;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000362F File Offset: 0x0000182F
		public FileType DiskType
		{
			get
			{
				return this._footer.DiskType;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000363C File Offset: 0x0000183C
		public long DynamicBlockCount
		{
			get
			{
				return (long)this._header.MaxTableEntries;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000364A File Offset: 0x0000184A
		public long DynamicBlockSize
		{
			get
			{
				return (long)((ulong)this._header.BlockSize);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003658 File Offset: 0x00001858
		public int DynamicChecksum
		{
			get
			{
				return (int)this._header.Checksum;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003665 File Offset: 0x00001865
		public string DynamicCookie
		{
			get
			{
				return this._header.Cookie;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003672 File Offset: 0x00001872
		public int DynamicHeaderVersion
		{
			get
			{
				return (int)this._header.HeaderVersion;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003680 File Offset: 0x00001880
		public IEnumerable<string> DynamicParentLocators
		{
			get
			{
				List<string> list = new List<string>(8);
				foreach (ParentLocator parentLocator in this._header.ParentLocators)
				{
					if (parentLocator.PlatformCode == "W2ku" || parentLocator.PlatformCode == "W2ru")
					{
						this._vhdStream.Position = parentLocator.PlatformDataOffset;
						byte[] bytes = StreamUtilities.ReadExact(this._vhdStream, parentLocator.PlatformDataLength);
						list.Add(Encoding.Unicode.GetString(bytes));
					}
				}
				return list;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000370D File Offset: 0x0000190D
		public DateTime DynamicParentTimestamp
		{
			get
			{
				return this._header.ParentTimestamp;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000371A File Offset: 0x0000191A
		public string DynamicParentUnicodeName
		{
			get
			{
				return this._header.ParentUnicodeName;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003727 File Offset: 0x00001927
		public Guid DynamicParentUniqueId
		{
			get
			{
				return this._header.ParentUniqueId;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003734 File Offset: 0x00001934
		public int Features
		{
			get
			{
				return (int)this._footer.Features;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003741 File Offset: 0x00001941
		public int FileFormatVersion
		{
			get
			{
				return (int)this._footer.FileFormatVersion;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000374E File Offset: 0x0000194E
		public int FooterChecksum
		{
			get
			{
				return (int)this._footer.Checksum;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000375B File Offset: 0x0000195B
		public Geometry Geometry
		{
			get
			{
				return this._footer.Geometry;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003768 File Offset: 0x00001968
		public long OriginalSize
		{
			get
			{
				return this._footer.OriginalSize;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003775 File Offset: 0x00001975
		public byte SavedState
		{
			get
			{
				return this._footer.SavedState;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003782 File Offset: 0x00001982
		public Guid UniqueId
		{
			get
			{
				return this._footer.UniqueId;
			}
		}

		// Token: 0x0400000B RID: 11
		private readonly Footer _footer;

		// Token: 0x0400000C RID: 12
		private readonly DynamicHeader _header;

		// Token: 0x0400000D RID: 13
		private readonly Stream _vhdStream;
	}
}
