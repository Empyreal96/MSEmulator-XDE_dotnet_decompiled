using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020003FF RID: 1023
	public sealed class TypeConfigurationEntry : RunspaceConfigurationEntry
	{
		// Token: 0x06002E2E RID: 11822 RVA: 0x000FEA93 File Offset: 0x000FCC93
		public TypeConfigurationEntry(string name, string fileName) : base(name)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000FEAC8 File Offset: 0x000FCCC8
		public TypeConfigurationEntry(TypeData typeData, bool isRemove) : base("*")
		{
			if (typeData == null)
			{
				throw PSTraceSource.NewArgumentException("typeData");
			}
			this._typeData = typeData;
			this._isRemove = isRemove;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000FEAF1 File Offset: 0x000FCCF1
		internal TypeConfigurationEntry(string name, string fileName, PSSnapInInfo psSnapinInfo) : base(name, psSnapinInfo)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000FEB27 File Offset: 0x000FCD27
		public TypeConfigurationEntry(string fileName) : base(fileName)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName.Trim()))
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000FEB5C File Offset: 0x000FCD5C
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x000FEB64 File Offset: 0x000FCD64
		public TypeData TypeData
		{
			get
			{
				return this._typeData;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000FEB6C File Offset: 0x000FCD6C
		public bool IsRemove
		{
			get
			{
				return this._isRemove;
			}
		}

		// Token: 0x0400184B RID: 6219
		private string _fileName;

		// Token: 0x0400184C RID: 6220
		private TypeData _typeData;

		// Token: 0x0400184D RID: 6221
		private bool _isRemove;
	}
}
