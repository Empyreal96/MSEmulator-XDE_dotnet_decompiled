using System;

namespace DiscUtils
{
	// Token: 0x0200002A RID: 42
	public sealed class UnixFileSystemInfo
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00004530 File Offset: 0x00002730
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00004538 File Offset: 0x00002738
		public long DeviceId { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00004541 File Offset: 0x00002741
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00004549 File Offset: 0x00002749
		public UnixFileType FileType { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00004552 File Offset: 0x00002752
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000455A File Offset: 0x0000275A
		public int GroupId { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00004563 File Offset: 0x00002763
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x0000456B File Offset: 0x0000276B
		public long Inode { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00004574 File Offset: 0x00002774
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000457C File Offset: 0x0000277C
		public int LinkCount { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00004585 File Offset: 0x00002785
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000458D File Offset: 0x0000278D
		public UnixFilePermissions Permissions { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00004596 File Offset: 0x00002796
		// (set) Token: 0x060001AF RID: 431 RVA: 0x0000459E File Offset: 0x0000279E
		public int UserId { get; set; }
	}
}
