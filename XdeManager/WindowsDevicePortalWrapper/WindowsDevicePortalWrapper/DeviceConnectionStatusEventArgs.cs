using System;

namespace Microsoft.Tools.WindowsDevicePortal
{
	// Token: 0x02000011 RID: 17
	public class DeviceConnectionStatusEventArgs : EventArgs
	{
		// Token: 0x0600014F RID: 335 RVA: 0x000077D5 File Offset: 0x000059D5
		internal DeviceConnectionStatusEventArgs(DeviceConnectionStatus status, DeviceConnectionPhase phase, string message = "")
		{
			this.Status = status;
			this.Phase = phase;
			this.Message = message;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000077F2 File Offset: 0x000059F2
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000077FA File Offset: 0x000059FA
		public DeviceConnectionStatus Status { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007803 File Offset: 0x00005A03
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000780B File Offset: 0x00005A0B
		public DeviceConnectionPhase Phase { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00007814 File Offset: 0x00005A14
		// (set) Token: 0x06000155 RID: 341 RVA: 0x0000781C File Offset: 0x00005A1C
		public string Message { get; private set; }
	}
}
