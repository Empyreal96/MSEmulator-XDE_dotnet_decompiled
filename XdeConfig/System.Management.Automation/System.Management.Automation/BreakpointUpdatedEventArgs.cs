using System;

namespace System.Management.Automation
{
	// Token: 0x020000E6 RID: 230
	public class BreakpointUpdatedEventArgs : EventArgs
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x00046463 File Offset: 0x00044663
		internal BreakpointUpdatedEventArgs(Breakpoint breakpoint, BreakpointUpdateType updateType, int breakpointCount)
		{
			this.Breakpoint = breakpoint;
			this.UpdateType = updateType;
			this.BreakpointCount = breakpointCount;
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00046480 File Offset: 0x00044680
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x00046488 File Offset: 0x00044688
		public Breakpoint Breakpoint { get; private set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00046491 File Offset: 0x00044691
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x00046499 File Offset: 0x00044699
		public BreakpointUpdateType UpdateType { get; private set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000464A2 File Offset: 0x000446A2
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x000464AA File Offset: 0x000446AA
		public int BreakpointCount { get; private set; }
	}
}
