using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace System.Management.Automation
{
	// Token: 0x020008A2 RID: 2210
	internal static class ResourceManagerCache
	{
		// Token: 0x06005487 RID: 21639 RVA: 0x001BECAC File Offset: 0x001BCEAC
		internal static ResourceManager GetResourceManager(Assembly assembly, string baseName)
		{
			if (assembly == null)
			{
				throw PSTraceSource.NewArgumentNullException("assembly");
			}
			if (string.IsNullOrEmpty(baseName))
			{
				throw PSTraceSource.NewArgumentException("baseName");
			}
			ResourceManager resourceManager = null;
			Dictionary<string, ResourceManager> dictionary = null;
			string assemblyLocation = ClrFacade.GetAssemblyLocation(assembly);
			lock (ResourceManagerCache.syncRoot)
			{
				if (ResourceManagerCache.resourceManagerCache.ContainsKey(assemblyLocation))
				{
					dictionary = ResourceManagerCache.resourceManagerCache[assemblyLocation];
					if (dictionary != null && dictionary.ContainsKey(baseName))
					{
						resourceManager = dictionary[baseName];
					}
				}
			}
			if (resourceManager == null)
			{
				resourceManager = ResourceManagerCache.InitRMWithAssembly(baseName, assembly);
				if (dictionary != null)
				{
					lock (ResourceManagerCache.syncRoot)
					{
						dictionary[baseName] = resourceManager;
						return resourceManager;
					}
				}
				Dictionary<string, ResourceManager> dictionary2 = new Dictionary<string, ResourceManager>();
				dictionary2[baseName] = resourceManager;
				lock (ResourceManagerCache.syncRoot)
				{
					ResourceManagerCache.resourceManagerCache[assemblyLocation] = dictionary2;
				}
			}
			return resourceManager;
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x06005488 RID: 21640 RVA: 0x001BEDD4 File Offset: 0x001BCFD4
		// (set) Token: 0x06005489 RID: 21641 RVA: 0x001BEDDB File Offset: 0x001BCFDB
		internal static bool DFT_DoMonitorFailingResourceLookup
		{
			get
			{
				return ResourceManagerCache.DFT_monitorFailingResourceLookup;
			}
			set
			{
				ResourceManagerCache.DFT_monitorFailingResourceLookup = value;
			}
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x001BEDE4 File Offset: 0x001BCFE4
		internal static string GetResourceString(Assembly assembly, string baseName, string resourceId)
		{
			if (assembly == null)
			{
				throw PSTraceSource.NewArgumentNullException("assembly");
			}
			if (string.IsNullOrEmpty(baseName))
			{
				throw PSTraceSource.NewArgumentException("baseName");
			}
			if (string.IsNullOrEmpty(resourceId))
			{
				throw PSTraceSource.NewArgumentException("resourceId");
			}
			ResourceManager resourceManager = ResourceManagerCache.GetResourceManager(assembly, baseName);
			string @string = resourceManager.GetString(resourceId);
			if (string.IsNullOrEmpty(@string))
			{
				bool dft_monitorFailingResourceLookup = ResourceManagerCache.DFT_monitorFailingResourceLookup;
			}
			return @string;
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x001BEE4C File Offset: 0x001BD04C
		private static ResourceManager InitRMWithAssembly(string baseName, Assembly assemblyToUse)
		{
			if (baseName != null && assemblyToUse != null)
			{
				return new ResourceManager(baseName, assemblyToUse);
			}
			throw PSTraceSource.NewArgumentException("assemblyToUse");
		}

		// Token: 0x04002B68 RID: 11112
		private static Dictionary<string, Dictionary<string, ResourceManager>> resourceManagerCache = new Dictionary<string, Dictionary<string, ResourceManager>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002B69 RID: 11113
		private static object syncRoot = new object();

		// Token: 0x04002B6A RID: 11114
		private static bool DFT_monitorFailingResourceLookup = true;
	}
}
