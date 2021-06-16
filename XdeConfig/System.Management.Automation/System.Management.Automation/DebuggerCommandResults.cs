using System;

namespace System.Management.Automation
{
	// Token: 0x020000F5 RID: 245
	public sealed class DebuggerCommandResults
	{
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0004ADFA File Offset: 0x00048FFA
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0004AE02 File Offset: 0x00049002
		public DebuggerResumeAction? ResumeAction { get; private set; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0004AE0B File Offset: 0x0004900B
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0004AE13 File Offset: 0x00049013
		public bool EvaluatedByDebugger { get; private set; }

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0004AE1C File Offset: 0x0004901C
		private DebuggerCommandResults()
		{
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0004AE24 File Offset: 0x00049024
		public DebuggerCommandResults(DebuggerResumeAction? resumeAction, bool evaluatedByDebugger)
		{
			this.ResumeAction = resumeAction;
			this.EvaluatedByDebugger = evaluatedByDebugger;
		}
	}
}
