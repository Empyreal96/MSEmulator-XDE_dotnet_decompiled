using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Runtime.InteropServices;
using System.Timers;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008F5 RID: 2293
	public abstract class EtwActivity
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060055F8 RID: 22008 RVA: 0x001C3614 File Offset: 0x001C1814
		// (remove) Token: 0x060055F9 RID: 22009 RVA: 0x001C3648 File Offset: 0x001C1848
		public static event EventHandler<EtwEventArgs> EventWritten;

		// Token: 0x060055FB RID: 22011 RVA: 0x001C36CB File Offset: 0x001C18CB
		public static bool SetActivityId(Guid activityId)
		{
			if (EtwActivity.GetActivityId() != activityId)
			{
				EventProvider.SetActivityId(ref activityId);
				return true;
			}
			return false;
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x001C36E4 File Offset: 0x001C18E4
		public static Guid CreateActivityId()
		{
			return EventProvider.CreateActivityId();
		}

		// Token: 0x060055FD RID: 22013 RVA: 0x001C36EC File Offset: 0x001C18EC
		public static Guid GetActivityId()
		{
			Guid empty = Guid.Empty;
			EtwActivity.UnsafeNativeMethods.EventActivityIdControl(EtwActivity.UnsafeNativeMethods.ActivityControlCode.Get, ref empty);
			return empty;
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x001C3714 File Offset: 0x001C1914
		public void CorrelateWithActivity(Guid parentActivityId)
		{
			EventProvider provider = this.GetProvider();
			if (!provider.IsEnabled())
			{
				return;
			}
			Guid guid = EtwActivity.CreateActivityId();
			EtwActivity.SetActivityId(guid);
			if (parentActivityId != Guid.Empty)
			{
				EventDescriptor transferEvent = this.TransferEvent;
				provider.WriteTransferEvent(ref transferEvent, parentActivityId, new object[]
				{
					guid,
					parentActivityId
				});
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06005600 RID: 22016 RVA: 0x001C3776 File Offset: 0x001C1976
		public bool IsEnabled
		{
			get
			{
				return this.GetProvider().IsEnabled();
			}
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x001C3783 File Offset: 0x001C1983
		public bool IsProviderEnabled(byte levels, long keywords)
		{
			return this.GetProvider().IsEnabled(levels, keywords);
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x001C3794 File Offset: 0x001C1994
		public void Correlate()
		{
			Guid activityId = Trace.CorrelationManager.ActivityId;
			this.CorrelateWithActivity(activityId);
		}

		// Token: 0x06005603 RID: 22019 RVA: 0x001C37B3 File Offset: 0x001C19B3
		public CallbackNoParameter Correlate(CallbackNoParameter callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return new CallbackNoParameter(new EtwActivity.CorrelatedCallback(this, callback).Callback);
		}

		// Token: 0x06005604 RID: 22020 RVA: 0x001C37D5 File Offset: 0x001C19D5
		public CallbackWithState Correlate(CallbackWithState callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return new CallbackWithState(new EtwActivity.CorrelatedCallback(this, callback).Callback);
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x001C37F7 File Offset: 0x001C19F7
		public AsyncCallback Correlate(AsyncCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return new AsyncCallback(new EtwActivity.CorrelatedCallback(this, callback).Callback);
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x001C3819 File Offset: 0x001C1A19
		public CallbackWithStateAndArgs Correlate(CallbackWithStateAndArgs callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return new CallbackWithStateAndArgs(new EtwActivity.CorrelatedCallback(this, callback).Callback);
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06005607 RID: 22023 RVA: 0x001C383B File Offset: 0x001C1A3B
		protected virtual Guid ProviderId
		{
			get
			{
				return EtwActivity.powerShellProviderId;
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06005608 RID: 22024 RVA: 0x001C3842 File Offset: 0x001C1A42
		protected virtual EventDescriptor TransferEvent
		{
			get
			{
				return EtwActivity._WriteTransferEvent;
			}
		}

		// Token: 0x06005609 RID: 22025 RVA: 0x001C384C File Offset: 0x001C1A4C
		protected void WriteEvent(EventDescriptor ed, params object[] payload)
		{
			EventProvider provider = this.GetProvider();
			if (!provider.IsEnabled())
			{
				return;
			}
			if (payload != null)
			{
				for (int i = 0; i < payload.Length; i++)
				{
					if (payload[i] == null)
					{
						payload[i] = string.Empty;
					}
				}
			}
			bool success = provider.WriteEvent(ref ed, payload);
			if (EtwActivity.EventWritten != null)
			{
				EtwActivity.EventWritten(this, new EtwEventArgs(ed, success, payload));
			}
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x001C38AC File Offset: 0x001C1AAC
		private EventProvider GetProvider()
		{
			if (this.currentProvider != null)
			{
				return this.currentProvider;
			}
			lock (EtwActivity.syncLock)
			{
				if (this.currentProvider != null)
				{
					return this.currentProvider;
				}
				if (EtwActivity.providers.ContainsKey(this.ProviderId))
				{
					this.currentProvider = EtwActivity.providers[this.ProviderId];
				}
				else
				{
					this.currentProvider = new EventProvider(this.ProviderId);
					EtwActivity.providers[this.ProviderId] = this.currentProvider;
				}
			}
			return this.currentProvider;
		}

		// Token: 0x04002DBC RID: 11708
		private static Guid powerShellProviderId = Guid.Parse("A0C1853B-5C40-4b15-8766-3CF1C58F985A");

		// Token: 0x04002DBD RID: 11709
		private static Dictionary<Guid, EventProvider> providers = new Dictionary<Guid, EventProvider>();

		// Token: 0x04002DBE RID: 11710
		private static object syncLock = new object();

		// Token: 0x04002DBF RID: 11711
		private static EventDescriptor _WriteTransferEvent = new EventDescriptor(7941, 1, 17, 5, 20, 0, 4611686018427387904L);

		// Token: 0x04002DC0 RID: 11712
		private EventProvider currentProvider;

		// Token: 0x020008F6 RID: 2294
		private class CorrelatedCallback
		{
			// Token: 0x0600560B RID: 22027 RVA: 0x001C3960 File Offset: 0x001C1B60
			public CorrelatedCallback(EtwActivity tracer, CallbackNoParameter callback)
			{
				if (callback == null)
				{
					throw new ArgumentNullException("callback");
				}
				if (tracer == null)
				{
					throw new ArgumentNullException("tracer");
				}
				this.tracer = tracer;
				this.parentActivityId = EtwActivity.GetActivityId();
				this.callbackNoParam = callback;
			}

			// Token: 0x0600560C RID: 22028 RVA: 0x001C399D File Offset: 0x001C1B9D
			public CorrelatedCallback(EtwActivity tracer, CallbackWithState callback)
			{
				if (callback == null)
				{
					throw new ArgumentNullException("callback");
				}
				if (tracer == null)
				{
					throw new ArgumentNullException("tracer");
				}
				this.tracer = tracer;
				this.parentActivityId = EtwActivity.GetActivityId();
				this.callbackWithState = callback;
			}

			// Token: 0x0600560D RID: 22029 RVA: 0x001C39DA File Offset: 0x001C1BDA
			public CorrelatedCallback(EtwActivity tracer, AsyncCallback callback)
			{
				if (callback == null)
				{
					throw new ArgumentNullException("callback");
				}
				if (tracer == null)
				{
					throw new ArgumentNullException("tracer");
				}
				this.tracer = tracer;
				this.parentActivityId = EtwActivity.GetActivityId();
				this.asyncCallback = callback;
			}

			// Token: 0x0600560E RID: 22030 RVA: 0x001C3A17 File Offset: 0x001C1C17
			public CorrelatedCallback(EtwActivity tracer, CallbackWithStateAndArgs callback)
			{
				if (callback == null)
				{
					throw new ArgumentNullException("callback");
				}
				if (tracer == null)
				{
					throw new ArgumentNullException("tracer");
				}
				this.tracer = tracer;
				this.parentActivityId = EtwActivity.GetActivityId();
				this.callbackWithStateAndArgs = callback;
			}

			// Token: 0x0600560F RID: 22031 RVA: 0x001C3A54 File Offset: 0x001C1C54
			private void Correlate()
			{
				this.tracer.CorrelateWithActivity(this.parentActivityId);
			}

			// Token: 0x06005610 RID: 22032 RVA: 0x001C3A67 File Offset: 0x001C1C67
			public void Callback()
			{
				this.Correlate();
				this.callbackNoParam();
			}

			// Token: 0x06005611 RID: 22033 RVA: 0x001C3A7A File Offset: 0x001C1C7A
			public void Callback(object state)
			{
				this.Correlate();
				this.callbackWithState(state);
			}

			// Token: 0x06005612 RID: 22034 RVA: 0x001C3A8E File Offset: 0x001C1C8E
			public void Callback(IAsyncResult asyncResult)
			{
				this.Correlate();
				this.asyncCallback(asyncResult);
			}

			// Token: 0x06005613 RID: 22035 RVA: 0x001C3AA2 File Offset: 0x001C1CA2
			public void Callback(object state, ElapsedEventArgs args)
			{
				this.Correlate();
				this.callbackWithStateAndArgs(state, args);
			}

			// Token: 0x04002DC2 RID: 11714
			private CallbackNoParameter callbackNoParam;

			// Token: 0x04002DC3 RID: 11715
			private CallbackWithState callbackWithState;

			// Token: 0x04002DC4 RID: 11716
			private CallbackWithStateAndArgs callbackWithStateAndArgs;

			// Token: 0x04002DC5 RID: 11717
			private AsyncCallback asyncCallback;

			// Token: 0x04002DC6 RID: 11718
			protected readonly Guid parentActivityId;

			// Token: 0x04002DC7 RID: 11719
			private readonly EtwActivity tracer;
		}

		// Token: 0x020008F7 RID: 2295
		private static class UnsafeNativeMethods
		{
			// Token: 0x06005614 RID: 22036
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			internal static extern uint EventActivityIdControl([In] EtwActivity.UnsafeNativeMethods.ActivityControlCode controlCode, [In] [Out] ref Guid activityId);

			// Token: 0x04002DC8 RID: 11720
			private const string ADVAPI32 = "advapi32.dll";

			// Token: 0x020008F8 RID: 2296
			internal enum ActivityControlCode : uint
			{
				// Token: 0x04002DCA RID: 11722
				Get = 1U,
				// Token: 0x04002DCB RID: 11723
				Set,
				// Token: 0x04002DCC RID: 11724
				Create,
				// Token: 0x04002DCD RID: 11725
				GetSet,
				// Token: 0x04002DCE RID: 11726
				CreateSet
			}
		}
	}
}
