using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000027 RID: 39
	public abstract class EventListener : IDisposable
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000154 RID: 340 RVA: 0x0000A704 File Offset: 0x00008904
		// (remove) Token: 0x06000155 RID: 341 RVA: 0x0000A73C File Offset: 0x0000893C
		private event EventHandler<EventSourceCreatedEventArgs> _EventSourceCreated;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000156 RID: 342 RVA: 0x0000A771 File Offset: 0x00008971
		// (remove) Token: 0x06000157 RID: 343 RVA: 0x0000A792 File Offset: 0x00008992
		public event EventHandler<EventSourceCreatedEventArgs> EventSourceCreated
		{
			add
			{
				this.CallBackForExistingEventSources(false, value);
				this._EventSourceCreated = (EventHandler<EventSourceCreatedEventArgs>)Delegate.Combine(this._EventSourceCreated, value);
			}
			remove
			{
				this._EventSourceCreated = (EventHandler<EventSourceCreatedEventArgs>)Delegate.Remove(this._EventSourceCreated, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000158 RID: 344 RVA: 0x0000A7AC File Offset: 0x000089AC
		// (remove) Token: 0x06000159 RID: 345 RVA: 0x0000A7E4 File Offset: 0x000089E4
		public event EventHandler<EventWrittenEventArgs> EventWritten;

		// Token: 0x0600015A RID: 346 RVA: 0x0000A828 File Offset: 0x00008A28
		protected EventListener()
		{
			this.CallBackForExistingEventSources(true, delegate(object obj, EventSourceCreatedEventArgs args)
			{
				args.EventSource.AddListener(this);
			});
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000A858 File Offset: 0x00008A58
		public virtual void Dispose()
		{
			lock (EventListener.EventListenersLock)
			{
				if (EventListener.s_Listeners != null)
				{
					if (this == EventListener.s_Listeners)
					{
						EventListener listenerToRemove = EventListener.s_Listeners;
						EventListener.s_Listeners = this.m_Next;
						EventListener.RemoveReferencesToListenerInEventSources(listenerToRemove);
					}
					else
					{
						EventListener eventListener = EventListener.s_Listeners;
						EventListener next;
						for (;;)
						{
							next = eventListener.m_Next;
							if (next == null)
							{
								goto IL_6D;
							}
							if (next == this)
							{
								break;
							}
							eventListener = next;
						}
						eventListener.m_Next = next.m_Next;
						EventListener.RemoveReferencesToListenerInEventSources(next);
					}
				}
				IL_6D:;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000A8F0 File Offset: 0x00008AF0
		public void EnableEvents(EventSource eventSource, EventLevel level)
		{
			this.EnableEvents(eventSource, level, EventKeywords.None);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000A8FC File Offset: 0x00008AFC
		public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword)
		{
			this.EnableEvents(eventSource, level, matchAnyKeyword, null);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A908 File Offset: 0x00008B08
		public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string> arguments)
		{
			if (eventSource == null)
			{
				throw new ArgumentNullException("eventSource");
			}
			eventSource.SendCommand(this, 0, 0, EventCommand.Update, true, level, matchAnyKeyword, arguments);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A934 File Offset: 0x00008B34
		public void DisableEvents(EventSource eventSource)
		{
			if (eventSource == null)
			{
				throw new ArgumentNullException("eventSource");
			}
			eventSource.SendCommand(this, 0, 0, EventCommand.Update, false, EventLevel.LogAlways, EventKeywords.None, null);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A95E File Offset: 0x00008B5E
		public static int EventSourceIndex(EventSource eventSource)
		{
			return eventSource.m_id;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A968 File Offset: 0x00008B68
		protected internal virtual void OnEventSourceCreated(EventSource eventSource)
		{
			EventHandler<EventSourceCreatedEventArgs> eventSourceCreated = this._EventSourceCreated;
			if (eventSourceCreated != null)
			{
				eventSourceCreated(this, new EventSourceCreatedEventArgs
				{
					EventSource = eventSource
				});
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A994 File Offset: 0x00008B94
		protected internal virtual void OnEventWritten(EventWrittenEventArgs eventData)
		{
			EventHandler<EventWrittenEventArgs> eventWritten = this.EventWritten;
			if (eventWritten != null)
			{
				eventWritten(this, eventData);
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		internal static void AddEventSource(EventSource newEventSource)
		{
			checked
			{
				lock (EventListener.EventListenersLock)
				{
					if (EventListener.s_EventSources == null)
					{
						EventListener.s_EventSources = new List<WeakReference>(2);
					}
					if (!EventListener.s_EventSourceShutdownRegistered)
					{
						EventListener.s_EventSourceShutdownRegistered = true;
						AppDomain.CurrentDomain.ProcessExit += EventListener.DisposeOnShutdown;
						AppDomain.CurrentDomain.DomainUnload += EventListener.DisposeOnShutdown;
					}
					int num = -1;
					if (EventListener.s_EventSources.Count % 64 == 63)
					{
						int num2 = EventListener.s_EventSources.Count;
						while (0 < num2)
						{
							num2--;
							WeakReference weakReference = EventListener.s_EventSources[num2];
							if (!weakReference.IsAlive)
							{
								num = num2;
								weakReference.Target = newEventSource;
								break;
							}
						}
					}
					if (num < 0)
					{
						num = EventListener.s_EventSources.Count;
						EventListener.s_EventSources.Add(new WeakReference(newEventSource));
					}
					newEventSource.m_id = num;
					for (EventListener next = EventListener.s_Listeners; next != null; next = next.m_Next)
					{
						newEventSource.AddListener(next);
					}
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		private static void DisposeOnShutdown(object sender, EventArgs e)
		{
			lock (EventListener.EventListenersLock)
			{
				foreach (WeakReference weakReference in EventListener.s_EventSources)
				{
					EventSource eventSource = weakReference.Target as EventSource;
					if (eventSource != null)
					{
						eventSource.Dispose();
					}
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000AB50 File Offset: 0x00008D50
		private static void RemoveReferencesToListenerInEventSources(EventListener listenerToRemove)
		{
			using (List<WeakReference>.Enumerator enumerator = EventListener.s_EventSources.GetEnumerator())
			{
				IL_7A:
				while (enumerator.MoveNext())
				{
					WeakReference weakReference = enumerator.Current;
					EventSource eventSource = weakReference.Target as EventSource;
					if (eventSource != null)
					{
						if (eventSource.m_Dispatchers.m_Listener == listenerToRemove)
						{
							eventSource.m_Dispatchers = eventSource.m_Dispatchers.m_Next;
						}
						else
						{
							EventDispatcher eventDispatcher = eventSource.m_Dispatchers;
							EventDispatcher next;
							for (;;)
							{
								next = eventDispatcher.m_Next;
								if (next == null)
								{
									goto IL_7A;
								}
								if (next.m_Listener == listenerToRemove)
								{
									break;
								}
								eventDispatcher = next;
							}
							eventDispatcher.m_Next = next.m_Next;
						}
					}
				}
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000AC00 File Offset: 0x00008E00
		[Conditional("DEBUG")]
		internal static void Validate()
		{
			checked
			{
				lock (EventListener.EventListenersLock)
				{
					Dictionary<EventListener, bool> dictionary = new Dictionary<EventListener, bool>();
					for (EventListener next = EventListener.s_Listeners; next != null; next = next.m_Next)
					{
						dictionary.Add(next, true);
					}
					int num = -1;
					foreach (WeakReference weakReference in EventListener.s_EventSources)
					{
						num++;
						EventSource eventSource = weakReference.Target as EventSource;
						if (eventSource != null)
						{
							for (EventDispatcher eventDispatcher = eventSource.m_Dispatchers; eventDispatcher != null; eventDispatcher = eventDispatcher.m_Next)
							{
							}
							foreach (EventListener eventListener in dictionary.Keys)
							{
								EventDispatcher eventDispatcher = eventSource.m_Dispatchers;
								while (eventDispatcher.m_Listener != eventListener)
								{
									eventDispatcher = eventDispatcher.m_Next;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000AD30 File Offset: 0x00008F30
		internal static object EventListenersLock
		{
			get
			{
				if (EventListener.s_EventSources == null)
				{
					Interlocked.CompareExchange<List<WeakReference>>(ref EventListener.s_EventSources, new List<WeakReference>(2), null);
				}
				return EventListener.s_EventSources;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000AD50 File Offset: 0x00008F50
		private void CallBackForExistingEventSources(bool addToListenersList, EventHandler<EventSourceCreatedEventArgs> callback)
		{
			lock (EventListener.EventListenersLock)
			{
				if (EventListener.s_CreatingListener)
				{
					throw new InvalidOperationException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ListenerCreatedInsideCallback", new object[0]));
				}
				try
				{
					EventListener.s_CreatingListener = true;
					if (addToListenersList)
					{
						this.m_Next = EventListener.s_Listeners;
						EventListener.s_Listeners = this;
					}
					foreach (WeakReference weakReference in EventListener.s_EventSources.ToArray())
					{
						EventSource eventSource = weakReference.Target as EventSource;
						if (eventSource != null)
						{
							callback(this, new EventSourceCreatedEventArgs
							{
								EventSource = eventSource
							});
						}
					}
				}
				finally
				{
					EventListener.s_CreatingListener = false;
				}
			}
		}

		// Token: 0x040000BB RID: 187
		internal volatile EventListener m_Next;

		// Token: 0x040000BC RID: 188
		internal static EventListener s_Listeners;

		// Token: 0x040000BD RID: 189
		internal static List<WeakReference> s_EventSources;

		// Token: 0x040000BE RID: 190
		private static bool s_CreatingListener = false;

		// Token: 0x040000BF RID: 191
		private static bool s_EventSourceShutdownRegistered = false;
	}
}
