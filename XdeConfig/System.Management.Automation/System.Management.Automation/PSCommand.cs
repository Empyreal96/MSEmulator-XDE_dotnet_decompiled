using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200023C RID: 572
	public sealed class PSCommand
	{
		// Token: 0x06001AF0 RID: 6896 RVA: 0x0009FB90 File Offset: 0x0009DD90
		public PSCommand()
		{
			this.Initialize(null, false, null);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0009FBB4 File Offset: 0x0009DDB4
		internal PSCommand(PSCommand commandToClone)
		{
			this.commands = new CommandCollection();
			foreach (Command command in commandToClone.Commands)
			{
				Command item = command.Clone();
				this.commands.Add(item);
				this.currentCommand = item;
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0009FC28 File Offset: 0x0009DE28
		internal PSCommand(Command command)
		{
			this.currentCommand = command;
			this.commands = new CommandCollection();
			this.commands.Add(this.currentCommand);
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0009FC54 File Offset: 0x0009DE54
		public PSCommand AddCommand(string command)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand = new Command(command, false);
			this.commands.Add(this.currentCommand);
			return this;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0009FCA4 File Offset: 0x0009DEA4
		public PSCommand AddCommand(string cmdlet, bool useLocalScope)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand = new Command(cmdlet, false, useLocalScope);
			this.commands.Add(this.currentCommand);
			return this;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0009FCF4 File Offset: 0x0009DEF4
		public PSCommand AddScript(string script)
		{
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand = new Command(script, true);
			this.commands.Add(this.currentCommand);
			return this;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0009FD44 File Offset: 0x0009DF44
		public PSCommand AddScript(string script, bool useLocalScope)
		{
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand = new Command(script, true, useLocalScope);
			this.commands.Add(this.currentCommand);
			return this;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0009FD92 File Offset: 0x0009DF92
		public PSCommand AddCommand(Command command)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand = command;
			this.commands.Add(this.currentCommand);
			return this;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0009FDD0 File Offset: 0x0009DFD0
		public PSCommand AddParameter(string parameterName, object value)
		{
			if (this.currentCommand == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PSCommandStrings.ParameterRequiresCommand, new object[]
				{
					"PSCommand"
				});
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand.Parameters.Add(parameterName, value);
			return this;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0009FE28 File Offset: 0x0009E028
		public PSCommand AddParameter(string parameterName)
		{
			if (this.currentCommand == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PSCommandStrings.ParameterRequiresCommand, new object[]
				{
					"PSCommand"
				});
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand.Parameters.Add(parameterName, true);
			return this;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0009FE84 File Offset: 0x0009E084
		public PSCommand AddArgument(object value)
		{
			if (this.currentCommand == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PSCommandStrings.ParameterRequiresCommand, new object[]
				{
					"PSCommand"
				});
			}
			if (this.owner != null)
			{
				this.owner.AssertChangesAreAccepted();
			}
			this.currentCommand.Parameters.Add(null, value);
			return this;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0009FEDA File Offset: 0x0009E0DA
		public PSCommand AddStatement()
		{
			if (this.commands.Count == 0)
			{
				return this;
			}
			this.commands[this.commands.Count - 1].IsEndOfStatement = true;
			return this;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x0009FF0A File Offset: 0x0009E10A
		public CommandCollection Commands
		{
			get
			{
				return this.commands;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x0009FF12 File Offset: 0x0009E112
		// (set) Token: 0x06001AFE RID: 6910 RVA: 0x0009FF1A File Offset: 0x0009E11A
		internal PowerShell Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0009FF23 File Offset: 0x0009E123
		public void Clear()
		{
			this.commands.Clear();
			this.currentCommand = null;
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0009FF37 File Offset: 0x0009E137
		public PSCommand Clone()
		{
			return new PSCommand(this);
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0009FF3F File Offset: 0x0009E13F
		private void Initialize(string command, bool isScript, bool? useLocalScope)
		{
			this.commands = new CommandCollection();
			if (command != null)
			{
				this.currentCommand = new Command(command, isScript, useLocalScope);
				this.commands.Add(this.currentCommand);
			}
		}

		// Token: 0x04000B19 RID: 2841
		private PowerShell owner;

		// Token: 0x04000B1A RID: 2842
		private CommandCollection commands;

		// Token: 0x04000B1B RID: 2843
		private Command currentCommand;
	}
}
