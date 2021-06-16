using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C7 RID: 711
	internal class Operation : IThrottleOperation
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x000C17E6 File Offset: 0x000BF9E6
		// (set) Token: 0x060021CF RID: 8655 RVA: 0x000C17DD File Offset: 0x000BF9DD
		public bool Done
		{
			get
			{
				return this.done;
			}
			set
			{
				this.done = value;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x000C17F7 File Offset: 0x000BF9F7
		// (set) Token: 0x060021D1 RID: 8657 RVA: 0x000C17EE File Offset: 0x000BF9EE
		public int SleepTime
		{
			get
			{
				return this.sleepTime;
			}
			set
			{
				this.sleepTime = value;
			}
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000C1800 File Offset: 0x000BFA00
		private void WorkerThreadMethodStart()
		{
			Thread.Sleep(this.sleepTime);
			this.done = true;
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.OperationState = OperationState.StartComplete;
			this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000C183C File Offset: 0x000BFA3C
		private void WorkerThreadMethodStop()
		{
			this.workerThreadStart.Abort();
			OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
			operationStateEventArgs.OperationState = OperationState.StopComplete;
			this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000C1870 File Offset: 0x000BFA70
		internal Operation()
		{
			this.done = false;
			this.workerThreadDelegate = new ThreadStart(this.WorkerThreadMethodStart);
			this.workerThreadStart = new Thread(this.workerThreadDelegate);
			this.workerThreadDelegate = new ThreadStart(this.WorkerThreadMethodStop);
			this.workerThreadStop = new Thread(this.workerThreadDelegate);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000C18D8 File Offset: 0x000BFAD8
		internal override void StartOperation()
		{
			this.workerThreadStart.Start();
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000C18E5 File Offset: 0x000BFAE5
		internal override void StopOperation()
		{
			this.workerThreadStop.Start();
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x060021D8 RID: 8664 RVA: 0x000C18F4 File Offset: 0x000BFAF4
		// (remove) Token: 0x060021D9 RID: 8665 RVA: 0x000C192C File Offset: 0x000BFB2C
		internal override event EventHandler<OperationStateEventArgs> OperationComplete;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x060021DA RID: 8666 RVA: 0x000C1964 File Offset: 0x000BFB64
		// (remove) Token: 0x060021DB RID: 8667 RVA: 0x000C199C File Offset: 0x000BFB9C
		internal event EventHandler<EventArgs> InternalEvent;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x060021DC RID: 8668 RVA: 0x000C19D4 File Offset: 0x000BFBD4
		// (remove) Token: 0x060021DD RID: 8669 RVA: 0x000C1A07 File Offset: 0x000BFC07
		internal event EventHandler<EventArgs> EventHandler
		{
			add
			{
				bool flag = null == this.InternalEvent;
				this.InternalEvent += value;
				if (flag)
				{
					this.OperationComplete += this.Operation_OperationComplete;
				}
			}
			remove
			{
				this.InternalEvent -= value;
			}
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000C1A10 File Offset: 0x000BFC10
		private void Operation_OperationComplete(object sender, OperationStateEventArgs e)
		{
			this.InternalEvent.SafeInvoke(sender, e);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000C1A20 File Offset: 0x000BFC20
		internal static void SubmitOperations(List<object> operations, ThrottleManager throttleManager)
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			foreach (object obj in operations)
			{
				list.Add((IThrottleOperation)obj);
			}
			throttleManager.SubmitOperations(list);
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000C1A80 File Offset: 0x000BFC80
		internal static void AddOperation(object operation, ThrottleManager throttleManager)
		{
			throttleManager.AddOperation((IThrottleOperation)operation);
		}

		// Token: 0x04001006 RID: 4102
		private ThreadStart workerThreadDelegate;

		// Token: 0x04001007 RID: 4103
		private Thread workerThreadStart;

		// Token: 0x04001008 RID: 4104
		private Thread workerThreadStop;

		// Token: 0x04001009 RID: 4105
		private int sleepTime = 100;

		// Token: 0x0400100A RID: 4106
		private bool done;
	}
}
