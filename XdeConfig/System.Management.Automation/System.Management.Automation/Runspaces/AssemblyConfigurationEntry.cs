using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000404 RID: 1028
	public sealed class AssemblyConfigurationEntry : RunspaceConfigurationEntry
	{
		// Token: 0x06002E45 RID: 11845 RVA: 0x000FEDC2 File Offset: 0x000FCFC2
		public AssemblyConfigurationEntry(string name, string fileName) : base(name)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentNullException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000FEDF7 File Offset: 0x000FCFF7
		internal AssemblyConfigurationEntry(string name, string fileName, PSSnapInInfo psSnapinInfo) : base(name, psSnapinInfo)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentNullException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x000FEE2D File Offset: 0x000FD02D
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x04001855 RID: 6229
		private string _fileName;
	}
}
