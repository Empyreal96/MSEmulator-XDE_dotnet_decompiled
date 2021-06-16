using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Resources;
using System.Security.AccessControl;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000462 RID: 1122
	public abstract class CmdletProvider : IResourceSupplier
	{
		// Token: 0x06003171 RID: 12657 RVA: 0x0010D2B0 File Offset: 0x0010B4B0
		internal void SetProviderInformation(ProviderInfo providerInfoToSet)
		{
			if (providerInfoToSet == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInfoToSet");
			}
			this.providerInformation = providerInfoToSet;
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x0010D2C8 File Offset: 0x0010B4C8
		internal virtual bool IsFilterSet()
		{
			return !string.IsNullOrEmpty(this.Filter);
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x0010D2E5 File Offset: 0x0010B4E5
		// (set) Token: 0x06003174 RID: 12660 RVA: 0x0010D2F0 File Offset: 0x0010B4F0
		internal CmdletProviderContext Context
		{
			get
			{
				return this.contextBase;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				if (value.Credential != null && value.Credential != PSCredential.Empty && !CmdletProviderManagementIntrinsics.CheckProviderCapabilities(ProviderCapabilities.Credentials, this.providerInformation))
				{
					throw PSTraceSource.NewNotSupportedException(SessionStateStrings.Credentials_NotSupported, new object[0]);
				}
				if (this.providerInformation != null && !string.IsNullOrEmpty(this.providerInformation.Name) && this.providerInformation.Name.Equals("FileSystem") && value.Credential != null && value.Credential != PSCredential.Empty && !value.ExecutionContext.CurrentCommandProcessor.Command.GetType().Name.Equals("NewPSDriveCommand"))
				{
					throw PSTraceSource.NewNotSupportedException(SessionStateStrings.FileSystemProviderCredentials_NotSupported, new object[0]);
				}
				if (!string.IsNullOrEmpty(value.Filter) && !CmdletProviderManagementIntrinsics.CheckProviderCapabilities(ProviderCapabilities.Filter, this.providerInformation))
				{
					throw PSTraceSource.NewNotSupportedException(SessionStateStrings.Filter_NotSupported, new object[0]);
				}
				if (value.UseTransaction && !CmdletProviderManagementIntrinsics.CheckProviderCapabilities(ProviderCapabilities.Transactions, this.providerInformation))
				{
					throw PSTraceSource.NewNotSupportedException(SessionStateStrings.Transactions_NotSupported, new object[0]);
				}
				this.contextBase = value;
				this.contextBase.ProviderInstance = this;
			}
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x0010D424 File Offset: 0x0010B624
		internal ProviderInfo Start(ProviderInfo providerInfo, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			return this.Start(providerInfo);
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x0010D434 File Offset: 0x0010B634
		internal object StartDynamicParameters(CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			return this.StartDynamicParameters();
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x0010D443 File Offset: 0x0010B643
		internal void Stop(CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			this.Stop();
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0010D452 File Offset: 0x0010B652
		protected internal virtual void StopProcessing()
		{
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x0010D454 File Offset: 0x0010B654
		internal void GetProperty(string path, Collection<string> providerSpecificPickList, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IPropertyCmdletProvider propertyCmdletProvider = this as IPropertyCmdletProvider;
			if (propertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IPropertyCmdletProvider_NotSupported, new object[0]);
			}
			propertyCmdletProvider.GetProperty(path, providerSpecificPickList);
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x0010D48C File Offset: 0x0010B68C
		internal object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IPropertyCmdletProvider propertyCmdletProvider = this as IPropertyCmdletProvider;
			if (propertyCmdletProvider == null)
			{
				return null;
			}
			return propertyCmdletProvider.GetPropertyDynamicParameters(path, providerSpecificPickList);
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x0010D4B4 File Offset: 0x0010B6B4
		internal void SetProperty(string path, PSObject propertyValue, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IPropertyCmdletProvider propertyCmdletProvider = this as IPropertyCmdletProvider;
			if (propertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IPropertyCmdletProvider_NotSupported, new object[0]);
			}
			propertyCmdletProvider.SetProperty(path, propertyValue);
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x0010D4EC File Offset: 0x0010B6EC
		internal object SetPropertyDynamicParameters(string path, PSObject propertyValue, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IPropertyCmdletProvider propertyCmdletProvider = this as IPropertyCmdletProvider;
			if (propertyCmdletProvider == null)
			{
				return null;
			}
			return propertyCmdletProvider.SetPropertyDynamicParameters(path, propertyValue);
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x0010D514 File Offset: 0x0010B714
		internal void ClearProperty(string path, Collection<string> propertyName, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IPropertyCmdletProvider propertyCmdletProvider = this as IPropertyCmdletProvider;
			if (propertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IPropertyCmdletProvider_NotSupported, new object[0]);
			}
			propertyCmdletProvider.ClearProperty(path, propertyName);
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x0010D54C File Offset: 0x0010B74C
		internal object ClearPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IPropertyCmdletProvider propertyCmdletProvider = this as IPropertyCmdletProvider;
			if (propertyCmdletProvider == null)
			{
				return null;
			}
			return propertyCmdletProvider.ClearPropertyDynamicParameters(path, providerSpecificPickList);
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x0010D574 File Offset: 0x0010B774
		internal void NewProperty(string path, string propertyName, string propertyTypeName, object value, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IDynamicPropertyCmdletProvider_NotSupported, new object[0]);
			}
			dynamicPropertyCmdletProvider.NewProperty(path, propertyName, propertyTypeName, value);
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x0010D5B0 File Offset: 0x0010B7B0
		internal object NewPropertyDynamicParameters(string path, string propertyName, string propertyTypeName, object value, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				return null;
			}
			return dynamicPropertyCmdletProvider.NewPropertyDynamicParameters(path, propertyName, propertyTypeName, value);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x0010D5DC File Offset: 0x0010B7DC
		internal void RemoveProperty(string path, string propertyName, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IDynamicPropertyCmdletProvider_NotSupported, new object[0]);
			}
			dynamicPropertyCmdletProvider.RemoveProperty(path, propertyName);
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x0010D614 File Offset: 0x0010B814
		internal object RemovePropertyDynamicParameters(string path, string propertyName, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				return null;
			}
			return dynamicPropertyCmdletProvider.RemovePropertyDynamicParameters(path, propertyName);
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x0010D63C File Offset: 0x0010B83C
		internal void RenameProperty(string path, string propertyName, string newPropertyName, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IDynamicPropertyCmdletProvider_NotSupported, new object[0]);
			}
			dynamicPropertyCmdletProvider.RenameProperty(path, propertyName, newPropertyName);
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x0010D678 File Offset: 0x0010B878
		internal object RenamePropertyDynamicParameters(string path, string sourceProperty, string destinationProperty, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				return null;
			}
			return dynamicPropertyCmdletProvider.RenamePropertyDynamicParameters(path, sourceProperty, destinationProperty);
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x0010D6A4 File Offset: 0x0010B8A4
		internal void CopyProperty(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IDynamicPropertyCmdletProvider_NotSupported, new object[0]);
			}
			dynamicPropertyCmdletProvider.CopyProperty(sourcePath, sourceProperty, destinationPath, destinationProperty);
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x0010D6E0 File Offset: 0x0010B8E0
		internal object CopyPropertyDynamicParameters(string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				return null;
			}
			return dynamicPropertyCmdletProvider.CopyPropertyDynamicParameters(path, sourceProperty, destinationPath, destinationProperty);
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x0010D70C File Offset: 0x0010B90C
		internal void MoveProperty(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IDynamicPropertyCmdletProvider_NotSupported, new object[0]);
			}
			dynamicPropertyCmdletProvider.MoveProperty(sourcePath, sourceProperty, destinationPath, destinationProperty);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x0010D748 File Offset: 0x0010B948
		internal object MovePropertyDynamicParameters(string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IDynamicPropertyCmdletProvider dynamicPropertyCmdletProvider = this as IDynamicPropertyCmdletProvider;
			if (dynamicPropertyCmdletProvider == null)
			{
				return null;
			}
			return dynamicPropertyCmdletProvider.MovePropertyDynamicParameters(path, sourceProperty, destinationPath, destinationProperty);
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x0010D774 File Offset: 0x0010B974
		internal IContentReader GetContentReader(string path, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IContentCmdletProvider contentCmdletProvider = this as IContentCmdletProvider;
			if (contentCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IContentCmdletProvider_NotSupported, new object[0]);
			}
			return contentCmdletProvider.GetContentReader(path);
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0010D7AC File Offset: 0x0010B9AC
		internal object GetContentReaderDynamicParameters(string path, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IContentCmdletProvider contentCmdletProvider = this as IContentCmdletProvider;
			if (contentCmdletProvider == null)
			{
				return null;
			}
			return contentCmdletProvider.GetContentReaderDynamicParameters(path);
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x0010D7D4 File Offset: 0x0010B9D4
		internal IContentWriter GetContentWriter(string path, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IContentCmdletProvider contentCmdletProvider = this as IContentCmdletProvider;
			if (contentCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IContentCmdletProvider_NotSupported, new object[0]);
			}
			return contentCmdletProvider.GetContentWriter(path);
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x0010D80C File Offset: 0x0010BA0C
		internal object GetContentWriterDynamicParameters(string path, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IContentCmdletProvider contentCmdletProvider = this as IContentCmdletProvider;
			if (contentCmdletProvider == null)
			{
				return null;
			}
			return contentCmdletProvider.GetContentWriterDynamicParameters(path);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x0010D834 File Offset: 0x0010BA34
		internal void ClearContent(string path, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IContentCmdletProvider contentCmdletProvider = this as IContentCmdletProvider;
			if (contentCmdletProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IContentCmdletProvider_NotSupported, new object[0]);
			}
			contentCmdletProvider.ClearContent(path);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x0010D86C File Offset: 0x0010BA6C
		internal object ClearContentDynamicParameters(string path, CmdletProviderContext cmdletProviderContext)
		{
			this.Context = cmdletProviderContext;
			IContentCmdletProvider contentCmdletProvider = this as IContentCmdletProvider;
			if (contentCmdletProvider == null)
			{
				return null;
			}
			return contentCmdletProvider.ClearContentDynamicParameters(path);
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x0010D894 File Offset: 0x0010BA94
		protected virtual ProviderInfo Start(ProviderInfo providerInfo)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
			return providerInfo;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x0010D8C8 File Offset: 0x0010BAC8
		protected virtual object StartDynamicParameters()
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x0010D8FC File Offset: 0x0010BAFC
		protected virtual void Stop()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x0010D92C File Offset: 0x0010BB2C
		public bool Stopping
		{
			get
			{
				bool stopping;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					stopping = this.Context.Stopping;
				}
				return stopping;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x0010D968 File Offset: 0x0010BB68
		public SessionState SessionState
		{
			get
			{
				SessionState result;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					result = new SessionState(this.Context.ExecutionContext.EngineSessionState);
				}
				return result;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x0010D9B0 File Offset: 0x0010BBB0
		public ProviderIntrinsics InvokeProvider
		{
			get
			{
				ProviderIntrinsics result;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					result = new ProviderIntrinsics(this.Context.ExecutionContext.EngineSessionState);
				}
				return result;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x0010D9F8 File Offset: 0x0010BBF8
		public CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				CommandInvocationIntrinsics result;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					result = new CommandInvocationIntrinsics(this.Context.ExecutionContext);
				}
				return result;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x0010DA3C File Offset: 0x0010BC3C
		public PSCredential Credential
		{
			get
			{
				PSCredential credential;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					credential = this.Context.Credential;
				}
				return credential;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x0010DA78 File Offset: 0x0010BC78
		protected internal ProviderInfo ProviderInfo
		{
			get
			{
				ProviderInfo result;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					result = this.providerInformation;
				}
				return result;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x0010DAB0 File Offset: 0x0010BCB0
		protected PSDriveInfo PSDriveInfo
		{
			get
			{
				PSDriveInfo drive;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					drive = this.Context.Drive;
				}
				return drive;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003199 RID: 12697 RVA: 0x0010DAEC File Offset: 0x0010BCEC
		protected object DynamicParameters
		{
			get
			{
				object dynamicParameters;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					dynamicParameters = this.Context.DynamicParameters;
				}
				return dynamicParameters;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x0010DB28 File Offset: 0x0010BD28
		public SwitchParameter Force
		{
			get
			{
				SwitchParameter force;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					force = this.Context.Force;
				}
				return force;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x0600319B RID: 12699 RVA: 0x0010DB64 File Offset: 0x0010BD64
		public string Filter
		{
			get
			{
				string filter;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					filter = this.Context.Filter;
				}
				return filter;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x0010DBA0 File Offset: 0x0010BDA0
		public Collection<string> Include
		{
			get
			{
				Collection<string> include;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					include = this.Context.Include;
				}
				return include;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x0600319D RID: 12701 RVA: 0x0010DBDC File Offset: 0x0010BDDC
		public Collection<string> Exclude
		{
			get
			{
				Collection<string> exclude;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					exclude = this.Context.Exclude;
				}
				return exclude;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x0010DC18 File Offset: 0x0010BE18
		public PSHost Host
		{
			get
			{
				PSHost engineHostInterface;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					engineHostInterface = this.Context.ExecutionContext.EngineHostInterface;
				}
				return engineHostInterface;
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x0010DC5C File Offset: 0x0010BE5C
		public virtual string GetResourceString(string baseName, string resourceId)
		{
			string result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (string.IsNullOrEmpty(baseName))
				{
					throw PSTraceSource.NewArgumentException("baseName");
				}
				if (string.IsNullOrEmpty(resourceId))
				{
					throw PSTraceSource.NewArgumentException("resourceId");
				}
				ResourceManager resourceManager = ResourceManagerCache.GetResourceManager(base.GetType().GetTypeInfo().Assembly, baseName);
				string text = null;
				try
				{
					text = resourceManager.GetString(resourceId, CultureInfo.CurrentUICulture);
				}
				catch (MissingManifestResourceException)
				{
					throw PSTraceSource.NewArgumentException("baseName", GetErrorText.ResourceBaseNameFailure, new object[]
					{
						baseName
					});
				}
				if (text == null)
				{
					throw PSTraceSource.NewArgumentException("resourceId", GetErrorText.ResourceIdFailure, new object[]
					{
						resourceId
					});
				}
				result = text;
			}
			return result;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x0010DD2C File Offset: 0x0010BF2C
		public void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				if (errorRecord == null)
				{
					throw PSTraceSource.NewArgumentNullException("errorRecord");
				}
				if (errorRecord.ErrorDetails != null && errorRecord.ErrorDetails.TextLookupError != null)
				{
					Exception textLookupError = errorRecord.ErrorDetails.TextLookupError;
					errorRecord.ErrorDetails.TextLookupError = null;
					MshLog.LogProviderHealthEvent(this.Context.ExecutionContext, this.ProviderInfo.Name, textLookupError, Severity.Warning);
				}
				ProviderInvocationException ex = new ProviderInvocationException(this.ProviderInfo, errorRecord);
				MshLog.LogProviderHealthEvent(this.Context.ExecutionContext, this.ProviderInfo.Name, ex, Severity.Warning);
				throw ex;
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_93;
				}
				goto IL_93;
				IL_93:;
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x0010DDDC File Offset: 0x0010BFDC
		public bool ShouldProcess(string target)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.Context.ShouldProcess(target);
			}
			return result;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x0010DE1C File Offset: 0x0010C01C
		public bool ShouldProcess(string target, string action)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.Context.ShouldProcess(target, action);
			}
			return result;
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x0010DE5C File Offset: 0x0010C05C
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.Context.ShouldProcess(verboseDescription, verboseWarning, caption);
			}
			return result;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x0010DE9C File Offset: 0x0010C09C
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.Context.ShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
			}
			return result;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x0010DEE0 File Offset: 0x0010C0E0
		public bool ShouldContinue(string query, string caption)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.Context.ShouldContinue(query, caption);
			}
			return result;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x0010DF20 File Offset: 0x0010C120
		public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = this.Context.ShouldContinue(query, caption, ref yesToAll, ref noToAll);
			}
			return result;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x0010DF64 File Offset: 0x0010C164
		public bool TransactionAvailable()
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.Context == null)
				{
					result = false;
				}
				else
				{
					result = this.Context.TransactionAvailable();
				}
			}
			return result;
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x0010DFAC File Offset: 0x0010C1AC
		public PSTransactionContext CurrentPSTransaction
		{
			get
			{
				if (this.Context == null)
				{
					return null;
				}
				return this.Context.CurrentPSTransaction;
			}
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x0010DFC4 File Offset: 0x0010C1C4
		public void WriteVerbose(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.Context.WriteVerbose(text);
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x0010E000 File Offset: 0x0010C200
		public void WriteWarning(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.Context.WriteWarning(text);
			}
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x0010E03C File Offset: 0x0010C23C
		public void WriteProgress(ProgressRecord progressRecord)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (progressRecord == null)
				{
					throw PSTraceSource.NewArgumentNullException("progressRecord");
				}
				this.Context.WriteProgress(progressRecord);
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x0010E088 File Offset: 0x0010C288
		public void WriteDebug(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.Context.WriteDebug(text);
			}
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x0010E0C4 File Offset: 0x0010C2C4
		public void WriteInformation(InformationRecord record)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.Context.WriteInformation(record);
			}
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x0010E100 File Offset: 0x0010C300
		public void WriteInformation(object messageData, string[] tags)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.Context.WriteInformation(messageData, tags);
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x0010E13C File Offset: 0x0010C33C
		private void WriteObject(object item, string path, bool isContainer)
		{
			PSObject psobject = this.WrapOutputInPSObject(item, path);
			psobject.AddOrSetProperty("PSIsContainer", isContainer ? Boxed.True : Boxed.False);
			CmdletProvider.providerBaseTracer.WriteLine("Attaching {0} = {1}", new object[]
			{
				"PSIsContainer",
				isContainer
			});
			this.Context.WriteObject(psobject);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x0010E1A0 File Offset: 0x0010C3A0
		private void WriteObject(object item, string path)
		{
			PSObject obj = this.WrapOutputInPSObject(item, path);
			this.Context.WriteObject(obj);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x0010E1C4 File Offset: 0x0010C3C4
		private PSObject WrapOutputInPSObject(object item, string path)
		{
			if (item == null)
			{
				throw PSTraceSource.NewArgumentNullException("item");
			}
			PSObject psobject = new PSObject(item);
			PSObject psobject2 = item as PSObject;
			if (psobject2 != null)
			{
				psobject.InternalTypeNames = new ConsolidatedString(psobject2.InternalTypeNames);
			}
			string providerQualifiedPath = LocationGlobber.GetProviderQualifiedPath(path, this.ProviderInfo);
			psobject.AddOrSetProperty("PSPath", providerQualifiedPath);
			CmdletProvider.providerBaseTracer.WriteLine("Attaching {0} = {1}", new object[]
			{
				"PSPath",
				providerQualifiedPath
			});
			NavigationCmdletProvider navigationCmdletProvider = this as NavigationCmdletProvider;
			if (navigationCmdletProvider != null && path != null)
			{
				string parentPath;
				if (this.PSDriveInfo != null)
				{
					parentPath = navigationCmdletProvider.GetParentPath(path, this.PSDriveInfo.Root, this.Context);
				}
				else
				{
					parentPath = navigationCmdletProvider.GetParentPath(path, string.Empty, this.Context);
				}
				string text = string.Empty;
				if (!string.IsNullOrEmpty(parentPath))
				{
					text = LocationGlobber.GetProviderQualifiedPath(parentPath, this.ProviderInfo);
				}
				psobject.AddOrSetProperty("PSParentPath", text);
				CmdletProvider.providerBaseTracer.WriteLine("Attaching {0} = {1}", new object[]
				{
					"PSParentPath",
					text
				});
				string childName = navigationCmdletProvider.GetChildName(path, this.Context);
				psobject.AddOrSetProperty("PSChildName", childName);
				CmdletProvider.providerBaseTracer.WriteLine("Attaching {0} = {1}", new object[]
				{
					"PSChildName",
					childName
				});
			}
			if (this.PSDriveInfo != null)
			{
				psobject.AddOrSetProperty(this.PSDriveInfo.GetNotePropertyForProviderCmdlets("PSDrive"));
				CmdletProvider.providerBaseTracer.WriteLine("Attaching {0} = {1}", new object[]
				{
					"PSDrive",
					this.PSDriveInfo
				});
			}
			psobject.AddOrSetProperty(this.ProviderInfo.GetNotePropertyForProviderCmdlets("PSProvider"));
			CmdletProvider.providerBaseTracer.WriteLine("Attaching {0} = {1}", new object[]
			{
				"PSProvider",
				this.ProviderInfo
			});
			return psobject;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x0010E3C0 File Offset: 0x0010C5C0
		public void WriteItemObject(object item, string path, bool isContainer)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.WriteObject(item, path, isContainer);
			}
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x0010E3F8 File Offset: 0x0010C5F8
		public void WritePropertyObject(object propertyValue, string path)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.WriteObject(propertyValue, path);
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x0010E430 File Offset: 0x0010C630
		public void WriteSecurityDescriptorObject(ObjectSecurity securityDescriptor, string path)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				this.WriteObject(securityDescriptor, path);
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x0010E468 File Offset: 0x0010C668
		public void WriteError(ErrorRecord errorRecord)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (errorRecord == null)
				{
					throw PSTraceSource.NewArgumentNullException("errorRecord");
				}
				if (errorRecord.ErrorDetails != null && errorRecord.ErrorDetails.TextLookupError != null)
				{
					MshLog.LogProviderHealthEvent(this.Context.ExecutionContext, this.ProviderInfo.Name, errorRecord.ErrorDetails.TextLookupError, Severity.Warning);
				}
				this.Context.WriteError(errorRecord);
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x0010E4F0 File Offset: 0x0010C6F0
		internal void GetSecurityDescriptor(string path, AccessControlSections sections, CmdletProviderContext context)
		{
			this.Context = context;
			ISecurityDescriptorCmdletProvider securityDescriptorCmdletProvider = this as ISecurityDescriptorCmdletProvider;
			CmdletProvider.CheckIfSecurityDescriptorInterfaceIsSupported(securityDescriptorCmdletProvider);
			securityDescriptorCmdletProvider.GetSecurityDescriptor(path, sections);
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x0010E51C File Offset: 0x0010C71C
		internal void SetSecurityDescriptor(string path, ObjectSecurity securityDescriptor, CmdletProviderContext context)
		{
			this.Context = context;
			ISecurityDescriptorCmdletProvider securityDescriptorCmdletProvider = this as ISecurityDescriptorCmdletProvider;
			CmdletProvider.CheckIfSecurityDescriptorInterfaceIsSupported(securityDescriptorCmdletProvider);
			securityDescriptorCmdletProvider.SetSecurityDescriptor(path, securityDescriptor);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x0010E545 File Offset: 0x0010C745
		private static void CheckIfSecurityDescriptorInterfaceIsSupported(ISecurityDescriptorCmdletProvider permissionProvider)
		{
			if (permissionProvider == null)
			{
				throw PSTraceSource.NewNotSupportedException(ProviderBaseSecurity.ISecurityDescriptorCmdletProvider_NotSupported, new object[0]);
			}
		}

		// Token: 0x04001A5A RID: 6746
		private CmdletProviderContext contextBase;

		// Token: 0x04001A5B RID: 6747
		private ProviderInfo providerInformation;

		// Token: 0x04001A5C RID: 6748
		[TraceSource("CmdletProviderClasses", "The namespace provider base classes tracer")]
		internal static PSTraceSource providerBaseTracer = PSTraceSource.GetTracer("CmdletProviderClasses", "The namespace provider base classes tracer");
	}
}
