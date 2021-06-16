using System;
using DiscUtils.Compression;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000041 RID: 65
	public sealed class NtfsOptions : DiscFileSystemOptions
	{
		// Token: 0x06000335 RID: 821 RVA: 0x000128ED File Offset: 0x00010AED
		internal NtfsOptions()
		{
			this.HideMetafiles = true;
			this.HideHiddenFiles = true;
			this.HideSystemFiles = true;
			this.HideDosFileNames = true;
			this.Compressor = new LZNT1();
			this.ReadCacheEnabled = true;
			this.FileLengthFromDirectoryEntries = true;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0001292A File Offset: 0x00010B2A
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00012932 File Offset: 0x00010B32
		public BlockCompressor Compressor { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0001293B File Offset: 0x00010B3B
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00012943 File Offset: 0x00010B43
		public bool FileLengthFromDirectoryEntries { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0001294C File Offset: 0x00010B4C
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00012954 File Offset: 0x00010B54
		public bool HideDosFileNames { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0001295D File Offset: 0x00010B5D
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00012965 File Offset: 0x00010B65
		public bool HideHiddenFiles { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0001296E File Offset: 0x00010B6E
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00012976 File Offset: 0x00010B76
		public bool HideMetafiles { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0001297F File Offset: 0x00010B7F
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00012987 File Offset: 0x00010B87
		public bool HideSystemFiles { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00012990 File Offset: 0x00010B90
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00012998 File Offset: 0x00010B98
		public bool ReadCacheEnabled { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000129A1 File Offset: 0x00010BA1
		// (set) Token: 0x06000345 RID: 837 RVA: 0x000129A9 File Offset: 0x00010BA9
		public ShortFileNameOption ShortNameCreation { get; set; }

		// Token: 0x06000346 RID: 838 RVA: 0x000129B4 File Offset: 0x00010BB4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Show: Normal ",
				this.HideMetafiles ? string.Empty : "Meta ",
				this.HideHiddenFiles ? string.Empty : "Hidden ",
				this.HideSystemFiles ? string.Empty : "System ",
				this.HideDosFileNames ? string.Empty : "ShortNames "
			});
		}
	}
}
