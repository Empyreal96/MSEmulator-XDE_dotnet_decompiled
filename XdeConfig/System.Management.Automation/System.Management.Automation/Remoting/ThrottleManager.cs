using System;
using System.Collections.Generic;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C6 RID: 710
	internal class ThrottleManager : IDisposable
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x000C11AF File Offset: 0x000BF3AF
		// (set) Token: 0x060021BE RID: 8638 RVA: 0x000C119A File Offset: 0x000BF39A
		internal int ThrottleLimit
		{
			get
			{
				return this.throttleLimit;
			}
			set
			{
				if (value > 0 && value <= ThrottleManager.THROTTLE_LIMIT_MAX)
				{
					this.throttleLimit = value;
				}
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000C11B8 File Offset: 0x000BF3B8
		internal void SubmitOperations(List<IThrottleOperation> operations)
		{
			lock (this.syncObject)
			{
				if (!this.submitComplete)
				{
					using (List<IThrottleOperation>.Enumerator enumerator = operations.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IThrottleOperation item = enumerator.Current;
							this.operationsQueue.Add(item);
						}
						goto IL_55;
					}
					goto IL_4F;
					IL_55:
					goto IL_61;
				}
				IL_4F:
				throw new InvalidOperationException();
			}
			IL_61:
			this.StartOperationsFromQueue();
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000C1248 File Offset: 0x000BF448
		internal void AddOperation(IThrottleOperation operation)
		{
			lock (this.syncObject)
			{
				if (this.submitComplete)
				{
					throw new InvalidOperationException();
				}
				this.operationsQueue.Add(operation);
			}
			this.StartOperationsFromQueue();
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000C12A4 File Offset: 0x000BF4A4
		internal void StopAllOperations()
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (!this.stopping)
				{
					this.stopping = true;
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.RaiseThrottleManagerEvents();
				return;
			}
			IThrottleOperation[] array;
			lock (this.syncObject)
			{
				this.submitComplete = true;
				this.operationsQueue.Clear();
				array = new IThrottleOperation[this.startOperationQueue.Count];
				this.startOperationQueue.CopyTo(array);
				foreach (IThrottleOperation throttleOperation in array)
				{
					this.stopOperationQueue.Add(throttleOperation);
					throttleOperation.IgnoreStop = true;
				}
			}
			foreach (IThrottleOperation throttleOperation2 in array)
			{
				throttleOperation2.StopOperation();
			}
			this.RaiseThrottleManagerEvents();
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000C13B4 File Offset: 0x000BF5B4
		internal void StopOperation(IThrottleOperation operation)
		{
			if (operation.IgnoreStop)
			{
				return;
			}
			if (this.operationsQueue.IndexOf(operation) != -1)
			{
				lock (this.syncObject)
				{
					if (this.operationsQueue.IndexOf(operation) != -1)
					{
						this.operationsQueue.Remove(operation);
						this.RaiseThrottleManagerEvents();
						return;
					}
				}
			}
			lock (this.syncObject)
			{
				this.stopOperationQueue.Add(operation);
				operation.IgnoreStop = true;
			}
			operation.StopOperation();
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000C146C File Offset: 0x000BF66C
		internal void EndSubmitOperations()
		{
			lock (this.syncObject)
			{
				this.submitComplete = true;
			}
			this.RaiseThrottleManagerEvents();
		}

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x060021C5 RID: 8645 RVA: 0x000C14B4 File Offset: 0x000BF6B4
		// (remove) Token: 0x060021C6 RID: 8646 RVA: 0x000C14EC File Offset: 0x000BF6EC
		internal event EventHandler<EventArgs> ThrottleComplete;

		// Token: 0x060021C7 RID: 8647 RVA: 0x000C1521 File Offset: 0x000BF721
		public ThrottleManager()
		{
			this.operationsQueue = new List<IThrottleOperation>();
			this.startOperationQueue = new List<IThrottleOperation>();
			this.stopOperationQueue = new List<IThrottleOperation>();
			this.syncObject = new object();
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000C1560 File Offset: 0x000BF760
		private void OperationCompleteHandler(object source, OperationStateEventArgs stateEventArgs)
		{
			lock (this.syncObject)
			{
				IThrottleOperation throttleOperation = source as IThrottleOperation;
				if (stateEventArgs.OperationState == OperationState.StartComplete)
				{
					int num = this.startOperationQueue.IndexOf(throttleOperation);
					if (num != -1)
					{
						this.startOperationQueue.RemoveAt(num);
					}
				}
				else
				{
					int num = this.startOperationQueue.IndexOf(throttleOperation);
					if (num != -1)
					{
						this.startOperationQueue.RemoveAt(num);
					}
					num = this.stopOperationQueue.IndexOf(throttleOperation);
					if (num != -1)
					{
						this.stopOperationQueue.RemoveAt(num);
					}
					throttleOperation.IgnoreStop = true;
				}
			}
			this.RaiseThrottleManagerEvents();
			this.StartOneOperationFromQueue();
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000C1618 File Offset: 0x000BF818
		private void StartOneOperationFromQueue()
		{
			IThrottleOperation throttleOperation = null;
			lock (this.syncObject)
			{
				if (this.operationsQueue.Count > 0)
				{
					throttleOperation = this.operationsQueue[0];
					this.operationsQueue.RemoveAt(0);
					throttleOperation.OperationComplete += this.OperationCompleteHandler;
					this.startOperationQueue.Add(throttleOperation);
				}
			}
			if (throttleOperation != null)
			{
				throttleOperation.StartOperation();
			}
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000C16A4 File Offset: 0x000BF8A4
		private void StartOperationsFromQueue()
		{
			int num = 0;
			int num2 = 0;
			lock (this.syncObject)
			{
				num = this.startOperationQueue.Count;
				num2 = this.operationsQueue.Count;
			}
			int num3 = this.throttleLimit - num;
			if (num3 > 0)
			{
				int num4 = (num3 > num2) ? num2 : num3;
				for (int i = 0; i < num4; i++)
				{
					this.StartOneOperationFromQueue();
				}
			}
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000C172C File Offset: 0x000BF92C
		private void RaiseThrottleManagerEvents()
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.submitComplete && this.startOperationQueue.Count == 0 && this.stopOperationQueue.Count == 0 && this.operationsQueue.Count == 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.ThrottleComplete.SafeInvoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000C17B0 File Offset: 0x000BF9B0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000C17BF File Offset: 0x000BF9BF
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAllOperations();
			}
		}

		// Token: 0x04000FFC RID: 4092
		private int throttleLimit = ThrottleManager.DEFAULT_THROTTLE_LIMIT;

		// Token: 0x04000FFE RID: 4094
		private static int DEFAULT_THROTTLE_LIMIT = 32;

		// Token: 0x04000FFF RID: 4095
		private static int THROTTLE_LIMIT_MAX = int.MaxValue;

		// Token: 0x04001000 RID: 4096
		private List<IThrottleOperation> operationsQueue;

		// Token: 0x04001001 RID: 4097
		private List<IThrottleOperation> startOperationQueue;

		// Token: 0x04001002 RID: 4098
		private List<IThrottleOperation> stopOperationQueue;

		// Token: 0x04001003 RID: 4099
		private object syncObject;

		// Token: 0x04001004 RID: 4100
		private bool submitComplete;

		// Token: 0x04001005 RID: 4101
		private bool stopping;
	}
}
