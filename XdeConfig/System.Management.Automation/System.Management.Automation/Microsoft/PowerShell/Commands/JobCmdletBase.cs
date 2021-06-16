using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Remoting;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200033F RID: 831
	public class JobCmdletBase : PSRemotingCmdlet
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x000E0474 File Offset: 0x000DE674
		internal List<Job> FindJobsMatchingByName(bool recurse, bool writeobject, bool writeErrorOnNoMatch, bool checkIfJobCanBeRemoved)
		{
			List<Job> list = new List<Job>();
			Hashtable hashtable = new Hashtable();
			if (this.names == null)
			{
				return list;
			}
			foreach (string text in this.names)
			{
				if (!string.IsNullOrEmpty(text))
				{
					bool flag = false;
					hashtable.Clear();
					flag = this.FindJobsMatchingByNameHelper(list, base.JobRepository.Jobs, text, hashtable, recurse, writeobject, checkIfJobCanBeRemoved);
					List<Job2> jobsByName = base.JobManager.GetJobsByName(text, this, false, writeobject, recurse, null);
					bool flag2 = jobsByName != null && jobsByName.Count > 0;
					if (flag2)
					{
						foreach (Job2 job in jobsByName)
						{
							if (this.CheckIfJob2CanBeRemoved(checkIfJobCanBeRemoved, "Name", job, RemotingErrorIdStrings.JobWithSpecifiedNameNotCompleted, new object[]
							{
								job.Id,
								job.Name
							}))
							{
								list.Add(job);
							}
						}
					}
					flag = (flag || flag2);
					if (!flag && writeErrorOnNoMatch && !WildcardPattern.ContainsWildcardCharacters(text))
					{
						Exception exception = PSTraceSource.NewArgumentException("Name", RemotingErrorIdStrings.JobWithSpecifiedNameNotFound, new object[]
						{
							text
						});
						base.WriteError(new ErrorRecord(exception, "JobWithSpecifiedNameNotFound", ErrorCategory.ObjectNotFound, text));
					}
				}
			}
			return list;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000E05E4 File Offset: 0x000DE7E4
		private bool CheckIfJob2CanBeRemoved(bool checkForRemove, string parameterName, Job2 job2, string resourceString, params object[] args)
		{
			if (!checkForRemove)
			{
				return true;
			}
			if (job2.IsFinishedState(job2.JobStateInfo.State))
			{
				return true;
			}
			string message = PSRemotingErrorInvariants.FormatResourceString(resourceString, args);
			Exception exception = new ArgumentException(message, parameterName);
			base.WriteError(new ErrorRecord(exception, "JobObjectNotFinishedCannotBeRemoved", ErrorCategory.InvalidOperation, job2));
			return false;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000E0634 File Offset: 0x000DE834
		private bool FindJobsMatchingByNameHelper(List<Job> matches, IList<Job> jobsToSearch, string name, Hashtable duplicateDetector, bool recurse, bool writeobject, bool checkIfJobCanBeRemoved)
		{
			bool result = false;
			WildcardPattern wildcardPattern = new WildcardPattern(name, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			foreach (Job job in jobsToSearch)
			{
				if (!duplicateDetector.ContainsKey(job.Id))
				{
					duplicateDetector.Add(job.Id, job.Id);
					if (wildcardPattern.IsMatch(job.Name))
					{
						result = true;
						if (!checkIfJobCanBeRemoved || this.CheckJobCanBeRemoved(job, "Name", RemotingErrorIdStrings.JobWithSpecifiedNameNotCompleted, new object[]
						{
							job.Id,
							job.Name
						}))
						{
							if (writeobject)
							{
								base.WriteObject(job);
							}
							else
							{
								matches.Add(job);
							}
						}
					}
					if (job.ChildJobs != null && job.ChildJobs.Count > 0 && recurse)
					{
						bool flag = this.FindJobsMatchingByNameHelper(matches, job.ChildJobs, name, duplicateDetector, recurse, writeobject, checkIfJobCanBeRemoved);
						if (flag)
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000E0754 File Offset: 0x000DE954
		internal List<Job> FindJobsMatchingByInstanceId(bool recurse, bool writeobject, bool writeErrorOnNoMatch, bool checkIfJobCanBeRemoved)
		{
			List<Job> list = new List<Job>();
			Hashtable hashtable = new Hashtable();
			if (this.instanceIds == null)
			{
				return list;
			}
			foreach (Guid guid in this.instanceIds)
			{
				hashtable.Clear();
				bool flag = this.FindJobsMatchingByInstanceIdHelper(list, base.JobRepository.Jobs, guid, hashtable, recurse, writeobject, checkIfJobCanBeRemoved);
				Job2 jobByInstanceId = base.JobManager.GetJobByInstanceId(guid, this, false, writeobject, recurse);
				bool flag2 = jobByInstanceId != null;
				if (flag2 && this.CheckIfJob2CanBeRemoved(checkIfJobCanBeRemoved, "InstanceId", jobByInstanceId, RemotingErrorIdStrings.JobWithSpecifiedInstanceIdNotCompleted, new object[]
				{
					jobByInstanceId.Id,
					jobByInstanceId.InstanceId
				}))
				{
					list.Add(jobByInstanceId);
				}
				if (!flag && !flag2 && writeErrorOnNoMatch)
				{
					Exception exception = PSTraceSource.NewArgumentException("InstanceId", RemotingErrorIdStrings.JobWithSpecifiedInstanceIdNotFound, new object[]
					{
						guid
					});
					base.WriteError(new ErrorRecord(exception, "JobWithSpecifiedInstanceIdNotFound", ErrorCategory.ObjectNotFound, guid));
				}
			}
			return list;
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000E0880 File Offset: 0x000DEA80
		private bool FindJobsMatchingByInstanceIdHelper(List<Job> matches, IList<Job> jobsToSearch, Guid instanceId, Hashtable duplicateDetector, bool recurse, bool writeobject, bool checkIfJobCanBeRemoved)
		{
			bool flag = false;
			foreach (Job job in jobsToSearch)
			{
				if (!duplicateDetector.ContainsKey(job.Id))
				{
					duplicateDetector.Add(job.Id, job.Id);
					if (job.InstanceId == instanceId)
					{
						flag = true;
						if (!checkIfJobCanBeRemoved || this.CheckJobCanBeRemoved(job, "InstanceId", RemotingErrorIdStrings.JobWithSpecifiedInstanceIdNotCompleted, new object[]
						{
							job.Id,
							job.InstanceId
						}))
						{
							if (writeobject)
							{
								base.WriteObject(job);
								break;
							}
							matches.Add(job);
							break;
						}
					}
				}
			}
			if (!flag && recurse)
			{
				foreach (Job job2 in jobsToSearch)
				{
					if (job2.ChildJobs != null && job2.ChildJobs.Count > 0)
					{
						flag = this.FindJobsMatchingByInstanceIdHelper(matches, job2.ChildJobs, instanceId, duplicateDetector, recurse, writeobject, checkIfJobCanBeRemoved);
						if (flag)
						{
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000E09D0 File Offset: 0x000DEBD0
		internal List<Job> FindJobsMatchingBySessionId(bool recurse, bool writeobject, bool writeErrorOnNoMatch, bool checkIfJobCanBeRemoved)
		{
			List<Job> list = new List<Job>();
			if (this.sessionIds == null)
			{
				return list;
			}
			Hashtable duplicateDetector = new Hashtable();
			foreach (int num in this.sessionIds)
			{
				bool flag = this.FindJobsMatchingBySessionIdHelper(list, base.JobRepository.Jobs, num, duplicateDetector, recurse, writeobject, checkIfJobCanBeRemoved);
				Job2 jobById = base.JobManager.GetJobById(num, this, false, writeobject, recurse);
				bool flag2 = jobById != null;
				if (flag2 && this.CheckIfJob2CanBeRemoved(checkIfJobCanBeRemoved, "SessionId", jobById, RemotingErrorIdStrings.JobWithSpecifiedSessionIdNotCompleted, new object[]
				{
					jobById.Id
				}))
				{
					list.Add(jobById);
				}
				if (!flag && !flag2 && writeErrorOnNoMatch)
				{
					Exception exception = PSTraceSource.NewArgumentException("SessionId", RemotingErrorIdStrings.JobWithSpecifiedSessionIdNotFound, new object[]
					{
						num
					});
					base.WriteError(new ErrorRecord(exception, "JobWithSpecifiedSessionNotFound", ErrorCategory.ObjectNotFound, num));
				}
			}
			return list;
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000E0ADC File Offset: 0x000DECDC
		private bool FindJobsMatchingBySessionIdHelper(List<Job> matches, IList<Job> jobsToSearch, int sessionId, Hashtable duplicateDetector, bool recurse, bool writeobject, bool checkIfJobCanBeRemoved)
		{
			bool flag = false;
			foreach (Job job in jobsToSearch)
			{
				if (job.Id == sessionId)
				{
					flag = true;
					if (!checkIfJobCanBeRemoved || this.CheckJobCanBeRemoved(job, "SessionId", RemotingErrorIdStrings.JobWithSpecifiedSessionIdNotCompleted, new object[]
					{
						job.Id
					}))
					{
						if (writeobject)
						{
							base.WriteObject(job);
							break;
						}
						matches.Add(job);
						break;
					}
				}
			}
			if (!flag && recurse)
			{
				foreach (Job job2 in jobsToSearch)
				{
					if (job2.ChildJobs != null && job2.ChildJobs.Count > 0)
					{
						flag = this.FindJobsMatchingBySessionIdHelper(matches, job2.ChildJobs, sessionId, duplicateDetector, recurse, writeobject, checkIfJobCanBeRemoved);
						if (flag)
						{
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000E0BE0 File Offset: 0x000DEDE0
		internal List<Job> FindJobsMatchingByCommand(bool writeobject)
		{
			List<Job> list = new List<Job>();
			if (this.commands == null)
			{
				return list;
			}
			List<Job> list2 = new List<Job>();
			list2.AddRange(base.JobRepository.Jobs);
			foreach (string text in this.commands)
			{
				List<Job2> jobsByCommand = base.JobManager.GetJobsByCommand(text, this, false, false, false, null);
				if (jobsByCommand != null)
				{
					foreach (Job2 item in jobsByCommand)
					{
						list2.Add(item);
					}
				}
				foreach (Job job in list2)
				{
					WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
					string text2 = job.Command.Trim();
					if (text2.Equals(text.Trim(), StringComparison.OrdinalIgnoreCase) || wildcardPattern.IsMatch(text2))
					{
						if (writeobject)
						{
							base.WriteObject(job);
						}
						else
						{
							list.Add(job);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000E0D14 File Offset: 0x000DEF14
		internal List<Job> FindJobsMatchingByState(bool writeobject)
		{
			List<Job> list = new List<Job>();
			List<Job> list2 = new List<Job>();
			list2.AddRange(base.JobRepository.Jobs);
			List<Job2> jobsByState = base.JobManager.GetJobsByState(this.jobstate, this, false, false, false, null);
			if (jobsByState != null)
			{
				foreach (Job2 item in jobsByState)
				{
					list2.Add(item);
				}
			}
			foreach (Job job in list2)
			{
				if (job.JobStateInfo.State == this.jobstate)
				{
					if (writeobject)
					{
						base.WriteObject(job);
					}
					else
					{
						list.Add(job);
					}
				}
			}
			return list;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000E0DFC File Offset: 0x000DEFFC
		internal List<Job> FindJobsMatchingByFilter(bool writeobject)
		{
			List<Job> list = new List<Job>();
			List<Job> list2 = new List<Job>();
			this.FindJobsMatchingByFilterHelper(list2, base.JobRepository.Jobs);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (object obj in this.filter.Keys)
			{
				string key = (string)obj;
				dictionary.Add(key, this.filter[key]);
			}
			List<Job2> jobsByFilter = base.JobManager.GetJobsByFilter(dictionary, this, false, false, true);
			if (jobsByFilter != null)
			{
				foreach (Job2 item in jobsByFilter)
				{
					list2.Add(item);
				}
			}
			foreach (Job job in list2)
			{
				if (writeobject)
				{
					base.WriteObject(job);
				}
				else
				{
					list.Add(job);
				}
			}
			return list;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000E0F38 File Offset: 0x000DF138
		private bool FindJobsMatchingByFilterHelper(List<Job> matches, List<Job> jobsToSearch)
		{
			return false;
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000E0F3C File Offset: 0x000DF13C
		internal List<Job> CopyJobsToList(Job[] jobs, bool writeobject, bool checkIfJobCanBeRemoved)
		{
			List<Job> list = new List<Job>();
			if (jobs == null)
			{
				return list;
			}
			foreach (Job job in jobs)
			{
				if (!checkIfJobCanBeRemoved || this.CheckJobCanBeRemoved(job, "Job", RemotingErrorIdStrings.JobWithSpecifiedSessionIdNotCompleted, new object[]
				{
					job.Id
				}))
				{
					if (writeobject)
					{
						base.WriteObject(job);
					}
					else
					{
						list.Add(job);
					}
				}
			}
			return list;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000E0FAC File Offset: 0x000DF1AC
		private bool CheckJobCanBeRemoved(Job job, string parameterName, string resourceString, params object[] list)
		{
			if (job.IsFinishedState(job.JobStateInfo.State))
			{
				return true;
			}
			string message = PSRemotingErrorInvariants.FormatResourceString(resourceString, list);
			Exception exception = new ArgumentException(message, parameterName);
			base.WriteError(new ErrorRecord(exception, "JobObjectNotFinishedCannotBeRemoved", ErrorCategory.InvalidOperation, job));
			return false;
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x000E0FF3 File Offset: 0x000DF1F3
		// (set) Token: 0x06002856 RID: 10326 RVA: 0x000E0FFB File Offset: 0x000DF1FB
		[Parameter(ValueFromPipelineByPropertyName = true, Position = 0, Mandatory = true, ParameterSetName = "NameParameterSet")]
		[ValidateNotNullOrEmpty]
		public string[] Name
		{
			get
			{
				return this.names;
			}
			set
			{
				this.names = value;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x000E1004 File Offset: 0x000DF204
		// (set) Token: 0x06002858 RID: 10328 RVA: 0x000E100C File Offset: 0x000DF20C
		[ValidateNotNullOrEmpty]
		[Parameter(ValueFromPipelineByPropertyName = true, Position = 0, Mandatory = true, ParameterSetName = "InstanceIdParameterSet")]
		public Guid[] InstanceId
		{
			get
			{
				return this.instanceIds;
			}
			set
			{
				this.instanceIds = value;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x000E1015 File Offset: 0x000DF215
		// (set) Token: 0x0600285A RID: 10330 RVA: 0x000E101D File Offset: 0x000DF21D
		[Parameter(ValueFromPipelineByPropertyName = true, Position = 0, Mandatory = true, ParameterSetName = "SessionIdParameterSet")]
		[ValidateNotNullOrEmpty]
		public virtual int[] Id
		{
			get
			{
				return this.sessionIds;
			}
			set
			{
				this.sessionIds = value;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x000E1026 File Offset: 0x000DF226
		// (set) Token: 0x0600285C RID: 10332 RVA: 0x000E102E File Offset: 0x000DF22E
		[Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "StateParameterSet")]
		public virtual JobState State
		{
			get
			{
				return this.jobstate;
			}
			set
			{
				this.jobstate = value;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x0600285D RID: 10333 RVA: 0x000E1037 File Offset: 0x000DF237
		// (set) Token: 0x0600285E RID: 10334 RVA: 0x000E103F File Offset: 0x000DF23F
		[ValidateNotNullOrEmpty]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "CommandParameterSet")]
		public virtual string[] Command
		{
			get
			{
				return this.commands;
			}
			set
			{
				this.commands = value;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x000E1048 File Offset: 0x000DF248
		// (set) Token: 0x06002860 RID: 10336 RVA: 0x000E1050 File Offset: 0x000DF250
		[Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "FilterParameterSet")]
		[ValidateNotNullOrEmpty]
		public virtual Hashtable Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
			}
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000E1059 File Offset: 0x000DF259
		protected override void BeginProcessing()
		{
			CommandDiscovery.AutoloadModulesWithJobSourceAdapters(base.Context, base.CommandOrigin);
		}

		// Token: 0x040013D8 RID: 5080
		internal const string JobParameterSet = "JobParameterSet";

		// Token: 0x040013D9 RID: 5081
		internal const string InstanceIdParameterSet = "InstanceIdParameterSet";

		// Token: 0x040013DA RID: 5082
		internal const string SessionIdParameterSet = "SessionIdParameterSet";

		// Token: 0x040013DB RID: 5083
		internal const string NameParameterSet = "NameParameterSet";

		// Token: 0x040013DC RID: 5084
		internal const string StateParameterSet = "StateParameterSet";

		// Token: 0x040013DD RID: 5085
		internal const string CommandParameterSet = "CommandParameterSet";

		// Token: 0x040013DE RID: 5086
		internal const string FilterParameterSet = "FilterParameterSet";

		// Token: 0x040013DF RID: 5087
		internal const string JobParameter = "Job";

		// Token: 0x040013E0 RID: 5088
		internal const string InstanceIdParameter = "InstanceId";

		// Token: 0x040013E1 RID: 5089
		internal const string SessionIdParameter = "SessionId";

		// Token: 0x040013E2 RID: 5090
		internal const string NameParameter = "Name";

		// Token: 0x040013E3 RID: 5091
		internal const string StateParameter = "State";

		// Token: 0x040013E4 RID: 5092
		internal const string CommandParameter = "Command";

		// Token: 0x040013E5 RID: 5093
		internal const string FilterParameter = "Filter";

		// Token: 0x040013E6 RID: 5094
		private string[] names;

		// Token: 0x040013E7 RID: 5095
		private Guid[] instanceIds;

		// Token: 0x040013E8 RID: 5096
		private int[] sessionIds;

		// Token: 0x040013E9 RID: 5097
		private JobState jobstate;

		// Token: 0x040013EA RID: 5098
		private string[] commands;

		// Token: 0x040013EB RID: 5099
		private Hashtable filter;
	}
}
