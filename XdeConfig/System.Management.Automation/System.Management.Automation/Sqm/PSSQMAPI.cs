using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using Microsoft.PowerShell;

namespace System.Management.Automation.Sqm
{
	// Token: 0x02000864 RID: 2148
	public static class PSSQMAPI
	{
		// Token: 0x0600527F RID: 21119 RVA: 0x001B814C File Offset: 0x001B634C
		static PSSQMAPI()
		{
			if (!WinSQMWrapper.IsWinSqmOptedIn())
			{
				return;
			}
			PSSQMAPI.dataValueCache = new Dictionary<uint, uint>();
			PSSQMAPI.cmdletData = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			PSSQMAPI.workflowData = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			PSSQMAPI.workflowCommonParameterData = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			PSSQMAPI.workflowOotbActivityData = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			PSSQMAPI.workflowSpecificParameterTypeData = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			PSSQMAPI.workflowStateData = new Dictionary<Guid, Tuple<uint, uint, uint, uint>>();
			PSSQMAPI.workflowTypeData = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
			PSSQMAPI.runspaceDurationData = new Dictionary<Guid, long>();
			PSSQMAPI.workflowExecutionDurationData = new Dictionary<Guid, long>();
			PSSQMAPI.timeValueThreshold = 600000000L;
			AppDomain.CurrentDomain.ProcessExit += PSSQMAPI.CurrentDomain_ProcessExit;
			PSSQMAPI.startedAtTick = DateTime.Now.Ticks;
			PSSQMAPI.isWinSQMEnabled = true;
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x001B822C File Offset: 0x001B642C
		public static void NoteRunspaceStart(Guid rsInstanceId)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				PSSQMAPI.runspaceDurationData[rsInstanceId] = DateTime.Now.Ticks;
			}
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x001B8288 File Offset: 0x001B6488
		public static void NoteRunspaceEnd(Guid rsInstanceId)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			long ticks = DateTime.Now.Ticks;
			long num = ticks;
			lock (PSSQMAPI.syncObject)
			{
				if (!PSSQMAPI.runspaceDurationData.ContainsKey(rsInstanceId))
				{
					return;
				}
				num = PSSQMAPI.runspaceDurationData[rsInstanceId];
				PSSQMAPI.runspaceDurationData.Remove(rsInstanceId);
			}
			try
			{
				long num2 = ticks - num;
				if (num2 >= PSSQMAPI.timeValueThreshold)
				{
					WinSQMWrapper.WinSqmAddToStream(9830U, new TimeSpan(num2).TotalMinutes.ToString(CultureInfo.InvariantCulture));
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x001B8350 File Offset: 0x001B6550
		public static void NoteWorkflowStart(Guid workflowInstanceId)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				PSSQMAPI.isWorkflowHost = true;
				PSSQMAPI.workflowExecutionDurationData[workflowInstanceId] = DateTime.Now.Ticks;
			}
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x001B83B0 File Offset: 0x001B65B0
		public static void NoteWorkflowEnd(Guid workflowInstanceId)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			long ticks = DateTime.Now.Ticks;
			long num;
			lock (PSSQMAPI.syncObject)
			{
				if (!PSSQMAPI.workflowExecutionDurationData.ContainsKey(workflowInstanceId))
				{
					return;
				}
				num = PSSQMAPI.workflowExecutionDurationData[workflowInstanceId];
				PSSQMAPI.workflowExecutionDurationData.Remove(workflowInstanceId);
			}
			try
			{
				long ticks2 = ticks - num;
				WinSQMWrapper.WinSqmAddToStream(9820U, new TimeSpan(ticks2).TotalMinutes.ToString(CultureInfo.InvariantCulture));
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x001B8470 File Offset: 0x001B6670
		public static void NoteSessionConfigurationIdleTimeout(int idleTimeout)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			try
			{
				WinSQMWrapper.WinSqmAddToStream(8351U, idleTimeout.ToString(CultureInfo.InvariantCulture));
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x001B84B8 File Offset: 0x001B66B8
		public static void NoteSessionConfigurationOutputBufferingMode(string optBufferingMode)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			try
			{
				WinSQMWrapper.WinSqmAddToStream(8376U, optBufferingMode);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x001B84F4 File Offset: 0x001B66F4
		public static void NoteWorkflowOutputStreamSize(uint size, string streamType)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			try
			{
				WinSQMWrapper.WinSqmAddToStream(9882U, streamType, size);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001B8530 File Offset: 0x001B6730
		public static void NoteWorkflowEndpointConfiguration(string quotaName, uint data)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			try
			{
				WinSQMWrapper.WinSqmAddToStream(9881U, quotaName, data);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001B856C File Offset: 0x001B676C
		public static void NoteWorkflowCommonParametersValues(string parameterName, uint data)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			try
			{
				WinSQMWrapper.WinSqmAddToStream(9869U, parameterName, data);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x001B85A8 File Offset: 0x001B67A8
		public static void UpdateWorkflowsConcurrentExecution(uint numberWorkflows)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				PSSQMAPI.isWorkflowHost = true;
				uint num;
				PSSQMAPI.dataValueCache.TryGetValue(9879U, out num);
				if (num < numberWorkflows)
				{
					PSSQMAPI.dataValueCache[9879U] = numberWorkflows;
				}
			}
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x001B8618 File Offset: 0x001B6818
		public static void IncrementWorkflowCommonParameterPresent(string parameterName)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				uint num;
				PSSQMAPI.workflowCommonParameterData.TryGetValue(parameterName, out num);
				num += 1U;
				PSSQMAPI.workflowCommonParameterData[parameterName] = num;
			}
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001B8678 File Offset: 0x001B6878
		public static void IncrementWorkflowActivityPresent(string activityName)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				uint num;
				PSSQMAPI.workflowOotbActivityData.TryGetValue(activityName, out num);
				num += 1U;
				PSSQMAPI.workflowOotbActivityData[activityName] = num;
			}
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x001B86D8 File Offset: 0x001B68D8
		public static void IncrementWorkflowExecuted(string workflowName)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				uint num;
				PSSQMAPI.workflowData.TryGetValue(workflowName.GetHashCode().ToString(CultureInfo.InvariantCulture), out num);
				num += 1U;
				PSSQMAPI.workflowData[workflowName.GetHashCode().ToString(CultureInfo.InvariantCulture)] = num;
			}
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x001B875C File Offset: 0x001B695C
		public static void IncrementWorkflowType(string workflowType)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				uint num;
				PSSQMAPI.workflowTypeData.TryGetValue(workflowType, out num);
				num += 1U;
				PSSQMAPI.workflowTypeData[workflowType] = num;
			}
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x001B87BC File Offset: 0x001B69BC
		public static void UpdateExecutionPolicy(string shellId, ExecutionPolicy executionPolicy)
		{
			if (!PSSQMAPI.isWinSQMEnabled || !shellId.Equals(Utils.DefaultPowerShellShellID, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				PSSQMAPI.dataValueCache[8344U] = (uint)executionPolicy;
			}
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x001B881C File Offset: 0x001B6A1C
		public static void IncrementData(CommandTypes cmdType)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			PSSqmDataPoint dataPoint;
			if (cmdType <= CommandTypes.ExternalScript)
			{
				switch (cmdType)
				{
				case CommandTypes.Alias:
					dataPoint = PSSqmDataPoint.Alias;
					break;
				case CommandTypes.Function:
					dataPoint = PSSqmDataPoint.Function;
					break;
				case CommandTypes.Alias | CommandTypes.Function:
					return;
				case CommandTypes.Filter:
					dataPoint = PSSqmDataPoint.Filter;
					break;
				default:
					if (cmdType != CommandTypes.Cmdlet)
					{
						if (cmdType != CommandTypes.ExternalScript)
						{
							return;
						}
						dataPoint = PSSqmDataPoint.ExternalScript;
					}
					else
					{
						dataPoint = PSSqmDataPoint.Cmdlet;
					}
					break;
				}
			}
			else if (cmdType != CommandTypes.Application)
			{
				if (cmdType != CommandTypes.Script)
				{
					if (cmdType != CommandTypes.Workflow)
					{
						return;
					}
					dataPoint = PSSqmDataPoint.Function;
				}
				else
				{
					dataPoint = PSSqmDataPoint.Script;
				}
			}
			else
			{
				dataPoint = PSSqmDataPoint.Application;
			}
			PSSQMAPI.IncrementDataPoint((uint)dataPoint);
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x001B88BA File Offset: 0x001B6ABA
		public static void IncrementData(CmdletInfo cmdlet)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			if (cmdlet.PSSnapIn == null || !cmdlet.PSSnapIn.IsDefault)
			{
				return;
			}
			PSSQMAPI.IncrementDataPoint(cmdlet.Name);
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x001B88E8 File Offset: 0x001B6AE8
		public static void InitiateWorkflowStateDataTracking(Job parentJob)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			if (parentJob.IsFinishedState(parentJob.JobStateInfo.State))
			{
				return;
			}
			if (parentJob.ChildJobs.Count == 0)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				if (!PSSQMAPI.workflowStateData.ContainsKey(parentJob.InstanceId))
				{
					PSSQMAPI.workflowStateData.Add(parentJob.InstanceId, new Tuple<uint, uint, uint, uint>((uint)parentJob.ChildJobs.Count, 0U, 0U, 0U));
					foreach (Job job in parentJob.ChildJobs)
					{
						if (job.IsFinishedState(job.JobStateInfo.State))
						{
							PSSQMAPI.IncrementWorkflowStateData(parentJob.InstanceId, job.JobStateInfo.State);
						}
					}
				}
			}
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x001B89E4 File Offset: 0x001B6BE4
		public static void IncrementWorkflowStateData(Guid parentJobInstanceId, JobState state)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				if (PSSQMAPI.workflowStateData.ContainsKey(parentJobInstanceId))
				{
					Tuple<uint, uint, uint, uint> tuple = PSSQMAPI.workflowStateData[parentJobInstanceId];
					uint num = tuple.Item1;
					if (num != 0U)
					{
						uint num2 = tuple.Item2;
						uint num3 = tuple.Item3;
						uint num4 = tuple.Item4;
						switch (state)
						{
						case JobState.Completed:
							num2 += 1U;
							break;
						case JobState.Failed:
							num3 += 1U;
							break;
						case JobState.Stopped:
							num4 += 1U;
							break;
						default:
							return;
						}
						num -= 1U;
						PSSQMAPI.workflowStateData[parentJobInstanceId] = new Tuple<uint, uint, uint, uint>(num, num2, num3, num4);
					}
				}
			}
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x001B8AB4 File Offset: 0x001B6CB4
		public static void IncrementWorkflowSpecificParameterType(Type parameterType)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				PSSQMAPI.IncrementDataPoint(9822U);
				uint num;
				PSSQMAPI.workflowSpecificParameterTypeData.TryGetValue(parameterType.FullName, out num);
				num += 1U;
				PSSQMAPI.workflowSpecificParameterTypeData[parameterType.FullName] = num;
			}
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x001B8B28 File Offset: 0x001B6D28
		public static void IncrementDataPoint(uint dataPoint)
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				uint num;
				PSSQMAPI.dataValueCache.TryGetValue(dataPoint, out num);
				num += 1U;
				PSSQMAPI.dataValueCache[dataPoint] = num;
			}
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x001B8B88 File Offset: 0x001B6D88
		public static void LogAllDataSuppressExceptions()
		{
			if (!PSSQMAPI.isWinSQMEnabled)
			{
				return;
			}
			lock (PSSQMAPI.syncObject)
			{
				long ticks = DateTime.Now.Ticks;
				long num = ticks - PSSQMAPI.startedAtTick;
				if (num >= PSSQMAPI.timeValueThreshold)
				{
					if (PSSQMAPI.isWorkflowHost)
					{
						double totalMinutes = TimeSpan.FromTicks(num).TotalMinutes;
						if (totalMinutes > 4294967295.0)
						{
							PSSQMAPI.dataValueCache.Add(9821U, uint.MaxValue);
						}
						else
						{
							PSSQMAPI.dataValueCache.Add(9821U, (uint)totalMinutes);
						}
					}
					PSSQMAPI.FlushDataSuppressExceptions();
					PSSQMAPI.startedAtTick = ticks;
				}
			}
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x001B8C3C File Offset: 0x001B6E3C
		private static void IncrementDataPoint(string cmdletName)
		{
			lock (PSSQMAPI.syncObject)
			{
				uint num;
				PSSQMAPI.cmdletData.TryGetValue(cmdletName, out num);
				num += 1U;
				PSSQMAPI.cmdletData[cmdletName] = num;
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x001B8C94 File Offset: 0x001B6E94
		private static void FlushDataSuppressExceptions()
		{
			try
			{
				uint num = 8344U;
				if (PSSQMAPI.dataValueCache.ContainsKey(num))
				{
					WinSQMWrapper.WinSqmSet(num, PSSQMAPI.dataValueCache[num]);
					PSSQMAPI.dataValueCache.Remove(num);
				}
				WinSQMWrapper.WinSqmIncrement(PSSQMAPI.dataValueCache);
				PSSQMAPI.dataValueCache.Clear();
				PSSQMAPI.WriteWorkflowStateDataToStreamAndClear();
				PSSQMAPI.WriteAllDataToStreamAndClear(PSSQMAPI.cmdletData, 9829U);
				PSSQMAPI.WriteAllDataToStreamAndClear(PSSQMAPI.workflowData, 9826U);
				PSSQMAPI.WriteAllDataToStreamAndClear(PSSQMAPI.workflowTypeData, 9878U);
				PSSQMAPI.WriteAllDataToStreamAndClear(PSSQMAPI.workflowCommonParameterData, 9824U);
				PSSQMAPI.WriteAllDataToStreamAndClear(PSSQMAPI.workflowOotbActivityData, 9825U);
				PSSQMAPI.WriteAllDataToStreamAndClear(PSSQMAPI.workflowSpecificParameterTypeData, 9827U);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x001B8D60 File Offset: 0x001B6F60
		private static void WriteWorkflowStateDataToStreamAndClear()
		{
			Dictionary<Guid, Tuple<uint, uint, uint, uint>> dictionary = new Dictionary<Guid, Tuple<uint, uint, uint, uint>>();
			foreach (KeyValuePair<Guid, Tuple<uint, uint, uint, uint>> keyValuePair in PSSQMAPI.workflowStateData)
			{
				if (keyValuePair.Value.Item1 != 0U)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
				else
				{
					uint num = keyValuePair.Value.Item2 + keyValuePair.Value.Item3 + keyValuePair.Value.Item4;
					if (num != 0U)
					{
						WinSQMWrapper.WinSqmAddToStream(9880U, JobState.Completed.ToString(), Convert.ToUInt32(keyValuePair.Value.Item2 / num * 100.0));
						WinSQMWrapper.WinSqmAddToStream(9880U, JobState.Failed.ToString(), Convert.ToUInt32(keyValuePair.Value.Item3 / num * 100.0));
						WinSQMWrapper.WinSqmAddToStream(9880U, JobState.Stopped.ToString(), Convert.ToUInt32(keyValuePair.Value.Item4 / num * 100.0));
					}
				}
			}
			PSSQMAPI.workflowStateData = dictionary;
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x001B8EC0 File Offset: 0x001B70C0
		private static void WriteAllDataToStreamAndClear(Dictionary<string, uint> data, uint dataPoint)
		{
			foreach (string text in data.Keys)
			{
				WinSQMWrapper.WinSqmAddToStream(dataPoint, text, data[text]);
			}
			data.Clear();
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x001B8F20 File Offset: 0x001B7120
		private static void CurrentDomain_ProcessExit(object source, EventArgs args)
		{
			PSSQMAPI.LogAllDataSuppressExceptions();
		}

		// Token: 0x04002A71 RID: 10865
		private static readonly long timeValueThreshold;

		// Token: 0x04002A72 RID: 10866
		private static readonly Dictionary<string, uint> cmdletData;

		// Token: 0x04002A73 RID: 10867
		private static readonly Dictionary<string, uint> workflowData;

		// Token: 0x04002A74 RID: 10868
		private static readonly Dictionary<string, uint> workflowCommonParameterData;

		// Token: 0x04002A75 RID: 10869
		private static readonly Dictionary<string, uint> workflowOotbActivityData;

		// Token: 0x04002A76 RID: 10870
		private static readonly Dictionary<string, uint> workflowSpecificParameterTypeData;

		// Token: 0x04002A77 RID: 10871
		private static Dictionary<Guid, Tuple<uint, uint, uint, uint>> workflowStateData;

		// Token: 0x04002A78 RID: 10872
		private static readonly Dictionary<string, uint> workflowTypeData;

		// Token: 0x04002A79 RID: 10873
		private static readonly Dictionary<uint, uint> dataValueCache;

		// Token: 0x04002A7A RID: 10874
		private static readonly Dictionary<Guid, long> runspaceDurationData;

		// Token: 0x04002A7B RID: 10875
		private static readonly Dictionary<Guid, long> workflowExecutionDurationData;

		// Token: 0x04002A7C RID: 10876
		private static long startedAtTick;

		// Token: 0x04002A7D RID: 10877
		private static readonly object syncObject = new object();

		// Token: 0x04002A7E RID: 10878
		private static bool isWorkflowHost = false;

		// Token: 0x04002A7F RID: 10879
		private static bool isWinSQMEnabled;
	}
}
