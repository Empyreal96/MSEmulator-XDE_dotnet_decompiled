using System;

namespace System.Management.Automation
{
	// Token: 0x02000035 RID: 53
	public class HostInformationMessage
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000A07B File Offset: 0x0000827B
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000A083 File Offset: 0x00008283
		public string Message { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000A08C File Offset: 0x0000828C
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000A094 File Offset: 0x00008294
		public bool? NoNewLine { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000A09D File Offset: 0x0000829D
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000A0A5 File Offset: 0x000082A5
		public ConsoleColor? ForegroundColor { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000A0AE File Offset: 0x000082AE
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000A0B6 File Offset: 0x000082B6
		public ConsoleColor? BackgroundColor { get; set; }

		// Token: 0x060002AD RID: 685 RVA: 0x0000A0BF File Offset: 0x000082BF
		public override string ToString()
		{
			return this.Message;
		}
	}
}
