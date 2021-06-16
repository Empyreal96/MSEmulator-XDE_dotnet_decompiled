using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000268 RID: 616
	internal class RemoteSessionStateProxy : SessionStateProxy
	{
		// Token: 0x06001D21 RID: 7457 RVA: 0x000A92CF File Offset: 0x000A74CF
		internal RemoteSessionStateProxy(RemoteRunspace runspace)
		{
			this._runspace = runspace;
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000A92E0 File Offset: 0x000A74E0
		public override void SetVariable(string name, object value)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (this.setVariableCommandNotFoundException != null)
			{
				throw this.setVariableCommandNotFoundException;
			}
			Pipeline pipeline = this._runspace.CreatePipeline();
			Command command = new Command("Microsoft.PowerShell.Utility\\Set-Variable");
			command.Parameters.Add("Name", name);
			command.Parameters.Add("Value", value);
			pipeline.Commands.Add(command);
			try
			{
				pipeline.Invoke();
			}
			catch (RemoteException ex)
			{
				if (string.Equals("CommandNotFoundException", ex.ErrorRecord.FullyQualifiedErrorId, StringComparison.OrdinalIgnoreCase))
				{
					this.setVariableCommandNotFoundException = new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, ex);
					throw this.setVariableCommandNotFoundException;
				}
				throw;
			}
			if (pipeline.Error.Count > 0)
			{
				ErrorRecord errorRecord = (ErrorRecord)pipeline.Error.Read();
				throw new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, errorRecord.Exception);
			}
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000A93CC File Offset: 0x000A75CC
		public override object GetVariable(string name)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (this.getVariableCommandNotFoundException != null)
			{
				throw this.getVariableCommandNotFoundException;
			}
			Pipeline pipeline = this._runspace.CreatePipeline();
			Command command = new Command("Microsoft.PowerShell.Utility\\Get-Variable");
			command.Parameters.Add("Name", name);
			pipeline.Commands.Add(command);
			Collection<PSObject> collection = null;
			try
			{
				collection = pipeline.Invoke();
			}
			catch (RemoteException ex)
			{
				if (string.Equals("CommandNotFoundException", ex.ErrorRecord.FullyQualifiedErrorId, StringComparison.OrdinalIgnoreCase))
				{
					this.getVariableCommandNotFoundException = new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, ex);
					throw this.getVariableCommandNotFoundException;
				}
				throw;
			}
			if (pipeline.Error.Count > 0)
			{
				ErrorRecord errorRecord = (ErrorRecord)pipeline.Error.Read();
				if (string.Equals("CommandNotFoundException", errorRecord.FullyQualifiedErrorId, StringComparison.OrdinalIgnoreCase))
				{
					throw new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, errorRecord.Exception);
				}
				throw new PSInvalidOperationException(errorRecord.Exception.Message, errorRecord.Exception);
			}
			else
			{
				if (collection.Count != 1)
				{
					return null;
				}
				return collection[0].Properties["Value"].Value;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001D24 RID: 7460 RVA: 0x000A9500 File Offset: 0x000A7700
		public override List<string> Applications
		{
			get
			{
				if (this.isInNoLangugeModeException != null)
				{
					throw this.isInNoLangugeModeException;
				}
				Pipeline pipeline = this._runspace.CreatePipeline();
				pipeline.Commands.AddScript("$executionContext.SessionState.Applications");
				List<string> list = new List<string>();
				try
				{
					foreach (PSObject psobject in pipeline.Invoke())
					{
						list.Add(psobject.BaseObject as string);
					}
				}
				catch (RemoteException ex)
				{
					if (ex.ErrorRecord.CategoryInfo.Category == ErrorCategory.ParserError)
					{
						this.isInNoLangugeModeException = new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, ex);
						throw this.isInNoLangugeModeException;
					}
					throw;
				}
				return list;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x000A95CC File Offset: 0x000A77CC
		public override List<string> Scripts
		{
			get
			{
				if (this.isInNoLangugeModeException != null)
				{
					throw this.isInNoLangugeModeException;
				}
				Pipeline pipeline = this._runspace.CreatePipeline();
				pipeline.Commands.AddScript("$executionContext.SessionState.Scripts");
				List<string> list = new List<string>();
				try
				{
					foreach (PSObject psobject in pipeline.Invoke())
					{
						list.Add(psobject.BaseObject as string);
					}
				}
				catch (RemoteException ex)
				{
					if (ex.ErrorRecord.CategoryInfo.Category == ErrorCategory.ParserError)
					{
						this.isInNoLangugeModeException = new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, ex);
						throw this.isInNoLangugeModeException;
					}
					throw;
				}
				return list;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x000A9698 File Offset: 0x000A7898
		public override DriveManagementIntrinsics Drive
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x000A96A0 File Offset: 0x000A78A0
		// (set) Token: 0x06001D28 RID: 7464 RVA: 0x000A973C File Offset: 0x000A793C
		public override PSLanguageMode LanguageMode
		{
			get
			{
				if (this.isInNoLangugeModeException != null)
				{
					return PSLanguageMode.NoLanguage;
				}
				Pipeline pipeline = this._runspace.CreatePipeline();
				pipeline.Commands.AddScript("$executionContext.SessionState.LanguageMode");
				Collection<PSObject> collection = null;
				try
				{
					collection = pipeline.Invoke();
				}
				catch (RemoteException ex)
				{
					if (ex.ErrorRecord.CategoryInfo.Category == ErrorCategory.ParserError)
					{
						this.isInNoLangugeModeException = new PSNotSupportedException(RunspaceStrings.NotSupportedOnRestrictedRunspace, ex);
						return PSLanguageMode.NoLanguage;
					}
					throw;
				}
				return (PSLanguageMode)LanguagePrimitives.ConvertTo(collection[0], typeof(PSLanguageMode), CultureInfo.InvariantCulture);
			}
			set
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x000A9743 File Offset: 0x000A7943
		public override PSModuleInfo Module
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x000A974A File Offset: 0x000A794A
		public override PathIntrinsics Path
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x000A9751 File Offset: 0x000A7951
		public override CmdletProviderManagementIntrinsics Provider
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x000A9758 File Offset: 0x000A7958
		public override PSVariableIntrinsics PSVariable
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x000A975F File Offset: 0x000A795F
		public override CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x000A9766 File Offset: 0x000A7966
		public override ProviderIntrinsics InvokeProvider
		{
			get
			{
				throw new PSNotSupportedException();
			}
		}

		// Token: 0x04000CEE RID: 3310
		private RemoteRunspace _runspace;

		// Token: 0x04000CEF RID: 3311
		private Exception isInNoLangugeModeException;

		// Token: 0x04000CF0 RID: 3312
		private Exception getVariableCommandNotFoundException;

		// Token: 0x04000CF1 RID: 3313
		private Exception setVariableCommandNotFoundException;
	}
}
