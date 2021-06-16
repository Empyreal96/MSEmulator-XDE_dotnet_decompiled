using System;
using System.Runtime.Serialization;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class XdePipeException : Exception
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003C73 File Offset: 0x00001E73
		public XdePipeException(string pipeName, string message) : this(pipeName, message, XdePipeError.Unknown)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003C7E File Offset: 0x00001E7E
		public XdePipeException(string pipeName, string message, XdePipeError pipeError) : base(message)
		{
			this.pipeName = pipeName;
			this.PipeError = pipeError;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003C95 File Offset: 0x00001E95
		public XdePipeException(string pipeName, string message, Exception e) : base(message, e)
		{
			this.pipeName = pipeName;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003CA6 File Offset: 0x00001EA6
		protected XdePipeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003CB0 File Offset: 0x00001EB0
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00003CB8 File Offset: 0x00001EB8
		public XdePipeError PipeError
		{
			get
			{
				return (XdePipeError)base.HResult;
			}
			set
			{
				base.HResult = (int)value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003CC1 File Offset: 0x00001EC1
		public override string Message
		{
			get
			{
				return StringUtilities.CurrentCultureFormat(PipeExceptions.PipeMessageFormat, new object[]
				{
					this.pipeName,
					base.Message
				});
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003CE5 File Offset: 0x00001EE5
		public string PipeName
		{
			get
			{
				return this.pipeName;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003CF0 File Offset: 0x00001EF0
		public static XdePipeException GetExceptionFromError(string pipeName, XdePipeError error)
		{
			string message;
			if (error == XdePipeError.ConnectionClosedByServer)
			{
				message = PipeExceptions.ConnectionClosedByTheDevice;
			}
			else
			{
				message = "Uknown error";
			}
			return new XdePipeException(pipeName, message, error);
		}

		// Token: 0x0400002F RID: 47
		private string pipeName;
	}
}
