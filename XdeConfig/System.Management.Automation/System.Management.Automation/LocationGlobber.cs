using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200046D RID: 1133
	internal sealed class LocationGlobber
	{
		// Token: 0x06003254 RID: 12884 RVA: 0x0010FA8C File Offset: 0x0010DC8C
		internal LocationGlobber(SessionState sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0010FAAC File Offset: 0x0010DCAC
		internal Collection<PathInfo> GetGlobbedMonadPathsFromMonadPath(string path, bool allowNonexistingPaths, out CmdletProvider providerInstance)
		{
			CmdletProviderContext context = new CmdletProviderContext(this.sessionState.Internal.ExecutionContext);
			return this.GetGlobbedMonadPathsFromMonadPath(path, allowNonexistingPaths, context, out providerInstance);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x0010FADC File Offset: 0x0010DCDC
		internal Collection<PathInfo> GetGlobbedMonadPathsFromMonadPath(string path, bool allowNonexistingPaths, CmdletProviderContext context, out CmdletProvider providerInstance)
		{
			providerInstance = null;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			Collection<PathInfo> collection = new Collection<PathInfo>();
			using (LocationGlobber.pathResolutionTracer.TraceScope("Resolving MSH path \"{0}\" to MSH path", new object[]
			{
				path
			}))
			{
				LocationGlobber.TraceFilters(context);
				if (LocationGlobber.IsHomePath(path))
				{
					using (LocationGlobber.pathResolutionTracer.TraceScope("Resolving HOME relative path.", new object[0]))
					{
						path = this.GetHomeRelativePath(path);
					}
				}
				bool flag = LocationGlobber.IsProviderDirectPath(path);
				bool flag2 = LocationGlobber.IsProviderQualifiedPath(path);
				if (flag || flag2)
				{
					collection = this.ResolvePSPathFromProviderPath(path, context, allowNonexistingPaths, flag, flag2, out providerInstance);
				}
				else
				{
					collection = this.ResolveDriveQualifiedPath(path, context, allowNonexistingPaths, out providerInstance);
				}
				if (!allowNonexistingPaths && collection.Count < 1 && !WildcardPattern.ContainsWildcardCharacters(path) && (context.Include == null || context.Include.Count == 0) && (context.Exclude == null || context.Exclude.Count == 0))
				{
					ItemNotFoundException ex = new ItemNotFoundException(path, "PathNotFound", SessionStateStrings.PathNotFound);
					LocationGlobber.pathResolutionTracer.TraceError("Item does not exist: {0}", new object[]
					{
						path
					});
					throw ex;
				}
			}
			return collection;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x0010FC38 File Offset: 0x0010DE38
		private Collection<string> ResolveProviderPathFromProviderPath(string providerPath, string providerId, bool allowNonexistingPaths, CmdletProviderContext context, out CmdletProvider providerInstance)
		{
			providerInstance = this.sessionState.Internal.GetProviderInstance(providerId);
			ContainerCmdletProvider containerCmdletProvider = providerInstance as ContainerCmdletProvider;
			ItemCmdletProvider itemCmdletProvider = providerInstance as ItemCmdletProvider;
			Collection<string> collection = new Collection<string>();
			if (!context.SuppressWildcardExpansion)
			{
				if (CmdletProviderManagementIntrinsics.CheckProviderCapabilities(ProviderCapabilities.ExpandWildcards, providerInstance.ProviderInfo))
				{
					LocationGlobber.pathResolutionTracer.WriteLine("Wildcard matching is being performed by the provider.", new object[0]);
					if (itemCmdletProvider != null && WildcardPattern.ContainsWildcardCharacters(providerPath))
					{
						collection = new Collection<string>(itemCmdletProvider.ExpandPath(providerPath, context));
					}
					else
					{
						collection.Add(providerPath);
					}
				}
				else
				{
					LocationGlobber.pathResolutionTracer.WriteLine("Wildcard matching is being performed by the engine.", new object[0]);
					if (containerCmdletProvider != null)
					{
						collection = this.GetGlobbedProviderPathsFromProviderPath(providerPath, allowNonexistingPaths, containerCmdletProvider, context);
					}
					else
					{
						collection.Add(providerPath);
					}
				}
			}
			else if (itemCmdletProvider != null)
			{
				if (allowNonexistingPaths || itemCmdletProvider.ItemExists(providerPath, context))
				{
					collection.Add(providerPath);
				}
			}
			else
			{
				collection.Add(providerPath);
			}
			if (!allowNonexistingPaths && collection.Count < 1 && !WildcardPattern.ContainsWildcardCharacters(providerPath) && (context.Include == null || context.Include.Count == 0) && (context.Exclude == null || context.Exclude.Count == 0))
			{
				ItemNotFoundException ex = new ItemNotFoundException(providerPath, "PathNotFound", SessionStateStrings.PathNotFound);
				LocationGlobber.pathResolutionTracer.TraceError("Item does not exist: {0}", new object[]
				{
					providerPath
				});
				throw ex;
			}
			return collection;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x0010FD88 File Offset: 0x0010DF88
		private Collection<PathInfo> ResolvePSPathFromProviderPath(string path, CmdletProviderContext context, bool allowNonexistingPaths, bool isProviderDirectPath, bool isProviderQualifiedPath, out CmdletProvider providerInstance)
		{
			Collection<PathInfo> collection = new Collection<PathInfo>();
			providerInstance = null;
			string text = null;
			string text2 = null;
			if (isProviderDirectPath)
			{
				LocationGlobber.pathResolutionTracer.WriteLine("Path is PROVIDER-DIRECT", new object[0]);
				text2 = path;
				text = this.sessionState.Path.CurrentLocation.Provider.Name;
			}
			else if (isProviderQualifiedPath)
			{
				LocationGlobber.pathResolutionTracer.WriteLine("Path is PROVIDER-QUALIFIED", new object[0]);
				text2 = LocationGlobber.ParseProviderPath(path, out text);
			}
			LocationGlobber.pathResolutionTracer.WriteLine("PROVIDER-INTERNAL path: {0}", new object[]
			{
				text2
			});
			LocationGlobber.pathResolutionTracer.WriteLine("Provider: {0}", new object[]
			{
				text
			});
			Collection<string> collection2 = this.ResolveProviderPathFromProviderPath(text2, text, allowNonexistingPaths, context, out providerInstance);
			PSDriveInfo hiddenDrive = providerInstance.ProviderInfo.HiddenDrive;
			foreach (string text3 in collection2)
			{
				string text4 = text3;
				if (context.Stopping)
				{
					throw new PipelineStoppedException();
				}
				string text5;
				if (LocationGlobber.IsProviderDirectPath(text4))
				{
					text5 = text4;
				}
				else
				{
					text5 = string.Format(CultureInfo.InvariantCulture, "{0}::{1}", new object[]
					{
						text,
						text4
					});
				}
				collection.Add(new PathInfo(hiddenDrive, providerInstance.ProviderInfo, text5, this.sessionState));
				LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
				{
					text5
				});
			}
			return collection;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x0010FF20 File Offset: 0x0010E120
		private Collection<PathInfo> ResolveDriveQualifiedPath(string path, CmdletProviderContext context, bool allowNonexistingPaths, out CmdletProvider providerInstance)
		{
			providerInstance = null;
			PSDriveInfo psdriveInfo = null;
			Collection<PathInfo> collection = new Collection<PathInfo>();
			LocationGlobber.pathResolutionTracer.WriteLine("Path is DRIVE-QUALIFIED", new object[0]);
			string driveRootRelativePathFromPSPath = this.GetDriveRootRelativePathFromPSPath(path, context, true, out psdriveInfo, out providerInstance);
			LocationGlobber.pathResolutionTracer.WriteLine("DRIVE-RELATIVE path: {0}", new object[]
			{
				driveRootRelativePathFromPSPath
			});
			LocationGlobber.pathResolutionTracer.WriteLine("Drive: {0}", new object[]
			{
				psdriveInfo.Name
			});
			LocationGlobber.pathResolutionTracer.WriteLine("Provider: {0}", new object[]
			{
				psdriveInfo.Provider
			});
			context.Drive = psdriveInfo;
			providerInstance = this.sessionState.Internal.GetContainerProviderInstance(psdriveInfo.Provider);
			ContainerCmdletProvider provider = providerInstance as ContainerCmdletProvider;
			ItemCmdletProvider itemCmdletProvider = providerInstance as ItemCmdletProvider;
			ProviderInfo providerInfo = providerInstance.ProviderInfo;
			string text;
			string text2;
			if (psdriveInfo.Hidden)
			{
				text = LocationGlobber.GetProviderQualifiedPath(driveRootRelativePathFromPSPath, providerInfo);
				text2 = driveRootRelativePathFromPSPath;
			}
			else
			{
				text = LocationGlobber.GetDriveQualifiedPath(driveRootRelativePathFromPSPath, psdriveInfo);
				text2 = this.GetProviderPath(path, context);
			}
			LocationGlobber.pathResolutionTracer.WriteLine("PROVIDER path: {0}", new object[]
			{
				text2
			});
			Collection<string> collection2 = new Collection<string>();
			if (!context.SuppressWildcardExpansion)
			{
				if (CmdletProviderManagementIntrinsics.CheckProviderCapabilities(ProviderCapabilities.ExpandWildcards, providerInfo))
				{
					LocationGlobber.pathResolutionTracer.WriteLine("Wildcard matching is being performed by the provider.", new object[0]);
					if (itemCmdletProvider != null && WildcardPattern.ContainsWildcardCharacters(driveRootRelativePathFromPSPath))
					{
						foreach (string providerPath in itemCmdletProvider.ExpandPath(text2, context))
						{
							collection2.Add(this.GetDriveRootRelativePathFromProviderPath(providerPath, psdriveInfo, context));
						}
					}
					else
					{
						collection2.Add(this.GetDriveRootRelativePathFromProviderPath(text2, psdriveInfo, context));
					}
				}
				else
				{
					LocationGlobber.pathResolutionTracer.WriteLine("Wildcard matching is being performed by the engine.", new object[0]);
					collection2 = this.ExpandMshGlobPath(driveRootRelativePathFromPSPath, allowNonexistingPaths, psdriveInfo, provider, context);
				}
			}
			else if (itemCmdletProvider != null)
			{
				if (allowNonexistingPaths || itemCmdletProvider.ItemExists(text2, context))
				{
					collection2.Add(text);
				}
			}
			else
			{
				collection2.Add(text);
			}
			if (!allowNonexistingPaths && collection2.Count < 1 && !WildcardPattern.ContainsWildcardCharacters(path) && (context.Include == null || context.Include.Count == 0) && (context.Exclude == null || context.Exclude.Count == 0))
			{
				ItemNotFoundException ex = new ItemNotFoundException(path, "PathNotFound", SessionStateStrings.PathNotFound);
				LocationGlobber.pathResolutionTracer.TraceError("Item does not exist: {0}", new object[]
				{
					path
				});
				throw ex;
			}
			foreach (string text3 in collection2)
			{
				if (context.Stopping)
				{
					throw new PipelineStoppedException();
				}
				if (psdriveInfo.Hidden)
				{
					if (LocationGlobber.IsProviderDirectPath(text3))
					{
						text = text3;
					}
					else
					{
						text = LocationGlobber.GetProviderQualifiedPath(text3, providerInfo);
					}
				}
				else
				{
					text = LocationGlobber.GetDriveQualifiedPath(text3, psdriveInfo);
				}
				collection.Add(new PathInfo(psdriveInfo, providerInfo, text, this.sessionState));
				LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
				{
					text
				});
			}
			return collection;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x00110248 File Offset: 0x0010E448
		internal Collection<string> GetGlobbedProviderPathsFromMonadPath(string path, bool allowNonexistingPaths, out ProviderInfo provider, out CmdletProvider providerInstance)
		{
			providerInstance = null;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext context = new CmdletProviderContext(this.sessionState.Internal.ExecutionContext);
			return this.GetGlobbedProviderPathsFromMonadPath(path, allowNonexistingPaths, context, out provider, out providerInstance);
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0011028C File Offset: 0x0010E48C
		internal Collection<string> GetGlobbedProviderPathsFromMonadPath(string path, bool allowNonexistingPaths, CmdletProviderContext context, out ProviderInfo provider, out CmdletProvider providerInstance)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			Collection<string> result;
			using (LocationGlobber.pathResolutionTracer.TraceScope("Resolving MSH path \"{0}\" to PROVIDER-INTERNAL path", new object[]
			{
				path
			}))
			{
				LocationGlobber.TraceFilters(context);
				if (LocationGlobber.IsProviderQualifiedPath(path))
				{
					context.Drive = null;
				}
				PSDriveInfo psdriveInfo = null;
				if (this.GetProviderPath(path, context, out provider, out psdriveInfo) == null)
				{
					providerInstance = null;
					LocationGlobber.tracer.WriteLine("provider returned a null path so return an empty array", new object[0]);
					LocationGlobber.pathResolutionTracer.WriteLine("Provider '{0}' returned null", new object[]
					{
						provider
					});
					result = new Collection<string>();
				}
				else
				{
					if (psdriveInfo != null)
					{
						context.Drive = psdriveInfo;
					}
					Collection<string> collection = new Collection<string>();
					foreach (PathInfo pathInfo in this.GetGlobbedMonadPathsFromMonadPath(path, allowNonexistingPaths, context, out providerInstance))
					{
						collection.Add(pathInfo.ProviderPath);
					}
					result = collection;
				}
			}
			return result;
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x001103C4 File Offset: 0x0010E5C4
		internal Collection<string> GetGlobbedProviderPathsFromProviderPath(string path, bool allowNonexistingPaths, string providerId, out CmdletProvider providerInstance)
		{
			providerInstance = null;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.sessionState.Internal.ExecutionContext);
			Collection<string> globbedProviderPathsFromProviderPath = this.GetGlobbedProviderPathsFromProviderPath(path, allowNonexistingPaths, providerId, cmdletProviderContext, out providerInstance);
			if (cmdletProviderContext.HasErrors())
			{
				ErrorRecord errorRecord = cmdletProviderContext.GetAccumulatedErrorObjects()[0];
				if (errorRecord != null)
				{
					throw errorRecord.Exception;
				}
			}
			return globbedProviderPathsFromProviderPath;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x00110428 File Offset: 0x0010E628
		internal Collection<string> GetGlobbedProviderPathsFromProviderPath(string path, bool allowNonexistingPaths, string providerId, CmdletProviderContext context, out CmdletProvider providerInstance)
		{
			providerInstance = null;
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (providerId == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerId");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			Collection<string> result;
			using (LocationGlobber.pathResolutionTracer.TraceScope("Resolving PROVIDER-INTERNAL path \"{0}\" to PROVIDER-INTERNAL path", new object[]
			{
				path
			}))
			{
				LocationGlobber.TraceFilters(context);
				result = this.ResolveProviderPathFromProviderPath(path, providerId, allowNonexistingPaths, context, out providerInstance);
			}
			return result;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x001104B4 File Offset: 0x0010E6B4
		internal string GetProviderPath(string path)
		{
			ProviderInfo providerInfo = null;
			return this.GetProviderPath(path, out providerInfo);
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x001104CC File Offset: 0x0010E6CC
		internal string GetProviderPath(string path, out ProviderInfo provider)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.sessionState.Internal.ExecutionContext);
			PSDriveInfo psdriveInfo = null;
			provider = null;
			string providerPath = this.GetProviderPath(path, cmdletProviderContext, out provider, out psdriveInfo);
			if (cmdletProviderContext.HasErrors())
			{
				Collection<ErrorRecord> accumulatedErrorObjects = cmdletProviderContext.GetAccumulatedErrorObjects();
				if (accumulatedErrorObjects != null && accumulatedErrorObjects.Count > 0)
				{
					throw accumulatedErrorObjects[0].Exception;
				}
			}
			return providerPath;
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x00110538 File Offset: 0x0010E738
		internal string GetProviderPath(string path, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			PSDriveInfo psdriveInfo = null;
			ProviderInfo providerInfo = null;
			return this.GetProviderPath(path, context, out providerInfo, out psdriveInfo);
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x00110568 File Offset: 0x0010E768
		internal string GetProviderPath(string path, CmdletProviderContext context, out ProviderInfo provider, out PSDriveInfo drive)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			string text = null;
			provider = null;
			drive = null;
			if (LocationGlobber.IsHomePath(path))
			{
				using (LocationGlobber.pathResolutionTracer.TraceScope("Resolving HOME relative path.", new object[0]))
				{
					path = this.GetHomeRelativePath(path);
				}
			}
			if (LocationGlobber.IsProviderDirectPath(path))
			{
				LocationGlobber.pathResolutionTracer.WriteLine("Path is PROVIDER-DIRECT", new object[0]);
				text = path;
				drive = null;
				provider = this.sessionState.Path.CurrentLocation.Provider;
				LocationGlobber.pathResolutionTracer.WriteLine("PROVIDER-INTERNAL path: {0}", new object[]
				{
					text
				});
				LocationGlobber.pathResolutionTracer.WriteLine("Provider: {0}", new object[]
				{
					provider
				});
			}
			else if (LocationGlobber.IsProviderQualifiedPath(path))
			{
				LocationGlobber.pathResolutionTracer.WriteLine("Path is PROVIDER-QUALIFIED", new object[0]);
				string name = null;
				text = LocationGlobber.ParseProviderPath(path, out name);
				drive = null;
				provider = this.sessionState.Internal.GetSingleProvider(name);
				LocationGlobber.pathResolutionTracer.WriteLine("PROVIDER-INTERNAL path: {0}", new object[]
				{
					text
				});
				LocationGlobber.pathResolutionTracer.WriteLine("Provider: {0}", new object[]
				{
					provider
				});
			}
			else
			{
				LocationGlobber.pathResolutionTracer.WriteLine("Path is DRIVE-QUALIFIED", new object[0]);
				CmdletProvider cmdletProvider = null;
				string driveRootRelativePathFromPSPath = this.GetDriveRootRelativePathFromPSPath(path, context, false, out drive, out cmdletProvider);
				LocationGlobber.pathResolutionTracer.WriteLine("DRIVE-RELATIVE path: {0}", new object[]
				{
					driveRootRelativePathFromPSPath
				});
				LocationGlobber.pathResolutionTracer.WriteLine("Drive: {0}", new object[]
				{
					drive.Name
				});
				LocationGlobber.pathResolutionTracer.WriteLine("Provider: {0}", new object[]
				{
					drive.Provider
				});
				context.Drive = drive;
				if (drive.Hidden)
				{
					text = driveRootRelativePathFromPSPath;
				}
				else
				{
					text = this.GetProviderSpecificPath(drive, driveRootRelativePathFromPSPath, context);
				}
				provider = drive.Provider;
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
			{
				text
			});
			if (provider != null && context != null && context.MyInvocation != null && context.ExecutionContext != null && context.ExecutionContext.InitialSessionState != null)
			{
				foreach (SessionStateProviderEntry sessionStateProviderEntry in context.ExecutionContext.InitialSessionState.Providers[provider.Name])
				{
					if (sessionStateProviderEntry.Visibility == SessionStateEntryVisibility.Private && context.MyInvocation.CommandOrigin == CommandOrigin.Runspace)
					{
						LocationGlobber.pathResolutionTracer.WriteLine("Provider is private: {0}", new object[]
						{
							provider.Name
						});
						throw new ProviderNotFoundException(provider.Name, SessionStateCategory.CmdletProvider, "ProviderNotFound", SessionStateStrings.ProviderNotFound, new object[0]);
					}
				}
			}
			return text;
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x001108B4 File Offset: 0x0010EAB4
		internal static bool IsProviderQualifiedPath(string path)
		{
			string text = null;
			return LocationGlobber.IsProviderQualifiedPath(path, out text);
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x001108CC File Offset: 0x0010EACC
		internal static bool IsProviderQualifiedPath(string path, out string providerId)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			providerId = null;
			bool flag = false;
			if (path.Length == 0)
			{
				flag = false;
			}
			else if (path.StartsWith(".\\", StringComparison.Ordinal) || path.StartsWith("./", StringComparison.Ordinal))
			{
				flag = false;
			}
			else
			{
				int num = path.IndexOf(':');
				if (num == -1 || num + 1 >= path.Length || path[num + 1] != ':')
				{
					flag = false;
				}
				else if (num > 0)
				{
					flag = true;
					providerId = path.Substring(0, num);
					LocationGlobber.tracer.WriteLine("providerId = {0}", new object[]
					{
						providerId
					});
				}
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x00110990 File Offset: 0x0010EB90
		internal static bool IsAbsolutePath(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			bool flag = false;
			if (path.Length == 0)
			{
				flag = false;
			}
			else if (path.StartsWith(".\\", StringComparison.Ordinal))
			{
				flag = false;
			}
			else
			{
				int num = path.IndexOf(":", StringComparison.Ordinal);
				if (num == -1)
				{
					flag = false;
				}
				else if (num > 0)
				{
					flag = true;
				}
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x00110A08 File Offset: 0x0010EC08
		internal bool IsAbsolutePath(string path, out string driveName)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			bool flag = false;
			if (this.sessionState.Drive.Current != null)
			{
				driveName = this.sessionState.Drive.Current.Name;
			}
			else
			{
				driveName = null;
			}
			if (path.Length == 0)
			{
				flag = false;
			}
			else if (path.StartsWith(".\\", StringComparison.Ordinal) || path.StartsWith("./", StringComparison.Ordinal))
			{
				flag = false;
			}
			else
			{
				int num = path.IndexOf(":", StringComparison.CurrentCulture);
				if (num == -1)
				{
					flag = false;
				}
				else if (num > 0)
				{
					driveName = path.Substring(0, num);
					flag = true;
				}
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x00110ACC File Offset: 0x0010ECCC
		private static string RemoveGlobEscaping(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			return WildcardPattern.Unescape(path);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x00110AF0 File Offset: 0x0010ECF0
		internal bool IsShellVirtualDrive(string driveName, out SessionStateScope scope)
		{
			if (driveName == null)
			{
				throw PSTraceSource.NewArgumentNullException("driveName");
			}
			bool flag = false;
			if (string.Compare(driveName, "GLOBAL", StringComparison.OrdinalIgnoreCase) == 0)
			{
				LocationGlobber.tracer.WriteLine("match found: {0}", new object[]
				{
					"GLOBAL"
				});
				flag = true;
				scope = this.sessionState.Internal.GlobalScope;
			}
			else if (string.Compare(driveName, "LOCAL", StringComparison.OrdinalIgnoreCase) == 0)
			{
				LocationGlobber.tracer.WriteLine("match found: {0}", new object[]
				{
					driveName
				});
				flag = true;
				scope = this.sessionState.Internal.CurrentScope;
			}
			else
			{
				scope = null;
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x00110BB4 File Offset: 0x0010EDB4
		internal string GetDriveRootRelativePathFromPSPath(string path, CmdletProviderContext context, bool escapeCurrentLocation, out PSDriveInfo workingDriveForPath, out CmdletProvider providerInstance)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			workingDriveForPath = null;
			string text = null;
			if (this.sessionState.Drive.Current != null)
			{
				text = this.sessionState.Drive.Current.Name;
			}
			bool flag = false;
			if (this.IsAbsolutePath(path, out text))
			{
				LocationGlobber.tracer.WriteLine("Drive Name: {0}", new object[]
				{
					text
				});
				try
				{
					workingDriveForPath = this.sessionState.Drive.Get(text);
				}
				catch (DriveNotFoundException)
				{
					if (this.sessionState.Drive.Current == null)
					{
						throw;
					}
					string text2 = this.sessionState.Drive.Current.Root.Replace('/', '\\');
					if (text2.IndexOf(":", StringComparison.CurrentCulture) >= 0)
					{
						string text3 = path.Replace('/', '\\');
						if (text3.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							path = path.Substring(text2.Length);
							path = path.TrimStart(new char[]
							{
								'\\'
							});
							path = '\\' + path;
							workingDriveForPath = this.sessionState.Drive.Current;
						}
					}
					if (!flag)
					{
						throw;
					}
				}
				if (!flag)
				{
					path = path.Substring(text.Length + 1);
				}
			}
			else
			{
				workingDriveForPath = this.sessionState.Drive.Current;
			}
			if (workingDriveForPath == null)
			{
				ItemNotFoundException ex = new ItemNotFoundException(path, "PathNotFound", SessionStateStrings.PathNotFound);
				LocationGlobber.pathResolutionTracer.TraceError("Item does not exist: {0}", new object[]
				{
					path
				});
				throw ex;
			}
			string result;
			try
			{
				providerInstance = this.sessionState.Internal.GetContainerProviderInstance(workingDriveForPath.Provider);
				context.Drive = workingDriveForPath;
				string text4 = this.GenerateRelativePath(workingDriveForPath, path, escapeCurrentLocation, providerInstance, context);
				result = text4;
			}
			catch (PSNotSupportedException)
			{
				providerInstance = null;
				result = "";
			}
			return result;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x00110DC8 File Offset: 0x0010EFC8
		private string GetDriveRootRelativePathFromProviderPath(string providerPath, PSDriveInfo drive, CmdletProviderContext context)
		{
			string text = "";
			CmdletProvider containerProviderInstance = this.sessionState.Internal.GetContainerProviderInstance(drive.Provider);
			NavigationCmdletProvider navigationCmdletProvider = containerProviderInstance as NavigationCmdletProvider;
			providerPath = providerPath.Replace('/', '\\');
			providerPath = providerPath.TrimEnd(new char[]
			{
				'\\'
			});
			string text2 = drive.Root.Replace('/', '\\');
			text2 = text2.TrimEnd(new char[]
			{
				'\\'
			});
			while (!string.IsNullOrEmpty(providerPath) && !providerPath.Equals(text2, StringComparison.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text = this.sessionState.Internal.MakePath(containerProviderInstance, navigationCmdletProvider.GetChildName(providerPath, context), text, context);
				}
				else
				{
					text = navigationCmdletProvider.GetChildName(providerPath, context);
				}
				providerPath = this.sessionState.Internal.GetParentPath(containerProviderInstance, providerPath, drive.Root, context);
			}
			return text;
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x00110EA4 File Offset: 0x0010F0A4
		internal string GenerateRelativePath(PSDriveInfo drive, string path, bool escapeCurrentLocation, CmdletProvider providerInstance, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			string text = drive.CurrentLocation;
			if (!string.IsNullOrEmpty(text) && text.StartsWith(drive.Root, StringComparison.Ordinal))
			{
				text = text.Substring(drive.Root.Length);
			}
			if (escapeCurrentLocation)
			{
				text = WildcardPattern.Escape(text);
			}
			if (!string.IsNullOrEmpty(path))
			{
				if (path[0] == '\\' || path[0] == '/')
				{
					text = string.Empty;
					path = path.Substring(1);
					LocationGlobber.tracer.WriteLine("path = {0}", new object[]
					{
						path
					});
				}
				else
				{
					while (path.Length > 0 && this.HasRelativePathTokens(path))
					{
						if (context.Stopping)
						{
							throw new PipelineStoppedException();
						}
						bool flag = false;
						bool flag2 = path.StartsWith("..", StringComparison.Ordinal);
						bool flag3 = path.Length == 2;
						bool flag4 = path.Length > 2 && (path[2] == '\\' || path[2] == '/');
						if (flag2 && (flag3 || flag4))
						{
							if (!string.IsNullOrEmpty(text))
							{
								text = this.sessionState.Internal.GetParentPath(providerInstance, text, drive.Root, context);
							}
							LocationGlobber.tracer.WriteLine("Parent path = {0}", new object[]
							{
								text
							});
							path = path.Substring(2);
							LocationGlobber.tracer.WriteLine("path = {0}", new object[]
							{
								path
							});
							if (path.Length == 0)
							{
								break;
							}
							if (path[0] == '\\' || path[0] == '/')
							{
								path = path.Substring(1);
							}
							LocationGlobber.tracer.WriteLine("path = {0}", new object[]
							{
								path
							});
							if (path.Length == 0)
							{
								break;
							}
						}
						else
						{
							if (path.Equals(".", StringComparison.OrdinalIgnoreCase))
							{
								path = string.Empty;
								break;
							}
							if (path.StartsWith(".\\", StringComparison.Ordinal) || path.StartsWith("./", StringComparison.Ordinal))
							{
								path = path.Substring(".\\".Length);
								flag = true;
								LocationGlobber.tracer.WriteLine("path = {0}", new object[]
								{
									path
								});
								if (path.Length == 0)
								{
									break;
								}
							}
							if (path.Length == 0 || !flag)
							{
								break;
							}
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(path))
			{
				text = this.sessionState.Internal.MakePath(providerInstance, text, path, context);
			}
			NavigationCmdletProvider navigationCmdletProvider = providerInstance as NavigationCmdletProvider;
			if (navigationCmdletProvider != null)
			{
				string path2 = this.sessionState.Internal.MakePath(context.Drive.Root, text, context);
				string text2 = navigationCmdletProvider.ContractRelativePath(path2, context.Drive.Root, false, context);
				if (!string.IsNullOrEmpty(text2))
				{
					if (text2.StartsWith(context.Drive.Root, StringComparison.Ordinal))
					{
						text = text2.Substring(context.Drive.Root.Length);
					}
					else
					{
						text = text2;
					}
				}
				else
				{
					text = "";
				}
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x001111E0 File Offset: 0x0010F3E0
		private bool HasRelativePathTokens(string path)
		{
			string text = path.Replace('/', '\\');
			return text.Equals(".", StringComparison.OrdinalIgnoreCase) || text.Equals("..", StringComparison.OrdinalIgnoreCase) || text.Contains("\\.\\") || text.Contains("\\..\\") || text.EndsWith("\\..", StringComparison.OrdinalIgnoreCase) || text.EndsWith("\\.", StringComparison.OrdinalIgnoreCase) || text.StartsWith("..\\", StringComparison.OrdinalIgnoreCase) || text.StartsWith(".\\", StringComparison.OrdinalIgnoreCase) || text.StartsWith("~", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x00111274 File Offset: 0x0010F474
		private string GetProviderSpecificPath(PSDriveInfo drive, string workingPath, CmdletProviderContext context)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			if (workingPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("workingPath");
			}
			drive.Trace();
			LocationGlobber.tracer.WriteLine("workingPath = {0}", new object[]
			{
				workingPath
			});
			string text = drive.Root;
			try
			{
				text = this.sessionState.Internal.MakePath(drive.Provider, text, workingPath, context);
			}
			catch (NotSupportedException)
			{
			}
			return text;
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x001112FC File Offset: 0x0010F4FC
		private static string ParseProviderPath(string path, out string providerId)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			int num = path.IndexOf("::", StringComparison.Ordinal);
			if (num <= 0)
			{
				ArgumentException ex = PSTraceSource.NewArgumentException("path", SessionStateStrings.NotProviderQualifiedPath, new object[0]);
				throw ex;
			}
			providerId = path.Substring(0, num);
			string text = path.Substring(num + "::".Length);
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x0011137C File Offset: 0x0010F57C
		internal Collection<string> GetGlobbedProviderPathsFromProviderPath(string path, bool allowNonexistingPaths, ContainerCmdletProvider containerProvider, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (containerProvider == null)
			{
				throw PSTraceSource.NewArgumentNullException("containerProvider");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			return this.ExpandGlobPath(path, allowNonexistingPaths, containerProvider, context);
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x001113C1 File Offset: 0x0010F5C1
		internal static bool StringContainsGlobCharacters(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			return WildcardPattern.ContainsWildcardCharacters(path);
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x001113D8 File Offset: 0x0010F5D8
		internal static bool ShouldPerformGlobbing(string path, CmdletProviderContext context)
		{
			bool flag = false;
			if (path != null)
			{
				flag = LocationGlobber.StringContainsGlobCharacters(path);
			}
			bool flag2 = false;
			bool flag3 = false;
			if (context != null)
			{
				bool flag4 = context.Include != null && context.Include.Count > 0;
				LocationGlobber.pathResolutionTracer.WriteLine("INCLUDE filter present: {0}", new object[]
				{
					flag4
				});
				bool flag5 = context.Exclude != null && context.Exclude.Count > 0;
				LocationGlobber.pathResolutionTracer.WriteLine("EXCLUDE filter present: {0}", new object[]
				{
					flag5
				});
				flag2 = (flag4 || flag5);
				flag3 = context.SuppressWildcardExpansion;
				LocationGlobber.pathResolutionTracer.WriteLine("NOGLOB parameter present: {0}", new object[]
				{
					flag3
				});
			}
			LocationGlobber.pathResolutionTracer.WriteLine("Path contains wildcard characters: {0}", new object[]
			{
				flag
			});
			bool flag6 = (flag || flag2) && !flag3;
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag6
			});
			return flag6;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x00111504 File Offset: 0x0010F704
		private Collection<string> ExpandMshGlobPath(string path, bool allowNonexistingPaths, PSDriveInfo drive, ContainerCmdletProvider provider, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			LocationGlobber.tracer.WriteLine("path = {0}", new object[]
			{
				path
			});
			NavigationCmdletProvider navigationCmdletProvider = provider as NavigationCmdletProvider;
			Collection<string> collection = new Collection<string>();
			using (LocationGlobber.pathResolutionTracer.TraceScope("EXPANDING WILDCARDS", new object[0]))
			{
				if (LocationGlobber.ShouldPerformGlobbing(path, context))
				{
					List<string> list = new List<string>();
					Stack<string> stack = new Stack<string>();
					using (LocationGlobber.pathResolutionTracer.TraceScope("Tokenizing path", new object[0]))
					{
						while (LocationGlobber.StringContainsGlobCharacters(path))
						{
							if (context.Stopping)
							{
								throw new PipelineStoppedException();
							}
							string text = path;
							if (navigationCmdletProvider != null)
							{
								text = navigationCmdletProvider.GetChildName(path, context);
							}
							if (string.IsNullOrEmpty(text))
							{
								break;
							}
							LocationGlobber.tracer.WriteLine("Pushing leaf element: {0}", new object[]
							{
								text
							});
							LocationGlobber.pathResolutionTracer.WriteLine("Leaf element: {0}", new object[]
							{
								text
							});
							stack.Push(text);
							if (navigationCmdletProvider != null)
							{
								string parentPath = navigationCmdletProvider.GetParentPath(path, drive.Root, context);
								if (string.Equals(parentPath, path, StringComparison.OrdinalIgnoreCase))
								{
									PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(SessionStateStrings.ProviderImplementationInconsistent, new object[]
									{
										provider.ProviderInfo.Name,
										path
									});
									throw ex;
								}
								path = parentPath;
							}
							else
							{
								path = string.Empty;
							}
							LocationGlobber.tracer.WriteLine("New path: {0}", new object[]
							{
								path
							});
							LocationGlobber.pathResolutionTracer.WriteLine("Parent path: {0}", new object[]
							{
								path
							});
						}
						LocationGlobber.tracer.WriteLine("Base container path: {0}", new object[]
						{
							path
						});
						if (stack.Count == 0)
						{
							string text2 = path;
							if (navigationCmdletProvider != null)
							{
								text2 = navigationCmdletProvider.GetChildName(path, context);
								if (!string.IsNullOrEmpty(text2))
								{
									path = navigationCmdletProvider.GetParentPath(path, null, context);
								}
							}
							else
							{
								path = string.Empty;
							}
							stack.Push(text2);
							LocationGlobber.pathResolutionTracer.WriteLine("Leaf element: {0}", new object[]
							{
								text2
							});
						}
						LocationGlobber.pathResolutionTracer.WriteLine("Root path of resolution: {0}", new object[]
						{
							path
						});
					}
					list.Add(path);
					while (stack.Count > 0)
					{
						if (context.Stopping)
						{
							throw new PipelineStoppedException();
						}
						string leafElement = stack.Pop();
						list = this.GenerateNewPSPathsWithGlobLeaf(list, drive, leafElement, stack.Count == 0, provider, context);
						if (stack.Count > 0)
						{
							using (LocationGlobber.pathResolutionTracer.TraceScope("Checking matches to ensure they are containers", new object[0]))
							{
								int i = 0;
								while (i < list.Count)
								{
									if (context.Stopping)
									{
										throw new PipelineStoppedException();
									}
									string mshQualifiedPath = LocationGlobber.GetMshQualifiedPath(list[i], drive);
									if (navigationCmdletProvider != null && !this.sessionState.Internal.IsItemContainer(mshQualifiedPath, context))
									{
										LocationGlobber.tracer.WriteLine("Removing {0} because it is not a container", new object[]
										{
											list[i]
										});
										LocationGlobber.pathResolutionTracer.WriteLine("{0} is not a container", new object[]
										{
											list[i]
										});
										list.RemoveAt(i);
									}
									else if (navigationCmdletProvider != null)
									{
										LocationGlobber.pathResolutionTracer.WriteLine("{0} is a container", new object[]
										{
											list[i]
										});
										i++;
									}
								}
							}
						}
					}
					using (List<string>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text3 = enumerator.Current;
							LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
							{
								text3
							});
							collection.Add(text3);
						}
						goto IL_508;
					}
				}
				string text4 = context.SuppressWildcardExpansion ? path : LocationGlobber.RemoveGlobEscaping(path);
				string format = "{0}:" + '\\' + "{1}";
				if (drive.Hidden)
				{
					if (LocationGlobber.IsProviderDirectPath(text4))
					{
						format = "{1}";
					}
					else
					{
						format = "{0}::{1}";
					}
				}
				else if (path.StartsWith('\\'.ToString(), StringComparison.Ordinal))
				{
					format = "{0}:{1}";
				}
				string text5 = string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					drive.Name,
					text4
				});
				if (!allowNonexistingPaths && !provider.ItemExists(this.GetProviderPath(text5, context), context))
				{
					ItemNotFoundException ex2 = new ItemNotFoundException(text5, "PathNotFound", SessionStateStrings.PathNotFound);
					LocationGlobber.pathResolutionTracer.TraceError("Item does not exist: {0}", new object[]
					{
						path
					});
					throw ex2;
				}
				LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
				{
					text5
				});
				collection.Add(text5);
				IL_508:;
			}
			return collection;
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x00111A8C File Offset: 0x0010FC8C
		internal static string GetMshQualifiedPath(string path, PSDriveInfo drive)
		{
			string result;
			if (drive.Hidden)
			{
				if (LocationGlobber.IsProviderDirectPath(path))
				{
					result = path;
				}
				else
				{
					result = LocationGlobber.GetProviderQualifiedPath(path, drive.Provider);
				}
			}
			else
			{
				result = LocationGlobber.GetDriveQualifiedPath(path, drive);
			}
			return result;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x00111AC8 File Offset: 0x0010FCC8
		internal static string RemoveMshQualifier(string path, PSDriveInfo drive)
		{
			string result;
			if (drive.Hidden)
			{
				result = LocationGlobber.RemoveProviderQualifier(path);
			}
			else
			{
				result = LocationGlobber.RemoveDriveQualifier(path);
			}
			return result;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x00111AF0 File Offset: 0x0010FCF0
		internal static string GetDriveQualifiedPath(string path, PSDriveInfo drive)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			string text = path;
			bool flag = true;
			int num = path.IndexOf(':');
			if (num != -1)
			{
				if (drive.Hidden)
				{
					flag = false;
				}
				else
				{
					string a = path.Substring(0, num);
					if (string.Equals(a, drive.Name, StringComparison.OrdinalIgnoreCase))
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				string format = "{0}:" + '\\' + "{1}";
				if (path.StartsWith('\\'.ToString(), StringComparison.Ordinal))
				{
					format = "{0}:{1}";
				}
				text = string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					drive.Name,
					path
				});
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x00111BD4 File Offset: 0x0010FDD4
		private static string RemoveDriveQualifier(string path)
		{
			string text = path;
			int num = path.IndexOf(":", StringComparison.Ordinal);
			if (num != -1)
			{
				if (path[num + 1] == '\\' || path[num + 1] == '/')
				{
					num++;
				}
				text = path.Substring(num + 1);
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x00111C38 File Offset: 0x0010FE38
		internal static string GetProviderQualifiedPath(string path, ProviderInfo provider)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			string text = path;
			bool flag = false;
			int num = path.IndexOf("::", StringComparison.Ordinal);
			if (num != -1)
			{
				string providerName = path.Substring(0, num);
				if (provider.NameEquals(providerName))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", new object[]
				{
					provider.FullName,
					"::",
					path
				});
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x00111CE0 File Offset: 0x0010FEE0
		internal static string RemoveProviderQualifier(string path)
		{
			string text = path;
			int num = path.IndexOf("::", StringComparison.Ordinal);
			if (num != -1)
			{
				text = path.Substring(num + "::".Length);
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x00111D30 File Offset: 0x0010FF30
		private List<string> GenerateNewPSPathsWithGlobLeaf(List<string> currentDirs, PSDriveInfo drive, string leafElement, bool isLastLeaf, ContainerCmdletProvider provider, CmdletProviderContext context)
		{
			if (currentDirs == null)
			{
				throw PSTraceSource.NewArgumentNullException("currentDirs");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			NavigationCmdletProvider navigationCmdletProvider = provider as NavigationCmdletProvider;
			List<string> list = new List<string>();
			if ((!string.IsNullOrEmpty(leafElement) && LocationGlobber.StringContainsGlobCharacters(leafElement)) || isLastLeaf)
			{
				string pattern = LocationGlobber.ConvertMshEscapeToRegexEscape(leafElement);
				WildcardPattern stringMatcher = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				Collection<WildcardPattern> includeMatcher = SessionStateUtilities.CreateWildcardsFromStrings(context.Include, WildcardOptions.IgnoreCase);
				Collection<WildcardPattern> excludeMatcher = SessionStateUtilities.CreateWildcardsFromStrings(context.Exclude, WildcardOptions.IgnoreCase);
				using (List<string>.Enumerator enumerator = currentDirs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						using (LocationGlobber.pathResolutionTracer.TraceScope("Expanding wildcards for items under '{0}'", new object[]
						{
							text
						}))
						{
							if (context.Stopping)
							{
								throw new PipelineStoppedException();
							}
							string empty = string.Empty;
							Collection<PSObject> childNamesInDir = this.GetChildNamesInDir(text, leafElement, !isLastLeaf, context, false, drive, provider, out empty);
							if (childNamesInDir == null)
							{
								LocationGlobber.tracer.TraceError("GetChildNames returned a null array", new object[0]);
								LocationGlobber.pathResolutionTracer.WriteLine("No child names returned for '{0}'", new object[]
								{
									text
								});
							}
							else
							{
								foreach (PSObject childObject in childNamesInDir)
								{
									if (context.Stopping)
									{
										throw new PipelineStoppedException();
									}
									string empty2 = string.Empty;
									if (LocationGlobber.IsChildNameAMatch(childObject, stringMatcher, includeMatcher, excludeMatcher, out empty2))
									{
										string text2 = empty2;
										if (navigationCmdletProvider != null)
										{
											string parent = LocationGlobber.RemoveMshQualifier(empty, drive);
											text2 = this.sessionState.Internal.MakePath(parent, empty2, context);
											text2 = LocationGlobber.GetMshQualifiedPath(text2, drive);
										}
										LocationGlobber.tracer.WriteLine("Adding child path to dirs {0}", new object[]
										{
											text2
										});
										text2 = (isLastLeaf ? text2 : WildcardPattern.Escape(text2));
										list.Add(text2);
									}
								}
							}
						}
					}
					return list;
				}
			}
			LocationGlobber.tracer.WriteLine("LeafElement does not contain any glob characters so do a MakePath", new object[0]);
			foreach (string text3 in currentDirs)
			{
				using (LocationGlobber.pathResolutionTracer.TraceScope("Expanding intermediate containers under '{0}'", new object[]
				{
					text3
				}))
				{
					if (context.Stopping)
					{
						throw new PipelineStoppedException();
					}
					string text4 = LocationGlobber.ConvertMshEscapeToRegexEscape(leafElement);
					string path = context.SuppressWildcardExpansion ? text3 : LocationGlobber.RemoveGlobEscaping(text3);
					string mshQualifiedPath = LocationGlobber.GetMshQualifiedPath(path, drive);
					string text5 = text4;
					if (navigationCmdletProvider != null)
					{
						string parent2 = LocationGlobber.RemoveMshQualifier(mshQualifiedPath, drive);
						text5 = this.sessionState.Internal.MakePath(parent2, text4, context);
						text5 = LocationGlobber.GetMshQualifiedPath(text5, drive);
					}
					if (this.sessionState.Internal.ItemExists(text5, context))
					{
						LocationGlobber.tracer.WriteLine("Adding child path to dirs {0}", new object[]
						{
							text5
						});
						LocationGlobber.pathResolutionTracer.WriteLine("Valid intermediate container: {0}", new object[]
						{
							text5
						});
						list.Add(text5);
					}
				}
			}
			return list;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x001120F8 File Offset: 0x001102F8
		internal Collection<string> ExpandGlobPath(string path, bool allowNonexistingPaths, ContainerCmdletProvider provider, CmdletProviderContext context)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			string text = null;
			string str = null;
			string filter = context.Filter;
			bool flag = provider.ConvertPath(path, context.Filter, ref text, ref str, context);
			if (flag)
			{
				LocationGlobber.tracer.WriteLine("Provider converted path and filter.", new object[0]);
				LocationGlobber.tracer.WriteLine("Original path: " + path, new object[0]);
				LocationGlobber.tracer.WriteLine("Converted path: " + text, new object[0]);
				LocationGlobber.tracer.WriteLine("Original filter: " + context.Filter, new object[0]);
				LocationGlobber.tracer.WriteLine("Converted filter: " + str, new object[0]);
				path = text;
				filter = context.Filter;
			}
			NavigationCmdletProvider navigationCmdletProvider = provider as NavigationCmdletProvider;
			LocationGlobber.tracer.WriteLine("path = {0}", new object[]
			{
				path
			});
			Collection<string> collection = new Collection<string>();
			using (LocationGlobber.pathResolutionTracer.TraceScope("EXPANDING WILDCARDS", new object[0]))
			{
				if (LocationGlobber.ShouldPerformGlobbing(path, context))
				{
					List<string> list = new List<string>();
					Stack<string> stack = new Stack<string>();
					using (LocationGlobber.pathResolutionTracer.TraceScope("Tokenizing path", new object[0]))
					{
						while (LocationGlobber.StringContainsGlobCharacters(path))
						{
							if (context.Stopping)
							{
								throw new PipelineStoppedException();
							}
							string text2 = path;
							if (navigationCmdletProvider != null)
							{
								text2 = navigationCmdletProvider.GetChildName(path, context);
							}
							if (string.IsNullOrEmpty(text2))
							{
								break;
							}
							LocationGlobber.tracer.WriteLine("Pushing leaf element: {0}", new object[]
							{
								text2
							});
							LocationGlobber.pathResolutionTracer.WriteLine("Leaf element: {0}", new object[]
							{
								text2
							});
							stack.Push(text2);
							if (navigationCmdletProvider != null)
							{
								string root = string.Empty;
								if (context != null)
								{
									PSDriveInfo drive = context.Drive;
									if (drive != null)
									{
										root = drive.Root;
									}
								}
								string parentPath = navigationCmdletProvider.GetParentPath(path, root, context);
								if (string.Equals(parentPath, path, StringComparison.OrdinalIgnoreCase))
								{
									PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(SessionStateStrings.ProviderImplementationInconsistent, new object[]
									{
										provider.ProviderInfo.Name,
										path
									});
									throw ex;
								}
								path = parentPath;
							}
							else
							{
								path = string.Empty;
							}
							LocationGlobber.tracer.WriteLine("New path: {0}", new object[]
							{
								path
							});
							LocationGlobber.pathResolutionTracer.WriteLine("Parent path: {0}", new object[]
							{
								path
							});
						}
						LocationGlobber.tracer.WriteLine("Base container path: {0}", new object[]
						{
							path
						});
						if (stack.Count == 0)
						{
							string text3 = path;
							if (navigationCmdletProvider != null)
							{
								text3 = navigationCmdletProvider.GetChildName(path, context);
								if (!string.IsNullOrEmpty(text3))
								{
									path = navigationCmdletProvider.GetParentPath(path, null, context);
								}
							}
							else
							{
								path = string.Empty;
							}
							stack.Push(text3);
							LocationGlobber.pathResolutionTracer.WriteLine("Leaf element: {0}", new object[]
							{
								text3
							});
						}
						LocationGlobber.pathResolutionTracer.WriteLine("Root path of resolution: {0}", new object[]
						{
							path
						});
					}
					list.Add(path);
					while (stack.Count > 0)
					{
						if (context.Stopping)
						{
							throw new PipelineStoppedException();
						}
						string leafElement = stack.Pop();
						list = this.GenerateNewPathsWithGlobLeaf(list, leafElement, stack.Count == 0, provider, context);
						if (stack.Count > 0)
						{
							using (LocationGlobber.pathResolutionTracer.TraceScope("Checking matches to ensure they are containers", new object[0]))
							{
								int i = 0;
								while (i < list.Count)
								{
									if (context.Stopping)
									{
										throw new PipelineStoppedException();
									}
									if (navigationCmdletProvider != null && !navigationCmdletProvider.IsItemContainer(list[i], context))
									{
										LocationGlobber.tracer.WriteLine("Removing {0} because it is not a container", new object[]
										{
											list[i]
										});
										LocationGlobber.pathResolutionTracer.WriteLine("{0} is not a container", new object[]
										{
											list[i]
										});
										list.RemoveAt(i);
									}
									else if (navigationCmdletProvider != null)
									{
										LocationGlobber.pathResolutionTracer.WriteLine("{0} is a container", new object[]
										{
											list[i]
										});
										i++;
									}
								}
							}
						}
					}
					using (List<string>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text4 = enumerator.Current;
							LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
							{
								text4
							});
							collection.Add(text4);
						}
						goto IL_550;
					}
				}
				string text5 = context.SuppressWildcardExpansion ? path : LocationGlobber.RemoveGlobEscaping(path);
				if (!allowNonexistingPaths && !provider.ItemExists(text5, context))
				{
					ItemNotFoundException ex2 = new ItemNotFoundException(path, "PathNotFound", SessionStateStrings.PathNotFound);
					LocationGlobber.pathResolutionTracer.TraceError("Item does not exist: {0}", new object[]
					{
						path
					});
					throw ex2;
				}
				LocationGlobber.pathResolutionTracer.WriteLine("RESOLVED PATH: {0}", new object[]
				{
					text5
				});
				collection.Add(text5);
				IL_550:;
			}
			if (flag)
			{
				context.Filter = filter;
			}
			return collection;
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x001126D4 File Offset: 0x001108D4
		internal List<string> GenerateNewPathsWithGlobLeaf(List<string> currentDirs, string leafElement, bool isLastLeaf, ContainerCmdletProvider provider, CmdletProviderContext context)
		{
			if (currentDirs == null)
			{
				throw PSTraceSource.NewArgumentNullException("currentDirs");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			NavigationCmdletProvider navigationCmdletProvider = provider as NavigationCmdletProvider;
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(leafElement) && (LocationGlobber.StringContainsGlobCharacters(leafElement) || isLastLeaf))
			{
				string pattern = LocationGlobber.ConvertMshEscapeToRegexEscape(leafElement);
				WildcardPattern stringMatcher = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
				Collection<WildcardPattern> includeMatcher = SessionStateUtilities.CreateWildcardsFromStrings(context.Include, WildcardOptions.IgnoreCase);
				Collection<WildcardPattern> excludeMatcher = SessionStateUtilities.CreateWildcardsFromStrings(context.Exclude, WildcardOptions.IgnoreCase);
				using (List<string>.Enumerator enumerator = currentDirs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						using (LocationGlobber.pathResolutionTracer.TraceScope("Expanding wildcards for items under '{0}'", new object[]
						{
							text
						}))
						{
							if (context.Stopping)
							{
								throw new PipelineStoppedException();
							}
							string parent = null;
							Collection<PSObject> childNamesInDir = this.GetChildNamesInDir(text, leafElement, !isLastLeaf, context, true, null, provider, out parent);
							if (childNamesInDir == null)
							{
								LocationGlobber.tracer.TraceError("GetChildNames returned a null array", new object[0]);
								LocationGlobber.pathResolutionTracer.WriteLine("No child names returned for '{0}'", new object[]
								{
									text
								});
							}
							else
							{
								foreach (PSObject childObject in childNamesInDir)
								{
									if (context.Stopping)
									{
										throw new PipelineStoppedException();
									}
									string empty = string.Empty;
									if (LocationGlobber.IsChildNameAMatch(childObject, stringMatcher, includeMatcher, excludeMatcher, out empty))
									{
										string text2 = empty;
										if (navigationCmdletProvider != null)
										{
											text2 = navigationCmdletProvider.MakePath(parent, empty, context);
										}
										LocationGlobber.tracer.WriteLine("Adding child path to dirs {0}", new object[]
										{
											text2
										});
										list.Add(text2);
									}
								}
							}
						}
					}
					return list;
				}
			}
			LocationGlobber.tracer.WriteLine("LeafElement does not contain any glob characters so do a MakePath", new object[0]);
			foreach (string text3 in currentDirs)
			{
				using (LocationGlobber.pathResolutionTracer.TraceScope("Expanding intermediate containers under '{0}'", new object[]
				{
					text3
				}))
				{
					if (context.Stopping)
					{
						throw new PipelineStoppedException();
					}
					string text4 = LocationGlobber.ConvertMshEscapeToRegexEscape(leafElement);
					string parent2 = context.SuppressWildcardExpansion ? text3 : LocationGlobber.RemoveGlobEscaping(text3);
					string text5 = text4;
					if (navigationCmdletProvider != null)
					{
						text5 = navigationCmdletProvider.MakePath(parent2, text4, context);
					}
					if (provider.ItemExists(text5, context))
					{
						LocationGlobber.tracer.WriteLine("Adding child path to dirs {0}", new object[]
						{
							text5
						});
						list.Add(text5);
						LocationGlobber.pathResolutionTracer.WriteLine("Valid intermediate container: {0}", new object[]
						{
							text5
						});
					}
				}
			}
			return list;
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x00112A34 File Offset: 0x00110C34
		private Collection<PSObject> GetChildNamesInDir(string dir, string leafElement, bool getAllContainers, CmdletProviderContext context, bool dirIsProviderPath, PSDriveInfo drive, ContainerCmdletProvider provider, out string modifiedDirPath)
		{
			string text = null;
			string text2 = null;
			string filter = context.Filter;
			bool flag = provider.ConvertPath(leafElement, context.Filter, ref text, ref text2, context);
			if (flag)
			{
				LocationGlobber.tracer.WriteLine("Provider converted path and filter.", new object[0]);
				LocationGlobber.tracer.WriteLine("Original path: " + leafElement, new object[0]);
				LocationGlobber.tracer.WriteLine("Converted path: " + text, new object[0]);
				LocationGlobber.tracer.WriteLine("Original filter: " + context.Filter, new object[0]);
				LocationGlobber.tracer.WriteLine("Converted filter: " + text2, new object[0]);
				leafElement = text;
				context.Filter = text2;
			}
			ReturnContainers returnContainers = ReturnContainers.ReturnAllContainers;
			if (!getAllContainers)
			{
				returnContainers = ReturnContainers.ReturnMatchingContainers;
			}
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(context);
			cmdletProviderContext.SetFilters(new Collection<string>(), new Collection<string>(), context.Filter);
			Collection<PSObject> result;
			try
			{
				modifiedDirPath = null;
				string path;
				if (dirIsProviderPath)
				{
					modifiedDirPath = (path = (context.SuppressWildcardExpansion ? dir : LocationGlobber.RemoveGlobEscaping(dir)));
				}
				else
				{
					modifiedDirPath = LocationGlobber.GetMshQualifiedPath(dir, drive);
					ProviderInfo providerInfo = null;
					CmdletProvider cmdletProvider = null;
					Collection<string> globbedProviderPathsFromMonadPath = this.GetGlobbedProviderPathsFromMonadPath(modifiedDirPath, false, cmdletProviderContext, out providerInfo, out cmdletProvider);
					modifiedDirPath = (context.SuppressWildcardExpansion ? modifiedDirPath : LocationGlobber.RemoveGlobEscaping(modifiedDirPath));
					if (globbedProviderPathsFromMonadPath.Count <= 0)
					{
						if (flag)
						{
							context.Filter = filter;
						}
						return new Collection<PSObject>();
					}
					path = globbedProviderPathsFromMonadPath[0];
				}
				if (provider.HasChildItems(path, cmdletProviderContext))
				{
					provider.GetChildNames(path, returnContainers, cmdletProviderContext);
				}
				if (cmdletProviderContext.HasErrors())
				{
					Collection<ErrorRecord> accumulatedErrorObjects = cmdletProviderContext.GetAccumulatedErrorObjects();
					if (accumulatedErrorObjects != null && accumulatedErrorObjects.Count > 0)
					{
						foreach (ErrorRecord errorRecord in accumulatedErrorObjects)
						{
							context.WriteError(errorRecord);
						}
					}
				}
				Collection<PSObject> accumulatedObjects = cmdletProviderContext.GetAccumulatedObjects();
				if (flag)
				{
					context.Filter = filter;
				}
				result = accumulatedObjects;
			}
			finally
			{
				cmdletProviderContext.RemoveStopReferral();
			}
			return result;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x00112C7C File Offset: 0x00110E7C
		private static bool IsChildNameAMatch(PSObject childObject, WildcardPattern stringMatcher, Collection<WildcardPattern> includeMatcher, Collection<WildcardPattern> excludeMatcher, out string childName)
		{
			bool flag = false;
			childName = null;
			object baseObject = childObject.BaseObject;
			if (baseObject is PSCustomObject)
			{
				LocationGlobber.tracer.TraceError("GetChildNames returned a null object", new object[0]);
			}
			else
			{
				childName = (baseObject as string);
				if (childName == null)
				{
					LocationGlobber.tracer.TraceError("GetChildNames returned an object that wasn't a string", new object[0]);
				}
				else
				{
					LocationGlobber.pathResolutionTracer.WriteLine("Name returned from provider: {0}", new object[]
					{
						childName
					});
					bool flag2 = WildcardPattern.ContainsWildcardCharacters(stringMatcher.Pattern);
					bool flag3 = stringMatcher.IsMatch(childName);
					LocationGlobber.tracer.WriteLine("isChildMatch = {0}", new object[]
					{
						flag3
					});
					bool flag4 = includeMatcher.Count > 0;
					bool flag5 = excludeMatcher.Count > 0;
					bool flag6 = SessionStateUtilities.MatchesAnyWildcardPattern(childName, includeMatcher, true);
					LocationGlobber.tracer.WriteLine("isIncludeMatch = {0}", new object[]
					{
						flag6
					});
					if (flag3 || (flag2 && flag4 && flag6))
					{
						LocationGlobber.pathResolutionTracer.WriteLine("Path wildcard match: {0}", new object[]
						{
							childName
						});
						flag = true;
						if (flag4 && !flag6)
						{
							LocationGlobber.pathResolutionTracer.WriteLine("Not included match: {0}", new object[]
							{
								childName
							});
							flag = false;
						}
						if (flag5 && SessionStateUtilities.MatchesAnyWildcardPattern(childName, excludeMatcher, false))
						{
							LocationGlobber.pathResolutionTracer.WriteLine("Excluded match: {0}", new object[]
							{
								childName
							});
							flag = false;
						}
					}
					else
					{
						LocationGlobber.pathResolutionTracer.WriteLine("NOT path wildcard match: {0}", new object[]
						{
							childName
						});
					}
				}
			}
			LocationGlobber.tracer.WriteLine("result = {0}; childName = {1}", new object[]
			{
				flag,
				childName
			});
			return flag;
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x00112E64 File Offset: 0x00111064
		private static string ConvertMshEscapeToRegexEscape(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			char[] array = path.ToCharArray();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.GetLength(0); i++)
			{
				if (array[i] == '`')
				{
					if (i + 1 < array.GetLength(0))
					{
						if (array[i + 1] == '`')
						{
							stringBuilder.Append('`');
							i++;
						}
						else
						{
							stringBuilder.Append('\\');
						}
					}
					else
					{
						stringBuilder.Append('\\');
					}
				}
				else if (array[i] == '\\')
				{
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append(array[i]);
				}
			}
			LocationGlobber.tracer.WriteLine("Original path: {0} Converted to: {1}", new object[]
			{
				path,
				stringBuilder.ToString()
			});
			return stringBuilder.ToString();
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x00112F28 File Offset: 0x00111128
		internal static bool IsHomePath(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			bool flag = false;
			if (LocationGlobber.IsProviderQualifiedPath(path))
			{
				int num = path.IndexOf("::", StringComparison.Ordinal);
				if (num != -1)
				{
					path = path.Substring(num + "::".Length);
				}
			}
			if (path.IndexOf("~", StringComparison.Ordinal) == 0)
			{
				if (path.Length == 1)
				{
					flag = true;
				}
				else if (path.Length > 1 && (path[1] == '\\' || path[1] == '/'))
				{
					flag = true;
				}
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x00112FD0 File Offset: 0x001111D0
		internal static bool IsProviderDirectPath(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			bool flag = false;
			if (path.StartsWith("\\\\", StringComparison.Ordinal) || path.StartsWith("//", StringComparison.Ordinal))
			{
				flag = true;
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x0011302C File Offset: 0x0011122C
		internal string GetHomeRelativePath(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			string text = path;
			if (LocationGlobber.IsHomePath(path) && this.sessionState.Drive.Current != null)
			{
				ProviderInfo providerInfo = this.sessionState.Drive.Current.Provider;
				if (LocationGlobber.IsProviderQualifiedPath(path))
				{
					int num = path.IndexOf("::", StringComparison.Ordinal);
					if (num != -1)
					{
						string name = path.Substring(0, num);
						providerInfo = this.sessionState.Internal.GetSingleProvider(name);
						path = path.Substring(num + "::".Length);
					}
				}
				if (path.IndexOf("~", StringComparison.Ordinal) == 0)
				{
					if (path.Length > 1 && (path[1] == '\\' || path[1] == '/'))
					{
						path = path.Substring(2);
					}
					else
					{
						path = path.Substring(1);
					}
					if (providerInfo.Home == null || providerInfo.Home.Length <= 0)
					{
						InvalidOperationException ex = PSTraceSource.NewInvalidOperationException(SessionStateStrings.HomePathNotSet, new object[]
						{
							providerInfo.Name
						});
						LocationGlobber.pathResolutionTracer.TraceError("HOME path not set for provider: {0}", new object[]
						{
							providerInfo.Name
						});
						throw ex;
					}
					CmdletProviderContext context = new CmdletProviderContext(this.sessionState.Internal.ExecutionContext);
					LocationGlobber.pathResolutionTracer.WriteLine("Getting home path for provider: {0}", new object[]
					{
						providerInfo.Name
					});
					LocationGlobber.pathResolutionTracer.WriteLine("Provider HOME path: {0}", new object[]
					{
						providerInfo.Home
					});
					if (string.IsNullOrEmpty(path))
					{
						path = providerInfo.Home;
					}
					else
					{
						path = this.sessionState.Internal.MakePath(providerInfo, providerInfo.Home, path, context);
					}
					LocationGlobber.pathResolutionTracer.WriteLine("HOME relative path: {0}", new object[]
					{
						path
					});
				}
				text = path;
			}
			LocationGlobber.tracer.WriteLine("result = {0}", new object[]
			{
				text
			});
			return text;
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x00113240 File Offset: 0x00111440
		private static void TraceFilters(CmdletProviderContext context)
		{
			if ((LocationGlobber.pathResolutionTracer.Options & PSTraceSourceOptions.WriteLine) != PSTraceSourceOptions.None)
			{
				LocationGlobber.pathResolutionTracer.WriteLine("Filter: {0}", new object[]
				{
					context.Filter ?? string.Empty
				});
				if (context.Include != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string arg in context.Include)
					{
						stringBuilder.AppendFormat("{0} ", arg);
					}
					LocationGlobber.pathResolutionTracer.WriteLine("Include: {0}", new object[]
					{
						stringBuilder.ToString()
					});
				}
				if (context.Exclude != null)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (string arg2 in context.Exclude)
					{
						stringBuilder2.AppendFormat("{0} ", arg2);
					}
					LocationGlobber.pathResolutionTracer.WriteLine("Exclude: {0}", new object[]
					{
						stringBuilder2.ToString()
					});
				}
			}
		}

		// Token: 0x04001A71 RID: 6769
		[TraceSource("LocationGlobber", "The location globber converts PowerShell paths with glob characters to zero or more paths.")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("LocationGlobber", "The location globber converts PowerShell paths with glob characters to zero or more paths.");

		// Token: 0x04001A72 RID: 6770
		[TraceSource("PathResolution", "Traces the path resolution algorithm.")]
		private static PSTraceSource pathResolutionTracer = PSTraceSource.GetTracer("PathResolution", "Traces the path resolution algorithm.", false);

		// Token: 0x04001A73 RID: 6771
		private SessionState sessionState;
	}
}
