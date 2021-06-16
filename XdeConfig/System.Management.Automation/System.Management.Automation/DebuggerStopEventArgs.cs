using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x020000E4 RID: 228
	public class DebuggerStopEventArgs : EventArgs
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x000463EC File Offset: 0x000445EC
		internal DebuggerStopEventArgs(InvocationInfo invocationInfo, List<Breakpoint> breakpoints)
		{
			this.InvocationInfo = invocationInfo;
			this.Breakpoints = new ReadOnlyCollection<Breakpoint>(breakpoints);
			this.ResumeAction = DebuggerResumeAction.Continue;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0004640E File Offset: 0x0004460E
		public DebuggerStopEventArgs(InvocationInfo invocationInfo, Collection<Breakpoint> breakpoints, DebuggerResumeAction resumeAction)
		{
			this.InvocationInfo = invocationInfo;
			this.Breakpoints = new ReadOnlyCollection<Breakpoint>(breakpoints);
			this.ResumeAction = resumeAction;
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00046430 File Offset: 0x00044630
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x00046438 File Offset: 0x00044638
		public InvocationInfo InvocationInfo { get; internal set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00046441 File Offset: 0x00044641
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x00046449 File Offset: 0x00044649
		public ReadOnlyCollection<Breakpoint> Breakpoints { get; private set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00046452 File Offset: 0x00044652
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x0004645A File Offset: 0x0004465A
		public DebuggerResumeAction ResumeAction { get; set; }
	}
}
