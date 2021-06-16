using System;

namespace CommandLine
{
	// Token: 0x0200003E RID: 62
	public sealed class SetValueExceptionError : NamedError
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00005399 File Offset: 0x00003599
		internal SetValueExceptionError(NameInfo nameInfo, Exception exception, object value) : base(ErrorType.SetValueExceptionError, nameInfo)
		{
			this.exception = exception;
			this.value = value;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000053B2 File Offset: 0x000035B2
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000053BA File Offset: 0x000035BA
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000063 RID: 99
		private readonly Exception exception;

		// Token: 0x04000064 RID: 100
		private readonly object value;
	}
}
