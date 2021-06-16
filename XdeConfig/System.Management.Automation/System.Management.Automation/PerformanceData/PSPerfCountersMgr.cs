using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.PerformanceData
{
	// Token: 0x02000907 RID: 2311
	public class PSPerfCountersMgr
	{
		// Token: 0x060056D5 RID: 22229 RVA: 0x001C68FD File Offset: 0x001C4AFD
		private PSPerfCountersMgr()
		{
			this._CounterSetIdToInstanceMapping = new ConcurrentDictionary<Guid, CounterSetInstanceBase>();
			this._CounterSetNameToIdMapping = new ConcurrentDictionary<string, Guid>();
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x001C6928 File Offset: 0x001C4B28
		~PSPerfCountersMgr()
		{
			this.RemoveAllCounterSets();
		}

		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x060056D7 RID: 22231 RVA: 0x001C6954 File Offset: 0x001C4B54
		public static PSPerfCountersMgr Instance
		{
			get
			{
				if (PSPerfCountersMgr._PSPerfCountersMgrInstance == null)
				{
					PSPerfCountersMgr._PSPerfCountersMgrInstance = new PSPerfCountersMgr();
				}
				return PSPerfCountersMgr._PSPerfCountersMgrInstance;
			}
		}

		// Token: 0x060056D8 RID: 22232 RVA: 0x001C696C File Offset: 0x001C4B6C
		public string GetCounterSetInstanceName()
		{
			Process currentProcess = Process.GetCurrentProcess();
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				currentProcess.Id
			});
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x001C69A8 File Offset: 0x001C4BA8
		public bool IsCounterSetRegistered(string counterSetName, out Guid counterSetId)
		{
			counterSetId = default(Guid);
			if (counterSetName == null)
			{
				ArgumentNullException exception = new ArgumentNullException("counterSetName");
				this._tracer.TraceException(exception);
				return false;
			}
			return this._CounterSetNameToIdMapping.TryGetValue(counterSetName, out counterSetId);
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x001C69E6 File Offset: 0x001C4BE6
		public bool IsCounterSetRegistered(Guid counterSetId, out CounterSetInstanceBase counterSetInst)
		{
			return this._CounterSetIdToInstanceMapping.TryGetValue(counterSetId, out counterSetInst);
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x001C69F8 File Offset: 0x001C4BF8
		public bool AddCounterSetInstance(CounterSetRegistrarBase counterSetRegistrarInstance)
		{
			if (counterSetRegistrarInstance == null)
			{
				ArgumentNullException exception = new ArgumentNullException("counterSetRegistrarInstance");
				this._tracer.TraceException(exception);
				return false;
			}
			Guid counterSetId = counterSetRegistrarInstance.CounterSetId;
			string counterSetName = counterSetRegistrarInstance.CounterSetName;
			CounterSetInstanceBase counterSetInstanceBase = null;
			if (this.IsCounterSetRegistered(counterSetId, out counterSetInstanceBase))
			{
				InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "A Counter Set Instance with id '{0}' is already registered", new object[]
				{
					counterSetId
				}));
				this._tracer.TraceException(exception2);
				return false;
			}
			try
			{
				if (!string.IsNullOrWhiteSpace(counterSetName))
				{
					Guid guid;
					if (this.IsCounterSetRegistered(counterSetName, out guid))
					{
						InvalidOperationException exception3 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "A Counter Set Instance with name '{0}' is already registered", new object[]
						{
							counterSetName
						}));
						this._tracer.TraceException(exception3);
						return false;
					}
					this._CounterSetNameToIdMapping.TryAdd(counterSetName, counterSetId);
				}
				this._CounterSetIdToInstanceMapping.TryAdd(counterSetId, counterSetRegistrarInstance.CounterSetInstance);
			}
			catch (OverflowException exception4)
			{
				this._tracer.TraceException(exception4);
				return false;
			}
			return true;
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x001C6B10 File Offset: 0x001C4D10
		public bool UpdateCounterByValue(Guid counterSetId, int counterId, long stepAmount = 1L, bool isNumerator = true)
		{
			CounterSetInstanceBase counterSetInstanceBase = null;
			if (this.IsCounterSetRegistered(counterSetId, out counterSetInstanceBase))
			{
				return counterSetInstanceBase.UpdateCounterByValue(counterId, stepAmount, isNumerator);
			}
			InvalidOperationException exception = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with id '{0}' is registered", new object[]
			{
				counterSetId
			}));
			this._tracer.TraceException(exception);
			return false;
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x001C6B6C File Offset: 0x001C4D6C
		public bool UpdateCounterByValue(Guid counterSetId, string counterName, long stepAmount = 1L, bool isNumerator = true)
		{
			CounterSetInstanceBase counterSetInstanceBase = null;
			if (this.IsCounterSetRegistered(counterSetId, out counterSetInstanceBase))
			{
				return counterSetInstanceBase.UpdateCounterByValue(counterName, stepAmount, isNumerator);
			}
			InvalidOperationException exception = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with id '{0}' is registered", new object[]
			{
				counterSetId
			}));
			this._tracer.TraceException(exception);
			return false;
		}

		// Token: 0x060056DE RID: 22238 RVA: 0x001C6BC8 File Offset: 0x001C4DC8
		public bool UpdateCounterByValue(string counterSetName, int counterId, long stepAmount = 1L, bool isNumerator = true)
		{
			if (counterSetName == null)
			{
				ArgumentNullException exception = new ArgumentNullException("counterSetName");
				this._tracer.TraceException(exception);
				return false;
			}
			Guid guid;
			if (this.IsCounterSetRegistered(counterSetName, out guid))
			{
				CounterSetInstanceBase counterSetInstanceBase = this._CounterSetIdToInstanceMapping[guid];
				return counterSetInstanceBase.UpdateCounterByValue(counterId, stepAmount, isNumerator);
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with id '{0}' is registered", new object[]
			{
				guid
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056DF RID: 22239 RVA: 0x001C6C4C File Offset: 0x001C4E4C
		public bool UpdateCounterByValue(string counterSetName, string counterName, long stepAmount = 1L, bool isNumerator = true)
		{
			if (counterSetName == null)
			{
				ArgumentNullException exception = new ArgumentNullException("counterSetName");
				this._tracer.TraceException(exception);
				return false;
			}
			Guid key;
			if (this.IsCounterSetRegistered(counterSetName, out key))
			{
				CounterSetInstanceBase counterSetInstanceBase = this._CounterSetIdToInstanceMapping[key];
				return counterSetInstanceBase.UpdateCounterByValue(counterName, stepAmount, isNumerator);
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with name {0} is registered", new object[]
			{
				counterSetName
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x001C6CCC File Offset: 0x001C4ECC
		public bool SetCounterValue(Guid counterSetId, int counterId, long counterValue = 1L, bool isNumerator = true)
		{
			CounterSetInstanceBase counterSetInstanceBase = null;
			if (this.IsCounterSetRegistered(counterSetId, out counterSetInstanceBase))
			{
				return counterSetInstanceBase.SetCounterValue(counterId, counterValue, isNumerator);
			}
			InvalidOperationException exception = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with id '{0}' is registered", new object[]
			{
				counterSetId
			}));
			this._tracer.TraceException(exception);
			return false;
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x001C6D28 File Offset: 0x001C4F28
		public bool SetCounterValue(Guid counterSetId, string counterName, long counterValue = 1L, bool isNumerator = true)
		{
			CounterSetInstanceBase counterSetInstanceBase = null;
			if (this.IsCounterSetRegistered(counterSetId, out counterSetInstanceBase))
			{
				return counterSetInstanceBase.SetCounterValue(counterName, counterValue, isNumerator);
			}
			InvalidOperationException exception = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with id '{0}' is registered", new object[]
			{
				counterSetId
			}));
			this._tracer.TraceException(exception);
			return false;
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x001C6D84 File Offset: 0x001C4F84
		public bool SetCounterValue(string counterSetName, int counterId, long counterValue = 1L, bool isNumerator = true)
		{
			if (counterSetName == null)
			{
				ArgumentNullException exception = new ArgumentNullException("counterSetName");
				this._tracer.TraceException(exception);
				return false;
			}
			Guid key;
			if (this.IsCounterSetRegistered(counterSetName, out key))
			{
				CounterSetInstanceBase counterSetInstanceBase = this._CounterSetIdToInstanceMapping[key];
				return counterSetInstanceBase.SetCounterValue(counterId, counterValue, isNumerator);
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with name '{0}' is registered", new object[]
			{
				counterSetName
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x001C6E04 File Offset: 0x001C5004
		public bool SetCounterValue(string counterSetName, string counterName, long counterValue = 1L, bool isNumerator = true)
		{
			if (counterSetName == null)
			{
				ArgumentNullException exception = new ArgumentNullException("counterSetName");
				this._tracer.TraceException(exception);
				return false;
			}
			Guid key;
			if (this.IsCounterSetRegistered(counterSetName, out key))
			{
				CounterSetInstanceBase counterSetInstanceBase = this._CounterSetIdToInstanceMapping[key];
				return counterSetInstanceBase.SetCounterValue(counterName, counterValue, isNumerator);
			}
			InvalidOperationException exception2 = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No Counter Set Instance with name '{0}' is registered", new object[]
			{
				counterSetName
			}));
			this._tracer.TraceException(exception2);
			return false;
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x001C6E84 File Offset: 0x001C5084
		internal void RemoveAllCounterSets()
		{
			ICollection<Guid> keys = this._CounterSetIdToInstanceMapping.Keys;
			foreach (Guid key in keys)
			{
				CounterSetInstanceBase counterSetInstanceBase = this._CounterSetIdToInstanceMapping[key];
				counterSetInstanceBase.Dispose();
			}
			this._CounterSetIdToInstanceMapping.Clear();
			this._CounterSetNameToIdMapping.Clear();
		}

		// Token: 0x04002E55 RID: 11861
		private static PSPerfCountersMgr _PSPerfCountersMgrInstance;

		// Token: 0x04002E56 RID: 11862
		private ConcurrentDictionary<Guid, CounterSetInstanceBase> _CounterSetIdToInstanceMapping;

		// Token: 0x04002E57 RID: 11863
		private ConcurrentDictionary<string, Guid> _CounterSetNameToIdMapping;

		// Token: 0x04002E58 RID: 11864
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();
	}
}
