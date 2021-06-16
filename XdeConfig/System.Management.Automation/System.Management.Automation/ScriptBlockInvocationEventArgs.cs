using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000492 RID: 1170
	internal sealed class ScriptBlockInvocationEventArgs : EventArgs
	{
		// Token: 0x06003430 RID: 13360 RVA: 0x0011CC80 File Offset: 0x0011AE80
		internal ScriptBlockInvocationEventArgs(ScriptBlock scriptBlock, bool useLocalScope, ScriptBlock.ErrorHandlingBehavior errorHandlingBehavior, object dollarUnder, object input, object scriptThis, Pipe outputPipe, InvocationInfo invocationInfo, params object[] args)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("scriptBlock");
			}
			this.ScriptBlock = scriptBlock;
			this.OutputPipe = outputPipe;
			this.UseLocalScope = useLocalScope;
			this.ErrorHandlingBehavior = errorHandlingBehavior;
			this.DollarUnder = dollarUnder;
			this.Input = input;
			this.ScriptThis = scriptThis;
			this.InvocationInfo = invocationInfo;
			this.Args = args;
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06003431 RID: 13361 RVA: 0x0011CCE6 File Offset: 0x0011AEE6
		// (set) Token: 0x06003432 RID: 13362 RVA: 0x0011CCEE File Offset: 0x0011AEEE
		internal ScriptBlock ScriptBlock { get; set; }

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06003433 RID: 13363 RVA: 0x0011CCF7 File Offset: 0x0011AEF7
		// (set) Token: 0x06003434 RID: 13364 RVA: 0x0011CCFF File Offset: 0x0011AEFF
		internal bool UseLocalScope { get; set; }

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x0011CD08 File Offset: 0x0011AF08
		// (set) Token: 0x06003436 RID: 13366 RVA: 0x0011CD10 File Offset: 0x0011AF10
		internal ScriptBlock.ErrorHandlingBehavior ErrorHandlingBehavior { get; set; }

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06003437 RID: 13367 RVA: 0x0011CD19 File Offset: 0x0011AF19
		// (set) Token: 0x06003438 RID: 13368 RVA: 0x0011CD21 File Offset: 0x0011AF21
		internal object DollarUnder { get; set; }

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06003439 RID: 13369 RVA: 0x0011CD2A File Offset: 0x0011AF2A
		// (set) Token: 0x0600343A RID: 13370 RVA: 0x0011CD32 File Offset: 0x0011AF32
		internal object Input { get; set; }

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x0011CD3B File Offset: 0x0011AF3B
		// (set) Token: 0x0600343C RID: 13372 RVA: 0x0011CD43 File Offset: 0x0011AF43
		internal object ScriptThis { get; set; }

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x0011CD4C File Offset: 0x0011AF4C
		// (set) Token: 0x0600343E RID: 13374 RVA: 0x0011CD54 File Offset: 0x0011AF54
		internal Pipe OutputPipe { get; set; }

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600343F RID: 13375 RVA: 0x0011CD5D File Offset: 0x0011AF5D
		// (set) Token: 0x06003440 RID: 13376 RVA: 0x0011CD65 File Offset: 0x0011AF65
		internal InvocationInfo InvocationInfo { get; set; }

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06003441 RID: 13377 RVA: 0x0011CD6E File Offset: 0x0011AF6E
		// (set) Token: 0x06003442 RID: 13378 RVA: 0x0011CD76 File Offset: 0x0011AF76
		internal object[] Args { get; set; }

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06003443 RID: 13379 RVA: 0x0011CD7F File Offset: 0x0011AF7F
		// (set) Token: 0x06003444 RID: 13380 RVA: 0x0011CD87 File Offset: 0x0011AF87
		internal Exception Exception { get; set; }
	}
}
