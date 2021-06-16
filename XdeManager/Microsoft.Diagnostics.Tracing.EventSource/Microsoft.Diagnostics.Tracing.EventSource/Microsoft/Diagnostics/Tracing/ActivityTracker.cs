using System;
using System.Diagnostics;
using System.Security;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000002 RID: 2
	internal class ActivityTracker
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public void OnStart(string providerName, string activityName, int task, ref Guid activityId, ref Guid relatedActivityId, EventActivityOptions options)
		{
			if (this.m_current == null)
			{
				if (this.m_checkedForEnable)
				{
					return;
				}
				this.m_checkedForEnable = true;
				if (TplEtwProvider.Log.IsEnabled(EventLevel.Informational, (EventKeywords)128L))
				{
					this.Enable();
				}
				if (this.m_current == null)
				{
					return;
				}
			}
			ActivityTracker.ActivityInfo value = this.m_current.Value;
			string text = this.NormalizeActivityName(providerName, activityName, task);
			TplEtwProvider log = TplEtwProvider.Log;
			if (log.Debug)
			{
				log.DebugFacilityMessage("OnStartEnter", text);
				log.DebugFacilityMessage("OnStartEnterActivityState", ActivityTracker.ActivityInfo.LiveActivities(value));
			}
			if (value != null)
			{
				if (value.m_level >= 100)
				{
					activityId = Guid.Empty;
					relatedActivityId = Guid.Empty;
					if (log.Debug)
					{
						log.DebugFacilityMessage("OnStartRET", "Fail");
					}
					return;
				}
				if ((options & EventActivityOptions.Recursive) == EventActivityOptions.None)
				{
					ActivityTracker.ActivityInfo activityInfo = this.FindActiveActivity(text, value);
					if (activityInfo != null)
					{
						this.OnStop(providerName, activityName, task, ref activityId);
						value = this.m_current.Value;
					}
				}
			}
			long uniqueId;
			if (value == null)
			{
				uniqueId = Interlocked.Increment(ref ActivityTracker.m_nextId);
			}
			else
			{
				uniqueId = Interlocked.Increment(ref value.m_lastChildID);
			}
			relatedActivityId = EventSource.CurrentThreadActivityId;
			ActivityTracker.ActivityInfo activityInfo2 = new ActivityTracker.ActivityInfo(text, uniqueId, value, relatedActivityId, options);
			this.m_current.Value = activityInfo2;
			activityId = activityInfo2.ActivityId;
			if (log.Debug)
			{
				log.DebugFacilityMessage("OnStartRetActivityState", ActivityTracker.ActivityInfo.LiveActivities(activityInfo2));
				log.DebugFacilityMessage1("OnStartRet", activityId.ToString(), relatedActivityId.ToString());
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002258 File Offset: 0x00000458
		public void OnStop(string providerName, string activityName, int task, ref Guid activityId)
		{
			if (this.m_current == null)
			{
				return;
			}
			string text = this.NormalizeActivityName(providerName, activityName, task);
			TplEtwProvider log = TplEtwProvider.Log;
			if (log.Debug)
			{
				log.DebugFacilityMessage("OnStopEnter", text);
				log.DebugFacilityMessage("OnStopEnterActivityState", ActivityTracker.ActivityInfo.LiveActivities(this.m_current.Value));
			}
			ActivityTracker.ActivityInfo activityInfo;
			for (;;)
			{
				ActivityTracker.ActivityInfo value = this.m_current.Value;
				activityInfo = null;
				ActivityTracker.ActivityInfo activityInfo2 = this.FindActiveActivity(text, value);
				if (activityInfo2 == null)
				{
					break;
				}
				activityId = activityInfo2.ActivityId;
				ActivityTracker.ActivityInfo activityInfo3 = value;
				while (activityInfo3 != activityInfo2 && activityInfo3 != null)
				{
					if (activityInfo3.m_stopped != 0)
					{
						activityInfo3 = activityInfo3.m_creator;
					}
					else
					{
						if (activityInfo3.CanBeOrphan())
						{
							if (activityInfo == null)
							{
								activityInfo = activityInfo3;
							}
						}
						else
						{
							activityInfo3.m_stopped = 1;
						}
						activityInfo3 = activityInfo3.m_creator;
					}
				}
				if (Interlocked.CompareExchange(ref activityInfo2.m_stopped, 1, 0) == 0)
				{
					goto Block_9;
				}
			}
			activityId = Guid.Empty;
			if (log.Debug)
			{
				log.DebugFacilityMessage("OnStopRET", "Fail");
			}
			return;
			Block_9:
			if (activityInfo == null)
			{
				ActivityTracker.ActivityInfo activityInfo2;
				activityInfo = activityInfo2.m_creator;
			}
			this.m_current.Value = activityInfo;
			if (log.Debug)
			{
				log.DebugFacilityMessage("OnStopRetActivityState", ActivityTracker.ActivityInfo.LiveActivities(activityInfo));
				log.DebugFacilityMessage("OnStopRet", activityId.ToString());
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000239C File Offset: 0x0000059C
		[SecuritySafeCritical]
		public void Enable()
		{
			if (this.m_current == null)
			{
				try
				{
					this.m_current = new AsyncLocal<ActivityTracker.ActivityInfo>(new Action<AsyncLocalValueChangedArgs<ActivityTracker.ActivityInfo>>(this.ActivityChanging));
				}
				catch (NotImplementedException)
				{
					Debugger.Log(0, null, "Activity Enabled() called but AsyncLocals Not Supported (pre V4.6).  Ignoring Enable");
				}
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000023EC File Offset: 0x000005EC
		public static ActivityTracker Instance
		{
			get
			{
				return ActivityTracker.s_activityTrackerInstance;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000023F3 File Offset: 0x000005F3
		private Guid CurrentActivityId
		{
			get
			{
				return this.m_current.Value.ActivityId;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002408 File Offset: 0x00000608
		private ActivityTracker.ActivityInfo FindActiveActivity(string name, ActivityTracker.ActivityInfo startLocation)
		{
			for (ActivityTracker.ActivityInfo activityInfo = startLocation; activityInfo != null; activityInfo = activityInfo.m_creator)
			{
				if (name == activityInfo.m_name && activityInfo.m_stopped == 0)
				{
					return activityInfo;
				}
			}
			return null;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000243C File Offset: 0x0000063C
		private string NormalizeActivityName(string providerName, string activityName, int task)
		{
			checked
			{
				if (activityName.EndsWith("Start"))
				{
					activityName = activityName.Substring(0, activityName.Length - "Start".Length);
				}
				else if (activityName.EndsWith("Stop"))
				{
					activityName = activityName.Substring(0, activityName.Length - "Stop".Length);
				}
				else if (task != 0)
				{
					activityName = "task" + task.ToString();
				}
				return providerName + activityName;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024B8 File Offset: 0x000006B8
		private void ActivityChanging(AsyncLocalValueChangedArgs<ActivityTracker.ActivityInfo> args)
		{
			ActivityTracker.ActivityInfo activityInfo = args.CurrentValue;
			ActivityTracker.ActivityInfo previousValue = args.PreviousValue;
			if (previousValue != null && previousValue.m_creator == activityInfo && (activityInfo == null || previousValue.m_activityIdToRestore != activityInfo.ActivityId))
			{
				EventSource.SetCurrentThreadActivityId(previousValue.m_activityIdToRestore);
				return;
			}
			while (activityInfo != null)
			{
				if (activityInfo.m_stopped == 0)
				{
					EventSource.SetCurrentThreadActivityId(activityInfo.ActivityId);
					return;
				}
				activityInfo = activityInfo.m_creator;
			}
		}

		// Token: 0x04000001 RID: 1
		private const ushort MAX_ACTIVITY_DEPTH = 100;

		// Token: 0x04000002 RID: 2
		private AsyncLocal<ActivityTracker.ActivityInfo> m_current;

		// Token: 0x04000003 RID: 3
		private bool m_checkedForEnable;

		// Token: 0x04000004 RID: 4
		private static ActivityTracker s_activityTrackerInstance = new ActivityTracker();

		// Token: 0x04000005 RID: 5
		private static long m_nextId = 0L;

		// Token: 0x02000003 RID: 3
		private class ActivityInfo
		{
			// Token: 0x0600000B RID: 11 RVA: 0x0000253C File Offset: 0x0000073C
			public ActivityInfo(string name, long uniqueId, ActivityTracker.ActivityInfo creator, Guid activityIDToRestore, EventActivityOptions options)
			{
				this.m_name = name;
				this.m_eventOptions = options;
				this.m_creator = creator;
				this.m_uniqueId = uniqueId;
				this.m_level = ((creator != null) ? checked(creator.m_level + 1) : 0);
				this.m_activityIdToRestore = activityIDToRestore;
				this.CreateActivityPathGuid(out this.m_guid, out this.m_activityPathGuidOffset);
			}

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600000C RID: 12 RVA: 0x0000259A File Offset: 0x0000079A
			public Guid ActivityId
			{
				get
				{
					return this.m_guid;
				}
			}

			// Token: 0x0600000D RID: 13 RVA: 0x000025A2 File Offset: 0x000007A2
			public static string Path(ActivityTracker.ActivityInfo activityInfo)
			{
				if (activityInfo == null)
				{
					return "";
				}
				return ActivityTracker.ActivityInfo.Path(activityInfo.m_creator) + "/" + activityInfo.m_uniqueId;
			}

			// Token: 0x0600000E RID: 14 RVA: 0x000025D0 File Offset: 0x000007D0
			public override string ToString()
			{
				string text = "";
				if (this.m_stopped != 0)
				{
					text = ",DEAD";
				}
				return string.Concat(new string[]
				{
					this.m_name,
					"(",
					ActivityTracker.ActivityInfo.Path(this),
					text,
					")"
				});
			}

			// Token: 0x0600000F RID: 15 RVA: 0x00002624 File Offset: 0x00000824
			public static string LiveActivities(ActivityTracker.ActivityInfo list)
			{
				if (list == null)
				{
					return "";
				}
				return list.ToString() + ";" + ActivityTracker.ActivityInfo.LiveActivities(list.m_creator);
			}

			// Token: 0x06000010 RID: 16 RVA: 0x0000264A File Offset: 0x0000084A
			public bool CanBeOrphan()
			{
				return (this.m_eventOptions & EventActivityOptions.Detachable) != EventActivityOptions.None;
			}

			// Token: 0x06000011 RID: 17 RVA: 0x0000265C File Offset: 0x0000085C
			[SecuritySafeCritical]
			private unsafe void CreateActivityPathGuid(out Guid idRet, out int activityPathGuidOffset)
			{
				checked
				{
					fixed (Guid* ptr = &idRet)
					{
						int whereToAddId = 0;
						if (this.m_creator != null)
						{
							whereToAddId = this.m_creator.m_activityPathGuidOffset;
							idRet = this.m_creator.m_guid;
						}
						else
						{
							int domainID = Thread.GetDomainID();
							whereToAddId = ActivityTracker.ActivityInfo.AddIdToGuid(ptr, whereToAddId, (uint)domainID, false);
						}
						activityPathGuidOffset = ActivityTracker.ActivityInfo.AddIdToGuid(ptr, whereToAddId, (uint)this.m_uniqueId, false);
						if (12 < activityPathGuidOffset)
						{
							this.CreateOverflowGuid(ptr);
						}
					}
				}
			}

			// Token: 0x06000012 RID: 18 RVA: 0x000026CC File Offset: 0x000008CC
			[SecurityCritical]
			private unsafe void CreateOverflowGuid(Guid* outPtr)
			{
				for (ActivityTracker.ActivityInfo creator = this.m_creator; creator != null; creator = creator.m_creator)
				{
					if (creator.m_activityPathGuidOffset <= 10)
					{
						uint id = (uint)Interlocked.Increment(ref creator.m_lastChildID);
						*outPtr = creator.m_guid;
						int num = ActivityTracker.ActivityInfo.AddIdToGuid(outPtr, creator.m_activityPathGuidOffset, id, true);
						if (num <= 12)
						{
							return;
						}
					}
				}
			}

			// Token: 0x06000013 RID: 19 RVA: 0x00002724 File Offset: 0x00000924
			[SecurityCritical]
			private unsafe static int AddIdToGuid(Guid* outPtr, int whereToAddId, uint id, bool overflow = false)
			{
				byte* ptr = (byte*)outPtr;
				checked
				{
					byte* ptr2 = ptr + 12;
					ptr += whereToAddId;
					if (ptr2 == ptr)
					{
						return 13;
					}
					if (0U < id && id <= 10U && !overflow)
					{
						ActivityTracker.ActivityInfo.WriteNibble(ref ptr, ptr2, id);
					}
					else
					{
						uint num = 4U;
						if (id <= 255U)
						{
							num = 1U;
						}
						else if (id <= 65535U)
						{
							num = 2U;
						}
						else if (id <= 16777215U)
						{
							num = 3U;
						}
						if (overflow)
						{
							if (ptr2 == ptr + 2)
							{
								return 13;
							}
							ActivityTracker.ActivityInfo.WriteNibble(ref ptr, ptr2, 11U);
						}
						ActivityTracker.ActivityInfo.WriteNibble(ref ptr, ptr2, 12U + (num - 1U));
						if (ptr < ptr2 && *ptr != 0)
						{
							if (id < 4096U)
							{
								*ptr = (byte)(192U + (id >> 8));
								id &= 255U;
							}
							ptr++;
						}
						while (0U < num)
						{
							if (ptr2 == ptr)
							{
								ptr++;
								break;
							}
							*(ptr++) = (byte)id;
							id >>= 8;
							num -= 1U;
						}
					}
					*(int*)(unchecked(outPtr + 12 / sizeof(Guid))) = (int)(*(uint*)outPtr + *(uint*)(unchecked(outPtr + 4 / sizeof(Guid))) + *(uint*)(unchecked(outPtr + 8 / sizeof(Guid))) + 1503500717U);
					return (int)(unchecked((long)((byte*)ptr - (byte*)outPtr)));
				}
			}

			// Token: 0x06000014 RID: 20 RVA: 0x00002818 File Offset: 0x00000A18
			[SecurityCritical]
			private unsafe static void WriteNibble(ref byte* ptr, byte* endPtr, uint value)
			{
				checked
				{
					if (*ptr != 0)
					{
						byte* ptr2;
						ptr = (ptr2 = ptr) + unchecked((IntPtr)1);
						byte* ptr3 = ptr2;
						*ptr3 |= (byte)value;
						return;
					}
					*ptr = (byte)(value << 4);
				}
			}

			// Token: 0x04000006 RID: 6
			internal readonly string m_name;

			// Token: 0x04000007 RID: 7
			private readonly long m_uniqueId;

			// Token: 0x04000008 RID: 8
			internal readonly Guid m_guid;

			// Token: 0x04000009 RID: 9
			internal readonly int m_activityPathGuidOffset;

			// Token: 0x0400000A RID: 10
			internal readonly int m_level;

			// Token: 0x0400000B RID: 11
			internal readonly EventActivityOptions m_eventOptions;

			// Token: 0x0400000C RID: 12
			internal long m_lastChildID;

			// Token: 0x0400000D RID: 13
			internal int m_stopped;

			// Token: 0x0400000E RID: 14
			internal readonly ActivityTracker.ActivityInfo m_creator;

			// Token: 0x0400000F RID: 15
			internal readonly Guid m_activityIdToRestore;

			// Token: 0x02000004 RID: 4
			private enum NumberListCodes : byte
			{
				// Token: 0x04000011 RID: 17
				End,
				// Token: 0x04000012 RID: 18
				LastImmediateValue = 10,
				// Token: 0x04000013 RID: 19
				PrefixCode,
				// Token: 0x04000014 RID: 20
				MultiByte1
			}
		}
	}
}
