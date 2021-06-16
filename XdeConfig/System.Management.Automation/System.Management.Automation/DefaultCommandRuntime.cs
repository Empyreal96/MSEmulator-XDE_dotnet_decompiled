using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Host;

namespace System.Management.Automation
{
	// Token: 0x0200004A RID: 74
	internal class DefaultCommandRuntime : ICommandRuntime2, ICommandRuntime
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public DefaultCommandRuntime(List<object> outputList)
		{
			if (outputList == null)
			{
				throw new ArgumentNullException("outputList");
			}
			this.output = outputList;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000DFE6 File Offset: 0x0000C1E6
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0000DFDD File Offset: 0x0000C1DD
		public PSHost Host
		{
			get
			{
				return this.host;
			}
			set
			{
				this.host = value;
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000DFEE File Offset: 0x0000C1EE
		public void WriteDebug(string text)
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
		public void WriteError(ErrorRecord errorRecord)
		{
			if (errorRecord.Exception != null)
			{
				throw errorRecord.Exception;
			}
			throw new InvalidOperationException(errorRecord.ToString());
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000E00C File Offset: 0x0000C20C
		public void WriteObject(object sendToPipeline)
		{
			this.output.Add(sendToPipeline);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000E01C File Offset: 0x0000C21C
		public void WriteObject(object sendToPipeline, bool enumerateCollection)
		{
			if (!enumerateCollection)
			{
				this.output.Add(sendToPipeline);
				return;
			}
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(sendToPipeline);
			if (enumerator == null)
			{
				this.output.Add(sendToPipeline);
				return;
			}
			while (enumerator.MoveNext())
			{
				object item = enumerator.Current;
				this.output.Add(item);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000E069 File Offset: 0x0000C269
		public void WriteProgress(ProgressRecord progressRecord)
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000E06B File Offset: 0x0000C26B
		public void WriteProgress(long sourceId, ProgressRecord progressRecord)
		{
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000E06D File Offset: 0x0000C26D
		public void WriteVerbose(string text)
		{
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000E06F File Offset: 0x0000C26F
		public void WriteWarning(string text)
		{
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000E071 File Offset: 0x0000C271
		public void WriteCommandDetail(string text)
		{
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000E073 File Offset: 0x0000C273
		public void WriteInformation(InformationRecord informationRecord)
		{
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000E075 File Offset: 0x0000C275
		public bool ShouldProcess(string target)
		{
			return true;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000E078 File Offset: 0x0000C278
		public bool ShouldProcess(string target, string action)
		{
			return true;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000E07B File Offset: 0x0000C27B
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			return true;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000E07E File Offset: 0x0000C27E
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			shouldProcessReason = ShouldProcessReason.None;
			return true;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000E085 File Offset: 0x0000C285
		public bool ShouldContinue(string query, string caption)
		{
			return true;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000E088 File Offset: 0x0000C288
		public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			return true;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000E08B File Offset: 0x0000C28B
		public bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll)
		{
			return true;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000E08E File Offset: 0x0000C28E
		public bool TransactionAvailable()
		{
			return false;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000E094 File Offset: 0x0000C294
		public PSTransactionContext CurrentPSTransaction
		{
			get
			{
				string cmdletRequiresUseTx = TransactionStrings.CmdletRequiresUseTx;
				throw new InvalidOperationException(cmdletRequiresUseTx);
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000E0AD File Offset: 0x0000C2AD
		public void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			if (errorRecord.Exception != null)
			{
				throw errorRecord.Exception;
			}
			throw new InvalidOperationException(errorRecord.ToString());
		}

		// Token: 0x0400016B RID: 363
		private List<object> output;

		// Token: 0x0400016C RID: 364
		private PSHost host;
	}
}
