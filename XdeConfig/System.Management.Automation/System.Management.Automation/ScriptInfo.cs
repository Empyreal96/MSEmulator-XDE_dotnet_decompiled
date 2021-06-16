using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000082 RID: 130
	public class ScriptInfo : CommandInfo, IScriptCommandInfo
	{
		// Token: 0x060006AA RID: 1706 RVA: 0x00020422 File Offset: 0x0001E622
		internal ScriptInfo(string name, ScriptBlock script, ExecutionContext context) : base(name, CommandTypes.Script, context)
		{
			if (script == null)
			{
				throw PSTraceSource.NewArgumentException("script");
			}
			this.ScriptBlock = script;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00020443 File Offset: 0x0001E643
		internal ScriptInfo(ScriptInfo other) : base(other)
		{
			this.ScriptBlock = other.ScriptBlock;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00020458 File Offset: 0x0001E658
		internal override CommandInfo CreateGetCommandCopy(object[] argumentList)
		{
			return new ScriptInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = argumentList
			};
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0002047D File Offset: 0x0001E67D
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.ScriptCommand;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00020484 File Offset: 0x0001E684
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x0002048C File Offset: 0x0001E68C
		public ScriptBlock ScriptBlock { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00020495 File Offset: 0x0001E695
		public override string Definition
		{
			get
			{
				return this.ScriptBlock.ToString();
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x000204A2 File Offset: 0x0001E6A2
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				return this.ScriptBlock.OutputType;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000204AF File Offset: 0x0001E6AF
		public override string ToString()
		{
			return this.ScriptBlock.ToString();
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x000204BC File Offset: 0x0001E6BC
		internal override bool ImplementsDynamicParameters
		{
			get
			{
				return this.ScriptBlock.HasDynamicParameters;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000204CC File Offset: 0x0001E6CC
		internal override CommandMetadata CommandMetadata
		{
			get
			{
				CommandMetadata result;
				if ((result = this._commandMetadata) == null)
				{
					result = (this._commandMetadata = new CommandMetadata(this.ScriptBlock, base.Name, LocalPipeline.GetExecutionContextFromTLS()));
				}
				return result;
			}
		}

		// Token: 0x040002BF RID: 703
		private CommandMetadata _commandMetadata;
	}
}
