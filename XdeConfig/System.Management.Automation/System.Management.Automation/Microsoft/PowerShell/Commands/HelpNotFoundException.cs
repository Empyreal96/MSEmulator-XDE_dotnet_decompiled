using System;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001C3 RID: 451
	[Serializable]
	public class HelpNotFoundException : SystemException, IContainsErrorRecord
	{
		// Token: 0x060014F2 RID: 5362 RVA: 0x0008304F File Offset: 0x0008124F
		public HelpNotFoundException(string helpTopic)
		{
			this._helpTopic = helpTopic;
			this.CreateErrorRecord();
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0008306F File Offset: 0x0008126F
		public HelpNotFoundException()
		{
			this.CreateErrorRecord();
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00083088 File Offset: 0x00081288
		public HelpNotFoundException(string helpTopic, Exception innerException) : base((innerException != null) ? innerException.Message : string.Empty, innerException)
		{
			this._helpTopic = helpTopic;
			this.CreateErrorRecord();
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000830BC File Offset: 0x000812BC
		private void CreateErrorRecord()
		{
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "HelpNotFound", ErrorCategory.ResourceUnavailable, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(typeof(HelpNotFoundException).GetTypeInfo().Assembly, "HelpErrors", "HelpNotFound", new object[]
			{
				this._helpTopic
			});
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x00083121 File Offset: 0x00081321
		public ErrorRecord ErrorRecord
		{
			get
			{
				return this._errorRecord;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x00083129 File Offset: 0x00081329
		public string HelpTopic
		{
			get
			{
				return this._helpTopic;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00083131 File Offset: 0x00081331
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

		// Token: 0x060014F9 RID: 5369 RVA: 0x0008314D File Offset: 0x0008134D
		protected HelpNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._helpTopic = info.GetString("HelpTopic");
			this.CreateErrorRecord();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00083179 File Offset: 0x00081379
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("HelpTopic", this._helpTopic);
		}

		// Token: 0x040008F0 RID: 2288
		private ErrorRecord _errorRecord;

		// Token: 0x040008F1 RID: 2289
		private string _helpTopic = "";
	}
}
