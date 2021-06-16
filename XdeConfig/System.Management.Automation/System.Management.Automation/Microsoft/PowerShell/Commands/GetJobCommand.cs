using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000340 RID: 832
	[Cmdlet("Get", "Job", DefaultParameterSetName = "SessionIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113328")]
	[OutputType(new Type[]
	{
		typeof(Job)
	})]
	public class GetJobCommand : JobCmdletBase
	{
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002863 RID: 10339 RVA: 0x000E1074 File Offset: 0x000DF274
		// (set) Token: 0x06002864 RID: 10340 RVA: 0x000E107C File Offset: 0x000DF27C
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		[Parameter(ParameterSetName = "CommandParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		public SwitchParameter IncludeChildJob
		{
			get
			{
				return this._includeChildJob;
			}
			set
			{
				this._includeChildJob = value;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002865 RID: 10341 RVA: 0x000E1085 File Offset: 0x000DF285
		// (set) Token: 0x06002866 RID: 10342 RVA: 0x000E108D File Offset: 0x000DF28D
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "CommandParameterSet")]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		public JobState ChildJobState
		{
			get
			{
				return this._childJobState;
			}
			set
			{
				this._childJobState = value;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x000E1096 File Offset: 0x000DF296
		// (set) Token: 0x06002868 RID: 10344 RVA: 0x000E109E File Offset: 0x000DF29E
		[Parameter(ParameterSetName = "CommandParameterSet")]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		public bool HasMoreData
		{
			get
			{
				return this._hasMoreData;
			}
			set
			{
				this._hasMoreData = value;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000E10A7 File Offset: 0x000DF2A7
		// (set) Token: 0x0600286A RID: 10346 RVA: 0x000E10AF File Offset: 0x000DF2AF
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "CommandParameterSet")]
		public DateTime Before
		{
			get
			{
				return this._beforeTime;
			}
			set
			{
				this._beforeTime = value;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x000E10B8 File Offset: 0x000DF2B8
		// (set) Token: 0x0600286C RID: 10348 RVA: 0x000E10C0 File Offset: 0x000DF2C0
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		[Parameter(ParameterSetName = "CommandParameterSet")]
		public DateTime After
		{
			get
			{
				return this._afterTime;
			}
			set
			{
				this._afterTime = value;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000E10C9 File Offset: 0x000DF2C9
		// (set) Token: 0x0600286E RID: 10350 RVA: 0x000E10D1 File Offset: 0x000DF2D1
		[Parameter(ParameterSetName = "SessionIdParameterSet")]
		[Parameter(ParameterSetName = "CommandParameterSet")]
		[Parameter(ParameterSetName = "InstanceIdParameterSet")]
		[Parameter(ParameterSetName = "NameParameterSet")]
		[Parameter(ParameterSetName = "StateParameterSet")]
		public int Newest
		{
			get
			{
				return this._newestCount;
			}
			set
			{
				this._newestCount = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x000E10DA File Offset: 0x000DF2DA
		// (set) Token: 0x06002870 RID: 10352 RVA: 0x000E10E2 File Offset: 0x000DF2E2
		[ValidateNotNullOrEmpty]
		[Parameter(ValueFromPipelineByPropertyName = true, Position = 0, ParameterSetName = "SessionIdParameterSet")]
		public override int[] Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				base.Id = value;
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000E1118 File Offset: 0x000DF318
		protected override void ProcessRecord()
		{
			List<Job> list = this.FindJobs();
			list.Sort(delegate(Job x, Job y)
			{
				if (x == null)
				{
					return -1;
				}
				return x.Id.CompareTo((y != null) ? y.Id : 1);
			});
			base.WriteObject(list, true);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000E1158 File Offset: 0x000DF358
		protected List<Job> FindJobs()
		{
			List<Job> list = new List<Job>();
			string parameterSetName;
			switch (parameterSetName = base.ParameterSetName)
			{
			case "NameParameterSet":
				list.AddRange(base.FindJobsMatchingByName(true, false, true, false));
				break;
			case "InstanceIdParameterSet":
				list.AddRange(base.FindJobsMatchingByInstanceId(true, false, true, false));
				break;
			case "SessionIdParameterSet":
				if (this.Id != null)
				{
					list.AddRange(base.FindJobsMatchingBySessionId(true, false, true, false));
				}
				else
				{
					list.AddRange(base.JobRepository.Jobs);
					list.AddRange(base.JobManager.GetJobs(this, true, false, null));
				}
				break;
			case "CommandParameterSet":
				list.AddRange(base.FindJobsMatchingByCommand(false));
				break;
			case "StateParameterSet":
				list.AddRange(base.FindJobsMatchingByState(false));
				break;
			case "FilterParameterSet":
				list.AddRange(base.FindJobsMatchingByFilter(false));
				break;
			}
			list.AddRange(this.FindChildJobs(list));
			list = this.ApplyHasMoreDataFiltering(list);
			return this.ApplyTimeFiltering(list);
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000E12BC File Offset: 0x000DF4BC
		private List<Job> ApplyHasMoreDataFiltering(List<Job> jobList)
		{
			if (!base.MyInvocation.BoundParameters.ContainsKey("HasMoreData"))
			{
				return jobList;
			}
			List<Job> list = new List<Job>();
			foreach (Job job in jobList)
			{
				if (job.HasMoreData == this._hasMoreData)
				{
					list.Add(job);
				}
			}
			return list;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000E133C File Offset: 0x000DF53C
		private List<Job> FindChildJobs(List<Job> jobList)
		{
			bool flag = base.MyInvocation.BoundParameters.ContainsKey("ChildJobState");
			bool flag2 = base.MyInvocation.BoundParameters.ContainsKey("IncludeChildJob");
			List<Job> list = new List<Job>();
			if (!flag && !flag2)
			{
				return list;
			}
			if (!flag && flag2)
			{
				using (List<Job>.Enumerator enumerator = jobList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Job job = enumerator.Current;
						if (job.ChildJobs != null && job.ChildJobs.Count > 0)
						{
							list.AddRange(job.ChildJobs);
						}
					}
					return list;
				}
			}
			foreach (Job job2 in jobList)
			{
				foreach (Job job3 in job2.ChildJobs)
				{
					if (job3.JobStateInfo.State == this._childJobState)
					{
						list.Add(job3);
					}
				}
			}
			return list;
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000E14FC File Offset: 0x000DF6FC
		private List<Job> ApplyTimeFiltering(List<Job> jobList)
		{
			bool flag = base.MyInvocation.BoundParameters.ContainsKey("Before");
			bool flag2 = base.MyInvocation.BoundParameters.ContainsKey("After");
			bool flag3 = base.MyInvocation.BoundParameters.ContainsKey("Newest");
			if (!flag && !flag2 && !flag3)
			{
				return jobList;
			}
			List<Job> list;
			if (flag || flag2)
			{
				list = new List<Job>();
				using (List<Job>.Enumerator enumerator = jobList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Job job = enumerator.Current;
						if (!(job.PSEndTime == DateTime.MinValue))
						{
							if (flag && flag2)
							{
								if (job.PSEndTime < this._beforeTime && job.PSEndTime > this._afterTime)
								{
									list.Add(job);
								}
							}
							else if ((flag && job.PSEndTime < this._beforeTime) || (flag2 && job.PSEndTime > this._afterTime))
							{
								list.Add(job);
							}
						}
					}
					goto IL_194;
				}
			}
			list = jobList;
			IL_194:
			if (!flag3 || list.Count == 0)
			{
				return list;
			}
			list.Sort(delegate(Job firstJob, Job secondJob)
			{
				if (firstJob.PSEndTime > secondJob.PSEndTime)
				{
					return -1;
				}
				if (firstJob.PSEndTime < secondJob.PSEndTime)
				{
					return 1;
				}
				return 0;
			});
			List<Job> list2 = new List<Job>();
			int num = 0;
			foreach (Job item in list)
			{
				if (++num > this._newestCount)
				{
					break;
				}
				if (!list2.Contains(item))
				{
					list2.Add(item);
				}
			}
			return list2;
		}

		// Token: 0x040013EC RID: 5100
		private SwitchParameter _includeChildJob;

		// Token: 0x040013ED RID: 5101
		private JobState _childJobState;

		// Token: 0x040013EE RID: 5102
		private bool _hasMoreData;

		// Token: 0x040013EF RID: 5103
		private DateTime _beforeTime;

		// Token: 0x040013F0 RID: 5104
		private DateTime _afterTime;

		// Token: 0x040013F1 RID: 5105
		private int _newestCount;
	}
}
