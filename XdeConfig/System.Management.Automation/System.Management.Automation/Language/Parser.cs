using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.PowerShell.DesiredStateConfiguration.Internal;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B1 RID: 1457
	public sealed class Parser
	{
		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x00142652 File Offset: 0x00140852
		// (set) Token: 0x06003DA5 RID: 15781 RVA: 0x0014265A File Offset: 0x0014085A
		internal bool ProduceV2Tokens { get; set; }

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00142663 File Offset: 0x00140863
		internal Parser()
		{
			this._tokenizer = new Tokenizer(this);
			this._errorList = new List<ParseError>();
			this._fileName = null;
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x0014268C File Offset: 0x0014088C
		public static ScriptBlockAst ParseFile(string fileName, out Token[] tokens, out ParseError[] errors)
		{
			bool flag = false;
			Parser parser = new Parser();
			if (!string.IsNullOrEmpty(fileName) && fileName.Length > ".schema.psm1".Length && fileName.EndsWith(".schema.psm1", StringComparison.OrdinalIgnoreCase))
			{
				parser._keywordModuleName = Path.GetFileName(fileName.Substring(0, fileName.Length - ".schema.psm1".Length));
				flag = true;
			}
			ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(fileName, fileName);
			List<Token> list = new List<Token>();
			ScriptBlockAst result;
			try
			{
				if (!flag)
				{
					DynamicKeyword.Push();
				}
				result = parser.Parse(fileName, externalScriptInfo.ScriptContents, list, out errors);
			}
			catch (Exception innerException)
			{
				throw new ParseException(ParserStrings.UnrecoverableParserError, innerException);
			}
			finally
			{
				if (!flag)
				{
					DynamicKeyword.Pop();
				}
			}
			tokens = list.ToArray();
			return result;
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00142758 File Offset: 0x00140958
		public static ScriptBlockAst ParseInput(string input, out Token[] tokens, out ParseError[] errors)
		{
			return Parser.ParseInput(input, null, out tokens, out errors);
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00142764 File Offset: 0x00140964
		public static ScriptBlockAst ParseInput(string input, string fileName, out Token[] tokens, out ParseError[] errors)
		{
			Parser parser = new Parser();
			List<Token> list = new List<Token>();
			ScriptBlockAst result;
			try
			{
				result = parser.Parse(fileName, input, list, out errors);
			}
			catch (Exception innerException)
			{
				throw new ParseException(ParserStrings.UnrecoverableParserError, innerException);
			}
			tokens = list.ToArray();
			return result;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x001427B0 File Offset: 0x001409B0
		internal ScriptBlockAst Parse(string fileName, string input, List<Token> tokenList, out ParseError[] errors)
		{
			ScriptBlockAst result;
			try
			{
				result = this.ParseTask(fileName, input, tokenList, false);
			}
			finally
			{
				errors = this._errorList.ToArray();
			}
			return result;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x00142814 File Offset: 0x00140A14
		private ScriptBlockAst ParseTask(string fileName, string input, List<Token> tokenList, bool recursed)
		{
			ScriptBlockAst scriptBlockAst = null;
			this._fileName = fileName;
			this._tokenizer.Initialize(fileName, input, tokenList);
			this._savingTokens = (tokenList != null);
			this._errorList.Clear();
			try
			{
				scriptBlockAst = this.ScriptBlockRule(null, false);
				scriptBlockAst.ScriptRequirements = this._tokenizer.GetScriptRequirements();
				SymbolResolver.ResolveSymbols(this, scriptBlockAst);
				SemanticChecks.CheckAst(this, scriptBlockAst);
			}
			catch (InsufficientExecutionStackException)
			{
				if (!recursed)
				{
					Task<ScriptBlockAst> task = new Task<ScriptBlockAst>(() => this.ParseTask(fileName, input, tokenList, true));
					task.Start();
					task.Wait();
					scriptBlockAst = task.Result;
				}
				else
				{
					this.ReportError(this._tokenizer.CurrentExtent(), () => ParserStrings.ScriptTooComplicated);
				}
			}
			return scriptBlockAst;
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x0014292C File Offset: 0x00140B2C
		internal static object ScanNumber(string str, Type toType)
		{
			str = str.Trim();
			if (str.Length == 0)
			{
				return 0;
			}
			Tokenizer tokenizer = new Parser()._tokenizer;
			tokenizer.Initialize(null, str, null);
			tokenizer.AllowSignedNumbers = true;
			NumberToken numberToken = tokenizer.NextToken() as NumberToken;
			if (numberToken == null || !tokenizer.IsAtEndOfScript(numberToken.Extent, false))
			{
				return LanguagePrimitives.ConvertTo(str, toType, CultureInfo.InvariantCulture);
			}
			return numberToken.Value;
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x0014299C File Offset: 0x00140B9C
		internal static ITypeName ScanType(string typename, bool ignoreErrors)
		{
			typename = typename.Trim();
			if (typename.Length == 0)
			{
				return null;
			}
			Parser parser = new Parser();
			Tokenizer tokenizer = parser._tokenizer;
			tokenizer.Initialize(null, typename, null);
			Token token;
			ITypeName typeName = parser.TypeNameRule(true, out token);
			SemanticChecks.CheckArrayTypeNameDepth(typeName, PositionUtilities.EmptyExtent, parser);
			if (!ignoreErrors && parser.ErrorList.Count > 0)
			{
				typeName = null;
			}
			return typeName;
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x001429FC File Offset: 0x00140BFC
		internal static ExpressionAst ScanString(string str)
		{
			str = str.Replace("\"", "\"\"");
			Parser parser = new Parser();
			parser._tokenizer.Initialize(null, '"' + str + '"', null);
			StringExpandableToken strToken = (StringExpandableToken)parser._tokenizer.NextToken();
			ExpressionAst result = parser.ExpandableStringRule(strToken);
			if (parser._errorList.Count > 0)
			{
				throw new ParseException(parser._errorList.ToArray());
			}
			return result;
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00142A7B File Offset: 0x00140C7B
		private static bool IgnoreTokenWhenUpdatingPreviousFirstLast(Token token)
		{
			return (token.Kind == TokenKind.Variable || token.Kind == TokenKind.Generic) && (token.Text.Equals("$^", StringComparison.OrdinalIgnoreCase) || token.Text.Equals("$$", StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00142AB8 File Offset: 0x00140CB8
		internal void SetPreviousFirstLastToken(ExecutionContext context)
		{
			Token firstToken = this._tokenizer.FirstToken;
			if (firstToken != null)
			{
				context.SetVariable(SpecialVariables.FirstTokenVarPath, this._previousFirstTokenText);
				if (!Parser.IgnoreTokenWhenUpdatingPreviousFirstLast(firstToken))
				{
					this._previousFirstTokenText = ((firstToken is StringToken) ? ((StringToken)firstToken).Value : firstToken.Text);
				}
				context.SetVariable(SpecialVariables.LastTokenVarPath, this._previousLastTokenText);
				Token lastToken = this._tokenizer.LastToken;
				if (!Parser.IgnoreTokenWhenUpdatingPreviousFirstLast(lastToken))
				{
					this._previousLastTokenText = ((lastToken is StringToken) ? ((StringToken)lastToken).Value : lastToken.Text);
				}
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06003DB1 RID: 15793 RVA: 0x00142B57 File Offset: 0x00140D57
		internal List<ParseError> ErrorList
		{
			get
			{
				return this._errorList;
			}
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x00142B5F File Offset: 0x00140D5F
		private void SkipNewlines()
		{
			if (this._ungotToken == null || this._ungotToken.Kind == TokenKind.NewLine)
			{
				this._ungotToken = null;
				this._tokenizer.SkipNewlines(false, false);
			}
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x00142B8B File Offset: 0x00140D8B
		private void V3SkipNewlines()
		{
			if (this._ungotToken == null || this._ungotToken.Kind == TokenKind.NewLine)
			{
				this._ungotToken = null;
				this._tokenizer.SkipNewlines(false, true);
			}
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x00142BB7 File Offset: 0x00140DB7
		private void SkipNewlinesAndSemicolons()
		{
			if (this._ungotToken == null || this._ungotToken.Kind == TokenKind.NewLine || this._ungotToken.Kind == TokenKind.Semi)
			{
				this._ungotToken = null;
				this._tokenizer.SkipNewlines(true, false);
			}
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00142BF4 File Offset: 0x00140DF4
		private void SyncOnError(bool consumeClosingToken, params TokenKind[] syncTokens)
		{
			int num = syncTokens.Contains(TokenKind.RParen) ? 1 : 0;
			int num2 = syncTokens.Contains(TokenKind.RCurly) ? 1 : 0;
			int num3 = syncTokens.Contains(TokenKind.RBracket) ? 1 : 0;
			Token token;
			for (;;)
			{
				token = this.NextToken();
				switch (token.Kind)
				{
				case TokenKind.EndOfInput:
					goto IL_DA;
				case TokenKind.LParen:
					num++;
					break;
				case TokenKind.RParen:
					num--;
					if (num == 0 && syncTokens.Contains(TokenKind.RParen))
					{
						goto Block_6;
					}
					break;
				case TokenKind.LCurly:
					num2++;
					break;
				case TokenKind.RCurly:
					num2--;
					if (num2 == 0 && syncTokens.Contains(TokenKind.RCurly))
					{
						goto Block_9;
					}
					break;
				case TokenKind.LBracket:
					num3++;
					break;
				case TokenKind.RBracket:
					num3--;
					if (num3 == 0 && syncTokens.Contains(TokenKind.RBracket))
					{
						goto Block_12;
					}
					break;
				}
				if (syncTokens.Contains(token.Kind) && num == 0 && num2 == 0 && num3 == 0)
				{
					return;
				}
			}
			Block_6:
			if (!consumeClosingToken)
			{
				this.UngetToken(token);
			}
			return;
			Block_9:
			if (!consumeClosingToken)
			{
				this.UngetToken(token);
			}
			return;
			Block_12:
			if (!consumeClosingToken)
			{
				this.UngetToken(token);
			}
			return;
			IL_DA:
			this.UngetToken(token);
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00142D08 File Offset: 0x00140F08
		private Token NextToken()
		{
			Token result = this._ungotToken ?? this._tokenizer.NextToken();
			this._ungotToken = null;
			return result;
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00142D34 File Offset: 0x00140F34
		private Token PeekToken()
		{
			Token token = this._ungotToken ?? this._tokenizer.NextToken();
			if (this._ungotToken == null)
			{
				this._ungotToken = token;
			}
			return token;
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00142D67 File Offset: 0x00140F67
		private Token NextMemberAccessToken(bool allowLBracket)
		{
			if (this._ungotToken != null)
			{
				return null;
			}
			return this._tokenizer.GetMemberAccessOperator(allowLBracket);
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00142D7F File Offset: 0x00140F7F
		private Token NextInvokeMemberToken()
		{
			if (this._ungotToken != null)
			{
				return null;
			}
			return this._tokenizer.GetInvokeMemberOpenParen();
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x00142D96 File Offset: 0x00140F96
		private Token NextLBracket()
		{
			if (this._ungotToken == null)
			{
				return this._tokenizer.GetLBracket();
			}
			if (this._ungotToken.Kind == TokenKind.LBracket)
			{
				return this.NextToken();
			}
			return null;
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00142DC3 File Offset: 0x00140FC3
		private StringToken GetVerbatimCommandArgumentToken()
		{
			if (this._ungotToken == null || this._ungotToken.Kind == TokenKind.Parameter)
			{
				this._ungotToken = null;
				return this._tokenizer.GetVerbatimCommandArgument();
			}
			return null;
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00142DEF File Offset: 0x00140FEF
		private void SkipToken()
		{
			this._ungotToken = null;
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x00142DF8 File Offset: 0x00140FF8
		private void UngetToken(Token token)
		{
			this._ungotToken = token;
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00142E04 File Offset: 0x00141004
		private void SetTokenizerMode(TokenizerMode mode)
		{
			if (mode != this._tokenizer.Mode && this._ungotToken != null && !this._ungotToken.Kind.HasTrait(TokenFlags.ParseModeInvariant))
			{
				this.Resync(this._ungotToken);
			}
			this._tokenizer.Mode = mode;
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00142E56 File Offset: 0x00141056
		private void Resync(Token token)
		{
			this._ungotToken = null;
			this._tokenizer.Resync(token);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00142E6B File Offset: 0x0014106B
		private void Resync(int restorePoint)
		{
			this._ungotToken = null;
			this._tokenizer.Resync(restorePoint);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00142E80 File Offset: 0x00141080
		private static bool IsSpecificParameter(Token token, string parameter)
		{
			ParameterToken parameterToken = (ParameterToken)token;
			return parameter.StartsWith(parameterToken.ParameterName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00142EA1 File Offset: 0x001410A1
		internal void NoteV3FeatureUsed()
		{
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00142EA4 File Offset: 0x001410A4
		internal void RequireStatementTerminator()
		{
			Token token = this.PeekToken();
			if (token.Kind == TokenKind.NewLine || token.Kind == TokenKind.Semi)
			{
				this.SkipToken();
				return;
			}
			if (token.Kind != TokenKind.EndOfInput)
			{
				this.ReportIncompleteInput(token.Extent, () => ParserStrings.MissingStatementTerminator);
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x00142F0C File Offset: 0x0014110C
		internal static IScriptExtent ExtentOf(IScriptExtent first, IScriptExtent last)
		{
			if (first is EmptyScriptExtent)
			{
				return last;
			}
			if (last is EmptyScriptExtent)
			{
				return first;
			}
			InternalScriptExtent internalScriptExtent = (InternalScriptExtent)first;
			InternalScriptExtent internalScriptExtent2 = (InternalScriptExtent)last;
			return new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.StartOffset, internalScriptExtent2.EndOffset);
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00142F54 File Offset: 0x00141154
		internal static IScriptExtent Before(IScriptExtent extent)
		{
			InternalScriptExtent internalScriptExtent = (InternalScriptExtent)extent;
			int num = internalScriptExtent.StartOffset - 1;
			if (num < 0)
			{
				num = 0;
			}
			return new InternalScriptExtent(internalScriptExtent.PositionHelper, num, num);
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x00142F84 File Offset: 0x00141184
		internal static IScriptExtent After(IScriptExtent extent)
		{
			InternalScriptExtent internalScriptExtent = (InternalScriptExtent)extent;
			int endOffset = internalScriptExtent.EndOffset;
			return new InternalScriptExtent(internalScriptExtent.PositionHelper, endOffset, endOffset);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00142FAC File Offset: 0x001411AC
		internal static IScriptExtent LastCharacterOf(IScriptExtent extent)
		{
			InternalScriptExtent internalScriptExtent = (InternalScriptExtent)extent;
			int num = internalScriptExtent.EndOffset - 1;
			if (num < 0)
			{
				num = 0;
			}
			return new InternalScriptExtent(internalScriptExtent.PositionHelper, num, num);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x00142FDC File Offset: 0x001411DC
		internal static IScriptExtent ExtentFromFirstOf(params object[] objs)
		{
			foreach (object obj in objs)
			{
				if (obj != null)
				{
					Token token = obj as Token;
					IScriptExtent result;
					if (token != null)
					{
						result = token.Extent;
					}
					else
					{
						Ast ast = obj as Ast;
						if (ast != null)
						{
							result = ast.Extent;
						}
						else
						{
							ITypeName typeName = obj as ITypeName;
							if (typeName != null)
							{
								result = typeName.Extent;
							}
							else
							{
								result = (IScriptExtent)obj;
							}
						}
					}
					return result;
				}
			}
			return PositionUtilities.EmptyExtent;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00143056 File Offset: 0x00141256
		internal static IScriptExtent ExtentOf(Token first, Token last)
		{
			return Parser.ExtentOf(first.Extent, last.Extent);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00143069 File Offset: 0x00141269
		internal static IScriptExtent ExtentOf(Ast first, Ast last)
		{
			return Parser.ExtentOf(first.Extent, last.Extent);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0014307C File Offset: 0x0014127C
		internal static IScriptExtent ExtentOf(Ast first, Token last)
		{
			return Parser.ExtentOf(first.Extent, last.Extent);
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0014308F File Offset: 0x0014128F
		internal static IScriptExtent ExtentOf(Token first, Ast last)
		{
			return Parser.ExtentOf(first.Extent, last.Extent);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x001430A2 File Offset: 0x001412A2
		internal static IScriptExtent ExtentOf(IScriptExtent first, Ast last)
		{
			return Parser.ExtentOf(first, last.Extent);
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x001430B0 File Offset: 0x001412B0
		internal static IScriptExtent ExtentOf(IScriptExtent first, Token last)
		{
			return Parser.ExtentOf(first, last.Extent);
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x001430BE File Offset: 0x001412BE
		internal static IScriptExtent ExtentOf(Ast first, IScriptExtent last)
		{
			return Parser.ExtentOf(first.Extent, last);
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x001430CC File Offset: 0x001412CC
		internal static IScriptExtent ExtentOf(Token first, IScriptExtent last)
		{
			return Parser.ExtentOf(first.Extent, last);
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x001430DA File Offset: 0x001412DA
		internal static IScriptExtent Before(Token token)
		{
			return Parser.Before(token.Extent);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x001430E7 File Offset: 0x001412E7
		internal static IScriptExtent After(Ast ast)
		{
			return Parser.After(ast.Extent);
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x001430F4 File Offset: 0x001412F4
		internal static IScriptExtent After(Token token)
		{
			return Parser.After(token.Extent);
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x00143390 File Offset: 0x00141590
		private static IEnumerable<Ast> GetNestedErrorAsts(params object[] asts)
		{
			foreach (object obj in asts)
			{
				if (obj != null)
				{
					Ast ast = obj as Ast;
					if (ast != null)
					{
						yield return ast;
					}
					else
					{
						IEnumerable<Ast> enumerable = obj as IEnumerable<Ast>;
						if (enumerable != null)
						{
							foreach (Ast ast2 in enumerable)
							{
								if (ast2 != null)
								{
									yield return ast2;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x001433B0 File Offset: 0x001415B0
		internal static bool TryParseAsConstantHashtable(string input, out Hashtable result)
		{
			result = null;
			if (string.IsNullOrWhiteSpace(input))
			{
				return false;
			}
			Token[] array;
			ParseError[] array2;
			ScriptBlockAst scriptBlockAst = Parser.ParseInput(input, out array, out array2);
			if (scriptBlockAst == null || array2.Length > 0 || scriptBlockAst.BeginBlock != null || scriptBlockAst.ProcessBlock != null || scriptBlockAst.DynamicParamBlock != null || scriptBlockAst.EndBlock.Traps != null)
			{
				return false;
			}
			ReadOnlyCollection<StatementAst> statements = scriptBlockAst.EndBlock.Statements;
			if (statements.Count != 1)
			{
				return false;
			}
			PipelineAst pipelineAst = statements[0] as PipelineAst;
			if (pipelineAst == null)
			{
				return false;
			}
			ExpressionAst pureExpression = pipelineAst.GetPureExpression();
			if (pureExpression == null)
			{
				return false;
			}
			HashtableAst hashtableAst = pureExpression as HashtableAst;
			if (hashtableAst == null)
			{
				return false;
			}
			object obj;
			if (!IsConstantValueVisitor.IsConstant(hashtableAst, out obj, false, true))
			{
				return false;
			}
			Hashtable hashtable = obj as Hashtable;
			result = hashtable;
			return true;
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0014346B File Offset: 0x0014166B
		private ScriptBlockAst ScriptBlockRule(Token lCurly, bool isFilter)
		{
			return this.ScriptBlockRule(lCurly, isFilter, null);
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x00143478 File Offset: 0x00141678
		private ScriptBlockAst ScriptBlockRule(Token lCurly, bool isFilter, StatementAst predefinedStatementAst)
		{
			this.SkipNewlines();
			List<UsingStatementAst> usingStatements = (lCurly == null) ? this.UsingStatementsRule() : null;
			int restorePoint = this._tokenizer.GetRestorePoint();
			ParamBlockAst paramBlockAst = this.ParamBlockRule();
			if (paramBlockAst == null)
			{
				this.Resync(restorePoint);
			}
			this.SkipNewlinesAndSemicolons();
			return this.ScriptBlockBodyRule(lCurly, usingStatements, paramBlockAst, isFilter, predefinedStatementAst);
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x001434C8 File Offset: 0x001416C8
		private List<UsingStatementAst> UsingStatementsRule()
		{
			List<UsingStatementAst> list = null;
			Token token;
			for (;;)
			{
				token = this.PeekToken();
				if (token.Kind != TokenKind.Using)
				{
					break;
				}
				this.SkipToken();
				StatementAst statementAst = this.UsingStatementRule(token);
				this.SkipNewlinesAndSemicolons();
				if (list == null)
				{
					list = new List<UsingStatementAst>();
				}
				UsingStatementAst usingStatementAst = statementAst as UsingStatementAst;
				if (usingStatementAst != null)
				{
					list.Add(usingStatementAst);
				}
			}
			this.Resync(token);
			return list;
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x00143524 File Offset: 0x00141724
		private ParamBlockAst ParamBlockRule()
		{
			this.SkipNewlines();
			List<AttributeBaseAst> list = this.AttributeListRule(false);
			this.SkipNewlines();
			Token token = this.PeekToken();
			if (token.Kind != TokenKind.Param)
			{
				return null;
			}
			this.SkipToken();
			this.SkipNewlines();
			Token token2 = this.NextToken();
			if (token2.Kind != TokenKind.LParen)
			{
				this.UngetToken(token2);
				return null;
			}
			List<ParameterAst> list2 = this.ParameterListRule();
			this.SkipNewlines();
			Token token3 = this.NextToken();
			IScriptExtent last = token3.Extent;
			if (token3.Kind != TokenKind.RParen)
			{
				this.UngetToken(token3);
				last = Parser.Before(token3);
				this.ReportIncompleteInput(Parser.After((list2 != null && list2.Any<ParameterAst>()) ? list2.Last<ParameterAst>().Extent : token2.Extent), () => ParserStrings.MissingEndParenthesisInFunctionParameterList);
			}
			List<AttributeAst> list3 = new List<AttributeAst>();
			if (list != null)
			{
				foreach (AttributeBaseAst attributeBaseAst in list)
				{
					AttributeAst attributeAst = attributeBaseAst as AttributeAst;
					if (attributeAst != null)
					{
						list3.Add(attributeAst);
					}
					else
					{
						this.ReportError(attributeBaseAst.Extent, () => ParserStrings.TypeNotAllowedBeforeParam, attributeBaseAst.TypeName.FullName);
					}
				}
			}
			return new ParamBlockAst(Parser.ExtentOf(token, last), list3, list2);
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x001436AC File Offset: 0x001418AC
		private List<ParameterAst> ParameterListRule()
		{
			List<ParameterAst> list = new List<ParameterAst>();
			Token token = null;
			for (;;)
			{
				ParameterAst parameterAst = this.ParameterRule();
				if (parameterAst == null)
				{
					break;
				}
				list.Add(parameterAst);
				this.SkipNewlines();
				token = this.PeekToken();
				if (token.Kind != TokenKind.Comma)
				{
					return list;
				}
				this.SkipToken();
			}
			if (token != null)
			{
				this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingExpressionAfterToken, token.Kind.Text());
			}
			return list;
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x00143730 File Offset: 0x00141930
		private ParameterAst ParameterRule()
		{
			ExpressionAst expressionAst = null;
			bool disableCommaOperator = this._disableCommaOperator;
			TokenizerMode mode = this._tokenizer.Mode;
			List<AttributeBaseAst> list;
			VariableToken variableToken;
			try
			{
				this._disableCommaOperator = true;
				this.SetTokenizerMode(TokenizerMode.Expression);
				this.SkipNewlines();
				list = this.AttributeListRule(false);
				this.SkipNewlines();
				Token token = this.NextToken();
				if (token.Kind != TokenKind.Variable && token.Kind != TokenKind.SplattedVariable)
				{
					this.UngetToken(token);
					if (list != null)
					{
						this.ReportIncompleteInput(Parser.After(list.Last<AttributeBaseAst>()), () => ParserStrings.InvalidFunctionParameter);
						this.SyncOnError(true, new TokenKind[]
						{
							TokenKind.RParen
						});
						IScriptExtent extent = Parser.ExtentOf(list[0].Extent, list[list.Count - 1].Extent);
						return new ParameterAst(extent, new VariableExpressionAst(extent, "__error__", false), list, null);
					}
					return null;
				}
				else
				{
					variableToken = (VariableToken)token;
					this.SkipNewlines();
					Token token2 = this.PeekToken();
					if (token2.Kind == TokenKind.Equals)
					{
						this.SkipToken();
						this.SkipNewlines();
						expressionAst = this.ExpressionRule();
						if (expressionAst == null)
						{
							this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.MissingExpressionAfterToken, token2.Kind.Text());
						}
					}
				}
			}
			finally
			{
				this._disableCommaOperator = disableCommaOperator;
				this.SetTokenizerMode(mode);
			}
			IScriptExtent first = (list == null) ? variableToken.Extent : list[0].Extent;
			IScriptExtent last = (expressionAst == null) ? variableToken.Extent : expressionAst.Extent;
			return new ParameterAst(Parser.ExtentOf(first, last), new VariableExpressionAst(variableToken), list, expressionAst);
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00143920 File Offset: 0x00141B20
		private List<AttributeBaseAst> AttributeListRule(bool inExpressionMode)
		{
			List<AttributeBaseAst> list = new List<AttributeBaseAst>();
			for (AttributeBaseAst attributeBaseAst = this.AttributeRule(); attributeBaseAst != null; attributeBaseAst = this.AttributeRule())
			{
				list.Add(attributeBaseAst);
				if (!inExpressionMode || attributeBaseAst is AttributeAst)
				{
					this.SkipNewlines();
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0014396C File Offset: 0x00141B6C
		private AttributeBaseAst AttributeRule()
		{
			Token token = this.NextLBracket();
			if (token == null)
			{
				return null;
			}
			this.V3SkipNewlines();
			Token token2;
			ITypeName typeName = this.TypeNameRule(true, out token2);
			if (typeName == null)
			{
				this.Resync(token);
				this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingTypename);
				return null;
			}
			Token token3 = this.NextToken();
			if (token3.Kind == TokenKind.LParen)
			{
				this.SkipNewlines();
				List<ExpressionAst> positionalArguments = new List<ExpressionAst>();
				List<NamedAttributeArgumentAst> namedArguments = new List<NamedAttributeArgumentAst>();
				IScriptExtent extent = token3.Extent;
				TokenizerMode mode = this._tokenizer.Mode;
				try
				{
					this.SetTokenizerMode(TokenizerMode.Expression);
					this.AttributeArgumentsRule(positionalArguments, namedArguments, ref extent);
				}
				finally
				{
					this.SetTokenizerMode(mode);
				}
				this.SkipNewlines();
				Token token4 = this.NextToken();
				if (token4.Kind != TokenKind.RParen)
				{
					this.UngetToken(token4);
					token4 = null;
					this.ReportIncompleteInput(Parser.After(extent), () => ParserStrings.MissingEndParenthesisInExpression);
				}
				this.SkipNewlines();
				Token token5 = this.NextToken();
				if (token5.Kind != TokenKind.RBracket)
				{
					this.UngetToken(token5);
					token5 = null;
					if (token4 != null)
					{
						this.ReportIncompleteInput(Parser.After(token4), () => ParserStrings.EndSquareBracketExpectedAtEndOfAttribute);
					}
				}
				token2.TokenFlags |= TokenFlags.AttributeName;
				return new AttributeAst(Parser.ExtentOf(token, Parser.ExtentFromFirstOf(new object[]
				{
					token5,
					token4,
					extent
				})), typeName, positionalArguments, namedArguments);
			}
			if (this.ProduceV2Tokens)
			{
				Token newToken = new Token((InternalScriptExtent)Parser.ExtentOf(token, token3), TokenKind.Identifier, TokenFlags.TypeName);
				this._tokenizer.ReplaceSavedTokens(token, token3, newToken);
			}
			if (token3.Kind != TokenKind.RBracket)
			{
				this.UngetToken(token3);
				this.ReportError(Parser.Before(token3), () => ParserStrings.EndSquareBracketExpectedAtEndOfAttribute);
				token3 = null;
			}
			return new TypeConstraintAst(Parser.ExtentOf(token, Parser.ExtentFromFirstOf(new object[]
			{
				token3,
				typeName.Extent
			})), typeName);
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x00143BC0 File Offset: 0x00141DC0
		private void AttributeArgumentsRule(ICollection<ExpressionAst> positionalArguments, ICollection<NamedAttributeArgumentAst> namedArguments, ref IScriptExtent lastItemExtent)
		{
			bool disableCommaOperator = this._disableCommaOperator;
			Token token = null;
			HashSet<string> hashSet = new HashSet<string>();
			try
			{
				this._disableCommaOperator = true;
				for (;;)
				{
					this.SkipNewlines();
					StringConstantExpressionAst stringConstantExpressionAst = this.SimpleNameRule();
					bool expressionOmitted = false;
					ExpressionAst expressionAst;
					if (stringConstantExpressionAst != null)
					{
						Token token2 = this.PeekToken();
						if (token2.Kind == TokenKind.Equals)
						{
							token2 = this.NextToken();
							this.SkipNewlines();
							expressionAst = this.ExpressionRule();
							if (expressionAst == null)
							{
								IScriptExtent extent = Parser.After(token2);
								this.ReportIncompleteInput(extent, () => ParserStrings.MissingExpressionInNamedArgument);
								expressionAst = new ErrorExpressionAst(extent, null);
								this.SyncOnError(true, new TokenKind[]
								{
									TokenKind.Comma,
									TokenKind.RParen,
									TokenKind.RBracket,
									TokenKind.NewLine
								});
							}
							lastItemExtent = expressionAst.Extent;
						}
						else
						{
							expressionAst = new ConstantExpressionAst(stringConstantExpressionAst.Extent, true);
							expressionOmitted = true;
							this.NoteV3FeatureUsed();
						}
					}
					else
					{
						expressionAst = this.ExpressionRule();
					}
					if (stringConstantExpressionAst != null)
					{
						if (hashSet.Contains(stringConstantExpressionAst.Value))
						{
							this.ReportError(stringConstantExpressionAst.Extent, () => ParserStrings.DuplicateNamedArgument, stringConstantExpressionAst.Value);
						}
						else
						{
							namedArguments.Add(new NamedAttributeArgumentAst(Parser.ExtentOf(stringConstantExpressionAst, expressionAst), stringConstantExpressionAst.Value, expressionAst, expressionOmitted));
						}
					}
					else if (expressionAst != null)
					{
						positionalArguments.Add(expressionAst);
						lastItemExtent = expressionAst.Extent;
					}
					else if (token != null)
					{
						IScriptExtent scriptExtent = Parser.After(token);
						this.ReportIncompleteInput(scriptExtent, () => ParserStrings.MissingExpressionAfterToken, token.Kind.Text());
						positionalArguments.Add(new ErrorExpressionAst(scriptExtent, null));
						lastItemExtent = scriptExtent;
					}
					this.SkipNewlines();
					token = this.PeekToken();
					if (token.Kind != TokenKind.Comma)
					{
						break;
					}
					lastItemExtent = token.Extent;
					this.SkipToken();
				}
			}
			finally
			{
				this._disableCommaOperator = disableCommaOperator;
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00143DE0 File Offset: 0x00141FE0
		private ITypeName TypeNameRule(bool allowAssemblyQualifiedNames, out Token firstTypeNameToken)
		{
			TokenizerMode mode = this._tokenizer.Mode;
			ITypeName result;
			try
			{
				this.SetTokenizerMode(TokenizerMode.TypeName);
				Token token = this.NextToken();
				if (token.Kind != TokenKind.Identifier)
				{
					this.UngetToken(token);
					firstTypeNameToken = null;
					result = null;
				}
				else
				{
					firstTypeNameToken = token;
					result = this.FinishTypeNameRule(token, false, allowAssemblyQualifiedNames);
				}
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return result;
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00143E48 File Offset: 0x00142048
		private ITypeName FinishTypeNameRule(Token typeName, bool unBracketedGenericArg = false, bool allowAssemblyQualifiedNames = true)
		{
			Token token = this.PeekToken();
			if (token.Kind == TokenKind.LBracket)
			{
				Token token2 = token;
				this.SkipToken();
				this.V3SkipNewlines();
				token = this.NextToken();
				TokenKind kind = token.Kind;
				if (kind != TokenKind.Identifier)
				{
					switch (kind)
					{
					case TokenKind.LBracket:
						goto IL_6A;
					case TokenKind.RBracket:
						break;
					default:
						if (kind != TokenKind.Comma)
						{
							if (token.Kind != TokenKind.EndOfInput)
							{
								this.ReportError(token.Extent, () => ParserStrings.UnexpectedToken, token.Text);
								this.SyncOnError(true, new TokenKind[]
								{
									TokenKind.RBracket
								});
							}
							else
							{
								this.UngetToken(token);
								this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.MissingTypename);
							}
							return new TypeName(typeName.Extent, typeName.Text);
						}
						break;
					}
					TypeName typeName2 = new TypeName(typeName.Extent, typeName.Text);
					return this.CompleteArrayTypeName(typeName2, typeName2, token);
				}
				IL_6A:
				return this.GenericTypeArgumentsRule(typeName, token, unBracketedGenericArg);
			}
			if (token.Kind != TokenKind.Comma || !allowAssemblyQualifiedNames || unBracketedGenericArg)
			{
				return new TypeName(typeName.Extent, typeName.Text);
			}
			this.SkipToken();
			string assemblyNameSpec = this._tokenizer.GetAssemblyNameSpec();
			if (string.IsNullOrWhiteSpace(assemblyNameSpec))
			{
				this.ReportError(Parser.After(token), () => ParserStrings.MissingAssemblyNameSpecification);
				return new TypeName(typeName.Extent, typeName.Text);
			}
			return new TypeName(Parser.ExtentOf(typeName.Extent, this._tokenizer.CurrentExtent()), typeName.Text, assemblyNameSpec);
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00144004 File Offset: 0x00142204
		private ITypeName GetSingleGenericArgument(Token firstToken)
		{
			if (firstToken.Kind == TokenKind.Identifier)
			{
				return this.FinishTypeNameRule(firstToken, true, true);
			}
			Token token = this.NextToken();
			if (token.Kind != TokenKind.Identifier)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingTypename);
				return new TypeName(firstToken.Extent, ":ErrorTypeName:");
			}
			ITypeName typeName = this.FinishTypeNameRule(token, false, true);
			if (typeName != null)
			{
				Token token2 = this.NextToken();
				if (token2.Kind != TokenKind.RBracket)
				{
					this.UngetToken(token2);
					this.ReportIncompleteInput(Parser.Before(token2), () => ParserStrings.EndSquareBracketExpectedAtEndOfType);
				}
			}
			return typeName;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x001440CC File Offset: 0x001422CC
		private ITypeName GenericTypeArgumentsRule(Token genericTypeName, Token firstToken, bool unBracketedGenericArg)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			List<ITypeName> list = new List<ITypeName>();
			ITypeName item = this.GetSingleGenericArgument(firstToken);
			list.Add(item);
			Token token;
			Token token2;
			for (;;)
			{
				this.V3SkipNewlines();
				token = this.NextToken();
				if (token.Kind != TokenKind.Comma)
				{
					break;
				}
				this.V3SkipNewlines();
				token2 = this.PeekToken();
				if (token2.Kind == TokenKind.Identifier || token2.Kind == TokenKind.LBracket)
				{
					this.SkipToken();
					item = this.GetSingleGenericArgument(token2);
				}
				else
				{
					this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingTypename);
					item = new TypeName(token.Extent, ":ErrorTypeName:");
				}
				list.Add(item);
			}
			if (token.Kind != TokenKind.RBracket)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.Before(token), () => ParserStrings.EndSquareBracketExpectedAtEndOfAttribute);
				token = null;
			}
			TypeName typeName = new TypeName(genericTypeName.Extent, genericTypeName.Text);
			GenericTypeName genericTypeName2 = new GenericTypeName(Parser.ExtentOf(genericTypeName.Extent, Parser.ExtentFromFirstOf(new object[]
			{
				token,
				list.LastOrDefault<ITypeName>(),
				firstToken
			})), typeName, list);
			token2 = this.PeekToken();
			if (token2.Kind == TokenKind.LBracket)
			{
				this.SkipToken();
				return this.CompleteArrayTypeName(genericTypeName2, typeName, this.NextToken());
			}
			if (token2.Kind == TokenKind.Comma && !unBracketedGenericArg)
			{
				this.SkipToken();
				string assemblyNameSpec = this._tokenizer.GetAssemblyNameSpec();
				if (string.IsNullOrEmpty(assemblyNameSpec))
				{
					this.ReportError(Parser.After(token2), () => ParserStrings.MissingAssemblyNameSpecification);
				}
				else
				{
					typeName.AssemblyName = assemblyNameSpec;
				}
			}
			return genericTypeName2;
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0014429C File Offset: 0x0014249C
		private ITypeName CompleteArrayTypeName(ITypeName elementType, TypeName typeForAssemblyQualification, Token firstTokenAfterLBracket)
		{
			Token token;
			for (;;)
			{
				TokenKind kind = firstTokenAfterLBracket.Kind;
				if (kind != TokenKind.EndOfInput)
				{
					if (kind != TokenKind.RBracket)
					{
						if (kind == TokenKind.Comma)
						{
							int num = 1;
							token = firstTokenAfterLBracket;
							Token token2;
							do
							{
								token2 = token;
								num++;
								token = this.NextToken();
							}
							while (token.Kind == TokenKind.Comma);
							if (token.Kind != TokenKind.RBracket)
							{
								this.UngetToken(token);
								this.ReportError(Parser.After(token2), () => ParserStrings.EndSquareBracketExpectedAtEndOfAttribute);
							}
							elementType = new ArrayTypeName(Parser.ExtentOf(elementType.Extent, token.Extent), elementType, num);
						}
						else
						{
							this.ReportError(firstTokenAfterLBracket.Extent, () => ParserStrings.UnexpectedToken, firstTokenAfterLBracket.Text);
							this.SyncOnError(true, new TokenKind[]
							{
								TokenKind.RBracket
							});
						}
					}
					else
					{
						elementType = new ArrayTypeName(Parser.ExtentOf(elementType.Extent, firstTokenAfterLBracket.Extent), elementType, 1);
					}
				}
				else
				{
					this.UngetToken(firstTokenAfterLBracket);
					this.ReportError(Parser.Before(firstTokenAfterLBracket), () => ParserStrings.EndSquareBracketExpectedAtEndOfAttribute);
				}
				token = this.PeekToken();
				if (token.Kind == TokenKind.Comma)
				{
					break;
				}
				if (token.Kind != TokenKind.LBracket)
				{
					return elementType;
				}
				this.SkipToken();
				firstTokenAfterLBracket = this.NextToken();
			}
			this.SkipToken();
			string assemblyNameSpec = this._tokenizer.GetAssemblyNameSpec();
			if (string.IsNullOrEmpty(assemblyNameSpec))
			{
				this.ReportError(Parser.After(token), () => ParserStrings.MissingAssemblyNameSpecification);
			}
			else
			{
				typeForAssemblyQualification.AssemblyName = assemblyNameSpec;
			}
			return elementType;
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x00144460 File Offset: 0x00142660
		private bool CompleteScriptBlockBody(Token lCurly, ref IScriptExtent bodyExtent, out IScriptExtent fullBodyExtent)
		{
			if (lCurly != null)
			{
				Token token = this.NextToken();
				IScriptExtent last;
				if (token.Kind != TokenKind.RCurly)
				{
					this.UngetToken(token);
					last = (bodyExtent ?? lCurly.Extent);
					this.ReportIncompleteInput(lCurly.Extent, token.Extent, () => ParserStrings.MissingEndCurlyBrace, new object[0]);
				}
				else
				{
					last = token.Extent;
					if (bodyExtent == null && lCurly.Extent.EndColumnNumber != token.Extent.StartColumnNumber)
					{
						bodyExtent = Parser.ExtentOf(Parser.After(lCurly), Parser.Before(token));
					}
				}
				fullBodyExtent = Parser.ExtentOf(lCurly, last);
			}
			else
			{
				fullBodyExtent = this._tokenizer.GetScriptExtent();
				Token token2 = this.NextToken();
				if (token2.Kind != TokenKind.EndOfInput)
				{
					this.ReportError(token2.Extent, () => ParserStrings.UnexpectedToken, token2.Text);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00144568 File Offset: 0x00142768
		private ScriptBlockAst ScriptBlockBodyRule(Token lCurly, List<UsingStatementAst> usingStatements, ParamBlockAst paramBlockAst, bool isFilter, StatementAst predefinedStatementAst)
		{
			Token token = this.PeekToken();
			if ((token.TokenFlags & TokenFlags.ScriptBlockBlockName) == TokenFlags.ScriptBlockBlockName)
			{
				return this.NamedBlockListRule(lCurly, usingStatements, paramBlockAst);
			}
			List<TrapStatementAst> traps = new List<TrapStatementAst>();
			List<StatementAst> list = new List<StatementAst>();
			if (predefinedStatementAst != null)
			{
				list.Add(predefinedStatementAst);
			}
			IScriptExtent scriptExtent = (paramBlockAst != null) ? paramBlockAst.Extent : null;
			IScriptExtent extent;
			do
			{
				IScriptExtent scriptExtent2 = this.StatementListRule(list, traps);
				if (scriptExtent == null)
				{
					scriptExtent = scriptExtent2;
				}
				else if (scriptExtent2 != null)
				{
					scriptExtent = Parser.ExtentOf(scriptExtent, scriptExtent2);
				}
			}
			while (!this.CompleteScriptBlockBody(lCurly, ref scriptExtent, out extent));
			return new ScriptBlockAst(extent, usingStatements, paramBlockAst, new StatementBlockAst(scriptExtent ?? PositionUtilities.EmptyExtent, list, traps), isFilter);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00144600 File Offset: 0x00142800
		private ScriptBlockAst NamedBlockListRule(Token lCurly, List<UsingStatementAst> usingStatements, ParamBlockAst paramBlockAst)
		{
			NamedBlockAst namedBlockAst = null;
			NamedBlockAst namedBlockAst2 = null;
			NamedBlockAst namedBlockAst3 = null;
			NamedBlockAst namedBlockAst4 = null;
			IScriptExtent scriptExtent = (lCurly != null) ? lCurly.Extent : ((paramBlockAst != null) ? paramBlockAst.Extent : null);
			IScriptExtent last = null;
			Token token;
			IScriptExtent extent;
			for (;;)
			{
				token = this.NextToken();
				TokenKind kind = token.Kind;
				if (kind <= TokenKind.Dynamicparam)
				{
					if (kind != TokenKind.Begin && kind != TokenKind.Dynamicparam)
					{
						break;
					}
				}
				else if (kind != TokenKind.End && kind != TokenKind.Process)
				{
					break;
				}
				if (scriptExtent == null)
				{
					scriptExtent = token.Extent;
				}
				last = token.Extent;
				StatementBlockAst statementBlockAst = this.StatementBlockRule();
				if (statementBlockAst == null)
				{
					this.ReportIncompleteInput(Parser.After(token.Extent), () => ParserStrings.MissingNamedStatementBlock, token.Kind.Text());
					statementBlockAst = new StatementBlockAst(token.Extent, new StatementAst[0], null);
				}
				else
				{
					last = statementBlockAst.Extent;
				}
				extent = Parser.ExtentOf(token, last);
				if (token.Kind == TokenKind.Begin && namedBlockAst2 == null)
				{
					namedBlockAst2 = new NamedBlockAst(extent, TokenKind.Begin, statementBlockAst, false);
				}
				else if (token.Kind == TokenKind.Process && namedBlockAst3 == null)
				{
					namedBlockAst3 = new NamedBlockAst(extent, TokenKind.Process, statementBlockAst, false);
				}
				else if (token.Kind == TokenKind.End && namedBlockAst4 == null)
				{
					namedBlockAst4 = new NamedBlockAst(extent, TokenKind.End, statementBlockAst, false);
				}
				else if (token.Kind == TokenKind.Dynamicparam && namedBlockAst == null)
				{
					namedBlockAst = new NamedBlockAst(extent, TokenKind.Dynamicparam, statementBlockAst, false);
				}
				else
				{
					this.ReportError(extent, () => ParserStrings.DuplicateScriptCommandClause, token.Kind.Text());
				}
				this.SkipNewlinesAndSemicolons();
			}
			this.UngetToken(token);
			extent = Parser.ExtentOf(scriptExtent, last);
			IScriptExtent extent2;
			this.CompleteScriptBlockBody(lCurly, ref extent, out extent2);
			return new ScriptBlockAst(extent2, usingStatements, paramBlockAst, namedBlockAst2, namedBlockAst3, namedBlockAst4, namedBlockAst);
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x001447E8 File Offset: 0x001429E8
		private StatementBlockAst StatementBlockRule()
		{
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.LCurly)
			{
				this.UngetToken(token);
				return null;
			}
			List<TrapStatementAst> traps = new List<TrapStatementAst>();
			List<StatementAst> statements = new List<StatementAst>();
			IScriptExtent scriptExtent = this.StatementListRule(statements, traps);
			Token token2 = this.NextToken();
			IScriptExtent last;
			if (token2.Kind != TokenKind.RCurly)
			{
				this.UngetToken(token2);
				last = (scriptExtent ?? token.Extent);
				this.ReportIncompleteInput(token.Extent, token2.Extent, () => ParserStrings.MissingEndCurlyBrace, new object[0]);
			}
			else
			{
				last = token2.Extent;
			}
			return new StatementBlockAst(Parser.ExtentOf(token, last), statements, traps);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x001448A8 File Offset: 0x00142AA8
		private IScriptExtent StatementListRule(List<StatementAst> statements, List<TrapStatementAst> traps)
		{
			StatementAst statementAst = null;
			StatementAst last = null;
			this.SkipNewlinesAndSemicolons();
			Token token;
			do
			{
				StatementAst statementAst2 = this.StatementRule();
				if (statementAst2 == null)
				{
					break;
				}
				this._tokenizer.CheckAstIsBeforeSignature(statementAst2);
				TrapStatementAst trapStatementAst = statementAst2 as TrapStatementAst;
				if (trapStatementAst != null)
				{
					traps.Add(trapStatementAst);
				}
				else
				{
					statements.Add(statementAst2);
				}
				if (statementAst == null)
				{
					statementAst = statementAst2;
				}
				last = statementAst2;
				this.SkipNewlinesAndSemicolons();
				token = this.PeekToken();
			}
			while (token.Kind != TokenKind.RParen && token.Kind != TokenKind.RCurly);
			if (statementAst != null)
			{
				return Parser.ExtentOf(statementAst, last);
			}
			return null;
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00144934 File Offset: 0x00142B34
		private StatementAst StatementRule()
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			int restorePoint = 0;
			Token token = this.NextToken();
			List<AttributeBaseAst> list = null;
			if (token.Kind == TokenKind.Generic && token.Text[0] == '[')
			{
				restorePoint = token.Extent.StartOffset;
				this.Resync(token);
				list = this.AttributeListRule(false);
				token = this.NextToken();
				if (list != null && list.Count > 0)
				{
					if ((token.TokenFlags & TokenFlags.StatementDoesntSupportAttributes) != TokenFlags.None)
					{
						if (list.OfType<TypeConstraintAst>().Any<TypeConstraintAst>())
						{
							this.Resync(restorePoint);
							token = this.NextToken();
						}
						else
						{
							this.ReportError(list[0].Extent, () => ParserStrings.UnexpectedAttribute, list[0].TypeName.FullName);
						}
					}
					else
					{
						if ((token.TokenFlags & TokenFlags.Keyword) != TokenFlags.None)
						{
							using (IEnumerator<AttributeBaseAst> enumerator = (from attr in list
							where !(attr is AttributeAst)
							select attr).GetEnumerator())
							{
								if (enumerator.MoveNext())
								{
									AttributeBaseAst attributeBaseAst = enumerator.Current;
									this.ReportError(attributeBaseAst.Extent, () => ParserStrings.TypeNotAllowedBeforeStatement, attributeBaseAst.TypeName.FullName);
								}
								goto IL_17C;
							}
						}
						this.Resync(restorePoint);
						token = this.NextToken();
					}
				}
			}
			IL_17C:
			TokenKind kind = token.Kind;
			StatementAst statementAst;
			if (kind != TokenKind.Label)
			{
				if (kind != TokenKind.EndOfInput)
				{
					switch (kind)
					{
					case TokenKind.Break:
						return this.BreakStatementRule(token);
					case TokenKind.Catch:
					case TokenKind.Else:
					case TokenKind.ElseIf:
					case TokenKind.Until:
						if (this._errorList.Count > 0)
						{
							this.SkipNewlines();
							return this.StatementRule();
						}
						break;
					case TokenKind.Class:
						return this.ClassDefinitionRule(list, token);
					case TokenKind.Continue:
						return this.ContinueStatementRule(token);
					case TokenKind.Data:
						return this.DataStatementRule(token);
					case TokenKind.Define:
					case TokenKind.From:
					case TokenKind.Var:
						this.ReportError(token.Extent, () => ParserStrings.ReservedKeywordNotAllowed, token.Kind.Text());
						return new ErrorStatementAst(token.Extent, null);
					case TokenKind.Do:
						return this.DoWhileStatementRule(null, token);
					case TokenKind.Exit:
						return this.ExitStatementRule(token);
					case TokenKind.Filter:
					case TokenKind.Function:
					case TokenKind.Workflow:
						return this.FunctionDeclarationRule(token);
					case TokenKind.For:
						return this.ForStatementRule(null, token);
					case TokenKind.Foreach:
						return this.ForeachStatementRule(null, token);
					case TokenKind.If:
						return this.IfStatementRule(token);
					case TokenKind.Return:
						return this.ReturnStatementRule(token);
					case TokenKind.Switch:
						return this.SwitchStatementRule(null, token);
					case TokenKind.Throw:
						return this.ThrowStatementRule(token);
					case TokenKind.Trap:
						return this.TrapStatementRule(token);
					case TokenKind.Try:
						return this.TryStatementRule(token);
					case TokenKind.Using:
						statementAst = this.UsingStatementRule(token);
						this.ReportError(statementAst.Extent, () => ParserStrings.UsingMustBeAtStartOfScript);
						return statementAst;
					case TokenKind.While:
						return this.WhileStatementRule(null, token);
					case TokenKind.Parallel:
					case TokenKind.Sequence:
						return this.BlockStatementRule(token);
					case TokenKind.Configuration:
						return this.ConfigurationStatementRule((list != null) ? list.OfType<AttributeAst>() : null, token);
					case TokenKind.DynamicKeyword:
					{
						DynamicKeyword keyword = DynamicKeyword.GetKeyword(token.Text);
						return this.DynamicKeywordStatementRule(token, keyword);
					}
					case TokenKind.Enum:
						return this.EnumDefinitionRule(list, token);
					}
					if (list != null)
					{
						this.Resync(restorePoint);
					}
					else
					{
						this.UngetToken(token);
					}
					statementAst = this.PipelineRule();
				}
				else if (list != null)
				{
					this.Resync(restorePoint);
					statementAst = this.PipelineRule();
				}
				else
				{
					this.UngetToken(token);
					statementAst = null;
				}
			}
			else
			{
				this.SkipNewlines();
				statementAst = this.LabeledStatementRule((LabelToken)token);
			}
			return statementAst;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00144DAC File Offset: 0x00142FAC
		private StringConstantExpressionAst SimpleNameRule()
		{
			Token token;
			return this.SimpleNameRule(out token);
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x00144DC4 File Offset: 0x00142FC4
		private StringConstantExpressionAst SimpleNameRule(out Token token)
		{
			try
			{
				this._tokenizer.WantSimpleName = true;
				token = this.PeekToken();
			}
			finally
			{
				this._tokenizer.WantSimpleName = false;
			}
			if (token.Kind == TokenKind.Identifier)
			{
				token.TokenFlags |= TokenFlags.MemberName;
				this.SkipToken();
				return new StringConstantExpressionAst(token.Extent, token.Text, StringConstantType.BareWord);
			}
			return null;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x00144E40 File Offset: 0x00143040
		private ExpressionAst LabelOrKeyRule()
		{
			StringConstantExpressionAst stringConstantExpressionAst = this.SimpleNameRule();
			if (stringConstantExpressionAst != null)
			{
				return stringConstantExpressionAst;
			}
			Token token = this.PeekToken();
			if (token.Kind != TokenKind.NewLine && token.Kind != TokenKind.Semi)
			{
				bool disableCommaOperator = this._disableCommaOperator;
				ExpressionAst expressionAst;
				try
				{
					this._disableCommaOperator = true;
					expressionAst = this.UnaryExpressionRule();
				}
				finally
				{
					this._disableCommaOperator = disableCommaOperator;
				}
				if (expressionAst != null)
				{
					return expressionAst;
				}
			}
			return null;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x00144EA8 File Offset: 0x001430A8
		private BreakStatementAst BreakStatementRule(Token breakToken)
		{
			ExpressionAst expressionAst = this.LabelOrKeyRule();
			IScriptExtent extent = (expressionAst != null) ? Parser.ExtentOf(breakToken, expressionAst) : breakToken.Extent;
			return new BreakStatementAst(extent, expressionAst);
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00144ED8 File Offset: 0x001430D8
		private ContinueStatementAst ContinueStatementRule(Token continueToken)
		{
			ExpressionAst expressionAst = this.LabelOrKeyRule();
			IScriptExtent extent = (expressionAst != null) ? Parser.ExtentOf(continueToken, expressionAst) : continueToken.Extent;
			return new ContinueStatementAst(extent, expressionAst);
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00144F08 File Offset: 0x00143108
		private ReturnStatementAst ReturnStatementRule(Token token)
		{
			PipelineBaseAst pipelineBaseAst = this.PipelineRule();
			IScriptExtent extent = (pipelineBaseAst != null) ? Parser.ExtentOf(token, pipelineBaseAst) : token.Extent;
			return new ReturnStatementAst(extent, pipelineBaseAst);
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00144F38 File Offset: 0x00143138
		private ExitStatementAst ExitStatementRule(Token token)
		{
			PipelineBaseAst pipelineBaseAst = this.PipelineRule();
			IScriptExtent extent = (pipelineBaseAst != null) ? Parser.ExtentOf(token, pipelineBaseAst) : token.Extent;
			return new ExitStatementAst(extent, pipelineBaseAst);
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x00144F68 File Offset: 0x00143168
		private ThrowStatementAst ThrowStatementRule(Token token)
		{
			PipelineBaseAst pipelineBaseAst = this.PipelineRule();
			IScriptExtent extent = (pipelineBaseAst != null) ? Parser.ExtentOf(token, pipelineBaseAst) : token.Extent;
			return new ThrowStatementAst(extent, pipelineBaseAst);
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x00144F98 File Offset: 0x00143198
		private StatementAst LabeledStatementRule(LabelToken label)
		{
			Token token = this.NextToken();
			TokenKind kind = token.Kind;
			if (kind <= TokenKind.Foreach)
			{
				if (kind == TokenKind.Do)
				{
					return this.DoWhileStatementRule(label, token);
				}
				switch (kind)
				{
				case TokenKind.For:
					return this.ForStatementRule(label, token);
				case TokenKind.Foreach:
					return this.ForeachStatementRule(label, token);
				}
			}
			else
			{
				if (kind == TokenKind.Switch)
				{
					return this.SwitchStatementRule(label, token);
				}
				if (kind == TokenKind.While)
				{
					return this.WhileStatementRule(label, token);
				}
			}
			this.Resync(label);
			return this.PipelineRule();
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00145030 File Offset: 0x00143230
		private StatementAst BlockStatementRule(Token kindToken)
		{
			StatementBlockAst statementBlockAst = this.StatementBlockRule();
			if (statementBlockAst == null)
			{
				this.ReportIncompleteInput(Parser.After(kindToken.Extent), () => ParserStrings.MissingStatementAfterKeyword, kindToken.Text);
				return new ErrorStatementAst(Parser.ExtentOf(kindToken, kindToken), null);
			}
			return new BlockStatementAst(Parser.ExtentOf(kindToken, statementBlockAst), kindToken, statementBlockAst);
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0014509C File Offset: 0x0014329C
		private bool InlineScriptRule(Token inlineScriptToken, List<CommandElementAst> elements)
		{
			StringConstantExpressionAst item = new StringConstantExpressionAst(inlineScriptToken.Extent, inlineScriptToken.Text, StringConstantType.BareWord);
			inlineScriptToken.TokenFlags |= TokenFlags.CommandName;
			elements.Add(item);
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.LCurly)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(inlineScriptToken), () => ParserStrings.MissingStatementAfterKeyword, inlineScriptToken.Text);
				return false;
			}
			ExpressionAst item2 = this.ScriptBlockExpressionRule(token);
			elements.Add(item2);
			return true;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00145138 File Offset: 0x00143338
		private StatementAst IfStatementRule(Token ifToken)
		{
			List<Tuple<PipelineBaseAst, StatementBlockAst>> list = new List<Tuple<PipelineBaseAst, StatementBlockAst>>();
			List<Ast> list2 = new List<Ast>();
			StatementBlockAst statementBlockAst = null;
			Token token = ifToken;
			Token token2;
			PipelineBaseAst pipelineBaseAst;
			Token token3;
			for (;;)
			{
				this.SkipNewlines();
				token2 = this.NextToken();
				if (token2.Kind != TokenKind.LParen)
				{
					break;
				}
				this.SkipNewlines();
				pipelineBaseAst = this.PipelineRule();
				if (pipelineBaseAst == null)
				{
					IScriptExtent extent = Parser.After(token2);
					this.ReportIncompleteInput(extent, () => ParserStrings.IfStatementMissingCondition, token.Text);
					pipelineBaseAst = new ErrorStatementAst(extent, null);
				}
				else
				{
					list2.Add(pipelineBaseAst);
				}
				this.SkipNewlines();
				token3 = this.NextToken();
				if (token3.Kind != TokenKind.RParen)
				{
					goto Block_3;
				}
				this.SkipNewlines();
				StatementBlockAst statementBlockAst2 = this.StatementBlockRule();
				if (statementBlockAst2 == null)
				{
					goto Block_5;
				}
				list2.Add(statementBlockAst2);
				list.Add(new Tuple<PipelineBaseAst, StatementBlockAst>(pipelineBaseAst, statementBlockAst2));
				this.SkipNewlines();
				token = this.PeekToken();
				if (token.Kind != TokenKind.ElseIf)
				{
					goto IL_1D3;
				}
				this.SkipToken();
			}
			this.UngetToken(token2);
			this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingOpenParenthesisInIfStatement, token.Text);
			return new ErrorStatementAst(Parser.ExtentOf(ifToken, token), list2);
			Block_3:
			this.UngetToken(token3);
			if (!(pipelineBaseAst is ErrorStatementAst))
			{
				this.ReportIncompleteInput(token3.Extent, () => ParserStrings.MissingEndParenthesisAfterStatement, token.Text);
			}
			return new ErrorStatementAst(Parser.ExtentOf(ifToken, Parser.Before(token3)), list2);
			Block_5:
			this.ReportIncompleteInput(token3.Extent, () => ParserStrings.MissingStatementBlock, token.Text);
			return new ErrorStatementAst(Parser.ExtentOf(ifToken, token3), list2);
			IL_1D3:
			if (token.Kind == TokenKind.Else)
			{
				this.SkipToken();
				this.SkipNewlines();
				statementBlockAst = this.StatementBlockRule();
				if (statementBlockAst == null)
				{
					this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingStatementBlockAfterElse);
					return new ErrorStatementAst(Parser.ExtentOf(ifToken, token), list2);
				}
			}
			IScriptExtent last = (statementBlockAst != null) ? statementBlockAst.Extent : list[list.Count - 1].Item2.Extent;
			IScriptExtent extent2 = Parser.ExtentOf(ifToken, last);
			return new IfStatementAst(extent2, list, statementBlockAst);
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x001453B0 File Offset: 0x001435B0
		private StatementAst SwitchStatementRule(LabelToken labelToken, Token switchToken)
		{
			IScriptExtent extent = (labelToken ?? switchToken).Extent;
			IScriptExtent scriptExtent = extent;
			bool flag = false;
			bool flag2 = false;
			this.SkipNewlines();
			bool flag3 = false;
			PipelineBaseAst pipelineBaseAst = null;
			Dictionary<string, Tuple<Token, Ast>> dictionary = null;
			Token token = this.PeekToken();
			SwitchFlags switchFlags = SwitchFlags.None;
			while (token.Kind == TokenKind.Parameter)
			{
				this.SkipToken();
				scriptExtent = token.Extent;
				dictionary = (dictionary ?? new Dictionary<string, Tuple<Token, Ast>>());
				if (Parser.IsSpecificParameter(token, "regex"))
				{
					switchFlags |= SwitchFlags.Regex;
					switchFlags &= ~SwitchFlags.Wildcard;
					if (!dictionary.ContainsKey("regex"))
					{
						dictionary.Add("regex", new Tuple<Token, Ast>(token, null));
					}
				}
				else if (Parser.IsSpecificParameter(token, "wildcard"))
				{
					switchFlags |= SwitchFlags.Wildcard;
					switchFlags &= ~SwitchFlags.Regex;
					if (!dictionary.ContainsKey("wildcard"))
					{
						dictionary.Add("wildcard", new Tuple<Token, Ast>(token, null));
					}
				}
				else if (Parser.IsSpecificParameter(token, "exact"))
				{
					switchFlags &= ~SwitchFlags.Regex;
					switchFlags &= ~SwitchFlags.Wildcard;
					if (!dictionary.ContainsKey("exact"))
					{
						dictionary.Add("exact", new Tuple<Token, Ast>(token, null));
					}
				}
				else if (Parser.IsSpecificParameter(token, "casesensitive"))
				{
					switchFlags |= SwitchFlags.CaseSensitive;
					if (!dictionary.ContainsKey("casesensitive"))
					{
						dictionary.Add("casesensitive", new Tuple<Token, Ast>(token, null));
					}
				}
				else if (Parser.IsSpecificParameter(token, "parallel"))
				{
					switchFlags |= SwitchFlags.Parallel;
					if (!dictionary.ContainsKey("parallel"))
					{
						dictionary.Add("parallel", new Tuple<Token, Ast>(token, null));
					}
				}
				else if (Parser.IsSpecificParameter(token, "file"))
				{
					switchFlags |= SwitchFlags.File;
					this.SkipNewlines();
					ExpressionAst singleCommandArgument = this.GetSingleCommandArgument(Parser.CommandArgumentContext.FileName);
					if (singleCommandArgument == null)
					{
						flag = true;
						flag2 = this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingFilenameOption);
						if (!dictionary.ContainsKey("file"))
						{
							dictionary.Add("file", new Tuple<Token, Ast>(token, null));
						}
					}
					else
					{
						scriptExtent = singleCommandArgument.Extent;
						pipelineBaseAst = new PipelineAst(singleCommandArgument.Extent, new CommandExpressionAst(singleCommandArgument.Extent, singleCommandArgument, null));
						if (!dictionary.ContainsKey("file"))
						{
							dictionary.Add("file", new Tuple<Token, Ast>(token, pipelineBaseAst));
						}
					}
				}
				else
				{
					flag = true;
					this.ReportError(token.Extent, () => ParserStrings.InvalidSwitchFlag, ((ParameterToken)token).ParameterName);
				}
				token = this.PeekToken();
			}
			if (token.Kind == TokenKind.Minus)
			{
				dictionary = (dictionary ?? new Dictionary<string, Tuple<Token, Ast>>());
				dictionary.Add("--%", new Tuple<Token, Ast>(token, null));
			}
			Token token2 = this.PeekToken();
			if (token2.Kind == TokenKind.LParen)
			{
				scriptExtent = token2.Extent;
				this.SkipToken();
				if ((switchFlags & SwitchFlags.File) == SwitchFlags.File)
				{
					flag = true;
					this.ReportError(token2.Extent, () => ParserStrings.PipelineValueRequired);
				}
				flag3 = true;
				this.SkipNewlines();
				pipelineBaseAst = this.PipelineRule();
				if (pipelineBaseAst == null)
				{
					flag = true;
					flag2 = this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.PipelineValueRequired);
				}
				else
				{
					scriptExtent = pipelineBaseAst.Extent;
				}
				this.SkipNewlines();
				Token token3 = this.NextToken();
				if (token3.Kind != TokenKind.RParen)
				{
					this.UngetToken(token3);
					if (!flag2)
					{
						flag = true;
						flag2 = this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingEndParenthesisInSwitchStatement);
					}
				}
				else
				{
					scriptExtent = token3.Extent;
				}
			}
			else if (pipelineBaseAst == null && (switchFlags & SwitchFlags.File) == SwitchFlags.None)
			{
				flag = true;
				flag2 = this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.PipelineValueRequired);
			}
			this.SkipNewlines();
			Token token4 = this.NextToken();
			StatementBlockAst statementBlockAst = null;
			List<Tuple<ExpressionAst, StatementBlockAst>> list = new List<Tuple<ExpressionAst, StatementBlockAst>>();
			List<Ast> list2 = new List<Ast>();
			Token last = null;
			if (token4.Kind != TokenKind.LCurly)
			{
				this.UngetToken(token4);
				if (!flag2)
				{
					flag = true;
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingCurlyBraceInSwitchStatement);
				}
			}
			else
			{
				scriptExtent = token4.Extent;
				this.SkipNewlines();
				Token token5;
				for (;;)
				{
					ExpressionAst singleCommandArgument2 = this.GetSingleCommandArgument(Parser.CommandArgumentContext.SwitchCondition);
					if (singleCommandArgument2 == null)
					{
						break;
					}
					list2.Add(singleCommandArgument2);
					scriptExtent = singleCommandArgument2.Extent;
					StatementBlockAst statementBlockAst2 = this.StatementBlockRule();
					if (statementBlockAst2 == null)
					{
						flag = true;
						flag2 = this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingSwitchStatementClause);
					}
					else
					{
						list2.Add(statementBlockAst2);
						scriptExtent = statementBlockAst2.Extent;
						StringConstantExpressionAst stringConstantExpressionAst = singleCommandArgument2 as StringConstantExpressionAst;
						if (stringConstantExpressionAst != null && stringConstantExpressionAst.StringConstantType == StringConstantType.BareWord && stringConstantExpressionAst.Value.Equals("default", StringComparison.OrdinalIgnoreCase))
						{
							if (statementBlockAst != null)
							{
								flag = true;
								this.ReportError(singleCommandArgument2.Extent, () => ParserStrings.MultipleSwitchDefaultClauses);
							}
							statementBlockAst = statementBlockAst2;
						}
						else
						{
							list.Add(new Tuple<ExpressionAst, StatementBlockAst>(singleCommandArgument2, statementBlockAst2));
						}
					}
					this.SkipNewlinesAndSemicolons();
					token5 = this.PeekToken();
					if (token5.Kind == TokenKind.RCurly)
					{
						goto Block_36;
					}
					if (token5.Kind == TokenKind.EndOfInput)
					{
						goto Block_37;
					}
				}
				flag = true;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingSwitchConditionExpression);
				if (this.PeekToken().Kind == TokenKind.RCurly)
				{
					this.SkipToken();
					goto IL_645;
				}
				goto IL_645;
				Block_36:
				last = token5;
				this.SkipToken();
				goto IL_645;
				Block_37:
				if (!flag2)
				{
					flag = true;
					this.ReportIncompleteInput(token4.Extent, token5.Extent, () => ParserStrings.MissingEndCurlyBrace, new object[0]);
				}
			}
			IL_645:
			if (flag)
			{
				return new ErrorStatementAst(Parser.ExtentOf(extent, scriptExtent), switchToken, dictionary, flag3 ? Parser.GetNestedErrorAsts(new object[]
				{
					pipelineBaseAst
				}) : null, Parser.GetNestedErrorAsts(new object[]
				{
					list2
				}));
			}
			return new SwitchStatementAst(Parser.ExtentOf(labelToken ?? switchToken, last), (labelToken != null) ? labelToken.LabelText : null, pipelineBaseAst, switchFlags, list, statementBlockAst);
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x00145AB0 File Offset: 0x00143CB0
		private StatementAst ConfigurationStatementRule(IEnumerable<AttributeAst> customAttributes, Token configurationToken)
		{
			IScriptExtent extent = configurationToken.Extent;
			IScriptExtent last = extent;
			bool flag = false;
			this.SkipNewlines();
			Token token = this.NextToken();
			Token token2 = token;
			if (token.Kind == TokenKind.LCurly)
			{
				this.ReportError(Parser.After(extent), () => ParserStrings.MissingConfigurationName);
				this.ScriptBlockExpressionRule(token);
				return null;
			}
			if (token.Kind == TokenKind.EndOfInput)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(token.Extent), () => ParserStrings.MissingConfigurationName);
				return null;
			}
			this.UngetToken(token);
			ExpressionAst wordOrExpression = this.GetWordOrExpression(token);
			object obj;
			if (wordOrExpression == null)
			{
				flag = true;
				this.ReportIncompleteInput(token.Extent, () => ParserStrings.MissingConfigurationName);
			}
			else if (IsConstantValueVisitor.IsConstant(wordOrExpression, out obj, false, false))
			{
				string text = obj as string;
				if (text == null || !Regex.IsMatch(text, "^[A-Za-z][A-Za-z0-9_]*$"))
				{
					flag = true;
					this.ReportError(wordOrExpression.Extent, () => ParserStrings.InvalidConfigurationName, text ?? string.Empty);
				}
			}
			this.SkipNewlines();
			Runspace runspace = null;
			bool flag2 = false;
			StatementAst result;
			try
			{
				if (Runspace.DefaultRunspace == null)
				{
					runspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault2());
					runspace.ThreadOptions = PSThreadOptions.UseCurrentThread;
					runspace.Open();
					Runspace.DefaultRunspace = runspace;
				}
				if (PsUtils.IsRunningOnProcessorArchitectureARM() || Runspace.DefaultRunspace.ExecutionContext.LanguageMode == PSLanguageMode.ConstrainedLanguage)
				{
					this.ReportError(configurationToken.Extent, () => ParserStrings.ConfigurationNotAllowedInConstrainedLanguage, configurationToken.Kind.Text());
					result = null;
				}
				else if (Utils.IsWinPEHost())
				{
					this.ReportError(configurationToken.Extent, () => ParserStrings.ConfigurationNotAllowedOnWinPE, configurationToken.Kind.Text());
					result = null;
				}
				else
				{
					ExpressionAst expressionAst = null;
					PowerShell powerShell = null;
					Parser engineParser = Runspace.DefaultRunspace.ExecutionContext.Engine.EngineParser;
					Runspace.DefaultRunspace.ExecutionContext.Engine.EngineParser = new Parser();
					try
					{
						if (runspace != null)
						{
							powerShell = PowerShell.Create();
							powerShell.Runspace = runspace;
						}
						else
						{
							powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace);
						}
						try
						{
							if (DynamicKeyword.GetKeyword("OMI_ConfigurationDocument") == null)
							{
								Collection<Exception> collection = new Collection<Exception>();
								DscClassCache.LoadDefaultCimKeywords(collection);
								if (collection.Count > 0)
								{
									this.ReportErrorsAsWarnings(collection);
								}
								if (this._configurationKeywordsDefinedInThisFile != null)
								{
									foreach (DynamicKeyword dynamicKeyword in this._configurationKeywordsDefinedInThisFile.Values)
									{
										if (!DynamicKeyword.ContainsKeyword(dynamicKeyword.Keyword))
										{
											DynamicKeyword.AddKeyword(dynamicKeyword);
										}
									}
								}
								flag2 = true;
							}
						}
						catch (Exception e)
						{
							Exception e3;
							Exception e = e3;
							this.ReportError(token2.Extent, () => e.ToString());
							return null;
						}
					}
					finally
					{
						if (powerShell != null)
						{
							powerShell.Dispose();
						}
						Runspace.DefaultRunspace.ExecutionContext.Engine.EngineParser = engineParser;
					}
					Token token3 = this.NextToken();
					if (token3.Kind != TokenKind.LCurly)
					{
						this.ReportIncompleteInput(Parser.After(token3.Extent), () => ParserStrings.MissingCurlyInConfigurationStatement);
						flag = true;
						this.UngetToken(token3);
					}
					else
					{
						bool inConfiguration = this._inConfiguration;
						try
						{
							this._inConfiguration = true;
							expressionAst = this.ScriptBlockExpressionRule(token3);
						}
						finally
						{
							this._inConfiguration = inConfiguration;
						}
						if (expressionAst == null)
						{
							this.ReportError(Parser.After(token3.Extent), () => ParserStrings.ConfigurationBodyEmpty);
							return null;
						}
					}
					if (flag)
					{
						result = new ErrorStatementAst(Parser.ExtentOf(extent, last), configurationToken, null);
					}
					else
					{
						StringConstantExpressionAst stringConstantExpressionAst = wordOrExpression as StringConstantExpressionAst;
						if (stringConstantExpressionAst != null)
						{
							DynamicKeyword dynamicKeyword2 = new DynamicKeyword
							{
								BodyMode = DynamicKeywordBodyMode.Hashtable,
								ImplementingModule = this._keywordModuleName,
								Keyword = stringConstantExpressionAst.Value,
								NameMode = DynamicKeywordNameMode.NameRequired,
								DirectCall = true
							};
							DynamicKeywordProperty dynamicKeywordProperty = new DynamicKeywordProperty
							{
								Mandatory = true,
								Name = "DependsOn"
							};
							dynamicKeyword2.Properties.Add(dynamicKeywordProperty.Name, dynamicKeywordProperty);
							ScriptBlockExpressionAst scriptBlockExpressionAst = expressionAst as ScriptBlockExpressionAst;
							if (scriptBlockExpressionAst != null)
							{
								ParamBlockAst paramBlock = scriptBlockExpressionAst.ScriptBlock.ParamBlock;
								if (paramBlock != null)
								{
									foreach (ParameterAst parameterAst in paramBlock.Parameters)
									{
										DynamicKeywordProperty dynamicKeywordProperty2 = new DynamicKeywordProperty();
										dynamicKeywordProperty2.Name = parameterAst.Name.VariablePath.UserPath;
										if (parameterAst.Attributes != null)
										{
											foreach (AttributeBaseAst attributeBaseAst in parameterAst.Attributes)
											{
												TypeConstraintAst typeConstraintAst = attributeBaseAst as TypeConstraintAst;
												if (typeConstraintAst != null)
												{
													dynamicKeywordProperty2.TypeConstraint = typeConstraintAst.TypeName.Name;
												}
												else
												{
													AttributeAst attributeAst = attributeBaseAst as AttributeAst;
													if (attributeAst != null && string.Equals(attributeAst.TypeName.Name, "Parameter", StringComparison.OrdinalIgnoreCase) && attributeAst.NamedArguments != null)
													{
														foreach (NamedAttributeArgumentAst namedAttributeArgumentAst in attributeAst.NamedArguments)
														{
															if (string.Equals(namedAttributeArgumentAst.ArgumentName, "Mandatory", StringComparison.OrdinalIgnoreCase))
															{
																if (namedAttributeArgumentAst.ExpressionOmitted)
																{
																	dynamicKeywordProperty2.Mandatory = true;
																}
																else if (namedAttributeArgumentAst.Argument != null)
																{
																	ConstantExpressionAst constantExpressionAst = namedAttributeArgumentAst.Argument as ConstantExpressionAst;
																	if (constantExpressionAst != null)
																	{
																		dynamicKeywordProperty2.Mandatory = LanguagePrimitives.IsTrue(constantExpressionAst.Value);
																	}
																}
															}
														}
													}
												}
											}
										}
										dynamicKeyword2.Properties.Add(dynamicKeywordProperty2.Name, dynamicKeywordProperty2);
									}
								}
							}
							if (flag2)
							{
								if (this._configurationKeywordsDefinedInThisFile == null)
								{
									this._configurationKeywordsDefinedInThisFile = new Dictionary<string, DynamicKeyword>();
								}
								this._configurationKeywordsDefinedInThisFile[dynamicKeyword2.Keyword] = dynamicKeyword2;
							}
							else
							{
								DynamicKeyword.AddKeyword(dynamicKeyword2);
							}
						}
						bool flag3 = false;
						if (customAttributes != null)
						{
							flag3 = customAttributes.Any((AttributeAst attribute) => attribute.TypeName.GetReflectionAttributeType() != null && attribute.TypeName.GetReflectionAttributeType() == typeof(DscLocalConfigurationManagerAttribute));
						}
						ScriptBlockExpressionAst scriptBlockExpressionAst2 = expressionAst as ScriptBlockExpressionAst;
						IScriptExtent extent2 = Parser.ExtentOf(extent, scriptBlockExpressionAst2);
						result = new ConfigurationDefinitionAst(extent2, scriptBlockExpressionAst2, flag3 ? ConfigurationType.Meta : ConfigurationType.Resource, wordOrExpression)
						{
							LCurlyToken = token3,
							ConfigurationToken = configurationToken,
							CustomAttributes = customAttributes,
							DefinedKeywords = DynamicKeyword.GetKeyword()
						};
					}
				}
			}
			catch (Exception e2)
			{
				Exception e = e2;
				CommandProcessorBase.CheckForSevereException(e);
				this.ReportError(token2.Extent, () => "ConfigurationStatementToken: " + e);
				result = null;
			}
			finally
			{
				if (runspace != null)
				{
					runspace.Close();
					Runspace.DefaultRunspace = null;
				}
				if (flag2)
				{
					DscClassCache.ClearCache();
					DynamicKeyword.Reset();
				}
				int restorePoint = this._tokenizer.GetRestorePoint();
				this.Resync(restorePoint);
			}
			return result;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00146380 File Offset: 0x00144580
		private ExpressionAst GetWordOrExpression(Token keywordToken)
		{
			Token token = this.NextToken();
			if (token.Kind == TokenKind.EndOfInput)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(keywordToken), () => ParserStrings.RequiredNameOrExpressionMissing);
				return null;
			}
			ExpressionAst commandArgument = this.GetCommandArgument(Parser.CommandArgumentContext.CommandArgument, token);
			if (commandArgument == null)
			{
				IScriptExtent extent = keywordToken.Extent;
				this.ReportError(Parser.After(extent), () => ParserStrings.ParameterRequiresArgument, keywordToken.Text);
			}
			return commandArgument;
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x0014641C File Offset: 0x0014461C
		private StatementAst ForeachStatementRule(LabelToken labelToken, Token forEachToken)
		{
			IScriptExtent first = (labelToken != null) ? labelToken.Extent : forEachToken.Extent;
			IScriptExtent scriptExtent = null;
			this.SkipNewlines();
			Token token = this.PeekToken();
			ForEachFlags forEachFlags = ForEachFlags.None;
			ExpressionAst expressionAst = null;
			while (token.Kind == TokenKind.Parameter)
			{
				this.SkipToken();
				if (Parser.IsSpecificParameter(token, "parallel"))
				{
					forEachFlags |= ForEachFlags.Parallel;
				}
				else if (Parser.IsSpecificParameter(token, "throttlelimit"))
				{
					this.SkipNewlines();
					expressionAst = this.GetSingleCommandArgument(Parser.CommandArgumentContext.CommandArgument);
					if (expressionAst == null)
					{
						this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingThrottleLimit);
					}
				}
				else
				{
					scriptExtent = token.Extent;
					this.ReportError(token.Extent, () => ParserStrings.InvalidForeachFlag, ((ParameterToken)token).ParameterName);
				}
				this.SkipNewlines();
				token = this.PeekToken();
			}
			Token token2 = this.NextToken();
			if (token2.Kind != TokenKind.LParen)
			{
				this.UngetToken(token2);
				scriptExtent = forEachToken.Extent;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingOpenParenthesisAfterKeyword, forEachToken.Kind.Text());
				return new ErrorStatementAst(Parser.ExtentOf(first, scriptExtent), null);
			}
			this.SkipNewlines();
			Token token3 = this.NextToken();
			if (token3.Kind != TokenKind.Variable && token3.Kind != TokenKind.SplattedVariable)
			{
				this.UngetToken(token3);
				scriptExtent = token2.Extent;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingVariableNameAfterForeach);
				return new ErrorStatementAst(Parser.ExtentOf(first, scriptExtent), null);
			}
			VariableExpressionAst variableExpressionAst = new VariableExpressionAst((VariableToken)token3);
			this.SkipNewlines();
			PipelineBaseAst pipelineBaseAst = null;
			StatementBlockAst statementBlockAst = null;
			Token token4 = this.NextToken();
			if (token4.Kind != TokenKind.In)
			{
				this.UngetToken(token4);
				scriptExtent = variableExpressionAst.Extent;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingInInForeach);
			}
			else
			{
				this.SkipNewlines();
				pipelineBaseAst = this.PipelineRule();
				if (pipelineBaseAst == null)
				{
					scriptExtent = token4.Extent;
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingForeachExpression);
				}
				else
				{
					this.SkipNewlines();
					Token token5 = this.NextToken();
					if (token5.Kind != TokenKind.RParen)
					{
						this.UngetToken(token5);
						scriptExtent = pipelineBaseAst.Extent;
						this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingEndParenthesisAfterForeach);
					}
					else
					{
						statementBlockAst = this.StatementBlockRule();
						if (statementBlockAst == null)
						{
							scriptExtent = token5.Extent;
							this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingForeachStatement);
						}
					}
				}
			}
			if (scriptExtent != null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(first, scriptExtent), Parser.GetNestedErrorAsts(new object[]
				{
					variableExpressionAst,
					pipelineBaseAst,
					statementBlockAst
				}));
			}
			return new ForEachStatementAst(Parser.ExtentOf(first, statementBlockAst), (labelToken != null) ? labelToken.LabelText : null, forEachFlags, expressionAst, variableExpressionAst, pipelineBaseAst, statementBlockAst);
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00146798 File Offset: 0x00144998
		private StatementAst ForStatementRule(LabelToken labelToken, Token forToken)
		{
			IScriptExtent scriptExtent = null;
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.LParen)
			{
				this.UngetToken(token);
				scriptExtent = forToken.Extent;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingOpenParenthesisAfterKeyword, forToken.Kind.Text());
				return new ErrorStatementAst(Parser.ExtentOf(labelToken ?? forToken, scriptExtent), null);
			}
			this.SkipNewlines();
			PipelineBaseAst pipelineBaseAst = this.PipelineRule();
			if (pipelineBaseAst != null)
			{
				scriptExtent = pipelineBaseAst.Extent;
			}
			if (this.PeekToken().Kind == TokenKind.Semi)
			{
				scriptExtent = this.NextToken().Extent;
			}
			this.SkipNewlines();
			PipelineBaseAst pipelineBaseAst2 = this.PipelineRule();
			if (pipelineBaseAst2 != null)
			{
				scriptExtent = pipelineBaseAst2.Extent;
			}
			if (this.PeekToken().Kind == TokenKind.Semi)
			{
				scriptExtent = this.NextToken().Extent;
			}
			this.SkipNewlines();
			PipelineBaseAst pipelineBaseAst3 = this.PipelineRule();
			if (pipelineBaseAst3 != null)
			{
				scriptExtent = pipelineBaseAst3.Extent;
			}
			this.SkipNewlines();
			Token token2 = this.NextToken();
			StatementBlockAst statementBlockAst = null;
			if (token2.Kind != TokenKind.RParen)
			{
				this.UngetToken(token2);
				if (scriptExtent == null)
				{
					scriptExtent = token.Extent;
				}
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingEndParenthesisAfterStatement, forToken.Kind.Text());
			}
			else
			{
				statementBlockAst = this.StatementBlockRule();
				if (statementBlockAst == null)
				{
					scriptExtent = token2.Extent;
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingLoopStatement, forToken.Kind.Text());
				}
			}
			if (statementBlockAst == null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(labelToken ?? forToken, scriptExtent), Parser.GetNestedErrorAsts(new object[]
				{
					pipelineBaseAst,
					pipelineBaseAst2,
					pipelineBaseAst3
				}));
			}
			return new ForStatementAst(Parser.ExtentOf(labelToken ?? forToken, statementBlockAst), (labelToken != null) ? labelToken.LabelText : null, pipelineBaseAst, pipelineBaseAst2, pipelineBaseAst3, statementBlockAst);
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x001469A4 File Offset: 0x00144BA4
		private StatementAst WhileStatementRule(LabelToken labelToken, Token whileToken)
		{
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.LParen)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(whileToken), () => ParserStrings.MissingOpenParenthesisAfterKeyword, whileToken.Text);
				return new ErrorStatementAst(Parser.ExtentOf(labelToken ?? whileToken, whileToken), null);
			}
			this.SkipNewlines();
			PipelineBaseAst pipelineBaseAst = this.PipelineRule();
			PipelineBaseAst pipelineBaseAst2 = null;
			if (pipelineBaseAst == null)
			{
				IScriptExtent extent = Parser.After(token);
				this.ReportIncompleteInput(extent, () => ParserStrings.MissingExpressionAfterKeyword, whileToken.Kind.Text());
				pipelineBaseAst = new ErrorStatementAst(extent, null);
			}
			else
			{
				pipelineBaseAst2 = pipelineBaseAst;
			}
			this.SkipNewlines();
			Token token2 = this.NextToken();
			if (token2.Kind != TokenKind.RParen)
			{
				this.UngetToken(token2);
				if (!(pipelineBaseAst is ErrorStatementAst))
				{
					this.ReportIncompleteInput(Parser.After(pipelineBaseAst), () => ParserStrings.MissingEndParenthesisAfterStatement, whileToken.Kind.Text());
				}
				return new ErrorStatementAst(Parser.ExtentOf(labelToken ?? whileToken, pipelineBaseAst), Parser.GetNestedErrorAsts(new object[]
				{
					pipelineBaseAst2
				}));
			}
			this.SkipNewlines();
			StatementBlockAst statementBlockAst = this.StatementBlockRule();
			if (statementBlockAst == null)
			{
				this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.MissingLoopStatement, whileToken.Kind.Text());
				return new ErrorStatementAst(Parser.ExtentOf(labelToken ?? whileToken, token2), Parser.GetNestedErrorAsts(new object[]
				{
					pipelineBaseAst2
				}));
			}
			return new WhileStatementAst(Parser.ExtentOf(labelToken ?? whileToken, statementBlockAst), (labelToken != null) ? labelToken.LabelText : null, pipelineBaseAst, statementBlockAst);
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00146B88 File Offset: 0x00144D88
		private StatementAst DynamicKeywordStatementRule(Token functionName, DynamicKeyword keywordData)
		{
			if (keywordData.PreParse != null)
			{
				try
				{
					ParseError[] array = keywordData.PreParse(keywordData);
					if (array != null && array.Length > 0)
					{
						foreach (ParseError error in array)
						{
							this.ReportError(error);
						}
					}
				}
				catch (Exception ex)
				{
					this.ReportError(functionName.Extent, () => ParserStrings.DynamicKeywordPreParseException, keywordData.ResourceName, ex.ToString());
					return null;
				}
			}
			if (keywordData.IsReservedKeyword)
			{
				this.ReportError(functionName.Extent, () => ParserStrings.UnsupportedReservedKeyword, keywordData.Keyword);
				return null;
			}
			if (keywordData.HasReservedProperties)
			{
				this.ReportError(functionName.Extent, () => ParserStrings.UnsupportedReservedProperty, "'Require', 'Trigger', 'Notify', 'Before', 'After' and 'Subscribe'");
				return null;
			}
			string text = string.Empty;
			DynamicKeywordStatementAst dynamicKeywordStatementAst;
			if (keywordData.BodyMode == DynamicKeywordBodyMode.Command)
			{
				this.UngetToken(functionName);
				dynamicKeywordStatementAst = (DynamicKeywordStatementAst)this.CommandRule(true);
				dynamicKeywordStatementAst.Keyword = keywordData;
				dynamicKeywordStatementAst.FunctionName = functionName;
			}
			else
			{
				this.SkipNewlines();
				ExpressionAst expressionAst = null;
				Token token = this.NextToken();
				if (token.Kind == TokenKind.EndOfInput)
				{
					this.UngetToken(token);
					if (keywordData.NameMode == DynamicKeywordNameMode.NameRequired || keywordData.NameMode == DynamicKeywordNameMode.SimpleNameRequired)
					{
						this.ReportIncompleteInput(Parser.After(functionName), () => ParserStrings.RequiredNameOrExpressionMissing);
					}
					else
					{
						this.ReportIncompleteInput(Parser.After(functionName.Extent), () => ParserStrings.MissingBraceInObjectDefinition);
					}
					return null;
				}
				Token token2 = null;
				if (token.Kind == TokenKind.LCurly)
				{
					token2 = token;
					if (keywordData.NameMode == DynamicKeywordNameMode.NameRequired || keywordData.NameMode == DynamicKeywordNameMode.SimpleNameRequired)
					{
						this.ReportError(Parser.After(functionName), () => ParserStrings.RequiredNameOrExpressionMissing);
						this.UngetToken(token);
						return null;
					}
				}
				else if (token.Kind == TokenKind.Identifier || token.Kind == TokenKind.DynamicKeyword)
				{
					if (keywordData.NameMode == DynamicKeywordNameMode.NoName)
					{
						this.ReportError(Parser.After(functionName), () => ParserStrings.UnexpectedNameForType, functionName.Text, token.Text);
						this.UngetToken(token);
						return null;
					}
					text = token.Text;
					if ((keywordData.NameMode == DynamicKeywordNameMode.SimpleNameRequired || keywordData.NameMode == DynamicKeywordNameMode.SimpleOptionalName) && string.IsNullOrEmpty(text))
					{
						this.ReportIncompleteInput(Parser.After(functionName), () => ParserStrings.RequiredNameOrExpressionMissing);
						this.UngetToken(token);
						return null;
					}
				}
				else
				{
					this.UngetToken(token);
					expressionAst = this.GetSingleCommandArgument(Parser.CommandArgumentContext.CommandName);
					if (expressionAst == null)
					{
						if (keywordData.NameMode == DynamicKeywordNameMode.SimpleNameRequired || keywordData.NameMode == DynamicKeywordNameMode.SimpleOptionalName)
						{
							this.ReportError(Parser.After(functionName), () => ParserStrings.RequiredNameOrExpressionMissing);
						}
						else
						{
							this.ReportError(Parser.After(functionName), () => ParserStrings.UnexpectedToken, token.Text);
						}
						return null;
					}
					if (keywordData.NameMode == DynamicKeywordNameMode.NoName)
					{
						this.ReportError(Parser.After(functionName), () => ParserStrings.UnexpectedNameForType, functionName.Text, expressionAst.ToString());
						return null;
					}
					if (keywordData.NameMode == DynamicKeywordNameMode.SimpleNameRequired || keywordData.NameMode == DynamicKeywordNameMode.SimpleOptionalName)
					{
						this.ReportError(token.Extent, () => ParserStrings.UnexpectedToken, token.Text);
						return null;
					}
				}
				ExpressionAst expressionAst2 = expressionAst;
				if (expressionAst == null)
				{
					expressionAst = new StringConstantExpressionAst(token.Extent, text, StringConstantType.BareWord);
				}
				this.SkipNewlines();
				if (token2 == null)
				{
					token2 = this.NextToken();
					if (token2.Kind == TokenKind.EndOfInput)
					{
						this.UngetToken(token2);
						this.ReportIncompleteInput(Parser.After(functionName.Extent), () => ParserStrings.MissingBraceInObjectDefinition);
						if (expressionAst2 != null)
						{
							return new ErrorStatementAst(Parser.ExtentOf(functionName, expressionAst2), Parser.GetNestedErrorAsts(new object[]
							{
								expressionAst2
							}));
						}
						return null;
					}
					else if (token2.Kind != TokenKind.LCurly)
					{
						InvokeMemberExpressionAst invokeMemberExpressionAst = expressionAst as InvokeMemberExpressionAst;
						if (invokeMemberExpressionAst != null && invokeMemberExpressionAst.Arguments.Count == 1 && invokeMemberExpressionAst.Arguments[0] is ScriptBlockExpressionAst && invokeMemberExpressionAst.Member.Extent.EndOffset == invokeMemberExpressionAst.Arguments[0].Extent.StartOffset)
						{
							this.ReportError(Parser.LastCharacterOf(invokeMemberExpressionAst.Member.Extent), () => ParserStrings.UnexpectedTokenInDynamicKeyword, functionName.Text);
						}
						else
						{
							this.ReportError(token2.Extent, () => ParserStrings.UnexpectedToken, token2.Text);
						}
						if (token2.Kind == TokenKind.Dot && expressionAst2 != null && token2.Extent.StartOffset == expressionAst2.Extent.EndOffset)
						{
							IScriptExtent extent = Parser.ExtentOf(expressionAst2, token2);
							ErrorExpressionAst member = new ErrorExpressionAst(extent, null);
							MemberExpressionAst memberExpressionAst = new MemberExpressionAst(expressionAst2.Extent, expressionAst2, member, false);
							return new ErrorStatementAst(extent, new MemberExpressionAst[]
							{
								memberExpressionAst
							});
						}
						this.UngetToken(token2);
						if (expressionAst2 != null)
						{
							return new ErrorStatementAst(Parser.ExtentOf(functionName, expressionAst2), Parser.GetNestedErrorAsts(new object[]
							{
								expressionAst2
							}));
						}
						return null;
					}
				}
				ExpressionAst expressionAst3 = null;
				if (keywordData.BodyMode == DynamicKeywordBodyMode.ScriptBlock)
				{
					bool inConfiguration = this._inConfiguration;
					try
					{
						this._inConfiguration = false;
						expressionAst3 = this.ScriptBlockExpressionRule(token2);
						goto IL_685;
					}
					finally
					{
						this._inConfiguration = inConfiguration;
					}
				}
				if (keywordData.BodyMode == DynamicKeywordBodyMode.Hashtable)
				{
					bool flag = string.Compare(functionName.Text, "Script", StringComparison.OrdinalIgnoreCase) == 0;
					try
					{
						if (flag)
						{
							DynamicKeyword.Push();
						}
						expressionAst3 = this.HashExpressionRule(token2, true);
					}
					finally
					{
						if (flag)
						{
							DynamicKeyword.Pop();
						}
					}
				}
				IL_685:
				if (expressionAst3 == null)
				{
					this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.MissingStatementAfterKeyword, keywordData.Keyword);
					if (expressionAst2 != null)
					{
						return new ErrorStatementAst(Parser.ExtentOf(functionName, expressionAst2), Parser.GetNestedErrorAsts(new object[]
						{
							expressionAst2
						}));
					}
					return null;
				}
				else
				{
					Collection<CommandElementAst> commandElements = new Collection<CommandElementAst>
					{
						new StringConstantExpressionAst(functionName.Extent, functionName.Text, StringConstantType.BareWord),
						(ExpressionAst)expressionAst.Copy(),
						(ExpressionAst)expressionAst3.Copy()
					};
					Token token3 = this.NextToken();
					IScriptExtent extent2 = Parser.ExtentOf(functionName, Parser.Before(token3));
					this.UngetToken(token3);
					dynamicKeywordStatementAst = new DynamicKeywordStatementAst(extent2, commandElements)
					{
						Keyword = keywordData,
						LCurly = token2,
						FunctionName = functionName,
						InstanceName = expressionAst,
						OriginalInstanceName = expressionAst2,
						BodyExpression = expressionAst3,
						ElementName = text
					};
				}
			}
			if (keywordData.PostParse != null)
			{
				try
				{
					ParseError[] array3 = keywordData.PostParse(dynamicKeywordStatementAst);
					if (array3 != null && array3.Length > 0)
					{
						foreach (ParseError error2 in array3)
						{
							this.ReportError(error2);
						}
					}
				}
				catch (Exception ex2)
				{
					this.ReportError(functionName.Extent, () => ParserStrings.DynamicKeywordPostParseException, keywordData.Keyword, ex2.ToString());
					return null;
				}
			}
			return dynamicKeywordStatementAst;
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x001473F8 File Offset: 0x001455F8
		internal StatementAst CreateErrorStatementAst(Token functionName, ExpressionAst instanceName, ExpressionAst bodyExpression)
		{
			return new ErrorStatementAst(Parser.ExtentOf(functionName, bodyExpression), Parser.GetNestedErrorAsts(new object[]
			{
				instanceName,
				bodyExpression
			}));
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x00147428 File Offset: 0x00145628
		private StatementAst DoWhileStatementRule(LabelToken labelToken, Token doToken)
		{
			IScriptExtent extent = (labelToken ?? doToken).Extent;
			IScriptExtent scriptExtent = null;
			Token token = null;
			Token token2 = null;
			PipelineBaseAst pipelineBaseAst = null;
			StatementBlockAst statementBlockAst = this.StatementBlockRule();
			if (statementBlockAst == null)
			{
				scriptExtent = doToken.Extent;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingLoopStatement, TokenKind.Do.Text());
			}
			else
			{
				this.SkipNewlines();
				token2 = this.NextToken();
				if (token2.Kind != TokenKind.While && token2.Kind != TokenKind.Until)
				{
					this.UngetToken(token2);
					scriptExtent = statementBlockAst.Extent;
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingWhileOrUntilInDoWhile);
				}
				else
				{
					this.SkipNewlines();
					Token token3 = this.NextToken();
					if (token3.Kind != TokenKind.LParen)
					{
						this.UngetToken(token3);
						scriptExtent = token2.Extent;
						this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingOpenParenthesisAfterKeyword, token2.Kind.Text());
					}
					else
					{
						this.SkipNewlines();
						pipelineBaseAst = this.PipelineRule();
						if (pipelineBaseAst == null)
						{
							scriptExtent = token3.Extent;
							this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingExpressionAfterKeyword, token2.Kind.Text());
						}
						this.SkipNewlines();
						token = this.NextToken();
						if (token.Kind != TokenKind.RParen)
						{
							this.UngetToken(token);
							if (pipelineBaseAst != null)
							{
								scriptExtent = pipelineBaseAst.Extent;
								this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingEndParenthesisAfterStatement, token2.Kind.Text());
							}
						}
					}
				}
			}
			if (scriptExtent != null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(extent, scriptExtent), Parser.GetNestedErrorAsts(new object[]
				{
					statementBlockAst,
					pipelineBaseAst
				}));
			}
			IScriptExtent extent2 = Parser.ExtentOf(extent, token);
			string label = (labelToken != null) ? labelToken.LabelText : null;
			if (token2.Kind == TokenKind.Until)
			{
				return new DoUntilStatementAst(extent2, label, pipelineBaseAst, statementBlockAst);
			}
			return new DoWhileStatementAst(extent2, label, pipelineBaseAst, statementBlockAst);
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00147684 File Offset: 0x00145884
		private StatementAst ClassDefinitionRule(List<AttributeBaseAst> customAttributes, Token classToken)
		{
			this.SkipNewlines();
			Token token;
			StringConstantExpressionAst stringConstantExpressionAst = this.SimpleNameRule(out token);
			if (stringConstantExpressionAst == null)
			{
				this.ReportIncompleteInput(Parser.After(classToken), () => ParserStrings.MissingNameAfterKeyword, classToken.Text);
				return new ErrorStatementAst(classToken.Extent, null);
			}
			token.TokenFlags &= ~TokenFlags.MemberName;
			token.TokenFlags |= TokenFlags.TypeName;
			this.SkipNewlines();
			TokenizerMode mode = this._tokenizer.Mode;
			List<TypeConstraintAst> list = new List<TypeConstraintAst>();
			StatementAst result;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Signature);
				Token token2 = this.PeekToken();
				if (token2.Kind == TokenKind.Colon)
				{
					this.SkipToken();
					this.SkipNewlines();
					Token token3 = null;
					for (;;)
					{
						Token token4;
						ITypeName typeName = this.TypeNameRule(false, out token4);
						if (typeName == null)
						{
							break;
						}
						list.Add(new TypeConstraintAst(typeName.Extent, typeName));
						this.SkipNewlines();
						token3 = this.PeekToken();
						if (token3.Kind != TokenKind.Comma)
						{
							goto IL_150;
						}
						this.SkipToken();
						this.SkipNewlines();
					}
					this.ReportIncompleteInput(Parser.After(Parser.ExtentFromFirstOf(new object[]
					{
						token3,
						token2
					})), () => ParserStrings.TypeNameExpected);
				}
				IL_150:
				Token token5 = this.NextToken();
				if (token5.Kind != TokenKind.LCurly)
				{
					this.UngetToken(token5);
					this.ReportIncompleteInput(Parser.After(stringConstantExpressionAst), () => ParserStrings.MissingTypeBody, classToken.Kind.Text());
					result = new ErrorStatementAst(Parser.ExtentOf(classToken, stringConstantExpressionAst), list);
				}
				else
				{
					IScriptExtent extent = token5.Extent;
					List<Ast> list2 = null;
					List<MemberAst> list3 = new List<MemberAst>();
					List<Ast> list4 = null;
					MemberAst memberAst;
					while ((memberAst = this.ClassMemberRule(stringConstantExpressionAst.Value, out list4)) != null || list4 != null)
					{
						if (memberAst != null)
						{
							list3.Add(memberAst);
							extent = memberAst.Extent;
						}
						if (list4 != null && list4.Count > 0)
						{
							if (list2 == null)
							{
								list2 = new List<Ast>();
							}
							list2.AddRange(list4);
							extent = list4.Last<Ast>().Extent;
						}
					}
					Token token6 = this.NextToken();
					if (token6.Kind != TokenKind.RCurly)
					{
						this.UngetToken(token6);
						this.ReportIncompleteInput(extent, () => ParserStrings.MissingEndCurlyBrace);
					}
					else
					{
						extent = token6.Extent;
					}
					IScriptExtent first = (customAttributes != null && customAttributes.Count > 0) ? customAttributes[0].Extent : classToken.Extent;
					IScriptExtent extent2 = Parser.ExtentOf(first, extent);
					TypeDefinitionAst typeDefinitionAst = new TypeDefinitionAst(extent2, stringConstantExpressionAst.Value, (customAttributes == null) ? null : customAttributes.OfType<AttributeAst>(), list3, TypeAttributes.Class, list);
					if (customAttributes != null && customAttributes.OfType<TypeConstraintAst>().Any<TypeConstraintAst>())
					{
						if (list2 == null)
						{
							list2 = new List<Ast>();
						}
						list2.AddRange(customAttributes.OfType<TypeConstraintAst>());
						list2.Add(typeDefinitionAst);
					}
					if (list2 != null && list2.Count > 0)
					{
						result = new ErrorStatementAst(extent2, list2);
					}
					else
					{
						result = typeDefinitionAst;
					}
				}
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return result;
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x001479D0 File Offset: 0x00145BD0
		private MemberAst ClassMemberRule(string className, out List<Ast> astsOnError)
		{
			IScriptExtent scriptExtent = null;
			List<AttributeAst> list = new List<AttributeAst>();
			TypeConstraintAst typeConstraintAst = null;
			bool flag = true;
			Token token = null;
			Token token2 = null;
			Token token3 = null;
			object obj = null;
			astsOnError = null;
			while (flag)
			{
				this.SkipNewlines();
				AttributeBaseAst attributeBaseAst = this.AttributeRule();
				if (attributeBaseAst != null)
				{
					obj = attributeBaseAst;
					if (scriptExtent == null)
					{
						scriptExtent = attributeBaseAst.Extent;
					}
					AttributeAst attributeAst = attributeBaseAst as AttributeAst;
					if (attributeAst != null)
					{
						list.Add(attributeAst);
					}
					else if (typeConstraintAst == null)
					{
						typeConstraintAst = (TypeConstraintAst)attributeBaseAst;
					}
					else
					{
						this.ReportError(attributeBaseAst.Extent, () => ParserStrings.TooManyTypes);
					}
				}
				else
				{
					token3 = this.PeekToken();
					if (scriptExtent == null)
					{
						scriptExtent = token3.Extent;
					}
					TokenKind kind = token3.Kind;
					if (kind != TokenKind.Static)
					{
						if (kind == TokenKind.Hidden)
						{
							if (token2 != null)
							{
								this.ReportError(token3.Extent, () => ParserStrings.DuplicateQualifier, token3.Text);
							}
							token2 = token3;
							obj = token3;
							this.SkipToken();
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						if (token != null)
						{
							this.ReportError(token3.Extent, () => ParserStrings.DuplicateQualifier, token3.Text);
						}
						token = token3;
						obj = token3;
						this.SkipToken();
					}
				}
			}
			if (token3.Kind == TokenKind.Variable)
			{
				this.SkipToken();
				VariableToken variableToken = token3 as VariableToken;
				ExpressionAst expressionAst = null;
				Token token4 = this.PeekToken();
				if (token4.Kind == TokenKind.Equals)
				{
					this.SkipToken();
					this.SkipNewlines();
					expressionAst = this.ExpressionRule();
				}
				PropertyAttributes propertyAttributes = PropertyAttributes.Public;
				if (token != null)
				{
					propertyAttributes |= PropertyAttributes.Static;
				}
				if (token2 != null)
				{
					propertyAttributes |= PropertyAttributes.Hidden;
				}
				IScriptExtent scriptExtent2 = (expressionAst != null) ? expressionAst.Extent : variableToken.Extent;
				Token token5 = this.PeekToken();
				if (token5.Kind != TokenKind.NewLine && token5.Kind != TokenKind.Semi && token5.Kind != TokenKind.RCurly)
				{
					this.ReportIncompleteInput(Parser.After(scriptExtent2), () => ParserStrings.MissingPropertyTerminator);
				}
				this.SkipNewlinesAndSemicolons();
				if (token5.Kind == TokenKind.Semi)
				{
					scriptExtent2 = token5.Extent;
				}
				if (!string.IsNullOrEmpty(variableToken.Name))
				{
					return new PropertyMemberAst(Parser.ExtentOf(scriptExtent, scriptExtent2), variableToken.Name, typeConstraintAst, list, propertyAttributes, expressionAst);
				}
				this.RecordErrorAsts(list, ref astsOnError);
				this.RecordErrorAsts(typeConstraintAst, ref astsOnError);
				this.RecordErrorAsts(expressionAst, ref astsOnError);
				return null;
			}
			else
			{
				if (token3.Kind != TokenKind.Identifier)
				{
					if (obj != null)
					{
						this.ReportIncompleteInput(Parser.After(Parser.ExtentFromFirstOf(new object[]
						{
							obj
						})), () => ParserStrings.IncompleteMemberDefinition);
						this.RecordErrorAsts(list, ref astsOnError);
						this.RecordErrorAsts(typeConstraintAst, ref astsOnError);
					}
					return null;
				}
				this.SkipToken();
				FunctionDefinitionAst functionDefinitionAst = this.MethodDeclarationRule(token3, className, token != null) as FunctionDefinitionAst;
				if (functionDefinitionAst == null)
				{
					this.SyncOnError(false, new TokenKind[]
					{
						TokenKind.RCurly
					});
					this.RecordErrorAsts(list, ref astsOnError);
					this.RecordErrorAsts(typeConstraintAst, ref astsOnError);
					return null;
				}
				MethodAttributes methodAttributes = MethodAttributes.Public;
				if (token != null)
				{
					methodAttributes |= MethodAttributes.Static;
				}
				if (token2 != null)
				{
					methodAttributes |= MethodAttributes.Hidden;
				}
				return new FunctionMemberAst(Parser.ExtentOf(scriptExtent, functionDefinitionAst), functionDefinitionAst, typeConstraintAst, list, methodAttributes);
			}
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x00147D44 File Offset: 0x00145F44
		private void RecordErrorAsts(Ast errAst, ref List<Ast> astsOnError)
		{
			if (errAst == null)
			{
				return;
			}
			if (astsOnError == null)
			{
				astsOnError = new List<Ast>();
			}
			astsOnError.Add(errAst);
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x00147D5D File Offset: 0x00145F5D
		private void RecordErrorAsts(IEnumerable<Ast> errAsts, ref List<Ast> astsOnError)
		{
			if (errAsts == null || !errAsts.Any<Ast>())
			{
				return;
			}
			if (astsOnError == null)
			{
				astsOnError = new List<Ast>();
			}
			astsOnError.AddRange(errAsts);
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x00147D80 File Offset: 0x00145F80
		private Token NextTypeIdentifierToken()
		{
			TokenizerMode mode = this._tokenizer.Mode;
			Token result;
			try
			{
				this.SetTokenizerMode(TokenizerMode.TypeName);
				Token token = this.NextToken();
				if (token.Kind != TokenKind.Identifier)
				{
					this.UngetToken(token);
					result = null;
				}
				else
				{
					result = token;
				}
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return result;
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x00147DD8 File Offset: 0x00145FD8
		private StatementAst EnumDefinitionRule(List<AttributeBaseAst> customAttributes, Token enumToken)
		{
			this.SkipNewlines();
			StringConstantExpressionAst stringConstantExpressionAst = this.SimpleNameRule();
			if (stringConstantExpressionAst == null)
			{
				this.ReportIncompleteInput(Parser.After(enumToken), () => ParserStrings.MissingNameAfterKeyword, enumToken.Text);
				return new ErrorStatementAst(enumToken.Extent, null);
			}
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.LCurly)
			{
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(stringConstantExpressionAst), () => ParserStrings.MissingTypeBody, enumToken.Kind.Text());
				return new ErrorStatementAst(Parser.ExtentOf(enumToken, stringConstantExpressionAst), null);
			}
			IScriptExtent extent = token.Extent;
			List<MemberAst> list = new List<MemberAst>();
			MemberAst memberAst;
			while ((memberAst = this.EnumMemberRule()) != null)
			{
				list.Add(memberAst);
				extent = memberAst.Extent;
			}
			Token token2 = this.NextToken();
			if (token2.Kind != TokenKind.RCurly)
			{
				this.UngetToken(token2);
				this.ReportIncompleteInput(Parser.After(extent), () => ParserStrings.MissingEndCurlyBrace);
			}
			IScriptExtent scriptExtent = (customAttributes != null && customAttributes.Count > 0) ? customAttributes[0].Extent : enumToken.Extent;
			IScriptExtent extent2 = Parser.ExtentOf(scriptExtent, token2);
			TypeDefinitionAst typeDefinitionAst = new TypeDefinitionAst(extent2, stringConstantExpressionAst.Value, (customAttributes == null) ? null : customAttributes.OfType<AttributeAst>(), list, TypeAttributes.Enum, null);
			if (customAttributes != null && customAttributes.OfType<TypeConstraintAst>().Any<TypeConstraintAst>())
			{
				List<Ast> list2 = new List<Ast>();
				list2.AddRange(customAttributes.OfType<TypeConstraintAst>());
				list2.Add(typeDefinitionAst);
				return new ErrorStatementAst(scriptExtent, list2);
			}
			return typeDefinitionAst;
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x00147F90 File Offset: 0x00146190
		private MemberAst EnumMemberRule()
		{
			this.SkipNewlines();
			StringConstantExpressionAst stringConstantExpressionAst = this.SimpleNameRule();
			if (stringConstantExpressionAst == null)
			{
				return null;
			}
			IScriptExtent extent = stringConstantExpressionAst.Extent;
			ExpressionAst expressionAst = null;
			Token token = this.PeekToken();
			bool flag = false;
			if (token.Kind == TokenKind.Equals)
			{
				this.SkipToken();
				expressionAst = this.ExpressionRule();
				if (expressionAst == null)
				{
					this.ReportError(Parser.After(token), () => ParserStrings.ExpectedValueExpression, token.Kind.Text());
					extent = token.Extent;
					flag = true;
				}
				else
				{
					extent = expressionAst.Extent;
				}
			}
			Token token2 = this.PeekToken();
			if (token2.Kind != TokenKind.NewLine && token2.Kind != TokenKind.Semi && token2.Kind != TokenKind.RCurly && !flag)
			{
				this.ReportIncompleteInput(Parser.After(extent), () => ParserStrings.MissingPropertyTerminator);
			}
			this.SkipNewlinesAndSemicolons();
			if (token2.Kind == TokenKind.Semi)
			{
				extent = token2.Extent;
			}
			return new PropertyMemberAst(Parser.ExtentOf(stringConstantExpressionAst, extent), stringConstantExpressionAst.Value, null, null, PropertyAttributes.Public | PropertyAttributes.Static | PropertyAttributes.Literal, expressionAst);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x001480B0 File Offset: 0x001462B0
		private StatementAst UsingStatementRule(Token usingToken)
		{
			Token token = this.NextToken();
			bool flag = false;
			bool flag2 = false;
			UsingStatementKind usingStatementKind;
			switch (token.Kind)
			{
			case TokenKind.Namespace:
				usingStatementKind = UsingStatementKind.Namespace;
				flag = true;
				break;
			case TokenKind.Module:
				usingStatementKind = UsingStatementKind.Module;
				flag = true;
				break;
			case TokenKind.Type:
				usingStatementKind = UsingStatementKind.Type;
				flag2 = true;
				break;
			case TokenKind.Assembly:
				usingStatementKind = UsingStatementKind.Assembly;
				break;
			case TokenKind.Command:
				usingStatementKind = UsingStatementKind.Command;
				flag2 = true;
				break;
			default:
				this.UngetToken(token);
				this.ReportIncompleteInput(Parser.After(usingToken), () => ParserStrings.MissingUsingStatementDirective);
				return new ErrorStatementAst(usingToken.Extent, null);
			}
			Token token2 = this.NextToken();
			if (token2.Kind == TokenKind.EndOfInput || token2.Kind == TokenKind.NewLine)
			{
				this.UngetToken(token2);
				this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingUsingItemName);
				return new ErrorStatementAst(Parser.ExtentOf(usingToken, token), null);
			}
			ExpressionAst expressionAst = this.GetCommandArgument(Parser.CommandArgumentContext.CommandArgument, token2);
			if (expressionAst == null)
			{
				this.ReportError(token2.Extent, () => ParserStrings.InvalidValueForUsingItemName, token2.Text);
			}
			if (!(expressionAst is StringConstantExpressionAst))
			{
				this.ReportError(Parser.ExtentOf(usingToken, Parser.ExtentFromFirstOf(new object[]
				{
					expressionAst,
					token2
				})), () => ParserStrings.InvalidValueForUsingItemName, expressionAst.Extent.Text);
				return new ErrorStatementAst(Parser.ExtentOf(usingToken, Parser.ExtentFromFirstOf(new object[]
				{
					expressionAst,
					token2
				})), null);
			}
			if (usingStatementKind == UsingStatementKind.Assembly)
			{
				StringConstantExpressionAst stringConstantExpressionAst = (StringConstantExpressionAst)expressionAst;
				string value = stringConstantExpressionAst.Value;
				Uri uri;
				if (Uri.TryCreate(value, UriKind.Absolute, out uri))
				{
					if (uri.IsUnc)
					{
						this.ReportError(stringConstantExpressionAst.Extent, () => ParserStrings.CannotLoadAssemblyFromUncPath, value);
					}
					if (uri.Scheme != "file")
					{
						this.ReportError(stringConstantExpressionAst.Extent, () => ParserStrings.CannotLoadAssemblyWithUriSchema, uri.Scheme);
					}
				}
				else
				{
					string text = value;
					try
					{
						string file = expressionAst.Extent.File;
						if (!Path.IsPathRooted(text))
						{
							string str;
							if (string.IsNullOrEmpty(file))
							{
								if (Runspace.DefaultRunspace != null)
								{
									str = Runspace.DefaultRunspace.ExecutionContext.EngineIntrinsics.SessionState.Path.CurrentLocation.Path;
								}
								else
								{
									str = Directory.GetCurrentDirectory();
								}
							}
							else
							{
								str = Path.GetDirectoryName(file);
							}
							text = str + "\\" + text;
						}
						if (!File.Exists(text))
						{
							GlobalAssemblyCache.ResolvePartialName(value, out text, null, null);
						}
					}
					catch
					{
					}
					if (text == null || !File.Exists(text))
					{
						this.ReportError(stringConstantExpressionAst.Extent, () => ParserStrings.ErrorLoadingAssembly, value);
					}
					else
					{
						expressionAst = new StringConstantExpressionAst(stringConstantExpressionAst.Extent, text, stringConstantExpressionAst.StringConstantType);
					}
				}
			}
			if (flag || flag2)
			{
				Token token3 = this.PeekToken();
				if (token3.Kind == TokenKind.Equals)
				{
					this.SkipToken();
					Token token4 = this.NextToken();
					if (token4.Kind == TokenKind.EndOfInput)
					{
						this.UngetToken(token4);
						this.ReportIncompleteInput(Parser.After(token3), () => ParserStrings.MissingNamespaceAlias);
						return new ErrorStatementAst(Parser.ExtentOf(usingToken, token3), null);
					}
					ExpressionAst commandArgument = this.GetCommandArgument(Parser.CommandArgumentContext.CommandArgument, token4);
					if (!(commandArgument is StringConstantExpressionAst))
					{
						return new ErrorStatementAst(Parser.ExtentOf(usingToken, commandArgument), new Ast[]
						{
							expressionAst,
							commandArgument
						});
					}
					this.RequireStatementTerminator();
					return new UsingStatementAst(Parser.ExtentOf(usingToken, token4), usingStatementKind, (StringConstantExpressionAst)expressionAst, (StringConstantExpressionAst)commandArgument);
				}
				else if (flag2)
				{
					this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.MissingEqualsInUsingAlias);
					return new ErrorStatementAst(Parser.ExtentOf(usingToken, expressionAst), new Ast[]
					{
						expressionAst
					});
				}
			}
			this.RequireStatementTerminator();
			return new UsingStatementAst(Parser.ExtentOf(usingToken, expressionAst), usingStatementKind, (StringConstantExpressionAst)expressionAst);
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0014855C File Offset: 0x0014675C
		private StatementAst MethodDeclarationRule(Token functionNameToken, string className, bool isStaticMethod)
		{
			string text = functionNameToken.Text;
			Token token = this.PeekToken();
			IScriptExtent scriptExtent = null;
			Token token2 = null;
			List<ParameterAst> list;
			if (token.Kind == TokenKind.LParen)
			{
				list = this.FunctionParameterDeclarationRule(out scriptExtent, out token2);
			}
			else
			{
				this.ReportIncompleteInput(Parser.After(functionNameToken), () => ParserStrings.MissingMethodParameterList);
				list = new List<ParameterAst>();
			}
			bool flag = text.Equals(className, StringComparison.OrdinalIgnoreCase);
			List<ExpressionAst> list2 = null;
			Token token3 = null;
			IScriptExtent last = null;
			TokenizerMode mode;
			if (flag && !isStaticMethod)
			{
				this.SkipNewlines();
				mode = this._tokenizer.Mode;
				try
				{
					this.SetTokenizerMode(TokenizerMode.Signature);
					Token token4 = this.PeekToken();
					if (token4.Kind == TokenKind.Colon)
					{
						this.SkipToken();
						this.SkipNewlines();
						token3 = this.PeekToken();
						if (token3.Kind == TokenKind.Base)
						{
							this.SkipToken();
							this.SkipNewlines();
							token = this.PeekToken();
							if (token.Kind == TokenKind.LParen)
							{
								this.SkipToken();
								list2 = this.InvokeParamParenListRule(token, out last);
								this.SkipNewlines();
							}
							else
							{
								scriptExtent = token3.Extent;
								this.ReportIncompleteInput(Parser.After(token3), () => ParserStrings.MissingMethodParameterList);
							}
						}
						else
						{
							scriptExtent = token4.Extent;
							this.ReportIncompleteInput(Parser.After(token4), () => ParserStrings.MissingBaseCtorCall);
						}
					}
				}
				finally
				{
					this.SetTokenizerMode(mode);
				}
				if (list2 == null)
				{
					list2 = new List<ExpressionAst>();
				}
			}
			Token token5 = this.NextToken();
			if (token5.Kind != TokenKind.LCurly)
			{
				this.UngetToken(token5);
				if (scriptExtent == null)
				{
					scriptExtent = Parser.ExtentFromFirstOf(new object[]
					{
						token2,
						functionNameToken
					});
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingFunctionBody);
				}
			}
			if (scriptExtent != null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(functionNameToken, scriptExtent), list);
			}
			StatementAst predefinedStatementAst = null;
			if (flag && !isStaticMethod)
			{
				IScriptExtent baseCallExtent;
				IScriptExtent baseKeywordExtent;
				if (token3 != null)
				{
					baseCallExtent = Parser.ExtentOf(token3, last);
					baseKeywordExtent = token3.Extent;
				}
				else
				{
					baseCallExtent = PositionUtilities.EmptyExtent;
					baseKeywordExtent = PositionUtilities.EmptyExtent;
				}
				BaseCtorInvokeMemberExpressionAst baseCtorInvokeMemberExpressionAst = new BaseCtorInvokeMemberExpressionAst(baseKeywordExtent, baseCallExtent, list2);
				predefinedStatementAst = new CommandExpressionAst(baseCtorInvokeMemberExpressionAst.Extent, baseCtorInvokeMemberExpressionAst, null);
			}
			mode = this._tokenizer.Mode;
			StatementAst result;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Command);
				ScriptBlockAst scriptBlockAst = this.ScriptBlockRule(token5, false, predefinedStatementAst);
				FunctionDefinitionAst functionDefinitionAst = new FunctionDefinitionAst(Parser.ExtentOf(functionNameToken, scriptBlockAst), false, false, functionNameToken, list, scriptBlockAst);
				result = functionDefinitionAst;
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return result;
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0014881C File Offset: 0x00146A1C
		private StatementAst FunctionDeclarationRule(Token functionToken)
		{
			this.SkipNewlines();
			Token token = this.NextToken();
			TokenKind kind = token.Kind;
			switch (kind)
			{
			case TokenKind.Variable:
			case TokenKind.SplattedVariable:
			case TokenKind.EndOfInput:
			case TokenKind.StringLiteral:
			case TokenKind.StringExpandable:
			case TokenKind.HereStringLiteral:
			case TokenKind.HereStringExpandable:
			case TokenKind.LParen:
			case TokenKind.RParen:
			case TokenKind.LCurly:
			case TokenKind.RCurly:
			case TokenKind.AtParen:
			case TokenKind.AtCurly:
			case TokenKind.Semi:
			case TokenKind.AndAnd:
			case TokenKind.OrOr:
			case TokenKind.Ampersand:
			case TokenKind.Pipe:
				break;
			case TokenKind.Parameter:
			case TokenKind.Number:
			case TokenKind.Label:
			case TokenKind.Identifier:
			case TokenKind.Generic:
			case TokenKind.NewLine:
			case TokenKind.LineContinuation:
			case TokenKind.Comment:
			case TokenKind.LBracket:
			case TokenKind.RBracket:
			case TokenKind.DollarParen:
				goto IL_ED;
			default:
				switch (kind)
				{
				case TokenKind.Redirection:
				case TokenKind.RedirectInStd:
					break;
				default:
					goto IL_ED;
				}
				break;
			}
			this.UngetToken(token);
			this.ReportIncompleteInput(Parser.After(functionToken), () => ParserStrings.MissingNameAfterKeyword, functionToken.Text);
			return new ErrorStatementAst(functionToken.Extent, null);
			IL_ED:
			token.TokenFlags &= ~TokenFlags.Keyword;
			IScriptExtent scriptExtent;
			Token token2;
			List<ParameterAst> list = this.FunctionParameterDeclarationRule(out scriptExtent, out token2);
			Token token3 = this.NextToken();
			if (token3.Kind != TokenKind.LCurly)
			{
				this.UngetToken(token3);
				if (scriptExtent == null)
				{
					scriptExtent = Parser.ExtentFromFirstOf(new object[]
					{
						token2,
						token
					});
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingFunctionBody);
				}
			}
			if (scriptExtent != null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(functionToken, scriptExtent), list);
			}
			bool isFilter = functionToken.Kind == TokenKind.Filter;
			bool flag = functionToken.Kind == TokenKind.Workflow;
			bool inWorkflowContext = this._tokenizer.InWorkflowContext;
			StatementAst result;
			try
			{
				this._tokenizer.InWorkflowContext = flag;
				ScriptBlockAst scriptBlockAst = this.ScriptBlockRule(token3, isFilter);
				if (token.Kind != TokenKind.Generic)
				{
					string text = token.Text;
				}
				else
				{
					string value = ((StringToken)token).Value;
				}
				FunctionDefinitionAst functionDefinitionAst = new FunctionDefinitionAst(Parser.ExtentOf(functionToken, scriptBlockAst), isFilter, flag, token, list, scriptBlockAst);
				result = functionDefinitionAst;
			}
			finally
			{
				this._tokenizer.InWorkflowContext = inWorkflowContext;
			}
			return result;
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00148A48 File Offset: 0x00146C48
		private List<ParameterAst> FunctionParameterDeclarationRule(out IScriptExtent endErrorStatement, out Token rParen)
		{
			List<ParameterAst> list = null;
			endErrorStatement = null;
			this.SkipNewlines();
			rParen = null;
			Token token = this.PeekToken();
			if (token.Kind == TokenKind.LParen)
			{
				this.SkipToken();
				list = this.ParameterListRule();
				this.SkipNewlines();
				rParen = this.NextToken();
				if (rParen.Kind != TokenKind.RParen)
				{
					this.UngetToken(rParen);
					endErrorStatement = (list.Any<ParameterAst>() ? list.Last<ParameterAst>().Extent : token.Extent);
					this.ReportIncompleteInput(Parser.After(endErrorStatement), () => ParserStrings.MissingEndParenthesisInFunctionParameterList);
				}
				this.SkipNewlines();
			}
			return list;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x00148AF4 File Offset: 0x00146CF4
		private StatementAst TrapStatementRule(Token trapToken)
		{
			int restorePoint = this._tokenizer.GetRestorePoint();
			this.SkipNewlines();
			AttributeBaseAst attributeBaseAst = this.AttributeRule();
			TypeConstraintAst typeConstraintAst = attributeBaseAst as TypeConstraintAst;
			if (attributeBaseAst != null && typeConstraintAst == null)
			{
				this.Resync(restorePoint);
			}
			StatementBlockAst statementBlockAst = this.StatementBlockRule();
			if (statementBlockAst == null)
			{
				IScriptExtent scriptExtent = Parser.ExtentFromFirstOf(new object[]
				{
					typeConstraintAst,
					trapToken
				});
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingTrapStatement);
				return new ErrorStatementAst(Parser.ExtentOf(trapToken, scriptExtent), Parser.GetNestedErrorAsts(new object[]
				{
					typeConstraintAst
				}));
			}
			return new TrapStatementAst(Parser.ExtentOf(trapToken, statementBlockAst), typeConstraintAst, statementBlockAst);
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x00148BB4 File Offset: 0x00146DB4
		private CatchClauseAst CatchBlockRule(ref IScriptExtent endErrorStatement, ref List<TypeConstraintAst> errorAsts)
		{
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.Catch)
			{
				this.UngetToken(token);
				return null;
			}
			List<TypeConstraintAst> list = null;
			Token token2 = null;
			int restorePoint;
			for (;;)
			{
				restorePoint = this._tokenizer.GetRestorePoint();
				this.SkipNewlines();
				AttributeBaseAst attributeBaseAst = this.AttributeRule();
				if (attributeBaseAst == null)
				{
					break;
				}
				TypeConstraintAst typeConstraintAst = attributeBaseAst as TypeConstraintAst;
				if (typeConstraintAst == null)
				{
					goto Block_4;
				}
				if (list == null)
				{
					list = new List<TypeConstraintAst>();
				}
				list.Add(typeConstraintAst);
				this.SkipNewlines();
				token2 = this.PeekToken();
				if (token2.Kind != TokenKind.Comma)
				{
					goto IL_C9;
				}
				this.SkipToken();
			}
			if (token2 != null)
			{
				endErrorStatement = token2.Extent;
				this.ReportIncompleteInput(Parser.After(endErrorStatement), () => ParserStrings.MissingTypeLiteralToken);
				goto IL_C9;
			}
			goto IL_C9;
			Block_4:
			this.Resync(restorePoint);
			IL_C9:
			StatementBlockAst statementBlockAst = this.StatementBlockRule();
			if (statementBlockAst == null)
			{
				if (token2 == null || endErrorStatement != token2.Extent)
				{
					endErrorStatement = ((list != null) ? list.Last<TypeConstraintAst>().Extent : token.Extent);
					this.ReportIncompleteInput(Parser.After(endErrorStatement), () => ParserStrings.MissingCatchHandlerBlock);
				}
				if (list != null)
				{
					if (errorAsts == null)
					{
						errorAsts = list;
					}
					else
					{
						errorAsts.Concat(list);
					}
				}
				return null;
			}
			return new CatchClauseAst(Parser.ExtentOf(token, statementBlockAst), list, statementBlockAst);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00148D10 File Offset: 0x00146F10
		private StatementAst TryStatementRule(Token tryToken)
		{
			this.SkipNewlines();
			StatementBlockAst statementBlockAst = this.StatementBlockRule();
			if (statementBlockAst == null)
			{
				this.ReportIncompleteInput(Parser.After(tryToken), () => ParserStrings.MissingTryStatementBlock);
				return new ErrorStatementAst(tryToken.Extent, null);
			}
			IScriptExtent scriptExtent = null;
			List<CatchClauseAst> list = new List<CatchClauseAst>();
			List<TypeConstraintAst> list2 = null;
			CatchClauseAst item;
			while ((item = this.CatchBlockRule(ref scriptExtent, ref list2)) != null)
			{
				list.Add(item);
			}
			this.SkipNewlines();
			Token token = this.PeekToken();
			StatementBlockAst statementBlockAst2 = null;
			if (token.Kind == TokenKind.Finally)
			{
				this.SkipToken();
				statementBlockAst2 = this.StatementBlockRule();
				if (statementBlockAst2 == null)
				{
					scriptExtent = token.Extent;
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingFinallyStatementBlock, token.Kind.Text());
				}
			}
			if (list.Count == 0 && statementBlockAst2 == null && scriptExtent == null)
			{
				scriptExtent = statementBlockAst.Extent;
				this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingCatchOrFinally);
			}
			if (scriptExtent != null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(tryToken, scriptExtent), Parser.GetNestedErrorAsts(new object[]
				{
					statementBlockAst,
					list,
					list2
				}));
			}
			return new TryStatementAst(Parser.ExtentOf(tryToken, (statementBlockAst2 != null) ? statementBlockAst2.Extent : list.Last<CatchClauseAst>().Extent), statementBlockAst, list, statementBlockAst2);
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x00148E94 File Offset: 0x00147094
		private StatementAst DataStatementRule(Token dataToken)
		{
			IScriptExtent scriptExtent = null;
			this.SkipNewlines();
			StringConstantExpressionAst stringConstantExpressionAst = this.SimpleNameRule();
			string variableName = (stringConstantExpressionAst != null) ? stringConstantExpressionAst.Value : null;
			this.SkipNewlines();
			Token token = this.PeekToken();
			List<ExpressionAst> list = null;
			if (token.Kind == TokenKind.Parameter)
			{
				this.SkipToken();
				if (!Parser.IsSpecificParameter(token, "SupportedCommand"))
				{
					scriptExtent = token.Extent;
					this.ReportError(scriptExtent, () => ParserStrings.InvalidParameterForDataSectionStatement, ((ParameterToken)token).ParameterName);
				}
				Token token2 = null;
				list = new List<ExpressionAst>();
				for (;;)
				{
					this.SkipNewlines();
					ExpressionAst singleCommandArgument = this.GetSingleCommandArgument(Parser.CommandArgumentContext.CommandName);
					if (singleCommandArgument == null)
					{
						break;
					}
					list.Add(singleCommandArgument);
					token2 = this.PeekToken();
					if (token2.Kind != TokenKind.Comma)
					{
						goto IL_114;
					}
					this.SkipToken();
				}
				if (scriptExtent == null)
				{
					this.ReportIncompleteInput(Parser.After(token2 ?? token), () => ParserStrings.MissingValueForSupportedCommandInDataSectionStatement);
				}
				scriptExtent = ((token2 != null) ? token2.Extent : token.Extent);
			}
			IL_114:
			StatementBlockAst statementBlockAst = null;
			if (scriptExtent == null)
			{
				statementBlockAst = this.StatementBlockRule();
				if (statementBlockAst == null)
				{
					scriptExtent = ((list != null) ? list.Last<ExpressionAst>().Extent : Parser.ExtentFromFirstOf(new object[]
					{
						stringConstantExpressionAst,
						dataToken
					}));
					this.ReportIncompleteInput(Parser.After(scriptExtent), () => ParserStrings.MissingStatementBlockForDataSection);
				}
			}
			if (scriptExtent != null)
			{
				return new ErrorStatementAst(Parser.ExtentOf(dataToken, scriptExtent), Parser.GetNestedErrorAsts(new object[]
				{
					list
				}));
			}
			return new DataStatementAst(Parser.ExtentOf(dataToken, statementBlockAst), variableName, list, statementBlockAst);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x00149060 File Offset: 0x00147260
		private PipelineBaseAst PipelineRule()
		{
			List<CommandBaseAst> list = new List<CommandBaseAst>();
			IScriptExtent scriptExtent = null;
			Token token = null;
			bool flag = true;
			while (flag)
			{
				Token token2 = null;
				TokenizerMode mode = this._tokenizer.Mode;
				ExpressionAst expressionAst;
				try
				{
					this.SetTokenizerMode(TokenizerMode.Expression);
					expressionAst = this.ExpressionRule();
					if (expressionAst != null)
					{
						Token token3 = this.PeekToken();
						if (token3.Kind.HasTrait(TokenFlags.AssignmentOperator))
						{
							this.SkipToken();
							token2 = token3;
						}
					}
				}
				finally
				{
					this.SetTokenizerMode(mode);
				}
				CommandBaseAst commandBaseAst;
				if (expressionAst != null)
				{
					if (list.Count > 0)
					{
						this.ReportError(expressionAst.Extent, () => ParserStrings.ExpressionsMustBeFirstInPipeline);
					}
					if (token2 != null)
					{
						this.SkipNewlines();
						StatementAst statementAst = this.StatementRule();
						if (statementAst == null)
						{
							IScriptExtent extent = Parser.After(token2);
							this.ReportIncompleteInput(extent, () => ParserStrings.ExpectedValueExpression, token2.Kind.Text());
							statementAst = new ErrorStatementAst(extent, null);
						}
						return new AssignmentStatementAst(Parser.ExtentOf(expressionAst, statementAst), expressionAst, token2.Kind, statementAst, token2.Extent);
					}
					RedirectionAst[] array = null;
					RedirectionToken redirectionToken = this.PeekToken() as RedirectionToken;
					RedirectionAst redirectionAst = null;
					while (redirectionToken != null)
					{
						this.SkipToken();
						if (array == null)
						{
							array = new RedirectionAst[7];
						}
						IScriptExtent scriptExtent2 = null;
						redirectionAst = this.RedirectionRule(redirectionToken, array, ref scriptExtent2);
						redirectionToken = (this.PeekToken() as RedirectionToken);
					}
					IScriptExtent scriptExtent3 = (redirectionAst != null) ? Parser.ExtentOf(expressionAst, redirectionAst) : expressionAst.Extent;
					IScriptExtent extent2 = scriptExtent3;
					ExpressionAst expression = expressionAst;
					IEnumerable<RedirectionAst> redirections;
					if (array == null)
					{
						redirections = null;
					}
					else
					{
						redirections = from r in array
						where r != null
						select r;
					}
					commandBaseAst = new CommandExpressionAst(extent2, expression, redirections);
				}
				else
				{
					commandBaseAst = (CommandAst)this.CommandRule(false);
				}
				if (commandBaseAst != null)
				{
					if (scriptExtent == null)
					{
						scriptExtent = commandBaseAst.Extent;
					}
					list.Add(commandBaseAst);
				}
				else if (list.Count > 0 || this.PeekToken().Kind == TokenKind.Pipe)
				{
					IScriptExtent extent3 = (token != null) ? Parser.After(token) : this.PeekToken().Extent;
					this.ReportIncompleteInput(extent3, () => ParserStrings.EmptyPipeElement);
				}
				token = this.PeekToken();
				TokenKind kind = token.Kind;
				if (kind <= TokenKind.EndOfInput)
				{
					if (kind != TokenKind.NewLine && kind != TokenKind.EndOfInput)
					{
						goto IL_341;
					}
				}
				else
				{
					switch (kind)
					{
					case TokenKind.RParen:
					case TokenKind.RCurly:
						break;
					case TokenKind.LCurly:
						goto IL_341;
					default:
						switch (kind)
						{
						case TokenKind.Semi:
							break;
						case TokenKind.AndAnd:
						case TokenKind.OrOr:
							this.SkipToken();
							this.SkipNewlines();
							this.ReportError(token.Extent, () => ParserStrings.InvalidEndOfLine, token.Text);
							if (this.PeekToken().Kind == TokenKind.EndOfInput)
							{
								flag = false;
								continue;
							}
							continue;
						case TokenKind.Ampersand:
							goto IL_341;
						case TokenKind.Pipe:
							this.SkipToken();
							this.SkipNewlines();
							if (this.PeekToken().Kind == TokenKind.EndOfInput)
							{
								flag = false;
								this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.EmptyPipeElement);
								continue;
							}
							continue;
						default:
							goto IL_341;
						}
						break;
					}
				}
				flag = false;
				continue;
				IL_341:
				this.ReportError(token.Extent, () => ParserStrings.UnexpectedToken, token.Text);
				flag = false;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return new PipelineAst(Parser.ExtentOf(scriptExtent, list[list.Count - 1]), list);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0014941C File Offset: 0x0014761C
		private RedirectionAst RedirectionRule(RedirectionToken redirectionToken, RedirectionAst[] redirections, ref IScriptExtent extent)
		{
			FileRedirectionToken fileRedirectionToken = redirectionToken as FileRedirectionToken;
			RedirectionAst redirectionAst;
			if (fileRedirectionToken != null || redirectionToken is InputRedirectionToken)
			{
				ExpressionAst expressionAst = this.GetSingleCommandArgument(Parser.CommandArgumentContext.FileName);
				if (expressionAst == null)
				{
					this.ReportError(Parser.After(redirectionToken), () => ParserStrings.MissingFileSpecification);
					expressionAst = new ErrorExpressionAst(redirectionToken.Extent, null);
				}
				if (fileRedirectionToken == null)
				{
					this.ReportError(redirectionToken.Extent, () => ParserStrings.RedirectionNotSupported, redirectionToken.Text);
					extent = Parser.ExtentOf(redirectionToken, expressionAst);
					return null;
				}
				redirectionAst = new FileRedirectionAst(Parser.ExtentOf(fileRedirectionToken, expressionAst), fileRedirectionToken.FromStream, expressionAst, fileRedirectionToken.Append);
			}
			else
			{
				MergingRedirectionToken mergingRedirectionToken = (MergingRedirectionToken)redirectionToken;
				RedirectionStream fromStream = mergingRedirectionToken.FromStream;
				RedirectionStream redirectionStream = mergingRedirectionToken.ToStream;
				if (redirectionStream != RedirectionStream.Output)
				{
					this.ReportError(redirectionToken.Extent, () => ParserStrings.RedirectionNotSupported, mergingRedirectionToken.Text);
					redirectionStream = RedirectionStream.Output;
				}
				else if (fromStream == redirectionStream)
				{
					this.ReportError(redirectionToken.Extent, () => ParserStrings.RedirectionNotSupported, mergingRedirectionToken.Text);
				}
				redirectionAst = new MergingRedirectionAst(mergingRedirectionToken.Extent, mergingRedirectionToken.FromStream, redirectionStream);
			}
			if (redirections[(int)redirectionAst.FromStream] == null)
			{
				redirections[(int)redirectionAst.FromStream] = redirectionAst;
			}
			else
			{
				string arg;
				switch (redirectionAst.FromStream)
				{
				case RedirectionStream.All:
					arg = ParserStrings.AllStream;
					break;
				case RedirectionStream.Output:
					arg = ParserStrings.OutputStream;
					break;
				case RedirectionStream.Error:
					arg = ParserStrings.ErrorStream;
					break;
				case RedirectionStream.Warning:
					arg = ParserStrings.WarningStream;
					break;
				case RedirectionStream.Verbose:
					arg = ParserStrings.VerboseStream;
					break;
				case RedirectionStream.Debug:
					arg = ParserStrings.DebugStream;
					break;
				case RedirectionStream.Information:
					arg = ParserStrings.InformationStream;
					break;
				default:
					throw PSTraceSource.NewArgumentOutOfRangeException("result.FromStream", redirectionAst.FromStream);
				}
				this.ReportError(redirectionAst.Extent, () => ParserStrings.StreamAlreadyRedirected, arg);
			}
			extent = redirectionAst.Extent;
			return redirectionAst;
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00149654 File Offset: 0x00147854
		private ExpressionAst GetSingleCommandArgument(Parser.CommandArgumentContext context)
		{
			if (this.PeekToken().Kind == TokenKind.Comma || this.PeekToken().Kind == TokenKind.EndOfInput)
			{
				return null;
			}
			TokenizerMode mode = this._tokenizer.Mode;
			ExpressionAst commandArgument;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Command);
				commandArgument = this.GetCommandArgument(context, this.NextToken());
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return commandArgument;
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x001496C0 File Offset: 0x001478C0
		private ExpressionAst GetCommandArgument(Parser.CommandArgumentContext context, Token token)
		{
			List<ExpressionAst> list = null;
			Token token2 = null;
			bool flag = false;
			ExpressionAst expressionAst;
			for (;;)
			{
				switch (token.Kind)
				{
				case TokenKind.Variable:
				case TokenKind.SplattedVariable:
				case TokenKind.Number:
				case TokenKind.StringLiteral:
				case TokenKind.StringExpandable:
				case TokenKind.HereStringLiteral:
				case TokenKind.HereStringExpandable:
				case TokenKind.LParen:
				case TokenKind.LCurly:
				case TokenKind.AtParen:
				case TokenKind.AtCurly:
				case TokenKind.DollarParen:
					this.UngetToken(token);
					expressionAst = this.PrimaryExpressionRule(true);
					break;
				case TokenKind.Parameter:
				case TokenKind.Label:
				case TokenKind.Identifier:
				case TokenKind.LineContinuation:
				case TokenKind.Comment:
				case TokenKind.LBracket:
				case TokenKind.RBracket:
				case TokenKind.PlusPlus:
				case TokenKind.DotDot:
				case TokenKind.ColonColon:
				case TokenKind.Dot:
				case TokenKind.Exclaim:
				case TokenKind.Multiply:
				case TokenKind.Divide:
				case TokenKind.Rem:
				case TokenKind.Plus:
				case TokenKind.Minus:
				case TokenKind.Equals:
				case TokenKind.PlusEquals:
				case TokenKind.MinusEquals:
				case TokenKind.MultiplyEquals:
				case TokenKind.DivideEquals:
				case TokenKind.RemainderEquals:
					goto IL_1C9;
				case TokenKind.Generic:
				{
					if ((context & Parser.CommandArgumentContext.CommandName) != (Parser.CommandArgumentContext)0)
					{
						token.TokenFlags |= TokenFlags.CommandName;
					}
					StringToken stringToken = (StringToken)token;
					StringExpandableToken stringExpandableToken = stringToken as StringExpandableToken;
					if (stringExpandableToken != null && context != Parser.CommandArgumentContext.CommandName)
					{
						List<ExpressionAst> nestedExpressions = this.ParseNestedExpressions(stringExpandableToken);
						expressionAst = new ExpandableStringExpressionAst(stringExpandableToken, stringExpandableToken.Value, stringExpandableToken.FormatString, nestedExpressions);
					}
					else
					{
						expressionAst = new StringConstantExpressionAst(stringToken.Extent, stringToken.Value, StringConstantType.BareWord);
						if (string.Equals(stringToken.Value, "--%", StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
						}
					}
					break;
				}
				case TokenKind.NewLine:
				case TokenKind.EndOfInput:
				case TokenKind.RParen:
				case TokenKind.RCurly:
				case TokenKind.Semi:
				case TokenKind.AndAnd:
				case TokenKind.OrOr:
				case TokenKind.Ampersand:
				case TokenKind.Pipe:
				case TokenKind.Comma:
				case TokenKind.MinusMinus:
				case TokenKind.Redirection:
				case TokenKind.RedirectInStd:
					goto IL_E0;
				default:
					goto IL_1C9;
				}
				IL_22E:
				if (context != Parser.CommandArgumentContext.CommandArgument || flag)
				{
					goto IL_272;
				}
				token = this.PeekToken();
				if (token.Kind == TokenKind.Comma)
				{
					token2 = token;
					if (list == null)
					{
						list = new List<ExpressionAst>();
					}
					list.Add(expressionAst);
					this.SkipToken();
					this.SkipNewlines();
					token = this.NextToken();
					continue;
				}
				goto IL_272;
				IL_1C9:
				expressionAst = new StringConstantExpressionAst(token.Extent, token.Text, StringConstantType.BareWord);
				token.TokenFlags &= ~TokenFlags.Keyword;
				switch (context)
				{
				case Parser.CommandArgumentContext.CommandName:
				case Parser.CommandArgumentContext.CommandNameAfterInvocationOperator:
					token.TokenFlags |= TokenFlags.CommandName;
					goto IL_22E;
				case (Parser.CommandArgumentContext)2:
					goto IL_22E;
				case Parser.CommandArgumentContext.FileName:
					break;
				default:
					if (context != Parser.CommandArgumentContext.CommandArgument && context != Parser.CommandArgumentContext.SwitchCondition)
					{
						goto IL_22E;
					}
					break;
				}
				token.SetIsCommandArgument();
				goto IL_22E;
			}
			IL_E0:
			this.UngetToken(token);
			if (token2 == null)
			{
				return null;
			}
			this.ReportIncompleteInput(Parser.After(token2), () => ParserStrings.MissingExpression, ",");
			return new ErrorExpressionAst(Parser.ExtentOf(list.First<ExpressionAst>(), token2), list);
			IL_272:
			if (list != null)
			{
				list.Add(expressionAst);
				return new ArrayLiteralAst(Parser.ExtentOf(list[0], list[list.Count - 1]), list);
			}
			return expressionAst;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x00149974 File Offset: 0x00147B74
		internal Ast CommandRule(bool forDynamicKeyword)
		{
			bool flag = false;
			bool flag2 = false;
			RedirectionAst[] array = null;
			List<CommandElementAst> list = new List<CommandElementAst>();
			TokenizerMode mode = this._tokenizer.Mode;
			Token token2;
			IScriptExtent last;
			bool flag3;
			bool flag4;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Command);
				Token token = this.NextToken();
				token2 = token;
				last = token.Extent;
				flag3 = false;
				flag4 = false;
				Parser.CommandArgumentContext commandArgumentContext;
				if (token.Kind == TokenKind.Dot)
				{
					flag3 = true;
					token = this.NextToken();
					commandArgumentContext = Parser.CommandArgumentContext.CommandNameAfterInvocationOperator;
				}
				else if (token.Kind == TokenKind.Ampersand)
				{
					flag4 = true;
					token = this.NextToken();
					commandArgumentContext = Parser.CommandArgumentContext.CommandNameAfterInvocationOperator;
				}
				else
				{
					commandArgumentContext = Parser.CommandArgumentContext.CommandName;
				}
				bool flag5 = true;
				while (flag5)
				{
					TokenKind kind = token.Kind;
					if (kind <= TokenKind.NewLine)
					{
						if (kind != TokenKind.Parameter)
						{
							if (kind != TokenKind.NewLine)
							{
								goto IL_324;
							}
						}
						else
						{
							if ((commandArgumentContext & Parser.CommandArgumentContext.CommandName) != (Parser.CommandArgumentContext)0 || flag)
							{
								last = token.Extent;
								token.TokenFlags |= TokenFlags.CommandName;
								StringConstantExpressionAst item = new StringConstantExpressionAst(token.Extent, token.Text, StringConstantType.BareWord);
								list.Add(item);
								goto IL_3EA;
							}
							ParameterToken parameterToken = (ParameterToken)token;
							ExpressionAst expressionAst;
							IScriptExtent scriptExtent;
							if (parameterToken.UsedColon && this.PeekToken().Kind != TokenKind.Comma)
							{
								expressionAst = this.GetCommandArgument(Parser.CommandArgumentContext.CommandArgument, this.NextToken());
								if (expressionAst == null)
								{
									scriptExtent = parameterToken.Extent;
									this.ReportError(Parser.After(scriptExtent), () => ParserStrings.ParameterRequiresArgument, parameterToken.Text);
								}
								else
								{
									scriptExtent = Parser.ExtentOf(token, expressionAst);
								}
							}
							else
							{
								expressionAst = null;
								scriptExtent = token.Extent;
							}
							last = scriptExtent;
							CommandParameterAst item2 = new CommandParameterAst(scriptExtent, parameterToken.ParameterName, expressionAst, token.Extent);
							list.Add(item2);
							goto IL_3EA;
						}
					}
					else if (kind != TokenKind.EndOfInput)
					{
						switch (kind)
						{
						case TokenKind.RParen:
						case TokenKind.RCurly:
						case TokenKind.Semi:
						case TokenKind.AndAnd:
						case TokenKind.OrOr:
						case TokenKind.Pipe:
							break;
						case TokenKind.LCurly:
						case TokenKind.LBracket:
						case TokenKind.RBracket:
						case TokenKind.AtParen:
						case TokenKind.AtCurly:
						case TokenKind.DollarParen:
							goto IL_324;
						case TokenKind.Ampersand:
							last = token.Extent;
							this.ReportError(token.Extent, () => ParserStrings.AmpersandNotAllowed);
							goto IL_3EA;
						case TokenKind.Comma:
							last = token.Extent;
							this.ReportError(token.Extent, () => ParserStrings.MissingArgument);
							this.SkipNewlines();
							goto IL_3EA;
						case TokenKind.MinusMinus:
							last = token.Extent;
							list.Add(flag ? new StringConstantExpressionAst(token.Extent, token.Text, StringConstantType.BareWord) : new CommandParameterAst(token.Extent, "-", null, token.Extent));
							flag = true;
							goto IL_3EA;
						default:
							switch (kind)
							{
							case TokenKind.Redirection:
							case TokenKind.RedirectInStd:
								if ((commandArgumentContext & Parser.CommandArgumentContext.CommandName) == (Parser.CommandArgumentContext)0)
								{
									if (array == null)
									{
										array = new RedirectionAst[7];
									}
									this.RedirectionRule((RedirectionToken)token, array, ref last);
									goto IL_3EA;
								}
								last = token.Extent;
								list.Add(new StringConstantExpressionAst(token.Extent, token.Text, StringConstantType.BareWord));
								goto IL_3EA;
							default:
								goto IL_324;
							}
							break;
						}
					}
					this.UngetToken(token);
					flag5 = false;
					continue;
					IL_324:
					if (token.Kind == TokenKind.InlineScript && commandArgumentContext == Parser.CommandArgumentContext.CommandName)
					{
						flag5 = this.InlineScriptRule(token, list);
						last = list.Last<CommandElementAst>().Extent;
						if (!flag5)
						{
							continue;
						}
					}
					else
					{
						ExpressionAst expressionAst2 = this.GetCommandArgument(commandArgumentContext, token);
						StringToken stringToken = token as StringToken;
						if (stringToken != null && string.Equals(stringToken.Value, "--%", StringComparison.OrdinalIgnoreCase))
						{
							list.Add(expressionAst2);
							last = expressionAst2.Extent;
							StringToken verbatimCommandArgumentToken = this.GetVerbatimCommandArgumentToken();
							if (verbatimCommandArgumentToken != null)
							{
								flag2 = true;
								flag5 = false;
								expressionAst2 = new StringConstantExpressionAst(verbatimCommandArgumentToken.Extent, verbatimCommandArgumentToken.Value, StringConstantType.BareWord);
								list.Add(expressionAst2);
								last = expressionAst2.Extent;
							}
						}
						else
						{
							last = expressionAst2.Extent;
							list.Add(expressionAst2);
						}
					}
					IL_3EA:
					if (!flag2)
					{
						commandArgumentContext = Parser.CommandArgumentContext.CommandArgument;
						token = this.NextToken();
					}
				}
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			if (list.Count == 0)
			{
				if (flag3 || flag4)
				{
					IScriptExtent extent = token2.Extent;
					this.ReportError(extent, () => ParserStrings.MissingExpression, token2.Text);
				}
				return null;
			}
			if (forDynamicKeyword)
			{
				return new DynamicKeywordStatementAst(Parser.ExtentOf(token2, last), list);
			}
			IScriptExtent extent2 = Parser.ExtentOf(token2, last);
			IEnumerable<CommandElementAst> commandElements = list;
			TokenKind invocationOperator = (flag3 || flag4) ? token2.Kind : TokenKind.Unknown;
			IEnumerable<RedirectionAst> redirections;
			if (array == null)
			{
				redirections = null;
			}
			else
			{
				redirections = from r in array
				where r != null
				select r;
			}
			return new CommandAst(extent2, commandElements, invocationOperator, redirections);
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00149E4C File Offset: 0x0014804C
		private ExpressionAst ExpressionRule()
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			TokenizerMode mode = this._tokenizer.Mode;
			ExpressionAst result;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Expression);
				ExpressionAst expressionAst = this.ArrayLiteralRule();
				if (expressionAst == null)
				{
					result = null;
				}
				else
				{
					Token token = this.PeekToken();
					if (!token.Kind.HasTrait(TokenFlags.BinaryOperator))
					{
						ParameterToken parameterToken = token as ParameterToken;
						if (parameterToken != null)
						{
							result = this.ErrorRecoveryParameterInExpression(parameterToken, expressionAst);
						}
						else
						{
							result = expressionAst;
						}
					}
					else
					{
						this.SkipToken();
						Stack<ExpressionAst> stack = new Stack<ExpressionAst>();
						Stack<Token> stack2 = new Stack<Token>();
						stack.Push(expressionAst);
						stack2.Push(token);
						int num = token.Kind.GetBinaryPrecedence();
						ExpressionAst expressionAst2;
						for (;;)
						{
							this.SkipNewlines();
							expressionAst = this.ArrayLiteralRule();
							if (expressionAst == null)
							{
								IScriptExtent extent = Parser.After(token);
								this.ReportIncompleteInput(extent, () => ParserStrings.ExpectedValueExpression, token.Text);
								expressionAst = new ErrorExpressionAst(extent, null);
							}
							stack.Push(expressionAst);
							token = this.NextToken();
							if (!token.Kind.HasTrait(TokenFlags.BinaryOperator))
							{
								break;
							}
							int i = token.Kind.GetBinaryPrecedence();
							while (i <= num)
							{
								expressionAst2 = stack.Pop();
								ExpressionAst expressionAst3 = stack.Pop();
								Token token2 = stack2.Pop();
								stack.Push(new BinaryExpressionAst(Parser.ExtentOf(expressionAst3, expressionAst2), expressionAst3, token2.Kind, expressionAst2, token2.Extent));
								if (stack2.Count == 0)
								{
									break;
								}
								num = stack2.Peek().Kind.GetBinaryPrecedence();
							}
							stack2.Push(token);
							num = i;
						}
						ParameterToken parameterToken = token as ParameterToken;
						this.UngetToken(token);
						expressionAst2 = stack.Pop();
						while (stack.Count > 0)
						{
							ExpressionAst expressionAst3 = stack.Pop();
							token = stack2.Pop();
							expressionAst2 = new BinaryExpressionAst(Parser.ExtentOf(expressionAst3, expressionAst2), expressionAst3, token.Kind, expressionAst2, token.Extent);
						}
						if (parameterToken != null)
						{
							result = this.ErrorRecoveryParameterInExpression(parameterToken, expressionAst2);
						}
						else
						{
							result = expressionAst2;
						}
					}
				}
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return result;
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x0014A080 File Offset: 0x00148280
		private ExpressionAst ErrorRecoveryParameterInExpression(ParameterToken paramToken, ExpressionAst expr)
		{
			this.ReportError(paramToken.Extent, () => ParserStrings.UnexpectedToken, paramToken.Text);
			this.SkipToken();
			return new ErrorExpressionAst(Parser.ExtentOf(expr, paramToken), new Ast[]
			{
				expr,
				new CommandParameterAst(paramToken.Extent, paramToken.ParameterName, null, paramToken.Extent)
			});
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0014A0F8 File Offset: 0x001482F8
		private ExpressionAst ArrayLiteralRule()
		{
			ExpressionAst expressionAst = this.UnaryExpressionRule();
			ExpressionAst first = expressionAst;
			Token token = this.PeekToken();
			if (token.Kind != TokenKind.Comma || this._disableCommaOperator)
			{
				return expressionAst;
			}
			List<ExpressionAst> list = new List<ExpressionAst>
			{
				expressionAst
			};
			while (token.Kind == TokenKind.Comma)
			{
				this.SkipToken();
				this.SkipNewlines();
				expressionAst = this.UnaryExpressionRule();
				if (expressionAst == null)
				{
					this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingExpressionAfterToken, token.Text);
					expressionAst = new ErrorExpressionAst(token.Extent, null);
					list.Add(expressionAst);
					break;
				}
				list.Add(expressionAst);
				token = this.PeekToken();
			}
			return new ArrayLiteralAst(Parser.ExtentOf(first, expressionAst), list);
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0014A1C4 File Offset: 0x001483C4
		private ExpressionAst UnaryExpressionRule()
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			ExpressionAst expressionAst = null;
			bool allowSignedNumbers = this._tokenizer.AllowSignedNumbers;
			Token token;
			try
			{
				this._tokenizer.AllowSignedNumbers = true;
				if (this._ungotToken != null && this._ungotToken.Kind == TokenKind.Minus)
				{
					this.Resync(this._ungotToken);
				}
				token = this.PeekToken();
			}
			finally
			{
				this._tokenizer.AllowSignedNumbers = allowSignedNumbers;
			}
			if (token.Kind.HasTrait(TokenFlags.UnaryOperator))
			{
				if (this._disableCommaOperator && token.Kind == TokenKind.Comma)
				{
					return null;
				}
				this.SkipToken();
				this.SkipNewlines();
				ExpressionAst expressionAst2 = this.UnaryExpressionRule();
				if (expressionAst2 == null)
				{
					this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingExpressionAfterOperator, token.Text);
					return new ErrorExpressionAst(token.Extent, null);
				}
				if (token.Kind == TokenKind.Comma)
				{
					expressionAst = new ArrayLiteralAst(Parser.ExtentOf(token, expressionAst2), new ExpressionAst[]
					{
						expressionAst2
					});
				}
				else
				{
					expressionAst = new UnaryExpressionAst(Parser.ExtentOf(token, expressionAst2), token.Kind, expressionAst2);
				}
			}
			else if (token.Kind == TokenKind.LBracket)
			{
				List<AttributeBaseAst> list = this.AttributeListRule(true);
				if (list == null)
				{
					return null;
				}
				AttributeBaseAst attributeBaseAst = list.Last<AttributeBaseAst>();
				if (attributeBaseAst is AttributeAst)
				{
					this.SkipNewlines();
					ExpressionAst expressionAst2 = this.UnaryExpressionRule();
					if (expressionAst2 == null)
					{
						this.ReportIncompleteInput(attributeBaseAst.Extent, () => ParserStrings.UnexpectedAttribute, attributeBaseAst.TypeName.FullName);
						return new ErrorExpressionAst(Parser.ExtentOf(token, attributeBaseAst), null);
					}
					expressionAst = new AttributedExpressionAst(Parser.ExtentOf(attributeBaseAst, expressionAst2), attributeBaseAst, expressionAst2);
				}
				else
				{
					Token token2 = (this._ungotToken != null) ? null : this.NextMemberAccessToken(false);
					if (token2 != null)
					{
						expressionAst = this.CheckPostPrimaryExpressionOperators(token2, new TypeExpressionAst(attributeBaseAst.Extent, attributeBaseAst.TypeName));
					}
					else
					{
						token = this.PeekToken();
						if (token.Kind != TokenKind.NewLine && token.Kind != TokenKind.Comma)
						{
							ExpressionAst expressionAst2 = this.UnaryExpressionRule();
							if (expressionAst2 != null)
							{
								expressionAst = new ConvertExpressionAst(Parser.ExtentOf(attributeBaseAst, expressionAst2), (TypeConstraintAst)attributeBaseAst, expressionAst2);
							}
						}
					}
					if (expressionAst == null)
					{
						expressionAst = new TypeExpressionAst(attributeBaseAst.Extent, attributeBaseAst.TypeName);
					}
				}
				for (int i = list.Count - 2; i >= 0; i--)
				{
					TypeConstraintAst typeConstraintAst = list[i] as TypeConstraintAst;
					expressionAst = ((typeConstraintAst != null) ? new ConvertExpressionAst(Parser.ExtentOf(typeConstraintAst, expressionAst), typeConstraintAst, expressionAst) : new AttributedExpressionAst(Parser.ExtentOf(list[i], expressionAst), list[i], expressionAst));
				}
			}
			else
			{
				expressionAst = this.PrimaryExpressionRule(true);
			}
			if (expressionAst != null)
			{
				token = this.PeekToken();
				TokenKind tokenKind = (token.Kind == TokenKind.PlusPlus) ? TokenKind.PostfixPlusPlus : ((token.Kind == TokenKind.MinusMinus) ? TokenKind.PostfixMinusMinus : TokenKind.Unknown);
				if (tokenKind != TokenKind.Unknown)
				{
					this.SkipToken();
					expressionAst = new UnaryExpressionAst(Parser.ExtentOf(expressionAst, token), tokenKind, expressionAst);
				}
			}
			return expressionAst;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x0014A4CC File Offset: 0x001486CC
		private ExpressionAst PrimaryExpressionRule(bool withMemberAccess)
		{
			Token token = this.NextToken();
			TokenKind kind = token.Kind;
			ExpressionAst expressionAst;
			switch (kind)
			{
			case TokenKind.Variable:
			case TokenKind.SplattedVariable:
				expressionAst = this.CheckUsingVariable((VariableToken)token, withMemberAccess);
				goto IL_D2;
			case TokenKind.Parameter:
				break;
			case TokenKind.Number:
				expressionAst = new ConstantExpressionAst((NumberToken)token);
				goto IL_D2;
			default:
				switch (kind)
				{
				case TokenKind.StringLiteral:
				case TokenKind.HereStringLiteral:
					expressionAst = new StringConstantExpressionAst((StringToken)token);
					goto IL_D2;
				case TokenKind.StringExpandable:
				case TokenKind.HereStringExpandable:
					expressionAst = this.ExpandableStringRule((StringExpandableToken)token);
					goto IL_D2;
				case TokenKind.LParen:
					expressionAst = this.ParenthesizedExpressionRule(token);
					goto IL_D2;
				case TokenKind.LCurly:
					expressionAst = this.ScriptBlockExpressionRule(token);
					goto IL_D2;
				case TokenKind.AtParen:
				case TokenKind.DollarParen:
					expressionAst = this.SubExpressionRule(token);
					goto IL_D2;
				case TokenKind.AtCurly:
					expressionAst = this.HashExpressionRule(token, false);
					goto IL_D2;
				}
				break;
			}
			this.UngetToken(token);
			return null;
			IL_D2:
			if (!withMemberAccess)
			{
				return expressionAst;
			}
			return this.CheckPostPrimaryExpressionOperators(this.NextMemberAccessToken(true), expressionAst);
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x0014A5C0 File Offset: 0x001487C0
		private ExpressionAst CheckUsingVariable(VariableToken variableToken, bool withMemberAccess)
		{
			VariablePath variablePath = variableToken.VariablePath;
			if (variablePath.IsDriveQualified && variablePath.DriveName.Equals("using", StringComparison.OrdinalIgnoreCase) && variablePath.UnqualifiedPath.Length > 0)
			{
				VariablePath variablePath2 = new VariablePath(variablePath.UnqualifiedPath);
				ExpressionAst expressionAst = new VariableExpressionAst(variableToken.Extent, variablePath2, variableToken.Kind == TokenKind.SplattedVariable);
				if (withMemberAccess)
				{
					expressionAst = this.CheckPostPrimaryExpressionOperators(this.NextMemberAccessToken(true), expressionAst);
				}
				return new UsingExpressionAst(expressionAst.Extent, expressionAst);
			}
			return new VariableExpressionAst(variableToken);
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x0014A644 File Offset: 0x00148844
		private ExpressionAst CheckPostPrimaryExpressionOperators(Token token, ExpressionAst expr)
		{
			while (token != null)
			{
				this.V3SkipNewlines();
				if (token.Kind == TokenKind.Dot || token.Kind == TokenKind.ColonColon)
				{
					expr = this.MemberAccessRule(expr, token);
				}
				else if (token.Kind == TokenKind.LBracket)
				{
					expr = this.ElementAccessRule(expr, token);
				}
				token = this.NextMemberAccessToken(true);
			}
			return expr;
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x0014A69C File Offset: 0x0014889C
		private ExpressionAst HashExpressionRule(Token atCurlyToken, bool parsingSchemaElement)
		{
			this.SkipNewlines();
			List<Tuple<ExpressionAst, StatementAst>> list = new List<Tuple<ExpressionAst, StatementAst>>();
			for (;;)
			{
				Tuple<ExpressionAst, StatementAst> keyValuePair = this.GetKeyValuePair(parsingSchemaElement);
				if (keyValuePair == null)
				{
					break;
				}
				list.Add(keyValuePair);
				Token token = this.PeekToken();
				if (token.Kind != TokenKind.NewLine && token.Kind != TokenKind.Semi)
				{
					break;
				}
				this.SkipNewlinesAndSemicolons();
			}
			Token token2 = this.NextToken();
			IScriptExtent last;
			if (token2.Kind != TokenKind.RCurly)
			{
				this.UngetToken(token2);
				Expression<Func<string>> errorExpr = parsingSchemaElement ? (() => ParserStrings.IncompletePropertyAssignmentBlock) : (() => ParserStrings.IncompleteHashLiteral);
				this.ReportIncompleteInput(list.Any<Tuple<ExpressionAst, StatementAst>>() ? Parser.After(list.Last<Tuple<ExpressionAst, StatementAst>>().Item2) : Parser.After(atCurlyToken), token2.Extent, errorExpr, new object[0]);
				last = Parser.Before(token2);
			}
			else
			{
				last = token2.Extent;
			}
			return new HashtableAst(Parser.ExtentOf(atCurlyToken, last), list)
			{
				IsSchemaElement = parsingSchemaElement
			};
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0014A7B0 File Offset: 0x001489B0
		private Tuple<ExpressionAst, StatementAst> GetKeyValuePair(bool parsingSchemaElement)
		{
			TokenizerMode mode = this._tokenizer.Mode;
			ExpressionAst expressionAst;
			Token token;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Expression);
				expressionAst = this.LabelOrKeyRule();
				if (expressionAst == null)
				{
					return null;
				}
				token = this.NextToken();
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			if (token.Kind != TokenKind.Equals)
			{
				this.UngetToken(token);
				IScriptExtent extent = Parser.After(expressionAst);
				Expression<Func<string>> errorExpr = parsingSchemaElement ? (() => ParserStrings.MissingEqualsInPropertyAssignmentBlock) : (() => ParserStrings.MissingEqualsInHashLiteral);
				this.ReportError(extent, errorExpr);
				this.SyncOnError(true, new TokenKind[]
				{
					TokenKind.RCurly,
					TokenKind.Semi,
					TokenKind.NewLine
				});
				return new Tuple<ExpressionAst, StatementAst>(expressionAst, new ErrorStatementAst(extent, null));
			}
			StatementAst statementAst;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Command);
				this.SkipNewlines();
				statementAst = this.StatementRule();
				if (statementAst == null)
				{
					IScriptExtent extent2 = Parser.After(token);
					Expression<Func<string>> errorExpr2 = parsingSchemaElement ? (() => ParserStrings.MissingEqualsInPropertyAssignmentBlock) : (() => ParserStrings.MissingStatementInHashLiteral);
					this.ReportIncompleteInput(extent2, errorExpr2);
					statementAst = new ErrorStatementAst(extent2, null);
				}
			}
			finally
			{
				this.SetTokenizerMode(mode);
			}
			return new Tuple<ExpressionAst, StatementAst>(expressionAst, statementAst);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x0014A93C File Offset: 0x00148B3C
		private ExpressionAst ScriptBlockExpressionRule(Token lCurly)
		{
			bool disableCommaOperator = this._disableCommaOperator;
			TokenizerMode mode = this._tokenizer.Mode;
			ScriptBlockAst scriptBlockAst;
			try
			{
				this._disableCommaOperator = false;
				this.SetTokenizerMode(TokenizerMode.Command);
				this.SkipNewlines();
				scriptBlockAst = this.ScriptBlockRule(lCurly, false);
			}
			finally
			{
				this._disableCommaOperator = disableCommaOperator;
				this.SetTokenizerMode(mode);
			}
			return new ScriptBlockExpressionAst(scriptBlockAst.Extent, scriptBlockAst);
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x0014A9A8 File Offset: 0x00148BA8
		private ExpressionAst SubExpressionRule(Token firstToken)
		{
			List<TrapStatementAst> traps = new List<TrapStatementAst>();
			List<StatementAst> statements = new List<StatementAst>();
			bool disableCommaOperator = this._disableCommaOperator;
			TokenizerMode mode = this._tokenizer.Mode;
			IScriptExtent scriptExtent;
			Token token;
			try
			{
				this._disableCommaOperator = false;
				this.SetTokenizerMode(TokenizerMode.Command);
				this.SkipNewlines();
				scriptExtent = this.StatementListRule(statements, traps);
				this.SkipNewlines();
				token = this.NextToken();
				if (token.Kind != TokenKind.RParen)
				{
					this.UngetToken(token);
					this.ReportIncompleteInput(token.Extent, () => ParserStrings.MissingEndParenthesisInSubexpression);
				}
			}
			finally
			{
				this._disableCommaOperator = disableCommaOperator;
				this.SetTokenizerMode(mode);
			}
			IScriptExtent extent = Parser.ExtentOf(firstToken, (token.Kind == TokenKind.RParen) ? token.Extent : (scriptExtent ?? firstToken.Extent));
			if (firstToken.Kind == TokenKind.DollarParen)
			{
				return new SubExpressionAst(extent, new StatementBlockAst(scriptExtent ?? PositionUtilities.EmptyExtent, statements, traps));
			}
			return new ArrayExpressionAst(extent, new StatementBlockAst(scriptExtent ?? PositionUtilities.EmptyExtent, statements, traps));
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x0014AAC4 File Offset: 0x00148CC4
		private ExpressionAst ParenthesizedExpressionRule(Token lParen)
		{
			TokenizerMode mode = this._tokenizer.Mode;
			bool disableCommaOperator = this._disableCommaOperator;
			PipelineBaseAst pipelineBaseAst;
			Token token;
			try
			{
				this.SetTokenizerMode(TokenizerMode.Command);
				this._disableCommaOperator = false;
				this.SkipNewlines();
				pipelineBaseAst = this.PipelineRule();
				if (pipelineBaseAst == null)
				{
					IScriptExtent extent = Parser.After(lParen);
					this.ReportIncompleteInput(extent, () => ParserStrings.ExpectedExpression);
					pipelineBaseAst = new ErrorStatementAst(extent, null);
				}
				this.SkipNewlines();
				token = this.NextToken();
				if (token.Kind != TokenKind.RParen)
				{
					this.UngetToken(token);
					this.ReportIncompleteInput(Parser.After(pipelineBaseAst), () => ParserStrings.MissingEndParenthesisInExpression);
					token = null;
				}
			}
			finally
			{
				this._disableCommaOperator = disableCommaOperator;
				this.SetTokenizerMode(mode);
			}
			return new ParenExpressionAst(Parser.ExtentOf(lParen, Parser.ExtentFromFirstOf(new object[]
			{
				token,
				pipelineBaseAst
			})), pipelineBaseAst);
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x0014ABD0 File Offset: 0x00148DD0
		private List<ExpressionAst> ParseNestedExpressions(StringExpandableToken expandableStringToken)
		{
			List<ExpressionAst> list = new List<ExpressionAst>();
			List<Token> list2 = this._savingTokens ? new List<Token>() : null;
			foreach (Token token in expandableStringToken.NestedTokens)
			{
				VariableToken variableToken = token as VariableToken;
				ExpressionAst item;
				if (variableToken != null)
				{
					item = this.CheckUsingVariable(variableToken, false);
					if (this._savingTokens)
					{
						list2.Add(variableToken);
					}
				}
				else
				{
					TokenizerState ts = null;
					try
					{
						ts = this._tokenizer.StartNestedScan((UnscannedSubExprToken)token);
						item = this.PrimaryExpressionRule(true);
						if (this._savingTokens)
						{
							list2.AddRange(this._tokenizer.TokenList);
						}
					}
					finally
					{
						this._ungotToken = null;
						this._tokenizer.FinishNestedScan(ts);
					}
				}
				list.Add(item);
			}
			if (this._savingTokens)
			{
				expandableStringToken.NestedTokens = new ReadOnlyCollection<Token>(list2);
			}
			return list;
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x0014ACD8 File Offset: 0x00148ED8
		private ExpressionAst ExpandableStringRule(StringExpandableToken strToken)
		{
			ExpressionAst result;
			if (strToken.NestedTokens != null)
			{
				List<ExpressionAst> nestedExpressions = this.ParseNestedExpressions(strToken);
				result = new ExpandableStringExpressionAst(strToken, strToken.Value, strToken.FormatString, nestedExpressions);
			}
			else
			{
				result = new StringConstantExpressionAst(strToken);
			}
			return result;
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x0014AD14 File Offset: 0x00148F14
		private ExpressionAst MemberNameRule()
		{
			ExpressionAst expressionAst = this.SimpleNameRule();
			if (expressionAst != null)
			{
				return expressionAst;
			}
			Token token = this.PeekToken();
			if (token.Kind.HasTrait(TokenFlags.UnaryOperator) || token.Kind == TokenKind.LBracket)
			{
				return this.UnaryExpressionRule();
			}
			return this.PrimaryExpressionRule(false);
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x0014AD60 File Offset: 0x00148F60
		private ExpressionAst MemberAccessRule(ExpressionAst targetExpr, Token operatorToken)
		{
			CommandElementAst commandElementAst = this.MemberNameRule();
			if (commandElementAst == null)
			{
				this.ReportIncompleteInput(Parser.After(operatorToken), () => ParserStrings.MissingPropertyName);
				commandElementAst = (this.GetSingleCommandArgument(Parser.CommandArgumentContext.CommandArgument) ?? new ErrorExpressionAst(Parser.ExtentOf(targetExpr, operatorToken), null));
			}
			else
			{
				Token token = this.NextInvokeMemberToken();
				if (token != null)
				{
					return this.MemberInvokeRule(targetExpr, token, operatorToken, commandElementAst);
				}
			}
			return new MemberExpressionAst(Parser.ExtentOf(targetExpr, commandElementAst), targetExpr, commandElementAst, operatorToken.Kind == TokenKind.ColonColon);
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x0014ADEC File Offset: 0x00148FEC
		private ExpressionAst MemberInvokeRule(ExpressionAst targetExpr, Token lBracket, Token operatorToken, CommandElementAst member)
		{
			IScriptExtent last = null;
			List<ExpressionAst> list;
			if (lBracket.Kind == TokenKind.LParen)
			{
				list = this.InvokeParamParenListRule(lBracket, out last);
			}
			else
			{
				list = new List<ExpressionAst>();
				this.SkipNewlines();
				ExpressionAst expressionAst = this.ScriptBlockExpressionRule(lBracket);
				list.Add(expressionAst);
				last = expressionAst.Extent;
			}
			return new InvokeMemberExpressionAst(Parser.ExtentOf(targetExpr, last), targetExpr, member, list, operatorToken.Kind == TokenKind.ColonColon);
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x0014AE50 File Offset: 0x00149050
		private List<ExpressionAst> InvokeParamParenListRule(Token lParen, out IScriptExtent lastExtent)
		{
			List<ExpressionAst> list = new List<ExpressionAst>();
			Token token = null;
			Token token2 = null;
			bool disableCommaOperator = this._disableCommaOperator;
			bool flag = false;
			try
			{
				this._disableCommaOperator = true;
				for (;;)
				{
					this.SkipNewlines();
					ExpressionAst expressionAst = this.ExpressionRule();
					if (expressionAst == null)
					{
						break;
					}
					list.Add(expressionAst);
					this.SkipNewlines();
					token = this.NextToken();
					if (token.Kind != TokenKind.Comma)
					{
						goto Block_5;
					}
				}
				if (token != null)
				{
					this.ReportIncompleteInput(Parser.After(token), () => ParserStrings.MissingExpressionAfterToken, TokenKind.Comma.Text());
					flag = true;
					goto IL_91;
				}
				goto IL_91;
				Block_5:
				this.UngetToken(token);
				token = null;
				IL_91:
				this.SkipNewlines();
				token2 = this.NextToken();
				if (token2.Kind != TokenKind.RParen)
				{
					this.UngetToken(token2);
					if (!flag)
					{
						this.ReportIncompleteInput(list.Any<ExpressionAst>() ? Parser.After(list.Last<ExpressionAst>()) : Parser.After(lParen), () => ParserStrings.MissingEndParenthesisInMethodCall);
					}
					token2 = null;
				}
			}
			finally
			{
				this._disableCommaOperator = disableCommaOperator;
			}
			lastExtent = Parser.ExtentFromFirstOf(new object[]
			{
				token2,
				token,
				list.LastOrDefault<ExpressionAst>(),
				lParen
			});
			return list;
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x0014AF9C File Offset: 0x0014919C
		private ExpressionAst ElementAccessRule(ExpressionAst primaryExpression, Token lBracket)
		{
			this.SkipNewlines();
			ExpressionAst expressionAst = this.ExpressionRule();
			if (expressionAst == null)
			{
				IScriptExtent extent = Parser.After(lBracket);
				this.ReportIncompleteInput(extent, () => ParserStrings.MissingArrayIndexExpression);
				expressionAst = new ErrorExpressionAst(lBracket.Extent, null);
			}
			this.SkipNewlines();
			Token token = this.NextToken();
			if (token.Kind != TokenKind.RBracket)
			{
				this.UngetToken(token);
				if (!(expressionAst is ErrorExpressionAst))
				{
					this.ReportIncompleteInput(Parser.After(expressionAst), () => ParserStrings.MissingEndSquareBracket);
				}
				token = null;
			}
			return new IndexExpressionAst(Parser.ExtentOf(primaryExpression, Parser.ExtentFromFirstOf(new object[]
			{
				token,
				expressionAst
			})), primaryExpression, expressionAst);
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0014B110 File Offset: 0x00149310
		private void SaveError(ParseError error)
		{
			if (this._errorList.Any<ParseError>())
			{
				if (this._errorList.Any((ParseError err) => err.ErrorId.Equals(error.ErrorId, StringComparison.Ordinal) && err.Extent.EndColumnNumber == error.Extent.EndColumnNumber && err.Extent.EndLineNumber == error.Extent.EndLineNumber && err.Extent.StartColumnNumber == error.Extent.StartColumnNumber && err.Extent.StartLineNumber == error.Extent.StartLineNumber))
				{
					return;
				}
			}
			this._errorList.Add(error);
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0014B16C File Offset: 0x0014936C
		private void SaveError(IScriptExtent extent, Expression<Func<string>> errorExpr, bool incompleteInput, params object[] args)
		{
			string text = null;
			string errorId = null;
			MemberExpression memberExpression = errorExpr.Body as MemberExpression;
			if (memberExpression != null)
			{
				PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
				if (propertyInfo != null)
				{
					MethodInfo getMethod = propertyInfo.GetMethod;
					if (getMethod != null && getMethod.IsStatic && getMethod.ReturnType == typeof(string))
					{
						text = (string)getMethod.Invoke(null, null);
						errorId = propertyInfo.Name;
					}
				}
			}
			if (text == null)
			{
				text = errorExpr.Compile()();
				errorId = "ParserError";
			}
			if (args != null && args.Any<object>())
			{
				text = string.Format(CultureInfo.CurrentCulture, text, args);
			}
			ParseError error = new ParseError(extent, errorId, text, incompleteInput);
			this.SaveError(error);
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x0014B22D File Offset: 0x0014942D
		private static object[] arrayOfOneArg
		{
			get
			{
				object[] result;
				if ((result = Parser._arrayOfOneArg) == null)
				{
					result = (Parser._arrayOfOneArg = new object[1]);
				}
				return result;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x0014B244 File Offset: 0x00149444
		private static object[] arrayOfTwoArgs
		{
			get
			{
				object[] result;
				if ((result = Parser._arrayOfTwoArgs) == null)
				{
					result = (Parser._arrayOfTwoArgs = new object[2]);
				}
				return result;
			}
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0014B25C File Offset: 0x0014945C
		internal bool ReportIncompleteInput(IScriptExtent extent, Expression<Func<string>> errorExpr)
		{
			bool flag = this._tokenizer.IsAtEndOfScript(extent, true);
			this.SaveError(extent, errorExpr, flag, null);
			return flag;
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x0014B284 File Offset: 0x00149484
		internal bool ReportIncompleteInput(IScriptExtent extent, Expression<Func<string>> errorExpr, object arg)
		{
			bool flag = this._tokenizer.IsAtEndOfScript(extent, true);
			Parser.arrayOfOneArg[0] = arg;
			this.SaveError(extent, errorExpr, flag, Parser.arrayOfOneArg);
			return flag;
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0014B2B8 File Offset: 0x001494B8
		internal bool ReportIncompleteInput(IScriptExtent errorPosition, IScriptExtent errorDetectedPosition, Expression<Func<string>> errorExpr, params object[] args)
		{
			bool flag = this._tokenizer.IsAtEndOfScript(errorDetectedPosition, true);
			this.SaveError(errorPosition, errorExpr, flag, args);
			return flag;
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x0014B2DF File Offset: 0x001494DF
		internal void ReportError(IScriptExtent extent, Expression<Func<string>> errorExpr)
		{
			this.SaveError(extent, errorExpr, false, null);
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x0014B2EB File Offset: 0x001494EB
		internal void ReportError(IScriptExtent extent, Expression<Func<string>> errorExpr, object arg)
		{
			Parser.arrayOfOneArg[0] = arg;
			this.SaveError(extent, errorExpr, false, Parser.arrayOfOneArg);
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x0014B303 File Offset: 0x00149503
		internal void ReportError(IScriptExtent extent, Expression<Func<string>> errorExpr, object arg1, object arg2)
		{
			Parser.arrayOfTwoArgs[0] = arg1;
			Parser.arrayOfTwoArgs[1] = arg2;
			this.SaveError(extent, errorExpr, false, Parser.arrayOfTwoArgs);
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0014B324 File Offset: 0x00149524
		internal void ReportError(IScriptExtent extent, Expression<Func<string>> errorExpr, params object[] args)
		{
			this.SaveError(extent, errorExpr, false, args);
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x0014B330 File Offset: 0x00149530
		internal void ReportError(ParseError error)
		{
			this.SaveError(error);
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x0014B33C File Offset: 0x0014953C
		private void ReportErrorsAsWarnings(Collection<Exception> errors)
		{
			ExecutionContext executionContext = Runspace.DefaultRunspace.ExecutionContext;
			if (executionContext != null && executionContext.InternalHost != null && executionContext.InternalHost.UI != null)
			{
				foreach (Exception ex in errors)
				{
					if (ex != null)
					{
						executionContext.InternalHost.UI.WriteWarningLine(ex.ToString());
					}
				}
			}
		}

		// Token: 0x04001F12 RID: 7954
		internal const string VERBATIM_ARGUMENT = "--%";

		// Token: 0x04001F13 RID: 7955
		internal const string VERBATIM_PARAMETERNAME = "-%";

		// Token: 0x04001F14 RID: 7956
		private readonly Tokenizer _tokenizer;

		// Token: 0x04001F15 RID: 7957
		private readonly List<ParseError> _errorList;

		// Token: 0x04001F16 RID: 7958
		internal Token _ungotToken;

		// Token: 0x04001F17 RID: 7959
		private bool _disableCommaOperator;

		// Token: 0x04001F18 RID: 7960
		private bool _savingTokens;

		// Token: 0x04001F19 RID: 7961
		private bool _inConfiguration;

		// Token: 0x04001F1A RID: 7962
		internal string _fileName;

		// Token: 0x04001F1B RID: 7963
		private string _keywordModuleName;

		// Token: 0x04001F1C RID: 7964
		private string _previousFirstTokenText;

		// Token: 0x04001F1D RID: 7965
		private string _previousLastTokenText;

		// Token: 0x04001F1E RID: 7966
		private Dictionary<string, DynamicKeyword> _configurationKeywordsDefinedInThisFile;

		// Token: 0x04001F1F RID: 7967
		[ThreadStatic]
		private static object[] _arrayOfOneArg;

		// Token: 0x04001F20 RID: 7968
		[ThreadStatic]
		private static object[] _arrayOfTwoArgs;

		// Token: 0x020005B2 RID: 1458
		[Flags]
		private enum CommandArgumentContext
		{
			// Token: 0x04001F27 RID: 7975
			CommandName = 1,
			// Token: 0x04001F28 RID: 7976
			CommandNameAfterInvocationOperator = 3,
			// Token: 0x04001F29 RID: 7977
			FileName = 4,
			// Token: 0x04001F2A RID: 7978
			CommandArgument = 8,
			// Token: 0x04001F2B RID: 7979
			SwitchCondition = 16
		}
	}
}
