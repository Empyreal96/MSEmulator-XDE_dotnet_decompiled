using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000828 RID: 2088
	public sealed class SessionStateTypeEntry : InitialSessionStateEntry
	{
		// Token: 0x06005001 RID: 20481 RVA: 0x001A7C03 File Offset: 0x001A5E03
		public SessionStateTypeEntry(string fileName) : base(fileName)
		{
			if (string.IsNullOrEmpty(fileName) || fileName.Trim().Length == 0)
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x001A7C38 File Offset: 0x001A5E38
		public SessionStateTypeEntry(TypeTable typeTable) : base("*")
		{
			if (typeTable == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeTable");
			}
			this._typeTable = typeTable;
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x001A7C5A File Offset: 0x001A5E5A
		public SessionStateTypeEntry(TypeData typeData, bool isRemove) : base("*")
		{
			if (typeData == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeData");
			}
			this._typeData = typeData;
			this._isRemove = isRemove;
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x001A7C84 File Offset: 0x001A5E84
		public override InitialSessionStateEntry Clone()
		{
			SessionStateTypeEntry sessionStateTypeEntry;
			if (this._fileName != null)
			{
				sessionStateTypeEntry = new SessionStateTypeEntry(this._fileName);
			}
			else if (this._typeTable != null)
			{
				sessionStateTypeEntry = new SessionStateTypeEntry(this._typeTable);
			}
			else
			{
				sessionStateTypeEntry = new SessionStateTypeEntry(this._typeData, this._isRemove);
			}
			sessionStateTypeEntry.SetPSSnapIn(base.PSSnapIn);
			sessionStateTypeEntry.SetModule(base.Module);
			return sessionStateTypeEntry;
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06005005 RID: 20485 RVA: 0x001A7CE8 File Offset: 0x001A5EE8
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x001A7CF0 File Offset: 0x001A5EF0
		public TypeTable TypeTable
		{
			get
			{
				return this._typeTable;
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06005007 RID: 20487 RVA: 0x001A7CF8 File Offset: 0x001A5EF8
		public TypeData TypeData
		{
			get
			{
				return this._typeData;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06005008 RID: 20488 RVA: 0x001A7D00 File Offset: 0x001A5F00
		public bool IsRemove
		{
			get
			{
				return this._isRemove;
			}
		}

		// Token: 0x040028ED RID: 10477
		private string _fileName;

		// Token: 0x040028EE RID: 10478
		private TypeTable _typeTable;

		// Token: 0x040028EF RID: 10479
		private TypeData _typeData;

		// Token: 0x040028F0 RID: 10480
		private bool _isRemove;
	}
}
