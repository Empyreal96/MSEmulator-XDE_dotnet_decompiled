using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000529 RID: 1321
	internal sealed class SubPipelineManager : IDisposable
	{
		// Token: 0x06003739 RID: 14137 RVA: 0x0012990F File Offset: 0x00127B0F
		internal void Initialize(LineOutput lineOutput, ExecutionContext context)
		{
			this.lo = lineOutput;
			this.InitializeCommandsHardWired(context);
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x0012991F File Offset: 0x00127B1F
		private void InitializeCommandsHardWired(ExecutionContext context)
		{
			this.RegisterCommandDefault(context, "out-lineoutput", typeof(OutLineOutputCommand));
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x00129938 File Offset: 0x00127B38
		private void RegisterCommandDefault(ExecutionContext context, string commandName, Type commandType)
		{
			SubPipelineManager.CommandEntry commandEntry = new SubPipelineManager.CommandEntry();
			commandEntry.command.Initialize(context, commandName, commandType);
			commandEntry.command.AddNamedParameter("LineOutput", this.lo);
			this.defaultCommandEntry = commandEntry;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x00129978 File Offset: 0x00127B78
		internal void Process(PSObject so)
		{
			SubPipelineManager.CommandEntry activeCommandEntry = this.GetActiveCommandEntry(so);
			activeCommandEntry.command.Process(so);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x0012999C File Offset: 0x00127B9C
		internal void ShutDown()
		{
			foreach (SubPipelineManager.CommandEntry commandEntry in this.commandEntryList)
			{
				commandEntry.command.ShutDown();
				commandEntry.command = null;
			}
			this.defaultCommandEntry.command.ShutDown();
			this.defaultCommandEntry.command = null;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x00129A18 File Offset: 0x00127C18
		public void Dispose()
		{
			foreach (SubPipelineManager.CommandEntry commandEntry in this.commandEntryList)
			{
				commandEntry.Dispose();
			}
			this.defaultCommandEntry.Dispose();
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x00129A78 File Offset: 0x00127C78
		private SubPipelineManager.CommandEntry GetActiveCommandEntry(PSObject so)
		{
			string typeName = PSObjectHelper.PSObjectIsOfExactType(so.InternalTypeNames);
			foreach (SubPipelineManager.CommandEntry commandEntry in this.commandEntryList)
			{
				if (commandEntry.AppliesToType(typeName))
				{
					return commandEntry;
				}
			}
			return this.defaultCommandEntry;
		}

		// Token: 0x04001C44 RID: 7236
		private LineOutput lo;

		// Token: 0x04001C45 RID: 7237
		private List<SubPipelineManager.CommandEntry> commandEntryList = new List<SubPipelineManager.CommandEntry>();

		// Token: 0x04001C46 RID: 7238
		private SubPipelineManager.CommandEntry defaultCommandEntry = new SubPipelineManager.CommandEntry();

		// Token: 0x0200052A RID: 1322
		private sealed class CommandEntry : IDisposable
		{
			// Token: 0x06003741 RID: 14145 RVA: 0x00129B08 File Offset: 0x00127D08
			internal bool AppliesToType(string typeName)
			{
				foreach (string a in this.applicableTypes)
				{
					if (string.Equals(a, typeName, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06003742 RID: 14146 RVA: 0x00129B68 File Offset: 0x00127D68
			public void Dispose()
			{
				if (this.command == null)
				{
					return;
				}
				this.command.Dispose();
				this.command = null;
			}

			// Token: 0x04001C47 RID: 7239
			internal CommandWrapper command = new CommandWrapper();

			// Token: 0x04001C48 RID: 7240
			private StringCollection applicableTypes = new StringCollection();
		}
	}
}
