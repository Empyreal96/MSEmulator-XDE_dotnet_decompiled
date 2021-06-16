using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x020000A2 RID: 162
	internal sealed class IMsTscAxEvents_EventProvider : IMsTscAxEvents_Event, IDisposable
	{
		// Token: 0x060024DF RID: 9439 RVA: 0x00002734 File Offset: 0x00000934
		private void Init()
		{
			IConnectionPoint connectionPoint = null;
			Guid guid = new Guid(new byte[]
			{
				98,
				85,
				109,
				51,
				168,
				239,
				46,
				72,
				140,
				179,
				197,
				192,
				252,
				122,
				125,
				182
			});
			((IConnectionPointContainer)this.m_wkConnectionPointContainer.Target).FindConnectionPoint(ref guid, out connectionPoint);
			this.m_ConnectionPoint = (IConnectionPoint)connectionPoint;
			this.m_aEventSinkHelpers = new ArrayList();
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x00002850 File Offset: 0x00000A50
		public void add_OnConnecting(IMsTscAxEvents_OnConnectingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnConnectingDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000028E8 File Offset: 0x00000AE8
		public void remove_OnConnecting(IMsTscAxEvents_OnConnectingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnConnectingDelegate != null && ((msTscAxEvents_SinkHelper.m_OnConnectingDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000029FC File Offset: 0x00000BFC
		public void add_OnConnected(IMsTscAxEvents_OnConnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnConnectedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x00002A94 File Offset: 0x00000C94
		public void remove_OnConnected(IMsTscAxEvents_OnConnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnConnectedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnConnectedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public void add_OnLoginComplete(IMsTscAxEvents_OnLoginCompleteEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnLoginCompleteDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x00002C40 File Offset: 0x00000E40
		public void remove_OnLoginComplete(IMsTscAxEvents_OnLoginCompleteEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnLoginCompleteDelegate != null && ((msTscAxEvents_SinkHelper.m_OnLoginCompleteDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x00002D54 File Offset: 0x00000F54
		public void add_OnDisconnected(IMsTscAxEvents_OnDisconnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnDisconnectedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x00002DEC File Offset: 0x00000FEC
		public void remove_OnDisconnected(IMsTscAxEvents_OnDisconnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnDisconnectedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnDisconnectedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x00002F00 File Offset: 0x00001100
		public void add_OnEnterFullScreenMode(IMsTscAxEvents_OnEnterFullScreenModeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnEnterFullScreenModeDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x00002F98 File Offset: 0x00001198
		public void remove_OnEnterFullScreenMode(IMsTscAxEvents_OnEnterFullScreenModeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnEnterFullScreenModeDelegate != null && ((msTscAxEvents_SinkHelper.m_OnEnterFullScreenModeDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000030AC File Offset: 0x000012AC
		public void add_OnLeaveFullScreenMode(IMsTscAxEvents_OnLeaveFullScreenModeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnLeaveFullScreenModeDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x00003144 File Offset: 0x00001344
		public void remove_OnLeaveFullScreenMode(IMsTscAxEvents_OnLeaveFullScreenModeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnLeaveFullScreenModeDelegate != null && ((msTscAxEvents_SinkHelper.m_OnLeaveFullScreenModeDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00003258 File Offset: 0x00001458
		public void add_OnChannelReceivedData(IMsTscAxEvents_OnChannelReceivedDataEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnChannelReceivedDataDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000032F0 File Offset: 0x000014F0
		public void remove_OnChannelReceivedData(IMsTscAxEvents_OnChannelReceivedDataEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnChannelReceivedDataDelegate != null && ((msTscAxEvents_SinkHelper.m_OnChannelReceivedDataDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00003404 File Offset: 0x00001604
		public void add_OnRequestGoFullScreen(IMsTscAxEvents_OnRequestGoFullScreenEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRequestGoFullScreenDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x0000349C File Offset: 0x0000169C
		public void remove_OnRequestGoFullScreen(IMsTscAxEvents_OnRequestGoFullScreenEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRequestGoFullScreenDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRequestGoFullScreenDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000035B0 File Offset: 0x000017B0
		public void add_OnRequestLeaveFullScreen(IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRequestLeaveFullScreenDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00003648 File Offset: 0x00001848
		public void remove_OnRequestLeaveFullScreen(IMsTscAxEvents_OnRequestLeaveFullScreenEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRequestLeaveFullScreenDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRequestLeaveFullScreenDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x0000375C File Offset: 0x0000195C
		public void add_OnFatalError(IMsTscAxEvents_OnFatalErrorEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnFatalErrorDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000037F4 File Offset: 0x000019F4
		public void remove_OnFatalError(IMsTscAxEvents_OnFatalErrorEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnFatalErrorDelegate != null && ((msTscAxEvents_SinkHelper.m_OnFatalErrorDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00003908 File Offset: 0x00001B08
		public void add_OnWarning(IMsTscAxEvents_OnWarningEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnWarningDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000039A0 File Offset: 0x00001BA0
		public void remove_OnWarning(IMsTscAxEvents_OnWarningEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnWarningDelegate != null && ((msTscAxEvents_SinkHelper.m_OnWarningDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00003AB4 File Offset: 0x00001CB4
		public void add_OnRemoteDesktopSizeChange(IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRemoteDesktopSizeChangeDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00003B4C File Offset: 0x00001D4C
		public void remove_OnRemoteDesktopSizeChange(IMsTscAxEvents_OnRemoteDesktopSizeChangeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRemoteDesktopSizeChangeDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRemoteDesktopSizeChangeDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00003C60 File Offset: 0x00001E60
		public void add_OnIdleTimeoutNotification(IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnIdleTimeoutNotificationDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00003CF8 File Offset: 0x00001EF8
		public void remove_OnIdleTimeoutNotification(IMsTscAxEvents_OnIdleTimeoutNotificationEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnIdleTimeoutNotificationDelegate != null && ((msTscAxEvents_SinkHelper.m_OnIdleTimeoutNotificationDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x00003E0C File Offset: 0x0000200C
		public void add_OnRequestContainerMinimize(IMsTscAxEvents_OnRequestContainerMinimizeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRequestContainerMinimizeDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x00003EA4 File Offset: 0x000020A4
		public void remove_OnRequestContainerMinimize(IMsTscAxEvents_OnRequestContainerMinimizeEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRequestContainerMinimizeDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRequestContainerMinimizeDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x00003FB8 File Offset: 0x000021B8
		public void add_OnConfirmClose(IMsTscAxEvents_OnConfirmCloseEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnConfirmCloseDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x00004050 File Offset: 0x00002250
		public void remove_OnConfirmClose(IMsTscAxEvents_OnConfirmCloseEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnConfirmCloseDelegate != null && ((msTscAxEvents_SinkHelper.m_OnConfirmCloseDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00004164 File Offset: 0x00002364
		public void add_OnReceivedTSPublicKey(IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnReceivedTSPublicKeyDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000041FC File Offset: 0x000023FC
		public void remove_OnReceivedTSPublicKey(IMsTscAxEvents_OnReceivedTSPublicKeyEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnReceivedTSPublicKeyDelegate != null && ((msTscAxEvents_SinkHelper.m_OnReceivedTSPublicKeyDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x00004310 File Offset: 0x00002510
		public void add_OnAutoReconnecting(IMsTscAxEvents_OnAutoReconnectingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnAutoReconnectingDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000043A8 File Offset: 0x000025A8
		public void remove_OnAutoReconnecting(IMsTscAxEvents_OnAutoReconnectingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnAutoReconnectingDelegate != null && ((msTscAxEvents_SinkHelper.m_OnAutoReconnectingDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000044BC File Offset: 0x000026BC
		public void add_OnAuthenticationWarningDisplayed(IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnAuthenticationWarningDisplayedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00004554 File Offset: 0x00002754
		public void remove_OnAuthenticationWarningDisplayed(IMsTscAxEvents_OnAuthenticationWarningDisplayedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnAuthenticationWarningDisplayedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnAuthenticationWarningDisplayedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x00004668 File Offset: 0x00002868
		public void add_OnAuthenticationWarningDismissed(IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnAuthenticationWarningDismissedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x00004700 File Offset: 0x00002900
		public void remove_OnAuthenticationWarningDismissed(IMsTscAxEvents_OnAuthenticationWarningDismissedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnAuthenticationWarningDismissedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnAuthenticationWarningDismissedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00004814 File Offset: 0x00002A14
		public void add_OnRemoteProgramResult(IMsTscAxEvents_OnRemoteProgramResultEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRemoteProgramResultDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000048AC File Offset: 0x00002AAC
		public void remove_OnRemoteProgramResult(IMsTscAxEvents_OnRemoteProgramResultEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRemoteProgramResultDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRemoteProgramResultDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000049C0 File Offset: 0x00002BC0
		public void add_OnRemoteProgramDisplayed(IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRemoteProgramDisplayedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x00004A58 File Offset: 0x00002C58
		public void remove_OnRemoteProgramDisplayed(IMsTscAxEvents_OnRemoteProgramDisplayedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRemoteProgramDisplayedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRemoteProgramDisplayedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x00004B6C File Offset: 0x00002D6C
		public void add_OnRemoteWindowDisplayed(IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnRemoteWindowDisplayedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x00004C04 File Offset: 0x00002E04
		public void remove_OnRemoteWindowDisplayed(IMsTscAxEvents_OnRemoteWindowDisplayedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnRemoteWindowDisplayedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnRemoteWindowDisplayedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x00004D18 File Offset: 0x00002F18
		public void add_OnLogonError(IMsTscAxEvents_OnLogonErrorEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnLogonErrorDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public void remove_OnLogonError(IMsTscAxEvents_OnLogonErrorEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnLogonErrorDelegate != null && ((msTscAxEvents_SinkHelper.m_OnLogonErrorDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x00004EC4 File Offset: 0x000030C4
		public void add_OnFocusReleased(IMsTscAxEvents_OnFocusReleasedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnFocusReleasedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x00004F5C File Offset: 0x0000315C
		public void remove_OnFocusReleased(IMsTscAxEvents_OnFocusReleasedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnFocusReleasedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnFocusReleasedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x00005070 File Offset: 0x00003270
		public void add_OnUserNameAcquired(IMsTscAxEvents_OnUserNameAcquiredEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnUserNameAcquiredDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00005108 File Offset: 0x00003308
		public void remove_OnUserNameAcquired(IMsTscAxEvents_OnUserNameAcquiredEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnUserNameAcquiredDelegate != null && ((msTscAxEvents_SinkHelper.m_OnUserNameAcquiredDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0000521C File Offset: 0x0000341C
		public void add_OnMouseInputModeChanged(IMsTscAxEvents_OnMouseInputModeChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnMouseInputModeChangedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000052B4 File Offset: 0x000034B4
		public void remove_OnMouseInputModeChanged(IMsTscAxEvents_OnMouseInputModeChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnMouseInputModeChangedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnMouseInputModeChangedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000053C8 File Offset: 0x000035C8
		public void add_OnServiceMessageReceived(IMsTscAxEvents_OnServiceMessageReceivedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnServiceMessageReceivedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x00005460 File Offset: 0x00003660
		public void remove_OnServiceMessageReceived(IMsTscAxEvents_OnServiceMessageReceivedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnServiceMessageReceivedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnServiceMessageReceivedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00005574 File Offset: 0x00003774
		public void add_OnConnectionBarPullDown(IMsTscAxEvents_OnConnectionBarPullDownEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnConnectionBarPullDownDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x0000560C File Offset: 0x0000380C
		public void remove_OnConnectionBarPullDown(IMsTscAxEvents_OnConnectionBarPullDownEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnConnectionBarPullDownDelegate != null && ((msTscAxEvents_SinkHelper.m_OnConnectionBarPullDownDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x00005720 File Offset: 0x00003920
		public void add_OnNetworkBandwidthChanged(IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnNetworkBandwidthChangedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000057B8 File Offset: 0x000039B8
		public void remove_OnNetworkBandwidthChanged(IMsTscAxEvents_OnNetworkBandwidthChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnNetworkBandwidthChangedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnNetworkBandwidthChangedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000058CC File Offset: 0x00003ACC
		public void add_OnAutoReconnected(IMsTscAxEvents_OnAutoReconnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = new IMsTscAxEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)msTscAxEvents_SinkHelper, out dwCookie);
				msTscAxEvents_SinkHelper.m_dwCookie = dwCookie;
				msTscAxEvents_SinkHelper.m_OnAutoReconnectedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)msTscAxEvents_SinkHelper);
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x00005964 File Offset: 0x00003B64
		public void remove_OnAutoReconnected(IMsTscAxEvents_OnAutoReconnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_aEventSinkHelpers != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper;
						for (;;)
						{
							msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (msTscAxEvents_SinkHelper.m_OnAutoReconnectedDelegate != null && ((msTscAxEvents_SinkHelper.m_OnAutoReconnectedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
							{
								break;
							}
							num++;
							if (num >= count)
							{
								goto IL_E3;
							}
						}
						this.m_aEventSinkHelpers.RemoveAt(num);
						this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
						if (count <= 1)
						{
							Marshal.ReleaseComObject(this.m_ConnectionPoint);
							this.m_ConnectionPoint = null;
							this.m_aEventSinkHelpers = null;
						}
					}
				}
				IL_E3:;
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x00005A78 File Offset: 0x00003C78
		public IMsTscAxEvents_EventProvider(object A_1)
		{
			this.m_wkConnectionPointContainer = new WeakReference((IConnectionPointContainer)A_1, false);
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x00005AA8 File Offset: 0x00003CA8
		public override void Finalize()
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint != null)
				{
					int count = this.m_aEventSinkHelpers.Count;
					int num = 0;
					if (0 < count)
					{
						do
						{
							IMsTscAxEvents_SinkHelper msTscAxEvents_SinkHelper = (IMsTscAxEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							this.m_ConnectionPoint.Unadvise(msTscAxEvents_SinkHelper.m_dwCookie);
							num++;
						}
						while (num < count);
					}
					Marshal.ReleaseComObject(this.m_ConnectionPoint);
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				bool flag;
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x00005B70 File Offset: 0x00003D70
		public void Dispose()
		{
			this.Finalize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000BC RID: 188
		private WeakReference m_wkConnectionPointContainer;

		// Token: 0x040000BD RID: 189
		private ArrayList m_aEventSinkHelpers;

		// Token: 0x040000BE RID: 190
		private IConnectionPoint m_ConnectionPoint;
	}
}
