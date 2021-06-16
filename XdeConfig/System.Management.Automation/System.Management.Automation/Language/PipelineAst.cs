using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000569 RID: 1385
	public class PipelineAst : PipelineBaseAst
	{
		// Token: 0x0600396F RID: 14703 RVA: 0x0012FE22 File Offset: 0x0012E022
		public PipelineAst(IScriptExtent extent, IEnumerable<CommandBaseAst> pipelineElements) : base(extent)
		{
			if (pipelineElements == null || !pipelineElements.Any<CommandBaseAst>())
			{
				throw PSTraceSource.NewArgumentException("pipelineElements");
			}
			this.PipelineElements = new ReadOnlyCollection<CommandBaseAst>(pipelineElements.ToArray<CommandBaseAst>());
			base.SetParents<CommandBaseAst>(this.PipelineElements);
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x0012FE60 File Offset: 0x0012E060
		public PipelineAst(IScriptExtent extent, CommandBaseAst commandAst) : base(extent)
		{
			if (commandAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandAst");
			}
			this.PipelineElements = new ReadOnlyCollection<CommandBaseAst>(new CommandBaseAst[]
			{
				commandAst
			});
			base.SetParent(commandAst);
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x0012FEA0 File Offset: 0x0012E0A0
		// (set) Token: 0x06003972 RID: 14706 RVA: 0x0012FEA8 File Offset: 0x0012E0A8
		public ReadOnlyCollection<CommandBaseAst> PipelineElements { get; private set; }

		// Token: 0x06003973 RID: 14707 RVA: 0x0012FEB4 File Offset: 0x0012E0B4
		public override ExpressionAst GetPureExpression()
		{
			if (this.PipelineElements.Count != 1)
			{
				return null;
			}
			CommandExpressionAst commandExpressionAst = this.PipelineElements[0] as CommandExpressionAst;
			if (commandExpressionAst != null && !commandExpressionAst.Redirections.Any<RedirectionAst>())
			{
				return commandExpressionAst.Expression;
			}
			return null;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x0012FEFC File Offset: 0x0012E0FC
		public override Ast Copy()
		{
			CommandBaseAst[] pipelineElements = Ast.CopyElements<CommandBaseAst>(this.PipelineElements);
			return new PipelineAst(base.Extent, pipelineElements);
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x0012FF21 File Offset: 0x0012E121
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.PipelineElements.Last<CommandBaseAst>().GetInferredType(context);
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x0012FF34 File Offset: 0x0012E134
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitPipeline(this);
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x0012FF40 File Offset: 0x0012E140
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitPipeline(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.PipelineElements.Count; i++)
				{
					CommandBaseAst commandBaseAst = this.PipelineElements[i];
					astVisitAction = commandBaseAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
