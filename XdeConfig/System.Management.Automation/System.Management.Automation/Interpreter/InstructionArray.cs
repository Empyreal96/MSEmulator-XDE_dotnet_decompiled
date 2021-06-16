using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006CF RID: 1743
	[DebuggerTypeProxy(typeof(InstructionArray.DebugView))]
	internal struct InstructionArray
	{
		// Token: 0x060047F7 RID: 18423 RVA: 0x0017D3A1 File Offset: 0x0017B5A1
		internal InstructionArray(int maxStackDepth, int maxContinuationDepth, Instruction[] instructions, object[] objects, RuntimeLabel[] labels, List<KeyValuePair<int, object>> debugCookies)
		{
			this.MaxStackDepth = maxStackDepth;
			this.MaxContinuationDepth = maxContinuationDepth;
			this.Instructions = instructions;
			this.DebugCookies = debugCookies;
			this.Objects = objects;
			this.Labels = labels;
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060047F8 RID: 18424 RVA: 0x0017D3D0 File Offset: 0x0017B5D0
		internal int Length
		{
			get
			{
				return this.Instructions.Length;
			}
		}

		// Token: 0x04002329 RID: 9001
		internal readonly int MaxStackDepth;

		// Token: 0x0400232A RID: 9002
		internal readonly int MaxContinuationDepth;

		// Token: 0x0400232B RID: 9003
		internal readonly Instruction[] Instructions;

		// Token: 0x0400232C RID: 9004
		internal readonly object[] Objects;

		// Token: 0x0400232D RID: 9005
		internal readonly RuntimeLabel[] Labels;

		// Token: 0x0400232E RID: 9006
		internal readonly List<KeyValuePair<int, object>> DebugCookies;

		// Token: 0x020006D0 RID: 1744
		internal sealed class DebugView
		{
			// Token: 0x060047F9 RID: 18425 RVA: 0x0017D3DA File Offset: 0x0017B5DA
			public DebugView(InstructionArray array)
			{
				this._array = array;
			}

			// Token: 0x17000F52 RID: 3922
			// (get) Token: 0x060047FA RID: 18426 RVA: 0x0017D406 File Offset: 0x0017B606
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public InstructionList.DebugView.InstructionView[] A0
			{
				get
				{
					return InstructionList.DebugView.GetInstructionViews(this._array.Instructions, this._array.Objects, (int index) => this._array.Labels[index].Index, this._array.DebugCookies);
				}
			}

			// Token: 0x0400232F RID: 9007
			private readonly InstructionArray _array;
		}
	}
}
