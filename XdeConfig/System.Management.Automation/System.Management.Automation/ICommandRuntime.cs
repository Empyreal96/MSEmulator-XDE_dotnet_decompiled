using System;
using System.Management.Automation.Host;

namespace System.Management.Automation
{
	// Token: 0x02000043 RID: 67
	public interface ICommandRuntime
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600032F RID: 815
		PSHost Host { get; }

		// Token: 0x06000330 RID: 816
		void WriteDebug(string text);

		// Token: 0x06000331 RID: 817
		void WriteError(ErrorRecord errorRecord);

		// Token: 0x06000332 RID: 818
		void WriteObject(object sendToPipeline);

		// Token: 0x06000333 RID: 819
		void WriteObject(object sendToPipeline, bool enumerateCollection);

		// Token: 0x06000334 RID: 820
		void WriteProgress(ProgressRecord progressRecord);

		// Token: 0x06000335 RID: 821
		void WriteProgress(long sourceId, ProgressRecord progressRecord);

		// Token: 0x06000336 RID: 822
		void WriteVerbose(string text);

		// Token: 0x06000337 RID: 823
		void WriteWarning(string text);

		// Token: 0x06000338 RID: 824
		void WriteCommandDetail(string text);

		// Token: 0x06000339 RID: 825
		bool ShouldProcess(string target);

		// Token: 0x0600033A RID: 826
		bool ShouldProcess(string target, string action);

		// Token: 0x0600033B RID: 827
		bool ShouldProcess(string verboseDescription, string verboseWarning, string caption);

		// Token: 0x0600033C RID: 828
		bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason);

		// Token: 0x0600033D RID: 829
		bool ShouldContinue(string query, string caption);

		// Token: 0x0600033E RID: 830
		bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll);

		// Token: 0x0600033F RID: 831
		bool TransactionAvailable();

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000340 RID: 832
		PSTransactionContext CurrentPSTransaction { get; }

		// Token: 0x06000341 RID: 833
		void ThrowTerminatingError(ErrorRecord errorRecord);
	}
}
