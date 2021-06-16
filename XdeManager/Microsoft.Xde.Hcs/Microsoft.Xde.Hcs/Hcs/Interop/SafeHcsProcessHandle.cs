using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000018 RID: 24
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public sealed class SafeHcsProcessHandle : SafeHcsHandle<SafeHcsProcessHandle>
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00004354 File Offset: 0x00002554
		public SafeHcsProcessHandle() : base(HcsFactory.GetHcs())
		{
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004361 File Offset: 0x00002561
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00004369 File Offset: 0x00002569
		public SafeHcsSystemHandle System { get; internal set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004372 File Offset: 0x00002572
		// (set) Token: 0x060000BB RID: 187 RVA: 0x0000437A File Offset: 0x0000257A
		public HCS_PROCESS_INFORMATION Info { get; internal set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004383 File Offset: 0x00002583
		// (set) Token: 0x060000BD RID: 189 RVA: 0x0000438B File Offset: 0x0000258B
		public AnonymousPipeClientStream StdInStream { get; internal set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004394 File Offset: 0x00002594
		// (set) Token: 0x060000BF RID: 191 RVA: 0x0000439C File Offset: 0x0000259C
		public AnonymousPipeClientStream StdOutStream { get; internal set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000043A5 File Offset: 0x000025A5
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000043AD File Offset: 0x000025AD
		public AnonymousPipeClientStream StdErrStream { get; internal set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000043B6 File Offset: 0x000025B6
		protected override HcsCloseHandleFunc CloseFunc
		{
			get
			{
				return new HcsCloseHandleFunc(base.GetHcs().CloseProcess);
			}
		}
	}
}
