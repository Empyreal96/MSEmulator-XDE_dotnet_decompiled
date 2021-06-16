using System;
using System.Collections.Concurrent;
using System.Diagnostics.PerformanceData;
using System.Globalization;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.PerformanceData
{
	// Token: 0x02000902 RID: 2306
	public abstract class CounterSetInstanceBase : IDisposable
	{
		// Token: 0x060056AD RID: 22189 RVA: 0x001C6024 File Offset: 0x001C4224
		protected CounterSetInstanceBase(CounterSetRegistrarBase counterSetRegistrarInst)
		{
			this._counterSetRegistrarBase = counterSetRegistrarInst;
			this._counterNameToIdMapping = new ConcurrentDictionary<string, int>();
			this._counterIdToTypeMapping = new ConcurrentDictionary<int, CounterType>();
			CounterInfo[] counterInfoArray = this._counterSetRegistrarBase.CounterInfoArray;
			for (int i = 0; i < counterInfoArray.Length; i++)
			{
				this._counterIdToTypeMapping.TryAdd(counterInfoArray[i].Id, counterInfoArray[i].Type);
				if (!string.IsNullOrWhiteSpace(counterInfoArray[i].Name))
				{
					this._counterNameToIdMapping.TryAdd(counterInfoArray[i].Name, counterInfoArray[i].Id);
				}
			}
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x001C60D4 File Offset: 0x001C42D4
		protected bool RetrieveTargetCounterIdIfValid(int counterId, bool isNumerator, out int targetCounterId)
		{
			targetCounterId = counterId;
			if (!isNumerator)
			{
				bool flag = false;
				CounterType counterType = this._counterIdToTypeMapping[counterId];
				CounterType counterType2 = counterType;
				if (counterType2 <= CounterType.MultiTimerPercentageActive)
				{
					if (counterType2 <= CounterType.RawFraction64)
					{
						if (counterType2 != CounterType.RawFraction32 && counterType2 != CounterType.RawFraction64)
						{
							goto IL_82;
						}
					}
					else if (counterType2 != CounterType.SampleFraction && counterType2 != CounterType.MultiTimerPercentageActive)
					{
						goto IL_82;
					}
				}
				else if (counterType2 <= CounterType.MultiTimerPercentageNotActive)
				{
					if (counterType2 != CounterType.MultiTimerPercentageActive100Ns && counterType2 != CounterType.MultiTimerPercentageNotActive)
					{
						goto IL_82;
					}
				}
				else if (counterType2 != CounterType.MultiTimerPercentageNotActive100Ns && counterType2 != CounterType.AverageTimer32 && counterType2 != CounterType.AverageCount64)
				{
					goto IL_82;
				}
				flag = true;
				IL_82:
				if (!flag)
				{
					InvalidOperationException exception = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Denominator for update not valid for the given counter id {0}", new object[]
					{
						counterId
					}));
					this._tracer.TraceException(exception);
					return false;
				}
				targetCounterId = counterId + 1;
			}
			return true;
		}

		// Token: 0x060056AF RID: 22191
		public abstract bool UpdateCounterByValue(int counterId, long stepAmount, bool isNumerator);

		// Token: 0x060056B0 RID: 22192
		public abstract bool UpdateCounterByValue(string counterName, long stepAmount, bool isNumerator);

		// Token: 0x060056B1 RID: 22193
		public abstract bool SetCounterValue(int counterId, long counterValue, bool isNumerator);

		// Token: 0x060056B2 RID: 22194
		public abstract bool SetCounterValue(string counterName, long counterValue, bool isNumerator);

		// Token: 0x060056B3 RID: 22195
		public abstract bool GetCounterValue(int counterId, bool isNumerator, out long counterValue);

		// Token: 0x060056B4 RID: 22196
		public abstract bool GetCounterValue(string counterName, bool isNumerator, out long counterValue);

		// Token: 0x060056B5 RID: 22197
		public abstract void Dispose();

		// Token: 0x04002E44 RID: 11844
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04002E45 RID: 11845
		protected CounterSetRegistrarBase _counterSetRegistrarBase;

		// Token: 0x04002E46 RID: 11846
		protected ConcurrentDictionary<string, int> _counterNameToIdMapping;

		// Token: 0x04002E47 RID: 11847
		protected ConcurrentDictionary<int, CounterType> _counterIdToTypeMapping;
	}
}
