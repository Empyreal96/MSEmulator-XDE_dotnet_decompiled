using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Management.Automation.Language
{
	// Token: 0x02000554 RID: 1364
	public class FunctionDefinitionAst : StatementAst, IParameterMetadataProvider
	{
		// Token: 0x060038C0 RID: 14528 RVA: 0x0012DDB4 File Offset: 0x0012BFB4
		public FunctionDefinitionAst(IScriptExtent extent, bool isFilter, bool isWorkflow, string name, IEnumerable<ParameterAst> parameters, ScriptBlockAst body) : base(extent)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			if (isFilter && isWorkflow)
			{
				throw PSTraceSource.NewArgumentException("isFilter");
			}
			this.IsFilter = isFilter;
			this.IsWorkflow = isWorkflow;
			this.Name = name;
			if (parameters != null && parameters.Any<ParameterAst>())
			{
				this.Parameters = new ReadOnlyCollection<ParameterAst>(parameters.ToArray<ParameterAst>());
				base.SetParents<ParameterAst>(this.Parameters);
			}
			this.Body = body;
			base.SetParent(body);
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0012DE4D File Offset: 0x0012C04D
		internal FunctionDefinitionAst(IScriptExtent extent, bool isFilter, bool isWorkflow, Token functionNameToken, IEnumerable<ParameterAst> parameters, ScriptBlockAst body) : this(extent, isFilter, isWorkflow, (functionNameToken.Kind == TokenKind.Generic) ? ((StringToken)functionNameToken).Value : functionNameToken.Text, parameters, body)
		{
			this.NameExtent = functionNameToken.Extent;
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060038C2 RID: 14530 RVA: 0x0012DE88 File Offset: 0x0012C088
		// (set) Token: 0x060038C3 RID: 14531 RVA: 0x0012DE90 File Offset: 0x0012C090
		public bool IsFilter { get; private set; }

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060038C4 RID: 14532 RVA: 0x0012DE99 File Offset: 0x0012C099
		// (set) Token: 0x060038C5 RID: 14533 RVA: 0x0012DEA1 File Offset: 0x0012C0A1
		public bool IsWorkflow { get; private set; }

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x060038C6 RID: 14534 RVA: 0x0012DEAA File Offset: 0x0012C0AA
		// (set) Token: 0x060038C7 RID: 14535 RVA: 0x0012DEB2 File Offset: 0x0012C0B2
		public string Name { get; private set; }

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x060038C8 RID: 14536 RVA: 0x0012DEBB File Offset: 0x0012C0BB
		// (set) Token: 0x060038C9 RID: 14537 RVA: 0x0012DEC3 File Offset: 0x0012C0C3
		public ReadOnlyCollection<ParameterAst> Parameters { get; private set; }

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x0012DECC File Offset: 0x0012C0CC
		// (set) Token: 0x060038CB RID: 14539 RVA: 0x0012DED4 File Offset: 0x0012C0D4
		public ScriptBlockAst Body { get; private set; }

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060038CC RID: 14540 RVA: 0x0012DEDD File Offset: 0x0012C0DD
		// (set) Token: 0x060038CD RID: 14541 RVA: 0x0012DEE5 File Offset: 0x0012C0E5
		internal IScriptExtent NameExtent { get; private set; }

		// Token: 0x060038CE RID: 14542 RVA: 0x0012DEF0 File Offset: 0x0012C0F0
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

		// Token: 0x060038CF RID: 14543 RVA: 0x0012DF24 File Offset: 0x0012C124
		public CommentHelpInfo GetHelpContent(Dictionary<Ast, Token[]> scriptBlockTokenCache)
		{
			if (scriptBlockTokenCache == null)
			{
				throw new ArgumentNullException("scriptBlockTokenCache");
			}
			Tuple<List<Token>, List<string>> helpCommentTokens = HelpCommentsParser.GetHelpCommentTokens(this, scriptBlockTokenCache);
			if (helpCommentTokens != null)
			{
				return HelpCommentsParser.GetHelpContents(helpCommentTokens.Item1, helpCommentTokens.Item2);
			}
			return null;
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0012DF60 File Offset: 0x0012C160
		public override Ast Copy()
		{
			ParameterAst[] parameters = Ast.CopyElements<ParameterAst>(this.Parameters);
			ScriptBlockAst body = Ast.CopyElement<ScriptBlockAst>(this.Body);
			return new FunctionDefinitionAst(base.Extent, this.IsFilter, this.IsWorkflow, this.Name, parameters, body)
			{
				NameExtent = this.NameExtent
			};
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0012DFB2 File Offset: 0x0012C1B2
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x0012DFC8 File Offset: 0x0012C1C8
		internal string GetParamTextFromParameterList(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple = null)
		{
			string text = null;
			IEnumerator<VariableExpressionAst> enumerator = null;
			if (usingVariablesTuple != null)
			{
				enumerator = (from varAst in usingVariablesTuple.Item1
				orderby varAst.Extent.StartOffset
				select varAst).GetEnumerator();
				text = usingVariablesTuple.Item2;
			}
			StringBuilder stringBuilder = new StringBuilder("param(");
			string value = "";
			if (text != null)
			{
				stringBuilder.Append(text);
				value = ", ";
			}
			for (int i = 0; i < this.Parameters.Count; i++)
			{
				ParameterAst parameterAst = this.Parameters[i];
				stringBuilder.Append(value);
				stringBuilder.Append((enumerator != null) ? parameterAst.GetParamTextWithDollarUsingHandling(enumerator) : parameterAst.ToString());
				value = ", ";
			}
			stringBuilder.Append(")");
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0012E0A3 File Offset: 0x0012C2A3
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitFunctionDefinition(this);
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0012E0AC File Offset: 0x0012C2AC
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitFunctionDefinition(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				if (this.Parameters != null)
				{
					for (int i = 0; i < this.Parameters.Count; i++)
					{
						ParameterAst parameterAst = this.Parameters[i];
						astVisitAction = parameterAst.InternalVisit(visitor);
						if (astVisitAction != AstVisitAction.Continue)
						{
							break;
						}
					}
				}
				if (astVisitAction == AstVisitAction.Continue)
				{
					astVisitAction = this.Body.InternalVisit(visitor);
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0012E120 File Offset: 0x0012C320
		RuntimeDefinedParameterDictionary IParameterMetadataProvider.GetParameterMetadata(bool automaticPositions, ref bool usesCmdletBinding)
		{
			if (this.Parameters != null)
			{
				return Compiler.GetParameterMetaData(this.Parameters, automaticPositions, ref usesCmdletBinding);
			}
			if (this.Body.ParamBlock != null)
			{
				return Compiler.GetParameterMetaData(this.Body.ParamBlock.Parameters, automaticPositions, ref usesCmdletBinding);
			}
			return new RuntimeDefinedParameterDictionary
			{
				Data = RuntimeDefinedParameterDictionary.EmptyParameterArray
			};
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0012E17A File Offset: 0x0012C37A
		IEnumerable<Attribute> IParameterMetadataProvider.GetScriptBlockAttributes()
		{
			return ((IParameterMetadataProvider)this.Body).GetScriptBlockAttributes();
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x060038D7 RID: 14551 RVA: 0x0012E187 File Offset: 0x0012C387
		ReadOnlyCollection<ParameterAst> IParameterMetadataProvider.Parameters
		{
			get
			{
				ReadOnlyCollection<ParameterAst> parameters;
				if ((parameters = this.Parameters) == null)
				{
					if (this.Body.ParamBlock == null)
					{
						return null;
					}
					parameters = this.Body.ParamBlock.Parameters;
				}
				return parameters;
			}
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x0012E1B2 File Offset: 0x0012C3B2
		PowerShell IParameterMetadataProvider.GetPowerShell(ExecutionContext context, Dictionary<string, object> variables, bool isTrustedInput, bool filterNonUsingVariables, bool? createLocalScope, params object[] args)
		{
			ExecutionContext.CheckStackDepth();
			return ScriptBlockToPowerShellConverter.Convert(this.Body, this.Parameters, isTrustedInput, context, variables, filterNonUsingVariables, createLocalScope, args);
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0012E1D4 File Offset: 0x0012C3D4
		string IParameterMetadataProvider.GetWithInputHandlingForInvokeCommand()
		{
			string withInputHandlingForInvokeCommand = ((IParameterMetadataProvider)this.Body).GetWithInputHandlingForInvokeCommand();
			if (this.Parameters != null)
			{
				return this.GetParamTextFromParameterList(null) + withInputHandlingForInvokeCommand;
			}
			return withInputHandlingForInvokeCommand;
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0012E204 File Offset: 0x0012C404
		Tuple<string, string> IParameterMetadataProvider.GetWithInputHandlingForInvokeCommandWithUsingExpression(Tuple<List<VariableExpressionAst>, string> usingVariablesTuple)
		{
			Tuple<string, string> withInputHandlingForInvokeCommandWithUsingExpression = ((IParameterMetadataProvider)this.Body).GetWithInputHandlingForInvokeCommandWithUsingExpression(usingVariablesTuple);
			if (this.Parameters == null)
			{
				return withInputHandlingForInvokeCommandWithUsingExpression;
			}
			string paramTextFromParameterList = this.GetParamTextFromParameterList(usingVariablesTuple);
			return new Tuple<string, string>(paramTextFromParameterList, withInputHandlingForInvokeCommandWithUsingExpression.Item2);
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x0012E23C File Offset: 0x0012C43C
		bool IParameterMetadataProvider.UsesCmdletBinding()
		{
			bool result = false;
			if (this.Parameters != null)
			{
				result = ParamBlockAst.UsesCmdletBinding(this.Parameters);
			}
			else if (this.Body.ParamBlock != null)
			{
				result = ((IParameterMetadataProvider)this.Body).UsesCmdletBinding();
			}
			return result;
		}
	}
}
