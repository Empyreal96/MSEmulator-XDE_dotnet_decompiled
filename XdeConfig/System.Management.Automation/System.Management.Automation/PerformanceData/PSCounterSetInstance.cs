using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Globalization;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.PerformanceData
{
	// Token: 0x02000903 RID: 2307
	public class PSCounterSetInstance : CounterSetInstanceBase
	{
		// Token: 0x060056B6 RID: 22198 RVA: 0x001C61A4 File Offset: 0x001C43A4
		private void CreateCounterSetInstance()
		{
			this._CounterSet = new CounterSet(this._counterSetRegistrarBase.ProviderId, this._counterSetRegistrarBase.CounterSetId, this._counterSetRegistrarBase.CounterSetInstType);
			foreach (CounterInfo counterInfo in this._counterSetRegistrarBase.CounterInfoArray)
			{
				if (counterInfo.Name == null)
				{
					this._CounterSet.AddCounter(counterInfo.Id, counterInfo.Type);
				}
				else
				{
					this._CounterSet.AddCounter(counterInfo.Id, counterInfo.Type, counterInfo.Name);
				}
			}
			string counterSetInstanceName = PSPerfCountersMgr.Instance.GetCounterSetInstanceName();
			this._CounterSetInstance = this._CounterSet.CreateCounterSetInstance(counterSetInstanceName);
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x001C6265 File Offset: 0x001C4465
		private void UpdateCounterByValue(CounterData TargetCounterData, long stepAmount)
		{
			if (stepAmount == -1L)
			{
				TargetCounterData.Decrement();
				return;
			}
			if (stepAmount == 1L)
			{
				TargetCounterData.Increment();
				return;
			}
			TargetCounterData.IncrementBy(stepAmount);
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x001C6286 File Offset: 0x001C4486
		public PSCounterSetInstance(CounterSetRegistrarBase counterSetRegBaseObj) : base(counterSetRegBaseObj)
		{
			this.CreateCounterSetInstance();
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x001C62A0 File Offset: 0x001C44A0
		~PSCounterSetInstance()
		{
			this.Dispose(false);
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x001C62D0 File Offset: 0x001C44D0
		protected virtual void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				if (disposing)
				{
					this._CounterSetInstance.Dispose();
					this._CounterSet.Dispose();
				}
				this._Disposed = true;
			}
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x001C62FA File Offset: 0x001C44FA
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x001C630C File Offset: 0x001C450C
		public override bool UpdateCounterByValue(int counterId, long stepAmount, bool isNumerator)
		{
			if (this._Disposed)
			{
				ObjectDisposedException exception = new ObjectDisposedException("PSCounterSetInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			int counterId2;
			if (!base.RetrieveTargetCounterIdIfValid(counterId, isNumerator, out counterId2))
			{
				return false;
			}
			CounterData counterData = this._CounterSetInstance.Counters[counterId2];
			if (counterData != null)
			{
				this.UpdateCounterByValue(counterData, stepAmount);
				return true;
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Lookup for counter corresponding to counter id {0} failed", new object[]
			{
				counterId
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x001C63A0 File Offset: 0x001C45A0
		public override bool UpdateCounterByValue(string counterName, long stepAmount, bool isNumerator)
		{
			if (this._Disposed)
			{
				ObjectDisposedException exception = new ObjectDisposedException("PSCounterSetInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			if (counterName == null)
			{
				ArgumentNullException exception2 = new ArgumentNullException("counterName");
				this._tracer.TraceException(exception2);
				return false;
			}
			bool result;
			try
			{
				int counterId = this._counterNameToIdMapping[counterName];
				result = this.UpdateCounterByValue(counterId, stepAmount, isNumerator);
			}
			catch (KeyNotFoundException)
			{
				InvalidOperationException exception3 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Lookup for counter corresponding to counter name {0} failed", new object[]
				{
					counterName
				}));
				this._tracer.TraceException(exception3);
				result = false;
			}
			return result;
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x001C6450 File Offset: 0x001C4650
		public override bool SetCounterValue(int counterId, long counterValue, bool isNumerator)
		{
			if (this._Disposed)
			{
				ObjectDisposedException exception = new ObjectDisposedException("PSCounterSetInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			int counterId2;
			if (!base.RetrieveTargetCounterIdIfValid(counterId, isNumerator, out counterId2))
			{
				return false;
			}
			CounterData counterData = this._CounterSetInstance.Counters[counterId2];
			if (counterData != null)
			{
				counterData.Value = counterValue;
				return true;
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Lookup for counter corresponding to counter id {0} failed", new object[]
			{
				counterId
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x001C64E4 File Offset: 0x001C46E4
		public override bool SetCounterValue(string counterName, long counterValue, bool isNumerator)
		{
			if (this._Disposed)
			{
				ObjectDisposedException exception = new ObjectDisposedException("PSCounterSetInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			if (counterName == null)
			{
				ArgumentNullException exception2 = new ArgumentNullException("counterName");
				this._tracer.TraceException(exception2);
				return false;
			}
			bool result;
			try
			{
				int counterId = this._counterNameToIdMapping[counterName];
				result = this.SetCounterValue(counterId, counterValue, isNumerator);
			}
			catch (KeyNotFoundException)
			{
				InvalidOperationException exception3 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Lookup for counter corresponding to counter name {0} failed", new object[]
				{
					counterName
				}));
				this._tracer.TraceException(exception3);
				result = false;
			}
			return result;
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x001C6594 File Offset: 0x001C4794
		public override bool GetCounterValue(int counterId, bool isNumerator, out long counterValue)
		{
			counterValue = -1L;
			if (this._Disposed)
			{
				ObjectDisposedException exception = new ObjectDisposedException("PSCounterSetInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			int counterId2;
			if (!base.RetrieveTargetCounterIdIfValid(counterId, isNumerator, out counterId2))
			{
				return false;
			}
			CounterData counterData = this._CounterSetInstance.Counters[counterId2];
			if (counterData != null)
			{
				counterValue = counterData.Value;
				return true;
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Lookup for counter corresponding to counter id {0} failed", new object[]
			{
				counterId
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x001C662C File Offset: 0x001C482C
		public override bool GetCounterValue(string counterName, bool isNumerator, out long counterValue)
		{
			counterValue = -1L;
			if (this._Disposed)
			{
				ObjectDisposedException exception = new ObjectDisposedException("PSCounterSetInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			if (counterName == null)
			{
				ArgumentNullException exception2 = new ArgumentNullException("counterName");
				this._tracer.TraceException(exception2);
				return false;
			}
			bool result;
			try
			{
				int counterId = this._counterNameToIdMapping[counterName];
				result = this.GetCounterValue(counterId, isNumerator, out counterValue);
			}
			catch (KeyNotFoundException)
			{
				InvalidOperationException exception3 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Lookup for counter corresponding to counter name {0} failed", new object[]
				{
					counterName
				}));
				this._tracer.TraceException(exception3);
				result = false;
			}
			return result;
		}

		// Token: 0x04002E48 RID: 11848
		private bool _Disposed;

		// Token: 0x04002E49 RID: 11849
		private CounterSet _CounterSet;

		// Token: 0x04002E4A RID: 11850
		private CounterSetInstance _CounterSetInstance;

		// Token: 0x04002E4B RID: 11851
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();
	}
}
