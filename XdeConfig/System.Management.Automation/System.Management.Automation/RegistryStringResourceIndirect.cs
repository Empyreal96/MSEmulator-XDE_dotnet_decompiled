using System;
using System.IO;
using System.Reflection;
using System.Security;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x02000852 RID: 2130
	internal sealed class RegistryStringResourceIndirect : IDisposable
	{
		// Token: 0x0600520B RID: 21003 RVA: 0x001B602D File Offset: 0x001B422D
		internal static RegistryStringResourceIndirect GetResourceIndirectReader()
		{
			return new RegistryStringResourceIndirect();
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x001B6034 File Offset: 0x001B4234
		public void Dispose()
		{
			if (!this._disposed && this._domain != null)
			{
				AppDomain.Unload(this._domain);
				this._domain = null;
				this._resourceRetriever = null;
			}
			this._disposed = true;
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x001B6066 File Offset: 0x001B4266
		private void CreateAppDomain()
		{
			if (this._domain == null)
			{
				this._domain = AppDomain.CreateDomain("ResourceIndirectDomain");
				this._resourceRetriever = (ResourceRetriever)this._domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "System.Management.Automation.ResourceRetriever");
			}
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x001B60A8 File Offset: 0x001B42A8
		internal string GetResourceStringIndirect(RegistryKey key, string valueName, string assemblyName, string modulePath)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewInvalidOperationException(MshSnapinInfo.ResourceReaderDisposed, new object[0]);
			}
			if (key == null)
			{
				throw PSTraceSource.NewArgumentNullException("key");
			}
			if (string.IsNullOrEmpty(valueName))
			{
				throw PSTraceSource.NewArgumentException("valueName");
			}
			if (string.IsNullOrEmpty(assemblyName))
			{
				throw PSTraceSource.NewArgumentException("assemblyName");
			}
			if (string.IsNullOrEmpty(modulePath))
			{
				throw PSTraceSource.NewArgumentException("modulePath");
			}
			string result = null;
			string regKeyValueAsString = RegistryStringResourceIndirect.GetRegKeyValueAsString(key, valueName);
			if (regKeyValueAsString != null)
			{
				result = this.GetResourceStringIndirect(assemblyName, modulePath, regKeyValueAsString);
			}
			return result;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x001B6130 File Offset: 0x001B4330
		internal string GetResourceStringIndirect(string assemblyName, string modulePath, string baseNameRIDPair)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewInvalidOperationException(MshSnapinInfo.ResourceReaderDisposed, new object[0]);
			}
			if (string.IsNullOrEmpty(assemblyName))
			{
				throw PSTraceSource.NewArgumentException("assemblyName");
			}
			if (string.IsNullOrEmpty(modulePath))
			{
				throw PSTraceSource.NewArgumentException("modulePath");
			}
			if (string.IsNullOrEmpty(baseNameRIDPair))
			{
				throw PSTraceSource.NewArgumentException("baseNameRIDPair");
			}
			string result = null;
			if (this._resourceRetriever == null)
			{
				this.CreateAppDomain();
			}
			if (this._resourceRetriever != null)
			{
				string[] array = baseNameRIDPair.Split(new char[]
				{
					','
				});
				if (array.Length == 2)
				{
					string baseName = array[0];
					string resourceID = array[1];
					result = this._resourceRetriever.GetStringResource(assemblyName, modulePath, baseName, resourceID);
				}
			}
			return result;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001B61E0 File Offset: 0x001B43E0
		private static string GetRegKeyValueAsString(RegistryKey key, string valueName)
		{
			string result = null;
			try
			{
				RegistryValueKind valueKind = key.GetValueKind(valueName);
				if (valueKind == RegistryValueKind.String)
				{
					result = (key.GetValue(valueName) as string);
				}
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (SecurityException)
			{
			}
			return result;
		}

		// Token: 0x04002A2D RID: 10797
		private bool _disposed;

		// Token: 0x04002A2E RID: 10798
		private AppDomain _domain;

		// Token: 0x04002A2F RID: 10799
		private ResourceRetriever _resourceRetriever;
	}
}
