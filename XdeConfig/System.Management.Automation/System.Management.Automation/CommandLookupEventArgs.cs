using System;

namespace System.Management.Automation
{
	// Token: 0x02000053 RID: 83
	public class CommandLookupEventArgs : EventArgs
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x000144E5 File Offset: 0x000126E5
		internal CommandLookupEventArgs(string commandName, CommandOrigin commandOrigin, ExecutionContext context)
		{
			this.commandName = commandName;
			this.commandOrigin = commandOrigin;
			this.context = context;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00014502 File Offset: 0x00012702
		public string CommandName
		{
			get
			{
				return this.commandName;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001450A File Offset: 0x0001270A
		public CommandOrigin CommandOrigin
		{
			get
			{
				return this.commandOrigin;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00014512 File Offset: 0x00012712
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x0001451A File Offset: 0x0001271A
		public bool StopSearch { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00014523 File Offset: 0x00012723
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x0001452B File Offset: 0x0001272B
		public CommandInfo Command { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00014534 File Offset: 0x00012734
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x0001453C File Offset: 0x0001273C
		public ScriptBlock CommandScriptBlock
		{
			get
			{
				return this.scriptBlock;
			}
			set
			{
				this.scriptBlock = value;
				if (this.scriptBlock != null)
				{
					string name = "LookupHandlerReplacementFor<<" + this.commandName + ">>";
					this.Command = new FunctionInfo(name, this.scriptBlock, this.context);
					this.StopSearch = true;
					return;
				}
				this.Command = null;
				this.StopSearch = false;
			}
		}

		// Token: 0x040001AC RID: 428
		private ExecutionContext context;

		// Token: 0x040001AD RID: 429
		private string commandName;

		// Token: 0x040001AE RID: 430
		private CommandOrigin commandOrigin;

		// Token: 0x040001AF RID: 431
		private ScriptBlock scriptBlock;
	}
}
