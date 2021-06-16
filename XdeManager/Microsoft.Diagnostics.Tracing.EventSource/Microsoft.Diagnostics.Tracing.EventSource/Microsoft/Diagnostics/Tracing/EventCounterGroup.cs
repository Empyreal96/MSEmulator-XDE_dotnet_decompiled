using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000015 RID: 21
	internal class EventCounterGroup : IDisposable
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x000090DD File Offset: 0x000072DD
		internal EventCounterGroup(EventSource eventSource, int eventSourceIndex)
		{
			this._eventSource = eventSource;
			this._eventSourceIndex = eventSourceIndex;
			this._eventCounters = new List<EventCounter>();
			this.RegisterCommandCallback();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00009104 File Offset: 0x00007304
		private void Add(EventCounter eventCounter)
		{
			this._eventCounters.Add(eventCounter);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00009112 File Offset: 0x00007312
		private void RegisterCommandCallback()
		{
			this._eventSource.EventCommandExecuted += this.OnEventSourceCommand;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000912C File Offset: 0x0000732C
		private void OnEventSourceCommand(object sender, EventCommandEventArgs e)
		{
			string s;
			float pollingIntervalInSeconds;
			if ((e.Command == EventCommand.Enable || e.Command == EventCommand.Update) && e.Arguments.TryGetValue("EventCounterIntervalSec", out s) && float.TryParse(s, out pollingIntervalInSeconds))
			{
				this.EnableTimer(pollingIntervalInSeconds);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00009170 File Offset: 0x00007370
		internal static void AddEventCounter(EventSource eventSource, EventCounter eventCounter)
		{
			int eventSourceIndex = EventListener.EventSourceIndex(eventSource);
			EventCounterGroup.EnsureEventSourceIndexAvailable(eventSourceIndex);
			EventCounterGroup eventCounterGroup = EventCounterGroup.GetEventCounterGroup(eventSource);
			eventCounterGroup.Add(eventCounter);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00009198 File Offset: 0x00007398
		private static void EnsureEventSourceIndexAvailable(int eventSourceIndex)
		{
			checked
			{
				if (EventCounterGroup.s_eventCounterGroups == null)
				{
					EventCounterGroup.s_eventCounterGroups = new EventCounterGroup[eventSourceIndex + 1];
					return;
				}
				if (eventSourceIndex >= EventCounterGroup.s_eventCounterGroups.Length)
				{
					EventCounterGroup[] destinationArray = new EventCounterGroup[eventSourceIndex + 1];
					Array.Copy(EventCounterGroup.s_eventCounterGroups, destinationArray, EventCounterGroup.s_eventCounterGroups.Length);
					EventCounterGroup.s_eventCounterGroups = destinationArray;
				}
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000091E8 File Offset: 0x000073E8
		private static EventCounterGroup GetEventCounterGroup(EventSource eventSource)
		{
			int num = EventListener.EventSourceIndex(eventSource);
			EventCounterGroup eventCounterGroup = EventCounterGroup.s_eventCounterGroups[num];
			if (eventCounterGroup == null)
			{
				eventCounterGroup = new EventCounterGroup(eventSource, num);
				EventCounterGroup.s_eventCounterGroups[num] = eventCounterGroup;
			}
			return eventCounterGroup;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00009218 File Offset: 0x00007418
		private void EnableTimer(float pollingIntervalInSeconds)
		{
			if (pollingIntervalInSeconds == 0f)
			{
				if (this._pollingTimer != null)
				{
					this._pollingTimer.Dispose();
					this._pollingTimer = null;
				}
				this._pollingIntervalInMilliseconds = 0;
				return;
			}
			if (this._pollingIntervalInMilliseconds == 0 || pollingIntervalInSeconds < (float)this._pollingIntervalInMilliseconds)
			{
				this._pollingIntervalInMilliseconds = checked((int)(unchecked(pollingIntervalInSeconds * 1000f)));
				if (this._pollingTimer != null)
				{
					this._pollingTimer.Dispose();
					this._pollingTimer = null;
				}
				this._timeStampSinceCollectionStarted = DateTime.Now;
				this._pollingTimer = new Timer(new TimerCallback(this.OnTimer), null, this._pollingIntervalInMilliseconds, this._pollingIntervalInMilliseconds);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00009374 File Offset: 0x00007574
		private void OnTimer(object state)
		{
			if (this._eventSource.IsEnabled())
			{
				DateTime now = DateTime.Now;
				TimeSpan timeSpan = now - this._timeStampSinceCollectionStarted;
				lock (this._pollingTimer)
				{
					foreach (EventCounter eventCounter in this._eventCounters)
					{
						EventCounterPayload eventCounterPayload = eventCounter.GetEventCounterPayload();
						eventCounterPayload.IntervalSec = (float)timeSpan.TotalSeconds;
						this._eventSource.Write("EventCounters", new EventSourceOptions
						{
							Level = EventLevel.LogAlways
						}, new
						{
							Payload = eventCounterPayload
						});
					}
					this._timeStampSinceCollectionStarted = now;
					return;
				}
			}
			this._pollingTimer.Dispose();
			this._pollingTimer = null;
			EventCounterGroup.s_eventCounterGroups[this._eventSourceIndex] = null;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00009474 File Offset: 0x00007674
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000947D File Offset: 0x0000767D
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && this._pollingTimer != null)
			{
				this._pollingTimer.Dispose();
				this._pollingTimer = null;
			}
			this._disposed = true;
		}

		// Token: 0x04000078 RID: 120
		private readonly EventSource _eventSource;

		// Token: 0x04000079 RID: 121
		private readonly int _eventSourceIndex;

		// Token: 0x0400007A RID: 122
		private readonly List<EventCounter> _eventCounters;

		// Token: 0x0400007B RID: 123
		private static EventCounterGroup[] s_eventCounterGroups;

		// Token: 0x0400007C RID: 124
		private DateTime _timeStampSinceCollectionStarted;

		// Token: 0x0400007D RID: 125
		private int _pollingIntervalInMilliseconds;

		// Token: 0x0400007E RID: 126
		private Timer _pollingTimer;

		// Token: 0x0400007F RID: 127
		private bool _disposed;
	}
}
