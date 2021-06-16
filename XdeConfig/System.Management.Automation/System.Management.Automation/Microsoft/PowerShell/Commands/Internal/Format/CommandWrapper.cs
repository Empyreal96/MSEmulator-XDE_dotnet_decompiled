using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200052C RID: 1324
	internal sealed class CommandWrapper : IDisposable
	{
		// Token: 0x06003746 RID: 14150 RVA: 0x00129BCE File Offset: 0x00127DCE
		internal void Initialize(ExecutionContext execContext, string nameOfCommand, Type typeOfCommand)
		{
			this.context = execContext;
			this.commandName = nameOfCommand;
			this.commandType = typeOfCommand;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x00129BE5 File Offset: 0x00127DE5
		internal void AddNamedParameter(string parameterName, object parameterValue)
		{
			this.commandParameterList.Add(CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, parameterName, null, PositionUtilities.EmptyExtent, parameterValue, false, false));
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x00129C06 File Offset: 0x00127E06
		internal Array Process(object o)
		{
			if (this.pp == null)
			{
				this.DelayedInternalInitialize();
			}
			return this.pp.Step(o);
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x00129C24 File Offset: 0x00127E24
		internal Array ShutDown()
		{
			if (this.pp == null)
			{
				return new object[0];
			}
			PipelineProcessor pipelineProcessor = this.pp;
			this.pp = null;
			return pipelineProcessor.Execute();
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x00129C54 File Offset: 0x00127E54
		private void DelayedInternalInitialize()
		{
			this.pp = new PipelineProcessor();
			CmdletInfo cmdletInfo = new CmdletInfo(this.commandName, this.commandType, null, null, this.context);
			CommandProcessor commandProcessor = new CommandProcessor(cmdletInfo, this.context);
			foreach (CommandParameterInternal parameter in this.commandParameterList)
			{
				commandProcessor.AddParameter(parameter);
			}
			this.pp.Add(commandProcessor);
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x00129CE8 File Offset: 0x00127EE8
		public void Dispose()
		{
			if (this.pp == null)
			{
				return;
			}
			this.pp.Dispose();
			this.pp = null;
		}

		// Token: 0x04001C4A RID: 7242
		private PipelineProcessor pp;

		// Token: 0x04001C4B RID: 7243
		private string commandName;

		// Token: 0x04001C4C RID: 7244
		private Type commandType;

		// Token: 0x04001C4D RID: 7245
		private List<CommandParameterInternal> commandParameterList = new List<CommandParameterInternal>();

		// Token: 0x04001C4E RID: 7246
		private ExecutionContext context;
	}
}
