using System;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000403 RID: 1027
	public sealed class ScriptConfigurationEntry : RunspaceConfigurationEntry
	{
		// Token: 0x06002E43 RID: 11843 RVA: 0x000FED85 File Offset: 0x000FCF85
		public ScriptConfigurationEntry(string name, string definition) : base(name)
		{
			if (string.IsNullOrEmpty(definition) || string.IsNullOrEmpty(definition.Trim()))
			{
				throw PSTraceSource.NewArgumentNullException("definition");
			}
			this._definition = definition.Trim();
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06002E44 RID: 11844 RVA: 0x000FEDBA File Offset: 0x000FCFBA
		public string Definition
		{
			get
			{
				return this._definition;
			}
		}

		// Token: 0x04001854 RID: 6228
		private string _definition;
	}
}
