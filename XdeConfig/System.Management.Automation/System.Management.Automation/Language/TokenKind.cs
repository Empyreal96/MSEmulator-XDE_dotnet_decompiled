using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005CB RID: 1483
	public enum TokenKind
	{
		// Token: 0x04001F6E RID: 8046
		Unknown,
		// Token: 0x04001F6F RID: 8047
		Variable,
		// Token: 0x04001F70 RID: 8048
		SplattedVariable,
		// Token: 0x04001F71 RID: 8049
		Parameter,
		// Token: 0x04001F72 RID: 8050
		Number,
		// Token: 0x04001F73 RID: 8051
		Label,
		// Token: 0x04001F74 RID: 8052
		Identifier,
		// Token: 0x04001F75 RID: 8053
		Generic,
		// Token: 0x04001F76 RID: 8054
		NewLine,
		// Token: 0x04001F77 RID: 8055
		LineContinuation,
		// Token: 0x04001F78 RID: 8056
		Comment,
		// Token: 0x04001F79 RID: 8057
		EndOfInput,
		// Token: 0x04001F7A RID: 8058
		StringLiteral,
		// Token: 0x04001F7B RID: 8059
		StringExpandable,
		// Token: 0x04001F7C RID: 8060
		HereStringLiteral,
		// Token: 0x04001F7D RID: 8061
		HereStringExpandable,
		// Token: 0x04001F7E RID: 8062
		LParen,
		// Token: 0x04001F7F RID: 8063
		RParen,
		// Token: 0x04001F80 RID: 8064
		LCurly,
		// Token: 0x04001F81 RID: 8065
		RCurly,
		// Token: 0x04001F82 RID: 8066
		LBracket,
		// Token: 0x04001F83 RID: 8067
		RBracket,
		// Token: 0x04001F84 RID: 8068
		AtParen,
		// Token: 0x04001F85 RID: 8069
		AtCurly,
		// Token: 0x04001F86 RID: 8070
		DollarParen,
		// Token: 0x04001F87 RID: 8071
		Semi,
		// Token: 0x04001F88 RID: 8072
		AndAnd,
		// Token: 0x04001F89 RID: 8073
		OrOr,
		// Token: 0x04001F8A RID: 8074
		Ampersand,
		// Token: 0x04001F8B RID: 8075
		Pipe,
		// Token: 0x04001F8C RID: 8076
		Comma,
		// Token: 0x04001F8D RID: 8077
		MinusMinus,
		// Token: 0x04001F8E RID: 8078
		PlusPlus,
		// Token: 0x04001F8F RID: 8079
		DotDot,
		// Token: 0x04001F90 RID: 8080
		ColonColon,
		// Token: 0x04001F91 RID: 8081
		Dot,
		// Token: 0x04001F92 RID: 8082
		Exclaim,
		// Token: 0x04001F93 RID: 8083
		Multiply,
		// Token: 0x04001F94 RID: 8084
		Divide,
		// Token: 0x04001F95 RID: 8085
		Rem,
		// Token: 0x04001F96 RID: 8086
		Plus,
		// Token: 0x04001F97 RID: 8087
		Minus,
		// Token: 0x04001F98 RID: 8088
		Equals,
		// Token: 0x04001F99 RID: 8089
		PlusEquals,
		// Token: 0x04001F9A RID: 8090
		MinusEquals,
		// Token: 0x04001F9B RID: 8091
		MultiplyEquals,
		// Token: 0x04001F9C RID: 8092
		DivideEquals,
		// Token: 0x04001F9D RID: 8093
		RemainderEquals,
		// Token: 0x04001F9E RID: 8094
		Redirection,
		// Token: 0x04001F9F RID: 8095
		RedirectInStd,
		// Token: 0x04001FA0 RID: 8096
		Format,
		// Token: 0x04001FA1 RID: 8097
		Not,
		// Token: 0x04001FA2 RID: 8098
		Bnot,
		// Token: 0x04001FA3 RID: 8099
		And,
		// Token: 0x04001FA4 RID: 8100
		Or,
		// Token: 0x04001FA5 RID: 8101
		Xor,
		// Token: 0x04001FA6 RID: 8102
		Band,
		// Token: 0x04001FA7 RID: 8103
		Bor,
		// Token: 0x04001FA8 RID: 8104
		Bxor,
		// Token: 0x04001FA9 RID: 8105
		Join,
		// Token: 0x04001FAA RID: 8106
		Ieq,
		// Token: 0x04001FAB RID: 8107
		Ine,
		// Token: 0x04001FAC RID: 8108
		Ige,
		// Token: 0x04001FAD RID: 8109
		Igt,
		// Token: 0x04001FAE RID: 8110
		Ilt,
		// Token: 0x04001FAF RID: 8111
		Ile,
		// Token: 0x04001FB0 RID: 8112
		Ilike,
		// Token: 0x04001FB1 RID: 8113
		Inotlike,
		// Token: 0x04001FB2 RID: 8114
		Imatch,
		// Token: 0x04001FB3 RID: 8115
		Inotmatch,
		// Token: 0x04001FB4 RID: 8116
		Ireplace,
		// Token: 0x04001FB5 RID: 8117
		Icontains,
		// Token: 0x04001FB6 RID: 8118
		Inotcontains,
		// Token: 0x04001FB7 RID: 8119
		Iin,
		// Token: 0x04001FB8 RID: 8120
		Inotin,
		// Token: 0x04001FB9 RID: 8121
		Isplit,
		// Token: 0x04001FBA RID: 8122
		Ceq,
		// Token: 0x04001FBB RID: 8123
		Cne,
		// Token: 0x04001FBC RID: 8124
		Cge,
		// Token: 0x04001FBD RID: 8125
		Cgt,
		// Token: 0x04001FBE RID: 8126
		Clt,
		// Token: 0x04001FBF RID: 8127
		Cle,
		// Token: 0x04001FC0 RID: 8128
		Clike,
		// Token: 0x04001FC1 RID: 8129
		Cnotlike,
		// Token: 0x04001FC2 RID: 8130
		Cmatch,
		// Token: 0x04001FC3 RID: 8131
		Cnotmatch,
		// Token: 0x04001FC4 RID: 8132
		Creplace,
		// Token: 0x04001FC5 RID: 8133
		Ccontains,
		// Token: 0x04001FC6 RID: 8134
		Cnotcontains,
		// Token: 0x04001FC7 RID: 8135
		Cin,
		// Token: 0x04001FC8 RID: 8136
		Cnotin,
		// Token: 0x04001FC9 RID: 8137
		Csplit,
		// Token: 0x04001FCA RID: 8138
		Is,
		// Token: 0x04001FCB RID: 8139
		IsNot,
		// Token: 0x04001FCC RID: 8140
		As,
		// Token: 0x04001FCD RID: 8141
		PostfixPlusPlus,
		// Token: 0x04001FCE RID: 8142
		PostfixMinusMinus,
		// Token: 0x04001FCF RID: 8143
		Shl,
		// Token: 0x04001FD0 RID: 8144
		Shr,
		// Token: 0x04001FD1 RID: 8145
		Colon,
		// Token: 0x04001FD2 RID: 8146
		Begin = 119,
		// Token: 0x04001FD3 RID: 8147
		Break,
		// Token: 0x04001FD4 RID: 8148
		Catch,
		// Token: 0x04001FD5 RID: 8149
		Class,
		// Token: 0x04001FD6 RID: 8150
		Continue,
		// Token: 0x04001FD7 RID: 8151
		Data,
		// Token: 0x04001FD8 RID: 8152
		Define,
		// Token: 0x04001FD9 RID: 8153
		Do,
		// Token: 0x04001FDA RID: 8154
		Dynamicparam,
		// Token: 0x04001FDB RID: 8155
		Else,
		// Token: 0x04001FDC RID: 8156
		ElseIf,
		// Token: 0x04001FDD RID: 8157
		End,
		// Token: 0x04001FDE RID: 8158
		Exit,
		// Token: 0x04001FDF RID: 8159
		Filter,
		// Token: 0x04001FE0 RID: 8160
		Finally,
		// Token: 0x04001FE1 RID: 8161
		For,
		// Token: 0x04001FE2 RID: 8162
		Foreach,
		// Token: 0x04001FE3 RID: 8163
		From,
		// Token: 0x04001FE4 RID: 8164
		Function,
		// Token: 0x04001FE5 RID: 8165
		If,
		// Token: 0x04001FE6 RID: 8166
		In,
		// Token: 0x04001FE7 RID: 8167
		Param,
		// Token: 0x04001FE8 RID: 8168
		Process,
		// Token: 0x04001FE9 RID: 8169
		Return,
		// Token: 0x04001FEA RID: 8170
		Switch,
		// Token: 0x04001FEB RID: 8171
		Throw,
		// Token: 0x04001FEC RID: 8172
		Trap,
		// Token: 0x04001FED RID: 8173
		Try,
		// Token: 0x04001FEE RID: 8174
		Until,
		// Token: 0x04001FEF RID: 8175
		Using,
		// Token: 0x04001FF0 RID: 8176
		Var,
		// Token: 0x04001FF1 RID: 8177
		While,
		// Token: 0x04001FF2 RID: 8178
		Workflow,
		// Token: 0x04001FF3 RID: 8179
		Parallel,
		// Token: 0x04001FF4 RID: 8180
		Sequence,
		// Token: 0x04001FF5 RID: 8181
		InlineScript,
		// Token: 0x04001FF6 RID: 8182
		Configuration,
		// Token: 0x04001FF7 RID: 8183
		DynamicKeyword,
		// Token: 0x04001FF8 RID: 8184
		Public,
		// Token: 0x04001FF9 RID: 8185
		Private,
		// Token: 0x04001FFA RID: 8186
		Static,
		// Token: 0x04001FFB RID: 8187
		Interface,
		// Token: 0x04001FFC RID: 8188
		Enum,
		// Token: 0x04001FFD RID: 8189
		Namespace,
		// Token: 0x04001FFE RID: 8190
		Module,
		// Token: 0x04001FFF RID: 8191
		Type,
		// Token: 0x04002000 RID: 8192
		Assembly,
		// Token: 0x04002001 RID: 8193
		Command,
		// Token: 0x04002002 RID: 8194
		Hidden,
		// Token: 0x04002003 RID: 8195
		Base
	}
}
