using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x0200037E RID: 894
	internal class OutOfProcessClientSessionTransportManager : OutOfProcessClientSessionTransportManagerBase
	{
		// Token: 0x06002BAB RID: 11179 RVA: 0x000F178C File Offset: 0x000EF98C
		internal OutOfProcessClientSessionTransportManager(Guid runspaceId, NewProcessConnectionInfo connectionInfo, PSRemotingCryptoHelper cryptoHelper) : base(runspaceId, cryptoHelper)
		{
			this.connectionInfo = connectionInfo;
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000F17A4 File Offset: 0x000EF9A4
		internal override void CreateAsync()
		{
			if (this.connectionInfo != null)
			{
				this._processInstance = (this.connectionInfo.Process ?? new PowerShellProcessInstance(this.connectionInfo.PSVersion, this.connectionInfo.Credential, this.connectionInfo.InitializationScript, this.connectionInfo.RunAs32));
				if (this.connectionInfo.Process != null)
				{
					this._processCreated = false;
				}
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateShell, PSOpcode.Connect, PSTask.CreateRunspace, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString()
			});
			try
			{
				lock (this.syncObject)
				{
					if (this.isClosed)
					{
						return;
					}
					this.serverProcess = this._processInstance.Process;
					if (this._processInstance.RunspacePool != null)
					{
						this._processInstance.RunspacePool.Close();
						this._processInstance.RunspacePool.Dispose();
					}
					this.stdInWriter = this._processInstance.StdInWriter;
					this.serverProcess.OutputDataReceived += this.OnOutputDataReceived;
					this.serverProcess.ErrorDataReceived += this.OnErrorDataReceived;
					this.serverProcess.Exited += base.OnExited;
					this._processInstance.Start();
					if (this.stdInWriter != null)
					{
						this.serverProcess.CancelErrorRead();
						this.serverProcess.CancelOutputRead();
					}
					this.serverProcess.BeginOutputReadLine();
					this.serverProcess.BeginErrorReadLine();
					this.stdInWriter = new OutOfProcessTextWriter(this.serverProcess.StandardInput);
					this._processInstance.StdInWriter = this.stdInWriter;
				}
			}
			catch (Win32Exception ex)
			{
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(new PSRemotingTransportException(ex, RemotingErrorIdStrings.IPCExceptionLaunchingProcess, new object[]
				{
					ex.Message
				})
				{
					ErrorCode = ex.HResult
				}, TransportMethodEnum.CreateShellEx);
				this.RaiseErrorHandler(eventArgs);
				return;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorId.IPCExceptionLaunchingProcess, RemotingErrorIdStrings.IPCExceptionLaunchingProcess, new object[]
				{
					ex2.Message
				});
				TransportErrorOccuredEventArgs eventArgs2 = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.CreateShellEx);
				this.RaiseErrorHandler(eventArgs2);
				return;
			}
			base.SendOneItem();
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000F1A4C File Offset: 0x000EFC4C
		internal override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing)
			{
				this.KillServerProcess();
				if (this.serverProcess != null && this._processCreated)
				{
					this.serverProcess.Dispose();
				}
			}
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000F1A79 File Offset: 0x000EFC79
		protected override void CleanupConnection()
		{
			this.KillServerProcess();
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000F1A81 File Offset: 0x000EFC81
		private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			base.HandleOutputDataReceived(e.Data);
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000F1A8F File Offset: 0x000EFC8F
		private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			base.HandleErrorDataReceived(e.Data);
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000F1AA0 File Offset: 0x000EFCA0
		private void KillServerProcess()
		{
			if (this.serverProcess == null)
			{
				return;
			}
			try
			{
				if (!this.serverProcess.HasExited)
				{
					this.serverProcess.Exited -= base.OnExited;
					if (this._processCreated)
					{
						this.serverProcess.CancelOutputRead();
						this.serverProcess.CancelErrorRead();
						this.serverProcess.Kill();
					}
					this.serverProcess.OutputDataReceived -= this.OnOutputDataReceived;
					this.serverProcess.ErrorDataReceived -= this.OnErrorDataReceived;
				}
			}
			catch (Win32Exception)
			{
				try
				{
					Process processById = Process.GetProcessById(this.serverProcess.Id);
					if (this._processCreated)
					{
						processById.Kill();
					}
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
		}

		// Token: 0x040015EB RID: 5611
		private Process serverProcess;

		// Token: 0x040015EC RID: 5612
		private NewProcessConnectionInfo connectionInfo;

		// Token: 0x040015ED RID: 5613
		private bool _processCreated = true;

		// Token: 0x040015EE RID: 5614
		private PowerShellProcessInstance _processInstance;
	}
}
