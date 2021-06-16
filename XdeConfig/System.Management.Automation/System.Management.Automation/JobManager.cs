using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Reflection;
using System.Security;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200028D RID: 653
	public sealed class JobManager
	{
		// Token: 0x06001F48 RID: 8008 RVA: 0x000B4E3C File Offset: 0x000B303C
		internal JobManager()
		{
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000B4E68 File Offset: 0x000B3068
		public bool IsRegistered(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return false;
			}
			bool result;
			lock (this._syncObject)
			{
				result = this._sourceAdapters.ContainsKey(typeName);
			}
			return result;
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000B4EBC File Offset: 0x000B30BC
		internal void RegisterJobSourceAdapter(Type jobSourceAdapterType)
		{
			object obj = null;
			if (jobSourceAdapterType.FullName != null && jobSourceAdapterType.FullName.EndsWith("WorkflowJobSourceAdapter", StringComparison.OrdinalIgnoreCase))
			{
				MethodInfo method = jobSourceAdapterType.GetMethod("GetInstance");
				obj = method.Invoke(null, null);
			}
			else
			{
				ConstructorInfo constructor = jobSourceAdapterType.GetConstructor(PSTypeExtensions.EmptyTypes);
				if (!constructor.IsPublic)
				{
					string message = string.Format(CultureInfo.CurrentCulture, RemotingErrorIdStrings.JobManagerRegistrationConstructorError, new object[]
					{
						jobSourceAdapterType.FullName
					});
					throw new InvalidOperationException(message);
				}
				try
				{
					obj = constructor.Invoke(null);
				}
				catch (MemberAccessException exception)
				{
					this.Tracer.TraceException(exception);
					throw;
				}
				catch (TargetInvocationException exception2)
				{
					this.Tracer.TraceException(exception2);
					throw;
				}
				catch (TargetParameterCountException exception3)
				{
					this.Tracer.TraceException(exception3);
					throw;
				}
				catch (NotSupportedException exception4)
				{
					this.Tracer.TraceException(exception4);
					throw;
				}
				catch (SecurityException exception5)
				{
					this.Tracer.TraceException(exception5);
					throw;
				}
			}
			if (obj != null)
			{
				lock (this._syncObject)
				{
					this._sourceAdapters.Add(jobSourceAdapterType.Name, (JobSourceAdapter)obj);
				}
			}
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000B502C File Offset: 0x000B322C
		internal static JobIdentifier GetJobIdentifier(Guid instanceId, string typeName)
		{
			JobIdentifier result;
			lock (JobManager.SyncObject)
			{
				if (JobManager.JobIdsForReuse.ContainsKey(instanceId) && JobManager.JobIdsForReuse[instanceId].Value.Equals(typeName))
				{
					result = new JobIdentifier(JobManager.JobIdsForReuse[instanceId].Key, instanceId);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000B50B0 File Offset: 0x000B32B0
		internal static void SaveJobId(Guid instanceId, int id, string typeName)
		{
			lock (JobManager.SyncObject)
			{
				if (!JobManager.JobIdsForReuse.ContainsKey(instanceId))
				{
					JobManager.JobIdsForReuse.Add(instanceId, new KeyValuePair<int, string>(id, typeName));
				}
			}
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000B510C File Offset: 0x000B330C
		public Job2 NewJob(JobDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}
			JobSourceAdapter jobSourceAdapter = this.GetJobSourceAdapter(definition);
			Job2 result;
			try
			{
				result = jobSourceAdapter.NewJob(definition);
			}
			catch (Exception ex)
			{
				this.Tracer.TraceException(ex);
				CommandProcessorBase.CheckForSevereException(ex);
				throw;
			}
			return result;
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000B5160 File Offset: 0x000B3360
		public Job2 NewJob(JobInvocationInfo specification)
		{
			if (specification == null)
			{
				throw new ArgumentNullException("specification");
			}
			if (specification.Definition == null)
			{
				throw new ArgumentException(RemotingErrorIdStrings.NewJobSpecificationError, "specification");
			}
			JobSourceAdapter jobSourceAdapter = this.GetJobSourceAdapter(specification.Definition);
			Job2 result = null;
			try
			{
				result = jobSourceAdapter.NewJob(specification);
			}
			catch (Exception ex)
			{
				this.Tracer.TraceException(ex);
				CommandProcessorBase.CheckForSevereException(ex);
				throw;
			}
			return result;
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000B51D4 File Offset: 0x000B33D4
		public void PersistJob(Job2 job, JobDefinition definition)
		{
			if (job == null)
			{
				throw new PSArgumentNullException("job");
			}
			if (definition == null)
			{
				throw new PSArgumentNullException("definition");
			}
			JobSourceAdapter jobSourceAdapter = this.GetJobSourceAdapter(definition);
			try
			{
				jobSourceAdapter.PersistJob(job);
			}
			catch (Exception ex)
			{
				this.Tracer.TraceException(ex);
				CommandProcessorBase.CheckForSevereException(ex);
				throw;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000B5234 File Offset: 0x000B3434
		private JobSourceAdapter AssertAndReturnJobSourceAdapter(string adapterTypeName)
		{
			JobSourceAdapter result;
			lock (this._syncObject)
			{
				if (!this._sourceAdapters.TryGetValue(adapterTypeName, out result))
				{
					throw new InvalidOperationException(RemotingErrorIdStrings.JobSourceAdapterNotFound);
				}
			}
			return result;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x000B528C File Offset: 0x000B348C
		private JobSourceAdapter GetJobSourceAdapter(JobDefinition definition)
		{
			string text;
			if (!string.IsNullOrEmpty(definition.JobSourceAdapterTypeName))
			{
				text = definition.JobSourceAdapterTypeName;
			}
			else
			{
				if (!(definition.JobSourceAdapterType != null))
				{
					throw new InvalidOperationException(RemotingErrorIdStrings.JobSourceAdapterNotFound);
				}
				text = definition.JobSourceAdapterType.Name;
			}
			bool flag = false;
			JobSourceAdapter result;
			lock (this._syncObject)
			{
				flag = this._sourceAdapters.TryGetValue(text, out result);
			}
			if (!flag)
			{
				if (string.IsNullOrEmpty(definition.ModuleName))
				{
					throw new InvalidOperationException(RemotingErrorIdStrings.JobSourceAdapterNotFound);
				}
				Exception ex = null;
				try
				{
					InitialSessionState initialSessionState = InitialSessionState.CreateDefault2();
					initialSessionState.Commands.Clear();
					initialSessionState.Formats.Clear();
					initialSessionState.Commands.Add(new SessionStateCmdletEntry("Import-Module", typeof(ImportModuleCommand), null));
					using (PowerShell powerShell = PowerShell.Create(initialSessionState))
					{
						powerShell.AddCommand("Import-Module");
						powerShell.AddParameter("Name", definition.ModuleName);
						powerShell.Invoke();
						if (powerShell.ErrorBuffer.Count > 0)
						{
							ex = powerShell.ErrorBuffer[0].Exception;
						}
					}
				}
				catch (RuntimeException ex2)
				{
					ex = ex2;
				}
				catch (InvalidOperationException ex3)
				{
					ex = ex3;
				}
				catch (ScriptCallDepthException ex4)
				{
					ex = ex4;
				}
				catch (SecurityException ex5)
				{
					ex = ex5;
				}
				catch (ThreadAbortException ex6)
				{
					ex = ex6;
				}
				if (ex != null)
				{
					throw new InvalidOperationException(RemotingErrorIdStrings.JobSourceAdapterNotFound, ex);
				}
				result = this.AssertAndReturnJobSourceAdapter(text);
			}
			return result;
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000B5464 File Offset: 0x000B3664
		internal List<Job2> GetJobs(Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, string[] jobSourceAdapterTypes)
		{
			return this.GetFilteredJobs(null, JobManager.FilterType.None, cmdlet, writeErrorOnException, writeObject, false, jobSourceAdapterTypes);
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000B5474 File Offset: 0x000B3674
		internal List<Job2> GetJobsByName(string name, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse, string[] jobSourceAdapterTypes)
		{
			return this.GetFilteredJobs(name, JobManager.FilterType.Name, cmdlet, writeErrorOnException, writeObject, recurse, jobSourceAdapterTypes);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000B5486 File Offset: 0x000B3686
		internal List<Job2> GetJobsByCommand(string command, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse, string[] jobSourceAdapterTypes)
		{
			return this.GetFilteredJobs(command, JobManager.FilterType.Command, cmdlet, writeErrorOnException, writeObject, recurse, jobSourceAdapterTypes);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000B5498 File Offset: 0x000B3698
		internal List<Job2> GetJobsByState(JobState state, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse, string[] jobSourceAdapterTypes)
		{
			return this.GetFilteredJobs(state, JobManager.FilterType.State, cmdlet, writeErrorOnException, writeObject, recurse, jobSourceAdapterTypes);
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000B54AF File Offset: 0x000B36AF
		internal List<Job2> GetJobsByFilter(Dictionary<string, object> filter, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse)
		{
			return this.GetFilteredJobs(filter, JobManager.FilterType.Filter, cmdlet, writeErrorOnException, writeObject, recurse, null);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000B54C0 File Offset: 0x000B36C0
		internal bool IsJobFromAdapter(Guid id, string name)
		{
			lock (this._syncObject)
			{
				foreach (JobSourceAdapter jobSourceAdapter in this._sourceAdapters.Values)
				{
					if (jobSourceAdapter.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						return jobSourceAdapter.GetJobByInstanceId(id, false) != null;
					}
				}
			}
			return false;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000B5560 File Offset: 0x000B3760
		private List<Job2> GetFilteredJobs(object filter, JobManager.FilterType filterType, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse, string[] jobSourceAdapterTypes)
		{
			List<Job2> list = new List<Job2>();
			lock (this._syncObject)
			{
				foreach (JobSourceAdapter sourceAdapter in this._sourceAdapters.Values)
				{
					List<Job2> list2 = null;
					if (this.CheckTypeNames(sourceAdapter, jobSourceAdapterTypes))
					{
						try
						{
							list2 = JobManager.CallJobFilter(sourceAdapter, filter, filterType, recurse);
						}
						catch (Exception ex)
						{
							this.Tracer.TraceException(ex);
							CommandProcessorBase.CheckForSevereException(ex);
							JobManager.WriteErrorOrWarning(writeErrorOnException, cmdlet, ex, "JobSourceAdapterGetJobsError", sourceAdapter);
						}
						if (list2 != null)
						{
							list.AddRange(list2);
						}
					}
				}
			}
			if (writeObject)
			{
				foreach (Job2 sendToPipeline in list)
				{
					cmdlet.WriteObject(sendToPipeline);
				}
			}
			return list;
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x000B5680 File Offset: 0x000B3880
		private bool CheckTypeNames(JobSourceAdapter sourceAdapter, string[] jobSourceAdapterTypes)
		{
			if (jobSourceAdapterTypes == null || jobSourceAdapterTypes.Length == 0)
			{
				return true;
			}
			string adapterName = this.GetAdapterName(sourceAdapter);
			foreach (string pattern in jobSourceAdapterTypes)
			{
				WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				if (wildcardPattern.IsMatch(adapterName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000B56D3 File Offset: 0x000B38D3
		private string GetAdapterName(JobSourceAdapter sourceAdapter)
		{
			if (string.IsNullOrEmpty(sourceAdapter.Name))
			{
				return sourceAdapter.GetType().ToString();
			}
			return sourceAdapter.Name;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000B56F4 File Offset: 0x000B38F4
		private static List<Job2> CallJobFilter(JobSourceAdapter sourceAdapter, object filter, JobManager.FilterType filterType, bool recurse)
		{
			List<Job2> list = new List<Job2>();
			IList<Job2> list2;
			switch (filterType)
			{
			case JobManager.FilterType.Command:
				list2 = sourceAdapter.GetJobsByCommand((string)filter, recurse);
				goto IL_6B;
			case JobManager.FilterType.Filter:
				list2 = sourceAdapter.GetJobsByFilter((Dictionary<string, object>)filter, recurse);
				goto IL_6B;
			case JobManager.FilterType.Name:
				list2 = sourceAdapter.GetJobsByName((string)filter, recurse);
				goto IL_6B;
			case JobManager.FilterType.State:
				list2 = sourceAdapter.GetJobsByState((JobState)filter, recurse);
				goto IL_6B;
			}
			list2 = sourceAdapter.GetJobs();
			IL_6B:
			if (list2 != null)
			{
				list.AddRange(list2);
			}
			return list;
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000B5777 File Offset: 0x000B3977
		internal Job2 GetJobById(int id, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse)
		{
			return this.GetJobThroughId<int>(Guid.Empty, id, cmdlet, writeErrorOnException, writeObject, recurse);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000B578B File Offset: 0x000B398B
		internal Job2 GetJobByInstanceId(Guid instanceId, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse)
		{
			return this.GetJobThroughId<Guid>(instanceId, 0, cmdlet, writeErrorOnException, writeObject, recurse);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000B579C File Offset: 0x000B399C
		private Job2 GetJobThroughId<T>(Guid guid, int id, Cmdlet cmdlet, bool writeErrorOnException, bool writeObject, bool recurse)
		{
			Job2 job = null;
			lock (this._syncObject)
			{
				foreach (JobSourceAdapter jobSourceAdapter in this._sourceAdapters.Values)
				{
					try
					{
						if (typeof(T) == typeof(Guid))
						{
							job = jobSourceAdapter.GetJobByInstanceId(guid, recurse);
						}
						else if (typeof(T) == typeof(int))
						{
							job = jobSourceAdapter.GetJobBySessionId(id, recurse);
						}
					}
					catch (Exception ex)
					{
						this.Tracer.TraceException(ex);
						CommandProcessorBase.CheckForSevereException(ex);
						JobManager.WriteErrorOrWarning(writeErrorOnException, cmdlet, ex, "JobSourceAdapterGetJobByInstanceIdError", jobSourceAdapter);
					}
					if (job != null)
					{
						if (writeObject)
						{
							cmdlet.WriteObject(job);
						}
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000B58B8 File Offset: 0x000B3AB8
		internal List<Job2> GetJobToStart(string definitionName, string definitionPath, string definitionType, Cmdlet cmdlet, bool writeErrorOnException)
		{
			List<Job2> list = new List<Job2>();
			WildcardPattern wildcardPattern = (definitionType != null) ? new WildcardPattern(definitionType, WildcardOptions.IgnoreCase) : null;
			lock (this._syncObject)
			{
				foreach (JobSourceAdapter jobSourceAdapter in this._sourceAdapters.Values)
				{
					try
					{
						if (wildcardPattern != null)
						{
							string adapterName = this.GetAdapterName(jobSourceAdapter);
							if (!wildcardPattern.IsMatch(adapterName))
							{
								continue;
							}
						}
						Job2 job = jobSourceAdapter.NewJob(definitionName, definitionPath);
						if (job != null)
						{
							list.Add(job);
						}
						if (wildcardPattern != null)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						this.Tracer.TraceException(ex);
						CommandProcessorBase.CheckForSevereException(ex);
						JobManager.WriteErrorOrWarning(writeErrorOnException, cmdlet, ex, "JobSourceAdapterGetJobByInstanceIdError", jobSourceAdapter);
					}
				}
			}
			return list;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000B59B8 File Offset: 0x000B3BB8
		private static void WriteErrorOrWarning(bool writeErrorOnException, Cmdlet cmdlet, Exception exception, string identifier, JobSourceAdapter sourceAdapter)
		{
			try
			{
				if (writeErrorOnException)
				{
					cmdlet.WriteError(new ErrorRecord(exception, identifier, ErrorCategory.OpenError, sourceAdapter));
				}
				else
				{
					string text = string.Format(CultureInfo.CurrentCulture, RemotingErrorIdStrings.JobSourceAdapterError, new object[]
					{
						exception.Message,
						sourceAdapter.Name
					});
					cmdlet.WriteWarning(text);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000B5A24 File Offset: 0x000B3C24
		internal List<string> GetLoadedAdapterNames(string[] adapterTypeNames)
		{
			List<string> list = new List<string>();
			lock (this._syncObject)
			{
				foreach (JobSourceAdapter sourceAdapter in this._sourceAdapters.Values)
				{
					if (this.CheckTypeNames(sourceAdapter, adapterTypeNames))
					{
						list.Add(this.GetAdapterName(sourceAdapter));
					}
				}
			}
			return list;
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000B5AC0 File Offset: 0x000B3CC0
		internal void RemoveJob(int sessionJobId, Cmdlet cmdlet, bool writeErrorOnException)
		{
			Job2 jobById = this.GetJobById(sessionJobId, cmdlet, writeErrorOnException, false, false);
			this.RemoveJob(jobById, cmdlet, false, false);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000B5AE4 File Offset: 0x000B3CE4
		internal bool RemoveJob(Job2 job, Cmdlet cmdlet, bool writeErrorOnException, bool throwExceptions = false)
		{
			bool flag = false;
			lock (this._syncObject)
			{
				foreach (JobSourceAdapter jobSourceAdapter in this._sourceAdapters.Values)
				{
					Job2 job2 = null;
					try
					{
						job2 = jobSourceAdapter.GetJobByInstanceId(job.InstanceId, true);
					}
					catch (Exception ex)
					{
						this.Tracer.TraceException(ex);
						CommandProcessorBase.CheckForSevereException(ex);
						if (throwExceptions)
						{
							throw;
						}
						JobManager.WriteErrorOrWarning(writeErrorOnException, cmdlet, ex, "JobSourceAdapterGetJobError", jobSourceAdapter);
					}
					if (job2 != null)
					{
						flag = true;
						this.RemoveJobIdForReuse(job2);
						try
						{
							jobSourceAdapter.RemoveJob(job);
						}
						catch (Exception ex2)
						{
							this.Tracer.TraceException(ex2);
							CommandProcessorBase.CheckForSevereException(ex2);
							if (throwExceptions)
							{
								throw;
							}
							JobManager.WriteErrorOrWarning(writeErrorOnException, cmdlet, ex2, "JobSourceAdapterRemoveJobError", jobSourceAdapter);
						}
					}
				}
			}
			if (!flag && throwExceptions)
			{
				string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ItemNotFoundInRepository, new object[]
				{
					"Job repository",
					job.InstanceId.ToString()
				});
				throw new ArgumentException(message);
			}
			return flag;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000B5C4C File Offset: 0x000B3E4C
		private void RemoveJobIdForReuse(Job job)
		{
			this.RemoveJobIdForReuseHelper(new Hashtable
			{
				{
					job.Id,
					job.Id
				}
			}, job);
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000B5C84 File Offset: 0x000B3E84
		private void RemoveJobIdForReuseHelper(Hashtable duplicateDetector, Job job)
		{
			lock (JobManager.SyncObject)
			{
				if (JobManager.JobIdsForReuse.ContainsKey(job.InstanceId))
				{
					JobManager.JobIdsForReuse.Remove(job.InstanceId);
				}
			}
			foreach (Job job2 in job.ChildJobs)
			{
				if (!duplicateDetector.ContainsKey(job2.Id))
				{
					duplicateDetector.Add(job2.Id, job2.Id);
					this.RemoveJobIdForReuse(job2);
				}
			}
		}

		// Token: 0x04000DCA RID: 3530
		private readonly PowerShellTraceSource Tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04000DCB RID: 3531
		private readonly Dictionary<string, JobSourceAdapter> _sourceAdapters = new Dictionary<string, JobSourceAdapter>();

		// Token: 0x04000DCC RID: 3532
		private readonly object _syncObject = new object();

		// Token: 0x04000DCD RID: 3533
		private static readonly Dictionary<Guid, KeyValuePair<int, string>> JobIdsForReuse = new Dictionary<Guid, KeyValuePair<int, string>>();

		// Token: 0x04000DCE RID: 3534
		private static readonly object SyncObject = new object();

		// Token: 0x0200028E RID: 654
		private enum FilterType
		{
			// Token: 0x04000DD0 RID: 3536
			None,
			// Token: 0x04000DD1 RID: 3537
			Command,
			// Token: 0x04000DD2 RID: 3538
			Filter,
			// Token: 0x04000DD3 RID: 3539
			Name,
			// Token: 0x04000DD4 RID: 3540
			State
		}
	}
}
