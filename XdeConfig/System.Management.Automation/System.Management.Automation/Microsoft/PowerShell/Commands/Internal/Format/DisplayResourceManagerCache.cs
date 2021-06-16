using System;
using System.Collections;
using System.Management.Automation;
using System.Reflection;
using System.Resources;
using System.Security;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000961 RID: 2401
	internal sealed class DisplayResourceManagerCache
	{
		// Token: 0x0600582F RID: 22575 RVA: 0x001CADF0 File Offset: 0x001C8FF0
		internal string GetTextTokenString(TextToken tt)
		{
			if (tt.resource != null)
			{
				string @string = this.GetString(tt.resource);
				if (@string != null)
				{
					return @string;
				}
			}
			return tt.text;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x001CAE1D File Offset: 0x001C901D
		internal void VerifyResource(StringResourceReference resourceReference, out DisplayResourceManagerCache.LoadingResult result, out DisplayResourceManagerCache.AssemblyBindingStatus bindingStatus)
		{
			this.GetStringHelper(resourceReference, out result, out bindingStatus);
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x001CAE2C File Offset: 0x001C902C
		private string GetString(StringResourceReference resourceReference)
		{
			DisplayResourceManagerCache.LoadingResult loadingResult;
			DisplayResourceManagerCache.AssemblyBindingStatus assemblyBindingStatus;
			return this.GetStringHelper(resourceReference, out loadingResult, out assemblyBindingStatus);
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x001CAE44 File Offset: 0x001C9044
		private string GetStringHelper(StringResourceReference resourceReference, out DisplayResourceManagerCache.LoadingResult result, out DisplayResourceManagerCache.AssemblyBindingStatus bindingStatus)
		{
			result = DisplayResourceManagerCache.LoadingResult.AssemblyNotFound;
			bindingStatus = DisplayResourceManagerCache.AssemblyBindingStatus.NotFound;
			DisplayResourceManagerCache.AssemblyLoadResult assemblyLoadResult;
			if (this._resourceReferenceToAssemblyCache.Contains(resourceReference))
			{
				assemblyLoadResult = (this._resourceReferenceToAssemblyCache[resourceReference] as DisplayResourceManagerCache.AssemblyLoadResult);
				bindingStatus = assemblyLoadResult.status;
			}
			else
			{
				assemblyLoadResult = new DisplayResourceManagerCache.AssemblyLoadResult();
				bool flag;
				assemblyLoadResult.a = this.LoadAssemblyFromResourceReference(resourceReference, out flag);
				if (assemblyLoadResult.a == null)
				{
					assemblyLoadResult.status = DisplayResourceManagerCache.AssemblyBindingStatus.NotFound;
				}
				else
				{
					assemblyLoadResult.status = (flag ? DisplayResourceManagerCache.AssemblyBindingStatus.FoundInGac : DisplayResourceManagerCache.AssemblyBindingStatus.FoundInPath);
				}
				this._resourceReferenceToAssemblyCache.Add(resourceReference, assemblyLoadResult);
			}
			bindingStatus = assemblyLoadResult.status;
			if (assemblyLoadResult.a == null)
			{
				result = DisplayResourceManagerCache.LoadingResult.AssemblyNotFound;
				return null;
			}
			try
			{
				string resourceString = ResourceManagerCache.GetResourceString(assemblyLoadResult.a, resourceReference.baseName, resourceReference.resourceId);
				if (resourceString == null)
				{
					result = DisplayResourceManagerCache.LoadingResult.StringNotFound;
					return null;
				}
				result = DisplayResourceManagerCache.LoadingResult.NoError;
				return resourceString;
			}
			catch (InvalidOperationException)
			{
				result = DisplayResourceManagerCache.LoadingResult.ResourceNotFound;
			}
			catch (MissingManifestResourceException)
			{
				result = DisplayResourceManagerCache.LoadingResult.ResourceNotFound;
			}
			catch (Exception)
			{
				throw;
			}
			return null;
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x001CAF48 File Offset: 0x001C9148
		private Assembly LoadAssemblyFromResourceReference(StringResourceReference resourceReference, out bool foundInGac)
		{
			foundInGac = false;
			return this._assemblyNameResolver.ResolveAssemblyName(resourceReference.assemblyName);
		}

		// Token: 0x04002F48 RID: 12104
		private DisplayResourceManagerCache.AssemblyNameResolver _assemblyNameResolver = new DisplayResourceManagerCache.AssemblyNameResolver();

		// Token: 0x04002F49 RID: 12105
		private Hashtable _resourceReferenceToAssemblyCache = new Hashtable();

		// Token: 0x02000962 RID: 2402
		internal enum LoadingResult
		{
			// Token: 0x04002F4B RID: 12107
			NoError,
			// Token: 0x04002F4C RID: 12108
			AssemblyNotFound,
			// Token: 0x04002F4D RID: 12109
			ResourceNotFound,
			// Token: 0x04002F4E RID: 12110
			StringNotFound
		}

		// Token: 0x02000963 RID: 2403
		internal enum AssemblyBindingStatus
		{
			// Token: 0x04002F50 RID: 12112
			NotFound,
			// Token: 0x04002F51 RID: 12113
			FoundInGac,
			// Token: 0x04002F52 RID: 12114
			FoundInPath
		}

		// Token: 0x02000964 RID: 2404
		private sealed class AssemblyLoadResult
		{
			// Token: 0x04002F53 RID: 12115
			internal Assembly a;

			// Token: 0x04002F54 RID: 12116
			internal DisplayResourceManagerCache.AssemblyBindingStatus status;
		}

		// Token: 0x02000965 RID: 2405
		private class AssemblyNameResolver
		{
			// Token: 0x06005836 RID: 22582 RVA: 0x001CAF84 File Offset: 0x001C9184
			internal Assembly ResolveAssemblyName(string assemblyName)
			{
				if (string.IsNullOrEmpty(assemblyName))
				{
					return null;
				}
				if (this._assemblyReferences.Contains(assemblyName))
				{
					return (Assembly)this._assemblyReferences[assemblyName];
				}
				Assembly assembly = this.ResolveAssemblyNameInLoadedAssemblies(assemblyName, true);
				if (assembly == null)
				{
					assembly = this.ResolveAssemblyNameInLoadedAssemblies(assemblyName, false);
				}
				this._assemblyReferences.Add(assemblyName, assembly);
				return assembly;
			}

			// Token: 0x06005837 RID: 22583 RVA: 0x001CAFE4 File Offset: 0x001C91E4
			private Assembly ResolveAssemblyNameInLoadedAssemblies(string assemblyName, bool fullName)
			{
				Assembly result = null;
				foreach (Assembly assembly in ClrFacade.GetAssemblies(null))
				{
					AssemblyName assemblyName2 = null;
					try
					{
						assemblyName2 = assembly.GetName();
					}
					catch (SecurityException)
					{
						continue;
					}
					string a = fullName ? assemblyName2.FullName : assemblyName2.Name;
					if (string.Equals(a, assemblyName, StringComparison.Ordinal))
					{
						return assembly;
					}
				}
				return result;
			}

			// Token: 0x04002F55 RID: 12117
			private Hashtable _assemblyReferences = new Hashtable(StringComparer.OrdinalIgnoreCase);
		}
	}
}
