using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000074 RID: 116
	internal sealed class EventSourceActivity : IDisposable
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000F3FD File Offset: 0x0000D5FD
		public EventSourceActivity(EventSource eventSource)
		{
			if (eventSource == null)
			{
				throw new ArgumentNullException("eventSource");
			}
			this.eventSource = eventSource;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000F41A File Offset: 0x0000D61A
		public static implicit operator EventSourceActivity(EventSource eventSource)
		{
			return new EventSourceActivity(eventSource);
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000F422 File Offset: 0x0000D622
		public EventSource EventSource
		{
			get
			{
				return this.eventSource;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000F42A File Offset: 0x0000D62A
		public Guid Id
		{
			get
			{
				return this.activityId;
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000F432 File Offset: 0x0000D632
		public EventSourceActivity Start<T>(string eventName, EventSourceOptions options, T data)
		{
			return this.Start<T>(eventName, ref options, ref data);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000F440 File Offset: 0x0000D640
		public EventSourceActivity Start(string eventName)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			EmptyStruct emptyStruct = default(EmptyStruct);
			return this.Start<EmptyStruct>(eventName, ref eventSourceOptions, ref emptyStruct);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000F468 File Offset: 0x0000D668
		public EventSourceActivity Start(string eventName, EventSourceOptions options)
		{
			EmptyStruct emptyStruct = default(EmptyStruct);
			return this.Start<EmptyStruct>(eventName, ref options, ref emptyStruct);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000F488 File Offset: 0x0000D688
		public EventSourceActivity Start<T>(string eventName, T data)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			return this.Start<T>(eventName, ref eventSourceOptions, ref data);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
		public void Stop<T>(T data)
		{
			this.Stop<T>(null, ref data);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		public void Stop<T>(string eventName)
		{
			EmptyStruct emptyStruct = default(EmptyStruct);
			this.Stop<EmptyStruct>(eventName, ref emptyStruct);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F4D2 File Offset: 0x0000D6D2
		public void Stop<T>(string eventName, T data)
		{
			this.Stop<T>(eventName, ref data);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F4DD File Offset: 0x0000D6DD
		public void Write<T>(string eventName, EventSourceOptions options, T data)
		{
			this.Write<T>(this.eventSource, eventName, ref options, ref data);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F4F0 File Offset: 0x0000D6F0
		public void Write<T>(string eventName, T data)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			this.Write<T>(this.eventSource, eventName, ref eventSourceOptions, ref data);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F518 File Offset: 0x0000D718
		public void Write(string eventName, EventSourceOptions options)
		{
			EmptyStruct emptyStruct = default(EmptyStruct);
			this.Write<EmptyStruct>(this.eventSource, eventName, ref options, ref emptyStruct);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F540 File Offset: 0x0000D740
		public void Write(string eventName)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			EmptyStruct emptyStruct = default(EmptyStruct);
			this.Write<EmptyStruct>(this.eventSource, eventName, ref eventSourceOptions, ref emptyStruct);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F56E File Offset: 0x0000D76E
		public void Write<T>(EventSource source, string eventName, EventSourceOptions options, T data)
		{
			this.Write<T>(source, eventName, ref options, ref data);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F57C File Offset: 0x0000D77C
		public void Dispose()
		{
			if (this.state == EventSourceActivity.State.Started)
			{
				EmptyStruct emptyStruct = default(EmptyStruct);
				this.Stop<EmptyStruct>(null, ref emptyStruct);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		private EventSourceActivity Start<T>(string eventName, ref EventSourceOptions options, ref T data)
		{
			if (this.state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			if (!this.eventSource.IsEnabled())
			{
				return this;
			}
			EventSourceActivity eventSourceActivity = new EventSourceActivity(this.eventSource);
			if (!this.eventSource.IsEnabled(options.Level, options.Keywords))
			{
				Guid id = this.Id;
				eventSourceActivity.activityId = Guid.NewGuid();
				eventSourceActivity.startStopOptions = options;
				eventSourceActivity.eventName = eventName;
				eventSourceActivity.startStopOptions.Opcode = EventOpcode.Start;
				this.eventSource.Write<T>(eventName, ref eventSourceActivity.startStopOptions, ref eventSourceActivity.activityId, ref id, ref data);
			}
			else
			{
				eventSourceActivity.activityId = this.Id;
			}
			return eventSourceActivity;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F64E File Offset: 0x0000D84E
		private void Write<T>(EventSource eventSource, string eventName, ref EventSourceOptions options, ref T data)
		{
			if (this.state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			if (eventName == null)
			{
				throw new ArgumentNullException();
			}
			eventSource.Write<T>(eventName, ref options, ref this.activityId, ref EventSourceActivity.s_empty, ref data);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F67C File Offset: 0x0000D87C
		private void Stop<T>(string eventName, ref T data)
		{
			if (this.state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			if (!this.StartEventWasFired)
			{
				return;
			}
			this.state = EventSourceActivity.State.Stopped;
			if (eventName == null)
			{
				eventName = this.eventName;
				if (eventName.EndsWith("Start"))
				{
					eventName = eventName.Substring(0, checked(eventName.Length - 5));
				}
				eventName += "Stop";
			}
			this.startStopOptions.Opcode = EventOpcode.Stop;
			this.eventSource.Write<T>(eventName, ref this.startStopOptions, ref this.activityId, ref EventSourceActivity.s_empty, ref data);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000F707 File Offset: 0x0000D907
		private bool StartEventWasFired
		{
			get
			{
				return this.eventName != null;
			}
		}

		// Token: 0x04000149 RID: 329
		private readonly EventSource eventSource;

		// Token: 0x0400014A RID: 330
		private EventSourceOptions startStopOptions;

		// Token: 0x0400014B RID: 331
		internal Guid activityId;

		// Token: 0x0400014C RID: 332
		private EventSourceActivity.State state;

		// Token: 0x0400014D RID: 333
		private string eventName;

		// Token: 0x0400014E RID: 334
		internal static Guid s_empty;

		// Token: 0x02000075 RID: 117
		private enum State
		{
			// Token: 0x04000150 RID: 336
			Started,
			// Token: 0x04000151 RID: 337
			Stopped
		}
	}
}
