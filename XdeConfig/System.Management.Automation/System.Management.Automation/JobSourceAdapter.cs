using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000283 RID: 643
	public abstract class JobSourceAdapter
	{
		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000B0CD0 File Offset: 0x000AEED0
		// (set) Token: 0x06001E8C RID: 7820 RVA: 0x000B0CD8 File Offset: 0x000AEED8
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x000B0CE1 File Offset: 0x000AEEE1
		protected JobIdentifier RetrieveJobIdForReuse(Guid instanceId)
		{
			return JobManager.GetJobIdentifier(instanceId, base.GetType().Name);
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x000B0CF4 File Offset: 0x000AEEF4
		public void StoreJobIdForReuse(Job2 job, bool recurse)
		{
			if (job == null)
			{
				PSTraceSource.NewArgumentNullException("job", RemotingErrorIdStrings.JobSourceAdapterCannotSaveNullJob, new object[0]);
			}
			JobManager.SaveJobId(job.InstanceId, job.Id, base.GetType().Name);
			if (recurse && job.ChildJobs != null && job.ChildJobs.Count > 0)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add(job.InstanceId, job.InstanceId);
				foreach (Job job2 in job.ChildJobs)
				{
					Job2 job3 = job2 as Job2;
					if (job3 != null)
					{
						this.StoreJobIdForReuseHelper(hashtable, job3, true);
					}
				}
			}
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000B0DC0 File Offset: 0x000AEFC0
		private void StoreJobIdForReuseHelper(Hashtable duplicateDetector, Job2 job, bool recurse)
		{
			if (duplicateDetector.ContainsKey(job.InstanceId))
			{
				return;
			}
			duplicateDetector.Add(job.InstanceId, job.InstanceId);
			JobManager.SaveJobId(job.InstanceId, job.Id, base.GetType().Name);
			if (!recurse || job.ChildJobs == null)
			{
				return;
			}
			foreach (Job job2 in job.ChildJobs)
			{
				Job2 job3 = job2 as Job2;
				if (job3 != null)
				{
					this.StoreJobIdForReuseHelper(duplicateDetector, job3, recurse);
				}
			}
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000B0E74 File Offset: 0x000AF074
		public Job2 NewJob(JobDefinition definition)
		{
			return this.NewJob(new JobInvocationInfo(definition, new Dictionary<string, object>()));
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x000B0E87 File Offset: 0x000AF087
		public virtual Job2 NewJob(string definitionName, string definitionPath)
		{
			return null;
		}

		// Token: 0x06001E92 RID: 7826
		public abstract Job2 NewJob(JobInvocationInfo specification);

		// Token: 0x06001E93 RID: 7827
		public abstract IList<Job2> GetJobs();

		// Token: 0x06001E94 RID: 7828
		public abstract IList<Job2> GetJobsByName(string name, bool recurse);

		// Token: 0x06001E95 RID: 7829
		public abstract IList<Job2> GetJobsByCommand(string command, bool recurse);

		// Token: 0x06001E96 RID: 7830
		public abstract Job2 GetJobByInstanceId(Guid instanceId, bool recurse);

		// Token: 0x06001E97 RID: 7831
		public abstract Job2 GetJobBySessionId(int id, bool recurse);

		// Token: 0x06001E98 RID: 7832
		public abstract IList<Job2> GetJobsByState(JobState state, bool recurse);

		// Token: 0x06001E99 RID: 7833
		public abstract IList<Job2> GetJobsByFilter(Dictionary<string, object> filter, bool recurse);

		// Token: 0x06001E9A RID: 7834
		public abstract void RemoveJob(Job2 job);

		// Token: 0x06001E9B RID: 7835 RVA: 0x000B0E8A File Offset: 0x000AF08A
		public virtual void PersistJob(Job2 job)
		{
		}

		// Token: 0x04000D76 RID: 3446
		private string _name = string.Empty;
	}
}
