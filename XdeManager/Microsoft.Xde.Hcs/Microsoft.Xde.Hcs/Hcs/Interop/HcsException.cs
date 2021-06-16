using System;
using System.Runtime.InteropServices;
using HCS.Compute.Error;
using HCS.Compute.System;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x0200000B RID: 11
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public class HcsException : Exception
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00003ED1 File Offset: 0x000020D1
		public HcsException(int resultCode, string result) : this(Marshal.GetExceptionForHR(resultCode), result)
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003EE0 File Offset: 0x000020E0
		private HcsException(Exception e, string result) : base(HcsException.ErrorString(e, result), e)
		{
			base.HResult = e.HResult;
			this.ExtendedInfo = result;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003F03 File Offset: 0x00002103
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003F0B File Offset: 0x0000210B
		public string ExtendedInfo { get; private set; }

		// Token: 0x06000072 RID: 114 RVA: 0x00003F14 File Offset: 0x00002114
		public static bool Failed(int resultCode)
		{
			return resultCode != 0 && resultCode != -1070137072;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003F28 File Offset: 0x00002128
		private static string ErrorString(Exception e, string result)
		{
			string text = StringUtilities.CurrentCultureFormat(Strings.HCSFunctionCallError, new object[]
			{
				e.Message
			});
			if (result != null)
			{
				ResultError resultError = JsonHelper.FromJson<ResultError>(result);
				if (resultError.ErrorEvents != null)
				{
					foreach (ErrorEvent errorEvent in resultError.ErrorEvents)
					{
						string message = errorEvent.Message;
						if (!string.IsNullOrEmpty(message))
						{
							text = text + "\n" + message;
						}
						string stackTrace = errorEvent.StackTrace;
						if (!string.IsNullOrEmpty(stackTrace))
						{
							text = "\n" + stackTrace;
						}
					}
				}
			}
			return text;
		}

		// Token: 0x04000025 RID: 37
		public const int Success = 0;

		// Token: 0x04000026 RID: 38
		public const int Pending = -1070137085;

		// Token: 0x04000027 RID: 39
		public const int AlreadyStopped = -1070137072;

		// Token: 0x04000028 RID: 40
		public const int Abort = -2147467260;

		// Token: 0x04000029 RID: 41
		public const int UnexpectedExit = -1070137082;

		// Token: 0x0400002A RID: 42
		public const int AccessDenied = -2143878885;
	}
}
