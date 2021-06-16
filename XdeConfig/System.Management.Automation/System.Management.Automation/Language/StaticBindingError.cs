using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000999 RID: 2457
	public class StaticBindingError
	{
		// Token: 0x06005A98 RID: 23192 RVA: 0x001E6D5A File Offset: 0x001E4F5A
		internal StaticBindingError(CommandElementAst commandElement, ParameterBindingException exception)
		{
			this.CommandElement = commandElement;
			this.BindingException = exception;
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x06005A99 RID: 23193 RVA: 0x001E6D70 File Offset: 0x001E4F70
		// (set) Token: 0x06005A9A RID: 23194 RVA: 0x001E6D78 File Offset: 0x001E4F78
		public CommandElementAst CommandElement { get; private set; }

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06005A9B RID: 23195 RVA: 0x001E6D81 File Offset: 0x001E4F81
		// (set) Token: 0x06005A9C RID: 23196 RVA: 0x001E6D89 File Offset: 0x001E4F89
		public ParameterBindingException BindingException { get; private set; }
	}
}
