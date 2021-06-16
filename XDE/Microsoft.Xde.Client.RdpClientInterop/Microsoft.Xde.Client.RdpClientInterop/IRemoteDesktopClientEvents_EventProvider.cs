using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Microsoft.Xde.Client.RdpClientInterop
{
	// Token: 0x020000A4 RID: 164
	internal sealed class IRemoteDesktopClientEvents_EventProvider : IRemoteDesktopClientEvents_Event, IDisposable
	{
		// Token: 0x0600252D RID: 9517 RVA: 0x00005EBC File Offset: 0x000040BC
		private void Init()
		{
			IConnectionPoint connectionPoint = null;
			Guid guid = new Guid(new byte[]
			{
				183,
				99,
				152,
				7,
				71,
				109,
				5,
				65,
				139,
				254,
				12,
				220,
				179,
				96,
				230,
				125
			});
			((IConnectionPointContainer)this.m_wkConnectionPointContainer.Target).FindConnectionPoint(ref guid, out connectionPoint);
			this.m_ConnectionPoint = (IConnectionPoint)connectionPoint;
			this.m_aEventSinkHelpers = new ArrayList();
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x00005FD8 File Offset: 0x000041D8
		public void add_OnConnecting(IRemoteDesktopClientEvents_OnConnectingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnConnectingDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x0600252F RID: 9519 RVA: 0x00006070 File Offset: 0x00004270
		public void remove_OnConnecting(IRemoteDesktopClientEvents_OnConnectingEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnConnectingDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnConnectingDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002530 RID: 9520 RVA: 0x00006184 File Offset: 0x00004384
		public void add_OnConnected(IRemoteDesktopClientEvents_OnConnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnConnectedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002531 RID: 9521 RVA: 0x0000621C File Offset: 0x0000441C
		public void remove_OnConnected(IRemoteDesktopClientEvents_OnConnectedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnConnectedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnConnectedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002532 RID: 9522 RVA: 0x00006330 File Offset: 0x00004530
		public void add_OnLoginCompleted(IRemoteDesktopClientEvents_OnLoginCompletedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnLoginCompletedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002533 RID: 9523 RVA: 0x000063C8 File Offset: 0x000045C8
		public void remove_OnLoginCompleted(IRemoteDesktopClientEvents_OnLoginCompletedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnLoginCompletedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnLoginCompletedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002534 RID: 9524 RVA: 0x000064DC File Offset: 0x000046DC
		public void add_OnDisconnected(IRemoteDesktopClientEvents_OnDisconnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnDisconnectedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002535 RID: 9525 RVA: 0x00006574 File Offset: 0x00004774
		public void remove_OnDisconnected(IRemoteDesktopClientEvents_OnDisconnectedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnDisconnectedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnDisconnectedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002536 RID: 9526 RVA: 0x00006688 File Offset: 0x00004888
		public void add_OnStatusChanged(IRemoteDesktopClientEvents_OnStatusChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnStatusChangedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002537 RID: 9527 RVA: 0x00006720 File Offset: 0x00004920
		public void remove_OnStatusChanged(IRemoteDesktopClientEvents_OnStatusChangedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnStatusChangedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnStatusChangedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002538 RID: 9528 RVA: 0x00006834 File Offset: 0x00004A34
		public void add_OnAutoReconnecting(IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnAutoReconnectingDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002539 RID: 9529 RVA: 0x000068CC File Offset: 0x00004ACC
		public void remove_OnAutoReconnecting(IRemoteDesktopClientEvents_OnAutoReconnectingEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnAutoReconnectingDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnAutoReconnectingDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x0600253A RID: 9530 RVA: 0x000069E0 File Offset: 0x00004BE0
		public void add_OnAutoReconnected(IRemoteDesktopClientEvents_OnAutoReconnectedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnAutoReconnectedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x0600253B RID: 9531 RVA: 0x00006A78 File Offset: 0x00004C78
		public void remove_OnAutoReconnected(IRemoteDesktopClientEvents_OnAutoReconnectedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnAutoReconnectedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnAutoReconnectedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x0600253C RID: 9532 RVA: 0x00006B8C File Offset: 0x00004D8C
		public void add_OnDialogDisplaying(IRemoteDesktopClientEvents_OnDialogDisplayingEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnDialogDisplayingDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x0600253D RID: 9533 RVA: 0x00006C24 File Offset: 0x00004E24
		public void remove_OnDialogDisplaying(IRemoteDesktopClientEvents_OnDialogDisplayingEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnDialogDisplayingDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnDialogDisplayingDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x0600253E RID: 9534 RVA: 0x00006D38 File Offset: 0x00004F38
		public void add_OnDialogDismissed(IRemoteDesktopClientEvents_OnDialogDismissedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnDialogDismissedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x0600253F RID: 9535 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public void remove_OnDialogDismissed(IRemoteDesktopClientEvents_OnDialogDismissedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnDialogDismissedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnDialogDismissedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002540 RID: 9536 RVA: 0x00006EE4 File Offset: 0x000050E4
		public void add_OnNetworkBandwidthChanged(IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnNetworkBandwidthChangedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002541 RID: 9537 RVA: 0x00006F7C File Offset: 0x0000517C
		public void remove_OnNetworkBandwidthChanged(IRemoteDesktopClientEvents_OnNetworkBandwidthChangedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnNetworkBandwidthChangedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnNetworkBandwidthChangedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002542 RID: 9538 RVA: 0x00007090 File Offset: 0x00005290
		public void add_OnAdminMessageReceived(IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnAdminMessageReceivedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002543 RID: 9539 RVA: 0x00007128 File Offset: 0x00005328
		public void remove_OnAdminMessageReceived(IRemoteDesktopClientEvents_OnAdminMessageReceivedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnAdminMessageReceivedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnAdminMessageReceivedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002544 RID: 9540 RVA: 0x0000723C File Offset: 0x0000543C
		public void add_OnKeyCombinationPressed(IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnKeyCombinationPressedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002545 RID: 9541 RVA: 0x000072D4 File Offset: 0x000054D4
		public void remove_OnKeyCombinationPressed(IRemoteDesktopClientEvents_OnKeyCombinationPressedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnKeyCombinationPressedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnKeyCombinationPressedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002546 RID: 9542 RVA: 0x000073E8 File Offset: 0x000055E8
		public void add_OnRemoteDesktopSizeChanged(IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler A_1)
		{
			try
			{
				bool flag;
				Monitor.Enter(this, ref flag);
				if (this.m_ConnectionPoint == null)
				{
					this.Init();
				}
				IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = new IRemoteDesktopClientEvents_SinkHelper();
				int dwCookie = 0;
				this.m_ConnectionPoint.Advise((object)remoteDesktopClientEvents_SinkHelper, out dwCookie);
				remoteDesktopClientEvents_SinkHelper.m_dwCookie = dwCookie;
				remoteDesktopClientEvents_SinkHelper.m_OnRemoteDesktopSizeChangedDelegate = A_1;
				this.m_aEventSinkHelpers.Add((object)remoteDesktopClientEvents_SinkHelper);
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

		// Token: 0x06002547 RID: 9543 RVA: 0x00007480 File Offset: 0x00005680
		public void remove_OnRemoteDesktopSizeChanged(IRemoteDesktopClientEvents_OnRemoteDesktopSizeChangedEventHandler A_1)
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
						IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper;
						for (;;)
						{
							remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							if (remoteDesktopClientEvents_SinkHelper.m_OnRemoteDesktopSizeChangedDelegate != null && ((remoteDesktopClientEvents_SinkHelper.m_OnRemoteDesktopSizeChangedDelegate.Equals((object)A_1) ? 1 : 0) & 255) != 0)
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
						this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x06002548 RID: 9544 RVA: 0x00007594 File Offset: 0x00005794
		public IRemoteDesktopClientEvents_EventProvider(object A_1)
		{
			this.m_wkConnectionPointContainer = new WeakReference((IConnectionPointContainer)A_1, false);
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000075C4 File Offset: 0x000057C4
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
							IRemoteDesktopClientEvents_SinkHelper remoteDesktopClientEvents_SinkHelper = (IRemoteDesktopClientEvents_SinkHelper)this.m_aEventSinkHelpers[num];
							this.m_ConnectionPoint.Unadvise(remoteDesktopClientEvents_SinkHelper.m_dwCookie);
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

		// Token: 0x0600254A RID: 9546 RVA: 0x0000768C File Offset: 0x0000588C
		public void Dispose()
		{
			this.Finalize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000CD RID: 205
		private WeakReference m_wkConnectionPointContainer;

		// Token: 0x040000CE RID: 206
		private ArrayList m_aEventSinkHelpers;

		// Token: 0x040000CF RID: 207
		private IConnectionPoint m_ConnectionPoint;
	}
}
