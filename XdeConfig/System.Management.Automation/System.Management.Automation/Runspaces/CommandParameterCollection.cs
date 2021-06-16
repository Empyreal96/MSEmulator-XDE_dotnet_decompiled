using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200022A RID: 554
	public sealed class CommandParameterCollection : Collection<CommandParameter>
	{
		// Token: 0x06001A0A RID: 6666 RVA: 0x0009B5BC File Offset: 0x000997BC
		public void Add(string name)
		{
			base.Add(new CommandParameter(name));
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0009B5CA File Offset: 0x000997CA
		public void Add(string name, object value)
		{
			base.Add(new CommandParameter(name, value));
		}
	}
}
