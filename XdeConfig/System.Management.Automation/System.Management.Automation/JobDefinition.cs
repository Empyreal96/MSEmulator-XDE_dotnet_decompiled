using System;
using System.IO;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000281 RID: 641
	[Serializable]
	public class JobDefinition : ISerializable
	{
		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x000B0938 File Offset: 0x000AEB38
		// (set) Token: 0x06001E69 RID: 7785 RVA: 0x000B0940 File Offset: 0x000AEB40
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x000B0949 File Offset: 0x000AEB49
		public Type JobSourceAdapterType
		{
			get
			{
				return this._jobSourceAdapterType;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x000B0951 File Offset: 0x000AEB51
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x000B0959 File Offset: 0x000AEB59
		public string ModuleName
		{
			get
			{
				return this._moduleName;
			}
			set
			{
				this._moduleName = value;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000B0962 File Offset: 0x000AEB62
		// (set) Token: 0x06001E6E RID: 7790 RVA: 0x000B096A File Offset: 0x000AEB6A
		public string JobSourceAdapterTypeName
		{
			get
			{
				return this._jobSourceAdapterTypeName;
			}
			set
			{
				this._jobSourceAdapterTypeName = value;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x000B0973 File Offset: 0x000AEB73
		public string Command
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x000B097B File Offset: 0x000AEB7B
		// (set) Token: 0x06001E71 RID: 7793 RVA: 0x000B0983 File Offset: 0x000AEB83
		public Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
			set
			{
				this._instanceId = value;
			}
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000B098C File Offset: 0x000AEB8C
		public virtual void Save(Stream stream)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000B0993 File Offset: 0x000AEB93
		public virtual void Load(Stream stream)
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000B099A File Offset: 0x000AEB9A
		public CommandInfo CommandInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000B099D File Offset: 0x000AEB9D
		public JobDefinition(Type jobSourceAdapterType, string command, string name)
		{
			this._jobSourceAdapterType = jobSourceAdapterType;
			if (jobSourceAdapterType != null)
			{
				this._jobSourceAdapterTypeName = jobSourceAdapterType.Name;
			}
			this._command = command;
			this._name = name;
			this._instanceId = Guid.NewGuid();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000B09DA File Offset: 0x000AEBDA
		protected JobDefinition(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000B09E7 File Offset: 0x000AEBE7
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D6A RID: 3434
		private string _name;

		// Token: 0x04000D6B RID: 3435
		private readonly Type _jobSourceAdapterType;

		// Token: 0x04000D6C RID: 3436
		private string _moduleName;

		// Token: 0x04000D6D RID: 3437
		private string _jobSourceAdapterTypeName;

		// Token: 0x04000D6E RID: 3438
		private readonly string _command;

		// Token: 0x04000D6F RID: 3439
		private Guid _instanceId;
	}
}
