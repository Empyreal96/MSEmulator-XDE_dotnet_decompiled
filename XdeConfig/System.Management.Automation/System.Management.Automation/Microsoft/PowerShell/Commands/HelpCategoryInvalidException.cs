using System;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001C2 RID: 450
	[Serializable]
	public class HelpCategoryInvalidException : ArgumentException, IContainsErrorRecord
	{
		// Token: 0x060014E9 RID: 5353 RVA: 0x00082EE5 File Offset: 0x000810E5
		public HelpCategoryInvalidException(string helpCategory)
		{
			this._helpCategory = helpCategory;
			this.CreateErrorRecord();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00082F0B File Offset: 0x0008110B
		public HelpCategoryInvalidException()
		{
			this.CreateErrorRecord();
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00082F2A File Offset: 0x0008112A
		public HelpCategoryInvalidException(string helpCategory, Exception innerException) : base((innerException != null) ? innerException.Message : string.Empty, innerException)
		{
			this._helpCategory = helpCategory;
			this.CreateErrorRecord();
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00082F64 File Offset: 0x00081164
		private void CreateErrorRecord()
		{
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "HelpCategoryInvalid", ErrorCategory.InvalidArgument, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(typeof(HelpCategoryInvalidException).GetTypeInfo().Assembly, "HelpErrors", "HelpCategoryInvalid", new object[]
			{
				this._helpCategory
			});
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x00082FC8 File Offset: 0x000811C8
		public ErrorRecord ErrorRecord
		{
			get
			{
				return this._errorRecord;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x00082FD0 File Offset: 0x000811D0
		public string HelpCategory
		{
			get
			{
				return this._helpCategory;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x00082FD8 File Offset: 0x000811D8
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

		// Token: 0x060014F0 RID: 5360 RVA: 0x00082FF4 File Offset: 0x000811F4
		protected HelpCategoryInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._helpCategory = info.GetString("HelpCategory");
			this.CreateErrorRecord();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x00083026 File Offset: 0x00081226
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("HelpCategory", this._helpCategory);
		}

		// Token: 0x040008EE RID: 2286
		private ErrorRecord _errorRecord;

		// Token: 0x040008EF RID: 2287
		private string _helpCategory = System.Management.Automation.HelpCategory.None.ToString();
	}
}
