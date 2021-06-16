using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Management.Automation
{
	// Token: 0x0200043C RID: 1084
	internal class WildcardPatternMatcher
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x00105074 File Offset: 0x00103274
		internal WildcardPatternMatcher(WildcardPattern wildcardPattern)
		{
			this._characterNormalizer = new WildcardPatternMatcher.CharacterNormalizer(wildcardPattern.Options);
			this._patternElements = WildcardPatternMatcher.MyWildcardPatternParser.Parse(wildcardPattern, this._characterNormalizer);
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x001050A0 File Offset: 0x001032A0
		internal bool IsMatch(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length);
			foreach (char x in str)
			{
				stringBuilder.Append(this._characterNormalizer.Normalize(x));
			}
			str = stringBuilder.ToString();
			WildcardPatternMatcher.PatternPositionsVisitor patternPositionsVisitor = new WildcardPatternMatcher.PatternPositionsVisitor(this._patternElements.Length);
			patternPositionsVisitor.Add(0);
			WildcardPatternMatcher.PatternPositionsVisitor patternPositionsVisitor2 = new WildcardPatternMatcher.PatternPositionsVisitor(this._patternElements.Length);
			for (int j = 0; j < str.Length; j++)
			{
				char currentStringCharacter = str[j];
				patternPositionsVisitor.StringPosition = j;
				patternPositionsVisitor2.StringPosition = j + 1;
				int num;
				while (patternPositionsVisitor.MoveNext(out num))
				{
					this._patternElements[num].ProcessStringCharacter(currentStringCharacter, num, patternPositionsVisitor, patternPositionsVisitor2);
				}
				WildcardPatternMatcher.PatternPositionsVisitor patternPositionsVisitor3 = patternPositionsVisitor;
				patternPositionsVisitor = patternPositionsVisitor2;
				patternPositionsVisitor2 = patternPositionsVisitor3;
			}
			int num2;
			while (patternPositionsVisitor.MoveNext(out num2))
			{
				this._patternElements[num2].ProcessEndOfString(num2, patternPositionsVisitor);
			}
			return patternPositionsVisitor.ReachedEndOfPattern;
		}

		// Token: 0x040019C8 RID: 6600
		private readonly WildcardPatternMatcher.PatternElement[] _patternElements;

		// Token: 0x040019C9 RID: 6601
		private readonly WildcardPatternMatcher.CharacterNormalizer _characterNormalizer;

		// Token: 0x0200043D RID: 1085
		private class PatternPositionsVisitor
		{
			// Token: 0x06002FAA RID: 12202 RVA: 0x00105198 File Offset: 0x00103398
			public PatternPositionsVisitor(int lengthOfPattern)
			{
				this._lengthOfPattern = lengthOfPattern;
				this._isPatternPositionVisitedMarker = new int[lengthOfPattern + 1];
				for (int i = 0; i < this._isPatternPositionVisitedMarker.Length; i++)
				{
					this._isPatternPositionVisitedMarker[i] = -1;
				}
				this._patternPositionsForFurtherProcessing = new int[lengthOfPattern];
				this._patternPositionsForFurtherProcessingCount = 0;
			}

			// Token: 0x17000B0D RID: 2829
			// (get) Token: 0x06002FAB RID: 12203 RVA: 0x001051EF File Offset: 0x001033EF
			// (set) Token: 0x06002FAC RID: 12204 RVA: 0x001051F7 File Offset: 0x001033F7
			public int StringPosition { private get; set; }

			// Token: 0x06002FAD RID: 12205 RVA: 0x00105200 File Offset: 0x00103400
			public void Add(int patternPosition)
			{
				if (this._isPatternPositionVisitedMarker[patternPosition] == this.StringPosition)
				{
					return;
				}
				this._isPatternPositionVisitedMarker[patternPosition] = this.StringPosition;
				if (patternPosition < this._lengthOfPattern)
				{
					this._patternPositionsForFurtherProcessing[this._patternPositionsForFurtherProcessingCount] = patternPosition;
					this._patternPositionsForFurtherProcessingCount++;
				}
			}

			// Token: 0x17000B0E RID: 2830
			// (get) Token: 0x06002FAE RID: 12206 RVA: 0x00105251 File Offset: 0x00103451
			public bool ReachedEndOfPattern
			{
				get
				{
					return this._isPatternPositionVisitedMarker[this._lengthOfPattern] >= this.StringPosition;
				}
			}

			// Token: 0x06002FAF RID: 12207 RVA: 0x0010526B File Offset: 0x0010346B
			public bool MoveNext(out int patternPosition)
			{
				if (this._patternPositionsForFurtherProcessingCount == 0)
				{
					patternPosition = -1;
					return false;
				}
				this._patternPositionsForFurtherProcessingCount--;
				patternPosition = this._patternPositionsForFurtherProcessing[this._patternPositionsForFurtherProcessingCount];
				return true;
			}

			// Token: 0x040019CA RID: 6602
			private readonly int _lengthOfPattern;

			// Token: 0x040019CB RID: 6603
			private readonly int[] _isPatternPositionVisitedMarker;

			// Token: 0x040019CC RID: 6604
			private readonly int[] _patternPositionsForFurtherProcessing;

			// Token: 0x040019CD RID: 6605
			private int _patternPositionsForFurtherProcessingCount;
		}

		// Token: 0x0200043E RID: 1086
		private abstract class PatternElement
		{
			// Token: 0x06002FB0 RID: 12208
			public abstract void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForCurrentStringPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForNextStringPosition);

			// Token: 0x06002FB1 RID: 12209
			public abstract void ProcessEndOfString(int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForEndOfStringPosition);
		}

		// Token: 0x0200043F RID: 1087
		private class QuestionMarkElement : WildcardPatternMatcher.PatternElement
		{
			// Token: 0x06002FB3 RID: 12211 RVA: 0x001052A0 File Offset: 0x001034A0
			public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForCurrentStringPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForNextStringPosition)
			{
				patternPositionsForNextStringPosition.Add(currentPatternPosition + 1);
			}

			// Token: 0x06002FB4 RID: 12212 RVA: 0x001052AC File Offset: 0x001034AC
			public override void ProcessEndOfString(int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForEndOfStringPosition)
			{
			}
		}

		// Token: 0x02000440 RID: 1088
		private class LiteralCharacterElement : WildcardPatternMatcher.QuestionMarkElement
		{
			// Token: 0x06002FB6 RID: 12214 RVA: 0x001052B6 File Offset: 0x001034B6
			public LiteralCharacterElement(char literalCharacter)
			{
				this._literalCharacter = literalCharacter;
			}

			// Token: 0x06002FB7 RID: 12215 RVA: 0x001052C5 File Offset: 0x001034C5
			public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForCurrentStringPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForNextStringPosition)
			{
				if (this._literalCharacter == currentStringCharacter)
				{
					base.ProcessStringCharacter(currentStringCharacter, currentPatternPosition, patternPositionsForCurrentStringPosition, patternPositionsForNextStringPosition);
				}
			}

			// Token: 0x040019CF RID: 6607
			private readonly char _literalCharacter;
		}

		// Token: 0x02000441 RID: 1089
		private class BracketExpressionElement : WildcardPatternMatcher.QuestionMarkElement
		{
			// Token: 0x06002FB8 RID: 12216 RVA: 0x001052DB File Offset: 0x001034DB
			public BracketExpressionElement(Regex regex)
			{
				this._regex = regex;
			}

			// Token: 0x06002FB9 RID: 12217 RVA: 0x001052EA File Offset: 0x001034EA
			public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForCurrentStringPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForNextStringPosition)
			{
				if (this._regex.IsMatch(new string(currentStringCharacter, 1)))
				{
					base.ProcessStringCharacter(currentStringCharacter, currentPatternPosition, patternPositionsForCurrentStringPosition, patternPositionsForNextStringPosition);
				}
			}

			// Token: 0x040019D0 RID: 6608
			private readonly Regex _regex;
		}

		// Token: 0x02000442 RID: 1090
		private class AsterixElement : WildcardPatternMatcher.PatternElement
		{
			// Token: 0x06002FBA RID: 12218 RVA: 0x0010530B File Offset: 0x0010350B
			public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForCurrentStringPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForNextStringPosition)
			{
				patternPositionsForCurrentStringPosition.Add(currentPatternPosition + 1);
				patternPositionsForNextStringPosition.Add(currentPatternPosition);
			}

			// Token: 0x06002FBB RID: 12219 RVA: 0x0010531E File Offset: 0x0010351E
			public override void ProcessEndOfString(int currentPatternPosition, WildcardPatternMatcher.PatternPositionsVisitor patternPositionsForEndOfStringPosition)
			{
				patternPositionsForEndOfStringPosition.Add(currentPatternPosition + 1);
			}
		}

		// Token: 0x02000443 RID: 1091
		private class MyWildcardPatternParser : WildcardPatternParser
		{
			// Token: 0x06002FBD RID: 12221 RVA: 0x00105334 File Offset: 0x00103534
			public static WildcardPatternMatcher.PatternElement[] Parse(WildcardPattern pattern, WildcardPatternMatcher.CharacterNormalizer characterNormalizer)
			{
				WildcardPatternMatcher.MyWildcardPatternParser myWildcardPatternParser = new WildcardPatternMatcher.MyWildcardPatternParser
				{
					_characterNormalizer = characterNormalizer,
					_regexOptions = WildcardPatternToRegexParser.TranslateWildcardOptionsIntoRegexOptions(pattern.Options)
				};
				WildcardPatternParser.Parse(pattern, myWildcardPatternParser);
				return myWildcardPatternParser._patternElements.ToArray();
			}

			// Token: 0x06002FBE RID: 12222 RVA: 0x00105373 File Offset: 0x00103573
			protected override void AppendLiteralCharacter(char c)
			{
				c = this._characterNormalizer.Normalize(c);
				this._patternElements.Add(new WildcardPatternMatcher.LiteralCharacterElement(c));
			}

			// Token: 0x06002FBF RID: 12223 RVA: 0x00105394 File Offset: 0x00103594
			protected override void AppendAsterix()
			{
				this._patternElements.Add(new WildcardPatternMatcher.AsterixElement());
			}

			// Token: 0x06002FC0 RID: 12224 RVA: 0x001053A6 File Offset: 0x001035A6
			protected override void AppendQuestionMark()
			{
				this._patternElements.Add(new WildcardPatternMatcher.QuestionMarkElement());
			}

			// Token: 0x06002FC1 RID: 12225 RVA: 0x001053B8 File Offset: 0x001035B8
			protected override void BeginBracketExpression()
			{
				this._bracketExpressionBuilder = new StringBuilder();
				this._bracketExpressionBuilder.Append('[');
			}

			// Token: 0x06002FC2 RID: 12226 RVA: 0x001053D3 File Offset: 0x001035D3
			protected override void AppendLiteralCharacterToBracketExpression(char c)
			{
				WildcardPatternToRegexParser.AppendLiteralCharacterToBracketExpression(this._bracketExpressionBuilder, c);
			}

			// Token: 0x06002FC3 RID: 12227 RVA: 0x001053E1 File Offset: 0x001035E1
			protected override void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange)
			{
				WildcardPatternToRegexParser.AppendCharacterRangeToBracketExpression(this._bracketExpressionBuilder, startOfCharacterRange, endOfCharacterRange);
			}

			// Token: 0x06002FC4 RID: 12228 RVA: 0x001053F0 File Offset: 0x001035F0
			protected override void EndBracketExpression()
			{
				this._bracketExpressionBuilder.Append(']');
				Regex regex = new Regex(this._bracketExpressionBuilder.ToString(), this._regexOptions);
				this._patternElements.Add(new WildcardPatternMatcher.BracketExpressionElement(regex));
			}

			// Token: 0x040019D1 RID: 6609
			private readonly List<WildcardPatternMatcher.PatternElement> _patternElements = new List<WildcardPatternMatcher.PatternElement>();

			// Token: 0x040019D2 RID: 6610
			private WildcardPatternMatcher.CharacterNormalizer _characterNormalizer;

			// Token: 0x040019D3 RID: 6611
			private RegexOptions _regexOptions;

			// Token: 0x040019D4 RID: 6612
			private StringBuilder _bracketExpressionBuilder;
		}

		// Token: 0x02000444 RID: 1092
		private class CharacterNormalizer
		{
			// Token: 0x06002FC6 RID: 12230 RVA: 0x00105446 File Offset: 0x00103646
			public CharacterNormalizer(WildcardOptions options)
			{
				if (WildcardOptions.CultureInvariant == (options & WildcardOptions.CultureInvariant))
				{
					this._cultureInfo = CultureInfo.InvariantCulture;
				}
				else
				{
					this._cultureInfo = CultureInfo.CurrentCulture;
				}
				this._caseInsensitive = (WildcardOptions.IgnoreCase == (options & WildcardOptions.IgnoreCase));
			}

			// Token: 0x06002FC7 RID: 12231 RVA: 0x00105478 File Offset: 0x00103678
			public char Normalize(char x)
			{
				if (this._caseInsensitive)
				{
					return this._cultureInfo.TextInfo.ToLower(x);
				}
				return x;
			}

			// Token: 0x040019D5 RID: 6613
			private readonly CultureInfo _cultureInfo;

			// Token: 0x040019D6 RID: 6614
			private readonly bool _caseInsensitive;
		}
	}
}
