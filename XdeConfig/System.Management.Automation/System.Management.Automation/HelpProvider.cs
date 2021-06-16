using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x020001B4 RID: 436
	internal abstract class HelpProvider
	{
		// Token: 0x0600143F RID: 5183 RVA: 0x0007C357 File Offset: 0x0007A557
		internal HelpProvider(HelpSystem helpSystem)
		{
			this._helpSystem = helpSystem;
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0007C366 File Offset: 0x0007A566
		internal HelpSystem HelpSystem
		{
			get
			{
				return this._helpSystem;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001441 RID: 5185
		internal abstract string Name { get; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001442 RID: 5186
		internal abstract HelpCategory HelpCategory { get; }

		// Token: 0x06001443 RID: 5187
		internal abstract IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest);

		// Token: 0x06001444 RID: 5188
		internal abstract IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent);

		// Token: 0x06001445 RID: 5189 RVA: 0x0007C468 File Offset: 0x0007A668
		internal virtual IEnumerable<HelpInfo> ProcessForwardedHelp(HelpInfo helpInfo, HelpRequest helpRequest)
		{
			helpInfo.ForwardHelpCategory ^= this.HelpCategory;
			yield return helpInfo;
			yield break;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0007C48C File Offset: 0x0007A68C
		internal virtual void Reset()
		{
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0007C490 File Offset: 0x0007A690
		internal void ReportHelpFileError(Exception exception, string target, string helpFile)
		{
			ErrorRecord errorRecord = new ErrorRecord(exception, "LoadHelpFileForTargetFailed", ErrorCategory.OpenError, null);
			errorRecord.ErrorDetails = new ErrorDetails(typeof(HelpProvider).GetTypeInfo().Assembly, "HelpErrors", "LoadHelpFileForTargetFailed", new object[]
			{
				target,
				helpFile,
				exception.Message
			});
			this.HelpSystem.LastErrors.Add(errorRecord);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0007C500 File Offset: 0x0007A700
		internal string GetDefaultShellSearchPath()
		{
			string shellID = this.HelpSystem.ExecutionContext.ShellID;
			string text = CommandDiscovery.GetShellPathFromRegistry(shellID);
			if (text == null)
			{
				text = Path.GetDirectoryName(PsUtils.GetMainModule(Process.GetCurrentProcess()).FileName);
			}
			else
			{
				text = Path.GetDirectoryName(text);
				if (!Directory.Exists(text))
				{
					text = Path.GetDirectoryName(PsUtils.GetMainModule(Process.GetCurrentProcess()).FileName);
				}
			}
			return text;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0007C564 File Offset: 0x0007A764
		internal bool AreSnapInsSupported()
		{
			return this._helpSystem.ExecutionContext.RunspaceConfiguration is RunspaceConfigForSingleShell;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0007C590 File Offset: 0x0007A790
		internal Collection<string> GetSearchPaths()
		{
			Collection<string> searchPaths = this.HelpSystem.GetSearchPaths();
			searchPaths.Add(this.GetDefaultShellSearchPath());
			return searchPaths;
		}

		// Token: 0x040008CE RID: 2254
		private HelpSystem _helpSystem;
	}
}
