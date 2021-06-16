using System;

namespace System.Management.Automation
{
	// Token: 0x020008AD RID: 2221
	[Flags]
	public enum PSTraceSourceOptions
	{
		// Token: 0x04002B80 RID: 11136
		None = 0,
		// Token: 0x04002B81 RID: 11137
		Constructor = 1,
		// Token: 0x04002B82 RID: 11138
		Dispose = 2,
		// Token: 0x04002B83 RID: 11139
		Finalizer = 4,
		// Token: 0x04002B84 RID: 11140
		Method = 8,
		// Token: 0x04002B85 RID: 11141
		Property = 16,
		// Token: 0x04002B86 RID: 11142
		Delegates = 32,
		// Token: 0x04002B87 RID: 11143
		Events = 64,
		// Token: 0x04002B88 RID: 11144
		Exception = 128,
		// Token: 0x04002B89 RID: 11145
		Lock = 256,
		// Token: 0x04002B8A RID: 11146
		Error = 512,
		// Token: 0x04002B8B RID: 11147
		Warning = 1024,
		// Token: 0x04002B8C RID: 11148
		Verbose = 2048,
		// Token: 0x04002B8D RID: 11149
		WriteLine = 4096,
		// Token: 0x04002B8E RID: 11150
		Scope = 8192,
		// Token: 0x04002B8F RID: 11151
		Assert = 16384,
		// Token: 0x04002B90 RID: 11152
		ExecutionFlow = 8303,
		// Token: 0x04002B91 RID: 11153
		Data = 6167,
		// Token: 0x04002B92 RID: 11154
		Errors = 640,
		// Token: 0x04002B93 RID: 11155
		All = 32767
	}
}
