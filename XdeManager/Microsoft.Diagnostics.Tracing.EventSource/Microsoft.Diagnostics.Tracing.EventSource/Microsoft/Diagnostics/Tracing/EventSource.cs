using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Diagnostics.Tracing.Internal;
using Microsoft.Reflection;
using Microsoft.Win32;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000005 RID: 5
	public class EventSource : IDisposable
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002843 File Offset: 0x00000A43
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000284B File Offset: 0x00000A4B
		public Guid Guid
		{
			get
			{
				return this.m_guid;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002853 File Offset: 0x00000A53
		public bool IsEnabled()
		{
			return this.m_eventSourceEnabled;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000285B File Offset: 0x00000A5B
		public bool IsEnabled(EventLevel level, EventKeywords keywords)
		{
			return this.IsEnabled(level, keywords, EventChannel.None);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002866 File Offset: 0x00000A66
		public bool IsEnabled(EventLevel level, EventKeywords keywords, EventChannel channel)
		{
			return this.m_eventSourceEnabled && this.IsEnabledCommon(this.m_eventSourceEnabled, this.m_level, this.m_matchAnyKeyword, level, keywords, channel);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002892 File Offset: 0x00000A92
		public EventSourceSettings Settings
		{
			get
			{
				return this.m_config;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000289C File Offset: 0x00000A9C
		public static Guid GetGuid(Type eventSourceType)
		{
			if (eventSourceType == null)
			{
				throw new ArgumentNullException("eventSourceType");
			}
			EventSourceAttribute eventSourceAttribute = (EventSourceAttribute)EventSource.GetCustomAttributeHelper(eventSourceType, typeof(EventSourceAttribute), EventManifestOptions.None);
			string name = eventSourceType.Name;
			if (eventSourceAttribute != null)
			{
				if (eventSourceAttribute.Guid != null)
				{
					Guid empty = Guid.Empty;
					if (Guid.TryParse(eventSourceAttribute.Guid, out empty))
					{
						return empty;
					}
				}
				if (eventSourceAttribute.Name != null)
				{
					name = eventSourceAttribute.Name;
				}
			}
			if (name == null)
			{
				throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("Argument_InvalidTypeName", new object[0]), "eventSourceType");
			}
			return EventSource.GenerateGuidFromName(name.ToUpperInvariant());
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002935 File Offset: 0x00000B35
		public static string GetName(Type eventSourceType)
		{
			return EventSource.GetName(eventSourceType, EventManifestOptions.None);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000293E File Offset: 0x00000B3E
		public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest)
		{
			return EventSource.GenerateManifest(eventSourceType, assemblyPathToIncludeInManifest, EventManifestOptions.None);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002948 File Offset: 0x00000B48
		public static string GenerateManifest(Type eventSourceType, string assemblyPathToIncludeInManifest, EventManifestOptions flags)
		{
			if (eventSourceType == null)
			{
				throw new ArgumentNullException("eventSourceType");
			}
			byte[] array = EventSource.CreateManifestAndDescriptors(eventSourceType, assemblyPathToIncludeInManifest, null, flags);
			if (array != null)
			{
				return Encoding.UTF8.GetString(array, 0, array.Length);
			}
			return null;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002988 File Offset: 0x00000B88
		public static IEnumerable<EventSource> GetSources()
		{
			List<EventSource> list = new List<EventSource>();
			lock (EventListener.EventListenersLock)
			{
				foreach (WeakReference weakReference in EventListener.s_EventSources)
				{
					EventSource eventSource = weakReference.Target as EventSource;
					if (eventSource != null && !eventSource.IsDisposed)
					{
						list.Add(eventSource);
					}
				}
			}
			return list;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A24 File Offset: 0x00000C24
		public static void SendCommand(EventSource eventSource, EventCommand command, IDictionary<string, string> commandArguments)
		{
			if (eventSource == null)
			{
				throw new ArgumentNullException("eventSource");
			}
			if (command <= EventCommand.Update && command != EventCommand.SendManifest)
			{
				throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_InvalidCommand", new object[0]), "command");
			}
			eventSource.SendCommand(null, 0, 0, command, true, EventLevel.LogAlways, EventKeywords.None, commandArguments);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002A71 File Offset: 0x00000C71
		[SecuritySafeCritical]
		public static void SetCurrentThreadActivityId(Guid activityId)
		{
			if (TplEtwProvider.Log != null)
			{
				TplEtwProvider.Log.SetActivityId(activityId);
			}
			UnsafeNativeMethods.ManifestEtw.EventActivityIdControl(UnsafeNativeMethods.ManifestEtw.ActivityControl.EVENT_ACTIVITY_CTRL_GET_SET_ID, ref activityId);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002A8E File Offset: 0x00000C8E
		[SecuritySafeCritical]
		public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue)
		{
			oldActivityThatWillContinue = activityId;
			UnsafeNativeMethods.ManifestEtw.EventActivityIdControl(UnsafeNativeMethods.ManifestEtw.ActivityControl.EVENT_ACTIVITY_CTRL_GET_SET_ID, ref oldActivityThatWillContinue);
			if (TplEtwProvider.Log != null)
			{
				TplEtwProvider.Log.SetActivityId(activityId);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public static Guid CurrentThreadActivityId
		{
			[SecuritySafeCritical]
			get
			{
				Guid result = default(Guid);
				UnsafeNativeMethods.ManifestEtw.EventActivityIdControl(UnsafeNativeMethods.ManifestEtw.ActivityControl.EVENT_ACTIVITY_CTRL_GET_ID, ref result);
				return result;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002AD3 File Offset: 0x00000CD3
		public Exception ConstructionException
		{
			get
			{
				return this.m_constructionException;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002ADC File Offset: 0x00000CDC
		public string GetTrait(string key)
		{
			checked
			{
				if (this.m_traits != null)
				{
					for (int i = 0; i < this.m_traits.Length - 1; i += 2)
					{
						if (this.m_traits[i] == key)
						{
							return this.m_traits[i + 1];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B24 File Offset: 0x00000D24
		public override string ToString()
		{
			return Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ToString", new object[]
			{
				this.Name,
				this.Guid
			});
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000027 RID: 39 RVA: 0x00002B5C File Offset: 0x00000D5C
		// (remove) Token: 0x06000028 RID: 40 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public event EventHandler<EventCommandEventArgs> EventCommandExecuted
		{
			add
			{
				lock (this)
				{
					this.m_eventCommandExecuted = (EventHandler<EventCommandEventArgs>)Delegate.Combine(this.m_eventCommandExecuted, value);
				}
				for (EventCommandEventArgs eventCommandEventArgs = this.m_deferredCommands; eventCommandEventArgs != null; eventCommandEventArgs = eventCommandEventArgs.nextCommand)
				{
					value(this, eventCommandEventArgs);
				}
			}
			remove
			{
				lock (this)
				{
					this.m_eventCommandExecuted = (EventHandler<EventCommandEventArgs>)Delegate.Remove(this.m_eventCommandExecuted, value);
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002C10 File Offset: 0x00000E10
		protected EventSource() : this(EventSourceSettings.EtwManifestEventFormat)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C19 File Offset: 0x00000E19
		protected EventSource(bool throwOnEventWriteErrors) : this(EventSourceSettings.EtwManifestEventFormat | (throwOnEventWriteErrors ? EventSourceSettings.ThrowOnEventWriteErrors : EventSourceSettings.Default))
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002C2A File Offset: 0x00000E2A
		protected EventSource(EventSourceSettings settings) : this(settings, null)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C34 File Offset: 0x00000E34
		protected EventSource(EventSourceSettings settings, params string[] traits)
		{
			this.m_config = this.ValidateSettings(settings);
			Type type = base.GetType();
			this.Initialize(EventSource.GetGuid(type), EventSource.GetName(type), traits);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C6E File Offset: 0x00000E6E
		protected virtual void OnEventCommand(EventCommandEventArgs command)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C70 File Offset: 0x00000E70
		[SecuritySafeCritical]
		protected void WriteEvent(int eventId)
		{
			this.WriteEventCore(eventId, 0, null);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C7C File Offset: 0x00000E7C
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, int arg1)
		{
			if (this.m_eventSourceEnabled)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(EventSource.EventData))];
				ptr->DataPointer = (IntPtr)((void*)(&arg1));
				ptr->Size = 4;
				this.WriteEventCore(eventId, 1, ptr);
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CBC File Offset: 0x00000EBC
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, int arg1, int arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData))];
				ptr->DataPointer = (IntPtr)((void*)(&arg1));
				ptr->Size = 4;
				ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
				ptr[1].Size = 4;
				this.WriteEventCore(eventId, 2, ptr);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002D20 File Offset: 0x00000F20
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, int arg1, int arg2, int arg3)
		{
			if (this.m_eventSourceEnabled)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(EventSource.EventData))];
				ptr->DataPointer = (IntPtr)((void*)(&arg1));
				ptr->Size = 4;
				ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
				ptr[1].Size = 4;
				ptr[2].DataPointer = (IntPtr)((void*)(&arg3));
				ptr[2].Size = 4;
				this.WriteEventCore(eventId, 3, ptr);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002DAC File Offset: 0x00000FAC
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, long arg1)
		{
			if (this.m_eventSourceEnabled)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(EventSource.EventData))];
				ptr->DataPointer = (IntPtr)((void*)(&arg1));
				ptr->Size = 8;
				this.WriteEventCore(eventId, 1, ptr);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002DEC File Offset: 0x00000FEC
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, long arg1, long arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData))];
				ptr->DataPointer = (IntPtr)((void*)(&arg1));
				ptr->Size = 8;
				ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
				ptr[1].Size = 8;
				this.WriteEventCore(eventId, 2, ptr);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002E50 File Offset: 0x00001050
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, long arg1, long arg2, long arg3)
		{
			if (this.m_eventSourceEnabled)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(EventSource.EventData))];
				ptr->DataPointer = (IntPtr)((void*)(&arg1));
				ptr->Size = 8;
				ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
				ptr[1].Size = 8;
				ptr[2].DataPointer = (IntPtr)((void*)(&arg3));
				ptr[2].Size = 8;
				this.WriteEventCore(eventId, 3, ptr);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002EDC File Offset: 0x000010DC
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, string arg1)
		{
			checked
			{
				if (this.m_eventSourceEnabled)
				{
					if (arg1 == null)
					{
						arg1 = "";
					}
					fixed (char* value = arg1)
					{
						EventSource.EventData* ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)1) * (UIntPtr)sizeof(EventSource.EventData)];
						ptr->DataPointer = (IntPtr)((void*)value);
						ptr->Size = (arg1.Length + 1) * 2;
						this.WriteEventCore(eventId, 1, ptr);
					}
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F3C File Offset: 0x0000113C
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, string arg1, string arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg1 == null)
				{
					arg1 = "";
				}
				if (arg2 == null)
				{
					arg2 = "";
				}
				fixed (char* value = arg1)
				{
					fixed (char* value2 = arg2)
					{
						EventSource.EventData* ptr;
						checked
						{
							ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData)];
							ptr->DataPointer = (IntPtr)((void*)value);
							ptr->Size = (arg1.Length + 1) * 2;
						}
						ptr[1].DataPointer = (IntPtr)((void*)value2);
						ptr[1].Size = checked((arg2.Length + 1) * 2);
						this.WriteEventCore(eventId, 2, ptr);
					}
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002FE8 File Offset: 0x000011E8
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, string arg1, string arg2, string arg3)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg1 == null)
				{
					arg1 = "";
				}
				if (arg2 == null)
				{
					arg2 = "";
				}
				if (arg3 == null)
				{
					arg3 = "";
				}
				fixed (char* value = arg1)
				{
					fixed (char* value2 = arg2)
					{
						fixed (char* value3 = arg3)
						{
							EventSource.EventData* ptr;
							checked
							{
								ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)3) * (UIntPtr)sizeof(EventSource.EventData)];
								ptr->DataPointer = (IntPtr)((void*)value);
								ptr->Size = (arg1.Length + 1) * 2;
							}
							ptr[1].DataPointer = (IntPtr)((void*)value2);
							ptr[1].Size = checked((arg2.Length + 1) * 2);
							ptr[2].DataPointer = (IntPtr)((void*)value3);
							ptr[2].Size = checked((arg3.Length + 1) * 2);
							this.WriteEventCore(eventId, 3, ptr);
						}
					}
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000030E8 File Offset: 0x000012E8
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, string arg1, int arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg1 == null)
				{
					arg1 = "";
				}
				fixed (char* value = arg1)
				{
					EventSource.EventData* ptr;
					checked
					{
						ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData)];
						ptr->DataPointer = (IntPtr)((void*)value);
						ptr->Size = (arg1.Length + 1) * 2;
					}
					ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
					ptr[1].Size = 4;
					this.WriteEventCore(eventId, 2, ptr);
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000316C File Offset: 0x0000136C
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, string arg1, int arg2, int arg3)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg1 == null)
				{
					arg1 = "";
				}
				fixed (char* value = arg1)
				{
					EventSource.EventData* ptr;
					checked
					{
						ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)3) * (UIntPtr)sizeof(EventSource.EventData)];
						ptr->DataPointer = (IntPtr)((void*)value);
						ptr->Size = (arg1.Length + 1) * 2;
					}
					ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
					ptr[1].Size = 4;
					ptr[2].DataPointer = (IntPtr)((void*)(&arg3));
					ptr[2].Size = 4;
					this.WriteEventCore(eventId, 3, ptr);
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000321C File Offset: 0x0000141C
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, string arg1, long arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg1 == null)
				{
					arg1 = "";
				}
				fixed (char* value = arg1)
				{
					EventSource.EventData* ptr;
					checked
					{
						ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData)];
						ptr->DataPointer = (IntPtr)((void*)value);
						ptr->Size = (arg1.Length + 1) * 2;
					}
					ptr[1].DataPointer = (IntPtr)((void*)(&arg2));
					ptr[1].Size = 8;
					this.WriteEventCore(eventId, 2, ptr);
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000032A0 File Offset: 0x000014A0
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, long arg1, string arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg2 == null)
				{
					arg2 = "";
				}
				fixed (char* value = arg2)
				{
					EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData))];
					ptr->DataPointer = (IntPtr)((void*)(&arg1));
					ptr->Size = 8;
					ptr[1].DataPointer = (IntPtr)((void*)value);
					ptr[1].Size = checked((arg2.Length + 1) * 2);
					this.WriteEventCore(eventId, 2, ptr);
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003324 File Offset: 0x00001524
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, int arg1, string arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg2 == null)
				{
					arg2 = "";
				}
				fixed (char* value = arg2)
				{
					EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData))];
					ptr->DataPointer = (IntPtr)((void*)(&arg1));
					ptr->Size = 4;
					ptr[1].DataPointer = (IntPtr)((void*)value);
					ptr[1].Size = checked((arg2.Length + 1) * 2);
					this.WriteEventCore(eventId, 2, ptr);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000033A8 File Offset: 0x000015A8
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, byte[] arg1)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg1 == null)
				{
					arg1 = new byte[0];
				}
				int size = arg1.Length;
				fixed (byte* ptr = &arg1[0])
				{
					EventSource.EventData* ptr2 = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventSource.EventData))];
					ptr2->DataPointer = (IntPtr)((void*)(&size));
					ptr2->Size = 4;
					ptr2[1].DataPointer = (IntPtr)((void*)ptr);
					ptr2[1].Size = size;
					this.WriteEventCore(eventId, 2, ptr2);
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003424 File Offset: 0x00001624
		[SecuritySafeCritical]
		protected unsafe void WriteEvent(int eventId, long arg1, byte[] arg2)
		{
			if (this.m_eventSourceEnabled)
			{
				if (arg2 == null)
				{
					arg2 = new byte[0];
				}
				int size = arg2.Length;
				fixed (byte* ptr = &arg2[0])
				{
					EventSource.EventData* ptr2 = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)3) * (UIntPtr)sizeof(EventSource.EventData))];
					ptr2->DataPointer = (IntPtr)((void*)(&arg1));
					ptr2->Size = 8;
					ptr2[1].DataPointer = (IntPtr)((void*)(&size));
					ptr2[1].Size = 4;
					ptr2[2].DataPointer = (IntPtr)((void*)ptr);
					ptr2[2].Size = size;
					this.WriteEventCore(eventId, 3, ptr2);
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000034CB File Offset: 0x000016CB
		[CLSCompliant(false)]
		[SecurityCritical]
		protected unsafe void WriteEventCore(int eventId, int eventDataCount, EventSource.EventData* data)
		{
			this.WriteEventWithRelatedActivityIdCore(eventId, null, eventDataCount, data);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000034D8 File Offset: 0x000016D8
		[CLSCompliant(false)]
		[SecurityCritical]
		protected unsafe void WriteEventWithRelatedActivityIdCore(int eventId, Guid* relatedActivityId, int eventDataCount, EventSource.EventData* data)
		{
			if (this.m_eventSourceEnabled)
			{
				try
				{
					if (relatedActivityId != null)
					{
						this.ValidateEventOpcodeForTransfer(ref this.m_eventData[eventId]);
					}
					EventOpcode opcode = (EventOpcode)this.m_eventData[eventId].Descriptor.Opcode;
					EventActivityOptions activityOptions = this.m_eventData[eventId].ActivityOptions;
					Guid* activityID = null;
					Guid empty = Guid.Empty;
					Guid empty2 = Guid.Empty;
					if (opcode != EventOpcode.Info && relatedActivityId == null && (activityOptions & EventActivityOptions.Disable) == EventActivityOptions.None)
					{
						if (opcode == EventOpcode.Start)
						{
							this.m_activityTracker.OnStart(this.m_name, this.m_eventData[eventId].Name, this.m_eventData[eventId].Descriptor.Task, ref empty, ref empty2, this.m_eventData[eventId].ActivityOptions);
						}
						else if (opcode == EventOpcode.Stop)
						{
							this.m_activityTracker.OnStop(this.m_name, this.m_eventData[eventId].Name, this.m_eventData[eventId].Descriptor.Task, ref empty);
						}
						if (empty != Guid.Empty)
						{
							activityID = &empty;
						}
						if (empty2 != Guid.Empty)
						{
							relatedActivityId = &empty2;
						}
					}
					if (!this.SelfDescribingEvents)
					{
						if (!this.m_provider.WriteEvent(ref this.m_eventData[eventId].Descriptor, activityID, relatedActivityId, eventDataCount, (IntPtr)((void*)data)))
						{
							this.ThrowEventSourceException(null);
						}
					}
					else
					{
						TraceLoggingEventTypes traceLoggingEventTypes = this.m_eventData[eventId].TraceLoggingEventTypes;
						if (traceLoggingEventTypes == null)
						{
							traceLoggingEventTypes = new TraceLoggingEventTypes(this.m_eventData[eventId].Name, this.m_eventData[eventId].Tags, this.m_eventData[eventId].Parameters);
							Interlocked.CompareExchange<TraceLoggingEventTypes>(ref this.m_eventData[eventId].TraceLoggingEventTypes, traceLoggingEventTypes, null);
						}
						EventSourceOptions eventSourceOptions = new EventSourceOptions
						{
							Keywords = (EventKeywords)this.m_eventData[eventId].Descriptor.Keywords,
							Level = (EventLevel)this.m_eventData[eventId].Descriptor.Level,
							Opcode = (EventOpcode)this.m_eventData[eventId].Descriptor.Opcode
						};
						this.WriteMultiMerge(this.m_eventData[eventId].Name, ref eventSourceOptions, traceLoggingEventTypes, activityID, relatedActivityId, data);
					}
					if (this.m_Dispatchers != null && this.m_eventData[eventId].EnabledForAnyListener)
					{
						this.WriteToAllListeners(eventId, relatedActivityId, eventDataCount, data);
					}
				}
				catch (Exception ex)
				{
					if (ex is EventSourceException)
					{
						throw;
					}
					this.ThrowEventSourceException(ex);
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000037BC File Offset: 0x000019BC
		[SecuritySafeCritical]
		protected void WriteEvent(int eventId, params object[] args)
		{
			this.WriteEventVarargs(eventId, null, args);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000037C8 File Offset: 0x000019C8
		[SecuritySafeCritical]
		protected unsafe void WriteEventWithRelatedActivityId(int eventId, Guid relatedActivityId, params object[] args)
		{
			this.WriteEventVarargs(eventId, &relatedActivityId, args);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000037D5 File Offset: 0x000019D5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000037E4 File Offset: 0x000019E4
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_eventSourceEnabled)
				{
					try
					{
						this.SendManifest(this.m_rawManifest);
					}
					catch (Exception)
					{
					}
					this.m_eventSourceEnabled = false;
				}
				if (this.m_provider != null)
				{
					this.m_provider.Dispose();
					this.m_provider = null;
				}
			}
			this.m_eventSourceEnabled = false;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003850 File Offset: 0x00001A50
		~EventSource()
		{
			this.Dispose(false);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003880 File Offset: 0x00001A80
		[SecurityCritical]
		private unsafe void WriteEventRaw(ref EventDescriptor eventDescriptor, Guid* activityID, Guid* relatedActivityID, int dataCount, IntPtr data)
		{
			if (this.m_provider == null)
			{
				this.ThrowEventSourceException(null);
				return;
			}
			if (!this.m_provider.WriteEventRaw(ref eventDescriptor, activityID, relatedActivityID, dataCount, data))
			{
				this.ThrowEventSourceException(null);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000038B1 File Offset: 0x00001AB1
		internal EventSource(Guid eventSourceGuid, string eventSourceName) : this(eventSourceGuid, eventSourceName, EventSourceSettings.EtwManifestEventFormat, null)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000038BD File Offset: 0x00001ABD
		internal EventSource(Guid eventSourceGuid, string eventSourceName, EventSourceSettings settings, string[] traits = null)
		{
			this.m_config = this.ValidateSettings(settings);
			this.Initialize(eventSourceGuid, eventSourceName, traits);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000038DC File Offset: 0x00001ADC
		[SecuritySafeCritical]
		private unsafe void Initialize(Guid eventSourceGuid, string eventSourceName, string[] traits)
		{
			try
			{
				this.m_traits = traits;
				if (this.m_traits != null && this.m_traits.Length % 2 != 0)
				{
					throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("TraitEven", new object[0]), "traits");
				}
				if (eventSourceGuid == Guid.Empty)
				{
					throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NeedGuid", new object[0]));
				}
				if (eventSourceName == null)
				{
					throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NeedName", new object[0]));
				}
				this.m_name = eventSourceName;
				this.m_guid = eventSourceGuid;
				this.m_activityTracker = ActivityTracker.Instance;
				this.InitializeProviderMetadata();
				EventSource.OverideEventProvider overideEventProvider = new EventSource.OverideEventProvider(this);
				overideEventProvider.Register(eventSourceGuid);
				EventListener.AddEventSource(this);
				this.m_provider = overideEventProvider;
				try
				{
					fixed (IntPtr* ptr = this.providerMetadata)
					{
						this.m_provider.SetInformation(UnsafeNativeMethods.ManifestEtw.EVENT_INFO_CLASS.SetTraits, (void*)ptr, this.providerMetadata.Length);
					}
				}
				finally
				{
					IntPtr* ptr = null;
				}
				this.m_completelyInited = true;
			}
			catch (Exception ex)
			{
				if (this.m_constructionException == null)
				{
					this.m_constructionException = ex;
				}
				this.ReportOutOfBandMessage("ERROR: Exception during construction of EventSource " + this.Name + ": " + ex.Message, true);
			}
			lock (EventListener.EventListenersLock)
			{
				for (EventCommandEventArgs eventCommandEventArgs = this.m_deferredCommands; eventCommandEventArgs != null; eventCommandEventArgs = eventCommandEventArgs.nextCommand)
				{
					this.DoCommand(eventCommandEventArgs);
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003A74 File Offset: 0x00001C74
		private static string GetName(Type eventSourceType, EventManifestOptions flags)
		{
			if (eventSourceType == null)
			{
				throw new ArgumentNullException("eventSourceType");
			}
			EventSourceAttribute eventSourceAttribute = (EventSourceAttribute)EventSource.GetCustomAttributeHelper(eventSourceType, typeof(EventSourceAttribute), flags);
			if (eventSourceAttribute != null && eventSourceAttribute.Name != null)
			{
				return eventSourceAttribute.Name;
			}
			return eventSourceType.Name;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003AC4 File Offset: 0x00001CC4
		private static Guid GenerateGuidFromName(string name)
		{
			byte[] bytes = Encoding.BigEndianUnicode.GetBytes(name);
			EventSource.Sha1ForNonSecretPurposes sha1ForNonSecretPurposes = default(EventSource.Sha1ForNonSecretPurposes);
			sha1ForNonSecretPurposes.Start();
			sha1ForNonSecretPurposes.Append(EventSource.namespaceBytes);
			sha1ForNonSecretPurposes.Append(bytes);
			Array.Resize<byte>(ref bytes, 16);
			sha1ForNonSecretPurposes.Finish(bytes);
			bytes[7] = ((bytes[7] & 15) | 80);
			return new Guid(bytes);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003B24 File Offset: 0x00001D24
		[SecurityCritical]
		private unsafe object DecodeObject(int eventId, int parameterId, ref EventSource.EventData* data)
		{
			IntPtr dataPointer = data.DataPointer;
			checked
			{
				data += (IntPtr)sizeof(EventSource.EventData);
				Type type = this.m_eventData[eventId].Parameters[parameterId].ParameterType;
				while (!(type == typeof(IntPtr)))
				{
					if (type == typeof(int))
					{
						return *(int*)((void*)dataPointer);
					}
					if (type == typeof(uint))
					{
						return *(uint*)((void*)dataPointer);
					}
					if (type == typeof(long))
					{
						return *(long*)((void*)dataPointer);
					}
					if (type == typeof(ulong))
					{
						return (ulong)(*(long*)((void*)dataPointer));
					}
					if (type == typeof(byte))
					{
						return *(byte*)((void*)dataPointer);
					}
					if (type == typeof(sbyte))
					{
						return *(sbyte*)((void*)dataPointer);
					}
					if (type == typeof(short))
					{
						return *(short*)((void*)dataPointer);
					}
					if (type == typeof(ushort))
					{
						return *(ushort*)((void*)dataPointer);
					}
					if (type == typeof(float))
					{
						return *(float*)((void*)dataPointer);
					}
					if (type == typeof(double))
					{
						return *(double*)((void*)dataPointer);
					}
					if (type == typeof(decimal))
					{
						return *(decimal*)((void*)dataPointer);
					}
					if (type == typeof(bool))
					{
						if (*(int*)((void*)dataPointer) == 1)
						{
							return true;
						}
						return false;
					}
					else
					{
						if (type == typeof(Guid))
						{
							return *(Guid*)((void*)dataPointer);
						}
						if (type == typeof(char))
						{
							return (char)(*(ushort*)((void*)dataPointer));
						}
						if (type == typeof(DateTime))
						{
							long fileTime = *(long*)((void*)dataPointer);
							return DateTime.FromFileTimeUtc(fileTime);
						}
						if (type == typeof(byte[]))
						{
							int num = *(int*)((void*)dataPointer);
							byte[] array = new byte[num];
							dataPointer = data.DataPointer;
							data += (IntPtr)sizeof(EventSource.EventData);
							for (int i = 0; i < num; i++)
							{
								array[i] = *(byte*)((void*)dataPointer);
							}
							return array;
						}
						if (type == typeof(byte*))
						{
							return null;
						}
						if (!type.IsEnum())
						{
							return Marshal.PtrToStringUni(dataPointer);
						}
						type = Enum.GetUnderlyingType(type);
					}
				}
				return *(IntPtr*)((void*)dataPointer);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003DF0 File Offset: 0x00001FF0
		private EventDispatcher GetDispatcher(EventListener listener)
		{
			EventDispatcher eventDispatcher;
			for (eventDispatcher = this.m_Dispatchers; eventDispatcher != null; eventDispatcher = eventDispatcher.m_Next)
			{
				if (eventDispatcher.m_Listener == listener)
				{
					return eventDispatcher;
				}
			}
			return eventDispatcher;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003E20 File Offset: 0x00002020
		[SecurityCritical]
		private unsafe void WriteEventVarargs(int eventId, Guid* childActivityID, object[] args)
		{
			if (this.m_eventSourceEnabled)
			{
				try
				{
					if (childActivityID != null)
					{
						this.ValidateEventOpcodeForTransfer(ref this.m_eventData[eventId]);
						if (!this.m_eventData[eventId].HasRelatedActivityID)
						{
							throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NoRelatedActivityId", new object[0]));
						}
					}
					this.LogEventArgsMismatches(this.m_eventData[eventId].Parameters, args);
					Guid* activityID = null;
					Guid empty = Guid.Empty;
					Guid empty2 = Guid.Empty;
					EventOpcode opcode = (EventOpcode)this.m_eventData[eventId].Descriptor.Opcode;
					EventActivityOptions activityOptions = this.m_eventData[eventId].ActivityOptions;
					if (childActivityID == null && (activityOptions & EventActivityOptions.Disable) == EventActivityOptions.None)
					{
						if (opcode == EventOpcode.Start)
						{
							this.m_activityTracker.OnStart(this.m_name, this.m_eventData[eventId].Name, this.m_eventData[eventId].Descriptor.Task, ref empty, ref empty2, this.m_eventData[eventId].ActivityOptions);
						}
						else if (opcode == EventOpcode.Stop)
						{
							this.m_activityTracker.OnStop(this.m_name, this.m_eventData[eventId].Name, this.m_eventData[eventId].Descriptor.Task, ref empty);
						}
						if (empty != Guid.Empty)
						{
							activityID = &empty;
						}
						if (empty2 != Guid.Empty)
						{
							childActivityID = &empty2;
						}
					}
					if (!this.SelfDescribingEvents)
					{
						if (!this.m_provider.WriteEvent(ref this.m_eventData[eventId].Descriptor, activityID, childActivityID, args))
						{
							this.ThrowEventSourceException(null);
						}
					}
					else
					{
						TraceLoggingEventTypes traceLoggingEventTypes = this.m_eventData[eventId].TraceLoggingEventTypes;
						if (traceLoggingEventTypes == null)
						{
							traceLoggingEventTypes = new TraceLoggingEventTypes(this.m_eventData[eventId].Name, EventTags.None, this.m_eventData[eventId].Parameters);
							Interlocked.CompareExchange<TraceLoggingEventTypes>(ref this.m_eventData[eventId].TraceLoggingEventTypes, traceLoggingEventTypes, null);
						}
						EventSourceOptions eventSourceOptions = new EventSourceOptions
						{
							Keywords = (EventKeywords)this.m_eventData[eventId].Descriptor.Keywords,
							Level = (EventLevel)this.m_eventData[eventId].Descriptor.Level,
							Opcode = (EventOpcode)this.m_eventData[eventId].Descriptor.Opcode
						};
						this.WriteMultiMerge(this.m_eventData[eventId].Name, ref eventSourceOptions, traceLoggingEventTypes, activityID, childActivityID, args);
					}
					if (this.m_Dispatchers != null && this.m_eventData[eventId].EnabledForAnyListener)
					{
						object[] args2 = this.SerializeEventArgs(eventId, args);
						this.WriteToAllListeners(eventId, childActivityID, args2);
					}
				}
				catch (Exception ex)
				{
					if (ex is EventSourceException)
					{
						throw;
					}
					this.ThrowEventSourceException(ex);
				}
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004134 File Offset: 0x00002334
		[SecurityCritical]
		private object[] SerializeEventArgs(int eventId, object[] args)
		{
			TraceLoggingEventTypes traceLoggingEventTypes = this.m_eventData[eventId].TraceLoggingEventTypes;
			if (traceLoggingEventTypes == null)
			{
				traceLoggingEventTypes = new TraceLoggingEventTypes(this.m_eventData[eventId].Name, EventTags.None, this.m_eventData[eventId].Parameters);
				Interlocked.CompareExchange<TraceLoggingEventTypes>(ref this.m_eventData[eventId].TraceLoggingEventTypes, traceLoggingEventTypes, null);
			}
			object[] array = new object[traceLoggingEventTypes.typeInfos.Length];
			checked
			{
				for (int i = 0; i < traceLoggingEventTypes.typeInfos.Length; i++)
				{
					array[i] = traceLoggingEventTypes.typeInfos[i].GetData(args[i]);
				}
				return array;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000041D8 File Offset: 0x000023D8
		private void LogEventArgsMismatches(ParameterInfo[] infos, object[] args)
		{
			bool flag = args.Length == infos.Length;
			int num = 0;
			checked
			{
				while (flag && num < args.Length)
				{
					Type parameterType = infos[num].ParameterType;
					if ((args[num] != null && args[num].GetType() != parameterType) || (args[num] == null && (!parameterType.IsGenericType || !(parameterType.GetGenericTypeDefinition() == typeof(Nullable<>)))))
					{
						flag = false;
						break;
					}
					num++;
				}
				if (!flag)
				{
					Debugger.Log(0, null, Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_VarArgsParameterMismatch", new object[0]) + "\r\n");
				}
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000426C File Offset: 0x0000246C
		[SecurityCritical]
		private unsafe void WriteToAllListeners(int eventId, Guid* childActivityID, int eventDataCount, EventSource.EventData* data)
		{
			int num = this.m_eventData[eventId].Parameters.Length;
			if (eventDataCount != num)
			{
				this.ReportOutOfBandMessage(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventParametersMismatch", new object[]
				{
					eventId,
					eventDataCount,
					num
				}), true);
				num = Math.Min(num, eventDataCount);
			}
			object[] array = new object[num];
			EventSource.EventData* ptr = data;
			checked
			{
				for (int i = 0; i < num; i++)
				{
					array[i] = this.DecodeObject(eventId, i, ref ptr);
				}
				this.WriteToAllListeners(eventId, childActivityID, array);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004300 File Offset: 0x00002500
		[SecurityCritical]
		private unsafe void WriteToAllListeners(int eventId, Guid* childActivityID, params object[] args)
		{
			EventWrittenEventArgs eventWrittenEventArgs = new EventWrittenEventArgs(this);
			eventWrittenEventArgs.EventId = eventId;
			if (childActivityID != null)
			{
				eventWrittenEventArgs.RelatedActivityId = *childActivityID;
			}
			eventWrittenEventArgs.EventName = this.m_eventData[eventId].Name;
			eventWrittenEventArgs.Message = this.m_eventData[eventId].Message;
			eventWrittenEventArgs.Payload = new ReadOnlyCollection<object>(args);
			this.DispatchToAllListeners(eventId, childActivityID, eventWrittenEventArgs);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004374 File Offset: 0x00002574
		[SecurityCritical]
		private unsafe void DispatchToAllListeners(int eventId, Guid* childActivityID, EventWrittenEventArgs eventCallbackArgs)
		{
			Exception ex = null;
			EventDispatcher eventDispatcher = this.m_Dispatchers;
			while (eventDispatcher != null)
			{
				if (eventId == -1)
				{
					goto IL_1B;
				}
				if (eventDispatcher.m_EventEnabled[eventId])
				{
					goto Block_2;
				}
				IL_45:
				eventDispatcher = eventDispatcher.m_Next;
				continue;
				Block_2:
				try
				{
					IL_1B:
					eventDispatcher.m_Listener.OnEventWritten(eventCallbackArgs);
				}
				catch (Exception ex2)
				{
					this.ReportOutOfBandMessage("ERROR: Exception during EventSource.OnEventWritten: " + ex2.Message, false);
					ex = ex2;
				}
				goto IL_45;
			}
			if (ex != null)
			{
				throw new EventSourceException(ex);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000044A8 File Offset: 0x000026A8
		[SecuritySafeCritical]
		private unsafe void WriteEventString(EventLevel level, long keywords, string msgString)
		{
			if (this.m_provider != null)
			{
				string text = "EventSourceMessage";
				if (this.SelfDescribingEvents)
				{
					EventSourceOptions eventSourceOptions = new EventSourceOptions
					{
						Keywords = (EventKeywords)keywords,
						Level = level
					};
					var <>f__AnonymousType = new
					{
						message = msgString
					};
					TraceLoggingEventTypes eventTypes = new TraceLoggingEventTypes(text, EventTags.None, new Type[]
					{
						<>f__AnonymousType.GetType()
					});
					this.WriteMultiMergeInner(text, ref eventSourceOptions, eventTypes, null, null, new object[]
					{
						<>f__AnonymousType
					});
					return;
				}
				if (this.m_rawManifest == null && this.m_outOfBandMessageCount == 1)
				{
					ManifestBuilder manifestBuilder = new ManifestBuilder(this.Name, this.Guid, this.Name, null, EventManifestOptions.None);
					manifestBuilder.StartEvent(text, new EventAttribute(0)
					{
						Level = EventLevel.LogAlways,
						Task = (EventTask)65534
					});
					manifestBuilder.AddEventParameter(typeof(string), "message");
					manifestBuilder.EndEvent();
					this.SendManifest(manifestBuilder.CreateManifest());
				}
				fixed (char* ptr = msgString)
				{
					EventDescriptor eventDescriptor;
					EventProvider.EventData eventData;
					checked
					{
						eventDescriptor = new EventDescriptor(0, 0, 0, (byte)level, 0, 0, keywords);
						eventData = default(EventProvider.EventData);
						eventData.Ptr = ptr;
						eventData.Size = (uint)(2 * (msgString.Length + 1));
						eventData.Reserved = 0U;
					}
					this.m_provider.WriteEvent(ref eventDescriptor, null, null, 1, (IntPtr)((void*)(&eventData)));
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000461C File Offset: 0x0000281C
		private void WriteStringToAllListeners(string eventName, string msg)
		{
			EventWrittenEventArgs eventWrittenEventArgs = new EventWrittenEventArgs(this);
			eventWrittenEventArgs.EventId = 0;
			eventWrittenEventArgs.Message = msg;
			eventWrittenEventArgs.Payload = new ReadOnlyCollection<object>(new List<object>
			{
				msg
			});
			eventWrittenEventArgs.PayloadNames = new ReadOnlyCollection<string>(new List<string>
			{
				"message"
			});
			eventWrittenEventArgs.EventName = eventName;
			checked
			{
				for (EventDispatcher eventDispatcher = this.m_Dispatchers; eventDispatcher != null; eventDispatcher = eventDispatcher.m_Next)
				{
					bool flag = false;
					if (eventDispatcher.m_EventEnabled == null)
					{
						flag = true;
					}
					else
					{
						for (int i = 0; i < eventDispatcher.m_EventEnabled.Length; i++)
						{
							if (eventDispatcher.m_EventEnabled[i])
							{
								flag = true;
								break;
							}
						}
					}
					try
					{
						if (flag)
						{
							eventDispatcher.m_Listener.OnEventWritten(eventWrittenEventArgs);
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000046EC File Offset: 0x000028EC
		private bool IsEnabledByDefault(int eventNum, bool enable, EventLevel currentLevel, EventKeywords currentMatchAnyKeyword)
		{
			if (!enable)
			{
				return false;
			}
			EventLevel level = (EventLevel)this.m_eventData[eventNum].Descriptor.Level;
			EventKeywords eventKeywords = (EventKeywords)(this.m_eventData[eventNum].Descriptor.Keywords & (long)(~(long)SessionMask.All.ToEventKeywords()));
			EventChannel channel = (EventChannel)this.m_eventData[eventNum].Descriptor.Channel;
			return this.IsEnabledCommon(enable, currentLevel, currentMatchAnyKeyword, level, eventKeywords, channel);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004768 File Offset: 0x00002968
		private bool IsEnabledCommon(bool enabled, EventLevel currentLevel, EventKeywords currentMatchAnyKeyword, EventLevel eventLevel, EventKeywords eventKeywords, EventChannel eventChannel)
		{
			if (!enabled)
			{
				return false;
			}
			if (currentLevel != EventLevel.LogAlways && currentLevel < eventLevel)
			{
				return false;
			}
			if (currentMatchAnyKeyword != EventKeywords.None && eventKeywords != EventKeywords.None)
			{
				if (eventChannel != EventChannel.None && this.m_channelData != null && this.m_channelData.Length > (int)eventChannel)
				{
					EventKeywords eventKeywords2 = (EventKeywords)(this.m_channelData[(int)eventChannel] | (ulong)eventKeywords);
					if (eventKeywords2 != EventKeywords.None && (eventKeywords2 & currentMatchAnyKeyword) == EventKeywords.None)
					{
						return false;
					}
				}
				else if ((eventKeywords & currentMatchAnyKeyword) == EventKeywords.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000047D4 File Offset: 0x000029D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void ThrowEventSourceException(Exception innerEx = null)
		{
			if (EventSource.m_EventSourceExceptionRecurenceCount > 0)
			{
				return;
			}
			checked
			{
				try
				{
					EventSource.m_EventSourceExceptionRecurenceCount += 1;
					switch (EventProvider.GetLastWriteEventError())
					{
					case EventProvider.WriteEventErrorCode.NoFreeBuffers:
						this.ReportOutOfBandMessage("EventSourceException: " + Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NoFreeBuffers", new object[0]), true);
						if (this.ThrowOnEventWriteErrors)
						{
							throw new EventSourceException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NoFreeBuffers", new object[0]), innerEx);
						}
						break;
					case EventProvider.WriteEventErrorCode.EventTooBig:
						this.ReportOutOfBandMessage("EventSourceException: " + Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventTooBig", new object[0]), true);
						if (this.ThrowOnEventWriteErrors)
						{
							throw new EventSourceException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventTooBig", new object[0]), innerEx);
						}
						break;
					case EventProvider.WriteEventErrorCode.NullInput:
						this.ReportOutOfBandMessage("EventSourceException: " + Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NullInput", new object[0]), true);
						if (this.ThrowOnEventWriteErrors)
						{
							throw new EventSourceException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NullInput", new object[0]), innerEx);
						}
						break;
					case EventProvider.WriteEventErrorCode.TooManyArgs:
						this.ReportOutOfBandMessage("EventSourceException: " + Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TooManyArgs", new object[0]), true);
						if (this.ThrowOnEventWriteErrors)
						{
							throw new EventSourceException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TooManyArgs", new object[0]), innerEx);
						}
						break;
					default:
						if (innerEx != null)
						{
							this.ReportOutOfBandMessage(string.Concat(new object[]
							{
								"EventSourceException: ",
								innerEx.GetType(),
								":",
								innerEx.Message
							}), true);
						}
						else
						{
							this.ReportOutOfBandMessage("EventSourceException", true);
						}
						if (this.ThrowOnEventWriteErrors)
						{
							throw new EventSourceException(innerEx);
						}
						break;
					}
				}
				finally
				{
					EventSource.m_EventSourceExceptionRecurenceCount -= 1;
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000049A8 File Offset: 0x00002BA8
		private void ValidateEventOpcodeForTransfer(ref EventSource.EventMetadata eventData)
		{
			if (eventData.Descriptor.Opcode != 9 && eventData.Descriptor.Opcode != 240 && eventData.Descriptor.Opcode != 1)
			{
				this.ThrowEventSourceException(null);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000049E0 File Offset: 0x00002BE0
		internal static EventOpcode GetOpcodeWithDefault(EventOpcode opcode, string eventName)
		{
			if (opcode == EventOpcode.Info && eventName != null)
			{
				if (eventName.EndsWith("Start"))
				{
					return EventOpcode.Start;
				}
				if (eventName.EndsWith("Stop"))
				{
					return EventOpcode.Stop;
				}
			}
			return opcode;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004A08 File Offset: 0x00002C08
		internal void SendCommand(EventListener listener, int perEventSourceSessionId, int etwSessionId, EventCommand command, bool enable, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string> commandArguments)
		{
			EventCommandEventArgs eventCommandEventArgs = new EventCommandEventArgs(command, commandArguments, this, listener, perEventSourceSessionId, etwSessionId, enable, level, matchAnyKeyword);
			lock (EventListener.EventListenersLock)
			{
				if (this.m_completelyInited)
				{
					this.m_deferredCommands = null;
					this.DoCommand(eventCommandEventArgs);
				}
				else
				{
					eventCommandEventArgs.nextCommand = this.m_deferredCommands;
					this.m_deferredCommands = eventCommandEventArgs;
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004A80 File Offset: 0x00002C80
		internal void DoCommand(EventCommandEventArgs commandArgs)
		{
			if (this.m_provider == null)
			{
				return;
			}
			this.m_outOfBandMessageCount = 0;
			if (commandArgs.perEventSourceSessionId > 0)
			{
				long num = (long)commandArgs.perEventSourceSessionId;
			}
			checked
			{
				try
				{
					this.EnsureDescriptorsInitialized();
					commandArgs.dispatcher = this.GetDispatcher(commandArgs.listener);
					if (commandArgs.dispatcher == null && commandArgs.listener != null)
					{
						throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ListenerNotFound", new object[0]));
					}
					if (commandArgs.Arguments == null)
					{
						commandArgs.Arguments = new Dictionary<string, string>();
					}
					if (commandArgs.Command == EventCommand.Update)
					{
						for (int i = 0; i < this.m_eventData.Length; i++)
						{
							this.EnableEventForDispatcher(commandArgs.dispatcher, i, this.IsEnabledByDefault(i, commandArgs.enable, commandArgs.level, commandArgs.matchAnyKeyword));
						}
						if (commandArgs.enable)
						{
							if (!this.m_eventSourceEnabled)
							{
								this.m_level = commandArgs.level;
								this.m_matchAnyKeyword = commandArgs.matchAnyKeyword;
							}
							else
							{
								if (commandArgs.level > this.m_level)
								{
									this.m_level = commandArgs.level;
								}
								if (commandArgs.matchAnyKeyword == EventKeywords.None)
								{
									this.m_matchAnyKeyword = EventKeywords.None;
								}
								else if (this.m_matchAnyKeyword != EventKeywords.None)
								{
									this.m_matchAnyKeyword |= commandArgs.matchAnyKeyword;
								}
							}
						}
						bool flag = commandArgs.perEventSourceSessionId >= 0;
						if (commandArgs.perEventSourceSessionId == 0 && !commandArgs.enable)
						{
							flag = false;
						}
						if (commandArgs.listener == null)
						{
							if (!flag)
							{
								commandArgs.perEventSourceSessionId = 0 - commandArgs.perEventSourceSessionId;
							}
							commandArgs.perEventSourceSessionId--;
						}
						commandArgs.Command = (flag ? EventCommand.Enable : EventCommand.Disable);
						if (flag && commandArgs.dispatcher == null && !this.SelfDescribingEvents)
						{
							this.SendManifest(this.m_rawManifest);
						}
						if (commandArgs.enable)
						{
							this.m_eventSourceEnabled = true;
						}
						this.OnEventCommand(commandArgs);
						EventHandler<EventCommandEventArgs> eventCommandExecuted = this.m_eventCommandExecuted;
						if (eventCommandExecuted != null)
						{
							eventCommandExecuted(this, commandArgs);
						}
						if (!commandArgs.enable)
						{
							for (int j = 0; j < this.m_eventData.Length; j++)
							{
								bool enabledForAnyListener = false;
								for (EventDispatcher eventDispatcher = this.m_Dispatchers; eventDispatcher != null; eventDispatcher = eventDispatcher.m_Next)
								{
									if (eventDispatcher.m_EventEnabled[j])
									{
										enabledForAnyListener = true;
										break;
									}
								}
								this.m_eventData[j].EnabledForAnyListener = enabledForAnyListener;
							}
							if (!this.AnyEventEnabled())
							{
								this.m_level = EventLevel.LogAlways;
								this.m_matchAnyKeyword = EventKeywords.None;
								this.m_eventSourceEnabled = false;
							}
						}
					}
					else
					{
						if (commandArgs.Command == EventCommand.SendManifest && this.m_rawManifest != null)
						{
							this.SendManifest(this.m_rawManifest);
						}
						this.OnEventCommand(commandArgs);
						EventHandler<EventCommandEventArgs> eventCommandExecuted2 = this.m_eventCommandExecuted;
						if (eventCommandExecuted2 != null)
						{
							eventCommandExecuted2(this, commandArgs);
						}
					}
				}
				catch (Exception ex)
				{
					this.ReportOutOfBandMessage("ERROR: Exception in Command Processing for EventSource " + this.Name + ": " + ex.Message, true);
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004D60 File Offset: 0x00002F60
		internal bool EnableEventForDispatcher(EventDispatcher dispatcher, int eventId, bool value)
		{
			if (dispatcher == null)
			{
				if (eventId >= this.m_eventData.Length)
				{
					return false;
				}
				if (this.m_provider != null)
				{
					this.m_eventData[eventId].EnabledForETW = value;
				}
			}
			else
			{
				if (eventId >= dispatcher.m_EventEnabled.Length)
				{
					return false;
				}
				dispatcher.m_EventEnabled[eventId] = value;
				if (value)
				{
					this.m_eventData[eventId].EnabledForAnyListener = true;
				}
			}
			return true;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004DD0 File Offset: 0x00002FD0
		private bool AnyEventEnabled()
		{
			checked
			{
				for (int i = 0; i < this.m_eventData.Length; i++)
				{
					if (this.m_eventData[i].EnabledForETW || this.m_eventData[i].EnabledForAnyListener)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00004E1F File Offset: 0x0000301F
		private bool IsDisposed
		{
			get
			{
				return this.m_provider == null || this.m_provider.m_disposed;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004E3C File Offset: 0x0000303C
		[SecuritySafeCritical]
		private void EnsureDescriptorsInitialized()
		{
			if (this.m_eventData == null)
			{
				this.m_rawManifest = EventSource.CreateManifestAndDescriptors(base.GetType(), this.Name, this, EventManifestOptions.None);
				foreach (WeakReference weakReference in EventListener.s_EventSources)
				{
					EventSource eventSource = weakReference.Target as EventSource;
					if (eventSource != null && eventSource.Guid == this.m_guid && !eventSource.IsDisposed && eventSource != this)
					{
						throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventSourceGuidInUse", new object[]
						{
							this.m_guid
						}));
					}
				}
				for (EventDispatcher eventDispatcher = this.m_Dispatchers; eventDispatcher != null; eventDispatcher = eventDispatcher.m_Next)
				{
					if (eventDispatcher.m_EventEnabled == null)
					{
						eventDispatcher.m_EventEnabled = new bool[this.m_eventData.Length];
					}
				}
			}
			if (EventSource.s_currentPid == 0U)
			{
				new SecurityPermission(PermissionState.Unrestricted).Assert();
				EventSource.s_currentPid = Win32Native.GetCurrentProcessId();
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004F54 File Offset: 0x00003154
		[SecuritySafeCritical]
		private unsafe bool SendManifest(byte[] rawManifest)
		{
			bool result = true;
			if (rawManifest == null)
			{
				return false;
			}
			checked
			{
				try
				{
					fixed (byte* ptr = rawManifest)
					{
						EventDescriptor eventDescriptor = new EventDescriptor(65534, 1, 0, 0, 254, 65534, 72057594037927935L);
						ManifestEnvelope manifestEnvelope = default(ManifestEnvelope);
						manifestEnvelope.Format = ManifestEnvelope.ManifestFormats.SimpleXmlFormat;
						manifestEnvelope.MajorVersion = 1;
						manifestEnvelope.MinorVersion = 0;
						manifestEnvelope.Magic = 91;
						int i = rawManifest.Length;
						manifestEnvelope.ChunkNumber = 0;
						EventProvider.EventData* ptr2 = stackalloc EventProvider.EventData[unchecked((UIntPtr)2) * (UIntPtr)sizeof(EventProvider.EventData)];
						ptr2->Ptr = &manifestEnvelope;
						ptr2->Size = (uint)sizeof(ManifestEnvelope);
						ptr2->Reserved = 0U;
						int num;
						unchecked
						{
							ptr2[1].Ptr = ptr;
							ptr2[1].Reserved = 0U;
							num = 65280;
						}
						for (;;)
						{
							IL_D3:
							manifestEnvelope.TotalChunks = (ushort)((i + (num - 1)) / num);
							while (i > 0)
							{
								unchecked(ptr2[1]).Size = (uint)Math.Min(i, num);
								if (this.m_provider != null && !this.m_provider.WriteEvent(ref eventDescriptor, null, null, 2, (IntPtr)((void*)ptr2)))
								{
									if (EventProvider.GetLastWriteEventError() == EventProvider.WriteEventErrorCode.EventTooBig && manifestEnvelope.ChunkNumber == 0 && num > 256)
									{
										num /= 2;
										goto IL_D3;
									}
									goto IL_14B;
								}
								else
								{
									i -= num;
									unchecked(ptr2[1]).Ptr += unchecked((ulong)(checked((uint)num)));
									manifestEnvelope.ChunkNumber += 1;
								}
							}
							goto IL_196;
						}
						IL_14B:
						result = false;
						if (this.ThrowOnEventWriteErrors)
						{
							this.ThrowEventSourceException(null);
						}
						IL_196:;
					}
				}
				finally
				{
					byte* ptr = null;
				}
				return result;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000511C File Offset: 0x0000331C
		internal static Attribute GetCustomAttributeHelper(MemberInfo member, Type attributeType, EventManifestOptions flags = EventManifestOptions.None)
		{
			if (!member.Module.Assembly.ReflectionOnly() && (flags & EventManifestOptions.AllowEventSourceOverride) == EventManifestOptions.None)
			{
				Attribute result = null;
				object[] customAttributes = member.GetCustomAttributes(attributeType, false);
				int num = 0;
				if (num < customAttributes.Length)
				{
					object obj = customAttributes[num];
					result = (Attribute)obj;
				}
				return result;
			}
			string fullName = attributeType.FullName;
			foreach (CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(member))
			{
				if (EventSource.AttributeTypeNamesMatch(attributeType, customAttributeData.Constructor.ReflectedType))
				{
					Attribute attribute = null;
					if (customAttributeData.ConstructorArguments.Count == 1)
					{
						attribute = (Attribute)Activator.CreateInstance(attributeType, new object[]
						{
							customAttributeData.ConstructorArguments[0].Value
						});
					}
					else if (customAttributeData.ConstructorArguments.Count == 0)
					{
						attribute = (Attribute)Activator.CreateInstance(attributeType);
					}
					if (attribute != null)
					{
						Type type = attribute.GetType();
						foreach (CustomAttributeNamedArgument customAttributeNamedArgument in customAttributeData.NamedArguments)
						{
							PropertyInfo property = type.GetProperty(customAttributeNamedArgument.MemberInfo.Name, BindingFlags.Instance | BindingFlags.Public);
							object obj2 = customAttributeNamedArgument.TypedValue.Value;
							if (property.PropertyType.IsEnum)
							{
								obj2 = Enum.Parse(property.PropertyType, obj2.ToString());
							}
							property.SetValue(attribute, obj2, null);
						}
						return attribute;
					}
				}
			}
			return null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000052E8 File Offset: 0x000034E8
		private static bool AttributeTypeNamesMatch(Type attributeType, Type reflectedAttributeType)
		{
			return attributeType == reflectedAttributeType || string.Equals(attributeType.FullName, reflectedAttributeType.FullName, StringComparison.Ordinal) || (string.Equals(attributeType.Name, reflectedAttributeType.Name, StringComparison.Ordinal) && attributeType.Namespace.EndsWith("Diagnostics.Tracing") && reflectedAttributeType.Namespace.EndsWith("Diagnostics.Tracing"));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000534C File Offset: 0x0000354C
		private static Type GetEventSourceBaseType(Type eventSourceType, bool allowEventSourceOverride, bool reflectionOnly)
		{
			if (eventSourceType.BaseType() == null)
			{
				return null;
			}
			do
			{
				eventSourceType = eventSourceType.BaseType();
			}
			while (eventSourceType != null && eventSourceType.IsAbstract());
			if (eventSourceType != null)
			{
				if (!allowEventSourceOverride)
				{
					if ((reflectionOnly && eventSourceType.FullName != typeof(EventSource).FullName) || (!reflectionOnly && eventSourceType != typeof(EventSource)))
					{
						return null;
					}
				}
				else if (eventSourceType.Name != "EventSource")
				{
					return null;
				}
			}
			return eventSourceType;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000053DC File Offset: 0x000035DC
		private static byte[] CreateManifestAndDescriptors(Type eventSourceType, string eventSourceDllName, EventSource source, EventManifestOptions flags = EventManifestOptions.None)
		{
			ManifestBuilder manifestBuilder = null;
			bool flag = source == null || !source.SelfDescribingEvents;
			Exception ex = null;
			byte[] result = null;
			if (eventSourceType.IsAbstract() && (flags & EventManifestOptions.Strict) == EventManifestOptions.None)
			{
				return null;
			}
			checked
			{
				try
				{
					MethodInfo[] methods = eventSourceType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					int num = 1;
					EventSource.EventMetadata[] array = null;
					Dictionary<string, string> dictionary = null;
					if (source != null || (flags & EventManifestOptions.Strict) != EventManifestOptions.None)
					{
						array = new EventSource.EventMetadata[methods.Length + 1];
						array[0].Name = "";
					}
					ResourceManager resources = null;
					EventSourceAttribute eventSourceAttribute = (EventSourceAttribute)EventSource.GetCustomAttributeHelper(eventSourceType, typeof(EventSourceAttribute), flags);
					if (eventSourceAttribute != null && eventSourceAttribute.LocalizationResources != null)
					{
						resources = new ResourceManager(eventSourceAttribute.LocalizationResources, eventSourceType.Assembly());
					}
					manifestBuilder = new ManifestBuilder(EventSource.GetName(eventSourceType, flags), EventSource.GetGuid(eventSourceType), eventSourceDllName, resources, flags);
					manifestBuilder.StartEvent("EventSourceMessage", new EventAttribute(0)
					{
						Level = EventLevel.LogAlways,
						Task = (EventTask)65534
					});
					manifestBuilder.AddEventParameter(typeof(string), "message");
					manifestBuilder.EndEvent();
					if ((flags & EventManifestOptions.Strict) != EventManifestOptions.None)
					{
						if (!(EventSource.GetEventSourceBaseType(eventSourceType, (flags & EventManifestOptions.AllowEventSourceOverride) != EventManifestOptions.None, eventSourceType.Assembly().ReflectionOnly()) != null))
						{
							manifestBuilder.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TypeMustDeriveFromEventSource", new object[0]), false);
						}
						if (!eventSourceType.IsAbstract() && !eventSourceType.IsSealed())
						{
							manifestBuilder.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TypeMustBeSealedOrAbstract", new object[0]), false);
						}
					}
					foreach (string text in new string[]
					{
						"Keywords",
						"Tasks",
						"Opcodes"
					})
					{
						Type nestedType = eventSourceType.GetNestedType(text);
						if (nestedType != null)
						{
							if (eventSourceType.IsAbstract())
							{
								manifestBuilder.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_AbstractMustNotDeclareKTOC", new object[]
								{
									nestedType.Name
								}), false);
							}
							else
							{
								foreach (FieldInfo staticField in nestedType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
								{
									EventSource.AddProviderEnumKind(manifestBuilder, staticField, text);
								}
							}
						}
					}
					manifestBuilder.AddKeyword("Session3", 17592186044416UL);
					manifestBuilder.AddKeyword("Session2", 35184372088832UL);
					manifestBuilder.AddKeyword("Session1", 70368744177664UL);
					manifestBuilder.AddKeyword("Session0", 140737488355328UL);
					if (eventSourceType != typeof(EventSource))
					{
						foreach (MethodInfo methodInfo in methods)
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							EventAttribute eventAttribute = (EventAttribute)EventSource.GetCustomAttributeHelper(methodInfo, typeof(EventAttribute), flags);
							if (!methodInfo.IsStatic)
							{
								if (eventSourceType.IsAbstract())
								{
									if (eventAttribute != null)
									{
										manifestBuilder.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_AbstractMustNotDeclareEventMethods", new object[]
										{
											methodInfo.Name,
											eventAttribute.EventId
										}), false);
									}
								}
								else
								{
									if (eventAttribute == null)
									{
										if (methodInfo.ReturnType != typeof(void) || methodInfo.IsVirtual || EventSource.GetCustomAttributeHelper(methodInfo, typeof(NonEventAttribute), flags) != null)
										{
											goto IL_66F;
										}
										EventAttribute eventAttribute2 = new EventAttribute(num);
										eventAttribute = eventAttribute2;
									}
									else if (eventAttribute.EventId <= 0)
									{
										manifestBuilder.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NeedPositiveId", new object[]
										{
											methodInfo.Name
										}), true);
										goto IL_66F;
									}
									if (methodInfo.Name.LastIndexOf('.') >= 0)
									{
										manifestBuilder.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventMustNotBeExplicitImplementation", new object[]
										{
											methodInfo.Name,
											eventAttribute.EventId
										}), false);
									}
									num++;
									string name = methodInfo.Name;
									if (eventAttribute.Opcode == EventOpcode.Info)
									{
										bool flag2 = eventAttribute.Task == EventTask.None;
										if (flag2)
										{
											eventAttribute.Task = (EventTask)65534 - eventAttribute.EventId;
										}
										if (!eventAttribute.IsOpcodeSet)
										{
											eventAttribute.Opcode = EventSource.GetOpcodeWithDefault(EventOpcode.Info, name);
										}
										if (flag2)
										{
											if (eventAttribute.Opcode == EventOpcode.Start)
											{
												string text2 = name.Substring(0, name.Length - "Start".Length);
												if (string.Compare(name, 0, text2, 0, text2.Length) == 0 && string.Compare(name, text2.Length, "Start", 0, Math.Max(name.Length - text2.Length, "Start".Length)) == 0)
												{
													manifestBuilder.AddTask(text2, (int)eventAttribute.Task);
												}
											}
											else if (eventAttribute.Opcode == EventOpcode.Stop)
											{
												int num2 = eventAttribute.EventId - 1;
												if (array != null && num2 < array.Length)
												{
													EventSource.EventMetadata eventMetadata = array[num2];
													string text3 = name.Substring(0, name.Length - "Stop".Length);
													if (eventMetadata.Descriptor.Opcode == 1 && string.Compare(eventMetadata.Name, 0, text3, 0, text3.Length) == 0 && string.Compare(eventMetadata.Name, text3.Length, "Start", 0, Math.Max(eventMetadata.Name.Length - text3.Length, "Start".Length)) == 0)
													{
														eventAttribute.Task = (EventTask)eventMetadata.Descriptor.Task;
														flag2 = false;
													}
												}
												if (flag2 && (flags & EventManifestOptions.Strict) != EventManifestOptions.None)
												{
													throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_StopsFollowStarts", new object[0]));
												}
											}
										}
									}
									bool hasRelatedActivityID = EventSource.RemoveFirstArgIfRelatedActivityId(ref parameters);
									if (source == null || !source.SelfDescribingEvents)
									{
										manifestBuilder.StartEvent(name, eventAttribute);
										for (int l = 0; l < parameters.Length; l++)
										{
											manifestBuilder.AddEventParameter(parameters[l].ParameterType, parameters[l].Name);
										}
										manifestBuilder.EndEvent();
									}
									if (source != null || (flags & EventManifestOptions.Strict) != EventManifestOptions.None)
									{
										EventSource.DebugCheckEvent(ref dictionary, array, methodInfo, eventAttribute, manifestBuilder, flags);
										if (eventAttribute.Channel != EventChannel.None)
										{
											eventAttribute.Keywords |= (EventKeywords)manifestBuilder.GetChannelKeyword(eventAttribute.Channel);
										}
										string key = "event_" + name;
										string localizedMessage = manifestBuilder.GetLocalizedMessage(key, CultureInfo.CurrentUICulture, false);
										if (localizedMessage != null)
										{
											eventAttribute.Message = localizedMessage;
										}
										EventSource.AddEventDescriptor(ref array, name, eventAttribute, parameters, hasRelatedActivityID);
									}
								}
							}
							IL_66F:;
						}
					}
					NameInfo.ReserveEventIDsBelow(num);
					if (source != null)
					{
						EventSource.TrimEventDescriptors(ref array);
						source.m_eventData = array;
						source.m_channelData = manifestBuilder.GetChannelData();
					}
					if (!eventSourceType.IsAbstract() && (source == null || !source.SelfDescribingEvents))
					{
						flag = ((flags & EventManifestOptions.OnlyIfNeededForRegistration) == EventManifestOptions.None || manifestBuilder.GetChannelData().Length > 0);
						if (!flag && (flags & EventManifestOptions.Strict) == EventManifestOptions.None)
						{
							return null;
						}
						result = manifestBuilder.CreateManifest();
					}
				}
				catch (Exception ex2)
				{
					if ((flags & EventManifestOptions.Strict) == EventManifestOptions.None)
					{
						throw;
					}
					ex = ex2;
				}
				if ((flags & EventManifestOptions.Strict) != EventManifestOptions.None && (manifestBuilder.Errors.Count > 0 || ex != null))
				{
					string text4 = string.Empty;
					if (manifestBuilder.Errors.Count > 0)
					{
						bool flag3 = true;
						using (IEnumerator<string> enumerator = manifestBuilder.Errors.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string str = enumerator.Current;
								if (!flag3)
								{
									text4 += Microsoft.Diagnostics.Tracing.Internal.Environment.NewLine;
								}
								flag3 = false;
								text4 += str;
							}
							goto IL_78C;
						}
					}
					text4 = "Unexpected error: " + ex.Message;
					IL_78C:
					throw new ArgumentException(text4, ex);
				}
				if (!flag)
				{
					return null;
				}
				return result;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005BBC File Offset: 0x00003DBC
		private static bool RemoveFirstArgIfRelatedActivityId(ref ParameterInfo[] args)
		{
			checked
			{
				if (args.Length > 0 && args[0].ParameterType == typeof(Guid) && string.Compare(args[0].Name, "relatedActivityId", StringComparison.OrdinalIgnoreCase) == 0)
				{
					ParameterInfo[] array = new ParameterInfo[args.Length - 1];
					Array.Copy(args, 1, array, 0, args.Length - 1);
					args = array;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005C24 File Offset: 0x00003E24
		private static void AddProviderEnumKind(ManifestBuilder manifest, FieldInfo staticField, string providerEnumKind)
		{
			bool flag = staticField.Module.Assembly.ReflectionOnly();
			Type fieldType = staticField.FieldType;
			if ((!flag && fieldType == typeof(EventOpcode)) || EventSource.AttributeTypeNamesMatch(fieldType, typeof(EventOpcode)))
			{
				if (!(providerEnumKind != "Opcodes"))
				{
					int value = (int)staticField.GetRawConstantValue();
					manifest.AddOpcode(staticField.Name, value);
					return;
				}
			}
			else
			{
				if ((flag || !(fieldType == typeof(EventTask))) && !EventSource.AttributeTypeNamesMatch(fieldType, typeof(EventTask)))
				{
					if ((!flag && fieldType == typeof(EventKeywords)) || EventSource.AttributeTypeNamesMatch(fieldType, typeof(EventKeywords)))
					{
						if (providerEnumKind != "Keywords")
						{
							goto IL_107;
						}
						ulong value2 = (ulong)((long)staticField.GetRawConstantValue());
						manifest.AddKeyword(staticField.Name, value2);
					}
					return;
				}
				if (!(providerEnumKind != "Tasks"))
				{
					int value3 = (int)staticField.GetRawConstantValue();
					manifest.AddTask(staticField.Name, value3);
					return;
				}
			}
			IL_107:
			manifest.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EnumKindMismatch", new object[]
			{
				staticField.Name,
				staticField.FieldType.Name,
				providerEnumKind
			}), false);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005D74 File Offset: 0x00003F74
		private static void AddEventDescriptor(ref EventSource.EventMetadata[] eventData, string eventName, EventAttribute eventAttribute, ParameterInfo[] eventParameters, bool hasRelatedActivityID)
		{
			checked
			{
				if (eventData == null || eventData.Length <= eventAttribute.EventId)
				{
					EventSource.EventMetadata[] array = new EventSource.EventMetadata[Math.Max(eventData.Length + 16, eventAttribute.EventId + 1)];
					Array.Copy(eventData, array, eventData.Length);
					eventData = array;
				}
				eventData[eventAttribute.EventId].Descriptor = new EventDescriptor(eventAttribute.EventId, eventAttribute.Version, (byte)eventAttribute.Channel, (byte)eventAttribute.Level, (byte)eventAttribute.Opcode, (int)eventAttribute.Task, (long)(eventAttribute.Keywords | (EventKeywords)SessionMask.All.ToEventKeywords()));
				eventData[eventAttribute.EventId].Tags = eventAttribute.Tags;
				eventData[eventAttribute.EventId].Name = eventName;
				eventData[eventAttribute.EventId].Parameters = eventParameters;
				eventData[eventAttribute.EventId].Message = eventAttribute.Message;
				eventData[eventAttribute.EventId].ActivityOptions = eventAttribute.ActivityOptions;
				eventData[eventAttribute.EventId].HasRelatedActivityID = hasRelatedActivityID;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005E90 File Offset: 0x00004090
		private static void TrimEventDescriptors(ref EventSource.EventMetadata[] eventData)
		{
			int num = eventData.Length;
			checked
			{
				while (0 < num)
				{
					num--;
					if (eventData[num].Descriptor.EventId != 0)
					{
						break;
					}
				}
				if (eventData.Length - num > 2)
				{
					EventSource.EventMetadata[] array = new EventSource.EventMetadata[num + 1];
					Array.Copy(eventData, array, array.Length);
					eventData = array;
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005EE0 File Offset: 0x000040E0
		internal void AddListener(EventListener listener)
		{
			lock (EventListener.EventListenersLock)
			{
				bool[] eventEnabled = null;
				if (this.m_eventData != null)
				{
					eventEnabled = new bool[this.m_eventData.Length];
				}
				this.m_Dispatchers = new EventDispatcher(this.m_Dispatchers, eventEnabled, listener);
				listener.OnEventSourceCreated(this);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005F54 File Offset: 0x00004154
		private static void DebugCheckEvent(ref Dictionary<string, string> eventsByName, EventSource.EventMetadata[] eventData, MethodInfo method, EventAttribute eventAttribute, ManifestBuilder manifest, EventManifestOptions options)
		{
			int eventId = eventAttribute.EventId;
			string name = method.Name;
			int helperCallFirstArg = EventSource.GetHelperCallFirstArg(method);
			if (helperCallFirstArg >= 0 && eventId != helperCallFirstArg)
			{
				manifest.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_MismatchIdToWriteEvent", new object[]
				{
					name,
					eventId,
					helperCallFirstArg
				}), true);
			}
			if (eventId < eventData.Length && eventData[eventId].Descriptor.EventId != 0)
			{
				manifest.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventIdReused", new object[]
				{
					name,
					eventId,
					eventData[eventId].Name
				}), true);
			}
			checked
			{
				for (int i = 0; i < eventData.Length; i++)
				{
					if (eventData[i].Name != null && eventData[i].Descriptor.Task == (int)eventAttribute.Task && (EventOpcode)eventData[i].Descriptor.Opcode == eventAttribute.Opcode)
					{
						manifest.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TaskOpcodePairReused", new object[]
						{
							name,
							eventId,
							eventData[i].Name,
							i
						}), false);
						if ((options & EventManifestOptions.Strict) == EventManifestOptions.None)
						{
							break;
						}
					}
				}
				if (eventAttribute.Opcode != EventOpcode.Info)
				{
					bool flag = false;
					if (eventAttribute.Task == EventTask.None)
					{
						flag = true;
					}
					else
					{
						EventTask eventTask = (EventTask)65534 - eventId;
						if (eventAttribute.Opcode != EventOpcode.Start && eventAttribute.Opcode != EventOpcode.Stop && eventAttribute.Task == eventTask)
						{
							flag = true;
						}
					}
					if (flag)
					{
						manifest.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventMustHaveTaskIfNonDefaultOpcode", new object[]
						{
							name,
							eventId
						}), false);
					}
				}
				if (eventsByName == null)
				{
					eventsByName = new Dictionary<string, string>();
				}
				if (eventsByName.ContainsKey(name))
				{
					manifest.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventNameReused", new object[]
					{
						name
					}), true);
				}
				eventsByName[name] = name;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00006154 File Offset: 0x00004354
		[SecuritySafeCritical]
		private static int GetHelperCallFirstArg(MethodInfo method)
		{
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			byte[] ilasByteArray = method.GetMethodBody().GetILAsByteArray();
			int num = -1;
			checked
			{
				for (int i = 0; i < ilasByteArray.Length; i++)
				{
					byte b = ilasByteArray[i];
					if (b <= 110)
					{
						switch (b)
						{
						case 0:
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
						case 6:
						case 7:
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 13:
						case 20:
						case 37:
							break;
						case 14:
						case 16:
							i++;
							break;
						case 15:
						case 17:
						case 18:
						case 19:
						case 33:
						case 34:
						case 35:
						case 36:
						case 38:
						case 39:
						case 41:
						case 42:
						case 43:
						case 46:
						case 47:
						case 48:
						case 49:
						case 50:
						case 51:
						case 52:
						case 53:
						case 54:
						case 55:
						case 56:
							return -1;
						case 21:
						case 22:
						case 23:
						case 24:
						case 25:
						case 26:
						case 27:
						case 28:
						case 29:
						case 30:
							if (i > 0 && ilasByteArray[i - 1] == 2)
							{
								num = (int)(ilasByteArray[i] - 22);
							}
							break;
						case 31:
							if (i > 0 && ilasByteArray[i - 1] == 2)
							{
								num = (int)ilasByteArray[i + 1];
							}
							i++;
							break;
						case 32:
							i += 4;
							break;
						case 40:
							i += 4;
							if (num >= 0)
							{
								for (int j = i + 1; j < ilasByteArray.Length; j++)
								{
									if (ilasByteArray[j] == 42)
									{
										return num;
									}
									if (ilasByteArray[j] != 0)
									{
										break;
									}
								}
							}
							num = -1;
							break;
						case 44:
						case 45:
							num = -1;
							i++;
							break;
						case 57:
						case 58:
							num = -1;
							i += 4;
							break;
						default:
							switch (b)
							{
							case 103:
							case 104:
							case 105:
							case 106:
							case 109:
							case 110:
								break;
							case 107:
							case 108:
								return -1;
							default:
								return -1;
							}
							break;
						}
					}
					else
					{
						switch (b)
						{
						case 140:
						case 141:
							i += 4;
							break;
						default:
							if (b != 162)
							{
								if (b != 254)
								{
									return -1;
								}
								i++;
								if (i >= ilasByteArray.Length || ilasByteArray[i] >= 6)
								{
									return -1;
								}
							}
							break;
						}
					}
				}
				return -1;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00006384 File Offset: 0x00004584
		internal void ReportOutOfBandMessage(string msg, bool flush)
		{
			checked
			{
				try
				{
					Debugger.Log(0, null, msg + "\r\n");
					if (this.m_outOfBandMessageCount < 15)
					{
						this.m_outOfBandMessageCount += 1;
					}
					else
					{
						if (this.m_outOfBandMessageCount == 16)
						{
							return;
						}
						this.m_outOfBandMessageCount = 16;
						msg = "Reached message limit.   End of EventSource error messages.";
					}
					this.WriteEventString(EventLevel.LogAlways, -1L, msg);
					this.WriteStringToAllListeners("EventSourceMessage", msg);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00006404 File Offset: 0x00004604
		private EventSourceSettings ValidateSettings(EventSourceSettings settings)
		{
			EventSourceSettings eventSourceSettings = EventSourceSettings.EtwManifestEventFormat | EventSourceSettings.EtwSelfDescribingEventFormat;
			if ((settings & eventSourceSettings) == eventSourceSettings)
			{
				throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_InvalidEventFormat", new object[0]), "settings");
			}
			if ((settings & eventSourceSettings) == EventSourceSettings.Default)
			{
				settings |= EventSourceSettings.EtwSelfDescribingEventFormat;
			}
			return settings;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00006440 File Offset: 0x00004640
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00006450 File Offset: 0x00004650
		private bool ThrowOnEventWriteErrors
		{
			get
			{
				return (this.m_config & EventSourceSettings.ThrowOnEventWriteErrors) != EventSourceSettings.Default;
			}
			set
			{
				if (value)
				{
					this.m_config |= EventSourceSettings.ThrowOnEventWriteErrors;
					return;
				}
				this.m_config &= ~EventSourceSettings.ThrowOnEventWriteErrors;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00006473 File Offset: 0x00004673
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00006483 File Offset: 0x00004683
		private bool SelfDescribingEvents
		{
			get
			{
				return (this.m_config & EventSourceSettings.EtwSelfDescribingEventFormat) != EventSourceSettings.Default;
			}
			set
			{
				if (!value)
				{
					this.m_config |= EventSourceSettings.EtwManifestEventFormat;
					this.m_config &= ~EventSourceSettings.EtwSelfDescribingEventFormat;
					return;
				}
				this.m_config |= EventSourceSettings.EtwSelfDescribingEventFormat;
				this.m_config &= ~EventSourceSettings.EtwManifestEventFormat;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000064C3 File Offset: 0x000046C3
		public EventSource(string eventSourceName) : this(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000064CD File Offset: 0x000046CD
		public EventSource(string eventSourceName, EventSourceSettings config) : this(eventSourceName, config, null)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000064D8 File Offset: 0x000046D8
		public EventSource(string eventSourceName, EventSourceSettings config, params string[] traits) : this((eventSourceName == null) ? default(Guid) : EventSource.GenerateGuidFromName(eventSourceName.ToUpperInvariant()), eventSourceName, config, traits)
		{
			if (eventSourceName == null)
			{
				throw new ArgumentNullException("eventSourceName");
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006518 File Offset: 0x00004718
		[SecuritySafeCritical]
		public void Write(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (!this.IsEnabled())
			{
				return;
			}
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			EmptyStruct emptyStruct = default(EmptyStruct);
			this.WriteImpl<EmptyStruct>(eventName, ref eventSourceOptions, ref emptyStruct, null, null);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000655C File Offset: 0x0000475C
		[SecuritySafeCritical]
		public void Write(string eventName, EventSourceOptions options)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (!this.IsEnabled())
			{
				return;
			}
			EmptyStruct emptyStruct = default(EmptyStruct);
			this.WriteImpl<EmptyStruct>(eventName, ref options, ref emptyStruct, null, null);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006598 File Offset: 0x00004798
		[SecuritySafeCritical]
		public void Write<T>(string eventName, T data)
		{
			if (!this.IsEnabled())
			{
				return;
			}
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			this.WriteImpl<T>(eventName, ref eventSourceOptions, ref data, null, null);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000065C5 File Offset: 0x000047C5
		[SecuritySafeCritical]
		public void Write<T>(string eventName, EventSourceOptions options, T data)
		{
			if (!this.IsEnabled())
			{
				return;
			}
			this.WriteImpl<T>(eventName, ref options, ref data, null, null);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000065DF File Offset: 0x000047DF
		[SecuritySafeCritical]
		public void Write<T>(string eventName, ref EventSourceOptions options, ref T data)
		{
			if (!this.IsEnabled())
			{
				return;
			}
			this.WriteImpl<T>(eventName, ref options, ref data, null, null);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000065F8 File Offset: 0x000047F8
		[SecuritySafeCritical]
		public unsafe void Write<T>(string eventName, ref EventSourceOptions options, ref Guid activityId, ref Guid relatedActivityId, ref T data)
		{
			if (!this.IsEnabled())
			{
				return;
			}
			fixed (Guid* ptr = &activityId, ptr2 = &relatedActivityId)
			{
				this.WriteImpl<T>(eventName, ref options, ref data, ptr, (relatedActivityId == Guid.Empty) ? null : ptr2);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006640 File Offset: 0x00004840
		[SecuritySafeCritical]
		private unsafe void WriteMultiMerge(string eventName, ref EventSourceOptions options, TraceLoggingEventTypes eventTypes, Guid* activityID, Guid* childActivityID, params object[] values)
		{
			if (!this.IsEnabled())
			{
				return;
			}
			byte level = ((options.valuesSet & 4) != 0) ? options.level : eventTypes.level;
			EventKeywords keywords = ((options.valuesSet & 1) != 0) ? options.keywords : eventTypes.keywords;
			if (this.IsEnabled((EventLevel)level, keywords))
			{
				this.WriteMultiMergeInner(eventName, ref options, eventTypes, activityID, childActivityID, values);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000066A4 File Offset: 0x000048A4
		[SecuritySafeCritical]
		private unsafe void WriteMultiMergeInner(string eventName, ref EventSourceOptions options, TraceLoggingEventTypes eventTypes, Guid* activityID, Guid* childActivityID, params object[] values)
		{
			byte level = ((options.valuesSet & 4) != 0) ? options.level : eventTypes.level;
			byte opcode = ((options.valuesSet & 8) != 0) ? options.opcode : eventTypes.opcode;
			EventTags tags = ((options.valuesSet & 2) != 0) ? options.tags : eventTypes.Tags;
			EventKeywords keywords = ((options.valuesSet & 1) != 0) ? options.keywords : eventTypes.keywords;
			NameInfo nameInfo = eventTypes.GetNameInfo(eventName ?? eventTypes.Name, tags);
			if (nameInfo == null)
			{
				return;
			}
			int identity = nameInfo.identity;
			EventDescriptor eventDescriptor = new EventDescriptor(identity, level, opcode, (long)keywords);
			int pinCount = eventTypes.pinCount;
			byte* scratch = stackalloc byte[(UIntPtr)eventTypes.scratchSize];
			checked
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)(checked(eventTypes.dataCount + 3))) * (UIntPtr)sizeof(EventSource.EventData)];
				GCHandle* ptr2 = stackalloc GCHandle[unchecked((UIntPtr)pinCount) * (UIntPtr)sizeof(GCHandle)];
				fixed (byte* ptr3 = this.providerMetadata, nameMetadata = nameInfo.nameMetadata, typeMetadata = eventTypes.typeMetadata)
				{
					ptr->SetMetadata(ptr3, this.providerMetadata.Length, 2);
					unchecked
					{
						ptr[1].SetMetadata(nameMetadata, nameInfo.nameMetadata.Length, 1);
						ptr[2].SetMetadata(typeMetadata, eventTypes.typeMetadata.Length, 1);
						RuntimeHelpers.PrepareConstrainedRegions();
					}
					try
					{
						DataCollector.ThreadInstance.Enable(scratch, eventTypes.scratchSize, ptr + 3, eventTypes.dataCount, ptr2, pinCount);
						for (int i = 0; i < eventTypes.typeInfos.Length; i++)
						{
							eventTypes.typeInfos[i].WriteObjectData(TraceLoggingDataCollector.Instance, values[i]);
						}
						this.WriteEventRaw(ref eventDescriptor, activityID, childActivityID, (int)(unchecked((long)(DataCollector.ThreadInstance.Finish() - ptr))), (IntPtr)((void*)ptr));
					}
					finally
					{
						this.WriteCleanup(ptr2, pinCount);
					}
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000068DC File Offset: 0x00004ADC
		[SecuritySafeCritical]
		internal unsafe void WriteMultiMerge(string eventName, ref EventSourceOptions options, TraceLoggingEventTypes eventTypes, Guid* activityID, Guid* childActivityID, EventSource.EventData* data)
		{
			if (!this.IsEnabled())
			{
				return;
			}
			EventDescriptor eventDescriptor;
			NameInfo nameInfo = this.UpdateDescriptor(eventName, eventTypes, ref options, out eventDescriptor);
			if (nameInfo != null)
			{
				EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)(checked(eventTypes.dataCount + eventTypes.typeInfos.Length * 2 + 3))) * (UIntPtr)sizeof(EventSource.EventData))];
				fixed (byte* ptr2 = this.providerMetadata, nameMetadata = nameInfo.nameMetadata, typeMetadata = eventTypes.typeMetadata)
				{
					ptr->SetMetadata(ptr2, this.providerMetadata.Length, 2);
					ptr[1].SetMetadata(nameMetadata, nameInfo.nameMetadata.Length, 1);
					ptr[2].SetMetadata(typeMetadata, eventTypes.typeMetadata.Length, 1);
					int num = 3;
					checked
					{
						for (int i = 0; i < eventTypes.typeInfos.Length; i++)
						{
							if (eventTypes.typeInfos[i].DataType == typeof(string))
							{
								unchecked
								{
									ptr[num].m_Ptr = &ptr[checked(num + 1)].m_Size;
									ptr[num].m_Size = 2;
								}
								num++;
								unchecked
								{
									ptr[num].m_Ptr = data[i].m_Ptr;
									ptr[num].m_Size = checked(unchecked(data[i]).m_Size - 2);
								}
								num++;
							}
							else
							{
								unchecked
								{
									ptr[num].m_Ptr = data[i].m_Ptr;
									ptr[num].m_Size = data[i].m_Size;
									if (data[i].m_Size == 4 && eventTypes.typeInfos[i].DataType == typeof(bool))
									{
										ptr[num].m_Size = 1;
									}
								}
								num++;
							}
						}
						this.WriteEventRaw(ref eventDescriptor, activityID, childActivityID, num, (IntPtr)((void*)ptr));
					}
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006B50 File Offset: 0x00004D50
		[SecuritySafeCritical]
		private unsafe void WriteImpl<T>(string eventName, ref EventSourceOptions options, ref T data, Guid* pActivityId, Guid* pRelatedActivityId)
		{
			try
			{
				SimpleEventTypes<T> instance = SimpleEventTypes<T>.Instance;
				options.Opcode = (options.IsOpcodeSet ? options.Opcode : EventSource.GetOpcodeWithDefault(options.Opcode, eventName));
				EventDescriptor eventDescriptor;
				NameInfo nameInfo = this.UpdateDescriptor(eventName, instance, ref options, out eventDescriptor);
				if (nameInfo != null)
				{
					int pinCount = instance.pinCount;
					byte* scratch = stackalloc byte[(UIntPtr)instance.scratchSize];
					checked
					{
						EventSource.EventData* ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)(checked(instance.dataCount + 3))) * (UIntPtr)sizeof(EventSource.EventData)];
						GCHandle* ptr2 = stackalloc GCHandle[unchecked((UIntPtr)pinCount) * (UIntPtr)sizeof(GCHandle)];
						try
						{
							fixed (byte* ptr3 = this.providerMetadata)
							{
								fixed (byte* ptr4 = nameInfo.nameMetadata)
								{
									fixed (byte* ptr5 = instance.typeMetadata)
									{
										ptr->SetMetadata(ptr3, this.providerMetadata.Length, 2);
										unchecked
										{
											ptr[1].SetMetadata(ptr4, nameInfo.nameMetadata.Length, 1);
											ptr[2].SetMetadata(ptr5, instance.typeMetadata.Length, 1);
											RuntimeHelpers.PrepareConstrainedRegions();
											EventOpcode opcode = (EventOpcode)eventDescriptor.Opcode;
											Guid empty = Guid.Empty;
											Guid empty2 = Guid.Empty;
											if (pActivityId == null && pRelatedActivityId == null && (options.ActivityOptions & EventActivityOptions.Disable) == EventActivityOptions.None)
											{
												if (opcode == EventOpcode.Start)
												{
													this.m_activityTracker.OnStart(this.m_name, eventName, 0, ref empty, ref empty2, options.ActivityOptions);
												}
												else if (opcode == EventOpcode.Stop)
												{
													this.m_activityTracker.OnStop(this.m_name, eventName, 0, ref empty);
												}
												if (empty != Guid.Empty)
												{
													pActivityId = &empty;
												}
												if (empty2 != Guid.Empty)
												{
													pRelatedActivityId = &empty2;
												}
											}
										}
										try
										{
											DataCollector.ThreadInstance.Enable(scratch, instance.scratchSize, ptr + 3, instance.dataCount, ptr2, pinCount);
											instance.typeInfo.WriteData(TraceLoggingDataCollector.Instance, ref data);
											this.WriteEventRaw(ref eventDescriptor, pActivityId, pRelatedActivityId, (int)(unchecked((long)(DataCollector.ThreadInstance.Finish() - ptr))), (IntPtr)((void*)ptr));
											if (this.m_Dispatchers != null)
											{
												EventPayload payload = (EventPayload)instance.typeInfo.GetData(data);
												this.WriteToAllListeners(eventName, ref eventDescriptor, nameInfo.tags, pActivityId, payload);
											}
										}
										catch (Exception ex)
										{
											if (ex is EventSourceException)
											{
												throw;
											}
											this.ThrowEventSourceException(ex);
										}
										finally
										{
											this.WriteCleanup(ptr2, pinCount);
										}
									}
								}
							}
						}
						finally
						{
							byte* ptr3 = null;
							byte* ptr4 = null;
							byte* ptr5 = null;
						}
					}
				}
			}
			catch (Exception ex2)
			{
				if (ex2 is EventSourceException)
				{
					throw;
				}
				this.ThrowEventSourceException(ex2);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00006E5C File Offset: 0x0000505C
		[SecurityCritical]
		private unsafe void WriteToAllListeners(string eventName, ref EventDescriptor eventDescriptor, EventTags tags, Guid* pActivityId, EventPayload payload)
		{
			EventWrittenEventArgs eventWrittenEventArgs = new EventWrittenEventArgs(this);
			eventWrittenEventArgs.EventName = eventName;
			eventWrittenEventArgs.m_level = (EventLevel)eventDescriptor.Level;
			eventWrittenEventArgs.m_keywords = (EventKeywords)eventDescriptor.Keywords;
			eventWrittenEventArgs.m_opcode = (EventOpcode)eventDescriptor.Opcode;
			eventWrittenEventArgs.m_tags = tags;
			eventWrittenEventArgs.EventId = -1;
			if (pActivityId != null)
			{
				eventWrittenEventArgs.RelatedActivityId = *pActivityId;
			}
			if (payload != null)
			{
				eventWrittenEventArgs.Payload = new ReadOnlyCollection<object>((IList<object>)payload.Values);
				eventWrittenEventArgs.PayloadNames = new ReadOnlyCollection<string>((IList<string>)payload.Keys);
			}
			this.DispatchToAllListeners(-1, pActivityId, eventWrittenEventArgs);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006EF8 File Offset: 0x000050F8
		[SecurityCritical]
		[NonEvent]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private unsafe void WriteCleanup(GCHandle* pPins, int cPins)
		{
			DataCollector.ThreadInstance.Disable();
			for (int num = 0; num != cPins; num = checked(num + 1))
			{
				if (IntPtr.Zero != (IntPtr)pPins[num])
				{
					pPins[num].Free();
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006F4C File Offset: 0x0000514C
		private void InitializeProviderMetadata()
		{
			checked
			{
				if (this.m_traits != null)
				{
					List<byte> list = new List<byte>(100);
					for (int i = 0; i < this.m_traits.Length - 1; i += 2)
					{
						if (this.m_traits[i].StartsWith("ETW_"))
						{
							string text = this.m_traits[i].Substring(4);
							byte item;
							if (!byte.TryParse(text, out item))
							{
								if (!(text == "GROUP"))
								{
									throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("UnknownEtwTrait", new object[]
									{
										text
									}), "traits");
								}
								item = 1;
							}
							string value = this.m_traits[i + 1];
							int count = list.Count;
							list.Add(0);
							list.Add(0);
							list.Add(item);
							int num = EventSource.AddValueToMetaData(list, value) + 3;
							list[count] = unchecked((byte)num);
							list[count + 1] = unchecked((byte)(num >> 8));
						}
					}
					this.providerMetadata = Statics.MetadataForString(this.Name, 0, list.Count, 0);
					int num2 = this.providerMetadata.Length - list.Count;
					using (List<byte>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							byte b = enumerator.Current;
							this.providerMetadata[num2++] = b;
						}
						return;
					}
				}
				this.providerMetadata = Statics.MetadataForString(this.Name, 0, 0, 0);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000070C8 File Offset: 0x000052C8
		private static int AddValueToMetaData(List<byte> metaData, string value)
		{
			if (value.Length == 0)
			{
				return 0;
			}
			int count = metaData.Count;
			char c = value[0];
			checked
			{
				if (c == '@')
				{
					metaData.AddRange(Encoding.UTF8.GetBytes(value.Substring(1)));
				}
				else if (c == '{')
				{
					metaData.AddRange(new Guid(value).ToByteArray());
				}
				else if (c == '#')
				{
					for (int i = 1; i < value.Length; i++)
					{
						if (value[i] != ' ')
						{
							if (i + 1 >= value.Length)
							{
								throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EvenHexDigits", new object[0]), "traits");
							}
							metaData.Add((byte)(EventSource.HexDigit(value[i]) * 16 + EventSource.HexDigit(value[i + 1])));
							i++;
						}
					}
				}
				else
				{
					if ('A' > c && ' ' != c)
					{
						throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("IllegalValue", new object[]
						{
							value
						}), "traits");
					}
					metaData.AddRange(Encoding.UTF8.GetBytes(value));
				}
				return metaData.Count - count;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000071E8 File Offset: 0x000053E8
		private static int HexDigit(char c)
		{
			if ('0' <= c && c <= '9')
			{
				return (int)(checked(c - '0'));
			}
			if ('a' <= c)
			{
				c -= ' ';
			}
			if ('A' <= c && c <= 'F')
			{
				return (int)(checked(c - 'A' + '\n'));
			}
			throw new ArgumentException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("BadHexDigit", new object[]
			{
				c
			}), "traits");
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00007248 File Offset: 0x00005448
		private NameInfo UpdateDescriptor(string name, TraceLoggingEventTypes eventInfo, ref EventSourceOptions options, out EventDescriptor descriptor)
		{
			NameInfo nameInfo = null;
			int traceloggingId = 0;
			byte level = ((options.valuesSet & 4) != 0) ? options.level : eventInfo.level;
			byte opcode = ((options.valuesSet & 8) != 0) ? options.opcode : eventInfo.opcode;
			EventTags tags = ((options.valuesSet & 2) != 0) ? options.tags : eventInfo.Tags;
			EventKeywords keywords = ((options.valuesSet & 1) != 0) ? options.keywords : eventInfo.keywords;
			if (this.IsEnabled((EventLevel)level, keywords))
			{
				nameInfo = eventInfo.GetNameInfo(name ?? eventInfo.Name, tags);
				traceloggingId = nameInfo.identity;
			}
			descriptor = new EventDescriptor(traceloggingId, level, opcode, (long)keywords);
			return nameInfo;
		}

		// Token: 0x04000015 RID: 21
		internal const string s_ActivityStartSuffix = "Start";

		// Token: 0x04000016 RID: 22
		internal const string s_ActivityStopSuffix = "Stop";

		// Token: 0x04000017 RID: 23
		private string m_name;

		// Token: 0x04000018 RID: 24
		internal int m_id;

		// Token: 0x04000019 RID: 25
		private Guid m_guid;

		// Token: 0x0400001A RID: 26
		internal volatile EventSource.EventMetadata[] m_eventData;

		// Token: 0x0400001B RID: 27
		private volatile byte[] m_rawManifest;

		// Token: 0x0400001C RID: 28
		private EventHandler<EventCommandEventArgs> m_eventCommandExecuted;

		// Token: 0x0400001D RID: 29
		private EventSourceSettings m_config;

		// Token: 0x0400001E RID: 30
		private bool m_eventSourceEnabled;

		// Token: 0x0400001F RID: 31
		internal EventLevel m_level;

		// Token: 0x04000020 RID: 32
		internal EventKeywords m_matchAnyKeyword;

		// Token: 0x04000021 RID: 33
		internal volatile EventDispatcher m_Dispatchers;

		// Token: 0x04000022 RID: 34
		private volatile EventSource.OverideEventProvider m_provider;

		// Token: 0x04000023 RID: 35
		private bool m_completelyInited;

		// Token: 0x04000024 RID: 36
		private Exception m_constructionException;

		// Token: 0x04000025 RID: 37
		private byte m_outOfBandMessageCount;

		// Token: 0x04000026 RID: 38
		private EventCommandEventArgs m_deferredCommands;

		// Token: 0x04000027 RID: 39
		private string[] m_traits;

		// Token: 0x04000028 RID: 40
		internal static uint s_currentPid;

		// Token: 0x04000029 RID: 41
		[ThreadStatic]
		private static byte m_EventSourceExceptionRecurenceCount = 0;

		// Token: 0x0400002A RID: 42
		internal volatile ulong[] m_channelData;

		// Token: 0x0400002B RID: 43
		private ActivityTracker m_activityTracker;

		// Token: 0x0400002C RID: 44
		private static readonly byte[] namespaceBytes = new byte[]
		{
			72,
			44,
			45,
			178,
			195,
			144,
			71,
			200,
			135,
			248,
			26,
			21,
			191,
			193,
			48,
			251
		};

		// Token: 0x0400002D RID: 45
		private byte[] providerMetadata;

		// Token: 0x02000006 RID: 6
		protected internal struct EventData
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000087 RID: 135 RVA: 0x00007327 File Offset: 0x00005527
			// (set) Token: 0x06000088 RID: 136 RVA: 0x00007334 File Offset: 0x00005534
			public IntPtr DataPointer
			{
				get
				{
					return (IntPtr)this.m_Ptr;
				}
				set
				{
					this.m_Ptr = (long)value;
				}
			}

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000089 RID: 137 RVA: 0x00007342 File Offset: 0x00005542
			// (set) Token: 0x0600008A RID: 138 RVA: 0x0000734A File Offset: 0x0000554A
			public int Size
			{
				get
				{
					return this.m_Size;
				}
				set
				{
					this.m_Size = value;
				}
			}

			// Token: 0x0600008B RID: 139 RVA: 0x00007353 File Offset: 0x00005553
			[SecurityCritical]
			internal unsafe void SetMetadata(byte* pointer, int size, int reserved)
			{
				this.m_Ptr = checked((long)((ulong)((UIntPtr)((void*)pointer))));
				this.m_Size = size;
				this.m_Reserved = reserved;
			}

			// Token: 0x0400002E RID: 46
			internal long m_Ptr;

			// Token: 0x0400002F RID: 47
			internal int m_Size;

			// Token: 0x04000030 RID: 48
			internal int m_Reserved;
		}

		// Token: 0x02000007 RID: 7
		private struct Sha1ForNonSecretPurposes
		{
			// Token: 0x0600008C RID: 140 RVA: 0x00007378 File Offset: 0x00005578
			public void Start()
			{
				if (this.w == null)
				{
					this.w = new uint[85];
				}
				this.length = 0L;
				this.pos = 0;
				this.w[80] = 1732584193U;
				this.w[81] = 4023233417U;
				this.w[82] = 2562383102U;
				this.w[83] = 271733878U;
				this.w[84] = 3285377520U;
			}

			// Token: 0x0600008D RID: 141 RVA: 0x000073F0 File Offset: 0x000055F0
			public void Append(byte input)
			{
				this.w[this.pos / 4] = (this.w[this.pos / 4] << 8 | (uint)input);
				if (64 == checked(++this.pos))
				{
					this.Drain();
				}
			}

			// Token: 0x0600008E RID: 142 RVA: 0x0000743C File Offset: 0x0000563C
			public void Append(byte[] input)
			{
				foreach (byte input2 in input)
				{
					this.Append(input2);
				}
			}

			// Token: 0x0600008F RID: 143 RVA: 0x00007464 File Offset: 0x00005664
			public void Finish(byte[] output)
			{
				long num = checked(this.length + unchecked((long)(checked(8 * this.pos))));
				this.Append(128);
				while (this.pos != 56)
				{
					this.Append(0);
				}
				this.Append((byte)(num >> 56));
				this.Append((byte)(num >> 48));
				this.Append((byte)(num >> 40));
				this.Append((byte)(num >> 32));
				this.Append((byte)(num >> 24));
				this.Append((byte)(num >> 16));
				this.Append((byte)(num >> 8));
				this.Append((byte)num);
				int num2 = (output.Length < 20) ? output.Length : 20;
				for (int num3 = 0; num3 != num2; num3++)
				{
					uint num4 = this.w[80 + num3 / 4];
					output[num3] = (byte)(num4 >> 24);
					this.w[80 + num3 / 4] = num4 << 8;
				}
			}

			// Token: 0x06000090 RID: 144 RVA: 0x00007538 File Offset: 0x00005738
			private void Drain()
			{
				uint num2;
				uint num3;
				uint num4;
				uint num5;
				uint num6;
				checked
				{
					for (int num = 16; num != 80; num++)
					{
						this.w[num] = EventSource.Sha1ForNonSecretPurposes.Rol1(this.w[num - 3] ^ this.w[num - 8] ^ this.w[num - 14] ^ this.w[num - 16]);
					}
					num2 = this.w[80];
					num3 = this.w[81];
					num4 = this.w[82];
					num5 = this.w[83];
					num6 = this.w[84];
				}
				for (int num7 = 0; num7 != 20; num7++)
				{
					uint num8 = (num3 & num4) | (~num3 & num5);
					uint num9 = EventSource.Sha1ForNonSecretPurposes.Rol5(num2) + num8 + num6 + 1518500249U + this.w[num7];
					num6 = num5;
					num5 = num4;
					num4 = EventSource.Sha1ForNonSecretPurposes.Rol30(num3);
					num3 = num2;
					num2 = num9;
				}
				for (int num10 = 20; num10 != 40; num10++)
				{
					uint num11 = num3 ^ num4 ^ num5;
					uint num12 = EventSource.Sha1ForNonSecretPurposes.Rol5(num2) + num11 + num6 + 1859775393U + this.w[num10];
					num6 = num5;
					num5 = num4;
					num4 = EventSource.Sha1ForNonSecretPurposes.Rol30(num3);
					num3 = num2;
					num2 = num12;
				}
				for (int num13 = 40; num13 != 60; num13++)
				{
					uint num14 = (num3 & num4) | (num3 & num5) | (num4 & num5);
					uint num15 = EventSource.Sha1ForNonSecretPurposes.Rol5(num2) + num14 + num6 + 2400959708U + this.w[num13];
					num6 = num5;
					num5 = num4;
					num4 = EventSource.Sha1ForNonSecretPurposes.Rol30(num3);
					num3 = num2;
					num2 = num15;
				}
				for (int num16 = 60; num16 != 80; num16++)
				{
					uint num17 = num3 ^ num4 ^ num5;
					uint num18 = EventSource.Sha1ForNonSecretPurposes.Rol5(num2) + num17 + num6 + 3395469782U + this.w[num16];
					num6 = num5;
					num5 = num4;
					num4 = EventSource.Sha1ForNonSecretPurposes.Rol30(num3);
					num3 = num2;
					num2 = num18;
				}
				this.w[80] += num2;
				this.w[81] += num3;
				this.w[82] += num4;
				this.w[83] += num5;
				this.w[84] += num6;
				checked
				{
					this.length += 512L;
					this.pos = 0;
				}
			}

			// Token: 0x06000091 RID: 145 RVA: 0x00007794 File Offset: 0x00005994
			private static uint Rol1(uint input)
			{
				return input << 1 | input >> 31;
			}

			// Token: 0x06000092 RID: 146 RVA: 0x0000779E File Offset: 0x0000599E
			private static uint Rol5(uint input)
			{
				return input << 5 | input >> 27;
			}

			// Token: 0x06000093 RID: 147 RVA: 0x000077A8 File Offset: 0x000059A8
			private static uint Rol30(uint input)
			{
				return input << 30 | input >> 2;
			}

			// Token: 0x04000031 RID: 49
			private long length;

			// Token: 0x04000032 RID: 50
			private uint[] w;

			// Token: 0x04000033 RID: 51
			private int pos;
		}

		// Token: 0x0200000C RID: 12
		private class OverideEventProvider : EventProvider
		{
			// Token: 0x060000B8 RID: 184 RVA: 0x00008A59 File Offset: 0x00006C59
			public OverideEventProvider(EventSource eventSource)
			{
				this.m_eventSource = eventSource;
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00008A68 File Offset: 0x00006C68
			protected override void OnControllerCommand(ControllerCommand command, IDictionary<string, string> arguments, int perEventSourceSessionId, int etwSessionId)
			{
				EventListener listener = null;
				this.m_eventSource.SendCommand(listener, perEventSourceSessionId, etwSessionId, (EventCommand)command, base.IsEnabled(), base.Level, base.MatchAnyKeyword, arguments);
			}

			// Token: 0x04000052 RID: 82
			private EventSource m_eventSource;
		}

		// Token: 0x0200000D RID: 13
		internal struct EventMetadata
		{
			// Token: 0x04000053 RID: 83
			public EventDescriptor Descriptor;

			// Token: 0x04000054 RID: 84
			public EventTags Tags;

			// Token: 0x04000055 RID: 85
			public bool EnabledForAnyListener;

			// Token: 0x04000056 RID: 86
			public bool EnabledForETW;

			// Token: 0x04000057 RID: 87
			public bool HasRelatedActivityID;

			// Token: 0x04000058 RID: 88
			public byte TriggersActivityTracking;

			// Token: 0x04000059 RID: 89
			public string Name;

			// Token: 0x0400005A RID: 90
			public string Message;

			// Token: 0x0400005B RID: 91
			public ParameterInfo[] Parameters;

			// Token: 0x0400005C RID: 92
			public TraceLoggingEventTypes TraceLoggingEventTypes;

			// Token: 0x0400005D RID: 93
			public EventActivityOptions ActivityOptions;
		}
	}
}
