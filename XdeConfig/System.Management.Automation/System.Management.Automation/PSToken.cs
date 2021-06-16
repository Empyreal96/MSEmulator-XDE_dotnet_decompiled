using System;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200049B RID: 1179
	public sealed class PSToken
	{
		// Token: 0x060034D2 RID: 13522 RVA: 0x0011EEDC File Offset: 0x0011D0DC
		internal PSToken(Token token)
		{
			this._type = PSToken.GetPSTokenType(token);
			this._extent = token.Extent;
			if (token is StringToken)
			{
				this._content = ((StringToken)token).Value;
				return;
			}
			if (token is VariableToken)
			{
				this._content = ((VariableToken)token).VariablePath.ToString();
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x0011EF3F File Offset: 0x0011D13F
		internal PSToken(IScriptExtent extent)
		{
			this._type = PSTokenType.Position;
			this._extent = extent;
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060034D4 RID: 13524 RVA: 0x0011EF56 File Offset: 0x0011D156
		public string Content
		{
			get
			{
				return this._content ?? this._extent.Text;
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x0011EF70 File Offset: 0x0011D170
		public static PSTokenType GetPSTokenType(Token token)
		{
			if ((token.TokenFlags & TokenFlags.CommandName) != TokenFlags.None)
			{
				return PSTokenType.Command;
			}
			if ((token.TokenFlags & TokenFlags.MemberName) != TokenFlags.None)
			{
				return PSTokenType.Member;
			}
			if ((token.TokenFlags & TokenFlags.AttributeName) != TokenFlags.None)
			{
				return PSTokenType.Attribute;
			}
			if ((token.TokenFlags & TokenFlags.TypeName) != TokenFlags.None)
			{
				return PSTokenType.Type;
			}
			return PSToken._tokenKindMapping[(int)token.Kind];
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060034D6 RID: 13526 RVA: 0x0011EFCB File Offset: 0x0011D1CB
		public PSTokenType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060034D7 RID: 13527 RVA: 0x0011EFD3 File Offset: 0x0011D1D3
		public int Start
		{
			get
			{
				return this._extent.StartOffset;
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060034D8 RID: 13528 RVA: 0x0011EFE0 File Offset: 0x0011D1E0
		public int Length
		{
			get
			{
				return this._extent.EndOffset - this._extent.StartOffset;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060034D9 RID: 13529 RVA: 0x0011EFF9 File Offset: 0x0011D1F9
		public int StartLine
		{
			get
			{
				return this._extent.StartLineNumber;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060034DA RID: 13530 RVA: 0x0011F006 File Offset: 0x0011D206
		public int StartColumn
		{
			get
			{
				return this._extent.StartColumnNumber;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x0011F013 File Offset: 0x0011D213
		public int EndLine
		{
			get
			{
				return this._extent.EndLineNumber;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x060034DC RID: 13532 RVA: 0x0011F020 File Offset: 0x0011D220
		public int EndColumn
		{
			get
			{
				return this._extent.EndColumnNumber;
			}
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x0011F030 File Offset: 0x0011D230
		// Note: this type is marked as 'beforefieldinit'.
		static PSToken()
		{
			PSTokenType[] array = new PSTokenType[169];
			array[1] = PSTokenType.Variable;
			array[2] = PSTokenType.Variable;
			array[3] = PSTokenType.CommandParameter;
			array[4] = PSTokenType.Number;
			array[5] = PSTokenType.LoopLabel;
			array[6] = PSTokenType.CommandArgument;
			array[7] = PSTokenType.CommandArgument;
			array[8] = PSTokenType.NewLine;
			array[9] = PSTokenType.LineContinuation;
			array[10] = PSTokenType.Comment;
			array[12] = PSTokenType.String;
			array[13] = PSTokenType.String;
			array[14] = PSTokenType.String;
			array[15] = PSTokenType.String;
			array[16] = PSTokenType.GroupStart;
			array[17] = PSTokenType.GroupEnd;
			array[18] = PSTokenType.GroupStart;
			array[19] = PSTokenType.GroupEnd;
			array[20] = PSTokenType.Operator;
			array[21] = PSTokenType.Operator;
			array[22] = PSTokenType.GroupStart;
			array[23] = PSTokenType.GroupStart;
			array[24] = PSTokenType.GroupStart;
			array[25] = PSTokenType.StatementSeparator;
			array[26] = PSTokenType.Operator;
			array[27] = PSTokenType.Operator;
			array[28] = PSTokenType.Operator;
			array[29] = PSTokenType.Operator;
			array[30] = PSTokenType.Operator;
			array[31] = PSTokenType.Operator;
			array[32] = PSTokenType.Operator;
			array[33] = PSTokenType.Operator;
			array[34] = PSTokenType.Operator;
			array[35] = PSTokenType.Operator;
			array[36] = PSTokenType.Operator;
			array[37] = PSTokenType.Operator;
			array[38] = PSTokenType.Operator;
			array[39] = PSTokenType.Operator;
			array[40] = PSTokenType.Operator;
			array[41] = PSTokenType.Operator;
			array[42] = PSTokenType.Operator;
			array[43] = PSTokenType.Operator;
			array[44] = PSTokenType.Operator;
			array[45] = PSTokenType.Operator;
			array[46] = PSTokenType.Operator;
			array[47] = PSTokenType.Operator;
			array[48] = PSTokenType.Operator;
			array[49] = PSTokenType.Operator;
			array[50] = PSTokenType.Operator;
			array[51] = PSTokenType.Operator;
			array[52] = PSTokenType.Operator;
			array[53] = PSTokenType.Operator;
			array[54] = PSTokenType.Operator;
			array[55] = PSTokenType.Operator;
			array[56] = PSTokenType.Operator;
			array[57] = PSTokenType.Operator;
			array[58] = PSTokenType.Operator;
			array[59] = PSTokenType.Operator;
			array[60] = PSTokenType.Operator;
			array[61] = PSTokenType.Operator;
			array[62] = PSTokenType.Operator;
			array[63] = PSTokenType.Operator;
			array[64] = PSTokenType.Operator;
			array[65] = PSTokenType.Operator;
			array[66] = PSTokenType.Operator;
			array[67] = PSTokenType.Operator;
			array[68] = PSTokenType.Operator;
			array[69] = PSTokenType.Operator;
			array[70] = PSTokenType.Operator;
			array[71] = PSTokenType.Operator;
			array[72] = PSTokenType.Operator;
			array[73] = PSTokenType.Operator;
			array[74] = PSTokenType.Operator;
			array[75] = PSTokenType.Operator;
			array[76] = PSTokenType.Operator;
			array[77] = PSTokenType.Operator;
			array[78] = PSTokenType.Operator;
			array[79] = PSTokenType.Operator;
			array[80] = PSTokenType.Operator;
			array[81] = PSTokenType.Operator;
			array[82] = PSTokenType.Operator;
			array[83] = PSTokenType.Operator;
			array[84] = PSTokenType.Operator;
			array[85] = PSTokenType.Operator;
			array[86] = PSTokenType.Operator;
			array[87] = PSTokenType.Operator;
			array[88] = PSTokenType.Operator;
			array[89] = PSTokenType.Operator;
			array[90] = PSTokenType.Operator;
			array[91] = PSTokenType.Operator;
			array[92] = PSTokenType.Operator;
			array[93] = PSTokenType.Operator;
			array[94] = PSTokenType.Operator;
			array[95] = PSTokenType.Operator;
			array[96] = PSTokenType.Operator;
			array[97] = PSTokenType.Operator;
			array[98] = PSTokenType.Operator;
			array[119] = PSTokenType.Keyword;
			array[120] = PSTokenType.Keyword;
			array[121] = PSTokenType.Keyword;
			array[122] = PSTokenType.Keyword;
			array[123] = PSTokenType.Keyword;
			array[124] = PSTokenType.Keyword;
			array[125] = PSTokenType.Keyword;
			array[126] = PSTokenType.Keyword;
			array[127] = PSTokenType.Keyword;
			array[128] = PSTokenType.Keyword;
			array[129] = PSTokenType.Keyword;
			array[130] = PSTokenType.Keyword;
			array[131] = PSTokenType.Keyword;
			array[132] = PSTokenType.Keyword;
			array[133] = PSTokenType.Keyword;
			array[134] = PSTokenType.Keyword;
			array[135] = PSTokenType.Keyword;
			array[136] = PSTokenType.Keyword;
			array[137] = PSTokenType.Keyword;
			array[138] = PSTokenType.Keyword;
			array[139] = PSTokenType.Keyword;
			array[140] = PSTokenType.Keyword;
			array[141] = PSTokenType.Keyword;
			array[142] = PSTokenType.Keyword;
			array[143] = PSTokenType.Keyword;
			array[144] = PSTokenType.Keyword;
			array[145] = PSTokenType.Keyword;
			array[146] = PSTokenType.Keyword;
			array[147] = PSTokenType.Keyword;
			array[148] = PSTokenType.Keyword;
			array[149] = PSTokenType.Keyword;
			array[150] = PSTokenType.Keyword;
			array[151] = PSTokenType.Keyword;
			array[152] = PSTokenType.Keyword;
			array[153] = PSTokenType.Keyword;
			array[154] = PSTokenType.Keyword;
			array[155] = PSTokenType.Keyword;
			array[156] = PSTokenType.Keyword;
			array[157] = PSTokenType.Keyword;
			array[158] = PSTokenType.Keyword;
			array[159] = PSTokenType.Keyword;
			array[160] = PSTokenType.Keyword;
			array[161] = PSTokenType.Keyword;
			array[162] = PSTokenType.Keyword;
			array[163] = PSTokenType.Keyword;
			array[164] = PSTokenType.Keyword;
			array[165] = PSTokenType.Keyword;
			array[166] = PSTokenType.Keyword;
			array[167] = PSTokenType.Keyword;
			PSToken._tokenKindMapping = array;
		}

		// Token: 0x04001AFC RID: 6908
		private string _content;

		// Token: 0x04001AFD RID: 6909
		private PSTokenType _type;

		// Token: 0x04001AFE RID: 6910
		private static readonly PSTokenType[] _tokenKindMapping;

		// Token: 0x04001AFF RID: 6911
		private readonly IScriptExtent _extent;
	}
}
