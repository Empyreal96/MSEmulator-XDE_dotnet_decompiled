using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000017 RID: 23
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public sealed class SafeHcsSystemHandle : SafeHcsHandle<SafeHcsSystemHandle>
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00004322 File Offset: 0x00002522
		public SafeHcsSystemHandle() : base(HcsFactory.GetHcs())
		{
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000432F File Offset: 0x0000252F
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00004337 File Offset: 0x00002537
		public string Id { get; internal set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004340 File Offset: 0x00002540
		protected override HcsCloseHandleFunc CloseFunc
		{
			get
			{
				return new HcsCloseHandleFunc(base.GetHcs().CloseComputeSystem);
			}
		}
	}
}
