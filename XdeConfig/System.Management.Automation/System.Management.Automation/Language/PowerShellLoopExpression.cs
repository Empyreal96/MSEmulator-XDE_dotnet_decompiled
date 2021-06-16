using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.Language
{
	// Token: 0x020005AE RID: 1454
	internal class PowerShellLoopExpression : Expression, IInstructionProvider
	{
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x001423BB File Offset: 0x001405BB
		public override bool CanReduce
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x001423BE File Offset: 0x001405BE
		public override Type Type
		{
			get
			{
				return typeof(void);
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x001423CA File Offset: 0x001405CA
		public override ExpressionType NodeType
		{
			get
			{
				return ExpressionType.Extension;
			}
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x001423CE File Offset: 0x001405CE
		internal PowerShellLoopExpression(IEnumerable<Expression> exprs)
		{
			this._exprs = exprs;
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x001423DD File Offset: 0x001405DD
		public override Expression Reduce()
		{
			return Expression.Block(this._exprs);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x001423EC File Offset: 0x001405EC
		public void AddInstructions(LightCompiler compiler)
		{
			EnterLoopInstruction enterLoopInstruction = null;
			compiler.PushLabelBlock(LabelScopeKind.Statement);
			foreach (Expression expression in this._exprs)
			{
				compiler.CompileAsVoid(expression);
				EnterLoopExpression enterLoopExpression = expression as EnterLoopExpression;
				if (enterLoopExpression != null)
				{
					enterLoopInstruction = enterLoopExpression.EnterLoopInstruction;
				}
			}
			compiler.PopLabelBlock(LabelScopeKind.Statement);
			if (enterLoopInstruction != null)
			{
				enterLoopInstruction.FinishLoop(compiler.Instructions.Count);
			}
		}

		// Token: 0x04001F0A RID: 7946
		private readonly IEnumerable<Expression> _exprs;
	}
}
