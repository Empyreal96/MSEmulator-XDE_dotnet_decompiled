using System;
using System.IO.Pipes;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002F0 RID: 752
	internal sealed class ContainerSessionNamedPipeClient : NamedPipeClientBase
	{
		// Token: 0x060023C0 RID: 9152 RVA: 0x000C874C File Offset: 0x000C694C
		public ContainerSessionNamedPipeClient(int procId, string appDomainName, string containerId)
		{
			if (string.IsNullOrEmpty(containerId))
			{
				throw new PSArgumentNullException("containerId");
			}
			this._pipeName = "\\\\.\\Containers\\" + containerId + "\\Device\\NamedPipe\\" + NamedPipeUtils.CreateProcessPipeName(procId, appDomainName);
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000C8784 File Offset: 0x000C6984
		protected override NamedPipeClientStream DoConnect(int timeout)
		{
			uint dwFlagsAndAttributes = 1073741824U;
			int tickCount = Environment.TickCount;
			SafePipeHandle safePipeHandle = null;
			int lastWin32Error;
			for (;;)
			{
				safePipeHandle = NamedPipeNative.CreateFile(this._pipeName, 3221225472U, 0U, IntPtr.Zero, 3U, dwFlagsAndAttributes, IntPtr.Zero);
				lastWin32Error = Marshal.GetLastWin32Error();
				if (!safePipeHandle.IsInvalid)
				{
					goto IL_70;
				}
				if ((long)lastWin32Error != 2L)
				{
					break;
				}
				int num = Environment.TickCount - tickCount;
				Thread.Sleep(100);
				if (num >= timeout)
				{
					goto Block_3;
				}
			}
			throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotConnectContainerNamedPipe, lastWin32Error));
			Block_3:
			NamedPipeClientStream result;
			try
			{
				IL_70:
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
	}
}
