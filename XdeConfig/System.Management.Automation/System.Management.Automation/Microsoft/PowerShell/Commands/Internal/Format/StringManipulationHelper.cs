using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200051D RID: 1309
	internal sealed class StringManipulationHelper
	{
		// Token: 0x060036F9 RID: 14073 RVA: 0x00128CE4 File Offset: 0x00126EE4
		static StringManipulationHelper()
		{
			StringManipulationHelper.CultureCollection.Add("en");
			StringManipulationHelper.CultureCollection.Add("fr");
			StringManipulationHelper.CultureCollection.Add("de");
			StringManipulationHelper.CultureCollection.Add("it");
			StringManipulationHelper.CultureCollection.Add("pt");
			StringManipulationHelper.CultureCollection.Add("es");
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x00129024 File Offset: 0x00127224
		private static IEnumerable<GetWordsResult> GetWords(string s)
		{
			StringBuilder sb = new StringBuilder();
			GetWordsResult result = default(GetWordsResult);
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == ' ' || s[i] == '\t' || s[i] == StringManipulationHelper.SoftHyphen)
				{
					result.Word = sb.ToString();
					sb.Clear();
					result.Delim = new string(s[i], 1);
					yield return result;
				}
				else if (s[i] == StringManipulationHelper.HardHyphen || s[i] == StringManipulationHelper.NonBreakingSpace)
				{
					result.Word = sb.ToString();
					sb.Clear();
					result.Delim = string.Empty;
					yield return result;
				}
				else
				{
					sb.Append(s[i]);
				}
			}
			result.Word = sb.ToString();
			result.Delim = string.Empty;
			yield return result;
			yield break;
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x00129041 File Offset: 0x00127241
		internal static StringCollection GenerateLines(DisplayCells displayCells, string val, int firstLineLen, int followingLinesLen)
		{
			if (StringManipulationHelper.CultureCollection.Contains(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
			{
				return StringManipulationHelper.GenerateLinesWithWordWrap(displayCells, val, firstLineLen, followingLinesLen);
			}
			return StringManipulationHelper.GenerateLinesWithoutWordWrap(displayCells, val, firstLineLen, followingLinesLen);
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x0012906C File Offset: 0x0012726C
		private static StringCollection GenerateLinesWithoutWordWrap(DisplayCells displayCells, string val, int firstLineLen, int followingLinesLen)
		{
			StringCollection stringCollection = new StringCollection();
			if (string.IsNullOrEmpty(val))
			{
				stringCollection.Add(val);
				return stringCollection;
			}
			string[] array = StringManipulationHelper.SplitLines(val);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || displayCells.Length(array[i]) <= firstLineLen)
				{
					stringCollection.Add(array[i]);
				}
				else
				{
					StringManipulationHelper.SplitLinesAccumulator splitLinesAccumulator = new StringManipulationHelper.SplitLinesAccumulator(stringCollection, firstLineLen, followingLinesLen);
					int num = 0;
					for (;;)
					{
						int activeLen = splitLinesAccumulator.ActiveLen;
						int num2 = displayCells.Length(array[i], num);
						int num3 = num2 - activeLen;
						if (num3 <= 0)
						{
							break;
						}
						int num4 = displayCells.GetHeadSplitLength(array[i], num, activeLen);
						if (num4 <= 0)
						{
							num4 = 1;
							splitLinesAccumulator.AddLine("?");
						}
						else
						{
							splitLinesAccumulator.AddLine(array[i].Substring(num, num4));
						}
						num += num4;
					}
					splitLinesAccumulator.AddLine(array[i].Substring(num));
				}
			}
			return stringCollection;
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x00129148 File Offset: 0x00127348
		private static StringCollection GenerateLinesWithWordWrap(DisplayCells displayCells, string val, int firstLineLen, int followingLinesLen)
		{
			StringCollection stringCollection = new StringCollection();
			if (string.IsNullOrEmpty(val))
			{
				stringCollection.Add(val);
				return stringCollection;
			}
			string[] array = StringManipulationHelper.SplitLines(val);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || displayCells.Length(array[i]) <= firstLineLen)
				{
					stringCollection.Add(array[i]);
				}
				else
				{
					int num = firstLineLen;
					int num2 = firstLineLen;
					bool flag = true;
					StringBuilder stringBuilder = new StringBuilder();
					foreach (GetWordsResult getWordsResult in StringManipulationHelper.GetWords(array[i]))
					{
						string text = getWordsResult.Word;
						if (getWordsResult.Delim == StringManipulationHelper.SoftHyphen.ToString())
						{
							int num3 = displayCells.Length(text) + displayCells.Length(StringManipulationHelper.SoftHyphen.ToString());
							if (num3 == num)
							{
								text += "-";
							}
						}
						else if (!string.IsNullOrEmpty(getWordsResult.Delim))
						{
							text += getWordsResult.Delim;
						}
						int num4 = displayCells.Length(text);
						if (num2 == 0)
						{
							if (flag)
							{
								flag = false;
								num2 = followingLinesLen;
							}
							if (num2 == 0)
							{
								break;
							}
							num = num2;
						}
						if (num4 > num2)
						{
							foreach (char c in text)
							{
								char value = c;
								int num5 = displayCells.Length(c);
								if (num5 > num2)
								{
									value = '?';
									num5 = 1;
								}
								if (num5 > num)
								{
									stringCollection.Add(stringBuilder.ToString());
									stringBuilder.Clear();
									stringBuilder.Append(value);
									if (flag)
									{
										flag = false;
										num2 = followingLinesLen;
									}
									num = num2 - num5;
								}
								else
								{
									stringBuilder.Append(value);
									num -= num5;
								}
							}
						}
						else if (num4 > num)
						{
							stringCollection.Add(stringBuilder.ToString());
							stringBuilder.Clear();
							stringBuilder.Append(text);
							if (flag)
							{
								flag = false;
								num2 = followingLinesLen;
							}
							num = num2 - num4;
						}
						else
						{
							stringBuilder.Append(text);
							num -= num4;
						}
					}
					stringCollection.Add(stringBuilder.ToString());
				}
			}
			return stringCollection;
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x0012938C File Offset: 0x0012758C
		internal static string[] SplitLines(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return new string[]
				{
					s
				};
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in s)
			{
				if (c != '\r')
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString().Split(StringManipulationHelper.newLineChar);
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x001293F4 File Offset: 0x001275F4
		internal static string TruncateAtNewLine(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			int num = s.IndexOfAny(StringManipulationHelper.lineBreakChars);
			if (num < 0)
			{
				return s;
			}
			return s.Substring(0, num) + "...";
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x0012942F File Offset: 0x0012762F
		internal static string PadLeft(string val, int count)
		{
			return new string(' ', count) + val;
		}

		// Token: 0x04001C22 RID: 7202
		private static readonly char SoftHyphen = '­';

		// Token: 0x04001C23 RID: 7203
		private static readonly char HardHyphen = '‑';

		// Token: 0x04001C24 RID: 7204
		private static readonly char NonBreakingSpace = '\u00a0';

		// Token: 0x04001C25 RID: 7205
		private static Collection<string> CultureCollection = new Collection<string>();

		// Token: 0x04001C26 RID: 7206
		private static readonly char[] newLineChar = new char[]
		{
			'\n'
		};

		// Token: 0x04001C27 RID: 7207
		private static readonly char[] lineBreakChars = new char[]
		{
			'\n',
			'\r'
		};

		// Token: 0x0200051E RID: 1310
		private sealed class SplitLinesAccumulator
		{
			// Token: 0x06003702 RID: 14082 RVA: 0x00129447 File Offset: 0x00127647
			internal SplitLinesAccumulator(StringCollection retVal, int firstLineLen, int followingLinesLen)
			{
				this._retVal = retVal;
				this._firstLineLen = firstLineLen;
				this._followingLinesLen = followingLinesLen;
			}

			// Token: 0x06003703 RID: 14083 RVA: 0x00129464 File Offset: 0x00127664
			internal void AddLine(string s)
			{
				if (!this._addedFirstLine)
				{
					this._addedFirstLine = true;
				}
				this._retVal.Add(s);
			}

			// Token: 0x17000C2B RID: 3115
			// (get) Token: 0x06003704 RID: 14084 RVA: 0x00129482 File Offset: 0x00127682
			internal int ActiveLen
			{
				get
				{
					if (this._addedFirstLine)
					{
						return this._followingLinesLen;
					}
					return this._firstLineLen;
				}
			}

			// Token: 0x04001C28 RID: 7208
			private StringCollection _retVal;

			// Token: 0x04001C29 RID: 7209
			private bool _addedFirstLine;

			// Token: 0x04001C2A RID: 7210
			private int _firstLineLen;

			// Token: 0x04001C2B RID: 7211
			private int _followingLinesLen;
		}
	}
}
