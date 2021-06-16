using System;
using System.Management.Automation.Host;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000327 RID: 807
	internal class RobustConnectionProgress
	{
		// Token: 0x060026EC RID: 9964 RVA: 0x000D9E75 File Offset: 0x000D8075
		public RobustConnectionProgress()
		{
			this._syncObject = new object();
			this._activity = RemotingErrorIdStrings.RCProgressActivity;
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000D9E94 File Offset: 0x000D8094
		public void StartProgress(long sourceId, string computerName, int secondsTotal, PSHost psHost)
		{
			if (psHost == null)
			{
				return;
			}
			if (secondsTotal < 1)
			{
				return;
			}
			if (string.IsNullOrEmpty(computerName))
			{
				throw new ArgumentNullException("computerName");
			}
			lock (this._syncObject)
			{
				if (!this._progressIsRunning)
				{
					this._progressIsRunning = true;
					this._sourceId = sourceId;
					this._secondsTotal = secondsTotal;
					this._secondsRemaining = secondsTotal;
					this._psHost = psHost;
					this._status = StringUtil.Format(RemotingErrorIdStrings.RCProgressStatus, computerName);
					this._progressRecord = new ProgressRecord(0, this._activity, this._status);
					this._updateTimer = new Timer(new TimerCallback(this.UpdateCallback), null, TimeSpan.Zero, new TimeSpan(0, 0, 1));
				}
			}
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000D9F68 File Offset: 0x000D8168
		public void StopProgress(long sourceId)
		{
			lock (this._syncObject)
			{
				if ((sourceId == this._sourceId || sourceId == 0L) && this._progressIsRunning)
				{
					this.RemoveProgressBar();
				}
			}
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000D9FC0 File Offset: 0x000D81C0
		private void UpdateCallback(object state)
		{
			lock (this._syncObject)
			{
				if (this._progressIsRunning)
				{
					if (this._secondsRemaining > 0)
					{
						this._progressRecord.PercentComplete = (this._secondsTotal - this._secondsRemaining) * 100 / this._secondsTotal;
						this._progressRecord.SecondsRemaining = this._secondsRemaining--;
						this._progressRecord.RecordType = ProgressRecordType.Processing;
						this._psHost.UI.WriteProgress(0L, this._progressRecord);
					}
					else
					{
						this.RemoveProgressBar();
					}
				}
			}
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000DA078 File Offset: 0x000D8278
		private void RemoveProgressBar()
		{
			this._progressIsRunning = false;
			this._progressRecord.RecordType = ProgressRecordType.Completed;
			this._psHost.UI.WriteProgress(0L, this._progressRecord);
			this._updateTimer.Dispose();
			this._updateTimer = null;
		}

		// Token: 0x04001337 RID: 4919
		private PSHost _psHost;

		// Token: 0x04001338 RID: 4920
		private string _activity;

		// Token: 0x04001339 RID: 4921
		private string _status;

		// Token: 0x0400133A RID: 4922
		private int _secondsTotal;

		// Token: 0x0400133B RID: 4923
		private int _secondsRemaining;

		// Token: 0x0400133C RID: 4924
		private ProgressRecord _progressRecord;

		// Token: 0x0400133D RID: 4925
		private long _sourceId;

		// Token: 0x0400133E RID: 4926
		private bool _progressIsRunning;

		// Token: 0x0400133F RID: 4927
		private object _syncObject;

		// Token: 0x04001340 RID: 4928
		private Timer _updateTimer;
	}
}
