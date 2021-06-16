using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003E9 RID: 1001
	internal sealed class WSManPluginEntryDelegates : IDisposable
	{
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002D44 RID: 11588 RVA: 0x000FB0BD File Offset: 0x000F92BD
		internal WSManPluginEntryDelegates.WSManPluginEntryDelegatesInternal UnmanagedStruct
		{
			get
			{
				return this.unmanagedStruct;
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000FB0C5 File Offset: 0x000F92C5
		internal WSManPluginEntryDelegates()
		{
			this.populateDelegates();
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000FB0DE File Offset: 0x000F92DE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000FB0ED File Offset: 0x000F92ED
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.CleanUpDelegates();
			this.disposed = true;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000FB108 File Offset: 0x000F9308
		~WSManPluginEntryDelegates()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000FB138 File Offset: 0x000F9338
		private void populateDelegates()
		{
			WSMPluginShellDelegate wsmpluginShellDelegate = new WSMPluginShellDelegate(WSManPluginManagedEntryWrapper.WSManPluginShell);
			this.pluginShellGCHandle = GCHandle.Alloc(wsmpluginShellDelegate);
			this.unmanagedStruct.wsManPluginShellCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginShellDelegate);
			WSMPluginReleaseShellContextDelegate wsmpluginReleaseShellContextDelegate = new WSMPluginReleaseShellContextDelegate(WSManPluginManagedEntryWrapper.WSManPluginReleaseShellContext);
			this.pluginReleaseShellContextGCHandle = GCHandle.Alloc(wsmpluginReleaseShellContextDelegate);
			this.unmanagedStruct.wsManPluginReleaseShellContextCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginReleaseShellContextDelegate);
			WSMPluginCommandDelegate wsmpluginCommandDelegate = new WSMPluginCommandDelegate(WSManPluginManagedEntryWrapper.WSManPluginCommand);
			this.pluginCommandGCHandle = GCHandle.Alloc(wsmpluginCommandDelegate);
			this.unmanagedStruct.wsManPluginCommandCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginCommandDelegate);
			WSMPluginReleaseCommandContextDelegate wsmpluginReleaseCommandContextDelegate = new WSMPluginReleaseCommandContextDelegate(WSManPluginManagedEntryWrapper.WSManPluginReleaseCommandContext);
			this.pluginReleaseCommandContextGCHandle = GCHandle.Alloc(wsmpluginReleaseCommandContextDelegate);
			this.unmanagedStruct.wsManPluginReleaseCommandContextCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginReleaseCommandContextDelegate);
			WSMPluginSendDelegate wsmpluginSendDelegate = new WSMPluginSendDelegate(WSManPluginManagedEntryWrapper.WSManPluginSend);
			this.pluginSendGCHandle = GCHandle.Alloc(wsmpluginSendDelegate);
			this.unmanagedStruct.wsManPluginSendCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginSendDelegate);
			WSMPluginReceiveDelegate wsmpluginReceiveDelegate = new WSMPluginReceiveDelegate(WSManPluginManagedEntryWrapper.WSManPluginReceive);
			this.pluginReceiveGCHandle = GCHandle.Alloc(wsmpluginReceiveDelegate);
			this.unmanagedStruct.wsManPluginReceiveCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginReceiveDelegate);
			WSMPluginSignalDelegate wsmpluginSignalDelegate = new WSMPluginSignalDelegate(WSManPluginManagedEntryWrapper.WSManPluginSignal);
			this.pluginSignalGCHandle = GCHandle.Alloc(wsmpluginSignalDelegate);
			this.unmanagedStruct.wsManPluginSignalCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginSignalDelegate);
			WSMPluginConnectDelegate wsmpluginConnectDelegate = new WSMPluginConnectDelegate(WSManPluginManagedEntryWrapper.WSManPluginConnect);
			this.pluginConnectGCHandle = GCHandle.Alloc(wsmpluginConnectDelegate);
			this.unmanagedStruct.wsManPluginConnectCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmpluginConnectDelegate);
			WSMShutdownPluginDelegate wsmshutdownPluginDelegate = new WSMShutdownPluginDelegate(WSManPluginManagedEntryWrapper.ShutdownPlugin);
			this.shutdownPluginGCHandle = GCHandle.Alloc(wsmshutdownPluginDelegate);
			this.unmanagedStruct.wsManPluginShutdownPluginCallbackNative = Marshal.GetFunctionPointerForDelegate(wsmshutdownPluginDelegate);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000FB2D0 File Offset: 0x000F94D0
		private void CleanUpDelegates()
		{
			this.pluginShellGCHandle.Free();
			this.pluginReleaseShellContextGCHandle.Free();
			this.pluginCommandGCHandle.Free();
			this.pluginReleaseCommandContextGCHandle.Free();
			this.pluginSendGCHandle.Free();
			this.pluginReceiveGCHandle.Free();
			this.pluginSignalGCHandle.Free();
			this.pluginConnectGCHandle.Free();
			this.shutdownPluginGCHandle.Free();
		}

		// Token: 0x040017BC RID: 6076
		private WSManPluginEntryDelegates.WSManPluginEntryDelegatesInternal unmanagedStruct = new WSManPluginEntryDelegates.WSManPluginEntryDelegatesInternal();

		// Token: 0x040017BD RID: 6077
		private bool disposed;

		// Token: 0x040017BE RID: 6078
		private GCHandle pluginShellGCHandle;

		// Token: 0x040017BF RID: 6079
		private GCHandle pluginReleaseShellContextGCHandle;

		// Token: 0x040017C0 RID: 6080
		private GCHandle pluginCommandGCHandle;

		// Token: 0x040017C1 RID: 6081
		private GCHandle pluginReleaseCommandContextGCHandle;

		// Token: 0x040017C2 RID: 6082
		private GCHandle pluginSendGCHandle;

		// Token: 0x040017C3 RID: 6083
		private GCHandle pluginReceiveGCHandle;

		// Token: 0x040017C4 RID: 6084
		private GCHandle pluginSignalGCHandle;

		// Token: 0x040017C5 RID: 6085
		private GCHandle pluginConnectGCHandle;

		// Token: 0x040017C6 RID: 6086
		private GCHandle shutdownPluginGCHandle;

		// Token: 0x020003EA RID: 1002
		[StructLayout(LayoutKind.Sequential)]
		internal class WSManPluginEntryDelegatesInternal
		{
			// Token: 0x040017C7 RID: 6087
			internal IntPtr wsManPluginShutdownPluginCallbackNative;

			// Token: 0x040017C8 RID: 6088
			internal IntPtr wsManPluginShellCallbackNative;

			// Token: 0x040017C9 RID: 6089
			internal IntPtr wsManPluginReleaseShellContextCallbackNative;

			// Token: 0x040017CA RID: 6090
			internal IntPtr wsManPluginCommandCallbackNative;

			// Token: 0x040017CB RID: 6091
			internal IntPtr wsManPluginReleaseCommandContextCallbackNative;

			// Token: 0x040017CC RID: 6092
			internal IntPtr wsManPluginSendCallbackNative;

			// Token: 0x040017CD RID: 6093
			internal IntPtr wsManPluginReceiveCallbackNative;

			// Token: 0x040017CE RID: 6094
			internal IntPtr wsManPluginSignalCallbackNative;

			// Token: 0x040017CF RID: 6095
			internal IntPtr wsManPluginConnectCallbackNative;
		}
	}
}
