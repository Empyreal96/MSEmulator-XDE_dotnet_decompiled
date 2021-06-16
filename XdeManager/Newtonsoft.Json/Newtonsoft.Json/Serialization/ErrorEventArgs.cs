using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000074 RID: 116
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0001C04B File Offset: 0x0001A24B
		public object CurrentObject { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x0001C053 File Offset: 0x0001A253
		public ErrorContext ErrorContext { get; }

		// Token: 0x06000664 RID: 1636 RVA: 0x0001C05B File Offset: 0x0001A25B
		public ErrorEventArgs(object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
