using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Language
{
	// Token: 0x020005A2 RID: 1442
	internal class FunctionContext
	{
		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0013A35D File Offset: 0x0013855D
		internal IScriptExtent CurrentPosition
		{
			get
			{
				if (this._sequencePoints == null)
				{
					return PositionUtilities.EmptyExtent;
				}
				return this._sequencePoints[this._currentSequencePointIndex];
			}
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x0013A37A File Offset: 0x0013857A
		internal void PushTrapHandlers(Type[] type, Action<FunctionContext>[] handler, Type[] tupleType)
		{
			this._traps.Add(Tuple.Create<Type[], Action<FunctionContext>[], Type[]>(type, handler, tupleType));
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x0013A38F File Offset: 0x0013858F
		internal void PopTrapHandlers()
		{
			this._traps.RemoveAt(this._traps.Count - 1);
		}

		// Token: 0x04001EA2 RID: 7842
		internal ScriptBlock _scriptBlock;

		// Token: 0x04001EA3 RID: 7843
		internal string _file;

		// Token: 0x04001EA4 RID: 7844
		internal bool _debuggerHidden;

		// Token: 0x04001EA5 RID: 7845
		internal bool _debuggerStepThrough;

		// Token: 0x04001EA6 RID: 7846
		internal IScriptExtent[] _sequencePoints;

		// Token: 0x04001EA7 RID: 7847
		internal ExecutionContext _executionContext;

		// Token: 0x04001EA8 RID: 7848
		internal Pipe _outputPipe;

		// Token: 0x04001EA9 RID: 7849
		internal BitArray _breakPoints;

		// Token: 0x04001EAA RID: 7850
		internal List<LineBreakpoint> _boundBreakpoints;

		// Token: 0x04001EAB RID: 7851
		internal int _currentSequencePointIndex;

		// Token: 0x04001EAC RID: 7852
		internal MutableTuple _localsTuple;

		// Token: 0x04001EAD RID: 7853
		internal List<Tuple<Type[], Action<FunctionContext>[], Type[]>> _traps = new List<Tuple<Type[], Action<FunctionContext>[], Type[]>>();

		// Token: 0x04001EAE RID: 7854
		internal string _functionName;
	}
}
