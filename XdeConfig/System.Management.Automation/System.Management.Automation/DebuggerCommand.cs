using System;

namespace System.Management.Automation
{
	// Token: 0x020000F7 RID: 247
	internal class DebuggerCommand
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x0004B6F0 File Offset: 0x000498F0
		public DebuggerCommand(string command, DebuggerResumeAction? action, bool repeatOnEnter, bool executedByDebugger)
		{
			this.resumeAction = action;
			this.command = command;
			this.repeatOnEnter = repeatOnEnter;
			this.executedByDebugger = executedByDebugger;
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0004B715 File Offset: 0x00049915
		public DebuggerResumeAction? ResumeAction
		{
			get
			{
				return this.resumeAction;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0004B71D File Offset: 0x0004991D
		public string Command
		{
			get
			{
				return this.command;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0004B725 File Offset: 0x00049925
		public bool RepeatOnEnter
		{
			get
			{
				return this.repeatOnEnter;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x0004B72D File Offset: 0x0004992D
		public bool ExecutedByDebugger
		{
			get
			{
				return this.executedByDebugger;
			}
		}

		// Token: 0x04000626 RID: 1574
		private DebuggerResumeAction? resumeAction;

		// Token: 0x04000627 RID: 1575
		private string command;

		// Token: 0x04000628 RID: 1576
		private bool repeatOnEnter;

		// Token: 0x04000629 RID: 1577
		private bool executedByDebugger;
	}
}
