using System;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x0200000D RID: 13
	public class ApplicationInstallStatusEventArgs : EventArgs
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00007785 File Offset: 0x00005985
		internal ApplicationInstallStatusEventArgs(ApplicationInstallStatus status, ApplicationInstallPhase phase, string message = "")
		{
			this.Status = status;
			this.Phase = phase;
			this.Message = message;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000077A2 File Offset: 0x000059A2
		// (set) Token: 0x06000146 RID: 326 RVA: 0x000077AA File Offset: 0x000059AA
		public ApplicationInstallStatus Status { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000077B3 File Offset: 0x000059B3
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000077BB File Offset: 0x000059BB
		public ApplicationInstallPhase Phase { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000077C4 File Offset: 0x000059C4
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000077CC File Offset: 0x000059CC
		public string Message { get; private set; }
	}
}
