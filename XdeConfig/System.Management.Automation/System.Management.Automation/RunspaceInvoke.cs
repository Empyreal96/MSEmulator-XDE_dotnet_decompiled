using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000253 RID: 595
	public class RunspaceInvoke : IDisposable
	{
		// Token: 0x06001C3A RID: 7226 RVA: 0x000A496C File Offset: 0x000A2B6C
		public RunspaceInvoke()
		{
			RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
			this._runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
			this._runspace.Open();
			if (Runspace.DefaultRunspace == null)
			{
				Runspace.DefaultRunspace = this._runspace;
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000A49AE File Offset: 0x000A2BAE
		public RunspaceInvoke(RunspaceConfiguration runspaceConfiguration)
		{
			if (runspaceConfiguration == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceConfiguration");
			}
			this._runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
			this._runspace.Open();
			if (Runspace.DefaultRunspace == null)
			{
				Runspace.DefaultRunspace = this._runspace;
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x000A49F0 File Offset: 0x000A2BF0
		public RunspaceInvoke(string consoleFilePath)
		{
			if (consoleFilePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("consoleFilePath");
			}
			PSConsoleLoadException ex;
			RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create(consoleFilePath, out ex);
			if (ex != null)
			{
				throw ex;
			}
			this._runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
			this._runspace.Open();
			if (Runspace.DefaultRunspace == null)
			{
				Runspace.DefaultRunspace = this._runspace;
			}
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000A4A48 File Offset: 0x000A2C48
		public RunspaceInvoke(Runspace runspace)
		{
			if (runspace == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspace");
			}
			this._runspace = runspace;
			if (Runspace.DefaultRunspace == null)
			{
				Runspace.DefaultRunspace = this._runspace;
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000A4A77 File Offset: 0x000A2C77
		public Collection<PSObject> Invoke(string script)
		{
			return this.Invoke(script, null);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000A4A84 File Offset: 0x000A2C84
		public Collection<PSObject> Invoke(string script, IEnumerable input)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("runspace");
			}
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			Pipeline pipeline = this._runspace.CreatePipeline(script);
			return pipeline.Invoke(input);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000A4AC8 File Offset: 0x000A2CC8
		public Collection<PSObject> Invoke(string script, IEnumerable input, out IList errors)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("runspace");
			}
			if (script == null)
			{
				throw PSTraceSource.NewArgumentNullException("script");
			}
			Pipeline pipeline = this._runspace.CreatePipeline(script);
			Collection<PSObject> result = pipeline.Invoke(input);
			errors = pipeline.Error.NonBlockingRead();
			return result;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x000A4B19 File Offset: 0x000A2D19
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x000A4B28 File Offset: 0x000A2D28
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed && disposing)
			{
				this._runspace.Close();
				this._runspace = null;
			}
			this._disposed = true;
		}

		// Token: 0x04000BAA RID: 2986
		private Runspace _runspace;

		// Token: 0x04000BAB RID: 2987
		private bool _disposed;
	}
}
