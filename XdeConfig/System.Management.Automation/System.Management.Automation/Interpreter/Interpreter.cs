using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D5 RID: 1749
	internal sealed class Interpreter
	{
		// Token: 0x06004891 RID: 18577 RVA: 0x0017EDA8 File Offset: 0x0017CFA8
		internal Interpreter(string name, LocalVariables locals, HybridReferenceDictionary<LabelTarget, BranchLabel> labelMapping, InstructionArray instructions, DebugInfo[] debugInfos, int compilationThreshold)
		{
			this._name = name;
			this._localCount = locals.LocalCount;
			this._closureVariables = locals.ClosureVariables;
			this._instructions = instructions;
			this._objects = instructions.Objects;
			this._labels = instructions.Labels;
			this._labelMapping = labelMapping;
			this._debugInfos = debugInfos;
			this._compilationThreshold = compilationThreshold;
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06004892 RID: 18578 RVA: 0x0017EE13 File Offset: 0x0017D013
		internal int ClosureSize
		{
			get
			{
				if (this._closureVariables == null)
				{
					return 0;
				}
				return this._closureVariables.Count;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x0017EE2A File Offset: 0x0017D02A
		internal int LocalCount
		{
			get
			{
				return this._localCount;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06004894 RID: 18580 RVA: 0x0017EE32 File Offset: 0x0017D032
		internal bool CompileSynchronously
		{
			get
			{
				return this._compilationThreshold <= 1;
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x0017EE40 File Offset: 0x0017D040
		internal InstructionArray Instructions
		{
			get
			{
				return this._instructions;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06004896 RID: 18582 RVA: 0x0017EE48 File Offset: 0x0017D048
		internal Dictionary<ParameterExpression, LocalVariable> ClosureVariables
		{
			get
			{
				return this._closureVariables;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x0017EE50 File Offset: 0x0017D050
		internal HybridReferenceDictionary<LabelTarget, BranchLabel> LabelMapping
		{
			get
			{
				return this._labelMapping;
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x0017EE58 File Offset: 0x0017D058
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Run(InterpretedFrame frame)
		{
			Instruction[] instructions = this._instructions.Instructions;
			int i = frame.InstructionIndex;
			while (i < instructions.Length)
			{
				i += instructions[i].Run(frame);
				frame.InstructionIndex = i;
			}
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0017EE94 File Offset: 0x0017D094
		internal static void AbortThreadIfRequested(InterpretedFrame frame, int targetLabelIndex)
		{
			ExceptionHandler currentAbortHandler = frame.CurrentAbortHandler;
			int index = frame.Interpreter._labels[targetLabelIndex].Index;
			if (currentAbortHandler != null && !currentAbortHandler.IsInsideCatchBlock(index) && !currentAbortHandler.IsInsideFinallyBlock(index))
			{
				frame.CurrentAbortHandler = null;
				Thread currentThread = Thread.CurrentThread;
				if ((currentThread.ThreadState & ThreadState.AbortRequested) != ThreadState.Running)
				{
					currentThread.Abort(Interpreter.AnyAbortException.ExceptionState);
				}
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x0600489A RID: 18586 RVA: 0x0017EF04 File Offset: 0x0017D104
		internal int ReturnAndRethrowLabelIndex
		{
			get
			{
				return this._labels.Length - 1;
			}
		}

		// Token: 0x04002366 RID: 9062
		internal const int RethrowOnReturn = 2147483647;

		// Token: 0x04002367 RID: 9063
		internal static readonly object NoValue = new object();

		// Token: 0x04002368 RID: 9064
		internal readonly int _compilationThreshold;

		// Token: 0x04002369 RID: 9065
		private readonly int _localCount;

		// Token: 0x0400236A RID: 9066
		private readonly HybridReferenceDictionary<LabelTarget, BranchLabel> _labelMapping;

		// Token: 0x0400236B RID: 9067
		private readonly Dictionary<ParameterExpression, LocalVariable> _closureVariables;

		// Token: 0x0400236C RID: 9068
		private readonly InstructionArray _instructions;

		// Token: 0x0400236D RID: 9069
		internal readonly object[] _objects;

		// Token: 0x0400236E RID: 9070
		internal readonly RuntimeLabel[] _labels;

		// Token: 0x0400236F RID: 9071
		internal readonly string _name;

		// Token: 0x04002370 RID: 9072
		internal readonly DebugInfo[] _debugInfos;

		// Token: 0x04002371 RID: 9073
		[ThreadStatic]
		internal static ThreadAbortException AnyAbortException = null;
	}
}
