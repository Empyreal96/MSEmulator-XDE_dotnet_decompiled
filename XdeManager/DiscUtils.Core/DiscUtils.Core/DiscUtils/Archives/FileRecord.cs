using System;

namespace DiscUtils.Archives
{
	// Token: 0x0200008B RID: 139
	internal sealed class FileRecord
	{
		// Token: 0x060004D0 RID: 1232 RVA: 0x0000E211 File Offset: 0x0000C411
		public FileRecord(string name, long start, long length)
		{
			this.Name = name;
			this.Start = start;
			this.Length = length;
		}

		// Token: 0x040001C4 RID: 452
		public long Length;

		// Token: 0x040001C5 RID: 453
		public string Name;

		// Token: 0x040001C6 RID: 454
		public long Start;
	}
}
