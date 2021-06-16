using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002EF RID: 751
	internal sealed class RemoteSessionNamedPipeClient : NamedPipeClientBase
	{
		// Token: 0x060023B8 RID: 9144 RVA: 0x000C8592 File Offset: 0x000C6792
		private RemoteSessionNamedPipeClient()
		{
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000C859A File Offset: 0x000C679A
		public RemoteSessionNamedPipeClient(Process process, string appDomainName) : this(NamedPipeUtils.CreateProcessPipeName(process, appDomainName))
		{
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000C85A9 File Offset: 0x000C67A9
		public RemoteSessionNamedPipeClient(int procId, string appDomainName) : this(NamedPipeUtils.CreateProcessPipeName(procId, appDomainName))
		{
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000C85B8 File Offset: 0x000C67B8
		internal RemoteSessionNamedPipeClient(string pipeName)
		{
			if (pipeName == null)
			{
				throw new PSArgumentNullException("pipeName");
			}
			this._pipeName = "\\\\.\\pipe\\" + pipeName;
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000C85E0 File Offset: 0x000C67E0
		internal RemoteSessionNamedPipeClient(string serverName, string namespaceName, string coreName)
		{
			if (serverName == null)
			{
				throw new PSArgumentNullException("serverName");
			}
			if (namespaceName == null)
			{
				throw new PSArgumentNullException("namespaceName");
			}
			if (coreName == null)
			{
				throw new PSArgumentNullException("coreName");
			}
			this._pipeName = string.Concat(new string[]
			{
				"\\\\",
				serverName,
				"\\",
				namespaceName,
				"\\",
				coreName
			});
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000C8654 File Offset: 0x000C6854
		public override void AbortConnect()
		{
			this._connecting = false;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000C8660 File Offset: 0x000C6860
		protected override NamedPipeClientStream DoConnect(int timeout)
		{
			int tickCount = Environment.TickCount;
			this._connecting = true;
			while (!NamedPipeNative.WaitNamedPipe(this._pipeName, 100U))
			{
				int num = Environment.TickCount - tickCount;
				if (!this._connecting || num >= timeout)
				{
					this._connecting = false;
					throw new TimeoutException(RemotingErrorIdStrings.ConnectNamedPipeTimeout);
				}
			}
			this._connecting = false;
			return this.OpenNamedPipe();
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000C86C8 File Offset: 0x000C68C8
		private NamedPipeClientStream OpenNamedPipe()
		{
			uint dwFlagsAndAttributes = 1073741824U;
			SafePipeHandle safePipeHandle = NamedPipeNative.CreateFile(this._pipeName, 3221225472U, 0U, IntPtr.Zero, 3U, dwFlagsAndAttributes, IntPtr.Zero);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (safePipeHandle.IsInvalid)
			{
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotConnectNamedPipe, lastWin32Error));
			}
			NamedPipeClientStream result;
			try
			{
				result = new NamedPipeClientStream(PipeDirection.InOut, true, true, safePipeHandle);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				safePipeHandle.Dispose();
				throw;
			}
			return result;
		}

		// Token: 0x04001189 RID: 4489
		private volatile bool _connecting;
	}
}
