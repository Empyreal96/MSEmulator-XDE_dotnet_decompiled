using System;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.Language
{
	// Token: 0x020005AF RID: 1455
	internal class EnterLoopExpression : Expression, IInstructionProvider
	{
		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x00142470 File Offset: 0x00140670
		public override bool CanReduce
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x00142473 File Offset: 0x00140673
		public override Type Type
		{
			get
			{
				return typeof(void);
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x0014247F File Offset: 0x0014067F
		public override ExpressionType NodeType
		{
			get
			{
				return ExpressionType.Extension;
			}
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x00142483 File Offset: 0x00140683
		public override Expression Reduce()
		{
			return ExpressionCache.Empty;
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x0014248A File Offset: 0x0014068A
		// (set) Token: 0x06003D97 RID: 15767 RVA: 0x00142492 File Offset: 0x00140692
		internal new PowerShellLoopExpression Loop { get; set; }

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06003D98 RID: 15768 RVA: 0x0014249B File Offset: 0x0014069B
		// (set) Token: 0x06003D99 RID: 15769 RVA: 0x001424A3 File Offset: 0x001406A3
		internal EnterLoopInstruction EnterLoopInstruction { get; private set; }

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003D9A RID: 15770 RVA: 0x001424AC File Offset: 0x001406AC
		// (set) Token: 0x06003D9B RID: 15771 RVA: 0x001424B4 File Offset: 0x001406B4
		internal int LoopStatementCount { get; set; }

		// Token: 0x06003D9C RID: 15772 RVA: 0x001424C0 File Offset: 0x001406C0
		public void AddInstructions(LightCompiler compiler)
		{
			if (this.LoopStatementCount < 300)
			{
				this.EnterLoopInstruction = new EnterLoopInstruction(this.Loop, compiler.Locals, 16, compiler.Instructions.Count);
				compiler.Instructions.Emit(this.EnterLoopInstruction);
			}
		}
	}
}
