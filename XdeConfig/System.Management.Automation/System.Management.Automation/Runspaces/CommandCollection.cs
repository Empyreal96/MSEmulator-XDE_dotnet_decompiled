using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001ED RID: 493
	public sealed class CommandCollection : Collection<Command>
	{
		// Token: 0x06001685 RID: 5765 RVA: 0x000900C8 File Offset: 0x0008E2C8
		internal CommandCollection()
		{
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000900D0 File Offset: 0x0008E2D0
		public void Add(string command)
		{
			if (string.Equals(command, "out-default", StringComparison.OrdinalIgnoreCase))
			{
				this.Add(command, true);
				return;
			}
			base.Add(new Command(command));
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000900F5 File Offset: 0x0008E2F5
		internal void Add(string command, bool mergeUnclaimedPreviousCommandError)
		{
			base.Add(new Command(command, false, new bool?(false), mergeUnclaimedPreviousCommandError));
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0009010B File Offset: 0x0008E30B
		public void AddScript(string scriptContents)
		{
			base.Add(new Command(scriptContents, true));
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x0009011A File Offset: 0x0008E31A
		public void AddScript(string scriptContents, bool useLocalScope)
		{
			base.Add(new Command(scriptContents, true, useLocalScope));
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x0009012C File Offset: 0x0008E32C
		internal string GetCommandStringForHistory()
		{
			Command command = base[0];
			return command.CommandText;
		}
	}
}
