using System;

namespace System.Management.Automation
{
	// Token: 0x0200000F RID: 15
	internal class CommandFactory
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004160 File Offset: 0x00002360
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004168 File Offset: 0x00002368
		internal ExecutionContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004171 File Offset: 0x00002371
		internal CommandFactory()
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004179 File Offset: 0x00002379
		internal CommandFactory(ExecutionContext context)
		{
			this.Context = context;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004188 File Offset: 0x00002388
		internal CommandProcessorBase CreateCommand(string commandName, CommandOrigin commandOrigin)
		{
			return this._CreateCommand(commandName, commandOrigin, new bool?(false));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004198 File Offset: 0x00002398
		internal CommandProcessorBase CreateCommand(string commandName, CommandOrigin commandOrigin, bool? useLocalScope)
		{
			return this._CreateCommand(commandName, commandOrigin, useLocalScope);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000041A3 File Offset: 0x000023A3
		internal CommandProcessorBase CreateCommand(string commandName, ExecutionContext executionContext, CommandOrigin commandOrigin)
		{
			this.Context = executionContext;
			return this._CreateCommand(commandName, commandOrigin, new bool?(false));
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000041BC File Offset: 0x000023BC
		private CommandProcessorBase _CreateCommand(string commandName, CommandOrigin commandOrigin, bool? useLocalScope)
		{
			if (this.context == null)
			{
				throw PSTraceSource.NewInvalidOperationException(DiscoveryExceptions.ExecutionContextNotSet, new object[0]);
			}
			CommandDiscovery commandDiscovery = this.context.CommandDiscovery;
			if (commandDiscovery == null)
			{
				throw PSTraceSource.NewInvalidOperationException(DiscoveryExceptions.CommandDiscoveryMissing, new object[0]);
			}
			return commandDiscovery.LookupCommandProcessor(commandName, commandOrigin, useLocalScope);
		}

		// Token: 0x04000038 RID: 56
		private ExecutionContext context;
	}
}
