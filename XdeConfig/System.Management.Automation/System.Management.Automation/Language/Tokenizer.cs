using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Language
{
	// Token: 0x020005E3 RID: 1507
	[DebuggerDisplay("Mode = {Mode}; Script = {_script}")]
	internal class Tokenizer
	{
		// Token: 0x0600403D RID: 16445 RVA: 0x001523B4 File Offset: 0x001505B4
		static Tokenizer()
		{
			for (int i = 0; i < Tokenizer._keywordText.Length; i++)
			{
				Tokenizer._keywordTable.Add(Tokenizer._keywordText[i], Tokenizer._keywordTokenKind[i]);
			}
			for (int j = 0; j < Tokenizer._operatorText.Length; j++)
			{
				Tokenizer._operatorTable.Add(Tokenizer._operatorText[j], Tokenizer._operatorTokenKind[j]);
			}
			"sig#beginsignatureblock".Aggregate(0, (int current, char t) => current + (int)t);
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x00152BFA File Offset: 0x00150DFA
		internal Tokenizer(Parser parser)
		{
			this._parser = parser;
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x00152C09 File Offset: 0x00150E09
		// (set) Token: 0x06004040 RID: 16448 RVA: 0x00152C11 File Offset: 0x00150E11
		internal TokenizerMode Mode { get; set; }

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x00152C1A File Offset: 0x00150E1A
		// (set) Token: 0x06004042 RID: 16450 RVA: 0x00152C22 File Offset: 0x00150E22
		internal bool AllowSignedNumbers { get; set; }

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06004043 RID: 16451 RVA: 0x00152C2B File Offset: 0x00150E2B
		// (set) Token: 0x06004044 RID: 16452 RVA: 0x00152C33 File Offset: 0x00150E33
		internal bool WantSimpleName { get; set; }

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x00152C3C File Offset: 0x00150E3C
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x00152C44 File Offset: 0x00150E44
		internal bool InWorkflowContext { get; set; }

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x00152C4D File Offset: 0x00150E4D
		// (set) Token: 0x06004048 RID: 16456 RVA: 0x00152C55 File Offset: 0x00150E55
		internal List<Token> TokenList { get; set; }

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x00152C5E File Offset: 0x00150E5E
		// (set) Token: 0x0600404A RID: 16458 RVA: 0x00152C66 File Offset: 0x00150E66
		internal Token FirstToken { get; private set; }

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x00152C6F File Offset: 0x00150E6F
		// (set) Token: 0x0600404C RID: 16460 RVA: 0x00152C77 File Offset: 0x00150E77
		internal Token LastToken { get; private set; }

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x0600404D RID: 16461 RVA: 0x00152C80 File Offset: 0x00150E80
		// (set) Token: 0x0600404E RID: 16462 RVA: 0x00152C88 File Offset: 0x00150E88
		private List<Token> RequiresTokens { get; set; }

		// Token: 0x0600404F RID: 16463 RVA: 0x00152C91 File Offset: 0x00150E91
		private bool InCommandMode()
		{
			return this.Mode == TokenizerMode.Command;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x00152C9C File Offset: 0x00150E9C
		private bool InExpressionMode()
		{
			return this.Mode == TokenizerMode.Expression;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00152CA7 File Offset: 0x00150EA7
		private bool InTypeNameMode()
		{
			return this.Mode == TokenizerMode.TypeName;
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00152CB2 File Offset: 0x00150EB2
		private bool InSignatureMode()
		{
			return this.Mode == TokenizerMode.Signature;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x00152CC0 File Offset: 0x00150EC0
		internal void Initialize(string fileName, string input, List<Token> tokenList)
		{
			this._positionHelper = new PositionHelper(fileName, input);
			this._script = input;
			this.TokenList = tokenList;
			this.FirstToken = null;
			this.LastToken = null;
			this.RequiresTokens = null;
			this._beginSignatureExtent = null;
			List<int> list = new List<int>(100)
			{
				0
			};
			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];
				if (c == '\r')
				{
					if (i + 1 < input.Length && input[i + 1] == '\n')
					{
						i++;
					}
					list.Add(i + 1);
				}
				if (c == '\n')
				{
					list.Add(i + 1);
				}
			}
			this._currentIndex = 0;
			this.Mode = TokenizerMode.Command;
			this._positionHelper.LineStartMap = list.ToArray();
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x00152D88 File Offset: 0x00150F88
		internal TokenizerState StartNestedScan(UnscannedSubExprToken nestedText)
		{
			TokenizerState result = new TokenizerState
			{
				CurrentIndex = this._currentIndex,
				NestedTokensAdjustment = this._nestedTokensAdjustment,
				Script = this._script,
				TokenStart = this._tokenStart,
				LastToken = this.LastToken,
				SkippedCharOffsets = this._skippedCharOffsets,
				TokenList = this.TokenList
			};
			this._currentIndex = 0;
			this._nestedTokensAdjustment = ((InternalScriptExtent)nestedText.Extent).StartOffset;
			this._script = nestedText.Value;
			this._tokenStart = 0;
			this._skippedCharOffsets = nestedText.SkippedCharOffsets;
			this.TokenList = ((this.TokenList != null) ? new List<Token>() : null);
			return result;
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00152E44 File Offset: 0x00151044
		internal void FinishNestedScan(TokenizerState ts)
		{
			this._currentIndex = ts.CurrentIndex;
			this._nestedTokensAdjustment = ts.NestedTokensAdjustment;
			this._script = ts.Script;
			this._tokenStart = ts.TokenStart;
			this.LastToken = ts.LastToken;
			this._skippedCharOffsets = ts.SkippedCharOffsets;
			this.TokenList = ts.TokenList;
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x00152EA8 File Offset: 0x001510A8
		private char GetChar()
		{
			int num = this._currentIndex++;
			if (num >= this._script.Length)
			{
				return '\0';
			}
			return this._script[num];
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x00152EE3 File Offset: 0x001510E3
		private void UngetChar()
		{
			this._currentIndex--;
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00152EF3 File Offset: 0x001510F3
		private char PeekChar()
		{
			if (this._currentIndex == this._script.Length)
			{
				return '\0';
			}
			return this._script[this._currentIndex];
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x00152F1B File Offset: 0x0015111B
		private void SkipChar()
		{
			this._currentIndex++;
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x00152F2B File Offset: 0x0015112B
		private bool AtEof()
		{
			return this._currentIndex > this._script.Length;
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00152F40 File Offset: 0x00151140
		internal static bool IsKeyword(string str)
		{
			return Tokenizer._keywordTable.ContainsKey(str) || (DynamicKeyword.ContainsKeyword(str) && !DynamicKeyword.IsHiddenKeyword(str));
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x00152F64 File Offset: 0x00151164
		internal void SkipNewlines(bool skipSemis, bool v3)
		{
			for (;;)
			{
				char @char = this.GetChar();
				char c = @char;
				if (c <= '#')
				{
					switch (c)
					{
					case '\t':
					case '\v':
					case '\f':
						break;
					case '\n':
					case '\r':
						if (v3)
						{
							this._parser.NoteV3FeatureUsed();
						}
						if (this.TokenList != null)
						{
							this._tokenStart = this._currentIndex - 1;
							this.ScanNewline(@char);
							this.NewToken(TokenKind.NewLine);
							continue;
						}
						continue;
					default:
						if (c != ' ')
						{
							if (c != '#')
							{
								goto IL_177;
							}
							this._tokenStart = this._currentIndex - 1;
							this.ScanLineComment();
							continue;
						}
						break;
					}
				}
				else if (c <= '`')
				{
					switch (c)
					{
					case ';':
						if (!skipSemis)
						{
							goto IL_18A;
						}
						if (this.TokenList != null)
						{
							this._tokenStart = this._currentIndex - 1;
							this.NewToken(TokenKind.Semi);
							continue;
						}
						continue;
					case '<':
						if (this.PeekChar() == '#')
						{
							this._tokenStart = this._currentIndex - 1;
							this.SkipChar();
							this.ScanBlockComment();
							continue;
						}
						goto IL_18A;
					default:
					{
						if (c != '`')
						{
							goto IL_177;
						}
						char char2 = this.GetChar();
						if (char2 == '\n' || char2 == '\r')
						{
							this._tokenStart = this._currentIndex - 2;
							this.ScanNewline(char2);
							this.NewToken(TokenKind.LineContinuation);
							continue;
						}
						if (char.IsWhiteSpace(char2))
						{
							this.SkipWhiteSpace();
							continue;
						}
						goto IL_16F;
					}
					}
				}
				else if (c != '\u0085' && c != '\u00a0')
				{
					goto IL_177;
				}
				this.SkipWhiteSpace();
				continue;
				IL_177:
				if (!@char.IsWhitespace())
				{
					goto IL_18A;
				}
				this.SkipWhiteSpace();
			}
			IL_16F:
			this.UngetChar();
			IL_18A:
			this.UngetChar();
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x00153104 File Offset: 0x00151304
		private void SkipWhiteSpace()
		{
			for (;;)
			{
				char c = this.PeekChar();
				if (!c.IsWhitespace())
				{
					break;
				}
				this.SkipChar();
			}
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x00153128 File Offset: 0x00151328
		internal int GetRestorePoint()
		{
			this._tokenStart = this._currentIndex;
			return this.CurrentExtent().StartOffset;
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x00153141 File Offset: 0x00151341
		internal void Resync(Token token)
		{
			this.Resync(((InternalScriptExtent)token.Extent).StartOffset);
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0015315C File Offset: 0x0015135C
		internal void Resync(int start)
		{
			int num = this._nestedTokensAdjustment;
			if (this._skippedCharOffsets != null)
			{
				int num2 = this._nestedTokensAdjustment;
				while (num2 < start - 1 && num2 < this._skippedCharOffsets.Length)
				{
					if (this._skippedCharOffsets[num2])
					{
						num++;
					}
					num2++;
				}
			}
			this._currentIndex = start - num;
			if (this._currentIndex > this._script.Length + 1)
			{
				this._currentIndex = this._script.Length + 1;
			}
			else if (0 > this._currentIndex)
			{
				this._currentIndex = 0;
			}
			if (this.FirstToken != null && this._currentIndex <= ((InternalScriptExtent)this.FirstToken.Extent).StartOffset)
			{
				this.FirstToken = null;
			}
			if (this.TokenList != null && this.TokenList.Count > 0)
			{
				this.RemoveTokensFromListDuringResync(this.TokenList, start);
			}
			if (this.RequiresTokens != null && this.RequiresTokens.Count > 0)
			{
				this.RemoveTokensFromListDuringResync(this.RequiresTokens, start);
			}
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00153260 File Offset: 0x00151460
		internal void RemoveTokensFromListDuringResync(List<Token> tokenList, int start)
		{
			int num = 0;
			int i = tokenList.Count - 1;
			if (i >= 0 && tokenList[i].Kind == TokenKind.EndOfInput)
			{
				i--;
			}
			while (i >= 0)
			{
				if (((InternalScriptExtent)tokenList[i].Extent).EndOffset <= start)
				{
					num = i + 1;
					break;
				}
				i--;
			}
			tokenList.RemoveRange(num, tokenList.Count - num);
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x001532C8 File Offset: 0x001514C8
		internal void ReplaceSavedTokens(Token firstOldToken, Token lastOldToken, Token newToken)
		{
			int startOffset = ((InternalScriptExtent)firstOldToken.Extent).StartOffset;
			int endOffset = ((InternalScriptExtent)lastOldToken.Extent).EndOffset;
			int num = -1;
			for (int i = this.TokenList.Count - 1; i >= 0; i--)
			{
				if (((InternalScriptExtent)this.TokenList[i].Extent).EndOffset == endOffset)
				{
					num = i;
				}
				else if (((InternalScriptExtent)this.TokenList[i].Extent).StartOffset == startOffset)
				{
					this.TokenList.RemoveRange(i, num - i + 1);
					this.TokenList.Insert(i, newToken);
					return;
				}
			}
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x00153370 File Offset: 0x00151570
		private void ScanNewline(char c)
		{
			if (c == '\r' && this.PeekChar() == '\n')
			{
				this.SkipChar();
			}
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x00153388 File Offset: 0x00151588
		internal void CheckAstIsBeforeSignature(Ast ast)
		{
			if (this._beginSignatureExtent == null)
			{
				return;
			}
			if (this._beginSignatureExtent.StartOffset < ast.Extent.StartOffset)
			{
				this.ReportError(ast.Extent, () => ParserStrings.TokenAfterEndOfValidScriptText);
			}
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x001533E2 File Offset: 0x001515E2
		private void ReportError(int errorOffset, Expression<Func<string>> message, params object[] args)
		{
			this._parser.ReportError(this.NewScriptExtent(errorOffset, errorOffset + 1), message, args);
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x001533FB File Offset: 0x001515FB
		private void ReportError(IScriptExtent extent, Expression<Func<string>> message)
		{
			this._parser.ReportError(extent, message);
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0015340A File Offset: 0x0015160A
		private void ReportError(IScriptExtent extent, Expression<Func<string>> message, object arg)
		{
			this._parser.ReportError(extent, message, arg);
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0015341A File Offset: 0x0015161A
		private void ReportError(IScriptExtent extent, Expression<Func<string>> message, object arg1, object arg2)
		{
			this._parser.ReportError(extent, message, arg1, arg2);
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0015342C File Offset: 0x0015162C
		private void ReportIncompleteInput(int errorOffset, Expression<Func<string>> message)
		{
			this._parser.ReportIncompleteInput(this.NewScriptExtent(errorOffset, this._currentIndex), message);
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x00153448 File Offset: 0x00151648
		private void ReportIncompleteInput(int errorOffset, Expression<Func<string>> message, object arg)
		{
			this._parser.ReportIncompleteInput(this.NewScriptExtent(errorOffset, this._currentIndex), message, arg);
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x00153465 File Offset: 0x00151665
		private InternalScriptExtent NewScriptExtent(int start, int end)
		{
			return new InternalScriptExtent(this._positionHelper, start + this._nestedTokensAdjustment, end + this._nestedTokensAdjustment);
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x00153484 File Offset: 0x00151684
		internal InternalScriptExtent CurrentExtent()
		{
			int num = this._tokenStart + this._nestedTokensAdjustment;
			int num2 = this._currentIndex + this._nestedTokensAdjustment;
			if (this._skippedCharOffsets != null)
			{
				int i;
				for (i = this._nestedTokensAdjustment; i < num; i++)
				{
					if (i >= this._skippedCharOffsets.Length)
					{
						break;
					}
					if (this._skippedCharOffsets[i])
					{
						num++;
						num2++;
					}
				}
				while (i < num2 && i < this._skippedCharOffsets.Length)
				{
					if (this._skippedCharOffsets[i])
					{
						num2++;
					}
					i++;
				}
			}
			return new InternalScriptExtent(this._positionHelper, num, num2);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x00153521 File Offset: 0x00151721
		internal IScriptExtent GetScriptExtent()
		{
			return this.NewScriptExtent(0, this._script.Length);
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x00153535 File Offset: 0x00151735
		private Token NewCommentToken()
		{
			return this.SaveToken<Token>(new Token(this.CurrentExtent(), TokenKind.Comment, TokenFlags.None));
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0015354C File Offset: 0x0015174C
		private T SaveToken<T>(T token) where T : Token
		{
			if (this.TokenList != null)
			{
				this.TokenList.Add(token);
			}
			switch (token.Kind)
			{
			case TokenKind.NewLine:
			case TokenKind.LineContinuation:
			case TokenKind.Comment:
			case TokenKind.EndOfInput:
				break;
			default:
				if (this.FirstToken == null)
				{
					this.FirstToken = token;
				}
				this.LastToken = token;
				break;
			}
			return token;
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x001535B9 File Offset: 0x001517B9
		private Token NewToken(TokenKind kind)
		{
			return this.SaveToken<Token>(new Token(this.CurrentExtent(), kind, TokenFlags.None));
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x001535CE File Offset: 0x001517CE
		private Token NewNumberToken(object value)
		{
			return this.SaveToken<NumberToken>(new NumberToken(this.CurrentExtent(), value, TokenFlags.None));
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x001535E3 File Offset: 0x001517E3
		private Token NewParameterToken(string name, bool sawColon)
		{
			return this.SaveToken<ParameterToken>(new ParameterToken(this.CurrentExtent(), name, sawColon));
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x001535F8 File Offset: 0x001517F8
		private VariableToken NewVariableToken(VariablePath path, bool splatted)
		{
			return this.SaveToken<VariableToken>(new VariableToken(this.CurrentExtent(), path, TokenFlags.None, splatted));
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0015360E File Offset: 0x0015180E
		private StringToken NewStringLiteralToken(string value, TokenKind tokenKind, TokenFlags flags)
		{
			return this.SaveToken<StringLiteralToken>(new StringLiteralToken(this.CurrentExtent(), flags, tokenKind, value));
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0015362C File Offset: 0x0015182C
		private StringToken NewStringExpandableToken(string value, string formatString, TokenKind tokenKind, List<Token> nestedTokens, TokenFlags flags)
		{
			if (nestedTokens != null && nestedTokens.Count == 0)
			{
				nestedTokens = null;
			}
			else if ((flags & TokenFlags.TokenInError) == TokenFlags.None)
			{
				if (nestedTokens.Any((Token tok) => tok.HasError))
				{
					flags |= TokenFlags.TokenInError;
				}
			}
			return this.SaveToken<StringExpandableToken>(new StringExpandableToken(this.CurrentExtent(), tokenKind, value, formatString, nestedTokens, flags));
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0015369D File Offset: 0x0015189D
		private Token NewGenericExpandableToken(string value, string formatString, List<Token> nestedTokens)
		{
			return this.NewStringExpandableToken(value, formatString, TokenKind.Generic, nestedTokens, TokenFlags.None);
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x001536AA File Offset: 0x001518AA
		private Token NewGenericToken(string value)
		{
			return this.NewStringLiteralToken(value, TokenKind.Generic, TokenFlags.None);
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x001536B5 File Offset: 0x001518B5
		private Token NewInputRedirectionToken()
		{
			return this.SaveToken<InputRedirectionToken>(new InputRedirectionToken(this.CurrentExtent()));
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x001536C8 File Offset: 0x001518C8
		private Token NewFileRedirectionToken(int from, bool append, bool fromSpecifiedExplicitly)
		{
			if (fromSpecifiedExplicitly && this.InExpressionMode())
			{
				this.UngetChar();
				if (append)
				{
					this.UngetChar();
				}
				return this.NewNumberToken(from);
			}
			return this.SaveToken<FileRedirectionToken>(new FileRedirectionToken(this.CurrentExtent(), (RedirectionStream)from, append));
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x00153704 File Offset: 0x00151904
		private Token NewMergingRedirectionToken(int from, int to)
		{
			return this.SaveToken<MergingRedirectionToken>(new MergingRedirectionToken(this.CurrentExtent(), (RedirectionStream)from, (RedirectionStream)to));
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x00153719 File Offset: 0x00151919
		private LabelToken NewLabelToken(string value)
		{
			return this.SaveToken<LabelToken>(new LabelToken(this.CurrentExtent(), TokenFlags.None, value));
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x00153730 File Offset: 0x00151930
		internal bool IsAtEndOfScript(IScriptExtent extent, bool checkCommentsAndWhitespace = false)
		{
			InternalScriptExtent internalScriptExtent = (InternalScriptExtent)extent;
			return internalScriptExtent.EndOffset >= this._script.Length || (checkCommentsAndWhitespace && this.OnlyWhitespaceOrCommentsAfterExtent(internalScriptExtent));
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x00153768 File Offset: 0x00151968
		private bool OnlyWhitespaceOrCommentsAfterExtent(InternalScriptExtent extent)
		{
			for (int i = extent.EndOffset; i < this._script.Length; i++)
			{
				if (this._script[i] == '#')
				{
					i = this.SkipLineComment(i + 1) - 1;
				}
				else if (this._script[i] == '<' && i + 1 < this._script.Length && this._script[i + 1] == '#')
				{
					i = this.SkipBlockComment(i + 2) - 1;
				}
				else if (!this._script[i].IsWhitespace())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x00153808 File Offset: 0x00151A08
		private int SkipLineComment(int i)
		{
			while (i < this._script.Length)
			{
				char c = this._script[i];
				if (c == '\r' || c == '\n')
				{
					break;
				}
				i++;
			}
			return i;
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00153844 File Offset: 0x00151A44
		private int SkipBlockComment(int i)
		{
			while (i < this._script.Length)
			{
				char c = this._script[i];
				if (c == '#' && i + 1 < this._script.Length && this._script[i + 1] == '>')
				{
					return i + 2;
				}
				i++;
			}
			return i;
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x001538A0 File Offset: 0x00151AA0
		private static char Backtick(char c)
		{
			if (c <= 'b')
			{
				if (c == '0')
				{
					return '\0';
				}
				switch (c)
				{
				case 'a':
					return '\a';
				case 'b':
					return '\b';
				}
			}
			else
			{
				if (c == 'f')
				{
					return '\f';
				}
				if (c == 'n')
				{
					return '\n';
				}
				switch (c)
				{
				case 'r':
					return '\r';
				case 't':
					return '\t';
				case 'v':
					return '\v';
				}
			}
			return c;
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0015390C File Offset: 0x00151B0C
		private void ScanToEndOfCommentLine(out bool sawBeginSig, out bool matchedRequires)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			matchedRequires = false;
			for (;;)
			{
				char @char = this.GetChar();
				if (!@char.IsWhitespace())
				{
					stringBuilder.Append(@char);
				}
				char c = @char;
				if (c <= 'E')
				{
					if (c <= '\n')
					{
						if (c != '\0')
						{
							if (c != '\n')
							{
								goto IL_180;
							}
							break;
						}
						else
						{
							if (this.AtEof())
							{
								break;
							}
							goto IL_180;
						}
					}
					else
					{
						if (c == '\r')
						{
							break;
						}
						if (c != 'E')
						{
							goto IL_180;
						}
					}
				}
				else
				{
					if (c <= 'U')
					{
						if (c != 'I')
						{
							switch (c)
							{
							case 'Q':
								goto IL_E5;
							case 'R':
								goto IL_F9;
							case 'S':
								goto IL_110;
							case 'T':
								goto IL_180;
							case 'U':
								goto IL_123;
							default:
								goto IL_180;
							}
						}
					}
					else
					{
						if (c == 'e')
						{
							goto IL_B9;
						}
						if (c != 'i')
						{
							switch (c)
							{
							case 'q':
								goto IL_E5;
							case 'r':
								goto IL_F9;
							case 's':
								goto IL_110;
							case 't':
								goto IL_180;
							case 'u':
								goto IL_123;
							default:
								goto IL_180;
							}
						}
					}
					if (num == 4)
					{
						num++;
						continue;
					}
					num = -1;
					continue;
					IL_E5:
					if (num == 2)
					{
						num++;
						continue;
					}
					num = -1;
					continue;
					IL_F9:
					if (num == 0 || num == 5)
					{
						num++;
						continue;
					}
					num = -1;
					continue;
					IL_110:
					if (num == 7)
					{
						matchedRequires = true;
						continue;
					}
					num = -1;
					continue;
					IL_123:
					if (num == 3)
					{
						num++;
						continue;
					}
					num = -1;
					continue;
				}
				IL_B9:
				if (num == 1 || num == 6)
				{
					num++;
					continue;
				}
				num = -1;
				continue;
				IL_180:
				num = -1;
			}
			this.UngetChar();
			if (stringBuilder.Length > "sig#beginsignatureblock\n".Length + 10)
			{
				sawBeginSig = false;
				return;
			}
			string first = stringBuilder.ToString().ToLowerInvariant();
			int stringSimilarity = Tokenizer.GetStringSimilarity(first, "sig#beginsignatureblock\n");
			sawBeginSig = (stringSimilarity < 10);
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x00153AA0 File Offset: 0x00151CA0
		private void ScanLineComment()
		{
			bool flag;
			bool flag2;
			this.ScanToEndOfCommentLine(out flag, out flag2);
			Token item = this.NewCommentToken();
			if (flag)
			{
				this._beginSignatureExtent = this.CurrentExtent();
				return;
			}
			if (flag2 && this._nestedTokensAdjustment == 0)
			{
				if (this.RequiresTokens == null)
				{
					this.RequiresTokens = new List<Token>();
				}
				this.RequiresTokens.Add(item);
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x00153AF8 File Offset: 0x00151CF8
		private void ScanBlockComment()
		{
			int errorOffset = this._currentIndex - 2;
			for (;;)
			{
				char @char = this.GetChar();
				if (@char == '#' && this.PeekChar() == '>')
				{
					break;
				}
				if (@char == '\r' || @char == '\n')
				{
					this.ScanNewline(@char);
				}
				else if (@char == '\0' && this.AtEof())
				{
					goto Block_5;
				}
			}
			this.SkipChar();
			goto IL_72;
			Block_5:
			this.UngetChar();
			this.ReportIncompleteInput(errorOffset, () => ParserStrings.MissingTerminatorMultiLineComment);
			IL_72:
			this.NewCommentToken();
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x00153B80 File Offset: 0x00151D80
		private static int GetStringSimilarity(string first, string second)
		{
			int[,] array = new int[first.Length + 1, second.Length + 1];
			for (int i = 0; i <= first.Length; i++)
			{
				array[i, 0] = i;
			}
			for (int j = 0; j <= second.Length; j++)
			{
				array[0, j] = j;
			}
			for (int k = 1; k <= first.Length; k++)
			{
				for (int l = 1; l <= second.Length; l++)
				{
					if (first[k - 1] == second[l - 1])
					{
						array[k, l] = array[k - 1, l - 1];
					}
					else
					{
						array[k, l] = Math.Min(Math.Min(array[k - 1, l] + 1, array[k, l - 1] + 1), array[k - 1, l - 1] + 1);
					}
				}
			}
			return array[first.Length, second.Length];
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x00153C80 File Offset: 0x00151E80
		internal ScriptRequirements GetScriptRequirements()
		{
			if (this.RequiresTokens == null)
			{
				return null;
			}
			Token[] array = this.RequiresTokens.ToArray();
			this.RequiresTokens = null;
			string requiredApplicationId = null;
			Version requiredPSVersion = null;
			List<ModuleSpecification> list = null;
			List<PSSnapInSpecification> list2 = null;
			List<string> list3 = null;
			bool isElevationRequired = false;
			foreach (Token token in array)
			{
				InternalScriptExtent internalScriptExtent = new InternalScriptExtent(this._positionHelper, token.Extent.StartOffset + 1, token.Extent.EndOffset);
				TokenizerState ts = this.StartNestedScan(new UnscannedSubExprToken(internalScriptExtent, TokenFlags.None, internalScriptExtent.Text, null));
				CommandAst commandAst = this._parser.CommandRule(false) as CommandAst;
				this._parser._ungotToken = null;
				this.FinishNestedScan(ts);
				string text = null;
				Version version = null;
				if (commandAst != null)
				{
					string commandName = commandAst.GetCommandName();
					if (!string.Equals(commandName, "requires", StringComparison.OrdinalIgnoreCase))
					{
						this.ReportError(commandAst.Extent, () => DiscoveryExceptions.ScriptRequiresInvalidFormat);
					}
					bool snapinSpecified = false;
					int j = 1;
					while (j < commandAst.CommandElements.Count)
					{
						CommandParameterAst commandParameterAst = commandAst.CommandElements[j] as CommandParameterAst;
						if (commandParameterAst != null && "pssnapin".StartsWith(commandParameterAst.ParameterName, StringComparison.OrdinalIgnoreCase))
						{
							snapinSpecified = true;
							if (list2 == null)
							{
								list2 = new List<PSSnapInSpecification>();
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
					for (int k = 1; k < commandAst.CommandElements.Count; k++)
					{
						CommandParameterAst commandParameterAst2 = commandAst.CommandElements[k] as CommandParameterAst;
						if (commandParameterAst2 != null)
						{
							this.HandleRequiresParameter(commandParameterAst2, commandAst.CommandElements, snapinSpecified, ref k, ref text, ref version, ref requiredApplicationId, ref requiredPSVersion, ref list, ref list3, ref isElevationRequired);
						}
						else
						{
							this.ReportError(commandAst.CommandElements[k].Extent, () => DiscoveryExceptions.ScriptRequiresInvalidFormat);
						}
					}
					if (text != null)
					{
						list2.Add(new PSSnapInSpecification(text)
						{
							Version = version
						});
					}
				}
			}
			return new ScriptRequirements
			{
				RequiredApplicationId = requiredApplicationId,
				RequiredPSVersion = requiredPSVersion,
				RequiresPSSnapIns = ((list2 != null) ? new ReadOnlyCollection<PSSnapInSpecification>(list2) : ScriptRequirements.EmptySnapinCollection),
				RequiredAssemblies = ((list3 != null) ? new ReadOnlyCollection<string>(list3) : ScriptRequirements.EmptyAssemblyCollection),
				RequiredModules = ((list != null) ? new ReadOnlyCollection<ModuleSpecification>(list) : ScriptRequirements.EmptyModuleCollection),
				IsElevationRequired = isElevationRequired
			};
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x00153F0C File Offset: 0x0015210C
		private void HandleRequiresParameter(CommandParameterAst parameter, ReadOnlyCollection<CommandElementAst> commandElements, bool snapinSpecified, ref int index, ref string snapinName, ref Version snapinVersion, ref string requiredShellId, ref Version requiredVersion, ref List<ModuleSpecification> requiredModules, ref List<string> requiredAssemblies, ref bool requiresElevation)
		{
			ExpressionAst expressionAst;
			if ((expressionAst = parameter.Argument) == null)
			{
				expressionAst = ((index + 1 < commandElements.Count) ? commandElements[++index] : null);
			}
			Ast ast = expressionAst;
			if ("runasadministrator".StartsWith(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
			{
				requiresElevation = true;
				if (ast != null)
				{
					this.ReportError(parameter.Extent, () => ParserStrings.ParameterCannotHaveArgument, parameter.ParameterName);
				}
				return;
			}
			if (ast == null)
			{
				this.ReportError(parameter.Extent, () => ParserStrings.ParameterRequiresArgument, parameter.ParameterName);
				return;
			}
			object obj;
			if (!IsConstantValueVisitor.IsConstant(ast, out obj, false, true))
			{
				this.ReportError(ast.Extent, () => ParserStrings.RequiresArgumentMustBeConstant);
				return;
			}
			if ("shellid".StartsWith(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
			{
				if (requiredShellId != null)
				{
					this.ReportError(parameter.Extent, () => ParameterBinderStrings.ParameterAlreadyBound, null, "shellid");
					return;
				}
				if (!(obj is string))
				{
					this.ReportError(ast.Extent, () => ParserStrings.RequiresInvalidStringArgument, "shellid");
					return;
				}
				requiredShellId = (string)obj;
				return;
			}
			else if ("pssnapin".StartsWith(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
			{
				if (!(obj is string))
				{
					this.ReportError(ast.Extent, () => ParserStrings.RequiresInvalidStringArgument, "pssnapin");
					return;
				}
				if (snapinName != null)
				{
					this.ReportError(parameter.Extent, () => ParameterBinderStrings.ParameterAlreadyBound, null, "pssnapin");
					return;
				}
				if (!PSSnapInInfo.IsPSSnapinIdValid((string)obj))
				{
					this.ReportError(ast.Extent, () => MshSnapInCmdletResources.InvalidPSSnapInName);
					return;
				}
				snapinName = (string)obj;
				return;
			}
			else
			{
				if (!"version".StartsWith(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
				{
					if ("assembly".StartsWith(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
					{
						if (obj is string || !(obj is IEnumerable))
						{
							requiredAssemblies = this.HandleRequiresAssemblyArgument(ast, obj, requiredAssemblies);
							return;
						}
						using (IEnumerator enumerator = ((IEnumerable)obj).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object arg = enumerator.Current;
								requiredAssemblies = this.HandleRequiresAssemblyArgument(ast, arg, requiredAssemblies);
							}
							return;
						}
					}
					if ("modules".StartsWith(parameter.ParameterName, StringComparison.OrdinalIgnoreCase))
					{
						object[] array;
						if ((array = (obj as object[])) == null)
						{
							array = new object[]
							{
								obj
							};
						}
						object[] array2 = array;
						foreach (object valueToConvert in array2)
						{
							ModuleSpecification item;
							try
							{
								item = LanguagePrimitives.ConvertTo<ModuleSpecification>(valueToConvert);
							}
							catch (InvalidCastException ex)
							{
								this.ReportError(ast.Extent, () => ParserStrings.RequiresModuleInvalid, ex.Message);
								return;
							}
							catch (ArgumentException ex2)
							{
								this.ReportError(ast.Extent, () => ParserStrings.RequiresModuleInvalid, ex2.Message);
								return;
							}
							if (requiredModules == null)
							{
								requiredModules = new List<ModuleSpecification>();
							}
							requiredModules.Add(item);
						}
						return;
					}
					this.ReportError(parameter.Extent, () => DiscoveryExceptions.ScriptRequiresInvalidFormat);
					return;
				}
				string versionString = (obj as string) ?? ast.Extent.Text;
				Version version = Utils.StringToVersion(versionString);
				if (version == null)
				{
					this.ReportError(ast.Extent, () => ParserStrings.RequiresVersionInvalid);
					return;
				}
				if (snapinSpecified)
				{
					if (snapinVersion != null)
					{
						this.ReportError(parameter.Extent, () => ParameterBinderStrings.ParameterAlreadyBound, null, "version");
						return;
					}
					snapinVersion = version;
					return;
				}
				else
				{
					if (requiredVersion != null && !requiredVersion.Equals(version))
					{
						this.ReportError(parameter.Extent, () => ParameterBinderStrings.ParameterAlreadyBound, null, "version");
						return;
					}
					requiredVersion = version;
					return;
				}
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x00154414 File Offset: 0x00152614
		private List<string> HandleRequiresAssemblyArgument(Ast argumentAst, object arg, List<string> requiredAssemblies)
		{
			if (!(arg is string))
			{
				this.ReportError(argumentAst.Extent, () => ParserStrings.RequiresInvalidStringArgument, "assembly");
			}
			else
			{
				if (requiredAssemblies == null)
				{
					requiredAssemblies = new List<string>();
				}
				if (!requiredAssemblies.Contains((string)arg))
				{
					requiredAssemblies.Add((string)arg);
				}
			}
			return requiredAssemblies;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x00154484 File Offset: 0x00152684
		internal StringToken GetVerbatimCommandArgument()
		{
			this.SkipWhiteSpace();
			this._tokenStart = this._currentIndex;
			bool flag = false;
			for (;;)
			{
				char @char = this.GetChar();
				if (@char == '\r' || @char == '\n' || (@char == '\0' && this.AtEof()))
				{
					break;
				}
				if (@char.IsDoubleQuote())
				{
					flag = !flag;
				}
				else if (!flag && (@char == '|' || (@char == '&' && !this.AtEof() && this.PeekChar() == '&')))
				{
					goto IL_66;
				}
			}
			this.UngetChar();
			goto IL_6C;
			IL_66:
			this.UngetChar();
			IL_6C:
			InternalScriptExtent internalScriptExtent = this.CurrentExtent();
			string text = internalScriptExtent.Text;
			return this.NewStringLiteralToken(text, TokenKind.Generic, TokenFlags.None);
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00154514 File Offset: 0x00152714
		private TokenFlags ScanStringLiteral(StringBuilder sb)
		{
			int errorOffset = this._currentIndex - 1;
			TokenFlags result = TokenFlags.None;
			char @char = this.GetChar();
			while (@char != '\0' || !this.AtEof())
			{
				if (@char.IsSingleQuote())
				{
					if (!this.PeekChar().IsSingleQuote())
					{
						break;
					}
					@char = this.GetChar();
				}
				sb.Append(@char);
				@char = this.GetChar();
			}
			if (@char == '\0')
			{
				this.UngetChar();
				this.ReportIncompleteInput(errorOffset, () => ParserStrings.TerminatorExpectedAtEndOfString, "'");
				result = TokenFlags.TokenInError;
			}
			return result;
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x001545A8 File Offset: 0x001527A8
		private Token ScanStringLiteral()
		{
			StringBuilder stringBuilder = new StringBuilder();
			TokenFlags flags = this.ScanStringLiteral(stringBuilder);
			return this.NewStringLiteralToken(stringBuilder.ToString(), TokenKind.StringLiteral, flags);
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x001545D4 File Offset: 0x001527D4
		private Token ScanSubExpression(bool hereString)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			this._tokenStart = this._currentIndex - 2;
			StringBuilder stringBuilder = new StringBuilder("$(");
			int num = 1;
			TokenFlags tokenFlags = TokenFlags.None;
			bool flag = true;
			List<int> list = new List<int>();
			while (flag)
			{
				char @char = this.GetChar();
				char c = @char;
				if (c <= '"')
				{
					if (c != '\0')
					{
						if (c != '"')
						{
							goto IL_141;
						}
					}
					else
					{
						if (this.AtEof())
						{
							this.UngetChar();
							this.ReportIncompleteInput(this._tokenStart, () => ParserStrings.IncompleteDollarSubexpressionReference);
							tokenFlags = TokenFlags.TokenInError;
							flag = false;
							continue;
						}
						goto IL_141;
					}
				}
				else
				{
					switch (c)
					{
					case '(':
						stringBuilder.Append(@char);
						num++;
						continue;
					case ')':
						stringBuilder.Append(@char);
						if (--num == 0)
						{
							flag = false;
							continue;
						}
						continue;
					default:
						if (c != '`')
						{
							switch (c)
							{
							case '“':
							case '”':
							case '„':
								break;
							default:
								goto IL_141;
							}
						}
						break;
					}
				}
				char c2 = this.PeekChar();
				if (!hereString && c2.IsDoubleQuote())
				{
					this.SkipChar();
					stringBuilder.Append(c2);
					list.Add(this._currentIndex - 2 + this._nestedTokensAdjustment);
					continue;
				}
				stringBuilder.Append(@char);
				continue;
				IL_141:
				stringBuilder.Append(@char);
			}
			BitArray bitArray;
			if (list.Count > 0)
			{
				bitArray = new BitArray(list.Last<int>() + 1);
				using (List<int>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int index = enumerator.Current;
						bitArray.Set(index, true);
					}
					goto IL_1A9;
				}
			}
			bitArray = this._skippedCharOffsets;
			IL_1A9:
			InternalScriptExtent scriptExtent = this.CurrentExtent();
			return new UnscannedSubExprToken(scriptExtent, tokenFlags, stringBuilder.ToString(), bitArray);
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x001547B4 File Offset: 0x001529B4
		private TokenFlags ScanStringExpandable(StringBuilder sb, StringBuilder formatSb, List<Token> nestedTokens)
		{
			TokenFlags result = TokenFlags.None;
			int errorOffset = this._currentIndex - 1;
			char c = this.GetChar();
			while (c != '\0' || !this.AtEof())
			{
				if (c.IsDoubleQuote())
				{
					if (this.PeekChar().IsDoubleQuote())
					{
						c = this.GetChar();
						goto IL_61;
					}
					break;
				}
				else if (c == '$')
				{
					if (!this.ScanDollarInStringExpandable(sb, formatSb, false, nestedTokens))
					{
						goto IL_61;
					}
				}
				else
				{
					if (c != '`')
					{
						goto IL_61;
					}
					char c2 = this.PeekChar();
					if (c2 != '\0')
					{
						this.SkipChar();
						c = Tokenizer.Backtick(c2);
						goto IL_61;
					}
					goto IL_61;
				}
				IL_83:
				c = this.GetChar();
				continue;
				IL_61:
				if (c == '{' || c == '}')
				{
					formatSb.Append(c);
				}
				sb.Append(c);
				formatSb.Append(c);
				goto IL_83;
			}
			if (c == '\0')
			{
				this.UngetChar();
				this.ReportIncompleteInput(errorOffset, () => ParserStrings.TerminatorExpectedAtEndOfString, "\"");
				result = TokenFlags.TokenInError;
			}
			return result;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x00154898 File Offset: 0x00152A98
		private bool ScanDollarInStringExpandable(StringBuilder sb, StringBuilder formatSb, bool hereString, List<Token> nestedTokens)
		{
			int num = this._currentIndex - 1;
			char c = this.PeekChar();
			int tokenStart = this._tokenStart;
			TokenizerMode mode = this.Mode;
			List<Token> tokenList = this.TokenList;
			Token token = null;
			try
			{
				this.TokenList = null;
				this.Mode = TokenizerMode.Expression;
				if (c == '(')
				{
					this.SkipChar();
					token = this.ScanSubExpression(hereString);
				}
				else if (c.IsVariableStart() || c == '{')
				{
					this._tokenStart = this._currentIndex - 1;
					token = this.ScanVariable(false, true);
				}
			}
			finally
			{
				this.TokenList = tokenList;
				this._tokenStart = tokenStart;
				this.Mode = mode;
			}
			if (token != null)
			{
				sb.Append(this._script, num, this._currentIndex - num);
				formatSb.Append('{');
				formatSb.Append(nestedTokens.Count);
				formatSb.Append('}');
				nestedTokens.Add(token);
				return true;
			}
			return false;
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x00154988 File Offset: 0x00152B88
		private Token ScanStringExpandable()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			List<Token> nestedTokens = new List<Token>();
			TokenFlags flags = this.ScanStringExpandable(stringBuilder, stringBuilder2, nestedTokens);
			return this.NewStringExpandableToken(stringBuilder.ToString(), stringBuilder2.ToString(), TokenKind.StringExpandable, nestedTokens, flags);
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x001549C8 File Offset: 0x00152BC8
		private bool ScanAfterHereStringHeader(string header)
		{
			int errorOffset = this._currentIndex - 2;
			char @char;
			do
			{
				@char = this.GetChar();
			}
			while (@char.IsWhitespace());
			if (@char == '\r' || @char == '\n')
			{
				this.ScanNewline(@char);
				return true;
			}
			if (@char == '\0' && this.AtEof())
			{
				this.UngetChar();
				this.ReportIncompleteInput(errorOffset, () => ParserStrings.TerminatorExpectedAtEndOfString, header[1] + '@');
				return false;
			}
			this.UngetChar();
			this.ReportError(this._currentIndex, () => ParserStrings.UnexpectedCharactersAfterHereStringHeader, new object[0]);
			for (;;)
			{
				@char = this.GetChar();
				if (@char == header[1] && this.PeekChar() == '@')
				{
					break;
				}
				if (@char == '\r' || @char == '\n' || (@char == '\0' && this.AtEof()))
				{
					goto IL_F0;
				}
			}
			this.SkipChar();
			return false;
			IL_F0:
			this.UngetChar();
			return false;
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x00154AD0 File Offset: 0x00152CD0
		private bool ScanPossibleHereStringFooter(Func<char, bool> test, Action<char> appendChar, ref int falseFooterOffset)
		{
			char @char = this.GetChar();
			if (test(@char) && this.PeekChar() == '@')
			{
				this.SkipChar();
				return true;
			}
			while (@char.IsWhitespace())
			{
				appendChar(@char);
				@char = this.GetChar();
			}
			if (@char == '\r' || @char == '\n' || (@char == '\0' && this.AtEof()))
			{
				this.UngetChar();
				return false;
			}
			if (test(@char) && this.PeekChar() == '@')
			{
				appendChar(@char);
				if (falseFooterOffset == -1)
				{
					falseFooterOffset = this._currentIndex - 1;
				}
				appendChar(this.GetChar());
			}
			else
			{
				this.UngetChar();
			}
			return false;
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x00154B88 File Offset: 0x00152D88
		private Token ScanHereStringLiteral()
		{
			int errorOffset = this._currentIndex - 2;
			int num = -1;
			if (!this.ScanAfterHereStringHeader("@'"))
			{
				return this.NewStringLiteralToken("", TokenKind.HereStringLiteral, TokenFlags.TokenInError);
			}
			TokenFlags flags = TokenFlags.None;
			StringBuilder sb = new StringBuilder();
			Action<char> appendChar = delegate(char c)
			{
				sb.Append(c);
			};
			if (!this.ScanPossibleHereStringFooter(new Func<char, bool>(CharExtensions.IsSingleQuote), appendChar, ref num))
			{
				int length;
				for (;;)
				{
					char @char = this.GetChar();
					if (@char == '\r' || @char == '\n')
					{
						length = sb.Length;
						sb.Append(@char);
						if (@char == '\r' && this.PeekChar() == '\n')
						{
							this.SkipChar();
							sb.Append('\n');
						}
						if (this.ScanPossibleHereStringFooter(new Func<char, bool>(CharExtensions.IsSingleQuote), appendChar, ref num))
						{
							break;
						}
					}
					else
					{
						if (@char == '\0' && this.AtEof())
						{
							goto IL_108;
						}
						sb.Append(@char);
					}
				}
				sb.Length = length;
				goto IL_16D;
				IL_108:
				this.UngetChar();
				if (num != -1)
				{
					this.ReportIncompleteInput(num, () => ParserStrings.WhitespaceBeforeHereStringFooter);
				}
				else
				{
					this.ReportIncompleteInput(errorOffset, () => ParserStrings.TerminatorExpectedAtEndOfString, "'@");
				}
				flags = TokenFlags.TokenInError;
			}
			IL_16D:
			return this.NewStringLiteralToken(sb.ToString(), TokenKind.HereStringLiteral, flags);
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x00154D3C File Offset: 0x00152F3C
		private Token ScanHereStringExpandable()
		{
			int errorOffset = this._currentIndex - 2;
			if (!this.ScanAfterHereStringHeader("@\""))
			{
				return this.NewStringExpandableToken("", "", TokenKind.HereStringExpandable, null, TokenFlags.TokenInError);
			}
			TokenFlags flags = TokenFlags.None;
			List<Token> nestedTokens = new List<Token>();
			int num = -1;
			StringBuilder sb = new StringBuilder();
			StringBuilder formatSb = new StringBuilder();
			Action<char> appendChar = delegate(char c)
			{
				sb.Append(c);
				formatSb.Append(c);
			};
			if (!this.ScanPossibleHereStringFooter(new Func<char, bool>(CharExtensions.IsDoubleQuote), appendChar, ref num))
			{
				int length;
				int length2;
				for (;;)
				{
					char c3 = this.GetChar();
					if (c3 == '\r' || c3 == '\n')
					{
						length = sb.Length;
						length2 = formatSb.Length;
						sb.Append(c3);
						formatSb.Append(c3);
						if (c3 == '\r' && this.PeekChar() == '\n')
						{
							this.SkipChar();
							sb.Append('\n');
							formatSb.Append('\n');
						}
						if (this.ScanPossibleHereStringFooter(new Func<char, bool>(CharExtensions.IsDoubleQuote), appendChar, ref num))
						{
							break;
						}
					}
					else
					{
						if (c3 == '$')
						{
							if (this.ScanDollarInStringExpandable(sb, formatSb, true, nestedTokens))
							{
								continue;
							}
						}
						else if (c3 == '`')
						{
							char c2 = this.PeekChar();
							if (c2 != '\0')
							{
								this.SkipChar();
								c3 = Tokenizer.Backtick(c2);
							}
						}
						if (c3 == '{' || c3 == '}')
						{
							formatSb.Append(c3);
						}
						if (c3 == '\0' && this.AtEof())
						{
							goto IL_1D1;
						}
						sb.Append(c3);
						formatSb.Append(c3);
					}
				}
				sb.Length = length;
				formatSb.Length = length2;
				goto IL_236;
				IL_1D1:
				this.UngetChar();
				if (num != -1)
				{
					this.ReportIncompleteInput(num, () => ParserStrings.WhitespaceBeforeHereStringFooter);
				}
				else
				{
					this.ReportIncompleteInput(errorOffset, () => ParserStrings.TerminatorExpectedAtEndOfString, "\"@");
				}
				flags = TokenFlags.TokenInError;
			}
			IL_236:
			return this.NewStringExpandableToken(sb.ToString(), formatSb.ToString(), TokenKind.HereStringExpandable, nestedTokens, flags);
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x00154FA4 File Offset: 0x001531A4
		private Token ScanVariable(bool splatted, bool inStringExpandable)
		{
			int currentIndex = this._currentIndex;
			StringBuilder stringBuilder = new StringBuilder(20);
			char c = this.GetChar();
			VariablePath variablePath;
			if (c == '{')
			{
				for (;;)
				{
					c = this.GetChar();
					char c2 = c;
					if (c2 <= '"')
					{
						if (c2 != '\0')
						{
							if (c2 == '"')
							{
								goto IL_A4;
							}
						}
						else if (this.AtEof())
						{
							goto Block_13;
						}
					}
					else if (c2 != '`')
					{
						switch (c2)
						{
						case '{':
							this.ReportError(this._currentIndex, () => ParserStrings.OpenBraceNeedsToBeBackTickedInVariableName, new object[0]);
							break;
						case '|':
							break;
						case '}':
							goto IL_12A;
						default:
							switch (c2)
							{
							case '“':
							case '”':
							case '„':
								goto IL_A4;
							}
							break;
						}
					}
					else
					{
						char @char = this.GetChar();
						if (@char == '\0' && this.AtEof())
						{
							break;
						}
						c = Tokenizer.Backtick(@char);
					}
					IL_11D:
					stringBuilder.Append(c);
					continue;
					IL_A4:
					if (!inStringExpandable)
					{
						goto IL_11D;
					}
					char char2 = this.GetChar();
					if (char2 == '\0' && this.AtEof())
					{
						goto Block_11;
					}
					if (char2.IsDoubleQuote())
					{
						c = char2;
						goto IL_11D;
					}
					this.UngetChar();
					goto IL_11D;
				}
				this.UngetChar();
				goto IL_12A;
				Block_11:
				this.UngetChar();
				goto IL_12A;
				Block_13:
				this.UngetChar();
				IL_12A:
				string text = stringBuilder.ToString();
				if (c != '}')
				{
					this.ReportIncompleteInput(currentIndex, () => ParserStrings.IncompleteDollarVariableReference);
				}
				if (text.Length == 0)
				{
					if (c == '}')
					{
						this.ReportError(this._currentIndex - 1, () => ParserStrings.EmptyVariableReference, new object[0]);
					}
					text = ":Error:";
				}
				if (this.InCommandMode())
				{
					char c3 = this.PeekChar();
					if (!c3.ForceStartNewToken() && c3 != '.' && c3 != '[')
					{
						this._currentIndex = this._tokenStart;
						stringBuilder.Clear();
						return this.ScanGenericToken(stringBuilder);
					}
				}
				variablePath = new VariablePath(text);
				if (string.IsNullOrEmpty(variablePath.UnqualifiedPath))
				{
					this.ReportError(this.NewScriptExtent(this._tokenStart, this._currentIndex), () => ParserStrings.InvalidBracedVariableReference);
				}
				return this.NewVariableToken(variablePath, false);
			}
			if (!c.IsVariableStart())
			{
				this.UngetChar();
				stringBuilder.Append('$');
				return this.ScanGenericToken(stringBuilder);
			}
			stringBuilder.Append(c);
			if (c != '$' && c != '?' && c != '^')
			{
				bool flag = true;
				while (flag)
				{
					c = this.GetChar();
					switch (c)
					{
					case '\0':
					case '\t':
					case '\n':
					case '\r':
					case ' ':
					case '&':
					case '(':
					case ')':
					case ',':
					case '.':
					case ';':
					case '[':
					case '{':
					case '|':
					case '}':
						this.UngetChar();
						flag = false;
						continue;
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case '?':
					case 'A':
					case 'B':
					case 'C':
					case 'D':
					case 'E':
					case 'F':
					case 'G':
					case 'H':
					case 'I':
					case 'J':
					case 'K':
					case 'L':
					case 'M':
					case 'N':
					case 'O':
					case 'P':
					case 'Q':
					case 'R':
					case 'S':
					case 'T':
					case 'U':
					case 'V':
					case 'W':
					case 'X':
					case 'Y':
					case 'Z':
					case '_':
					case 'a':
					case 'b':
					case 'c':
					case 'd':
					case 'e':
					case 'f':
					case 'g':
					case 'h':
					case 'i':
					case 'j':
					case 'k':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'p':
					case 'q':
					case 'r':
					case 's':
					case 't':
					case 'u':
					case 'v':
					case 'w':
					case 'x':
					case 'y':
					case 'z':
						stringBuilder.Append(c);
						continue;
					case ':':
						if (this.PeekChar() == ':')
						{
							this.UngetChar();
							flag = false;
							continue;
						}
						stringBuilder.Append(c);
						continue;
					}
					if (char.IsLetterOrDigit(c))
					{
						stringBuilder.Append(c);
					}
					else
					{
						if (this.InCommandMode() && !c.ForceStartNewToken())
						{
							this._currentIndex = this._tokenStart;
							stringBuilder.Clear();
							return this.ScanGenericToken(stringBuilder);
						}
						this.UngetChar();
						flag = false;
					}
				}
			}
			else if (this.InCommandMode() && !this.PeekChar().ForceStartNewToken())
			{
				this._currentIndex = this._tokenStart;
				stringBuilder.Clear();
				return this.ScanGenericToken(stringBuilder);
			}
			variablePath = new VariablePath(stringBuilder.ToString());
			if (string.IsNullOrEmpty(variablePath.UnqualifiedPath))
			{
				Expression<Func<string>> message;
				if (variablePath.IsDriveQualified)
				{
					message = (() => ParserStrings.InvalidVariableReferenceWithDrive);
				}
				else
				{
					message = (() => ParserStrings.InvalidVariableReference);
				}
				this.ReportError(this.NewScriptExtent(this._tokenStart, this._currentIndex), message);
			}
			return this.NewVariableToken(variablePath, splatted);
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x0015557C File Offset: 0x0015377C
		private Token ScanParameter()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			bool sawColon = false;
			while (flag)
			{
				char @char = this.GetChar();
				if (@char.IsWhitespace())
				{
					this.UngetChar();
					break;
				}
				char c = @char;
				switch (c)
				{
				case '\0':
				case '\n':
				case '\r':
				case '&':
				case '(':
				case ')':
				case ',':
				case '.':
				case ';':
				case '[':
				case '{':
				case '|':
				case '}':
					this.UngetChar();
					flag = false;
					continue;
				case '\u0001':
				case '\u0002':
				case '\u0003':
				case '\u0004':
				case '\u0005':
				case '\u0006':
				case '\a':
				case '\b':
				case '\t':
				case '\v':
				case '\f':
				case '\u000e':
				case '\u000f':
				case '\u0010':
				case '\u0011':
				case '\u0012':
				case '\u0013':
				case '\u0014':
				case '\u0015':
				case '\u0016':
				case '\u0017':
				case '\u0018':
				case '\u0019':
				case '\u001a':
				case '\u001b':
				case '\u001c':
				case '\u001d':
				case '\u001e':
				case '\u001f':
				case ' ':
				case '!':
				case '#':
				case '$':
				case '%':
				case '*':
				case '+':
				case '-':
				case '/':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '<':
				case '=':
				case '>':
				case '?':
				case '@':
				case '\\':
				case ']':
				case '^':
				case '_':
				case '`':
					goto IL_2B7;
				case '"':
				case '\'':
					break;
				case ':':
					flag = false;
					sawColon = true;
					if (!this.InCommandMode())
					{
						this.UngetChar();
						continue;
					}
					continue;
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'G':
				case 'H':
				case 'I':
				case 'J':
				case 'K':
				case 'L':
				case 'M':
				case 'N':
				case 'O':
				case 'P':
				case 'Q':
				case 'R':
				case 'S':
				case 'T':
				case 'U':
				case 'V':
				case 'W':
				case 'X':
				case 'Y':
				case 'Z':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
				case 'g':
				case 'h':
				case 'i':
				case 'j':
				case 'k':
				case 'l':
				case 'm':
				case 'n':
				case 'o':
				case 'p':
				case 'q':
				case 'r':
				case 's':
				case 't':
				case 'u':
				case 'v':
				case 'w':
				case 'x':
				case 'y':
				case 'z':
					stringBuilder.Append(@char);
					continue;
				default:
					switch (c)
					{
					case '‘':
					case '’':
					case '‚':
					case '‛':
					case '“':
					case '”':
					case '„':
						break;
					default:
						goto IL_2B7;
					}
					break;
				}
				if (this.InCommandMode())
				{
					this.UngetChar();
					stringBuilder.Insert(0, this._script[this._tokenStart]);
					return this.ScanGenericToken(stringBuilder);
				}
				this.UngetChar();
				flag = false;
				continue;
				IL_2B7:
				if (this.InCommandMode())
				{
					stringBuilder.Append(@char);
				}
				else
				{
					this.UngetChar();
					flag = false;
				}
			}
			TokenKind kind;
			if (this.InExpressionMode() && Tokenizer._operatorTable.TryGetValue(stringBuilder.ToString(), out kind))
			{
				return this.NewToken(kind);
			}
			if (stringBuilder.Length == 0)
			{
				return this.NewToken(TokenKind.Minus);
			}
			return this.NewParameterToken(stringBuilder.ToString(), sawColon);
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x001558A3 File Offset: 0x00153AA3
		private Token CheckOperatorInCommandMode(char c, TokenKind tokenKind)
		{
			if (this.InCommandMode() && !this.PeekChar().ForceStartNewToken())
			{
				return this.ScanGenericToken(c);
			}
			return this.NewToken(tokenKind);
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x001558CC File Offset: 0x00153ACC
		private Token CheckOperatorInCommandMode(char c1, char c2, TokenKind tokenKind)
		{
			if (this.InCommandMode() && !this.PeekChar().ForceStartNewToken())
			{
				StringBuilder stringBuilder = new StringBuilder(4);
				stringBuilder.Append(c1);
				stringBuilder.Append(c2);
				return this.ScanGenericToken(stringBuilder);
			}
			return this.NewToken(tokenKind);
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x00155914 File Offset: 0x00153B14
		private Token ScanGenericToken(char firstChar)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(firstChar);
			return this.ScanGenericToken(stringBuilder);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x00155938 File Offset: 0x00153B38
		private Token ScanGenericToken(StringBuilder sb)
		{
			List<Token> list = new List<Token>();
			StringBuilder stringBuilder = new StringBuilder(sb.ToString());
			char c = this.GetChar();
			while (!c.ForceStartNewToken())
			{
				if (c == '`')
				{
					char c2 = this.PeekChar();
					if (c2 != '\0')
					{
						this.SkipChar();
						c = Tokenizer.Backtick(c2);
						goto IL_A0;
					}
					goto IL_A0;
				}
				else if (c.IsSingleQuote())
				{
					int length = sb.Length;
					this.ScanStringLiteral(sb);
					for (int i = length; i < sb.Length; i++)
					{
						stringBuilder.Append(sb[i]);
					}
				}
				else if (c.IsDoubleQuote())
				{
					this.ScanStringExpandable(sb, stringBuilder, list);
				}
				else if (c != '$' || !this.ScanDollarInStringExpandable(sb, stringBuilder, false, list))
				{
					goto IL_A0;
				}
				IL_C2:
				c = this.GetChar();
				continue;
				IL_A0:
				sb.Append(c);
				stringBuilder.Append(c);
				if (c == '{' || c == '}')
				{
					stringBuilder.Append(c);
					goto IL_C2;
				}
				goto IL_C2;
			}
			this.UngetChar();
			string text = sb.ToString();
			if (list.Count > 0)
			{
				return this.NewGenericExpandableToken(text, stringBuilder.ToString(), list);
			}
			if (DynamicKeyword.ContainsKeyword(text) && !DynamicKeyword.IsHiddenKeyword(text))
			{
				return this.NewToken(TokenKind.DynamicKeyword);
			}
			return this.NewGenericToken(text);
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x00155A68 File Offset: 0x00153C68
		private void ScanHexDigits(StringBuilder sb)
		{
			char c = this.PeekChar();
			while (c.IsHexDigit())
			{
				this.SkipChar();
				sb.Append(c);
				c = this.PeekChar();
			}
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x00155A9C File Offset: 0x00153C9C
		private int ScanDecimalDigits(StringBuilder sb)
		{
			int num = 0;
			char c = this.PeekChar();
			while (c.IsDecimalDigit())
			{
				num++;
				this.SkipChar();
				sb.Append(c);
				c = this.PeekChar();
			}
			return num;
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x00155AD8 File Offset: 0x00153CD8
		private void ScanExponent(StringBuilder sb, ref int signIndex, ref bool notNumber)
		{
			char c = this.PeekChar();
			if (c == '+' || c.IsDash())
			{
				this.SkipChar();
				signIndex = sb.Length;
				sb.Append(c);
			}
			if (this.ScanDecimalDigits(sb) == 0)
			{
				notNumber = true;
			}
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x00155B1C File Offset: 0x00153D1C
		private void ScanNumberAfterDot(StringBuilder sb, ref int signIndex, ref bool notNumber)
		{
			this.ScanDecimalDigits(sb);
			char c = this.PeekChar();
			if (c == 'e' || c == 'E')
			{
				this.SkipChar();
				sb.Append(c);
				this.ScanExponent(sb, ref signIndex, ref notNumber);
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x00155B5C File Offset: 0x00153D5C
		private static bool TryGetNumberValue(string strNum, bool hex, bool real, char suffix, long multiplier, out object result)
		{
			try
			{
				NumberStyles style = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent;
				if (suffix == 'd' || suffix == 'D')
				{
					decimal d;
					if (decimal.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out d))
					{
						result = d * multiplier;
						return true;
					}
					result = null;
					return false;
				}
				else if (real)
				{
					double num;
					if (double.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out num))
					{
						if (num == 0.0 && strNum[0] == '-')
						{
							num = --0.0;
						}
						if (suffix == 'l' || suffix == 'L')
						{
							result = checked((long)Convert.ChangeType(num, typeof(long), CultureInfo.InvariantCulture) * multiplier);
						}
						else
						{
							result = num * (double)multiplier;
						}
						return true;
					}
					result = null;
					return false;
				}
				else
				{
					if (hex && !strNum[0].IsHexDigit())
					{
						if (strNum[0] == '-')
						{
							multiplier = checked(0L - multiplier);
						}
						strNum = strNum.Substring(1);
					}
					style = (hex ? NumberStyles.AllowHexSpecifier : NumberStyles.AllowLeadingSign);
					long num2;
					if (suffix != 'l' && suffix != 'L')
					{
						int value;
						TypeCode typeCode;
						BigInteger bigInteger;
						decimal value2;
						if (int.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out value))
						{
							typeCode = TypeCode.Int32;
							bigInteger = value;
						}
						else if (long.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out num2))
						{
							typeCode = TypeCode.Int64;
							bigInteger = num2;
						}
						else if (decimal.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out value2))
						{
							typeCode = TypeCode.Decimal;
							bigInteger = (BigInteger)value2;
						}
						else
						{
							double num3;
							if (!hex && double.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out num3))
							{
								result = num3 * (double)multiplier;
								return true;
							}
							result = null;
							return false;
						}
						bigInteger *= multiplier;
						if (bigInteger >= -2147483648L && bigInteger <= 2147483647L && typeCode <= TypeCode.Int32)
						{
							result = (int)bigInteger;
						}
						else if (bigInteger >= -9223372036854775808L && bigInteger <= 9223372036854775807L && typeCode <= TypeCode.Int64)
						{
							result = (long)bigInteger;
						}
						else if (bigInteger >= (BigInteger)(-79228162514264337593543950335m) && bigInteger <= (BigInteger)79228162514264337593543950335m && typeCode <= TypeCode.Decimal)
						{
							result = (decimal)bigInteger;
						}
						else
						{
							result = (double)bigInteger;
						}
						return true;
					}
					if (long.TryParse(strNum, style, NumberFormatInfo.InvariantInfo, out num2))
					{
						result = checked(num2 * multiplier);
						return true;
					}
					result = null;
					return false;
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			result = null;
			return false;
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x00155E58 File Offset: 0x00154058
		private Token ScanNumber(char firstChar)
		{
			bool hex;
			bool real;
			char suffix;
			long multiplier;
			string text = this.ScanNumberHelper(firstChar, out hex, out real, out suffix, out multiplier);
			if (text == null)
			{
				this._currentIndex = this._tokenStart;
				StringBuilder sb = new StringBuilder();
				return this.ScanGenericToken(sb);
			}
			object value;
			if (!Tokenizer.TryGetNumberValue(text, hex, real, suffix, multiplier, out value))
			{
				if (!this.InExpressionMode())
				{
					this._currentIndex = this._tokenStart;
					StringBuilder sb = new StringBuilder();
					return this.ScanGenericToken(sb);
				}
				this.ReportError(this._currentIndex, () => ParserStrings.BadNumericConstant, new object[]
				{
					this._script.Substring(this._tokenStart, this._currentIndex - this._tokenStart)
				});
			}
			return this.NewNumberToken(value);
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x00155F2C File Offset: 0x0015412C
		private string ScanNumberHelper(char firstChar, out bool hex, out bool real, out char suffix, out long multiplier)
		{
			hex = false;
			real = false;
			suffix = '\0';
			multiplier = 1L;
			bool flag = false;
			int num = -1;
			StringBuilder stringBuilder = new StringBuilder();
			if (firstChar.IsDash() || firstChar == '+')
			{
				stringBuilder.Append(firstChar);
				firstChar = this.GetChar();
			}
			char c;
			if (firstChar == '.')
			{
				stringBuilder.Append('.');
				this.ScanNumberAfterDot(stringBuilder, ref num, ref flag);
				real = true;
			}
			else
			{
				c = this.PeekChar();
				if (firstChar == '0' && (c == 'x' || c == 'X'))
				{
					this.SkipChar();
					this.ScanHexDigits(stringBuilder);
					if (stringBuilder.Length == 0)
					{
						flag = true;
					}
					hex = true;
				}
				else
				{
					stringBuilder.Append(firstChar);
					this.ScanDecimalDigits(stringBuilder);
					c = this.PeekChar();
					char c2 = c;
					if (c2 != '.')
					{
						if (c2 == 'E' || c2 == 'e')
						{
							this.SkipChar();
							stringBuilder.Append(c);
							real = true;
							this.ScanExponent(stringBuilder, ref num, ref flag);
						}
					}
					else
					{
						this.SkipChar();
						if (this.PeekChar() == '.')
						{
							this.UngetChar();
						}
						else
						{
							stringBuilder.Append(c);
							real = true;
							this.ScanNumberAfterDot(stringBuilder, ref num, ref flag);
						}
					}
				}
			}
			c = this.PeekChar();
			if (c.IsTypeSuffix())
			{
				this.SkipChar();
				suffix = c;
				c = this.PeekChar();
			}
			if (c.IsMultiplierStart())
			{
				this.SkipChar();
				if (c == 'k' || c == 'K')
				{
					multiplier = 1024L;
				}
				else if (c == 'm' || c == 'M')
				{
					multiplier = 1048576L;
				}
				else if (c == 'g' || c == 'G')
				{
					multiplier = 1073741824L;
				}
				else if (c == 't' || c == 'T')
				{
					multiplier = 1099511627776L;
				}
				else if (c == 'p' || c == 'P')
				{
					multiplier = 1125899906842624L;
				}
				char c3 = this.PeekChar();
				if (c3 == 'b' || c3 == 'B')
				{
					this.SkipChar();
					c = this.PeekChar();
				}
				else
				{
					flag = true;
				}
			}
			if (!c.ForceStartNewToken() && (!this.InExpressionMode() || !c.ForceStartNewTokenAfterNumber()))
			{
				flag = true;
			}
			if (flag)
			{
				return null;
			}
			if (num != -1 && stringBuilder[num] != '-' && stringBuilder[num].IsDash())
			{
				stringBuilder[num] = '-';
			}
			if (stringBuilder[0] != '-' && stringBuilder[0].IsDash())
			{
				stringBuilder[0] = '-';
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0015616C File Offset: 0x0015436C
		internal Token GetMemberAccessOperator(bool allowLBracket)
		{
			char c;
			for (c = this.PeekChar(); c == '<'; c = this.PeekChar())
			{
				this._tokenStart = this._currentIndex;
				this.SkipChar();
				if (this.PeekChar() != '#')
				{
					this.UngetChar();
					return null;
				}
				this.SkipChar();
				this.ScanBlockComment();
			}
			if (c == '.')
			{
				this._tokenStart = this._currentIndex;
				this.SkipChar();
				c = this.PeekChar();
				if (c == '.')
				{
					this.UngetChar();
					return null;
				}
				if (this.InCommandMode() && (c.IsWhitespace() || c == '\0' || c == '\r' || c == '\n'))
				{
					this.UngetChar();
					return null;
				}
				return this.NewToken(TokenKind.Dot);
			}
			else if (c == ':')
			{
				this._tokenStart = this._currentIndex;
				this.SkipChar();
				c = this.PeekChar();
				if (c != ':')
				{
					this.UngetChar();
					return null;
				}
				this.SkipChar();
				c = this.PeekChar();
				if (this.InCommandMode() && (c.IsWhitespace() || c == '\0' || c == '\r' || c == '\n'))
				{
					this.UngetChar();
					this.UngetChar();
					return null;
				}
				return this.NewToken(TokenKind.ColonColon);
			}
			else
			{
				if (c == '[' && allowLBracket)
				{
					this._tokenStart = this._currentIndex;
					this.SkipChar();
					return this.NewToken(TokenKind.LBracket);
				}
				return null;
			}
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x001562AC File Offset: 0x001544AC
		internal Token GetInvokeMemberOpenParen()
		{
			char c = this.PeekChar();
			if (c == '(')
			{
				this._tokenStart = this._currentIndex;
				this.SkipChar();
				return this.NewToken(TokenKind.LParen);
			}
			if (c == '{')
			{
				this._tokenStart = this._currentIndex;
				this.SkipChar();
				return this.NewToken(TokenKind.LCurly);
			}
			return null;
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x00156304 File Offset: 0x00154504
		internal Token GetLBracket()
		{
			int currentIndex = this._currentIndex;
			bool flag = false;
			for (;;)
			{
				this._tokenStart = this._currentIndex;
				char @char = this.GetChar();
				char c = @char;
				if (c <= '.')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
						case '\v':
						case '\f':
							break;
						case '\n':
							goto IL_E7;
						default:
							if (c != ' ')
							{
								goto IL_E7;
							}
							break;
						}
					}
					else
					{
						if (c == '#')
						{
							flag = true;
							this.ScanLineComment();
							continue;
						}
						if (c != '.')
						{
							goto IL_E7;
						}
						goto IL_D3;
					}
				}
				else if (c <= '[')
				{
					switch (c)
					{
					case ':':
						goto IL_D3;
					case ';':
						goto IL_E7;
					case '<':
						if (this.PeekChar() == '#')
						{
							flag = false;
							this.SkipChar();
							this.ScanBlockComment();
							continue;
						}
						goto IL_C2;
					default:
						if (c != '[')
						{
							goto IL_E7;
						}
						goto IL_CA;
					}
				}
				else if (c != '\u0085' && c != '\u00a0')
				{
					goto IL_E7;
				}
				flag = true;
				this.SkipWhiteSpace();
				continue;
				IL_E7:
				if (!@char.IsWhitespace())
				{
					goto IL_FC;
				}
				flag = true;
				this.SkipWhiteSpace();
			}
			IL_C2:
			this.UngetChar();
			goto IL_102;
			IL_CA:
			return this.NewToken(TokenKind.LBracket);
			IL_D3:
			if (flag)
			{
				this.Resync(currentIndex);
				goto IL_102;
			}
			this.UngetChar();
			goto IL_102;
			IL_FC:
			this.UngetChar();
			IL_102:
			return null;
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x00156414 File Offset: 0x00154614
		private Token ScanDot()
		{
			char c = this.PeekChar();
			if (c == '.')
			{
				this.SkipChar();
				c = this.PeekChar();
				if (this.InCommandMode() && !c.ForceStartNewToken())
				{
					this.UngetChar();
					return this.ScanGenericToken('.');
				}
				return this.NewToken(TokenKind.DotDot);
			}
			else
			{
				if (c.IsDecimalDigit())
				{
					return this.ScanNumber('.');
				}
				if (this.InCommandMode() && !c.ForceStartNewToken() && c != '$' && c != '"' && c != '\'')
				{
					return this.ScanGenericToken('.');
				}
				return this.NewToken(TokenKind.Dot);
			}
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x001564A3 File Offset: 0x001546A3
		private bool CharIsAllowedInIdentifierAfterFirstCharacter(char c)
		{
			return char.IsLetter(c) || c == '_' || c.IsDecimalDigit();
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x001564BC File Offset: 0x001546BC
		private Token ScanIdentifier(char firstChar)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(firstChar);
			char @char;
			for (;;)
			{
				@char = this.GetChar();
				if (!this.CharIsAllowedInIdentifierAfterFirstCharacter(@char))
				{
					break;
				}
				stringBuilder.Append(@char);
			}
			this.UngetChar();
			if (this.InTypeNameMode())
			{
				return this.ScanTypeName();
			}
			if (!this.WantSimpleName && this.InCommandMode() && !@char.ForceStartNewToken())
			{
				return this.ScanGenericToken(stringBuilder);
			}
			if (!this.WantSimpleName && (this.InCommandMode() || this.InSignatureMode()))
			{
				string text = stringBuilder.ToString();
				TokenKind tokenKind;
				if (Tokenizer._keywordTable.TryGetValue(text, out tokenKind) && (tokenKind != TokenKind.InlineScript || this.InWorkflowContext))
				{
					return this.NewToken(tokenKind);
				}
				if (DynamicKeyword.ContainsKeyword(text) && !DynamicKeyword.IsHiddenKeyword(text))
				{
					return this.NewToken(TokenKind.DynamicKeyword);
				}
			}
			return this.NewToken(TokenKind.Identifier);
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x00156590 File Offset: 0x00154790
		private Token ScanTypeName()
		{
			char @char;
			do
			{
				@char = this.GetChar();
				char c = @char;
				if (c <= '+')
				{
					if (c == '#' || c == '+')
					{
						continue;
					}
				}
				else
				{
					if (c == '.')
					{
						continue;
					}
					switch (c)
					{
					case '\\':
					case '_':
					case '`':
						continue;
					}
				}
			}
			while (char.IsLetterOrDigit(@char));
			this.UngetChar();
			Token token = this.NewToken(TokenKind.Identifier);
			token.TokenFlags |= TokenFlags.TypeName;
			return token;
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x00156604 File Offset: 0x00154804
		private void ScanAssemblyNameSpecToken(StringBuilder sb)
		{
			this.SkipWhiteSpace();
			this._tokenStart = this._currentIndex;
			for (;;)
			{
				char @char = this.GetChar();
				if (@char.ForceStartNewTokenInAssemblyNameSpec())
				{
					break;
				}
				sb.Append(@char);
			}
			this.UngetChar();
			Token token = this.NewToken(TokenKind.Identifier);
			token.TokenFlags |= TokenFlags.TypeName;
			this.SkipWhiteSpace();
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x00156664 File Offset: 0x00154864
		internal string GetAssemblyNameSpec()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ScanAssemblyNameSpecToken(stringBuilder);
			while (this.PeekChar() == ',')
			{
				this._tokenStart = this._currentIndex;
				stringBuilder.Append(", ");
				this.SkipChar();
				this.NewToken(TokenKind.Comma);
				this.ScanAssemblyNameSpecToken(stringBuilder);
				if (this.PeekChar() == '=')
				{
					this._tokenStart = this._currentIndex;
					stringBuilder.Append("=");
					this.SkipChar();
					this.NewToken(TokenKind.Equals);
					this.ScanAssemblyNameSpecToken(stringBuilder);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x001566F8 File Offset: 0x001548F8
		private Token ScanLabel()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char @char = this.GetChar();
			if (!@char.IsIndentifierStart())
			{
				stringBuilder.Append(':');
				if (@char == '\0')
				{
					this.UngetChar();
					return this.NewGenericToken(stringBuilder.ToString());
				}
				this.UngetChar();
				return this.ScanGenericToken(stringBuilder);
			}
			else
			{
				while (@char.IsIndentifierFollow())
				{
					stringBuilder.Append(@char);
					@char = this.GetChar();
				}
				if (this.InCommandMode() && !@char.ForceStartNewToken())
				{
					stringBuilder.Insert(0, ':');
					stringBuilder.Append(@char);
					return this.ScanGenericToken(stringBuilder);
				}
				this.UngetChar();
				return this.NewLabelToken(stringBuilder.ToString());
			}
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0015679C File Offset: 0x0015499C
		internal Token NextToken()
		{
			char @char;
			char c2;
			for (;;)
			{
				this._tokenStart = this._currentIndex;
				@char = this.GetChar();
				char c = @char;
				switch (c)
				{
				case '\0':
					goto IL_862;
				case '\u0001':
				case '\u0002':
				case '\u0003':
				case '\u0004':
				case '\u0005':
				case '\u0006':
				case '\a':
				case '\b':
				case '\u000e':
				case '\u000f':
				case '\u0010':
				case '\u0011':
				case '\u0012':
				case '\u0013':
				case '\u0014':
				case '\u0015':
				case '\u0016':
				case '\u0017':
				case '\u0018':
				case '\u0019':
				case '\u001a':
				case '\u001b':
				case '\u001c':
				case '\u001d':
				case '\u001e':
				case '\u001f':
				case '?':
				case '\\':
				case '^':
				case '~':
				case '\u007f':
				case '\u0080':
				case '\u0081':
				case '\u0082':
				case '\u0083':
				case '\u0084':
				case '\u0086':
				case '\u0087':
				case '\u0088':
				case '\u0089':
				case '\u008a':
				case '\u008b':
				case '\u008c':
				case '\u008d':
				case '\u008e':
				case '\u008f':
				case '\u0090':
				case '\u0091':
				case '\u0092':
				case '\u0093':
				case '\u0094':
				case '\u0095':
				case '\u0096':
				case '\u0097':
				case '\u0098':
				case '\u0099':
				case '\u009a':
				case '\u009b':
				case '\u009c':
				case '\u009d':
				case '\u009e':
				case '\u009f':
					break;
				case '\t':
				case '\v':
				case '\f':
				case ' ':
				case '\u0085':
				case '\u00a0':
					this.SkipWhiteSpace();
					continue;
				case '\n':
				case '\r':
					goto IL_39B;
				case '!':
					goto IL_782;
				case '"':
					goto IL_2F5;
				case '#':
					this.ScanLineComment();
					continue;
				case '$':
					goto IL_5DE;
				case '%':
					goto IL_5B7;
				case '&':
					goto IL_73E;
				case '\'':
					goto IL_2EE;
				case '(':
					goto IL_64E;
				case ')':
					goto IL_657;
				case '*':
					goto IL_50C;
				case '+':
					goto IL_43C;
				case ',':
					goto IL_6B2;
				case '-':
					goto IL_496;
				case '.':
					goto IL_6A2;
				case '/':
					goto IL_590;
				case '0':
				case '7':
				case '8':
				case '9':
					goto IL_6BB;
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
					goto IL_6C3;
				case ':':
					goto IL_7FF;
				case ';':
					goto IL_6A9;
				case '<':
					if (this.PeekChar() == '#')
					{
						this.SkipChar();
						this.ScanBlockComment();
						continue;
					}
					goto IL_61B;
				case '=':
					goto IL_432;
				case '>':
					goto IL_622;
				case '@':
					goto IL_2FC;
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'G':
				case 'H':
				case 'I':
				case 'J':
				case 'K':
				case 'L':
				case 'M':
				case 'N':
				case 'O':
				case 'P':
				case 'Q':
				case 'R':
				case 'S':
				case 'T':
				case 'U':
				case 'V':
				case 'W':
				case 'X':
				case 'Y':
				case 'Z':
				case '_':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
				case 'g':
				case 'h':
				case 'i':
				case 'j':
				case 'k':
				case 'l':
				case 'm':
				case 'n':
				case 'o':
				case 'p':
				case 'q':
				case 'r':
				case 's':
				case 't':
				case 'u':
				case 'v':
				case 'w':
				case 'x':
				case 'y':
				case 'z':
					goto IL_646;
				case '[':
					goto IL_660;
				case ']':
					goto IL_687;
				case '`':
					c2 = this.GetChar();
					if (c2 == '\n' || c2 == '\r')
					{
						this.ScanNewline(c2);
						this.NewToken(TokenKind.LineContinuation);
						continue;
					}
					if (char.IsWhiteSpace(c2))
					{
						this.SkipWhiteSpace();
						continue;
					}
					if (c2 == '\0' && this.AtEof())
					{
						this.ReportIncompleteInput(this._currentIndex, () => ParserStrings.IncompleteString);
						this.UngetChar();
						continue;
					}
					goto IL_425;
				case '{':
					goto IL_690;
				case '|':
					goto IL_760;
				case '}':
					goto IL_699;
				default:
					switch (c)
					{
					case '–':
					case '—':
					case '―':
						goto IL_496;
					case '‘':
					case '’':
					case '‚':
					case '‛':
						goto IL_2EE;
					case '“':
					case '”':
					case '„':
						goto IL_2F5;
					}
					break;
				}
				if (!@char.IsWhitespace())
				{
					goto IL_8AA;
				}
				this.SkipWhiteSpace();
			}
			IL_2EE:
			return this.ScanStringLiteral();
			IL_2F5:
			return this.ScanStringExpandable();
			IL_2FC:
			c2 = this.GetChar();
			if (c2 == '{')
			{
				return this.NewToken(TokenKind.AtCurly);
			}
			if (c2 == '(')
			{
				return this.NewToken(TokenKind.AtParen);
			}
			if (c2.IsSingleQuote())
			{
				return this.ScanHereStringLiteral();
			}
			if (c2.IsDoubleQuote())
			{
				return this.ScanHereStringExpandable();
			}
			this.UngetChar();
			if (c2.IsVariableStart())
			{
				return this.ScanVariable(true, false);
			}
			this.ReportError(this._currentIndex - 1, () => ParserStrings.UnrecognizedToken, new object[0]);
			return this.NewToken(TokenKind.Unknown);
			IL_39B:
			this.ScanNewline(@char);
			return this.NewToken(TokenKind.NewLine);
			IL_425:
			return this.ScanGenericToken(Tokenizer.Backtick(c2));
			IL_432:
			return this.CheckOperatorInCommandMode(@char, TokenKind.Equals);
			IL_43C:
			c2 = this.PeekChar();
			if (c2 == '+')
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.PlusPlus);
			}
			if (c2 == '=')
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.PlusEquals);
			}
			if (this.AllowSignedNumbers && (char.IsDigit(c2) || c2 == '.'))
			{
				return this.ScanNumber(@char);
			}
			return this.CheckOperatorInCommandMode(@char, TokenKind.Plus);
			IL_496:
			c2 = this.PeekChar();
			if (c2.IsDash())
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.MinusMinus);
			}
			if (c2 == '=')
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.MinusEquals);
			}
			if (char.IsLetter(c2) || c2 == '_' || c2 == '?')
			{
				return this.ScanParameter();
			}
			if (this.AllowSignedNumbers && (char.IsDigit(c2) || c2 == '.'))
			{
				return this.ScanNumber(@char);
			}
			return this.CheckOperatorInCommandMode(@char, TokenKind.Minus);
			IL_50C:
			c2 = this.PeekChar();
			if (c2 == '=')
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.MultiplyEquals);
			}
			if (c2 != '>')
			{
				return this.CheckOperatorInCommandMode(@char, TokenKind.Multiply);
			}
			this.SkipChar();
			c2 = this.PeekChar();
			if (c2 == '>')
			{
				this.SkipChar();
				return this.NewFileRedirectionToken(0, true, false);
			}
			if (c2 == '&')
			{
				this.SkipChar();
				c2 = this.PeekChar();
				if (c2 == '1')
				{
					this.SkipChar();
					return this.NewMergingRedirectionToken(0, 1);
				}
				this.UngetChar();
			}
			return this.NewFileRedirectionToken(0, false, false);
			IL_590:
			c2 = this.PeekChar();
			if (c2 == '=')
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.DivideEquals);
			}
			return this.CheckOperatorInCommandMode(@char, TokenKind.Divide);
			IL_5B7:
			c2 = this.PeekChar();
			if (c2 == '=')
			{
				this.SkipChar();
				return this.CheckOperatorInCommandMode(@char, c2, TokenKind.RemainderEquals);
			}
			return this.CheckOperatorInCommandMode(@char, TokenKind.Rem);
			IL_5DE:
			if (this.PeekChar() == '(')
			{
				this.SkipChar();
				return this.NewToken(TokenKind.DollarParen);
			}
			return this.ScanVariable(false, false);
			IL_61B:
			return this.NewInputRedirectionToken();
			IL_622:
			if (this.PeekChar() == '>')
			{
				this.SkipChar();
				return this.NewFileRedirectionToken(1, true, false);
			}
			return this.NewFileRedirectionToken(1, false, false);
			IL_646:
			return this.ScanIdentifier(@char);
			IL_64E:
			return this.NewToken(TokenKind.LParen);
			IL_657:
			return this.NewToken(TokenKind.RParen);
			IL_660:
			if (this.InCommandMode() && !this.PeekChar().ForceStartNewToken())
			{
				return this.ScanGenericToken('[');
			}
			return this.NewToken(TokenKind.LBracket);
			IL_687:
			return this.NewToken(TokenKind.RBracket);
			IL_690:
			return this.NewToken(TokenKind.LCurly);
			IL_699:
			return this.NewToken(TokenKind.RCurly);
			IL_6A2:
			return this.ScanDot();
			IL_6A9:
			return this.NewToken(TokenKind.Semi);
			IL_6B2:
			return this.NewToken(TokenKind.Comma);
			IL_6BB:
			return this.ScanNumber(@char);
			IL_6C3:
			if (this.PeekChar() != '>')
			{
				return this.ScanNumber(@char);
			}
			this.SkipChar();
			c2 = this.PeekChar();
			if (c2 == '>')
			{
				this.SkipChar();
				return this.NewFileRedirectionToken((int)(@char - '0'), true, true);
			}
			if (c2 == '&')
			{
				this.SkipChar();
				c2 = this.PeekChar();
				if (c2 == '1' || c2 == '2')
				{
					this.SkipChar();
					return this.NewMergingRedirectionToken((int)(@char - '0'), (int)(c2 - '0'));
				}
				this.UngetChar();
			}
			return this.NewFileRedirectionToken((int)(@char - '0'), false, true);
			IL_73E:
			if (this.PeekChar() == '&')
			{
				this.SkipChar();
				return this.NewToken(TokenKind.AndAnd);
			}
			return this.NewToken(TokenKind.Ampersand);
			IL_760:
			if (this.PeekChar() == '|')
			{
				this.SkipChar();
				return this.NewToken(TokenKind.OrOr);
			}
			return this.NewToken(TokenKind.Pipe);
			IL_782:
			c2 = this.PeekChar();
			if ((this.InCommandMode() && !c2.ForceStartNewToken()) || (this.InExpressionMode() && c2.IsIndentifierStart()))
			{
				return this.ScanGenericToken(@char);
			}
			if (this.InExpressionMode() && (char.IsDigit(c2) || c2 == '.'))
			{
				bool flag;
				bool flag2;
				char c3;
				long num;
				string text = this.ScanNumberHelper(@char, out flag, out flag2, out c3, out num);
				this._currentIndex = this._tokenStart;
				@char = this.GetChar();
				if (text == null)
				{
					return this.ScanGenericToken(@char);
				}
			}
			return this.NewToken(TokenKind.Exclaim);
			IL_7FF:
			if (this.PeekChar() == ':')
			{
				this.SkipChar();
				if (this.InCommandMode() && !this.WantSimpleName && !this.PeekChar().ForceStartNewToken())
				{
					StringBuilder sb = new StringBuilder("::");
					return this.ScanGenericToken(sb);
				}
				return this.NewToken(TokenKind.ColonColon);
			}
			else
			{
				if (this.InCommandMode())
				{
					return this.ScanLabel();
				}
				return this.NewToken(TokenKind.Colon);
			}
			IL_862:
			if (this.AtEof())
			{
				return this.SaveToken<Token>(new Token(this.NewScriptExtent(this._tokenStart + 1, this._tokenStart + 1), TokenKind.EndOfInput, TokenFlags.None));
			}
			return this.ScanGenericToken(@char);
			IL_8AA:
			if (char.IsLetter(@char))
			{
				return this.ScanIdentifier(@char);
			}
			return this.ScanGenericToken(@char);
		}

		// Token: 0x04002063 RID: 8291
		private const string shellIDToken = "shellid";

		// Token: 0x04002064 RID: 8292
		private const string PSSnapinToken = "pssnapin";

		// Token: 0x04002065 RID: 8293
		private const string versionToken = "version";

		// Token: 0x04002066 RID: 8294
		private const string assemblyToken = "assembly";

		// Token: 0x04002067 RID: 8295
		private const string modulesToken = "modules";

		// Token: 0x04002068 RID: 8296
		private const string elevationToken = "runasadministrator";

		// Token: 0x04002069 RID: 8297
		private static readonly Dictionary<string, TokenKind> _keywordTable = new Dictionary<string, TokenKind>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400206A RID: 8298
		private static readonly Dictionary<string, TokenKind> _operatorTable = new Dictionary<string, TokenKind>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400206B RID: 8299
		private readonly Parser _parser;

		// Token: 0x0400206C RID: 8300
		private PositionHelper _positionHelper;

		// Token: 0x0400206D RID: 8301
		private int _nestedTokensAdjustment;

		// Token: 0x0400206E RID: 8302
		private BitArray _skippedCharOffsets;

		// Token: 0x0400206F RID: 8303
		private string _script;

		// Token: 0x04002070 RID: 8304
		private int _tokenStart;

		// Token: 0x04002071 RID: 8305
		private int _currentIndex;

		// Token: 0x04002072 RID: 8306
		private InternalScriptExtent _beginSignatureExtent;

		// Token: 0x04002073 RID: 8307
		private static readonly string[] _keywordText = new string[]
		{
			"elseif",
			"if",
			"else",
			"switch",
			"foreach",
			"from",
			"in",
			"for",
			"while",
			"until",
			"do",
			"try",
			"catch",
			"finally",
			"trap",
			"data",
			"return",
			"continue",
			"break",
			"exit",
			"throw",
			"begin",
			"process",
			"end",
			"dynamicparam",
			"function",
			"filter",
			"param",
			"class",
			"define",
			"var",
			"using",
			"workflow",
			"parallel",
			"sequence",
			"inlinescript",
			"configuration",
			"public",
			"private",
			"static",
			"interface",
			"enum",
			"namespace",
			"module",
			"type",
			"assembly",
			"command",
			"hidden",
			"base"
		};

		// Token: 0x04002074 RID: 8308
		private static readonly TokenKind[] _keywordTokenKind = new TokenKind[]
		{
			TokenKind.ElseIf,
			TokenKind.If,
			TokenKind.Else,
			TokenKind.Switch,
			TokenKind.Foreach,
			TokenKind.From,
			TokenKind.In,
			TokenKind.For,
			TokenKind.While,
			TokenKind.Until,
			TokenKind.Do,
			TokenKind.Try,
			TokenKind.Catch,
			TokenKind.Finally,
			TokenKind.Trap,
			TokenKind.Data,
			TokenKind.Return,
			TokenKind.Continue,
			TokenKind.Break,
			TokenKind.Exit,
			TokenKind.Throw,
			TokenKind.Begin,
			TokenKind.Process,
			TokenKind.End,
			TokenKind.Dynamicparam,
			TokenKind.Function,
			TokenKind.Filter,
			TokenKind.Param,
			TokenKind.Class,
			TokenKind.Define,
			TokenKind.Var,
			TokenKind.Using,
			TokenKind.Workflow,
			TokenKind.Parallel,
			TokenKind.Sequence,
			TokenKind.InlineScript,
			TokenKind.Configuration,
			TokenKind.Public,
			TokenKind.Private,
			TokenKind.Static,
			TokenKind.Interface,
			TokenKind.Enum,
			TokenKind.Namespace,
			TokenKind.Module,
			TokenKind.Type,
			TokenKind.Assembly,
			TokenKind.Command,
			TokenKind.Hidden,
			TokenKind.Base
		};

		// Token: 0x04002075 RID: 8309
		internal static readonly string[] _operatorText = new string[]
		{
			"bnot",
			"not",
			"eq",
			"ieq",
			"ceq",
			"ne",
			"ine",
			"cne",
			"ge",
			"ige",
			"cge",
			"gt",
			"igt",
			"cgt",
			"lt",
			"ilt",
			"clt",
			"le",
			"ile",
			"cle",
			"like",
			"ilike",
			"clike",
			"notlike",
			"inotlike",
			"cnotlike",
			"match",
			"imatch",
			"cmatch",
			"notmatch",
			"inotmatch",
			"cnotmatch",
			"replace",
			"ireplace",
			"creplace",
			"contains",
			"icontains",
			"ccontains",
			"notcontains",
			"inotcontains",
			"cnotcontains",
			"in",
			"iin",
			"cin",
			"notin",
			"inotin",
			"cnotin",
			"split",
			"isplit",
			"csplit",
			"isnot",
			"is",
			"as",
			"f",
			"and",
			"band",
			"or",
			"bor",
			"xor",
			"bxor",
			"join",
			"shl",
			"shr"
		};

		// Token: 0x04002076 RID: 8310
		private static readonly TokenKind[] _operatorTokenKind = new TokenKind[]
		{
			TokenKind.Bnot,
			TokenKind.Not,
			TokenKind.Ieq,
			TokenKind.Ieq,
			TokenKind.Ceq,
			TokenKind.Ine,
			TokenKind.Ine,
			TokenKind.Cne,
			TokenKind.Ige,
			TokenKind.Ige,
			TokenKind.Cge,
			TokenKind.Igt,
			TokenKind.Igt,
			TokenKind.Cgt,
			TokenKind.Ilt,
			TokenKind.Ilt,
			TokenKind.Clt,
			TokenKind.Ile,
			TokenKind.Ile,
			TokenKind.Cle,
			TokenKind.Ilike,
			TokenKind.Ilike,
			TokenKind.Clike,
			TokenKind.Inotlike,
			TokenKind.Inotlike,
			TokenKind.Cnotlike,
			TokenKind.Imatch,
			TokenKind.Imatch,
			TokenKind.Cmatch,
			TokenKind.Inotmatch,
			TokenKind.Inotmatch,
			TokenKind.Cnotmatch,
			TokenKind.Ireplace,
			TokenKind.Ireplace,
			TokenKind.Creplace,
			TokenKind.Icontains,
			TokenKind.Icontains,
			TokenKind.Ccontains,
			TokenKind.Inotcontains,
			TokenKind.Inotcontains,
			TokenKind.Cnotcontains,
			TokenKind.Iin,
			TokenKind.Iin,
			TokenKind.Cin,
			TokenKind.Inotin,
			TokenKind.Inotin,
			TokenKind.Cnotin,
			TokenKind.Isplit,
			TokenKind.Isplit,
			TokenKind.Csplit,
			TokenKind.IsNot,
			TokenKind.Is,
			TokenKind.As,
			TokenKind.Format,
			TokenKind.And,
			TokenKind.Band,
			TokenKind.Or,
			TokenKind.Bor,
			TokenKind.Xor,
			TokenKind.Bxor,
			TokenKind.Join,
			TokenKind.Shl,
			TokenKind.Shr
		};
	}
}
