using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000575 RID: 1397
	public class DynamicKeywordStatementAst : StatementAst
	{
		// Token: 0x060039DB RID: 14811 RVA: 0x00131F90 File Offset: 0x00130190
		public DynamicKeywordStatementAst(IScriptExtent extent, IEnumerable<CommandElementAst> commandElements) : base(extent)
		{
			if (commandElements == null || commandElements.Count<CommandElementAst>() <= 0)
			{
				throw PSTraceSource.NewArgumentException("commandElements");
			}
			this.CommandElements = new ReadOnlyCollection<CommandElementAst>(commandElements.ToArray<CommandElementAst>());
			base.SetParents<CommandElementAst>(this.CommandElements);
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x060039DC RID: 14812 RVA: 0x00131FCD File Offset: 0x001301CD
		// (set) Token: 0x060039DD RID: 14813 RVA: 0x00131FD5 File Offset: 0x001301D5
		public ReadOnlyCollection<CommandElementAst> CommandElements { get; private set; }

		// Token: 0x060039DE RID: 14814 RVA: 0x00131FE0 File Offset: 0x001301E0
		public override Ast Copy()
		{
			IEnumerable<CommandElementAst> commandElements = Ast.CopyElements<CommandElementAst>(this.CommandElements);
			return new DynamicKeywordStatementAst(base.Extent, commandElements)
			{
				Keyword = this.Keyword,
				LCurly = this.LCurly,
				FunctionName = this.FunctionName,
				InstanceName = Ast.CopyElement<ExpressionAst>(this.InstanceName),
				OriginalInstanceName = Ast.CopyElement<ExpressionAst>(this.OriginalInstanceName),
				BodyExpression = Ast.CopyElement<ExpressionAst>(this.BodyExpression),
				ElementName = this.ElementName
			};
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x0013206C File Offset: 0x0013026C
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitDynamicKeywordStatement(this);
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x0013208C File Offset: 0x0013028C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = AstVisitAction.Continue;
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitDynamicKeywordStatement(this);
				if (astVisitAction == AstVisitAction.SkipChildren)
				{
					return visitor.CheckForPostAction(this, AstVisitAction.Continue);
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				foreach (CommandElementAst commandElementAst in this.CommandElements)
				{
					astVisitAction = commandElementAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x0013210C File Offset: 0x0013030C
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.CommandElements[0].GetInferredType(context);
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x060039E2 RID: 14818 RVA: 0x00132120 File Offset: 0x00130320
		// (set) Token: 0x060039E3 RID: 14819 RVA: 0x00132128 File Offset: 0x00130328
		internal DynamicKeyword Keyword
		{
			get
			{
				return this._keyword;
			}
			set
			{
				this._keyword = value.Copy();
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x00132136 File Offset: 0x00130336
		// (set) Token: 0x060039E5 RID: 14821 RVA: 0x0013213E File Offset: 0x0013033E
		internal Token LCurly { get; set; }

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x00132147 File Offset: 0x00130347
		// (set) Token: 0x060039E7 RID: 14823 RVA: 0x0013214F File Offset: 0x0013034F
		internal Token FunctionName { get; set; }

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x00132158 File Offset: 0x00130358
		// (set) Token: 0x060039E9 RID: 14825 RVA: 0x00132160 File Offset: 0x00130360
		internal ExpressionAst InstanceName { get; set; }

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x00132169 File Offset: 0x00130369
		// (set) Token: 0x060039EB RID: 14827 RVA: 0x00132171 File Offset: 0x00130371
		internal ExpressionAst OriginalInstanceName { get; set; }

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x0013217A File Offset: 0x0013037A
		// (set) Token: 0x060039ED RID: 14829 RVA: 0x00132182 File Offset: 0x00130382
		internal ExpressionAst BodyExpression { get; set; }

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x0013218B File Offset: 0x0013038B
		// (set) Token: 0x060039EF RID: 14831 RVA: 0x00132193 File Offset: 0x00130393
		internal string ElementName { get; set; }

		// Token: 0x060039F0 RID: 14832 RVA: 0x0013219C File Offset: 0x0013039C
		internal PipelineAst GenerateCommandCallPipelineAst()
		{
			if (this._commandCallPipelineAst != null)
			{
				return this._commandCallPipelineAst;
			}
			Collection<CommandElementAst> collection = new Collection<CommandElementAst>();
			if (string.IsNullOrEmpty(this.Keyword.ImplementingModule))
			{
				collection.Add(new StringConstantExpressionAst(this.FunctionName.Extent, this.FunctionName.Text, StringConstantType.BareWord));
			}
			else
			{
				collection.Add(new StringConstantExpressionAst(this.FunctionName.Extent, this.Keyword.ImplementingModule + '\\' + this.FunctionName.Text, StringConstantType.BareWord));
			}
			ExpressionAst bodyExpression = this.BodyExpression;
			HashtableAst hashtableAst = bodyExpression as HashtableAst;
			if (this.Keyword.DirectCall)
			{
				if (this.Keyword.NameMode != DynamicKeywordNameMode.NoName)
				{
					collection.Add(new CommandParameterAst(this.FunctionName.Extent, "InstanceName", this.InstanceName, this.FunctionName.Extent));
				}
				if (hashtableAst != null)
				{
					bool flag = true;
					foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
					{
						StringConstantExpressionAst stringConstantExpressionAst = tuple.Item1 as StringConstantExpressionAst;
						if (stringConstantExpressionAst == null)
						{
							flag = false;
							break;
						}
						if (!this.Keyword.Properties.ContainsKey(stringConstantExpressionAst.Value))
						{
							flag = false;
							break;
						}
						if (tuple.Item2 is ErrorStatementAst)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						using (IEnumerator<Tuple<ExpressionAst, StatementAst>> enumerator2 = hashtableAst.KeyValuePairs.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Tuple<ExpressionAst, StatementAst> tuple2 = enumerator2.Current;
								StringConstantExpressionAst stringConstantExpressionAst2 = (StringConstantExpressionAst)tuple2.Item1;
								ExpressionAst argument = new SubExpressionAst(this.FunctionName.Extent, new StatementBlockAst(this.FunctionName.Extent, new StatementAst[]
								{
									(StatementAst)tuple2.Item2.Copy()
								}, null));
								collection.Add(new CommandParameterAst(this.FunctionName.Extent, stringConstantExpressionAst2.Value, argument, this.LCurly.Extent));
							}
							goto IL_3F9;
						}
					}
					collection.Add(new CommandParameterAst(this.FunctionName.Extent, "InvalidPropertyHashtable", hashtableAst, this.LCurly.Extent));
				}
			}
			else
			{
				InvokeMemberExpressionAst argument2 = new InvokeMemberExpressionAst(this.FunctionName.Extent, new TypeExpressionAst(this.FunctionName.Extent, new TypeName(this.FunctionName.Extent, typeof(DynamicKeyword).FullName)), new StringConstantExpressionAst(this.FunctionName.Extent, "GetKeyword", StringConstantType.BareWord), new List<ExpressionAst>
				{
					new StringConstantExpressionAst(this.FunctionName.Extent, this.FunctionName.Text, StringConstantType.BareWord)
				}, true);
				collection.Add(new CommandParameterAst(this.FunctionName.Extent, "KeywordData", argument2, this.LCurly.Extent));
				collection.Add(new CommandParameterAst(this.FunctionName.Extent, "Name", this.InstanceName, this.LCurly.Extent));
				collection.Add(new CommandParameterAst(this.LCurly.Extent, "Value", bodyExpression, this.LCurly.Extent));
				string value = string.Concat(new object[]
				{
					this.FunctionName.Extent.File,
					"::",
					this.FunctionName.Extent.StartLineNumber,
					"::",
					this.FunctionName.Extent.StartColumnNumber,
					"::",
					this.FunctionName.Extent.Text
				});
				collection.Add(new CommandParameterAst(this.LCurly.Extent, "SourceMetadata", new StringConstantExpressionAst(this.FunctionName.Extent, value, StringConstantType.BareWord), this.LCurly.Extent));
			}
			IL_3F9:
			CommandAst commandAst = new CommandAst(this.FunctionName.Extent, collection, TokenKind.Unknown, null);
			commandAst.DefiningKeyword = this.Keyword;
			this._commandCallPipelineAst = new PipelineAst(this.FunctionName.Extent, commandAst);
			return this._commandCallPipelineAst;
		}

		// Token: 0x04001D40 RID: 7488
		private DynamicKeyword _keyword;

		// Token: 0x04001D41 RID: 7489
		private PipelineAst _commandCallPipelineAst;
	}
}
