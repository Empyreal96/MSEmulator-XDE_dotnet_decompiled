using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000540 RID: 1344
	public class ScriptBlockAst : Ast, IParameterMetadataProvider
	{
		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x0012B0D7 File Offset: 0x001292D7
		// (set) Token: 0x060037C9 RID: 14281 RVA: 0x0012B0DF File Offset: 0x001292DF
		internal bool IsConfiguration { get; private set; }

		// Token: 0x060037CA RID: 14282 RVA: 0x0012B0E8 File Offset: 0x001292E8
		public ScriptBlockAst(IScriptExtent extent, IEnumerable<UsingStatementAst> usingStatements, IEnumerable<AttributeAst> attributes, ParamBlockAst paramBlock, NamedBlockAst beginBlock, NamedBlockAst processBlock, NamedBlockAst endBlock, NamedBlockAst dynamicParamBlock) : base(extent)
		{
			this.SetUsingStatements(usingStatements);
			if (attributes != null)
			{
				this.Attributes = new ReadOnlyCollection<AttributeAst>(attributes.ToArray<AttributeAst>());
				base.SetParents<AttributeAst>(this.Attributes);
			}
			else
			{
				this.Attributes = ScriptBlockAst.EmptyAttributeList;
			}
			if (paramBlock != null)
			{
				this.ParamBlock = paramBlock;
				base.SetParent(paramBlock);
			}
			if (beginBlock != null)
			{
				this.BeginBlock = beginBlock;
				base.SetParent(beginBlock);
			}
			if (processBlock != null)
			{
				this.ProcessBlock = processBlock;
				base.SetParent(processBlock);
			}
			if (endBlock != null)
			{
				this.EndBlock = endBlock;
				base.SetParent(endBlock);
			}
			if (dynamicParamBlock != null)
			{
				this.DynamicParamBlock = dynamicParamBlock;
				base.SetParent(dynamicParamBlock);
			}
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0012B194 File Offset: 0x00129394
		public ScriptBlockAst(IScriptExtent extent, IEnumerable<UsingStatementAst> usingStatements, ParamBlockAst paramBlock, NamedBlockAst beginBlock, NamedBlockAst processBlock, NamedBlockAst endBlock, NamedBlockAst dynamicParamBlock) : this(extent, usingStatements, null, paramBlock, beginBlock, processBlock, endBlock, dynamicParamBlock)
		{
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x0012B1B3 File Offset: 0x001293B3
		public ScriptBlockAst(IScriptExtent extent, ParamBlockAst paramBlock, NamedBlockAst beginBlock, NamedBlockAst processBlock, NamedBlockAst endBlock, NamedBlockAst dynamicParamBlock) : this(extent, null, paramBlock, beginBlock, processBlock, endBlock, dynamicParamBlock)
		{
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0012B1C5 File Offset: 0x001293C5
		public ScriptBlockAst(IScriptExtent extent, List<UsingStatementAst> usingStatements, ParamBlockAst paramBlock, StatementBlockAst statements, bool isFilter) : this(extent, usingStatements, null, paramBlock, statements, isFilter, false)
		{
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x0012B1D6 File Offset: 0x001293D6
		public ScriptBlockAst(IScriptExtent extent, ParamBlockAst paramBlock, StatementBlockAst statements, bool isFilter) : this(extent, null, null, paramBlock, statements, isFilter, false)
		{
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0012B1E6 File Offset: 0x001293E6
		public ScriptBlockAst(IScriptExtent extent, ParamBlockAst paramBlock, StatementBlockAst statements, bool isFilter, bool isConfiguration) : this(extent, null, null, paramBlock, statements, isFilter, isConfiguration)
		{
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0012B1F7 File Offset: 0x001293F7
		public ScriptBlockAst(IScriptExtent extent, IEnumerable<UsingStatementAst> usingStatements, ParamBlockAst paramBlock, StatementBlockAst statements, bool isFilter, bool isConfiguration) : this(extent, usingStatements, null, paramBlock, statements, isFilter, isConfiguration)
		{
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0012B209 File Offset: 0x00129409
		public ScriptBlockAst(IScriptExtent extent, IEnumerable<AttributeAst> attributes, ParamBlockAst paramBlock, StatementBlockAst statements, bool isFilter, bool isConfiguration) : this(extent, null, attributes, paramBlock, statements, isFilter, isConfiguration)
		{
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x0012B21C File Offset: 0x0012941C
		public ScriptBlockAst(IScriptExtent extent, IEnumerable<UsingStatementAst> usingStatements, IEnumerable<AttributeAst> attributes, ParamBlockAst paramBlock, StatementBlockAst statements, bool isFilter, bool isConfiguration) : base(extent)
		{
			this.SetUsingStatements(usingStatements);
			if (attributes != null)
			{
				this.Attributes = new ReadOnlyCollection<AttributeAst>(attributes.ToArray<AttributeAst>());
				base.SetParents<AttributeAst>(this.Attributes);
			}
			else
			{
				this.Attributes = ScriptBlockAst.EmptyAttributeList;
			}
			if (statements == null)
			{
				throw PSTraceSource.NewArgumentNullException("statements");
			}
			if (paramBlock != null)
			{
				this.ParamBlock = paramBlock;
				base.SetParent(paramBlock);
			}
			if (isFilter)
			{
				this.ProcessBlock = new NamedBlockAst(statements.Extent, TokenKind.Process, statements, true);
				base.SetParent(this.ProcessBlock);
				return;
			}
			this.EndBlock = new NamedBlockAst(statements.Extent, TokenKind.End, statements, true);
			this.IsConfiguration = isConfiguration;
			base.SetParent(this.EndBlock);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0012B2E0 File Offset: 0x001294E0
		private void SetUsingStatements(IEnumerable<UsingStatementAst> usingStatements)
		{
			if (usingStatements != null)
			{
				this.UsingStatements = new ReadOnlyCollection<UsingStatementAst>(usingStatements.ToArray<UsingStatementAst>());
				base.SetParents<UsingStatementAst>(this.UsingStatements);
				return;
			}
			this.UsingStatements = ScriptBlockAst.EmptyUsingStatementList;
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x0012B30E File Offset: 0x0012950E
		// (set) Token: 0x060037D5 RID: 14293 RVA: 0x0012B316 File Offset: 0x00129516
		public ReadOnlyCollection<AttributeAst> Attributes { get; private set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x060037D6 RID: 14294 RVA: 0x0012B31F File Offset: 0x0012951F
		// (set) Token: 0x060037D7 RID: 14295 RVA: 0x0012B327 File Offset: 0x00129527
		public ReadOnlyCollection<UsingStatementAst> UsingStatements { get; private set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x060037D8 RID: 14296 RVA: 0x0012B330 File Offset: 0x00129530
		// (set) Token: 0x060037D9 RID: 14297 RVA: 0x0012B338 File Offset: 0x00129538
		public ParamBlockAst ParamBlock { get; private set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x060037DA RID: 14298 RVA: 0x0012B341 File Offset: 0x00129541
		// (set) Token: 0x060037DB RID: 14299 RVA: 0x0012B349 File Offset: 0x00129549
		public NamedBlockAst BeginBlock { get; private set; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x060037DC RID: 14300 RVA: 0x0012B352 File Offset: 0x00129552
		// (set) Token: 0x060037DD RID: 14301 RVA: 0x0012B35A File Offset: 0x0012955A
		public NamedBlockAst ProcessBlock { get; private set; }

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x060037DE RID: 14302 RVA: 0x0012B363 File Offset: 0x00129563
		// (set) Token: 0x060037DF RID: 14303 RVA: 0x0012B36B File Offset: 0x0012956B
		public NamedBlockAst EndBlock { get; private set; }

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x0012B374 File Offset: 0x00129574
		// (set) Token: 0x060037E1 RID: 14305 RVA: 0x0012B37C File Offset: 0x0012957C
		public NamedBlockAst DynamicParamBlock { get; private set; }

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x060037E2 RID: 14306 RVA: 0x0012B385 File Offset: 0x00129585
		// (set) Token: 0x060037E3 RID: 14307 RVA: 0x0012B38D File Offset: 0x0012958D
		public ScriptRequirements ScriptRequirements { get; internal set; }

		// Token: 0x060037E4 RID: 14308 RVA: 0x0012B398 File Offset: 0x00129598
		public CommentHelpInfo GetHelpContent()
		{
			Dictionary<Ast, Token[]> scriptBlockTokenCache = new Dictionary<Ast, Token[]>();
			Tuple<List<Token>, List<string>> helpCommentTokens = HelpCommentsParser.GetHelpCommentTokens(this, scriptBlockTokenCache);
			if (helpCommentTokens != null)
			{
				return HelpCommentsParser.GetHelpContents(helpCommentTokens.Item1, helpCommentTokens.Item2);
			}
			return null;
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x0012B3CC File Offset: 0x001295CC
		public ScriptBlock GetScriptBlock()
		{
			Parser parser = new Parser();
			SemanticChecks.CheckAst(parser, this);
			if (parser.ErrorList.Any<ParseError>())
			{
				throw new ParseException(parser.ErrorList.ToArray());
			}
			return new ScriptBlock(this, false);
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0012B40C File Offset: 0x0012960C
		public override Ast Copy()
		{
			ParamBlockAst paramBlock = Ast.CopyElement<ParamBlockAst>(this.ParamBlock);
			NamedBlockAst beginBlock = Ast.CopyElement<NamedBlockAst>(this.BeginBlock);
			NamedBlockAst processBlock = Ast.CopyElement<NamedBlockAst>(this.ProcessBlock);
			NamedBlockAst endBlock = Ast.CopyElement<NamedBlockAst>(this.EndBlock);
			NamedBlockAst dynamicParamBlock = Ast.CopyElement<NamedBlockAst>(this.DynamicParamBlock);
			AttributeAst[] attributes = Ast.CopyElements<AttributeAst>(this.Attributes);
			UsingStatementAst[] usingStatements = Ast.CopyElements<UsingStatementAst>(this.UsingStatements);
			return new ScriptBlockAst(base.Extent, usingStatements, attributes, paramBlock, beginBlock, processBlock, endBlock, dynamicParamBlock)
			{
				IsConfiguration = this.IsConfiguration,
				ScriptRequirements = this.ScriptRequirements
			};
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x0012B4A8 File Offset: 0x001296A8
		internal string ToStringForSerialization()
		{
			string text = this.ToString();
			if (base.Parent != null)
			{
				text = text.Substring(1, text.Length - 2);
			}
			return text;
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0012B4E4 File Offset: 0x001296E4
		internal string ToStringForSerialization(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple, int initialStartOffset, int initialEndOffset)
		{
			List<VariableExpressionAst> item = usingVariablesTuple.Item1;
			string text = usingVariablesTuple.Item2;
			List<Ast> list = new List<Ast>(item);
			if (this.ParamBlock != null)
			{
				list.Add(this.ParamBlock);
			}
			int startOffset = base.Extent.StartOffset;
			int num = initialStartOffset - startOffset;
			int num2 = initialEndOffset - startOffset;
			string text2 = this.ToString();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Ast ast2 in from ast in list
			orderby ast.Extent.StartOffset
			select ast)
			{
				int num3 = ast2.Extent.StartOffset - startOffset;
				int num4 = ast2.Extent.EndOffset - startOffset;
				if (num3 >= num)
				{
					if (num3 >= num2)
					{
						break;
					}
					VariableExpressionAst variableExpressionAst = ast2 as VariableExpressionAst;
					if (variableExpressionAst != null)
					{
						string userPath = variableExpressionAst.VariablePath.UserPath;
						string str = variableExpressionAst.Splatted ? "@" : "$";
						string value = str + "__using_" + userPath;
						stringBuilder.Append(text2.Substring(num, num3 - num));
						stringBuilder.Append(value);
						num = num4;
					}
					else
					{
						ParamBlockAst paramBlockAst = ast2 as ParamBlockAst;
						int num5;
						if (paramBlockAst.Parameters.Count == 0)
						{
							num5 = num4 - 1;
						}
						else
						{
							ParameterAst parameterAst = paramBlockAst.Parameters[0];
							num5 = ((parameterAst.Attributes.Count == 0) ? (parameterAst.Name.Extent.StartOffset - startOffset) : (parameterAst.Attributes[0].Extent.StartOffset - startOffset));
							text += ",\n";
						}
						stringBuilder.Append(text2.Substring(num, num5 - num));
						stringBuilder.Append(text);
						num = num5;
					}
				}
			}
			stringBuilder.Append(text2.Substring(num, num2 - num));
			string text3 = stringBuilder.ToString();
			if (base.Parent != null && initialStartOffset == base.Extent.StartOffset && initialEndOffset == base.Extent.EndOffset)
			{
				text3 = text3.Substring(1, text3.Length - 2);
			}
			return text3;
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x0012BA98 File Offset: 0x00129C98
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			if (this.BeginBlock != null)
			{
				foreach (PSTypeName typename in this.BeginBlock.GetInferredType(context))
				{
					yield return typename;
				}
			}
			if (this.ProcessBlock != null)
			{
				foreach (PSTypeName typename2 in this.ProcessBlock.GetInferredType(context))
				{
					yield return typename2;
				}
			}
			if (this.EndBlock != null)
			{
				foreach (PSTypeName typename3 in this.EndBlock.GetInferredType(context))
				{
					yield return typename3;
				}
			}
			yield break;
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x0012BABC File Offset: 0x00129CBC
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitScriptBlock(this);
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x0012BAC8 File Offset: 0x00129CC8
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitScriptBlock(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				if (astVisitAction == AstVisitAction.Continue)
				{
					foreach (UsingStatementAst usingStatementAst in this.UsingStatements)
					{
						astVisitAction = usingStatementAst.InternalVisit(astVisitor);
						if (astVisitAction != AstVisitAction.Continue)
						{
							break;
						}
					}
				}
				if (astVisitAction == AstVisitAction.Continue)
				{
					foreach (AttributeAst attributeAst in this.Attributes)
					{
						astVisitAction = attributeAst.InternalVisit(astVisitor);
						if (astVisitAction != AstVisitAction.Continue)
						{
							break;
						}
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue && this.ParamBlock != null)
			{
				astVisitAction = this.ParamBlock.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.DynamicParamBlock != null)
			{
				astVisitAction = this.DynamicParamBlock.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.BeginBlock != null)
			{
				astVisitAction = this.BeginBlock.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.ProcessBlock != null)
			{
				astVisitAction = this.ProcessBlock.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue && this.EndBlock != null)
			{
				astVisitAction = this.EndBlock.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x0012BC08 File Offset: 0x00129E08
		RuntimeDefinedParameterDictionary IParameterMetadataProvider.GetParameterMetadata(bool automaticPositions, ref bool usesCmdletBinding)
		{
			if (this.ParamBlock != null)
			{
				return Compiler.GetParameterMetaData(this.ParamBlock.Parameters, automaticPositions, ref usesCmdletBinding);
			}
			return new RuntimeDefinedParameterDictionary
			{
				Data = RuntimeDefinedParameterDictionary.EmptyParameterArray
			};
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x0012BDE8 File Offset: 0x00129FE8
		IEnumerable<Attribute> IParameterMetadataProvider.GetScriptBlockAttributes()
		{
			for (int index = 0; index < this.Attributes.Count; index++)
			{
				AttributeAst attributeAst = this.Attributes[index];
				yield return Compiler.GetAttribute(attributeAst);
			}
			if (this.ParamBlock != null)
			{
				for (int index2 = 0; index2 < this.ParamBlock.Attributes.Count; index2++)
				{
					AttributeAst attributeAst2 = this.ParamBlock.Attributes[index2];
					yield return Compiler.GetAttribute(attributeAst2);
				}
			}
			yield break;
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060037EE RID: 14318 RVA: 0x0012BE05 File Offset: 0x0012A005
		ReadOnlyCollection<ParameterAst> IParameterMetadataProvider.Parameters
		{
			get
			{
				if (this.ParamBlock == null)
				{
					return null;
				}
				return this.ParamBlock.Parameters;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060037EF RID: 14319 RVA: 0x0012BE1C File Offset: 0x0012A01C
		ScriptBlockAst IParameterMetadataProvider.Body
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0012BE1F File Offset: 0x0012A01F
		PowerShell IParameterMetadataProvider.GetPowerShell(ExecutionContext context, Dictionary<string, object> variables, bool isTrustedInput, bool filterNonUsingVariables, bool? createLocalScope, params object[] args)
		{
			ExecutionContext.CheckStackDepth();
			return ScriptBlockToPowerShellConverter.Convert(this, null, isTrustedInput, context, variables, filterNonUsingVariables, createLocalScope, args);
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x0012BE36 File Offset: 0x0012A036
		string IParameterMetadataProvider.GetWithInputHandlingForInvokeCommand()
		{
			return this.GetWithInputHandlingForInvokeCommandImpl(null);
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x0012BE40 File Offset: 0x0012A040
		Tuple<string, string> IParameterMetadataProvider.GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			string item = usingVariablesTuple.Item2;
			string withInputHandlingForInvokeCommandImpl = this.GetWithInputHandlingForInvokeCommandImpl(usingVariablesTuple);
			string item2 = null;
			if (this.ParamBlock == null)
			{
				item2 = "param(" + item + ")" + Environment.NewLine;
			}
			return new Tuple<string, string>(item2, withInputHandlingForInvokeCommandImpl);
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0012BE84 File Offset: 0x0012A084
		private string GetWithInputHandlingForInvokeCommandImpl(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			string text;
			string text2;
			PipelineAst simplePipeline = this.GetSimplePipeline(false, out text, out text2);
			if (simplePipeline == null)
			{
				if (usingVariablesTuple != null)
				{
					return this.ToStringForSerialization(usingVariablesTuple, base.Extent.StartOffset, base.Extent.EndOffset);
				}
				return this.ToStringForSerialization();
			}
			else if (simplePipeline.PipelineElements[0] is CommandExpressionAst)
			{
				if (usingVariablesTuple != null)
				{
					return this.ToStringForSerialization(usingVariablesTuple, base.Extent.StartOffset, base.Extent.EndOffset);
				}
				return this.ToStringForSerialization();
			}
			else
			{
				if (!AstSearcher.IsUsingDollarInput(this))
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (this.ParamBlock != null)
					{
						string value = (usingVariablesTuple == null) ? this.ParamBlock.ToString() : this.ToStringForSerialization(usingVariablesTuple, this.ParamBlock.Extent.StartOffset, this.ParamBlock.Extent.EndOffset);
						stringBuilder.Append(value);
					}
					stringBuilder.Append("$input |");
					string value2 = (usingVariablesTuple == null) ? simplePipeline.ToString() : this.ToStringForSerialization(usingVariablesTuple, simplePipeline.Extent.StartOffset, simplePipeline.Extent.EndOffset);
					stringBuilder.Append(value2);
					return stringBuilder.ToString();
				}
				if (usingVariablesTuple != null)
				{
					return this.ToStringForSerialization(usingVariablesTuple, base.Extent.StartOffset, base.Extent.EndOffset);
				}
				return this.ToStringForSerialization();
			}
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x0012BFE4 File Offset: 0x0012A1E4
		bool IParameterMetadataProvider.UsesCmdletBinding()
		{
			bool flag = false;
			if (this.ParamBlock != null)
			{
				flag = this.ParamBlock.Attributes.Any((AttributeAst attribute) => typeof(CmdletBindingAttribute) == attribute.TypeName.GetReflectionAttributeType());
				if (!flag)
				{
					flag = ParamBlockAst.UsesCmdletBinding(this.ParamBlock.Parameters);
				}
			}
			return flag;
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0012C04C File Offset: 0x0012A24C
		internal PipelineAst GetSimplePipeline(bool allowMultiplePipelines, out string errorId, out string errorMsg)
		{
			if (this.BeginBlock != null || this.ProcessBlock != null || this.DynamicParamBlock != null)
			{
				errorId = "CanConvertOneClauseOnly";
				errorMsg = AutomationExceptions.CanConvertOneClauseOnly;
				return null;
			}
			if (this.EndBlock == null || this.EndBlock.Statements.Count < 1)
			{
				errorId = "CantConvertEmptyPipeline";
				errorMsg = AutomationExceptions.CantConvertEmptyPipeline;
				return null;
			}
			if (this.EndBlock.Traps != null && this.EndBlock.Traps.Any<TrapStatementAst>())
			{
				errorId = "CantConvertScriptBlockWithTrap";
				errorMsg = AutomationExceptions.CantConvertScriptBlockWithTrap;
				return null;
			}
			if (this.EndBlock.Statements.Any((StatementAst ast) => !(ast is PipelineAst)))
			{
				errorId = "CanOnlyConvertOnePipeline";
				errorMsg = AutomationExceptions.CanOnlyConvertOnePipeline;
				return null;
			}
			if (this.EndBlock.Statements.Count != 1 && !allowMultiplePipelines)
			{
				errorId = "CanOnlyConvertOnePipeline";
				errorMsg = AutomationExceptions.CanOnlyConvertOnePipeline;
				return null;
			}
			errorId = null;
			errorMsg = null;
			return this.EndBlock.Statements[0] as PipelineAst;
		}

		// Token: 0x04001C7A RID: 7290
		private static readonly ReadOnlyCollection<AttributeAst> EmptyAttributeList = new ReadOnlyCollection<AttributeAst>(new AttributeAst[0]);

		// Token: 0x04001C7B RID: 7291
		private static readonly ReadOnlyCollection<UsingStatementAst> EmptyUsingStatementList = new ReadOnlyCollection<UsingStatementAst>(new UsingStatementAst[0]);
	}
}
