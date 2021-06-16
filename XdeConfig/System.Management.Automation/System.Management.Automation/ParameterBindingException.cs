using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000894 RID: 2196
	[Serializable]
	public class ParameterBindingException : RuntimeException
	{
		// Token: 0x06005432 RID: 21554 RVA: 0x001BCDD8 File Offset: 0x001BAFD8
		internal ParameterBindingException(ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(errorCategory, invocationInfo, errorPosition, errorId, null, null)
		{
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentException("resourceString");
			}
			if (string.IsNullOrEmpty(errorId))
			{
				throw PSTraceSource.NewArgumentException("errorId");
			}
			this.invocationInfo = invocationInfo;
			if (this.invocationInfo != null)
			{
				this.commandName = invocationInfo.MyCommand.Name;
			}
			this.parameterName = parameterName;
			this.parameterType = parameterType;
			this.typeSpecified = typeSpecified;
			if (errorPosition == null && this.invocationInfo != null)
			{
				errorPosition = invocationInfo.ScriptPosition;
			}
			if (errorPosition != null)
			{
				this.line = (long)errorPosition.StartLineNumber;
				this.offset = (long)errorPosition.StartColumnNumber;
			}
			this.resourceString = resourceString;
			this.errorId = errorId;
			if (args != null)
			{
				this.args = args;
			}
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x001BCED4 File Offset: 0x001BB0D4
		internal ParameterBindingException(Exception innerException, ErrorCategory errorCategory, InvocationInfo invocationInfo, IScriptExtent errorPosition, string parameterName, Type parameterType, Type typeSpecified, string resourceString, string errorId, params object[] args) : base(errorCategory, invocationInfo, errorPosition, errorId, null, innerException)
		{
			if (invocationInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("invocationInfo");
			}
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentException("resourceString");
			}
			if (string.IsNullOrEmpty(errorId))
			{
				throw PSTraceSource.NewArgumentException("errorId");
			}
			this.invocationInfo = invocationInfo;
			this.commandName = invocationInfo.MyCommand.Name;
			this.parameterName = parameterName;
			this.parameterType = parameterType;
			this.typeSpecified = typeSpecified;
			if (errorPosition == null)
			{
				errorPosition = invocationInfo.ScriptPosition;
			}
			if (errorPosition != null)
			{
				this.line = (long)errorPosition.StartLineNumber;
				this.offset = (long)errorPosition.StartColumnNumber;
			}
			this.resourceString = resourceString;
			this.errorId = errorId;
			if (args != null)
			{
				this.args = args;
			}
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x001BCFD4 File Offset: 0x001BB1D4
		internal ParameterBindingException(Exception innerException, ParameterBindingException pbex, string resourceString, params object[] args) : base(string.Empty, innerException)
		{
			if (pbex == null)
			{
				throw PSTraceSource.NewArgumentNullException("pbex");
			}
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentException("resourceString");
			}
			this.invocationInfo = pbex.CommandInvocation;
			if (this.invocationInfo != null)
			{
				this.commandName = this.invocationInfo.MyCommand.Name;
			}
			IScriptExtent scriptPosition = null;
			if (this.invocationInfo != null)
			{
				scriptPosition = this.invocationInfo.ScriptPosition;
			}
			this.line = pbex.Line;
			this.offset = pbex.Offset;
			this.parameterName = pbex.ParameterName;
			this.parameterType = pbex.ParameterType;
			this.typeSpecified = pbex.TypeSpecified;
			this.errorId = pbex.ErrorId;
			this.resourceString = resourceString;
			if (args != null)
			{
				this.args = args;
			}
			base.SetErrorCategory(pbex.ErrorRecord._category);
			base.SetErrorId(this.errorId);
			if (this.invocationInfo != null)
			{
				base.ErrorRecord.SetInvocationInfo(new InvocationInfo(this.invocationInfo.MyCommand, scriptPosition));
			}
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x001BD120 File Offset: 0x001BB320
		protected ParameterBindingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.message = info.GetString("ParameterBindingException_Message");
			this.parameterName = info.GetString("ParameterName");
			this.line = info.GetInt64("Line");
			this.offset = info.GetInt64("Offset");
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x001BD1B0 File Offset: 0x001BB3B0
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ParameterBindingException_Message", this.Message);
			info.AddValue("ParameterName", this.parameterName);
			info.AddValue("Line", this.line);
			info.AddValue("Offset", this.offset);
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x001BD217 File Offset: 0x001BB417
		public ParameterBindingException()
		{
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x001BD254 File Offset: 0x001BB454
		public ParameterBindingException(string message) : base(message)
		{
			this.message = message;
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x001BD2A4 File Offset: 0x001BB4A4
		public ParameterBindingException(string message, Exception innerException) : base(message, innerException)
		{
			this.message = message;
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x0600543A RID: 21562 RVA: 0x001BD2F5 File Offset: 0x001BB4F5
		public override string Message
		{
			get
			{
				if (this.message == null)
				{
					this.message = this.BuildMessage();
				}
				return this.message;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x0600543B RID: 21563 RVA: 0x001BD311 File Offset: 0x001BB511
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x0600543C RID: 21564 RVA: 0x001BD319 File Offset: 0x001BB519
		public Type ParameterType
		{
			get
			{
				return this.parameterType;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x0600543D RID: 21565 RVA: 0x001BD321 File Offset: 0x001BB521
		public Type TypeSpecified
		{
			get
			{
				return this.typeSpecified;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x0600543E RID: 21566 RVA: 0x001BD329 File Offset: 0x001BB529
		public string ErrorId
		{
			get
			{
				return this.errorId;
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x0600543F RID: 21567 RVA: 0x001BD331 File Offset: 0x001BB531
		public long Line
		{
			get
			{
				return this.line;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06005440 RID: 21568 RVA: 0x001BD339 File Offset: 0x001BB539
		public long Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x06005441 RID: 21569 RVA: 0x001BD341 File Offset: 0x001BB541
		public InvocationInfo CommandInvocation
		{
			get
			{
				return this.invocationInfo;
			}
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x001BD34C File Offset: 0x001BB54C
		private string BuildMessage()
		{
			object[] array = new object[0];
			if (this.args != null)
			{
				array = new object[this.args.Length + 6];
				array[0] = this.commandName;
				array[1] = this.parameterName;
				array[2] = this.parameterType;
				array[3] = this.typeSpecified;
				array[4] = this.line;
				array[5] = this.offset;
				this.args.CopyTo(array, 6);
			}
			string result = string.Empty;
			if (!string.IsNullOrEmpty(this.resourceString))
			{
				result = StringUtil.Format(this.resourceString, array);
			}
			return result;
		}

		// Token: 0x04002B26 RID: 11046
		private string message;

		// Token: 0x04002B27 RID: 11047
		private string parameterName = string.Empty;

		// Token: 0x04002B28 RID: 11048
		private Type parameterType;

		// Token: 0x04002B29 RID: 11049
		private Type typeSpecified;

		// Token: 0x04002B2A RID: 11050
		private string errorId;

		// Token: 0x04002B2B RID: 11051
		private long line = long.MinValue;

		// Token: 0x04002B2C RID: 11052
		private long offset = long.MinValue;

		// Token: 0x04002B2D RID: 11053
		private InvocationInfo invocationInfo;

		// Token: 0x04002B2E RID: 11054
		private string resourceString;

		// Token: 0x04002B2F RID: 11055
		private object[] args = new object[0];

		// Token: 0x04002B30 RID: 11056
		private string commandName;
	}
}
