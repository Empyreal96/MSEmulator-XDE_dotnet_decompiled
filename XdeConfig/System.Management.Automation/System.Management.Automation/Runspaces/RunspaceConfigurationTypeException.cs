using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200040A RID: 1034
	[Serializable]
	public class RunspaceConfigurationTypeException : SystemException, IContainsErrorRecord
	{
		// Token: 0x06002E72 RID: 11890 RVA: 0x000FF9A1 File Offset: 0x000FDBA1
		internal RunspaceConfigurationTypeException(string assemblyName, string typeName)
		{
			this._assemblyName = assemblyName;
			this._typeName = typeName;
			this.CreateErrorRecord();
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000FF9D3 File Offset: 0x000FDBD3
		public RunspaceConfigurationTypeException()
		{
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000FF9F1 File Offset: 0x000FDBF1
		internal RunspaceConfigurationTypeException(string assemblyName, string typeName, Exception innerException) : base(innerException.Message, innerException)
		{
			this._assemblyName = assemblyName;
			this._typeName = typeName;
			this.CreateErrorRecord();
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000FFA2A File Offset: 0x000FDC2A
		public RunspaceConfigurationTypeException(string message) : base(message)
		{
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000FFA49 File Offset: 0x000FDC49
		public RunspaceConfigurationTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000FFA6C File Offset: 0x000FDC6C
		private void CreateErrorRecord()
		{
			if (!string.IsNullOrEmpty(this._assemblyName) && !string.IsNullOrEmpty(this._typeName))
			{
				this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "UndefinedRunspaceConfigurationType", ErrorCategory.ResourceUnavailable, null);
				this._errorRecord.ErrorDetails = new ErrorDetails(typeof(RunspaceConfigurationTypeException).GetTypeInfo().Assembly, "MiniShellErrors", "UndefinedRunspaceConfigurationType", new object[]
				{
					this._assemblyName,
					this._typeName
				});
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06002E78 RID: 11896 RVA: 0x000FFAF4 File Offset: 0x000FDCF4
		public ErrorRecord ErrorRecord
		{
			get
			{
				return this._errorRecord;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06002E79 RID: 11897 RVA: 0x000FFAFC File Offset: 0x000FDCFC
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06002E7A RID: 11898 RVA: 0x000FFB04 File Offset: 0x000FDD04
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06002E7B RID: 11899 RVA: 0x000FFB0C File Offset: 0x000FDD0C
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

		// Token: 0x06002E7C RID: 11900 RVA: 0x000FFB28 File Offset: 0x000FDD28
		protected RunspaceConfigurationTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._typeName = info.GetString("TypeName");
			this._assemblyName = info.GetString("AssemblyName");
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000FFB75 File Offset: 0x000FDD75
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("TypeName", this._typeName);
			info.AddValue("AssemblyName", this._assemblyName);
		}

		// Token: 0x04001863 RID: 6243
		private ErrorRecord _errorRecord;

		// Token: 0x04001864 RID: 6244
		private string _assemblyName = "";

		// Token: 0x04001865 RID: 6245
		private string _typeName = "";
	}
}
