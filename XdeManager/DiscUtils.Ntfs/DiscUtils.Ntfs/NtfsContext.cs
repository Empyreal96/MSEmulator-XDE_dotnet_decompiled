using System;
using System.IO;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200003B RID: 59
	internal sealed class NtfsContext : INtfsContext
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000D3D3 File Offset: 0x0000B5D3
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000D3DB File Offset: 0x0000B5DB
		public Stream RawStream { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000D3EC File Offset: 0x0000B5EC
		public AttributeDefinitions AttributeDefinitions { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000D3F5 File Offset: 0x0000B5F5
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000D3FD File Offset: 0x0000B5FD
		public UpperCase UpperCase { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000D406 File Offset: 0x0000B606
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000D40E File Offset: 0x0000B60E
		public BiosParameterBlock BiosParameterBlock { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000D417 File Offset: 0x0000B617
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000D41F File Offset: 0x0000B61F
		public MasterFileTable Mft { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000D428 File Offset: 0x0000B628
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000D430 File Offset: 0x0000B630
		public ClusterBitmap ClusterBitmap { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000D439 File Offset: 0x0000B639
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000D441 File Offset: 0x0000B641
		public SecurityDescriptors SecurityDescriptors { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000D44A File Offset: 0x0000B64A
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000D452 File Offset: 0x0000B652
		public ObjectIds ObjectIds { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000D45B File Offset: 0x0000B65B
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000D463 File Offset: 0x0000B663
		public ReparsePoints ReparsePoints { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000D46C File Offset: 0x0000B66C
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000D474 File Offset: 0x0000B674
		public Quotas Quotas { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000D47D File Offset: 0x0000B67D
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000D485 File Offset: 0x0000B685
		public NtfsOptions Options { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000D48E File Offset: 0x0000B68E
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000D496 File Offset: 0x0000B696
		public GetFileByIndexFn GetFileByIndex { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000D49F File Offset: 0x0000B69F
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000D4A7 File Offset: 0x0000B6A7
		public GetFileByRefFn GetFileByRef { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		public GetDirectoryByIndexFn GetDirectoryByIndex { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000D4C1 File Offset: 0x0000B6C1
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000D4C9 File Offset: 0x0000B6C9
		public GetDirectoryByRefFn GetDirectoryByRef { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000D4D2 File Offset: 0x0000B6D2
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000D4DA File Offset: 0x0000B6DA
		public AllocateFileFn AllocateFile { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000D4E3 File Offset: 0x0000B6E3
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000D4EB File Offset: 0x0000B6EB
		public ForgetFileFn ForgetFile { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000D4FC File Offset: 0x0000B6FC
		public bool ReadOnly { get; set; }
	}
}
