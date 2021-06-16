using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000400 RID: 1024
	public sealed class FormatConfigurationEntry : RunspaceConfigurationEntry
	{
		// Token: 0x06002E35 RID: 11829 RVA: 0x000FEB74 File Offset: 0x000FCD74
		public FormatConfigurationEntry(string name, string fileName) : base(name)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000FEBA9 File Offset: 0x000FCDA9
		internal FormatConfigurationEntry(string name, string fileName, PSSnapInInfo psSnapinInfo) : base(name, psSnapinInfo)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000FEBDF File Offset: 0x000FCDDF
		public FormatConfigurationEntry(string fileName) : base(fileName)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000FEC14 File Offset: 0x000FCE14
		public FormatConfigurationEntry(ExtendedTypeDefinition typeDefinition) : base("*")
		{
			if (typeDefinition == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeDefinition");
			}
			this._typeDefinition = typeDefinition;
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06002E39 RID: 11833 RVA: 0x000FEC36 File Offset: 0x000FCE36
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06002E3A RID: 11834 RVA: 0x000FEC3E File Offset: 0x000FCE3E
		public ExtendedTypeDefinition FormatData
		{
			get
			{
				return this._typeDefinition;
			}
		}

		// Token: 0x0400184E RID: 6222
		private string _fileName;

		// Token: 0x0400184F RID: 6223
		private ExtendedTypeDefinition _typeDefinition;
	}
}
