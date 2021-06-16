using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Tracing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002ED RID: 749
	internal sealed class RemoteSessionNamedPipeServer : IDisposable
	{
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002397 RID: 9111 RVA: 0x000C7A0F File Offset: 0x000C5C0F
		public NamedPipeServerStream Stream
		{
			get
			{
				return this._serverPipeStream;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x000C7A17 File Offset: 0x000C5C17
		public string PipeName
		{
			get
			{
				return this._pipeName;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x000C7A1F File Offset: 0x000C5C1F
		public bool IsListenerRunning
		{
			get
			{
				return this._listenerRunning;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600239A RID: 9114 RVA: 0x000C7A27 File Offset: 0x000C5C27
		public StreamReader TextReader
		{
			get
			{
				return this._streamReader;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x0600239B RID: 9115 RVA: 0x000C7A2F File Offset: 0x000C5C2F
		public StreamWriter TextWriter
		{
			get
			{
				return this._streamWriter;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600239C RID: 9116 RVA: 0x000C7A37 File Offset: 0x000C5C37
		public bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600239D RID: 9117 RVA: 0x000C7A3F File Offset: 0x000C5C3F
		internal static int NamedPipeBufferSizeForRemoting
		{
			get
			{
				return 32768;
			}
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x0600239E RID: 9118 RVA: 0x000C7A48 File Offset: 0x000C5C48
		// (remove) Token: 0x0600239F RID: 9119 RVA: 0x000C7A80 File Offset: 0x000C5C80
		public event EventHandler<ListenerEndedEventArgs> ListenerEnded;

		// Token: 0x060023A0 RID: 9120 RVA: 0x000C7AB8 File Offset: 0x000C5CB8
		public static RemoteSessionNamedPipeServer CreateRemoteSessionNamedPipeServer()
		{
			string currentAppDomainName = NamedPipeUtils.GetCurrentAppDomainName();
			return new RemoteSessionNamedPipeServer(NamedPipeUtils.CreateProcessPipeName(Process.GetCurrentProcess(), currentAppDomainName));
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000C7ADC File Offset: 0x000C5CDC
		internal RemoteSessionNamedPipeServer(string pipeName)
		{
			if (pipeName == null)
			{
				throw new PSArgumentNullException("pipeName");
			}
			this._syncObject = new object();
			this._pipeName = pipeName;
			this._serverPipeStream = this.CreateNamedPipe(".", "pipe", pipeName, RemoteSessionNamedPipeServer.GetServerPipeSecurity());
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000C7B38 File Offset: 0x000C5D38
		private NamedPipeServerStream CreateNamedPipe(string serverName, string namespaceName, string coreName, CommonSecurityDescriptor securityDesc)
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
			string lpName = string.Concat(new string[]
			{
				"\\\\",
				serverName,
				"\\",
				namespaceName,
				"\\",
				coreName
			});
			NamedPipeNative.SECURITY_ATTRIBUTES securityAttributes = null;
			GCHandle? gchandle = null;
			if (securityDesc != null)
			{
				byte[] array = new byte[securityDesc.BinaryLength];
				securityDesc.GetBinaryForm(array, 0);
				gchandle = new GCHandle?(GCHandle.Alloc(array, GCHandleType.Pinned));
				securityAttributes = NamedPipeNative.GetSecurityAttributes(gchandle.Value);
			}
			SafePipeHandle safePipeHandle = NamedPipeNative.CreateNamedPipe(lpName, 1074266115U, 6U, 1U, 32768U, 32768U, 0U, securityAttributes);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (gchandle != null)
			{
				gchandle.Value.Free();
			}
			if (safePipeHandle.IsInvalid)
			{
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotCreateNamedPipe, lastWin32Error));
			}
			NamedPipeServerStream result;
			try
			{
				result = new NamedPipeServerStream(PipeDirection.InOut, true, false, safePipeHandle);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				safePipeHandle.Dispose();
				throw;
			}
			return result;
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000C7C78 File Offset: 0x000C5E78
		static RemoteSessionNamedPipeServer()
		{
			RemoteSessionNamedPipeServer.CreateIPCNamedPipeServerSingleton();
			RemoteSessionNamedPipeServer.CreateAppDomainUnloadHandler();
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000C7C94 File Offset: 0x000C5E94
		public void Dispose()
		{
			lock (this._syncObject)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
			}
			if (this._streamReader != null)
			{
				try
				{
					this._streamReader.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				this._streamReader = null;
			}
			if (this._streamWriter != null)
			{
				try
				{
					this._streamWriter.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				this._streamWriter = null;
			}
			if (this._serverPipeStream != null)
			{
				try
				{
					this._serverPipeStream.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000C7D5C File Offset: 0x000C5F5C
		public void StartListening(Action<RemoteSessionNamedPipeServer> clientConnectCallback)
		{
			if (clientConnectCallback == null)
			{
				throw new PSArgumentNullException("clientConnectCallback");
			}
			lock (this._syncObject)
			{
				if (this._listenerRunning)
				{
					throw new InvalidOperationException(RemotingErrorIdStrings.NamedPipeAlreadyListening);
				}
				this._listenerRunning = true;
				new Thread(new ParameterizedThreadStart(this.ProcessListeningThread))
				{
					Name = "IPC Listener Thread",
					IsBackground = true
				}.Start(clientConnectCallback);
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000C7DEC File Offset: 0x000C5FEC
		private static CommonSecurityDescriptor GetServerPipeSecurity()
		{
			SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
			DiscretionaryAcl discretionaryAcl = new DiscretionaryAcl(false, false, 1);
			discretionaryAcl.AddAccess(AccessControlType.Allow, sid, 2032031, InheritanceFlags.None, PropagationFlags.None);
			CommonSecurityDescriptor commonSecurityDescriptor = new CommonSecurityDescriptor(false, false, ControlFlags.OwnerDefaulted | ControlFlags.GroupDefaulted | ControlFlags.DiscretionaryAclPresent, null, null, null, discretionaryAcl);
			if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
			{
				commonSecurityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Allow, WindowsIdentity.GetCurrent().User, 2032031, InheritanceFlags.None, PropagationFlags.None);
			}
			return commonSecurityDescriptor;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000C7E5D File Offset: 0x000C605D
		private void WaitForConnection()
		{
			this._serverPipeStream.WaitForConnection();
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000C7E6C File Offset: 0x000C606C
		private void ProcessListeningThread(object state)
		{
			string text = Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture);
			string currentAppDomainName = NamedPipeUtils.GetCurrentAppDomainName();
			this._tracer.WriteMessage("RemoteSessionNamedPipeServer", "StartListening", Guid.Empty, "Listener thread started on Process {0} in AppDomainName {1}.", new string[]
			{
				text,
				currentAppDomainName
			});
			PSEtwLog.LogOperationalInformation(PSEventId.NamedPipeIPC_ServerListenerStarted, PSOpcode.Open, PSTask.NamedPipe, PSKeyword.UseAlwaysOperational, new object[]
			{
				text,
				currentAppDomainName
			});
			Exception ex = null;
			string text2 = string.Empty;
			bool restartListener = true;
			try
			{
				this.WaitForConnection();
				try
				{
					text2 = WindowsIdentity.GetCurrent().Name;
				}
				catch (SecurityException)
				{
				}
				this._tracer.WriteMessage("RemoteSessionNamedPipeServer", "StartListening", Guid.Empty, "Client connection started on Process {0} in AppDomainName {1} for User {2}.", new string[]
				{
					text,
					currentAppDomainName,
					text2
				});
				PSEtwLog.LogOperationalInformation(PSEventId.NamedPipeIPC_ServerConnect, PSOpcode.Connect, PSTask.NamedPipe, PSKeyword.UseAlwaysOperational, new object[]
				{
					text,
					currentAppDomainName,
					text2
				});
				this._streamReader = new StreamReader(this._serverPipeStream);
				this._streamWriter = new StreamWriter(this._serverPipeStream);
				this._streamWriter.AutoFlush = true;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				ex = ex2;
			}
			if (ex != null)
			{
				string text3 = (!string.IsNullOrEmpty(ex.Message)) ? ex.Message : string.Empty;
				this._tracer.WriteMessage("RemoteSessionNamedPipeServer", "StartListening", Guid.Empty, "Unexpected error in listener thread on process {0} in AppDomainName {1}.  Error Message: {2}", new string[]
				{
					text,
					currentAppDomainName,
					text3
				});
				PSEtwLog.LogOperationalError(PSEventId.NamedPipeIPC_ServerListenerError, PSOpcode.Exception, PSTask.NamedPipe, PSKeyword.UseAlwaysOperational, new object[]
				{
					text,
					currentAppDomainName,
					text3
				});
				this.Dispose();
				return;
			}
			ex = null;
			try
			{
				Action<RemoteSessionNamedPipeServer> action = state as Action<RemoteSessionNamedPipeServer>;
				action(this);
			}
			catch (IOException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				ex = ex3;
				restartListener = false;
			}
			this._tracer.WriteMessage("RemoteSessionNamedPipeServer", "StartListening", Guid.Empty, "Client connection ended on process {0} in AppDomainName {1} for User {2}.", new string[]
			{
				text,
				currentAppDomainName,
				text2
			});
			PSEtwLog.LogOperationalInformation(PSEventId.NamedPipeIPC_ServerDisconnect, PSOpcode.Close, PSTask.NamedPipe, PSKeyword.UseAlwaysOperational, new object[]
			{
				text,
				currentAppDomainName,
				text2
			});
			if (ex == null)
			{
				this._tracer.WriteMessage("RemoteSessionNamedPipeServer", "StartListening", Guid.Empty, "Listener thread ended on process {0} in AppDomainName {1}.", new string[]
				{
					text,
					currentAppDomainName
				});
				PSEtwLog.LogOperationalInformation(PSEventId.NamedPipeIPC_ServerListenerEnded, PSOpcode.Close, PSTask.NamedPipe, PSKeyword.UseAlwaysOperational, new object[]
				{
					text,
					currentAppDomainName
				});
			}
			else
			{
				string text4 = (!string.IsNullOrEmpty(ex.Message)) ? ex.Message : string.Empty;
				this._tracer.WriteMessage("RemoteSessionNamedPipeServer", "StartListening", Guid.Empty, "Unexpected error in listener thread on process {0} in AppDomainName {1}.  Error Message: {2}", new string[]
				{
					text,
					currentAppDomainName,
					text4
				});
				PSEtwLog.LogOperationalError(PSEventId.NamedPipeIPC_ServerListenerError, PSOpcode.Exception, PSTask.NamedPipe, PSKeyword.UseAlwaysOperational, new object[]
				{
					text,
					currentAppDomainName,
					text4
				});
			}
			lock (this._syncObject)
			{
				this._listenerRunning = false;
			}
			this.Dispose();
			this.ListenerEnded.SafeInvoke(this, new ListenerEndedEventArgs(ex, restartListener));
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000C8278 File Offset: 0x000C6478
		internal static void RunServerMode()
		{
			RemoteSessionNamedPipeServer.IPCNamedPipeServerEnabled = true;
			RemoteSessionNamedPipeServer.CreateIPCNamedPipeServerSingleton();
			if (RemoteSessionNamedPipeServer.IPCNamedPipeServer == null)
			{
				throw new RuntimeException(RemotingErrorIdStrings.NamedPipeServerCannotStart);
			}
			ManualResetEventSlim clientConnectionEnded = new ManualResetEventSlim(false);
			RemoteSessionNamedPipeServer.IPCNamedPipeServer.ListenerEnded -= RemoteSessionNamedPipeServer.OnIPCNamedPipeServerEnded;
			RemoteSessionNamedPipeServer.IPCNamedPipeServer.ListenerEnded += delegate(object sender, ListenerEndedEventArgs e)
			{
				clientConnectionEnded.Set();
			};
			clientConnectionEnded.Wait();
			clientConnectionEnded.Dispose();
			RemoteSessionNamedPipeServer.IPCNamedPipeServerEnabled = false;
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x000C82FC File Offset: 0x000C64FC
		internal static void CreateIPCNamedPipeServerSingleton()
		{
			lock (RemoteSessionNamedPipeServer.SyncObject)
			{
				if (RemoteSessionNamedPipeServer.IPCNamedPipeServerEnabled)
				{
					if (RemoteSessionNamedPipeServer.IPCNamedPipeServer != null)
					{
						if (!RemoteSessionNamedPipeServer.IPCNamedPipeServer.IsDisposed)
						{
							goto IL_78;
						}
					}
					try
					{
						try
						{
							RemoteSessionNamedPipeServer.IPCNamedPipeServer = RemoteSessionNamedPipeServer.CreateRemoteSessionNamedPipeServer();
						}
						catch (IOException)
						{
							return;
						}
						RemoteSessionNamedPipeServer.IPCNamedPipeServer.ListenerEnded += RemoteSessionNamedPipeServer.OnIPCNamedPipeServerEnded;
						RemoteSessionNamedPipeServer.IPCNamedPipeServer.StartListening(new Action<RemoteSessionNamedPipeServer>(RemoteSessionNamedPipeServer.ClientConnectionCallback));
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
						RemoteSessionNamedPipeServer.IPCNamedPipeServer = null;
					}
					IL_78:;
				}
			}
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x000C8404 File Offset: 0x000C6604
		private static void CreateAppDomainUnloadHandler()
		{
			AppDomain.CurrentDomain.DomainUnload += delegate(object sender, EventArgs args)
			{
				RemoteSessionNamedPipeServer.IPCNamedPipeServerEnabled = false;
				RemoteSessionNamedPipeServer ipcnamedPipeServer = RemoteSessionNamedPipeServer.IPCNamedPipeServer;
				if (ipcnamedPipeServer != null)
				{
					try
					{
						ipcnamedPipeServer.Dispose();
					}
					catch (ObjectDisposedException)
					{
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
			};
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x000C842D File Offset: 0x000C662D
		private static void OnIPCNamedPipeServerEnded(object sender, ListenerEndedEventArgs args)
		{
			if (args.RestartListener)
			{
				RemoteSessionNamedPipeServer.CreateIPCNamedPipeServerSingleton();
			}
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x000C843C File Offset: 0x000C663C
		private static void ClientConnectionCallback(RemoteSessionNamedPipeServer pipeServer)
		{
			NamedPipeProcessMediator.Run(string.Empty, pipeServer);
		}

		// Token: 0x04001174 RID: 4468
		private const string _threadName = "IPC Listener Thread";

		// Token: 0x04001175 RID: 4469
		private const int _namedPipeBufferSizeForRemoting = 32768;

		// Token: 0x04001176 RID: 4470
		private const int _pipeAccessMaskFullControl = 2032031;

		// Token: 0x04001177 RID: 4471
		private readonly NamedPipeServerStream _serverPipeStream;

		// Token: 0x04001178 RID: 4472
		private readonly string _pipeName;

		// Token: 0x04001179 RID: 4473
		private readonly object _syncObject;

		// Token: 0x0400117A RID: 4474
		private bool _disposed;

		// Token: 0x0400117B RID: 4475
		private bool _listenerRunning;

		// Token: 0x0400117C RID: 4476
		private StreamReader _streamReader;

		// Token: 0x0400117D RID: 4477
		private StreamWriter _streamWriter;

		// Token: 0x0400117E RID: 4478
		private PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x0400117F RID: 4479
		private static object SyncObject = new object();

		// Token: 0x04001180 RID: 4480
		internal static RemoteSessionNamedPipeServer IPCNamedPipeServer;

		// Token: 0x04001181 RID: 4481
		internal static bool IPCNamedPipeServerEnabled = true;
	}
}
