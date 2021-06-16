using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x020005F7 RID: 1527
	internal class UsingExpressionAstSearcher : AstSearcher
	{
		// Token: 0x060041CD RID: 16845 RVA: 0x0015C0E8 File Offset: 0x0015A2E8
		internal static IEnumerable<Ast> FindAllUsingExpressionExceptForWorkflow(Ast ast)
		{
			UsingExpressionAstSearcher usingExpressionAstSearcher = new UsingExpressionAstSearcher((Ast astParam) => astParam is UsingExpressionAst, false, true);
			ast.InternalVisit(usingExpressionAstSearcher);
			return usingExpressionAstSearcher.Results;
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x0015C128 File Offset: 0x0015A328
		private UsingExpressionAstSearcher(Func<Ast, bool> callback, bool stopOnFirst, bool searchNestedScriptBlocks) : base(callback, stopOnFirst, searchNestedScriptBlocks)
		{
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x0015C133 File Offset: 0x0015A333
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst ast)
		{
			if (ast.IsWorkflow)
			{
				return AstVisitAction.SkipChildren;
			}
			return base.CheckScriptBlock(ast);
		}
	}
}
