using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Help
{
	// Token: 0x020001DB RID: 475
	internal class UpdatableHelpExceptionContext
	{
		// Token: 0x060015D8 RID: 5592 RVA: 0x0008A5DC File Offset: 0x000887DC
		internal UpdatableHelpExceptionContext(UpdatableHelpSystemException exception)
		{
			this._exception = exception;
			this._modules = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			this._cultures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x0008A60B File Offset: 0x0008880B
		// (set) Token: 0x060015DA RID: 5594 RVA: 0x0008A613 File Offset: 0x00088813
		internal HashSet<string> Modules
		{
			get
			{
				return this._modules;
			}
			set
			{
				this._modules = value;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x0008A61C File Offset: 0x0008881C
		// (set) Token: 0x060015DC RID: 5596 RVA: 0x0008A624 File Offset: 0x00088824
		internal HashSet<string> Cultures
		{
			get
			{
				return this._cultures;
			}
			set
			{
				this._cultures = value;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x0008A62D File Offset: 0x0008882D
		internal UpdatableHelpSystemException Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0008A635 File Offset: 0x00088835
		internal ErrorRecord CreateErrorRecord(UpdatableHelpCommandType commandType)
		{
			return new ErrorRecord(new Exception(this.GetExceptionMessage(commandType)), this._exception.FullyQualifiedErrorId, this._exception.ErrorCategory, this._exception.TargetObject);
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0008A66C File Offset: 0x0008886C
		internal string GetExceptionMessage(UpdatableHelpCommandType commandType)
		{
			SortedSet<string> values = new SortedSet<string>(this._modules, StringComparer.CurrentCultureIgnoreCase);
			SortedSet<string> values2 = new SortedSet<string>(this._cultures, StringComparer.CurrentCultureIgnoreCase);
			string text = string.Join(", ", values);
			string text2 = string.Join(", ", values2);
			string result;
			if (commandType == UpdatableHelpCommandType.UpdateHelpCommand)
			{
				if (this._cultures.Count == 0)
				{
					result = StringUtil.Format(HelpDisplayStrings.FailedToUpdateHelpForModule, text, this._exception.Message);
				}
				else
				{
					result = StringUtil.Format(HelpDisplayStrings.FailedToUpdateHelpForModuleWithCulture, new object[]
					{
						text,
						text2,
						this._exception.Message
					});
				}
			}
			else if (this._cultures.Count == 0)
			{
				result = StringUtil.Format(HelpDisplayStrings.FailedToSaveHelpForModule, text, this._exception.Message);
			}
			else
			{
				result = StringUtil.Format(HelpDisplayStrings.FailedToSaveHelpForModuleWithCulture, new object[]
				{
					text,
					text2,
					this._exception.Message
				});
			}
			return result;
		}

		// Token: 0x04000944 RID: 2372
		private UpdatableHelpSystemException _exception;

		// Token: 0x04000945 RID: 2373
		private HashSet<string> _modules;

		// Token: 0x04000946 RID: 2374
		private HashSet<string> _cultures;
	}
}
