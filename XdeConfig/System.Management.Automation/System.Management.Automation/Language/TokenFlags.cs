using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005CC RID: 1484
	[Flags]
	public enum TokenFlags
	{
		// Token: 0x04002005 RID: 8197
		None = 0,
		// Token: 0x04002006 RID: 8198
		BinaryPrecedenceLogical = 1,
		// Token: 0x04002007 RID: 8199
		BinaryPrecedenceBitwise = 2,
		// Token: 0x04002008 RID: 8200
		BinaryPrecedenceComparison = 3,
		// Token: 0x04002009 RID: 8201
		BinaryPrecedenceAdd = 4,
		// Token: 0x0400200A RID: 8202
		BinaryPrecedenceMultiply = 5,
		// Token: 0x0400200B RID: 8203
		BinaryPrecedenceFormat = 6,
		// Token: 0x0400200C RID: 8204
		BinaryPrecedenceRange = 7,
		// Token: 0x0400200D RID: 8205
		BinaryPrecedenceMask = 7,
		// Token: 0x0400200E RID: 8206
		Keyword = 16,
		// Token: 0x0400200F RID: 8207
		ScriptBlockBlockName = 32,
		// Token: 0x04002010 RID: 8208
		BinaryOperator = 256,
		// Token: 0x04002011 RID: 8209
		UnaryOperator = 512,
		// Token: 0x04002012 RID: 8210
		CaseSensitiveOperator = 1024,
		// Token: 0x04002013 RID: 8211
		SpecialOperator = 4096,
		// Token: 0x04002014 RID: 8212
		AssignmentOperator = 8192,
		// Token: 0x04002015 RID: 8213
		ParseModeInvariant = 32768,
		// Token: 0x04002016 RID: 8214
		TokenInError = 65536,
		// Token: 0x04002017 RID: 8215
		DisallowedInRestrictedMode = 131072,
		// Token: 0x04002018 RID: 8216
		PrefixOrPostfixOperator = 262144,
		// Token: 0x04002019 RID: 8217
		CommandName = 524288,
		// Token: 0x0400201A RID: 8218
		MemberName = 1048576,
		// Token: 0x0400201B RID: 8219
		TypeName = 2097152,
		// Token: 0x0400201C RID: 8220
		AttributeName = 4194304,
		// Token: 0x0400201D RID: 8221
		CanConstantFold = 8388608,
		// Token: 0x0400201E RID: 8222
		StatementDoesntSupportAttributes = 16777216
	}
}
