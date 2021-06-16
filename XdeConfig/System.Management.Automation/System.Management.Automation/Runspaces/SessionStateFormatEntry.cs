using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000829 RID: 2089
	public sealed class SessionStateFormatEntry : InitialSessionStateEntry
	{
		// Token: 0x06005009 RID: 20489 RVA: 0x001A7D08 File Offset: 0x001A5F08
		public SessionStateFormatEntry(string fileName) : base("*")
		{
			if (string.IsNullOrEmpty(fileName) || fileName.Trim().Length == 0)
			{
				throw PSTraceSource.NewArgumentException("fileName");
			}
			this._fileName = fileName.Trim();
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x001A7D41 File Offset: 0x001A5F41
		public SessionStateFormatEntry(FormatTable formattable) : base("*")
		{
			if (formattable == null)
			{
				throw PSTraceSource.NewArgumentNullException("formattable");
			}
			this._formattable = formattable;
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x001A7D63 File Offset: 0x001A5F63
		public SessionStateFormatEntry(ExtendedTypeDefinition typeDefinition) : base("*")
		{
			if (typeDefinition == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeDefinition");
			}
			this._typeDefinition = typeDefinition;
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x001A7D88 File Offset: 0x001A5F88
		public override InitialSessionStateEntry Clone()
		{
			SessionStateFormatEntry sessionStateFormatEntry;
			if (this._fileName != null)
			{
				sessionStateFormatEntry = new SessionStateFormatEntry(this._fileName);
			}
			else if (this._formattable != null)
			{
				sessionStateFormatEntry = new SessionStateFormatEntry(this._formattable);
			}
			else
			{
				sessionStateFormatEntry = new SessionStateFormatEntry(this._typeDefinition);
			}
			sessionStateFormatEntry.SetPSSnapIn(base.PSSnapIn);
			sessionStateFormatEntry.SetModule(base.Module);
			return sessionStateFormatEntry;
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x0600500D RID: 20493 RVA: 0x001A7DE6 File Offset: 0x001A5FE6
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x0600500E RID: 20494 RVA: 0x001A7DEE File Offset: 0x001A5FEE
		public FormatTable Formattable
		{
			get
			{
				return this._formattable;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x0600500F RID: 20495 RVA: 0x001A7DF6 File Offset: 0x001A5FF6
		public ExtendedTypeDefinition FormatData
		{
			get
			{
				return this._typeDefinition;
			}
		}

		// Token: 0x040028F1 RID: 10481
		private string _fileName;

		// Token: 0x040028F2 RID: 10482
		private FormatTable _formattable;

		// Token: 0x040028F3 RID: 10483
		private ExtendedTypeDefinition _typeDefinition;
	}
}
