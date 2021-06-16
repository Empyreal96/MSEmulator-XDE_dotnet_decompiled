using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.PowerShell.DesiredStateConfiguration.Internal;

namespace System.Management.Automation
{
	// Token: 0x0200097A RID: 2426
	internal class CompletionAnalysis
	{
		// Token: 0x06005925 RID: 22821 RVA: 0x001D323B File Offset: 0x001D143B
		internal CompletionAnalysis(Ast ast, Token[] tokens, IScriptPosition cursorPosition, Hashtable options)
		{
			this._ast = ast;
			this._tokens = tokens;
			this._cursorPosition = cursorPosition;
			this._options = options;
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x001D3260 File Offset: 0x001D1460
		private static bool IsInterestingToken(Token token)
		{
			return token.Kind != TokenKind.NewLine && token.Kind != TokenKind.EndOfInput;
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x001D327A File Offset: 0x001D147A
		private static bool IsCursorWithinOrJustAfterExtent(IScriptPosition cursor, IScriptExtent extent)
		{
			return cursor.Offset > extent.StartOffset && cursor.Offset <= extent.EndOffset;
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x001D329D File Offset: 0x001D149D
		private static bool IsCursorRightAfterExtent(IScriptPosition cursor, IScriptExtent extent)
		{
			return cursor.Offset == extent.EndOffset;
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x001D32AD File Offset: 0x001D14AD
		private static bool IsCursorAfterExtentAndInTheSameLine(IScriptPosition cursor, IScriptExtent extent)
		{
			return cursor.Offset >= extent.EndOffset && extent.EndLineNumber == cursor.LineNumber;
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x001D32CD File Offset: 0x001D14CD
		private static bool IsCursorBeforeExtent(IScriptPosition cursor, IScriptExtent extent)
		{
			return cursor.Offset < extent.StartOffset;
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x001D32DD File Offset: 0x001D14DD
		private static bool IsCursorAfterExtent(IScriptPosition cursor, IScriptExtent extent)
		{
			return extent.EndOffset < cursor.Offset;
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x001D32ED File Offset: 0x001D14ED
		private static bool IsCursorOutsideOfExtent(IScriptPosition cursor, IScriptExtent extent)
		{
			return cursor.Offset < extent.StartOffset || cursor.Offset > extent.EndOffset;
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x001D3380 File Offset: 0x001D1580
		internal CompletionContext CreateCompletionContext(ExecutionContext executionContext)
		{
			Token token3 = null;
			IScriptPosition positionForAstSearch = this._cursorPosition;
			bool flag = false;
			Token token2 = this._tokens.LastOrDefault((Token token) => CompletionAnalysis.IsCursorWithinOrJustAfterExtent(this._cursorPosition, token.Extent) && CompletionAnalysis.IsInterestingToken(token));
			if (token2 == null)
			{
				token3 = this._tokens.LastOrDefault((Token token) => CompletionAnalysis.IsCursorAfterExtent(this._cursorPosition, token.Extent) && CompletionAnalysis.IsInterestingToken(token));
				if (token3 != null)
				{
					positionForAstSearch = token3.Extent.EndScriptPosition;
					flag = true;
				}
			}
			else
			{
				StringExpandableToken stringExpandableToken = token2 as StringExpandableToken;
				if (stringExpandableToken != null && stringExpandableToken.NestedTokens != null)
				{
					token2 = (stringExpandableToken.NestedTokens.LastOrDefault((Token token) => CompletionAnalysis.IsCursorWithinOrJustAfterExtent(this._cursorPosition, token.Extent) && CompletionAnalysis.IsInterestingToken(token)) ?? stringExpandableToken);
				}
			}
			List<Ast> list = AstSearcher.FindAll(this._ast, (Ast ast) => CompletionAnalysis.IsCursorWithinOrJustAfterExtent(positionForAstSearch, ast.Extent), true).ToList<Ast>();
			return new CompletionContext
			{
				TokenAtCursor = token2,
				TokenBeforeCursor = token3,
				CursorPosition = this._cursorPosition,
				RelatedAsts = list,
				Options = this._options,
				ExecutionContext = executionContext,
				ReplacementIndex = (flag ? this._cursorPosition.Offset : 0),
				CurrentTypeDefinitionAst = Ast.GetAncestorTypeDefinitionAst(list.Last<Ast>()),
				CustomArgumentCompleters = executionContext.CustomArgumentCompleters,
				NativeArgumentCompleters = executionContext.NativeArgumentCompleters
			};
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x001D3504 File Offset: 0x001D1704
		private static Ast GetLastAstAtCursor(ScriptBlockAst scriptBlockAst, IScriptPosition cursorPosition)
		{
			IEnumerable<Ast> source = AstSearcher.FindAll(scriptBlockAst, (Ast ast) => CompletionAnalysis.IsCursorRightAfterExtent(cursorPosition, ast.Extent), true);
			return source.LastOrDefault<Ast>();
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x001D3538 File Offset: 0x001D1738
		private static bool CompleteAgainstSwitchFile(Ast lastAst, Token tokenBeforeCursor)
		{
			ErrorStatementAst errorStatementAst = lastAst as ErrorStatementAst;
			Tuple<Token, Ast> tuple;
			if (errorStatementAst != null && errorStatementAst.Flags != null && errorStatementAst.Kind != null && tokenBeforeCursor != null && errorStatementAst.Kind.Kind.Equals(TokenKind.Switch) && errorStatementAst.Flags.TryGetValue("file", out tuple))
			{
				return tuple.Item1.Extent.EndOffset == tokenBeforeCursor.Extent.EndOffset;
			}
			if (!(lastAst.Parent is CommandExpressionAst))
			{
				return false;
			}
			PipelineAst pipelineAst = lastAst.Parent.Parent as PipelineAst;
			if (pipelineAst == null)
			{
				return false;
			}
			errorStatementAst = (pipelineAst.Parent as ErrorStatementAst);
			return errorStatementAst != null && errorStatementAst.Kind != null && errorStatementAst.Flags != null && (errorStatementAst.Kind.Kind.Equals(TokenKind.Switch) && errorStatementAst.Flags.TryGetValue("file", out tuple)) && tuple.Item2 == pipelineAst;
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x001D363B File Offset: 0x001D183B
		private static bool CompleteOperator(Token tokenAtCursor, Ast lastAst)
		{
			if (tokenAtCursor.Kind == TokenKind.Minus)
			{
				return lastAst is BinaryExpressionAst;
			}
			return tokenAtCursor.Kind == TokenKind.Parameter && lastAst is CommandParameterAst && lastAst.Parent is ExpressionAst;
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x001D3698 File Offset: 0x001D1898
		private static bool CompleteAgainstStatementFlags(Ast scriptAst, Ast lastAst, Token token, out TokenKind kind)
		{
			kind = TokenKind.Unknown;
			ErrorStatementAst errorStatementAst = lastAst as ErrorStatementAst;
			if (errorStatementAst != null && errorStatementAst.Kind != null)
			{
				TokenKind kind2 = errorStatementAst.Kind.Kind;
				if (kind2 == TokenKind.Switch)
				{
					kind = TokenKind.Switch;
					return true;
				}
			}
			ScriptBlockAst scriptBlockAst = scriptAst as ScriptBlockAst;
			if (token != null && token.Kind == TokenKind.Minus && scriptBlockAst != null)
			{
				IEnumerable<Ast> source = AstSearcher.FindAll(scriptBlockAst, (Ast ast) => CompletionAnalysis.IsCursorAfterExtent(token.Extent.StartScriptPosition, ast.Extent), true);
				Ast ast2 = source.LastOrDefault<Ast>();
				errorStatementAst = null;
				while (ast2 != null)
				{
					errorStatementAst = (ast2 as ErrorStatementAst);
					if (errorStatementAst != null)
					{
						break;
					}
					ast2 = ast2.Parent;
				}
				if (errorStatementAst != null && errorStatementAst.Kind != null)
				{
					TokenKind kind3 = errorStatementAst.Kind.Kind;
					Tuple<Token, Ast> tuple;
					if (kind3 == TokenKind.Switch && errorStatementAst.Flags != null && errorStatementAst.Flags.TryGetValue("--%", out tuple) && CompletionAnalysis.IsTokenTheSame(tuple.Item1, token))
					{
						kind = TokenKind.Switch;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x001D37B0 File Offset: 0x001D19B0
		private static bool IsTokenTheSame(Token x, Token y)
		{
			return x.Kind == y.Kind && x.TokenFlags == y.TokenFlags && x.Extent.StartLineNumber == y.Extent.StartLineNumber && x.Extent.StartColumnNumber == y.Extent.StartColumnNumber && x.Extent.EndLineNumber == y.Extent.EndLineNumber && x.Extent.EndColumnNumber == y.Extent.EndColumnNumber;
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x001D383C File Offset: 0x001D1A3C
		internal List<CompletionResult> GetResults(PowerShell powerShell, out int replacementIndex, out int replacementLength)
		{
			CompletionContext completionContext = this.CreateCompletionContext(powerShell.GetContextFromTLS());
			completionContext.Helper = new CompletionExecutionHelper(powerShell);
			PSLanguageMode? pslanguageMode = null;
			List<CompletionResult> resultHelper;
			try
			{
				if (ExecutionContext.HasEverUsedConstrainedLanguage)
				{
					pslanguageMode = new PSLanguageMode?(completionContext.ExecutionContext.LanguageMode);
					completionContext.ExecutionContext.LanguageMode = PSLanguageMode.ConstrainedLanguage;
				}
				resultHelper = this.GetResultHelper(completionContext, out replacementIndex, out replacementLength, false);
			}
			finally
			{
				if (pslanguageMode != null)
				{
					completionContext.ExecutionContext.LanguageMode = pslanguageMode.Value;
				}
			}
			return resultHelper;
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x001D3A2C File Offset: 0x001D1C2C
		internal List<CompletionResult> GetResultHelper(CompletionContext completionContext, out int replacementIndex, out int replacementLength, bool isQuotedString)
		{
			replacementIndex = -1;
			replacementLength = -1;
			Token tokenAtCursor = completionContext.TokenAtCursor;
			Ast ast = completionContext.RelatedAsts.Last<Ast>();
			List<CompletionResult> list = null;
			if (tokenAtCursor != null)
			{
				replacementIndex = tokenAtCursor.Extent.StartScriptPosition.Offset;
				replacementLength = tokenAtCursor.Extent.EndScriptPosition.Offset - replacementIndex;
				completionContext.ReplacementIndex = replacementIndex;
				completionContext.ReplacementLength = replacementLength;
				TokenKind kind = tokenAtCursor.Kind;
				if (kind <= TokenKind.Comma)
				{
					switch (kind)
					{
					case TokenKind.Variable:
					case TokenKind.SplattedVariable:
						completionContext.WordToComplete = ((VariableToken)tokenAtCursor).VariablePath.UserPath;
						list = CompletionCompleters.CompleteVariable(completionContext);
						goto IL_81E;
					case TokenKind.Parameter:
					{
						if (isQuotedString)
						{
							goto IL_81E;
						}
						completionContext.WordToComplete = tokenAtCursor.Text;
						CommandAst commandAst = ast.Parent as CommandAst;
						if (ast is StringConstantExpressionAst && commandAst != null && commandAst.CommandElements.Count == 1)
						{
							list = CompletionAnalysis.CompleteFileNameAsCommand(completionContext);
							goto IL_81E;
						}
						TokenKind kind2;
						if (CompletionAnalysis.CompleteAgainstStatementFlags(null, ast, null, out kind2))
						{
							list = CompletionCompleters.CompleteStatementFlags(kind2, completionContext.WordToComplete);
							goto IL_81E;
						}
						if (CompletionAnalysis.CompleteOperator(tokenAtCursor, ast))
						{
							list = CompletionCompleters.CompleteOperator(completionContext.WordToComplete);
							goto IL_81E;
						}
						if (completionContext.WordToComplete.EndsWith(":", StringComparison.Ordinal))
						{
							replacementIndex = tokenAtCursor.Extent.EndScriptPosition.Offset;
							replacementLength = 0;
							completionContext.WordToComplete = string.Empty;
							list = CompletionCompleters.CompleteCommandArgument(completionContext);
							goto IL_81E;
						}
						list = CompletionCompleters.CompleteCommandParameter(completionContext);
						goto IL_81E;
					}
					case TokenKind.Number:
						if (ast is ConstantExpressionAst && (ast.Parent is CommandAst || ast.Parent is CommandParameterAst || (ast.Parent is ArrayLiteralAst && (ast.Parent.Parent is CommandAst || ast.Parent.Parent is CommandParameterAst))))
						{
							completionContext.WordToComplete = tokenAtCursor.Text;
							list = CompletionCompleters.CompleteCommandArgument(completionContext);
							replacementIndex = completionContext.ReplacementIndex;
							replacementLength = completionContext.ReplacementLength;
							goto IL_81E;
						}
						goto IL_81E;
					case TokenKind.Label:
					case TokenKind.NewLine:
					case TokenKind.LineContinuation:
					case TokenKind.EndOfInput:
					case TokenKind.HereStringLiteral:
					case TokenKind.HereStringExpandable:
					case TokenKind.RParen:
					case TokenKind.LCurly:
					case TokenKind.RCurly:
					case TokenKind.LBracket:
						goto IL_50E;
					case TokenKind.Identifier:
					case TokenKind.Generic:
						break;
					case TokenKind.Comment:
						if (!isQuotedString)
						{
							completionContext.WordToComplete = tokenAtCursor.Text;
							list = CompletionCompleters.CompleteComment(completionContext);
							goto IL_81E;
						}
						goto IL_81E;
					case TokenKind.StringLiteral:
					case TokenKind.StringExpandable:
						list = this.GetResultForString(completionContext, ref replacementIndex, ref replacementLength, isQuotedString);
						goto IL_81E;
					case TokenKind.LParen:
					case TokenKind.AtParen:
						goto IL_4B8;
					case TokenKind.RBracket:
					{
						if (!(ast is TypeExpressionAst))
						{
							goto IL_81E;
						}
						TypeExpressionAst targetExpr = (TypeExpressionAst)ast;
						List<CompletionResult> list2 = new List<CompletionResult>();
						CompletionCompleters.CompleteMemberHelper(true, "*", targetExpr, completionContext, list2);
						if (list2.Count > 0)
						{
							replacementIndex++;
							replacementLength = 0;
							list = (from entry in list2
							let completionText = TokenKind.ColonColon.Text() + entry.CompletionText
							select new CompletionResult(completionText, entry.ListItemText, entry.ResultType, entry.ToolTip)).ToList<CompletionResult>();
							goto IL_81E;
						}
						goto IL_81E;
					}
					default:
					{
						if (kind != TokenKind.Comma)
						{
							goto IL_50E;
						}
						if (ast is ErrorExpressionAst && (ast.Parent is CommandAst || ast.Parent is CommandParameterAst))
						{
							replacementIndex += replacementLength;
							replacementLength = 0;
							list = CompletionCompleters.CompleteCommandArgument(completionContext);
							goto IL_81E;
						}
						bool flag;
						list = this.GetResultForEnumPropertyValueOfDSCResource(completionContext, string.Empty, ref replacementIndex, ref replacementLength, out flag);
						goto IL_81E;
					}
					}
				}
				else
				{
					switch (kind)
					{
					case TokenKind.ColonColon:
					case TokenKind.Dot:
						replacementIndex += tokenAtCursor.Text.Length;
						replacementLength = 0;
						list = CompletionCompleters.CompleteMember(completionContext, tokenAtCursor.Kind == TokenKind.ColonColon);
						goto IL_81E;
					case TokenKind.Exclaim:
					case TokenKind.Divide:
					case TokenKind.Rem:
					case TokenKind.Plus:
						goto IL_50E;
					case TokenKind.Multiply:
						break;
					case TokenKind.Minus:
					{
						if (CompletionAnalysis.CompleteOperator(tokenAtCursor, ast))
						{
							list = CompletionCompleters.CompleteOperator("");
							goto IL_81E;
						}
						TokenKind kind2;
						if (CompletionAnalysis.CompleteAgainstStatementFlags(completionContext.RelatedAsts[0], null, tokenAtCursor, out kind2))
						{
							completionContext.WordToComplete = tokenAtCursor.Text;
							list = CompletionCompleters.CompleteStatementFlags(kind2, completionContext.WordToComplete);
							goto IL_81E;
						}
						goto IL_81E;
					}
					case TokenKind.Equals:
						goto IL_4B8;
					default:
						if (kind != TokenKind.Redirection)
						{
							if (kind != TokenKind.DynamicKeyword)
							{
								goto IL_50E;
							}
							DynamicKeywordStatementAst dynamicKeywordStatementAst;
							ConfigurationDefinitionAst ancestorConfigurationAstAndKeywordAst = this.GetAncestorConfigurationAstAndKeywordAst(completionContext.CursorPosition, ast, out dynamicKeywordStatementAst);
							bool flag2 = false;
							completionContext.WordToComplete = tokenAtCursor.Text.Trim();
							return this.GetResultForIdentifierInConfiguration(completionContext, ancestorConfigurationAstAndKeywordAst, null, out flag2);
						}
						else
						{
							if (ast is ErrorExpressionAst && ast.Parent is FileRedirectionAst)
							{
								completionContext.WordToComplete = string.Empty;
								completionContext.ReplacementIndex = (replacementIndex += tokenAtCursor.Text.Length);
								completionContext.ReplacementLength = (replacementLength = 0);
								list = new List<CompletionResult>(CompletionCompleters.CompleteFilename(completionContext));
								goto IL_81E;
							}
							goto IL_81E;
						}
						break;
					}
				}
				list = this.GetResultForIdentifier(completionContext, ref replacementIndex, ref replacementLength, isQuotedString);
				goto IL_81E;
				IL_4B8:
				if (ast is AttributeAst)
				{
					completionContext.ReplacementIndex = (replacementIndex += tokenAtCursor.Text.Length);
					completionContext.ReplacementLength = (replacementLength = 0);
					list = this.GetResultForAttributeArgument(completionContext, ref replacementIndex, ref replacementLength);
					goto IL_81E;
				}
				bool flag3;
				list = this.GetResultForEnumPropertyValueOfDSCResource(completionContext, string.Empty, ref replacementIndex, ref replacementLength, out flag3);
				goto IL_81E;
				IL_50E:
				if ((tokenAtCursor.TokenFlags & TokenFlags.Keyword) != TokenFlags.None)
				{
					completionContext.WordToComplete = tokenAtCursor.Text;
					list = CompletionAnalysis.CompleteFileNameAsCommand(completionContext);
					List<CompletionResult> list3 = CompletionCompleters.CompleteCommand(completionContext);
					if (list3 != null && list3.Count > 0)
					{
						list.AddRange(list3);
					}
				}
				else
				{
					replacementIndex = -1;
					replacementLength = -1;
				}
			}
			else
			{
				IScriptPosition cursorPosition = completionContext.CursorPosition;
				bool flag4 = string.IsNullOrWhiteSpace(cursorPosition.Line);
				Token tokenBeforeCursor = completionContext.TokenBeforeCursor;
				bool flag5 = false;
				if (tokenBeforeCursor != null)
				{
					flag5 = (completionContext.TokenBeforeCursor.Kind == TokenKind.LineContinuation);
				}
				bool flag6 = flag4 && !flag5;
				bool flag7 = ast is ExpressionAst;
				if (!isQuotedString && !flag6 && (ast is CommandParameterAst || ast is CommandAst || (flag7 && ast.Parent is CommandAst) || (flag7 && ast.Parent is CommandParameterAst) || (flag7 && ast.Parent is ArrayLiteralAst && (ast.Parent.Parent is CommandAst || ast.Parent.Parent is CommandParameterAst))))
				{
					completionContext.WordToComplete = string.Empty;
					HashtableAst hashtableAst = ast as HashtableAst;
					if (hashtableAst != null && this.CheckForPendingAssigment(hashtableAst))
					{
						return list;
					}
					if (hashtableAst != null)
					{
						completionContext.ReplacementIndex = (replacementIndex = completionContext.CursorPosition.Offset);
						completionContext.ReplacementLength = (replacementLength = 0);
						list = CompletionCompleters.CompleteHashtableKey(completionContext, hashtableAst);
					}
					else
					{
						list = CompletionCompleters.CompleteCommandArgument(completionContext);
						replacementIndex = completionContext.ReplacementIndex;
						replacementLength = completionContext.ReplacementLength;
					}
				}
				else if (!isQuotedString)
				{
					bool flag8 = (tokenAtCursor != null && tokenAtCursor.Kind == TokenKind.LineContinuation) || (tokenBeforeCursor != null && tokenBeforeCursor.Kind == TokenKind.LineContinuation);
					if (flag4 && !flag8)
					{
						list = this.GetResultForHashtable(completionContext);
						if (list == null || list.Count == 0)
						{
							DynamicKeywordStatementAst keywordAst;
							ConfigurationDefinitionAst ancestorConfigurationAstAndKeywordAst2 = this.GetAncestorConfigurationAstAndKeywordAst(cursorPosition, ast, out keywordAst);
							if (ancestorConfigurationAstAndKeywordAst2 != null)
							{
								bool flag9;
								list = this.GetResultForIdentifierInConfiguration(completionContext, ancestorConfigurationAstAndKeywordAst2, keywordAst, out flag9);
							}
						}
					}
					else if (completionContext.TokenAtCursor == null && tokenBeforeCursor != null)
					{
						TokenKind kind3 = tokenBeforeCursor.Kind;
						if (kind3 <= TokenKind.AtParen)
						{
							if (kind3 != TokenKind.LParen)
							{
								if (kind3 != TokenKind.AtParen)
								{
									goto IL_789;
								}
							}
							else
							{
								if (ast is AttributeAst)
								{
									completionContext.ReplacementLength = (replacementLength = 0);
									list = this.GetResultForAttributeArgument(completionContext, ref replacementIndex, ref replacementLength);
									goto IL_789;
								}
								bool flag10;
								list = this.GetResultForEnumPropertyValueOfDSCResource(completionContext, string.Empty, ref replacementIndex, ref replacementLength, out flag10);
								goto IL_789;
							}
						}
						else if (kind3 != TokenKind.Comma && kind3 != TokenKind.Equals)
						{
							goto IL_789;
						}
						bool flag11;
						list = this.GetResultForEnumPropertyValueOfDSCResource(completionContext, string.Empty, ref replacementIndex, ref replacementLength, out flag11);
					}
					IL_789:
					if (list != null && list.Count > 0)
					{
						completionContext.ReplacementIndex = (replacementIndex = completionContext.CursorPosition.Offset);
						completionContext.ReplacementLength = (replacementLength = 0);
					}
					else
					{
						bool flag12 = false;
						if (ast is ErrorExpressionAst && ast.Parent is FileRedirectionAst)
						{
							flag12 = true;
						}
						else if (ast is ErrorStatementAst && CompletionAnalysis.CompleteAgainstSwitchFile(ast, completionContext.TokenBeforeCursor))
						{
							flag12 = true;
						}
						if (flag12)
						{
							completionContext.WordToComplete = string.Empty;
							list = new List<CompletionResult>(CompletionCompleters.CompleteFilename(completionContext));
							replacementIndex = completionContext.ReplacementIndex;
							replacementLength = completionContext.ReplacementLength;
						}
					}
				}
			}
			IL_81E:
			if (list == null || list.Count == 0)
			{
				TypeExpressionAst typeExpressionAst = completionContext.RelatedAsts.OfType<TypeExpressionAst>().FirstOrDefault<TypeExpressionAst>();
				TypeName typeName = null;
				if (typeExpressionAst != null)
				{
					typeName = CompletionAnalysis.FindTypeNameToComplete(typeExpressionAst.TypeName, this._cursorPosition);
				}
				else
				{
					TypeConstraintAst typeConstraintAst = completionContext.RelatedAsts.OfType<TypeConstraintAst>().FirstOrDefault<TypeConstraintAst>();
					if (typeConstraintAst != null)
					{
						typeName = CompletionAnalysis.FindTypeNameToComplete(typeConstraintAst.TypeName, this._cursorPosition);
					}
				}
				if (typeName != null)
				{
					replacementIndex = typeName.Extent.StartOffset;
					replacementLength = typeName.Extent.EndOffset - replacementIndex;
					completionContext.WordToComplete = typeName.FullName;
					list = CompletionCompleters.CompleteType(completionContext, "", "");
				}
			}
			if (list == null || list.Count == 0)
			{
				list = this.GetResultForHashtable(completionContext);
			}
			if (list == null || list.Count == 0)
			{
				string text = completionContext.RelatedAsts[0].Extent.Text;
				if (Regex.IsMatch(text, "^[\\S]+$") && completionContext.RelatedAsts.Count > 0 && completionContext.RelatedAsts[0] is ScriptBlockAst)
				{
					replacementIndex = completionContext.RelatedAsts[0].Extent.StartScriptPosition.Offset;
					replacementLength = completionContext.RelatedAsts[0].Extent.EndScriptPosition.Offset - replacementIndex;
					completionContext.WordToComplete = text;
					list = CompletionAnalysis.CompleteFileNameAsCommand(completionContext);
				}
			}
			return list;
		}

		// Token: 0x06005935 RID: 22837 RVA: 0x001D43B4 File Offset: 0x001D25B4
		private List<CompletionResult> GetResultForHashtable(CompletionContext completionContext)
		{
			Ast ast = completionContext.RelatedAsts.Last<Ast>();
			HashtableAst hashtableAst = null;
			IScriptPosition cursorPosition = completionContext.CursorPosition;
			HashtableAst hashtableAst2 = ast as HashtableAst;
			if (hashtableAst2 != null)
			{
				if (cursorPosition.Offset < hashtableAst2.Extent.EndOffset)
				{
					hashtableAst = hashtableAst2;
				}
				else if (cursorPosition.Offset == hashtableAst2.Extent.EndOffset && (completionContext.TokenAtCursor == null || completionContext.TokenAtCursor.Kind != TokenKind.RCurly))
				{
					hashtableAst = hashtableAst2;
				}
			}
			else
			{
				Ast ast2;
				hashtableAst2 = Ast.GetAncestorHashtableAst(ast, out ast2);
				if (hashtableAst2 != null)
				{
					DynamicKeywordStatementAst ancestorAst = Ast.GetAncestorAst<DynamicKeywordStatementAst>(hashtableAst2);
					if (ancestorAst != null && string.IsNullOrWhiteSpace(cursorPosition.Line) && cursorPosition.Offset > ast2.Extent.EndOffset && cursorPosition.Offset <= hashtableAst2.Extent.EndOffset)
					{
						hashtableAst = hashtableAst2;
					}
				}
			}
			hashtableAst2 = hashtableAst;
			if (hashtableAst2 != null)
			{
				completionContext.ReplacementIndex = completionContext.CursorPosition.Offset;
				completionContext.ReplacementLength = 0;
				return CompletionCompleters.CompleteHashtableKey(completionContext, hashtableAst2);
			}
			return null;
		}

		// Token: 0x06005936 RID: 22838 RVA: 0x001D449C File Offset: 0x001D269C
		private bool CheckForPendingAssigment(HashtableAst hashTableAst)
		{
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashTableAst.KeyValuePairs)
			{
				if (tuple.Item2 is ErrorStatementAst)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x001D44F8 File Offset: 0x001D26F8
		internal static TypeName FindTypeNameToComplete(ITypeName type, IScriptPosition cursor)
		{
			TypeName typeName = type as TypeName;
			if (typeName != null)
			{
				if (cursor.Offset <= type.Extent.StartOffset || cursor.Offset > type.Extent.EndOffset)
				{
					return null;
				}
				return typeName;
			}
			else
			{
				GenericTypeName genericTypeName = type as GenericTypeName;
				if (genericTypeName != null)
				{
					typeName = CompletionAnalysis.FindTypeNameToComplete(genericTypeName.TypeName, cursor);
					if (typeName != null)
					{
						return typeName;
					}
					foreach (ITypeName type2 in genericTypeName.GenericArguments)
					{
						typeName = CompletionAnalysis.FindTypeNameToComplete(type2, cursor);
						if (typeName != null)
						{
							return typeName;
						}
					}
					return null;
				}
				else
				{
					ArrayTypeName arrayTypeName = type as ArrayTypeName;
					if (arrayTypeName != null)
					{
						return CompletionAnalysis.FindTypeNameToComplete(arrayTypeName.ElementType, cursor) ?? null;
					}
					return null;
				}
			}
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x001D45C8 File Offset: 0x001D27C8
		private static string GetFirstLineSubString(string stringToComplete, out bool hasNewLine)
		{
			hasNewLine = false;
			if (!string.IsNullOrEmpty(stringToComplete))
			{
				int num = stringToComplete.IndexOfAny(CompletionAnalysis.newlineCharacters);
				if (num >= 0)
				{
					stringToComplete = stringToComplete.Substring(0, num);
					hasNewLine = true;
				}
			}
			return stringToComplete;
		}

		// Token: 0x06005939 RID: 22841 RVA: 0x001D4600 File Offset: 0x001D2800
		private Tuple<ExpressionAst, StatementAst> GetHashEntryContainsCursor(IScriptPosition cursor, HashtableAst hashTableAst, bool isCursorInString)
		{
			Tuple<ExpressionAst, StatementAst> result = null;
			foreach (Tuple<ExpressionAst, StatementAst> tuple in hashTableAst.KeyValuePairs)
			{
				if (CompletionAnalysis.IsCursorWithinOrJustAfterExtent(cursor, tuple.Item2.Extent))
				{
					result = tuple;
					break;
				}
				if (!isCursorInString)
				{
					if (tuple.Item2.Extent.StartLineNumber > tuple.Item1.Extent.EndLineNumber && CompletionAnalysis.IsCursorAfterExtentAndInTheSameLine(cursor, tuple.Item1.Extent))
					{
						result = tuple;
						break;
					}
					if (!CompletionAnalysis.IsCursorBeforeExtent(cursor, tuple.Item1.Extent) && CompletionAnalysis.IsCursorAfterExtentAndInTheSameLine(cursor, tuple.Item2.Extent))
					{
						result = tuple;
					}
				}
			}
			return result;
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x001D470C File Offset: 0x001D290C
		private List<CompletionResult> GetResultForEnumPropertyValueOfDSCResource(CompletionContext completionContext, string stringToComplete, ref int replacementIndex, ref int replacementLength, out bool shouldContinue)
		{
			shouldContinue = true;
			bool flag = completionContext.TokenAtCursor is StringToken;
			List<CompletionResult> list = null;
			Ast ast = completionContext.RelatedAsts.Last<Ast>();
			Ast ast2;
			HashtableAst ancestorHashtableAst = Ast.GetAncestorHashtableAst(ast, out ast2);
			if (ancestorHashtableAst != null)
			{
				DynamicKeywordStatementAst ancestorAst = Ast.GetAncestorAst<DynamicKeywordStatementAst>(ancestorHashtableAst);
				if (ancestorAst != null)
				{
					IScriptPosition cursorPosition = completionContext.CursorPosition;
					Tuple<ExpressionAst, StatementAst> hashEntryContainsCursor = this.GetHashEntryContainsCursor(cursorPosition, ancestorHashtableAst, flag);
					if (hashEntryContainsCursor != null)
					{
						StringConstantExpressionAst stringConstantExpressionAst = hashEntryContainsCursor.Item1 as StringConstantExpressionAst;
						if (stringConstantExpressionAst != null && ancestorAst.Keyword.Properties.ContainsKey(stringConstantExpressionAst.Value))
						{
							DynamicKeywordProperty dynamicKeywordProperty = ancestorAst.Keyword.Properties[stringConstantExpressionAst.Value];
							List<string> existingValues = null;
							WildcardPattern wildcardPattern = null;
							bool flag2 = string.Equals(dynamicKeywordProperty.Name, "DependsOn", StringComparison.OrdinalIgnoreCase);
							bool flag3 = false;
							string text = (completionContext.TokenAtCursor is StringExpandableToken) ? "\"" : "'";
							if ((dynamicKeywordProperty.ValueMap != null && dynamicKeywordProperty.ValueMap.Count > 0) || flag2)
							{
								shouldContinue = false;
								existingValues = new List<string>();
								if (string.Equals(dynamicKeywordProperty.TypeConstraint, "StringArray", StringComparison.OrdinalIgnoreCase))
								{
									ArrayLiteralAst ancestorAst2 = Ast.GetAncestorAst<ArrayLiteralAst>(ast);
									if (ancestorAst2 != null && ancestorAst2.Elements.Count > 0)
									{
										foreach (ExpressionAst expressionAst in ancestorAst2.Elements)
										{
											StringConstantExpressionAst stringConstantExpressionAst2 = expressionAst as StringConstantExpressionAst;
											if (stringConstantExpressionAst2 != null && CompletionAnalysis.IsCursorOutsideOfExtent(cursorPosition, expressionAst.Extent))
											{
												existingValues.Add(stringConstantExpressionAst2.Value);
											}
										}
									}
								}
								stringToComplete = CompletionAnalysis.GetFirstLineSubString(stringToComplete, out flag3);
								completionContext.WordToComplete = stringToComplete;
								replacementLength = (completionContext.ReplacementLength = stringToComplete.Length);
								if (completionContext.TokenAtCursor is StringToken)
								{
									replacementIndex = completionContext.TokenAtCursor.Extent.StartOffset + 1;
								}
								else
								{
									replacementIndex = completionContext.CursorPosition.Offset - replacementLength;
								}
								completionContext.ReplacementIndex = replacementIndex;
								string pattern = stringToComplete + "*";
								wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
								list = new List<CompletionResult>();
							}
							if (dynamicKeywordProperty.ValueMap != null && dynamicKeywordProperty.ValueMap.Count > 0)
							{
								IEnumerable<string> enumerable = from x in dynamicKeywordProperty.ValueMap.Keys
								orderby x
								select x into v
								where !existingValues.Contains(v, StringComparer.OrdinalIgnoreCase)
								select v;
								IEnumerable<string> enumerable2 = from v in enumerable
								where wildcardPattern.IsMatch(v)
								select v;
								if (enumerable2 == null || !enumerable2.Any<string>())
								{
									enumerable2 = enumerable;
								}
								using (IEnumerator<string> enumerator2 = enumerable2.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										string text2 = enumerator2.Current;
										string text3 = flag ? text2 : (text + text2 + text);
										if (flag3)
										{
											text3 += text;
										}
										list.Add(new CompletionResult(text3, text2, CompletionResultType.Text, text2));
									}
									return list;
								}
							}
							if (flag2)
							{
								ConfigurationDefinitionAst ancestorAst3 = Ast.GetAncestorAst<ConfigurationDefinitionAst>(ancestorAst);
								if (ancestorAst3 != null)
								{
									NamedBlockAst namedBlockAst = ancestorAst.Parent as NamedBlockAst;
									if (namedBlockAst != null)
									{
										List<string> list2 = new List<string>();
										foreach (StatementAst statementAst in namedBlockAst.Statements)
										{
											DynamicKeywordStatementAst dynamicKeywordStatementAst = statementAst as DynamicKeywordStatementAst;
											if (dynamicKeywordStatementAst != null && dynamicKeywordStatementAst != ancestorAst && !string.Equals(dynamicKeywordStatementAst.Keyword.Keyword, "Node", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(dynamicKeywordStatementAst.ElementName))
											{
												StringBuilder stringBuilder = new StringBuilder("[");
												stringBuilder.Append(dynamicKeywordStatementAst.Keyword.Keyword);
												stringBuilder.Append("]");
												stringBuilder.Append(dynamicKeywordStatementAst.ElementName);
												string text4 = stringBuilder.ToString();
												if (!existingValues.Contains(text4, StringComparer.OrdinalIgnoreCase) && !list2.Contains(text4, StringComparer.OrdinalIgnoreCase))
												{
													list2.Add(text4);
												}
											}
										}
										IEnumerable<string> enumerable3 = from r in list2
										where wildcardPattern.IsMatch(r)
										select r;
										if (enumerable3 == null || !enumerable3.Any<string>())
										{
											enumerable3 = list2;
										}
										foreach (string text5 in enumerable3)
										{
											string text6 = flag ? text5 : (text + text5 + text);
											if (flag3)
											{
												text6 += text;
											}
											list.Add(new CompletionResult(text6, text5, CompletionResultType.Text, text5));
										}
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600593B RID: 22843 RVA: 0x001D4C34 File Offset: 0x001D2E34
		private List<CompletionResult> GetResultForString(CompletionContext completionContext, ref int replacementIndex, ref int replacementLength, bool isQuotedString)
		{
			if (isQuotedString)
			{
				return null;
			}
			Token tokenAtCursor = completionContext.TokenAtCursor;
			Ast ast = completionContext.RelatedAsts.Last<Ast>();
			List<CompletionResult> list = null;
			ExpandableStringExpressionAst expandableStringExpressionAst = ast as ExpandableStringExpressionAst;
			StringConstantExpressionAst stringConstantExpressionAst = ast as StringConstantExpressionAst;
			if (stringConstantExpressionAst == null && expandableStringExpressionAst == null)
			{
				return list;
			}
			string text = (stringConstantExpressionAst != null) ? stringConstantExpressionAst.Value : expandableStringExpressionAst.Value;
			StringConstantType stringConstantType = (stringConstantExpressionAst != null) ? stringConstantExpressionAst.StringConstantType : expandableStringExpressionAst.StringConstantType;
			string text2 = null;
			bool flag;
			list = this.GetResultForEnumPropertyValueOfDSCResource(completionContext, text, ref replacementIndex, ref replacementLength, out flag);
			if (!flag || (list != null && list.Count > 0))
			{
				return list;
			}
			if (stringConstantType == StringConstantType.DoubleQuoted)
			{
				Match match = Regex.Match(text, "(\\$[\\w\\d]+\\.[\\w\\d\\*]*)$");
				if (match.Success)
				{
					text2 = match.Groups[1].Value;
				}
				else if ((match = Regex.Match(text, "(\\[[\\w\\d\\.]+\\]::[\\w\\d\\*]*)$")).Success)
				{
					text2 = match.Groups[1].Value;
				}
			}
			if (text2 != null)
			{
				int offset = tokenAtCursor.Extent.StartScriptPosition.Offset;
				int num = this._cursorPosition.Offset - offset - 1;
				if (num >= text.Length)
				{
					num = text.Length;
				}
				CompletionAnalysis completionAnalysis = new CompletionAnalysis(this._ast, this._tokens, this._cursorPosition, this._options);
				CompletionContext completionContext2 = completionAnalysis.CreateCompletionContext(completionContext.ExecutionContext);
				completionContext2.Helper = completionContext.Helper;
				int length;
				int num2;
				List<CompletionResult> resultHelper = completionAnalysis.GetResultHelper(completionContext2, out length, out num2, true);
				if (resultHelper == null || resultHelper.Count <= 0)
				{
					return list;
				}
				list = new List<CompletionResult>();
				replacementIndex = offset + 1 + (num - text2.Length);
				replacementLength = text2.Length;
				string str = text2.Substring(0, length);
				using (List<CompletionResult>.Enumerator enumerator = resultHelper.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompletionResult completionResult = enumerator.Current;
						string text3 = str + completionResult.CompletionText;
						if (completionResult.ResultType == CompletionResultType.Property)
						{
							text3 = TokenKind.DollarParen.Text() + text3 + TokenKind.RParen.Text();
						}
						else if (completionResult.ResultType == CompletionResultType.Method)
						{
							text3 = TokenKind.DollarParen.Text() + text3;
						}
						text3 += "\"";
						list.Add(new CompletionResult(text3, completionResult.ListItemText, completionResult.ResultType, completionResult.ToolTip));
					}
					return list;
				}
			}
			CommandElementAst stringAst = ast as CommandElementAst;
			string text4 = CompletionCompleters.ConcatenateStringPathArguments(stringAst, string.Empty, completionContext);
			if (text4 != null)
			{
				completionContext.WordToComplete = text4;
				if (ast.Parent is CommandAst || ast.Parent is CommandParameterAst)
				{
					list = CompletionCompleters.CompleteCommandArgument(completionContext);
					replacementIndex = completionContext.ReplacementIndex;
					replacementLength = completionContext.ReplacementLength;
				}
				else
				{
					list = new List<CompletionResult>(CompletionCompleters.CompleteFilename(completionContext));
					if (text4.IndexOf('-') != -1)
					{
						List<CompletionResult> list2 = CompletionCompleters.CompleteCommand(completionContext);
						if (list2 != null && list2.Count > 0)
						{
							list.AddRange(list2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x001D4F38 File Offset: 0x001D3138
		private ConfigurationDefinitionAst GetAncestorConfigurationAstAndKeywordAst(IScriptPosition cursorPosition, Ast ast, out DynamicKeywordStatementAst keywordAst)
		{
			ConfigurationDefinitionAst configurationDefinitionAst = Ast.GetAncestorConfigurationDefinitionAstAndDynamicKeywordStatementAst(ast, out keywordAst);
			while (configurationDefinitionAst != null && cursorPosition.Offset > configurationDefinitionAst.Extent.EndOffset)
			{
				configurationDefinitionAst = Ast.GetAncestorAst<ConfigurationDefinitionAst>(configurationDefinitionAst.Parent);
			}
			return configurationDefinitionAst;
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x001D4FE4 File Offset: 0x001D31E4
		private List<CompletionResult> GetResultForIdentifierInConfiguration(CompletionContext completionContext, ConfigurationDefinitionAst configureAst, DynamicKeywordStatementAst keywordAst, out bool matched)
		{
			List<CompletionResult> list = null;
			matched = false;
			IEnumerable<DynamicKeyword> enumerable = from k in configureAst.DefinedKeywords
			where string.Compare(k.Keyword, "Node", StringComparison.OrdinalIgnoreCase) == 0 || (k.IsCompatibleWithConfigurationType(configureAst.ConfigurationType) && !DynamicKeyword.IsHiddenKeyword(k.Keyword) && !k.IsReservedKeyword)
			select k;
			if (keywordAst != null && completionContext.CursorPosition.Offset < keywordAst.Extent.EndOffset)
			{
				enumerable = keywordAst.Keyword.GetAllowedKeywords(enumerable);
			}
			if (enumerable != null && enumerable.Any<DynamicKeyword>())
			{
				string pattern = (completionContext.WordToComplete ?? string.Empty) + "*";
				WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
				IEnumerable<DynamicKeyword> enumerable2 = from k in enumerable
				where wildcardPattern.IsMatch(k.Keyword)
				select k;
				if (enumerable2 == null || !enumerable2.Any<DynamicKeyword>())
				{
					enumerable2 = enumerable;
				}
				else
				{
					matched = true;
				}
				foreach (DynamicKeyword dynamicKeyword in enumerable2)
				{
					string dscresourceUsageString = DscClassCache.GetDSCResourceUsageString(dynamicKeyword);
					if (list == null)
					{
						list = new List<CompletionResult>();
					}
					list.Add(new CompletionResult(dynamicKeyword.Keyword, dynamicKeyword.Keyword, CompletionResultType.DynamicKeyword, dscresourceUsageString));
				}
			}
			return list;
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x001D5130 File Offset: 0x001D3330
		private List<CompletionResult> GetResultForIdentifier(CompletionContext completionContext, ref int replacementIndex, ref int replacementLength, bool isQuotedString)
		{
			Token tokenAtCursor = completionContext.TokenAtCursor;
			Ast ast = completionContext.RelatedAsts.Last<Ast>();
			List<CompletionResult> list = null;
			completionContext.WordToComplete = tokenAtCursor.Text;
			StringConstantExpressionAst stringConstantExpressionAst = ast as StringConstantExpressionAst;
			if (stringConstantExpressionAst != null)
			{
				if (stringConstantExpressionAst.Value.Equals("$", StringComparison.Ordinal))
				{
					completionContext.WordToComplete = "";
					return CompletionCompleters.CompleteVariable(completionContext);
				}
				UsingStatementAst usingStatementAst = stringConstantExpressionAst.Parent as UsingStatementAst;
				if (usingStatementAst != null)
				{
					completionContext.ReplacementIndex = stringConstantExpressionAst.Extent.StartOffset;
					completionContext.ReplacementLength = stringConstantExpressionAst.Extent.EndOffset - replacementIndex;
					completionContext.WordToComplete = stringConstantExpressionAst.Extent.Text;
					switch (usingStatementAst.UsingStatementKind)
					{
					case UsingStatementKind.Assembly:
					case UsingStatementKind.Command:
					case UsingStatementKind.Type:
						break;
					case UsingStatementKind.Module:
					{
						HashSet<string> extension = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
						{
							".psm1",
							".psd1",
							".dll",
							".cdxml",
							".xaml"
						};
						list = CompletionCompleters.CompleteFilename(completionContext, false, extension).ToList<CompletionResult>();
						if (completionContext.WordToComplete.IndexOfAny(new char[]
						{
							'\\',
							'/',
							':'
						}) != -1)
						{
							return list;
						}
						List<CompletionResult> list2 = CompletionCompleters.CompleteModuleName(completionContext, false);
						if (list2 != null && list2.Count > 0)
						{
							list.AddRange(list2);
						}
						return list;
					}
					case UsingStatementKind.Namespace:
						list = CompletionCompleters.CompleteNamespace(completionContext, "", "");
						return list;
					default:
						throw new ArgumentOutOfRangeException("UsingStatementKind");
					}
				}
			}
			list = this.GetResultForAttributeArgument(completionContext, ref replacementIndex, ref replacementLength);
			if (list != null)
			{
				return list;
			}
			if ((tokenAtCursor.TokenFlags & TokenFlags.CommandName) != TokenFlags.None)
			{
				if (completionContext.RelatedAsts.Count > 0 && completionContext.RelatedAsts[0] is ScriptBlockAst)
				{
					Ast ast2 = null;
					InternalScriptPosition internalScriptPosition = (InternalScriptPosition)this._cursorPosition;
					int num = internalScriptPosition.Offset - tokenAtCursor.Text.Length;
					if (num >= 0)
					{
						InternalScriptPosition cursorPosition = internalScriptPosition.CloneWithNewOffset(num);
						ScriptBlockAst scriptBlockAst = (ScriptBlockAst)completionContext.RelatedAsts[0];
						ast2 = CompletionAnalysis.GetLastAstAtCursor(scriptBlockAst, cursorPosition);
					}
					if (ast2 != null && ast2.Extent.EndLineNumber == tokenAtCursor.Extent.StartLineNumber && ast2.Extent.EndColumnNumber == tokenAtCursor.Extent.StartColumnNumber)
					{
						if (tokenAtCursor.Text.IndexOfAny(new char[]
						{
							'\\',
							'/'
						}) == 0)
						{
							string text = CompletionCompleters.ConcatenateStringPathArguments(ast2 as CommandElementAst, tokenAtCursor.Text, completionContext);
							if (text != null)
							{
								completionContext.WordToComplete = text;
								list = new List<CompletionResult>(CompletionCompleters.CompleteFilename(completionContext));
								if (list.Count > 0)
								{
									replacementIndex = ast2.Extent.StartScriptPosition.Offset;
									replacementLength += ast2.Extent.Text.Length;
								}
								return list;
							}
							VariableExpressionAst variableExpressionAst = ast2 as VariableExpressionAst;
							string text2 = (variableExpressionAst != null) ? CompletionCompleters.CombineVariableWithPartialPath(variableExpressionAst, tokenAtCursor.Text, completionContext.ExecutionContext) : null;
							if (text2 == null)
							{
								return list;
							}
							completionContext.WordToComplete = text2;
							replacementIndex = ast2.Extent.StartScriptPosition.Offset;
							replacementLength += ast2.Extent.Text.Length;
							completionContext.ReplacementIndex = replacementIndex;
							completionContext.ReplacementLength = replacementLength;
						}
						else if (!(ast2 is ErrorExpressionAst) || !(ast2.Parent is IndexExpressionAst))
						{
							return list;
						}
					}
				}
				if (isQuotedString)
				{
					return list;
				}
				StringExpandableToken stringExpandableToken = tokenAtCursor as StringExpandableToken;
				if (stringExpandableToken != null && stringExpandableToken.NestedTokens != null && stringConstantExpressionAst != null)
				{
					try
					{
						string wordToComplete = null;
						ExpandableStringExpressionAst expandableStringAst = new ExpandableStringExpressionAst(stringConstantExpressionAst.Extent, stringConstantExpressionAst.Value, StringConstantType.BareWord);
						if (!CompletionCompleters.IsPathSafelyExpandable(expandableStringAst, string.Empty, completionContext.ExecutionContext, out wordToComplete))
						{
							return list;
						}
						completionContext.WordToComplete = wordToComplete;
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
						return list;
					}
				}
				DynamicKeywordStatementAst keywordAst;
				ConfigurationDefinitionAst ancestorConfigurationAstAndKeywordAst = this.GetAncestorConfigurationAstAndKeywordAst(completionContext.CursorPosition, ast, out keywordAst);
				bool flag = false;
				List<CompletionResult> list3 = null;
				if (ancestorConfigurationAstAndKeywordAst != null)
				{
					list3 = this.GetResultForIdentifierInConfiguration(completionContext, ancestorConfigurationAstAndKeywordAst, keywordAst, out flag);
				}
				list = CompletionAnalysis.CompleteFileNameAsCommand(completionContext);
				List<CompletionResult> list4 = CompletionCompleters.CompleteCommand(completionContext);
				if (list4 != null && list4.Count > 0)
				{
					list.AddRange(list4);
				}
				if (flag && list3 != null)
				{
					list.InsertRange(0, list3);
				}
				else if (!flag && list3 != null && list4.Count == 0)
				{
					list.AddRange(list3);
				}
				return list;
			}
			else if (tokenAtCursor.Text.Length == 1 && tokenAtCursor.Text[0].IsDash() && (ast.Parent is CommandAst || ast.Parent is DynamicKeywordStatementAst))
			{
				if (isQuotedString)
				{
					return list;
				}
				return CompletionCompleters.CompleteCommandParameter(completionContext);
			}
			else
			{
				TokenKind tokenKind = TokenKind.Unknown;
				bool flag2 = ast.Parent is MemberExpressionAst;
				bool flag3 = flag2 && ((MemberExpressionAst)ast.Parent).Static;
				bool flag4 = false;
				if (!flag2)
				{
					if (tokenAtCursor.Text.Equals(TokenKind.Dot.Text(), StringComparison.Ordinal))
					{
						tokenKind = TokenKind.Dot;
						flag2 = true;
					}
					else if (tokenAtCursor.Text.Equals(TokenKind.ColonColon.Text(), StringComparison.Ordinal))
					{
						tokenKind = TokenKind.ColonColon;
						flag2 = true;
					}
					else if (tokenAtCursor.Kind.Equals(TokenKind.Multiply) && ast is BinaryExpressionAst)
					{
						BinaryExpressionAst binaryExpressionAst = (BinaryExpressionAst)ast;
						MemberExpressionAst memberExpressionAst = binaryExpressionAst.Left as MemberExpressionAst;
						IScriptExtent errorPosition = binaryExpressionAst.ErrorPosition;
						if (memberExpressionAst != null && binaryExpressionAst.Operator == TokenKind.Multiply && errorPosition.StartOffset == memberExpressionAst.Member.Extent.EndOffset)
						{
							flag3 = memberExpressionAst.Static;
							tokenKind = (flag3 ? TokenKind.ColonColon : TokenKind.Dot);
							flag2 = true;
							flag4 = true;
							completionContext.RelatedAsts.Remove(binaryExpressionAst);
							completionContext.RelatedAsts.Add(memberExpressionAst);
							StringConstantExpressionAst stringConstantExpressionAst2 = memberExpressionAst.Member as StringConstantExpressionAst;
							if (stringConstantExpressionAst2 != null)
							{
								replacementIndex = stringConstantExpressionAst2.Extent.StartScriptPosition.Offset;
								replacementLength += stringConstantExpressionAst2.Extent.Text.Length;
							}
						}
					}
				}
				if (flag2)
				{
					list = CompletionCompleters.CompleteMember(completionContext, flag3 || tokenKind == TokenKind.ColonColon);
					if (list.Any<CompletionResult>())
					{
						if (!flag4 && tokenKind != TokenKind.Unknown)
						{
							replacementIndex += tokenAtCursor.Text.Length;
							replacementLength = 0;
						}
						return list;
					}
				}
				if (ast.Parent is HashtableAst)
				{
					list = CompletionCompleters.CompleteHashtableKey(completionContext, (HashtableAst)ast.Parent);
					if (list != null && list.Any<CompletionResult>())
					{
						return list;
					}
				}
				if (isQuotedString)
				{
					return list;
				}
				bool flag5 = false;
				if (ast.Parent is FileRedirectionAst || CompletionAnalysis.CompleteAgainstSwitchFile(ast, completionContext.TokenBeforeCursor))
				{
					string text3 = CompletionCompleters.ConcatenateStringPathArguments(ast as CommandElementAst, string.Empty, completionContext);
					if (text3 != null)
					{
						flag5 = true;
						completionContext.WordToComplete = text3;
					}
				}
				else if (tokenAtCursor.Text.IndexOfAny(new char[]
				{
					'\\',
					'/'
				}) == 0)
				{
					CommandBaseAst commandBaseAst = ast.Parent as CommandBaseAst;
					if (commandBaseAst != null && commandBaseAst.Redirections.Any<RedirectionAst>())
					{
						FileRedirectionAst fileRedirectionAst = commandBaseAst.Redirections[0] as FileRedirectionAst;
						if (fileRedirectionAst != null && fileRedirectionAst.Extent.EndLineNumber == ast.Extent.StartLineNumber && fileRedirectionAst.Extent.EndColumnNumber == ast.Extent.StartColumnNumber)
						{
							string text4 = CompletionCompleters.ConcatenateStringPathArguments(fileRedirectionAst.Location, tokenAtCursor.Text, completionContext);
							if (text4 != null)
							{
								flag5 = true;
								completionContext.WordToComplete = text4;
								replacementIndex = fileRedirectionAst.Location.Extent.StartScriptPosition.Offset;
								replacementLength += fileRedirectionAst.Location.Extent.EndScriptPosition.Offset - replacementIndex;
								completionContext.ReplacementIndex = replacementIndex;
								completionContext.ReplacementLength = replacementLength;
							}
						}
					}
				}
				if (flag5)
				{
					return new List<CompletionResult>(CompletionCompleters.CompleteFilename(completionContext));
				}
				string text5 = CompletionCompleters.ConcatenateStringPathArguments(ast as CommandElementAst, string.Empty, completionContext);
				if (text5 != null)
				{
					completionContext.WordToComplete = text5;
				}
				list = CompletionCompleters.CompleteCommandArgument(completionContext);
				replacementIndex = completionContext.ReplacementIndex;
				replacementLength = completionContext.ReplacementLength;
				return list;
			}
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x001D596C File Offset: 0x001D3B6C
		private List<CompletionResult> GetResultForAttributeArgument(CompletionContext completionContext, ref int replacementIndex, ref int replacementLength)
		{
			Type type = null;
			string text = string.Empty;
			Ast ast3 = completionContext.RelatedAsts.Find((Ast ast) => ast is NamedAttributeArgumentAst);
			NamedAttributeArgumentAst namedAttributeArgumentAst = ast3 as NamedAttributeArgumentAst;
			if (ast3 != null && namedAttributeArgumentAst != null)
			{
				type = ((AttributeAst)namedAttributeArgumentAst.Parent).TypeName.GetReflectionAttributeType();
				text = namedAttributeArgumentAst.ArgumentName;
				replacementIndex = namedAttributeArgumentAst.Extent.StartOffset;
				replacementLength = text.Length;
			}
			else
			{
				Ast ast2 = completionContext.RelatedAsts.Find((Ast ast) => ast is AttributeAst);
				AttributeAst attributeAst = ast2 as AttributeAst;
				if (ast2 != null && attributeAst != null)
				{
					type = attributeAst.TypeName.GetReflectionAttributeType();
				}
			}
			if (type != null)
			{
				PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				List<CompletionResult> list = new List<CompletionResult>();
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.Name != "TypeId" && propertyInfo.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
					{
						list.Add(new CompletionResult(propertyInfo.Name, propertyInfo.Name, CompletionResultType.Property, propertyInfo.PropertyType.ToString() + " " + propertyInfo.Name));
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x06005940 RID: 22848 RVA: 0x001D5AD0 File Offset: 0x001D3CD0
		private static List<CompletionResult> CompleteFileNameAsCommand(CompletionContext completionContext)
		{
			bool flag = CompletionCompleters.IsAmpersandNeeded(completionContext, true);
			List<CompletionResult> list = new List<CompletionResult>();
			bool flag2 = false;
			if (completionContext.Options == null)
			{
				completionContext.Options = new Hashtable
				{
					{
						"LiteralPaths",
						true
					}
				};
			}
			else if (!completionContext.Options.ContainsKey("LiteralPaths"))
			{
				completionContext.Options.Add("LiteralPaths", true);
				flag2 = true;
			}
			try
			{
				IEnumerable<CompletionResult> enumerable = CompletionCompleters.CompleteFilename(completionContext);
				foreach (CompletionResult completionResult in enumerable)
				{
					string text = completionResult.CompletionText;
					int length = text.Length;
					if (flag && length > 2 && text[0].IsSingleQuote() && text[length - 1].IsSingleQuote())
					{
						text = "& " + text;
						list.Add(new CompletionResult(text, completionResult.ListItemText, completionResult.ResultType, completionResult.ToolTip));
					}
					else
					{
						list.Add(completionResult);
					}
				}
			}
			finally
			{
				if (flag2)
				{
					completionContext.Options.Remove("LiteralPaths");
				}
			}
			return list;
		}

		// Token: 0x04002FEA RID: 12266
		private readonly Ast _ast;

		// Token: 0x04002FEB RID: 12267
		private readonly Token[] _tokens;

		// Token: 0x04002FEC RID: 12268
		private readonly IScriptPosition _cursorPosition;

		// Token: 0x04002FED RID: 12269
		private readonly Hashtable _options;

		// Token: 0x04002FEE RID: 12270
		private static char[] newlineCharacters = new char[]
		{
			'\r',
			'\n'
		};
	}
}
