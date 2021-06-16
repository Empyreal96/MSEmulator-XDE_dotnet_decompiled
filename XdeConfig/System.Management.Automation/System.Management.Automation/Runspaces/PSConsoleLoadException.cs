using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000850 RID: 2128
	[Serializable]
	public class PSConsoleLoadException : SystemException, IContainsErrorRecord
	{
		// Token: 0x060051F6 RID: 20982 RVA: 0x001B5B5C File Offset: 0x001B3D5C
		internal PSConsoleLoadException(MshConsoleInfo consoleInfo, Collection<PSSnapInException> exceptions)
		{
			if (!string.IsNullOrEmpty(consoleInfo.Filename))
			{
				this._consoleFileName = consoleInfo.Filename;
			}
			if (exceptions != null)
			{
				this._PSSnapInExceptions = exceptions;
			}
			this.CreateErrorRecord();
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x001B5BAE File Offset: 0x001B3DAE
		public PSConsoleLoadException()
		{
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x001B5BCC File Offset: 0x001B3DCC
		public PSConsoleLoadException(string message) : base(message)
		{
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x001B5BEB File Offset: 0x001B3DEB
		public PSConsoleLoadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x001B5C0C File Offset: 0x001B3E0C
		private void CreateErrorRecord()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.PSSnapInExceptions != null)
			{
				foreach (PSSnapInException ex in this.PSSnapInExceptions)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append(ex.Message);
				}
			}
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "ConsoleLoadFailure", ErrorCategory.ResourceUnavailable, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(typeof(PSConsoleLoadException).GetTypeInfo().Assembly, "ConsoleInfoErrorStrings", "ConsoleLoadFailure", new object[]
			{
				this._consoleFileName,
				stringBuilder.ToString()
			});
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x060051FB RID: 20987 RVA: 0x001B5CDC File Offset: 0x001B3EDC
		public ErrorRecord ErrorRecord
		{
			get
			{
				return this._errorRecord;
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x060051FC RID: 20988 RVA: 0x001B5CE4 File Offset: 0x001B3EE4
		internal Collection<PSSnapInException> PSSnapInExceptions
		{
			get
			{
				return this._PSSnapInExceptions;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x001B5CEC File Offset: 0x001B3EEC
		public override string Message
		{
			get
			{
				if (this._errorRecord != null)
				{
					return this._errorRecord.ToString();
				}
				return base.Message;
			}
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x001B5D08 File Offset: 0x001B3F08
		protected PSConsoleLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._consoleFileName = info.GetString("ConsoleFileName");
			this.CreateErrorRecord();
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x001B5D3F File Offset: 0x001B3F3F
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ConsoleFileName", this._consoleFileName);
		}

		// Token: 0x04002A25 RID: 10789
		private ErrorRecord _errorRecord;

		// Token: 0x04002A26 RID: 10790
		private string _consoleFileName = "";

		// Token: 0x04002A27 RID: 10791
		private Collection<PSSnapInException> _PSSnapInExceptions = new Collection<PSSnapInException>();
	}
}
