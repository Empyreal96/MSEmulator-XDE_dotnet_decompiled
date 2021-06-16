using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Remoting;
using System.Net;
using System.Text;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200024D RID: 589
	public sealed class PowerShellProcessInstance : IDisposable
	{
		// Token: 0x06001C18 RID: 7192 RVA: 0x000A3808 File Offset: 0x000A1A08
		public PowerShellProcessInstance(Version powerShellVersion, PSCredential credential, ScriptBlock initializationScript, bool useWow64)
		{
			string text = PowerShellProcessInstance.PSExePath;
			if (useWow64)
			{
				string environmentVariable = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
				if (!string.IsNullOrEmpty(environmentVariable) && (environmentVariable.Equals("amd64", StringComparison.OrdinalIgnoreCase) || environmentVariable.Equals("ia64", StringComparison.OrdinalIgnoreCase)))
				{
					text = PowerShellProcessInstance.PSExePath.ToLowerInvariant().Replace("\\system32\\", "\\syswow64\\");
					if (!File.Exists(text))
					{
						string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.IPCWowComponentNotPresent, new object[]
						{
							text
						});
						throw new PSInvalidOperationException(message);
					}
				}
			}
			string text2 = string.Empty;
			Version version = powerShellVersion ?? PSVersionInfo.PSVersion;
			if (null == version)
			{
				version = PSVersionInfo.PSVersion;
			}
			text2 = string.Format(CultureInfo.InvariantCulture, "-Version {0}", new object[]
			{
				new Version(version.Major, version.Minor)
			});
			text2 = string.Format(CultureInfo.InvariantCulture, "{0} -s -NoLogo -NoProfile", new object[]
			{
				text2
			});
			if (initializationScript != null)
			{
				string text3 = initializationScript.ToString();
				if (!string.IsNullOrEmpty(text3))
				{
					string text4 = Convert.ToBase64String(Encoding.Unicode.GetBytes(text3));
					text2 = string.Format(CultureInfo.InvariantCulture, "{0} -EncodedCommand {1}", new object[]
					{
						text2,
						text4
					});
				}
			}
			this._startInfo = new ProcessStartInfo
			{
				FileName = (useWow64 ? text : PowerShellProcessInstance.PSExePath),
				Arguments = text2,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true,
				LoadUserProfile = true
			};
			if (credential != null)
			{
				NetworkCredential networkCredential = credential.GetNetworkCredential();
				this._startInfo.UserName = networkCredential.UserName;
				this._startInfo.Domain = (string.IsNullOrEmpty(networkCredential.Domain) ? "." : networkCredential.Domain);
				this._startInfo.Password = credential.Password;
			}
			this._process = new Process
			{
				StartInfo = this._startInfo,
				EnableRaisingEvents = true
			};
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000A3A34 File Offset: 0x000A1C34
		public PowerShellProcessInstance() : this(null, null, null, false)
		{
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x000A3A40 File Offset: 0x000A1C40
		public bool HasExited
		{
			get
			{
				return this._processExited || (this._started && this._process != null && this._process.HasExited);
			}
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000A3A69 File Offset: 0x000A1C69
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000A3A78 File Offset: 0x000A1C78
		private void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				return;
			}
			lock (this._syncObject)
			{
				if (this._isDisposed)
				{
					return;
				}
				this._isDisposed = true;
			}
			if (disposing)
			{
				try
				{
					if (this._process != null && !this._process.HasExited)
					{
						this._process.Kill();
					}
				}
				catch (InvalidOperationException)
				{
				}
				catch (Win32Exception)
				{
				}
				catch (NotSupportedException)
				{
				}
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x000A3B20 File Offset: 0x000A1D20
		public Process Process
		{
			get
			{
				return this._process;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x000A3B28 File Offset: 0x000A1D28
		// (set) Token: 0x06001C1F RID: 7199 RVA: 0x000A3B6C File Offset: 0x000A1D6C
		internal RunspacePool RunspacePool
		{
			get
			{
				RunspacePool runspacePool;
				lock (this._syncObject)
				{
					runspacePool = this._runspacePool;
				}
				return runspacePool;
			}
			set
			{
				lock (this._syncObject)
				{
					this._runspacePool = value;
				}
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001C20 RID: 7200 RVA: 0x000A3BB0 File Offset: 0x000A1DB0
		// (set) Token: 0x06001C21 RID: 7201 RVA: 0x000A3BB8 File Offset: 0x000A1DB8
		internal OutOfProcessTextWriter StdInWriter
		{
			get
			{
				return this._textWriter;
			}
			set
			{
				this._textWriter = value;
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000A3BC4 File Offset: 0x000A1DC4
		internal void Start()
		{
			if (this.HasExited)
			{
				throw new InvalidOperationException();
			}
			lock (this._syncObject)
			{
				if (this._started)
				{
					return;
				}
				this._started = true;
				this._process.Exited += this.ProcessExited;
			}
			this._process.Start();
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000A3C40 File Offset: 0x000A1E40
		private void ProcessExited(object sender, EventArgs e)
		{
			lock (this._syncObject)
			{
				this._processExited = true;
			}
		}

		// Token: 0x04000B76 RID: 2934
		private readonly ProcessStartInfo _startInfo;

		// Token: 0x04000B77 RID: 2935
		private Process _process;

		// Token: 0x04000B78 RID: 2936
		private static readonly string PSExePath = Path.Combine(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID), "powershell.exe");

		// Token: 0x04000B79 RID: 2937
		private RunspacePool _runspacePool;

		// Token: 0x04000B7A RID: 2938
		private readonly object _syncObject = new object();

		// Token: 0x04000B7B RID: 2939
		private OutOfProcessTextWriter _textWriter;

		// Token: 0x04000B7C RID: 2940
		private bool _started;

		// Token: 0x04000B7D RID: 2941
		private bool _isDisposed;

		// Token: 0x04000B7E RID: 2942
		private bool _processExited;
	}
}
