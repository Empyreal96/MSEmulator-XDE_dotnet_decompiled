using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000073 RID: 115
	public class ErrorContext
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x0001BFE4 File Offset: 0x0001A1E4
		internal ErrorContext(object originalObject, object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001C009 File Offset: 0x0001A209
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x0001C011 File Offset: 0x0001A211
		internal bool Traced { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001C01A File Offset: 0x0001A21A
		public Exception Error { get; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001C022 File Offset: 0x0001A222
		public object OriginalObject { get; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001C02A File Offset: 0x0001A22A
		public object Member { get; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001C032 File Offset: 0x0001A232
		public string Path { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001C03A File Offset: 0x0001A23A
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0001C042 File Offset: 0x0001A242
		public bool Handled { get; set; }
	}
}
