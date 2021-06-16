using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000200 RID: 512
	[OutputType(new Type[]
	{
		typeof(HistoryInfo)
	})]
	[Cmdlet("Get", "History", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113317")]
	public class GetHistoryCommand : PSCmdlet
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x0009325B File Offset: 0x0009145B
		// (set) Token: 0x060017CD RID: 6093 RVA: 0x00093263 File Offset: 0x00091463
		[Parameter(Position = 0, ValueFromPipeline = true)]
		[ValidateRange(1L, 9223372036854775807L)]
		public long[] Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x0009326C File Offset: 0x0009146C
		// (set) Token: 0x060017CF RID: 6095 RVA: 0x00093274 File Offset: 0x00091474
		[Parameter(Position = 1)]
		[ValidateRange(0, 32767)]
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._countParameterSpecified = true;
				this._count = value;
			}
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00093284 File Offset: 0x00091484
		protected override void ProcessRecord()
		{
			History history = ((LocalRunspace)base.Context.CurrentRunspace).History;
			if (this._id == null)
			{
				if (!this._countParameterSpecified)
				{
					this._count = history.Buffercapacity();
				}
				HistoryInfo[] entries = history.GetEntries(0L, (long)this._count, true);
				for (long num = (long)(entries.Length - 1); num >= 0L; num -= 1L)
				{
					base.WriteObject(entries[(int)(checked((IntPtr)num))]);
				}
				return;
			}
			if (!this._countParameterSpecified)
			{
				foreach (long num2 in this._id)
				{
					HistoryInfo entry = history.GetEntry(num2);
					if (entry != null && entry.Id == num2)
					{
						base.WriteObject(entry);
					}
					else
					{
						Exception exception = new ArgumentException(StringUtil.Format(HistoryStrings.NoHistoryForId, num2));
						base.WriteError(new ErrorRecord(exception, "GetHistoryNoHistoryForId", ErrorCategory.ObjectNotFound, num2));
					}
				}
				return;
			}
			if (this._id.Length > 1)
			{
				Exception exception2 = new ArgumentException(StringUtil.Format(HistoryStrings.NoCountWithMultipleIds, new object[0]));
				base.ThrowTerminatingError(new ErrorRecord(exception2, "GetHistoryNoCountWithMultipleIds", ErrorCategory.InvalidArgument, this._count));
				return;
			}
			long id2 = this._id[0];
			base.WriteObject(history.GetEntries(id2, (long)this._count, false), true);
		}

		// Token: 0x04000A0B RID: 2571
		private long[] _id;

		// Token: 0x04000A0C RID: 2572
		private bool _countParameterSpecified;

		// Token: 0x04000A0D RID: 2573
		private int _count;
	}
}
