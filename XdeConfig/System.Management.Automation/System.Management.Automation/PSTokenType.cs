using System;

namespace System.Management.Automation
{
	// Token: 0x0200049C RID: 1180
	public enum PSTokenType
	{
		// Token: 0x04001B01 RID: 6913
		Unknown,
		// Token: 0x04001B02 RID: 6914
		Command,
		// Token: 0x04001B03 RID: 6915
		CommandParameter,
		// Token: 0x04001B04 RID: 6916
		CommandArgument,
		// Token: 0x04001B05 RID: 6917
		Number,
		// Token: 0x04001B06 RID: 6918
		String,
		// Token: 0x04001B07 RID: 6919
		Variable,
		// Token: 0x04001B08 RID: 6920
		Member,
		// Token: 0x04001B09 RID: 6921
		LoopLabel,
		// Token: 0x04001B0A RID: 6922
		Attribute,
		// Token: 0x04001B0B RID: 6923
		Type,
		// Token: 0x04001B0C RID: 6924
		Operator,
		// Token: 0x04001B0D RID: 6925
		GroupStart,
		// Token: 0x04001B0E RID: 6926
		GroupEnd,
		// Token: 0x04001B0F RID: 6927
		Keyword,
		// Token: 0x04001B10 RID: 6928
		Comment,
		// Token: 0x04001B11 RID: 6929
		StatementSeparator,
		// Token: 0x04001B12 RID: 6930
		NewLine,
		// Token: 0x04001B13 RID: 6931
		LineContinuation,
		// Token: 0x04001B14 RID: 6932
		Position
	}
}
