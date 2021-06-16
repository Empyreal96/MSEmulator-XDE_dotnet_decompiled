using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200059C RID: 1436
	internal static class CharExtensions
	{
		// Token: 0x06003C0D RID: 15373 RVA: 0x001379F8 File Offset: 0x00135BF8
		static CharExtensions()
		{
			CharTraits[] array = new CharTraits[128];
			array[0] = (CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[9] = (CharTraits.Whitespace | CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[10] = (CharTraits.Newline | CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[11] = (CharTraits.Whitespace | CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[12] = (CharTraits.Whitespace | CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[13] = (CharTraits.Newline | CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[32] = (CharTraits.Whitespace | CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[33] = CharTraits.ForceStartNewTokenAfterNumber;
			array[35] = CharTraits.ForceStartNewTokenAfterNumber;
			array[36] = CharTraits.VarNameFirst;
			array[37] = CharTraits.ForceStartNewTokenAfterNumber;
			array[38] = CharTraits.ForceStartNewToken;
			array[40] = CharTraits.ForceStartNewToken;
			array[41] = CharTraits.ForceStartNewToken;
			array[42] = CharTraits.ForceStartNewTokenAfterNumber;
			array[43] = CharTraits.ForceStartNewTokenAfterNumber;
			array[44] = (CharTraits.ForceStartNewToken | CharTraits.ForceStartNewAssemblyNameSpecToken);
			array[45] = CharTraits.ForceStartNewTokenAfterNumber;
			array[46] = CharTraits.ForceStartNewTokenAfterNumber;
			array[47] = CharTraits.ForceStartNewTokenAfterNumber;
			array[48] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[49] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[50] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[51] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[52] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[53] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[54] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[55] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[56] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[57] = (CharTraits.HexDigit | CharTraits.Digit | CharTraits.VarNameFirst);
			array[58] = CharTraits.VarNameFirst;
			array[59] = CharTraits.ForceStartNewToken;
			array[60] = CharTraits.ForceStartNewTokenAfterNumber;
			array[61] = (CharTraits.ForceStartNewAssemblyNameSpecToken | CharTraits.ForceStartNewTokenAfterNumber);
			array[62] = CharTraits.ForceStartNewTokenAfterNumber;
			array[63] = CharTraits.VarNameFirst;
			array[65] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[66] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[67] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[68] = (CharTraits.IdentifierStart | CharTraits.TypeSuffix | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[69] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[70] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[71] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[72] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[73] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[74] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[75] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[76] = (CharTraits.IdentifierStart | CharTraits.TypeSuffix | CharTraits.VarNameFirst);
			array[77] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[78] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[79] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[80] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[81] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[82] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[83] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[84] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[85] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[86] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[87] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[88] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[89] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[90] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[93] = (CharTraits.ForceStartNewAssemblyNameSpecToken | CharTraits.ForceStartNewTokenAfterNumber);
			array[94] = CharTraits.VarNameFirst;
			array[95] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[97] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[98] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[99] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[100] = (CharTraits.IdentifierStart | CharTraits.TypeSuffix | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[101] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[102] = (CharTraits.IdentifierStart | CharTraits.HexDigit | CharTraits.VarNameFirst);
			array[103] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[104] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[105] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[106] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[107] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[108] = (CharTraits.IdentifierStart | CharTraits.TypeSuffix | CharTraits.VarNameFirst);
			array[109] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[110] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[111] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[112] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[113] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[114] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[115] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[116] = (CharTraits.IdentifierStart | CharTraits.MultiplierStart | CharTraits.VarNameFirst);
			array[117] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[118] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[119] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[120] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[121] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[122] = (CharTraits.IdentifierStart | CharTraits.VarNameFirst);
			array[123] = CharTraits.ForceStartNewToken;
			array[124] = CharTraits.ForceStartNewToken;
			array[125] = CharTraits.ForceStartNewToken;
			CharExtensions._traits = array;
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00137D63 File Offset: 0x00135F63
		internal static bool IsWhitespace(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & CharTraits.Whitespace) != CharTraits.None;
			}
			if (c <= 'Ā')
			{
				return c == '\u00a0' || c == '\u0085';
			}
			return char.IsSeparator(c);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00137D9F File Offset: 0x00135F9F
		internal static bool IsDash(this char c)
		{
			return c == '-' || c == '–' || c == '—' || c == '―';
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x00137DC0 File Offset: 0x00135FC0
		internal static bool IsSingleQuote(this char c)
		{
			return c == '\'' || c == '‘' || c == '’' || c == '‚' || c == '‛';
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x00137DE9 File Offset: 0x00135FE9
		internal static bool IsDoubleQuote(this char c)
		{
			return c == '"' || c == '“' || c == '”' || c == '„';
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x00137E0A File Offset: 0x0013600A
		internal static bool IsVariableStart(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & CharTraits.VarNameFirst) != CharTraits.None;
			}
			return char.IsLetterOrDigit(c);
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00137E2E File Offset: 0x0013602E
		internal static bool IsIndentifierStart(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & CharTraits.IdentifierStart) != CharTraits.None;
			}
			return char.IsLetter(c);
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00137E4E File Offset: 0x0013604E
		internal static bool IsIndentifierFollow(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & (CharTraits.IdentifierStart | CharTraits.Digit)) != CharTraits.None;
			}
			return char.IsLetterOrDigit(c);
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00137E72 File Offset: 0x00136072
		internal static bool IsHexDigit(this char c)
		{
			return c < '\u0080' && (CharExtensions._traits[(int)c] & CharTraits.HexDigit) != CharTraits.None;
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00137E8E File Offset: 0x0013608E
		internal static bool IsDecimalDigit(this char c)
		{
			return c < '\u0080' && (CharExtensions._traits[(int)c] & CharTraits.Digit) != CharTraits.None;
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x00137EAD File Offset: 0x001360AD
		internal static bool IsTypeSuffix(this char c)
		{
			return c < '\u0080' && (CharExtensions._traits[(int)c] & CharTraits.TypeSuffix) != CharTraits.None;
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00137EC8 File Offset: 0x001360C8
		internal static bool IsMultiplierStart(this char c)
		{
			return c < '\u0080' && (CharExtensions._traits[(int)c] & CharTraits.MultiplierStart) != CharTraits.None;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x00137EE3 File Offset: 0x001360E3
		internal static bool ForceStartNewToken(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & CharTraits.ForceStartNewToken) != CharTraits.None;
			}
			return c.IsWhitespace();
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x00137F07 File Offset: 0x00136107
		internal static bool ForceStartNewTokenAfterNumber(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & CharTraits.ForceStartNewTokenAfterNumber) != CharTraits.None;
			}
			return c.IsDash();
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x00137F2B File Offset: 0x0013612B
		internal static bool ForceStartNewTokenInAssemblyNameSpec(this char c)
		{
			if (c < '\u0080')
			{
				return (CharExtensions._traits[(int)c] & CharTraits.ForceStartNewAssemblyNameSpecToken) != CharTraits.None;
			}
			return c.IsWhitespace();
		}

		// Token: 0x04001DB6 RID: 7606
		private static readonly CharTraits[] _traits;
	}
}
