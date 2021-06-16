using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000409 RID: 1033
	[Serializable]
	public class RunspaceConfigurationAttributeException : SystemException, IContainsErrorRecord
	{
		// Token: 0x06002E66 RID: 11878 RVA: 0x000FF794 File Offset: 0x000FD994
		internal RunspaceConfigurationAttributeException(string error, string assemblyName)
		{
			this._error = error;
			this._assemblyName = assemblyName;
			this.CreateErrorRecord();
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000FF7C6 File Offset: 0x000FD9C6
		public RunspaceConfigurationAttributeException()
		{
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000FF7E4 File Offset: 0x000FD9E4
		public RunspaceConfigurationAttributeException(string message) : base(message)
		{
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000FF803 File Offset: 0x000FDA03
		public RunspaceConfigurationAttributeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000FF823 File Offset: 0x000FDA23
		internal RunspaceConfigurationAttributeException(string error, string assemblyName, Exception innerException) : base(innerException.Message, innerException)
		{
			this._error = error;
			this._assemblyName = assemblyName;
			this.CreateErrorRecord();
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000FF85C File Offset: 0x000FDA5C
		private void CreateErrorRecord()
		{
			if (!string.IsNullOrEmpty(this._error) && !string.IsNullOrEmpty(this._assemblyName))
			{
				this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._error, ErrorCategory.ResourceUnavailable, null);
				this._errorRecord.ErrorDetails = new ErrorDetails(typeof(RunspaceConfigurationAttributeException).GetTypeInfo().Assembly, "MiniShellErrors", this._error, new object[]
				{
					this._assemblyName
				});
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06002E6C RID: 11884 RVA: 0x000FF8DD File Offset: 0x000FDADD
		public ErrorRecord ErrorRecord
		{
			get
			{
				return this._errorRecord;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06002E6D RID: 11885 RVA: 0x000FF8E5 File Offset: 0x000FDAE5
		public string Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06002E6E RID: 11886 RVA: 0x000FF8ED File Offset: 0x000FDAED
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06002E6F RID: 11887 RVA: 0x000FF8F5 File Offset: 0x000FDAF5
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

		// Token: 0x06002E70 RID: 11888 RVA: 0x000FF914 File Offset: 0x000FDB14
		protected RunspaceConfigurationAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._error = info.GetString("Error");
			this._assemblyName = info.GetString("AssemblyName");
			this.CreateErrorRecord();
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000FF967 File Offset: 0x000FDB67
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Error", this._error);
			info.AddValue("AssemblyName", this._assemblyName);
		}

		// Token: 0x04001860 RID: 6240
		private ErrorRecord _errorRecord;

		// Token: 0x04001861 RID: 6241
		private string _error = "";

		// Token: 0x04001862 RID: 6242
		private string _assemblyName = "";
	}
}
