using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200082A RID: 2090
	public sealed class SessionStateAssemblyEntry : InitialSessionStateEntry
	{
		// Token: 0x06005010 RID: 20496 RVA: 0x001A7DFE File Offset: 0x001A5FFE
		public SessionStateAssemblyEntry(string name, string fileName) : base(name)
		{
			this._fileName = fileName;
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x001A7E0E File Offset: 0x001A600E
		public SessionStateAssemblyEntry(string name) : base(name)
		{
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001A7E18 File Offset: 0x001A6018
		public override InitialSessionStateEntry Clone()
		{
			SessionStateAssemblyEntry sessionStateAssemblyEntry = new SessionStateAssemblyEntry(base.Name, this._fileName);
			sessionStateAssemblyEntry.SetPSSnapIn(base.PSSnapIn);
			sessionStateAssemblyEntry.SetModule(base.Module);
			return sessionStateAssemblyEntry;
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06005013 RID: 20499 RVA: 0x001A7E50 File Offset: 0x001A6050
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x040028F4 RID: 10484
		private string _fileName;
	}
}
