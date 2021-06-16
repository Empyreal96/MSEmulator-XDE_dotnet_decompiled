using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B0 RID: 1456
	internal class UpdatePositionExpr : Expression, IInstructionProvider
	{
		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003D9E RID: 15774 RVA: 0x00142517 File Offset: 0x00140717
		public override bool CanReduce
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x0014251A File Offset: 0x0014071A
		public override Type Type
		{
			get
			{
				return typeof(void);
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x00142526 File Offset: 0x00140726
		public override ExpressionType NodeType
		{
			get
			{
				return ExpressionType.Extension;
			}
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0014252A File Offset: 0x0014072A
		public UpdatePositionExpr(IScriptExtent extent, int sequencePoint, SymbolDocumentInfo debugSymbolDocument, bool checkBreakpoints)
		{
			this._extent = extent;
			this._checkBreakpoints = checkBreakpoints;
			this._debugSymbolDocument = debugSymbolDocument;
			this._sequencePoint = sequencePoint;
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x00142550 File Offset: 0x00140750
		public override Expression Reduce()
		{
			List<Expression> list = new List<Expression>();
			if (this._debugSymbolDocument != null)
			{
				list.Add(Expression.DebugInfo(this._debugSymbolDocument, this._extent.StartLineNumber, this._extent.StartColumnNumber, this._extent.EndLineNumber, this._extent.EndColumnNumber));
			}
			list.Add(Expression.Assign(Expression.Field(Compiler._functionContext, CachedReflectionInfo.FunctionContext__currentSequencePointIndex), ExpressionCache.Constant(this._sequencePoint)));
			if (this._checkBreakpoints)
			{
				list.Add(Expression.IfThen(Expression.GreaterThan(Expression.Field(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_DebuggingMode), ExpressionCache.Constant(0)), Expression.Call(Expression.Field(Compiler._executionContextParameter, CachedReflectionInfo.ExecutionContext_Debugger), CachedReflectionInfo.Debugger_OnSequencePointHit, new Expression[]
				{
					Compiler._functionContext
				})));
			}
			list.Add(ExpressionCache.Empty);
			return Expression.Block(list);
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00142634 File Offset: 0x00140834
		public void AddInstructions(LightCompiler compiler)
		{
			compiler.Instructions.Emit(UpdatePositionInstruction.Create(this._sequencePoint, this._checkBreakpoints));
		}

		// Token: 0x04001F0E RID: 7950
		private readonly IScriptExtent _extent;

		// Token: 0x04001F0F RID: 7951
		private readonly SymbolDocumentInfo _debugSymbolDocument;

		// Token: 0x04001F10 RID: 7952
		private readonly int _sequencePoint;

		// Token: 0x04001F11 RID: 7953
		private readonly bool _checkBreakpoints;
	}
}
